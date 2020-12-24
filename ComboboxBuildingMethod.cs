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
using static CAD_Client.ToolEnum;
namespace CAD_Client {
    internal sealed class ComboboxBuildingMethod {
        internal readonly BuildingMethod BuildingMethod;
        internal string DisplayMember { get; }



        internal ComboboxBuildingMethod(BuildingMethod _buildingMethod) {
            BuildingMethod = _buildingMethod;
            DisplayMember = _buildingMethod.GetDescription();
        }

    }
}
