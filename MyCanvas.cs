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
    //MyScreen#01: исправить прорисовку snap
    //MyScreen#02: расчленить MyCanvas на математическую плоскость и холст дополнительной прорисовки
    /// <summary>
    /// Холст. Всегда ненулевой и отображает заданный участок математической плоскости
    /// относительно смещения реальных координат левого верхнего угла пикселя холста от реального центра координат.
    /// Содержит инструменты дополнительной прорисовки на плоскости. Содержит все существующие фигуры.
    /// </summary>
    internal sealed class MyCanvas {
        /// <summary>
        /// Положение первого пикселя холста в реальных координатах.
        /// </summary>
        internal int X { set; get; }
        /// <summary>
        /// Положение первого пикселя холста в реальных координатах.
        /// </summary>
        internal int Y { set; get; }
        /// <summary>
        /// Положение первого пикселя холста в реальных координатах. Смещение, дельта.
        /// </summary>
        internal Point Location {
            set {
                if (value.X != X && value.Y != Y) {
                    X = value.X;
                    Y = value.Y;
                }
            }
            get => new Point(X, Y);
        }

        internal int Width => Bitmap.Width;
        internal int Height => Bitmap.Height;

        internal Bitmap Bitmap { private set; get; }
        private readonly Graphics screen;
        private readonly int gridSizePx = 25;
        private readonly Pen gridPen = new Pen(Color.DarkGray, 1);
        private readonly int snapSideLength = 10;



        //????Добавить ограничение на пустой bitmap и т.д.
        internal MyCanvas(Bitmap bitmap) {
            Bitmap = bitmap ?? throw new ArgumentNullException();
            screen = Graphics.FromImage(bitmap);
            X = Width / 2;
            Y = Height / 2;
        }



        #region Прорисовка
        /// <summary>
        /// Прорисует все существующие фигуры. Холст будет предварительно очищен.
        /// </summary>
        internal void RedrawFigures(MyListContainer<MyFigure> figures, List<MyFigure> supportFigures, MyRay polarLine) {
            //???Сигнатура выглядит как ужас.
            screen.Clear(Color.FromArgb(250, 64, 64, 64));
            polarLine.Display(screen, Location);
            foreach (var figure in figures) {
                figure.Display(screen, Location);
            }
            foreach (var figure in supportFigures) {
                figure.Display(screen, Location);
            }
        }

        /// <summary>
        /// Визуализирует сетку на холсте с заданной размерностью через пиксельное выражение реального центра координат.
        /// </summary>
        private void DrawGrid(int gridInterval) {
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
            for (; i < Width; i += gridSizePx) {
                screen.DrawLine(gridPen, i, 0, i, Height);
            }

            i = CalculateOffset(Y, gridSizePx);
            //Горизонтальные
            for (; i < Height; i += gridSizePx) {
                screen.DrawLine(gridPen, 0, i, Width, i);
            }
        }
        /// <summary>
        /// Вернёт координаты первого "узла" для прорисовки сетки соответствующей координаты.
        /// </summary>
        private static int CalculateOffset(in int coord, in int gridSizePx) => coord % gridSizePx <= 0 ? Math.Abs(coord % gridSizePx) : gridSizePx - coord % gridSizePx;


        /// <summary>
        /// Визуализирует на холсте точку привязки в заданной пиксельной координате относительно <see cref="System.Drawing.Bitmap"/>.
        /// </summary>
        /// <para>P-></para>
        internal void DrawSnapPoint(Point snapPxLocation) => screen.DrawRectangle(new Pen(Color.Green, 2), snapPxLocation.X, snapPxLocation.Y, snapSideLength, snapSideLength);
        #endregion


        #region Snap
        /// <summary>
        /// Проверит, следует ли создавать привязку курсора по заданному расстоянию к пикселю холста.
        /// <para>P-></para>
        /// </summary>
        /// <param name="cursor"> Положение курсора мыши относительно холста. </param>
        /// <param name="pxLocation"> Положение пикселя относительно холста. </param>
        /// <param name="snapDistancePx"> Достаточное расстояние от курсора до пикселя для создания привязки. </param>
        internal bool CheckSnap(Point cursor, Point pxLocation, int snapDistancePx) {
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
        private bool IsPointInSquare(Point px, Point squareCenter, int pxInterval) =>
            //for pxSquare string @....@....@ pxInterval = 5 or lower for true
            Math.Abs(px.X - squareCenter.X) <= pxInterval && Math.Abs(px.Y - squareCenter.Y) <= pxInterval;
        #endregion


        #region API
        /// <summary>
        /// Сместит реальные координаты верхней левой точки холста на заданную величину.
        /// </summary>
        internal void MoveOn(Point offset) {
            Debugger.Log($"Начат MoveOn:");
            Debugger.Log($"X: {X}, Y: {Y}");
            X += offset.X;
            Y += offset.Y;
            Debugger.Log($"X: {X}, Y: {Y}");
        }


        /// <summary>
        /// Вернёт координаты пикселеля холста, отобразившего бы заданную в реальных координатах точку.
        /// <para>R->P</para>
        /// </summary>
        internal Point ToPx(PointF realCoord) {
            return new Point((int)Math.Round(realCoord.X) - X, (int)Math.Round(realCoord.Y) - Y);
        }
        /// <summary>
        /// Вернёт реальные координаты пикселя холста.
        /// <para>P->R</para>
        /// </summary>
        internal PointF ToReal(PointF pointPx) => pointPx.Sum(Location);
        #endregion

    }
}
