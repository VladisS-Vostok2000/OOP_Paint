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
        public Color FillColor;
        
        public IReadOnlyList<PointF> Vertexes { get; }
        protected PointF[] VertexesArray;


        protected MyFigure(int vertexesCount) {
            VertexesArray = new PointF[vertexesCount];
            Vertexes = Array.AsReadOnly(VertexesArray);

        }
        protected MyFigure(Pen _pen, int vertexesCount) : this(vertexesCount) {
            Pen = _pen;
            FiguresCount++;
            Id = FiguresCount;

        }
        protected MyFigure(Color _color, int vertexesCount) : this(new Pen(_color, 1), vertexesCount) {

        }



        //MyFigure#81: реализовать удаление фигуры и присваивание ID.
        public void Dispose() { }


        public virtual void Draw(Graphics _screen) {
            Pen _pen;
            if (IsSelected) {
                _pen = SelectedPen;
            }
            else
            if (IsHightLighed) {
                _pen = HightLightedPen;
            }
            else {
                _pen = Pen;
            }

            DrawFigure(_screen, _pen);
        }
        protected abstract void DrawFigure(Graphics _screen, Pen _pen);


        public String GetDescription() {
            return FiguresEnum.GetDescription(this);
        }


        //???Может переместить их куда-то? Неочевидные методы.
        /// <summary>
        /// Возвращает левый верхний угол прямоугольника с горизональной гранью по координатам двух точек
        /// </summary>
        /// <param name="_p1">Первая точка</param>
        /// <param name="_p2">Вторая точка</param>
        public static PointF FindLeftUpCornerCoord(Single _x1, Single _y1, Single _x2, Single _y2) {
            Single lowX = _x1 > _x2 ? _x2 : _x1;
            Single lowY = _y1 > _y2 ? _y2 : _y1;
            return new PointF(lowX, lowY);
        }
        /// <summary>
        /// Обрубает координатный прямоугольник до квадрата относительно первой точки
        /// </summary>
        public static (PointF, PointF) CutCoordinatesRectangleToSquare(Single _x1, Single _y1, Single _x2, Single _y2) {
            var p1 = new PointF(_x1, _y1);
            var p2 = new PointF(_x2, _y2);

            Single a = Math.Abs(p2.X - p1.X);
            Single b = Math.Abs(p2.Y - p1.Y);
            if (Math.Abs(a) > Math.Abs(b)) {
                p2.X = _x1 > _x2 ? p1.X - b : p1.X + b;
            }
            else {
                p2.Y = _y1 > _y2 ? p1.Y - a : p1.Y + a;
            }
            return (p1, p2);
        }
        /// <summary>
        /// Находит длину между двумя точками
        /// </summary>
        public static Single FindLength(PointF _p1, PointF _p2) {
            return (Single)Math.Sqrt(Math.Pow(_p1.X - _p2.X, 2) + Math.Pow(_p1.Y - _p2.Y, 2));
        }
    }
}
