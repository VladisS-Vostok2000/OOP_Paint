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
    internal class ReadOnlyMyListContainer<T> : IEnumerable<T>, ICollection<T> {
        private readonly List<T> list = new List<T>();
        internal event EventHandler ContainerChanged;

        internal int Count => list.Count;

        int ICollection<T>.Count => list.Count;
        bool ICollection<T>.IsReadOnly => true;
        
        
        
        internal ReadOnlyMyListContainer(List<T> myListContainer) => list = myListContainer;



        internal T this[int index] => list[index];



        void ICollection<T>.Add(T item) => list.Add(item);
        void ICollection<T>.Clear() => list.Clear();
        bool ICollection<T>.Contains(T item) => list.Contains(item);
        void ICollection<T>.CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);
        bool ICollection<T>.Remove(T item) => list.Remove(item);
        //???Даже близко не понимаю, что это значит. Мне просто нужен был foreach.
        public IEnumerator<T> GetEnumerator() => list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();


        #region API
        internal void Add(T value) {
            list.Add(value);
            ContainerChanged?.Invoke(this, EventArgs.Empty);
        }
        internal void RemoveAt(int index) {
            list.RemoveAt(index);
            ContainerChanged?.Invoke(this, EventArgs.Empty);
        }
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
