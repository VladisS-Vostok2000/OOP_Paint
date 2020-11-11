using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Paint {
    public class MyRectangle : MyFigure {
        private const int vetrexesCount = 4;
        public Single Width { set; get; }
        public Single Height { set; get; }



        public MyRectangle(Single _x1, Single _y1, Single _x2, Single _y2, Pen _pen) : base(_pen, vetrexesCount) {
            //???Повторяющийся код
            InitializeFigure(_x1, _y1, _x2, _y2);

        }
        public MyRectangle(Single _x1, Single _y1, Single _x2, Single _y2, Color _color) : base(_color, vetrexesCount) {
            //???Повторяющийся код
            InitializeFigure(_x1, _y1, _x2, _y2);

        }
        public void InitializeFigure(Single _x1, Single _y1, Single _x2, Single _y2) {
            Location = FindLeftUpCornerCoord(_x1, _y1, _x2, _y2);
            Width = Math.Abs(_x1 - _x2);
            Height = Math.Abs(_y1 - _y2);

        }


        public void Resize(Single _x1, Single _y1, Single _x2, Single _y2) {
            InitializeFigure(_x1, _y1, _x2, _y2);

        }


        protected override void DrawFigure(Graphics _screen, Pen _pen) {
            if (Width != 0 && Height != 0) {
                if (IsFill) {
                    _screen.FillRectangle(new SolidBrush(FillColor), X, Y, Width, Height);
                }
                _screen.DrawRectangle(_pen, X, Y, Width, Height);
            }
            else
            if (Width != Height) {
                _screen.DrawLine(_pen, X, Y, X + Width, Y + Height);
            }
        }

    }
}
