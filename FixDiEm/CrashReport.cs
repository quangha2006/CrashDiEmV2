using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace FixDiEm
{
    public struct StackTraceLine
    {
        public string FullLine;
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
        private bool isShowAddress = true;
        private bool isShowSOPath = true;

        public CrashReport()
        {
            DeviceIndex = new List<int>();
        }
        public string[] GetStactrace(AppSettings settings)
        {
            if (Stactrace == null || isShowAddress != settings.IsShowCrashAddress || isShowSOPath != !settings.IsRemoveSOPath)
            {

                Stactrace = new string[Stacktracelines.Length];

                isShowAddress = settings.IsShowCrashAddress;
                isShowSOPath = !settings.IsRemoveSOPath; // ><

                if (isShowAddress && isShowSOPath || CrashType == CrashType.JAVA) // Get full stacktrace
                    for (int i =0; i < Stacktracelines.Length; i++)
                    {
                        Stactrace[i] = Stacktracelines[i].FullLine;
                    }
                else
                    for (int i = 0; i < Stacktracelines.Length; i++)
                    {
                        string address = isShowAddress ? Stacktracelines[i].CrashAddress: "";
                        string sopath = Stacktracelines[i].SOPath;
                        string space = " ";
                        if (!isShowSOPath)
                        {
                            var sopathRegex = settings.GameSoPathRegex.ToString();

                            if (Regex.IsMatch(sopath, sopathRegex))
                            {
                                sopath = "";
                                space = "";// Need opt here
                            }
                        }
                        string line = $"{Stacktracelines[i].LineIndex} {address} {sopath}{space}{Stacktracelines[i].Function} {Stacktracelines[i].SourceFile}";
                        Stactrace[i] = line;
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

                Stacktracelines[i].FullLine = line;

                if (CrashType == CrashType.JAVA)
                    continue;

                string[] lineSplit = line.Split(' ');
                Stacktracelines[i].LineIndex = $"{lineSplit[2]} {lineSplit[4]}";
                Stacktracelines[i].CrashAddress = lineSplit[5];

                if (lineSplit.Length > 7)
                {
                    string therestofLine = line.Substring(line.IndexOf(Stacktracelines[i].CrashAddress)+ Stacktracelines[i].CrashAddress.Length).TrimStart();
                    // Need re-check here, Cause we still have .oat
                    /*if (lineSplit[7].Contains(".so"))
                    {
                        Stacktracelines[i].SOPath = lineSplit[7];

                        therestofLine = therestofLine.Substring(therestofLine.IndexOf(".so") + 3).Trim();
                    }
                    else if (lineSplit[7].Contains(".apk"))
                    {
                        Stacktracelines[i].SOPath = lineSplit[7];

                        therestofLine = therestofLine.Substring(therestofLine.IndexOf(".apk") + 4).Trim();
                    }*/
                    if (lineSplit[7][0] == '/')
                    {
                        Stacktracelines[i].SOPath = lineSplit[7];

                        int indexSpace = therestofLine.IndexOf(' ');
                        if (indexSpace != -1)
                            therestofLine = therestofLine.Substring(indexSpace + 1).Trim();
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
