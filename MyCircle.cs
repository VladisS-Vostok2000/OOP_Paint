using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Paint {
    public class MyCircle : MyFigure {
        public Int32 Radius { protected set; get; }
        public Point Center { protected set; get; }



        //#MyFigure#18: исправить конструкторы в связи с парадигмой
        public MyCircle(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2, Pen _pen) : base(_pen) {
            //???Не очень как-то смотрится, но я не могу объявить новый this из-за ручки, которую
            //нет смысла тут писать ещё раз. Либо ещё раз переопределить X и Y можно, но...?
            //Это то же самое дублирование кода, если что-то изменится. Дублирование конструктора
            //методом куда безопаснее смотрится.
            InitializeFigure(_x1, _y1, _x2, _y2);

        }
        public MyCircle(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2, Color _color) : base(_color) {
            //???Не очень как-то смотрится, но я не могу объявить новый this из-за ручки, которую
            //нет смысла тут писать ещё раз. Либо ещё раз переопределить X и Y можно, но...?
            //Это то же самое дублирование кода, если что-то изменится. Дублирование конструктора
            //методом куда безопаснее смотрится.
            InitializeFigure(_x1, _y1, _x2, _y2);

        }
        public void InitializeFigure(int _x1, int _y1, int _x2, int _y2) {
            (Point p1, Point p2) = CutCoordinatesRectangleToSquare(_x1, _y1, _x2, _y2);
            Radius = Math.Abs(_x1 - _x2) / 2;
            Center = new Point(
                (X + Math.Abs(_x1 - _x2)) / 2,
                (Y + Math.Abs(_y1 - _y2)) / 2
            );

        }



        /// <summary>
        /// Обрубает координатный прямоугольник до квадрата относительно первой точки
        /// </summary>
        protected (Point, Point) CutCoordinatesRectangleToSquare(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2) {
            var p1 = new Point(_x1, _y1);
            var p2 = new Point(_x2, _y2);

            Int32 a = Math.Abs(p2.X - p1.X);
            Int32 b = Math.Abs(p2.Y - p1.Y);
            if (Math.Abs(a) > Math.Abs(b)) {
                p2.X = _x1 > _x2 ? p1.X - b : p1.X + b;
            }
            else {
                p2.Y = _y1 > _y2 ? p1.Y - a : p1.Y + a;
            }
            return (p1, p2);
        }


        public override void Draw(Graphics _screen) {
            _screen.DrawEllipse(Pen, X, Y, X + Radius * 2, Y + Radius * 2);

        }

    }
}
