using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static CAD_Client.ToolEnum;
using static CAD_Client.Debugger;
using System.IO;

//!!!Projekt#01: смена фигуры во время рисования вызывает непредвиденную ошибку.
//!!!Projekt#50: добавить масштаб
//Projekt#40: добавить перемещение
//!!!Пересмотреть snap: код не должен знать о существовании привязки
namespace CAD_Client {
    //!!!MainForm#20: добавить плавающие контролы
    public sealed partial class GUI_Form : Form {
        private int scale = 1;
        private readonly int snapDistancePx = 10;
        private readonly int gridSizePx = 25;
        private int screenX;
        private int screenY;
        private readonly Pen gridPen = new Pen(Color.DarkGray, 1);

        private readonly Bitmap bitmap;
        private readonly Graphics screen;
        private readonly MyClient code;
        private readonly MyCursor myCursor;



        public GUI_Form(MyClient code) {
            InitializeComponent();

            this.code = code;
            bitmap = new Bitmap(496, 290);
            screen = Graphics.FromImage(bitmap);
            myCursor = new MyCursor();
            screenX = bitmap.Size.Width / 2;
            screenY = bitmap.Size.Height / 2;

            this.code.SelectedToolChanged += Code_SelectedTool_Changed;
            this.code.SelectedBuildingVariantChanged += Code_SelectedBuildingMethod_Changed;
            this.code.FiguresListChanged += Code_FiguresList_Changed;
            this.code.ConstructorOperationStatusChanged += MainCode_ConstructorOperationStatus_Changed;
            this.code.PolarLineEnablingChanged += Code_PolarLineEnabling_Changed;

            MainFormCmbbxBuildingVariants.DisplayMember = "DisplayMember";
            MainFormCmbbxBuildingVariants.ValueMember = "BuildingMethod";
            MainFormLstbxFigures.DisplayMember = "DisplayMember";
            MainFormLstbxFigures.ValueMember = "Id";
        }
        private void MainForm_Load(object sender, EventArgs e) {

        }



        #region Работа с экраном
        private void MainFromPctrbxScreen_MouseDown(object sender, MouseEventArgs e) {
            if (code.SelectedTool == Tool.Select) {
                code.SetPoint(e.Location);
            }
            else
            if (code.SelectedTool == Tool.Moving && e.Button == MouseButtons.Middle) {
                myCursor.DoSnap(ControlPointToScreen(e.Location, MainFromPctrbxScreen), int.MaxValue);
            }
        }
        private void MainFormPctrbxScreen_MouseMove(object sender, MouseEventArgs e) {
            Point mouseLocation = e.Location;

            code.AddSoftPoint(e.Location);

            CheckSnap(e.Location);

            MainFormSttsstpLblMouseX.Text = mouseLocation.X.ToString().PadLeft(3);
            MainFormSttsstpLblMouseY.Text = mouseLocation.Y.ToString().PadLeft(3);
        }
        private void MainFromPctrbxScreen_MouseUp(object sender, MouseEventArgs e) {
            //За пределами экрана
            if (e.X > (sender as PictureBox).Width || e.X < 0 ||
                e.Y > (sender as PictureBox).Height || e.Y < 0) {
                return;
            }

            if (code.SelectedTool == Tool.Moving) {
                myCursor.StopSnap();
            }

            code.SetPoint(e.Location);
        }


        private void MainFormTmr_Tick(object sender, EventArgs e) {
            code.DrawFigures(screen);
            DrawGrid();
            Display();
            //Debugger.Log("Display");
        }
        /// <summary>
        /// Нарисует сетку на экране с центром координат в (0; 0).
        /// </summary>
        private void DrawGrid() {
            //Вертикальные
            Point delta = new Point(screenX % gridSizePx, screenY % gridSizePx);
            for (int i = gridSizePx - delta.X; i < bitmap.Width; i += gridSizePx) {
                screen.DrawLine(gridPen, i, 0, i, bitmap.Height);
            }

            //Горизонтальные
            for (int i = gridSizePx - delta.Y; i < bitmap.Height; i += gridSizePx) {
                screen.DrawLine(gridPen, 0, i, bitmap.Width, i);
            }
        }
        private void Display() {
            MainFromPctrbxScreen.Image = bitmap;
        }


