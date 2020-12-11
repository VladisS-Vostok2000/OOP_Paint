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
using static OOP_Paint.FiguresEnum;
namespace OOP_Paint {
    public abstract class MyFigure : IDisposable {
        //!!!MyFigure#01: рефакторинг конструкторов: изменить порядок параметров и убрать задание через "color".
        public static Int32 FiguresCount = 0;
        public Int32 Id { private set; get; } = 0;

        protected Single x;
        public virtual Single X { set; get; }
        protected Single y;
        public virtual Single Y { set; get; }
        //!!!MyFigure#20: реализовать сдвиг фигур по локации
        /// <summary> Верхняя левая точка описывающего квадрата. При изменении перемещается вся фигура. </summary>
        public PointF Location {
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

        public Pen Pen { set; get; } = new Pen(Color.Black, 1);
        public Pen SelectedPen { set; get; } = new Pen(Color.White, 2);
        public Pen HightLightedPen { set; get; } = new Pen(Color.BlueViolet, 2);
        public Boolean IsSelected { set; get; }
        public Boolean IsHightLighed { set; get; }
        public Boolean IsFill { set; get; }
        public Boolean IsHide { set; get; }
        public Color FillColor { set; get; }


        public IReadOnlyList<PointF> Vertexes { get; }
        protected readonly PointF[] VertexesArray;



        protected MyFigure(in Int32 vertexesCount) {
            VertexesArray = new PointF[vertexesCount];
            Vertexes = Array.AsReadOnly(VertexesArray);

        }
        protected MyFigure(Pen pen, in Int32 vertexesCount) : this(vertexesCount) {
            Pen = pen;
            FiguresCount++;
            Id = FiguresCount;

        }
        protected MyFigure(Color color, in Int32 vertexesCount) : this(new Pen(color, 1), vertexesCount) {

        }



        //MyFigure#81: реализовать удаление фигуры и присваивание ID.
        public void Dispose() { }


        public virtual void Draw(Graphics screen) {
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


        public String GetDescription() {
            return FiguresEnum.GetDescription(this);
        }

    }
}
