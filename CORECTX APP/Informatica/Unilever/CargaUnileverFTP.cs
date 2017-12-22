using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.IO;
using WinSCP;
using System.Net;
using System.Windows.Forms;

namespace Sinconizacion_EXactus.CORECTX_APP.Informatica.Unilever
{
    class CargaUnileverFTP
    {

        // DataSet DMSexport = new DataSet();
        conexionXML con = new conexionXML();
        DataTable ImportRegionUser = new DataTable();
        DataTable ImportStore = new DataTable();
        DataTable ImportVisit = new DataTable();
        int Proceso;
        DataTable ImportRegionUserror = new DataTable();
        DataTable ImportStorerror = new DataTable();

        DataTable sucursales = new DataTable();
        DataTable Areas = new DataTable();
        DataTable Vendedores = new DataTable();
        DataTable clientes = new DataTable();
        DataTable ImportRoute = new DataTable();
        DataTable ImportStock = new DataTable();
        DataTable ImportOrderInvoices = new DataTable();

        DataTable ImportRouterror = new DataTable();
        DataTable ImportStockerror = new DataTable();
        DataTable ImportOrderInvoiceserror = new DataTable();
        String HOST;

        String COD_BOD;
        String COD_RUTA;
        string Carpeta_local;
        string Carpeta_Historico;

        String USER;
        String SFTPKEY;
        String AUTOMATICO;
        String PUERTO;
        String TIPOFTP;
        String fecha;
        String fecha_facini;
        String fecha_facfin;
        DataTable ImportItemInvoiceTypes = new DataTable();
        DataTable config = new DataTable();
        public SmtpClient smtp1 = new SmtpClient();
        public MailMessage email = new MailMessage();