        /// <summary>
        /// Создаст, продолжит или прервёт привязку при соответствущих условиях.
        /// </summary>
        /// <param name="newPoint"> Новое положение курсора мыши относительно <see cref=""/>. </param>
        private void CheckSnap(Point newPoint) {
            if (myCursor.IsSnapped) {
                Point newPointScreenLocation = PointToScreen(newPoint);
                myCursor.ContinueSnap(newPointScreenLocation);
                if (code.SelectedTool == Tool.Moving) {
                    screenX += myCursor.Cumulate.X;
                    screenY += myCursor.Cumulate.Y;
                }
            }
            else {
                //if (e.Button == MouseButtons.None) {
                int figCount = code.GetFiguresCount();
                if (figCount != 0) {
                    PointF vertex = code.FindNearestVertex(newPoint);
                    //Привязка считается в отображаемых пикселях
                    ConvertRealCoordToPx(vertex, out Point vertexPx);
                    bool isNearlyVertex = IsPxInSquare(newPoint, vertexPx, snapDistancePx);
                    if (isNearlyVertex) {
                        //Snap выполняется для экранных координат.
                        Point vertexScreenLocation = PointToScreen(vertexPx);
                        myCursor.DoSnap(vertexScreenLocation, snapDistancePx);
                    }
                }
                //}
            }
        }
        /// <summary>
        /// Возвращает реальное расположение точки в отображаемое пиксельное экранное.
        /// </summary>
        private void ConvertRealCoordToPx(in PointF location, out Point pxLocation) {
            pxLocation = new Point {
                X = (int)Math.Round(location.X),
                Y = (int)Math.Round(location.Y)
            };
        }
        /// <summary>
        /// Определит, лежит ли пиксель в квадрате (или на его грани) с заданным центром и половиной стороны.
        /// </summary>
        private bool IsPxInSquare(Point px, Point center, int interval) =>
            Math.Abs(px.X - center.X) <= interval && Math.Abs(px.Y - center.Y) <= interval;

        #endregion

        #region Кнопки
        private void MainFormBttnCircle_Click(object sender, EventArgs e) {
            Tool firgureToSelect = Tool.Circle;
            code.SelectedTool = firgureToSelect;

            //Это, наверное, всё же лучше запихуть в Code в этой реализации, т.к.
            //сообщение одно для всех платформ. Однако для разных людей это не так.
            //Вопрос отложен.
            MainFormSttsstpLblHint.Text = "Окружность, ограниченная прямоугольником. Выберете первую точку";

        }
        private void MainFormBttnRectangle_Click(object sender, EventArgs e) {
            code.SelectedTool = Tool.Rectangle;
            MainFormSttsstpLblHint.Text = "Прямоугольник. Выберете первую точку";
        }
        private void MainFormBttnCut_Click(object sender, EventArgs e) {
            code.SelectedTool = Tool.Cut;
            MainFormSttsstpLblHint.Text = "Отрезок. Выберете первую точку";
        }
        private void MainFormBttnSelect_Click(object sender, EventArgs e) {
            code.SelectedTool = Tool.Select;
        }
        private void MainFormBttnNothing_Click(object sender, EventArgs e) {
            code.SelectedTool = Tool.None;
        }

        private void MainFormCmbbxBuildingVariants_SelectedIndexChanged(object sender, EventArgs e) {
            code.SelectedBuildingMethod = ((ComboboxBuildingMethod)MainFormCmbbxBuildingVariants.SelectedItem).BuildingMethod;
        }
        private void MainFormLstbxFigures_SelectedIndexChanged(object sender, EventArgs e) {
            //Мы не можем знать, снялось выделение или появилось, таким образом нужно снять выделения
            //со всех фигур и задать их заново
            int figuresCount = code.GetFiguresCount();
            for (int i = 0; i < figuresCount; i++) {
                code.UnselectFigure(((sender as ListBox).Items[0] as ListBoxFigure).Id);
            }

            foreach (int index in (sender as ListBox).SelectedIndices) {
                code.SelectFigure(((sender as ListBox).Items[index] as ListBoxFigure).Id);
            }

            code.DrawFigures(screen);
        }

        private void MainFormTlstrpSpltbttnPolarLine_Click(object sender, EventArgs e) {
            code.PolarLineEnabled = !code.PolarLineEnabled;
        }
        #endregion

