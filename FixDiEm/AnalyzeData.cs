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

        public CrashData[] CrashDataRaw { set; get; }
        public int ReportLoaded { private set; get; } = 0;
        public List<Device> DevicesList { set; get; }

        public List<CrashReport> IssueList { set; get; }
        private int numTxtFiles = 0;

        public string LogCrashPath { set; get; }

        public int TxtCount
        {
            get
            {
                if (numTxtFiles == 0)
                    numTxtFiles = Directory.GetFiles(LogCrashPath, "*.txt", SearchOption.AllDirectories).Length;
                return numTxtFiles;
            }
        }


        private void ClearAndReInitData()
        {
            //Clear Old Data
            ReportLoaded = 0;
            CrashDataRaw = null;
            DevicesList = null;
            IssueList = null;
            CrashDataRaw = new CrashData[this.TxtCount];
            IssueList = new List<CrashReport>();
            DevicesList = new List<Device>();
        }
        public delegate void ReadFile(string qua);

        public int LoadCrashLogs(BackgroundWorker backgroundWorker, MySetting setting)
        {

            ClearAndReInitData();

            if (CrashDataRaw.Length <= 0)
                return 0;

            int index = 0;
            string[] List_files;

            if (!Directory.Exists(LogCrashPath))
            {
                return 0;
            }

            List_files = Directory.GetFiles(LogCrashPath, "*.txt", SearchOption.AllDirectories);
            
            long milliseconds_1 = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            long milliseconds_2 = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            foreach (string file in List_files)
            {
                string contents = File.ReadAllText(file);
                string[] lines = contents.Split('\n');
                if (lines[0] == "NATIVE CRASH" || lines[0] == "JAVA CRASH")
                {
                    ConvertData(ref lines, file.Remove(0, LogCrashPath.Length), setting, index);
                    backgroundWorker.ReportProgress(++index);
                }
                if (backgroundWorker.CancellationPending)
                {
                    break;
                }
            }

            long milliseconds_3 = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            
            Console.WriteLine("Time to load: {0}, files: {1}", milliseconds_3 - milliseconds_1, ReportLoaded);
            return ReportLoaded;
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

                foreach(var issue in IssueList)
                {
                    if (issue.AddressHashCode == hashCode && issue.FolderName == folderName)
                    {
                        data.IssueID = issue.ID;

                        issue.DeviceIndex.Add(index);

                        isNewIssue = false;
                        break;
                    }
                }
                if (isNewIssue)
                {
                    int posLast = backtraceData[0].LastIndexOf('/');
                    string issueName = posLast > 0 ? backtraceData[0].Substring(posLast + 1) : backtraceData[0];

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
                        ID = IssueList.Count(),
                        FolderName = folderName,
                        Name = issueName
                    };
                    issuedata.DeviceIndex.Add(index);

                    IssueList.Add(issuedata);
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

                    foreach(var issue in IssueList)
                    {
                        if (issue.AddressHashCode == hashCode && issue.FolderName == folderName)
                        {
                            data.IssueID = issue.ID;

                            issue.DeviceIndex.Add(index);

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
                            ID = IssueList.Count(),
                            FolderName = folderName
                        };
                        issuedata.DeviceIndex.Add(index);

                        IssueList.Add(issuedata);
                        data.IssueID = issuedata.ID;
                    }
                }
            }
            ReportLoaded++;
            CrashDataRaw[index] = data;
        }
        
        public void ProcessData()
        {
            // Sumup Devices
            for( int index = 0; index < ReportLoaded; index++)
            {
                CrashData data = CrashDataRaw[index];
                bool isAdded = false;
                foreach (Device device in DevicesList)
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
                    DevicesList.Add(device);
                }
            }
        }

        public string[] GetBacktraceByID(int ID)
        {
            if (ID == -1)
                return null;

            foreach (var issue in IssueList)
            {
                if (issue.ID == ID)
                {
                    return issue.Stactrace;
                }
            }
            return null;
        }
        public CrashData GetCrashDataIndex(int index, ref bool found)
        {
            if (index >=0 && index <= ReportLoaded)
            {
                found = true;
                return CrashDataRaw[index];
            }
            found = false;
            return CrashDataRaw[0];
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
                CrashDataRaw,
                IssueList,
                DevicesList
            });
            File.WriteAllText(path, json_final);
        }
        public void LoadDataFromFile(string path)
        {
            if (File.Exists(path))
            {
                CrashDataRaw = null;
                IssueList = null;
                DevicesList = null;

                string json = File.ReadAllText(path);

                RootObject data = JsonConvert.DeserializeObject<RootObject>(json);

                CrashDataRaw = data.CrashReportRaw;

                IssueList = data.IssuesList;

                DevicesList = data.DevicesList;

                if (CrashDataRaw != null && IssueList != null && DevicesList != null)
                {
                    ReportLoaded = CrashDataRaw.Count();
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
