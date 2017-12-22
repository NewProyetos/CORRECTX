using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using System.Diagnostics;


namespace Sinconizacion_EXactus
{
    public partial class Sincronizacion : DevExpress.XtraEditors.XtraForm
    {

        //conexion conet = new conexion();
        //Conexion2 coned = new Conexion2();
        //conexion_master conedmaster = new conexion_master();
        conexionXML con = new conexionXML();
        public static String fecha_sinc;
        public static string PedidoN;
        public static String conduit;
        public static String time;
        public String feha_hist;
        DateTime fechaup;
        public string stado;
        public int boni;
        DataTable Usuarios_exactus = new DataTable();
        DataTable Usuarios_APP_Exactus = new DataTable();
        String tiempo;
        TreeNode node3 = new TreeNode();
        TreeNode node4 = new TreeNode();
        TreeNode node5 = new TreeNode();

        TreeNode node = new TreeNode();
        TreeNode node1 = new TreeNode();
        string Nombre_user;

        private ContextMenuStrip menugrid = new ContextMenuStrip();
       

        DataTable devoluciones = new DataTable();
        String ULTIMONCF;
        String NCF;

        public Sincronizacion()
        {
            InitializeComponent();
            DevExpress.Skins.SkinManager.EnableFormSkins();
        }

        public static String Selected_File;
        

        private void Fecha_Sincro_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
         {

            ToolStripMenuItem KILL = new ToolStripMenuItem("KILL",null, new EventHandler(kill_process));
            menugrid.Items.AddRange(new ToolStripMenuItem[] { KILL });

            this.Text = "SINCRONIZACION   (" + Login.empresa + " ) ";
            descuentos(null, null);
             consulta_task(null, null);
            // timer2.Interval = (60000) * 10;
            // timer2.Start();
             button7.Enabled = false;

            MaximizeBox = false;
              tiempo = DateTime.Now.ToString("tt");

              if (Main_Menu.Carga_ERP_FR != 1)
              {
                  groupBox2.Enabled = false;
                  button6.Enabled = false;
                  groupBox5.Enabled = false;
                  groupBox5.Enabled = true;
                  this.comboBox3.Text = "Tarde";
                  this.comboBox1.Text = "FRmdescarga";
              
                                                  
              }

              else
              {
                  if (tiempo == "AM")
                  {
                      this.comboBox3.Text = "Mañana";
                      this.comboBox1.Text = "FRmcarga";

                  }
                  else
                  {
                      this.comboBox3.Text = "Tarde";
                      this.comboBox1.Text = "FRmdescarga";
                  }
              }
              if (Main_Menu.Descuentos_Bonidicaciones_acceso != 1)
              {

                  button7.Enabled = false;

              }
              else
              {
                  button7.Enabled = true;
              }
            
           // label18.Text = "Ver." + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            
            Fecha_Sincro.Format = DateTimePickerFormat.Custom;
            Fecha_Sincro.CustomFormat = " dd-MM-yyyy";
            
        

            con.conectar("EX");




            SqlCommand cm2 = new SqlCommand("SELECT PED.COD_ZON FROM  ERPADMIN.alFAC_ENC_PED as PED LEFT JOIN ERPADMIN.RUTA_ASIGNADA_RT as RT on PED.COD_ZON = RT.RUTA where RT.COMPANIA = '"+Login.empresa+"' GROUP BY COD_ZON ORDER BY COD_ZON ASC", con.conex);
            cm2.CommandTimeout = 0;
            SqlDataReader dr2 = cm2.ExecuteReader();
            
            while (dr2.Read())
            {
                comboBox2.Items.Add(dr2["COD_ZON"]);
            }
            dr2.Close();

            listView1.View = View.Details; //Detalles
            listView1.GridLines = true; //Lineas
            listView1.FullRowSelect = true;

            listView2.View = View.Details; //Detalles
            listView2.GridLines = true; //Lineas
            listView2.FullRowSelect = true;

            listView3.View = View.Details; //Detalles
            listView3.GridLines = true; //Lineas
            listView3.FullRowSelect = true;

            listView4.View = View.Details; //Detalles
            listView4.GridLines = true; //Lineas
            listView4.FullRowSelect = true;

            listView5.View = View.Details; //Detalles
            listView5.GridLines = true; //Lineas
            listView5.FullRowSelect = true;

            con.Desconectar("EX");
          

        }

