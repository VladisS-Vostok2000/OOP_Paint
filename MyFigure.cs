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
        public Int32 Id = 0;
        protected Single x;
        public virtual Single X { set; get; }
        protected Single y;
        public virtual Single Y { set; get; }

        //MyFigure#20: реализовать сдвиг фигур по локации
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
        public Boolean IsSelected;
        public Boolean IsHightLighed;
        public Boolean IsFill;
        public Boolean IsHide;
        public Color FillColor;
        
        public IReadOnlyList<PointF> Vertexes { get; }
        protected readonly PointF[] VertexesArray;



        protected MyFigure(int vertexesCount) {
            VertexesArray = new PointF[vertexesCount];
            Vertexes = Array.AsReadOnly(VertexesArray);

        }
        protected MyFigure(Pen pen, int vertexesCount) : this(vertexesCount) {
            Pen = pen;
            FiguresCount++;
            Id = FiguresCount;

        }
        protected MyFigure(Color color, int vertexesCount) : this(new Pen(color, 1), vertexesCount) {

        }



        //MyFigure#81: реализовать удаление фигуры и присваивание ID.
        public void Dispose() { }


        public virtual void Draw(Graphics screen) {
            if (IsHide) {
                return;
            }

            Pen pen;
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

            DrawFigure(screen, pen);
        }
        protected abstract void DrawFigure(Graphics screen, Pen pen);


        public String GetDescription() {
            return FiguresEnum.GetDescription(this);
        }


        //???Может переместить их куда-то? Неочевидные методы.
        /// <summary>
        /// Возвращает левый верхний угол прямоугольника с горизональной гранью по координатам двух точек
        /// </summary>
        /// <param name="p1">Первая точка</param>
        /// <param name="p2">Вторая точка</param>
        public static PointF FindLeftUpCornerCoord(Single x1, Single y1, Single x2, Single y2) {
            Single lowX = x1 > x2 ? x2 : x1;
            Single lowY = y1 > y2 ? y2 : y1;
            return new PointF(lowX, lowY);
        }
        /// <summary>
        /// Обрубает координатный прямоугольник до квадрата относительно первой точки
        /// </summary>
        public static (PointF, PointF) CutCoordinatesRectangleToSquare(Single x1, Single y1, Single x2, Single y2) {
            var p1 = new PointF(x1, y1);
            var p2 = new PointF(x2, y2);

            Single a = Math.Abs(p2.X - p1.X);
            Single b = Math.Abs(p2.Y - p1.Y);
            if (Math.Abs(a) > Math.Abs(b)) {
                p2.X = x1 > x2 ? p1.X - b : p1.X + b;
            }
            else {
                p2.Y = y1 > y2 ? p1.Y - a : p1.Y + a;
            }
            return (p1, p2);
        }
        /// <summary>
        /// Находит длину между двумя точками
        /// </summary>
        public static Single FindLength(PointF p1, PointF p2) {
            return (Single)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

    }
}
