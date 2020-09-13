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
            this.MainFromPctrbxScreen = new System.Windows.Forms.PictureBox();
            this.MainFormCmbbxFigureChoose = new System.Windows.Forms.ComboBox();
            this.MainFormLblFigureChoose = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.MainFromPctrbxScreen)).BeginInit();
            this.SuspendLayout();
            // 
            // MainFromPctrbxScreen
            // 
            this.MainFromPctrbxScreen.BackColor = System.Drawing.Color.White;
            this.MainFromPctrbxScreen.Location = new System.Drawing.Point(13, 13);
            this.MainFromPctrbxScreen.Name = "MainFromPctrbxScreen";
            this.MainFromPctrbxScreen.Size = new System.Drawing.Size(775, 297);
            this.MainFromPctrbxScreen.TabIndex = 0;
            this.MainFromPctrbxScreen.TabStop = false;
            // 
            // MainFormCmbbxFigureChoose
            // 
            this.MainFormCmbbxFigureChoose.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MainFormCmbbxFigureChoose.FormattingEnabled = true;
            this.MainFormCmbbxFigureChoose.Items.AddRange(new object[] {
            "Круг",
            "Прямоугольник"});
            this.MainFormCmbbxFigureChoose.Location = new System.Drawing.Point(12, 329);
            this.MainFormCmbbxFigureChoose.Name = "MainFormCmbbxFigureChoose";
            this.MainFormCmbbxFigureChoose.Size = new System.Drawing.Size(150, 21);
            this.MainFormCmbbxFigureChoose.TabIndex = 0;
            this.MainFormCmbbxFigureChoose.SelectionChangeCommitted += new System.EventHandler(this.MainFormCmbbxFigureChoose_SelectionChangeCommitted);
            // 
            // MainFormLblFigureChoose
            // 
            this.MainFormLblFigureChoose.AutoSize = true;
            this.MainFormLblFigureChoose.Location = new System.Drawing.Point(12, 313);
            this.MainFormLblFigureChoose.Name = "MainFormLblFigureChoose";
            this.MainFormLblFigureChoose.Size = new System.Drawing.Size(81, 13);
            this.MainFormLblFigureChoose.TabIndex = 2;
            this.MainFormLblFigureChoose.Text = "Выбор фигуры";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.MainFormLblFigureChoose);
            this.Controls.Add(this.MainFormCmbbxFigureChoose);
            this.Controls.Add(this.MainFromPctrbxScreen);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.MainFromPctrbxScreen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox MainFromPctrbxScreen;
        private System.Windows.Forms.ComboBox MainFormCmbbxFigureChoose;
        private System.Windows.Forms.Label MainFormLblFigureChoose;
    }
}

