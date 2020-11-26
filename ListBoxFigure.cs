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
    public sealed class ListBoxFigure {
        public String DisplayMember { get; }
        public Int32 Id { get; }



        public ListBoxFigure(MyFigure _object) {
            Id = _object.Id;
            DisplayMember = $"[{_object.X},{_object.Y}]".PadRight(10) + _object.GetDescription();
        }

    }
}
