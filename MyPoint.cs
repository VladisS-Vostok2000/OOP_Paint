using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CAD_Client {
    [Obsolete("Тяжело поддерживать много фигур", false)]
    internal class MyPoint : MyFigure {



        internal MyPoint(float x, float y, Color color) : base(color) {
            X = x;
            Y = y;
        }



        [Obsolete]
        private protected void Display(Graphics screen, Pen pen) {
            screen.DrawEllipse(pen, X, Y, 2, 2);
        }


        internal override void Move(PointF newLocation) {
            throw new NotImplementedException();
        }

        private protected override void FindLocation() {
            throw new NotImplementedException();
        }

        private protected override void Display(Graphics screen, Pen pen, PointF graphicsCenterInRealCoord) {
            throw new NotImplementedException();
        }
    }
}
