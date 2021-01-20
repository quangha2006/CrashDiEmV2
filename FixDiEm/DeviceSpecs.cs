using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixDiEm
{
    class DeviceSpecs
    {
        public struct DeviceSpec
        {
            public string ModelCode;
            public string GPU;
            public string FormFactor;
            public string SystemOnChip;
            public string TotalMem;
            public string ABIs;
            public string OpenGLESVer;
            public string SDKs;
        }
        private struct Manufacturer
        {
            string Name;
            List<ModelName> ModelNames;
            public void Add()
            {

            }
        }
        private struct ModelName
        {
            string Name;
            List<DeviceName> DeviceNames;
        }
        private struct DeviceName
        {
            string Name;
            DeviceSpec Spec;
        }
        private List<Manufacturer> Manufacturers = new List<Manufacturer>();

        public bool LoadFormFile(string filePath)
        {
            if (File.Exists(filePath))
            {

                StreamReader file = new StreamReader(filePath);
                //Read first line to check format
                string line = file.ReadLine();
                if (line.Split(',').Length != 11)
                    return false;

                while ((line = file.ReadLine()) != null)
                {
                    string[] lines = line.Split(',');

                    string manufac      = lines[0];
                    string modelname    = lines[1];
                    string devicename   = lines[2];
                    string modelcode    = lines[3];
                    string gpu          = lines[4];
                    string formfactor   = lines[5];
                    string sysonchip    = lines[6];
                    string totalmem     = lines[7];
                    string abis         = lines[8];
                    string glesversion  = lines[9];
                    string sdk          = lines[10];

                    DeviceSpec devicespec = new DeviceSpec
                    {
                        ModelCode       = modelcode,
                        GPU             = gpu,
                        FormFactor      = formfactor, 
                        SystemOnChip    = sysonchip,
                        TotalMem        = totalmem,
                        ABIs            = abis,
                        OpenGLESVer     = glesversion,
                        SDKs            = sdk
                    };
                    
                }
            }
            return false;
        }
    }
}
