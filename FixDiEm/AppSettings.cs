using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace FixDiEm
{
    public class AppSettings
    {
        //Why I need to create this struct? Because I have no way to make a json without it.
        private struct AppSetting
        {
            public string ArmSoPath;
            public string Arm64SoPath;
            public string X86SoPath;
            public string X86_64SoPath;
            public string CrashLogsPath;
            public bool IsShowSOPath;
            public bool IsGroupIssueByGoogle;
            public bool IsParseDsym;
            public bool IsShowCrashAddress;
            public int NumLineToShow;
            public Regex SoPathRegex;
            public Regex GameSoPathRegex;
            public Regex ReportFileStructureRegex;
        }
        
        private AppSetting Settings;

        public void SaveToFile(string saveFileName)
        {
            string json = JsonConvert.SerializeObject(Settings);
            File.WriteAllText(saveFileName, json);
        }
        public bool LoadSettings(string saveFileName)
        {
            if (File.Exists(saveFileName))
            {
                string json = File.ReadAllText(saveFileName);
                try
                {
                    Settings = JsonConvert.DeserializeObject<AppSetting>(json);
                    return true;
                }
                catch (Exception e)
                {
                    File.Delete(saveFileName);
                    Console.WriteLine("Exception when read {0}: {1}", saveFileName, e.ToString());
                }
            }
            return false;
        }
        public string ArmSoPath
        {
            set
            {
                Settings.ArmSoPath = value;
            }
            get
            {
                return Settings.ArmSoPath;
            }
        }
        public string Arm64SoPath
        {
            set
            {
                Settings.Arm64SoPath = value;
            }
            get
            {
                return Settings.Arm64SoPath;
            }
        }
        public string X86SoPath
        {
            set
            {
                Settings.X86SoPath = value;
            }
            get
            {
                return Settings.X86SoPath;
            }
        }
        public string X86_64SoPath
        {
            set
            {
                Settings.X86_64SoPath = value;
            }
            get
            {
                return Settings.X86_64SoPath;
            }
        }
        public string CrashLogPath
        {
            set
            {
                Settings.CrashLogsPath = value;
            }
            get
            {
                return Settings.CrashLogsPath;
            }
        }
        public bool IsShowSOPath
        {
            set
            {
                Settings.IsShowSOPath = value;
            }
            get
            {
                return Settings.IsShowSOPath;
            }
        }
        public bool IsGroupIssueByGoogle
        {
            set
            {
                Settings.IsGroupIssueByGoogle = value;
            }
            get
            {
                return Settings.IsGroupIssueByGoogle;
            }
        }
        public bool IsParseDsym
        {
            set
            {
                Settings.IsParseDsym = value;
            }
            get
            {
                return Settings.IsParseDsym;
            }
        }
        public bool IsShowCrashAddress
        {
            set
            {
                Settings.IsShowCrashAddress = value;
            }
            get
            {
                return Settings.IsShowCrashAddress;
            }
        }
        public int NumLineToShow
        {
            set
            {
                Settings.NumLineToShow = value;
            }
            get
            {
                return Settings.NumLineToShow;
            }
        }
        public Regex SoPathRegex
        {
            set
            {
                Settings.SoPathRegex = value;
            }
            get
            {
                return Settings.SoPathRegex;
            }
        }
        public Regex GameSoPathRegex
        {
            set
            {
                Settings.GameSoPathRegex = value;
            }
            get
            {
                return Settings.GameSoPathRegex;
            }
        }
        public Regex ReportFileStructureRegex
        {
            set
            {
                Settings.ReportFileStructureRegex = value;
            }
            get
            {
                return Settings.ReportFileStructureRegex;
            }
        }
    }
}
