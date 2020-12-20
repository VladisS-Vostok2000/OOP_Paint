using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAD_Client {
    internal class MyRectangle : MyPoligon {
        private const int vetrexesCount = 4;
        internal float Width { set; get; }
        internal float Height { set; get; }



        internal MyRectangle(in float x1, in float y1, in float x2, in float y2) : base(vertexesCount) {
            //???Повторяющийся код
            InitializeFigure(x1, y1, x2, y2);
        }
        internal MyRectangle(in float x1, in float y1, in float x2, in float y2, Pen pen) : base(vetrexesCount, pen) {
            //???Повторяющийся код
            InitializeFigure(x1, y1, x2, y2);
        }
        internal MyRectangle(in float x1, in float y1, in float x2, in float y2, Color color) : base(vetrexesCount, color) {
            //???Повторяющийся код
            InitializeFigure(x1, y1, x2, y2);
        }
        internal void InitializeFigure(in float x1, in float y1, in float x2, in float y2) {
            Location = MyGeometry.FindLeftUpCornerCoord(x1, y1, x2, y2);
            Width = Math.Abs(x1 - x2);
            Height = Math.Abs(y1 - y2);
        }


        internal void Resize(in float x1, in float y1, in float x2, in float y2) {
            InitializeFigure(x1, y1, x2, y2);
        }


        protected override void DrawFigure(Graphics screen, Pen pen) {
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
