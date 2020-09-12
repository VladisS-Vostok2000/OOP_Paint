using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_Paint {
    public partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e) {
            Screen = MainFromPctrbxScreen.CreateGraphics();
        }
        private void Form1_Shown(object sender, EventArgs e) {
        }
        private Graphics Screen;


    }
}