        #region API
        private void Code_SelectedTool_Changed(Tool value, EventArgs e) {
            List<BuildingMethod> pbm = ReturnPossibleBuildingVariants(value);
            MainFormCmbbxBuildingVariants.Items.Clear();
            var cbm = new ComboboxBuildingMethod[pbm.Count];
            for (int i = 0; i < pbm.Count; i++) {
                cbm[i] = new ComboboxBuildingMethod(pbm[i]);
            }
            MainFormCmbbxBuildingVariants.Items.AddRange(cbm);
        }
        //!!!MainForm#42.1: выделение нескольких элементов некорректно работает
        private void Code_SelectedBuildingMethod_Changed(BuildingMethod value, EventArgs e) {
            for (int i = 0; i < MainFormCmbbxBuildingVariants.Items.Count; i++) {
                if ((MainFormCmbbxBuildingVariants.Items[i] as ComboboxBuildingMethod).BuildingMethod == value) {
                    MainFormCmbbxBuildingVariants.SelectedIndex = i;
                    break;
                }
            }

        }
        //!!!MainForm#42: исправить в соответствии с возможностью выбрать несколько элементов
        private void Code_FiguresList_Changed(object sender, EventArgs e) {
            int currListSelectedIndex = MainFormLstbxFigures.SelectedIndex;
            bool wasSmnSelected = currListSelectedIndex != -1;
            int currListSelectedItemId = -1;
            if (wasSmnSelected) {
                currListSelectedItemId = (MainFormLstbxFigures.SelectedItem as ListBoxFigure).Id;
            }

            MainFormLstbxFigures.Items.Clear();
            var figuresToListboxList = new List<ListBoxFigure>();
            var figuresList = sender as MyListContainer<MyFigure>;
            for (int i = 0; i < figuresList.Count; i++) {
                figuresToListboxList.Add(
                    new ListBoxFigure(figuresList[i]));
            }
            MainFormLstbxFigures.Items.AddRange(figuresToListboxList.ToArray());

            if (wasSmnSelected) {
                int listBoxItemToSelectIndex = -1;
                for (int i = 0; i < MainFormLstbxFigures.Items.Count; i++) {
                    if ((MainFormLstbxFigures.Items[i] as ListBoxFigure).Id == currListSelectedItemId) {
                        listBoxItemToSelectIndex = i;
                        break;
                    }
                }

                if (listBoxItemToSelectIndex != -1) { //Выделенный элемент не был удалён
                    MainFormLstbxFigures.SelectedIndex = listBoxItemToSelectIndex;
                }
            }

        }
        private void MainCode_ConstructorOperationStatus_Changed(ConstructorOperationStatus sender, EventArgs e) {
            if (sender.Result == ConstructorOperationStatus.OperationStatus.Continious) {
                MainFormSttsstpLblHint.Text = sender.OperationMessage;
                //code.AddPolarLine(ControlPointToScreen(Cursor.Position, MainFromPctrbxScreen));
            }
            else
            if (sender.Result == ConstructorOperationStatus.OperationStatus.Canselled) {
                MainFormSttsstpLblHint.Text = "Отменено";
            }
            else
            if (sender.Result == ConstructorOperationStatus.OperationStatus.Finished) {
                code.DrawFigures(screen);
                MainFormSttsstpLblHint.Text = "Успешно.";
            }
        }
        //???Вот в дефолтных ивентах названия PolarLineEnablingChanged или PolarLineEnabling_Changed?
        //Потому что всегда выдаёт имя ивента в методе без земли.
        private void Code_PolarLineEnabling_Changed(object sender, EventArgs e) {
            if ((sender as MyClient).PolarLineEnabled) {
                this.MainFormTlstrpSpltbttnPolarLine.Image = global::CAD_Client.Properties.Resources.PolarLineEnabled;
            }
            else {
                this.MainFormTlstrpSpltbttnPolarLine.Image = global::CAD_Client.Properties.Resources.PolarLineDisabled;
            }
        }
        #endregion


        /// <summary> Вычисляет местоположение мыши в координатах контрола. </summary>
        private Point ControlPointToScreen(Point location, Control controlOnForm) {
            Point out_point = PointToScreen(location);
            out_point.X += controlOnForm.Location.X;
            out_point.Y += controlOnForm.Location.Y;
            return out_point;
        }


        private void button1_Click(object sender, EventArgs e) {
            screenX += 2;
            screenY += 2;
        }

        private void MainFormBttnMove_Click(object sender, EventArgs e) {
            code.SelectedTool = Tool.Moving;
        }
    }
}
//MainForm#46: Поменять таймер на MouseMowe
//[Closed]: неактуально, пока используется Graphics.Clear()