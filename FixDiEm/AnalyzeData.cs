using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;
using System.Data;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;

namespace FixDiEm
{
    class AnalyzeData
    {
        public struct MySetting
        {
            public bool ParseDsym;
            public bool RemoveSOPath;
        }

        private string m_DataPath;
        private CrashData[] m_CrashDataRaw;
        private List<CrashReport> m_issueList;
        private int m_numTxtFiles = 0;
        private int m_numReportLoaded = 0;
        private List<Device> m_DevicesList;

        public string LogCrashPath
        {
            set
            {
                m_DataPath = value;
            }
            get
            {
                return m_DataPath;
            }
        }
        public int TxtCount
        {
            get
            {
                if (m_numTxtFiles == 0)
                    m_numTxtFiles = Directory.GetFiles(m_DataPath, "*.txt", SearchOption.AllDirectories).Length;
                return m_numTxtFiles;
            }
        }
        public int ReportLoaded
        {
            private set
            {
                m_numReportLoaded = value;
            }
            get
            {
                return m_numReportLoaded;
            }
        }
        public List<CrashReport> IssuesList
        {
            set
            {
                m_issueList = value;
            }
            get
            {
                return m_issueList;
            }
        }
        public ref List<CrashReport> IssuesListRef
        {
            get
            {
                return ref m_issueList;
            }
        }
        public List<Device> DevicesList
        {
            set
            {
                m_DevicesList = value;
            }
            get
            {
                return m_DevicesList;
            }
        }
        public ref List<Device> DevicesListRef
        {
            get
            {
                return ref m_DevicesList;
            }
        }
        public ref CrashData[] CrashReportRawRef
        {
            get
            {
                return ref m_CrashDataRaw;
            }
        }
        public CrashData[] CrashReportRaw
        {
            set
            {
                m_CrashDataRaw = value;
            }
            get
            {
                return m_CrashDataRaw;
            }
        }
        private void ClearAndReInitData()
        {
            //Clear Old Data
            m_numReportLoaded = 0;
            m_CrashDataRaw = null;
            m_DevicesList = null;
            m_issueList = null;
            m_CrashDataRaw = new CrashData[this.TxtCount];
            m_issueList = new List<CrashReport>();
            m_DevicesList = new List<Device>();
        }
        public delegate void ReadFile(string qua);

