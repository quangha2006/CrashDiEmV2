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
        public string Offset; // Do we need to store it??, maybe not!
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
                string line = stacktracelines[i];

                if (line.Length <= 0)
                    continue;

                string[] lineSplit = line.Split(' ');
                Stacktracelines[i].LineIndex = $"{lineSplit[2]} {lineSplit[4]}";
                Stacktracelines[i].CrashAddress = lineSplit[5];

                if (lineSplit.Length > 7)
                {
                    string therestofLine = line.Substring(line.IndexOf(Stacktracelines[i].CrashAddress)+ Stacktracelines[i].CrashAddress.Length).TrimStart();

                    if (lineSplit[7].Contains(".so")) // Need implement with .apk
                    {
                        Stacktracelines[i].SOPath = lineSplit[7];

                        therestofLine = therestofLine.Substring(therestofLine.IndexOf(".so") + 3).Trim();
                    }

                    string offset = therestofLine.IndexOf("(offset") == 0 ? therestofLine.Substring(0, therestofLine.IndexOf(')') + 1) : "";
                    Stacktracelines[i].Offset = offset;

                    if (offset.Length > 0)
                        therestofLine = therestofLine.Remove(0, offset.Length).TrimStart();

                    if (therestofLine.Contains("(SourceCode:"))
                    {
                        var indexSource = therestofLine.IndexOf("(SourceCode:");
                        Stacktracelines[i].SourceFile = therestofLine.Substring(indexSource);
                        therestofLine = therestofLine.Remove(indexSource).Trim();
                    }
                    Stacktracelines[i].Function = therestofLine;
                }
            }
        }
    }
}
