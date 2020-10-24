using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixDiEm
{
    class Device
    {
        public string DeviceBrand;
        public string DeviceName;
        public string DeviceModel;
        public List<int> CrashLogIndex;
        public Device(string devicebrand, string devicename, string devicemodel)
        {
            this.DeviceBrand = devicebrand;
            this.DeviceName = devicename;
            this.DeviceModel = devicemodel;
            CrashLogIndex = new List<int>();
        }
    }
}
