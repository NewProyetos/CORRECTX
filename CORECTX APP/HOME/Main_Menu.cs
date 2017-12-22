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
    public partial class Main_Menu : DevExpress.XtraEditors.XtraForm
    {
        public Main_Menu()
        {
            InitializeComponent();
            DevExpress.Skins.SkinManager.EnableFormSkins();
        }
        public static string  Departamento;
        conexionXML con = new conexionXML();
       // Conexion2 coned = new Conexion2();
        //conexion conex = new conexion();
       public static DateTime fechaup;
        Validar_Acceso vacc = new Validar_Acceso();
        String menu_id;
        String APP_id;
        String SUBAPP_id;
        public static String Agencia;
        public static String multisucursal;
        public static String Puesto;
        public static String Principal_ID;
        public static String USERWEB;
        public static Int32 Descuentos_Bonidicaciones_acceso;
        public static Int32 Carga_ERP_FR;
        public static String COD_EMPLEADO;
        public static String TIPO_RRHH;
        public static String EMPRESA;
        public static Int32 logo;
        public static String autoev;
        public static String usuario_devol;
        public DataTable DTpedidos = new DataTable();
        DataTable config = new DataTable();
        private void Form6_Load(object sender, EventArgs e)
        {
            // Validamos si se esta ejecutando un proceso CARGA DE FACTURAS AL FR ------------------------------
            validar(null, null);
            //---------------------------------------------------

            //----- Carga los Accesos del Usuario ------------------------------------------------------------
            vacc.Valida_accion(Login.usuario.ToUpper(),Login.empresa_id);
            //---------------------------------------------------------------------------------------------------

            Descuentos_Bonidicaciones_acceso = 0;
            
            Carga_ERP_FR = 0;
            
            MaximumSize = new Size(945, 131);
             ControlBox = false;

            //------------ obtenemos la vercion actual del Assembly -------------------------------
             var ensablado = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            
             this.Text = "CORRECTX    (" + Login.usuario.ToUpper() +"/"+ Login.empresa +" )      " + ensablado+ " ";
            
            //---serramos el formulario loguin -------------------------------------------------------
            Login fm5 = new Login();
             fm5.Close();

            //--------- Desabilitamos todos los  botones antes de validar accesos -------------------------
             desabilitar();
            //--------------------------------------------------------------------------------------------
            
            // -------------------------------OBTERNER INFORMACION DE USUARIO -----------------------

             con.conectar("DM");

             SqlCommand cm2 = new SqlCommand("SELECT [DEPARTAMENTO],[AGENCIA],[PUESTO],[Principal_ID],[USERWEB],[COD_EMPLEADO],[TIPO_RRHH],[EMPRESA],[LOGO_EMP],[AUTOEV] FROM [DM].[CORRECT].[USUARIOS] where USUARIO = '" + Login.usuario.ToUpper() + "'", con.condm);
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
                Departamento = Convert.ToString(dr2["DEPARTAMENTO"]);
                Agencia = Convert.ToString(dr2["AGENCIA"]);
                Puesto = Convert.ToString(dr2["PUESTO"]);
                Principal_ID = Convert.ToString(dr2["Principal_ID"]).Trim();
                USERWEB = Convert.ToString(dr2["USERWEB"]);
                COD_EMPLEADO = Convert.ToString(dr2["COD_EMPLEADO"]);
                TIPO_RRHH = Convert.ToString(dr2["TIPO_RRHH"]);
                EMPRESA = Convert.ToString(dr2["EMPRESA"]);
                logo = Convert.ToInt32(dr2["LOGO_EMP"]);
               
            }
            dr2.Close();




            con.Desconectar("DM");
            //----------------------------------------------------------------------------------------------
                





            
            // --------valida acceso a menus----------------------------------------------------------------
            int can_menu = vacc.Menu_Acces.Rows.Count;
            if (can_menu <= 0)
            {
                MaximumSize = new Size(200, 121);
            }
            else
            {
                for (int i = 0; i < vacc.Menu_Acces.Rows.Count; i++)
                {
                    menu_id = vacc.Menu_Acces.Rows[i]["MENU_ID"].ToString();
                    
                    int tamaño = vacc.Menu_Acces.Rows.Count;

     // modifica el tamaño segun  la cantidad de aplicaciones  habilitadas  -------------------------------

                    switch (tamaño)
                    {
                        case 1:
                            MaximumSize = new Size(290, 130);
                            break;

                        case 2:
                            MaximumSize = new Size(370, 130);
                            break;
                        case 3:
                            MaximumSize = new Size(455, 130);
                            break;
                        case 4:
                            MaximumSize = new Size(550, 130);
                            break;
                        case 5:
                            MaximumSize = new Size(664, 130);
                            break;
                        case 6:
                            MaximumSize = new Size(742, 130);
                            break;
                        case 7:
                            MaximumSize = new Size(841, 133);
                            break;
                        case 8:
                            MaximumSize = new Size(945, 131);
                            break;
                    }

//---------------- se muestra el menu segun acceso ------------------------------------
 
                    switch (menu_id)
                    {
                        case "1": // Informatica
                            toolStripDropDownButton1.Visible = true;
                            break;
                        case "2": // Ventas
                            toolStripDropDownButton4.Visible = true;
                            break;
                        case "3":// Creditos
                            toolStripDropDownButton2.Visible = true;
                            break;
                        case "4": //Bodega
                            toolStripDropDownButton3.Visible = true;
                            break;
                        case "5": //Flota
                            toolStripDropDownButton5.Visible = true;
                            break;
                        case "6": //RRHH
                            toolStripButton3.Visible = true;
                            break;
                        case "7": //COMPRAS
                            toolStripButton6.Visible = true;
                            break;
                        case "8": //SEGURIDAD
                            toolStripButton7.Visible = true;
                            break;






                        default:
                            desabilitar();
                            break;


                    }



                }

// ---------------- se habilitan APP  segun acceso ------------------------------------------
                for (int i = 0; i < vacc.APP_acces.Rows.Count; i++)
                {
                    APP_id = vacc.APP_acces.Rows[i]["APP_ID"].ToString();
                   
                    switch (APP_id)
                    {
                        case "1":
                            sincronizacionToolStripMenuItem.Visible = true;
                            break;
                        case "2":
                            sISCMToolStripMenuItem.Visible = true;
                            break;
                        case "3":
                            casosToolStripMenuItem.Visible = true;
                            break;
                        case "4":
                            correlativoToolStripMenuItem.Visible = true;
                            break;
                        case "5":
                            oCToolStripMenuItem.Visible = true;
                            break;
                        case "6":
                            clienteUpdateFRToolStripMenuItem.Visible = true;
                            break;
                        case "7":
                            updateClientePedidoToolStripMenuItem.Visible = true;
                            break;
                        case "8":
                            excelToKMLToolStripMenuItem.Visible = true;
                            break;
                        case "9":
                            kimberlyToolStripMenuItem.Visible = true;
                            break;
                        case "10":
                            accesoToolStripMenuItem.Visible = true;
                            break;
                        case "11":
                            gPSRutasToolStripMenuItem.Visible = true;
                            break;
                        case "12":
                            casosToolStripMenuItem.Visible = true;
                            break;
                        case "13":
                            ruterosToolStripMenuItem.Visible = true;
                            break;
                        case "14":
                            reporteCarteraToolStripMenuItem.Visible = true;
                            break;
                        case "15":
                            cargadorPagosDeContadoToolStripMenuItem.Visible = true;
                            break;
                        case "16":
                            facturacionToolStripMenuItem.Visible = true;
                            break;
                        case "17":
                            regaliasToolStripMenuItem.Visible = true;
                            break;
                        case "18":
                            cargasToolStripMenuItem.Visible = true;
                            break;
                        case "19":
                            vehiculosToolStripMenuItem.Visible = true;
                            break;
                        case "20":
                            asignacionesToolStripMenuItem.Visible = true;
                            break;
                        case "21":
                            mantenimientosToolStripMenuItem.Visible = true;
                            break;
                        case "22":
                            kilometrajesToolStripMenuItem.Visible = true;
                            break;
                        case "23":
                            combustiblesToolStripMenuItem.Visible = true;
                            break;
                        case "24":
                            reportesToolStripMenuItem.Visible = true;
                            break;
                        case "25":
                            correlativosToolStripMenuItem.Visible = true;
                            break;

                        case "26":
                            limitesDeCreditoToolStripMenuItem.Visible = true;
                            break;

                        case "27":
                            reportesBodegaToolStripMenuItem1.Visible = true;
                            break;
                        case "28":
                            reportesDismoappToolStripMenuItem.Visible = true;
                            break;

                        case "29":
                           herramientasToolStripMenuItem.Visible = true;                            
                            break;

                        case "30":
                            competenciaToolStripMenuItem.Visible = true;
                            break;
                        case "31":
                            importadorStreetToolStripMenuItem.Visible= true;
                            break;
                        case "32":
                            empresasToolStripMenuItem.Visible = true;
                            break;
                        case "33":

                            reportesToolStripMenuItem1.Visible = true;
                            break;

                        case "34":
                           reporteAccesosToolStripMenuItem.Visible = true;
                            break;
                        case "35":
                            autoev = "S";
                            break;
                        case "36":
                            impuestoDistribucionToolStripMenuItem.Visible = true;
                            break;
                        case "37":
                            festivosDISMOToolStripMenuItem.Visible = true;
                            break;
                        case "38":
                            devolucionRepartoToolStripMenuItem.Visible = true;
                            break;
                        case "39":
                            usuario_devol = "S";
                            break;

                        case "44":
                            merchandisingToolStripMenuItem.Visible = true;
                            break;
                        case "48":
                            visitasComentariosToolStripMenuItem.Visible = true;
                            break;
                        case "49":
                            promocionesToolStripMenuItem.Visible = true;
                            break;
                        case "50":
                            facturacionToolStripMenuItem1.Visible = true;
                            break;

                        case "51":
                            regaliasVentasToolStripMenuItem.Visible = true;
                        break;
                        case "52":

                            regaliasToolStripMenuItem.Visible = true;
                            break;
                        case "53":
                            preciosToolStripMenuItem.Visible = true;                            
                            break;

                        case "54":                  
                            
                            cargaToolStripMenuItem.Visible = true;
                            break;

                        case "55":

                            controlFlotaToolStripMenuItem.Visible = true;
                            break;

                        case "59":

                            edicionDatosEmpleadosMarcacionToolStripMenuItem.Visible = true;
                            break;
                        case "60":

                            procesosExactusToolStripMenuItem.Visible = true;
                            break;
                        case "61":

                            multisucursal = "S";
                            break;
                    }




                }
            }
            // ------ Validamos accesos a Sub App --------------------------------------
              for (int i = 0; i < vacc.SUBAPP_acces.Rows.Count; i++)
              {
                  SUBAPP_id = vacc.SUBAPP_acces.Rows[i]["SUBAPP_ID"].ToString();

                  switch (SUBAPP_id)
                  {
                      case "1":
                          preimpresoToolStripMenuItem.Visible = true;
                          break;
                      case "2":
                          procesarOcToolStripMenuItem.Visible = true;
                          break;
                      case "3":
                          exportadorVentaKCToolStripMenuItem.Visible = true;
                          break;
                      case "4":
                          reporteCarteraToolStripMenuItem1.Visible = true;
                          break;
                      case "5":
                          reporteEfectivoVentaToolStripMenuItem1.Visible = true;
                          break;
                      case "6":
                          updateDireccionesToolStripMenuItem.Visible = true;
                          break;
                      case "12":
                          Descuentos_Bonidicaciones_acceso = 1;
                          break;
                      case"13":
                          Carga_ERP_FR = 1;
                          break;
                      case "14":
                          pedidosFacturacionToolStripMenuItem.Visible = true;
                          break;
                      case "15":
                          saldosToolStripMenuItem.Visible = true;
                          break;
                      case "16":
                          rutasToolStripMenuItem.Visible = true;
                          break;
                      case "17":
                          cargaCostosToolStripMenuItem.Visible = true;
                          break;

                            case "18":
                          evaluacionesToolStripMenuItem.Visible = true;
                          break;

                            case "19":
                          diasInventarioToolStripMenuItem.Visible = true;
                          break;

                      case "20":
                          marcacionToolStripMenuItem.Visible = true;
                          break;
                    case "25":
                        libroInventariosToolStripMenuItem.Visible = true;
                        break;

                    case "26":
                        promocionesPorPedidoToolStripMenuItem.Visible = true;
                        break;

                    case "27":
                        facturacionToolStripMenuItem.Visible = true;
                        break;
                    case "28":
                        reporteDescuentosToolStripMenuItem.Visible = true;
                        break;
                    case "29":
                        fillrateToolStripMenuItem.Visible = true;
                        break;
                    case "30":
                        reporteDescuentosXDocumentoToolStripMenuItem.Visible = true;
                        break;
                    case "31":
                        reporteTransaccionesToolStripMenuItem.Visible = true;
                        break;

                    case "32":
                        reporteLiqidacionesToolStripMenuItem.Visible = true;
                        break;

                    case "33":
                        constanciasToolStripMenuItem.Visible = true;
                        break;
                }



            }

















        }


        // ---- desde aqui estan los controles de apertura de formularios ----------------------------------
        private void sincronizacionToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Sincronizacion fm1 = new Sincronizacion();
            fm1.Show();
        }

        private void sISCMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = @"C:\SISCM\SISDM.exe";
                p.StartInfo.Arguments = "login.dbf";
                p.Start();
            }
            catch
            {
                MessageBox.Show("No se encuentra instaldo SISDM");
            }
        }

        private void preimpresoToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Correlativo_fac fm11 = new Correlativo_fac();
            fm11.Show();
        }

        private void casosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Casos_Main fm9 = new Casos_Main();
            fm9.Show();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            XMLRW.writeLogin("", "", "", "");
            this.Close();
        }

        private void sISDMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = @"C:\SISCM\SISDM.exe";
                p.StartInfo.Arguments = "login.dbf";
                p.Start();
            }
            catch
            {
                MessageBox.Show("No se encuentra instaldo SISDM");
            }

        }

        private void reporteCarteraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    System.Diagnostics.Process p = new System.Diagnostics.Process();
            //    p.StartInfo.FileName = @"C:\CORRECT\App\ReporteCC.exe";
            //    p.Start();
            //}
            //catch
            //{
            //    MessageBox.Show("Error al intentar cargar Cartera de Credito");
            //}

           
        }

        private void sISDMToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = @"C:\SISCM\SISDM.exe";
                p.StartInfo.Arguments = "login.dbf";
                p.Start();
            }
            catch
            {
                MessageBox.Show("No se encuentra instaldo SISDM");
            }

        }

        private void sincronizacionToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            Sincronizacion fm1 = new Sincronizacion();
            fm1.Show();
        }

        private void regaliasToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Regalias_Vencido fm13 = new Regalias_Vencido();
            fm13.Show();
        }

        private void gPSRutasToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            GPS_clientes fm8 = new GPS_clientes();
            fm8.Show();
        }

        private void ingresoDeCasosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Casos_Main fm9 = new Casos_Main();
            fm9.Show();
        }

        private void gPSRutasToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GPS_clientes fm8 = new GPS_clientes();
            fm8.Show();
        }

        private void ingresoDeCasosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Casos_Main fm9 = new Casos_Main();
            fm9.Show();

        }

        private void procesarOcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OC fm14 = new OC();
            fm14.Show();
        }

        private void reporteCarteraToolStripMenuItem1_Click(object sender, EventArgs e)
        {
           // Reporte_Cartera fm7 = new Reporte_Cartera();
            Reporte_Carera_XLS fm7 = new Reporte_Carera_XLS();
            fm7.Show();
        }

        private void reporteEfectivoVentaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Reporte_venta_cobro fm15 = new Reporte_venta_cobro();
            fm15.Show();
        }

        private void cargasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cargas cr = new Cargas();

            cr.Show();
        }

        private void clienteUpdateFRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Update_Clientes_FR upc = new Update_Clientes_FR();
            upc.Show();
        }

        private void updateClientePedidoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Update_PEDIDOS upp = new Update_PEDIDOS();
            upp.Show();
        }

        private void actualizarNombrePedidosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Update_PEDIDOS udp = new Update_PEDIDOS();
            udp.Show();
        }

        private void excelToKMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExceltoKML exckml = new ExceltoKML();
            exckml.Show();
        }

