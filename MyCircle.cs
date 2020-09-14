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
        private MyCircle()
        public MyCircle(Point _firstClick, Point _secondClick, Pen _pen) {
            (Point p1, Point p2) = CutCoordinatesRectangleToSquare(_firstClick, _secondClick);
            {
                Width = Math.Abs(p1.X - p2.X);
                Height = Math.Abs(p1.Y - p2.Y);
                Pen = _pen;
                Radius = Width / 2;
                Center = FindLeftUpCornerCoord(p1, p2);

                Point leftCorn = FindLeftUpCornerCoord(p1, p2);
                {
                    X = leftCorn.X;
                    Y = leftCorn.Y;
                }
            }
        }
        public MyCircle(Point _firstClick, Point _secondClick, Color _color, Single _lineWidth) {
            (Point p1, Point p2) = CutCoordinatesRectangleToSquare(_firstClick, _secondClick);
            {
                Width = Math.Abs(p1.X - p2.X);
                Height = Math.Abs(p1.Y - p2.Y);
                Pen = new Pen(_color, _lineWidth);
                Radius = Width / 2;
                Center = FindLeftUpCornerCoord(p1, p2);

                Point leftCorner = FindLeftUpCornerCoord(p1, p2);
                {
                    X = leftCorner.X;
                    Y = leftCorner.Y;
                }
            }
        }


        public override void Draw(Graphics _screen) {
            _screen.DrawEllipse(Pen, X, Y, Width, Height);
        }


        /// <summary>
        /// Обрубает координатный прямоугольник до квадрата относительно первой точки
        /// </summary>
        /// <param name="_p1"></param>
        /// <param name="_p2"></param>
        private (Point, Point) CutCoordinatesRectangleToSquare(Point _p1, Point _p2) {
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
        private Point FindLeftUpCornerCoord(Point _p1, Point _p2) {
            Int32 lowX = _p1.X > _p2.X ? _p2.X : _p1.Y;
            Int32 lowY = _p1.Y > _p2.Y ? _p2.Y : _p1.Y;
            return new Point(lowX, lowY);
        }
    }
}
