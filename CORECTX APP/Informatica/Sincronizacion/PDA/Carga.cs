using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Data.SqlServerCe;
using System.Data.SqlClient;
using System.Net;
using System.Data.SQLite;

namespace Sinconizacion_EXactus.CORECTX_APP.Informatica.Sincronizacion.PDA
{
    public partial class Carga : Form
    {
        public Carga()
        {
            InitializeComponent();
        }
        conexionXML con = new conexionXML();
        DataTable tablas = new DataTable();
        DataTable intermedia = new DataTable();
        int parametros;
        String Consulta;
        string parametro;
        SQLiteConnection conexion;
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
          

        }

        private void Carga_Load(object sender, EventArgs e)
        {
            button2.Hide();
            textBox1.Enabled = false;
            richTextBox1.ReadOnly = true;
            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;

            dataGridView2.Enabled = true;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.AutoResizeColumns();
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView2.ReadOnly = true;
            dataGridView2.AllowUserToAddRows = false;

            carga_data();
           

        }

        private void carga_data()
        {
            tablas.Clear();

            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("SELECT  PASO, TABLA,CONSULTA,PARAMETROS  FROM   CORRECT.Sincronizacion_DM", con.condm);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(tablas);
            dataGridView1.DataSource = tablas;

            con.Desconectar("DM");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // insersqlite(tablas,)
            // sqlite_conexcion("P103");
            //if (parametros >= 1)
            //{
            //    parametro = textBox1.Text;

            //    if (parametro == string.Empty || parametro == null || parametro == "")
            //    {
            //        MessageBox.Show("Debe Ingresar un codigo de HH para la prueba como parametro");
            //    }
            //    else
            //    {
            //        prueba(Consulta, parametro);

            //    }
            //}

            //else
            //{
            //    prueba(Consulta, null);
            //}





            for (int i = 0; i < tablas.Rows.Count; i++)
            {

                intermedia.Clear();
                if (intermedia.Columns.Count > 0)
                {
                    intermedia.Columns.Clear();
                }
                DataRow row = tablas.Rows[i];

                String SECUENCIA = row["PASO"].ToString();
                String TABLA = row["TABLA"].ToString();
                String CONSULTA = row["CONSULTA"].ToString();
                int parametro = Convert.ToInt32(row["PARAMETROS"]);

                con.conectar("EX");
                SqlCommand cmd = new SqlCommand(CONSULTA, con.conex);
                if (parametro > 0)
                {
                    cmd.Parameters.AddWithValue("@Handheld", "P103");
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(intermedia);

                con.Desconectar("EX");

                //  string s = string.Join(", ", tablas.Rows.OfType<DataRow>().Select(r => r[0].ToString()));
              //  columname(intermedia, TABLA);
                insersqlite(intermedia, TABLA, "P103");

            }


            //   CargaHHWorker.RunWorkerAsync();
        }
        private void columname(DataTable dt,string tabla)
        {

            string[] columnNames = dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();
            
            string nombre = string.Join(",", columnNames);

            SqlCeConnection con_hh = new SqlCeConnection();
            con_hh.ConnectionString = "Data Source=" + @"C:\FRM600\FRM600.sdf ";
            con_hh.Open();
            SqlCeCommand cm1 = new SqlCeCommand("DELETE ERPADMIN_" + tabla + "", con_hh);
            cm1.ExecuteNonQuery();

            con_hh.Close();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];




                StringBuilder sb = new StringBuilder();

                sb.AppendLine(string.Join("','", dr.ItemArray));

                string values = "'" + sb.ToString().Trim('\n').Trim('\r') + "'";
                string otro = values;
               // SqlCeConnection con_hh = new SqlCeConnection();
              //  con_hh.ConnectionString = "Data Source=" + @"C:\FRM600\FRM600.sdf ";
                con_hh.Open();             

                SqlCeCommand cm2 = new SqlCeCommand("INSERT INTO ERPADMIN_" + tabla + " (" + nombre + ") values (" + values + ")", con_hh);
                cm2.ExecuteNonQuery();




                con_hh.Close();
            }
        }


        private void insersqlite(DataTable dt, string tabla,string pda)
        {
            conexion = new SQLiteConnection("Data Source=//192.168.1.123/Fileserver/Carga/" + pda + "/FRM600.db;Version=3;New=True;Compress=True;");
            conexion.Open();

            string[] columnNames = dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();

            string nombre = string.Join(",", columnNames);

            SQLiteCommand cm1 = new SQLiteCommand("DELETE  FROM  ERPADMIN_" + tabla + "", conexion);
            cm1.ExecuteNonQuery();

            conexion.Close();


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];

                conexion.Open();


                StringBuilder sb = new StringBuilder();

                sb.AppendLine(string.Join("','", dr.ItemArray));

                string values = "'" + sb.ToString().Trim('\n').Trim('\r') + "'";
                string otro = values;
                // SqlCeConnection con_hh = new SqlCeConnection();
                //  con_hh.ConnectionString = "Data Source=" + @"C:\FRM600\FRM600.sdf ";

                SQLiteCommand cm2 = new SQLiteCommand("INSERT INTO ERPADMIN_" + tabla + " (" + nombre + ") values (" + values + ")",conexion);
                cm2.ExecuteNonQuery();

                conexion.Close();
            }
        }

        private void CargaHHWorker_DoWork(object sender, DoWorkEventArgs e)
        {
           



               


            
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int idx = dataGridView1.CurrentRow.Index;
            string tabla = Convert.ToString(dataGridView1.Rows[idx].Cells[1].Value);
            textBox1.Enabled = false;


            if (tabla != string.Empty || tabla != null)
            {
                consulta(tabla);
                linkLabel2.Text = "Editar Consulta";
                button2.Hide();

            }


        }

        public void consulta(string tabla)
        {
         //  conexion = new SQLiteConnection("Data Source=//192.168.1.123/Fileserver/Carga/"+pda+"/FRM600.db;Version=3;New=True;Compress=True;");

            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("SELECT [CONSULTA],[PARAMETROS]  FROM [DM].[CORRECT].[Sincronizacion_DM] where TABLA = '"+tabla+"'", con.condm);
            SqlDataReader dar = cmd.ExecuteReader();

            while (dar.Read())
            {
                Consulta = Convert.ToString(dar["CONSULTA"]);
                parametros = Convert.ToInt32(dar["PARAMETROS"]);
            }
            dar.Close();

            

            if (parametros >= 1)
            {
                textBox1.Enabled = true;
                
            }

            con.Desconectar("DM");
            richTextBox1.Text = Consulta;
        }

        public void prueba(string consula, string param)
        {
            intermedia.Clear();
            if (intermedia.Columns.Count >= 1)
            {
                intermedia.Columns.Clear();
            }
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand(consula, con.conex);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            if (parametros >= 1)
            {
                cmd.Parameters.AddWithValue("@Handheld", param);
            }
            
            da.Fill(intermedia);
            dataGridView2.DataSource = intermedia;
            con.Desconectar("EX");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            richTextBox1.ReadOnly = false;

            

        }

        private void richTextBox1_Leave(object sender, EventArgs e)
        {
           
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            linkLabel2.Text = "Editar Consulta";
            button2.Hide();
        }

        private void valida_cambio(string nuevaconsl)
        {
            if (nuevaconsl == Consulta)
            {
                linkLabel2.Text = "Editar Consulta";
                button2.Hide();
            }
            else
            {
                linkLabel2.Text = "Guardar Consulta";
                button2.Show();
            }

        }

        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (richTextBox1.ReadOnly)
            {
            }
            else
            {
                string cs = richTextBox1.Text;
                valida_cambio(cs);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            nueva_tabla nv = new nueva_tabla();
            DialogResult res = nv.ShowDialog();

            if (res == DialogResult.OK)
            {
                carga_data();
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFile("http://138.99.3.134:15021/DM_UP_DW/DMFILE.svc/File/P103/P103/txt", @"c:\myfile.txt");
        }

        public void sqlite_conexcion(string pda)
        {
            conexion = new SQLiteConnection("Data Source=//192.168.1.123/Fileserver/Carga/"+pda+"/FRM600.db;Version=3;New=True;Compress=True;");
            //try
            //{
                conexion.Open();

                conexion.Close();

            //}
            //catch
            //{
               
            //    conexion.Close();
            //}


        }
    }
}
