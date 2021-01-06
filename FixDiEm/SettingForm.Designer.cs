namespace FixDiEm
{
    partial class SettingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label_SoPath = new System.Windows.Forms.Label();
            this.textBox_SoPath = new System.Windows.Forms.TextBox();
            this.label_GameSoPath = new System.Windows.Forms.Label();
            this.textBox_GameSoPath = new System.Windows.Forms.TextBox();
            this.label_ReportFileStructure = new System.Windows.Forms.Label();
            this.textBox_ReportFileStructure = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_SoPath
            // 
            this.label_SoPath.AutoSize = true;
            this.label_SoPath.Location = new System.Drawing.Point(6, 13);
            this.label_SoPath.Name = "label_SoPath";
            this.label_SoPath.Size = new System.Drawing.Size(45, 13);
            this.label_SoPath.TabIndex = 0;
            this.label_SoPath.Text = "SoPath:";
            this.label_SoPath.Click += new System.EventHandler(this.label1_Click);
            // 
            // textBox_SoPath
            // 
            this.textBox_SoPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_SoPath.Location = new System.Drawing.Point(9, 32);
            this.textBox_SoPath.Name = "textBox_SoPath";
            this.textBox_SoPath.Size = new System.Drawing.Size(471, 20);
            this.textBox_SoPath.TabIndex = 1;
            // 
            // label_GameSoPath
            // 
            this.label_GameSoPath.AutoSize = true;
            this.label_GameSoPath.Location = new System.Drawing.Point(7, 70);
            this.label_GameSoPath.Name = "label_GameSoPath";
            this.label_GameSoPath.Size = new System.Drawing.Size(73, 13);
            this.label_GameSoPath.TabIndex = 2;
            this.label_GameSoPath.Text = "GameSoPath:";
            // 
            // textBox_GameSoPath
            // 
            this.textBox_GameSoPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_GameSoPath.Location = new System.Drawing.Point(9, 92);
            this.textBox_GameSoPath.Name = "textBox_GameSoPath";
            this.textBox_GameSoPath.Size = new System.Drawing.Size(471, 20);
            this.textBox_GameSoPath.TabIndex = 3;
            // 
            // label_ReportFileStructure
            // 
            this.label_ReportFileStructure.AutoSize = true;
            this.label_ReportFileStructure.Location = new System.Drawing.Point(6, 124);
            this.label_ReportFileStructure.Name = "label_ReportFileStructure";
            this.label_ReportFileStructure.Size = new System.Drawing.Size(101, 13);
            this.label_ReportFileStructure.TabIndex = 4;
            this.label_ReportFileStructure.Text = "ReportFileStructure:";
            // 
            // textBox_ReportFileStructure
            // 
            this.textBox_ReportFileStructure.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_ReportFileStructure.Location = new System.Drawing.Point(9, 145);
            this.textBox_ReportFileStructure.Multiline = true;
            this.textBox_ReportFileStructure.Name = "textBox_ReportFileStructure";
            this.textBox_ReportFileStructure.Size = new System.Drawing.Size(471, 203);
            this.textBox_ReportFileStructure.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(319, 361);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(405, 360);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 395);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox_ReportFileStructure);
            this.Controls.Add(this.label_ReportFileStructure);
            this.Controls.Add(this.textBox_GameSoPath);
            this.Controls.Add(this.label_GameSoPath);
            this.Controls.Add(this.textBox_SoPath);
            this.Controls.Add(this.label_SoPath);
            this.Name = "SettingForm";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_SoPath;
        private System.Windows.Forms.TextBox textBox_SoPath;
        private System.Windows.Forms.Label label_GameSoPath;
        private System.Windows.Forms.TextBox textBox_GameSoPath;
        private System.Windows.Forms.Label label_ReportFileStructure;
        private System.Windows.Forms.TextBox textBox_ReportFileStructure;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}