        private void kill_process(object sender, EventArgs e)
        {
            if (Nombre_user == "")
            {

                MessageBox.Show("Seleccione un Usuario para cerrar secion");
            }
            else
            {

                MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show("Desea Eliminar la conecxion SQL-EXACTUS de USUARIO: "+Nombre_user+"?", "KILL PROCESS", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {


                    con.conectar("MAS");

                    SqlCommand cmd = new SqlCommand("[dbo].[KILL_EXACTUS]", con.conmas);
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@usuario", Nombre_user);
                    cmd.ExecuteNonQuery();
                    con.Desconectar("MAS");

                    button2_Click(null, null);
                }

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            fecha_sinc = Fecha_Sincro.Value.ToString("yyyy-MM-dd");
            conduit = this.comboBox1.Text;
 
           
            con.conectar("EX");
            treeView1.Nodes.Clear();
            treeView1.ImageList = imageList1;
            
           
            // carga los nodos del tree con las rutas sincronizadas  en FRMCARGA------------------
            if (this.comboBox3.Text == "Mañana")
            {


                SqlCommand cm = new SqlCommand("SELECT SINC.PDA,SINC.CONDUIT,SINC.STATE,SINC.SYNC_START,SINC.SYNC_END FROM [EXACTUS].[ERPADMIN].[EMM_SYNCINFO] as SINC   LEFT JOIN [EXACTUS].[ERPADMIN].[RUTA_ASIGNADA_RT] as RUT on SINC.PDA = RUT.HANDHELD  WHERE   RUT.COMPANIA = '" + Login.empresa + "' and SINC.CONDUIT = '" + this.comboBox1.Text + "' AND  SINC.SYNC_START BETWEEN '" + this.Fecha_Sincro.Value.ToString("yyyy-MM-dd") + " 1:00:00' AND '" + this.Fecha_Sincro.Value.ToString("yyyy-MM-dd") + " 12:00:00'  GROUP BY SINC.CONDUIT,SINC.STATE,SINC.SYNC_START,SINC.SYNC_END,SINC.PDA  ORDER BY SINC.PDA ", con.conex);
                cm.CommandTimeout = 0;
                SqlDataReader dr = cm.ExecuteReader();

                try
                {

                    while (dr.Read())
                    {
                       node = new TreeNode(dr["PDA"].ToString());
                       
                        
                        node1 = new TreeNode(dr["SYNC_START"].ToString());
                        
                        node.Nodes.Add(node1);
                        node1.Nodes.Add(dr["CONDUIT"].ToString());
                        node1.Nodes.Add("Estado: " + dr["STATE"].ToString());
                        string estado = Convert.ToString(dr["STATE"]);
                         node1.Nodes.Add("Fin: " + dr["SYNC_END"].ToString());

                         if (estado == "Exito")
                         {
                             node.ImageIndex = 0;
                             node.SelectedImageIndex = 0;
                         }
                         else
                         {
                             node1.ImageIndex = 2;
                             node1.SelectedImageIndex = 2;
                             node.ImageIndex = 2;
                             node.SelectedImageIndex = 2;


                         }

                        treeView1.Nodes.Add(node);

                     

                    }
                    dr.Close();
                    
                    
                }
                catch
                {
                    MessageBox.Show("No se pudo cargar informacion");
                    dr.Close();
                }

                SqlCommand cm1 = new SqlCommand("SELECT COUNT(HANDHELD) as 'sinc' FROM [EXACTUS].[ERPADMIN].[HANDHELD_RT]  WHERE  HANDHELD  IN (SELECT SYNC.PDA FROM [EXACTUS].[ERPADMIN].[EMM_SYNCINFO] as SYNC LEFT JOIN ERPADMIN.RUTA_ASIGNADA_RT as RT on SYNC.PDA = RT.HANDHELD  WHERE RT.COMPANIA = '"+Login.empresa+"' AND CONDUIT = '" + this.comboBox1.Text + "'  AND  SYNC_START BETWEEN '" + Fecha_Sincro.Value.ToString("yyyy-MM-dd") + " 1:00:00' AND '" + Fecha_Sincro.Value.ToString("yyyy-MM-dd") + " 12:00:00')", con.conex);
                cm1.CommandTimeout = 0;
                SqlDataReader dr1 = cm1.ExecuteReader();
                while (dr1.Read())
                {
                    label21.Text = Convert.ToString(dr1["SINC"]);
                }
                dr1.Close();

                SqlCommand cm2 = new SqlCommand("SELECT COUNT (A. HANDHELD) as 'NO SINC' FROM  ERPADMIN.RUTA_ASIGNADA_RT A INNER JOIN ERPADMIN.AGENTE_RT B ON A.AGENTE = B.AGENTE WHERE A.COMPANIA = '"+Login.empresa+"' AND A. HANDHELD NOT IN (SELECT PDA FROM [EXACTUS].[ERPADMIN].[EMM_SYNCINFO] WHERE  CONDUIT = '" + this.comboBox1.Text + "'  AND  SYNC_START BETWEEN '" + Fecha_Sincro.Value.ToString("yyyy-MM-dd") + " 1:00:00' AND '" + Fecha_Sincro.Value.ToString("yyyy-MM-dd") + " 12:00:00')AND B.NOMBRE <> 'OFFLINE' ", con.conex);
                cm2.CommandTimeout = 0;
                SqlDataReader dr2 = cm2.ExecuteReader();
                while (dr2.Read())
                {
                    this.linkLabel2.Text = Convert.ToString(dr2["NO SINC"]);
                }
                dr2.Close();



            }
            else
            {
                // carga los nodos del tree con las rutas sincronizadas  en FRMDESCARGA------------------
                if (this.comboBox3.Text == "Tarde")
                {


                    SqlCommand cm = new SqlCommand("SELECT SYNC.PDA,SYNC.CONDUIT,SYNC.STATE,SYNC.SYNC_START,SYNC.SYNC_END FROM [EXACTUS].[ERPADMIN].[EMM_SYNCINFO] AS SYNC LEFT JOIN ERPADMIN.RUTA_ASIGNADA_RT as RT ON SYNC.PDA = RT.HANDHELD WHERE RT.COMPANIA = '"+Login.empresa+"' AND SYNC.CONDUIT = '" + this.comboBox1.Text + "' AND  SYNC.SYNC_START BETWEEN '" + this.Fecha_Sincro.Value.ToString("yyyy-MM-dd") + " 12:00:00' AND '" + this.Fecha_Sincro.Value.ToString("yyyy-MM-dd") + " 23:59:00' GROUP BY SYNC.CONDUIT,SYNC.STATE,SYNC.SYNC_START,SYNC.SYNC_END,PDA  ORDER BY PDA ", con.conex);
                    cm.CommandTimeout = 0;
                    SqlDataReader dr = cm.ExecuteReader();
                    
                    try
                    {


                        while (dr.Read())
                        {
                            node = new TreeNode(dr["PDA"].ToString());
                            node1 = new TreeNode(dr["SYNC_START"].ToString());
                            node.Nodes.Add(node1);
                            node1.Nodes.Add(dr["CONDUIT"].ToString());

                            node1.Nodes.Add("Estado: " + dr["STATE"].ToString());
                            string estado = Convert.ToString(dr["STATE"]);

                            node1.Nodes.Add("Fin: " + dr["SYNC_END"].ToString());


                            if (estado == "Exito")
                            {
                                node.ImageIndex = 0;
                                node.SelectedImageIndex = 0;
                            }
                            else
                            {
                                node1.ImageIndex = 2;
                                node1.SelectedImageIndex = 2;
                                node.ImageIndex = 2;
                                node.SelectedImageIndex = 2;


                            }
                            treeView1.Nodes.Add(node);



                        }
                        dr.Close();
                    }
                    catch
                    {
                        MessageBox.Show("No se pudo cargar informacion");
                        dr.Close();
                    }
                    // Carga la cantidad de  Rutas  SINCRONIZADAS --------------------------------------- 
                    SqlCommand cm1 = new SqlCommand("SELECT COUNT(SINC.HANDHELD) as 'sinc' FROM [EXACTUS].[ERPADMIN].[HANDHELD_RT] as  SINC LEFT JOIN ERPADMIN.RUTA_ASIGNADA_RT as RT on SINC.HANDHELD = RT.HANDHELD WHERE RT.COMPANIA = '"+Login.empresa+"' AND SINC.HANDHELD  IN (SELECT PDA FROM [EXACTUS].[ERPADMIN].[EMM_SYNCINFO] WHERE CONDUIT = '" + this.comboBox1.Text + "'  AND  SYNC_START BETWEEN '" + Fecha_Sincro.Value.ToString("yyyy-MM-dd") + " 12:00:00' AND '" + Fecha_Sincro.Value.ToString("yyyy-MM-dd") + " 23:59:00')", con.conex);
                    cm1.CommandTimeout = 0;
                    SqlDataReader dr1 = cm1.ExecuteReader();
                    while (dr1.Read())
                    {
                        label21.Text = Convert.ToString(dr1["SINC"]);
                    }
                    dr1.Close();
                    // Carga la cantidad de  Rutas NO SINCRONIZADAS --------------------------------------- 

                    SqlCommand cm2 = new SqlCommand("SELECT COUNT (A. HANDHELD) as 'NO SINC' FROM  ERPADMIN.RUTA_ASIGNADA_RT A INNER JOIN ERPADMIN.AGENTE_RT B ON A.AGENTE = B.AGENTE WHERE A.COMPANIA = '"+Login.empresa+"' AND A. HANDHELD NOT IN (SELECT PDA FROM [EXACTUS].[ERPADMIN].[EMM_SYNCINFO] WHERE  CONDUIT = '" + this.comboBox1.Text + "'  AND  SYNC_START BETWEEN '" + Fecha_Sincro.Value.ToString("yyyy-MM-dd") + " 12:00:00' AND '" + Fecha_Sincro.Value.ToString("yyyy-MM-dd") + " 23:59:00')AND B.NOMBRE <> 'OFFLINE' ", con.conex);
                    cm2.CommandTimeout = 0;
                    SqlDataReader dr2 = cm2.ExecuteReader();
                    while (dr2.Read())
                    {
                        this.linkLabel2.Text = Convert.ToString(dr2["NO SINC"]);
                    }
                    dr2.Close();

                }
                else
                {
                    // TODAS LAS SINCRONIZACIONES MAÑANA Y TARDE --------------------------------------- 
                    if (this.comboBox3.Text == "Todos")
                    {

                        SqlCommand cm = new SqlCommand("SELECT PDA,CONDUIT,STATE,SYNC_START,SYNC_END FROM [EXACTUS].[ERPADMIN].[EMM_SYNCINFO] as SYNC LEFT JOIN ERPADMIN.RUTA_ASIGNADA_RT as  RT ON SYNC.PDA = RT.HANDHELD  WHERE RT.COMPANIA = '"+Login.empresa+"' AND CONDUIT = '" + this.comboBox1.Text + "' AND DATEADD(dd, 0, DATEDIFF(dd, 0, SYNC_START ))='" + this.Fecha_Sincro.Value.ToString("yyyy-MM-dd") + "'GROUP BY CONDUIT,STATE,SYNC_START,SYNC_END,PDA ", con.conex);
                        cm.CommandTimeout = 0;
                        SqlDataReader dr = cm.ExecuteReader();

                        try
                        {


                            while (dr.Read())
                            {
                                node = new TreeNode(dr["PDA"].ToString());
                                node1 = new TreeNode(dr["SYNC_START"].ToString());
                                node.Nodes.Add(node1);
                                node1.Nodes.Add(dr["CONDUIT"].ToString());
                                node1.Nodes.Add("Estado: " + dr["STATE"].ToString());
                                string estado = Convert.ToString(dr["STATE"]);
                                //node1.Nodes.Add("Incio: "+dr["SYNC_START"].ToString());
                                node1.Nodes.Add("Fin: " + dr["SYNC_END"].ToString());

                                if (estado == "Exito")
                                {
                                    node.ImageIndex = 0;
                                    node.SelectedImageIndex = 0;
                                }
                                else
                                {
                                    node1.ImageIndex = 2;
                                    node1.SelectedImageIndex = 2;
                                    node.ImageIndex = 2;
                                    node.SelectedImageIndex = 2;


                                }

                                treeView1.Nodes.Add(node);



                            }

                            dr.Close();


                            SqlCommand nosic = new SqlCommand("SELECT COUNT(A. HANDHELD) as 'NO SINC' FROM  ERPADMIN.RUTA_ASIGNADA_RT A INNER JOIN ERPADMIN.AGENTE_RT B ON A.AGENTE = B.AGENTE WHERE A.COMPANIA = '"+Login.empresa+"' AND A. HANDHELD NOT IN (SELECT PDA FROM [EXACTUS].[ERPADMIN].[EMM_SYNCINFO] WHERE  CONDUIT = '" + Sincronizacion.conduit + "' AND PDA LIKE 'P%' AND  SYNC_START BETWEEN '" + Sincronizacion.fecha_sinc + " 1:00:00' AND '" + Sincronizacion.fecha_sinc + " 23:59:59')AND B.NOMBRE <> 'OFFLINE' ", con.conex);
                            nosic.CommandTimeout = 0;
                            SqlDataReader nosicndr = nosic.ExecuteReader();
                            while (nosicndr.Read())
                            {
                                this.linkLabel2.Text = Convert.ToString(nosicndr["NO SINC"]);
                            }

                            nosicndr.Close();
                        }
                        catch
                        {
                            MessageBox.Show("No se pudo cargar informacion");
                            dr.Close();
                        }

                         //Carga la cantidad de  Rutas  SINCRONIZADAS --------------------------------------- 
                        SqlCommand cm1 = new SqlCommand("SELECT COUNT(SINC.HANDHELD) as 'sinc' FROM [EXACTUS].[ERPADMIN].[HANDHELD_RT] as SINC LEFT JOIN ERPADMIN.RUTA_ASIGNADA_RT AS RT ON SINC.HANDHELD = RT.HANDHELD  WHERE RT.COMPANIA = '" + Login.empresa+"' AND SINC.HANDHELD  IN (SELECT PDA FROM [EXACTUS].[ERPADMIN].[EMM_SYNCINFO] WHERE CONDUIT = '" + this.comboBox1.Text + "'  AND DATEADD(dd, 0, DATEDIFF(dd, 0, SYNC_START ))='" + Fecha_Sincro.Value.ToString("yyyy-MM-dd") + "')", con.conex);
                        cm1.CommandTimeout = 0;
                        SqlDataReader dr1 = cm1.ExecuteReader();
                        while (dr1.Read())
                        {
                            label21.Text = Convert.ToString(dr1["SINC"]);
                        }
                        dr1.Close();

                        //// Carga la cantidad de  Rutas NO SINCRONIZADAS --------------------------------------- 
                        //SqlCommand cm2 = new SqlCommand("SELECT COUNT(HANDHELD) as 'NO sinc' FROM [EXACTUS].[ERPADMIN].[HANDHELD_RT]  WHERE HANDHELD NOT IN (SELECT PDA FROM [EXACTUS].[ERPADMIN].[EMM_SYNCINFO] WHERE CONDUIT = '" + this.comboBox1.Text + "' AND PDA LIKE 'P%' AND DATEADD(dd, 0, DATEDIFF(dd, 0, SYNC_START ))='" + Fecha_Sincro.Value.ToString("yyyy-MM-dd") + "')", con.conex);
                        //SqlDataReader dr2 = cm2.ExecuteReader();
                        //while (dr2.Read())
                        //{
                        //    this.linkLabel2.Text = Convert.ToString(dr2["NO SINC"]);
                        //}
                        //dr2.Close();
                    
                    }
                  
                
                }






            
            }                         
            


            con.Desconectar("EX");

            button4_Click(null, null);
            
        }
        private void treeView1_NodeMouseClick(object sender,TreeNodeMouseClickEventArgs e)
        {
            MessageBox.Show("Ok");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Usuarios_exactus.Clear();
            Usuarios_APP_Exactus.Clear();
            treeView2.Nodes.Clear();
          
          

           con.conectar("MAS");

            SqlCommand cmd = new SqlCommand("[dbo].[USUARIOS_EXACTUS]",con.conmas );
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(Usuarios_exactus);
            SqlCommand cmd1 = new SqlCommand("[dbo].[USUARIOS_APP_EXACTUS]", con.conmas);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.CommandTimeout = 0;
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            da1.Fill(Usuarios_APP_Exactus);

            con.Desconectar("MAS");

            for (int i = 0; i < Usuarios_exactus.Rows.Count; i++)
            {

                node3 = new TreeNode(Usuarios_exactus.Rows[i]["Login"].ToString());
                node3.Name = "USUARIO";
                string nombre_loguin = Usuarios_exactus.Rows[i]["login"].ToString();
                string nombre_loguin_host = Usuarios_exactus.Rows[i]["HostName"].ToString();
                node5 = new TreeNode(Usuarios_exactus.Rows[i]["HostName"].ToString());

                for (int o = 0; o < Usuarios_APP_Exactus.Rows.Count; o++)
                {
                   
                    string nombre_app_loguin = Usuarios_APP_Exactus.Rows[o]["login"].ToString();
                    string nombre_app_host = Usuarios_APP_Exactus.Rows[o]["HostName"].ToString();

                    if (nombre_loguin == nombre_app_loguin)
                    {

                        if (nombre_loguin_host == nombre_app_host)
                        {
                            node4 = new TreeNode(Usuarios_APP_Exactus.Rows[o]["ProgramName"].ToString());
                            string Nombre_programa = (Usuarios_APP_Exactus.Rows[o]["ProgramName"].ToString());
                            node5.Nodes.Add(node4);
                        }
                       
                    }
                    
                }
                
                node3.Nodes.Add(node5);
                
                

                treeView2.Nodes.Add(node3);
                int cantidad = Usuarios_exactus.Rows.Count;
                if (Main_Menu.Departamento == "INFORMATICA")
                {
                    treeView2.ContextMenuStrip = menugrid;
                }
                label36.Text = Convert.ToString(cantidad);
            }

            
           
            
           }

   

        private void button3_Click(object sender, EventArgs e)
        {
            // Carga Formulario2 que continede el historico de procesos de los Usuarios --------------------------------------- 
            Procesos Fm = new Procesos();
            Fm.ShowDialog();

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

       

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            // Carga los datos de las Rutas (pedidos,Facturas,cobros,etc) --------------------------------------- 
            con.conectar("EX");

            SqlCommand cm3 = new SqlCommand("select COUNT (NUM_PED)as FacturasPronta from  ERPADMIN.alFAC_ENC_PED where COD_ZON = '" + this.comboBox2.Text + "' AND DATEADD(dd, 0, DATEDIFF(dd, 0, FEC_PED ))='" + this.Fecha_Sincro.Value.ToString("yyyy-MM-dd") + "'AND  TIP_DOC = 'F' AND ESTADO = 'F'", con.conex);
            cm3.CommandTimeout = 0;
            SqlDataReader dr3 = cm3.ExecuteReader();

            while (dr3.Read())
            {
                textBox4.Text = Convert.ToString(dr3["FacturasPronta"]);
            }
            dr3.Close();

            SqlCommand cm10 = new SqlCommand("select COUNT (NUM_PED)as FacturasPronta from  ERPADMIN.alFAC_ENC_PED where COD_ZON = '" + this.comboBox2.Text + "' AND DATEADD(dd, 0, DATEDIFF(dd, 0, FEC_PED ))='" + this.Fecha_Sincro.Value.ToString("yyyy-MM-dd") + "'AND  TIP_DOC = 'F' AND ESTADO = 'F' AND DOC_PRO = 'S'", con.conex);
            cm10.CommandTimeout = 0;
            SqlDataReader dr10 = cm10.ExecuteReader();

            while (dr10.Read())
            {
                textBox8.Text = Convert.ToString(dr10["FacturasPronta"]);
            }
            dr10.Close();




            SqlCommand cm4 = new SqlCommand("select COUNT (NUM_REC)as cobros from   ERPADMIN.alCXC_DOC_APL where COD_ZON = '" + this.comboBox2.Text + "' AND DATEADD(dd, 0, DATEDIFF(dd, 0, FEC_PRO ))='" + this.Fecha_Sincro.Value.ToString("yyyy-MM-dd") + "'", con.conex);
            cm4.CommandTimeout = 0;
            SqlDataReader dr4 = cm4.ExecuteReader();
            while (dr4.Read())
            {
                textBox2.Text = Convert.ToString(dr4["cobros"]);
            }
            dr4.Close();

            SqlCommand cm5 = new SqlCommand("select COUNT (pedido) as pedidos from  ERPADMIN.V_MonitoreoPedidos a left join  ERPADMIN.alFAC_ENC_PED b on a.Pedido = b.NUM_PED where a.ruta = '" + this.comboBox2.Text + "' AND DATEADD(dd, 0, DATEDIFF(dd, 0, a.fecha_pedido ))='" + this.Fecha_Sincro.Value.ToString("yyyy-MM-dd") + "'AND  a.ESTADO = 'N'", con.conex);
            cm5.CommandTimeout = 0;
            SqlDataReader dr5 = cm5.ExecuteReader();
            while (dr5.Read())
            {
                textBox1.Text = Convert.ToString(dr5["pedidos"]);
            }
            dr5.Close();

            SqlCommand cm6 = new SqlCommand("select COUNT (pedido) as Facturas from  ERPADMIN.V_MonitoreoPedidos a left join  ERPADMIN.alFAC_ENC_PED b on a.Pedido = b.NUM_PED where a.ruta = '" + this.comboBox2.Text + "' AND DATEADD(dd, 0, DATEDIFF(dd, 0, a.fecha_pedido ))='" + this.Fecha_Sincro.Value.ToString("yyyy-MM-dd") + "'AND  a.ESTADO = 'N' AND b.CLASE ='N' ", con.conex);
            cm6.CommandTimeout = 0;
            SqlDataReader dr6 = cm6.ExecuteReader();
            while (dr6.Read())
            {
                textBox5.Text = Convert.ToString(dr6["Facturas"]);
            }
            dr6.Close();


            SqlCommand cm7 = new SqlCommand("select COUNT (pedido) as CCF from  ERPADMIN.V_MonitoreoPedidos a left join  ERPADMIN.alFAC_ENC_PED b on a.Pedido = b.NUM_PED where a.ruta = '" + this.comboBox2.Text + "' AND DATEADD(dd, 0, DATEDIFF(dd, 0, a.fecha_pedido ))='" + this.Fecha_Sincro.Value.ToString("yyyy-MM-dd") + "'AND  a.ESTADO = 'N' AND b.CLASE ='C' ", con.conex);
            cm7.CommandTimeout = 0;
            SqlDataReader dr7 = cm7.ExecuteReader();
            while (dr7.Read())
            {
                textBox6.Text = Convert.ToString(dr7["CCF"]);
            }
            dr7.Close();



            SqlCommand cm8 = new SqlCommand("select COUNT (NUM_DEV)as devolucion from  ERPADMIN.alFAC_ENC_DEV where COD_ZON = '" + this.comboBox2.Text + "' AND DATEADD(dd, 0, DATEDIFF(dd, 0, FEC_DEV ))='" + this.Fecha_Sincro.Value.ToString("yyyy-MM-dd") + "' AND EST_DEV = 'A' ", con.conex);
            cm8.CommandTimeout = 0;
            SqlDataReader dr8 = cm8.ExecuteReader();
            while (dr8.Read())
            {
                textBox3.Text = Convert.ToString(dr8["devolucion"]);
            }
            dr8.Close();



            SqlCommand cm9 = new SqlCommand("select COUNT (pedido) as Facturados from  ERPADMIN.V_MonitoreoPedidos a left join  ERPADMIN.alFAC_ENC_PED b on a.Pedido = b.NUM_PED where a.ruta = '" + this.comboBox2.Text + "' AND DATEADD(dd, 0, DATEDIFF(dd, 0, a.fecha_pedido ))='" + this.Fecha_Sincro.Value.ToString("yyyy-MM-dd") + "'AND  a.ESTADO = 'F'", con.conex);
            cm9.CommandTimeout = 0;
            cm9.CommandTimeout = 0;
            SqlDataReader dr9 = cm9.ExecuteReader();
            
            while (dr9.Read())
            {
                textBox7.Text = Convert.ToString(dr9["Facturados"]);
            }
            dr9.Close();
            con.Desconectar("EX");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Carga las Rutas que sincronizaron pero que no se ha cargado la info al ERP --------------------------------------- 

            con.conectar("EX");
            listView2.Clear();
            listView2.Columns.Add("Ruta", 92, HorizontalAlignment.Left);

            SqlCommand cm11 = new SqlCommand("SELECT COD_ZON as RUTA FROM ERPADMIN.alFAC_ENC_PED AS PED LEFT JOIN ERPADMIN.RUTA_ASIGNADA_RT AS RT ON PED.COD_ZON = RT.RUTA WHERE RT.COMPANIA = '"+Login.empresa+"' AND DOC_PRO IS NULL AND DATEADD(dd, 0, DATEDIFF(dd, 0, FEC_PED ))='" + this.Fecha_Sincro.Value.ToString("yyyy-MM-dd") + "' GROUP BY COD_ZON ", con.conex);
            cm11.CommandTimeout = 0;
            SqlDataReader dr11 = cm11.ExecuteReader();
            
            

            while (dr11.Read())
            {
                ListViewItem lvItem1 = new ListViewItem();
                lvItem1.SubItems[0].Text = dr11[0].ToString();
                lvItem1.SubItems.Add(dr11[0].ToString());
                listView2.Items.Add(lvItem1);
            }
            dr11.Close();
            // Carga los  Facturas que traen las Rutas y que no han sido cargadas al ERP --------------------------------------- 

            listView3.Clear();
            listView3.Columns.Add("Ruta", 40, HorizontalAlignment.Left);
            listView3.Columns.Add("Factura", 100, HorizontalAlignment.Left);

            SqlCommand cm12 = new SqlCommand("SELECT  PED.COD_ZON as RUTA, PED.NUM_PED as Pedido from  ERPADMIN.alFAC_ENC_PED as  PED  LEFT JOIN [EXACTUS].[ERPADMIN].[RUTA_ASIGNADA_RT] as RUT  on PED.COD_ZON = RUT.RUTA  left join "+Login.empresa+ ".FACTURA AS FAC on PED.NUM_PED = FAC.FACTURA where PED.TIP_DOC = 'F' AND DATEADD(dd, 0, DATEDIFF(dd, 0, PED.FEC_PED ))='" + this.Fecha_Sincro.Value.ToString("yyyy-MM-dd") + "' AND FAC.FACTURA is NULL and RUT.COMPANIA = '"+Login.empresa+"'", con.conex);
          //SqlCommand cm15 = new SqlCommand("select  a.COD_ZON as RUTA, a.NUM_PED as Pedido from   ERPADMIN.alFAC_ENC_PED a left join " + Login.empresa + ".FACTURA b on a.NUM_PED = b.FACTURA where a.TIP_DOC = 'F' AND DATEADD(dd, 0, DATEDIFF(dd, 0, a.FEC_PED ))='" + this.Fecha_Sincro.Value.ToString("yyyy-MM-dd") + "' AND b.FACTURA is NULL ", con.conex);
            cm12.CommandTimeout = 0;
            SqlDataReader dr12 = cm12.ExecuteReader();
           


            while (dr12.Read())
            {
                ListViewItem lvItem2 = new ListViewItem();
                lvItem2.SubItems[0].Text = dr12[0].ToString();
                lvItem2.SubItems.Add(dr12[1].ToString());
                lvItem2.SubItems.Add(dr12[1].ToString());
                listView3.Items.Add(lvItem2);
            }
            dr12.Close();

            // PEDIDOS -------------------------------------------------------------------------------------------------

            listView1.Clear();
            listView1.Columns.Add("Ruta", 40, HorizontalAlignment.Left);
            listView1.Columns.Add("Pedido", 100, HorizontalAlignment.Left);

            SqlCommand cm13 = new SqlCommand("select  a.COD_ZON as RUTA, a.NUM_PED as PEDIDO   from   ERPADMIN.alFAC_ENC_PED a  LEFT JOIN [EXACTUS].[ERPADMIN].[RUTA_ASIGNADA_RT] as RUT  on a.COD_ZON = RUT.RUTA  LEFT join "+Login.empresa+".PEDIDO b on a.NUM_PED = b.PEDIDO where a.TIP_DOC = '1' AND DATEADD(dd, 0, DATEDIFF(dd, 0, a.FEC_PED ))='" + this.Fecha_Sincro.Value.ToString("yyyy-MM-dd") + "'  and B.ESTADO = 'N' and RUT.COMPANIA = '" + Login.empresa+"' order by a.COD_ZON ", con.conex);
            cm13.CommandTimeout = 0;
            SqlDataReader dr13= cm13.ExecuteReader();
            

            while (dr13.Read())
            {
                ListViewItem lvItem5 = new ListViewItem();
                lvItem5.SubItems[0].Text = dr13[0].ToString();
                lvItem5.SubItems.Add(dr13[1].ToString());
                lvItem5.SubItems.Add(dr13[1].ToString());
                listView1.Items.Add(lvItem5);
            }
            dr13.Close();


        // Cobros -------------------------------------------------------

            listView4.Clear();
            listView4.Columns.Add("Ruta", 40, HorizontalAlignment.Left);
            listView4.Columns.Add("Recibo", 100, HorizontalAlignment.Left);

            SqlCommand cm14 = new SqlCommand("SELECT REC.COD_ZON,REC.NUM_REC FROM [EXACTUS].[ERPADMIN].[alCXC_MOV_DIR] as REC LEFT JOIN [EXACTUS].[ERPADMIN].[RUTA_ASIGNADA_RT] as RUT on REC.COD_ZON = RUT.RUTA  where NUM_REC not in (SELECT DOCUMENTO FROM [EXACTUS].[" + Login.empresa + "].[DOCUMENTOS_CC])  AND DATEADD(dd, 0, DATEDIFF(dd, 0, FEC_PRO ))='" + this.Fecha_Sincro.Value.ToString("yyyy-MM-dd") + "' and RUT.COMPANIA = '" + Login.empresa + "'  order by COD_ZON", con.conex);
            cm14.CommandTimeout = 0;
            SqlDataReader dr14 = cm14.ExecuteReader();
            

            while (dr14.Read())
            {
                ListViewItem lvItem7 = new ListViewItem();
                lvItem7.SubItems[0].Text = dr14[0].ToString();
                lvItem7.SubItems.Add(dr14[1].ToString());
                lvItem7.SubItems.Add(dr14[1].ToString());
                listView4.Items.Add(lvItem7);
            }
            dr14.Close();


            // Devoluciones  ---------------------------------------------------

            
            listView5.Clear();
            listView5.Columns.Add("Ruta", 40, HorizontalAlignment.Left);
            listView5.Columns.Add("Devolcion", 100, HorizontalAlignment.Left);

            SqlCommand cm15 = new SqlCommand("SELECT DEV.[COD_ZON],DEV.[NUM_DEV] FROM [EXACTUS].[ERPADMIN].[alFAC_ENC_DEV] as DEV LEFT JOIN ERPADMIN.RUTA_ASIGNADA_RT as RT on DEV.COD_ZON = RT.RUTA where RT.COMPANIA = '"+Login.empresa+"' AND NUM_DEV not in (SELECT DOCUMENTO FROM [EXACTUS].[" + Login.empresa + "].[DOCUMENTOS_CC]  where tipo = 'N/C' and FECHA_DOCUMENTO > DATEADD(DD, DATEDIFF(dd,72,GETDATE()),0) )   AND DATEADD(dd, 0, DATEDIFF(dd, 0, FEC_DEV ))='" + this.Fecha_Sincro.Value.ToString("yyyy-MM-dd") + "'", con.conex);
            cm15.CommandTimeout = 0;
            SqlDataReader dr15 = cm15.ExecuteReader();
           
           
            


            while (dr15.Read())
            {
                ListViewItem lvItem8 = new ListViewItem();
                lvItem8.SubItems[0].Text = dr15[0].ToString();
                lvItem8.SubItems.Add(dr15[1].ToString());
                lvItem8.SubItems.Add(dr15[1].ToString());
                listView5.Items.Add(lvItem8);
            }
            dr15.Close();





            con.Desconectar("EX");   

        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Carga el archivo TXT  del Proceso de carga HH --------------------------------------- 
        

            if (textBox9.Text == "")
                
        
            {
                MessageBox.Show("Ingrese Path del archivo Log");
            }
            else
            {
                log_carga_HH fm3 = new log_carga_HH();                
                fm3.ShowDialog();
                
            }
            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Abre una ventana para selecionar el Archivo TXT despues de haber sido cargado las HH --------------------------------------- 

           Selected_File =  string.Empty;
           this.textBox9.Clear();
           openFileDialog1.AutoUpgradeEnabled = false;
           openFileDialog1.InitialDirectory = @"\\192.168.1.25\c$\ExactusERP\Log_Carga_FR_HH";
            openFileDialog1.Title = "Select a File";
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.Filter = "Log Files|*.log| Text Files|*.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
               
            }
            else
            {
                Selected_File = openFileDialog1.FileName;
                this.textBox9.Text = Selected_File;

            }
        }


        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Abre el formulario que contiene las rutas que no han sincronizado --------------------------------------- 
            No_Sinc fm4 = new No_Sinc();
            fm4.ShowDialog();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox1.Text == "FRmcrear" || this.comboBox1.Text == "FRmcarga")
            {
                this.comboBox3.Text = "Mañana";
                time = "Mañana";

            }
            else
            {
                if (this.comboBox1.Text == "FRmdescarga")
                {
                    this.comboBox3.Text = "Tarde";
                    time = "Tarde";
                }
                else
                    
                {
                    time = "Todos";
                }
             }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            if (this.comboBox3.Text == "Mañana")
            {
                time = "Mañana";
            }
            else
                if (this.comboBox3.Text == "Tarde")
                {
                    time = "Tarde";
                }
                else if (this.comboBox3.Text == "Todos")
                {
                    time = "Todos";
                }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // --- Proceso que llama a  cargaFR-ERP ------------------

