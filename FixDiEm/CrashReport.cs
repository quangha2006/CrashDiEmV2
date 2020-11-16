using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixDiEm
{
    public struct StackTraceLine
    {
        public string LineIndex;
        public string CrashAddress;
        public string SOPath;
        public string Offset;
        public string Function;
        public string SourceFile;
    }
    class CrashReport
    {
        public string           Name            { set; get; }
        public string           FolderName      { set; get; }
        public string[]         Stactrace       { set; get; }
        public StackTraceLine[] Stacktracelines { set; get; }
        public int              AddressHashCode { set; get; }
        public int              ID              { set; get; }
        public List<int>        DeviceIndex     { set; get; }

        public CrashReport()
        {
            DeviceIndex = new List<int>();
        }
        public void SetStackTraceLines(string[] stacktracelines)
        {
            Stacktracelines = new StackTraceLine[stacktracelines.Count()];
            for(int i = 0; i < stacktracelines.Count(); i++)
            {
                string[] line = stacktracelines[i].Split(' ');
                Stacktracelines[i].LineIndex = line[2] + " " + line[4];
                Stacktracelines[i].CrashAddress = line[5];

                if (line.Length > 7 && line[7].Contains(".so"))
                {
                    Stacktracelines[i].SOPath = line[7];
                    Console.WriteLine(Stacktracelines[i].SOPath);
                }

            }
        }
    }
}
