using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_Paint {
    public abstract class Figure {
        public int X { protected set; get; }
        public int Y { protected set; get; }
        public int Width { protected set; get; }
        public int Height { protected set; get; }
        public Pen Pen { protected set; get; }


        public virtual void Draw(Graphics _screen) { }
    }
}
