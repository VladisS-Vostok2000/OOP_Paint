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
        public Int32 X { set; get; }
        public Int32 Y { set; get; }
        public Pen Pen { set; get; } = new Pen(Color.Black, 2);



        //!!!MyFigure#75: перестройка конструкторов
        protected MyFigure() {

        }
        protected MyFigure(Pen _pen) {
            Pen = _pen;

        }
        protected MyFigure(Color _color) : this(new Pen(_color, 1)) {

        }
        /// <summary>
        /// Устанавливает верхний левый угол прямоугольника, заданного двумя точками.
        /// </summary>
        protected MyFigure(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2) {
            Point leftCorner = FindLeftUpCornerCoord(_x1, _y1, _x2, _y2);
            X = leftCorner.X;
            Y = leftCorner.Y;

        }
        /// <summary>
        /// Устанавливает верхний левый угол прямоугольника, заданного двумя точками, и цвет фигуры.
        /// </summary>
        protected MyFigure(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2, Color _color) : this(_x1, _y1, _x2, _y2) {
            //???Повторяющийся код, ожидалось this(_color), однако оператор this уже занят.
            Pen = new Pen(_color, 1);

        }
        /// <summary>
        /// Устанавливает верхний левый угол прямоугольника, заданного двумя точками, ширину и цвет фигуры.
        /// </summary>
        protected MyFigure(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2, Pen _pen) : this(_x1, _y1, _x2, _y2) {
            //???Повторяющийся код, ожидалось this(_pen), однако оператор this уже занят.
            Pen = _pen;

        }



        /// <summary>
        /// Возвращает левый верхний угол прямоугольника по координатам двух точек
        /// </summary>
        /// <param name="_p1">Первая точка</param>
        /// <param name="_p2">Вторая точка</param>
        protected Point FindLeftUpCornerCoord(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2) {
            Int32 lowX = _x1 > _x2 ? _x2 : _x1;
            Int32 lowY = _y1 > _y2 ? _y2 : _y1;
            return new Point(lowX, lowY);
        }


        public void Dispose() { }

        #region //MyFigure.ReturnPossibleBBuildingVariants
        //!!!MyFigure#23: переработка ReturnPossibleBuildingVariants - fixed
        //???Несколько вариантов реализации: сторонний статический класс-расширение,
        //со switch-логикой вообще всех фигур, или переопределеляемый метод объекта,
        //или статический метод в MyFigure.
        //Второй вариант двумя способами: полем и методом. Статическим или виртуальным.
        //Он же жёстко требует объекта или повторного (или наследуемого?) статического метода.
        //Сторонний статический класс выглядит вырожденно, но зато имеет всё вместе.
        //Статический метод подходит из-за универсальности.
        //!!!//???Есть идея вообще сделать перечисление Figures здесь, тогда со всем разбираться не
        //придётся и конвертеры не нужны, и имена фигур без проблем получить можно.
        //По совету перечисление вынесено в отдельный класс, и метод соответственно.
        #region код
        //public List<BuildingMethod> ReturnPossibleBuildingVariants() {
        //    return ReturnPossibleBuildingVariants(this.GetType());
        //}
        //public static List<BuildingMethod> ReturnPossibleBuildingVariants(Type _type) {
        //    var out_list = new List<BuildingMethod>() { BuildingMethod.None };
        //    //Держу в курсе switch принимает только константы
        //    if (_type == typeof(MyCircle)) {
        //        out_list.Add(BuildingMethod.CircleDotRadius);
        //        out_list.Add(BuildingMethod.CircleInRectangleByTwoDots);
        //    }
        //    else {
        //        throw new Exception("Неизвестная фигура.");
        //    }

        //    return out_list;
        //}
        #endregion
        #endregion

        public abstract void Draw(Graphics _screen);

    }
}
