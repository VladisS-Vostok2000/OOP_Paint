using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CAD_Client {
    //??? Сделать статическим?
    internal sealed class MyCursor {
        internal bool Snapped { private set; get; }
        internal Point SnapLocation { private set; get; }
        internal int SnapDistance { private set; get; }
        internal Point Cumulate { private set; get; }

        internal event EventHandler SnapTorned;



        /// <summary>
        /// Привязывает курсор к заданному пикселю.
        /// </summary>
        /// <param name="target">Расположение мыши в экранных координатах. </param>
        internal void CreateSnap(Point target, int snapDistance) {
            Debugger.Log($"Начат CreateSnap");
            Debugger.Log($"target: ({target.X}; {target.Y})");
            if (Snapped) {
                StopSnap();
            }
            else {
                Snapped = true;
            }

            Debugger.Log($"SnapLocation, Cursor.Position: ({target.X}; {target.Y})");
            SnapLocation = target;
            Cursor.Position = target;
            SnapDistance = snapDistance;
        }
        /// <summary>
        /// Вернёт курсор на <see cref="SnapLocation"/>. Смещение мыши будет записано в <see cref="Cumulate"/>.
        /// </summary>
        /// <param name="target">Расположение мыши в экранных координатах.</param>
        /// <exception cref="Exception">Метод вызван, но привязки в данный момент не существует.</exception>
        internal void ContinueSnap(Point target) {
            Debugger.Log($"Начат ContinueSnap");
            Debugger.Log($"SnapPoLocation: ({SnapLocation.X}; {SnapLocation.Y})");
            Debugger.Log($"target: ({target.X}; {target.Y})");
            if (!Snapped) {
                throw new Exception("Привязки не существует, но вызыван.");
            }

            //???Попался на Structure - evil. Если мы изменяем поле структуры, она неявно создаётся и присваивается сама себе с новым полем?
            //Cumulate = Cumulate.Sum(SnapLocation.Substract(target));
            Cumulate = new Point(Cumulate.X + SnapLocation.X - target.X, Cumulate.Y + SnapLocation.Y - target.Y);
            Debugger.Log($"Cumulate: ({Cumulate.X}; {Cumulate.Y})");

            if (Math.Abs(Cumulate.X) > SnapDistance || Math.Abs(Cumulate.Y) > SnapDistance) {
                StopSnap();
            }
            else {
                Cursor.Position = SnapLocation;
                Debugger.Log($"newMouseScreenLocation: ({Cursor.Position.X}; {Cursor.Position.Y})");
            }
        }
        /// <summary>
        /// Разрывает привязку курсора.
        /// </summary>
        /// <param name="jumpCumulate"> Следует ли курсору переместиться на накопленные координаты смещения. </param>
        internal void StopSnap(bool jumpCumulate = true) {
            Debugger.Log($"StopSnap\r\n");
            Snapped = false;
            if (jumpCumulate == true) {
                Cursor.Position = new Point(Cursor.Position.X - Cumulate.X, Cursor.Position.Y - Cumulate.Y);
            }
            Cumulate = new Point(0, 0);
            SnapTorned?.Invoke(this, EventArgs.Empty);
        }

    }
}
