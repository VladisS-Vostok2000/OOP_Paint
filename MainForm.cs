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


namespace OOP_Paint {
    public sealed partial class MainForm : Form {
        private readonly Graphics screen;
        private readonly MainCode Code = new MainCode();
        private static Int32 test = 0;



        public MainForm() {
            InitializeComponent();
            screen = MainFromPctrbxScreen.CreateGraphics();
            Code.SelectedFigureChanged += Code_SelectedFigure_Changed;

        }



        private void MainForm_Load(Object sender, EventArgs e) {

        }
        private void MainForm_Shown(Object sender, EventArgs e) {

        }



        private void MainFormPctrbxScreen_MouseMove(Object sender, MouseEventArgs e) {
            Point mouseLocation = e.Location;
            MainFormSttsstpLblMouseX.Text = mouseLocation.X.ToString().PadLeft(3);
            MainFormSttsstpLblMouseY.Text = mouseLocation.Y.ToString().PadLeft(3);
            test++;
            if (test == 1000) {
                MessageBox.Show(e.Clicks.ToString());
            }
            ConstructorOperationResult constructorResult = Code.ThreatMouseEvent(e);
        }
        private void MainFromPctrbxScreen_MouseUp(Object sender, MouseEventArgs e) {
            if (e.X > (sender as PictureBox).Width || e.X < 0 ||
                e.Y > (sender as PictureBox).Height || e.Y < 0) {
                return;
            }

            ConstructorOperationResult constructorResult = Code.ThreatMouseEvent(e);
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
            //Это всё в событие нужно запихнуть. А вообще его не должно быть, код сам вернёт, что нужно
            //MainFormCmbbxBuildingVariants.Items.Clear();
            //List<BuildingMethod> bm = ReturnPossibleBuildingVariants();
            //var listNames = new List<String>();
            //foreach (var cm in bm) {
            //    listNames.Add(cm.Name);
            //}
            //MainFormCmbbxBuildingVariants.SelectedIndex = 0;
            MainFormSttsstpLblHint.Text = "Окружность, ограниченная прямоугольником. Выберете первую точку";

        }


        //Работа с Code
        //!!!#MF5:Зацикленные методы
        private void MainFormCmbbxBuildingVariants_SelectedIndexChanged(Object sender, EventArgs e) {


        }
        private void Code_SelectedFigure_Changed(Object sender, EventArgs e) {
            List<BuildingMethod> fbm = ReturnPossibleBuildingVariants(Code.SelectedFigure);


        }
        private void Code_BuildingVariantChanged(Object sender, EventArgs e) {


        }

    }
}
