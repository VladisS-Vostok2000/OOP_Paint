using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Paint {
    public class MyPoint : MyFigure {
        private const Int32 vetrexesCount = 1;
        public MyPoint(Single _x, Single _y, Color _color) : base(_color, vetrexesCount) {
            X = _x;
            Y = _y;

        }



        protected override void DrawFigure(Graphics _screen, Pen _pen) {
            _screen.DrawEllipse(_pen, X, Y, 2, 2);
        }

    }
}
