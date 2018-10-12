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
using System.Globalization;

namespace Sinconizacion_EXactus.CORECTX_APP.VENTAS
{
    public partial class importador_objetivos : Form
    {
        public importador_objetivos()
        {
            InitializeComponent();
        }
        string Selected_File;
        DataTable dt = new DataTable();
        DataTable noc = new DataTable();
        int MES_N,CLIENTES;
        string FECHA,RUTA,FAMILIA,SUB_FAMILIA,LINEA,MONTO,VOLUME,USUARIO_CREA,USUARIO_UPDATE,FECHA_UPDATE;
        DateTime FECHA_CREA;




        conexionXML con = new conexionXML();
        int ingreso;
        int numero_mes;

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            button2.Text = "Cargar";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button2.Text == "Importar")
            {
                button2.Text = "Cargar";
            }
          

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
                button2.Enabled = true;
            }
        }


        private void Carga_data_xls()
        {
            string nombre_hoja = textBox2.Text;
            button2.Text = "Importar";


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


                    //foreach (DataRow row in dt.Rows)
                    //{
                    //    if (String.IsNullOrEmpty(row["Fecha"].ToString()))

                    //        row.Delete();
                    //}
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

        private void button2_Click(object sender, EventArgs e)
        {

            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;

            dataGridView2.Enabled = true;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.ReadOnly = true;
            dataGridView2.AllowUserToAddRows = false;


            if (button2.Text == "Cargar")
            {
                Carga_data_xls();
            }

            else if (button2.Text == "Importar") 
            {
                MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show("SE CARGARA LOS OBJETIVOS DE EL MES :  " + this.comboBox1.Text + " DESEA CONTINUAR CON LA CARGA?", "ACTUALIZACION DE OBJETIVOS", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {

                    importadordb(dt);

                }
                else
                {

                }
                    // Imprortar_informacion();
                
            }
        }

        private void Imprortar_informacion()
        {


        }

        private void importador_objetivos_Load(object sender, EventArgs e)
        {
            string MES;
            DateTime fecha_hoy = DateTime.Now;
            MES = MonthName(fecha_hoy.Month);
            MES_N = fecha_hoy.Month;
            comboBox1.Text = MES.ToUpper();

            noc.Columns.Add("ERROR", typeof(string));
        }

        public string MonthName(int month)
        {
            DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;
            return dtinfo.GetMonthName(month);
        }

        public int mes_n(string nombre)
        {
            

            switch (nombre)
            {
                case "ENERO":
                    numero_mes = 1;
                    break;

                case "FEBRERO":
                    numero_mes = 2;
                    break;

                case "MARZO":
                    numero_mes = 3;
                    break;

                case "ABRIL":
                    numero_mes = 4;
                    break;

                case "MAYO":
                    numero_mes = 5;
                    break;

                case "JUNIO":
                    numero_mes = 6;
                    break;

                case "JULIO":
                    numero_mes = 7;
                    break;

                case "AGOSTO":
                    numero_mes = 8;
                    break;

                case "SEPTIEMBRE":
                    numero_mes = 9;
                    break;

                case "OCTUBRE":
                    numero_mes = 10;
                    break;

                case "NOVIEMBRE":
                    numero_mes = 11;
                    break;

                case "DICIEMBRE":
                    numero_mes = 12;
                    break;



            }

            return numero_mes;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mes_n(comboBox1.Text) < MES_N)
            {
                MessageBox.Show("NO PUEDE SELECCIONAR UN MES ANTERIOR AL ACTUAL", "MES ERROR");
                button2.Enabled = false;
            }
            else
            {
                button2.Enabled = true;
            }
        }

        private void importadordb(DataTable dts)
        {
            FECHA_CREA = DateTime.Now;
            string anio = DateTime.Now.Year.ToString();
            FECHA = anio+"/"+ Convert.ToString(numero_mes) + "/" + "01";

            borrado_objetivo(FECHA);

            DateTime FC = Convert.ToDateTime(FECHA);
          


            for (int i = 0; i < dts.Rows.Count; i++)
            {

                DataRow row = dts.Rows[i];


                RUTA = Convert.ToString(row["RUTA"]);
                FAMILIA = Convert.ToString(row["FAMILIA"]);
                SUB_FAMILIA = Convert.ToString(row["SUB-FAMILIA"]);
                LINEA = Convert.ToString(row["LINEA"]);                
                MONTO = Convert.ToString(row["CUOTA"]);
                VOLUME = Convert.ToString(row["VOLUMEN"]);
                if (DBNull.Value == (row["CLIENTE"]))
                {
                    CLIENTES = 0;
                }
                else
                {
                    CLIENTES = Convert.ToInt32(row["CLIENTE"]);
                }
                //USUARIO_CREA = Login.usuario.ToUpper();
                USUARIO_CREA = "TURCIOSI";


                if (DBNull.Value == (row["RUTA"]))
                {
                    noc.Rows.Add(" NO se encontro codigo Ruta en Objetivo");
                    dataGridView2.DataSource = noc;
                    dataGridView2.Refresh();
                }

                else
                     if (DBNull.Value == (row["FAMILIA"]))
                {
                    noc.Rows.Add(" NO se encontro codigo Familia en Objetivo");
                    dataGridView2.DataSource = noc;
                    dataGridView2.Refresh();
                }
                else
                { 
                    if (DBNull.Value == (row["CUOTA"]))
                     {
                      MONTO = "0.00";
                    //dataGridView2.DataSource = nocargados;
                    //dataGridView2.Refresh();
                        }
              
                            if (DBNull.Value == (row["VOLUMEN"]))
                         {
                    VOLUME = "0.00";
                    //dataGridView2.DataSource = nocargados;
                    //dataGridView2.Refresh();
                       }


                 
                 
                    {
                        if (exist_obj(numero_mes, RUTA, FAMILIA, LINEA))
                        {
                            noc.Rows.Add(" ya existe este objetivo para mes " + comboBox1.Text + " de la ruta " + RUTA + " y familia " + FAMILIA + "");
                            dataGridView2.DataSource = noc;
                            dataGridView2.Refresh();
                        }
                        else
                        {
                            con.conectar("DM");

                            SqlCommand cmd1 = new SqlCommand("[DM].[CORRECT].[INSERT_OBJETIVOS_DISMO]", con.condm);
                            cmd1.CommandType = CommandType.StoredProcedure;

                            cmd1.Parameters.AddWithValue("@RUTA", RUTA);
                            cmd1.Parameters.AddWithValue("@FAMILIA", FAMILIA);
                            if (SUB_FAMILIA != "")
                            {
                                cmd1.Parameters.AddWithValue("@SUB_FAMILIA", SUB_FAMILIA);
                            }
                            else
                            {
                                cmd1.Parameters.AddWithValue("@SUB_FAMILIA", null);
                            }
                            if (LINEA != "")
                            {
                               cmd1.Parameters.AddWithValue("@LINEA", LINEA);
                            }
                            else
                            {
                                cmd1.Parameters.AddWithValue("@LINEA", null);
                            }
                            cmd1.Parameters.AddWithValue("@FECHA", FC);
                            cmd1.Parameters.AddWithValue("@MONTO", MONTO);
                            cmd1.Parameters.AddWithValue("@VOLUMEN", VOLUME);
                            cmd1.Parameters.AddWithValue("@USUARIO_CREA", USUARIO_CREA);
                            cmd1.Parameters.AddWithValue("@FECHA_CREA", FECHA_CREA);
                            
                            cmd1.Parameters.AddWithValue("@CLIENTE", CLIENTES);

                            cmd1.ExecuteNonQuery();
                            row.Delete();
                            dataGridView1.DataSource = dt;
                            dataGridView1.Refresh();
                            ingreso = ingreso + 1;

                            con.Desconectar("DM");

                        }
                    }
                }
            }
            button1.Text = "Cargar";

        }

        private bool exist_obj(int n_mes, string ruta, string familia,string linea)
        {

            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("SELECT COUNT (ID) FROM [DM].[CORRECT].[OBJETIVOS_DISMO]  where DATEPART(MONTH,FECHA)= @N_MES and RUTA = @RUTA and FAMILIA = @FAMILIA and LINEA = @LINEA", con.condm);
            cmd.Parameters.AddWithValue("@N_MES", n_mes);
            cmd.Parameters.AddWithValue("@RUTA", ruta);
            cmd.Parameters.AddWithValue("@FAMILIA", familia);
            cmd.Parameters.AddWithValue("@LINEA", linea);


            int contar = Convert.ToInt32(cmd.ExecuteScalar());
            con.Desconectar("DM");

            if (contar == 0)
            {
                return false;

            }
            else
            {
                return true;
            }




        }

        private void borrado_objetivo(string fecha)
        {

            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("DELETE [DM].[CORRECT].[OBJETIVOS_DISMO]  where FECHA = '"+fecha+"'", con.condm);
            cmd.ExecuteNonQuery();
            con.Desconectar("DM");

        }
    }

    
}
