using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Paint {
    public class MyPoint : MyFigure {
        public Color Color { private set; get; }
        

        protected MyPoint(Int32 _x, Int32 _y) {
            Name = "Точка";
            X = _x;
            Y = _y;
            Width = 0;
            Height = 0;
        }
        public MyPoint(Int32 _x, Int32 _y, Color _color) : this(_x, _y) {
            Color = _color;
        }


        public override void Draw(Graphics _screen) {
            _screen.DrawEllipse(new Pen(Color, 2), X, Y, 2, 2);
        }

    }
}
