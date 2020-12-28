using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAD_Client {
    /// <summary>
    /// Замкнутая ломаная.
    /// </summary>
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



        internal override void Move(PointF newLocation) {
            for (int i = 0; i < VertexesArray.Length; i++) {
                VertexesArray[i] = newLocation.Sum(VertexesArray[i].Substract(Location));
            }
            Location = newLocation;
        }


        private protected override void FindLocation() {
            throw new NotImplementedException();
        }

        private protected override void Display(Graphics screen, Pen pen, PointF graphicsCenterInRealCoord) {
            throw new NotImplementedException();
        }
    }
}
