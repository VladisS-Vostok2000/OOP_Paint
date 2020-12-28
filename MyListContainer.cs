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

namespace CAD_Client {
    internal class MyListContainer<T> : IEnumerable<T>, ICollection<T> {
        private readonly List<T> list = new List<T>();

        internal int Count => list.Count;


        public bool IsReadOnly => throw new NotImplementedException();

        internal event EventHandler ContainerChanged;



        internal MyListContainer() { }



        internal T this[int index] {
            set => list[index] = value;
            get => list[index];
        }



        public IEnumerator<T> GetEnumerator() => list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();
        int ICollection<T>.Count => list.Count();
        void ICollection<T>.Add(T item) => list.Add(item);
        void ICollection<T>.Clear() => list.Clear();
        public bool Contains(T item) => list.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);
        public bool Remove(T item) => list.Remove(item);


        #region API
        internal void Add(T value) {
            list.Add(value);
            ContainerChanged?.Invoke(this, EventArgs.Empty);
        }
        internal void RemoveAt(int index) {
            list.RemoveAt(index);
            ContainerChanged?.Invoke(this, EventArgs.Empty);
        }
        internal void Clear() {
            list.Clear();
            ContainerChanged?.Invoke(this, EventArgs.Empty);
        }


        internal ReadOnlyMyListContainer<T> ToReadOnly() => new ReadOnlyMyListContainer<T>(list);
        #endregion


        #region xml
        internal void ToXml(string filename) {
            XmlWriter writer = XmlWriter.Create(filename);
        }
        internal void FromXml(string filename) {
            XmlReader reader = XmlReader.Create(filename);
        }
        #endregion

    }
}