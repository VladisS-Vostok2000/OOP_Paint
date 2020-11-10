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
    //MyCut#01: заменить вершины на контейнер
    class MyCut : MyFigure{
        private PointF p1;
        public PointF P1 { 
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
        private PointF p2;
        public PointF P2 {
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
        public Single Length { private set; get; }
        public List<PointF> Vertexes { private set; get; }



        public MyCut(Color _color, PointF _p1, PointF _p2) : base(_color) {
            //???Повторяющийся код
            InitializeFigure(_p1, _p2);
        }
        public MyCut(Pen _pen, PointF _p1, PointF _p2) : base (_pen) {
            //???Повторяющийся код
            InitializeFigure(_p1, _p2);
        }
        private void InitializeFigure(PointF _p1, PointF _p2) {
            P1 = _p1;
            P2 = _p2;
        }



        protected override void DrawFigure(Graphics _screen, Pen _pen) {
            _screen.DrawLine(_pen, P1, P2);
        }


        public void Resize(PointF _p1, PointF _p2) {
            InitializeFigure(_p1, _p2);
        }
        public void Resize(Single _x1, Single _y1, Single _x2, Single _y2) {
            InitializeFigure(new PointF(_x1, _y1), new PointF(_x2, _y2));
        }

    }
}
