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
using static CAD_Client.ToolEnum;

namespace CAD_Client {
    public class MyCut : MyFigure{
        private const int VetrexesCount = 2;
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
        public float Length { private set; get; }



        public MyCut(Color color, PointF p1, PointF p2) : base(color) {
            //???Повторяющийся код
            InitializeFigure(p1, p2);
        }
        public MyCut(Pen pen, PointF p1, PointF p2) : base (pen) {
            //???Повторяющийся код
            InitializeFigure(p1, p2);
        }
        private void InitializeFigure(PointF p1, PointF p2) {
            this.p1 = p1;
            this.p2 = p2;
            Length = MyGeometry.FindLengthBetweenPoints(p1, p2);
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


        public void Resize(PointF p1, PointF p2) {
            InitializeFigure(p1, p2);
        }
        public void Resize(float x1, float y1, float x2, float y2) {
            InitializeFigure(new PointF(x1, y1), new PointF(x2, y2));
        }

    }
}
