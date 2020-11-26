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

        public int LoadCrashLogs(BackgroundWorker backgroundWorker, AppSettings setting)
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

            foreach (string file in List_files)
            {
                string contents = File.ReadAllText(file);

                if (ConvertData(ref contents, file.Remove(0, LogCrashPath.Length), setting, index))
                    backgroundWorker.ReportProgress(++index);

                if (backgroundWorker.CancellationPending)
                {
                    break;
                }
            }

            long milliseconds_3 = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            
            Console.WriteLine("Time to load: {0}, files: {1}", milliseconds_3 - milliseconds_1, ReportLoaded);
            return ReportLoaded;
        }
        private bool ConvertData(ref string fileContent, string path, AppSettings setting, int index)
        {
            Regex reg = setting.ReportFileStructureRegex;

            MatchCollection result = reg.Matches(fileContent);

            if (result.Count == 0)
            {
                Console.WriteLine("Can't parse file: {0}", path);
                return false;
            }
            var type         = result[0].Groups["crashtype"].Value.Trim();
            var appcode      = result[0].Groups["appcode"].Value.Trim();
            var Datetime     = result[0].Groups["datetime"].Value.Trim().Split(',');
            var versioncode  = result[0].Groups["versioncode"].Value.Trim();
            var versionname  = result[0].Groups["versionname"].Value.Trim();
            var devicemodel  = result[0].Groups["devicemodel"].Value.Trim();
            var devicename   = result[0].Groups["devicename"].Value.Trim();
            var devicebrand  = result[0].Groups["devicebrand"].Value.Trim();
            var apilevel     = result[0].Groups["apilevel"].Value.Trim();
            var architec     = result[0].Groups["architecture"].Value.Trim();
            var lines        = result[0].Groups["stacktrace"].Value.Trim().Split('\n');
            var folderName   = Path.GetFileName(Path.GetDirectoryName(path));
            var datetime     = new DateTime();

            if (type != "NATIVE CRASH" && type != "JAVA CRASH")
            {
                Console.WriteLine("Not a report file: {0}", path);
                return false;
            }
            //Parse DateTime
            if (Datetime.Length > 0)
            {
                string[] regexs = { "dd-MM-yyyy", "MMM dd", "MMM d" };
                string st_datetime = Datetime[0].Trim();
                foreach (var regex in regexs)
                {
                    if (DateTime.TryParseExact(st_datetime, regex, null, System.Globalization.DateTimeStyles.None, out datetime))
                        break;
                }

                if (Datetime.Length > 1)
                {
                    string[] regexs2 = { "hh:mm tt", "h:mm tt" };
                    string st_datetime2 = Datetime[1].Trim();

                    foreach (var regex in regexs2)
                    {
                        if (DateTime.TryParseExact(st_datetime2, regex, null, System.Globalization.DateTimeStyles.None, out DateTime clocktime))
                        {
                            datetime = datetime.AddHours(clocktime.Hour);
                            datetime = datetime.AddMinutes(clocktime.Minute);
                            break;
                        }
                    }
                }
            }

            int.TryParse(apilevel, out int APIlevel);

            CrashData data = new CrashData(path)
            {
                CrashType = type == "NATIVE CRASH" ? CrashType.NATIVE : type == "JAVA CRASH" ? CrashType.JAVA : CrashType.NONE,

                AppCode = appcode,
                DateTime = datetime,
                VersionCode = versioncode,
                VersionName = versionname,
                DeviceModel = devicemodel,
                DeviceName = devicename,
                DeviceBrand = devicebrand,
                APILevel = APIlevel
            };

            switch(architec.Trim())
            {
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
                case "N/A":
                default:
                    data.architecture = Architecture.Unknow;
                    break;
            }
            if (data.CrashType == CrashType.NATIVE)
            {
                // Find where is backtrace?
                int backtraceBeginLineIndex = 0;
                foreach (string line in lines)
                {
                    if (line == "backtrace:" || line == "Stacktrace:") //Stacktrace is new version
                        break;
                    backtraceBeginLineIndex++;
                }
                // process backtrace:
                int numlineBacktrace = lines.Count() - backtraceBeginLineIndex - 1;

                string[] backtraceData = new string[numlineBacktrace];
                for (int i = 0; i < numlineBacktrace; i++)
                {
                    string currentLine = lines[i + backtraceBeginLineIndex + 1];
                    //string finalCurrentLine = currentLine;
                    //if (currentLine.Length > 0 && setting.IsRemoveSOPath) // Clear SO Path
                    //{
                        
                    //     Regex regex = setting.SoPathRegex;
                    //     Match resultSoPathRegex = regex.Match(currentLine);

                    //     if (resultSoPathRegex != Match.Empty)
                    //     {
                    //         finalCurrentLine = currentLine.Remove(resultSoPathRegex.Index, resultSoPathRegex.Length);
                    //     }
                        
                    //}
                    backtraceData[i] = currentLine;
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
                    CrashReport issuedata = new CrashReport
                    {
                        AddressHashCode = hashCode,
                        ID = IssueList.Count(),
                        FolderName = folderName,
                        CrashType = CrashType.NATIVE
                    };
                    issuedata.DeviceIndex.Add(index);
                    issuedata.SetStackTraceLines(backtraceData);

                    foreach (var line in issuedata.Stacktracelines) // Get issue name
                    {
                        if (!String.IsNullOrEmpty(line.Function))
                        {
                            issuedata.Name = line.Function;
                            break;
                        }
                    }
                    if (String.IsNullOrEmpty(issuedata.Name))
                    {
                        foreach (var line in issuedata.Stacktracelines) // Get issue name
                        {
                            if (!String.IsNullOrEmpty(line.SOPath))
                            {
                                if (line.SOPath.Contains(".apk"))
                                { 
                                    issuedata.Name = line.SOPath.Substring(line.SOPath.LastIndexOf('/') + 1);
                                    break;
                                }else if (line.SOPath.Contains(".so"))
                                {
                                    issuedata.Name = line.SOPath.Substring(line.SOPath.LastIndexOf('/') + 1);
                                    break;
                                }
                            }
                        }
                    }
                    IssueList.Add(issuedata);
                    data.IssueID = issuedata.ID;

                }
            } // end native crash
            else // Java Crash
            {
                int backtraceBeginLine = 0;

                foreach (var line in lines)
                {
                    if (string.IsNullOrEmpty(line))
                        backtraceBeginLine++;
                    else
                        break;
                }

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
                            ID = IssueList.Count(),
                            FolderName = folderName,
                            CrashType = CrashType.JAVA
                        };
                        issuedata.DeviceIndex.Add(index);
                        issuedata.SetStackTraceLines(backtraceData);
                        IssueList.Add(issuedata);
                        data.IssueID = issuedata.ID;
                    }
                }
            }
            ReportLoaded++;
            CrashDataRaw[index] = data;
            return true;
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
                    return issue.GetStactrace();
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
