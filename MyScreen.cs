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
using static CAD_Client.Debugger;
using System.IO;

namespace CAD_Client {
    internal sealed partial class MyScreen {
        private readonly Bitmap bitmap;
        private readonly Graphics screen;



        MyScreen(Bitmap bitmap) {
            this.bitmap = bitmap;
            screen = Graphics.FromImage(bitmap);
        }



        /// <summary>
        /// Прорисует все существующие фигуры.
        /// </summary>
        /// <returns><see cref="Bitmap"/> изображение. </returns>
        internal Bitmap RedrawFigures(MyListContainer<MyFigure> figures, MyListContainer<MyFigure> supportFigures, MyRay polarLine) {
            screen.Clear(Color.FromArgb(250, 64, 64, 64));
            foreach (var figure in figures) {
                figure.Draw(screen);
            }
            foreach (var figure in supportFigures) {
                figure.Draw(screen);
            }
            //snapPoint.Draw(screen);
            polarLine.Draw(screen);
            return bitmap;
        }

    }
}
