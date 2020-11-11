//using OOP_Paint;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.Linq;
//using System.Net.Http.Headers;
//using System.Runtime.CompilerServices;
//using System.Security.Cryptography;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using System.Windows.Forms.VisualStyles;
//using static OOP_Paint.FiguresEnum;


//namespace OOP_Paint {
//    public abstract class MyFiguree {
//        Pen Pen;

//        /// <summary>Публичное Свойство "Только для Чтения" 
//        /// с неизменяемой оболочкой списка вершин.</summary>
//        public IReadOnlyList<PointF> Vertexes { get; }

//        /// <summary>Защищённое свойство для возможности изменения
//        /// элементов массива вершин в производных классах, 
//        /// но без возможности замены самого массива.</summary>
//        protected PointF[] VertexesArray { get; }



//        /// <summary>Запись в <see cref="Vertexes"/> оболочки "Только для чтения" массива вершин.</summary>
//        protected MyFiguree(int vertexesCount) {
//            Vertexes = Array.AsReadOnly(VertexesArray = new PointF[vertexesCount]);
//        }

//        protected MyFiguree(Pen pen, int vertexesCount) : this(vertexesCount) => Pen = pen;
//        protected MyFiguree(Color color, int vertexesCount) : this(new Pen(color, 1), vertexesCount) { }



//        protected abstract void DrawFigure(Graphics _screen, Pen _pen);

//    }



//    public class MyCutt : MyFiguree {
//        public float Length { private set; get; }



//        public MyCutt(Color _color, PointF _p1, PointF _p2) : base(_color, 2) {
//            //???Повторяющийся код
//            InitializeFigure(_p1, _p2);
//        }
//        public MyCutt(Pen _pen, PointF _p1, PointF _p2) : base(_pen, 2) {
//            //???Повторяющийся код
//            InitializeFigure(_p1, _p2);
//        }
//        private void InitializeFigure(PointF _p1, PointF _p2) {
//            //Присваивание вершин внутри класса
//            VertexesArray[0] = _p1;
//            VertexesArray[1] = _p2;
//            Length = FindLength(Vertexes[0], Vertexes[1]);
//        }



//        public static float FindLength(PointF _p1, PointF _p2) {
//            //someCodeHere
//            return 4;
//        }


//        protected override void DrawFigure(Graphics _screen, Pen _pen) {
//            //Чтение вершин внутри класса 
//            _screen.DrawLine(_pen, Vertexes[0], Vertexes[1]);
//        }


//        public void Resize(PointF _p1, PointF _p2) {
//            InitializeFigure(_p1, _p2);
//        }
//        public void Resize(float _x1, float _y1, float _x2, float _y2) {
//            InitializeFigure(new PointF(_x1, _y1), new PointF(_x2, _y2));
//        }

//    }
//}

//public class MyClass {
//    public void Main() {
//        var a = new MyCutt(Color.White, new PointF(0, 0), new PointF(5, 5));
//        //Пример чтения поля
//        float length = MyCutt.FindLength(a.Vertexes[0], new PointF(3, 2));

//        //А вот так делать нужно запретить
//        a.Vertexes[0] = new PointF(4, 4); // Будет выдавать ошибку
//    }

//}