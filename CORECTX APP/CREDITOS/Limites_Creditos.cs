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
    public partial class Limites_Creditos : Form
    {
        public static String Selected_File;
        DataTable dt = new DataTable();
        DataTable nocargados = new DataTable();
        conexionXML con = new conexionXML();
        //conexion conex = new conexion();
        Int32 ingresos;
        Double LIMITECREDITO;
        String CLIENTE;
        DateTime FECHA;
        String USUARIO ;
        String DIASCRED;
        String tipoing;
        String empresa = Login.empresa;
       
        public Limites_Creditos()
        {
            InitializeComponent();
        }

        private void Limites_Creditos_Load(object sender, EventArgs e)
        {
            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            USUARIO = Login.usuario.ToUpper();
            
          
                       
            nocargados.Columns.Add("CODIGO", typeof(string));
            nocargados.Columns.Add("NOMBRE", typeof(string));
            nocargados.Columns.Add("LIMITE DE CREDITO", typeof(string));
            nocargados.Columns.Add("DIAS CREDITO", typeof(string));
            nocargados.Columns.Add("ERROR", typeof(string));
            nocargados.Columns.Add("FECHA", typeof(DateTime));

            

            button2.Enabled = false;
            button3.Enabled = false;
            comboBox1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
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
                        if (String.IsNullOrEmpty(row["Código"].ToString()))
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

        private void button3_Click(object sender, EventArgs e)
        {
            Carga_data_xls();
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            label4.Text = Convert.ToString(dt.Rows.Count);
            int cant_reg = dt.Rows.Count;
            if (cant_reg > 0)
            {
                
                button2.Enabled = true;
                comboBox1.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Seleccione Tipo de Actualización");
                comboBox1.Focus();
            }
            else
            {


                nocargados.Clear();

                ingresos = 0;
                int registros;
              
                registros = dt.Rows.Count;

                if (registros >= 1)
                {

                    //try
                    //{
                        con.conectar("EX");

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow row = dt.Rows[i];
                          


                            if (DBNull.Value == (row["Código"]) || DBNull.Value == (row["Limite"]))
                            {

                            }
                            else
                            {
                                CLIENTE = Convert.ToString(row["Código"]);
                                DIASCRED = Convert.ToString(row["condicion"]);
                                FECHA = DateTime.Today;
                                LIMITECREDITO = Convert.ToDouble(row["Limite"]);

                                switch (CLIENTE.Length)
                                {
                                    case 1:
                                        CLIENTE = "0000" + CLIENTE;
                                        break;
                                    case 2:
                                        CLIENTE = "000" + CLIENTE;
                                        break;
                                    case 3:
                                        CLIENTE = "00" + CLIENTE;
                                        break;
                                    case 4:
                                        CLIENTE = "0" + CLIENTE;
                                        break;
                                    case 5:

                                        break;
                                }



                                if (comboBox1.Text == "LIMITES")
                                {
                                    tipoing = "L";
                                    SqlCommand cmd = new SqlCommand("UPDATE [EXACTUS].["+empresa+"].[CLIENTE]SET LIMITE_CREDITO =@LIMITECREDITO ,USUARIO_ULT_MOD = @USUARIO,FCH_HORA_ULT_MOD =@FECHA where CLIENTE = @CLIENTE ", con.conex);

                                    cmd.Parameters.Add("@LIMITECREDITO", SqlDbType.Decimal).Value = LIMITECREDITO;
                                    cmd.Parameters.Add("@USUARIO", SqlDbType.NVarChar).Value = USUARIO;
                                    cmd.Parameters.Add("@FECHA", SqlDbType.DateTime).Value = FECHA;
                                    cmd.Parameters.Add("@CLIENTE", SqlDbType.NVarChar).Value = CLIENTE;


                                    cmd.ExecuteNonQuery();



                                    row.Delete();
                                    dataGridView1.DataSource = dt;
                                    dataGridView1.Refresh();
                                    ingresos = ingresos + 1;

                                   

                                }
                                else
                                    if (comboBox1.Text == "DIAS")
                                    {
                                        tipoing = "D";

                                        SqlCommand cmd = new SqlCommand("UPDATE [EXACTUS].["+empresa+"].[CLIENTE]SET CONDICION_PAGO =@CONDICION_PAGO ,USUARIO_ULT_MOD = @USUARIO,FCH_HORA_ULT_MOD =@FECHA where CLIENTE = @CLIENTE ", con.conex);

                                        cmd.Parameters.Add("@CONDICION_PAGO", SqlDbType.NVarChar).Value = DIASCRED;
                                        cmd.Parameters.Add("@USUARIO", SqlDbType.NVarChar).Value = USUARIO;
                                        cmd.Parameters.Add("@FECHA", SqlDbType.DateTime).Value = FECHA;
                                        cmd.Parameters.Add("@CLIENTE", SqlDbType.NVarChar).Value = CLIENTE;


                                        cmd.ExecuteNonQuery();



                                        row.Delete();
                                        dataGridView1.DataSource = dt;
                                        dataGridView1.Refresh();
                                        ingresos = ingresos + 1;
                                       

                                    }
                                    else if (comboBox1.Text == "TODOS")
                                    {
                                        tipoing = "T";
                                        SqlCommand cmd = new SqlCommand("UPDATE [EXACTUS].["+empresa+"].[CLIENTE]SET LIMITE_CREDITO =@LIMITECREDITO, CONDICION_PAGO =@CONDICION_PAGO ,USUARIO_ULT_MOD = @USUARIO,FCH_HORA_ULT_MOD =@FECHA where CLIENTE = @CLIENTE ", con.conex);

                                        cmd.Parameters.Add("@LIMITECREDITO", SqlDbType.Decimal).Value = LIMITECREDITO;
                                        cmd.Parameters.Add("@CONDICION_PAGO", SqlDbType.NVarChar).Value = DIASCRED;
                                        cmd.Parameters.Add("@USUARIO", SqlDbType.NVarChar).Value = USUARIO;
                                        cmd.Parameters.Add("@FECHA", SqlDbType.DateTime).Value = FECHA;
                                        cmd.Parameters.Add("@CLIENTE", SqlDbType.NVarChar).Value = CLIENTE;


                                        cmd.ExecuteNonQuery();



                                        row.Delete();
                                        dataGridView1.DataSource = dt;
                                        dataGridView1.Refresh();
                                        ingresos = ingresos + 1;

                                      

                                    }
                            }


                        }
                        switch (tipoing)
                        {
                            case "L":
                                MessageBox.Show("Se Actalizo Limite de Creditos a: " + ingresos + " Clientes.");
                                break;
                            case "D":
                                MessageBox.Show("Se Actalizo Dias Credito a: " + ingresos + " Clientes.");
                                break;
                            case "T":
                                MessageBox.Show("Se Actalizo Limite de Creditos y Dias Credito  a: " + ingresos + " Clientes.");
                                break;

                        }
                        con.Desconectar("EX");
                    }
                    //catch
                    //{
                    //    con.Desconectar("EX");

                    //    MessageBox.Show("ERROR AL CARGAR DATOS");

                    //}
                //}



            }


        }

    }
}























                     


                      
