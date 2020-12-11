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
using static OOP_Paint.Debugger;
using System.IO;

namespace OOP_Paint {
    public static class MyGeometry {
        /// <summary>
        /// Возвращает левый верхний угол прямоугольника с горизональной гранью по координатам двух точек
        /// </summary>
        /// <param name="p1">Первая точка</param>
        /// <param name="p2">Вторая точка</param>
        public static PointF FindLeftUpCornerCoord(in Single x1, in Single y1, in Single x2, in Single y2) {
            Single lowX = x1 > x2 ? x2 : x1;
            Single lowY = y1 > y2 ? y2 : y1;
            return new PointF(lowX, lowY);
        }
        public static PointF FindLeftUpCornerCoord(in PointF p1, in PointF p2) => FindLeftUpCornerCoord(p1.X, p1.Y, p2.X, p2.Y);
        /// <summary>
        /// Обрубает координатный прямоугольник до квадрата относительно первой точки
        /// </summary>
        public static (PointF, PointF) CutCoordinatesRectangleToSquare(in Single x1, in Single y1, in Single x2, in Single y2) {
            var p1 = new PointF(x1, y1);
            var p2 = new PointF(x2, y2);

            Single a = Math.Abs(p2.X - p1.X);
            Single b = Math.Abs(p2.Y - p1.Y);
            if (Math.Abs(a) > Math.Abs(b)) {
                p2.X = x1 > x2 ? p1.X - b : p1.X + b;
            }
            else {
                p2.Y = y1 > y2 ? p1.Y - a : p1.Y + a;
            }
            return (p1, p2);
        }
        /// <summary>
        /// Находит длину между двумя точками
        /// </summary>
        public static Single FindLength(in PointF p1, in PointF p2) {
            return (Single)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
        /// <summary> Вернёт точку пересечения перпендикулярных прямых. </summary>
        /// <param name="p1"> Первая точка первой прямой </param>
        /// <param name="p2"> Вторая точка первой прямой </param>
        /// <param name="p3"> Первая точка второй прямой </param>
        /// <returns></returns>
        public static PointF MakePointProjectionOnLine(in PointF p1, in PointF p2, in PointF p3) {
            Single a = p2.X - p1.X;
            Single b = p2.Y - p1.Y;
            Single denominator = a * a + b * b;
            Single numerator1 = p3.X * (p3.Y - a) - p3.Y * (b + p3.X);
            Single numerator2 = p1.X * p2.Y - p1.Y * p2.X;
            Single x = (b * numerator2 - a * numerator1) / denominator;
            Single y = (a * numerator2 + b * numerator1) / -denominator;
            return new PointF(x, y);
        }
        /// <summary>
        /// Возвращает последовательные вершины прямоугольника, образованного "перпендикулярным" сдвигом отрезка на интервал
        /// в обе стороны.
        /// </summary>
        public static PointF[] FindCutArea(in PointF p1, PointF p2, Single interval) {
            Single cutLength = MyGeometry.FindLength(p1, p2);
            Single z = (p2.X - p1.X) * interval / cutLength;
            Single a = (p2.Y - p1.Y) * interval / cutLength;
            PointF[] rect = {
                new PointF(p1.X - a, p1.Y + z),
                new PointF(p1.X + a, p1.Y - z),
                new PointF(p2.X + a, p2.Y - z),
                new PointF(p2.X - a, p2.Y + z),
            };

            return rect;
        }
        /// <summary> Находит, выше (1) точка прямой, лежит на ней (0) или ниже (-1). Если прямая вертикальна или координаты её точек идентичны - exсeption. </summary>
        /// <param name="p1"> Координаты первой точки прямой </param>
        /// <param name="p2"> Коордианы второй точки прямой</param>
        /// <param name="p3"> Координаты точки </param>
        /// <exception cref="Exception"> Точки совпадают или прямая вертикальна </exception>
        public static Int32 IsPointOverLine(in PointF p1, in PointF p2, in PointF p3) {
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

            Single a = p2.X - p1.X;
            if (a > 0) {
                return Math.Sign((-a * (p3.Y - p1.Y)) + (p2.Y - p1.Y) * (p3.X - p1.X));
            }
            else {
                return -Math.Sign((-a * (p3.Y - p1.Y)) + (p2.Y - p1.Y) * (p3.X - p1.X));
            }
        }
        /// <summary> Возвращает false, если точка лежит за пределами области. </summary>
        /// <param name="area"> Замкнутый выпуклый полигон с последовательными вершинами </param>
        public static Boolean IsPointInArea(in PointF point, in PointF[] area) {
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
            Int32 last = area.Length - 1;
            Int32 prelast = area.Length - 2;
            Single a = area[last].X - area[prelast].X;
            Single b = area[last].Y - area[prelast].Y;

            Single c = area[0].X - area[prelast].X;
            Single d = area[0].Y - area[prelast].Y;

            Single e = point.X - area[prelast].X;
            Single f = point.Y - area[prelast].Y;

            Boolean isOk = Math.Sign(a * d - b * c) == Math.Sign(a * f - b * e);
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

            for (Int32 i = 0; i < area.Length - 2; i++) {
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
        /// <summary> True, если точка лежит на заданной прямой </summary>
        /// <param name="cutP1"> Первая точка прямой </param>
        /// <param name="cutP2"> Вторая точка прямой </param>
        public static Boolean CheckIsPointOnLine(in PointF cutP1, in PointF cutP2, in PointF target) {
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
        /// True, если точка лежит на отрезке
        /// </summary>
        /// <param name="cutP1"></param>
        /// <param name="cutP2"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Boolean CheckIsPointOnCut(in PointF cutP1, in PointF cutP2, in PointF target) {
            Boolean isInLine = CheckIsPointOnLine(cutP1, cutP2, target);
            if (!isInLine) {
                return false;
            }

            return target.X <= Math.Max(cutP1.X, cutP2.X) && target.X >= Math.Min(cutP1.X, cutP2.X);
        }
        /// <summary> Определяет параллельность/коллинеарность отрезков </summary>
        public static Boolean AreLinesParallel(in PointF p1, in PointF p2, in PointF p3, in PointF p4) {
            //Если отношения смещений на клетку х и у двух отрезков по модулю равны, то они параллельны (k коэфф один)
            //И по свойству пропорции:
            if (Math.Abs((p1.X - p2.X) * (p3.Y - p4.Y)) == Math.Abs((p1.Y - p2.Y) * (p3.X - p4.X))) {
                return true;
            }

            return false;
        }
        /// <summary> Находит точку пересечения прямых. Exception, если они параллельны. </summary>
        public static PointF FindCross(in PointF p1, in PointF p2, in PointF p3, in PointF p4) {
            //параллельны/что-то совпадает
            Boolean isParallel = MyGeometry.AreLinesParallel(p1, p2, p3, p4);
            if (isParallel) {
                throw new Exception();
            }

            Single y = ((p4.X * p3.Y - p3.X * p4.Y) * (p1.Y - p2.Y) - (p3.Y - p4.Y) * (p2.X * p1.Y - p1.X * p2.Y)) / ((p3.Y - p4.Y) * (p1.X - p2.X) + (p4.X - p3.X) * (p1.Y - p2.Y));
            Single x;
            if (p1.Y - p2.Y == 0) {
                x = (y * (p3.X - p4.X) + (p4.X * p3.Y - p3.X * p4.Y)) / (p3.Y - p4.Y);
            }
            else {
                x = (y * (p1.X - p2.X) + (p2.X * p1.Y - p1.X * p2.Y)) / (p1.Y - p2.Y);
            }

            return new PointF(x, y);
        }
        /// <summary> По двум точкам находит ближайшую горизонтальную, вертикальную или диагональную прямую, проходящие через первую точку. </summary>
        /// <returns> Точка, лежащая на ближайшей прямой с направлением второй точки. </returns>
        public static PointF ChoosePolarLine(in PointF p1, in PointF p2) {
            Single a = p2.X - p1.X;
            Single b = p2.Y - p1.Y;
            Single k = b / a;

            //Вторая точка уже лежит на прямой
            if (k == 0 || Math.Abs(k) == 1 || Single.IsInfinity(k)) {
                return new PointF(p1.X + a, p1.Y + b);
            }

            var p3 = new PointF(a, b);
            Single c;
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

            Single sqrt2 = (Single)Math.Sqrt(2);
            var p5 = new PointF(Math.Sign(a) * Math.Abs(c) / sqrt2, Math.Sign(b) * Math.Abs(c) / sqrt2);

            Boolean isDiagonalCloser = Math.Abs(a * p4.X + b * p4.Y) <= a * p5.X + b * p5.Y;
            if (isDiagonalCloser) {
                return new PointF(p1.X + p5.X, p1.Y + p5.Y);
            }
            else {
                return new PointF(p1.X + p4.X, p1.Y + p4.Y);
            }

        }

    }
}
