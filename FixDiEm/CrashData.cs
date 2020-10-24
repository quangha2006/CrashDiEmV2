using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixDiEm
{
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

    class CrashData
    {
        public string Path;
        public CrashType CrashType;
        public string AppCode;
        public DateTime DateTime;
        public string VersionCode;
        public string VersionName;
        public string DeviceModel;
        public string DeviceName;
        public string DeviceBrand;
        public Int32 APILevel;
        public Architecture architecture;
        public int IssueID;
        public CrashData(string path)
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
}
