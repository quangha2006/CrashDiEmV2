using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace FixDiEm
{
    class AppSettings
    {
        private struct AppSetting
        {
            public string ArmSoPath;
            public string Arm64SoPath;
            public string X86SoPath;
            public string X86_64SoPath;
            public string CrashLogsPath;
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
                Settings = JsonConvert.DeserializeObject<AppSetting>(json);
                return true;
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
    }
}
