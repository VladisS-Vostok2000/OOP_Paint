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
using static OOP_Paint.FiguresEnum;
using static OOP_Paint.Debugger;
using System.IO;

//!!!Projekt#01: смена фигуры во время рисования вызывает непредвиденную ошибку.
//Projekt#30: добавить полярные линии
//!!!Projekt#50: добавить масштаб
//!!!Projekt#07: добавить модификаторы in
//Projekt#32: добавить луч
namespace OOP_Paint {
    //!!!MainForm#20: добавить плавающие контролы
    public sealed partial class MainForm : Form {
        private Int32 scale = 1;
        private readonly Int32 snapDistancePx = 10;

        private readonly Bitmap bitmap;
        private readonly Graphics screen;
        private readonly MainCode code;
        private readonly MyCursor myCursor;



        public MainForm(MainCode code) {
            Debugger.Log("Start");
            this.code = code;

            InitializeComponent();

            bitmap = new Bitmap(496, 290);
            screen = Graphics.FromImage(bitmap);
            myCursor = new MyCursor(snapDistancePx);

            this.code.SelectedToolChanged += Code_SelectedTool_Changed;
            this.code.SelectedBuildingVariantChanged += Code_SelectedBuildingMethod_Changed;
            this.code.FiguresListChanged += Code_FiguresList_Changed;
            this.code.ConstructorOperationStatusChanged += MainCode_ConstructorOperationStatus_Changed;
            myCursor.SnapTorned += MyCursor_SnapTorned;

            MainFormCmbbxBuildingVariants.DisplayMember = "DisplayMember";
            MainFormCmbbxBuildingVariants.ValueMember = "BuildingMethod";
            MainFormLstbxFigures.DisplayMember = "DisplayMember";
            MainFormLstbxFigures.ValueMember = "Id";
        }
        private void MainForm_Load(Object sender, EventArgs e) {

        }



        private void MainFromPctrbxScreen_MouseDown(Object sender, MouseEventArgs e) {
            //MainFormTmr.Stop();
            if (code.SelectedTool == Figure.Select) {
                //MainFormTmr.Enabled = true;
                code.SetPoint(e.Location);
            }
            //MainFormTmr.Start();
        }
        private void MainFormPctrbxScreen_MouseMove(Object sender, MouseEventArgs e) {
            Point mouseLocation = e.Location;
            MainFormSttsstpLblMouseX.Text = mouseLocation.X.ToString().PadLeft(3);
            MainFormSttsstpLblMouseY.Text = mouseLocation.Y.ToString().PadLeft(3);

            code.AddSoftPoint(e.Location);

            #region Snap
            if (myCursor.IsSnapped) {
                myCursor.ContinueSnap(ControlPointToScreen(e.Location, MainFromPctrbxScreen));
            }
            else {
                Debugger.Log("if (e.Button == MouseButtons.None)");
                if (e.Button == MouseButtons.None) {
                    Int32 figCount = code.GetFiguresCount();
                    Debugger.Log("if (figCount != 0)");
                    if (figCount != 0) {
                        PointF vetrex = code.FindNearestVertex(e.Location);
                        //Привязка считается в отображаемых пикселях
                        ConvertRealCoordToPx(vetrex, out Point vetrexPx);
                        Single distance = MyFigure.FindLength(vetrexPx, e.Location);
                        Debugger.Log("if (distance < snapDistancePx)");
                        if (distance < snapDistancePx) {
                            Int32 x = (Int32)Math.Round(vetrex.X);
                            Int32 y = (Int32)Math.Round(vetrex.Y);
                            Point point = ControlPointToScreen(new Point(x, y), MainFromPctrbxScreen);
                            Debugger.Log($"SnapCreating");
                            Debugger.Log($"MouseLocation: ({Cursor.Position.X};{Cursor.Position.Y})");
                            myCursor.DoSnap(point);
                            //У кода - координаты реальные. 
                            code.AddSnapPoint(new Point(x, y));
                        }
                    }
                }

            }
            #endregion


        }
        private void MainFromPctrbxScreen_MouseUp(Object sender, MouseEventArgs e) {
            //За пределами экрана
            if (e.X > (sender as PictureBox).Width || e.X < 0 ||
                e.Y > (sender as PictureBox).Height || e.Y < 0) {
                return;
            }

            code.SetPoint(e.Location);
        }
        private void MainFormTmr_Tick(Object sender, EventArgs e) {
            code.DrawFigures(screen);
            Display();
            //Debugger.Log("Display");
        }
        private void Display() {
            MainFromPctrbxScreen.Image = bitmap;
        }

        private void ConvertRealCoordToPx(PointF location, out Point pxLocation) {
            pxLocation = new Point {
                X = (Int32)Math.Round(location.X),
                Y = (Int32)Math.Round(location.Y)
            };
        }

        private void MainFormBttnCircle_Click(Object sender, EventArgs e) {
            Figure firgureToSelect = Figure.Circle;
            code.SelectedTool = firgureToSelect;

            //Это, наверное, всё же лучше запихуть в Code в этой реализации, т.к.
            //сообщение одно для всех платформ. Однако для разных людей это не так.
            //Вопрос отложен.
            MainFormSttsstpLblHint.Text = "Окружность, ограниченная прямоугольником. Выберете первую точку";

        }
        private void MainFormBttnRectangle_Click(Object sender, EventArgs e) {
            code.SelectedTool = Figure.Rectangle;
            MainFormSttsstpLblHint.Text = "Прямоугольник. Выберете первую точку";
        }
        private void MainFormBttnCut_Click(Object sender, EventArgs e) {
            code.SelectedTool = Figure.Cut;
            MainFormSttsstpLblHint.Text = "Отрезок. Выберете первую точку";
        }
        private void MainFormBttnSelect_Click(Object sender, EventArgs e) {
            code.SelectedTool = Figure.Select;
        }
        private void MainFormBttnNothing_Click(Object sender, EventArgs e) {
            code.SelectedTool = Figure.None;
        }

