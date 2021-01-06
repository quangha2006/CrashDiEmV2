using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FixDiEm
{
    public partial class SettingForm : Form
    {
        private AppSettings currentSetting;
        public SettingForm()
        {
            InitializeComponent();
        }
        public void SetSetting(AppSettings setting)
        {
            currentSetting = setting;
            //Apply Data
            textBox_SoPath.Text = currentSetting.SoPathRegex.ToString();
            textBox_GameSoPath.Text = currentSetting.GameSoPathRegex.ToString();
            textBox_ReportFileStructure.Text = currentSetting.ReportFileStructureRegex.ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
