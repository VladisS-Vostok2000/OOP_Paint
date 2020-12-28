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
        internal MyPoligon(int vertexesCount, Pen pen) : base(pen) {
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



        private protected override void FindLocation() {
            throw new NotImplementedException();
        }


        private protected override void Display(Graphics screen, Pen pen, PointF graphicsCenterInRealCoord) {
            screen.DrawLine(pen, ToGraphicsCoord(graphicsCenterInRealCoord, Vertexes[0]), ToGraphicsCoord(graphicsCenterInRealCoord, Vertexes[Vertexes.Count - 1]));
            for (int i = 1; i < Vertexes.Count; i++) {
                PointF p1 = ToGraphicsCoord(graphicsCenterInRealCoord, Vertexes[i]);
                PointF p2 = ToGraphicsCoord(graphicsCenterInRealCoord, Vertexes[i - 1]);
                screen.DrawLine(pen, p1, p2);
            }
        }


        internal override void Move(PointF newLocation) {
            for (int i = 0; i < VertexesArray.Length; i++) {
                //Смещение от старой локации прибавляется к новой
                VertexesArray[i] = newLocation.Sum(VertexesArray[i].Substract(Location));
            }
            Location = newLocation;
        }

    }
}
