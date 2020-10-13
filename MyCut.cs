using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static OOP_Paint.FiguresEnum;

namespace OOP_Paint {
    //MyCut#88: реализовать отрезок
    class MyCut : MyFigure{
        protected Point P1 { set; get; }
        protected Point P2 { set; get; }


        public MyCut(Color _color, Point _p1, Point _p2) : base(_color) {
            //???Повторяющийся код
            InitializeFigure(_p1, _p2);
        }

        public MyCut(Pen _pen, Point _p1, Point _p2) : base (_pen) {
            //???Повторяющийся код
            InitializeFigure(_p1, _p2);
        }
        private void InitializeFigure(Point _p1, Point _p2) {
            Location = FindLeftUpCornerCoord(_p1.X, _p1.Y, _p2.X, _p2.Y);
            P1 = _p1;
            P2 = _p2;
        }


        public override void Draw(Graphics _screen) {
            _screen.DrawLine(Pen, P1, P2);
        }

    }
}
