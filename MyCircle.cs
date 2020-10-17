using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Paint {
    public class MyCircle : MyFigure {
        protected Int32 radius;
        public Int32 Radius {
            set {
                if (value != radius) {
                    X += Radius - value;
                    Y += Radius - value;
                    radius = value;
                }
            }
            get {
                return radius;
            }
        }
        public Point Center {
            set {
                X = Math.Abs(value.X - (X + Radius)) + X;
                Y = Math.Abs(value.Y - (Y + Radius)) + Y;
            }
            get {
                return new Point(X + Radius, Y + Radius);
            }
        }


        /// <summary>
        /// Ограничивающий прямоугольник
        /// </summary>
        public MyCircle(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2, Pen _pen) : base(_pen) {
            //???Повторяющийся код
            InitializeFigure(_x1, _y1, _x2, _y2);
        }
        /// <summary>
        /// Ограничивающий прямоугольник
        /// </summary>
        public MyCircle(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2, Color _color) : base(_color) {
            //???Повторяющийся код
            InitializeFigure(_x1, _y1, _x2, _y2);
        }
        /// <summary>
        /// Центр, радиус
        /// </summary>
        public MyCircle(Pen _pen, Point _center, Int32 _radius) : base(_pen) {
            InitializeFigure(_center, _radius);
        }
        /// <summary>
        /// Инициализирует вписанную в прямоугольник окружность в угол первой точки
        /// </summary>
        public void InitializeFigure(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2) {
            (Point p1, Point p2) = CutCoordinatesRectangleToSquare(_x1, _y1, _x2, _y2);
            {
                Radius = Math.Abs(p1.X - p2.X) / 2;
                Location = FindLeftUpCornerCoord(p1.X, p1.Y, p2.X, p2.Y);
            }
        }
        protected void InitializeFigure(Point _center, Int32 _radius) {
            Radius = _radius;
            Center = _center;
        }


        public void Resize(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2) {
            //???Повторяющийся код
            InitializeFigure(_x1, _y1, _x2, _y2);
        }
        public void Resize(Point _center, Int32 _radius) {
            //???Повторяющийся код
            InitializeFigure(
                _center,
                _radius
            );
        }


        public override void Draw(Graphics _screen) {
            _screen.DrawEllipse(Pen, X, Y, Radius * 2, Radius * 2);

        }

    }
}
