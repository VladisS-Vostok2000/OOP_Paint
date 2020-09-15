using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Paint {
    public class MyRectangle : MyFigure {
        public MyRectangle(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2) : base(_x1, _y1, _x2, _y2) { }
        public MyRectangle(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2, Pen _pen) : base(_x1, _y1, _x2, _y2, _pen) { }
        public MyRectangle(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2, Color _color) : base(_x1, _y1, _x2, _y2, _color) { }


        public override void Draw(Graphics _screen) {
            //!!!#MyRectangle #2: при ширине/высоте в 0 прорисовывается ничего
            _screen.DrawRectangle(Pen, X, Y, Width, Height);
        }
    }
}
