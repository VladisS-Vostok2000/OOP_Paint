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
    internal class MyCut : MyFigure {
        private protected PointF p1;
        internal PointF P1 { 
            private protected set {
                if (value != p1) {
                    InitializeFigure(value, P2);
                }
            } 
            get {
                return p1;
            }
        }
        private protected PointF p2;
        internal PointF P2 {
            set {
                if (value != p2) {
                    p2 = value;
                    Location = MyGeometry.FindLeftUpCornerCoord(p1, value);
                }
            }
            get {
                return p2;
            }
        }
        internal float Length { private set; get; }



        internal MyCut(PointF p1, PointF p2) {
            //???Повторяющийся код
            InitializeFigure(p1, p2);
        }
        internal MyCut(Color color, PointF p1, PointF p2) : base(color) {
            //???Повторяющийся код
            InitializeFigure(p1, p2);
        }
        internal MyCut(Pen pen, PointF p1, PointF p2) : base (pen) {
            //???Повторяющийся код
            InitializeFigure(p1, p2);
        }
        private void InitializeFigure(PointF p1, PointF p2) {
            Location = MyGeometry.FindLeftUpCornerCoord(p1, p2);
            this.p1 = p1;
            this.p2 = p2;
            Length = MyGeometry.FindLengthBetweenPoints(P1, P2);
        }



        internal override void Move(PointF newLocation) {
            p1 = newLocation.Sum(P1.Substract(Location));
            p2 = newLocation.Sum(P2.Substract(Location));
            Location = newLocation;
        }


        private protected override void Display(Graphics screen, Pen pen, Point center) {
            screen.DrawLine(pen, P1.Substract(center), P2.Substract(center));
        }
        
    }
}
