using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Paint {
    public class MyRectangle : MyFigure {
        private MyRectangle(int _x1, int _y1, int _x2, int _y2) {
            X = _x1;
            Y = _y1;
            Width = Math.Abs(_x1 - _x2);
            Height = Math.Abs(_y1 - _y2);
        }
        public MyRectangle(int _x1, int _y1, int _x2, int _y2, Pen _pen) : this(_x1, _y1, _x2, _y2) {
            Pen = _pen;
        }
        public MyRectangle(int _x1, int _y1, int _x2, int _y2, Color _color) : this(_x1, _y1, _x2, _y2) {
            Pen = new Pen(_color, 2);
        }

        public override void Draw(Graphics _screen) {
            _screen.DrawRectangle(Pen, X, Y, Width, Height);
        }
    }
}
