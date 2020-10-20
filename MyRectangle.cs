using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Paint {
    //MyRectangle#44: добавить заливку
    public class MyRectangle : MyFigure {
        public Int32 Width { set; get; }
        public Int32 Height { set; get; }



        public MyRectangle(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2, Pen _pen) : base(_pen) {
            //???Повторяющийся код
            InitializeFigure(_x1, _y1, _x2, _y2);

        }
        public MyRectangle(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2, Color _color) : base(_color) {
            //???Повторяющийся код
            InitializeFigure(_x1, _y1, _x2, _y2);

        }
        public void InitializeFigure(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2) {
            Location = FindLeftUpCornerCoord(_x1, _y1, _x2, _y2);
            Width = Math.Abs(_x1 - _x2);
            Height = Math.Abs(_y1 - _y2);

        }


        public void Resize(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2) {
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
