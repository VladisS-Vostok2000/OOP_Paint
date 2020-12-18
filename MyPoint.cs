using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CAD_Client {
    public class MyPoint : MyFigure {



        public MyPoint(float x, float y, Color color) : base(color) {
            X = x;
            Y = y;
        }



        protected override void DrawFigure(Graphics screen, Pen pen) {
            screen.DrawEllipse(pen, X, Y, 2, 2);
        }

    }
}
