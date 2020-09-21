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
        public MainForm() {
            InitializeComponent();
            screen = MainFromPctrbxScreen.CreateGraphics();
            Code.Figures.ListChanged += Figures_ListChanged1;
        }


        private void Figures_ListChanged1(Object sender, ListChangedEventArgs e) {
            MainFormLstbxFigures.Items.Clear();
            foreach (var figure in sender as BindingList<MyFigure>) {
                MainFormLstbxFigures.Items.Add(figure.Name + $": ({figure.X},{figure.Y})");
            }
        }

        private void Form1_Load(Object sender, EventArgs e) {

        }
        private void Form1_Shown(Object sender, EventArgs e) {

        }


        private void MainFormSttsstp_MouseMove(Object sender, MouseEventArgs e) {
            Point mouseLocation = e.Location;
            MainFormSttsstpLblMouseX.Text = mouseLocation.X.ToString();
            MainFormSttsstpLblMouseY.Text = mouseLocation.Y.ToString();
            if (isFigureDrawing) {
               Code.DrawFigures(screen);
            }
        }
        private void MainFromPctrbxScreen_MouseUp(Object sender, MouseEventArgs e) {
            if (e.X > (sender as PictureBox).Width || e.X < 0 || 
                e.Y>(sender as PictureBox).Height || e.Y < 0) {
                return;
            }

            ConstructorResult constructorResult = Code.AddMouseClick(e.Location);
            if (constructorResult.Result == ConstructorResult.OperationStatus.Continious) {
                isFigureDrawing = true;
            }
            else
            if (constructorResult.Result == ConstructorResult.OperationStatus.Canselled) {
                isFigureDrawing = false;
                MainFormSttsstpLblHint.Text = "Отменено";
            }
            else
            if (constructorResult.Result == ConstructorResult.OperationStatus.Finished) {
                isFigureDrawing = false;
                Code.DrawFigures(screen);
                MainFormSttsstpLblHint.Text = "Успешно.";
            }
            
        }

        private void MainFormBttnCircle_Click(Object sender, EventArgs e) {
            MainFormSttsstpLblHint.Text = "Окружность, ограниченная прямоугольником. Выберете первую точку";
            Code.CurrSelectedFigure = MainCode.Figure.Circle;

        }

    }
}
