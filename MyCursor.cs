using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_Paint {
    public sealed class MyCursor {
        //MyCursor#1: Исправить Snap: разрывается непрерывно
        public bool IsSnapped { get; set; }
        private Point SnapLocation;
        public int SnapDistance { get; set; }
        private int cumulateX;
        private int cumulateY;



        public MyCursor(int snapDistance) {
            SnapDistance = snapDistance;
        }



        public event EventHandler SnapTorned;



        public void DoSnap(Point target) {
            IsSnapped = true;
            SnapLocation = target;
            Cursor.Position = target;
        }
        public void ContinueSnap(Point target) {
            //???
            if (!IsSnapped) {
                throw new Exception();
            }

            cumulateX += SnapLocation.X - target.X;
            cumulateY += SnapLocation.Y - target.Y;

            if (Math.Abs(cumulateX) > SnapDistance || Math.Abs(cumulateY) > SnapDistance) {
                IsSnapped = false;
                SnapTorned.Invoke(this, EventArgs.Empty);
            }
        }

    }
}
