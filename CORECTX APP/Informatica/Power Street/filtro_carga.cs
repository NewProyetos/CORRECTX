using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sinconizacion_EXactus.CORECTX_APP.Informatica.Power_Street
{
    public partial class filtro_carga : Form
    {
        public filtro_carga()
        {
            InitializeComponent();
        }
        //DataTable nameroute = Rutas_Trade_Reps.Rutassup;
        public string ROUTENAME;
        private void filtro_carga_Load(object sender, EventArgs e)
        {
            combo(Rutas_Trade_Reps.Rutassup);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ROUTENAME = "";
            if (radioButton1.Checked)
            {
                if (textBox1.Text != "")
                {
                    ROUTENAME = textBox1.Text;
                
                }
            }
            else if (radioButton2.Checked)
            {
                if (comboBox1.Text != "")
                {
                    ROUTENAME = comboBox1.Text;
                
                }
            }
        }

        public void combo(DataTable dts)
        {
            //comboBox1.Items.Clear();
            comboBox1.Items.Clear();

            var result = from row in dts.AsEnumerable()
                         group row by row.Field<string>("dia") into grp
                         select new
                         {
                             ruta = grp.Key,

                         };
            foreach (var t in result)
            {
                if (t.ruta == null || t.ruta == "")
                {

                }
                else
                {
                    comboBox1.Items.Add(t.ruta);
                }
            }


        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                textBox1.Enabled = true;
                comboBox1.Enabled = false;
            }
            if (radioButton2.Checked)
            {
                textBox1.Enabled = false;
                comboBox1.Enabled = true;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                textBox1.Enabled = true;
                comboBox1.Enabled = false;
            }
            if (radioButton2.Checked)
            {
                textBox1.Enabled = false;
                comboBox1.Enabled = true;
            }
        }
    }
}
