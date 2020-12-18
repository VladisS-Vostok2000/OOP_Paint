using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAD_Client {
    public class MyRay : MyFigure {
        public PointF P1 { private set; get; }
        public PointF P2 { private set; get; }



        /// <summary> Создаст экземпляр луча из первой точки в заданном второй точкой направлении. </summary>
        public MyRay(Pen pen) : base(pen) { }
        /// <summary> Создаст экземпляр луча из первой точки в заданном второй точкой направлении. </summary>
        public MyRay(in PointF p1, in PointF p2) {
            InitializeFigure(p1, p2);
        }
        /// <summary> Создаст экземпляр луча из первой точки в заданном второй точкой направлении. </summary>
        public MyRay(Pen pen, in PointF p1, in PointF p2) : base(pen) {
            MyGeometry.FindLeftUpCornerCoord(p1, p2);
        }
        public void InitializeFigure(in PointF p1, in PointF p2) {
            P1 = p1;
            P2 = new PointF((p2.X - p1.X) * 1000 + p1.X, (p2.Y - p1.Y) * 1000 + p1.Y);
            Location = MyGeometry.FindLeftUpCornerCoord(p1, p2);
        }



        protected override void DrawFigure(Graphics screen, Pen pen) {
            screen.DrawLine(pen, P1, P2);
        }

    }
}
