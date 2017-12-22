using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Sinconizacion_EXactus
{
    public partial class FrmKC_ExportadorVtaTxT : Form
    {
        DataTable KC = new DataTable();
        DataSet ds = new DataSet();
        conexionXML con = new conexionXML();
        public FrmKC_ExportadorVtaTxT()
        {
            InitializeComponent();
        }

        private void Generar_Click(object sender, EventArgs e)
        {

            
                try
                {
                    con.conectar("DM");
                    SqlDataAdapter da = new SqlDataAdapter("[CORRECT].[KC_EXPORTADOR]", con.condm);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.Add("@FINI", SqlDbType.DateTime);
                    da.SelectCommand.Parameters.Add("@FFIN", SqlDbType.DateTime);
         
                    da.SelectCommand.Parameters["@FINI"].Value = Convert.ToDateTime(FechaIni.Value.ToShortDateString());
                    da.SelectCommand.Parameters["@FFIN"].Value = Convert.ToDateTime(FechaFin.Value.ToShortDateString());
                   
                    KC.Clear();
                    da.Fill(KC);

                    this.dataGridView1.DataSource = KC;

                    con.Desconectar("DM");

                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se Pudo conectar a la BD Error: " + ex.Message);
                }
            

            //Generando Archivo
            dlGuardar.Filter = "Fichero TXT (*.txt)|*.txt";
            dlGuardar.FileName = "DatosKC";
            dlGuardar.Title = "Exportar a TxT";
            if (dlGuardar.ShowDialog() == DialogResult.OK)
            {
                StringBuilder csvMemoria = new StringBuilder();

                ////para los títulos de las columnas, encabezado
                //for (int i = 0; i < dt.Columns.Count; i++)
                //{
                //    if (i == dt.Columns.Count - 1)
                //    {
                //        csvMemoria.Append(String.Format("{0}", dt.Columns[i].Caption));
                //    }
                //    else
                //    {
                //        csvMemoria.Append(String.Format("{0},", dt.Columns[i].Caption));
                //    }
                //}
               // csvMemoria.AppendLine();

                //   csvMemoria.Append(String.Format("{0}|", dt.Rows[m].ItemArray[n].ToString().Substring(0, dt.Rows[m].ItemArray[n].ToString().Length-4)));
                this.progressBar1.Value=0;
                for (int m = 0; m < KC.Rows.Count; m++)
                {
                   
                    int x =KC.Rows.Count;

                    this.progressBar1.Increment(Convert.ToInt32 ( (Convert.ToDecimal(m) / Convert.ToDecimal(x)) * 100 ));

                    for (int n = 0; n < KC.Columns.Count; n++)
                    {
                        //si es la última columna no poner el |
                        if (n == KC.Columns.Count - 1)
                        { 
                                csvMemoria.Append(String.Format("{0}", KC.Rows[m].ItemArray[n].ToString().Trim()));
                        }
                        else
                        {
                            if (KC.Rows[m].ItemArray[n].GetType() == Type.GetType("System.DateTime"))
                            {
                                csvMemoria.Append(String.Format("{0}|", KC.Rows[m].ItemArray[n].ToString().Substring(0, 10)));
                            }
                            else
                            {
                          
                                if (KC.Rows[m].ItemArray[n].GetType() == Type.GetType("System.Decimal") )
                                {
                                    if (KC.Columns[n].Caption.Equals("LONGITUD") || KC.Columns[n].Caption.Equals("LATITUD"))
                                        csvMemoria.Append(String.Format("{0}|", KC.Rows[m].ItemArray[n].ToString().Trim()));
                                    else
                                        csvMemoria.Append(String.Format("{0}|", Math.Round(Convert.ToDecimal(KC.Rows[m].ItemArray[n]),3).ToString()));
                                }
                                else
                                    csvMemoria.Append(String.Format("{0}|", KC.Rows[m].ItemArray[n].ToString().Trim()));
                            }
                        }
                    }
                    csvMemoria.AppendLine();
                }
                System.IO.StreamWriter sw =
                    new System.IO.StreamWriter(dlGuardar.FileName, false,
                       System.Text.Encoding.Default);
                sw.Write(csvMemoria.ToString());
                sw.Close();
            }
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnFTP_Click(object sender, EventArgs e)
        {

            //GENERAR ARCHIVO 

            try
                {
                con.conectar("DM");
                    SqlDataAdapter da = new SqlDataAdapter("[CORRECT].[KC_EXPORTADOR]", con.condm);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.Add("@FINI", SqlDbType.DateTime);
                    da.SelectCommand.Parameters.Add("@FFIN", SqlDbType.DateTime);

                    //Calcular inicio de Mes los primeros 5 dias toma tambien los ultimos 5 dias del mes anterior
                    int diaini = 0;

                    if (DateTime.Today.Day <= 5)
                        diaini = (DateTime.Today.Day + 5)*-1;
                    else
                        diaini = (DateTime.Today.Day - 1) * -1;

                    da.SelectCommand.Parameters["@FINI"].Value = Convert.ToDateTime(DateTime.Today.AddDays(diaini));
                    da.SelectCommand.Parameters["@FFIN"].Value = Convert.ToDateTime(DateTime.Today);

                    KC.Clear();
                    da.Fill(KC);

                con.Desconectar("DM");
                    

                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se Pudo conectar a la BD Error: " + ex.Message);
                }
            

           
                StringBuilder csvMemoria = new StringBuilder();

                for (int m = 0; m < KC.Rows.Count; m++)
                {

                    int x = KC.Rows.Count;

                    for (int n = 0; n < KC.Columns.Count; n++)
                    {
                        //si es la última columna no poner el |
                        if (n == KC.Columns.Count - 1)
                        {
                            csvMemoria.Append(String.Format("{0}", KC.Rows[m].ItemArray[n].ToString().Trim()));
                        }
                        else
                        {
                            if (KC.Rows[m].ItemArray[n].GetType() == Type.GetType("System.DateTime"))
                            {
                                csvMemoria.Append(String.Format("{0}|", KC.Rows[m].ItemArray[n].ToString().Substring(0, 10)));
                            }
                            else
                            {

                                if (KC.Rows[m].ItemArray[n].GetType() == Type.GetType("System.Decimal"))
                                {
                                    if (KC.Columns[n].Caption.Equals("LONGITUD") || KC.Columns[n].Caption.Equals("LATITUD"))
                                        csvMemoria.Append(String.Format("{0}|", KC.Rows[m].ItemArray[n].ToString().Trim()));
                                    else
                                        csvMemoria.Append(String.Format("{0}|", Math.Round(Convert.ToDecimal(KC.Rows[m].ItemArray[n]), 3).ToString()));
                                }
                                else
                                    csvMemoria.Append(String.Format("{0}|", KC.Rows[m].ItemArray[n].ToString().Trim()));
                            }
                        }
                    }
                    csvMemoria.AppendLine();
                }

                //Fecha en formato ddmmyyyy
                string NArchivo;
                NArchivo = DateTime.Today.Day.ToString().PadLeft(2,'0') + DateTime.Today.Month.ToString().PadLeft(2,'0') + DateTime.Today.Year.ToString();

                string NomredelTXT;
                NomredelTXT= NArchivo+".txt";

                System.IO.StreamWriter sw =
                    new System.IO.StreamWriter(@"C:\CORRECT\DatosKC" + NomredelTXT, false, System.Text.Encoding.Default);
                sw.Write(csvMemoria.ToString());
                sw.Close();     

            //CARGAR AL FTP
            /* Create Object Instance */
            ftp ftpClient = new ftp(@"ftp://dt.kcmkt.com/", "9FRZ000001@dt9JAR.kcmkt.com", "Clave1");

            /* Upload a File */
            ftpClient.upload("/DatosKC" + NomredelTXT, @"C:\CORRECT\DatosKC" + NomredelTXT);

            MessageBox.Show("Carga Completa");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Exportador exportar = new Exportador();
            exportar.NombreReporte = "DATOS EXPORTADOS A KIMBERLY";
            KC.TableName = "KC";
            ds.Tables.Add(KC);
           
            exportar.aExcel(ds, FechaIni.Value, FechaFin.Value);
       //     exportar.aExcel(dt);
            
        }

        private void FrmKC_ExportadorVtaTxT_Load(object sender, EventArgs e)
        {

        }
    }
}
