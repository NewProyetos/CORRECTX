using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using System.Data.OleDb;
using System.Xml;

namespace Sinconizacion_EXactus.CORECTX_APP.BODEGA.PRECIOS
{
    public partial class importadorListaPrecios : Form
    {
        public importadorListaPrecios()
        {
            InitializeComponent();
        }
        string Selected_File;
        DataTable dt = new DataTable();
        conexionXML con = new conexionXML();
        string USUARIO;
        string empresa;

        private void importadorListaPrecios_Load(object sender, EventArgs e)
        {
            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            USUARIO = Login.usuario.ToUpper();
            empresa = Login.empresa;
            button3.Enabled = false;
            carga_niveles();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "Hoja1";

            Selected_File = string.Empty;
            this.textBox1.Clear();
            openFileDialog1.AutoUpgradeEnabled = false;
            openFileDialog1.InitialDirectory = @"%USERPROFILE%\Documents";
            openFileDialog1.Title = "Select a File";
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.Filter = "Excel 2007 Files|*.xlsx| Excel 2003 Files|*.xls";
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {

            }
            else
            {
                Selected_File = openFileDialog1.FileName;
                this.textBox1.Text = Selected_File;

            }
        }

        private void Carga_data_xls()
        {
            string nombre_hoja = textBox2.Text;



            if (textBox1.Text == "" || textBox1.Text == null)
            {
                MessageBox.Show("no se a selecionado un archivo de Excel");

            }
            else
            {

                //try
                //{


                OleDbConnection cnn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Selected_File + "; Extended Properties=Excel 12.0;");

                OleDbCommand oconn = new OleDbCommand("select * from [" + nombre_hoja + "$]", cnn);
                cnn.Open();
                OleDbDataAdapter adp = new OleDbDataAdapter(oconn);
                dt.Clear();
                dt.Columns.Clear();
                adp.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();

                foreach (DataRow row in dt.Rows)
                {
                    if (String.IsNullOrEmpty(row["PRECIO UNITARIO"].ToString()) || String.IsNullOrEmpty(row["ARTICULO"].ToString()))
                        row.Delete();
                }

                dt.AcceptChanges();



                //}
                //catch
                //{
                //    MessageBox.Show("Error al abrir archivo de Excel");
                //}


            }

        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            label4.Text = Convert.ToString(dt.Rows.Count);
            int cant_reg = dt.Rows.Count;
            if (cant_reg > 0)
            {

                button3.Enabled = true;
                //comboBox1.Enabled = true;
            }
        }

        private void carga_niveles()
        {
            con.conectar("EX");

            SqlCommand cm1 = new SqlCommand("SELECT [NIVEL_PRECIO] FROM [EXACTUS].["+empresa+"].[VERSION_NIVEL]", con.conex);
            SqlDataReader dr1 = cm1.ExecuteReader();
            while (dr1.Read())
            {
                comboBox1.Items.Add(dr1["NIVEL_PRECIO"]);

            }
            dr1.Close();




            con.Desconectar("EX");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Carga_data_xls();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int registros;

            registros = dt.Rows.Count;

            if (registros >= 1)
            {

                //try
                //{
                con.conectar("EX");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DateTime fecha_ = DateTime.Now;
                   
                    DataRow row = dt.Rows[i];

                     string NIVEL = Convert.ToString(row["NIVEL"]);
                    decimal PRECIO = Convert.ToDecimal(row["PRECIO UNITARIO"]);
                    string ARTICULO = Convert.ToString(row["ARTICULO"]);

                    if (NIVEL == comboBox1.Text)
                    {
                        if (existe_articulo(ARTICULO))
                        {

                            SqlCommand cmd = new SqlCommand("UPDATE [EXACTUS].[" + empresa + "].[ARTICULO_PRECIO] SET PRECIO = @PRECIO , USUARIO_ULT_MODIF = @USUARIO , FECHA_ULT_MODIF = @FECHA  where NIVEL_PRECIO = @NIVEL and ARTICULO = @ARTICULO", con.conex);

                            cmd.Parameters.Add("@PRECIO", SqlDbType.Decimal).Value = PRECIO;
                            cmd.Parameters.Add("@USUARIO", SqlDbType.VarChar).Value = USUARIO;
                            cmd.Parameters.Add("@FECHA", SqlDbType.DateTime).Value = fecha_;
                            cmd.Parameters.Add("@NIVEL", SqlDbType.VarChar).Value = NIVEL;
                            cmd.Parameters.Add("@ARTICULO", SqlDbType.VarChar).Value = ARTICULO;


                            cmd.ExecuteNonQuery();



                            row.Delete();
                            dataGridView1.DataSource = dt;
                            dataGridView1.Refresh();
                        }
                        else
                        {

                        }


                    }
                    else
                    {
                        MessageBox.Show("NIVEL DE PRECIO SELECCIONADO ES DIFERENTE ");
                        comboBox1.Focus();
                    }
                    

                }
                con.Desconectar("EX");
            }
        }


        private bool existe_articulo(string artic)
        {
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("SELECT COUNT (*) FROM [EXACTUS].["+empresa+"].[ARTICULO]  where ARTICULO = @ARTICULO", con.conex);
            cmd.Parameters.AddWithValue("@ARTICULO", artic);
            cmd.ExecuteNonQuery();
            int contar = Convert.ToInt32(cmd.ExecuteScalar());
            con.Desconectar("EX");
            if (contar == 0)
            {
                return false;

            }
            else
            {
                return true;
            }


        }
    }
}