        public void InterfaceDMS()
        {

            config = XMLRW.Readxml("SFTP");

            DataRow rowtp = config.Rows[0];
            //HOST = Convert.ToString(rowtp["HOST"]);
            //USER = Convert.ToString(rowtp["USER"]);
            //SFTPKEY = Encripter.Desencriptar(Convert.ToString(rowtp["SFTPKEY"]));
            //textBox8.Text = Convert.ToString(rowtp["PUERTO"]);
            AUTOMATICO = Convert.ToString(rowtp["AUTOMATICO"]);


            if (AUTOMATICO == "YES")
            {
                load_data_empresa_suc(11);

                ImportRegionUser.Columns.Add("cdRegion", typeof(string));
                ImportRegionUser.Columns.Add("cdRegionType", typeof(string));
                ImportRegionUser.Columns.Add("dsRegion", typeof(string));
                ImportRegionUser.Columns.Add("cdRegionStatus", typeof(string));
                ImportRegionUser.Columns.Add("cdParentRegion", typeof(string));
                ImportRegionUser.Columns.Add("cdUser", typeof(string));
                ImportRegionUser.Columns.Add("nmFirstName", typeof(string));
                ImportRegionUser.Columns.Add("nmLastName", typeof(string));
                ImportRegionUser.Columns.Add("nrPhone1", typeof(string));
                ImportRegionUser.Columns.Add("nrPhone2", typeof(string));
                ImportRegionUser.Columns.Add("Email", typeof(string));
                ImportRegionUser.Columns.Add("cdUserStatus", typeof(string));

                ImportStore.Columns.Add("cdStore", typeof(string));
                ImportStore.Columns.Add("cdStatus", typeof(string));
                ImportStore.Columns.Add("cdStoreBrand", typeof(string));
                ImportStore.Columns.Add("dsName", typeof(string));
                ImportStore.Columns.Add("dsCorporateName", typeof(string));
                ImportStore.Columns.Add("cdClass1", typeof(string));
                ImportStore.Columns.Add("cdClass2", typeof(string));
                ImportStore.Columns.Add("cdCity", typeof(string));
                ImportStore.Columns.Add("cdRegion", typeof(string));
                ImportStore.Columns.Add("nmAdress", typeof(string));
                ImportStore.Columns.Add("nrAdress", typeof(string));
                ImportStore.Columns.Add("nmAddresComplement", typeof(string));
                ImportStore.Columns.Add("nrZipCode", typeof(string));
                ImportStore.Columns.Add("nmNeighborthood", typeof(string));

                ImportStorerror.Columns.Add("cdStore", typeof(string));
                ImportStorerror.Columns.Add("cderror", typeof(string));

                Proceso = 1;

                procesamiento();
            }
            else //Si el Correct.XML AUTOMATICO esta configurado como NO (osea que Manual) carga el formulario
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new CORECTX_APP.Informatica.Unilever.InterfaceDMS());
            }
            
        }

        //

        public void procesamiento()
        {
            for (int i = 0; i < sucursales.Rows.Count; i++)
            {
                DataRow row = sucursales.Rows[i];
                HOST = Convert.ToString(row["ftp"]);
                USER = Convert.ToString(row["Usuario"]);
                SFTPKEY = Convert.ToString(row["Clave"]);
                COD_BOD = Convert.ToString(row["COD_BOD"]);
                COD_RUTA = Convert.ToString(row["COD_RUTA"]);
                Carpeta_local = Convert.ToString(row["CARPETA_LOCAL"]);
                Carpeta_Historico = Convert.ToString(row["CARPETA_HISTORICO"]);
                PUERTO = Convert.ToString(row["COD_RUTA"]);
                TIPOFTP = Convert.ToString(row["tipo"]);

                int diaini = 0;
                //Determinando cuantos dias le vamos a restar a la fecha actual para determinar la fecha de inicio de la carga
               
                if ( DateTime.Today.Day <= 7)
                {
                    diaini = (DateTime.Today.Day + 7) * -1;
                }
                else
                {
                    if (DateTime.Today.Day <= 10 || DateTime.Today.Day == 28)
                    {

                        diaini = (DateTime.Today.Day - 1) * -1;
                    }
                    else
                    {
                        diaini = -10;
                    }

                }

                fecha = DateTime.Now.ToString("yyyyMMddhhmmss");
                ////fecha_fac = DateTime.Now.ToString("yyyy/MM/dd");
               
               fecha_facini = DateTime.Today.AddDays(diaini).ToString("yyyy/MM/dd");
                   //fecha_facfin = DateTime.Now.ToString("yyyy/MM/dd");
               fecha_facfin = DateTime.Today.AddDays(-1).ToString("yyyy/MM/dd");       

                Vendedores.Clear();
                clientes.Clear();
    
                //  ImportRegionUserWorker.RunWorkerAsync();
                ImportRegionUserW();
             //   ImportVisitW();
              
            }
        }

   
        private void ImportRegionUserW()
        {

            ImportRegionUser.Clear();
            Areas.Clear();
            Vendedores.Clear();
            con.conectar("EX");

            SqlCommand cm1 = new SqlCommand("SELECT [E_MAIL] as AREA, 'ACT' as ACTIVO  FROM [EXACTUS].[DISMOGT].[VENDEDOR]  WHERE E_MAIL is not null and LEFT(VENDEDOR,2) like '" + COD_RUTA + "'   GROUP BY E_MAIL", con.conex);
            SqlDataAdapter da1 = new SqlDataAdapter(cm1);
            cm1.CommandTimeout = 1200;
            da1.Fill(Areas);

            con.Desconectar("EX");


            if (Areas.Rows.Count >= 1)
            {
                for (int i = 0; i < Areas.Rows.Count; i++)
                {
                    string AREA;
                    string A_ACTIVA;
                    DataRow row = Areas.Rows[i];
                    AREA = Convert.ToString(row["AREA"]);
                    A_ACTIVA = Convert.ToString(row["ACTIVO"]);
                    int conutn = ImportRegionUser.Columns.Count;
                    ImportRegionUser.Rows.Add(AREA, "TERRI", AREA, A_ACTIVA, "", "", "", "", "", "", "", "");
                    con.conectar("EX");

                    Vendedores.Clear();
                    SqlCommand cm2 = new SqlCommand("SELECT VEN.VENDEDOR as cdRegion,'ZONE' as  cdRegionType,VEN.E_MAIL+'-'+VEN.VENDEDOR as dsRegion,CASE VEN.ACTIVO WHEN  'S' THEN 'ACT'  ELSE 'DEACT' END as cdRegionStatus,VEN.E_MAIL as cdParentRegion, VEN.VENDEDOR as cdUser, LEFT(VEN.nombre, ISNULL(NULLIF(CHARINDEX(' ', VEN.nombre) - 1, -1), LEN(VEN.nombre))) as nmFirstName, SUBSTRING(VEN.nombre, CHARINDEX(' ', VEN.nombre) + 1, LEN(VEN.nombre)) as nmLastName, VEN.[E_MAIL] as cdParentRegion, '22017700' as nrPhone1, '' as nrPhone2, 'sac@lamorazan.com' as Email, CASE VEN.ACTIVO WHEN  'S' THEN 'ACT'  ELSE 'DEACT' END as cdUserStatus FROM [EXACTUS].[DISMOGT].[VENDEDOR] VEN WHERE  VEN.E_MAIL = '" + AREA + "'", con.conex);
                    SqlDataAdapter da = new SqlDataAdapter(cm2);
                    da.Fill(Vendedores);

                    con.Desconectar("EX");
                    if (Vendedores.Rows.Count >= 1)
                    {
                        for (int j = 0; j < Vendedores.Rows.Count; j++)
                        {

                            string cdRegion;
                            string cdRegionType;
                            string dsRegion;
                            string cdRegionStatus;
                            string cdParentRegion;
                            string cdUser;
                            string nmFirstName;
                            string nmLastName;
                            string nrPhone2;
                            string nrPhone1;
                            string Email;
                            string cdUserStatus;

                            DataRow rows = Vendedores.Rows[j];
                            cdRegion = Convert.ToString(rows["cdRegion"]);
                            cdRegionType = Convert.ToString(rows["cdRegionType"]);
                            dsRegion = Convert.ToString(rows["dsRegion"]);
                            cdRegionStatus = Convert.ToString(rows["cdRegionStatus"]);
                            cdParentRegion = Convert.ToString(rows["cdParentRegion"]);
                            cdUser = Convert.ToString(rows["cdUser"]);
                            nmFirstName = Convert.ToString(rows["nmFirstName"]);
                            nmLastName = Convert.ToString(rows["nmLastName"]);
                            nrPhone2 = Convert.ToString(rows["nrPhone2"]);
                            nrPhone1 = Convert.ToString(rows["nrPhone1"]);
                            Email = Convert.ToString(rows["Email"]);
                            cdUserStatus = Convert.ToString(rows["cdUserStatus"]);

                            ImportRegionUser.Rows.Add(cdRegion, cdRegionType, dsRegion, cdRegionStatus, cdParentRegion, cdUser, nmFirstName.ToUpper(), nmLastName.ToUpper(), nrPhone1, nrPhone2, Email, cdUserStatus);
                        }
                    }
                }
            }

            if (ImportRegionUser.Rows.Count > 1)
            {
                To_txt(ImportRegionUser, "ImportRegionUser", 1);
                //   checkBox2.Checked = false;
            }
            if (Proceso == 1)
            {
                ImportStoreW();
            }
          
        }

       
        private void ImportStoreW()
        {
            string[] direccion;
            string direccionlong;


            string cdStore;
            string dsCorporateName;
            string dsName;
            string cdStoreBrand;
            string cdCity;
            string cdRegion;
            string nmAddress;
            string nrAddress;
            string nmAddresComplement;
            string cdStatus;
            string cdClass1;
            string cdClass2;
            string nmNeighborhood;
            string nrZipCode;

            ImportStore.Clear();
            clientes.Clear();
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("[DISMOGT].[ImportStoreV2]", con.conex);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 1200;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            cmd.Parameters.AddWithValue("@fechaini", fecha_facini);
            cmd.Parameters.AddWithValue("@fechafin", fecha_facfin);
            cmd.Parameters.AddWithValue("@Ruta", COD_RUTA);
            da.Fill(clientes);
            con.Desconectar("EX");

            for (int i = 0; i < clientes.Rows.Count; i++)
            {
                DataRow rows = clientes.Rows[i];

                direccionlong = Convert.ToString(rows["nmAddress"]);
                direccion = direccionlong.Split(',');

                if (direccionlong.Contains(','))
                {
                    nmAddresComplement = direccion[1];
                    nmAddress = direccion[0];

                    if (nmAddress.Length > 50)
                    {
                        nmAddress = nmAddress.Substring(0, 45);
                    }
                    if (nmAddresComplement.Length > 50)
                    {
                        nmAddresComplement = nmAddresComplement.Substring(0, 45);
                    }

                }
                else
                {
                    if (direccionlong.Length > 50)
                    {
                        nmAddress = direccionlong.Substring(0, 45);
                        nmAddresComplement = "";
                    }
                    else
                    {
                        nmAddress = direccionlong;
                        nmAddresComplement = "";
                    }
                }


                cdStore = Convert.ToString(rows["cdStore"]);
                dsCorporateName = Convert.ToString(rows["dsCorporateName"]);

                if (dsCorporateName.Length > 50)
                {
                    dsCorporateName = dsCorporateName.Substring(0, 49);
                }

                dsName = Convert.ToString(rows["dsName"]);

                if (dsName.Length > 50)
                {
                    dsName = dsName.Substring(0, 49);
                }
                cdStoreBrand = Convert.ToString(rows["cdStoreBrand"]);
                cdCity = Convert.ToString(rows["cdCity"]);
                cdRegion = Convert.ToString(rows["cdRegion"]);
                nrAddress = Convert.ToString(rows["nrAddress"]);
                cdStatus = Convert.ToString(rows["cdStatus"]);
                cdClass1 = Convert.ToString(rows["cdClass1"]);
                cdClass2 = Convert.ToString(rows["cdClass2"]);
                nmNeighborhood = Convert.ToString(rows["nmNeighborhood"]);
                nrZipCode = Convert.ToString(rows["nrZipCode"]);

                if (cdCity == string.Empty)
                {
                    ImportStorerror.Rows.Add(cdStore, "NO SE ENCONTRO CIUDAD");
                }

                else if (cdRegion == string.Empty)
                {
                    ImportStorerror.Rows.Add(cdStore, "NO SE ENCONTRO region");
                }
                // if(cdClass1 == string.Empty &&  )

                else if (cdClass1 == string.Empty)
                {
                    ImportStorerror.Rows.Add(cdStore, "NO SE ENCONTRO CANAL");
                }

                else if (cdClass2 == string.Empty)
                {
                    ImportStorerror.Rows.Add(cdStore, "NO SE ENCONTRO SUBCANAL");
                }
                else
                {
                    ImportStore.Rows.Add(cdStore, cdStatus, cdStoreBrand, caracteres(dsName), caracteres(dsCorporateName), cdClass1, cdClass2, cdCity, cdRegion, caracteres(nmAddress), nrAddress, caracteres(nmAddresComplement), nrZipCode, nmNeighborhood);
                }
            }

            if (ImportStore.Rows.Count > 1)
            {
                To_txt(ImportStore, "ImportStore", 1);
                //  checkBox3.Checked = false;
            }

            if (ImportStorerror.Rows.Count >= 1)
            {
                To_txt(ImportStorerror, "ImportStoreERROR", 0);
            }

            if (Proceso == 1)
            {
                ImportRouteW();
            }
           
        }

      
        private void ImportRouteW()
        {
            ImportRoute.Clear();
            con.conectar("EX");
            SqlCommand cm2 = new SqlCommand("SELECT VEN.VENDEDOR as cdRoute,VEN.NOMBRE as nmRoute,CASE VEN.ACTIVO WHEN  'S' THEN 'ACT'  ELSE 'DEACT' END as cdStatus FROM [EXACTUS].[DISMOGT].[VENDEDOR] VEN WHERE  E_MAIL is not null and LEFT(VEN.VENDEDOR,2) = '" + COD_RUTA + "'", con.conex);
            cm2.CommandTimeout = 120;
            SqlDataAdapter da = new SqlDataAdapter(cm2);
            da.Fill(ImportRoute);
            con.Desconectar("EX");


            if (ImportRoute.Rows.Count > 1)
            {
                To_txt(ImportRoute, "ImportRoute", 1);
                //   checkBox4.Checked = false;
            }
            if (Proceso == 1)
            {
                ImportStockW();
            }

        }

     
        private void ImportStockW()
        {
            ImportStock.Clear();
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("[DISMOGT].[ImportStockV2]", con.conex);
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fechai", fecha_facini);
            cmd.Parameters.AddWithValue("@fechaf", fecha_facfin);
            cmd.Parameters.AddWithValue("@BODEGA", COD_BOD);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ImportStock);
            con.Desconectar("EX");

            if (ImportStock.Rows.Count > 1)
            {
                To_txt(ImportStock, "ImportStock", 1);
                //   checkBox5.Checked = false;
            }
            if (Proceso == 1)
            {
                ImportOrderInvoiceW();
            }

        }

        private void ImportOrderInvoiceW()
        {

            ImportOrderInvoices.Clear();
            con.conectar("EX");
            SqlCommand cmd1 = new SqlCommand("[DISMOGT].[ImportOrderInvoiceV2]", con.conex);
            cmd1.CommandTimeout = 0;
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@fechaini", fecha_facini);
            cmd1.Parameters.AddWithValue("@fechafin", fecha_facfin);
            cmd1.Parameters.AddWithValue("@Ruta", COD_RUTA);

            SqlDataAdapter daim = new SqlDataAdapter(cmd1);
            daim.Fill(ImportOrderInvoices);
            con.Desconectar("EX");

            if (ImportOrderInvoices.Rows.Count > 1)
            {
                To_txt(ImportOrderInvoices, "ImportOrderInvoice", 1);
                //   checkBox6.Checked = false;
            }
            if (Proceso == 1)
            {
                ImportItemInvoiceTypeW();
            }
         
        }

       
        private void ImportItemInvoiceTypeW()
        {
            ImportItemInvoiceTypes.Clear();
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("[DISMOGT].[ImportItemInvoiceTypeV2]", con.conex);
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fechaini", fecha_facini);
            cmd.Parameters.AddWithValue("@fechafin", fecha_facfin);
            cmd.Parameters.AddWithValue("@Ruta", COD_RUTA);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ImportItemInvoiceTypes);
            con.Desconectar("EX");

            if (ImportItemInvoiceTypes.Rows.Count > 1)
            {
                To_txt(ImportItemInvoiceTypes, "ImportOrderItemInvoiceItem", 1);
                //   checkBox7.Checked = false;

            }
            if (Proceso == 1)
            {
                ImportVisitW();
            }
        }

        private void ImportVisitW()
        {
            ImportVisit.Clear();
            con.conectar("EX");
            SqlCommand cmdv = new SqlCommand("[DISMOGT].[DMS_ImportVisitV2]", con.conex);
            cmdv.CommandTimeout = 0;
            cmdv.CommandType = CommandType.StoredProcedure;
            cmdv.Parameters.AddWithValue("@fechaini", fecha_facini);
            cmdv.Parameters.AddWithValue("@fechafin", fecha_facfin);
            cmdv.Parameters.AddWithValue("@COMPANIA", "DISMOGT");
            cmdv.Parameters.AddWithValue("@RUTA", COD_RUTA);

            SqlDataAdapter dav = new SqlDataAdapter(cmdv);
            dav.Fill(ImportVisit);
            con.Desconectar("EX");

            if (ImportVisit.Rows.Count > 1)
            {
                To_txt(ImportVisit, "ImportVisit", 1);
                //   checkBox8.Checked = false;
            }

        }


        private string caracteres(string text)
        {
            var inputString = text;
            var normalizedString = inputString.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            for (int i = 0; i < normalizedString.Length; i++)
            {
                var uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(normalizedString[i]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(normalizedString[i]);
                }
            }
            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }


        private void carga_FTP(string interfaces)
        {

            // string fecha = Convert.ToString(dateTimePicker1.Value.ToString("ddMMyyyy"));

            string user = USER;
            string pass = SFTPKEY;
            //  string direccion = "ftp://190.143.71.122/Inbox/";
            string direccion = HOST + "/Inbox";

            ftp ftpClient = new ftp(HOST + ":" + PUERTO, user, pass);

            FtpWebRequest arh = (FtpWebRequest)FtpWebRequest.Create(new Uri(direccion + interfaces));

            arh.Method = WebRequestMethods.Ftp.UploadFile;
            arh.Proxy = null;
            arh.Credentials = new NetworkCredential(user, pass);
            arh.UsePassive = true;
            arh.KeepAlive = true;

            FileStream fs = File.OpenRead(Carpeta_local);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();

            Stream ftpstream = arh.GetRequestStream();
            ftpstream.Write(buffer, 0, buffer.Length);
            ftpstream.Close();

        }

        public void To_txt(DataTable dt, string interfaces, int estado)
        {
            string Direccion;
            if (estado == 1)
            {
                Direccion = Carpeta_local;
            }
            else
            {
                Direccion = @"C:\CORRECT\Unilever\DMS\ERROR\";
            }

            var result = new StringBuilder();


            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {

                    result.Append(row[i].ToString());
                    result.Append(i == dt.Columns.Count - 1 ? "" : "\t");
                }
                result.AppendLine();
            }

            string fullpath = Direccion + interfaces + "_" + fecha + ".txt";

            StreamWriter objWriter = new StreamWriter(fullpath, true);
            objWriter.WriteLine(result.ToString());
            objWriter.Close();

            if (Proceso == 1)
            {
                if (interfaces == "ImportVisit")
                {

                    if (TIPOFTP.Trim() == "SFTP")
                    {
                        sftp();
                    }
                    else if (TIPOFTP == "FTP")
                    {
                        carga_FTP(interfaces + "_" + fecha + ".txt");
                    }

                    // carga_FTP(fullpath, interfaces + "_" + fecha + ".txt");
                }
            }   
        }


        public void sftp()
        {

            SessionOptions sessionOptions = new SessionOptions
            {

                Protocol = Protocol.Sftp,
                HostName = HOST,
                UserName = USER,
                Password = SFTPKEY,
                GiveUpSecurityAndAcceptAnySshHostKey = true
            };
            using (Session session = new Session())
            {
                // Connect
                try
                {
                    session.Open(sessionOptions);

                    // Upload files
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;

                    TransferOperationResult transferResult;
                    transferResult = session.PutFiles(Carpeta_local + "*", "/Import/Inbox/", false, transferOptions);

                    //Throw on any error
                    transferResult.Check();

                    //Print results

                    string Direccion;
                    
                    Direccion = @"C:\CORRECT\Unilever\";
                    var result = new StringBuilder();

                    foreach (TransferEventArgs transfer in transferResult.Transfers)
                    {
                            result.Append("Upload of " + transfer.FileName.ToString()+ " succeeded ");
                        //  result.Append(i == dt.Columns.Count - 1 ? "" : "\t");
                       
                        result.AppendLine();
                    }

                    string fullpath = Direccion +"RegistroFTP" + "_" + fecha + ".txt";

                    StreamWriter objWriter = new StreamWriter(fullpath, true);
                    objWriter.WriteLine(result.ToString());
                    objWriter.Close();

                    ///////////

            /*       foreach (TransferEventArgs transfer in transferResult.Transfers)
                    {

                        Console.WriteLine("Upload of {0} succeeded", transfer.FileName);
                    }
*/
                    mover();

                    //           this.Close();

                    session.Close();

                }
                catch (Exception e)
                {
                  //  MessageBox.Show(Convert.ToString(e));
                    eliminar();
                    correo();
                 //   this.Close();
                }
            }


        }

        private void mover()
        {
            string sourcePath = @"C:\CORRECT\Unilever\DMS\Import\";
            string targetPath = @"C:\CORRECT\Unilever\DMS\Last\";
            //if (!Directory.Exists(targetPath))
            //{
            //    Directory.CreateDirectory(targetPath);
            //}
            foreach (var srcPath in Directory.GetFiles(Carpeta_local))
            {
                File.Copy(srcPath, srcPath.Replace(Carpeta_local, Carpeta_Historico), true);
                File.Delete(srcPath);

            }

        }
        private void eliminar()
        {
            string sourcePath = @"C:\CORRECT\Unilever\DMS\Import\";

            foreach (var srcPath in Directory.GetFiles(Carpeta_local))
            {

                File.Delete(srcPath);
            }

        }
        private void correo()
        {
            email.From = new MailAddress("admindm@lamorazan.com");
            email.To.Add(new MailAddress("carlos_hercules@lamorazan.com"));
            email.CC.Add(new MailAddress("isaac_turcios@lamorazan.com"));
            email.CC.Add(new MailAddress("misael_santos@lamorazan.com"));
            email.CC.Add(new MailAddress("javier_estrada@lamorazan.com"));

            email.Subject = "ERROR EN  CARGA ARCHIVOS DMS UNILEVER";
            email.Body = "Las Interfaces  para DMS UNILEVER no se generaron  o no se Cargaron <br /> Fecha Modificacion: " + fecha + "";

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


        private void load_data_empresa_suc(int empresa)
        {
            sucursales.Clear();
            con.conectar("DM");

            SqlCommand cm2 = new SqlCommand("SELECT SUC.[ID_SUCURSAL],SUC.[EMPRESA_EXACTUS],SUC.[SUCURSAL],SUC.[COD_BOD],SUC.[COD_RUTA],SUC.[SFTP_ID],FTP.Ftp,FTP.Usuario,FTP.Clave,FTP.Puerto,FTP.Tipo,SUC.[CARPETA_LOCAL],[CARPETA_HISTORICO]  FROM [DM].[CORRECT].[SUCURSALES_EXATUS] as SUC  LEFT JOIN [DM].[CORRECT].[FTP_LOGIN] as FTP  on SUC.SFTP_ID = FTP.id where SUC.EMPRESA_EXACTUS = '" + empresa + "' and FTP.Estado = 'A' and FTP.Proveedor = 'UNILEVER'", con.condm);
            SqlDataAdapter da = new SqlDataAdapter(cm2);
            da.Fill(sucursales);

            con.Desconectar("DM");

        }

    }
}