            validar_devoluciones();

             MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Desea Ejecuta el proceso de Carga ERP?", "CARGA FR-ERP", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
              


                
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = @"C:\SoftlandERP\CargaFR-ERP.exe";
                p.StartInfo.Arguments = ("sa D!sW0ML3.50 DISMO EXACTUS");
                p.Start();
            }

        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
            string Selected_File_carga;

            OpenFileDialog openFileDialog3 = new OpenFileDialog();

            openFileDialog3.AutoUpgradeEnabled = false;


            Selected_File_carga = string.Empty;
          
           string  usuario = SystemInformation.UserName;
           string direccion = @"C:\CORRECT\app";

            
            openFileDialog3.InitialDirectory = direccion;
            openFileDialog3.Title = "Select a File";
            openFileDialog3.FileName = string.Empty;
            openFileDialog3.Filter = "Log Files|*.Txt| Text Files|*.log";
            if (openFileDialog3.ShowDialog() == DialogResult.Cancel)
            {

            }
            else
            {
                Selected_File_carga = openFileDialog3.FileName;

                System.Diagnostics.Process.Start(@"Notepad.exe", Selected_File_carga);

                

            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label18.Text = DateTime.Now.ToLongTimeString();

            fechaup = DateTime.Now;

            if (fechaup.ToLongTimeString() == "4:15:05 PM")
            {
                consulta_task(null, null);
               
            }
            if (fechaup.ToLongTimeString() == "5:00:05 PM")
            {

                consulta_task(null, null);
            }

            if (fechaup.ToLongTimeString() == "6:00:05 PM")
            {
                consulta_task(null, null);
            }

            if (fechaup.ToLongTimeString() == "7:30:05 PM")
            {
                consulta_task(null, null);
            }
            if (fechaup.ToLongTimeString() == "8:30:05 PM")
            {
                consulta_task(null, null);
            }
            if (fechaup.ToLongTimeString() == "9:30:05 PM")
            {
                consulta_task(null, null);
            }

        }

