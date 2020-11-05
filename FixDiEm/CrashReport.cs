using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixDiEm
{
    class CrashReport
    {
        public string       Name            { set; get; }
        public string       FolderName      { set; get; }
        public string[]     Stactrace       { set; get; }
        public int          AddressHashCode { set; get; }
        public int          ID              { set; get; }
        public List<int>    DeviceIndex     { set; get; }

        public CrashReport()
        {
            DeviceIndex = new List<int>();
        }
    }
}
