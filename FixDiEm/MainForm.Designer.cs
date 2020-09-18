namespace FixDiEm
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btn_armeabi = new System.Windows.Forms.Button();
            this.btn_x86 = new System.Windows.Forms.Button();
            this.textBox_arm = new System.Windows.Forms.TextBox();
            this.textBox_x86 = new System.Windows.Forms.TextBox();
            this.btn_arm64_v8a = new System.Windows.Forms.Button();
            this.btn_x86_64 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox_x86_64 = new System.Windows.Forms.TextBox();
            this.textBox_armV8 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox_txt = new System.Windows.Forms.CheckBox();
            this.textBox_CrashLogs = new System.Windows.Forms.TextBox();
            this.btn_Select_crash_logs = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBox_GroupIssueByGoogle = new System.Windows.Forms.CheckBox();
            this.checkBox_RemoveSOPath = new System.Windows.Forms.CheckBox();
            this.checkBox_parseDsym = new System.Windows.Forms.CheckBox();
            this.btn_Analyse = new System.Windows.Forms.Button();
            this.checkBox_autoShutdown = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btn_Load = new System.Windows.Forms.Button();
            this.btn_Save = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage = new System.Windows.Forms.TabPage();
            this.listView_Issue = new System.Windows.Forms.ListView();
            this.column_STT = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column_CountReport = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column_Issue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage_Devices = new System.Windows.Forms.TabPage();
            this.listView_Devices = new System.Windows.Forms.ListView();
            this.columnHeader_Manufacture = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_Name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_DvCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_ReportCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_index = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage_Arc = new System.Windows.Forms.TabPage();
            this.listView3 = new System.Windows.Forms.ListView();
            this.tabPage_Api = new System.Windows.Forms.TabPage();
            this.listView4 = new System.Windows.Forms.ListView();
            this.textBox_Highlight = new System.Windows.Forms.TextBox();
            this.label_Highlight = new System.Windows.Forms.Label();
            this.textBox_Resultt = new System.Windows.Forms.TextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.numericUpDown_MaxLineOfStackToShow = new System.Windows.Forms.NumericUpDown();
            this.label_NumLineStack = new System.Windows.Forms.Label();
            this.checkBox_showAddress = new System.Windows.Forms.CheckBox();
            this.backgroundWorker_PostData = new System.ComponentModel.BackgroundWorker();
            this.progressBar1 = new FixDiEm.TextProgressBar();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage.SuspendLayout();
            this.tabPage_Devices.SuspendLayout();
            this.tabPage_Arc.SuspendLayout();
            this.tabPage_Api.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_MaxLineOfStackToShow)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_armeabi
            // 
            this.btn_armeabi.Location = new System.Drawing.Point(6, 21);
            this.btn_armeabi.Name = "btn_armeabi";
            this.btn_armeabi.Size = new System.Drawing.Size(75, 22);
            this.btn_armeabi.TabIndex = 0;
            this.btn_armeabi.Text = "armeabi-v7a";
            this.btn_armeabi.UseVisualStyleBackColor = true;
            this.btn_armeabi.Click += new System.EventHandler(this.btn_armeabi_Click);
            // 
            // btn_x86
            // 
            this.btn_x86.Location = new System.Drawing.Point(6, 48);
            this.btn_x86.Name = "btn_x86";
            this.btn_x86.Size = new System.Drawing.Size(75, 22);
            this.btn_x86.TabIndex = 1;
            this.btn_x86.Text = "x86";
            this.btn_x86.UseVisualStyleBackColor = true;
            this.btn_x86.Click += new System.EventHandler(this.btn_x86_Click);
            // 
            // textBox_arm
            // 
            this.textBox_arm.Location = new System.Drawing.Point(87, 22);
            this.textBox_arm.Name = "textBox_arm";
            this.textBox_arm.Size = new System.Drawing.Size(260, 20);
            this.textBox_arm.TabIndex = 2;
            // 
            // textBox_x86
            // 
            this.textBox_x86.Location = new System.Drawing.Point(87, 49);
            this.textBox_x86.Name = "textBox_x86";
            this.textBox_x86.Size = new System.Drawing.Size(260, 20);
            this.textBox_x86.TabIndex = 3;
            // 
            // btn_arm64_v8a
            // 
            this.btn_arm64_v8a.Location = new System.Drawing.Point(374, 21);
            this.btn_arm64_v8a.Name = "btn_arm64_v8a";
            this.btn_arm64_v8a.Size = new System.Drawing.Size(75, 22);
            this.btn_arm64_v8a.TabIndex = 4;
            this.btn_arm64_v8a.Text = "arm64-v8a";
            this.btn_arm64_v8a.UseVisualStyleBackColor = true;
            this.btn_arm64_v8a.Click += new System.EventHandler(this.btn_arm64_v8a_Click);
            // 
            // btn_x86_64
            // 
            this.btn_x86_64.Location = new System.Drawing.Point(374, 48);
            this.btn_x86_64.Name = "btn_x86_64";
            this.btn_x86_64.Size = new System.Drawing.Size(75, 22);
            this.btn_x86_64.TabIndex = 5;
            this.btn_x86_64.Text = "x86_64";
            this.btn_x86_64.UseVisualStyleBackColor = true;
            this.btn_x86_64.Click += new System.EventHandler(this.btn_x86_64_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_x86_64);
            this.groupBox1.Controls.Add(this.textBox_armV8);
            this.groupBox1.Controls.Add(this.btn_armeabi);
            this.groupBox1.Controls.Add(this.btn_x86_64);
            this.groupBox1.Controls.Add(this.btn_x86);
            this.groupBox1.Controls.Add(this.btn_arm64_v8a);
            this.groupBox1.Controls.Add(this.textBox_arm);
            this.groupBox1.Controls.Add(this.textBox_x86);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(732, 85);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Dsym";
            // 
            // textBox_x86_64
            // 
            this.textBox_x86_64.Location = new System.Drawing.Point(455, 49);
            this.textBox_x86_64.Name = "textBox_x86_64";
            this.textBox_x86_64.Size = new System.Drawing.Size(260, 20);
            this.textBox_x86_64.TabIndex = 7;
            // 
            // textBox_armV8
            // 
            this.textBox_armV8.Location = new System.Drawing.Point(455, 22);
            this.textBox_armV8.Name = "textBox_armV8";
            this.textBox_armV8.Size = new System.Drawing.Size(260, 20);
            this.textBox_armV8.TabIndex = 6;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox_txt);
            this.groupBox2.Controls.Add(this.textBox_CrashLogs);
            this.groupBox2.Controls.Add(this.btn_Select_crash_logs);
            this.groupBox2.Location = new System.Drawing.Point(761, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(475, 85);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Crash Logs";
            // 
            // checkBox_txt
            // 
            this.checkBox_txt.AutoSize = true;
            this.checkBox_txt.Checked = true;
            this.checkBox_txt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_txt.Enabled = false;
            this.checkBox_txt.Location = new System.Drawing.Point(7, 52);
            this.checkBox_txt.Name = "checkBox_txt";
            this.checkBox_txt.Size = new System.Drawing.Size(44, 17);
            this.checkBox_txt.TabIndex = 2;
            this.checkBox_txt.Text = "*.txt";
            this.checkBox_txt.UseVisualStyleBackColor = true;
            // 
            // textBox_CrashLogs
            // 
            this.textBox_CrashLogs.Location = new System.Drawing.Point(89, 22);
            this.textBox_CrashLogs.Name = "textBox_CrashLogs";
            this.textBox_CrashLogs.Size = new System.Drawing.Size(371, 20);
            this.textBox_CrashLogs.TabIndex = 1;
            this.textBox_CrashLogs.TextChanged += new System.EventHandler(this.textBox_CrashLogs_TextChanged);
            // 
            // btn_Select_crash_logs
            // 
            this.btn_Select_crash_logs.Location = new System.Drawing.Point(7, 21);
            this.btn_Select_crash_logs.Name = "btn_Select_crash_logs";
            this.btn_Select_crash_logs.Size = new System.Drawing.Size(75, 22);
            this.btn_Select_crash_logs.TabIndex = 0;
            this.btn_Select_crash_logs.Text = "Select";
            this.btn_Select_crash_logs.UseVisualStyleBackColor = true;
            this.btn_Select_crash_logs.Click += new System.EventHandler(this.btn_Select_crash_logs_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.progressBar1);
            this.groupBox3.Controls.Add(this.checkBox_GroupIssueByGoogle);
            this.groupBox3.Controls.Add(this.checkBox_RemoveSOPath);
            this.groupBox3.Controls.Add(this.checkBox_parseDsym);
            this.groupBox3.Controls.Add(this.btn_Analyse);
            this.groupBox3.Location = new System.Drawing.Point(12, 104);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(732, 54);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Analyse";
            // 
            // checkBox_GroupIssueByGoogle
            // 
            this.checkBox_GroupIssueByGoogle.AutoSize = true;
            this.checkBox_GroupIssueByGoogle.Checked = true;
            this.checkBox_GroupIssueByGoogle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_GroupIssueByGoogle.Location = new System.Drawing.Point(291, 25);
            this.checkBox_GroupIssueByGoogle.Name = "checkBox_GroupIssueByGoogle";
            this.checkBox_GroupIssueByGoogle.Size = new System.Drawing.Size(132, 17);
            this.checkBox_GroupIssueByGoogle.TabIndex = 3;
            this.checkBox_GroupIssueByGoogle.Text = "Group Issue by google";
            this.checkBox_GroupIssueByGoogle.UseVisualStyleBackColor = true;
            // 
            // checkBox_RemoveSOPath
            // 
            this.checkBox_RemoveSOPath.AutoSize = true;
            this.checkBox_RemoveSOPath.Checked = true;
            this.checkBox_RemoveSOPath.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_RemoveSOPath.Location = new System.Drawing.Point(176, 25);
            this.checkBox_RemoveSOPath.Name = "checkBox_RemoveSOPath";
            this.checkBox_RemoveSOPath.Size = new System.Drawing.Size(109, 17);
            this.checkBox_RemoveSOPath.TabIndex = 2;
            this.checkBox_RemoveSOPath.Text = "Remove SO Path";
            this.checkBox_RemoveSOPath.UseVisualStyleBackColor = true;
            // 
            // checkBox_parseDsym
            // 
            this.checkBox_parseDsym.AutoSize = true;
            this.checkBox_parseDsym.Location = new System.Drawing.Point(89, 25);
            this.checkBox_parseDsym.Name = "checkBox_parseDsym";
            this.checkBox_parseDsym.Size = new System.Drawing.Size(82, 17);
            this.checkBox_parseDsym.TabIndex = 1;
            this.checkBox_parseDsym.Text = "Parse Dsym";
            this.checkBox_parseDsym.UseVisualStyleBackColor = true;
            // 
            // btn_Analyse
            // 
            this.btn_Analyse.ForeColor = System.Drawing.Color.Black;
            this.btn_Analyse.Location = new System.Drawing.Point(7, 20);
            this.btn_Analyse.Name = "btn_Analyse";
            this.btn_Analyse.Size = new System.Drawing.Size(75, 23);
            this.btn_Analyse.TabIndex = 0;
            this.btn_Analyse.Text = "Analyse";
            this.btn_Analyse.UseVisualStyleBackColor = true;
            this.btn_Analyse.Click += new System.EventHandler(this.btn_Analyse_Click);
            // 
            // checkBox_autoShutdown
            // 
            this.checkBox_autoShutdown.AutoSize = true;
            this.checkBox_autoShutdown.Location = new System.Drawing.Point(170, 26);
            this.checkBox_autoShutdown.Name = "checkBox_autoShutdown";
            this.checkBox_autoShutdown.Size = new System.Drawing.Size(99, 17);
            this.checkBox_autoShutdown.TabIndex = 4;
            this.checkBox_autoShutdown.Text = "Auto Shutdown";
            this.checkBox_autoShutdown.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btn_Load);
            this.groupBox4.Controls.Add(this.checkBox_autoShutdown);
            this.groupBox4.Controls.Add(this.btn_Save);
            this.groupBox4.Location = new System.Drawing.Point(759, 104);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(477, 54);
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Load and save";
            // 
            // btn_Load
            // 
            this.btn_Load.Location = new System.Drawing.Point(89, 20);
            this.btn_Load.Name = "btn_Load";
            this.btn_Load.Size = new System.Drawing.Size(75, 23);
            this.btn_Load.TabIndex = 1;
            this.btn_Load.Text = "Load";
            this.btn_Load.UseVisualStyleBackColor = true;
            this.btn_Load.Click += new System.EventHandler(this.btn_Load_Click);
            // 
            // btn_Save
            // 
            this.btn_Save.Location = new System.Drawing.Point(7, 20);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(75, 23);
            this.btn_Save.TabIndex = 0;
            this.btn_Save.Text = "Save";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tabControl.Controls.Add(this.tabPage);
            this.tabControl.Controls.Add(this.tabPage_Devices);
            this.tabControl.Controls.Add(this.tabPage_Arc);
            this.tabControl.Controls.Add(this.tabPage_Api);
            this.tabControl.Location = new System.Drawing.Point(12, 164);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(351, 562);
            this.tabControl.TabIndex = 10;
            // 
            // tabPage
            // 
            this.tabPage.Controls.Add(this.listView_Issue);
            this.tabPage.Location = new System.Drawing.Point(4, 22);
            this.tabPage.Name = "tabPage";
            this.tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage.Size = new System.Drawing.Size(343, 536);
            this.tabPage.TabIndex = 0;
            this.tabPage.Text = "Issues";
            this.tabPage.UseVisualStyleBackColor = true;
            this.tabPage.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // listView_Issue
            // 
            this.listView_Issue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listView_Issue.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.column_STT,
            this.column_CountReport,
            this.column_Issue});
            this.listView_Issue.FullRowSelect = true;
            this.listView_Issue.GridLines = true;
            this.listView_Issue.HideSelection = false;
            this.listView_Issue.Location = new System.Drawing.Point(0, 0);
            this.listView_Issue.Name = "listView_Issue";
            this.listView_Issue.Size = new System.Drawing.Size(342, 534);
            this.listView_Issue.TabIndex = 0;
            this.listView_Issue.UseCompatibleStateImageBehavior = false;
            this.listView_Issue.View = System.Windows.Forms.View.Details;
            this.listView_Issue.SelectedIndexChanged += new System.EventHandler(this.listView_Issue_SelectedIndexChanged);
            // 
            // column_STT
            // 
            this.column_STT.Text = "STT";
            this.column_STT.Width = 0;
            // 
            // column_CountReport
            // 
            this.column_CountReport.Text = "Num report";
            this.column_CountReport.Width = 64;
            // 
            // column_Issue
            // 
            this.column_Issue.Text = "Issue";
            this.column_Issue.Width = 274;
            // 
            // tabPage_Devices
            // 
            this.tabPage_Devices.Controls.Add(this.listView_Devices);
            this.tabPage_Devices.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Devices.Name = "tabPage_Devices";
            this.tabPage_Devices.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Devices.Size = new System.Drawing.Size(343, 536);
            this.tabPage_Devices.TabIndex = 1;
            this.tabPage_Devices.Text = "Devices";
            this.tabPage_Devices.UseVisualStyleBackColor = true;
            // 
            // listView_Devices
            // 
            this.listView_Devices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listView_Devices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_Manufacture,
            this.columnHeader_Name,
            this.columnHeader_DvCode,
            this.columnHeader_ReportCount,
            this.columnHeader_index});
            this.listView_Devices.FullRowSelect = true;
            this.listView_Devices.GridLines = true;
            this.listView_Devices.HideSelection = false;
            this.listView_Devices.Location = new System.Drawing.Point(0, 0);
            this.listView_Devices.Name = "listView_Devices";
            this.listView_Devices.Size = new System.Drawing.Size(342, 534);
            this.listView_Devices.TabIndex = 0;
            this.listView_Devices.UseCompatibleStateImageBehavior = false;
            this.listView_Devices.View = System.Windows.Forms.View.Details;
            this.listView_Devices.SelectedIndexChanged += new System.EventHandler(this.listView_Devices_SelectedIndexChanged);
            // 
            // columnHeader_Manufacture
            // 
            this.columnHeader_Manufacture.Text = "Manufac";
            this.columnHeader_Manufacture.Width = 70;
            // 
            // columnHeader_Name
            // 
            this.columnHeader_Name.Text = "Name";
            this.columnHeader_Name.Width = 100;
            // 
            // columnHeader_DvCode
            // 
            this.columnHeader_DvCode.Text = "Code";
            this.columnHeader_DvCode.Width = 90;
            // 
            // columnHeader_ReportCount
            // 
            this.columnHeader_ReportCount.Text = "Report Count";
            this.columnHeader_ReportCount.Width = 100;
            // 
            // columnHeader_index
            // 
            this.columnHeader_index.Width = 0;
            // 
            // tabPage_Arc
            // 
            this.tabPage_Arc.Controls.Add(this.listView3);
            this.tabPage_Arc.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Arc.Name = "tabPage_Arc";
            this.tabPage_Arc.Size = new System.Drawing.Size(343, 536);
            this.tabPage_Arc.TabIndex = 2;
            this.tabPage_Arc.Text = "Architecture";
            this.tabPage_Arc.UseVisualStyleBackColor = true;
            // 
            // listView3
            // 
            this.listView3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listView3.HideSelection = false;
            this.listView3.Location = new System.Drawing.Point(0, 0);
            this.listView3.Name = "listView3";
            this.listView3.Size = new System.Drawing.Size(342, 534);
            this.listView3.TabIndex = 0;
            this.listView3.UseCompatibleStateImageBehavior = false;
            // 
            // tabPage_Api
            // 
            this.tabPage_Api.Controls.Add(this.listView4);
            this.tabPage_Api.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Api.Name = "tabPage_Api";
            this.tabPage_Api.Size = new System.Drawing.Size(343, 536);
            this.tabPage_Api.TabIndex = 3;
            this.tabPage_Api.Text = "API Level";
            this.tabPage_Api.UseVisualStyleBackColor = true;
            // 
            // listView4
            // 
            this.listView4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listView4.HideSelection = false;
            this.listView4.Location = new System.Drawing.Point(0, 0);
            this.listView4.Name = "listView4";
            this.listView4.Size = new System.Drawing.Size(342, 534);
            this.listView4.TabIndex = 0;
            this.listView4.UseCompatibleStateImageBehavior = false;
            // 
            // textBox_Highlight
            // 
            this.textBox_Highlight.Location = new System.Drawing.Point(439, 164);
            this.textBox_Highlight.Name = "textBox_Highlight";
            this.textBox_Highlight.Size = new System.Drawing.Size(305, 20);
            this.textBox_Highlight.TabIndex = 11;
            // 
            // label_Highlight
            // 
            this.label_Highlight.AutoSize = true;
            this.label_Highlight.Location = new System.Drawing.Point(390, 167);
            this.label_Highlight.Name = "label_Highlight";
            this.label_Highlight.Size = new System.Drawing.Size(48, 13);
            this.label_Highlight.TabIndex = 12;
            this.label_Highlight.Text = "Highlight";
            this.label_Highlight.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // textBox_Resultt
            // 
            this.textBox_Resultt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Resultt.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_Resultt.Location = new System.Drawing.Point(393, 190);
            this.textBox_Resultt.MaxLength = 62767;
            this.textBox_Resultt.Multiline = true;
            this.textBox_Resultt.Name = "textBox_Resultt";
            this.textBox_Resultt.ReadOnly = true;
            this.textBox_Resultt.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_Resultt.Size = new System.Drawing.Size(939, 535);
            this.textBox_Resultt.TabIndex = 13;
            this.textBox_Resultt.WordWrap = false;
            // 
            // numericUpDown_MaxLineOfStackToShow
            // 
            this.numericUpDown_MaxLineOfStackToShow.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown_MaxLineOfStackToShow.Location = new System.Drawing.Point(849, 163);
            this.numericUpDown_MaxLineOfStackToShow.Name = "numericUpDown_MaxLineOfStackToShow";
            this.numericUpDown_MaxLineOfStackToShow.Size = new System.Drawing.Size(46, 20);
            this.numericUpDown_MaxLineOfStackToShow.TabIndex = 14;
            // 
            // label_NumLineStack
            // 
            this.label_NumLineStack.AutoSize = true;
            this.label_NumLineStack.Location = new System.Drawing.Point(758, 166);
            this.label_NumLineStack.Name = "label_NumLineStack";
            this.label_NumLineStack.Size = new System.Drawing.Size(91, 13);
            this.label_NumLineStack.TabIndex = 15;
            this.label_NumLineStack.Text = "Num line to show:";
            // 
            // checkBox_showAddress
            // 
            this.checkBox_showAddress.AutoSize = true;
            this.checkBox_showAddress.Location = new System.Drawing.Point(910, 166);
            this.checkBox_showAddress.Name = "checkBox_showAddress";
            this.checkBox_showAddress.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox_showAddress.Size = new System.Drawing.Size(124, 17);
            this.checkBox_showAddress.TabIndex = 16;
            this.checkBox_showAddress.Text = "Show Crash Address";
            this.checkBox_showAddress.UseVisualStyleBackColor = true;
            // 
            // backgroundWorker_PostData
            // 
            this.backgroundWorker_PostData.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            // 
            // progressBar1
            // 
            this.progressBar1.CustomText = "";
            this.progressBar1.Location = new System.Drawing.Point(486, 19);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.ProgressColor = System.Drawing.Color.LightGreen;
            this.progressBar1.Size = new System.Drawing.Size(234, 23);
            this.progressBar1.TabIndex = 5;
            this.progressBar1.TextColor = System.Drawing.Color.Black;
            this.progressBar1.TextFont = new System.Drawing.Font("Times New Roman", 11F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.progressBar1.VisualMode = FixDiEm.TextProgressBar.ProgressBarDisplayMode.CurrProgress;
            this.progressBar1.Click += new System.EventHandler(this.progressBar1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1344, 736);
            this.Controls.Add(this.checkBox_showAddress);
            this.Controls.Add(this.label_NumLineStack);
            this.Controls.Add(this.numericUpDown_MaxLineOfStackToShow);
            this.Controls.Add(this.textBox_Resultt);
            this.Controls.Add(this.label_Highlight);
            this.Controls.Add(this.textBox_Highlight);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "FixDiEm | quang.haduy@gameloft.com";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPage.ResumeLayout(false);
            this.tabPage_Devices.ResumeLayout(false);
            this.tabPage_Arc.ResumeLayout(false);
            this.tabPage_Api.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_MaxLineOfStackToShow)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_armeabi;
        private System.Windows.Forms.Button btn_x86;
        private System.Windows.Forms.TextBox textBox_arm;
        private System.Windows.Forms.TextBox textBox_x86;
        private System.Windows.Forms.Button btn_arm64_v8a;
        private System.Windows.Forms.Button btn_x86_64;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_x86_64;
        private System.Windows.Forms.TextBox textBox_armV8;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox_CrashLogs;
        private System.Windows.Forms.Button btn_Select_crash_logs;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBox_autoShutdown;
        private System.Windows.Forms.CheckBox checkBox_GroupIssueByGoogle;
        private System.Windows.Forms.CheckBox checkBox_RemoveSOPath;
        private System.Windows.Forms.CheckBox checkBox_parseDsym;
        private System.Windows.Forms.Button btn_Analyse;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btn_Load;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage;
        private System.Windows.Forms.TabPage tabPage_Devices;
        private System.Windows.Forms.TabPage tabPage_Arc;
        private System.Windows.Forms.TabPage tabPage_Api;
        private System.Windows.Forms.TextBox textBox_Highlight;
        private System.Windows.Forms.Label label_Highlight;
        private System.Windows.Forms.TextBox textBox_Resultt;
        private System.Windows.Forms.ListView listView_Issue;
        private System.Windows.Forms.ListView listView_Devices;
        private System.Windows.Forms.ListView listView3;
        private System.Windows.Forms.ListView listView4;
        private System.Windows.Forms.CheckBox checkBox_txt;
        private FixDiEm.TextProgressBar progressBar1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ColumnHeader columnHeader_Manufacture;
        private System.Windows.Forms.ColumnHeader columnHeader_Name;
        private System.Windows.Forms.ColumnHeader columnHeader_DvCode;
        private System.Windows.Forms.ColumnHeader columnHeader_ReportCount;
        private System.Windows.Forms.ColumnHeader column_STT;
        private System.Windows.Forms.ColumnHeader column_CountReport;
        private System.Windows.Forms.NumericUpDown numericUpDown_MaxLineOfStackToShow;
        private System.Windows.Forms.Label label_NumLineStack;
        private System.Windows.Forms.ColumnHeader column_Issue;
        private System.Windows.Forms.CheckBox checkBox_showAddress;
        private System.Windows.Forms.ColumnHeader columnHeader_index;
        private System.ComponentModel.BackgroundWorker backgroundWorker_PostData;
    }
}