        private void consulta_task(object sender, EventArgs e)
        {

            con.conectar("DM");

            SqlCommand cm1 = new SqlCommand("SELECT [Estado]  FROM [DM].[CORRECT].[Tareas_programada]  where Nombre_Tarea = 'Exactus Carga ERP'", con.condm);
            cm1.CommandTimeout = 0;
            SqlDataReader dr1 = cm1.ExecuteReader();
            while (dr1.Read())
            {
                stado = null;
                stado = Convert.ToString(dr1["Estado"]);

                switch (stado)
                {
                    case ("Finalizado"):
                        label28.ForeColor = Color.Green;
                        break;

                    case ("Pendiente"):
                        label28.ForeColor = Color.Orange;
                        break;

                    case ("Ejecutandoce"):
                        label28.ForeColor = Color.Red;
                        break;

                        
                }



                label28.Text = stado;
            }
            dr1.Close();

            SqlCommand cm2 = new SqlCommand("SELECT [Estado]  FROM [DM].[CORRECT].[Tareas_programada]  where Nombre_Tarea = 'Exactus Carga ERP 1'", con.condm);
            cm2.CommandTimeout = 0;
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
                stado = null;
                stado =  Convert.ToString(dr2["Estado"]);

                switch (stado)
                {
                    case ("Finalizado"):
                        label29.ForeColor = Color.Green;
                        break;

                    case ("Pendiente"):
                        label29.ForeColor = Color.Orange;
                        break;

                    case ("Ejecutandoce"):
                        label29.ForeColor = Color.Red;
                        break;


                }

                label29.Text = stado;
            }
            dr2.Close();

