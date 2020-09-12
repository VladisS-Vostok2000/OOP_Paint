using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Paint {
    class MyCircle : Figure {
        public override void Draw(Point _upLeft, Point _downRight, Pen _pen, Graphics _screen) {
            int radius = FindRadius(_upLeft, _downRight);
        }
        private int FindRadius(Point _upLeft, Point _downRight) {
            var square = CutCoordinatesRectangleToSquare(Point _upLeft, Point _downRight);
        }
        //???Метод принимает верхняя точка всегда слева, а рисование возможно и снизу
        //Либо менять местами, либо как-то конвертировать
        private (Point, Point) CutCoordinatesRectangleToSquare(Point _upLeft, Point _downRight) {
            switch(Math.Sign(_downRight.X - _upLeft.X - (_downRight.Y - _upLeft.Y)) {

            }
        }
    }
}
