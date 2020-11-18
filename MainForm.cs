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
//!!!Projekt#30: добавить полярные линии
//!!!Projekt#50: добавить масштаб
//!!!Projekt#07: добавить модификаторы in
namespace OOP_Paint {
    //!!!MainForm#20: добавить плавающие контролы
    public sealed partial class MainForm : Form {
        private int scale = 1;
        private readonly int snapDistancePx = 10;

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

            this.code.SelectedFigureChanged += Code_SelectedFigureChanged;
            this.code.SelectedBuildingVariantChanged += Code_SelectedBuildingMethodChanged;
            this.code.FiguresListChanged += Code_FiguresListChanged;
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
            if (code.SelectedFigure == Figure.Select) {
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

            if (myCursor.IsSnapped) {
                myCursor.ContinueSnap(ControlPointToScreen(e.Location, MainFromPctrbxScreen));
            }
            else {
                if (e.Button == MouseButtons.None) {
                    int figCount = code.GetFiguresCount();
                    if (figCount != 0) {
                        PointF vetrex = code.FindNearestVertex(e.Location);
                        //Привязка считается в отображаемых пикселях
                        ConvertRealCoordToPx(vetrex, out Point vetrexPx);
                        float distance = MyFigure.FindLength(vetrexPx, e.Location);
                        if (distance < snapDistancePx) {
                            int x = (int)Math.Round(vetrex.X);
                            int y = (int)Math.Round(vetrex.Y);
                            Point point = ControlPointToScreen(new Point(x, y), MainFromPctrbxScreen);
                            Debugger.Log($"SnapCreating");
                            Debugger.Log($"MouseLocation: ({Cursor.Position.X};{Cursor.Position.Y})");
                            myCursor.DoSnap(point);
                            code.AddSnapPoint(new Point(x, y));
                        }
                    }
                }

            }
        }
        private void MainFromPctrbxScreen_MouseUp(Object sender, MouseEventArgs e) {
            //За пределами экрана
            if (e.X > (sender as PictureBox).Width || e.X < 0 ||
                e.Y > (sender as PictureBox).Height || e.Y < 0) {
                return;
            }

            ConstructorOperationResult constructorResult = code.SetPoint(e.Location);
            if (constructorResult.Result == ConstructorOperationResult.OperationStatus.Continious) {
                //MainFormTmr.Enabled = true;
                MainFormSttsstpLblHint.Text = constructorResult.OperationMessage;
            }
            else
            if (constructorResult.Result == ConstructorOperationResult.OperationStatus.Canselled) {
                //MainFormTmr.Enabled = false;
                MainFormSttsstpLblHint.Text = "Отменено";
            }
            else
            if (constructorResult.Result == ConstructorOperationResult.OperationStatus.Finished) {
                //MainFormTmr.Enabled = false;
                code.DrawFigures(screen);
                MainFormSttsstpLblHint.Text = "Успешно.";
            }
        }
        private void MainFormTmr_Tick(Object sender, EventArgs e) {
            code.DrawFigures(screen);
            Display();
            Debugger.Log("Display");
        }
        private void Display() {
            MainFromPctrbxScreen.Image = bitmap;
        }

        private void ConvertRealCoordToPx(PointF location, out Point pxLocation) {
            pxLocation = new Point {
                X = (int)Math.Round(location.X),
                Y = (int)Math.Round(location.Y)
            };
        }