            SqlCommand cm3 = new SqlCommand("SELECT [Estado]  FROM [DM].[CORRECT].[Tareas_programada]  where Nombre_Tarea = 'Exactus Carga ERP 2'", con.condm);
            cm3.CommandTimeout = 0;
            SqlDataReader dr3 = cm3.ExecuteReader();
            while (dr3.Read())
            {
                stado = null;
                stado = Convert.ToString(dr3["Estado"]);
                switch (stado)
                {
                    case ("Finalizado"):
                        label30.ForeColor = Color.Green;
                        break;

                    case ("Pendiente"):
                        label30.ForeColor = Color.Orange;
                        break;

                    case ("Ejecutandoce"):
                        label30.ForeColor = Color.Red;
                        break;


                }

                label30.Text = stado;

            }
            dr3.Close();

            SqlCommand cm4 = new SqlCommand("SELECT [Estado]  FROM [DM].[CORRECT].[Tareas_programada]  where Nombre_Tarea = 'Exactus Carga ERP 3'", con.condm);
            cm4.CommandTimeout = 0;
            SqlDataReader dr4 = cm4.ExecuteReader();
            
            while (dr4.Read())
            {
                stado = null;
                stado = Convert.ToString(dr4["Estado"]);

                switch (stado)
                {
                    case ("Finalizado"):
                        label31.ForeColor = Color.Green;
                        break;

                    case ("Pendiente"):
                        label31.ForeColor = Color.Orange;
                        break;

                    case ("Ejecutandoce"):
                        label31.ForeColor = Color.Red;
                        break;


                }

                label31.Text = stado;





            }
            dr4.Close();

