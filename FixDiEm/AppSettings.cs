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
        private struct appSetting
        {
            public string ArmSoPath;
            public string Arm64SoPath;
            public string X86SoPath;
            public string X86_64SoPath;
            public string CrashLogsPath;
        }

        private appSetting m_Settings;// = new appSetting();
        public string m_SaveFileName = "appSetting.json";
        public void SaveToFile()
        {
            string json = JsonConvert.SerializeObject(m_Settings);
            File.WriteAllText(m_SaveFileName, json);
        }
        public bool LoadSettings()
        {
            if (File.Exists(m_SaveFileName))
            {
                string json = File.ReadAllText(m_SaveFileName);
                m_Settings = JsonConvert.DeserializeObject<appSetting>(json);
                return true;
            }
            return false;
        }
        public string ArmSoPath
        {
            set
            {
                m_Settings.ArmSoPath = value;
            }
            get
            {
                return m_Settings.ArmSoPath;
            }
        }
        public string Arm64SoPath
        {
            set
            {
                m_Settings.Arm64SoPath = value;
            }
            get
            {
                return m_Settings.Arm64SoPath;
            }
        }
        public string X86SoPath
        {
            set
            {
                m_Settings.X86SoPath = value;
            }
            get
            {
                return m_Settings.X86SoPath;
            }
        }
        public string X86_64SoPath
        {
            set
            {
                m_Settings.X86_64SoPath = value;
            }
            get
            {
                return m_Settings.X86_64SoPath;
            }
        }
        public string CrashLogPath
        {
            set
            {
                m_Settings.CrashLogsPath = value;
            }
            get
            {
                return m_Settings.CrashLogsPath;
            }
        }
    }
}
