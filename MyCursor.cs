using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CAD_Client {
    internal sealed class MyCursor {
        internal bool IsSnapped { private set; get; }
        internal Point SnapLocation { private set; get; }
        internal int SnapDistance { private set; get; }
        internal Point Cumulate { private set; get; }

        internal event EventHandler SnapTorned;



        /// <summary>
        /// Привязывает курсор к заданной экранной координате.
        /// </summary>
        /// <param name="target"></param>
        internal void DoSnap(Point target, int snapDistance) {
            if (IsSnapped) {
                StopSnap();
            } else {
                IsSnapped = true;
            }

            SnapLocation = target;
            SnapDistance = snapDistance;
            Cursor.Position = target;
        }
        /// <summary>
        /// Вернёт курсор на <see cref="SnapLocation"/>. Смещение мыши будет записано в <see cref="Cumulate"/>.
        /// </summary>
        /// <param name="target"></param>
        internal void ContinueSnap(Point target) {
            if (!IsSnapped) {
                throw new Exception();
            }

            //???Попался на Structure - evil. Если мы изменяем поле структуры, она неявно создаётся и присваивается сама себе с новым полем?
            Cumulate = new Point(Cumulate.X + SnapLocation.X - target.X, Cumulate.Y + SnapLocation.Y - target.Y);
            Cursor.Position = SnapLocation;

            if (Math.Abs(Cumulate.X) > SnapDistance || Math.Abs(Cumulate.Y) > SnapDistance) {
                StopSnap();
            }
        }
        /// <summary>
        /// Очистит данные от предыдущей привязки.
        /// </summary>
        private void ClearSnap() {
            Cumulate = new Point();
        }
        /// <summary>
        /// Разрывает привязку курсора.
        /// </summary>
        /// <param name="jumpCumulate"> Следует ли курсору переместиться на накопленные координаты смещения. </param>
        internal void StopSnap(bool jumpCumulate = true) {
            IsSnapped = false;
            if (jumpCumulate == true) {
                Cursor.Position = new Point(Cursor.Position.X - Cumulate.X, Cursor.Position.Y - Cumulate.Y);
            }
            ClearSnap();
            SnapTorned.Invoke(this, EventArgs.Empty);
        }

    }
}