            SqlCommand cm5 = new SqlCommand("SELECT [Estado]  FROM [DM].[CORRECT].[Tareas_programada]  where Nombre_Tarea = 'Exactus Carga ERP 4'", con.condm);
            cm5.CommandTimeout = 0;
            SqlDataReader dr5 = cm5.ExecuteReader();
            
            while (dr5.Read())
            {
                stado = null;
                stado = Convert.ToString(dr5["Estado"]);

                switch (stado)
                {
                    case ("Finalizado"):
                        label32.ForeColor = Color.Green;
                        break;

                    case ("Pendiente"):
                        label32.ForeColor = Color.Orange;
                        break;

                    case ("Ejecutandoce"):
                        label32.ForeColor = Color.Red;
                        break;


                }

                label32.Text = stado;
            }
            dr5.Close();

            SqlCommand cm6 = new SqlCommand("SELECT [Estado]  FROM [DM].[CORRECT].[Tareas_programada]  where Nombre_Tarea = 'Exactus Carga ERP 5'", con.condm);
            cm6.CommandTimeout = 0;
            SqlDataReader dr6 = cm6.ExecuteReader();
            
            while (dr6.Read())
            {
                stado = null;
                stado =  Convert.ToString(dr6["Estado"]);

                switch (stado)
                {
                    case ("Finalizado"):
                        label33.ForeColor = Color.Green;
                        break;

                    case ("Pendiente"):
                        label33.ForeColor = Color.Orange;
                        break;

                    case ("Ejecutandoce"):
                        label33.ForeColor = Color.Red;
                        break;


                }
                label33.Text = stado;
            }
            dr6.Close();


            



