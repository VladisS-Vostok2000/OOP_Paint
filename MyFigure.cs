using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static OOP_Paint.FiguresEnum;
namespace OOP_Paint {
    public abstract class MyFigure : IDisposable {
        public Int32 X { set; get; }
        public Int32 Y { set; get; }
        //MyFigure#36: создание поля Location
        public Point Location {
            set {
                if (value.X != X && value.Y != Y) {
                    X = value.X;
                    Y = value.Y;

                }
            } 
            get {
                return new Point(X, Y);
            }
        }
        public Pen Pen { set; get; } = new Pen(Color.Black, 2);



        //!!!MyFigure#75: перестройка конструкторов
        //Выстройка X и Y неочевидна: она часто зависит от конкретной фиугуры. Здесь она незачем.
        //Аналогичные инструкции, определённые в конструкторе, поэтому повторяющийся код должен быть
        //уничтожен именно отсюда.
        protected MyFigure(Pen _pen) {
            Pen = _pen;

        }
        protected MyFigure(Color _color) : this(new Pen(_color, 1)) {

        }
        //protected MyFigure() {
        //}
        ///// <summary>
        ///// Устанавливает верхний левый угол прямоугольника, заданного двумя точками.
        ///// </summary>
        //protected MyFigure(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2) {
        //    Point leftCorner = FindLeftUpCornerCoord(_x1, _y1, _x2, _y2);
        //    X = leftCorner.X;
        //    Y = leftCorner.Y;

        //}
        ///// <summary>
        ///// Устанавливает верхний левый угол прямоугольника, заданного двумя точками, и цвет фигуры.
        ///// </summary>
        //protected MyFigure(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2, Color _color) : this(_x1, _y1, _x2, _y2) {
        //    //???Повторяющийся код, ожидалось this(_color), однако оператор this уже занят.
        //    Pen = new Pen(_color, 1);

        //}
        ///// <summary>
        ///// Устанавливает верхний левый угол прямоугольника, заданного двумя точками, ширину и цвет фигуры.
        ///// </summary>
        //protected MyFigure(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2, Pen _pen) : this(_x1, _y1, _x2, _y2) {
        //    //???Повторяющийся код, ожидалось this(_pen), однако оператор this уже занят.
        //    Pen = _pen;

        //}



        public void Dispose() { }


        public abstract void Draw(Graphics _screen);

    }
}
