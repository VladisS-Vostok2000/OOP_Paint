using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_Paint {
    public abstract class MyFigure {
        public String Name { get; }
        public Int32 X { set; get; }
        public Int32 Y { set; get; }
        public Int32 Width { set; get; }
        public Int32 Height { set; get; }
        public Pen Pen { set; get; } = new Pen(Color.Black, 2);



        protected MyFigure() {

        }
        protected MyFigure(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2) {
            Point leftCorner = FindLeftUpCornerCoord(_x1, _y1, _x2, _y2);
            X = leftCorner.X;
            Y = leftCorner.Y;
            Width = Math.Abs(_x1 - _x2);
            Height = Math.Abs(_y1 - _y2);
        }
        protected MyFigure(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2, Color _color) : this(_x1, _y1, _x2, _y2) {
            Pen = new Pen(_color, 2);
        }
        protected MyFigure(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2, Pen _pen) : this(_x1, _y1, _x2, _y2) {
            Pen = _pen;
        }



        /// <summary>
        /// Находит левый верхний угол прямоугольника по координатам двух точек
        /// </summary>
        /// <param name="_p1">Первая точка</param>
        /// <param name="_p2">Вторая точка</param>
        /// <returns></returns>
        protected Point FindLeftUpCornerCoord(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2) {
            Int32 lowX = _x1 > _x2 ? _x2 : _x1;
            Int32 lowY = _y1 > _y2 ? _y2 : _y1;
            return new Point(lowX, lowY);
        }



        //MyFigure#4: лишние методы: уничтожить, открыть свойства. 



        public virtual void Draw(Graphics _screen) { }

    }
}
