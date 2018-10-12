using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.FileIO;
using System.Net;
using System.Data.SqlClient;


namespace Sinconizacion_EXactus
{
    public partial class Importador_Street : Form
    {
        public Importador_Street()
        {
            InitializeComponent();
        }

        conexionXML con = new conexionXML();
        String Selected_File;

        public static DataTable Encabezados_PED = new DataTable();
        public static DataTable Detalle_PED = new DataTable();
        public static DataTable Detalle_linea = new DataTable();
        public static DataTable Encabezados_PED_Ex = new DataTable();
        public static DataTable Detalle_PED_Ex = new DataTable();
        public static DataTable Cobros_ex = new DataTable();

        public static DataTable Cobros = new DataTable();
        public static DataTable clientes = new DataTable();
        public static DataTable Encabezados_COM = new DataTable();
        public static DataTable Detalle_COM = new DataTable();
        public static DataTable Lista_precios = new DataTable();
        public static DataTable Errores = new DataTable();
        public static Int32 consulta;
        public static DataTable Clientes_Nuevos = new DataTable();
        public static Int32 procesados;
        String cod_zona;
        String Enc_pedidos;
        String Det_pedidos;
        String Enc_copmpra;
        String Det_compra;
        String clientest;
        string num_dev;
        String cantidad;
        Int32 cantidad_Documentos;
        String Pw_cliente;
        public static String Separador;
        public static String Usuario;
        public static DateTime fecha_proceso;
        String tipo_doc;
        Int32 producto_noenonctrado;
        String Articulo_noenc;


        String Clientes_FTP;
        String cobro_FTP;
        String lista_precio_FTP;
        String Cantidad_Cobros;

        private void button2_Click(object sender, EventArgs e)
        {

            Errores.Clear();
            toolStripStatusLabel1.Text = "";
            statusStrip1.BackColor = Color.Transparent;

            linkLabel9.Hide();

            Separador = textBox8.Text;

            backgroundDescargaftp.RunWorkerAsync();



        }

        // Leer archivo txt o cvs  y lo carga en las tablas 
        private static DataTable GetDataTabletFromCSVFile(string path, string tipo_doc)
        {

            DataTable csvData = new DataTable();
            csvData.Clear();
            string[] lineaenc;


            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(path))
                {

                    csvReader.SetDelimiters(new string[] { Separador });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();


                    // agrega encabezado a txt
                    switch (tipo_doc)
                    {
                        case "FTP_ENC":
                            lineaenc = new string[] 
            {
                "Numero","Numero_control","Serie","Tipodoc","Ruta","Vendedor","Transportista","Codigo_cliente","Codigo_alternativo","Fechapedido","Fechaini","Fechafin","Fechaliq","Impuesto","Impperc","Impret","Monto_sin_imp","Monto_con_imp","Dctocliente","Dctoxlinea","Cantidad_items","Listaprec","Estado_pedido","Perfilc","Deposito","Latitud","Longitud",""
            };
                            foreach (string column in lineaenc)
                            {
                                DataColumn serialno = new DataColumn(column);
                                serialno.AllowDBNull = true;
                                csvData.Columns.Add(serialno);
                            }
                            break;

                        case "FTP_DET":

                            lineaenc = new string[] 
            {
                "Codreng","Nrodoc","Serie","Tipodoc","Articulo","Codialte","Cantidad","Preclist","Impuesto1","Impuesto2","Dcto","Monto_sin_imp","Monto_con_imp","Exp","Listaprec","Preciocosto",""
            };
                            foreach (string column in lineaenc)
                            {
                                DataColumn serialno = new DataColumn(column);
                                serialno.AllowDBNull = true;
                                csvData.Columns.Add(serialno);
                            }
                            break;

                        case "FTP_COB":

                            lineaenc = new string[] 
            {
                "Numero_recibo","Serie","Ruta","Vendedor","Cobrador","Codigo","Codigoak","Fecha","Estado","Total","Pagoefectivo","Monto_cheque","Fechaini","Fechafin","Impreso","Tipo","Documento_aplicado","Serie_documento_aplicado","Tipo_pago","Saldo",""
            };
                            foreach (string column in lineaenc)
                            {
                                DataColumn serialno = new DataColumn(column);
                                serialno.AllowDBNull = true;
                                csvData.Columns.Add(serialno);
                            }
                            break;

                        case "FTP_CLIE":

                            lineaenc = new string[] 
            {
                "Codigo","Codigoak","Razon","Nombre","Contacto","Direccion","Municipio","Departamento","Pais","Lista","Telefono1","Telefono2","Fax","Dircorp","Fechaing","Descripcion","Condicion_pago","Dto","Vendedor","Transportista","Dia_visita","Documento_generar","Categoria","Numero_dui","Registro_fiscal","Giro_negocio","Nit","Tipo_contribuyente","Canal","Latitud","Longitud","",""
            };
                            foreach (string column in lineaenc)
                            {
                                DataColumn serialno = new DataColumn(column);
                                serialno.AllowDBNull = true;
                                csvData.Columns.Add(serialno);
                            }
                            break;
                        // encabezado tomadolo del archivo txt
                        default:
                            foreach (string column in colFields)
                            {
                                DataColumn serialno = new DataColumn(column);
                                serialno.AllowDBNull = true;
                                csvData.Columns.Add(serialno);
                            }
                            break;
                    }









                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        DataRow dr = csvData.NewRow();
                        //Making empty value as empty
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == null)
                                fieldData[i] = string.Empty;

