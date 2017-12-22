using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sinconizacion_EXactus
{
    public partial class ISSS : Form
    {
        public ISSS()
        {
            InitializeComponent();
        }

        String N_afiliacion;
        String Mes;
        String año;
        private void ISSS_Load(object sender, EventArgs e)
        {
            textBox1.Text = "901110502";
            Mes = Convert.ToString(DateTime.Now.Month);
            año = Convert.ToString(DateTime.Now.Year);
            N_afiliacion = textBox1.Text;

            
            comboBox1.Text = Mes;
            comboBox2.Text = año;

        }
    }
}
