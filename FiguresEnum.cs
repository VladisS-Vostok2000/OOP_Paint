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
            Circle,

        }

        //???Если у меня неизбежно фигуры и методы отделены, зачем дифференцировать 
        //методы построения по фигурам?
        public enum BuildingMethod {
            //[Description("Нет")]
            None,
            //[Description("Точка")]
            Point,
            //[Description("Прямой угол, точка")]
            CircleInRectangleByTwoDots,
            //[Description("Центр, радиус")]
            CircleCenterRadius,

        }


        //!!!FiguresEnum#7: реализовать конвертер в имя.
        public static string ToString(BuildingMethod _bm) {
            switch(_bm) {
                case BuildingMethod.None: return "";
                case BuildingMethod.CircleCenterRadius: return "Центр, радиус.";
                case BuildingMethod.CircleInRectangleByTwoDots: return "Ограничивающий прямоугольник";
                default: throw new Exception("Неизвестная фигура");
            }
        }


        public static List<BuildingMethod> ReturnPossibleBuildingVariants(Figure _figure) {
            var out_list = new List<BuildingMethod>();
            switch(_figure) {
                case Figure.None:
                    out_list.Add(BuildingMethod.None);
                    break;
                case Figure.Circle:
                    out_list.Add(BuildingMethod.CircleInRectangleByTwoDots);
                    out_list.Add(BuildingMethod.CircleCenterRadius);
                    break;
                default: throw new Exception("Неизвестная фигура.");
            }

            return out_list;
        }
    }
}
