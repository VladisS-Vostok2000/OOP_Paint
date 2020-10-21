using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Paint {
    public class MyCircle : MyFigure {
        protected Single radius;
        public Single Radius {
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
        public PointF Center {
            set {
                X = Math.Abs(value.X - (X + Radius)) + X;
                Y = Math.Abs(value.Y - (Y + Radius)) + Y;
            }
            get {
                return new PointF(X + Radius, Y + Radius);
            }
        }


        /// <summary>
        /// Ограничивающий прямоугольник
        /// </summary>
        public MyCircle(Single _x1, Single _y1, Single _x2, Single _y2, Pen _pen) : base(_pen) {
            //???Повторяющийся код
            InitializeFigure(_x1, _y1, _x2, _y2);
        }
        /// <summary>
        /// Ограничивающий прямоугольник
        /// </summary>
        public MyCircle(Single _x1, Single _y1, Single _x2, Single _y2, Color _color) : base(_color) {
            //???Повторяющийся код
            InitializeFigure(_x1, _y1, _x2, _y2);
        }
        /// <summary>
        /// Центр, радиус
        /// </summary>
        public MyCircle(Pen _pen, Point _center, Single _radius) : base(_pen) {
            InitializeFigure(_center, _radius);
        }
        /// <summary>
        /// Инициализирует вписанную в прямоугольник окружность в угол первой точки
        /// </summary>
        public void InitializeFigure(Single _x1, Single _y1, Single _x2, Single _y2) {
            (PointF p1, PointF p2) = CutCoordinatesRectangleToSquare(_x1, _y1, _x2, _y2);
            {
                Radius = Math.Abs(p1.X - p2.X) / 2;
                Location = FindLeftUpCornerCoord(p1.X, p1.Y, p2.X, p2.Y);
            }
        }
        protected void InitializeFigure(PointF _center, Single _radius) {
            Radius = _radius;
            Center = _center;
        }


        public void Resize(Single _x1, Single _y1, Single _x2, Single _y2) {
            //???Повторяющийся код
            InitializeFigure(_x1, _y1, _x2, _y2);
        }
        public void Resize(PointF _center, Single _radius) {
            //???Повторяющийся код
            InitializeFigure(
                _center,
                _radius
            );
        }


        protected override void DrawFigure(Graphics _screen, Pen _pen) {
            _screen.DrawEllipse(_pen, X, Y, Radius * 2, Radius * 2);
        }

    }
}