//<<<<<<< .mine
//        private void ruterosToolStripMenuItem_Click(object sender, EventArgs e)
//        {
//            Ruteros tr = new Ruteros();
//            tr.Show();
//        }

//=======
        private void cargadorPagosDeContadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
           FrmCC_CargadorRecibos Cargadorpagos =new FrmCC_CargadorRecibos();
           Cargadorpagos.Show();
        }

        private void exportadorVentaKCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmKC_ExportadorVtaTxT ExportadorKC = new FrmKC_ExportadorVtaTxT();
            ExportadorKC.Show();
        }

        private void ruterosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rutas rt = new Rutas();

            rt.Show();
        }

        private void actualizarNombrePedidosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Update_PEDIDOS upp = new Update_PEDIDOS();
            upp.Show();
        }


        public void imagen_Ejecutando(PictureBox imagenes)
        {
            imagenes.Image = Properties.Resources.no_ready;

            imagenes.Refresh();
            imagenes.Visible = true;
        }

        public void imagen_Listo(PictureBox imagenes)
        {
            imagenes.Image = Properties.Resources.ready;

            imagenes.Refresh();
            imagenes.Visible = true;
        }


        // ----- valida el estatus de la  tarea programada  CARGA FACTURAS AL FR -------------------
        private bool Exists(string estatus)
        {

            //coned.con.Open();
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("SELECT COUNT (*) FROM [DM].[CORRECT].[Tareas_programada]  where Estado = @estatus", con.condm);
            cmd.Parameters.AddWithValue("estatus", estatus);
            cmd.CommandTimeout = 0;

            int contar = Convert.ToInt32(cmd.ExecuteScalar());
            con.Desconectar("DM");
            //coned.con.Close();

            if (contar == 0)
            {
                return false;

            }
            else
            {
                return true;
            }

        }


        // Control de procesos sefun caritas -----------------------------
        private void validar(object sender, EventArgs e)
        { 
          string estado = "Ejecutandoce";
          if (Exists(estado))
          {
              imagen_Ejecutando(pictureBox2);
              label1.Text = "Carga de FR-ERP Ejecutándose...";
             
              this.timer1.Interval = (60000);
              this.timer1.Start();


          }
          else
          { 
          imagen_Listo (pictureBox2);
          label1.Text = "Sin Ejecución";

              this.timer1.Stop();

         

            
          }

        
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            validar(null, null);
        }
        //-------------- programacion de  tiempo de refrescamiendo para validar  ejeccicion de Proceso FR-------------------------
        private void timer2_Tick(object sender, EventArgs e)
        {

            label3.Text = DateTime.Now.ToLongTimeString();
            fechaup = DateTime.Now;

            if (fechaup.ToLongTimeString() == "4:00:05 PM")
            {
                validar(null, null);

            }
            if (fechaup.ToLongTimeString() == "5:00:05 PM")
            {
                validar(null, null);

            }

            if (fechaup.ToLongTimeString() == "6:15:05 PM")
            {
                validar(null, null);
            }

            if (fechaup.ToLongTimeString() == "7:30:05 PM")
            {
                validar(null, null);
            }
            if (fechaup.ToLongTimeString() == "8:45:05 PM")
            {
                validar(null, null);
            }
            if (fechaup.ToLongTimeString() == "9:50:05 PM")
             {
                 validar(null, null);
             } 
                
                      
        }
        //------------------------------------------------------------------------------------------------


        private void updateDireccionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Direcciones_UPDATE Direcciones = new Direcciones_UPDATE();
            Direcciones.Show();
        }

        private void sincronizacionToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Sincronizacion sicn = new Sincronizacion();
            sicn.Show();
        }

        private void nombresPDAUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Update_Clientes_FR upc = new Update_Clientes_FR();
            upc.Show();
        }

        private void accesoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Accesos ac = new Accesos();
            ac.Show();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void correlativosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Correlativo_process cop = new Correlativo_process();
            cop.Show();
        }

        private void vehiculosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Control_Vehiculos cvh = new Control_Vehiculos();
            cvh.Show();
        }

        private void combustiblesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Combustible_flota cbf = new Combustible_flota();
            cbf.Show();

        }

        private void reportesToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // Reporte_flota rpf = new Reporte_flota();
          //  rpf.Show();
        }

        private void correlativoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void limitesDeCreditoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Limites_Creditos ltc = new Limites_Creditos();

            ltc.Show();

        }

        private void pedidosFacturacionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reporte_Pedidos_facturas rpf = new Reporte_Pedidos_facturas();
            rpf.Show();
        }

        private void saldosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reporte_Saldos_Reps Saldosrep = new Reporte_Saldos_Reps();
            Saldosrep.Show();
        }

        // desabilita dodos los iconos  esto se ejecuta al iniciar el formulario MAIN-----------------------------------
        private void desabilitar()
        {
            toolStripDropDownButton1.Visible = false;
            toolStripDropDownButton2.Visible = false;
            toolStripDropDownButton3.Visible = false;
            toolStripDropDownButton4.Visible = false;
            toolStripDropDownButton5.Visible = false;
            toolStripButton6.Visible = false;
            toolStripButton3.Visible = false;
            correlativoToolStripMenuItem.Visible = false;
            sincronizacionToolStripMenuItem.Visible = false;
            sISCMToolStripMenuItem.Visible = false;
            casosToolStripMenuItem.Visible = false;
            correlativosToolStripMenuItem.Visible = false;
            oCToolStripMenuItem.Visible = false;
            clienteUpdateFRToolStripMenuItem.Visible = false;
            updateClientePedidoToolStripMenuItem.Visible = false;
            excelToKMLToolStripMenuItem.Visible = false;
            kimberlyToolStripMenuItem.Visible = false;
            accesoToolStripMenuItem.Visible = false;
            gPSRutasToolStripMenuItem.Visible = false;
            ruterosToolStripMenuItem.Visible = false;
            reporteCarteraToolStripMenuItem.Visible = false;
            cargadorPagosDeContadoToolStripMenuItem.Visible = false;
            facturacionToolStripMenuItem.Visible = false;
            regaliasToolStripMenuItem.Visible = false;
            cargasToolStripMenuItem.Visible = false;
            vehiculosToolStripMenuItem.Visible = false;
            asignacionesToolStripMenuItem.Visible = false;
            mantenimientosToolStripMenuItem.Visible = false;
            kilometrajesToolStripMenuItem.Visible = false;
            combustiblesToolStripMenuItem.Visible = false;
            reportesToolStripMenuItem.Visible = false;
            preimpresoToolStripMenuItem.Visible = false;
            procesarOcToolStripMenuItem.Visible = false;
            exportadorVentaKCToolStripMenuItem.Visible = false;
            reporteCarteraToolStripMenuItem1.Visible = false;
            reporteEfectivoVentaToolStripMenuItem1.Visible = false;
            updateDireccionesToolStripMenuItem.Visible = false;
            reportesToolStripMenuItem.Visible = false;
            limitesDeCreditoToolStripMenuItem.Visible = false;
            reportesBodegaToolStripMenuItem1.Visible = false;
            pedidosFacturacionToolStripMenuItem.Visible = false;
            reportesDismoappToolStripMenuItem.Visible = false;
            rutasToolStripMenuItem.Visible = false;
            herramientasToolStripMenuItem.Visible = false;
            competenciaToolStripMenuItem.Visible = false;
            importadorStreetToolStripMenuItem.Visible = false;
            empresasToolStripMenuItem.Visible = false;
            evaluacionesToolStripMenuItem.Visible = false;
            reportesToolStripMenuItem1.Visible = false;
            toolStripButton7.Visible = false;
            reporteAccesosToolStripMenuItem.Visible = false;
            diasInventarioToolStripMenuItem.Visible = false;
            impuestoDistribucionToolStripMenuItem.Visible = false;
            festivosDISMOToolStripMenuItem.Visible = false;
            devolucionRepartoToolStripMenuItem.Visible = false;
            marcacionToolStripMenuItem.Visible = false;
            merchandisingToolStripMenuItem.Visible = false;
            visitasComentariosToolStripMenuItem.Visible = false;
            libroInventariosToolStripMenuItem.Visible = false;
            promocionesToolStripMenuItem.Visible = false;
            promocionesPorPedidoToolStripMenuItem.Visible = false;
            facturacionToolStripMenuItem.Visible = false;
            facturacionToolStripMenuItem1.Visible = false;
            regaliasVentasToolStripMenuItem.Visible = false;
            regaliasToolStripMenuItem.Visible = false;
            reporteDescuentosToolStripMenuItem.Visible = false;
            fillrateToolStripMenuItem.Visible = false;
            reporteDescuentosXDocumentoToolStripMenuItem.Visible = false;
            preciosToolStripMenuItem.Visible = false;
            cargaToolStripMenuItem.Visible = false;
            reporteTransaccionesToolStripMenuItem.Visible = false;
            controlFlotaToolStripMenuItem.Visible = false;
            edicionDatosEmpleadosMarcacionToolStripMenuItem.Visible = false;
            procesosExactusToolStripMenuItem.Visible = false;
            reporteLiqidacionesToolStripMenuItem.Visible = false;
            constanciasToolStripMenuItem.Visible = false;

        }

        private void rutasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rutas_Trade_Reps RutaReps = new Rutas_Trade_Reps();
            RutaReps.Show();
        }

        private void competenciaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Menu_Evaluaciones ev = new Menu_Evaluaciones();
            CORECTX_APP.RRHH.Evaluaciones_Main ev = new CORECTX_APP.RRHH.Evaluaciones_Main();
            ev.Show();
        }

        private void cargaCostosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //   Carga_Costos_Regalias csc = new Carga_Costos_Regalias();
            CORECTX_APP.BODEGA.costos_articulo csc = new CORECTX_APP.BODEGA.costos_articulo();
            csc.Show();
        }

        private void pictureBox2_DoubleClick(object sender, EventArgs e)
        {
            validar(null,null);
        }

        private void importadorStreetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void empresasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CORECTX_APP.Informatica.Administracion_Empresas emp = new CORECTX_APP.Informatica.Administracion_Empresas();
            emp.Show();
        }

        private void evaluacionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CORECTX_APP.RRHH.Reportes_Evaluaciones RRHEV = new CORECTX_APP.RRHH.Reportes_Evaluaciones();
            RRHEV.Show();
        }

        private void reporteAccesosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CORECTX_APP.SEGURIDAD.Reporte_Accesos repotr = new CORECTX_APP.SEGURIDAD.Reporte_Accesos();
            repotr.Show();
        }

        private void diasInventarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CORECTX_APP.BODEGA.REportes Diasinv = new CORECTX_APP.BODEGA.REportes();
            Diasinv.Show();
                
        }

        private void impuestoDistribucionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CORECTX_APP.CREDITOS.IMPUESTO_DISTRIB.Pago_Impuesto_Distribucion impdis = new CORECTX_APP.CREDITOS.IMPUESTO_DISTRIB.Pago_Impuesto_Distribucion();
            impdis.Show();
        }

        private void festivosDISMOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CORECTX_APP.Informatica.festivos fst = new CORECTX_APP.Informatica.festivos();
            fst.Show();
        }

        private void devolucionRepartoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CORECTX_APP.CREDITOS.LIQUIDACIONES.LIQUIDACIONES liq = new CORECTX_APP.CREDITOS.LIQUIDACIONES.LIQUIDACIONES();
            liq.Show();
        }

        private void marcacionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CORECTX_APP.RRHH.Boleta_Marcacion marca = new CORECTX_APP.RRHH.Boleta_Marcacion();
            marca.Show();
        }

        private void reporteMueblesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CORECTX_APP.VENTAS.muebles_imagenes muebles = new CORECTX_APP.VENTAS.muebles_imagenes();
            muebles.Show();
        }

        private void visitasComentariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CORECTX_APP.VENTAS.Visitas_Comentarios coment = new CORECTX_APP.VENTAS.Visitas_Comentarios();
            coment.Show();

          
        }

        private void toolStripDropDownButton4_Click(object sender, EventArgs e)
        {

        }

        private void libroInventariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CORECTX_APP.BODEGA.REGALIAS.Report_Libro lib = new CORECTX_APP.BODEGA.REGALIAS.Report_Libro();
            lib.Show();

        }

        private void promocionesPorPedidoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CORECTX_APP.BODEGA.Promociones_pedidas promoped = new CORECTX_APP.BODEGA.Promociones_pedidas();
            promoped.Show();
        }

        private void regaliasToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CORECTX_APP.BODEGA.REGALIAS.Regalias reg = new CORECTX_APP.BODEGA.REGALIAS.Regalias();
            reg.Show();
        }

        private void ingresoFacturasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CORECTX_APP.VENTAS.FACTURACION.Facturacion fac = new CORECTX_APP.VENTAS.FACTURACION.Facturacion();
            fac.Show();
        }

        private void regaliasVentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = @"C:\CORRECT\Regalias\Solicitud Regalia.exe";
               // p.StartInfo.Arguments = "login.dbf";
                p.Start();
            }
            catch
            {
                MessageBox.Show("No se encuentra instaldo Regalias para Ventas");
            }
        }

        private void reporteDescuentosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CORECTX_APP.CREDITOS.REPORTES.descuentos des = new CORECTX_APP.CREDITOS.REPORTES.descuentos();
            des.Show();
        }

        private void fillrateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CORECTX_APP.BODEGA.fillaje_capacidades fill = new CORECTX_APP.BODEGA.fillaje_capacidades();
            fill.Show();
        }

        private void reporteDescuentosXDocumentoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CORECTX_APP.CREDITOS.REPORTES.DescuentoxDocuemtos desdoc = new CORECTX_APP.CREDITOS.REPORTES.DescuentoxDocuemtos(1, reporteDescuentosXDocumentoToolStripMenuItem.Text);
            desdoc.Show();
        }

        private void preciosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CORECTX_APP.BODEGA.PRECIOS.importadorListaPrecios impre = new CORECTX_APP.BODEGA.PRECIOS.importadorListaPrecios();
            impre.Show();
        }

        private void cargaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CORECTX_APP.RRHH.CARGA_BAC bac = new CORECTX_APP.RRHH.CARGA_BAC();
            bac.Show();
        }

        private void controlFlotaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process f = new System.Diagnostics.Process();
            f.StartInfo.FileName = @"C:\CORRECT\CORECTX APP\FLOTA\FLOTA.exe";          
            f.Start();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {

        }

        private void edicionDatosEmpleadosMarcacionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CORECTX_APP.RRHH.Actulizacion_Marcacion act = new CORECTX_APP.RRHH.Actulizacion_Marcacion();
            act.Show();
              
        }

        private void reporteTransaccionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CORECTX_APP.CREDITOS.REPORTES.Reporte_Lavado Replab = new CORECTX_APP.CREDITOS.REPORTES.Reporte_Lavado();
            Replab.Show();
                
                 
        }

        private void importadorPowerStreetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Importador_Street imp = new Importador_Street();
            imp.Show();
        }

        private void exportadorDMSToolStripMenuItem_Click(object sender, EventArgs e)
        {

            config = XMLRW.Readxml("SFTP");

            DataRow rowtp = config.Rows[0];
            string AUTOMATICO = Convert.ToString(rowtp["AUTOMATICO"]);

            if (AUTOMATICO == "YES")
            {
                MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show("EL EXPORTADOR ESTA EN AUTOMATICO SI LO ABRE SE EJECUTARA AUROMATICA MENTE DESEA EJECUTARLO? ", "EXPORTADOR UNILEVERD DMS", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {
                    CORECTX_APP.Informatica.Unilever.InterfaceDMS dms = new CORECTX_APP.Informatica.Unilever.InterfaceDMS();
                    dms.Show();
                }
                else
                {

                }
            }
            else
            {
                CORECTX_APP.Informatica.Unilever.InterfaceDMS dms = new CORECTX_APP.Informatica.Unilever.InterfaceDMS();
                dms.Show();
            }
                 
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }

        private void procesosExactusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CORECTX_APP.Informatica.Procesos.Procesos_Exactus proc = new CORECTX_APP.Informatica.Procesos.Procesos_Exactus();
            proc.Show();
        }

        private void regaliasVentasToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Reporte_Regalia reg = new Reporte_Regalia("Nuevo");
            reg.Show();
        }

        private void reporteLiqidacionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CORECTX_APP.CREDITOS.REPORTES.DescuentoxDocuemtos desdoc = new CORECTX_APP.CREDITOS.REPORTES.DescuentoxDocuemtos(2, reporteLiqidacionesToolStripMenuItem.Text);
            desdoc.Show();
        }

        private void constanciasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CORECTX_APP.RRHH.Constancias cons = new CORECTX_APP.RRHH.Constancias();
            cons.Show();
        }
    }
 
    }

