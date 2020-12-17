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
    public class MyListContainer<T> {
        private readonly List<T> list = new List<T>();

        public int Count { private set; get; } = 0;

        public event EventHandler ContainerChanged;



        public MyListContainer() {
            ContainerChanged += MyListContainer_MyContainerChanged;
        }



        public T this[int index] {
            set => list[index] = value;
            get => list[index];
        }



        private void MyListContainer_MyContainerChanged(object sender, EventArgs e) {
            Count = list.Count;
        }
        public IEnumerator<T> GetEnumerator() {
            return list.GetEnumerator();
        }


        public void Add(T value) {
            list.Add(value);
            ContainerChanged?.Invoke(this, EventArgs.Empty);
        }
        public void RemoveAt(int index) {
            list.RemoveAt(index);
            ContainerChanged?.Invoke(this, EventArgs.Empty);
        }


        #region xml
        public void ToXml(string filename) {
            XmlWriter writer = XmlWriter.Create(filename);
        }
        public void FromXml(string filename) {
            XmlReader reader = XmlReader.Create(filename);
        }
        #endregion

    }
}