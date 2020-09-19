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
        private enum Figure {
            Null = 0,
            Circle,
            Rectangle,
        }
        private Figure currSelectedFigure;
        private MyFigure currSupportFigure;
        private Int32 currConstructorStage;
        private static readonly Pen supportPen = new Pen(Color.Red) { Width = 2, DashStyle = DashStyle.Dash };
        private readonly List<MyFigure> figuresToDrawList;
        private readonly List<Control> figureConstructorList;

        private readonly Graphics screen;


        public MainForm() {
            InitializeComponent();
            figuresToDrawList = new List<MyFigure>();
            screen = MainFromPctrbxScreen.CreateGraphics();
            currConstructorStage = 0;
            figureConstructorList = new List<Control>();
        }
        private void Form1_Load(Object sender, EventArgs e) {
            #region Пробую создание конструктора
            //test_Circle = new List<Control>();
            //var lbl = new Label() {
            //    Text = "Вариант построения",
            //    TabStop = true,
            //    Location = new Point(MainFormCmbbxFigureVariants.Location.X +
            //    MainFormCmbbxFigureVariants.Width +
            //    MainFormCmbbxFigureVariants.Margin.Right,
            //    MainFormLblFigureChoose.Location.Y)
            //};
            //var cmbbx = new ComboBox() {
            //    TabIndex = 1,
            //    Location = new Point(lbl.Location.X, lbl.Location.Y + lbl.Height + lbl.Margin.Bottom)
            //};
            //cmbbx.Items.AddRange(new String[] { "Ограниченная прямоугольником", "Описывающая прямоугольник"});
            //test_Circle.Add(lbl);
            //test_Circle.Add(cmbbx);

            ////Попытка #2
            //FiguresToDrawList = new List<Figure>();
            #endregion
        }
        private void Form1_Shown(Object sender, EventArgs e) {
        }


        //Логика GUI
        private void MainFormTmr_Tick(Object sender, EventArgs e) {

        }
        private void AddPointToConstructor(Point _coord) {
            switch (currSelectedFigure) {
                case Figure.Circle:
                    switch (currConstructorStage) {
                        case 0:
                            //??? Логика связана с GUI, очень нехорошо, но идей нет
                            MainFormStttsstpLblHint.Text = "Окружность, ограниченная прямоугольником. Выберете первую точку";
                            currSupportFigure = new MyRectangle(_coord.X, _coord.Y, _coord.X, _coord.Y, supportPen);


                            currConstructorStage++;
                            break;
                        default: throw new Exception();
                    }


                    break;
                default: throw new NotImplementedException();
            }
        }


        //GUI
        private void MainFormSttsstp_MouseMove(Object sender, MouseEventArgs e) {
            Point mouseLocation = e.Location;
            MainFormSttsstpLblMouseX.Text = mouseLocation.X.ToString();
            MainFormSttsstpLblMouseY.Text = mouseLocation.Y.ToString();
        }
        private void MainFromPctrbxScreen_MouseUp(Object sender, MouseEventArgs e) {
            if (currSelectedFigure == Figure.Null) {
                return;
            }

            AddPointToConstructor(e.Location);
        }

    }
}
