using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace OOP_Paint {
    public partial class MainForm : Form {
        private event EventHandler SelectedFigureChanged;
        enum Figure {
            Circle,
            Rectangle,
        }
        private Figure currFigue;
        private Figure CurrFigure {
            set     {
                if (currFigue - value != 0) {
                    SelectedFigureChanged(this, new EventArgs());
                    currFigue = value;
                }
            }
            get => currFigue;
        }
        private Int32 currFigureConstructorLevel;
        private Graphics screen;
        private List<Control> figureConstructor;
        private const Int32 constructorMargin = 3;
        private Int32 currClick;
        private List<MyFigure> FiguresToDrawList;
        private List<Control> test_Circle;

        public MainForm() {
            InitializeComponent();
            screen = MainFromPctrbxScreen.CreateGraphics();
            SelectedFigureChanged += MainForm_SelectedFigureChanged;
            figureConstructor = new List<Control>();
        }
        private void Form1_Load(object sender, EventArgs e) {
            //Пробую создание конструктора
            test_Circle = new List<Control>();
            var lbl = new Label() {
                Text = "Вариант построения",
                TabStop = true,
                Location = new Point(MainFormCmbbxFigureChoose.Location.X +
                MainFormCmbbxFigureChoose.Width +
                MainFormCmbbxFigureChoose.Margin.Right,
                MainFormLblFigureChoose.Location.Y)
            };
            var cmbbx = new ComboBox() {
                TabIndex = 1,
                Location = new Point(lbl.Location.X, lbl.Location.Y + lbl.Height + lbl.Margin.Bottom)
            };
            cmbbx.Items.AddRange(new string[] { "Ограниченная прямоугольником", "Описывающая прямоугольник"});
            test_Circle.Add(lbl);
            test_Circle.Add(cmbbx);

            //Попытка #2
            FiguresToDrawList = new List<Figure>();
        }
        private void Form1_Shown(object sender, EventArgs e) {
        }






        private void MainForm_SelectedFigureChanged(object sender, EventArgs e) {
            foreach (Control cntcl in figureConstructor) {
                cntcl.Dispose();
            }
            //???Нужно его очищать?
            figureConstructor.Clear();

            currFigureConstructorLevel = 1;



            switch (CurrFigure) {
                case Figure.Circle:
                    //(1, 150, "Выбор построения", "Ограниченный областью", "Описывающая прямоугольник", "Три точки");

                    //???А как насчёт динамического добавления полей? Тогда не придётся ебаться с координатными
                    //константами. А придётся ебаться с уникальными ивентами... Или же они вычисляются
                    //в redraw event?..
                    //var lbl = new Label() {
                    //    Name = "MainFormLbl1BuildingVariant",
                    //    TabStop = true,
                    //    Location = new Point(),
                    //};
                    //figureConstructor.Add(lbl);

                    //var cmbbx = new ComboBox() {
                    //    Name = "MainFormCmbbx1BuildingVariant",
                    //    TabIndex = 1,
                    //    Width = 150,
                    //    DropDownStyle = ComboBoxStyle.DropDownList,
                    //    X = ,
                    //    Y = ,
                    //};
                    //string[] m = { "Ограниченный прямоугольником", "Описанный вокруг прямоугольника", "Три точки" };
                    //cmbbx.Items.AddRange(m);
                    //figureConstructor.Add(cmbbx);
                    //(figureConstructor[figureConstructor.Count] as ComboBox).SelectedIndexChanged += MainForm_FigureConstructorCombobox_SelectedIndexChanged;

                    //var 




                    break;
                case Figure.Rectangle:
                    break;
            }
        }


        private Point FindConstructorControlLocation(Int32 _constructorLevel) {
            Control currControl = MainFormCmbbxFigureChoose;
            for (Int32 i = figureConstructor.Count - 1; i > 0; i--) {
                if (figureConstructor[i].TabIndex > _constructorLevel - 2) {
                    break;
                }

                if (currControl.Location.X + currControl.Width > figureConstructor[i].Location.X + figureConstructor[i].Width) {
                    currControl = figureConstructor[i];
                }
            }

            return new Point(
                currControl.Location.X +
                currControl.Width +
                currControl.Margin.Right,
                MainFormLblFigureChoose.Location.Y);
        }
        private ComboBox CreateNewContstructorCombobox(Int32 _tabIndex, Int32 _width, String _labelText, params String[] _textVariants) {
            Point location = FindConstructorControlLocation(_tabIndex);
            
            var cmbbx = new ComboBox() {
                TabIndex = _tabIndex,
                Width = _width,
                Location = location,
            };
            cmbbx.Items.AddRange(_textVariants);
            return cmbbx;
        }
        private void MainFormCmbbxFigureChoose_SelectionChangeCommitted(object sender, EventArgs e) {
            switch ((sender as ComboBox).SelectedItem) {
                case "Круг":
                    CurrFigure = Figure.Circle;
                    break;
                case "Прямоугольник":
                    CurrFigure = Figure.Rectangle;
                    break;
                default: throw new Exception();
            }
        }
        private void MainForm_FigureConstructorCombobox_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void MainForm_Paint(Object sender, PaintEventArgs e) {
        }

        private void MainFromPctrbxScreen_Click(Object sender, EventArgs e) {
            currClick++;
            switch(currClick) {
                case 1:
                    FiguresToDrawList.Add(new MyPoint((int)numericUpDown1.Value, (int)numericUpDown2.Value, Color.Red));
                    break;
                case 2:
                    FiguresToDrawList.Remove(new MyRectangle)
            }
        }

        private void numericUpDown1_ValueChanged(Object sender, EventArgs e) {

        }
    }
}
