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
        private static string path = @"C:\OOP_PaintLog.log";
        private static StreamWriter streamWriter = new StreamWriter(new FileStream(path, FileMode.Create, FileAccess.Write));



        public static void Log(string log) {
            streamWriter.WriteLine(log);
        }
        public static void Stop() {
            streamWriter.Close();
        }

        #region Это затратный режим (хз как сделать переключение)
        //private static bool hard;
        //public static string path { get; } = @"C:\OOP_PaintLog.log";
        //static Debugger() {
        //    using (var file = File.Create(path));
        //}



        //public static void Log(string log) {
        //    if (hard) {
        //        using (var streamWriter = new StreamWriter(path, true)) {
        //            streamWriter.WriteLine(log);
        //        }
        //    }
        //}
        #endregion

    }
}
