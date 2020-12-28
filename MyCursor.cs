using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace CAD_Client {
    //MyCursor: разделить длину создания привязки и силу, с которой нужно вытянуть курсор (Cumulate).
    //MyCursor: превратить в статический.
    //???Сделать статическим?
    //->Этот класс использует статический курсор, что может негативно отразиться при создании нескольких объектов.
    
    //???Мне нужна причина, зачем MyCursor задан в экранных координатах.
    //->Cursor.Position использует экранные координаты.
    //???Не лучше бы тогда использовать клиентские, а конкретно для курсора PointToScreen? Рассмотреть вопрос, можно ли инкапсулировать курсор в форму.
    internal sealed class MyCursor {
        internal bool Snapped { private set; get; }
        private Point snapLocation;
        internal Point SnapLocation {
            private set {
                if (value != snapLocation) {
                    snapLocation = value;
                }
            }
            get {
                if (Snapped) {
                    return snapLocation;
                }
                else throw new Exception("Привязка отсутствует, но запрошено её расположение");
            }
        }
        internal int SnapDistance { private set; get; }
        internal Point Offset { private set; get; }
        //????Удалить isStatic и всё, что с этим связано.
        /// <summary>
        /// Для привязки ведётся накопление смещений <see cref="Offset"/>?
        /// </summary>
        private bool isStatic;
        private bool jumpCumulate;

        internal event EventHandler SnapTorned;



        /// <summary>
        /// Привязывает курсор к заданному экранному пикселю. По накопленному смещению включительно большему
        /// заданному интервалу привязка будет разорвана.
        /// <para>S-></para>
        /// </summary>
        /// <param name="target"> Расположение мыши в экранных координатах. </param>
        /// <param name="jumpCumulate"> Следует ли переместить мышь на её накопленные смещения после разрыва привязки. </param>
        internal void CreateSnap(in Point target, in int snapDistance, in bool jumpCumulate) {
            Debugger.Log($"Начат CreateSnap");
            Debugger.Log($"target: ({target.X}; {target.Y})");
            if (Snapped) {
                StopSnap();
            }
            else {
                Snapped = true;
            }

            isStatic = false;
            this.jumpCumulate = jumpCumulate;
            SnapLocation = target;
            Cursor.Position = target;
            Debugger.Log($"SnapLocation, Cursor.Position: ({target.X}; {target.Y})");
            SnapDistance = snapDistance;
        }
        /// <summary>
        /// Привязывает курсор к заданному экранному пикселю. Для разрыва используйте <see cref="StopSnap"/>.
        /// Последнее смещение будет записано в <see cref="Offset"/>.
        /// <para>S-></para>
        /// </summary>
        /// <param name="target"> Расположение мыши в экранных координатах. </param>
        internal void CreateSnap(in Point target) {
            Debugger.Log($"Начат CreateSnap");
            Debugger.Log($"target: ({target.X}; {target.Y})");
            if (Snapped) {
                StopSnap();
            }
            else {
                Snapped = true;
            }

            SnapLocation = target;
            Cursor.Position = target;
            Debugger.Log($"SnapLocation, Cursor.Position: ({target.X}; {target.Y})");
        }
        /// <summary>
        /// Вернёт курсор на <see cref="SnapLocation"/> экранных координат. Заданное смещение мыши в экранных координатах будет записано в <see cref="Offset"/>.
        /// <para>S-></para>
        /// </summary>
        /// <param name="target">Расположение мыши в экранных координатах.</param>
        /// <exception cref="Exception">Метод вызван, но привязки в данный момент не существует.</exception>
        internal void ContinueSnap(in Point target) {
            Debugger.Log($"Начат ContinueSnap");
            Debugger.Log($"SnapPoLocation: ({SnapLocation.X}; {SnapLocation.Y})");
            Debugger.Log($"target: ({target.X}; {target.Y})");
            if (!Snapped) {
                throw new Exception("Привязки не существует, но вызыван.");
            }
            if (isStatic) {
                Offset = new Point(SnapLocation.X - target.X, SnapLocation.Y - target.Y);
                Debugger.Log($"Offset: ({Offset.X}; {Offset.Y})");
                Cursor.Position = SnapLocation;
                Debugger.Log($"newMouseScreenLocation: ({Cursor.Position.X}; {Cursor.Position.Y})");
            }
            else {
                //???Попался на Structure - evil. Если мы изменяем поле структуры, она неявно создаётся и присваивается сама себе с новым полем?
                //????
                //Cumulate = Cumulate.Sum(SnapLocation.Substract(target));
                Offset = new Point(Offset.X + SnapLocation.X - target.X, Offset.Y + SnapLocation.Y - target.Y);
                Debugger.Log($"Offset: ({Offset.X}; {Offset.Y})");

                if (Math.Abs(Offset.X) > SnapDistance || Math.Abs(Offset.Y) > SnapDistance) {
                    StopSnap();
                }
                else {
                    Cursor.Position = SnapLocation;
                    Debugger.Log($"newMouseScreenLocation: ({Cursor.Position.X}; {Cursor.Position.Y})");
                }
            }
        }
        /// <summary>
        /// Разрывает привязку курсора.
        /// </summary>
        /// <param name="jumpCumulate"> Следует ли курсору переместиться на накопленные координаты смещения. </param>
        internal void StopSnap() {
            Debugger.Log($"StopSnap\r\n");
            Snapped = false;
            if (jumpCumulate == true) {
                Cursor.Position = new Point(Cursor.Position.X - Offset.X, Cursor.Position.Y - Offset.Y);
            }
            //???Смещение может быть нулём и при присутствующей привязке. Следует определить Exception на свойство?
            Offset = new Point(0, 0);
            SnapTorned?.Invoke(this, EventArgs.Empty);
        }

    }
}
