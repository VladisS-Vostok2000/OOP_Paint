using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CAD_Client {
    internal class MyRay : MyFigure {
        internal PointF Vector { set; get; }
        /// <summary>
        /// Вернёт true, если начало и точка направления совпадают.
        /// </summary>
        internal bool IsPoint => Vector == Location;



        /// <summary>
        /// Создаст экземпляр луча из первой точки в заданном второй точкой направлении.
        /// </summary>
        /// <param name="start">Начало луча.</param>
        /// <param name="vector">Точка, задающая направление луча.</param>
        internal MyRay(in PointF start, in PointF vector) {
            InitializeFigure(start, vector);
        }
        /// <summary>
        /// Создаст экземпляр луча из первой точки в заданном второй точкой направлении.
        /// </summary>
        internal MyRay(Pen pen, in PointF start, in PointF vector) : base(pen) {
            InitializeFigure(start, vector);
        }
        private void InitializeFigure(PointF start, PointF vector) {
            if (start == vector) {
                throw new ArgumentException("Точки совпадают.");
            }

            Location = start;
            Vector = vector;
        }



        private protected override void FindLocation() { }


        private protected override void Display(Graphics screen, Pen pen, PointF graphicsCenterInRealCoord) {
            if (!IsPoint) {
                screen.DrawLine(pen, Location.Substract(graphicsCenterInRealCoord), Vector.Sum(Vector.Substract(Location).Multiply(1000)).Substract(graphicsCenterInRealCoord));
            }
        }


        internal override void Move(PointF newLocation) {
            Vector = newLocation.Sum(Vector.Substract(Location));
            Location = newLocation;
        }

    }
}
