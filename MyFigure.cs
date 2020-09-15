using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_Paint {
    public abstract class MyFigure {
        public Int32 X { protected set; get; }
        public Int32 Y { protected set; get; }
        public Int32 Width { protected set; get; }
        public Int32 Height { protected set; get; }
        public Pen Pen { protected set; get; } = new Pen(Color.Black, 2);

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


        /// <summary>
        /// Переопределяет координаты верхнего левого угла
        /// </summary>
        /// <param name="_newX"></param>
        /// <param name="_newY"></param>
        public void Move(Int32 _newX, Int32 _newY) {
            X = _newX;
            Y = _newY;
        }
        /// <summary>
        /// Переопределяет размер ограничивающего прямоугольника
        /// </summary>
        /// <param name="_newWidth"></param>
        /// <param name="_newHeight"></param>
        public void Resize(Int32 _newWidth, Int32 _newHeight) {
            Width = _newWidth;
            Height = _newHeight;
        }

        public virtual void Draw(Graphics _screen) { }

    }
}
