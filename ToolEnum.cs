using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CAD_Client {
    internal static class ToolEnum {
        internal enum Tool {
            None,
            Moving,
            Select,
            Cut,
            Circle,
            Rectangle,
        }

        //???Если у меня неизбежно фигуры и методы отделены, зачем дифференцировать 
        //методы построения по фигурам?
        internal enum BuildingMethod {
            None,
            Point,
            CircleInRectangleByTwoDots,
            CircleCenterRadius,
            RectangleTwoPoints,
            CutTwoPoints
        }



        internal static string GetDescription(this BuildingMethod _bm) {
            switch (_bm) {
                case BuildingMethod.None: return "";
                case BuildingMethod.CircleCenterRadius: return "Центр, радиус.";
                case BuildingMethod.CircleInRectangleByTwoDots: return "Ограничивающий прямоугольник";
                case BuildingMethod.RectangleTwoPoints: return "Две точки";
                case BuildingMethod.CutTwoPoints: return "Две точки";
                default: throw new Exception($"Для метода построения {_bm} не реализовано описание.");
            }

        }
        internal static string GetDescription(this Tool _figure) {
            switch (_figure) {
                case Tool.None: return "";
                case Tool.Circle: return "Окружность";
                case Tool.Rectangle: return "Прямоугольник";
                case Tool.Cut: return "Отрезок";
                default: throw new Exception($"Для фигуры {_figure} не реализовано описание.");
            }

        }
        internal static string GetDescription(MyFigure _myFigure) {
            //???Всё-таки мне пригодился конвертер. Смотрится не очень. Стоит ли внедрить
            //FiguresEnum в MyFigure?
            if (_myFigure is MyCircle) {
                return Tool.Circle.GetDescription();
            }
            else
            if (_myFigure is MyRectangle) {
                return Tool.Rectangle.GetDescription();
            }
            else if (_myFigure is MyCut) {
                return Tool.Cut.GetDescription();
            }
            else throw new Exception($"Для фигуры {_myFigure} не реализовано описание");
        }
        internal static List<BuildingMethod> ReturnPossibleBuildingVariants(Tool _figure) {
            var out_list = new List<BuildingMethod>();
            switch (_figure) {
                case Tool.None:
                case Tool.Select:
                case Tool.Moving:
                    out_list.Add(BuildingMethod.None);
                    break;
                case Tool.Circle:
                    out_list.Add(BuildingMethod.CircleInRectangleByTwoDots);
                    out_list.Add(BuildingMethod.CircleCenterRadius);
                    break;
                case Tool.Rectangle:
                    out_list.Add(BuildingMethod.RectangleTwoPoints);
                    break;
                case Tool.Cut:
                    out_list.Add(BuildingMethod.CutTwoPoints);
                    break;
                default: throw new Exception($"Для фигуры {_figure} не реализованы варианты построения.");
            }

            return out_list;
        }
    }
}