        private void MainFormCmbbxBuildingVariants_SelectedIndexChanged(Object sender, EventArgs e) {
            code.SelectedBuildingMethod = ((ComboboxBuildingMethod)MainFormCmbbxBuildingVariants.SelectedItem).BuildingMethod;
        }
        private void MainFormLstbxFigures_SelectedIndexChanged(Object sender, EventArgs e) {
            //Мы не можем знать, снялось выделение или появилось, таким образом нужно снять выделения
            //со всех фигур и задать их заново
            Int32 figuresCount = code.GetFiguresCount();
            for (Int32 i = 0; i < figuresCount; i++) {
                code.UnselectFigure(((sender as ListBox).Items[0] as ListBoxFigure).Id);
            }

            foreach (Int32 index in (sender as ListBox).SelectedIndices) {
                code.SelectFigure(((sender as ListBox).Items[index] as ListBoxFigure).Id);
            }

            code.DrawFigures(screen);
        }


        #region API
        private void Code_SelectedTool_Changed(Figure value, EventArgs e) {
            List<BuildingMethod> pbm = ReturnPossibleBuildingVariants(value);
            MainFormCmbbxBuildingVariants.Items.Clear();
            var cbm = new ComboboxBuildingMethod[pbm.Count];
            for (Int32 i = 0; i < pbm.Count; i++) {
                cbm[i] = new ComboboxBuildingMethod(pbm[i]);
            }
            MainFormCmbbxBuildingVariants.Items.AddRange(cbm);
        }
        //!!!MainForm#42.1: выделение нескольких элементов некорректно работает
        private void Code_SelectedBuildingMethod_Changed(BuildingMethod value, EventArgs e) {
            for (Int32 i = 0; i < MainFormCmbbxBuildingVariants.Items.Count; i++) {
                if ((MainFormCmbbxBuildingVariants.Items[i] as ComboboxBuildingMethod).BuildingMethod == value) {
                    MainFormCmbbxBuildingVariants.SelectedIndex = i;
                    break;
                }
            }

        }
        //!!!MainForm#42: исправить в соответствии с возможностью выбрать несколько элементов
        private void Code_FiguresList_Changed(object sender, EventArgs e) {
            Int32 currListSelectedIndex = MainFormLstbxFigures.SelectedIndex;
            Boolean wasSmnSelected = currListSelectedIndex != -1;
            Int32 currListSelectedItemId = -1;
            if (wasSmnSelected) {
                currListSelectedItemId = (MainFormLstbxFigures.SelectedItem as ListBoxFigure).Id;
            }

            MainFormLstbxFigures.Items.Clear();
            var figuresToListboxList = new List<ListBoxFigure>();
            var figuresList = sender as MyListContainer<MyFigure>;
            for (Int32 i = 0; i < figuresList.Count; i++) {
                figuresToListboxList.Add(
                    new ListBoxFigure(figuresList[i]));
            }
            MainFormLstbxFigures.Items.AddRange(figuresToListboxList.ToArray());

            if (wasSmnSelected) {
                Int32 listBoxItemToSelectIndex = -1;
                for (Int32 i = 0; i < MainFormLstbxFigures.Items.Count; i++) {
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
                code.AddPolarLine(ControlPointToScreen(Cursor.Position, MainFromPctrbxScreen));
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
        private void MyCursor_SnapTorned(Object sender, EventArgs e) {
            code.RemoveSnapPoint();
        }
        #endregion


        /// <summary> Вычисляет местоположение мыши в координатах контрола. </summary>
        private Point ControlPointToScreen(Point location, Control controlOnForm) {
            Point out_point = PointToScreen(location);
            out_point.X += controlOnForm.Location.X;
            out_point.Y += controlOnForm.Location.Y;
            return out_point;
        }

        private void button1_Click(Object sender, EventArgs e) {
            var p2 = new PointF(1, 1);
            var p3 = new PointF(4, 4);
            var p1 = new PointF(1, 3);
            PointF result = MakeProjectionOnPolarLine(p1, p2, p3);
            //Ожидается: (2, 2)
        }
        private PointF MakeProjectionOnPolarLine(in PointF p1, in PointF p2, in PointF p3) {
            Single x1 = p2.X;
            Single y1 = p2.Y;
            Single x2 = p3.X;
            Single y2 = p3.Y;
            Single x3 = p1.X;
            Single y3 = p1.Y;
            Single a = x2 - x1;
            Single b = y2 - y1;
            Single x = (a * b * y3 - a * a * x1 + a * x3 - b * y1 + b * b + a * a) / ((b - a) * (b + a));
            Single y = (y3 * b * b - a * a * y1 + a * b * (x1 - x3)) / ((1 - a) * (1 + a));
            return new PointF(x, y);
        }


    }
}
//MainForm#46: Поменять таймер на MouseMowe
//[Closed]: неактуально, пока используется Graphics.Clear()