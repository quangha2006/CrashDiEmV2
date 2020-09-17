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

namespace FixDiEm
{
    class AnalyzeData
    {
        public struct MySetting
        {
            public bool ParseDsym;
            public bool RemoveSOPath;
        }
        public enum Architecture
        {
            Unknow,
            armeabi,
            armeabi_v7a,
            arm64_v8a,
            x86,
            x86_64
        }
        public enum CrashType
        {
            Native_Crash,
            JAVA_Crash
        }

        public struct CrashData
        {
            public string Path;
            public CrashType CrashType;
            public string AppCode;
            public string DateTime;
            public string VersionCode;
            public string VersionName;
            public string DeviceModel;
            public string DeviceName;
            public string DeviceBrand;
            public Int32 APILevel;
            public Architecture architecture;
            public int IssueID;
            public CrashData(string path) : this()
            {
               Path = path;
               APILevel = 0;
               IssueID = -1;
            }
            public string GetArchitectureAsString()
            {
                switch (this.architecture)
                {
                    case Architecture.arm64_v8a:
                        return "arm64-v8a";
                    case Architecture.armeabi_v7a:
                        return "armeabi-v7a";
                    case Architecture.x86:
                        return "x86";
                    case Architecture.x86_64:
                        return "x86_64";
                    case Architecture.Unknow:
                    default:
                        return "Unknow";
                }
            }
        }
        public struct Device
        {
            public string DeviceBrand;
            public string DeviceName;
            public string DeviceModel;
            public List<int> CrashLogIndex;
            public Device(string devicebrand, string devicename, string devicemodel)  : this()
            {
                this.DeviceBrand = devicebrand;
                this.DeviceName = devicename;
                this.DeviceModel = devicemodel;
                CrashLogIndex = new List<int>();
            }
        }

