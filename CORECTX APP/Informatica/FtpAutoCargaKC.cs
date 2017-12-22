using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Sinconizacion_EXactus
{
    class FtpAutoCargaKC
    {
        public void Ejecutar()
        {
            conexionXML con =  new conexionXML ();
            DataTable dt = new DataTable();
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
                        diaini = (DateTime.Today.Day + 5) * -1;
                    else
                        diaini = (DateTime.Today.Day - 1) * -1;

                    da.SelectCommand.Parameters["@FINI"].Value = Convert.ToDateTime(DateTime.Today.AddDays(diaini));
                    da.SelectCommand.Parameters["@FFIN"].Value = Convert.ToDateTime(DateTime.Today);

                    dt.Clear();
                    da.Fill(dt);

                    con.Desconectar("DM");

                }
                catch (Exception ex)
                {
                  //  MessageBox.Show("No se Pudo conectar a la BD Error: " + ex.Message);
                }
            

            StringBuilder csvMemoria = new StringBuilder();

            for (int m = 0; m < dt.Rows.Count; m++)
            {

                int x = dt.Rows.Count;

                for (int n = 0; n < dt.Columns.Count; n++)
                {
                    //si es la última columna no poner el |
                    if (n == dt.Columns.Count - 1)
                    {
                        csvMemoria.Append(String.Format("{0}", dt.Rows[m].ItemArray[n].ToString().Trim()));
                    }
                    else
                    {
                        if (dt.Rows[m].ItemArray[n].GetType() == Type.GetType("System.DateTime"))
                        {
                            csvMemoria.Append(String.Format("{0}|", dt.Rows[m].ItemArray[n].ToString().Substring(0, 10)));
                        }
                        else
                        {

                            if (dt.Rows[m].ItemArray[n].GetType() == Type.GetType("System.Decimal"))
                            {
                                if (dt.Columns[n].Caption.Equals("LONGITUD") || dt.Columns[n].Caption.Equals("LATITUD"))
                                    csvMemoria.Append(String.Format("{0}|", dt.Rows[m].ItemArray[n].ToString().Trim()));
                                else
                                    csvMemoria.Append(String.Format("{0}|", Math.Round(Convert.ToDecimal(dt.Rows[m].ItemArray[n]), 3).ToString()));
                            }
                            else
                                csvMemoria.Append(String.Format("{0}|", dt.Rows[m].ItemArray[n].ToString().Trim()));
                        }
                    }
                }
                csvMemoria.AppendLine();
            }

            //Fecha en formato ddmmyyyy
            string NArchivo;
            NArchivo = DateTime.Today.Day.ToString().PadLeft(2, '0') + DateTime.Today.Month.ToString().PadLeft(2, '0') + DateTime.Today.Year.ToString();

            string NomredelTXT;
            NomredelTXT = NArchivo + ".txt";

            System.IO.StreamWriter sw =
                new System.IO.StreamWriter(@"C:\CORRECT\DatosKC" + NomredelTXT, false, System.Text.Encoding.Default);
            sw.Write(csvMemoria.ToString());
            sw.Close();

            //CARGAR AL FTP
            /* Create Object Instance */
            ftp ftpClient = new ftp(@"ftp://dt.kcmkt.com/", "9FRZ000001@dt9JAR.kcmkt.com", "Clave1");

            /* Upload a File */
            ftpClient.upload("/DatosKC" + NomredelTXT, @"C:\CORRECT\DatosKC" + NomredelTXT);
        
        }
    }
}
