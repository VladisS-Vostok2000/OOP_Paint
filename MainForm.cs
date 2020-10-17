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

//!!!Projekt#14: смена фигуры во время рисования вызывает непредвиденную ошибку.
//!!!Projekt#20: добавить магнитную привязку
//!!!Projekt#30: добавить полярные линии
//Projekt#40: добавить подсветку фигур
//MainForm#41: привязать выделение в листе к выделению фигуры
namespace OOP_Paint {
    //!!!MainForm#20: добавить плавающие контролы
    public sealed partial class MainForm : Form {
        private readonly Graphics screen;
        private readonly MainCode Code = new MainCode();



        public MainForm() {
            InitializeComponent();
            screen = MainFromPctrbxScreen.CreateGraphics();
            Code.SelectedFigureChanged += Code_SelectedFigure_Changed;
            Code.SelectedBuildingVariantChanged += Code_SelectedBuildingMethod_Changed;
            Code.FiguresListChanged += Code_FiguresListChanged;
            MainFormCmbbxBuildingVariants.DisplayMember = "Name";
            MainFormCmbbxBuildingVariants.ValueMember = "BuildingMethod";
            MainFormLstbxFigures.DisplayMember = "Name";
            MainFormLstbxFigures.ValueMember = "Id";
        }
        private void MainForm_Load(Object sender, EventArgs e) {

        }



        private void MainFormPctrbxScreen_MouseMove(Object sender, MouseEventArgs e) {
            Point mouseLocation = e.Location;
            MainFormSttsstpLblMouseX.Text = mouseLocation.X.ToString().PadLeft(3);
            MainFormSttsstpLblMouseY.Text = mouseLocation.Y.ToString().PadLeft(3);

            Code.AddSoftPoint(e.Location);
        }
        private void MainFromPctrbxScreen_MouseUp(Object sender, MouseEventArgs e) {
            if (e.X > (sender as PictureBox).Width || e.X < 0 ||
                e.Y > (sender as PictureBox).Height || e.Y < 0) {
                return;
            }

            ConstructorOperationResult constructorResult = Code.SetPoint(e.Location);
            if (constructorResult.Result == ConstructorOperationResult.OperationStatus.Continious) {
                MainFormTmr.Enabled = true;
                MainFormSttsstpLblHint.Text = constructorResult.OperationMessage;
            }
            else
            if (constructorResult.Result == ConstructorOperationResult.OperationStatus.Canselled) {
                MainFormTmr.Enabled = false;
                MainFormSttsstpLblHint.Text = "Отменено";
            }
            else
            if (constructorResult.Result == ConstructorOperationResult.OperationStatus.Finished) {
                MainFormTmr.Enabled = false;
                Code.DrawFigures(screen);
                MainFormSttsstpLblHint.Text = "Успешно.";
            }
        }
        private void MainFormTmr_Tick(Object sender, EventArgs e) {
            Code.DrawFigures(screen);
        }


        private void Figures_ListChanged(Object sender, ListChangedEventArgs e) {
            MainFormLstbxFigures.Items.Clear();
            foreach (var figure in sender as BindingList<MyFigure>) {
                MainFormLstbxFigures.Items.Add(figure.ToString() + $": ({figure.X},{figure.Y})");
            }
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
        private void MainFormCmbbxBuildingVariants_SelectedIndexChanged(Object sender, EventArgs e) {
            Code.SelectedBuildingMethod = ((ComboboxBuildingMethod)MainFormCmbbxBuildingVariants.SelectedItem).BuildingMethod;
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
        private void Code_SelectedBuildingMethod_Changed(BuildingMethod _value, EventArgs e) {
            for (Int32 i = 0; i < MainFormCmbbxBuildingVariants.Items.Count; i++) {
                if ((MainFormCmbbxBuildingVariants.Items[i] as ComboboxBuildingMethod).BuildingMethod == _value) {
                    MainFormCmbbxBuildingVariants.SelectedIndex = i;
                    break;
                }
            }

        }
        private void Code_FiguresListChanged(Object sender, ListChangedEventArgs e) {
            Int32 currListSelectedIndex = MainFormLstbxFigures.SelectedIndex;
            Boolean wasSmnSelected = currListSelectedIndex != -1;
            Int32 currListSelectedItemId = -1;
            if (wasSmnSelected) {
                currListSelectedItemId = (MainFormLstbxFigures.SelectedItem as ListBoxFigure).Id;
            }

            MainFormLstbxFigures.Items.Clear();
            var toListboxList = new List<ListBoxFigure>();
            var figuresList = sender as BindingList<MyFigure>;
            for (Int32 i = 0; i < figuresList.Count; i++) {
                toListboxList.Add(
                    new ListBoxFigure(figuresList[i]));
            }
            MainFormLstbxFigures.Items.AddRange(toListboxList.ToArray());

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

    }
}
//MainForm#46: Поменять таймер на MouseMowe
//[Closed]: неактуально, пока используется Graphics.Clear()