            con.Desconectar("DM");

            string estado = "Ejecutandoce";

            if (Exists(estado))
            {
              this.timer2.Stop();
              this.timer2.Interval = (60000);
              this.timer2.Start();
            }
            else
            {
                this.timer2.Stop();
                this.timer2.Interval = (60000) * 10;
                this.timer2.Start();
            
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            consulta_task(null, null);
            button1_Click(null, null);
            //button4_Click(null, null);
        }



        private bool Exists(string estatus)
        {

            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("SELECT COUNT (*) FROM [DM].[CORRECT].[Tareas_programada]  where Estado = @estatus", con.condm);
            cmd.Parameters.AddWithValue("estatus", estatus);
            cmd.CommandTimeout = 0;
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

        private void button7_Click(object sender, EventArgs e)
        {
            string hoy = DateTime.Today.ToLongTimeString();

              MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Desea Actualzar los Descuentos y Promociones?", "Descuentos y Promociones", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                try
                {
                    con.conectar("DM");

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con.condm;
                    cmd.CommandText = "INSERT INTO [DM].[CORRECT].[Historico_Update_Des_y_Boni] ([USUARIO],[FECHA]) VALUES (@USUARIO,@FECHA)";
                    cmd.Parameters.Add("@USUARIO", SqlDbType.NVarChar).Value = Login.usuario.ToUpper();
                    cmd.Parameters.AddWithValue("@FECHA",Main_Menu.fechaup);
                    cmd.ExecuteNonQuery();                          
                    
                    
                    
                    SqlCommand sp = new SqlCommand("[CORRECT].[COPIA_DESC_ERP_A_FR]", con.condm);
                    sp.CommandType = CommandType.StoredProcedure;
                    sp.CommandTimeout = 0;
                        sp.ExecuteNonQuery();

                                                          


                    con.Desconectar("DM");
                    descuentos(null, null);

                    MessageBox.Show("Actualizacion Exitosa");
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Error al actualizar los Descuentos", ex.ToString());
                }

            }

        }
        private void descuentos(object sender, EventArgs e)
        {

            try
            {
                int desc = 0;
                con.conectar("EX");

                SqlCommand cm3 = new SqlCommand("SELECT  COUNT (CLIENTE) as 'Clientes' FROM [EXACTUS].[ERPADMIN].[DESCUENTO_CLIART] ", con.conex);
                cm3.CommandTimeout = 0;
                SqlDataReader dr3 = cm3.ExecuteReader();

                while (dr3.Read())
                {
                    label17.Text ="D:"+ Convert.ToString(dr3["Clientes"]);
                    boni = Convert.ToInt32(dr3["Clientes"]);
                }
                dr3.Close();




                SqlCommand cm4 = new SqlCommand("SELECT  COUNT (CLIENTE) as 'Clientes' FROM [EXACTUS].[ERPADMIN].[BONIFICACION_CLIART] ", con.conex);
                cm4.CommandTimeout = 0;
                SqlDataReader dr4 = cm4.ExecuteReader();
                
                while (dr4.Read())
                {
                    label34.Text = "B:" + Convert.ToString(dr4["Clientes"]);
                    desc = Convert.ToInt32(dr4["Clientes"]);
                }
                dr4.Close();


                con.Desconectar("EX");

             
            }

            catch
            {
                MessageBox.Show("conexion Error");
            }
            con.Desconectar("EX");

        
        }

