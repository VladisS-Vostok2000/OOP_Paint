using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Paint {
    class MyCircle : Figure {
        public override void Draw(Point _firstClick, Point _secondClick, Pen _pen, Graphics _screen) {
            CutCoordinatesRectangleToSquare(_firstClick, _secondClick);
            int radius = FindRadius(_firstClick, _secondClick);
        }
        private int FindRadius(Point _p1, Point _p2) {
            var square = CutCoordinatesRectangleToSquare(Point _p1, Point _p2);
        }

        /// <summary>
        /// Обрубает координатный прямоугольник до квадрата
        /// </summary>
        /// <param name="_p1"></param>
        /// <param name="_p2"></param>
        private void CutCoordinatesRectangleToSquare(Point _p1, Point _p2) {
            int a = _p2.X - _p1.X;
            int b = _p2.Y - _p1.Y;
            if (Math.Abs(a) > Math.Abs(b)) {
                _p2.X = _p1.X + b;
            }
            else {
                _p2.Y = _p1.Y + a;
            }
        }
    }
}
