using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrashDiEmV2
{
    public partial class MainForm : Form
    {
        private AnalyzeData m_analyzeData = new AnalyzeData();
        private AppSettings m_appSettings = new AppSettings();
        public delegate void SetUpProgressBarDelegate(int minimum, int maximum, bool enable = true, string customText = null);
        private string m_dataToShow;
        private ListViewColumnSorter lvwColumnSorter;
        public MainForm()
        {
            InitializeComponent();
            btn_Analyse.Enabled = false;
            progressBar1.Enabled = false;
            progressBar1.VisualMode = TextProgressBar.ProgressBarDisplayMode.NoText;
            backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            numericUpDown_MaxLineOfStackToShow.Value = 20;
            lvwColumnSorter = new ListViewColumnSorter();
            this.listView_Devices.ListViewItemSorter = lvwColumnSorter;
            listView_Devices.ColumnClick += ListView_Devices_ColumnClick;
            //Load Setting
            if (m_appSettings.LoadSettings())
            {
                textBox_arm.Text = m_appSettings.ArmSoPath;
                textBox_armV8.Text = m_appSettings.Arm64SoPath;
                textBox_x86.Text = m_appSettings.X86SoPath;
                textBox_x86_64.Text = m_appSettings.X86_64SoPath;
                textBox_CrashLogs.Text = m_appSettings.CrashLogPath;
            }

        }

        private void SaveSettings()
        {
            m_appSettings.ArmSoPath = textBox_arm.Text;
            m_appSettings.Arm64SoPath = textBox_armV8.Text;
            m_appSettings.X86SoPath = textBox_x86.Text;
            m_appSettings.X86_64SoPath  = textBox_x86_64.Text;
            m_appSettings.CrashLogPath = textBox_CrashLogs.Text;

            m_appSettings.SaveToFile();
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
                SaveSettings();
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
                SaveSettings();
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
                SaveSettings();
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
                SaveSettings();
            }
        }

        private void btn_Select_crash_logs_Click(object sender, EventArgs e)
        {
            //    OpenFileDialog dlg = new OpenFileDialog()
            //    {
            //        Title = "Select Crash Logs Folder",
            //        ValidateNames = false,
            //        CheckFileExists = false,
            //        CheckPathExists = true,
            //        FileName = "Folder Selection."
            //      };

            //    if (dlg.ShowDialog() == DialogResult.OK)
            //    {
            //        string folderPath;

            //        folderPath = dlg.FileName;
            //        textBox_CrashLogs.Text = folderPath;
            //    }

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string folderPath = fbd.SelectedPath;
                textBox_CrashLogs.Text = folderPath;
                SaveSettings();
            }
        }

        private void btn_Analyse_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                btn_Analyse_ChangeStage(true);
                backgroundWorker1.CancelAsync();
            }
            else
            {
                btn_Analyse_ChangeStage(false);
                backgroundWorker1.RunWorkerAsync();
            }
            listView_Issue.Items.Clear();
            listView_Devices.Items.Clear();
            listView3.Items.Clear();
            listView4.Items.Clear();
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            m_analyzeData.LogCrashPath = textBox_CrashLogs.Text;
            int fCount = m_analyzeData.ReportCount;
            if (fCount > 0)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    SetUpProgressBar(0, fCount, true, "Loading");
                });
                AnalyzeData.MySetting setting;
                setting.ParseDsym = checkBox_parseDsym.Checked;
                setting.RemoveSOPath = checkBox_RemoveSOPath.Checked;
                int filesLoaded = m_analyzeData.LoadCrashLogs(backgroundWorker1, setting);

                if (!backgroundWorker1.CancellationPending) // If all files was read, parse it!
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                    SetUpProgressBar(0, fCount, true, "Parsing");
                    });
                    m_analyzeData.ProcessData();

                    this.Invoke((MethodInvoker)delegate
                    {
                        SetUpProgressBar(filesLoaded, fCount, true, "Loaded");
                    });
                }
            }
        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (!backgroundWorker1.CancellationPending)
                UpdateProgressBar(e.ProgressPercentage);
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btn_Analyse_ChangeStage(true);
            // Show Data_Issues;
            if (checkBox_GroupIssueByGoogle.Checked)
                ShowData_Issue_ByGoogle();
            else
                ShowData_Issue_ByAddress();
            // Show Data_DeviceList;
            ShowData_Device();
        }
        public void SetUpProgressBar(int minimum, int maximum, bool enable = true, string customText = null)
        {
            progressBar1.Enabled = enable;
            if (customText != null)
            {
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
        private void textBox_CrashLogs_TextChanged(object sender, EventArgs e)
        {
            if (textBox_CrashLogs.Text.Length > 0 && Directory.Exists(textBox_CrashLogs.Text))
                btn_Analyse.Enabled = true;
            else
                btn_Analyse.Enabled = false;
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
                var index = listView_Devices.SelectedIndices[0];
                PostData_DeviceList(index);
            }
        }
        private void PostData_DeviceList(int index)
        {
            var devicesList = m_analyzeData.DevicesList;

            List<int> dataList = devicesList[index].CrashLogIndex;
            //string m_dataToShow;
            m_dataToShow = "";
            List<int> issueIDshowed = new List<int>();
            foreach (int indexToGet in dataList)
            {
                bool isDataFound = false;
                bool shouldAdd = true;
                AnalyzeData.CrashData data = m_analyzeData.GetCrashDataIndex(indexToGet, ref isDataFound);
                //Check this crash was add to texbox
                foreach (int id in issueIDshowed)
                {
                    if (id == data.IssueID)
                        shouldAdd = false;
                }
                if (isDataFound && shouldAdd)
                {
                    m_dataToShow += (data.Path + "\r\n");
                    m_dataToShow += ("App code: " + data.AppCode + "\r\n");
                    m_dataToShow += ("Version Code: " + data.VersionCode + "\r\n");
                    m_dataToShow += ("Date time: " + data.DateTime + "\r\n");
                    m_dataToShow += ("Device: " + data.DeviceBrand + " " + data.DeviceName + " " + data.DeviceModel + "\r\n");
                    m_dataToShow += ("Architecture: " + data.GetArchitectureAsString() + "\r\n");
                    m_dataToShow += ("*** *** *** *** *** *** *** *** *** *** *** *** *** *** *** ***\r\n");
                    m_dataToShow += ("backtrace:\r\n");

                    m_dataToShow += ("\r\n");

                    string[] collectData = m_analyzeData.GetBacktraceByID(data.IssueID);
                    if (collectData != null)
                    {
                        //foreach (string line in collectData)
                        int collectData_Count = collectData.Count();
                        for (int i = 0; i < collectData_Count; i ++)
                        {
                            m_dataToShow += (collectData[i] + "\r\n");
                            if (numericUpDown_MaxLineOfStackToShow.Value > 0 && i > numericUpDown_MaxLineOfStackToShow.Value)
                            {
                                m_dataToShow += ((collectData_Count - i - 1)  + " more lines....\r\n");
                                break;
                            }
                        }
                        issueIDshowed.Add(data.IssueID);
                    }
#if DEBUG
                    m_dataToShow += ("ID: " + data.IssueID);
                    //m_dataToShow += ("code: [" + data.IssueID + "]");
#endif
                    m_dataToShow += ("\r\n==================================================================\r\n");
                }
            }
            textBox_Resultt.Text = m_dataToShow;
        }
        private void ShowData_Device()
        {
            var devicesList = m_analyzeData.DevicesList;
            foreach (var device in devicesList)
            {
                var item = new ListViewItem(new string[] { device.DeviceBrand, device.DeviceName, device.DeviceModel, device.CrashLogIndex.Count().ToString() });
                listView_Devices.Items.Add(item);
            }
        }
        private void ShowData_Issue_ByAddress()
        {
            var issueList = m_analyzeData.IssuesList;
            foreach (var issue in issueList)
            {
                var item = new ListViewItem(new string[] { listView_Issue.Items.Count.ToString(), issue.DeviceIndex.Count().ToString(), issue.Name });
                listView_Issue.Items.Add(item);
            }
        }
        private void ShowData_Issue_ByGoogle()
        {
            var issueList = m_analyzeData.IssuesList;
            List<string> issueAdded = new List<string>();

            foreach (var issue in issueList)
            {
                if (!issueAdded.Contains(issue.FolderName))
                {
                    var item = new ListViewItem(new string[] { listView_Issue.Items.Count.ToString(), issue.DeviceIndex.Count.ToString(), issue.FolderName });
                    listView_Issue.Items.Add(item);
                    issueAdded.Add(issue.FolderName);
                }
                else // Get Index and increase num report 
                {
                    int indexlistview = issueAdded.IndexOf(issue.FolderName);
                    int currentValue = int.Parse(listView_Issue.Items[indexlistview].SubItems[1].Text);

                    listView_Issue.Items[indexlistview].SubItems[1].Text = (currentValue + issue.DeviceIndex.Count).ToString();
                    if (issue.FolderName == "CrashTop_2")
                        Console.WriteLine("CrashTop_2 index = " + indexlistview + " currentValue = " + currentValue + " added = " + issue.DeviceIndex.Count);
                }
            }
        }

        private void listView_Issue_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_Resultt.Clear();

            if (listView_Issue.SelectedItems.Count > 0)
            {
                var item = listView_Issue.SelectedItems[0];
                string issuename = item.SubItems[2].Text;
                PostData_IssueList(issuename);
            }
        }
        private void PostData_IssueList(string issueName)
        {
            var issueList = m_analyzeData.IssuesList;
            m_dataToShow = "";
            foreach(var issue in issueList)
            {
                if (issue.FolderName == issueName)
                {
                    m_dataToShow += ("backtrace:\r\n");
                    m_dataToShow += ("\r\n");
                    int collectData_Count = issue.Stactrace.Count();
                    for (int i = 0; i < collectData_Count; i++)
                    {
                        m_dataToShow += (issue.Stactrace[i] + "\r\n");
                        if (numericUpDown_MaxLineOfStackToShow.Value > 0 && i > numericUpDown_MaxLineOfStackToShow.Value)
                        {
                            m_dataToShow += ((collectData_Count - i - 1) + " more lines....\r\n");
                            break;
                        }
                    }
#if DEBUG
                    m_dataToShow += ("ID: " + issue.ID);
                    //m_dataToShow += ("code: [" + data.IssueID + "]");
#endif
                    m_dataToShow += ("\r\n==================================================================\r\n");
                }
            }
            textBox_Resultt.Text = m_dataToShow;
        }
        private void ListView_Devices_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.listView_Devices.Sort();
        }
    }
}
