using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Sinconizacion_EXactus.CORECTX_APP.CONTABILIDAD
{
    public partial class Reporte_Diario : Form
    {
        public Reporte_Diario()
        {
            InitializeComponent();
        }
        conexionXML con = new conexionXML();
        DataTable datos = new DataTable();
        string tipo_aciento;

        private void Reporte_Diario_Load(object sender, EventArgs e)
        {
            tipo_asiento();
            comboBox1.Text = "FA";
            button2.Enabled = false;
            groupBox3.Hide();
            tipo_aciento = comboBox1.Text;
        }

        private void tipo_asiento()
        {
            con.conectar("EX");

            SqlCommand cmd = new SqlCommand("SELECT TIPO_ASIENTO FROM [EXACTUS].[dismo].[ASIENTO_DE_DIARIO] group by TIPO_ASIENTO", con.conex);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                comboBox1.Items.Add(dr["TIPO_ASIENTO"]);

            }

            dr.Close();

            con.Desconectar("EX");
        }

     

        private void button1_Click(object sender, EventArgs e)
        {

            if (cuentasWorker.IsBusy != true)
            {
                groupBox3.Show();
                cuentasWorker.RunWorkerAsync();

            }

            
         
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string FileName = "C:\\CORRECT\\XLS\\ Asientos_Contables_" + comboBox1.Text + "_" + dateTimePicker1.Value.ToString("ddddMMyyy") + ".xlsx";
            gridView1.ExportToXlsx(FileName);



            Microsoft.Office.Interop.Excel.Application excell;
            Microsoft.Office.Interop.Excel.Workbook workbook;
            excell = new Microsoft.Office.Interop.Excel.Application();
            excell.Visible = true;
            workbook = excell.Workbooks.Open(FileName);

        }

        private void cuentasWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            datos.Clear();
            
            con.conectar("DM");

            SqlCommand cmd = new SqlCommand("[CORRECT].[REPORTE_ASIENTOS_CONTABLES]", con.condm);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 50;
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            cmd.Parameters.AddWithValue("@FINI", Convert.ToDateTime(this.dateTimePicker1.Value.ToString("yyyy/MM/dd")));
            cmd.Parameters.AddWithValue("@FFIN", Convert.ToDateTime(this.dateTimePicker2.Value.ToString("yyyy/MM/dd")));
            cmd.Parameters.AddWithValue("@TIPO_ASIENTO", tipo_aciento);

            da.Fill(datos);
            cmd.ExecuteNonQuery();
            
        }

        private void cuentasWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dtfill(datos);
            if (datos.Rows.Count > 0)
            {
                button2.Enabled = true;
            }
            groupBox3.Hide();
        }

        private void dtfill(DataTable dt)
        {
            gridControl1.Refresh();
            gridControl1.DataSource = datos;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tipo_aciento = comboBox1.Text;
        }
    }
}
