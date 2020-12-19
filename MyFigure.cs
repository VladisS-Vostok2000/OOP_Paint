using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CAD_Client.ToolEnum;
namespace CAD_Client {
    internal abstract class MyFigure : IDisposable {
        internal static int FiguresCount = 0;
        internal int Id { private set; get; } = 0;

        protected float x;
        internal virtual float X { set; get; }
        protected float y;
        internal virtual float Y { set; get; }
        //!!!MyFigure#20: реализовать сдвиг фигур по локации
        /// <summary> Верхняя левая точка описывающего квадрата. При изменении перемещается вся фигура. </summary>
        internal PointF Location {
            set {
                if (value.X != X || value.Y != Y) {
                    X = value.X;
                    Y = value.Y;
                }
            }
            get {
                return new PointF(X, Y);
            }
        }

        internal Pen Pen { set; get; } = new Pen(Color.Black, 1);
        internal Pen SelectedPen { set; get; } = new Pen(Color.White, 2);
        internal Pen HightLightedPen { set; get; } = new Pen(Color.BlueViolet, 2);
        internal bool IsSelected { set; get; }
        internal bool IsHightLighed { set; get; }
        internal bool IsFill { set; get; }
        internal bool IsHide { set; get; }
        internal Color FillColor { set; get; }



        protected MyFigure() { }
        protected MyFigure(Pen pen) {
            Pen = pen;
            FiguresCount++;
            Id = FiguresCount;
        }
        protected MyFigure(Color color) : this(new Pen(color, 1)) { }



        //MyFigure#81: реализовать удаление фигуры и присваивание ID.
        public void Dispose() { }


        internal virtual void Draw(Graphics screen) {
            if (IsHide) {
                return;
            }

            ChoosePen(out Pen pen);
            DrawFigure(screen, pen);
        }
        protected virtual Pen ChoosePen(out Pen pen) {
            if (IsSelected) {
                pen = SelectedPen;
            }
            else
            if (IsHightLighed) {
                pen = HightLightedPen;
            }
            else {
                pen = Pen;
            }
            return pen;
        }
        protected abstract void DrawFigure(Graphics screen, Pen pen);


        internal string GetDescription() {
            return ToolEnum.GetDescription(this);
        }

    }
}
