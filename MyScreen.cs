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
        /// <summary>
        /// Реальная координата левой верхней точки дисплея.
        /// </summary>
        internal int X { set; get; }
        /// <summary>
        /// Реальная координата левой верхней точки дисплея.
        /// </summary>
        internal int Y { set; get; }
        internal int Width { private set; get; }
        internal int Height { private set; get; }

        private readonly Bitmap bitmap;
        private readonly Graphics screen;
        private readonly int gridSizePx = 25;
        private readonly Pen gridPen = new Pen(Color.DarkGray, 1);

        //private Point snapPoint;
        //internal Point SnapPoint { 
        //    private set {
        //        if (Snapped) {
        //            throw new Exception("Привязка существует, но создаётся новая");
        //        }
        //        snapPoint = value;
        //    }
        //    get => snapPoint;
        //}
        //internal bool Snapped;
        private static readonly MyRectangle snap = new MyRectangle(0, 0, 9, 9) { IsHide = true, Pen = new Pen(Color.Green, 2) };
        private readonly MyCursor myCursor;



        internal MyScreen(Bitmap bitmap, MyCursor myCursor) {
            this.bitmap = bitmap;
            screen = Graphics.FromImage(bitmap);
            Width = this.bitmap.Size.Width;
            Height = this.bitmap.Size.Height;
            X = -5;//-Width / 2;
            Y = -5;//-Height / 2;

            this.myCursor = myCursor;
        }



        #region Прорисовка
        /// <summary>
        /// Прорисует все существующие фигуры.
        /// </summary>
        /// <returns><see cref="Bitmap"/> изображение. </returns>
        internal Bitmap RedrawFigures(MyListContainer<MyFigure> figures, List<MyFigure> supportFigures, MyRay polarLine) {
            //???Сигнатура выглядит как ужас.
            screen.Clear(Color.FromArgb(250, 64, 64, 64));
            DrawGrid();
            polarLine.Draw(screen);
            foreach (var figure in figures) {
                figure.Draw(screen);
            }
            foreach (var figure in supportFigures) {
                figure.Draw(screen);
            }
            DrawSnapPoint();
            return bitmap;
        }
        /// <summary>
        /// Нарисует сетку на экране размерностью <see cref="gridSizePx"/>.
        /// </summary>
        private void DrawGrid() {
            //-------------------------------------------------------------------------------------------------------------------------------------------------------
            //Дано расположение пиксельной координаты в реальных: (0; 0)* = (X; Y). Тогда:
            //delta = (X - 0; Y - 0) = (X; Y) - смещение пиксельных координат от реальных.
            //delta % gridPxSize = (X; Y) % gridPxSize - в частном порядке смещение левого верхнего угла от центра координат в пикселях, но для единичного разряда (узлы сетки)
            //offset - это координаты первого вхождения прямой через центры координат единичного разряда (узлов сетки).
            //Когда смещение нулевое или отрицательное, т.е. реальный центр координат ниже или правее, модуль смещения == offset.
            //Иначе это расстояние до левого, т.е. отрицательного узла. Тогда offset = gridPxSize - смещение, т.е. следующее вхождение.
            //-------------------------------------------------------------------------------------------------------------------------------------------------------

            //Вертикальные
            int i = CalculateOffset(X, gridSizePx);
            for (; i < bitmap.Width; i += gridSizePx) {
                screen.DrawLine(gridPen, i, 0, i, bitmap.Height);
            }

            i = CalculateOffset(Y, gridSizePx);
            //Горизонтальные
            for (; i < bitmap.Height; i += gridSizePx) {
                screen.DrawLine(gridPen, 0, i, bitmap.Width, i);
            }
        }
        /// <summary>
        /// Вернёт координаты первого "узла" для прорисовки сетки соответствующей координаты.
        /// </summary>
        private static int CalculateOffset(in int coord, in int gridSizePx) => coord % gridSizePx <= 0 ? Math.Abs(coord % gridSizePx) : gridSizePx - coord % gridSizePx;
        /// <summary>
        /// Визуализирует точку привязки.
        /// </summary>
        private void DrawSnapPoint() {
            if (!myCursor.Snapped) {
                return;
            }

            snap.Location = new PointF(myCursor.SnapLocation.X - snap.Width / 2, myCursor.SnapLocation.Y - snap.Width / 2);
            snap.Draw(screen);
        }
        #endregion

        #region Snap
        /// <summary>
        /// Проверит, следует ли создавать привязку курсора к заданному пикселю. 
        /// </summary>
        /// <param name="cursor"> Положение курсора мыши относительно <see cref="Bitmap"/>. </param>
        /// <param name="pxLocation"> Положение пикселя относительно <see cref="Bitmap"/>. </param>
        /// <param name="snapDistancePx"> Достаточное расстояние от курсора до пикселя для создания привязки. </param>
        internal bool CheckSnap(Point cursor, Point pxLocation, int snapDistancePx) {
            if (myCursor.Snapped) {
                return true;
            }

            //Когда курсор не на дисплее, привязка не требуется.
            if (cursor.X < 0 || cursor.Y < 0 || cursor.X >= Width || cursor.Y >= Height) {
                return false;
            }

            bool nearVertex = IsPointInSquare(cursor, pxLocation, snapDistancePx);
            if (nearVertex) {
                return true;
            }

            return false;
        }
        /// <summary>
        /// Определит, лежит ли пиксель в квадрате (или на его грани) с заданным центром и расстоянием от грани до центра в пикселях.
        /// </summary>
        private bool IsPointInSquare(Point px, Point center, int pxInterval) =>
            //for pxSquare string @....@....@ => pxInterval = 5 here
            Math.Abs(px.X - center.X) <= pxInterval && Math.Abs(px.Y - center.Y) <= pxInterval;
        #endregion

    }
}
