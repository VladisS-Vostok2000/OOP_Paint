using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static CAD_Client.ToolEnum;
using System.IO;

namespace CAD_Client {
    /// <summary>
    /// Позволяет визуализировать себя в объект <see cref="Bitmap"/>.
    /// </summary>
    internal interface IBitmapable {
        Bitmap ToBitmap(PointF bitmapLocation, int bitmapWidth, int bitmapHeight);
    }
}
