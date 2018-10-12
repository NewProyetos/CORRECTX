using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sinconizacion_EXactus.CORECTX_APP.Reports
{
    public partial class analiview : Form
    {
        public analiview()
        {
            InitializeComponent();
        }
        //public Uri DashboardUri { get; set; }
        String Selected_File;

        private void button1_Click(object sender, EventArgs e)
        {
            dashboardViewer1.DashboardSource = new Uri(Selected_File, UriKind.Relative);
            
        }

        private void analiview_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Selected_File = string.Empty;
            this.textBox1.Clear();
            openFileDialog1.AutoUpgradeEnabled = false;
            openFileDialog1.InitialDirectory = @"%USERPROFILE%\Documents";
            openFileDialog1.Title = "Select a File";
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.Filter = "Xml files|*.xml";

            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {

            }
            else
            {
                Selected_File = openFileDialog1.FileName;
                this.textBox1.Text = Selected_File;
            }
        }
    }
}
