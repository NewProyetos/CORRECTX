using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using WinSCP;
using System.Net.Mail;
using System.Net;

namespace Sinconizacion_EXactus
{
    public class GenerarArchivos
    {
        public SmtpClient smtp1 = new SmtpClient();
        public MailMessage email = new MailMessage();
        DataTable linea = new DataTable();
        DataTable Datos = new DataTable();
        conexionXML con = new conexionXML();


        public GenerarArchivos()
        {
            // int NroReporte = 0; // 1=Reporte de Ventas 2=Clientes 3=Inventario
            // Generar fecha actual menos 15 dias



            // DataTable dt ;
            // DataSet DS; 


            cargar_lineas_prod();


            if (linea.Rows.Count >= 1)
            {
                for (int t = 0; t < linea.Rows.Count; t++)  //Controla las columnas
                {

                    string Nombre_linea;
                    DataRow rowL = linea.Rows[t];
                    Nombre_linea = Convert.ToString(rowL["DESCRIPCION"]);



                    string NArchivo; //fecha de Archivo
                    string NomredelTXT = ""; //Nombre de Archivo
                    System.IO.StreamWriter sw; // Escritura de archivo
                    int x = 0;
                    StringBuilder csvMemoria;
                    //Calcular inicio de Mes los primeros 5 dias toma tambien los ultimos 5 dias del mes anterior
                    int diaini = 0;
                    int diafin = 0;
                    SqlDataAdapter da;

                    //GENERAR ARCHIVO 
                    for (int i = 1; i <= 4; i++)
                    {
                        // DS = new DataSet();
                        try
                        {
                            //REPORTE DE VENTAS

                            con.conectar("DM");
                            da = new SqlDataAdapter("CORRECT.KC_EXPORTA_VENTAV2", con.condm);

                            da.SelectCommand.CommandType = CommandType.StoredProcedure;
                            da.SelectCommand.Parameters.Add("@FINI", SqlDbType.DateTime);
                            da.SelectCommand.Parameters.Add("@FFIN", SqlDbType.DateTime);
                            da.SelectCommand.Parameters.Add("@REP", SqlDbType.Int);
                            da.SelectCommand.Parameters.Add("@EMPRESA", SqlDbType.VarChar);

                            diaini = -15;   //Resto 15 dias a Fecha actual

                            diafin = -1;  //Resto solo un dia a la Fecha Actual

                            if (DateTime.Today.Day == 1 && i == 4) // Si es el primer dia de cada mes Imprime el mensual como reporte 4
                            {
                                da.SelectCommand.Parameters["@FINI"].Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month - 1, 1);
                                da.SelectCommand.Parameters["@FFIN"].Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddDays(-1);
                                da.SelectCommand.Parameters["@REP"].Value = Convert.ToInt32("1");
                                da.SelectCommand.Parameters["@EMPRESA"].Value = Nombre_linea;
                            }
                            else                                 // Si no es el primer dia se imprime ultimos 15 dias de venta
                            {
                                da.SelectCommand.Parameters["@FINI"].Value = Convert.ToDateTime(DateTime.Today.AddDays(diaini));
                                da.SelectCommand.Parameters["@FFIN"].Value = Convert.ToDateTime(DateTime.Today.AddDays(diafin));
                                da.SelectCommand.Parameters["@REP"].Value = Convert.ToInt32(i);
                                da.SelectCommand.Parameters["@EMPRESA"].Value = Nombre_linea;
                            }

                            da.SelectCommand.CommandTimeout = 1200;

                            Datos.Clear();
                            if (Datos.Columns.Count > 1)
                            {
                                Datos.Columns.Clear();
                            }
                            da.Fill(Datos);  //Guardando datos en tabla

                            //  con.Desconectar("DM");

                        }
                        catch (Exception ex)
                        {
                            //  MessageBox.Show("No se Pudo conectar a la BD Error: " + ex.Message);
                        }

                        //GENERANDO ARCHIVO DE TEXTO

                        csvMemoria = new StringBuilder();

                        for (int m = 0; m < Datos.Rows.Count; m++)  //Controla las filas
                        {
                            x = Datos.Rows.Count;

                            for (int n = 0; n < Datos.Columns.Count; n++)  //Controla las columnas
                            {
                                //si es la última columna no poner el |
                                if (n == Datos.Columns.Count - 1)
                                {
                                    csvMemoria.Append(String.Format("{0}", Datos.Rows[m].ItemArray[n].ToString().Trim()));
                                }
                                else
                                {
                                    if (Datos.Rows[m].ItemArray[n].GetType() == Type.GetType("System.DateTime"))
                                    {
                                        csvMemoria.Append(String.Format("{0}|", Datos.Rows[m].ItemArray[n].ToString().Substring(0, 10)));
                                    }
                                    else
                                    {
                                        if (Datos.Rows[m].ItemArray[n].GetType() == Type.GetType("System.Decimal"))
                                        {
                                            if (Datos.Columns[n].Caption.Equals("LONGITUD") || Datos.Columns[n].Caption.Equals("LATITUD"))
                                                csvMemoria.Append(String.Format("{0}|", Datos.Rows[m].ItemArray[n].ToString().Trim()));
                                            else
                                                csvMemoria.Append(String.Format("{0}|", Math.Round(Convert.ToDecimal(Datos.Rows[m].ItemArray[n]), 3).ToString()));
                                        }
                                        else
                                            csvMemoria.Append(String.Format("{0}|", Datos.Rows[m].ItemArray[n].ToString().Trim()));
                                    }
                                }
                            }
                            csvMemoria.AppendLine();
                        }

                        //Fecha en formato ddmmyyyy

                        if (DateTime.Today.Day == 1)  //Si es primero de mes
                        {
                            if (DateTime.Today.Month == 1)  //Si es 1 de Enero coloca fecha del ultimo dia del año pasado
                                NArchivo = DateTime.Today.AddDays(-1).Day.ToString().PadLeft(2, '0') + DateTime.Today.AddDays(-1).Month.ToString().PadLeft(2, '0') + DateTime.Today.AddDays(-1).Year.ToString();
                            else  // Si uno de mes pero no es enero
                                NArchivo = DateTime.Today.AddDays(-1).Day.ToString().PadLeft(2, '0') + DateTime.Today.AddDays(-1).Month.ToString().PadLeft(2, '0') + DateTime.Today.Year.ToString();
                        }
                        else
                        {
                            NArchivo = DateTime.Today.AddDays(-1).Day.ToString().PadLeft(2, '0') + DateTime.Today.Month.ToString().PadLeft(2, '0') + DateTime.Today.Year.ToString();
                        }

                        if (Nombre_linea == "KIMBERLYPRO")
                        {
                            NomredelTXT = "P"+NArchivo + "_" + i.ToString() +".txt";
                        }
                        else
                        {
                            NomredelTXT = NArchivo + "_" + i.ToString() + ".txt";
                        }

                        sw = new System.IO.StreamWriter(@"C:\CORRECT\DatosKC" + NomredelTXT, false, System.Text.Encoding.Default);
                        sw.Write(csvMemoria.ToString());
                        sw.Close();
                        Datos.Clear();

                        if (DateTime.Today.Day > 1 && i == 3) //Si llego al 3 y no es dia primero sale del for para no generar el mensual
                            break;

                    } //fin del For

                    //CARGAR AL FTP
                    /* Create Object Instance */
                    // String ftpsitio= @"ftp://dt.kcmkt.com/";



                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("SELECT Empresa,Ftp,Usuario,Clave FROM DM.CORRECT.FTP_LOGIN WHERE EMPRESA='KIMBERLYSFTP'", con.condm);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    DataRow row = dt.Rows[0];

                    String Servidorftp = row["Ftp"].ToString();
                    String Usuario = row["Usuario"].ToString();
                    String Clave = row["Clave"].ToString();


                    // ftp ftpClient = new ftp(@"ftp://dt.kcmkt.com/", "9FRZ000001@dt9FRZ.kcmkt.com", "Clave1");

                    //ftp ftpClient = new ftp(@Servidorftp, Usuario, Clave);



                    for (int i = 1; i <= 4; i++) //Cargando Archivo por Archivo
                    {
                        if (DateTime.Today.Day == 1)
                        {
                            if (DateTime.Today.Month == 1)  //Si es 1 de Enero coloca fecha del ultimo dia del año pasado
                                NArchivo = DateTime.Today.AddDays(-1).Day.ToString().PadLeft(2, '0') + DateTime.Today.AddDays(-1).Month.ToString().PadLeft(2, '0') + DateTime.Today.AddDays(-1).Year.ToString();
                            else //Si es primero de mes
                                NArchivo = DateTime.Today.AddDays(-1).Day.ToString().PadLeft(2, '0') + DateTime.Today.AddDays(-1).Month.ToString().PadLeft(2, '0') + DateTime.Today.Year.ToString();
                        }
                        else
                        {
                            NArchivo = DateTime.Today.AddDays(-1).Day.ToString().PadLeft(2, '0') + DateTime.Today.Month.ToString().PadLeft(2, '0') + DateTime.Today.Year.ToString();
                        }

                        if (Nombre_linea == "KIMBERLYPRO")
                        {
                            NomredelTXT = "P"+NArchivo + "_" + i.ToString() + ".txt";
                        }
                        else

                        {
                            NomredelTXT = NArchivo + "_" + i.ToString() + ".txt";
                        }
                        /* Upload a File */
                        //  ftpClient.upload("/DatosKC" + NomredelTXT, @"C:\CORRECT\DatosKC" + NomredelTXT);

                     sftp(Servidorftp, Usuario, Clave, NomredelTXT);

                        if (DateTime.Today.Day > 1 && i == 3) //Si llego al 3 y no es dia primero sale del for para no generar el mensual
                            break;
                    }
                    con.Desconectar("DM");
                    

                    //  MessageBox.Show(row["Ftp"].ToString()+","+row["Usuario"].ToString()+","+row["Clave"].ToString()+"  Generacion Automatica Terminada");
                }
                Application.Exit();
            }
        }

