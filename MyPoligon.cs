using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Paint {
    public class MyPoligon : MyFigure {
        public IReadOnlyList<PointF> Vertexes { get; }
        protected readonly PointF[] VertexesArray;



        protected MyPoligon(int vertexesCount) {
            //???Повторяющийся код
            if (vertexesCount < 2) {
                throw new ArgumentException();
            }

            VertexesArray = new PointF[vertexesCount];
            Vertexes = Array.AsReadOnly(VertexesArray);
        }
        protected MyPoligon(int vertexesCount, Color color) : base(color) {
            //???Повторяющийся код
            if (vertexesCount < 2) {
                throw new ArgumentException();
            }

            VertexesArray = new PointF[vertexesCount];
            Vertexes = Array.AsReadOnly(VertexesArray);
        }
        protected MyPoligon(int vertexesCount, Pen pen) : base(pen) {
            //???Повторяющийся код
            if (vertexesCount < 2) {
                throw new ArgumentException();
            }

            VertexesArray = new PointF[vertexesCount];
            Vertexes = Array.AsReadOnly(VertexesArray);
        }



        protected override void DrawFigure(Graphics screen, Pen pen) {
            throw new NotImplementedException();
        }

    }
}
