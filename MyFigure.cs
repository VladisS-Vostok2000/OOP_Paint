using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//Project#34: удаление Name
namespace OOP_Paint {
    public abstract class MyFigure : IDisposable {
        public enum BuildingMethod {
            [Description("Нет")]
            None,
            [Description("Точка")]
            Point,
            [Description("Прямой угол, точка")]
            CircleInRectangleByTwoDots,
            [Description("Центр, радиус")]
            CircleDotRadius,

        }

        //???Enum в отдельном классе + подключить через using
        //???Name уникально идентифицирует класс, но по-прежнему может быть изменён,
        //однако не может быть определён как readonly поле, так как наследуется.
        //Так как строка, можно скрывать поле и в каждом классе создавать новое,
        //однако при его изменении здесь последствия могут быть неожиданными.
        public static String Name { protected set; get; }
        public Int32 X { set; get; }
        public Int32 Y { set; get; }
        public Int32 Width { set; get; }
        public Int32 Height { set; get; }
        public Pen Pen { set; get; } = new Pen(Color.Black, 2);



        protected MyFigure() {

        }
        protected MyFigure(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2) {
            Point leftCorner = FindLeftUpCornerCoord(_x1, _y1, _x2, _y2);
            X = leftCorner.X;
            Y = leftCorner.Y;
            Width = Math.Abs(_x1 - _x2);
            Height = Math.Abs(_y1 - _y2);
        }
        protected MyFigure(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2, Color _color) : this(_x1, _y1, _x2, _y2) {
            Pen = new Pen(_color, 2);
        }
        protected MyFigure(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2, Pen _pen) : this(_x1, _y1, _x2, _y2) {
            Pen = _pen;
        }



        /// <summary>
        /// Находит левый верхний угол прямоугольника по координатам двух точек
        /// </summary>
        /// <param name="_p1">Первая точка</param>
        /// <param name="_p2">Вторая точка</param>
        /// <returns></returns>
        protected Point FindLeftUpCornerCoord(Int32 _x1, Int32 _y1, Int32 _x2, Int32 _y2) {
            Int32 lowX = _x1 > _x2 ? _x2 : _x1;
            Int32 lowY = _y1 > _y2 ? _y2 : _y1;
            return new Point(lowX, lowY);
        }
        //???Поле Name уникально для класса и одинаково для всех объектов, поэтому оно статические
        //Однако Name хочется получать как из класса, так и из объекта.
        //переопределить ToString()
        public  override String ToString() {
            return Name;
        }
        public void Dispose() { }


        //???Несколько вариантов реализации: сторонний статический класс-расширение,
        //со switch-логикой вообще всех фигур, или переопределеляемый метод объекта,
        //или статический метод в MyFigure.
        //Второй вариант двумя способами: полем и методом. Статическим или виртуальным.
        //Он же жёстко требует объекта или повторного (или наследуемого?) статического метода.
        //Сторонний статический класс выглядит вырожденно, но зато имеет всё вместе.
        //Статический метод подходит из-за универсальности.
        //!!!//???Есть идея вообще сделать перечисление Figures здесь, тогда со всем разбираться не
        //придётся и конвертеры не нужны, и имена фигур без проблем получить можно.
        public List<BuildingMethod> ReturnPossibleBuildingVariants() {
            return ReturnPossibleBuildingVariants(Name);
        }
        public static List<BuildingMethod> ReturnPossibleBuildingVariants(String _name) {
            var out_list = new List<BuildingMethod>() { BuildingMethod.None };
            switch (_name) {
                case "Круг":
                    out_list.Add(BuildingMethod.CircleDotRadius);
                    out_list.Add(BuildingMethod.CircleInRectangleByTwoDots);
                    break;
                default: break;
            }

            return out_list;
        }


        public abstract void Draw(Graphics _screen);

    }
}
