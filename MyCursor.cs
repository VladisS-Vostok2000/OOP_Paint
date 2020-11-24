using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_Paint {
    public sealed class MyCursor {
        public Boolean IsSnapped { get; set; }
        private Point SnapLocation;
        public Int32 SnapDistance { get; set; }
        private Int32 cumulateX;
        private Int32 cumulateY;



        public MyCursor(Int32 snapDistance) {
            SnapDistance = snapDistance;
        }



        public event EventHandler SnapTorned;



        public void DoSnap(Point target) {
            IsSnapped = true;
            SnapLocation = target;
            Cursor.Position = target;
            Debugger.Log($"SnapCreated: ({target.X};{target.Y})");
            Debugger.Log($"Mouse new Location: ({Cursor.Position.X};{Cursor.Position.Y})");
        }
        public void ContinueSnap(Point target) {
            if (!IsSnapped) {
                throw new Exception();
            }
            Debugger.Log("SnapContinue");
            Debugger.Log($"Snap cumulateX: {cumulateX}");
            Debugger.Log($"Snap cumulateY: {cumulateY}");
            Debugger.Log($"MouseLocation: ({Cursor.Position.X};{Cursor.Position.Y})");
            cumulateX += SnapLocation.X - target.X;
            cumulateY += SnapLocation.Y - target.Y;
            Debugger.Log($"Snap cumulateX now: {cumulateX}");
            Debugger.Log($"Snap cumulateY now: {cumulateY}");
            Cursor.Position = SnapLocation;
            Debugger.Log($"Mouse new Location: ({Cursor.Position.X};{Cursor.Position.Y})");

            if (Math.Abs(cumulateX) > SnapDistance || Math.Abs(cumulateY) > SnapDistance) {
                StopSnap();
            }
        }


        public void StopSnap() {
            IsSnapped = false;
            Cursor.Position = new Point(Cursor.Position.X - cumulateX, Cursor.Position.Y - cumulateY);
            cumulateX = 0;
            cumulateY = 0;
            Debugger.Log($"Snap torned\r\n");
            SnapTorned.Invoke(this, EventArgs.Empty);
        }

    }
}
