using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static OOP_Paint.FiguresEnum;

//!!!Projekt#01: смена фигуры во время рисования вызывает непредвиденную ошибку.
//Projekt#20: добавить магнитную привязку
//!!!Projekt#30: добавить полярные линии
//!!!Projekt#50: добавить масштаб
//!!!Projekt#07: добавить модификаторы in
//Projekt#40: добавить контейнер фигур
namespace OOP_Paint {
    //!!!MainForm#20: добавить плавающие контролы
    public sealed partial class MainForm : Form {
        private readonly int snapDistance = 5;

        private readonly Bitmap bitmap;
        private readonly Graphics screen;
        private readonly MainCode Code;
        private readonly MyCursor myCursor;




        public MainForm(MainCode _code) {
            Code = _code;

            InitializeComponent();

            bitmap = new Bitmap(496, 290);
            screen = Graphics.FromImage(bitmap);
            myCursor = new MyCursor(snapDistance);

            Code.SelectedFigureChanged += Code_SelectedFigure_Changed;
            Code.SelectedBuildingVariantChanged += Code_SelectedBuildingMethod_Changed;
            Code.FiguresListChanged += Code_FiguresListChanged;
            MainFormCmbbxBuildingVariants.DisplayMember = "DisplayMember";
            MainFormCmbbxBuildingVariants.ValueMember = "BuildingMethod";
            MainFormLstbxFigures.DisplayMember = "DisplayMember";
            MainFormLstbxFigures.ValueMember = "Id";
        }
        private void MainForm_Load(Object sender, EventArgs e) {

        }



        private void MainFromPctrbxScreen_MouseDown(Object sender, MouseEventArgs e) {
            //MainFormTmr.Stop();
            if (Code.SelectedFigure == Figure.Select) {
                //MainFormTmr.Enabled = true;
                Code.SetPoint(e.Location);
            }
            //MainFormTmr.Start();
        }
        private void MainFormPctrbxScreen_MouseMove(Object sender, MouseEventArgs e) {
            Point mouseLocation = e.Location;
            MainFormSttsstpLblMouseX.Text = mouseLocation.X.ToString().PadLeft(3);
            MainFormSttsstpLblMouseY.Text = mouseLocation.Y.ToString().PadLeft(3);

            Code.AddSoftPoint(e.Location);

            if (myCursor.IsSnapped) {
                myCursor.ContinueSnap(e.Location);
            }
            else {
                if (e.Button == MouseButtons.None) {
                    PointF vetrex = Code.FindNearestVertex(e.Location);
                    float distance = MyFigure.FindLength(vetrex, e.Location);
                    if (distance <= snapDistance) {
                        myCursor.DoSnap(e.Location);
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

            ConstructorOperationResult constructorResult = Code.SetPoint(e.Location);
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
                Code.DrawFigures(screen);
                MainFormSttsstpLblHint.Text = "Успешно.";
            }
        }
        private void MainFormTmr_Tick(Object sender, EventArgs e) {
            Code.DrawFigures(screen);
            Display();
        }
        private void Display() {
            MainFromPctrbxScreen.Image = bitmap;
        }


        private void MainFormBttnCircle_Click(Object sender, EventArgs e) {
            Figure firgureToSelect = Figure.Circle;
            Code.SelectedFigure = firgureToSelect;

            //Это, наверное, всё же лучше запихуть в Code в этой реализации, т.к.
            //сообщение одно для всех платформ. Однако для разных людей это не так.
            //Вопрос отложен.
            MainFormSttsstpLblHint.Text = "Окружность, ограниченная прямоугольником. Выберете первую точку";

        }
        private void MainFormBttnRectangle_Click(Object sender, EventArgs e) {
            Code.SelectedFigure = Figure.Rectangle;
            MainFormSttsstpLblHint.Text = "Прямоугольник. Выберете первую точку";
        }
        private void MainFormBttnCut_Click(Object sender, EventArgs e) {
            Code.SelectedFigure = Figure.Cut;
            MainFormSttsstpLblHint.Text = "Отрезок. Выберете первую точку";
        }
        private void MainFormBttnSelect_Click(Object sender, EventArgs e) {
            Code.SelectedFigure = Figure.Select;
        }
        private void MainFormBttnNothing_Click(Object sender, EventArgs e) {
            Code.SelectedFigure = Figure.None;
        }

        private void MainFormCmbbxBuildingVariants_SelectedIndexChanged(Object sender, EventArgs e) {
            Code.SelectedBuildingMethod = ((ComboboxBuildingMethod)MainFormCmbbxBuildingVariants.SelectedItem).BuildingMethod;
        }
        private void MainFormLstbxFigures_SelectedIndexChanged(Object sender, EventArgs e) {
            //Мы не можем знать, снялось выделение или появилось, таким образом нужно снять выделения
            //со всех фигур и задать их заново
            Int32 figuresCount = Code.GetFiguresCount();
            for (Int32 i = 0; i < figuresCount; i++) {
                Code.UnselectFigure(((sender as ListBox).Items[0] as ListBoxFigure).Id);
            }

            foreach (Int32 index in (sender as ListBox).SelectedIndices) {
                Code.SelectFigure(((sender as ListBox).Items[index] as ListBoxFigure).Id);
            }

            Code.DrawFigures(screen);
        }


        #region API
        private void Code_SelectedFigure_Changed(Figure _value, EventArgs e) {
            List<BuildingMethod> pbm = ReturnPossibleBuildingVariants(_value);
            MainFormCmbbxBuildingVariants.Items.Clear();
            var cbm = new ComboboxBuildingMethod[pbm.Count];
            for (Int32 i = 0; i < pbm.Count; i++) {
                cbm[i] = new ComboboxBuildingMethod(pbm[i]);
            }
            MainFormCmbbxBuildingVariants.Items.AddRange(cbm);
        }
        //!!!MainForm#42.1: выделение нескольких элементов некорректно работает
        private void Code_SelectedBuildingMethod_Changed(BuildingMethod _value, EventArgs e) {
            for (Int32 i = 0; i < MainFormCmbbxBuildingVariants.Items.Count; i++) {
                if ((MainFormCmbbxBuildingVariants.Items[i] as ComboboxBuildingMethod).BuildingMethod == _value) {
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
        #endregion



        private void button1_Click(Object sender, EventArgs e) {
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