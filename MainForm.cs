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

namespace OOP_Paint {
    public sealed partial class MainForm : Form {
        private readonly Graphics screen;
        private readonly MainCode Code = new MainCode();
        private bool isFigureDrawing = false;
        private static int test = 0;
        public MainForm() {
            InitializeComponent();
            screen = MainFromPctrbxScreen.CreateGraphics();
            Code.Figures.ListChanged += Figures_ListChanged;

        }

        private void Figures_ListChanged(Object sender, ListChangedEventArgs e) {
            MainFormLstbxFigures.Items.Clear();
            foreach (var figure in sender as BindingList<MyFigure>) {
                MainFormLstbxFigures.Items.Add(figure.Name + $": ({figure.X},{figure.Y})");
            }

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
            ConstructorResult constructorResult = Code.ThreatMouseEvent(e);
        }
        private void MainFromPctrbxScreen_MouseUp(Object sender, MouseEventArgs e) {
            if (e.X > (sender as PictureBox).Width || e.X < 0 ||
                e.Y > (sender as PictureBox).Height || e.Y < 0) {
                return;
            }

            ConstructorResult constructorResult = Code.ThreatMouseEvent(e);
            if (constructorResult.Result == ConstructorResult.OperationStatus.Continious) {
                MainFormTmr.Enabled = true;
                MainFormSttsstpLblHint.Text = constructorResult.OperationMessage;
            }
            else
            if (constructorResult.Result == ConstructorResult.OperationStatus.Canselled) {
                MainFormTmr.Enabled = false;
                MainFormSttsstpLblHint.Text = "Отменено";
            }
            else
            if (constructorResult.Result == ConstructorResult.OperationStatus.Finished) {
                MainFormTmr.Enabled = false;
                Code.DrawFigures(screen);
                MainFormSttsstpLblHint.Text = "Успешно.";
            }

        }


        private void MainFormBttnCircle_Click(Object sender, EventArgs e) {
            MainFormSttsstpLblHint.Text = "Окружность, ограниченная прямоугольником. Выберете первую точку";
            Code.CurrSelectedFigure = MainCode.Figure.Circle;
            MainFormCmbbxBuildingVariants.Items.Clear();
            MainFormCmbbxBuildingVariants.Items.AddRange(Code.ReturnPossibleBuildingVariantsAsString());

        }


        private void MainFormTmr_Tick(Object sender, EventArgs e) {
            Code.DrawFigures(screen);
        }
    }
}
