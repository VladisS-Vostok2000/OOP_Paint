using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Paint {
    public class MyCircle : MyFigure {
        public Int32 Radius { protected set; get; }
        public Point Center { protected set; get; }
        public MyCircle(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2) : base() {
            (Point p1, Point p2) = CutCoordinatesRectangleToSquare(_x1, _y1, _x2, _y2);
            Width = Math.Abs(p1.X - p2.X);
            Height = Math.Abs(p1.Y - p2.Y);
            Radius = Width / 2;
            Point leftCorner = FindLeftUpCornerCoord(_x1, _y1, _x2, _y2);
            X = leftCorner.X;
            Y = leftCorner.Y;
        }
        public MyCircle(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2, Pen _pen) : this(_x1, _x2, _y1, _y2) {
            Pen = _pen;
        }
        public MyCircle(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2, Color _color) : this(_x1, _x2, _y1, _y2) {
            Pen = new Pen(_color, 2);
        }


        /// <summary>
        /// Обрубает координатный прямоугольник до квадрата относительно первой точки
        /// </summary>
        /// <param name="_p1"></param>
        /// <param name="_p2"></param>
        private (Point, Point) CutCoordinatesRectangleToSquare(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2) {
            var _p1 = new Point(_x1, _x2);
            var _p2 = new Point(_x2, _y2);

            Int32 a = _p2.X - _p1.X;
            Int32 b = _p2.Y - _p1.Y;
            if (Math.Abs(a) > Math.Abs(b)) {
                _p2.X = _p1.X + b;
            }
            else {
                _p2.Y = _p1.Y + a;
            }
            return (_p1, _p2);
        }


        public override void Draw(Graphics _screen) {
            _screen.DrawEllipse(Pen, X, Y, Width, Height);
        }



    }
}
