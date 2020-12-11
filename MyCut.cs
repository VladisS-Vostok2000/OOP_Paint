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
    public class MyCut : MyFigure{
        private const Int32 VetrexesCount = 2;
        private PointF p1;
        public PointF P1 { 
            set {
                if (value != p1) {
                    InitializeFigure(value, P2);
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
                    InitializeFigure(P1, value);
                }
            }
            get {
                return p2;
            }
        }
        public Single Length { private set; get; }



        public MyCut(Color color, in PointF p1, in PointF p2) : base(color, VetrexesCount) {
            //???Повторяющийся код
            InitializeFigure(p1, p2);
        }
        public MyCut(Pen pen, in PointF p1, in PointF p2) : base (pen, VetrexesCount) {
            //???Повторяющийся код
            InitializeFigure(p1, p2);
        }
        private void InitializeFigure(in PointF p1, in PointF p2) {
            this.p1 = p1;
            this.p2 = p2;
            VertexesArray[0] = p1;
            VertexesArray[1] = p2;
            Length = MyGeometry.FindLength(p1, p2);
            ResetLocation();
        }



        protected void ResetLocation() {
            PointF location = MyGeometry.FindLeftUpCornerCoord(P1.X, P1.Y, P2.X, P2.Y);
            x = location.X;
            y = location.Y;
        }
        protected override void DrawFigure(Graphics screen, Pen pen) {
            screen.DrawLine(pen, P1, P2);
        }

    }
}
