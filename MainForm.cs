using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace OOP_Paint {
    public partial class MainForm : Form {
        enum Figure {
            Circle = 1,
            Rectangle,
        }
        private Figure currSelectedFigure;
        private Figure CurrSelectedFigure {
            set {
                if (currSelectedFigure != value) {
                    SelectedFigureChanged(this, new EventArgs());
                    currSelectedFigure = value;
                }
            }
            get => currSelectedFigure;
        }
        private event EventHandler SelectedFigureChanged;
        //private Int32 currFigureConstructorLevel;
        //private List<Control> figureConstructor;
        //private const Int32 constructorMargin = 3;
        //private Int32 currClick;
        private MyFigure currSupportFigure;
        private Int32 currConstructorStage;
        private readonly Graphics screen;
        private readonly List<MyFigure> figuresToDrawList;


        public MainForm() {
            InitializeComponent();
            figuresToDrawList = new List<MyFigure>();
            screen = MainFromPctrbxScreen.CreateGraphics();
            SelectedFigureChanged += MainForm_SelectedFigureChanged;
            currConstructorStage = 0;
            //figureConstructor = new List<Control>();
        }
        private void Form1_Load(Object sender, EventArgs e) {
            #region Пробую создание конструктора
            //test_Circle = new List<Control>();
            //var lbl = new Label() {
            //    Text = "Вариант построения",
            //    TabStop = true,
            //    Location = new Point(MainFormCmbbxFigureChoose.Location.X +
            //    MainFormCmbbxFigureChoose.Width +
            //    MainFormCmbbxFigureChoose.Margin.Right,
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


        private void MainForm_SelectedFigureChanged(Object sender, EventArgs e) {
            currConstructorStage = 1;
            currSupportFigure = new MyPoint((Int32)numericUpDown1.Value, (Int32)numericUpDown2.Value, Color.Red);

            #region Попытка #1
            //foreach (Control cntcl in figureConstructor) {
            //    cntcl.Dispose();
            //}
            ////???Нужно его очищать?
            //figureConstructor.Clear();

            //currFigureConstructorLevel = 1;



            //switch (CurrFigure) {
            //    case Figure.Circle:
            //        //(1, 150, "Выбор построения", "Ограниченный областью", "Описывающая прямоугольник", "Три точки");

            //        //???А как насчёт динамического добавления полей? Тогда не придётся ебаться с координатными
            //        //константами. А придётся ебаться с уникальными ивентами... Или же они вычисляются
            //        //в redraw event?..
            //        //var lbl = new Label() {
            //        //    Name = "MainFormLbl1BuildingVariant",
            //        //    TabStop = true,
            //        //    Location = new Point(),
            //        //};
            //        //figureConstructor.Add(lbl);

            //        //var cmbbx = new ComboBox() {
            //        //    Name = "MainFormCmbbx1BuildingVariant",
            //        //    TabIndex = 1,
            //        //    Width = 150,
            //        //    DropDownStyle = ComboBoxStyle.DropDownList,
            //        //    X = ,
            //        //    Y = ,
            //        //};
            //        //string[] m = { "Ограниченный прямоугольником", "Описанный вокруг прямоугольника", "Три точки" };
            //        //cmbbx.Items.AddRange(m);
            //        //figureConstructor.Add(cmbbx);
            //        //(figureConstructor[figureConstructor.Count] as ComboBox).SelectedIndexChanged += MainForm_FigureConstructorCombobox_SelectedIndexChanged;

            //        //var 




            //        break;
            //    case Figure.Rectangle:
            //        break;
            //}
            #endregion
        }
        private void MainFormCmbbxFigureChoose_SelectionChangeCommitted(Object sender, EventArgs e) {
            switch ((sender as ComboBox).SelectedItem) {
                case "Круг":
                    CurrSelectedFigure = Figure.Circle;
                    break;
                case "Прямоугольник":
                    CurrSelectedFigure = Figure.Rectangle;
                    break;
                default: throw new Exception();
            }
        }
        //!!! MainForm#1: оформить разрозненные точечные контролы в динамические
        private void numericUpDown1_ValueChanged(Object sender, EventArgs e) {
            #region #20: попытка обойти событие 
            ChangeCurrentSupportFifureProperties();
            #endregion
        }
        private void numericUpDown2_ValueChanged(Object sender, EventArgs e) {
            #region #20: попытка обойти событие 
            ChangeCurrentSupportFifureProperties();
            #endregion
        }
        private void numericUpDown3_ValueChanged(Object sender, EventArgs e) {
            #region #20: попытка обойти событие 
            ChangeCurrentSupportFifureProperties();
            #endregion
        }
        private void numericUpDown4_ValueChanged(Object sender, EventArgs e) {
            #region #20: попытка обойти событие 
            ChangeCurrentSupportFifureProperties();
            #endregion
        }
        private void numericUpDown3_Enter(Object sender, EventArgs e) {
            if (currConstructorStage < 2) {
                currConstructorStage = 2;
            }
        }
        private void numericUpDown4_Enter(Object sender, EventArgs e) {
            if (currConstructorStage < 2) {
                currConstructorStage = 2;
            }
        }
        private void ChangeCurrentSupportFifureProperties() {
            switch (currConstructorStage) {
                //???Динамеическое изменение получится здесь подключить?
                //Если не просто создавать объект, а сразу рисовать?
                //??? Создание нового объекта *чрезвычайно* неэффективно,
                //но наши свойства закрытые, в то время как создание через 4 
                //координаты уже реализовано в конструкторе. Зачем поля закрыты?
                case 1:
                    currSupportFigure = new MyPoint((int)numericUpDown1.Value, (int)numericUpDown2.Value, Color.Red);
                    break;
                case 2:
                    currSupportFigure = new MyRectangle(
                        (int)numericUpDown1.Value,
                        (int)numericUpDown2.Value,
                        (int)numericUpDown3.Value,
                        (int)numericUpDown4.Value,
                        Color.Red
                      );
                    currSupportFigure.Pen.DashStyle = DashStyle.Dash;
                    currSupportFigure.Pen.Width = 1;
                    break;
                default: throw new Exception();
            }
        }




        private void MainFormTmr_Tick(Object sender, EventArgs e) {
            screen.Clear(Color.FromArgb(30, 30, 30));
            foreach (var figure in figuresToDrawList) {
                figure.Draw(screen);
            }
            currSupportFigure?.Draw(screen);
        }



        #region Пробую создание конструктора
        //private Point FindConstructorControlLocation(Int32 _constructorLevel) {
        //    Control currControl = MainFormCmbbxFigureChoose;
        //    for (Int32 i = figureConstructor.Count - 1; i > 0; i--) {
        //        if (figureConstructor[i].TabIndex > _constructorLevel - 2) {
        //            break;
        //        }

        //        if (currControl.Location.X + currControl.Width > figureConstructor[i].Location.X + figureConstructor[i].Width) {
        //            currControl = figureConstructor[i];
        //        }
        //    }

        //    return new Point(
        //        currControl.Location.X +
        //        currControl.Width +
        //        currControl.Margin.Right,
        //        MainFormLblFigureChoose.Location.Y);
        //}
        //private ComboBox CreateNewContstructorCombobox(Int32 _tabIndex, Int32 _width, String _labelText, params String[] _textVariants) {
        //    Point location = FindConstructorControlLocation(_tabIndex);

        //    var cmbbx = new ComboBox() {
        //        TabIndex = _tabIndex,
        //        Width = _width,
        //        Location = location,
        //    };
        //    cmbbx.Items.AddRange(_textVariants);
        //    return cmbbx;
        //}
        #endregion
        private void MainFromPctrbxScreen_Click(Object sender, EventArgs e) {
            #region Попытка #1
            //currClick++;
            //switch(currClick) {
            //    case 1:
            //        figuresToDrawList.Add(new MyPoint((Int32)numericUpDown1.Value, (Int32)numericUpDown2.Value, Color.Red));
            //        break;
            //    case 2:
            //        //FiguresToDrawList.Remove(new MyRectangle)
            //        break;
            //    default: throw new Exception();
            //}
            #endregion
        }

    }
}
