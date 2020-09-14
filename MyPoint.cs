using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Paint {
    public class MyPoint : MyFigure {
        public MyPoint(Int32 _x, Int32 _y) : base(_x, _y, _x, _y) { }
        public MyPoint(Int32 _x, Int32 _y, Color _color) : base(_x, _y, _x, _y, _color) { }
        public MyPoint(Int32 _x, Int32 _y, Pen _pen) : base(_x, _y, _x, _y, _pen) { }


        public override void Draw(Graphics _screen) {
            _screen.DrawLine(Pen, X, Y, X, Y);
        }
    }
}
