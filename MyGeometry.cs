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
        public static PointF FindLeftUpCornerCoord(Single x1, Single y1, Single x2, Single y2) {
            Single lowX = x1 > x2 ? x2 : x1;
            Single lowY = y1 > y2 ? y2 : y1;
            return new PointF(lowX, lowY);
        }
        public static PointF FindLeftUpCornerCoord(in PointF p1, in PointF p2) => FindLeftUpCornerCoord(p1.X, p1.Y, p2.X, p2.Y);
        /// <summary>
        /// Обрубает координатный прямоугольник до квадрата относительно первой точки
        /// </summary>
        public static (PointF, PointF) CutCoordinatesRectangleToSquare(Single x1, Single y1, Single x2, Single y2) {
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
        public static Single FindLength(PointF p1, PointF p2) {
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
    }
}
