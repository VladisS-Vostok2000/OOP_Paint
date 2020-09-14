using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Paint {
    public class MyPoint : MyFigure {
        public MyPoint (int _x, int _y, Pen _pen) {
            X = _x;
            Y = _y;
            Pen = _pen;
            Width = 0;
            Height = 0;
        }
        public MyPoint(int _x, int _y, Color _color) {
            X = _x;
            Y = _y;
            Pen = new Pen(_color, 2);
            Width = 0;
            Height = 0;
        }

        public override void Draw(Graphics _screen) {
            _screen.DrawLine(Pen, X, Y, X, Y);
        }
    }
}
