using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static OOP_Paint.FiguresEnum;

namespace OOP_Paint {
    class MyCut : MyFigure{
        private Point p1;
        public Point P1 { 
            set {
                if (value != p1) {
                    p1 = value;
                    Location = FindLeftUpCornerCoord(value.X, value.Y, P2.X, P2.Y);
                    Length = FindLength(value, P2);
                }
            } 
            get {
                return p1;
            }
        }
        private Point p2;
        public Point P2 {
            set {
                if (value != p2) {
                    p2 = value;
                    Location = FindLeftUpCornerCoord(value.X, value.Y, P2.X, P2.Y);
                    FindLength(value, P2);
                }
            }
            get {
                return p2;
            }
        }
        public Int32 Length { private set; get; }



        public MyCut(Color _color, Point _p1, Point _p2) : base(_color) {
            //???Повторяющийся код
            InitializeFigure(_p1, _p2);
        }
        public MyCut(Pen _pen, Point _p1, Point _p2) : base (_pen) {
            //???Повторяющийся код
            InitializeFigure(_p1, _p2);
        }
        private void InitializeFigure(Point _p1, Point _p2) {
            P1 = _p1;
            P2 = _p2;
        }



        public override void Draw(Graphics _screen) {
            _screen.DrawLine(Pen, P1, P2);
        }


        public void Resize(Point _p1, Point _p2) {
            InitializeFigure(_p1, _p2);
        }
        public void Resize(int _x1, int _y1, int _x2, int _y2) {
            InitializeFigure(new Point(_x1, _y1), new Point(_x2, _y2));
        }

    }
}
