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
using System.IO;

//!!!Projekt#01: смена фигуры во время рисования вызывает непредвиденную ошибку.
//!!!Projekt#50: добавить масштаб
//!!!MyClient#20: переименовать перечисления-названия фигур в инструменты с соответствующим
//!!!Projekt#05: расчленить MyClient на MyFigureConstructor и MyCanvas
namespace CAD_Client {
    //!!!MainForm#20: добавить плавающие контролы
    internal sealed partial class GUI_Form : Form {
        private int scale = 1;

        private readonly MyMathPlane myMathPlane;
        private readonly MyClient constructor;
        private readonly MyCursor myCursor;
        private readonly MyCanvas myCanvas;
        private int snapDistancePx = 10;

        private Point prewiousCursorScreenPosition;



        internal GUI_Form(MyMathPlane myMathPlane) {
            InitializeComponent();

            myCursor = new MyCursor();
            myCanvas = new MyCanvas(MainFromPctrbxScreen.Width, MainFromPctrbxScreen.Height);
            constructor = new MyClient(myMathPlane);
            this.myMathPlane = myMathPlane;

            this.constructor.SelectedToolChanged += Code_SelectedTool_Changed;
            this.constructor.SelectedBuildingVariantChanged += Code_SelectedBuildingMethod_Changed;
            this.constructor.FiguresListChanged += Code_FiguresList_Changed;
            this.constructor.ConstructorOperationStatusChanged += MainCode_ConstructorOperationStatus_Changed;
            this.constructor.PolarLineEnablingChanged += Code_PolarLineEnabling_Changed;

            MainFormCmbbxBuildingVariants.DisplayMember = "DisplayMember";
            MainFormCmbbxBuildingVariants.ValueMember = "BuildingMethod";
            MainFormLstbxFigures.DisplayMember = "DisplayMember";
            MainFormLstbxFigures.ValueMember = "Id";
        }
        private void MainForm_Load(object sender, EventArgs e) {

        }



        #region Работа с экраном
        private void MainFromPctrbxScreen_MouseDown(object sender, MouseEventArgs e) {
            Debugger.Log($"Начато: MainFromPctrbxScreen_MouseDown");
            if (constructor.SelectedTool == Tool.Select) {
                //SomeCodeHere - Selection
            }
        }
        private void MainFormPctrbxScreen_MouseMove(object sender, MouseEventArgs e) {
            // Пиксельное - отображение на экране относительно bitmap
            // Экранное - отображение на экране относительно самого экрана
            // Реальное - точка в настоящих координатах
            // Cursor == Mouse
            // RealCoord == RealLocation
            // Px == BitmapLocation
            Point cursorPxLocation = e.Location;
            Debugger.Log($"Начат MainFormPctrbxScreen_MouseMove,");
            Debugger.Log($"cursorPxLocation = e.Location = ({e.X}; {e.Y})");
            Debugger.Log($"myScreenOffset: ({myCanvas.Location.X}; {myCanvas.Location.Y})");
            PointF cursorRealLocation = myCanvas.ToReal(cursorPxLocation);
            Debugger.Log($"realCoord = ({cursorRealLocation.X}; {cursorRealLocation.Y})");
            constructor.AddSoftPoint(cursorRealLocation);

            #region Привязка
            if (myCursor.Snapped) {
                Point cursorScreenLocation = (sender as Control).PointToScreen(cursorPxLocation);
                Debugger.Log($"cursorScreenLocation = ({cursorScreenLocation.X}; {cursorScreenLocation.Y})");
                myCursor.ContinueSnap(cursorScreenLocation);
            }
            //Создание привязки
            else
            //???Не очень нравится это решение. Почему ФОРМА должна знать о количестве фигур?
            if (myMathPlane.Count != 0 && constructor.SelectedTool == Tool.None) {
                PointF vertexRealLocation = constructor.FindNearestVertex(cursorRealLocation);
                Debugger.Log($"vertexRealLocation: ({vertexRealLocation.X}; {vertexRealLocation.Y})");
                Point vertexPxLocation = myCanvas.ToPx(vertexRealLocation);
                Debugger.Log($"vertexPxLocation: ({vertexPxLocation.X}; {vertexPxLocation.Y})");
                bool IsVertexClose = myCanvas.CheckSnap(cursorPxLocation, vertexPxLocation, snapDistancePx);
                if (IsVertexClose) {
                    //Snap выполняется для экранных координат.
                    Point vertexScreenLocation = (sender as Control).PointToScreen(vertexPxLocation);
                    Debugger.Log($"vertexScreenLocation: ({vertexScreenLocation.X}; {vertexScreenLocation.Y})");
                    myCursor.CreateSnap(vertexScreenLocation, snapDistancePx, true);
                }
            }
            #endregion

            #region Перемещение экрана
            if (constructor.SelectedTool == Tool.Moving && e.Button == MouseButtons.Middle) {
                Point mouseScreenLocation = (sender as Control).PointToScreen(e.Location);
                Debugger.Log($"mouseScreenLocation: ({mouseScreenLocation.X}; {mouseScreenLocation.Y})");
                Debugger.Log($"offset: ({prewiousCursorScreenPosition.X}; {prewiousCursorScreenPosition.Y})");
                Point offset = (mouseScreenLocation.Substract(prewiousCursorScreenPosition)).Invert();
                Debugger.Log($"offset: ({offset.X}; {offset.Y})");
                myCanvas.MoveOn(offset);
            }
            #endregion

            prewiousCursorScreenPosition = (sender as Control).PointToScreen(e.Location);

            MainFormSttsstpLblMouseX.Text = e.Location.X.ToString().PadLeft(3);
            MainFormSttsstpLblMouseY.Text = e.Location.Y.ToString().PadLeft(3);
        }
        private void MainFromPctrbxScreen_MouseUp(object sender, MouseEventArgs e) {
            Debugger.Log($"Начато: MainFromPctrbxScreen_MouseUp");
            //За пределами экрана
            if (e.X > (sender as PictureBox).Width || e.X < 0 ||
                e.Y > (sender as PictureBox).Height || e.Y < 0) {
                return;
            }

            if (constructor.SelectedTool == Tool.Moving && e.Button != MouseButtons.Middle) {
                myCursor.StopSnap();
            }

            Debugger.Log($"e.Location = ({e.Location.X}; {e.Location.Y})");
            Debugger.Log($"myScreenLocation: ({myCanvas.Location.X}; {myCanvas.Location.Y})");
            Point realCoord = e.Location.Sum(myCanvas.Location);
            Debugger.Log($"realCoord = ({realCoord.X}; {realCoord.Y})");
            constructor.SetPoint(realCoord);
        }


