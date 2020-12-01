using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FixDiEm
{
    public partial class MainForm : Form
    {
        private string AppTitle = "FixDiEm | quang.haduy@gameloft.com";
        private string TextConfirmClosing = "Do you really want to exit?";
        private string SaveFileName = "appSetting.json";
        private bool IsSaveSettings = false;
        private AnalyzeData analyzeData = new AnalyzeData();
        private AppSettings appSettings = new AppSettings();
        private string dataToShow;
        private ListViewColumnSorter lvDevicesColumnSorter;
        private ListViewColumnSorter lvIssueColumnSorter;

        public MainForm()
        {
            InitializeComponent();
            this.Text = AppTitle;
            btn_Analyse.Enabled = false;
            progressBar1.Enabled = false;
            progressBar1.VisualMode = TextProgressBar.ProgressBarDisplayMode.NoText;

            btn_Save.Enabled = false;

            backgroundWorker_AnalyzeData = new BackgroundWorker();
            backgroundWorker_AnalyzeData.WorkerReportsProgress = true;
            backgroundWorker_AnalyzeData.WorkerSupportsCancellation = true;
            backgroundWorker_AnalyzeData.DoWork += backgroundWorker_AnalyzeData_DoWork;
            backgroundWorker_AnalyzeData.ProgressChanged += backgroundWorker_AnalyzeData_ProgressChanged;
            backgroundWorker_AnalyzeData.RunWorkerCompleted += backgroundWorker_AnalyzeData_RunWorkerCompleted;

            backgroundWorker_SaveData = new BackgroundWorker();
            backgroundWorker_SaveData.WorkerReportsProgress = true;
            backgroundWorker_SaveData.WorkerSupportsCancellation = true;
            backgroundWorker_SaveData.DoWork += backgroundWorker_SaveData_DoWork;
            //backgroundWorker_SaveData.ProgressChanged += backgroundWorker_SaveData_ProgressChanged;
            backgroundWorker_SaveData.RunWorkerCompleted += backgroundWorker_SaveData_RunWorkerCompleted;

            backgroundWorker_ShowCrashIssue = new BackgroundWorker();
            //backgroundWorker_ShowCrashIssue.WorkerReportsProgress = true;
            backgroundWorker_ShowCrashIssue.WorkerSupportsCancellation = true;
            backgroundWorker_ShowCrashIssue.DoWork += backgroundWorker_ShowCrashIssue_DoWork;
            //backgroundWorker_ShowCrashIssue.ProgressChanged += backgroundWorker_ShowCrashIssue_ProgressChanged;
            //backgroundWorker_ShowCrashIssue.RunWorkerCompleted += backgroundWorker_ShowCrashIssue_RunWorkerCompleted;

            numericUpDown_MaxLineOfStackToShow.Value = 20;

            lvDevicesColumnSorter = new ListViewColumnSorter();
            listView_Devices.ListViewItemSorter = lvDevicesColumnSorter;
            listView_Devices.ColumnClick += ListView_Devices_ColumnClick;

            lvIssueColumnSorter = new ListViewColumnSorter();
            listView_Issue.ListViewItemSorter = lvIssueColumnSorter;
            listView_Issue.ColumnClick += ListView_Issue_ColumnClick;
            //Load Setting
            if (appSettings.LoadSettings(SaveFileName))
            {
                textBox_arm.Text = appSettings.ArmSoPath;
                textBox_armV8.Text = appSettings.Arm64SoPath;
                textBox_x86.Text = appSettings.X86SoPath;
                textBox_x86_64.Text = appSettings.X86_64SoPath;
                textBox_CrashLogs.Text = appSettings.CrashLogPath;
                checkBox_parseDsym.Checked = appSettings.IsParseDsym;
                checkBox_GroupIssueByGoogle.Checked = appSettings.IsGroupIssueByGoogle;
                checkBox_RemoveSOPath.Checked = appSettings.IsRemoveSOPath;
                checkBox_showAddress.Checked = appSettings.IsShowCrashAddress;
                numericUpDown_MaxLineOfStackToShow.Value = appSettings.NumLineToShow;
            }
            if (appSettings.SoPathRegex == null || appSettings.ReportFileStructureRegex  == null || appSettings.GameSoPathRegex == null)
            {
                AppSettings_Changed(null,null);
                appSettings.SoPathRegex = new Regex(@"/data.+(\.so\s|\.apk\s|\.so$|\.apk$)"); //cheat
                appSettings.GameSoPathRegex = new Regex(@"/data.+(?!.apk).+(\.so$)"); //cheat
                appSettings.ReportFileStructureRegex = new Regex(
                    @"(?<crashtype>(^(.*)(\r\r|\n\n|\r\n\r\n)))" +
                    @"App Code:(?<appcode>.*)\n" +
                    @"Date time:(?<datetime>.*)\n" +
                    @"Version code:(?<versioncode>.*)\n" +
                    @"Version name:(?<versionname>.*)\n" +
                    @"Device model:(?<devicemodel>.*)\n" +
                    @"Device name:(?<devicename>.*)\n" +
                    @"Device brand:(?<devicebrand>.*)\n" +
                    @"API level:(?<apilevel>.*)\n" +
                    @"Architecture:(?<architecture>.*\n)" +
                    @"(?<stacktrace>(.|\n)+)");
            }
            //Set callback TextChanged
            textBox_arm.TextChanged += AppSettings_Changed;
            textBox_armV8.TextChanged += AppSettings_Changed;
            textBox_x86.TextChanged += AppSettings_Changed;
            textBox_x86_64.TextChanged += AppSettings_Changed;
            textBox_CrashLogs.TextChanged += AppSettings_Changed;
            checkBox_parseDsym.CheckStateChanged += AppSettings_Changed;
            checkBox_GroupIssueByGoogle.CheckStateChanged += AppSettings_Changed;
            checkBox_RemoveSOPath.CheckStateChanged += AppSettings_Changed;
            checkBox_showAddress.CheckStateChanged += AppSettings_Changed;
            checkBox_showAddress.CheckStateChanged += AppSettings_Changed;
            numericUpDown_MaxLineOfStackToShow.ValueChanged += AppSettings_Changed;
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            DialogResult d = MessageBox.Show(TextConfirmClosing, AppTitle, MessageBoxButtons.YesNo);
            if (d == DialogResult.No)
            {
                e.Cancel = true;
            }
            else if (IsSaveSettings)
            {
                SaveSettings();
            }

            base.OnFormClosing(e);
        }
        private void SaveSettings()
        {
            appSettings.SaveToFile(SaveFileName);
        }
        private void AppSettings_Changed(object sender, EventArgs e)
        {
            IsSaveSettings = true;

            appSettings.ArmSoPath = textBox_arm.Text;
            appSettings.Arm64SoPath = textBox_armV8.Text;
            appSettings.X86SoPath = textBox_x86.Text;
            appSettings.X86_64SoPath = textBox_x86_64.Text;
            appSettings.CrashLogPath = textBox_CrashLogs.Text;
            appSettings.IsParseDsym = checkBox_parseDsym.Checked;
            appSettings.IsGroupIssueByGoogle = checkBox_GroupIssueByGoogle.Checked;
            appSettings.IsRemoveSOPath = checkBox_RemoveSOPath.Checked;
            appSettings.IsShowCrashAddress = checkBox_showAddress.Checked;
            appSettings.NumLineToShow = (int)numericUpDown_MaxLineOfStackToShow.Value;
        }
        private void textBox_CrashLogs_TextChanged(object sender, EventArgs e)
        {
            if (textBox_CrashLogs.Text.Length > 0 && Directory.Exists(textBox_CrashLogs.Text))
                btn_Analyse.Enabled = true;
            else
                btn_Analyse.Enabled = false;
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void btn_armeabi_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog()
            {
                Filter = "Dsym (*.so, *.dsym)|*.so;*.dsym",
                Title = "Select Dsym for armeabi"
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string fileName;

                fileName = dlg.FileName;
                textBox_arm.Text = fileName;
            }
        }

        private void btn_x86_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog()
            {
                Filter = "Dsym (*.so, *.dsym)|*.so;*.dsym",
                Title = "Select Dsym for x86"
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string fileName;

                fileName = dlg.FileName;
                textBox_x86.Text = fileName;
            }
        }

        private void btn_arm64_v8a_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog()
            {
                Filter = "Dsym (*.so, *.dsym)|*.so;*.dsym",
                Title = "Select Dsym for arm64"
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string fileName;

                fileName = dlg.FileName;
                textBox_armV8.Text = fileName;
            }
        }

        private void btn_x86_64_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog()
            {
                Filter = "Dsym (*.so, *.dsym)|*.so;*.dsym",
                Title = "Select Dsym for x86_64"
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string fileName;

                fileName = dlg.FileName;
                textBox_x86_64.Text = fileName;
            }
        }

        private void btn_Select_crash_logs_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = textBox_CrashLogs.Text;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string folderPath = fbd.SelectedPath;
                textBox_CrashLogs.Text = folderPath;
            }
        }

        private void btn_Analyse_Click(object sender, EventArgs e)
        {
            if (backgroundWorker_AnalyzeData.IsBusy)
            {
                btn_Analyse_ChangeStage(true);
                backgroundWorker_AnalyzeData.CancelAsync();
            }
            else
            {
                SetUpProgressBar(0, 0, true, "Checking...");
                btn_Analyse_ChangeStage(false);
                backgroundWorker_AnalyzeData.RunWorkerAsync();
            }
            listView_Issue.Items.Clear();
            listView_Devices.Items.Clear();
            listView3.Items.Clear();
            listView4.Items.Clear();
        }
        private void backgroundWorker_AnalyzeData_DoWork(object sender, DoWorkEventArgs e)
        {
            analyzeData.LogCrashPath = textBox_CrashLogs.Text;
            int fCount = analyzeData.TxtCount;
            if (fCount > 0)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    SetUpProgressBar(0, fCount, true, "Loading");
                });

                int filesLoaded = analyzeData.LoadCrashLogs(backgroundWorker_AnalyzeData, appSettings);

                analyzeData.ProcessData();

                if (!backgroundWorker_AnalyzeData.CancellationPending)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        SetUpProgressBar(0, fCount, true, "Parsing");
                    });
                    // Need implement parse Dsym here

                    Invoke((MethodInvoker)delegate
                    {
                        SetUpProgressBar(filesLoaded, fCount, true, "Loaded");
                    });
                }
            }
        }
        private void backgroundWorker_AnalyzeData_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (!backgroundWorker_AnalyzeData.CancellationPending)
                UpdateProgressBar(e.ProgressPercentage);
        }
        private void backgroundWorker_AnalyzeData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btn_Analyse_ChangeStage(true);
            // Show Data_Issues;
            if (checkBox_GroupIssueByGoogle.Checked)
                ShowData_Issue_ByGoogle();
            else
                ShowData_Issue_ByAddress();

            // Show Data_DeviceList;
            ShowData_Device();

            lvIssueColumnSorter.SortColumn = 1;  // Collumn reportCount
            lvIssueColumnSorter.Order = SortOrder.Descending;
            listView_Issue.Sort();
            lvIssueColumnSorter.Order = SortOrder.Ascending;
            lvIssueColumnSorter.SortColumn = 2;
            listView_Issue.Sort();

            lvDevicesColumnSorter.SortColumn = 3;
            lvDevicesColumnSorter.Order = SortOrder.Descending;
            listView_Devices.Sort();

            btn_Save.Enabled = true;
        }
        public void SetUpProgressBar(int minimum, int maximum, bool enable = true, string customText = null)
        {
            progressBar1.Enabled = enable;
            if (customText != null)
            {
                if (minimum == 0 && maximum == 0)
                    progressBar1.VisualMode = TextProgressBar.ProgressBarDisplayMode.CustomText;
                else
                    progressBar1.VisualMode = TextProgressBar.ProgressBarDisplayMode.TextAndCurrProgress;
                progressBar1.CustomText = customText;
            }

            progressBar1.Minimum = minimum;
            progressBar1.Maximum = maximum;
        }
        public void UpdateProgressBar(int value)
        {
            progressBar1.Value = value;
        }


        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
        private void btn_Analyse_ChangeStage(bool isCanAnalyse)
        {
            if (isCanAnalyse)
            {
                btn_Analyse.Text = "Analyse";
                btn_Analyse.ForeColor = Color.Black;
            }
            else
            {
                btn_Analyse.Text = "Stop!";
                btn_Analyse.ForeColor = Color.Red;
            }
        }

        private void listView_Devices_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_Resultt.Clear();
            if (listView_Devices.SelectedItems.Count > 0)
            {
                var item = listView_Devices.SelectedItems[0];
                PostData_DeviceList(int.Parse(item.SubItems[4].Text));
            }
        }
        private void PostData_DeviceList(int index)
        {
            var devicesList = analyzeData.DevicesList;

            List<int> dataList = devicesList[index].CrashLogIndex;
            //string m_dataToShow;
            dataToShow = "";
            List<int> issueIDshowed = new List<int>();
            foreach (int indexToGet in dataList)
            {
                bool isDataFound = false;
                bool shouldAdd = true;
                CrashData data = analyzeData.GetCrashDataIndex(indexToGet, ref isDataFound);
                //Check this crash was add to texbox
                foreach (int id in issueIDshowed)
                {
                    if (id == data.IssueID)
                        shouldAdd = false;
                }
                if (isDataFound && shouldAdd)
                {
                    dataToShow += $"Path: {data.Path}\r\n";
                    dataToShow += $"App code: {data.AppCode}\r\n";
                    dataToShow += $"Version Code: {data.VersionCode}\r\n";
                    dataToShow += $"Date time: {(data.DateTime != DateTime.MinValue ? data.DateTime.ToString() : "")}\r\n";
                    dataToShow += $"Device: {data.DeviceBrand} {data.DeviceName} {data.DeviceModel}\r\n";
                    dataToShow += $"Architecture: {data.GetArchitectureAsString()}\r\n";
                    dataToShow += $"*** *** *** *** *** *** *** *** *** *** *** *** *** *** *** ***\r\n";
                    dataToShow += $"Stacktrace:\r\n";

                    dataToShow += "\r\n";

                    string[] backTraces = analyzeData.GetBacktraceByID(data.IssueID, appSettings);
                    if (backTraces != null)
                    {

                        int lineCount = 0;
                        foreach(var line in backTraces)
                        {
                            if (checkBox_showAddress.Checked)
                            {
                                dataToShow += $"{line}\r\n";
                            }
                            else
                            {
                                dataToShow += $"{RemoveAddressInCrashLine(line)}\r\n";

                            }
                            lineCount++;
                            if (numericUpDown_MaxLineOfStackToShow.Value > 0 && lineCount > numericUpDown_MaxLineOfStackToShow.Value)
                            {
                                dataToShow += $"{backTraces.Length - lineCount - 1} more lines....\r\n";
                                break;
                            }
                        }
                        issueIDshowed.Add(data.IssueID);
                    }
#if DEBUG
                    dataToShow += $"ID: {data.IssueID}";
#endif
                    dataToShow += ("\r\n==================================================================\r\n");
                }
            }
            textBox_Resultt.Text = dataToShow;
        }
        private string RemoveAddressInCrashLine(string input)
        {
            string output = input;
            if (input.Contains("  pc "))
            {
                int begin = input.IndexOf('p') + 3;
                int index_endAddress = input.IndexOf(' ', begin);
                int len = index_endAddress - begin;
                if (len > 0)
                {
                    output = input.Substring(0, begin) + input.Substring(index_endAddress).TrimStart();
                }

            }
            return output;
        }
        private void ShowData_Device()
        {
            listView_Devices.Items.Clear();

            foreach (var device in analyzeData.DevicesList)
            {
                var item = new ListViewItem(new string[] { device.DeviceBrand, device.DeviceName, device.DeviceModel, device.CrashLogIndex.Count().ToString(), listView_Devices.Items.Count.ToString() });
                listView_Devices.Items.Add(item);
            }
        }
        private void ShowData_Issue_ByAddress()
        {
            foreach(var issue in analyzeData.IssueList)
            {
                var item = new ListViewItem(new string[] { listView_Issue.Items.Count.ToString(), issue.DeviceIndex.Count().ToString(), issue.FolderName, issue.Name });
                listView_Issue.Items.Add(item);
            }
        }
        private void ShowData_Issue_ByGoogle()
        {
            listView_Issue.Items.Clear();

            foreach(var issue in analyzeData.IssueList)
            {
                int indexlistview = listView_Issue.Items.IndexOfKey(issue.FolderName);
                if (indexlistview < 0) //Not added to list yet.
                {
                    var item = new ListViewItem(new string[] { listView_Issue.Items.Count.ToString(), issue.DeviceIndex.Count.ToString(), issue.FolderName, issue.Name });
                    item.Name = issue.FolderName; // This is key of Item.
                    listView_Issue.Items.Add(item);
                }
                else //Found, increase reportcount
                {
                    int currentValueOfReportCount = int.Parse(listView_Issue.Items[indexlistview].SubItems[1].Text);

                    listView_Issue.Items[indexlistview].SubItems[1].Text = (currentValueOfReportCount + issue.DeviceIndex.Count).ToString();
                }
            }
        }

        private void listView_Issue_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_Resultt.Clear();

            if (listView_Issue.SelectedItems.Count > 0)
            {
                var item = listView_Issue.SelectedItems[0];

                string folderName = item.SubItems[2].Text;

                backgroundWorker_ShowCrashIssue.RunWorkerAsync(argument: folderName);
                //PostData_IssueList(issuename);
            }
        }
        private void backgroundWorker_ShowCrashIssue_DoWork(object sender, DoWorkEventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                textBox_Resultt.Text = "Collecting data!";
            });

            Invoke((MethodInvoker)delegate
            {
                PostData_IssueList(e.Argument.ToString());
            });
        }
        private void PostData_IssueList(string folderName)
        {

            dataToShow = "";

            foreach(var issue in analyzeData.IssueList)
            {
                if (issue.FolderName == folderName)
                {
                    List<string> Path = new List<string>();
                    List<string> AppCode = new List<string>();
                    List<string> VersionCode = new List<string>();
                    //List<string> DateTime = new List<string>();
                    Dictionary<string, int> DeviceName = new Dictionary<string, int>();
                    List<int> Api = new List<int>();
                    List<string> Arch = new List<string>();
                    DateTime Timebegin = new DateTime();
                    DateTime Timeend = new DateTime();

                    foreach(var report in analyzeData.CrashDataRaw)
                    {
                        if (report!= null && report.IssueID == issue.ID)
                        {
                            if (Path.Count <= 0)
                                Path.Add(report.Path);

                            if (!AppCode.Contains(report.AppCode))
                                AppCode.Add(report.AppCode);

                            if (!VersionCode.Contains(report.VersionCode))
                                VersionCode.Add(report.VersionCode);

                            if (Timebegin == DateTime.MinValue)
                                Timebegin = report.DateTime;
                            else if (report.DateTime < Timebegin)
                                Timebegin = report.DateTime;

                            if (Timeend == DateTime.MinValue)
                                Timeend = report.DateTime;
                            else if (report.DateTime > Timeend)
                                Timeend = report.DateTime;

                            string devicename = $"{report.DeviceName} {report.DeviceModel}";
                            if (DeviceName.ContainsKey(devicename))
                                DeviceName[devicename] += 1;
                            else
                                DeviceName.Add(devicename, 1);

                            if (!Api.Contains(report.APILevel))
                                Api.Add(report.APILevel);

                            if (!Arch.Contains(report.GetArchitectureAsString()))
                                Arch.Add(report.GetArchitectureAsString());
                        }
                    }

                    string stdatetimebegin = Timebegin != DateTime.MinValue ? Timebegin.ToString() : "";
                    string stdatetimeend = Timeend != DateTime.MinValue ? Timeend.ToString() : "";
                    string stdatefinal = stdatetimebegin + (stdatetimebegin != "" && stdatetimeend != "" ? " to " : "") + stdatetimeend;

                    dataToShow += ($"Path: {string.Join(",", Path.ToArray())}\r\n");
                    dataToShow += ($"App code: {string.Join(",", AppCode.ToArray())}\r\n");
                    dataToShow += ($"Version Code: {string.Join(",", VersionCode.ToArray())}\r\n");
                    dataToShow += ($"Date time: {stdatefinal}\r\n");
                    dataToShow += ($"Device: {string.Join(",", DeviceName.ToArray())}\r\n");
                    dataToShow += ($"Api: {string.Join(",", Api.ToArray())}\r\n");
                    dataToShow += ($"Architecture: {string.Join(",", Arch.ToArray())}\r\n");
                    dataToShow += ("*** *** *** *** *** *** *** *** *** *** *** *** *** *** *** ***\r\nStacktrace:\r\n\r\n");

                    var stacktrace = issue.GetStactrace(appSettings);
                    int collectData_Count = stacktrace.Length;
                    for (int i = 0; i < collectData_Count; i++)
                    {
                        dataToShow += stacktrace[i] + "\r\n";
                        //if (checkBox_showAddress.Checked)
                        //{
                        //    dataToShow += crashline;
                        //}
                        //else
                        //{
                        //    dataToShow += RemoveAddressInCrashLine(crashline);

                        //}
                        if (numericUpDown_MaxLineOfStackToShow.Value > 0 && i > numericUpDown_MaxLineOfStackToShow.Value)
                        {
                            dataToShow += $"{collectData_Count - i} more lines....\r\n";
                            break;
                        }
                    }
#if DEBUG
                    dataToShow += $"ID: {issue.ID}";
#endif
                    dataToShow += ("\r\n==================================================================\r\n");
                }
            }
            textBox_Resultt.Text = dataToShow;
        }
        private void ListView_Devices_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvDevicesColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvDevicesColumnSorter.Order == SortOrder.Ascending)
                {
                    lvDevicesColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvDevicesColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvDevicesColumnSorter.SortColumn = e.Column;
                lvDevicesColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.listView_Devices.Sort();
        }

        private void ListView_Issue_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvIssueColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvIssueColumnSorter.Order == SortOrder.Ascending)
                {
                    lvIssueColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvIssueColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvIssueColumnSorter.SortColumn = e.Column;
                lvIssueColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.listView_Issue.Sort();
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = saveFileDialog1.FileName;
                // Check file name
                if (!filename.EndsWith(".json"))
                    filename += ".json";

                backgroundWorker_SaveData.RunWorkerAsync(argument: filename);
            }

        }

        private void backgroundWorker_SaveData_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
               this.Text = this.AppTitle + " | Saving data.....";
            });

            string filename = (string)e.Argument;
            analyzeData.SaveDataToFile(filename);
        }

        private void backgroundWorker_SaveData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                this.Text += $" Completed at {DateTime.Now.ToString()}!";
            });
        }

        private void btn_Load_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog()
            {
                Filter = "All files (*.*)|*.*|Json (*.json)|*.json",
                Title = "Select json file to load",
                FilterIndex = 2
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                SetUpProgressBar(0, 0, true, "Loading Data....");

                analyzeData.LoadDataFromFile(dlg.FileName);

                if (analyzeData.ReportLoaded > 0)
                {
                    ShowData_Issue_ByGoogle();
                    ShowData_Device();

                    //Sort
                    lvIssueColumnSorter.SortColumn = 1;  // Collumn reportCount
                    lvIssueColumnSorter.Order = SortOrder.Descending;
                    listView_Issue.Sort();

                    lvDevicesColumnSorter.SortColumn = 3;
                    lvDevicesColumnSorter.Order = SortOrder.Descending;
                    listView_Devices.Sort();

                    SetUpProgressBar(analyzeData.ReportLoaded, analyzeData.ReportLoaded, true, "Loaded");
                }
            }
        }

        private void btn_Settings_Click(object sender, EventArgs e)
        {

        }
    }
}
