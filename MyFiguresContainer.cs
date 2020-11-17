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

namespace OOP_Paint {
    public class MyFiguresContainer {
        public readonly BindingList<MyFigure> FiguresList = new BindingList<MyFigure>();
        public readonly string[] test = new string[15];


        public MyFigure this[int index] {
            get {
                return FiguresList[index];
            }
            set {
                FiguresList[index] = value;
            }
        }



        public IEnumerator<MyFigure> GetEnumerator() {
            return FiguresList.GetEnumerator();
        }


        #region xml
        public void ToXml(string filename) {
            XmlWriter writer = XmlWriter.Create(filename);
        }
        public void FromXml(string filename) {
            XmlReader reader = XmlReader.Create(filename);
        }

        public Boolean MoveNext() {
            throw new NotImplementedException();
        }

        public void Reset() {
            throw new NotImplementedException();
        }
        #endregion

    }
}