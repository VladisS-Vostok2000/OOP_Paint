using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAD_Client {
    public class MyCircle : MyFigure {
        private const int vetrexesCount = 0;
        protected float radius;
        public float Radius {
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
        public MyCircle(in float x1, in float y1, in float x2, in float y2, Pen pen) : base(pen) {
            //???Повторяющийся код
            InitializeFigure(x1, y1, x2, y2);
        }
        /// <summary>
        /// Ограничивающий прямоугольник
        /// </summary>
        public MyCircle(in float x1, in float y1, in float x2, in float y2, Color color) : base(color) {
            //???Повторяющийся код
            InitializeFigure(x1, y1, x2, y2);
        }
        /// <summary>
        /// Центр, радиус
        /// </summary>
        public MyCircle(Pen pen, in Point center, in float radius) : base(pen) {
            InitializeFigure(center, radius);
        }
        /// <summary>
        /// Инициализирует вписанную в прямоугольник окружность в угол первой точки.
        /// </summary>
        public void InitializeFigure(in float x1, in float y1, in float x2, in float y2) {
            (PointF p1, PointF p2) = MyGeometry.CutCoordinatesRectangleToSquare(x1, y1, x2, y2);
            float radius = Math.Abs(p1.X - p2.X) / 2;
            PointF center = MyGeometry.FindLeftUpCornerCoord(p1.X, p1.Y, p2.X, p2.Y);
            InitializeFigure(center, radius);
        }
        protected void InitializeFigure(in PointF center, in float radius) {
            Radius = radius;
            Center = center;
        }


        public void Resize(in float x1, in float y1, in float x2, in float y2) {
            InitializeFigure(x1, y1, x2, y2);
        }
        public void Resize(in PointF center, in float radius) {
            InitializeFigure(center, radius );
        }


        protected override void DrawFigure(Graphics screen, Pen pen) {
            screen.DrawEllipse(pen, X, Y, Radius * 2, Radius * 2);
        }

    }
}
