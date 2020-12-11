using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Paint {
    public class MyRay : MyFigure {
        private const int vertexesCount = 2;

        public PointF P1 { private set; get; }
        public PointF P2 { private set; get; }



        /// <summary> Создаст экземпляр луча из первой точки в заданном второй точкой направлении. </summary>
        public MyRay(Pen pen) : base(pen, vertexesCount) { }
        /// <summary> Создаст экземпляр луча из первой точки в заданном второй точкой направлении. </summary>
        public MyRay(in PointF p1, in PointF p2) : base(vertexesCount) {
            InitializeFigure(p1, p2);
        }
        /// <summary> Создаст экземпляр луча из первой точки в заданном второй точкой направлении. </summary>
        public MyRay(Pen pen, in PointF p1, in PointF p2) : base(pen, vertexesCount) {
            MyGeometry.FindLeftUpCornerCoord(p1, p2);
        }
        public void InitializeFigure(in PointF p1, in PointF p2) {
            P1 = p1;
            P2 = new PointF((p2.X - p1.X) * 1000 + p1.X, (p2.Y - p1.Y) * 1000 + p1.Y);
            Location = MyGeometry.FindLeftUpCornerCoord(p1, p2);
            VertexesArray[0] = P1;
            VertexesArray[1] = P2;
        }



        protected override void DrawFigure(Graphics screen, Pen pen) {
            screen.DrawLine(pen, P1, P2);
        }

    }
}
