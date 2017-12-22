using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data.SqlClient;




namespace Sinconizacion_EXactus
{
    public partial class Reporte_Cartera : Form
    {
        public Reporte_Cartera()
        {
            InitializeComponent();
        }
        
        conexionXML con = new conexionXML();
        private void Form7_Load(object sender, EventArgs e)
        {
            comboBox1.Enabled = false;
            textBox1.Enabled = false;
            con.conectar("EX");

            SqlCommand cm1 = new SqlCommand("SELECT Vendedor FROM [EXACTUS].[dismo].[SoftlandBI_CC_SaldoClientes] GROUP BY Vendedor", con.conex);
            SqlDataReader dr1 = cm1.ExecuteReader();


            while (dr1.Read())
            {
                comboBox1.Items.Add(dr1["Vendedor"]);
            }

            dr1.Close();
            con.Desconectar("EX");

        }







        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                comboBox1.Enabled = true;
                textBox1.Enabled = true;
                textBox1.Text = null;
                comboBox1.Text = null;
            }
            else
            {
                comboBox1.Enabled = false;
                textBox1.Enabled = false;
                textBox1.Text = null;
                comboBox1.Text = null;

            }

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            textBox1.Text = null;
            comboBox1.Text = null;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {



            con.conectar("EX");


            SqlCommand cmd = new SqlCommand("[dismo].[ReporteCC]", con.conex);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            if (textBox1.Text == "")
            {
                cmd.Parameters.AddWithValue("@cliente", null);
            }
            else
            {
                cmd.Parameters.AddWithValue("@cliente", this.textBox1.Text);

            }

            if (comboBox1.Text == "")
            {
                cmd.Parameters.AddWithValue("@Ruta", null);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Ruta", this.comboBox1.Text);
             }

            try
            {
                SqlDataAdapter dat = new SqlDataAdapter(cmd);

                ReporteCC ds = new ReporteCC();

                dat.Fill(ds, "ReporteCC");
                



                CrystalReport1 cr = new CrystalReport1();

                cr.SetDataSource(ds);

                crystalReportViewer1.ReportSource = cr;


                con.Desconectar("EX");
            }
            catch (SystemException exec)
            {
                MessageBoxButtons bt1 = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(exec.ToString(), "!!!!ERROR!!!!", bt1, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);

            
            }

            }
        }
    }