        public int LoadCrashLogs(BackgroundWorker backgroundWorker, MySetting setting)
        {

            ClearAndReInitData();

            if (m_CrashDataRaw.Length <= 0)
                return 0;

            int index = 0;
            string[] List_files;

            if (!Directory.Exists(LogCrashPath))
            {
                return 0;
            }

            List_files = Directory.GetFiles(LogCrashPath, "*.txt", SearchOption.AllDirectories);
            

            //ConcurrentQueue<string> FileContentRead_Queued = new ConcurrentQueue<string>();
            //string lastfile = List_files.Last();
            long milliseconds_1 = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            //Task ReadFile_task = Task.Run(() => 
            //{
            //    foreach (string file in List_files)
            //    {
            //        string contents = File.ReadAllText(file);
            //        FileContentRead_Queued.Enqueue(contents);

            //        if (backgroundWorker.CancellationPending)
            //        {
            //            return;
            //        }
            //    }
            //});

            //ReadFile_task.Wait();
            //while (index < List_files.Length)
            //{
            //    string enqString;
            //    if (FileContentRead_Queued.TryDequeue(out enqString))
            //    {
            //        string[] lines = enqString.Split('\n');

            //        if (lines[0] == "NATIVE CRASH" || lines[0] == "JAVA CRASH")
            //        {
            //            ConvertData(ref lines, List_files[index].Remove(0, LogCrashPath.Length), setting, index);

            //            backgroundWorker.ReportProgress(++index);
            //        }
            //    }
            //    if (backgroundWorker.CancellationPending)
            //    {
            //        break;
            //    }
            //}

            //ReadFile_task.Wait();

            //Task.WaitAll(ReadFile_task);

            long milliseconds_2 = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            foreach (string file in List_files)
            {
                string contents = File.ReadAllText(file);
                string[] lines = contents.Split('\n');
                if (lines[0] == "NATIVE CRASH" || lines[0] == "JAVA CRASH")
                {
                    ConvertData(ref lines, file.Remove(0, m_DataPath.Length), setting, index);
                    backgroundWorker.ReportProgress(++index);
                }
                if (backgroundWorker.CancellationPending)
                {
                    break;
                }
            }

            long milliseconds_3 = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            
            Console.WriteLine("Time to load: {0}, files: {1}", milliseconds_3 - milliseconds_1, this.ReportLoaded);
            return this.ReportLoaded;
        }
        private void ConvertData(ref string[] lines, string path, MySetting setting, int index)
        {
            string type = lines[0];
            string appcode = lines[2];
            string[] Datetime = (lines[3].Substring(lines[3].IndexOf(':') + 2)).TrimEnd().Split(',');
            string Versioncode = lines[4];
            string Versionname = lines[5];
            string Devicemodel = lines[6];
            string Devicename = lines[7].TrimEnd();
            string Devicebrand = lines[8];
            string APIlevel = lines[9];
            string architecture = lines[10];
            string folderName = Path.GetFileName(Path.GetDirectoryName(path));
            DateTime datetime;
            DateTime clocktime;

            //Parse DateTime
            if (!DateTime.TryParseExact(Datetime[0].TrimEnd(), "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out datetime))
                if (!DateTime.TryParseExact(Datetime[0].TrimEnd(), "MMM dd", null, System.Globalization.DateTimeStyles.None, out datetime))
                    if (!DateTime.TryParseExact(Datetime[0].TrimEnd(), "MMM d", null, System.Globalization.DateTimeStyles.None, out datetime))
                        Console.WriteLine("Cannot parse date in file: " + path);
            
            if (!DateTime.TryParseExact(Datetime[1].TrimStart(), "hh:mm tt", null, System.Globalization.DateTimeStyles.None, out clocktime))
                if (!DateTime.TryParseExact(Datetime[1].TrimStart(), "h:mm tt", null, System.Globalization.DateTimeStyles.None, out clocktime))
                    Console.WriteLine("Cannot parse time in file: " + path);

            datetime = datetime.AddHours(clocktime.Hour);
            datetime = datetime.AddMinutes(clocktime.Minute);

            CrashData data = new CrashData(path)
            {
                CrashType = type == "NATIVE CRASH" ? CrashType.Native_Crash : CrashType.JAVA_Crash,

                AppCode = appcode.Substring(lines[2].IndexOf(':') + 2),
                DateTime = datetime,
                VersionCode = Versioncode.Substring(Versioncode.IndexOf(':') + 2),
                VersionName = Versionname.Substring(Versionname.IndexOf(':') + 2),
                DeviceModel = Devicemodel.Substring(Devicemodel.IndexOf(':') + 2),
                DeviceName = Devicename.IndexOf(':') < Devicename.Length - 1 ? Devicename.Substring(Devicename.IndexOf(':') + 2) : "",
                DeviceBrand = Devicebrand.Substring(Devicebrand.IndexOf(':') + 2)
            };
            try
            {
                data.APILevel = Int32.Parse(APIlevel.Substring(APIlevel.IndexOf(':') + 2));
            }
            catch (FormatException)
            {
                Console.WriteLine("FormatException: value: {0} Path: {1}", APIlevel, path);
            }

            switch(architecture.Substring(architecture.IndexOf(':') + 2))
            {
                case "N/A":
                    data.architecture = Architecture.Unknow;
                    break;
                case "armeabi-v8":
                    data.architecture = Architecture.arm64_v8a;
                    break;
                case "armeabi-v7a":
                    data.architecture = Architecture.armeabi_v7a;
                    break;
                case "x86":
                    data.architecture = Architecture.x86;
                    break;
                case "x86_64":
                    data.architecture = Architecture.x86_64;
                    break;
                default:
                    data.architecture = Architecture.Unknow;
                    break;
            }
            if (data.CrashType == CrashType.Native_Crash)
            {
                // Find where is backtrace?
                int backtraceBeginLine = 0;
                foreach (string line in lines) // Can find begin line 10, opt late!
                {
                    if (line == "backtrace:")
                        break;
                    backtraceBeginLine++;
                }
                // process backtrace:
                int numlineBacktrace = lines.Count() - backtraceBeginLine - 2;
                string[] backtraceData = new string[numlineBacktrace];
                for (int i = 0; i < numlineBacktrace; i++)
                {
                    string currentLine = lines[i + backtraceBeginLine + 1];
                    string finalCurrentLine = currentLine;
                    if (setting.RemoveSOPath) // Clear SO Path
                    {
                        // /data/app/com.gameloft.android.ANMP.GloftA9HM-FrOY_R937xKDYVA2yPYfhQ==/lib/arm64/libAsphalt9.so (offset 0x309c000)
                        if (currentLine.Contains("offset "))
                        {
                            int start = currentLine.IndexOf('/');
                            int end = currentLine.IndexOf(')');
                            int len = end - start + 2;
                            if (end < (currentLine.Length - 1))
                            {
                                finalCurrentLine = currentLine.Remove(start, len);
                            }
                        }
                        else if (currentLine.Contains(" /data/") && currentLine.Contains(".so "))
                        {
                            int start = currentLine.IndexOf('/');
                            int end = currentLine.IndexOf('(');
                            int len = end - start;
                            if (len > 0)
                            {
                                finalCurrentLine = currentLine.Remove(start, len);
                            }
                        }
                    }
                    backtraceData[i] = finalCurrentLine;
                }
                //Check issue and add to list
                //Get AddressString
                string addressString = "";
                //Check is the line match with crash address
                //Regex rgx = new Regex(@"^  #\d|[0-999]  pc $");
                foreach (string line in backtraceData)
                {
                    string[] splitLine = line.Split(' ');
                    if (splitLine.Count() > 6 && splitLine[4].Equals("pc"))
                        addressString += splitLine[5];
                }
                int hashCode = addressString.GetHashCode();
                bool isNewIssue = true;
                for(int i = 0; i < m_issueList.Count(); i++)
                {
                    if (m_issueList[i].AddressHashCode == hashCode && m_issueList[i].FolderName == folderName)
                    {
                        data.IssueID = m_issueList[i].ID;

                        m_issueList[i].DeviceIndex.Add(index);

                        isNewIssue = false;
                        break;
                    }
                }
                if (isNewIssue)
                {
                    string issueName = backtraceData[0];
                    foreach(var line in backtraceData)
                    {
                        string[] lines_issuename = line.Split(' ');
                        string lastArray = lines_issuename[lines_issuename.Length - 1];
                        if (lastArray.StartsWith("(") && lastArray.EndsWith(")"))
                        {
                            issueName = lastArray;
                            break;
                        }
                    }
                    CrashReport issuedata = new CrashReport
                    {
                        AddressHashCode = hashCode,
                        Stactrace = backtraceData,
                        ID = m_issueList.Count(),
                        FolderName = folderName,
                        Name = issueName
                    };
                    issuedata.DeviceIndex.Add(index);

                    m_issueList.Add(issuedata);
                    data.IssueID = issuedata.ID;
                }
            } // end native crash
            else // Java Crash
            {
                int backtraceBeginLine = 12;

                if (backtraceBeginLine >= lines.Count())
                {
                    Console.WriteLine("Not found backtrace in: {0}", path);
                    data.IssueID = -1;
                }
                else
                {
                    // process backtrace:
                    int numlineBacktrace = lines.Count() - backtraceBeginLine - 1;
                    string[] backtraceData = new string[numlineBacktrace];
                    for (int i = 0; i < numlineBacktrace; i++)
                    {
                        backtraceData[i] = lines[i + backtraceBeginLine];
                    }
                    //Check issue and add to list
                    //Get AddressString
                    string addressString = backtraceData[0] + backtraceData[1] + backtraceData[2] + backtraceData[numlineBacktrace - 1];
                    int hashCode = addressString.GetHashCode();
                    bool isNewIssue = true;
                    for (int i = 0; i < m_issueList.Count(); i++)
                    {
                        if (m_issueList[i].AddressHashCode == hashCode && m_issueList[i].FolderName == folderName)
                        {
                            data.IssueID = m_issueList[i].ID;

                            m_issueList[i].DeviceIndex.Add(index);

                            isNewIssue = false;
                            break;
                        }
                    }
                    if (isNewIssue)
                    {
                        CrashReport issuedata = new CrashReport
                        {
                            Name = backtraceData[0],
                            AddressHashCode = hashCode,
                            Stactrace = backtraceData,
                            ID = m_issueList.Count(),
                            FolderName = folderName
                        };
                        issuedata.DeviceIndex.Add(index);

                        m_issueList.Add(issuedata);
                        data.IssueID = issuedata.ID;
                    }
                }
            }
            m_numReportLoaded++;
            m_CrashDataRaw[index] = data;
        }
        
        public void ProcessData()
        {
            // Sumup Devices
            for( int index = 0; index < ReportLoaded; index++)
            {
                CrashData data = m_CrashDataRaw[index];
                bool isAdded = false;
                foreach (Device device in m_DevicesList)
                {
                    if (device.DeviceBrand == data.DeviceBrand)
                    {
                        if (device.DeviceName == data.DeviceName)
                        {
                            if (device.DeviceModel  == data.DeviceModel)
                            {
                                isAdded = true;
                                device.CrashLogIndex.Add(index);
                                break;
                            }
                        }
                    }
                }
                if (!isAdded) // Add this device to list
                {
                    Device device = new Device(data.DeviceBrand, data.DeviceName, data.DeviceModel);
                    device.CrashLogIndex.Add(index);
                    m_DevicesList.Add(device);
                }
            }
        }

        public string[] GetBacktraceByID(int ID)
        {
            //Can opt by return index m_issueData[ID].Stactrace
            if (ID == -1)
                return null;

            foreach (var issue in m_issueList)
            {
                if (issue.ID == ID)
                {
                    return issue.Stactrace;
                }
            }
            return null;
        }
        public ref CrashData GetCrashDataIndex(int index, ref bool found)
        {
            if (index >=0 && index <= m_numReportLoaded)
            {
                found = true;
                return ref m_CrashDataRaw[index];
            }
            found = false;
            return ref m_CrashDataRaw[0];
        }
        public class RootObject
        {
            public CrashData[] CrashReportRaw { set; get; }
            public List<CrashReport> IssuesList { set; get; }
            public List<Device> DevicesList { set; get; }
        }
        public void SaveDataToFile(string path)
        {
            string json_final = JsonConvert.SerializeObject(new
            {
                CrashReportRaw,
                IssuesList,
                DevicesList
            });
            File.WriteAllText(path, json_final);
        }
        public void LoadDataFromFile(string path)
        {
            if (File.Exists(path))
            {
                CrashReportRaw = null;
                IssuesList = null;
                DevicesList = null;

                string json = File.ReadAllText(path);

                RootObject data = JsonConvert.DeserializeObject<RootObject>(json);

                CrashReportRaw = data.CrashReportRaw;

                IssuesList = data.IssuesList;

                DevicesList = data.DevicesList;

                if (CrashReportRaw != null && IssuesList != null && DevicesList != null)
                {
                    ReportLoaded = CrashReportRaw.Count();
                }
                else
                {
                    string message = "Can't load save file!";
                    string title = "Error!";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show(message, title, buttons, MessageBoxIcon.Error);
                }
                    
            }


        }

    }
}
