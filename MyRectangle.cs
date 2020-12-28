using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAD_Client {
    internal class MyRectangle : MyPoligon {
        private const int vertexesCount = 4;
        internal float Width { private protected set; get; }
        internal float Height { private protected set; get; }



        internal MyRectangle(in float x1, in float y1, in float x2, in float y2) : base(vertexesCount) {
            //???Повторяющийся код
            InitializeFigure(x1, y1, x2, y2);
        }
        internal MyRectangle(in float x1, in float y1, in float x2, in float y2, Pen pen) : base(vertexesCount, pen) {
            //???Повторяющийся код
            InitializeFigure(x1, y1, x2, y2);
        }
        internal MyRectangle(in float x1, in float y1, in float x2, in float y2, Color color) : base(vertexesCount, color) {
            //???Повторяющийся код
            InitializeFigure(x1, y1, x2, y2);
        }
        internal void InitializeFigure(in float x1, in float y1, in float x2, in float y2) {
            Location = MyGeometry.FindLeftUpCornerCoord(x1, y1, x2, y2);
            Width = Math.Abs(x1 - x2);
            Height = Math.Abs(y1 - y2);
            VertexesArray[0] = new PointF(x1, y1);
            VertexesArray[1] = new PointF(x2, y1);
            VertexesArray[2] = new PointF(x2, y2);
            VertexesArray[3] = new PointF(x1, y2);
        }



        [Obsolete]
        internal void Resize(in float x1, in float y1, in float x2, in float y2) => InitializeFigure(x1, y1, x2, y2);

        [Obsolete]
        private protected void Display(Graphics screen, Pen pen) {
            if (Width != 0 && Height != 0) {
                if (IsFill) {
                    screen.FillRectangle(new SolidBrush(FillColor), X, Y, Width, Height);
                }
                screen.DrawRectangle(pen, X, Y, Width, Height);
            }
            else
            if (Width != Height) {
                screen.DrawLine(pen, X, Y, X + Width, Y + Height);
            }
        }

    }
}
