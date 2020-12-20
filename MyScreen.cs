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
        internal int X { private set; get; }
        /// <summary>
        /// Реальная координата левой верхней точки дисплея.
        /// </summary>
        internal int Y { private set; get; }
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
        private readonly int snapDistancePx = 5;
        private readonly MyCursor myCursor;



        internal MyScreen(Bitmap bitmap, MyCursor myCursor) {
            this.bitmap = bitmap;
            screen = Graphics.FromImage(bitmap);
            Width = this.bitmap.Size.Width;
            Height = this.bitmap.Size.Height;
            X = -Width / 2;
            Y = -Height / 2;

            this.myCursor = myCursor;
        }



        #region Прорисовка
        /// <summary>
        /// Прорисует все существующие фигуры.
        /// </summary>
        /// <returns><see cref="Bitmap"/> изображение. </returns>
        internal Bitmap RedrawFigures(MyListContainer<MyFigure> figures, MyListContainer<MyFigure> supportFigures, MyRay polarLine) {
            //???Выглядит как ужас.
            screen.Clear(Color.FromArgb(250, 64, 64, 64));
            foreach (var figure in figures) {
                figure.Draw(screen);
            }
            foreach (var figure in supportFigures) {
                figure.Draw(screen);
            }
            DrawSnapPoint();
            polarLine.Draw(screen);
            return bitmap;
        }
        /// <summary>
        /// Нарисует сетку на экране размерностью <see cref="gridSizePx"/>.
        /// </summary>
        private void DrawGrid() {
            //Вертикальные
            Point delta = new Point(X % gridSizePx, Y % gridSizePx);
            for (int i = gridSizePx - delta.X; i < bitmap.Width; i += gridSizePx) {
                screen.DrawLine(gridPen, i, 0, i, bitmap.Height);
            }

            //Горизонтальные
            for (int i = gridSizePx - delta.Y; i < bitmap.Height; i += gridSizePx) {
                screen.DrawLine(gridPen, 0, i, bitmap.Width, i);
            }
        }
        /// <summary>
        /// Визуализирует точку привязки.
        /// </summary>
        private void DrawSnapPoint() {
            if (!myCursor.Snapped) {
                return;
            }

            snap.Move(myCursor.SnapLocation.X - snapDistancePx, myC




        }
        #endregion

        #region Snap
        /// <summary>
        /// Проверит, следует ли создавать привязку курсора к заданной вершине в пиксельном отображении. 
        /// </summary>
        /// <param name="cursor"> Положение курсора мыши относительно <see cref="Bitmap"/>. </param>
        /// <param name="vertex"> Положение ближайшей к курсору вершины относительно <see cref="Bitmap"/>. </param>
        private bool CheckSnap(Point cursor, Point vertex) {
            if (myCursor.Snapped) {
                return true;
            }

            //Когда курсор не на дисплее, привязка не требуется.
            if (cursor.X < 0 || cursor.Y < 0 || cursor.X >= Width || cursor.Y >= Height) {
                return false;
            }

            bool nearVertex = IsPointInSquare(cursor, vertex, snapDistancePx);
            if (nearVertex) {
                return true;
            }

            return false;
        }
        /// <summary>
        /// Возвращает реальное расположение точки в отображаемое пиксельное экранное.
        /// </summary>
        private void ConvertRealCoordToPx(in PointF location, out Point pxLocation) {
            pxLocation = new Point {
                X = (int)Math.Round(location.X),
                Y = (int)Math.Round(location.Y)
            };
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
