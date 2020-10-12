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

namespace OOP_Paint {
    public struct ListBoxFigure {
        public readonly string Name;
        public readonly int Id;
        


        public ListBoxFigure(MyFigure _object) {
            Id = _object.Id;
            Name = _object.GetDescription();

        }

    }
}
