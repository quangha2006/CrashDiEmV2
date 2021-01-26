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
            public string Name;
            public List<ModelName> ModelNames; // how about map?
            public bool Add(ModelName model)
            {
                if (ModelNames.Any(x => x.Name == model.Name))
                {
                    ModelName currModelName = ModelNames.First(x => x.Name == model.Name);
                    return currModelName.Add(model.GetFirstDevice());
                }
                else
                    ModelNames.Add(model);
                return true;
            }
        }
        private struct ModelName
        {
            public string Name;
            public List<DeviceName> DeviceNames;
            public bool Add(DeviceName device)
            {
                if (DeviceNames.Any(x => x.Name == device.Name))
                {
                    DeviceName currDevice = DeviceNames.First(x => x.Name == device.Name);
                    return currDevice.Add(device.GetFirstDevice());
                }
                else
                    DeviceNames.Add(device);
                return true;
            }
            public DeviceName GetFirstDevice()
            {
                return DeviceNames.ElementAt(0);
            }
        }
        private struct DeviceName
        {
            public string Name;
            public List<ModelCode> ModelCodes;
            public bool Add(ModelCode modelcode)
            {
                if (ModelCodes.Any(x => x.Name == modelcode.Name))
                {
                    //ModelCode modelCode = ModelCodes.First(x => x.Name == modelcode.Name);
                    return false;
                    //Console.WriteLine("Warning!: Duplicate model code {0}", modelcode.Name);
                }
                else
                    ModelCodes.Add(modelcode);
                return true;
            }
            public ModelCode GetFirstDevice()
            {
                return ModelCodes.ElementAt(0);
            }
        }
        private struct ModelCode
        {
            public string Name;
            public DeviceSpec Spec;
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
                    if (lines.Length != 11)
                    {
                        Console.WriteLine("ERROR!: {0}", line);
                        continue;
                    }
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

                    DeviceSpec deviceSpec = new DeviceSpec
                    {
                        GPU             = gpu,
                        FormFactor      = formfactor, 
                        SystemOnChip    = sysonchip,
                        TotalMem        = totalmem,
                        ABIs            = abis,
                        OpenGLESVer     = glesversion,
                        SDKs            = sdk
                    };

                    ModelCode modelCode = new ModelCode
                    {
                        Name = modelcode,
                        Spec = deviceSpec
                    };

                    DeviceName deviceName = new DeviceName
                    {
                        Name = devicename,
                        ModelCodes = new List<ModelCode> { modelCode }
                    };

                    ModelName modelName = new ModelName
                    {
                        Name = modelname,
                        DeviceNames = new List<DeviceName> { deviceName }
                    };
                    // find manufac
                    if (Manufacturers.Any(x => x.Name == manufac))
                    {
                        Manufacturer Manufac = Manufacturers.First(x => x.Name == manufac);
                        if (!Manufac.Add(modelName))
                        {
                            Console.WriteLine("Warning!: Duplicate device {0}", line);
                            Console.WriteLine("Warning!: at Manufacturers[{0}]", line, Manufacturers.IndexOf(Manufac));
                        }
                    }
                    else
                    {
                        Manufacturer newManufac = new Manufacturer
                        {
                            Name = manufac,
                            ModelNames = new List<ModelName> { modelName }
                        };
                        Manufacturers.Add(newManufac);
                    }    

                }
            }
            return false;
        }
    }
}