        private void MainFormTmr_Tick(object sender, EventArgs e) {
            myCanvas.Clear();
            myCanvas.DrawGrid();
            ICollection<MyFigure> myFigures = myMathPlane.GetMyFigures();
            myCanvas.DrawFigures(myFigures);
            var constructorStatus = constructor.ConstructorOperationStatus;
            if (constructorStatus.Result == ConstructorOperationStatus.OperationStatus.Continious) {
                myFigures = constructor.GetSupportFiguresList();
                myCanvas.DrawFigures(myFigures);
            }
            try {
                Point snapLocation = myCursor.SnapLocation;
                myCanvas.DrawSnapPoint(PointToClient(snapLocation));
            }
            catch (Exception) { }
            MainFromPctrbxScreen.Image = myCanvas.Bitmap;
            MainFormSttsstrpLblHint.Text = constructorStatus.OperationMessage;
        }
        #endregion


        #region Кнопки
        private void MainFormBttnCircle_Click(object sender, EventArgs e) {
            Tool firgureToSelect = Tool.Circle;
            constructor.SelectedTool = firgureToSelect;

            //???Это, наверное, всё же лучше запихуть в Code в этой реализации, т.к.
            //сообщение одно для всех платформ. Однако для разных людей это не так.
            //->Вопрос отложен.
            MainFormSttsstrpLblHint.Text = "Окружность, ограниченная прямоугольником. Выберете первую точку";

        }
        private void MainFormBttnRectangle_Click(object sender, EventArgs e) {
            constructor.SelectedTool = Tool.Rectangle;
            MainFormSttsstrpLblHint.Text = "Прямоугольник. Выберете первую точку";
        }
        private void MainFormBttnCut_Click(object sender, EventArgs e) {
            constructor.SelectedTool = Tool.Cut;
            MainFormSttsstrpLblHint.Text = "Отрезок. Выберете первую точку";
        }
        private void MainFormBttnSelect_Click(object sender, EventArgs e) {
            constructor.SelectedTool = Tool.Select;
        }
        private void MainFormBttnNothing_Click(object sender, EventArgs e) {
            constructor.SelectedTool = Tool.None;
        }
        private void MainFormBttnMove_Click(object sender, EventArgs e) {
            constructor.SelectedTool = Tool.Moving;
        }

        private void MainFormCmbbxBuildingVariants_SelectedIndexChanged(object sender, EventArgs e) {
            constructor.SelectedBuildingMethod = ((ComboboxBuildingMethod)MainFormCmbbxBuildingVariants.SelectedItem).BuildingMethod;
        }
        private void MainFormLstbxFigures_SelectedIndexChanged(object sender, EventArgs e) {
            //Мы не можем знать, снялось выделение или появилось, таким образом нужно снять выделения
            //со всех фигур и задать их заново
            int figuresCount = myMathPlane.Count;
            for (int i = 0; i < figuresCount; i++) {
                constructor.UnselectFigure(((sender as ListBox).Items[0] as ListBoxFigure).Id);
            }

            foreach (int index in (sender as ListBox).SelectedIndices) {
                constructor.SelectFigure(((sender as ListBox).Items[index] as ListBoxFigure).Id);
            }
        }
        private void MainFormTlstrpSpltbttnPolarLine_Click(object sender, EventArgs e) {
            constructor.PolarLineEnabled = !constructor.PolarLineEnabled;
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
                MainFormSttsstrpLblHint.Text = sender.OperationMessage;
            }
            else
            if (sender.Result == ConstructorOperationStatus.OperationStatus.Canselled) {
                MainFormSttsstrpLblHint.Text = "Отменено";
            }
            else
            if (sender.Result == ConstructorOperationStatus.OperationStatus.Finished) {
                MainFormSttsstrpLblHint.Text = "Успешно.";
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


        private void button1_Click(object sender, EventArgs e) {

        }
        private void button1_MouseUp(object sender, MouseEventArgs e) {
            //Cursor.Position = (sender as Control).PointToScreen(e.Location);
        }


    }
}
//MainForm#46: Поменять таймер на MouseMowe
//[Closed]: неактуально, пока используется Graphics.Clear()



//[Вопросы]:
//internal Pen Pen {
//    set {
//        if (value == null) {
//            throw new ArgumentNullException();
//        }
//        if (value != pen) {
//            pen = value;
//        }
//    }
//    //??? pen имеет модификатор доступности блока set private, хотя по факту общедоступен.
//    //Решено: pen можно менять как угодно, но присвоить ему null невозможно, так что ошибки исключены.
//    get => pen;
//}