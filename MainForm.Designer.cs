namespace OOP_Paint {
    partial class MainForm {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.MainFromPctrbxScreen = new System.Windows.Forms.PictureBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.MainFormTmr = new System.Windows.Forms.Timer(this.components);
            this.MainFormSttsstp = new System.Windows.Forms.StatusStrip();
            this.MainFormStttsstpLblHint = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.MainFormSttsstpLblMouseX = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.MainFormSttsstpLblMouseY = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.MainFromPctrbxScreen)).BeginInit();
            this.MainFormSttsstp.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainFromPctrbxScreen
            // 
            this.MainFromPctrbxScreen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.MainFromPctrbxScreen.Location = new System.Drawing.Point(13, 13);
            this.MainFromPctrbxScreen.Name = "MainFromPctrbxScreen";
            this.MainFromPctrbxScreen.Size = new System.Drawing.Size(496, 290);
            this.MainFromPctrbxScreen.TabIndex = 0;
            this.MainFromPctrbxScreen.TabStop = false;
            this.MainFromPctrbxScreen.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainFormSttsstp_MouseMove);
            this.MainFromPctrbxScreen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainFromPctrbxScreen_MouseUp);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(516, 13);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(272, 290);
            this.listBox1.TabIndex = 7;
            // 
            // MainFormTmr
            // 
            this.MainFormTmr.Enabled = true;
            this.MainFormTmr.Interval = 20;
            this.MainFormTmr.Tick += new System.EventHandler(this.MainFormTmr_Tick);
            // 
            // MainFormSttsstp
            // 
            this.MainFormSttsstp.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MainFormStttsstpLblHint,
            this.toolStripStatusLabel1,
            this.MainFormSttsstpLblMouseX,
            this.toolStripStatusLabel3,
            this.MainFormSttsstpLblMouseY});
            this.MainFormSttsstp.Location = new System.Drawing.Point(0, 428);
            this.MainFormSttsstp.Name = "MainFormSttsstp";
            this.MainFormSttsstp.Size = new System.Drawing.Size(800, 22);
            this.MainFormSttsstp.TabIndex = 10;
            this.MainFormSttsstp.Text = "statusStrip1";
            // 
            // MainFormStttsstpLblHint
            // 
            this.MainFormStttsstpLblHint.AutoSize = false;
            this.MainFormStttsstpLblHint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.MainFormStttsstpLblHint.Name = "MainFormStttsstpLblHint";
            this.MainFormStttsstpLblHint.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.MainFormStttsstpLblHint.Size = new System.Drawing.Size(250, 17);
            this.MainFormStttsstpLblHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(17, 17);
            this.toolStripStatusLabel1.Text = "X:";
            // 
            // MainFormSttsstpLblMouseX
            // 
            this.MainFormSttsstpLblMouseX.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.MainFormSttsstpLblMouseX.Name = "MainFormSttsstpLblMouseX";
            this.MainFormSttsstpLblMouseX.Size = new System.Drawing.Size(25, 17);
            this.MainFormSttsstpLblMouseX.Text = "888";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(17, 17);
            this.toolStripStatusLabel3.Text = "Y:";
            // 
            // MainFormSttsstpLblMouseY
            // 
            this.MainFormSttsstpLblMouseY.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.MainFormSttsstpLblMouseY.Name = "MainFormSttsstpLblMouseY";
            this.MainFormSttsstpLblMouseY.Size = new System.Drawing.Size(25, 17);
            this.MainFormSttsstpLblMouseY.Text = "888";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.MainFormSttsstp);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.MainFromPctrbxScreen);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.MainFromPctrbxScreen)).EndInit();
            this.MainFormSttsstp.ResumeLayout(false);
            this.MainFormSttsstp.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox MainFromPctrbxScreen;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Timer MainFormTmr;
        private System.Windows.Forms.StatusStrip MainFormSttsstp;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel MainFormSttsstpLblMouseX;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel MainFormSttsstpLblMouseY;
        private System.Windows.Forms.ToolStripStatusLabel MainFormStttsstpLblHint;
    }
}

