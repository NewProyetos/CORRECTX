using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Sinconizacion_EXactus.CORECTX_APP.RRHH
{
    public partial class CARGA_BAC : Form
    {
        public CARGA_BAC()
        {
            InitializeComponent();
        }

        conexionXML con = new conexionXML();
        DataSet bac = new  DataSet ();
        DataTable table = new DataTable();
        string Plan, Envio, Nomina;
        string fecha;
        int NumNomina;
        Exportador exp = new Exportador();



        private void CARGA_BAC_Load(object sender, EventArgs e)
        {
            numericUpDown1.Value = 1;
            textBox1.Text = "7070";
            nomina();
            numero_nomina(comboBox1.Text);
            numericUpDown2.Maximum = 10000;
            numericUpDown1.Maximum = 10000;
        }

        private void nomina()
        {
            con.conectar("EX");
            SqlCommand cm0 = new SqlCommand("SELECT NOMINA  FROM [EXACTUS].[dismo].[NOMINA] ORDER BY NOMINA", con.conex);
            SqlDataReader dr0 = cm0.ExecuteReader();
            while (dr0.Read())
            {
                comboBox1.Items.Add(dr0["NOMINA"]);
            }
            dr0.Close();

            comboBox1.Text = "DM01";
            con.Desconectar("EX");

        }
        private void numero_nomina(string nomina)
        {
            con.conectar("EX");
            SqlCommand cm0 = new SqlCommand("SELECT MAX(NUMERO_NOMINA) as Numero FROM  [EXACTUS].[dismo].[EMPLEADO_NOMI_NETO] where NOMINA='"+nomina+"'", con.conex);
            SqlDataReader dr0 = cm0.ExecuteReader();
            while (dr0.Read())
            {
               numericUpDown2.Value = Convert.ToInt32(dr0["Numero"]);
            }
            dr0.Close();

            
            con.Desconectar("EX");

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            numero_nomina(comboBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            table.Clear();
            if (bac.Tables.Contains("CARGA_BAC"))
            {
                bac.Tables.Remove(table);
            }

            Plan = textBox1.Text;
            Envio = Convert.ToString(numericUpDown1.Value);
            fecha = dateTimePicker1.Value.ToString("yyyy/MM/dd");
            Nomina = comboBox1.Text;
            NumNomina = Convert.ToInt32(numericUpDown2.Value);
            table.TableName = "CARGA_BAC";
            generadatos(Plan, Envio, fecha, Nomina, NumNomina);
        }

        private void generadatos(string pl ,string en,string fech,string nom ,int numnom)
        {
            con.conectar("EX");


            SqlCommand cmd = new SqlCommand("[dismo].[RH_BAC]", con.conex);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Plan", pl );
            cmd.Parameters.AddWithValue("@Envio", en);
            cmd.Parameters.AddWithValue("@fecha", fech);
            cmd.Parameters.AddWithValue("@Nomina", nom);
            cmd.Parameters.AddWithValue("@NumNomina", numnom);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(table);

            bac.Tables.Add(table);
            con.Desconectar("EX");

            exp.aExcel(bac);
        }

      
    }
}
