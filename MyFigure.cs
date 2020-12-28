using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CAD_Client.ToolEnum;
namespace CAD_Client {
    /// <summary>
    /// Представляет класс отображающихся фигур. Содержат в себе все необходимые геометрические параметры и характеристики
    /// цветопредставления. Локация содержит верхнюю левую точку описывающего горизонтального прямоугольника фигуру. При
    /// задании локации все соответствующие геометрические параметры сдвигаются, т.к. считаются не относительно локации.
    /// </summary>
    //???Отличие Private protected от public в internal классе?
    internal abstract class MyFigure : IDisposable {
        internal static int FiguresCount { private set; get; }
        internal int Id { private set; get; }

        internal float X { private protected set; get; }
        internal float Y { private protected set; get; }
        /// <summary>
        /// Верхняя левая точка описывающего квадрата. При изменении должна перемещаться вся фигура.
        /// </summary>
        internal PointF Location {
            private protected set {
                if (value.X != X || value.Y != Y) {
                    X = value.X;
                    Y = value.Y;
                }
            }
            get {
                return new PointF(X, Y);
            }
        }

        private protected Pen pen;
        internal Pen Pen {
            set {
                if (value == null) {
                    throw new ArgumentNullException();
                }
                if (value != pen) {
                    pen = value;
                }
            }
            //??? pen имеет модификатор доступности блока set private, хотя по факту общедоступен.
            //Решено: pen можно менять как угодно, но присвоить ему null невозможно, так что ошибки исключены.
            get => pen;
        }
        //!!!MyFigure#05: проверять все свойства-классы на null
        internal Pen SelectedPen { set; get; } = new Pen(Color.White, 2);
        internal Pen HightLightedPen { set; get; } = new Pen(Color.BlueViolet, 2);
        internal bool IsSelected { set; get; }
        internal bool IsHightLighed { set; get; }
        internal bool IsFill { set; get; }
        internal bool IsHide { set; get; }
        internal Color FillColor { set; get; }



        private protected MyFigure() : this(Color.Black) { }
        private protected MyFigure(Pen pen) {
            Pen = pen ?? throw new ArgumentNullException();

            FiguresCount++;
            Id = FiguresCount;
        }
        private protected MyFigure(Color color) : this(new Pen(color, 1)) { }



        //!!!MyFigure#81: реализовать удаление фигуры и присваивание ID.
        public void Dispose() { }


        /// <summary>
        /// Присвоит локации фигуры верхнюю левую точку описывающего горизонтального прямоугольника фигуру.
        /// Используется при изменении геометрических параметров.
        /// </summary>
        private protected abstract void FindLocation();


        #region Прорисовка
        /// <summary>
        /// Визуализирует фигуру на заданном <see cref="Graphics"/> относительно реальных координат.
        /// </summary>
        /// <para>P-></para>
        /// <param name="graphicsCenterInRealCoord"> Координаты верхнего левого пикселя <see cref="Graphics"/> на реальной плоскости. </param>
        internal virtual void Draw(Graphics screen, PointF graphicsCenterInRealCoord) {
            if (IsHide) {
                return;
            }

            ChoosePen(out Pen pen);

            Display(screen, pen, graphicsCenterInRealCoord);
        }
        private protected virtual Pen ChoosePen(out Pen pen) {
            if (IsSelected) {
                pen = SelectedPen;
            }
            else
            if (IsHightLighed) {
                pen = HightLightedPen;
            }
            else {
                pen = Pen;
            }
            return pen;
        }
        /// <summary>
        /// Визуализирует фигуру на заданном <see cref="Graphics"/> относительно реальных координат.
        /// </summary>
        /// <param name="graphicsLocationInRealCoord"> Левый верхний пиксель заданного <see cref="Graphics"/> в реальных координатах. </param>
        private protected abstract void Display(Graphics screen, Pen pen, PointF graphicsLocationInRealCoord);
        /// <summary>
        /// Вернёт точку реальных координат, выраженных в графические.
        /// <para>R->P</para>
        /// </summary>
        /// <param name="graphicsLocationInRealCoord"> Левый верхний пиксель заданного <see cref="Graphics"/> в реальных координатах. </param>
        private protected static PointF ToGraphicsCoord(PointF graphicsLocationInRealCoord, PointF pointInRealCoord) => pointInRealCoord.Substract(graphicsLocationInRealCoord);
        #endregion


        #region API
        internal string GetDescription() {
            return ToolEnum.GetDescription(this);
        }


        /// <summary>
        /// Переместит все геометрическе параметры в заданное положение.
        /// <para>R-></para>
        /// </summary>
        internal abstract void Move(PointF newLocation);
        /// <summary>
        /// Переместит все геометрическе параметры в заданное положение.
        /// <para>R-></para>
        /// </summary>
        internal virtual void Move(float x, float y) => Move(new PointF(x, y));
        #endregion

    }
}
