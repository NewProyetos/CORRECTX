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
    public partial class import_fac_flota : Form
    {
        public import_fac_flota()
        {
            InitializeComponent();
        }
        DataTable dt = new DataTable();
        public static String Selected_File;
       // Conexion2 coned = new Conexion2();
        conexionXML con = new conexionXML();
        String PLACA ;
        Double KILOMETRAJE;
        String FACTURA;
        Double TOTAL;
        Double GALONES;
        String PROVEEDOR;
        String REGISTRO;
        String AGENCIA;
        String FORMA_PAGO;
        String FECHA;
        Int32 Año;
        Int32 mes;
        Double IVA;
        Double FOVIAL;
        Double COTRANS;
        Double TOTAL_IMPUESTO;
        Double COMPRA_GRAVADA;
        Double COMPRA_NETA;
        Double COSTO_GALON;
        Double ultimoKLM;
        String ESTATUS = "A";
        int ingreso = 0;
        DataTable nocargados = new DataTable();
                
        private void import_fac_flota_Load(object sender, EventArgs e)
        {
            button2.Text = "Importar";

            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;


            dataGridView2.Enabled = true;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.ReadOnly = true;
            dataGridView2.AllowUserToAddRows = false;




            nocargados.Columns.Add("FACTURA", typeof(string));
            nocargados.Columns.Add("PLACA", typeof(string));
            nocargados.Columns.Add("KILOMETRAJE", typeof(string));
            nocargados.Columns.Add("ERROR", typeof(string));
            nocargados.Columns.Add("FECHA", typeof(DateTime));

            button2.Enabled = false;
            button3.Enabled = false;

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
                                   
                   OleDbCommand oconn = new OleDbCommand("select * from ["+nombre_hoja+"$]", cnn);
                   cnn.Open();
                   OleDbDataAdapter adp = new OleDbDataAdapter(oconn);
                   dt.Clear();
                   dt.Columns.Clear();
                   adp.Fill(dt);
                  

                   foreach (DataRow row in dt.Rows)
                   {
                       if (String.IsNullOrEmpty(row["Fecha"].ToString()))
                         
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

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
                label4.Text = dataGridView1.RowCount.ToString();
                int cant_reg = dataGridView1.RowCount;
                if (cant_reg > 0)
                {
                    button2.Enabled = true;
                }



        }

        private bool Exists_FAC(string factura , string registro)
        {
            
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("select COUNT (*) from [DM].[CORRECT].[FAC_FLOTA] where FACTURA = '"+factura+"' and REGISTRO = '"+registro+"'", con.condm);
          

            string contar = Convert.ToString(cmd.ExecuteScalar());
            con.Desconectar("DM");
            
         
            if (contar == "0")
            {
                return false;

            }
            else
            {
                return true;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            


            if (button2.Text == "Importar")
            {
                nocargados.Clear();
                dataGridView2.DataSource = nocargados;
                dataGridView2.Refresh();
                int registros;
                ingreso = 0;
          
            
                 registros = dt.Rows.Count;
                
                if (registros >= 1)
                {

                    //try
                    //{
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                          

                            DataRow row = dt.Rows[i];
                            ultimoKLM = 0.0;

                            if (DBNull.Value == (row["Placa"]))
                            {

                            }

                            else
                                if (Exists_FAC(Convert.ToString(row["No cc Fiscal"]), REGISTRO))
                                {


                                    row.Delete();
                                    dataGridView1.DataSource = dt;
                                    dataGridView1.Refresh();


                                }
                            else
                            {
                                if (DBNull.Value == (row["Kilom#"]))
                                {
                                    nocargados.Rows.Add(row["No cc Fiscal"], row["Placa"], row["Kilom#"], " NO ingresaron Kilometraje", Convert.ToDateTime(FECHA));
                                    dataGridView2.DataSource = nocargados;
                                    dataGridView2.Refresh();
                                }

                                else
                                {

                                    if (DBNull.Value == (row["No cc Fiscal"]))
                                    {
                                        nocargados.Rows.Add(row["No cc Fiscal"], row["Placa"], row["Kilom#"], " NO ingresaron numero de Factura", Convert.ToDateTime(FECHA));
                                        dataGridView2.DataSource = nocargados;
                                        dataGridView2.Refresh();
                                    }

                                    else
                                    {
                                        if (DBNull.Value == (row["Total"]))
                                        {
                                            nocargados.Rows.Add(row["No cc Fiscal"], row["Placa"], row["Kilom#"], " NO Presenta Total de factura", Convert.ToDateTime(FECHA));
                                            dataGridView2.DataSource = nocargados;
                                            dataGridView2.Refresh();
                                        }
                                        else
                                        {
                                            if (DBNull.Value == (row["Galones"]))
                                            {
                                                nocargados.Rows.Add(row["No cc Fiscal"], row["Placa"], row["Kilom#"], " NO ingresaron Galones Consumidos", Convert.ToDateTime(FECHA));
                                                dataGridView2.DataSource = nocargados;
                                                dataGridView2.Refresh();
                                            }
                                            else
                                            {
                                                if (DBNull.Value == (row["No# Registro"]))
                                                {
                                                    nocargados.Rows.Add(row["No cc Fiscal"], row["Placa"], row["Kilom#"], " NO ingresaron numero de Registro de Cliente", Convert.ToDateTime(FECHA));
                                                    dataGridView2.DataSource = nocargados;
                                                    dataGridView2.Refresh();
                                                }
                                                else
                                                {
                                                    if (DBNull.Value == (row["Agencia"]))
                                                    {
                                                        nocargados.Rows.Add(row["No cc Fiscal"], row["Placa"], row["Kilom#"], " NO ingresaron Agencia", Convert.ToDateTime(FECHA));
                                                        dataGridView2.DataSource = nocargados;
                                                        dataGridView2.Refresh();
                                                    }
                                                    else
                                                    {
                                                        if (DBNull.Value == (row["Forma de Pago"]))
                                                        {
                                                            nocargados.Rows.Add(row["No cc Fiscal"], row["Placa"], row["Kilom#"], " NO ingresaron Forma de Pago", Convert.ToDateTime(FECHA));
                                                            dataGridView2.DataSource = nocargados;
                                                            dataGridView2.Refresh();
                                                        }
                                                        else
                                                        {
                                                            if (DBNull.Value == (row["Fecha"]))
                                                            {
                                                                nocargados.Rows.Add(row["No cc Fiscal"], row["Placa"], row["Kilom#"], " NO ingresaron Fecha de Factura", Convert.ToDateTime(FECHA));
                                                                dataGridView2.DataSource = nocargados;
                                                                dataGridView2.Refresh();
                                                            }
                                                            else
                                                                if (DBNull.Value == (row["Año"]))
                                                                {
                                                                    nocargados.Rows.Add(row["No cc Fiscal"], row["Placa"], row["Kilom#"], " NO ingresaron el año de Facturacion", Convert.ToDateTime(FECHA));
                                                                    dataGridView2.DataSource = nocargados;
                                                                    dataGridView2.Refresh();
                                                                }
                                                                else
                                                                    if (DBNull.Value == (row["Mes"]))
                                                                    {
                                                                        nocargados.Rows.Add(row["No cc Fiscal"], row["Placa"], row["Kilom#"], " NO ingresaron el mes de Facturacion", Convert.ToDateTime(FECHA));
                                                                        dataGridView2.DataSource = nocargados;
                                                                        dataGridView2.Refresh();
                                                                    }
                                                                    else
                                                                    {


                                                                        PLACA = Convert.ToString(row["Placa"]);
                                                                        KILOMETRAJE = Convert.ToDouble(row["Kilom#"]);
                                                                        FACTURA = Convert.ToString(row["No cc Fiscal"]);
                                                                        TOTAL = Convert.ToDouble(row["Total"]);
                                                                        GALONES = Convert.ToDouble(row["Galones"]);
                                                                        PROVEEDOR = Convert.ToString(row["Nombre del Proveedor"]);
                                                                        REGISTRO = Convert.ToString(row["No# Registro"]);
                                                                        AGENCIA = Convert.ToString(row["Agencia"]);
                                                                        FORMA_PAGO = Convert.ToString(row["Forma de Pago"]);
                                                                        FECHA = Convert.ToString(row["Fecha"]);
                                                                        DateTime FECHADT = Convert.ToDateTime(row["Fecha"]);
                                                                        Año = Convert.ToInt32(row["Año"]);
                                                                        mes = Convert.ToInt32(row["Mes"]);
                                                                        FOVIAL = Math.Round(GALONES * 0.2, 2);
                                                                        COTRANS = Math.Round(GALONES * 0.1, 2);
                                                                        TOTAL_IMPUESTO = FOVIAL + COTRANS;
                                                                        IVA = Math.Round((TOTAL - TOTAL_IMPUESTO) - (TOTAL - TOTAL_IMPUESTO) / 1.13, 2);
                                                                        COMPRA_GRAVADA = TOTAL - TOTAL_IMPUESTO - IVA;
                                                                        COMPRA_NETA = COMPRA_GRAVADA + TOTAL_IMPUESTO;
                                                                        COSTO_GALON = COMPRA_NETA / GALONES;



                                                                        if (FACTURA == "0")
                                                                        {


                                                                        }


                                                                        else
                                                                        {
                                                                            if (existe_Placa(PLACA))
                                                                            {


                                                                                if (Exists_FAC(FACTURA, REGISTRO))
                                                                                {


                                                                                    row.Delete();
                                                                                    dataGridView1.DataSource = dt;
                                                                                    dataGridView1.Refresh();


                                                                                }

                                                                                else
                                                                                {

                                                                                    if (existe_ultiKL(PLACA))
                                                                                    {
                                                                                        con.conectar("DM");
                                                                                        string FECHACORTA = FECHADT.ToString("yyyy/MM/dd");

                                                                                        SqlCommand cm1 = new SqlCommand("select TOP 1 (KILOMETRAJE) from [DM].[CORRECT].[FAC_FLOTA] where PLACA ='" + PLACA + "' AND (DATEADD(dd, 0, DATEDIFF(dd, 0, FECHA)) <= '" + FECHACORTA + " ' AND ESTATUS = 'A')", con.condm);
                                                                                        SqlDataReader dr1 = cm1.ExecuteReader();
                                                                                        while (dr1.Read())
                                                                                        {
                                                                                            ultimoKLM = Convert.ToInt32(dr1["KILOMETRAJE"]);

                                                                                        }
                                                                                        dr1.Close();

                                                                                    }
                                                                                    else
                                                                                    {

                                                                                        ultimoKLM = 0;
                                                                                        con.conectar("DM");
                                                                                    }
                                                                                    if (KILOMETRAJE > ultimoKLM)
                                                                                    {





                                                                                        SqlCommand cmd1 = new SqlCommand("[CORRECT].[INSERT_FAC_FLOTA]", con.condm);
                                                                                        cmd1.CommandType = CommandType.StoredProcedure;

                                                                                        cmd1.Parameters.AddWithValue("@PLACA", PLACA);
                                                                                        cmd1.Parameters.AddWithValue("@KILOMETRAJE", KILOMETRAJE);
                                                                                        cmd1.Parameters.AddWithValue("@FACTURA", FACTURA);
                                                                                        cmd1.Parameters.AddWithValue("@TOTAL", TOTAL);
                                                                                        cmd1.Parameters.AddWithValue("@GALONES", GALONES);
                                                                                        cmd1.Parameters.AddWithValue("@PROVEEDOR", PROVEEDOR);
                                                                                        cmd1.Parameters.AddWithValue("@REGISTRO", REGISTRO);
                                                                                        cmd1.Parameters.AddWithValue("@AGENCIA", AGENCIA);
                                                                                        cmd1.Parameters.AddWithValue("@FORMA_PAGO", FORMA_PAGO);
                                                                                        cmd1.Parameters.AddWithValue("@FECHA", Convert.ToDateTime(FECHA));
                                                                                        cmd1.Parameters.AddWithValue("@MES", mes);
                                                                                        cmd1.Parameters.AddWithValue("@AÑO", Año);
                                                                                        cmd1.Parameters.AddWithValue("@IVA", IVA);
                                                                                        cmd1.Parameters.AddWithValue("@FOVIAL", FOVIAL);
                                                                                        cmd1.Parameters.AddWithValue("@COTRANS", COTRANS);
                                                                                        cmd1.Parameters.AddWithValue("@TOTAL_IMPUESTO", TOTAL_IMPUESTO);
                                                                                        cmd1.Parameters.AddWithValue("@COMPRA_GRAVADA", COMPRA_GRAVADA);
                                                                                        cmd1.Parameters.AddWithValue("@COMPRA_NETA", COMPRA_NETA);
                                                                                        cmd1.Parameters.AddWithValue("@COSTO_GALON", COSTO_GALON);
                                                                                        cmd1.Parameters.AddWithValue("@fecha_crea", Convert.ToDateTime(FECHA));
                                                                                        cmd1.Parameters.AddWithValue("@USUARIO", "TURICOSI");
                                                                                        cmd1.Parameters.AddWithValue("@ESTATUS", ESTATUS);


                                                                                        cmd1.ExecuteNonQuery();




                                                                                        row.Delete();
                                                                                        dataGridView1.DataSource = dt;
                                                                                        dataGridView1.Refresh();
                                                                                        ingreso = ingreso + 1;
                                                                                       

                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        // MessageBox.Show("FACRUTA # " + FACTURA + " VEHICULO PLACAS:" + PLACA + " KILOMETRAJE ANTERIOR " + ultimoKLM + " ULTIMO KILOMETRAJE: " + KILOMETRAJE + "");

                                                                                        nocargados.Rows.Add(FACTURA, PLACA, KILOMETRAJE, " Kilometraje menor al Anterior = (" +ultimoKLM +")", Convert.ToDateTime(FECHA));
                                                                                        dataGridView2.DataSource = nocargados;
                                                                                        dataGridView2.Refresh();
                                                                                    }

                                                                                    con.Desconectar("DM");
                                                                                   button2.Text = "Reprocesar";
                                                                                }




                                                                            }

                                                                            else
                                                                            {
                                                                                // MessageBox.Show("NUMERO DE PLACA : " + PLACA + "  EN FACTURA No. " + FACTURA + " NO EXISTE");
                                                                                nocargados.Rows.Add(FACTURA, PLACA, KILOMETRAJE, " Placa NO existe en Base de Datos", Convert.ToDateTime(FECHA));
                                                                                dataGridView2.DataSource = nocargados;
                                                                                dataGridView2.Refresh();
                                                                            }


                                                                        }
                                                                    }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }


                        dt.AcceptChanges();
                    //}

                    //catch (Exception n)
                    //{
                    //    MessageBox.Show("ERROR NO SE PUEDE INGRESAR REGISTROS  ERROR:" + n.ToString() + "");
                    //}

                }
                else
                {
                    MessageBox.Show("No hay registros para procesar");

                }
                MessageBox.Show("Facturas Ingresadas: " + ingreso + " ");
                button2.Enabled = false;
            }
            else if (button2.Text == "Reprocesar")
            {

                  reporcesar();          
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            Carga_data_xls();
        }

        private bool existe_Placa(string placa)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("select COUNT (*) from [DM].[CORRECT].[VEHICULOS] where PLACA ='" + placa + "'", con.condm);
            cmd.Parameters.AddWithValue("placa", Convert.ToInt32(placa));


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

        private bool existe_ultiKL(string placa)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("select COUNT (*) from [DM].[CORRECT].[FAC_FLOTA] where PLACA ='"+PLACA+"'", con.condm);
            cmd.Parameters.AddWithValue("placa", Convert.ToInt32(placa));


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

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            //if (button2.Text == "Importar")
            //{
            //    dataGridView1.ReadOnly = true;

            //}
            //else
            //{
            dataGridView1.ReadOnly = false;
            //}
        }


        private void reporcesar()
        {


            int rep_registro = dataGridView1.Rows.Count;
            MessageBox.Show(Convert.ToString(rep_registro));

            if (rep_registro >= 1)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    DataRow row = dt.Rows[i];

                    row["Mes"] = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    row["Año"] = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    row["Fecha"] = Convert.ToDateTime(dataGridView1.Rows[i].Cells[2].Value.ToString());
                    row["Forma de Pago"] = dataGridView1.Rows[i].Cells[3].Value.ToString();
                    row["Agencia"] = dataGridView1.Rows[i].Cells[4].Value.ToString();
                    row["No# Registro"] = dataGridView1.Rows[i].Cells[5].Value.ToString();
                    row["Placa"] = dataGridView1.Rows[i].Cells[7].Value.ToString();
                    row["Kilom#"] = dataGridView1.Rows[i].Cells[9].Value.ToString();
                    row["No cc Fiscal"] = dataGridView1.Rows[i].Cells[10].Value.ToString();
                    row["Total"] = dataGridView1.Rows[i].Cells[11].Value.ToString();

                    
                }

                button2.Text = "Importar";

                button2_Click(null, null);

            }
        
        }

    }
}
