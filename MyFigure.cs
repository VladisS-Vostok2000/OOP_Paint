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
        //MyFigure#80: создание id
        public static Int32 FiguresCount = 0;
        public Int32 Id = 0;
        public Int32 X { set; get; }
        public Int32 Y { set; get; }
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
        public Pen Pen { set; get; } = new Pen(Color.Black, 2);



        protected MyFigure(Pen _pen) {
            Pen = _pen;
            FiguresCount++;
            Id = FiguresCount;

        }
        protected MyFigure(Color _color) : this(new Pen(_color, 1)) {

        }



        //MyFigure#81: реализовать удаление фигуры и присваивание ID.
        public void Dispose() { }


        public abstract void Draw(Graphics _screen);


        public String GetDescription() {
            return FiguresEnum.GetDescription(this);
        }


        //???Может переместить их куда-то? Неочевидные методы.
        /// <summary>
        /// Возвращает левый верхний угол прямоугольника по координатам двух точек
        /// </summary>
        /// <param name="_p1">Первая точка</param>
        /// <param name="_p2">Вторая точка</param>
        protected static Point FindLeftUpCornerCoord(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2) {
            Int32 lowX = _x1 > _x2 ? _x2 : _x1;
            Int32 lowY = _y1 > _y2 ? _y2 : _y1;
            return new Point(lowX, lowY);
        }
        /// <summary>
        /// Обрубает координатный прямоугольник до квадрата относительно первой точки
        /// </summary>
        protected static (Point, Point) CutCoordinatesRectangleToSquare(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2) {
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




    }
}
