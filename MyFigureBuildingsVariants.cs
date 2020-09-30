using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_Paint {
    //#FB1: некорректное проектирование: начало слияния FigureBuilding с MyFigure
    public static class MyFigureBuildingsVariants {
        public enum Method {
            [Description("Нет")]
            None,
            [Description("Прямой угол, точка")]
            CircleInRectangleByTwoDots,
            [Description("Центр, радиус")]
            CircleDotRadius,

        }
    }
}
