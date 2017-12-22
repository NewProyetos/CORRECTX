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

namespace Sinconizacion_EXactus
{
    public partial class OFERTAS_STREET : Form
    {
        public static String Selected_File;
        DataTable dt = new DataTable();

        public OFERTAS_STREET()
        {
            InitializeComponent();
        }

        private void BT_Select_Click(object sender, EventArgs e)
        {
        
            Bt_cargar.Text = "Cargar";
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


                    foreach (DataRow row in dt.Rows)
                    {
                        if (String.IsNullOrEmpty(row["Desde"].ToString()))

                            row.Delete();
                    }
                    dt.AcceptChanges();


                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow row = dt.Rows[i];

                        string politica = Convert.ToString(row["Politica   De Venta"]);

                       MessageBox.Show(caracteres(politica));


                        string articulos = Convert.ToString(row["Articulos"]);

                       MessageBox.Show(caracteres(articulos));







                    }

                    //dataGridView1.DataSource = dt;
                    //dataGridView1.Refresh();


                //}
                //catch
                //{
                //    MessageBox.Show("Error al abrir archivo de Excel");
                //}


            }

        }

        private void Bt_cargar_Click(object sender, EventArgs e)
        {
            if (Bt_cargar.Text == "Cargar")
            {
                Carga_data_xls();
            }
        }

        private void OFERTAS_STREET_Load(object sender, EventArgs e)
        {

            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
                 int cant_reg = dataGridView1.RowCount;
                 if (cant_reg > 0)
                 {
                   Bt_cargar.Text = "Importar";
                 }
        }

       public  string caracteres(string cadena)
        {
            char[] arcadena;
            char caracter = '-';

            arcadena = cadena.ToArray();
            for (int i = 0; i < arcadena.Length; i++)
            {

                if (arcadena[i] == caracter)
                {
                                        // MessageBox.Show(cadena.Substring(0, i));
                    cadena = cadena.Substring(0, i);
                    return cadena;
                }
              
                 //char caracter2 = arcadena[i];
                 //for (int j = 0; j < arcadena.Length; j++)
                 //{
                   
                 //}
            
            }

            return cadena;
        
        }

    }
}
