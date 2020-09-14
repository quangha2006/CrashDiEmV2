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
            public string m_ArmSoPath;
            public string m_Arm64SoPath;
            public string m_X86SoPath;
            public string m_X86_64SoPath;
            public string m_CrashLogsPath;
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
                m_Settings.m_ArmSoPath = value;
            }
            get
            {
                return m_Settings.m_ArmSoPath;
            }
        }
        public string Arm64SoPath
        {
            set
            {
                m_Settings.m_Arm64SoPath = value;
            }
            get
            {
                return m_Settings.m_Arm64SoPath;
            }
        }
        public string X86SoPath
        {
            set
            {
                m_Settings.m_X86SoPath = value;
            }
            get
            {
                return m_Settings.m_X86SoPath;
            }
        }
        public string X86_64SoPath
        {
            set
            {
                m_Settings.m_X86_64SoPath = value;
            }
            get
            {
                return m_Settings.m_X86_64SoPath;
            }
        }
        public string CrashLogPath
        {
            set
            {
                m_Settings.m_CrashLogsPath = value;
            }
            get
            {
                return m_Settings.m_CrashLogsPath;
            }
        }
    }
}
