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
            this.MainFormLstbxFigures = new System.Windows.Forms.ListBox();
            this.MainFormTmr = new System.Windows.Forms.Timer(this.components);
            this.MainFormSttsstp = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.MainFormSttsstpLblMouseX = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.MainFormSttsstpLblMouseY = new System.Windows.Forms.ToolStripStatusLabel();
            this.MainFormSttsstpLblHint = new System.Windows.Forms.ToolStripStatusLabel();
            this.MainFormBttnCircle = new System.Windows.Forms.Button();
            this.MainFormCmbbxBuildingVariants = new System.Windows.Forms.ComboBox();
            this.MainFormBttnRectangle = new System.Windows.Forms.Button();
            this.MainFormBttnCut = new System.Windows.Forms.Button();
            this.MainFormBttnSelect = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.MainFormBttnNothing = new System.Windows.Forms.Button();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.MainFormSttsstpLblMouseScreenX = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.MainFormSttsstpLblMouseScreenY = new System.Windows.Forms.ToolStripStatusLabel();
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
            this.MainFromPctrbxScreen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainFromPctrbxScreen_MouseDown);
            this.MainFromPctrbxScreen.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainFormPctrbxScreen_MouseMove);
            this.MainFromPctrbxScreen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainFromPctrbxScreen_MouseUp);
            // 
            // MainFormLstbxFigures
            // 
            this.MainFormLstbxFigures.Font = new System.Drawing.Font("Segoe Mono Boot", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MainFormLstbxFigures.FormattingEnabled = true;
            this.MainFormLstbxFigures.ItemHeight = 15;
            this.MainFormLstbxFigures.Location = new System.Drawing.Point(516, 13);
            this.MainFormLstbxFigures.Name = "MainFormLstbxFigures";
            this.MainFormLstbxFigures.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.MainFormLstbxFigures.Size = new System.Drawing.Size(272, 289);
            this.MainFormLstbxFigures.TabIndex = 7;
            this.MainFormLstbxFigures.SelectedIndexChanged += new System.EventHandler(this.MainFormLstbxFigures_SelectedIndexChanged);
            // 
            // MainFormTmr
            // 
            this.MainFormTmr.Enabled = true;
            this.MainFormTmr.Interval = 50;
            this.MainFormTmr.Tick += new System.EventHandler(this.MainFormTmr_Tick);
            // 
            // MainFormSttsstp
            // 
            this.MainFormSttsstp.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.MainFormSttsstpLblMouseX,
            this.toolStripStatusLabel3,
            this.MainFormSttsstpLblMouseY,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel4,
            this.MainFormSttsstpLblMouseScreenX,
            this.toolStripStatusLabel5,
            this.MainFormSttsstpLblMouseScreenY,
            this.MainFormSttsstpLblHint});
            this.MainFormSttsstp.Location = new System.Drawing.Point(0, 428);
            this.MainFormSttsstp.Name = "MainFormSttsstp";
            this.MainFormSttsstp.Size = new System.Drawing.Size(800, 22);
            this.MainFormSttsstp.TabIndex = 10;
            this.MainFormSttsstp.Text = "statusStrip1";
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
            this.MainFormSttsstpLblMouseX.Font = new System.Drawing.Font("Segoe Mono Boot", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MainFormSttsstpLblMouseX.Name = "MainFormSttsstpLblMouseX";
            this.MainFormSttsstpLblMouseX.Size = new System.Drawing.Size(29, 17);
            this.MainFormSttsstpLblMouseX.Text = "   ";
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
            this.MainFormSttsstpLblMouseY.Font = new System.Drawing.Font("Segoe Mono Boot", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MainFormSttsstpLblMouseY.Name = "MainFormSttsstpLblMouseY";
            this.MainFormSttsstpLblMouseY.Size = new System.Drawing.Size(29, 17);
            this.MainFormSttsstpLblMouseY.Text = "   ";
            // 
            // MainFormSttsstpLblHint
            // 
            this.MainFormSttsstpLblHint.AutoSize = false;
            this.MainFormSttsstpLblHint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.MainFormSttsstpLblHint.Name = "MainFormSttsstpLblHint";
            this.MainFormSttsstpLblHint.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.MainFormSttsstpLblHint.Size = new System.Drawing.Size(566, 17);
            this.MainFormSttsstpLblHint.Spring = true;
            this.MainFormSttsstpLblHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MainFormBttnCircle
            // 
            this.MainFormBttnCircle.Location = new System.Drawing.Point(13, 310);
            this.MainFormBttnCircle.Name = "MainFormBttnCircle";
            this.MainFormBttnCircle.Size = new System.Drawing.Size(75, 23);
            this.MainFormBttnCircle.TabIndex = 11;
            this.MainFormBttnCircle.Text = "Круг";
            this.MainFormBttnCircle.UseVisualStyleBackColor = true;
            this.MainFormBttnCircle.Click += new System.EventHandler(this.MainFormBttnCircle_Click);
            // 
            // MainFormCmbbxBuildingVariants
            // 
            this.MainFormCmbbxBuildingVariants.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MainFormCmbbxBuildingVariants.FormattingEnabled = true;
            this.MainFormCmbbxBuildingVariants.Location = new System.Drawing.Point(13, 340);
            this.MainFormCmbbxBuildingVariants.Name = "MainFormCmbbxBuildingVariants";
            this.MainFormCmbbxBuildingVariants.Size = new System.Drawing.Size(150, 21);
            this.MainFormCmbbxBuildingVariants.TabIndex = 12;
            this.MainFormCmbbxBuildingVariants.SelectedIndexChanged += new System.EventHandler(this.MainFormCmbbxBuildingVariants_SelectedIndexChanged);
            // 
            // MainFormBttnRectangle
            // 
            this.MainFormBttnRectangle.Location = new System.Drawing.Point(94, 311);
            this.MainFormBttnRectangle.Name = "MainFormBttnRectangle";
            this.MainFormBttnRectangle.Size = new System.Drawing.Size(106, 23);
            this.MainFormBttnRectangle.TabIndex = 11;
            this.MainFormBttnRectangle.Text = "Прямоугольник";
            this.MainFormBttnRectangle.UseVisualStyleBackColor = true;
            this.MainFormBttnRectangle.Click += new System.EventHandler(this.MainFormBttnRectangle_Click);
            // 
            // MainFormBttnCut
            // 
            this.MainFormBttnCut.Location = new System.Drawing.Point(206, 309);
            this.MainFormBttnCut.Name = "MainFormBttnCut";
            this.MainFormBttnCut.Size = new System.Drawing.Size(106, 23);
            this.MainFormBttnCut.TabIndex = 11;
            this.MainFormBttnCut.Text = "Отрезок";
            this.MainFormBttnCut.UseVisualStyleBackColor = true;
            this.MainFormBttnCut.Click += new System.EventHandler(this.MainFormBttnCut_Click);
            // 
            // MainFormBttnSelect
            // 
            this.MainFormBttnSelect.Location = new System.Drawing.Point(318, 311);
            this.MainFormBttnSelect.Name = "MainFormBttnSelect";
            this.MainFormBttnSelect.Size = new System.Drawing.Size(106, 23);
            this.MainFormBttnSelect.TabIndex = 11;
            this.MainFormBttnSelect.Text = "Выделение";
            this.MainFormBttnSelect.UseVisualStyleBackColor = true;
            this.MainFormBttnSelect.Click += new System.EventHandler(this.MainFormBttnSelect_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(248, 372);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "Тест";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MainFormBttnNothing
            // 
            this.MainFormBttnNothing.Location = new System.Drawing.Point(430, 311);
            this.MainFormBttnNothing.Name = "MainFormBttnNothing";
            this.MainFormBttnNothing.Size = new System.Drawing.Size(106, 23);
            this.MainFormBttnNothing.TabIndex = 11;
            this.MainFormBttnNothing.Text = "Ничего";
            this.MainFormBttnNothing.UseVisualStyleBackColor = true;
            this.MainFormBttnNothing.Click += new System.EventHandler(this.MainFormBttnNothing_Click);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.toolStripStatusLabel2.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(4, 17);
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(17, 17);
            this.toolStripStatusLabel4.Text = "X:";
            // 
            // MainFormSttsstpLblMouseScreenX
            // 
            this.MainFormSttsstpLblMouseScreenX.Font = new System.Drawing.Font("Segoe Mono Boot", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MainFormSttsstpLblMouseScreenX.Name = "MainFormSttsstpLblMouseScreenX";
            this.MainFormSttsstpLblMouseScreenX.Size = new System.Drawing.Size(29, 17);
            this.MainFormSttsstpLblMouseScreenX.Text = "   ";
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(17, 17);
            this.toolStripStatusLabel5.Text = "Y:";
            // 
            // MainFormSttsstpLblMouseScreenY
            // 
            this.MainFormSttsstpLblMouseScreenY.Font = new System.Drawing.Font("Segoe Mono Boot", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MainFormSttsstpLblMouseScreenY.Name = "MainFormSttsstpLblMouseScreenY";
            this.MainFormSttsstpLblMouseScreenY.Size = new System.Drawing.Size(29, 17);
            this.MainFormSttsstpLblMouseScreenY.Text = "   ";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.MainFormCmbbxBuildingVariants);
            this.Controls.Add(this.MainFormBttnNothing);
            this.Controls.Add(this.MainFormBttnSelect);
            this.Controls.Add(this.MainFormBttnCut);
            this.Controls.Add(this.MainFormBttnRectangle);
            this.Controls.Add(this.MainFormBttnCircle);
            this.Controls.Add(this.MainFormSttsstp);
            this.Controls.Add(this.MainFormLstbxFigures);
            this.Controls.Add(this.MainFromPctrbxScreen);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MainFromPctrbxScreen)).EndInit();
            this.MainFormSttsstp.ResumeLayout(false);
            this.MainFormSttsstp.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox MainFromPctrbxScreen;
        private System.Windows.Forms.ListBox MainFormLstbxFigures;
        private System.Windows.Forms.Timer MainFormTmr;
        private System.Windows.Forms.StatusStrip MainFormSttsstp;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel MainFormSttsstpLblMouseX;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel MainFormSttsstpLblMouseY;
        private System.Windows.Forms.ToolStripStatusLabel MainFormSttsstpLblHint;
        private System.Windows.Forms.Button MainFormBttnCircle;
        private System.Windows.Forms.ComboBox MainFormCmbbxBuildingVariants;
        private System.Windows.Forms.Button MainFormBttnRectangle;
        private System.Windows.Forms.Button MainFormBttnCut;
        private System.Windows.Forms.Button MainFormBttnSelect;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button MainFormBttnNothing;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel MainFormSttsstpLblMouseScreenX;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ToolStripStatusLabel MainFormSttsstpLblMouseScreenY;
    }
}

