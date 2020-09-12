using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_Paint {
    public abstract class Figure {
        public int X { private set; get; }
        public int Y { private set; get; }
        public Color Color { private set; get; }

        public virtual void Draw(Point _upLeft, Point _downRight, Pen _pen, Graphics _screen) { }
    }
}
