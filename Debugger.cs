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
    public static class Debugger {
        private static FileStream fileStream = new FileStream(@"C:\OOP_PaintLog.log", FileMode.Create, FileAccess.Write);
        private static StreamWriter streamWriter = new StreamWriter(fileStream);



        public static void Log(string log) {
            //using(var streamWriter = new StreamWriter(fileStream)) ;Это не работает
            streamWriter.Write(log + "\r\n"); //А тут норм
        }
        public static void Stop() {
            streamWriter.Close();
        }



    }
}
