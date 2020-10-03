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
        public enum Figures {
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
    }
}