        private void listView3_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

        private void listView3_DoubleClick(object sender, EventArgs e)
        {
          
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection indexes = this.listView1.SelectedIndices;
            foreach (int index in indexes)
            {
                PedidoN = this.listView1.Items[index].SubItems[1].Text;


            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (PedidoN != "" || PedidoN != null)
            {
                MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show("Desea Cambiar la fecha al Pedido  No. "+PedidoN+"?", "Cambio de Fechas Pedidos", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {


                    Cambio_fecha_Pedido cmb = new Cambio_fecha_Pedido();
                    cmb.Show();


                }
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }



        private void validar_devoluciones()
        {
            string ULT_PREFIJO = "";
            int nuevoval = 0;
            devoluciones.Clear();
            con.conectar("EX");
            SqlCommand cmdev = new SqlCommand("SELECT 'N'+RIGHT([COD_ZON],3) as PREFIJO,[NCF_PREFIJO],[NUM_DEV] FROM [EXACTUS].[ERPADMIN].[alFAC_ENC_DEV]  where NUM_DEV not in (SELECT  FACTURA FROM [EXACTUS].[" + Login.empresa + "].[FACTURA] where TIPO_DOCUMENTO = 'D' and FECHA_HORA > DATEADD(DD, DATEDIFF(dd,72,GETDATE()),0))   AND DATEADD(dd, 0, DATEDIFF(dd, 0, FEC_DEV )) >= '" + this.Fecha_Sincro.Value.ToString("yyyy-MM-dd") + "' and COD_ZON not like 'G%'", con.conex);
            cmdev.CommandTimeout = 0;
            SqlDataAdapter cd15 = new SqlDataAdapter(cmdev);
            cd15.Fill(devoluciones);

            for (int i = 0; i < devoluciones.Rows.Count; i++)
            {
                DataRow row = devoluciones.Rows[i];
                String PREFIJO = row["PREFIJO"].ToString();
               String NUN_REGALIA = row["NUM_DEV"].ToString();


               

                if (devoluciones.Rows.Count >= 1)
                {
                  
                    //if (DBNull.Value == row["NCF_PREFIJO"])
                    //{
                        con.conectar("EX");
                        SqlCommand CMNCF= new SqlCommand("SELECT [ULTIMO_VALOR]  FROM [EXACTUS].[" + Login.empresa + "].[NCF_CONSECUTIVO]  where PREFIJO = '" + PREFIJO+"'", con.conex);
                        CMNCF.CommandTimeout = 0;
                        SqlDataReader drcf = CMNCF.ExecuteReader();
                      
                        while (drcf.Read())
                        {

                            ULTIMONCF = Convert.ToString(drcf["ULTIMO_VALOR"]);
                        }

                        con.Desconectar("EX");
                    if (ULT_PREFIJO == PREFIJO)
                    {
                        ULTIMONCF = NCF;
                    }
                 
                    nuevoval = (Convert.ToInt32(ULTIMONCF)) + 1;
                    string nvalor = Convert.ToString(nuevoval);


                        int ceros = ULTIMONCF.Length - nvalor.Length;

                        switch (ceros)
                        {
                            case 1:
                                NCF = "0" + nuevoval;
                                break;
                            case 2:
                                NCF = "00" + nuevoval;
                                break;
                            case 3:
                                NCF = "000" + nuevoval;
                                break;
                            case 4:
                                NCF = "0000" + nuevoval;
                                break;
                            case 5:
                                NCF = "00000" + nuevoval;
                                break;
                            case 6:
                                NCF = "000000" + nuevoval;
                                break;
                            case 7:
                                NCF = "0000000" + nuevoval;
                                break;
                            case 8:
                                NCF = "00000000" + nuevoval;
                                break;
                            case 9:
                                NCF = "000000000" + nuevoval;
                                break;
                            case 10:
                                NCF = "0000000000" + nuevoval;
                                break;

                            case 11:
                                NCF = "0000000000" + nuevoval;
                                break;
                        }

                   

                        con.conectar("EX");
                        SqlCommand cmd = new SqlCommand("UPDATE dev SET dev.COD_PAIS = clie.COD_PAIS,dev.NCF_PREFIJO = '" + PREFIJO + "',dev.NCF = '" + NCF + "',dev.COD_GEO1 = 'Ninguna',dev.COD_GEO2 = 'Ninguna',SERIE_RESOLUCION = ''  FROM [EXACTUS].[ERPADMIN].[alFAC_ENC_DEV] dev  left join [EXACTUS].[ERPADMIN].[CLIENTE_CIA] clie  on dev.COD_CLT = clie.COD_CLT  where dev.NUM_DEV = '"+NUN_REGALIA+"'", con.conex);
                        cmd.CommandTimeout = 0;
                        cmd.ExecuteNonQuery();

                        con.Desconectar("EX");
                      ULT_PREFIJO = PREFIJO;

                    //}
                    //else
                    //{

                    //    con.conectar("EX");
                    //    SqlCommand cmd = new SqlCommand("UPDATE dev SET dev.COD_PAIS = clie.PAIS,dev.COD_GEO1 = 'Ninguna',dev.COD_GEO2 = 'Ninguna',SERIE_RESOLUCION = ''  FROM [EXACTUS].[ERPADMIN].[alFAC_ENC_DEV] dev  left join [EXACTUS].[dismo].[CLIENTE] clie  on dev.COD_CLT = clie.CLIENTE where dev.DOC_PRO is null", con.conex);
                    //    cmd.CommandTimeout = 0;
                    //    cmd.ExecuteNonQuery();

                    //    con.Desconectar("EX");

                    
                    //}


                }
              

            
            }








            con.Desconectar("EX");
        
        }

        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Nombre_user = "";


            if (treeView2.SelectedNode.Name == "USUARIO")
            {
                Nombre_user = treeView2.SelectedNode.Text;
            }
           
        }
    }
}
