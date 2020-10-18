using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_Paint {
    public static class FiguresEnum {
        public enum Figure {
            None,
            Cut,
            Circle,
            Rectangle,
        }

        //???Если у меня неизбежно фигуры и методы отделены, зачем дифференцировать 
        //методы построения по фигурам?
        public enum BuildingMethod {
            None,
            Point,
            CircleInRectangleByTwoDots,
            CircleCenterRadius,
            RectangleTwoPoints,
            CutTwoPoints

        }



        public static String GetDescription(this BuildingMethod _bm) {
            switch (_bm) {
                case BuildingMethod.None: return "";
                case BuildingMethod.CircleCenterRadius: return "Центр, радиус.";
                case BuildingMethod.CircleInRectangleByTwoDots: return "Ограничивающий прямоугольник";
                case BuildingMethod.RectangleTwoPoints: return "Две точки";
                case BuildingMethod.CutTwoPoints: return "Две точки";
                default: throw new Exception($"Для метода построения {_bm} не реализовано описание.");
            }

        }
        public static String GetDescription(this Figure _figure) {
            switch (_figure) {
                case Figure.None: return "";
                case Figure.Circle: return "Окружность";
                case Figure.Rectangle: return "Прямоугольник";
                case Figure.Cut: return "Отрезок";
                default: throw new Exception($"Для фигуры {_figure} не реализовано описание.");
            }

        }
        public static String GetDescription(MyFigure _myFigure) {
            //???Всё-таки мне пригодился конвертер. Смотрится не очень. Стоит ли внедрить
            //FiguresEnum в MyFigure?
            if (_myFigure is MyCircle) {
                return Figure.Circle.GetDescription();
            }
            else
            if (_myFigure is MyRectangle) {
                return Figure.Rectangle.GetDescription();
            }
            else if (_myFigure is MyCut) {
                return Figure.Cut.GetDescription();
            }
            else throw new Exception($"Для фигуры {_myFigure} не реализовано описание");
        }
        public static List<BuildingMethod> ReturnPossibleBuildingVariants(Figure _figure) {
            var out_list = new List<BuildingMethod>();
            switch (_figure) {
                case Figure.None:
                    out_list.Add(BuildingMethod.None);
                    break;
                case Figure.Circle:
                    out_list.Add(BuildingMethod.CircleInRectangleByTwoDots);
                    out_list.Add(BuildingMethod.CircleCenterRadius);
                    break;
                case Figure.Rectangle:
                    out_list.Add(BuildingMethod.RectangleTwoPoints);
                    break;
                case Figure.Cut:
                    out_list.Add(BuildingMethod.CutTwoPoints);
                    break;
                default: throw new Exception($"Для фигуры {_figure} не реализованы варианты построения.");
            }

            return out_list;
        }
    }
}
