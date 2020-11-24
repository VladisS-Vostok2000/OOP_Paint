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
using static OOP_Paint.FiguresEnum;
using static OOP_Paint.Debugger;
using System.IO;

namespace OOP_Paint {
    public static class Debugger {
        private static FileStream fileStream = new FileStream(@"C:\OOP_PaintLog.log", FileMode.Create);
        private static StreamWriter streamWriter = new StreamWriter(fileStream);



        public static void Log(String log) {
            streamWriter.Write(log + "\r\n");
        }
        public static void Stop() {
            streamWriter.Close();
        }

    }
}
