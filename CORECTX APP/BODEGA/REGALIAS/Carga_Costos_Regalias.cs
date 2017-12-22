using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Data.OleDb;
using System.Data.SqlClient;


namespace Sinconizacion_EXactus
{
     
    public partial class Carga_Costos_Regalias : Form
    {
        public static String Selected_File;
        DataTable dt = new DataTable();
        conexionXML con = new conexionXML();
        String code;
        String Costo;

        public Carga_Costos_Regalias()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            carga_data_xls();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            textBox2.Text = "Hoja1";

            Selected_File = string.Empty;
            this.textBox1.Clear();
            openFileDialog1.AutoUpgradeEnabled = false;
            openFileDialog1.InitialDirectory = @"%USERPROFILE%\Documents";
            openFileDialog1.Title = "Select a File";
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.Filter = "Excel 2003 Files|*.xls| Excel 2007 Files|*.xlsx";
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {

            }
            else
            {
                Selected_File = openFileDialog1.FileName;
                this.textBox1.Text = Selected_File;
                button3.Enabled = true;
            }
        }

        private void Carga_Costos_Regalias_Load(object sender, EventArgs e)
        {
            button3.Enabled = false;

            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;

        }

        private void carga_data_xls()
        {
            string nombre_hoja = textBox2.Text;
           // button2.Text = "Importar";


            if (textBox1.Text == "" || textBox1.Text == null)
            {
                MessageBox.Show("no se a selecionado un archivo de Excel");

            }
            else
            {

                try
                {


                    OleDbConnection cnn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Selected_File + "; Extended Properties=Excel 12.0;");

                    OleDbCommand oconn = new OleDbCommand("select * from [" + nombre_hoja + "$]", cnn);
                    cnn.Open();
                    OleDbDataAdapter adp = new OleDbDataAdapter(oconn);
                    dt.Clear();
                    dt.Columns.Clear();
                    adp.Fill(dt);


                    foreach (DataRow row in dt.Rows)
                    {
                        if (String.IsNullOrEmpty(row["Costo"].ToString()))

                            row.Delete();
                    }
                    dt.AcceptChanges();

                    dataGridView1.DataSource = dt;
                    dataGridView1.Refresh();


                }
                catch
                {
                    MessageBox.Show("Error al abrir archivo de Excel");
                }


            }
        
        
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DateTime Fecha_crea;
            Fecha_crea = DateTime.Now;
            int Registros = dt.Rows.Count;
            eliminar_costos();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                con.conectar("DM");
                DataRow row = dt.Rows[i];

                code = Convert.ToString(row["Code"]);
                Costo = Convert.ToString(row["Costo"]);

                SqlCommand cmd1 = new SqlCommand();
                cmd1.Connection = con.condm;
                cmd1.CommandText = "INSERT INTO [DM].[CORRECT].[COSTOS]([ARTICULO],[COSTO],[fecha_update])  VALUES(@ARTICULO,@COSTO,@fecha_update)";
                cmd1.Parameters.Add("@ARTICULO", SqlDbType.NVarChar).Value = code;
                cmd1.Parameters.Add("@COSTO", SqlDbType.NVarChar).Value = Costo;
                cmd1.Parameters.Add("@fecha_update", SqlDbType.DateTime).Value = Fecha_crea;
                cmd1.ExecuteNonQuery();

                row.Delete();

                con.Desconectar("DM");
            }


          


        }

        private void eliminar_costos()
        {
            con.conectar("DM");   
            string cmdst = "DELETE [DM].[CORRECT].[COSTOS]";

            SqlCommand cmd = new SqlCommand(cmdst);
            cmd.Connection = con.condm;
            cmd.ExecuteNonQuery();   
            con.Desconectar("DM");
        
        }
    }
}