        public void sftp( string host ,string user ,string sftpkey,string Nombrearc)
        {

            SessionOptions sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Sftp,
                HostName = host,
                UserName = user,
                Password = sftpkey,
                GiveUpSecurityAndAcceptAnySshHostKey = true
            };
            using (Session session = new Session())
            {
                // Connect
                try
                {
                    session.Open(sessionOptions);

                    SessionException sec;
                    

                    // Upload files
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Automatic;

                    transferOptions.OverwriteMode = OverwriteMode.Overwrite;
                    transferOptions.ResumeSupport.State = TransferResumeSupportState.Off;

                    

                    TransferOperationResult transferResult;


                    transferResult = session.PutFiles(@"C:\CORRECT\DatosKC"+Nombrearc+"", @"/", false, transferOptions);


                    foreach (TransferEventArgs transfer in transferResult.Transfers)

                    {
                        //MessageBox.Show("Upload of {0} succeeded", transfer.FileName);
                        // Console.WriteLine
                    }

                    //Throw on any error
                    // transferResult.Check();

                    //Print results
                    //foreach (TransferEventArgs transfer in transferResult.Transfers)
                    //{

                    //    Console.WriteLine("Upload of {0} succeeded", transfer.FileName);
                    //}


                    


                }
                catch
                {
                    //MessageBox.Show("Error en sftp");


                    correo();
                }
            }


        }
        private void correo()
        {
            email.From = new MailAddress("admindm@lamorazan.com");
            email.To.Add(new MailAddress("carlos_hercules@lamorazan.com"));
            email.CC.Add(new MailAddress("isaac_turcios@lamorazan.com"));
            email.CC.Add(new MailAddress("misael_santos@lamorazan.com"));
            email.CC.Add(new MailAddress("javier_estrada@lamorazan.com"));

            email.Subject = "ERROR EN  CARGA ARCHIVOS SFTP KIMBERLY";
            email.Body = "Las Interfaces  para SFTP KIMBERLY no se generaron  o no se Cargaron <br /> Fecha Modificacion: " + DateTime.Today.ToString() + "";

            email.IsBodyHtml = true;
            email.Priority = MailPriority.Normal;

            smtp1.Host = "smtpout.secureserver.net";
            smtp1.Port = 25;
            smtp1.EnableSsl = false;
            smtp1.UseDefaultCredentials = false;

            smtp1.Credentials = new NetworkCredential("admindm@lamorazan.com", "Ma1lAdw1uDM");

            smtp1.Send(email);
            email.Dispose();

        }
        private void cargar_lineas_prod()
        {
            linea.Clear();

            con.conectar("DM");
            
            SqlDataAdapter dac = new SqlDataAdapter("SELECT [DESCRIPCION] FROM [DM].[CORRECT].[PROVEEDOR_LINEA_FTP]  where PROVEEEDOR = 'KIMBERLY'", con.condm);
            //se indica el nombre de la tabla
            dac.Fill(linea);

            

        }
    }
}
