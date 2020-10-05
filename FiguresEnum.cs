using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Tracing;
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

        //???Если у меня раздельно отличаются фигуры и методы построения,
        //а обойти это нельзя, то зачем последним знать имя фигуры?
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
        public static string ToString(this BuildingMethod _buildingMethod) {
            switch(_buildingMethod) {
                case BuildingMethod.None: return "";
                case BuildingMethod.CircleCenterRadius: return "Центр, радиус";
                //Неверная бизнес-логика: "Ограниченная прямоугольником" - нет такого
                //метода. Требуется заменить на "Прямой угол, радиус." Или есть... Вопрос спорный.
                case BuildingMethod.CircleInRectangleByTwoDots: return "Ограниченная прямоугольником";
                default: throw new Exception("Неизвестный метод построения");
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
