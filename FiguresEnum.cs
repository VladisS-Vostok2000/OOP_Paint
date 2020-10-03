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



        //!!!//???Дублирует FindPossibleBuildingVariants в MyFigure, ОЧЕНЬ ПЛОХО!!!
        //Используется, так как есть перечисление в MainCode.
        //Избавиться от метода в классе нельзя, так как мне нужны объекты в списке,
        //классы для сравнения объектов. Выходит, перечисление лишнее?
        public static List<BuildingMethod> ReturnPossibleBuildingVariants(Figure _figure) {
            var out_list = new List<BuildingMethod>();
            switch(_figure) {
                case Figure.None:
                    out_list.Add(BuildingMethod.None);
                    break;
                case Figure.Circle:
                    out_list.Add(BuildingMethod.CircleInRectangleByTwoDots);
                    out_list.Add(BuildingMethod.CircleDotRadius);
                    break;
                default: throw new Exception("Неизвестная фигура.");
            }

            return out_list;
        }
    }
}
