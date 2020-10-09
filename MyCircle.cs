using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Paint {
    public class MyCircle : MyFigure {
        public Int32 Radius { set; get; }
        public Point Center { set; get; }



        public MyCircle(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2, Pen _pen) : base(_pen) {
            //???Повторяющийся код
            InitializeFigure(_x1, _y1, _x2, _y2);

        }
        public MyCircle(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2, Color _color) : base(_color) {
            //???Повторяющийся код
            InitializeFigure(_x1, _y1, _x2, _y2);

        }
        //MyCircle#98: некорректная инициализация
        public void InitializeFigure(int _x1, int _y1, int _x2, int _y2) {
            (Point p1, Point p2) = CutCoordinatesRectangleToSquare(_x1, _y1, _x2, _y2);
            {
                Location = FindLeftUpCornerCoord(p1.X, p1.Y, p2.X, p2.Y);
                Radius = Math.Abs(p1.X - p2.X) / 2;
            }
            Center = new Point(
                X + Radius,
                Y + Radius
            );
            int a = 1;
        }


        public void Resize(int _x1, int _y1, int _x2, int _y2) {
            InitializeFigure(_x1, _y1, _x2, _y2);

        }


        public override void Draw(Graphics _screen) {
            _screen.DrawEllipse(Pen, X, Y, Radius * 2, Radius * 2);

        }

    }
}