        private void MainFormBttnCircle_Click(Object sender, EventArgs e) {
            Figure firgureToSelect = Figure.Circle;
            code.SelectedFigure = firgureToSelect;

            //Это, наверное, всё же лучше запихуть в Code в этой реализации, т.к.
            //сообщение одно для всех платформ. Однако для разных людей это не так.
            //Вопрос отложен.
            MainFormSttsstpLblHint.Text = "Окружность, ограниченная прямоугольником. Выберете первую точку";

        }
        private void MainFormBttnRectangle_Click(Object sender, EventArgs e) {
            code.SelectedFigure = Figure.Rectangle;
            MainFormSttsstpLblHint.Text = "Прямоугольник. Выберете первую точку";
        }
        private void MainFormBttnCut_Click(Object sender, EventArgs e) {
            code.SelectedFigure = Figure.Cut;
            MainFormSttsstpLblHint.Text = "Отрезок. Выберете первую точку";
        }
        private void MainFormBttnSelect_Click(Object sender, EventArgs e) {
            code.SelectedFigure = Figure.Select;
        }
        private void MainFormBttnNothing_Click(Object sender, EventArgs e) {
            code.SelectedFigure = Figure.None;
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
        private void Code_SelectedFigureChanged(Figure value, EventArgs e) {
            List<BuildingMethod> pbm = ReturnPossibleBuildingVariants(value);
            MainFormCmbbxBuildingVariants.Items.Clear();
            var cbm = new ComboboxBuildingMethod[pbm.Count];
            for (Int32 i = 0; i < pbm.Count; i++) {
                cbm[i] = new ComboboxBuildingMethod(pbm[i]);
            }
            MainFormCmbbxBuildingVariants.Items.AddRange(cbm);
        }
        //!!!MainForm#42.1: выделение нескольких элементов некорректно работает
        private void Code_SelectedBuildingMethodChanged(BuildingMethod value, EventArgs e) {
            for (Int32 i = 0; i < MainFormCmbbxBuildingVariants.Items.Count; i++) {
                if ((MainFormCmbbxBuildingVariants.Items[i] as ComboboxBuildingMethod).BuildingMethod == value) {
                    MainFormCmbbxBuildingVariants.SelectedIndex = i;
                    break;
                }
            }

        }
        //!!!MainForm#42: исправить в соответствии с возможностью выбрать несколько элементов
        private void Code_FiguresListChanged(Object sender, ListChangedEventArgs e) {
            Int32 currListSelectedIndex = MainFormLstbxFigures.SelectedIndex;
            Boolean wasSmnSelected = currListSelectedIndex != -1;
            Int32 currListSelectedItemId = -1;
            if (wasSmnSelected) {
                currListSelectedItemId = (MainFormLstbxFigures.SelectedItem as ListBoxFigure).Id;
            }

            MainFormLstbxFigures.Items.Clear();
            var figuresToListboxList = new List<ListBoxFigure>();
            var figuresList = sender as BindingList<MyFigure>;
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

        private void MyCursor_SnapTorned(object sender, EventArgs e) {
            code.RemoveSnapPoint();
        }
        #endregion


        private Point ControlPointToScreen(Point location, Control controlOnForm) {
            Point out_point = PointToScreen(location);
            out_point.X += controlOnForm.Location.X;
            out_point.Y += controlOnForm.Location.Y;
            return out_point;
        }


        private void button1_Click(Object sender, EventArgs e) {
            Debugger.Stop();
            //var a = new List<MyFigure>();
            //a.Add(new MyCut(Color.Black, new PointF(0, 0), new PointF(5, 5)));

            //PointF b = Code.FindNearestVertex(a, new PointF(3, 3));
            //MessageBox.Show(b.X.ToString() + ";" + b.Y.ToString());


            //int a = int.MaxValue;
            //a++;

            //Code.SelectedFigure = Figure.Cut;
            //Code.SelectedBuildingMethod = BuildingMethod.CutTwoPoints;
            //Code.SetPoint(new Point(10, 10));
            //Code.SetPoint(new Point(50, 50));
            //Code.SelectedFigure = Figure.None;
            //Code.AddSoftPoint(new PointF(48, 49));

            ////Вот здесь что-то странное
            ////k=-45*
            //var a = Code.FindCutArea(new PointF(10, 10), new PointF(50, 50), 2);
            //var f = Code.FindCutArea(new PointF(50, 50), new PointF(10, 10), 2);
            ////Ожидается:
            ////(10-1.4142;10+1.4142), (10+1.4142;10-1.4142), (50-1.4142;50+1.4142), (50+1.4142;50-1.4142)
            ////(8;11), (11;8), (48;51), (51;48)
            ////ok

            ////k=0
            //var b = Code.FindCutArea(new PointF(10, 10), new PointF(10, 50), 2);
            //var g = Code.FindCutArea(new PointF(10, 50), new PointF(10, 10), 2);
            ////Ожидается:
            ////(8;10), (12;10), (8;50), (12;50)
            ////ok

            ////y=0
            //var c = Code.FindCutArea(new PointF(10, 10), new PointF(50, 10), 2);
            //var h = Code.FindCutArea(new PointF(50, 10), new PointF(10, 10), 2);
            ////Ожидается:
            ////(10;8), (10;12), (50;8), (50;12)
            ////ok

            ////k=45*
            //var d = Code.FindCutArea(new PointF(10, 50), new PointF(50, 10), 2);
            //var i = Code.FindCutArea(new PointF(50, 10), new PointF(10, 50), 2);
            ////Ожидается:
            ////(10-1.4142;50-1.4142), (10+1.4142;10+1.4142), (50-1.4142;10-1.4142), (50+1.4142;10+1.4142)
            ////(8;48), (11;51), (48;8), (51;11)

            //MessageBox.Show("");
        }

    }
}
//MainForm#46: Поменять таймер на MouseMowe
//[Closed]: неактуально, пока используется Graphics.Clear()