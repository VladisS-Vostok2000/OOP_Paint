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
    /// Математическая плоскость, хранящая все существующие фигуры в реальных координатах.
    /// </summary>
    internal sealed class MyMathPlane : IBitmapable, IEnumerable<MyFigure> {
        private readonly MyListContainer<MyFigure> myFigures = new MyListContainer<MyFigure>();
        internal int Count => myFigures.Count;



        internal MyFigure this[int index] {
            set => myFigures[index] = value;
            get => myFigures[index];
        }



        public IEnumerator<MyFigure> GetEnumerator() => myFigures.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => myFigures.GetEnumerator();


        #region API
        /// <summary>
        /// Вернёт <see cref="Graphics"/> с визуализированными на нём фигурами.
        /// </summary>
        /// <param name="bitmapLocation"> Расположение левого верхнего пикселя возвращаемого <see cref="Bitmap"/> в реальных координатах. </param>
        [Obsolete]
        Bitmap IBitmapable.ToBitmap(PointF bitmapLocation, int bitmapWidth, int bitmapHeight) {
            /*--------------------------------------------------------------------------------
            Graphics все свои объекты рисует относительно левого верхнего пикселя.
            Фигуры же представлены в реальных координатах. Graphics - это участок
            матплоскости, коих (участков) может быть сколько угодно, и у каждого будут свои
            координаты. Соответственно, нужно знать расположение Graphics относительно
            реальных координат. Я решил, что самый понятный способ это знание расположения
            верхнего левого пикселя относительно реальных координат. Расстояние от 
            центров позволит рисовать реальные фигуры на пиксельных участках с собественными
            координатами. Модуль смещения синонимично расстоянию, но если расстояние это
            просто величина, коротая даст радиус всех возможных участков, то смещение это
            совокупность расстояний по X и по Y центров, что даёт конкретную точку местоположения.
            Сам по себе модуль полезен мало. Т.к. не модуль смещения - это смещение <чего-то> от <чего-то>,
            то я решил, что смещение это такая величина, что, прибавь её к реальным координатам,
            получишь частные. Исходя из всех вышеперечисленных фактов, составляется чёткая картина:
                Здесь и далее: P - любая точка в рельных координатах, P* - точка в частных координатах,
                R - центр реальных координат, D - центр частных координат, d - дельта, смещение.
            P + d = P*;
            d = P* - P;
            В частном случае, когда известны центры координат, кроме реального центра координат в частном,
            а они нулевые в соответствущих им координатным плоскостям:
            D* = R = (0; 0);
            d = -D;
            R* = d;
            P* = P - D = P + d;
            //------------------------------------------------------------------------------*/
            Bitmap bitmap = new Bitmap(bitmapWidth, bitmapHeight);
            Graphics graphics = Graphics.FromImage(bitmap);
            foreach (var figure in myFigures) figure.Draw(graphics, bitmapLocation);
            return bitmap;
        }


        internal ReadOnlyMyListContainer<MyFigure> GetMyFigures() => myFigures.ToReadOnly();


        internal void Add(MyFigure myFigure) => myFigures.Add(myFigure);
        internal void RemoveAt(int index) => myFigures.RemoveAt(index);
        #endregion

    }
}