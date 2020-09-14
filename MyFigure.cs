using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_Paint {
    public abstract class MyFigure
        {
        public int X { protected set; get; }
        public int Y { protected set; get; }
        public int Width { protected set; get; }
        public int Height { protected set; get; }
        public Pen Pen { protected set; get; }

        public void Move(int _newX, int _newY) {
            X = _newX;
            Y = _newY;
        }
        public void Resize(int _newWidth, int _newHeight) {
            Width = _newWidth;
            Height = _newHeight;
        }


        public virtual void Draw(Graphics _screen) { }

    }
}
