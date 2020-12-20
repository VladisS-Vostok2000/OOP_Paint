using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAD_Client {
    internal class MyPoligon : MyFigure {
        internal IReadOnlyList<PointF> Vertexes { get; }
        protected readonly PointF[] VertexesArray;



        internal MyPoligon(int vertexesCount) {
            //???Повторяющийся код
            //???Не могу определять read-only поля в методе
            if (vertexesCount < 2) {
                throw new ArgumentException();
            }

            VertexesArray = new PointF[vertexesCount];
            Vertexes = Array.AsReadOnly(VertexesArray);
        }
        internal MyPoligon(int vertexesCount, Color color) : base(color) {
            //???Повторяющийся код
            //???Не могу определять read-only поля в методе
            if (vertexesCount < 2) {
                throw new ArgumentException();
            }

            VertexesArray = new PointF[vertexesCount];
            Vertexes = Array.AsReadOnly(VertexesArray);
        }
        internal MyPoligon(int vertexesCount, Pen pen) : base(pen) {
            //???Повторяющийся код
            //???Не могу определять read-only поля в методе
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
