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
    public partial class Dashboard_view : Form
    {
        public Dashboard_view()
        {
            InitializeComponent();
        }
        string Selected_File;
        private void button1_Click(object sender, EventArgs e)
        {
            Selected_File = string.Empty;
            this.textBox1.Clear();
            openFileDialog1.AutoUpgradeEnabled = false;
            openFileDialog1.InitialDirectory = @"%USERPROFILE%\Documents";
            openFileDialog1.Title = "Select a File";
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.Filter = "xlm Files|*.xml";
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {

            }
            else
            {
                

                Selected_File = openFileDialog1.FileName;
                this.textBox1.Text = Selected_File;
                dashboardViewer1.LoadDashboard(Selected_File);
            }

           
        }
    }
}
