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
        //!!!MyFigure#01: рефакторинг конструкторов: изменить порядок параметров и убрать задание через "color".
        public static Int32 FiguresCount = 0;
        public Int32 Id = 0;
        protected Int32 x;
        public virtual Int32 X { set; get; }
        protected Int32 y;
        public virtual Int32 Y { set; get; }
        public Point Location {
            set {
                if (value.X != X || value.Y != Y) {
                    X = value.X;
                    Y = value.Y;
                }
            }
            get {
                return new Point(X, Y);
            }
        }
        public Pen Pen { set; get; } = new Pen(Color.Black, 1);
        public Pen SelectedPen { set; get; } = new Pen(Color.White, 2);
        public Boolean IsSelected;
        public Boolean IsFill;
        public Color FillColor;



        protected MyFigure(Pen _pen) {
            Pen = _pen;
            FiguresCount++;
            Id = FiguresCount;

        }
        protected MyFigure(Color _color) : this(new Pen(_color, 1)) {

        }



        //MyFigure#81: реализовать удаление фигуры и присваивание ID.
        public void Dispose() { }


        public virtual void Draw(Graphics _screen) {
            Pen _pen;
            if (IsSelected) {
                _pen = SelectedPen;
            }
            else {
                _pen = Pen;
            }
            DrawFigure(_screen, _pen);
        }
        protected abstract void DrawFigure(Graphics _screen, Pen _pen);


        public String GetDescription() {
            return FiguresEnum.GetDescription(this);
        }


        //???Может переместить их куда-то? Неочевидные методы.
        /// <summary>
        /// Возвращает левый верхний угол прямоугольника с горизональной гранью по координатам двух точек
        /// </summary>
        /// <param name="_p1">Первая точка</param>
        /// <param name="_p2">Вторая точка</param>
        public static PointF FindLeftUpCornerCoord(Single _x1, Single _y1, Single _x2, Single _y2) {
            Single lowX = _x1 > _x2 ? _x2 : _x1;
            Single lowY = _y1 > _y2 ? _y2 : _y1;
            return new PointF(lowX, lowY);
        }
        /// <summary>
        /// Обрубает координатный прямоугольник до квадрата относительно первой точки
        /// </summary>
        public static (Point, Point) CutCoordinatesRectangleToSquare(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2) {
            var p1 = new Point(_x1, _y1);
            var p2 = new Point(_x2, _y2);

            Int32 a = Math.Abs(p2.X - p1.X);
            Int32 b = Math.Abs(p2.Y - p1.Y);
            if (Math.Abs(a) > Math.Abs(b)) {
                p2.X = _x1 > _x2 ? p1.X - b : p1.X + b;
            }
            else {
                p2.Y = _y1 > _y2 ? p1.Y - a : p1.Y + a;
            }
            return (p1, p2);
        }
        /// <summary>
        /// Находил целочисленную длину между двумя точками
        /// </summary>
        public static double FindLength(Point _p1, Point _p2) {
            return Math.Sqrt(Math.Pow(_p1.X - _p2.X, 2) + Math.Pow(_p1.Y - _p2.Y, 2));
        }
    }
}
