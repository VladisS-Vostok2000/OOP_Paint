using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CAD_Client {
    internal class MyRay : MyFigure {
        private PointF vector;



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
            this.vector = vector;
        }



        internal override void Move(PointF newLocation) {
            vector = newLocation.Sum(vector.Substract(Location));
            Location = newLocation;
        }
        
        
        private protected override void Display(Graphics screen, Pen pen) {
            screen.DrawLine(pen, Location, vector.Multiply(1000));
        }

    }
}
