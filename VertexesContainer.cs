using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Paint {
    //VertexesContainer#10: реализовать контейнер вершин фигур
    class VertexesContainer {
        public readonly int VetrexesCount;
        private readonly PointF[] vertexes;



        public PointF this[int _i] => vertexes[_i];



        public VertexesContainer(int _vertexesCount) {
            vertexes = new PointF[_vertexesCount - 1];
            VetrexesCount = _vertexesCount;
        }

    }
}
