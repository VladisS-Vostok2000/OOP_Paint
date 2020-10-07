using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Paint {
    public class MyCircle : MyFigure {
        public Int32 Radius { set; get; }
        public Point Center { set; get; }



        public MyCircle(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2, Pen _pen) : base(_pen) {
            //???Повторяющийся код
            InitializeFigure(_x1, _y1, _x2, _y2);

        }
        public MyCircle(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2, Color _color) : base(_color) {
            //???Повторяющийся код
            InitializeFigure(_x1, _y1, _x2, _y2);

        }
        //MyCircle#98: некорректная инициализация
        public void InitializeFigure(int _x1, int _y1, int _x2, int _y2) {
            (Point p1, Point p2) = CutCoordinatesRectangleToSquare(_x1, _y1, _x2, _y2);
            Location = FindLeftUpCornerCoord(_x1, _y1, _x2, _y2);
            Radius = Math.Abs(p1.X - p2.X) / 2;
            Center = new Point(
                (X + Math.Abs(p1.X - p2.X)) / 2,
                (Y + Math.Abs(p1.Y - p2.Y) / 2)
            );

        }



        /// <summary>
        /// Обрубает координатный прямоугольник до квадрата относительно первой точки
        /// </summary>
        protected (Point, Point) CutCoordinatesRectangleToSquare(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2) {
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
        /// Возвращает левый верхний угол прямоугольника по координатам двух точек
        /// </summary>
        /// <param name="_p1">Первая точка</param>
        /// <param name="_p2">Вторая точка</param>
        protected Point FindLeftUpCornerCoord(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2) {
            Int32 lowX = _x1 > _x2 ? _x2 : _x1;
            Int32 lowY = _y1 > _y2 ? _y2 : _y1;
            return new Point(lowX, lowY);
        }

        public void Resize(int _x1, int _y1) {
            InitializeFigure(X, Y, _x1, _y1);

        }


        public override void Draw(Graphics _screen) {
            _screen.DrawEllipse(Pen, X, Y, X + Radius * 2, Y + Radius * 2);

        }

    }
}
