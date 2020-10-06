using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static OOP_Paint.FiguresEnum;
//???Если я один фиг использую класс вместо перечисления с ToString, может, просто перечисление в структуру обернуть??
//Но это не логично, так как BuildingMethod ограниченное число всегда!!
//MSDN пишет, что классы-перечисления это норма. Ну штош...
namespace OOP_Paint {
    public sealed class ComboboxBuildingMethod {
        public readonly BuildingMethod BuildingMethod;
        //Пидарас!
        //Пи-до-ра-си-на!
        //DataSourse работает исключительно с публичными полями. Вроде можно заменить.
        public string Name { get; }



        public ComboboxBuildingMethod(BuildingMethod _buildingMethod) {
            BuildingMethod = _buildingMethod;
            Name = _buildingMethod.GetDescription();
        }
    }
}