                            dr[i] = fieldData[i];
                        }
                        csvData.Rows.Add(dr);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
            return csvData;
        }

        private void Importador_Street_Load(object sender, EventArgs e)
        {
            radioButton3.Checked = true;
            textBox8.Text = ",";
            linkLabel1.Hide();
            linkLabel2.Hide();
            linkLabel3.Hide();
            linkLabel4.Hide();
            linkLabel5.Hide();
            linkLabel6.Hide();
            linkLabel7.Hide();


            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;

            Clientes_Nuevos = new DataTable();
            Clientes_Nuevos.Columns.Add("CODIGO", typeof(string));
            Clientes_Nuevos.Columns.Add("CODIGO_STREET", typeof(string));
            Clientes_Nuevos.Columns.Add("FACTURA", typeof(string));
            Clientes_Nuevos.Columns.Add("NCF", typeof(string));
            Clientes_Nuevos.Columns.Add("TIPO", typeof(string));
            Clientes_Nuevos.Columns.Add("FECHA", typeof(DateTime));

            Errores = new DataTable();
            Errores.Columns.Add("DOCUMENTO", typeof(string));
            Errores.Columns.Add("cliente", typeof(string));
            Errores.Columns.Add("correlativo", typeof(string));
            Errores.Columns.Add("fecha", typeof(DateTime));
            Errores.Columns.Add("Tipo", typeof(string));
            Errores.Columns.Add("Error", typeof(string));


        }

        private void textBox1_DoubleClick(object sender, EventArgs e)
        {

            string file1 = examinar();
            textBox1.Text = file1;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox1.Enabled = true;

            }
            else
            {
                textBox1.Enabled = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                textBox2.Enabled = true;

            }
            else
            {
                textBox2.Enabled = false;
            }

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                textBox3.Enabled = true;

            }
            else
            {
                textBox3.Enabled = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                textBox4.Enabled = true;

            }
            else
            {
                textBox4.Enabled = false;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                textBox5.Enabled = true;

            }
            else
            {
                textBox5.Enabled = false;
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                textBox6.Enabled = true;

            }
            else
            {
                textBox6.Enabled = false;
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked)
            {
                textBox7.Enabled = true;

            }
            else
            {
                textBox7.Enabled = false;
            }
        }

        private String examinar()
        {


            Selected_File = string.Empty;
            //this.textBox1.Clear();
            openFileDialog1.AutoUpgradeEnabled = false;
            openFileDialog1.InitialDirectory = @"C:\CORRECT\Street";
            openFileDialog1.Title = "Select a File";
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.Filter = "Text Files|*.txt|CSV Files|*.csv";
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {

            }
            else
            {
                Selected_File = openFileDialog1.FileName;
                //this.textBox1.Text = Selected_File;





            }







            return openFileDialog1.FileName;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_DoubleClick(object sender, EventArgs e)
        {
            examinar();

            textBox2.Text = Selected_File;
        }
        // Proeceso de Conexion y Descarga de Archivos del FTP
        private void Descarga_archivosftp()
        {
            DateTime ahora = DateTime.Now;

            string user = "dmpowerstreet";
            string pass = "CxtR3ADd";
            string direccion = "ftp://ftp.dismonline.info/Unilever/new.csv";


            string fecha = Convert.ToString(dateTimePicker1.Value.ToString("ddMMyyyy"));
            ftp ftpClient = new ftp(@"ftp://ftp.dismonline.info/", user, pass);

            FtpWebRequest arh = (FtpWebRequest)FtpWebRequest.Create(new Uri(direccion));
            arh.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            arh.Proxy = null;
            arh.Credentials = new NetworkCredential(user, pass);
            arh.UsePassive = true;
            arh.KeepAlive = true;


            //try
            //{


            //   //String[] lista= ftpClient.directoryListSimple(@"Unilever\");

            //   //List<string> doto = new List<string>(lista);
            //   //var message = string.Join(",", doto);
            //   //MessageBox.Show(message);

            //}
            //catch (WebException e)
            //{
            //    MessageBox.Show(Convert.ToString(e));
            //}

            Enc_pedidos = @"C:\CORRECT\Unilever\Enc_Pedido" + fecha + ".txt";
            Det_pedidos = @"C:\CORRECT\Unilever\Det_Pedido" + fecha + ".txt";
            Clientes_FTP = @"C:\CORRECT\Unilever\Clientes_Nuevos" + fecha + ".txt";
            cobro_FTP = @"C:\CORRECT\Unilever\Cobros" + fecha + ".txt";
            lista_precio_FTP = @"C:\CORRECT\Unilever\Lista_precio" + fecha + ".txt";

            ftpClient.download(@"Unilever\clientes_nuevos_" + fecha + "_.txt", Clientes_FTP);
            ftpClient.download(@"Unilever\detalles_facturas_" + fecha + "_.txt", Det_pedidos);
            ftpClient.download(@"Unilever\encabezado_factura_" + fecha + "_.txt", Enc_pedidos);
            ftpClient.download(@"Unilever\cobros_" + fecha + "_.txt", cobro_FTP);
            ftpClient.download(@"Unilever\lista_precios_" + fecha + "_.txt", lista_precio_FTP);







        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            consulta = 1;
            informacion_import_street inf = new informacion_import_street();
            inf.ShowDialog();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            consulta = 2;
            informacion_import_street inf = new informacion_import_street();
            inf.ShowDialog();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            toolStripStatusLabel1.Text = "DESCARGANDO ARCHIVOS...";
            statusStrip1.BackColor = Color.DarkSlateBlue;
            if (radioButton1.Checked)
            {
                Descarga_archivosftp();
            }

        }

        private void backgroundDescargaftp_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            label8.Text = cantidad;

            if (radioButton1.Checked)
            {

                if (File.Exists(Enc_pedidos))
                {

                    this.textBox1.Text = Enc_pedidos;
                    this.checkBox1.Checked = true;
                }




                if (File.Exists(Det_pedidos))
                {
                    this.textBox2.Text = Det_pedidos;
                    this.checkBox2.Checked = true;
                }

                if (File.Exists(cobro_FTP))
                {
                    this.textBox3.Text = cobro_FTP;
                    this.checkBox3.Checked = true;
                }


                if (File.Exists(Clientes_FTP))
                {
                    this.textBox4.Text = Clientes_FTP;
                    this.checkBox4.Checked = true;
                }




            }
            else if (radioButton3.Checked)
            {
                if (checkBox1.Checked)
                {
                    Enc_pedidos = this.textBox1.Text;

                }
                if (checkBox2.Checked)
                {
                    Det_pedidos = this.textBox2.Text;
                }
                if (checkBox4.Checked)
                {
                    clientest = this.textBox4.Text;
                }


                if (checkBox6.Checked)
                {
                Enc_copmpra = this.textBox4.Text;
                }
                if (checkBox7.Checked)
                {
                    Det_compra = this.textBox4.Text;
                }



            }




            backgroundCargatablas.RunWorkerAsync();
        }

        private void backgroundDescargaftp_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {



            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundCargatablas_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void backgroundCargatablas_DoWork(object sender, DoWorkEventArgs e)
        {
            string tipo_exp_enc;
            string tipo_exp_det;        
            string tipo_exp_cob;
            string tipo_exp_clie;
            
            toolStripStatusLabel1.Text = "CARGANDO TABLAS ...";
            if (radioButton1.Checked)
            {
                tipo_exp_enc = "FTP_ENC";
                tipo_exp_det = "FTP_DET";
                tipo_exp_cob = "FTP_COB";
                tipo_exp_clie = "FTP_CLIE";
            }
            else
            {
                tipo_exp_enc = "ENC";
                tipo_exp_det = "DET";
                tipo_exp_cob = "COB";
                tipo_exp_clie = "CLIE";
                

            }
            if (checkBox1.Checked)
            {
                Encabezados_PED.Clear();
                Encabezados_PED.Columns.Clear();
                Encabezados_PED = GetDataTabletFromCSVFile(textBox1.Text, tipo_exp_enc);


            }

            if (checkBox2.Checked)
            {
                Detalle_PED.Clear();
                Detalle_PED.Columns.Clear();
                Detalle_PED = GetDataTabletFromCSVFile(textBox2.Text, tipo_exp_det);


            }

            if (checkBox3.Checked)
            {
                Cobros.Clear();
                Cobros.Columns.Clear();
                Cobros = GetDataTabletFromCSVFile(textBox3.Text, tipo_exp_cob);


            }

            if (checkBox4.Checked)
            {
                clientes.Clear();
                clientes.Columns.Clear();
                clientes = GetDataTabletFromCSVFile(textBox4.Text, tipo_exp_clie);
            }


            if (checkBox6.Checked)
            {
                Encabezados_COM.Clear();
                Encabezados_COM.Columns.Clear();
                Encabezados_COM = GetDataTabletFromCSVFile(textBox6.Text, tipo_exp_enc);
            }
            if (checkBox7.Checked)
            {

                Detalle_COM.Clear();
                Detalle_COM.Columns.Clear();
                Detalle_COM = GetDataTabletFromCSVFile(textBox7.Text, tipo_exp_det);
            }


        }

        private void backgroundCargatablas_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            statusStrip1.BackColor = Color.DarkSeaGreen;
            toolStripStatusLabel1.Text = "Descarga Finalizada";

            if (checkBox1.Checked)
            {
                if (Encabezados_PED.Rows.Count >= 1)
                {

                    linkLabel1.Show();
                    linkLabel1.Text = Convert.ToString(Encabezados_PED.Rows.Count);

                }
                else
                {

                    linkLabel1.Text = "0";
                }
            }
            if (checkBox2.Checked)
            {
                if (Detalle_PED.Rows.Count >= 1)
                {

                    linkLabel2.Show();
                    linkLabel2.Text = Convert.ToString(Detalle_PED.Rows.Count);

                }
                else
                {

                    linkLabel2.Text = "0";
                }
            }

            if (checkBox3.Checked)
            {
                if (Cobros.Rows.Count >= 1)
                {

                    linkLabel3.Show();
                    linkLabel3.Text = Convert.ToString(Cobros.Rows.Count);

                }
                else
                {

                    linkLabel3.Text = "0";
                }
            }

            if (checkBox4.Checked)
            {
                if (clientes.Rows.Count >= 1)
                {

                    linkLabel4.Show();
                    linkLabel4.Text = Convert.ToString(clientes.Rows.Count);

                }
                else
                {

                    linkLabel4.Text = "0";
                }
            }

            if (checkBox6.Checked)
            {
                if (Encabezados_COM.Rows.Count >= 1)
                {
                    
                    linkLabel6.Show();
                    linkLabel6.Text = Convert.ToString(Encabezados_COM.Rows.Count);

                }
                else
                {

                    linkLabel6.Text = "0";
                }
            }

            if (checkBox7.Checked)
            {
                if (Detalle_COM.Rows.Count >= 1)
                {
                    
                    linkLabel7.Show();
                    linkLabel7.Text = Convert.ToString(Detalle_COM.Rows.Count);

                }
                else
                {

                    linkLabel7.Text = "0";
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fecha_proceso = DateTime.Now;
            Usuario = Login.usuario.ToUpper();


            if (checkBox4.Checked)
            {
                // backgroundCLIENTES.RunWorkerAsync();
            }

            if (checkBox4.Checked && clientes.Rows.Count >= 1)
            {
                this.progressBar1.Value = 0;
                backgroundCLIENTES.RunWorkerAsync();
            }
            else
                if (checkBox1.Checked && Encabezados_PED.Rows.Count >= 1)
                {
                    if (checkBox2.Checked && Detalle_PED.Rows.Count >= 1)
                    {
                        this.progressBar1.Value = 0;
                        backgrounFact.RunWorkerAsync();
                    }
                    else
                    {
                        MessageBox.Show("Para Ingresar Pedidos debe Existir Detalle de Pedido");
                        textBox2.Focus();
                    }

                }
                else
                    if (checkBox2.Checked && Detalle_PED.Rows.Count >= 1)
                    {
                        if (checkBox1.Checked && Encabezados_PED.Rows.Count >= 1)
                        {
                            backgrounFact.RunWorkerAsync();
                        }
                        else
                        {
                            MessageBox.Show("Para Ingresar Detalle de Pedidos debe Existir Encabezado de Pedido");
                            textBox1.Focus();
                        }

                    }
            if (checkBox3.Checked && Cobros.Rows.Count >= 1)
            {
                backgroundCobros.RunWorkerAsync();

            }


            if (checkBox6.Checked && Encabezados_COM.Rows.Count >= 1)
            {
                //backgroundCobros.RunWorkerAsync();


            }



            //  





        }
        // Valida si cliente Existe
        private bool existe_cliente(string cliente)
        {
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("select COUNT (*) FROM [EXACTUS].[dismo].[CLIENTE]  where cliente ='" + cliente + "'", con.conex);
            cmd.Parameters.AddWithValue("cliente", Convert.ToInt32(cliente));


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
        //Valida si existe el documento (pedido) Numero Preimpreso
        private bool existe_Documento(string factura)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("SELECT COUNT (*)  FROM [DM].[STREET].[ENC_PED_STREET]  where NUM_DOC_PREIMP ='" + factura + "'", con.condm);
            cmd.Parameters.AddWithValue("NUM_DOC_PREIMP", Convert.ToInt32(factura));


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

        private bool existe_Documento_exactus(string factura)
        {
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*)  FROM [EXACTUS].[ERPADMIN].[alFAC_ENC_PED]  WHERE NUM_PED='" + factura + "'", con.conex);
            cmd.Parameters.AddWithValue("NUM_PED", factura);


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

        private bool existe_Devolucion_exactus(string factura)
        {
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*)  FROM [EXACTUS].[ERPADMIN].[alFAC_ENC_DEV]  WHERE NUM_DEV='" + factura + "'", con.conex);
            cmd.Parameters.AddWithValue("NUM_DEV", factura);


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

        //Valida si existe el documento (pedido) Numero  Sistema
        private bool existe_Documento_sys(string factura ,string cliente, string fecha)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("SELECT COUNT (*)  FROM [DM].[STREET].[ENC_PED_STREET]  where NUM_DOC_SIS ='"+factura+"'and COD_CLIE ='"+cliente+"' and FECHA_PEDIDO = '"+fecha+"'", con.condm);
            cmd.Parameters.AddWithValue("NUM_DOC_PREIMP", Convert.ToInt32(factura));


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

        // valida si existe codigo de DETALLE DIRECCION EN EXACTUS
        private bool existe_direccion(int codigo_direccion)
        {
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("SELECT COUNT (*)  FROM [EXACTUS].[dismo].[DETALLE_DIRECCION]  where DETALLE_DIRECCION = '" + codigo_direccion + "'", con.conex);
            cmd.Parameters.AddWithValue("DETALLE_DIRECCIONP", codigo_direccion);


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

        private bool existe_NIT(string NIT)
        {
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("SELECT COUNT (*)  FROM [EXACTUS].[dismo].[NIT] where NIT = '" + NIT + "'", con.conex);
            cmd.Parameters.AddWithValue("NIT", NIT);


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

        // Valida si existe la linea del documento
        private bool existe_linea_Documento(string factura, string linea, string serie, string tipo_doc,string fecha)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("SELECT COUNT (*)  FROM [DM].[STREET].[DET_PED_STREET] DET INNER JOIN [DM].[STREET].[ENC_PED_STREET]ENC ON DET.NUM_DOC_SIS = ENC.NUM_DOC_SIS WHERE DET.NUM_DOC_SIS like '%"+factura+"%' and DET.NUMERO_LINEA = '"+linea+"' and DET.SERIE_DOC = '"+serie+"' and DET.TIPO_DOC = '"+tipo_doc+"' and ENC.FECHA_PEDIDO = '"+fecha+"'", con.condm);
            cmd.Parameters.AddWithValue("NUM_DOC_SIS", Convert.ToInt32(factura));
            cmd.Parameters.AddWithValue("NUMERO_LINEA", Convert.ToInt32(linea));
            cmd.Parameters.AddWithValue("SERIE_DOC", Convert.ToString(serie));
            cmd.Parameters.AddWithValue("TIPO_DOC", Convert.ToString(tipo_doc));


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


        // Valida si existe producto
        private bool existe_codproduct(string articulo)
        {
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*)  FROM [EXACTUS].[dismo].[ARTICULO] WHERE ARTICULO = '" + articulo + "'", con.conex);
            cmd.Parameters.AddWithValue("ARTICULO ", articulo);


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

        private void linkLabel8_MouseClick(object sender, MouseEventArgs e)
        {
            consulta = 8;
            informacion_import_street inf = new informacion_import_street();
            inf.ShowDialog();


        }


        // Proceso Carga de Encabezados de Facturas a tabla temporal
        private void backgrounFact_DoWork(object sender, DoWorkEventArgs e)
        {
            toolStripStatusLabel1.Text = "CARGANDO FACTURAS UNILEVER...";
            statusStrip1.BackColor = Color.Azure;
            procesados = 0;
           

            for (int i = 0; i < Encabezados_PED.Rows.Count; i++)
            {
                producto_noenonctrado = 0;
                DataRow row = Encabezados_PED.Rows[i];

                string factura = Convert.ToString(row["Numero"]);
                string correlativo = Convert.ToString(row["Numero_control"]);
                correlativo = correlativo.Replace("-", "");
                string serie = Convert.ToString(row["Serie"]);
                string tipo_doc = Convert.ToString(row["Tipodoc"]);
                string Vendedor = Convert.ToString(row["Vendedor"]);

                //factura = "0" + factura;



                if (DBNull.Value != (row["Numero"]))
                {


                    con.conectar("DM");
                    SqlCommand comand7 = new SqlCommand("SELECT [CODIGO]  FROM [DM].[STREET].[VENDEDORES] Where CODIGO_PW = '" + Vendedor + "'", con.condm);
                    SqlDataReader dr7 = comand7.ExecuteReader();

                    while (dr7.Read())
                    {
                        Vendedor = Convert.ToString(dr7["CODIGO"]);
                    }

                    dr7.Close();

                    con.Desconectar("DM");

                    string Entrega = Convert.ToString(row["Transportista"]);

                    con.conectar("DM");
                    SqlCommand comand8 = new SqlCommand("SELECT [CODIGO]  FROM [DM].[STREET].[ENTREGADORES]  WHERE COD_STREET = '" + Entrega + "'", con.condm);
                    SqlDataReader dr8 = comand8.ExecuteReader();

                    while (dr8.Read())
                    {
                        Entrega = Convert.ToString(dr8["CODIGO"]);
                    }

                    dr8.Close();

                    con.Desconectar("DM");
                    DateTime FECHA_LIQ;
                    Pw_cliente = Convert.ToString(row["Codigo_cliente"]);
                    string cliente_street = Convert.ToString(row["Codigo_cliente"]);
                    DateTime FECHA = Convert.ToDateTime(row["Fechapedido"]);
                    DateTime FECHA_INI = Convert.ToDateTime(row["Fechaini"]);
                    DateTime FECHA_FIN = Convert.ToDateTime(row["Fechafin"]);
                    if (DBNull.Value == row["Fechaliq"] || Convert.ToString(row["Fechaliq"]) == "")
                    {
                         FECHA_LIQ = Convert.ToDateTime(row["Fechafin"]);
                    }
                    else
                    {
                         FECHA_LIQ = Convert.ToDateTime(row["Fechaliq"]);
                    }
                    double iva = Convert.ToDouble(row["Impuesto"]);
                    double percep = Convert.ToDouble(row["Impperc"]);
                    double retencion = Convert.ToDouble(row["Impret"]);
                    double monto_sin_iva = Convert.ToDouble(row["Monto_sin_imp"]);
                    double monto_con_iva = Convert.ToDouble(row["Monto_con_imp"]);
                    double descuento_ciente = Convert.ToDouble(row["Dctocliente"]);
                    double descuento_linea = Convert.ToDouble(row["Dctoxlinea"]);
                    int cantidad_item = Convert.ToInt32(row["Cantidad_items"]);
                    string lista_precio = Convert.ToString(row["Listaprec"]);
                    descuento_ciente = descuento_ciente * (-1);


                    if (tipo_doc == "NC" || tipo_doc == "nc")
                    {
                        iva = iva * (-1);
                        percep = percep * (-1);
                        retencion = retencion * (-1);
                        monto_sin_iva = monto_sin_iva * (-1);
                        monto_con_iva = monto_con_iva * (-1);
                        descuento_ciente = descuento_ciente * (-1);


                        if (serie == "M")
                        {
                            correlativo = factura;
                            serie = "N400";
                        }
                    }



                    con.conectar("DM");
                    SqlCommand comand2 = new SqlCommand("SELECT[NUM_EXACTUS]  FROM [DM].[STREET].[LISTA_PRECIOS]  Where CODIGO = '" + lista_precio + "'", con.condm);
                    SqlDataReader dr2 = comand2.ExecuteReader();

                    while (dr2.Read())
                    {
                        lista_precio = Convert.ToString(dr2["NUM_EXACTUS"]);
                    }

                    dr2.Close();

                    con.Desconectar("DM");


                    string estado = Convert.ToString(row["Estado_pedido"]);


                    string Credito_cliente = Convert.ToString(row["Perfilc"]);

                    if (Credito_cliente.Length == 1)
                    {
                        Credito_cliente = "0" + Credito_cliente;

                    }

                    string Bodega = Convert.ToString(row["Deposito"]);


                    con.conectar("DM");
                    SqlCommand comand3 = new SqlCommand("SELECT [BODEGA_EXACTUS]  FROM [DM].[STREET].[BODEGAS]  where ID_STREET = '" + Bodega + "'", con.condm);
                    SqlDataReader dr3 = comand3.ExecuteReader();

                    while (dr3.Read())
                    {
                        Bodega = Convert.ToString(dr3["BODEGA_EXACTUS"]);
                    }

                    dr3.Close();

                    con.Desconectar("DM");


                    string Latitud = Convert.ToString(row["Latitud"]);
                    string Longitud = Convert.ToString(row["Longitud"]);




                    if (Pw_cliente.Length <= 4)
                    {

                        int ceros = 5 - Pw_cliente.Length;

                        switch (ceros)
                        {
                            case 1:
                                Pw_cliente = "0" + Pw_cliente;
                                break;
                            case 2:
                                Pw_cliente = "00" + Pw_cliente;
                                break;
                            case 3:
                                Pw_cliente = "000" + Pw_cliente;
                                break;
                            case 4:
                                Pw_cliente = "0000" + Pw_cliente;
                                break;


                        }
                    }

                    if (existe_cliente(Pw_cliente))
                    {

                        if (existe_Documento_sys(factura, Pw_cliente, FECHA.ToString("yyyy/MM/dd")))
                        {
                            Errores.Rows.Add(factura, Pw_cliente, correlativo, FECHA, tipo_doc.ToUpper(), "Numero de Documento ya existe en la Base de datos");


                        }
                        else
                        {

                            string orderby = "Codreng";
                            DataRow[] lienas_detalle;

                            string exprecion = "Nrodoc = '" + factura + "'";


                            lienas_detalle = Detalle_PED.Select(exprecion, orderby);

                            if (lienas_detalle.Length >= 1)
                            {
                                producto_noenonctrado = 0;

                                for (int j = 0; j < lienas_detalle.Length; j++)
                                {

                                    string linea_articulo = Convert.ToString(lienas_detalle[j][5]);

                                    if (existe_codproduct(linea_articulo))
                                    {

                                    }
                                    else
                                    {
                                        producto_noenonctrado = producto_noenonctrado + 1;
                                        Articulo_noenc = linea_articulo;
                                        Errores.Rows.Add(factura, Pw_cliente, correlativo, FECHA, tipo_doc.ToUpper(), "No se encontro Articulo  " + Articulo_noenc + " en Exactus");
                                    }

                                }

                                if (producto_noenonctrado < 1)
                                {
                                    con.conectar("DM");
                                    SqlCommand cmd1 = new SqlCommand("[STREET].[INSERT_ENC_PED_STREET]", con.condm);
                                    cmd1.CommandType = CommandType.StoredProcedure;

                                    cmd1.Parameters.AddWithValue("@NUM_DOC_SIS", factura);
                                    cmd1.Parameters.AddWithValue("@NUM_DOC_PREIMP", correlativo);
                                    cmd1.Parameters.AddWithValue("@TIPO_DOC", tipo_doc.ToUpper());
                                    cmd1.Parameters.AddWithValue("@RUTA", Vendedor.Replace("V", "R"));
                                    cmd1.Parameters.AddWithValue("@VENDEDOR", Vendedor);
                                    cmd1.Parameters.AddWithValue("@ENTREGA", Entrega);
                                    cmd1.Parameters.AddWithValue("@COD_CLIE", Pw_cliente);
                                    cmd1.Parameters.AddWithValue("@COD_CLIE_ALT", cliente_street);
                                    cmd1.Parameters.AddWithValue("@FECHA_PEDIDO", FECHA);
                                    cmd1.Parameters.AddWithValue("@HORA_INICIO_PEDIDO", FECHA_INI);
                                    cmd1.Parameters.AddWithValue("@HORA_FIN_PEDIDO", FECHA_FIN);
                                    cmd1.Parameters.AddWithValue("@FECHA_DESPACHO", FECHA_LIQ);
                                    cmd1.Parameters.AddWithValue("@MONTO_IMP", iva);
                                    cmd1.Parameters.AddWithValue("@MONTO_IMP_PERC", percep);
                                    cmd1.Parameters.AddWithValue("@MONTO_IMP_RET", retencion);
                                    cmd1.Parameters.AddWithValue("@MONTO_SIN_IMP", monto_sin_iva);
                                    cmd1.Parameters.AddWithValue("@MONTO_CON_IMP", monto_con_iva);
                                    cmd1.Parameters.AddWithValue("@MONTO_DESC_CLIE", descuento_ciente);
                                    cmd1.Parameters.AddWithValue("@MONTO_DESC_LINEA", descuento_linea);
                                    cmd1.Parameters.AddWithValue("@CANT_ITEM", cantidad_item);
                                    cmd1.Parameters.AddWithValue("@LISTA_PRECIO", lista_precio);
                                    cmd1.Parameters.AddWithValue("@ESTADO_PEDIDO", estado.ToUpper());
                                    cmd1.Parameters.AddWithValue("@CONDICION_CLIENTE", Credito_cliente);
                                    cmd1.Parameters.AddWithValue("@BODEGA", "B400");
                                    cmd1.Parameters.AddWithValue("@LATITUD", Latitud);
                                    cmd1.Parameters.AddWithValue("@LONGITUD", Longitud);
                                    cmd1.Parameters.AddWithValue("@FECHA_CREA", fecha_proceso);
                                    cmd1.Parameters.AddWithValue("@USUARIO_CREA", Usuario);
                                    cmd1.Parameters.AddWithValue("@SERIE_DOC", serie);
                                    cmd1.Parameters.AddWithValue("@PROCESADO", "N");
                                    cmd1.ExecuteNonQuery();

                                    con.Desconectar("DM");


                                    //// CARGA DE LIENAS DE  PEDIDIO -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                                    Detalle_linea.Clear();

                                    var Query = from rows in Detalle_PED.AsEnumerable()
                                                where rows.Field<string>("Nrodoc") == factura
                                                select rows;
                                    Detalle_linea = Query.CopyToDataTable();




                                    for (int k = 0; k < Detalle_linea.Rows.Count; k++)
                                    {
                                        DataRow rowln = Detalle_linea.Rows[k];

                                        string facturaln = Convert.ToString(rowln["Nrodoc"]);
                                        string liena = Convert.ToString(rowln["Codreng"]);
                                        string serieln = Convert.ToString(rowln["Serie"]);
                                        string Tipodocln = Convert.ToString(rowln["Tipodoc"]);
                                        string Articuloln = Convert.ToString(rowln["Articulo"]);
                                        string COD_DOCUMETO = correlativo;
                                        // factura = '0' + factura;





                                        int cantidadln = Convert.ToInt32(rowln["Cantidad"]);
                                        double precio_listaln = Convert.ToDouble(rowln["Preclist"]);

                                        double Impuesto1ln = Convert.ToDouble(rowln["Impuesto1"]);
                                        double Impuesto2ln = Convert.ToDouble(rowln["Impuesto2"]);

                                        double Descuentoln = Convert.ToDouble(rowln["Dcto"]);

                                        Descuentoln = Descuentoln * (-1);

                                        double Monto_sin_ivaln = Convert.ToDouble(rowln["Monto_sin_imp"]);
                                        double Monto_con_ivaln = Convert.ToDouble(rowln["Monto_con_imp"]);
                                        string Lista_precioln = Convert.ToString(rowln["Listaprec"]);


                                        string Bonificadaln = "N";
                                        int liena_bonificarln = 5000;


                                        if (Tipodocln == "NC")
                                        {
                                            Impuesto1ln = Impuesto1ln * (-1);
                                            Impuesto2ln = Impuesto2ln * (-1);
                                            Monto_sin_ivaln = Monto_sin_ivaln * (-1);
                                            Monto_con_ivaln = Monto_con_ivaln * (-1);
                                           // Descuentoln = Descuentoln * (-1);
                                        }



                                        if (Monto_sin_ivaln <= 0.00000 || Monto_con_ivaln <= 0.00000)
                                        {
                                            Bonificadaln = "B";
                                            int linealn = Convert.ToInt32(liena);
                                            if (linealn == 0)
                                            {
                                                liena_bonificarln = Convert.ToInt32(liena);
                                            }
                                            else if (linealn > 0)
                                            {
                                                liena_bonificarln = (Convert.ToInt32(liena)) - 1;
                                            }

                                        }
                                        else
                                        {


                                        }

                                        con.conectar("DM");
                                        SqlCommand comand2ln = new SqlCommand("SELECT[NUM_EXACTUS]  FROM [DM].[STREET].[LISTA_PRECIOS]  Where CODIGO = '" + Lista_precioln + "'", con.condm);
                                        SqlDataReader dr2ln = comand2ln.ExecuteReader();

                                        while (dr2ln.Read())
                                        {
                                            Lista_precioln = Convert.ToString(dr2ln["NUM_EXACTUS"]);
                                        }

                                        dr2ln.Close();

                                        con.Desconectar("DM");

                                        double costoln = Convert.ToDouble(rowln["Preciocosto"]);
                                        string tipo_unidad = Convert.ToString(rowln["Exp"]);






                                        if (existe_linea_Documento(facturaln, liena, serieln, Tipodocln, FECHA.ToString("yyyy-MM-dd")))
                                        {

                                        }
                                        else
                                        {
                                            if (DBNull.Value == rowln["Cantidad"])
                                            {
                                                Errores.Rows.Add(facturaln, Pw_cliente, liena, fecha_proceso, Tipodocln, "No existe cantidad en linea");
                                            }



                                            con.conectar("DM");
                                            SqlCommand cmd1ln = new SqlCommand("[STREET].[INSERT_DET_PED_STREET]", con.condm);
                                            cmd1ln.CommandType = CommandType.StoredProcedure;

                                            cmd1ln.Parameters.AddWithValue("@NUM_DOC_SIS", facturaln);
                                            cmd1ln.Parameters.AddWithValue("@NUMERO_LINEA", liena);
                                            cmd1ln.Parameters.AddWithValue("@TIPO_DOC", Tipodocln.ToUpper());
                                            cmd1ln.Parameters.AddWithValue("@COD_ART", Articuloln);
                                            cmd1ln.Parameters.AddWithValue("@CATIDAD", cantidadln);
                                            cmd1ln.Parameters.AddWithValue("@PRECIO_UNIT", precio_listaln);
                                            cmd1ln.Parameters.AddWithValue("@MONTO_IMP1", Impuesto1ln);
                                            cmd1ln.Parameters.AddWithValue("@MONTO_IMP2", Impuesto2ln);
                                            cmd1ln.Parameters.AddWithValue("@MONTO_DESC_ART", Descuentoln);
                                            cmd1ln.Parameters.AddWithValue("@MONTO_DESC_FAM", Descuentoln);
                                            cmd1ln.Parameters.AddWithValue("@SUBTOTAL_LINEA", Monto_sin_ivaln);
                                            cmd1ln.Parameters.AddWithValue("@TIPO_UNIDA", tipo_unidad);
                                            cmd1ln.Parameters.AddWithValue("@LISTA_PRECIO", Lista_precioln);
                                            cmd1ln.Parameters.AddWithValue("@COSTO_ART", costoln);
                                            cmd1ln.Parameters.AddWithValue("@FECHA_CREA", fecha_proceso);
                                            cmd1ln.Parameters.AddWithValue("@USUARIO", Usuario);
                                            cmd1ln.Parameters.AddWithValue("@SERIE_DOC", serieln);
                                            cmd1ln.Parameters.AddWithValue("@PROCESADO", "N");
                                            cmd1ln.Parameters.AddWithValue("@NUM_DOC", COD_DOCUMETO);
                                            cmd1ln.Parameters.AddWithValue("@ART_BON", Bonificadaln);
                                            if (liena_bonificarln < 5000)
                                            {
                                                cmd1ln.Parameters.AddWithValue("@LINEA_ART_BON", liena_bonificarln);
                                            }
                                            else
                                            {

                                            }
                                            cmd1ln.ExecuteNonQuery();
                                            con.Desconectar("DM");
                                        }






                                    }




                                }


                                /// FIN CARGA DE LINEA PEDIDO ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------





                                procesados = procesados + 1;



                                int clien = 0;
                                int error = 0;
                                if (Clientes_Nuevos.Rows.Count > 0)
                                {
                                    clien = Clientes_Nuevos.Rows.Count;
                                }
                                else
                                {
                                    clien = 0;
                                }
                                if (Errores.Rows.Count > 0)
                                {
                                    error = Errores.Rows.Count;
                                }
                                else
                                {
                                    error = 0;

                                }
                                int percentage = (i + 1) * 100 / (Encabezados_PED.Rows.Count);
                                backgrounFact.ReportProgress(percentage);



                            }

                          

                            

                            else
                            {
                                Errores.Rows.Add(factura, Pw_cliente, correlativo, FECHA, tipo_doc.ToUpper(), "No se encontraron Detalles para esta Factura");
                            }

                        }





                    }
                    else
                    {


                        Clientes_Nuevos.Rows.Add(Pw_cliente, cliente_street, factura, correlativo, tipo_doc.ToUpper(), FECHA);


                    
                    }

                }
                }
        }

        private void backgrounFact_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;

            label10.Text = Convert.ToString(e.ProgressPercentage)+" %";
        }

        private void backgrounFact_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            progressBar1.Value = 0;

            toolStripStatusLabel1.Text = "CARGA FACTURAS REALIZADA..." + "  Pedidos Cargados: " + Convert.ToString(procesados);
            statusStrip1.BackColor = Color.DarkSeaGreen;

            if (Clientes_Nuevos.Rows.Count >= 1)
            {
                this.linkLabel8.Text = Convert.ToString(Clientes_Nuevos.Rows.Count) + " Clientes Nuevos Encontrador";

            }

            else
            {
                this.linkLabel8.Text = "";
            }

            if (Errores.Rows.Count >= 1)
            {
                linkLabel9.Show();
                linkLabel9.Text = Convert.ToString(Errores.Rows.Count) + "  Errores Encontrados";

                To_txt(Errores);

            }

            else
            {
                linkLabel9.Text = "";
            }



        //    backgroundDet_fac.RunWorkerAsync();

        }

        private void linkLabel9_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel9_MouseClick(object sender, MouseEventArgs e)
        {
            consulta = 9;
            informacion_import_street inf = new informacion_import_street();
            inf.ShowDialog();
        }
        // Proceso de Carga Detalle de Facturas a tabla temporal
        private void backgroundDet_fac_DoWork(object sender, DoWorkEventArgs e)
        {
            toolStripStatusLabel1.Text = "CARGANDO DETALLE FACTURAS...";
            statusStrip1.BackColor = Color.Chocolate;
            for (int i = 0; i < Detalle_PED.Rows.Count; i++)
            {
                DataRow row = Detalle_PED.Rows[i];

                string factura = Convert.ToString(row["Nrodoc"]);
                string liena = Convert.ToString(row["Codreng"]);
                string serie = Convert.ToString(row["Serie"]);
                string Tipodoc = Convert.ToString(row["Tipodoc"]);
                string Articulo = Convert.ToString(row["Codialte"]);
                string COD_DOCUMETO = "";
               // factura = '0' + factura;
                
                    con.conectar("DM");
                    SqlCommand comand3 = new SqlCommand("SELECT [NUM_DOC_PREIMP] FROM [DM].[STREET].[ENC_PED_STREET]  Where NUM_DOC_SIS = '" + factura + "' and TIPO_DOC = '" + Tipodoc.ToUpper() + "'", con.condm);
                    SqlDataReader dr3 = comand3.ExecuteReader();

                    while (dr3.Read())
                    {
                        COD_DOCUMETO = Convert.ToString(dr3["NUM_DOC_PREIMP"]);
                    }

                    dr3.Close();

                    con.Desconectar("DM");
                    


                    int cantidad = Convert.ToInt32(row["Cantidad"]);
                    double precio_lista = Convert.ToDouble(row["Preclist"]);

                    double Impuesto1 = Convert.ToDouble(row["Impuesto1"]);
                    double Impuesto2 = Convert.ToDouble(row["Impuesto2"]);
                   
                    double Descuento = Convert.ToDouble(row["Dcto"]);

                    Descuento = Descuento * (-1);

                    double Monto_sin_iva = Convert.ToDouble(row["Monto_sin_imp"]);
                    double Monto_con_iva = Convert.ToDouble(row["Monto_con_imp"]);
                    string Lista_precio = Convert.ToString(row["Listaprec"]);


                    string Bonificada = "N";
                    int liena_bonificar = 5000 ;

                    if (Monto_sin_iva <= 0.00000 || Monto_con_iva <= 0.00000)
                    {
                        Bonificada = "B";
                        int linea = Convert.ToInt32(liena);
                        if (linea == 0)
                        {
                            liena_bonificar = Convert.ToInt32(liena);
                        }
                        else if (linea > 0)
                        {
                            liena_bonificar = (Convert.ToInt32(liena)) - 1;
                        }

                    }
                    else
                    { 
                      
                    
                    }

                    con.conectar("DM");
                    SqlCommand comand2 = new SqlCommand("SELECT[NUM_EXACTUS]  FROM [DM].[STREET].[LISTA_PRECIOS]  Where CODIGO = '" + Lista_precio + "'", con.condm);
                    SqlDataReader dr2 = comand2.ExecuteReader();

                    while (dr2.Read())
                    {
                        Lista_precio = Convert.ToString(dr2["NUM_EXACTUS"]);
                    }

                    dr2.Close();

                    con.Desconectar("DM");

                    double costo = Convert.ToDouble(row["Preciocosto"]);
                    string tipo_unidad = Convert.ToString(row["Exp"]);




                    
                       
                        if (existe_linea_Documento(factura, liena, serie, Tipodoc,fecha_proceso.ToString()))
                        {

                        }
                        else
                        {
                            if (DBNull.Value == row["Cantidad"])
                            {
                                Errores.Rows.Add(factura, Pw_cliente, liena, fecha_proceso, Tipodoc, "No existe cantidad en linea");
                            }

                            

                            con.conectar("DM");
                            SqlCommand cmd1 = new SqlCommand("[STREET].[INSERT_DET_PED_STREET]", con.condm);
                            cmd1.CommandType = CommandType.StoredProcedure;

                            cmd1.Parameters.AddWithValue("@NUM_DOC_SIS", factura);
                            cmd1.Parameters.AddWithValue("@NUMERO_LINEA", liena);
                            cmd1.Parameters.AddWithValue("@TIPO_DOC", Tipodoc.ToUpper());
                            cmd1.Parameters.AddWithValue("@COD_ART", Articulo);
                            cmd1.Parameters.AddWithValue("@CATIDAD", cantidad);
                            cmd1.Parameters.AddWithValue("@PRECIO_UNIT", precio_lista);
                            cmd1.Parameters.AddWithValue("@MONTO_IMP1", Impuesto1);
                            cmd1.Parameters.AddWithValue("@MONTO_IMP2", Impuesto2);
                            cmd1.Parameters.AddWithValue("@MONTO_DESC_ART", Descuento);
                            cmd1.Parameters.AddWithValue("@MONTO_DESC_FAM", Descuento);
                            cmd1.Parameters.AddWithValue("@SUBTOTAL_LINEA", Monto_sin_iva);
                            cmd1.Parameters.AddWithValue("@TIPO_UNIDA", tipo_unidad);
                            cmd1.Parameters.AddWithValue("@LISTA_PRECIO", Lista_precio);
                            cmd1.Parameters.AddWithValue("@COSTO_ART", costo);
                            cmd1.Parameters.AddWithValue("@FECHA_CREA", fecha_proceso);
                            cmd1.Parameters.AddWithValue("@USUARIO", Usuario);
                            cmd1.Parameters.AddWithValue("@SERIE_DOC", serie);
                            cmd1.Parameters.AddWithValue("@PROCESADO", "N");
                            cmd1.Parameters.AddWithValue("@NUM_DOC", COD_DOCUMETO);
                            cmd1.Parameters.AddWithValue("@ART_BON", Bonificada);
                            if (liena_bonificar < 5000)
                            {
                                cmd1.Parameters.AddWithValue("@LINEA_ART_BON", liena_bonificar);
                            }
                            else
                            {
                               
                            }
                            cmd1.ExecuteNonQuery();
                            con.Desconectar("DM");
                        }

                  




                }




            




        }

        //Proceso Carga de Clientes desde csv  a tabla temporal
        private void backgroundCLIENTES_DoWork(object sender, DoWorkEventArgs e)
        {
            procesados = 0;
            int detalledirec = 0;

            for (int i = 0; i < clientes.Rows.Count; i++)
            {
                DataRow row = clientes.Rows[i];

               
                string codigo = Convert.ToString(row["Codigo"]);
                string Nombre = Convert.ToString(row["Razon"]);
                string Alias = Convert.ToString(row["Nombre"]);
                string Direccion = Convert.ToString(row["Direccion"]);
                string zona = Convert.ToString(row["Municipio"]);
                string nombre_filtro;

                if (Nombre.Length < 16)
                {
                    nombre_filtro = Nombre.Substring(0, Nombre.Length);
                }
                else
                {
                    nombre_filtro = Nombre.Substring(0, 16);
                }

                if (nombre_filtro == "CLIENTE EVENTUAL")
                {
                   // MessageBox.Show(nombre_filtro);
                }

                else
                {
                    con.conectar("EX");
                    SqlCommand comand2 = new SqlCommand("SELECT  [COD_EXATUS] FROM [DM].[STREET].[UBICACIONES] WHERE DESCRIPCION = '" + zona.ToUpper() + "'", con.conex);
                    SqlDataReader dr2 = comand2.ExecuteReader();

                    while (dr2.Read())
                    {
                        cod_zona = Convert.ToString(dr2["COD_EXATUS"]);
                    }

                    dr2.Close();

                    con.Desconectar("EX");
                    string listaprecio = Convert.ToString(row["Lista"]);




                    string Departamento = Convert.ToString(row["Municipio"]);
                   

                    con.conectar("EX");
                    SqlCommand comand3 = new SqlCommand("SELECT [LISTA_EXACTUS]  FROM [DM].[STREET].[LISTA_PRECIOS]  WHERE CODIGO = '" + listaprecio + "'", con.conex);
                    SqlDataReader dr3 = comand3.ExecuteReader();

                    while (dr3.Read())
                    {
                        listaprecio = Convert.ToString(dr3["LISTA_EXACTUS"]);
                    }

                    dr3.Close();

                    con.Desconectar("EX");


                    string Telefono1 = Convert.ToString(row["Telefono1"]);
                    string Telefono2 = Convert.ToString(row["Telefono2"]);
                    DateTime FECHA_IN = Convert.ToDateTime(row["Fechaing"]);
                    string condicion_pago = Convert.ToString(row["Condicion_pago"]);
                    con.conectar("EX");
                    SqlCommand comand6 = new SqlCommand("SELECT  [COD_EXACTUS]  FROM [DM].[STREET].[DIAS_CREDITO]  where DESCRIPCION = '" + condicion_pago + "'", con.conex);
                    SqlDataReader dr6 = comand6.ExecuteReader();

                    while (dr6.Read())
                    {
                        condicion_pago = Convert.ToString(dr6["COD_EXACTUS"]);
                    }

                    dr6.Close();

                    con.Desconectar("EX");

                    string Vendedor = Convert.ToString(row["Vendedor"]);

                    con.conectar("DM");
                    SqlCommand comand4 = new SqlCommand("SELECT [CODIGO]  FROM [DM].[STREET].[VENDEDORES] Where CODIGO_PW = '" + Vendedor + "'", con.condm);
                    SqlDataReader dr4 = comand4.ExecuteReader();

                    while (dr4.Read())
                    {
                        Vendedor = Convert.ToString(dr4["CODIGO"]);
                    }

                    dr4.Close();

                    con.Desconectar("DM");






                    string entregador = Convert.ToString(row["Transportista"]);


                    con.conectar("DM");
                    SqlCommand comand5 = new SqlCommand("SELECT [CODIGO]  FROM [DM].[STREET].[ENTREGADORES]  WHERE COD_STREET = '" + entregador + "'", con.condm);
                    SqlDataReader dr5 = comand5.ExecuteReader();

                    while (dr5.Read())
                    {
                        entregador = Convert.ToString(dr5["CODIGO"]);
                    }

                    dr5.Close();

                    con.Desconectar("DM");



                    string categoria = Convert.ToString(row["Documento_generar"]);


                    if (categoria.Substring(0, 7) == "Factura")
                    {
                        categoria = "N";
                        tipo_doc = "FCF";
                    }
                    else if (categoria.Substring(0, 3) == "CCF")
                    {
                        categoria = "C";
                        tipo_doc = "CCF";
                    }
                    else
                    {

                    }
                    string DUI = Convert.ToString(row["Numero_dui"]);
                    string registro = Convert.ToString(row["Registro_fiscal"]);
                    string NIT = Convert.ToString(row["Nit"]);


                    string tipo_contri = Convert.ToString(row["Tipo_contribuyente"]);
                    if (DBNull.Value != (row["Tipo_contribuyente"]) || tipo_contri != "")
                    {

                        //string tipo_contribuyente;

                        if (tipo_contri == "2")
                        {
                            tipo_contri = "O";
                        }
                        else
                        {
                            tipo_contri = "F";
                        }

                    }

                    else
                    {

                    }


                    string canal = Convert.ToString(row["Canal"]);







                    con.conectar("EX");
                    SqlCommand comand = new SqlCommand("SELECT TOP 1 [DETALLE_DIRECCION] FROM [EXACTUS].[dismo].[DETALLE_DIRECCION]  order by DETALLE_DIRECCION desc", con.conex);
                    SqlDataReader dr1 = comand.ExecuteReader();


                    while (dr1.Read())
                    {
                        detalledirec = Convert.ToInt32(dr1["DETALLE_DIRECCION"]);

                    }

                    detalledirec = detalledirec + 1;
                    dr1.Close();

                    con.Desconectar("EX");
                    //------------------  Consulta si existe cliente ---------------------------------------------------------
                    if (existe_cliente(codigo))
                    {
                        //----------------------- Ingreso a tablas de Errores  ---------------------------------
                        Errores.Rows.Add(codigo, Nombre, listaprecio, FECHA_IN, tipo_doc.ToUpper(), "Cliente ya existe en la Base de datos");
                    }

                    else
                    {

                        if (Vendedor == "")
                        {
                            Errores.Rows.Add(codigo, Nombre, listaprecio, FECHA_IN, tipo_doc.ToUpper(), "Cliente no tiene Asignado vendedor");
                        }
                        else
                        {



                            if (Direccion == "" || detalledirec == null)
                            {

                            }

                            else
                            {

                                //----------------------- Ingreso de Detalles de Direccion a EXACTUS ---------------------------------
                                if (existe_direccion(detalledirec))
                                {

                                }
                                else
                                {

                                    int percentage = (i + 1) * 100 / clientes.Rows.Count;
                                    backgroundCLIENTES.ReportProgress(percentage);
                                    procesados = procesados + 1;


                                    con.conectar("EX");
                                    Guid GuD = Guid.NewGuid();
                                    SqlCommand cmd3 = new SqlCommand();
                                    cmd3.Connection = con.conex;
                                    cmd3.CommandText = "INSERT INTO [EXACTUS].[dismo].[DETALLE_DIRECCION]([DETALLE_DIRECCION],[DIRECCION],[CAMPO_1],[CAMPO_2],[CAMPO_3],[CAMPO_4],[CAMPO_5],[CAMPO_6],[CAMPO_7],[CAMPO_8],[CAMPO_9],[CAMPO_10],[RowPointer],[NoteExistsFlag],[RecordDate],[CreatedBy],[UpdatedBy],[CreateDate])  VALUES(@DETALLE_DIRECCION,@DIRECCION,@CAMPO_1,@CAMPO_2,@CAMPO_3,@CAMPO_4,@CAMPO_5,@CAMPO_6,@CAMPO_7,@CAMPO_8,@CAMPO_9,@CAMPO_10,@RowPointer,@NoteExistsFlag,@RecordDate,@CreatedBy,@UpdatedBy,@CreateDate)";
                                    cmd3.Parameters.Add("@DETALLE_DIRECCION", SqlDbType.Int).Value = detalledirec;
                                    cmd3.Parameters.Add("@DIRECCION", SqlDbType.VarChar).Value = "ESTANDAR";
                                    cmd3.Parameters.Add("@CAMPO_1", SqlDbType.VarChar).Value = Direccion;
                                    cmd3.Parameters.Add("@CAMPO_2", SqlDbType.VarChar).Value = "";
                                    cmd3.Parameters.Add("@CAMPO_3", SqlDbType.VarChar).Value = "";
                                    cmd3.Parameters.Add("@CAMPO_4", SqlDbType.VarChar).Value = "";
                                    cmd3.Parameters.Add("@CAMPO_5", SqlDbType.VarChar).Value = "";
                                    cmd3.Parameters.Add("@CAMPO_6", SqlDbType.VarChar).Value = "";
                                    cmd3.Parameters.Add("@CAMPO_7", SqlDbType.VarChar).Value = "";
                                    cmd3.Parameters.Add("@CAMPO_8", SqlDbType.VarChar).Value = "";
                                    cmd3.Parameters.Add("@CAMPO_9", SqlDbType.VarChar).Value = "";
                                    cmd3.Parameters.Add("@CAMPO_10", SqlDbType.NVarChar).Value = "";
                                    cmd3.Parameters.Add("@RowPointer", SqlDbType.UniqueIdentifier).Value = GuD;
                                    cmd3.Parameters.Add("@NoteExistsFlag", SqlDbType.TinyInt).Value = 0;
                                    cmd3.Parameters.Add("@RecordDate", SqlDbType.DateTime).Value = fecha_proceso;
                                    cmd3.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = Usuario;
                                    cmd3.Parameters.Add("@UpdatedBy", SqlDbType.VarChar).Value = Usuario;
                                    cmd3.Parameters.Add("@CreateDate", SqlDbType.DateTime).Value = fecha_proceso;




                                    cmd3.ExecuteNonQuery();


                                    con.Desconectar("EX");
                                }


                                if (DBNull.Value == (row["Nit"]) || NIT.Length < 17 || NIT == "")
                                {
                                    NIT = "ND";
                                }

                                else
                                {
                                    //----------------------- Ingreso de Detalles de NIT a EXACTUS ---------------------------------
                                    if (existe_NIT(NIT))
                                    {

                                    }
                                    else
                                    {
                                        con.conectar("EX");
                                        Guid GuD1 = Guid.NewGuid();
                                        SqlCommand cmd4 = new SqlCommand();
                                        cmd4.Connection = con.conex;
                                        cmd4.CommandText = "INSERT INTO [EXACTUS].[dismo].[NIT]([NIT],[RAZON_SOCIAL],[ALIAS],[NOTAS],[TIPO],[DIGITO_VERIFICADOR],[RowPointer],[NoteExistsFlag],[RecordDate],[CreatedBy],[UpdatedBy],[CreateDate])  VALUES(@NIT,@RAZON_SOCIAL,@ALIAS,@NOTAS,@TIPO,@DIGITO_VERIFICADOR,@RowPointer,@NoteExistsFlag,@RecordDate,@CreatedBy,@UpdatedBy,@CreateDate)";
                                        cmd4.Parameters.Add("@NIT", SqlDbType.VarChar).Value = NIT;
                                        cmd4.Parameters.Add("@RAZON_SOCIAL", SqlDbType.VarChar).Value = Nombre;
                                        cmd4.Parameters.Add("@ALIAS", SqlDbType.VarChar).Value = Alias;
                                        cmd4.Parameters.Add("@NOTAS", SqlDbType.VarChar).Value = "";
                                        cmd4.Parameters.Add("@TIPO", SqlDbType.VarChar).Value = "ND";
                                        cmd4.Parameters.Add("@DIGITO_VERIFICADOR", SqlDbType.VarChar).Value = "";
                                        cmd4.Parameters.Add("@RowPointer", SqlDbType.UniqueIdentifier).Value = GuD1;
                                        cmd4.Parameters.Add("@NoteExistsFlag", SqlDbType.TinyInt).Value = 0;
                                        cmd4.Parameters.Add("@RecordDate", SqlDbType.DateTime).Value = fecha_proceso;
                                        cmd4.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = Usuario;
                                        cmd4.Parameters.Add("@UpdatedBy", SqlDbType.VarChar).Value = Usuario;
                                        cmd4.Parameters.Add("@CreateDate", SqlDbType.DateTime).Value = fecha_proceso;

                                        cmd4.ExecuteNonQuery();

                                        con.Desconectar("EX");

                                    }

                                }

                                //----------------------- Ingreso de Detalles de CLIENTES a EXACTUS ---------------------------------


                                

                                con.conectar("EX");
                                SqlCommand cmd1 = new SqlCommand("[dismo].[DM_INSERTCLIE_STREET]", con.conex);
                                cmd1.CommandType = CommandType.StoredProcedure;

                                cmd1.Parameters.AddWithValue("@CLIENTE", codigo);
                                cmd1.Parameters.AddWithValue("@NOMBRE", Nombre);
                                cmd1.Parameters.AddWithValue("@DETALLE_DIRECCION", detalledirec);
                                cmd1.Parameters.AddWithValue("@ALIAS", Alias);
                                cmd1.Parameters.AddWithValue("@DIRECCION", Direccion);
                                cmd1.Parameters.AddWithValue("@CONDICION_CREDITO", condicion_pago);
                                cmd1.Parameters.AddWithValue("@LISTA_PRECIO", listaprecio);
                                cmd1.Parameters.AddWithValue("@TELEFONO1", Telefono1);
                                cmd1.Parameters.AddWithValue("@TELEFONO2", Telefono2);
                                cmd1.Parameters.AddWithValue("@TIPO_DOCUMENTO", tipo_doc.ToUpper());
                                cmd1.Parameters.AddWithValue("@DUI", DUI);
                                cmd1.Parameters.AddWithValue("@CANAL", canal);
                                cmd1.Parameters.AddWithValue("@ZONA", cod_zona);
                                cmd1.Parameters.AddWithValue("@CLASEDOC", categoria);
                                cmd1.Parameters.AddWithValue("@TIPO_CONTRIBUYENTE", tipo_contri);
                                cmd1.Parameters.AddWithValue("@CONTRIBUYENTE", NIT);
                               // cmd1.Parameters.Add("@FECHA_INGRESO", SqlDbType.DateTime).Value = fecha_proceso;
                                cmd1.Parameters.AddWithValue("@FECHA_INGRESO", fecha_proceso);
                                cmd1.Parameters.AddWithValue("@RUTA", entregador);
                                cmd1.Parameters.AddWithValue("@VENDEDOR", Vendedor);
                                cmd1.Parameters.AddWithValue("@COBRADOR", entregador.Replace("E", "C"));
                                if (registro == "")
                                { }
                                else
                                {
                                    cmd1.Parameters.AddWithValue("@REGISTRO", registro);
                                }
                                cmd1.ExecuteNonQuery();



                                // DIRECCION DE ENBARQUE ------------------------------------------------------------------------------------
                                Guid GuD2 = Guid.NewGuid();
                                SqlCommand cmd6 = new SqlCommand();
                                cmd6.Connection = con.conex;
                                cmd6.CommandText = "INSERT INTO [EXACTUS].[dismo].[DIRECC_EMBARQUE]( [CLIENTE],[DIRECCION],[DESCRIPCION],[CONTACTO],[CARGO],[TELEFONO1],[TELEFONO2],[FAX],[EMAIL],[RowPointer],[NoteExistsFlag],[RecordDate],[CreatedBy],[UpdatedBy],[CreateDate])  VALUES( @CLIENTE,@DIRECCION,@DESCRIPCION,@CONTACTO,@CARGO,@TELEFONO1,@TELEFONO2,@FAX,@EMAIL,@RowPointer,@NoteExistsFlag,@RecordDate,@CreatedBy,@UpdatedBy,@CreateDate)";
                                cmd6.Parameters.Add("@CLIENTE", SqlDbType.VarChar).Value = codigo;
                                cmd6.Parameters.Add("@DIRECCION", SqlDbType.VarChar).Value = "ND";
                                // cmd6.Parameters.Add("@DETALLE_DIRECCION", SqlDbType.Int).Value = 0;
                                cmd6.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar).Value = "NULL";
                                cmd6.Parameters.Add("@CONTACTO", SqlDbType.VarChar).Value = "NULL";
                                cmd6.Parameters.Add("@CARGO", SqlDbType.VarChar).Value = "NULL";
                                cmd6.Parameters.Add("@TELEFONO1", SqlDbType.VarChar).Value = "NULL";
                                cmd6.Parameters.Add("@TELEFONO2", SqlDbType.VarChar).Value = "NULL";
                                cmd6.Parameters.Add("@FAX", SqlDbType.VarChar).Value = "NULL";
                                cmd6.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = "NULL";
                                cmd6.Parameters.Add("@RowPointer", SqlDbType.UniqueIdentifier).Value = GuD2;
                                cmd6.Parameters.Add("@NoteExistsFlag", SqlDbType.TinyInt).Value = 0;
                                cmd6.Parameters.Add("@RecordDate", SqlDbType.DateTime).Value = fecha_proceso;
                                cmd6.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = Usuario;
                                cmd6.Parameters.Add("@UpdatedBy", SqlDbType.VarChar).Value = Usuario;
                                cmd6.Parameters.Add("@CreateDate", SqlDbType.DateTime).Value = fecha_proceso;




                                cmd6.ExecuteNonQuery();

                                con.Desconectar("EX");










                            }



                        }

                        if (Exists_FR_cli_rt_(codigo))
                        {



                            con.conectar("DM");

                            SqlCommand cmd1 = new SqlCommand("[CORRECT].[CREACLIE_FR]", con.condm);
                            cmd1.CommandType = CommandType.StoredProcedure;

                            cmd1.Parameters.AddWithValue("@TABLA", 1);
                            cmd1.Parameters.AddWithValue("@CODCLI", codigo);
                            cmd1.Parameters.AddWithValue("@NOMBRE", Nombre);
                            cmd1.Parameters.AddWithValue("@empresa", Login.empresa);
                            cmd1.ExecuteNonQuery();

                            con.Desconectar("DM");

                        }

                        if (Exists_FR_asoc_rt_(codigo))
                        {



                            con.conectar("DM");

                            SqlCommand cmd2 = new SqlCommand("[CORRECT].[CREACLIE_FR]", con.condm);
                            cmd2.CommandType = CommandType.StoredProcedure;

                            cmd2.Parameters.AddWithValue("@TABLA", 2);
                            cmd2.Parameters.AddWithValue("@CODCLI", codigo);
                            cmd2.Parameters.AddWithValue("@NOMBRE", Nombre);
                            cmd2.Parameters.AddWithValue("@empresa", Login.empresa);

                            cmd2.ExecuteNonQuery();

                            con.Desconectar("DM");
                        }


                    }

                }
            }
        }

        private void textBox4_DoubleClick(object sender, EventArgs e)
        {
            examinar();

            textBox4.Text = Selected_File;
        }

        private void linkLabel4_MouseClick(object sender, MouseEventArgs e)
        {
            consulta = 4;
            informacion_import_street inf = new informacion_import_street();
            inf.ShowDialog();
        }

        private void backgroundCLIENTES_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripStatusLabel2.Text = "Clientes Cargasdo:"+Convert.ToString(procesados);
            if (Errores.Rows.Count >= 1)
            {
                linkLabel9.Show();
                linkLabel9.Text = Convert.ToString(Errores.Rows.Count) + "  Errores Encontrados";



            }

            else
            {
                linkLabel9.Text = "";
            }


        }

        private void backgroundDet_fac_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripStatusLabel1.Text = "CARGA FACTURAS REALIZADA..."+"  Pedidos Cargados: "+Convert.ToString(procesados);
            statusStrip1.BackColor = Color.DarkSeaGreen;

            if (Clientes_Nuevos.Rows.Count >= 1)
            {
                this.linkLabel8.Text = Convert.ToString(Clientes_Nuevos.Rows.Count) + " Clientes Nuevos Encontrador";

            }

            else
            {
                this.linkLabel8.Text = "";
            }

            if (Errores.Rows.Count >= 1)
            {
                linkLabel9.Show();
                linkLabel9.Text = Convert.ToString(Errores.Rows.Count) + "  Errores Encontrados";

                To_txt(Errores);
                
            }

            else
            {
                linkLabel9.Text = "";
            }

        }

        private void textBox3_DoubleClick(object sender, EventArgs e)
        {
            examinar();

            textBox3.Text = Selected_File;
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            consulta = 3;
            informacion_import_street inf = new informacion_import_street();
            inf.ShowDialog();
        }

        private void Insert_Exactus_FACTURAS()
        {




        }

        private void button3_Click(object sender, EventArgs e)
        {
            fecha_proceso = DateTime.Now;
            Encabezados_PED_Ex = new DataTable();
            Detalle_PED_Ex = new DataTable();
            cantidad_Documentos = 0;

            Encabezados_PED_Ex.Clear();
            Detalle_PED_Ex.Clear();


            backgroundDOCUMENTOS_to_EXACTUS.RunWorkerAsync();

            // Insert_Exactus_FACTURAS();


        }
        // carga de Cobros a tabla temporal
        private void backgroundCobros_DoWork(object sender, DoWorkEventArgs e)
        {
            toolStripStatusLabel1.Text = "Cargando Cobros....";

            for (int i = 0; i < Cobros.Rows.Count; i++)
            {
                DataRow row = Cobros.Rows[i];


                string documento = Convert.ToString(row["Numero_recibo"]);

                int caracteres = documento.Length - 6;
                string numero = documento.Substring(6, caracteres);
                documento = documento.Substring(1, 5);

                int ceros = 12 - (documento.Length + caracteres);

                switch (ceros)
                {
                    case 1:
                        documento = documento + "0" + numero;
                        break;
                    case 2:
                        documento = documento + "00" + numero;
                        break;
                    case 3:
                        documento = documento + "000" + numero;
                        break;
                    case 4:
                        documento = documento + "0000" + numero;
                        break;
                    case 5:
                        documento = documento + "00000" + numero;
                        break;
                    case 6:
                        documento = documento + "000000" + numero;
                        break;
                    case 7:
                        documento = documento + "0000000" + numero;
                        break;
                    case 8:
                        documento = documento + "00000000" + numero;
                        break;
                    case 9:
                        documento = documento + "000000000" + numero;
                        break;
                    case 10:
                        documento = documento + "0000000000" + numero;
                        break;

                    case 11:
                        documento = documento + "0000000000" + numero;
                        break;
                }








                string Vendedor = Convert.ToString(row["Vendedor"]);
                Vendedor = Vendedor.Replace("V", "R");

                string cliente = Convert.ToString(row["Codigo"]);
                string clientesys = Convert.ToString(row["Codigo"]);
                DateTime Fecha = Convert.ToDateTime(row["Fecha"]);
                string Estado = Convert.ToString(row["Estado"]);
                if (Estado != "A")
                {
                    Estado = "N";
                }
                string Total = Convert.ToString(row["Total"]);
                string Pagoefectivo = Convert.ToString(row["Pagoefectivo"]);
                string Monto_cheque = Convert.ToString(row["Monto_cheque"]);
                DateTime Fechaini = Convert.ToDateTime(row["Fechaini"]);
                DateTime Fechafin = Convert.ToDateTime(row["Fechafin"]);
                string Saldo = Convert.ToString(row["Saldo"]);
                string Documento_aplicado = Convert.ToString(row["Documento_aplicado"]);
                string tipo_pago = Convert.ToString(row["Tipo_pago"]);
                string serie_doc = Convert.ToString(row["Serie_documento_aplicado"]);


                if (existe_Documento_sys(Documento_aplicado,cliente,Fecha.ToString("yyyy/MM/dd")))
                {
                    con.conectar("DM");
                    SqlCommand comand2 = new SqlCommand("SELECT [NUM_DOC_PREIMP] FROM [DM].[STREET].[ENC_PED_STREET]  where NUM_DOC_SIS = '" + Documento_aplicado + "'", con.condm);
                    SqlDataReader dr2 = comand2.ExecuteReader();

                    while (dr2.Read())
                    {
                        Documento_aplicado = Convert.ToString(dr2["NUM_DOC_PREIMP"]);
                    }

                    dr2.Close();

                    con.Desconectar("DM");


                    con.conectar("DM");
                    SqlCommand cmd1 = new SqlCommand("[STREET].[INSERT_COBROS_STREET]", con.condm);
                    cmd1.CommandType = CommandType.StoredProcedure;

                    cmd1.Parameters.AddWithValue("@NUM_REC", documento);
                    cmd1.Parameters.AddWithValue("@RUTA", Vendedor);
                    cmd1.Parameters.AddWithValue("@VENDEDOR", Vendedor.Replace("R", "V"));
                    cmd1.Parameters.AddWithValue("@COD_CLIE", clientesys);
                    cmd1.Parameters.AddWithValue("@COD_CLIE_ALT", cliente);
                    cmd1.Parameters.AddWithValue("@FECHA_APLIC", Fecha);
                    cmd1.Parameters.AddWithValue("@ESTADO", Estado);
                    cmd1.Parameters.AddWithValue("@MONTO_DOC", Total);
                    cmd1.Parameters.AddWithValue("@MONTO_EFEC", Pagoefectivo);
                    cmd1.Parameters.AddWithValue("@MONTO_CHEQ", Monto_cheque);
                    cmd1.Parameters.AddWithValue("@FECHA_HORA_INICIO", Fechaini);
                    cmd1.Parameters.AddWithValue("@FECHA_HORA_FIN", Fechafin);
                    cmd1.Parameters.AddWithValue("@IMPRESO", "N");
                    cmd1.Parameters.AddWithValue("@TIPO", "5");
                    cmd1.Parameters.AddWithValue("@DOC_APLICA", Documento_aplicado);
                    cmd1.Parameters.AddWithValue("@TIPO_PAGO", tipo_pago);
                    cmd1.Parameters.AddWithValue("@SALDO_DOC", Saldo);
                    cmd1.Parameters.AddWithValue("@FECHA_CREA", Fecha);
                    cmd1.Parameters.AddWithValue("@USUARIO", Usuario);
                    cmd1.Parameters.AddWithValue("@SERIE_REC", serie_doc);
                    cmd1.Parameters.AddWithValue("@PROCESADO", 'N');

                    cmd1.ExecuteNonQuery();
                    con.Desconectar("DM");





                }
                else
                {
                    Errores.Rows.Add(documento, cliente, Documento_aplicado, Fecha, Estado, "No Existe  Factura para Recibo No. " + Documento_aplicado + " ");

                }






            }

        }


        private bool Exists_FR_cli_rt_(string cliente)
        {
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("select COUNT (*) from [EXACTUS].[ERPADMIN].[CLIENTE_RT] where CLIENTE = @cliente ", con.conex);
            cmd.Parameters.AddWithValue("cliente", cliente);

            int contar = Convert.ToInt32(cmd.ExecuteScalar());
            con.Desconectar("EX");

            if (contar == 0)
            {
                return true;

            }
            else
            {
                return false;
            }

        }

        private bool Exists_FR_asoc_rt_(string cliente)
        {
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("select COUNT (*) from [EXACTUS].[ERPADMIN].[CLIENTE_ASOC_RT] where CLIENTE = @cliente ", con.conex);
            cmd.Parameters.AddWithValue("cliente", cliente);

            int contar = Convert.ToInt32(cmd.ExecuteScalar());
            con.Desconectar("EX");

            if (contar == 0)
            {
                return true;

            }
            else
            {
                return false;
            }

        }

        private void backgroundCobros_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripStatusLabel1.Text = "Carga de Cobros Finalizada...";
        }
        // Inserta Cobros en las Tablas del FR
        private void backgroundCobros_to_EXACTUS_DoWork(object sender, DoWorkEventArgs e)
        {
            toolStripStatusLabel1.Text = "Importando Cobros a Exactus FR....";
            statusStrip1.BackColor = Color.Azure;

            con.conectar("DM");
            SqlCommand cmd3 = new SqlCommand("[STREET].[COBROS_STREET]", con.condm);
            cmd3.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
            da3.Fill(Cobros_ex);
            con.Desconectar("DM");

            if (Cobros_ex.Rows.Count < 1)
            {

            }
            else
            {

                for (int j = 0; j < Cobros_ex.Rows.Count; j++)
                {
                    DataRow row2 = Cobros_ex.Rows[j];

                    string COD_CIA = Convert.ToString(row2["COD_CIA"]);
                    string NUM_REC = Convert.ToString(row2["NUM_REC"]);
                    string COD_ZON = Convert.ToString(row2["RUTA"]);
                    string NUM_DOC_AF = Convert.ToString(row2["DOC_APLICA"]);
                    string COD_CLT = Convert.ToString(row2["COD_CLIE"]);
                    DateTime FEC_DOC = Convert.ToDateTime(row2["FECHA_CREA"]);
                    DateTime FEC_PROC = Convert.ToDateTime(row2["FECHA_APLIC"]);
                    string IND_ANL = Convert.ToString(row2["ESTADO"]);
                    string MON_MOV_LOCAL = Convert.ToString(row2["MONTO_DOC"]);
                    string MON_CHE_LOCAL = Convert.ToString(row2["MONTO_CHEQ"]);
                    string MON_SAL_LOC = Convert.ToString(row2["SALDO_DOC"]);
                    DateTime HOR_INI = Convert.ToDateTime(row2["FECHA_HORA_INICIO"]);
                    DateTime HOR_FIN = Convert.ToDateTime(row2["FECHA_HORA_FIN"]);
                    DateTime RecordDate = Convert.ToDateTime(row2["FECHA_APLIC"]);
                    DateTime CreateDate = Convert.ToDateTime(row2["FECHA_APLIC"]);
                    string CreatedBy = Convert.ToString(row2["USUARIOS"]);
                    string UpdatedBy = Convert.ToString(row2["USUARIOS"]);
                    string PROCESADO = Convert.ToString(row2["PROCESADO"]);



                    if (Exists_FR_CXC_DOCAPL_rec(NUM_REC))
                    {
                        Errores.Rows.Add(NUM_REC, COD_CLT, NUM_DOC_AF, FEC_DOC, COD_ZON, "Recibo ya existe en TABLA DE FR DOC_APLICA");
                    }
                    else
                    {
                        // MessageBox.Show("NO TABLA 2");

                        con.conectar("EX");
                        SqlCommand cmd5 = new SqlCommand("[dismo].[STREET_COBROS]", con.conex);
                        cmd5.CommandType = CommandType.StoredProcedure;
                        cmd5.Parameters.AddWithValue("@TABLA", 2);
                        cmd5.Parameters.AddWithValue("@COD_ZON", COD_ZON);
                        cmd5.Parameters.AddWithValue("@NUM_REC", NUM_REC);
                        cmd5.Parameters.AddWithValue("@NUM_DOC_AF", NUM_DOC_AF);
                        cmd5.Parameters.AddWithValue("@COD_CLT", COD_CLT);
                        cmd5.Parameters.AddWithValue("@FEC_DOC", FEC_DOC);
                        cmd5.Parameters.AddWithValue("@FEC_PROC", FEC_PROC);
                        cmd5.Parameters.AddWithValue("@IND_ANL", IND_ANL);
                        cmd5.Parameters.AddWithValue("@MON_MOV_LOCAL", MON_MOV_LOCAL);
                        cmd5.Parameters.AddWithValue("@MON_CHE_LOCAL", MON_CHE_LOCAL);
                        cmd5.Parameters.AddWithValue("@MON_SAL_LOC", MON_SAL_LOC);
                        cmd5.Parameters.AddWithValue("@HOR_INI", HOR_INI);
                        cmd5.Parameters.AddWithValue("@HOR_FIN", HOR_FIN);
                        cmd5.Parameters.AddWithValue("@RecordDate", RecordDate);
                        cmd5.Parameters.AddWithValue("@CreatedBy", CreatedBy);
                        cmd5.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                        cmd5.Parameters.AddWithValue("@CreateDate", CreateDate);
                        cmd5.ExecuteNonQuery();
                        con.Desconectar("EX");
                    }

                    if (Exists_FR_CXC_MOVDIR_rec(NUM_REC))
                    {
                        Errores.Rows.Add(NUM_REC, COD_CLT, NUM_DOC_AF, FEC_DOC, COD_ZON, "Recibo ya existe en TABLA DE FR MOV_DIR");

                    }
                    else
                    {
                        // MessageBox.Show("NO TABLA 1");


                        con.conectar("EX");
                        SqlCommand cmd4 = new SqlCommand("[dismo].[STREET_COBROS]", con.conex);
                        cmd4.CommandType = CommandType.StoredProcedure;
                        cmd4.Parameters.AddWithValue("@TABLA", 1);
                        cmd4.Parameters.AddWithValue("@COD_ZON", COD_ZON);
                        cmd4.Parameters.AddWithValue("@NUM_REC", NUM_REC);
                        cmd4.Parameters.AddWithValue("@NUM_DOC_AF", NUM_DOC_AF);
                        cmd4.Parameters.AddWithValue("@COD_CLT", COD_CLT);
                        cmd4.Parameters.AddWithValue("@FEC_DOC", FEC_DOC);
                        cmd4.Parameters.AddWithValue("@FEC_PROC", FEC_PROC);
                        cmd4.Parameters.AddWithValue("@IND_ANL", IND_ANL);
                        cmd4.Parameters.AddWithValue("@MON_MOV_LOCAL", MON_MOV_LOCAL);
                        cmd4.Parameters.AddWithValue("@MON_CHE_LOCAL", MON_CHE_LOCAL);
                        cmd4.Parameters.AddWithValue("@MON_SAL_LOC", MON_SAL_LOC);
                        cmd4.Parameters.AddWithValue("@HOR_INI", HOR_INI);
                        cmd4.Parameters.AddWithValue("@HOR_FIN", HOR_FIN);
                        cmd4.Parameters.AddWithValue("@RecordDate", RecordDate);
                        cmd4.Parameters.AddWithValue("@CreatedBy", CreatedBy);
                        cmd4.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                        cmd4.Parameters.AddWithValue("@CreateDate", CreateDate);
                        cmd4.ExecuteNonQuery();
                        con.Desconectar("EX");




                    }

                    if (PROCESADO != "S")
                    {
                        // acctualiza el estado del Recibo en la tabla temporal  

                        con.conectar("DM");
                        SqlCommand cmd8 = new SqlCommand();
                        cmd8.Connection = con.condm;
                        cmd8.CommandText = "UPDATE [DM].[STREET].[RECIBOS_STREET]SET PROCESADO = 'S'WHERE NUM_REC = @NUM_REC";
                        cmd8.Parameters.Add("@NUM_REC", SqlDbType.VarChar).Value = NUM_REC;

                        cmd8.ExecuteNonQuery();

                        con.Desconectar("DM");
                    }

                }
            }



        }

        private bool Exists_FR_CXC_MOVDIR_rec(string REC)
        {
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("select COUNT (*) from [EXACTUS].[ERPADMIN].[alCXC_MOV_DIR] where NUM_REC = @REC ", con.conex);
            cmd.Parameters.AddWithValue("REC", REC);

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

        private bool Exists_FR_CXC_DOCAPL_rec(string REC)
        {
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("select COUNT (*) from [EXACTUS].[ERPADMIN].[alCXC_DOC_APL] where NUM_REC = @REC ", con.conex);
            cmd.Parameters.AddWithValue("REC", REC);

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

        private void button4_Click(object sender, EventArgs e)
        {
            Cobros_ex = new DataTable();
            Cobros_ex.Clear();

            backgroundCobros_to_EXACTUS.RunWorkerAsync();
        }

        private void backgroundCobros_to_EXACTUS_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (Cobros_ex.Rows.Count < 1)
            {
                toolStripStatusLabel1.Text = "No Existen Recibos para Procesar";
                statusStrip1.BackColor = Color.Yellow;
            }
            else
            {

                toolStripStatusLabel1.Text = "Importacion de Cobros Finalizada..";
                statusStrip1.BackColor = Color.Coral;

                if (Errores.Rows.Count >= 1)
                {
                    linkLabel9.Text = Convert.ToString(Errores.Rows.Count) + "  Errores Encontrados";

                }

                else
                {
                    linkLabel9.Text = "";
                }
            }
        }


        // Ingresa los Documentos a las Tablas de EXACTUS ERPADMIN
        private void backgroundDOCUMENTOS_to_EXACTUS_DoWork(object sender, DoWorkEventArgs e)
        {
            toolStripStatusLabel1.Text = "Ingresando Documento";
            statusStrip1.BackColor = Color.CadetBlue;

            con.conectar("DM");
            SqlCommand cmd1 = new SqlCommand("[STREET].[ENC_FAC_STREET]", con.condm);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.CommandTimeout = 0;
            if (radioButton5.Checked)
            {
                cmd1.Parameters.AddWithValue("@TIPO_DOC", "N");
            }
            else
            {
                cmd1.Parameters.AddWithValue("@TIPO_DOC", "F");
            }
            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            da.Fill(Encabezados_PED_Ex);







            con.Desconectar("DM");


            for (int i = 0; i < Encabezados_PED_Ex.Rows.Count; i++)
            {




                DataRow row = Encabezados_PED_Ex.Rows[i];
               

                string COD_CIA = Convert.ToString(row["COD_CIA"]);
                string NUM_PED = Convert.ToString(row["NUM_PED"]);
                string COD_ZON = Convert.ToString(row["COD_ZON"]);
                string COD_CLT = Convert.ToString(row["COD_CLT"]);
                string TIPO_DOC = Convert.ToString(row["TIPO_DOC"]);
                DateTime HORA_FIN = Convert.ToDateTime(row["HORA_FIN"]);
                DateTime FEC_PED = Convert.ToDateTime(row["FEC_PED"]);
                DateTime FEC_DES = Convert.ToDateTime(row["FEC_DES"]);
                DateTime HORA_INI = Convert.ToDateTime(row["HORA_INI"]);
                string MON_IMP_VT = Convert.ToString(row["MON_IMP_VT"]);
                string MON_IMP_CS = Convert.ToString(row["MON_IMP_CS"]);
                string MON_CIV = Convert.ToString(row["MON_CIV"]);
                string MON_SIV = Convert.ToString(row["MON_SIV"]);
                string MON_DSC = Convert.ToString(row["MON_DSC"]);
                string NUM_ITM = Convert.ToString(row["NUM_ITM"]);
                string LST_PRE = Convert.ToString(row["LST_PRE"]);
                string ESTADO = Convert.ToString(row["ESTADO"]);

                string CONDICION_PAGO = Convert.ToString(row["CONDICION_PAGO"]);
                string BODEGA = Convert.ToString(row["BODEGA"]);
                string PAIS = Convert.ToString(row["PAIS"]);
                string CLASE_DOCUMENTO = Convert.ToString(row["CLASE_DOCUMENTO"]);
                string DIR_ENT = Convert.ToString(row["DIR_ENT"]);
                string DESC1 = Convert.ToString(row["DESC1"]);
                string DESC2 = Convert.ToString(row["DESC2"]);
                string MONT_DESC2 = Convert.ToString(row["MONT_DESC2"]);
                string DESCUENTO_CASCADA = Convert.ToString(row["DESCUENTO_CASCADA"]);
                string IMPRESO = Convert.ToString(row["IMPRESO"]);
                string CONSIGNACION = Convert.ToString(row["CONSIGNACION"]);
                string NCF_PREFIJO = Convert.ToString(row["NCF_PREFIJO"]);
                string NCF = Convert.ToString(row["NCF"]);
                //string NCF = "";

                Detalle_PED_Ex.Clear();

                con.conectar("DM");
                SqlCommand cmd2 = new SqlCommand("[STREET].[DETALLE_FACTURA_PARA_EXACTUSFR]", con.condm);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@NUM_DOC", NUM_PED);
                cmd2.Parameters.AddWithValue("@TIPO_DOC", TIPO_DOC);
               
              


                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                da2.Fill(Detalle_PED_Ex);
                con.Desconectar("DM");


                //con.conectar("EX");
                //SqlCommand comand2 = new SqlCommand("SELECT [ULTIMO_VALOR] FROM [EXACTUS].[dismo].[NCF_CONSECUTIVO]  Where PREFIJO = '"+NCF_PREFIJO+"'", con.conex);
                //  SqlDataReader dr2 = comand2.ExecuteReader();

                //while (dr2.Read())
                //{
                //    NCF_NUM = Convert.ToInt32(dr2["ULTIMO_VALOR"]);

                //}

                //dr2.Close();

                //con.Desconectar("EX");


                // NCF = Convert.ToString(NCF_NUM);
                switch (NCF.Length)
                {
                    case 1:
                        NCF = "0000000" + NCF;
                        break;
                    case 2:
                        NCF = "000000" + NCF;
                        break;
                    case 3:
                        NCF = "00000" + NCF;
                        break;
                    case 4:
                        NCF = "0000" + NCF;
                        break;
                    case 5:
                        NCF = "000" + NCF;
                        break;
                    case 6:
                        NCF = "00" + NCF;
                        break;
                    case 7:
                        NCF = "0" + NCF;
                        break;

                }

                string NIVEL_PRECIO = Convert.ToString(row["NIVEL_PRECIO"]);

                string MONEDA = Convert.ToString(row["MONEDA"]);

                DateTime RecordDate = Convert.ToDateTime(row["RecordDate"]);
                string CreatedBy = Convert.ToString(row["CreatedBy"]);
                string UpdatedBy = Convert.ToString(row["UpdatedBy"]);
                DateTime CreateDate = Convert.ToDateTime(row["CreateDate"]);
                if (TIPO_DOC == "N")
                {
                    #region "Devoluciones"


                    switch (NUM_PED.Length)
                    {
                        case 1:
                            NUM_PED = "0000000" + NUM_PED;
                            break;
                        case 2:
                            NUM_PED = "000000" + NUM_PED;
                            break;
                        case 3:
                            NUM_PED = "00000" + NUM_PED;
                            break;
                        case 4:
                            NUM_PED = "0000" + NUM_PED;
                            break;
                        case 5:
                            NUM_PED = "000" + NUM_PED;
                            break;
                        case 6:
                            NUM_PED = "00" + NUM_PED;
                            break;
                        case 7:
                            NUM_PED = "0" + NUM_PED;
                            break;
                       

                      

                    }
                  
                    if (NUM_PED.Length > 8)
                    {
                        int nun_pedido = Convert.ToInt32(NUM_PED);
                        if (COD_ZON.Length > 3)
                        {
                            num_dev = COD_ZON.Substring(1, 3) + "D" + Convert.ToString(nun_pedido);
                        }
                        else
                        {
                            num_dev = COD_ZON + "D" + Convert.ToString(nun_pedido);
                        }
                    }
                    else
                    {
                       num_dev = COD_ZON.Substring(1, 3) + "D" + NUM_PED;
                    }
                    if (existe_Devolucion_exactus(num_dev))
                    {

                        Errores.Rows.Add(num_dev, COD_CLT, MON_CIV, RecordDate, TIPO_DOC.ToUpper(), "DEVOLUCION YA EXISTE EN EXACTUS FR");
                    }
                    else
                    {
                        if (existe_Documento_exactusERP(num_dev))
                        {

                            Errores.Rows.Add(num_dev, COD_CLT, MON_CIV, RecordDate, TIPO_DOC.ToUpper(), "DEVOLUCION YA EXISTE EN EXACTUS ERP");
                        }
                        else
                        {
                            if (existe_Devolucion_exactus(num_dev))
                            {
                                Errores.Rows.Add(num_dev, COD_CLT, MON_CIV, RecordDate, TIPO_DOC.ToUpper(), "DEVOLUCION YA EXISTE EN EXACTUS FR");
                            }
                            else
                            {

                                if (Detalle_PED_Ex.Rows.Count < 1)
                                {
                                    Errores.Rows.Add(num_dev, clientes, MON_CIV, RecordDate, TIPO_DOC.ToUpper(), "No se encontraron lineas para este pedido");
                                }

                                else
                                {
                                    if (existe_cliente(COD_CLT))
                                    {

                                        if (ESTADO == "F")
                                        {
                                            ESTADO = "A";
                                        }


                                        con.conectar("EX");
                                        SqlCommand cmd5 = new SqlCommand("[dismo].[STREET_DEV]", con.conex);
                                        cmd5.CommandType = CommandType.StoredProcedure;

                                        cmd5.Parameters.AddWithValue("@NUM_DEV", num_dev);
                                        cmd5.Parameters.AddWithValue("@COD_CIA", COD_CIA);
                                        cmd5.Parameters.AddWithValue("@COD_ZON", COD_ZON);
                                        cmd5.Parameters.AddWithValue("@COD_CLT", COD_CLT);
                                        cmd5.Parameters.AddWithValue("@HOR_INI", HORA_INI);
                                        cmd5.Parameters.AddWithValue("@HOR_FIN", HORA_FIN);
                                        cmd5.Parameters.AddWithValue("@FEC_DEV", FEC_PED);
                                        cmd5.Parameters.AddWithValue("@OBS_DEV", null);
                                        cmd5.Parameters.AddWithValue("@NUM_ITM", NUM_ITM);
                                        cmd5.Parameters.AddWithValue("@LST_PRE", LST_PRE);
                                        cmd5.Parameters.AddWithValue("@EST_DEV", ESTADO);
                                        cmd5.Parameters.AddWithValue("@MON_SIV", MON_SIV);
                                        cmd5.Parameters.AddWithValue("@MON_DSC", MONT_DESC2);
                                        cmd5.Parameters.AddWithValue("@POR_DSC_AP", MONT_DESC2);
                                        cmd5.Parameters.AddWithValue("@MON_IMP_VT", MON_IMP_VT);
                                        cmd5.Parameters.AddWithValue("@MON_IMP_CS", MON_IMP_CS);
                                        cmd5.Parameters.AddWithValue("@COD_BOD", BODEGA);
                                        cmd5.Parameters.AddWithValue("@NCF_PREFIJO", NCF_PREFIJO);
                                        cmd5.Parameters.AddWithValue("@NCF", NCF);
                                        cmd5.Parameters.AddWithValue("@NIVEL_PRECIO", NIVEL_PRECIO);
                                        cmd5.Parameters.AddWithValue("@COD_PAIS", PAIS);
                                        cmd5.Parameters.AddWithValue("@CreatedBy", "sa");
                                        cmd5.Parameters.AddWithValue("@UpdatedBy", "sa");


                                        // cmd5.Parameters.AddWithValue("@RecordDate", FEC_PED);



                                        cmd5.ExecuteNonQuery();


                                        con.Desconectar("EX");



                                        for (int j = 0; j < Detalle_PED_Ex.Rows.Count; j++)
                                        {
                                            DataRow row2 = Detalle_PED_Ex.Rows[j];
                                            string ART_BON;


                                            string LINEA_NUM = Convert.ToString(row2["NUMERO_LINEA"]);
                                            string LINEA_BON = Convert.ToString(row2["LINEA_ART_BON"]);

                                            string NUM_DOC = Convert.ToString(row2["NUM_DOC"]);
                                            string COD_ART = Convert.ToString(row2["COD_ART"]);

                                            string MON_TOT = Convert.ToString(row2["MON_TOT"]);



                                            if ((Convert.ToDouble(MON_TOT)) <= 0.00)
                                            {
                                                ART_BON = "B";
                                            }
                                            else
                                            {
                                                ART_BON = Convert.ToString(row2["ART_BON"]);
                                            }




                                            string MON_PRC_MN = Convert.ToString(row2["MON_PRC_MN"]);
                                            string POR_DSC_AP = Convert.ToString(row2["POR_DSC_AP"]);

                                            string MON_DSC_DET = Convert.ToString(row2["MON_DSC"]);
                                            string MON_PRC_MX = Convert.ToString(row2["MON_PRC_MX"]);
                                            string CNT_MAX = Convert.ToString(row2["CNT_MAX"]);
                                            string CNT_MIN = Convert.ToString(row2["CNT_MIN"]);
                                            string LST_PR_DET = Convert.ToString(row2["LST_PRE"]);

                                            string MON_DSCL = Convert.ToString(row2["MON_DSC"]);







                                            if (existe_Devolucion_exactus(num_dev))
                                            {
                                                if (existe_linea_dev(num_dev, COD_ART, "B"))
                                                {
                                                    Update_linea_art_dev(num_dev, COD_ART, "B", MON_TOT, MON_PRC_MX, MON_PRC_MN, CNT_MAX, CNT_MIN);
                                                }
                                                else
                                                {
                                                    con.conectar("EX");
                                                    SqlCommand cmd6 = new SqlCommand("[dismo].[STREET_DET_DEV]", con.conex);
                                                    cmd6.CommandType = CommandType.StoredProcedure;


                                                    cmd6.Parameters.AddWithValue("@NUM_DEV", num_dev);
                                                    cmd6.Parameters.AddWithValue("@COD_CIA", COD_CIA);
                                                    cmd6.Parameters.AddWithValue("@COD_ZON", COD_ZON);
                                                    cmd6.Parameters.AddWithValue("@COD_ART", COD_ART);
                                                    cmd6.Parameters.AddWithValue("@IND_DEV", "B");
                                                    cmd6.Parameters.AddWithValue("@MON_TOT", MON_TOT);
                                                    cmd6.Parameters.AddWithValue("@MON_PRC_MX", MON_PRC_MX);
                                                    cmd6.Parameters.AddWithValue("@MON_PRC_MN", MON_PRC_MN);
                                                    cmd6.Parameters.AddWithValue("@CNT_MAX", CNT_MAX);
                                                    cmd6.Parameters.AddWithValue("@CNT_MIN", CNT_MIN);
                                                    cmd6.Parameters.AddWithValue("@LST_PRE", LST_PR_DET);
                                                    cmd6.Parameters.AddWithValue("@MON_DSC", MON_DSCL);
                                                    cmd6.Parameters.AddWithValue("@POR_DSC_AP", POR_DSC_AP);
                                                    cmd6.Parameters.AddWithValue("@CreateDate", CreateDate);
                                                    cmd6.Parameters.AddWithValue("@CreatedBy", "sa");
                                                    cmd6.Parameters.AddWithValue("@UpdatedBy", "sa");


                                                    // cmd6.Parameters.AddWithValue("@TOPE", COD_CIA);




                                                    cmd6.ExecuteNonQuery();

                                                    con.Desconectar("EX");


                                                    int percentage = (i + 1) * 100 / Encabezados_PED_Ex.Rows.Count;
                                                    backgroundDOCUMENTOS_to_EXACTUS.ReportProgress(percentage);

                                                    // acctualiza el estado del pedido en la tabla temporal  

                                                    con.conectar("DM");
                                                    SqlCommand cmd8 = new SqlCommand();
                                                    cmd8.Connection = con.condm;
                                                    cmd8.CommandText = "UPDATE [DM].[STREET].[ENC_PED_STREET]SET PROCESADO = 'S' WHERE NUM_DOC_PREIMP = @DOC_PREIM and TIPO_DOC = @TIPO_DOC";
                                                    cmd8.Parameters.Add("@DOC_PREIM", SqlDbType.VarChar).Value = NUM_PED;
                                                    cmd8.Parameters.Add("@TIPO_DOC", SqlDbType.VarChar).Value = "N";

                                                    cmd8.ExecuteNonQuery();

                                                    con.Desconectar("DM");

                                                    cantidad_Documentos = cantidad_Documentos + 1;
                                                }
                                            }
                                            else
                                            {
                                            }




                                        }


                                        Update_cnt_lineas_enq(num_dev);


                                    }
                                }


                            }

                        }
                    }
                }
                #endregion
                else
                #region "Facturas y Pedidos"
                {


                    if (existe_Documento_exactus(NUM_PED))
                    {

                        Errores.Rows.Add(NUM_PED, COD_CLT, MON_CIV, RecordDate, TIPO_DOC.ToUpper(), "FACTURA YA EXISTE EN EXACTUS FR");
                    }
                    else
                    {
                        if (existe_Documento_exactusERP(NUM_PED))
                        {
                            Errores.Rows.Add(NUM_PED, COD_CLT, MON_CIV, RecordDate, TIPO_DOC.ToUpper(), "FACTURA YA EXISTE EN EXACTUS ERP");
                        }
                        else
                        {


                            if (Detalle_PED_Ex.Rows.Count < 1)
                            {
                                Errores.Rows.Add(NUM_PED, clientes, MON_CIV, RecordDate, TIPO_DOC.ToUpper(), "No se encontraron lineas para este pedido");
                            }


                            else
                            {

                                if (existe_cliente(COD_CLT))
                                {
                                    con.conectar("EX");
                                    SqlCommand cmd5 = new SqlCommand("[dismo].[STREET_PED]", con.conex);
                                    cmd5.CommandType = CommandType.StoredProcedure;

                                    cmd5.Parameters.AddWithValue("@COD_CIA", COD_CIA);
                                    cmd5.Parameters.AddWithValue("@NUM_PED", NUM_PED);
                                    cmd5.Parameters.AddWithValue("@COD_ZON", COD_ZON);
                                    cmd5.Parameters.AddWithValue("@COD_CLT", COD_CLT);
                                    if (radioButton4.Checked)
                                    {
                                        cmd5.Parameters.AddWithValue("@TIP_DOC", "1");
                                    }
                                    else if (radioButton2.Checked)
                                    {
                                        cmd5.Parameters.AddWithValue("@TIP_DOC", "F");
                                    }

                                    cmd5.Parameters.AddWithValue("@HOR_FIN", HORA_FIN);
                                    cmd5.Parameters.AddWithValue("@FEC_PED", FEC_PED);
                                    cmd5.Parameters.AddWithValue("@FEC_DES", FEC_DES);
                                    cmd5.Parameters.AddWithValue("@HOR_INI", HORA_INI);
                                    cmd5.Parameters.AddWithValue("@MON_IMP_VT", MON_IMP_VT);
                                    cmd5.Parameters.AddWithValue("@MON_IMP_CS", MON_IMP_CS);
                                    cmd5.Parameters.AddWithValue("@MON_CIV", MON_CIV);
                                    cmd5.Parameters.AddWithValue("@MON_SIV", MON_SIV);
                                    cmd5.Parameters.AddWithValue("@MON_DSC", MONT_DESC2);
                                    cmd5.Parameters.AddWithValue("@NUM_ITM", NUM_ITM);
                                    cmd5.Parameters.AddWithValue("@LST_PRE", LST_PRE);

                                    if (radioButton4.Checked)
                                    {
                                        cmd5.Parameters.AddWithValue("@ESTADO", "N");
                                    }
                                    else if (radioButton2.Checked)
                                    {
                                        cmd5.Parameters.AddWithValue("@ESTADO", ESTADO);
                                    }


                                    cmd5.Parameters.AddWithValue("@COD_CND", CONDICION_PAGO);


                                    cmd5.Parameters.AddWithValue("@COD_BOD", COD_ZON.Replace("R", "B"));
                                    cmd5.Parameters.AddWithValue("@COD_PAIS", PAIS);

                                    cmd5.Parameters.AddWithValue("@CLASE", CLASE_DOCUMENTO);

                                    cmd5.Parameters.AddWithValue("@DESC1", DESC1);
                                    cmd5.Parameters.AddWithValue("@DESC2", DESC2);
                                    cmd5.Parameters.AddWithValue("@MONT_DESC1", MON_DSC);
                                    cmd5.Parameters.AddWithValue("@MONT_DESC2", MON_DSC);


                                    if (radioButton4.Checked)
                                    {
                                        cmd5.Parameters.AddWithValue("@IMPRESO", "N");
                                    }
                                    else if (radioButton2.Checked)
                                    {
                                        cmd5.Parameters.AddWithValue("@IMPRESO", IMPRESO);

                                        cmd5.Parameters.AddWithValue("@NCF_PREFIJO", NCF_PREFIJO);


                                        cmd5.Parameters.AddWithValue("@NCF", NCF);

                                    }



                                    cmd5.Parameters.AddWithValue("@NIVEL_PRECIO", NIVEL_PRECIO);
                                    cmd5.Parameters.AddWithValue("@RecordDate", FEC_PED);
                                    cmd5.Parameters.AddWithValue("@CreatedBy", "sa");
                                    cmd5.Parameters.AddWithValue("@UpdatedBy", "sa");
                                    cmd5.Parameters.AddWithValue("@CreateDate", FEC_PED);


                                    cmd5.ExecuteNonQuery();








                                    con.Desconectar("EX");



                                    for (int j = 0; j < Detalle_PED_Ex.Rows.Count; j++)
                                    {
                                        DataRow row2 = Detalle_PED_Ex.Rows[j];
                                        string ART_BON;


                                        string LINEA_NUM = Convert.ToString(row2["NUMERO_LINEA"]);
                                        string LINEA_BON = Convert.ToString(row2["LINEA_ART_BON"]);

                                        string NUM_DOC = Convert.ToString(row2["NUM_DOC"]);
                                        string COD_ART = Convert.ToString(row2["COD_ART"]);

                                        string MON_TOT = Convert.ToString(row2["MON_TOT"]);



                                        if ((Convert.ToDouble(MON_TOT)) <= 0.00)
                                        {
                                            ART_BON = "B";
                                        }
                                        else
                                        {
                                            ART_BON = Convert.ToString(row2["ART_BON"]);
                                        }




                                        string MON_PRC_MN = Convert.ToString(row2["MON_PRC_MN"]);
                                        string POR_DSC_AP = Convert.ToString(row2["POR_DSC_AP"]);

                                        string MON_DSC_DET = Convert.ToString(row2["MON_DSC"]);
                                        string MON_PRC_MX = Convert.ToString(row2["MON_PRC_MX"]);
                                        string CNT_MAX = Convert.ToString(row2["CNT_MAX"]);
                                        string CNT_MIN = Convert.ToString(row2["CNT_MIN"]);
                                        string LST_PR_DET = Convert.ToString(row2["LST_PRE"]);

                                        string MON_DSCL = Convert.ToString(row2["MON_DSC"]);





                                        if (existe_Documento_exactus(NUM_DOC))
                                        {
                                            con.conectar("EX");
                                            SqlCommand cmd6 = new SqlCommand("[dismo].[STREET_DET_PED]", con.conex);
                                            cmd6.CommandType = CommandType.StoredProcedure;

                                            cmd6.Parameters.AddWithValue("@NUM_LN", LINEA_NUM);
                                            cmd6.Parameters.AddWithValue("@NUM_PED", NUM_DOC);
                                            cmd6.Parameters.AddWithValue("@COD_CIA", COD_CIA);
                                            cmd6.Parameters.AddWithValue("@COD_ART", COD_ART);
                                            cmd6.Parameters.AddWithValue("@ART_BON", ART_BON);
                                            cmd6.Parameters.AddWithValue("@MON_DSC_MN", MON_PRC_MN);
                                            cmd6.Parameters.AddWithValue("@POR_DESC_AP", POR_DSC_AP);
                                            cmd6.Parameters.AddWithValue("@MON_TOT", MON_TOT);
                                            cmd6.Parameters.AddWithValue("@MON_DSC", MON_DSCL);
                                            cmd6.Parameters.AddWithValue("@MON_PRC_MX", MON_PRC_MX);
                                            cmd6.Parameters.AddWithValue("@CNT_MAX", CNT_MAX);
                                            cmd6.Parameters.AddWithValue("@CNT_MIN", CNT_MIN);
                                            cmd6.Parameters.AddWithValue("@COD_ART_RFR", LINEA_BON);
                                            cmd6.Parameters.AddWithValue("@LST_PRE", LST_PR_DET);
                                            // cmd6.Parameters.AddWithValue("@TOPE", COD_CIA);
                                            cmd6.Parameters.AddWithValue("@RecordDate", CreateDate);
                                            cmd6.Parameters.AddWithValue("@CreatedBy", CreatedBy);



                                            cmd6.ExecuteNonQuery();

                                            con.Desconectar("EX");



                                            int percentage = (i + 1) * 100 / Encabezados_PED_Ex.Rows.Count;
                                            backgroundDOCUMENTOS_to_EXACTUS.ReportProgress(percentage);

                                            // acctualiza el estado del pedido en la tabla temporal  

                                            con.conectar("DM");
                                            SqlCommand cmd8 = new SqlCommand();
                                            cmd8.Connection = con.condm;
                                            cmd8.CommandText = "UPDATE [DM].[STREET].[ENC_PED_STREET]SET PROCESADO = 'S' WHERE NUM_DOC_PREIMP = @DOC_PREIM";
                                            cmd8.Parameters.Add("@DOC_PREIM", SqlDbType.VarChar).Value = NUM_PED;

                                            cmd8.ExecuteNonQuery();

                                            con.Desconectar("DM");

                                            cantidad_Documentos = cantidad_Documentos + 1;

                                        }
                                        else
                                        {
                                        }


                                    }

                                    #endregion


                                  

                                }

                                else
                                {

                                }


                            }


                        }



                    }

                }

              


            }

        }

        private void backgroundDOCUMENTOS_to_EXACTUS_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            if (Encabezados_PED_Ex.Rows.Count < 1)
            {
                toolStripStatusLabel1.Text = "No Existen Documengtos para Procesar";
                statusStrip1.BackColor = Color.Yellow;
            }
            else
            {

                toolStripStatusLabel1.Text = "Importacion de Documentos Finalizada.. "+cantidad_Documentos+" Cargados... ";
                statusStrip1.BackColor = Color.Coral;
                insert_Ruta_regalias();


                if (Errores.Rows.Count >= 1)
                {
                    linkLabel9.Show();
                    linkLabel9.Text = Convert.ToString(Errores.Rows.Count) + "  Errores Encontrados";

                }

                else
                {
                    linkLabel9.Text = "";
                }

                
            }
        }

        private void backgroundDOCUMENTOS_to_EXACTUS_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundCLIENTES_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_DoubleClick(object sender, EventArgs e)
        {

            examinar();

            textBox6.Text = Selected_File;

        }

        private void textBox7_DoubleClick(object sender, EventArgs e)
        {
            examinar();

            textBox7.Text = Selected_File;
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            consulta = 5;
            informacion_import_street inf = new informacion_import_street();
            inf.ShowDialog();
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            consulta = 6;
            informacion_import_street inf = new informacion_import_street();
            inf.ShowDialog();
        }

        private void backgroundENC_COMPRAS_DoWork(object sender, DoWorkEventArgs e)
        {
            toolStripStatusLabel1.Text = "CARGANDO ENCABEZADOS COMPRAS...";
            statusStrip1.BackColor = Color.BurlyWood;


            for (int i = 0; i < Encabezados_COM.Rows.Count; i++)
            {
                
                DataRow row = Encabezados_COM.Rows[i];

                string Compra = Convert.ToString(row["Numero"]);
                string Serie = Convert.ToString(row["Serie"]);
                string Tipo_DOC = Convert.ToString(row["Tipodoc"]);


                string correlativo = Convert.ToString(row["Numero_control"]);
                correlativo = correlativo.Replace("-", "");


                    
            }






        }

        private void To_txt(DataTable dt)
        {
            string Direccion = @"C:\CORRECT\errores\";
            string fecha = DateTime.Now.ToString("ddMMyyyy");
        
      

             var result = new StringBuilder();


         

            
        foreach (DataRow row in dt.Rows)         
        {             
            for (int i = 0; i < dt.Columns.Count; i++)             
            {
              
                result.Append(row[i].ToString());                 
                result.Append(i == dt.Columns.Count - 1 ? "\n" : ",");             
            }
            result.AppendLine();
        }
 
        StreamWriter objWriter = new StreamWriter(Direccion+"Error_Carga_PW"+fecha+".txt", true);
        objWriter.WriteLine(result.ToString());
        objWriter.Close();

              
        
        
        }

        private bool existe_Documento_exactusERP(string factura)
        {
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*)  FROM [EXACTUS].[dismo].[FACTURA]  WHERE FACTURA='" + factura + "'", con.conex);
            cmd.Parameters.AddWithValue("FACTURA", factura);


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



       




        private void linkLabel10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CORECTX_APP.Informatica.Power_Street.Facturas_No_procesados fn = new CORECTX_APP.Informatica.Power_Street.Facturas_No_procesados();
            fn.Show();
        }

        private bool existe_linea_dev(string devol,string art,string indc)
        {
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*)   FROM[EXACTUS].[ERPADMIN].[alFAC_DET_DEV] WHERE NUM_DEV = '"+devol+"' AND COD_ART = '"+art+"' AND IND_DEV = '"+indc+"'", con.conex);
            //cmd.Parameters.AddWithValue("FACTURA", devol);


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

        private void Update_linea_art_dev(string devol, string art, string indc,string mon_tot ,string mon_prc_mx, string mon_prc_mn,string cnt_max ,string cnt_min)
        {

            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("UPDATE [EXACTUS].[ERPADMIN].[alFAC_DET_DEV] SET MON_TOT = MON_TOT+ '"+mon_tot+"' ,MON_PRC_MN = MON_PRC_MN + '"+mon_prc_mn+"',MON_PRC_MX = MON_PRC_MX +'"+mon_prc_mx+"',  CNT_MAX = CNT_MAX + '"+cnt_max+"' , CNT_MIN = CNT_MIN + '"+cnt_min+"' WHERE NUM_DEV = '" + devol+"' AND COD_ART = '"+art+"' AND IND_DEV = '"+indc+"'", con.conex);
            //cmd.Parameters.AddWithValue("FACTURA", devol);

            cmd.ExecuteNonQuery();
            con.Desconectar("EX");

        }
        private void Update_cnt_lineas_enq(string devol)
        {
            con.conectar("EX");

            SqlCommand cmdcnt = new SqlCommand("SELECT COUNT (*) FROM [EXACTUS].[ERPADMIN].[alFAC_DET_DEV]  where NUM_DEV = '"+devol+"'", con.conex);
            int num_linea = Convert.ToInt32(cmdcnt.ExecuteScalar());

            SqlCommand cmd = new SqlCommand("UPDATE [EXACTUS].[ERPADMIN].[alFAC_ENC_DEV] SET NUM_ITM = '"+num_linea+"' WHERE NUM_DEV ='"+devol+"'", con.conex);
            //cmd.Parameters.AddWithValue("FACTURA", devol);

            cmd.ExecuteNonQuery();
            con.Desconectar("EX");

        }
        private void insert_Ruta_regalias()
        {
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("INSERT INTO [EXACTUS].[ERPADMIN].[RUTA_CFG] SELECT COD_CIA,COD_ZON ,'UN1','P'+RIGHT(COD_ZON,3),'B400', GETDATE()-1 ,GETDATE()+20,NEWID(),0,GETDATE(),'sa','sa',GETDATE(),'V'+RIGHT(COD_ZON,3),'VENDEDOR'+' '+RIGHT(COD_ZON,3),'C'+RIGHT(COD_ZON,3),'COBRADOR'+' '+RIGHT(COD_ZON,3)  FROM [EXACTUS].[ERPADMIN].[alFAC_ENC_DEV]  where DOC_PRO is null and DATEPART(MONTH,FEC_DEV) >= DATEPART(MONTH,GETDATE()-36)  and COD_ZON not in (SELECT [RUTA] FROM [EXACTUS].[ERPADMIN].[RUTA_CFG])  group by COD_CIA,COD_ZON", con.conex);
            //cmd.Parameters.AddWithValue("FACTURA", devol);

            cmd.ExecuteNonQuery();
            con.Desconectar("EX");

        }
    }

}
