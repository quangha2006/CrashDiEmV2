using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixDiEm
{
    class CrashReport
    {
        private string m_Name;
        private int m_ID;
        private List<int> m_DeviceIndex;
        private string m_AddressList;
        private string m_FolderName;
        private string[] m_Stactrace;

        public CrashReport()
        {
            m_DeviceIndex = new List<int>();
        }

        public string Name
        {
            set
            {
                m_Name = value;
            }
            get
            {
                return m_Name;
            }
        }
        public int ID
        {
            set
            {
                m_ID = value;
            }
            get
            {
                return m_ID;
            }
        }
        // tmp remove this, cause cannot save json
        //public ref List<int> DeviceIndexRef
        //{
        //    //set
        //    //{
        //    //    m_DeviceIndex = value;
        //    //}
        //    get
        //    {
        //        return ref m_DeviceIndex;
        //    }
        //}
        public List<int> DeviceIndex
        {
            set
            {
                m_DeviceIndex = value;
            }
            get
            {
                return m_DeviceIndex;
            }
        }
        public string AddressList
        {
            set
            {
                m_AddressList = value;
            }
            get
            {
                return m_AddressList;
            }
        }
        public string FolderName
        {
            set
            {
                m_FolderName = value;
            }
            get
            {
                return m_FolderName;
            }
        }
        public string[] Stactrace
        {
            set
            {
                m_Stactrace = value;
            }
            get
            {
                return m_Stactrace;
            }
        }
    }
}