        private string m_DataPath;
        private CrashData[] m_CrashDataRaw;
        private List<CrashReport> m_issueList;
        private int m_numTxtFiles = 0;
        private int m_numReportLoaded = 0;
        private List<Device> m_DevicesList = new List<Device>(); 

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
        public List<AnalyzeData.Device> DevicesList
        {
            get
            {
                return m_DevicesList;
            }
        }
        public ref List<AnalyzeData.Device> DevicesListRef
        {
            get
            {
                return ref m_DevicesList;
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
        public int LoadCrashLogs(BackgroundWorker backgroundWorker, MySetting setting)
        {
            if (this.TxtCount < 0)
                return 0;

            ClearAndReInitData();

            int index = 0;
            string[] List_files;
            long totalTime = 0;

            if (Directory.Exists(m_DataPath))
            {
                List_files = Directory.GetFiles(m_DataPath, "*.txt", SearchOption.AllDirectories);
                long milliseconds_start = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                foreach (string file in List_files)
                {
                    string contents = File.ReadAllText(file);
                    string[] lines = contents.Split('\n');
                    if (lines[0] == "NATIVE CRASH" || lines[0] == "JAVA CRASH")
                    {
                        ConvertData(ref lines, file, setting, index);
                        index++;
                        backgroundWorker.ReportProgress(index);
                    }
                    if (backgroundWorker.CancellationPending)
                    {
                        //Need clear data here!
                        m_CrashDataRaw = null;
                        return 0;
                    }
                }
                long milliseconds_end = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                totalTime += (milliseconds_end - milliseconds_start);
            }
            
            Console.WriteLine("Time to load " + this.ReportLoaded + " files: " + totalTime);
            return this.ReportLoaded;
        }
        private void ConvertData(ref string[] lines, string path, MySetting setting, int index)
        {
            string type = lines[0];
            string appcode = lines[2].Substring(lines[2].IndexOf(':') + 2);
            string Datetime = lines[3];
            string Versioncode = lines[4];
            string Versionname = lines[5];
            string Devicemodel = lines[6];
            string Devicename = lines[7];
            string Devicebrand = lines[8];
            string APIlevel = lines[9];
            string architecture = lines[10];
            string folderName = Path.GetFileName(Path.GetDirectoryName(path));
            CrashData data = new CrashData(path);

            data.CrashType = type == "NATIVE CRASH" ? CrashType.Native_Crash : CrashType.JAVA_Crash;

            data.AppCode = appcode;

            data.DateTime = Datetime.Substring(Datetime.IndexOf(':') + 2);
            data.VersionCode = Versioncode.Substring(Versioncode.IndexOf(':') + 2);
            data.VersionName = Versionname.Substring(Versionname.IndexOf(':') + 2);
            data.DeviceModel = Devicemodel.Substring(Devicemodel.IndexOf(':') + 2);
            data.DeviceName = Devicename.Substring(Devicename.IndexOf(':') + 2);
            data.DeviceBrand = Devicebrand.Substring(Devicebrand.IndexOf(':') + 2);
            try
            {
                data.APILevel = Int32.Parse(APIlevel.Substring(APIlevel.IndexOf(':') + 2));
            }
            catch (FormatException)
            {
                Console.WriteLine("FormatException: value: " + APIlevel + " Path:" + path);
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

                    if (setting.RemoveSOPath) // Clear SO Path
                    {
                        if (currentLine.Contains("offset"))
                        {
                            int start = currentLine.IndexOf('/');
                            int end = currentLine.IndexOf(')');
                            int len = end - start + 2;
                            if (end < (currentLine.Length - 1))
                            {
                                backtraceData[i] = currentLine.Remove(start, len);
                            }
                            else
                                backtraceData[i] = currentLine;
                        }
                        else if (currentLine.Contains("libAsphalt9.so"))
                        {
                            int start = currentLine.IndexOf('/');
                            int end = currentLine.IndexOf('(');
                            int len = end - start;
                            if (len > 0)
                                backtraceData[i] = currentLine.Remove(start, len);
                            else
                                backtraceData[i] = currentLine;
                        }
                        else
                        {
                            backtraceData[i] = currentLine;
                        }
                    }
                    else
                    {
                        backtraceData[i] = currentLine;
                    }
                }
                //Check issue and add to list
                //Get AddressString
                string addressString = "";
                foreach (string line in backtraceData)
                {
                    string[] splitLine = line.Split(' ');
                    addressString += splitLine[5];
                }
                bool isNewIssue = true;
                for(int i = 0; i < m_issueList.Count(); i++)
                {
                    if (m_issueList[i].AddressList == addressString && m_issueList[i].FolderName == folderName)
                    {
                        data.IssueID = m_issueList[i].ID;

                        m_issueList[i].DeviceIndex.Add(index);
                        if (Path.GetFileName(Path.GetDirectoryName(path)) == "CrashTop_2")
                        {
                            Console.WriteLine("Add device to " + i + "Issue Folder: " + m_issueList[i].FolderName + " Currrent device: " + m_issueList[i].DeviceIndex.Count);
                        }
                        isNewIssue = false;
                        break;
                    }
                }
                if (isNewIssue)
                {
                    CrashReport issuedata = new CrashReport();
                    issuedata.AddressList = addressString;
                    issuedata.Stactrace = backtraceData;
                    issuedata.ID = m_issueList.Count();
                    issuedata.FolderName = Path.GetFileName(Path.GetDirectoryName(path));
                    issuedata.Name = issuedata.FolderName;
                    issuedata.DeviceIndex.Add(index);
                    m_issueList.Add(issuedata);
                    data.IssueID = issuedata.ID;
                    if (issuedata.FolderName == "CrashTop_2")
                    {
                        Console.WriteLine("Create new issue in folder CrashTop_2 ID = " + issuedata.ID);
                    }
                }
            } // end native crash
            else // Java Crash
            {
                int backtraceBeginLine = 0;
                foreach (string line in lines) // Can find begin line 10, opt late!
                {
                    if (line.Contains("java."))
                        break;
                    backtraceBeginLine++;
                }
                if (backtraceBeginLine == lines.Count())
                {
                    Console.WriteLine("Not found backtrace in " + path);
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
                    bool isNewIssue = true;
                    for (int i = 0; i < m_issueList.Count(); i++)
                    {
                        if (m_issueList[i].AddressList == addressString && m_issueList[i].FolderName == folderName)
                        {
                            data.IssueID = m_issueList[i].ID;

                            m_issueList[i].DeviceIndex.Add(index);

                            isNewIssue = false;
                            break;
                        }
                    }
                    if (isNewIssue)
                    {
                        CrashReport issuedata = new CrashReport();
                        issuedata.Name = backtraceData[0];
                        issuedata.AddressList = addressString;
                        issuedata.Stactrace = backtraceData;
                        issuedata.ID = m_issueList.Count();
                        issuedata.FolderName = Path.GetFileName(Path.GetDirectoryName(path));
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
        public ref CrashData[] CrashReportRawRef
        {
            get
            {
                return ref m_CrashDataRaw;
            }
        }

    }
}
