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
        public CrashType        CrashType       { set; get; }
        public string           FolderName      { set; get; }
        public StackTraceLine[] Stacktracelines { set; get; }
        public int              AddressHashCode { set; get; }
        public int              ID              { set; get; }
        public List<int>        DeviceIndex     { set; get; }
        private string[] Stactrace = null;

        public CrashReport()
        {
            DeviceIndex = new List<int>();
        }
        public string[] GetStactrace()
        {
            if (Stactrace == null)
            {
                Stactrace = new string[Stacktracelines.Length];

                for (int i =0; i < Stacktracelines.Length; i++)
                {
                    Stactrace[i] = $"{Stacktracelines[i].LineIndex} {Stacktracelines[i].SOPath} {Stacktracelines[i].Function} {Stacktracelines[i].SourceFile}";
                }
            }
            return Stactrace;
        }
        public void SetStackTraceLines(string[] stacktracelines)
        {
            Stactrace = stacktracelines;

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

                    if (lineSplit[7].Contains(".so"))
                    {
                        Stacktracelines[i].SOPath = lineSplit[7];

                        therestofLine = therestofLine.Substring(therestofLine.IndexOf(".so") + 3).Trim();
                    }
                    else if (lineSplit[7].Contains(".apk"))
                    {
                        Stacktracelines[i].SOPath = lineSplit[7];

                        therestofLine = therestofLine.Substring(therestofLine.IndexOf(".apk") + 4).Trim();
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
