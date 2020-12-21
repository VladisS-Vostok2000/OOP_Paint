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
    internal static class MyGeometry {
        /// <summary> По двум точкам находит ближайшую горизонтальную, вертикальную или диагональную прямую, проходящие через первую точку. </summary>
        /// <returns> Точка, лежащая на ближайшей прямой с направлением второй точки. </returns>
        internal static PointF DirectToPolarLine(in PointF p1, in PointF p2) {
            float a = p2.X - p1.X;
            float b = p2.Y - p1.Y;
            float k = b / a;

            //Вторая точка уже лежит на прямой
            if (k == 0 || Math.Abs(k) == 1 || float.IsInfinity(k)) {
                return new PointF(p1.X + a, p1.Y + b);
            }

            var p3 = new PointF(a, b);
            float c;
            PointF p4;
            //Выясняем, с какой прямой будем сравнивать
            if (Math.Abs(k) < 1) {
                //С горизонтальной
                p4 = new PointF(a, 0);
                c = p3.X;
            }
            else {
                //С вертикальной
                p4 = new PointF(0, b);
                c = p3.Y;
            }

            float sqrt2 = (float)Math.Sqrt(2);
            var p5 = new PointF(Math.Sign(a) * Math.Abs(c) / sqrt2, Math.Sign(b) * Math.Abs(c) / sqrt2);

            bool isDiagonalCloser = Math.Abs(a * p4.X + b * p4.Y) <= a * p5.X + b * p5.Y;
            if (isDiagonalCloser) {
                return new PointF(p1.X + p5.X, p1.Y + p5.Y);
            }
            else {
                return new PointF(p1.X + p4.X, p1.Y + p4.Y);
            }

        }
        /// <summary>
        /// True, если два отрезка параллельны/коллинеарны.
        /// </summary>
        internal static bool IsLinesParallel(in PointF p1, in PointF p2, in PointF p3, in PointF p4) {
            //Если отношения смещений на клетку х и у двух отрезков по модулю равны, то они параллельны (k коэфф один)
            //И по свойству пропорции:
            if (Math.Abs((p1.X - p2.X) * (p3.Y - p4.Y)) == Math.Abs((p1.Y - p2.Y) * (p3.X - p4.X))) {
                return true;
            }

            return false;
        }
        /// <summary>
        /// Вернёт true, если точка лежит в заданном отрезке.
        /// </summary>
        internal static bool IsPointInCut(in PointF cutP1, in PointF cutP2, in PointF target) {
            bool isInLine = IsPointOnLine(cutP1, cutP2, target);
            if (!isInLine) {
                return false;
            }

            return target.X <= Math.Max(cutP1.X, cutP2.X) && target.X >= Math.Min(cutP1.X, cutP2.X);
        }
        /// <summary>
        /// Вернёт true, если точка лежит на заданной прямой.
        /// </summary>
        internal static bool IsPointOnLine(in PointF cutP1, in PointF cutP2, in PointF target) {
            #region Человеческий вид
            //float a = p2.X - p1.X;
            //float b = p2.Y - p1.Y;
            //float c = target.X - p1.X;
            //float d = target.X - p1.X;

            //int e = Math.Sign(a * d - b * c);
            //if (e != 0) {
            //    return false;
            //}
            #endregion

            return ((cutP2.X - cutP1.X) * (target.Y - cutP1.Y) - (cutP2.Y - cutP1.Y) * (target.X - cutP1.X)) == 0;
        }
        /// <summary>
        /// Возвращает последовательные вершины прямоугольника, образованного "перпендикулярным" сдвигом отрезка на интервал
        /// в обе стороны.
        /// </summary>
        internal static PointF[] FindCutArea(in PointF p1, in PointF p2, in float interval) {
            float cutLength = MyGeometry.FindLengthBetweenPoints(p1, p2);
            float z = (p2.X - p1.X) * interval / cutLength;
            float a = (p2.Y - p1.Y) * interval / cutLength;
            PointF[] rect = {
                new PointF(p1.X - a, p1.Y + z),
                new PointF(p1.X + a, p1.Y - z),
                new PointF(p2.X + a, p2.Y - z),
                new PointF(p2.X - a, p2.Y + z),
            };

            return rect;
        }
        /// <summary> Возвращает false, если точка лежит за пределами области. </summary>
        /// <param name="area">Замкнутый выпуклый полигон с последовательными вершинами</param>
        internal static bool IsPointInArea(in PointF point, PointF[] area) {
            if (area.Length < 2) {
                throw new Exception();
            }
            //Такое уже включает в себя проверка
            foreach (var apex in area) {
                if (apex == point) {
                    return true;
                }
            }

            //Здесь можно проще: как-то через бинарный поиск
            //По-моему, тут цикл на 2 можно увеличить и подключить процентик
            int last = area.Length - 1;
            int prelast = area.Length - 2;
            float a = area[last].X - area[prelast].X;
            float b = area[last].Y - area[prelast].Y;

            float c = area[0].X - area[prelast].X;
            float d = area[0].Y - area[prelast].Y;

            float e = point.X - area[prelast].X;
            float f = point.Y - area[prelast].Y;

            bool isOk = Math.Sign(a * d - b * c) == Math.Sign(a * f - b * e);
            if (!isOk) {
                return false;
            }

            a = area[0].X - area[last].X;
            b = area[0].Y - area[last].Y;

            c = area[1].X - area[last].X;
            d = area[1].Y - area[last].Y;

            e = point.X - area[last].X;
            f = point.Y - area[last].Y;

            isOk = Math.Sign(a * d - b * c) == Math.Sign(a * f - b * e);
            if (!isOk) {
                return false;
            }

            for (int i = 0; i < area.Length - 2; i++) {
                //Основная сторона
                a = area[i + 1].X - area[i].X;
                b = area[i + 1].Y - area[i].Y;
                //Внутренняя сторона
                c = area[i + 2].X - area[i].X;
                d = area[i + 2].Y - area[i].Y;
                //Вектор от стороны к точке
                e = point.X - area[i].X;
                f = point.Y - area[i].Y;

                //Вращение стороны к следующей стороне (всегда вовнутрь) должно быть равно этому же вращению стороны к вектору
                isOk = Math.Sign(a * d - b * c) == Math.Sign(a * f - b * e);
                if (!isOk) {
                    return false;
                }
            }

            return true;
        }
        /// <summary>
        /// Находит, выше (1) точка прямой, лежит на ней (0) или ниже (-1). Если прямая вертикальна или координаты её точек идентичны - exeption.
        /// </summary>
        /// <param name="p1">
        /// Координаты первой точки прямой
        /// </param>
        /// <param name="p2">
        /// Коордианы второй точки прямой</param>
        /// <param name="p3">
        /// Координаты точки
        /// </param>
        internal static int IsPointOverLine(in PointF p1, in PointF p2, in PointF p3) {
            #region Неоптимальный способ
            //return point.Y > ((point.X - p1.X) * (p2.Y - p1.Y) + point.Y * (p2.X - p1.X))/(p2.X - p1.X);
            #endregion

            //Если точки совпадают или прямая вертикальна
            if (p1.X == p2.X) {
                throw new Exception();
            }

            #region Человеческий вид
            //Single a = p2.X - p1.X;
            //Single b = -p2.Y - -p1.Y;
            //Single c = p3.X - p1.X;
            //Single d = -p3.Y - -p1.Y;

            //Int32 e = Math.Sign(a * d - b * c);

            //if (a > 0) {
            //    return e;
            //}
            //else {
            //    return -e;
            //}
            #endregion

            float a = p2.X - p1.X;
            if (a > 0) {
                return Math.Sign((-a * (p3.Y - p1.Y)) + (p2.Y - p1.Y) * (p3.X - p1.X));
            }
            else {
                return -Math.Sign((-a * (p3.Y - p1.Y)) + (p2.Y - p1.Y) * (p3.X - p1.X));
            }
        }
        /// <summary>
        /// Возвращает левый верхний угол прямоугольника с горизональной гранью по координатам двух точек
        /// </summary>
        /// <param name="p1">Первая точка</param>
        /// <param name="p2">Вторая точка</param>
        internal static PointF FindLeftUpCornerCoord(in PointF p1, in PointF p2) => FindLeftUpCornerCoord(p1.X, p1.Y, p2.X, p2.Y);
        /// <summary>
        /// Возвращает левый верхний угол прямоугольника с горизональной гранью по координатам двух точек
        /// </summary>
        internal static PointF FindLeftUpCornerCoord(in float x1, in float y1, in float x2, in float y2) {
            float lowX = x1 > x2 ? x2 : x1;
            float lowY = y1 > y2 ? y2 : y1;
            return new PointF(lowX, lowY);
        }
        /// <summary>
        /// Обрубает координатный прямоугольник до квадрата относительно первой точки
        /// </summary>
        internal static (PointF, PointF) CutCoordinatesRectangleToSquare(in float x1, in float y1, in float x2, in float y2) {
            var p1 = new PointF(x1, y1);
            var p2 = new PointF(x2, y2);

            float a = Math.Abs(p2.X - p1.X);
            float b = Math.Abs(p2.Y - p1.Y);
            if (Math.Abs(a) > Math.Abs(b)) {
                p2.X = x1 > x2 ? p1.X - b : p1.X + b;
            }
            else {
                p2.Y = y1 > y2 ? p1.Y - a : p1.Y + a;
            }
            return (p1, p2);
        }
        /// <summary>
        /// Находит расстояние между двумя точками.
        /// </summary>
        internal static float FindLengthBetweenPoints(in PointF p1, in PointF p2) {
            return (float)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
        /// <summary> Вернёт точку пересечения перпендикулярных прямых. </summary>
        /// <param name="p1"> Первая точка первой прямой </param>
        /// <param name="p2"> Вторая точка первой прямой </param>
        /// <param name="p3"> Первая точка второй прямой </param>
        /// <returns></returns>
        internal static PointF MakePointProjectionOnLine(in PointF p1, in PointF p2, in PointF p3) {
            float a = p2.X - p1.X;
            float b = p2.Y - p1.Y;
            float denominator = a * a + b * b;
            float numerator1 = p3.X * (p3.Y - a) - p3.Y * (b + p3.X);
            float numerator2 = p1.X * p2.Y - p1.Y * p2.X;
            float x = (b * numerator2 - a * numerator1) / denominator;
            float y = (a * numerator2 + b * numerator1) / -denominator;
            return new PointF(x, y);
        }
        /// <summary> 
        /// Находит точку пересечения прямых. Exception, если они параллельны.
        /// </summary>
        internal static PointF FindCross(in PointF p1, in PointF p2, in PointF p3, in PointF p4) {
            //параллельны/что-то совпадает
            bool isParallel = MyGeometry.IsLinesParallel(p1, p2, p3, p4);
            if (isParallel) {
                throw new Exception();
            }

            float y = ((p4.X * p3.Y - p3.X * p4.Y) * (p1.Y - p2.Y) - (p3.Y - p4.Y) * (p2.X * p1.Y - p1.X * p2.Y)) / ((p3.Y - p4.Y) * (p1.X - p2.X) + (p4.X - p3.X) * (p1.Y - p2.Y));
            float x;
            if (p1.Y - p2.Y == 0) {
                x = (y * (p3.X - p4.X) + (p4.X * p3.Y - p3.X * p4.Y)) / (p3.Y - p4.Y);
            }
            else {
                x = (y * (p1.X - p2.X) + (p2.X * p1.Y - p1.X * p2.Y)) / (p1.Y - p2.Y);
            }

            return new PointF(x, y);
        }
        /// <summary>
        /// Вычитает из экземпляра соответствующие координаты заданной <see cref="PointF"/>.
        /// </summary>
        internal static PointF Substract(this PointF p1, in PointF p2) => new PointF(p1.X - p2.X, p1.Y - p2.Y);
        /// <summary>
        /// Складывает из экземпляра и заданной <see cref="PointF"/> соответствующие координаты.
        /// </summary>
        internal static PointF Sum(this PointF p1, in PointF p2) => new PointF(p1.X + p2.X, p1.Y + p2.Y);
        /// <summary>
        /// Вернёт точку с помноженными координатами на заданное число.
        /// </summary>
        internal static PointF Multiply(this PointF p1,in float factor) => new PointF(p1.X * factor, p1.Y * factor);

    }
}
