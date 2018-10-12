using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraTreeList.Nodes;

namespace Sinconizacion_EXactus.CORECTX_APP.BODEGA.PROMOCIONES
{
    public partial class Promociones : Form
    {
        public Promociones()
        {
            InitializeComponent();
        }
        conexionXML con = new conexionXML();
        DataTable promocionesdt = new DataTable();
        DataTable promocionesas = new DataTable();
        DataTable promocionesdetalle = new DataTable();
        DataTable proveedor = new DataTable();

        DataTable NewboniEnc = new DataTable();
        DataTable NewboniDet = new DataTable();

        TreeNode nodeproveedor;
        ToolStripButton btupdate = new ToolStripButton();
        ToolStripButton btnuevo = new ToolStripButton();
        ToolStripButton btbuscar = new ToolStripButton();
        ToolStripButton btguardar = new ToolStripButton();
        ToolStripComboBox cbtipo = new ToolStripComboBox();
        ToolStripTextBox txt_clie = new ToolStripTextBox();
        DataTable tbfind = new DataTable();
        TreeNode nodefind;
        TreeNode nodefindt;
        private ContextMenu menugrid = new ContextMenu();
       
        Label comencmb = new Label();
        Label clientes= new Label();
        Label fecha_finlb = new Label();
        string menu_names;
        string tipo_asig;
        ToolStripControlHost cli;
        private string dragedItemText ;

        bool nuevo;
       


        private void Promociones_Load(object sender, EventArgs e)
        {
            btnuevo.Click += new EventHandler(NUEVO);
            cli  = new ToolStripControlHost(clientes);
            toolStripComboBox1.Enabled = false;
            btnuevo.Enabled = false;

            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;          
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ReadOnly = true;

            dataGridView1.AllowDrop = true;
            treeView1.AllowDrop = true;
            dragedItemText = "";
            cbtipo.SelectedIndexChanged += new EventHandler(change_cmb);

            dataGridView1.MouseDown += new  MouseEventHandler(dataGridView1_MouseDown);
            treeView1.DragEnter += new  DragEventHandler(treeView1_DragEnter);
            treeView1.DragDrop += new DragEventHandler(treeView1_DragDrop);

            MenuItem QUITAR = new MenuItem("QUITAR RUTA", new System.EventHandler(this.QUITARS));
            menugrid.MenuItems.Add(QUITAR);

            prepara_datos();

        }
        private void prepara_datos()
        {

            NewboniEnc.Columns.Add("NOMBRE", typeof(string));
            NewboniEnc.Columns.Add("PROVEEDOR", typeof(string));
            NewboniEnc.Columns.Add("FECHA_INI", typeof(string));
            NewboniEnc.Columns.Add("FECHA_FIN", typeof(string));
            NewboniEnc.Columns.Add("COMENTARIO", typeof(string));


            //NewboniEnc.Columns.Add("USUARIO_CREA", typeof(string));
            //NewboniEnc.Columns.Add("FECHA_CREA", typeof(string));
            //NewboniEnc.Columns.Add("USUARIO_UPDATE", typeof(string));
            //NewboniEnc.Columns.Add("FECHA_UPDATE", typeof(string));
            //NewboniEnc.Columns.Add("EMPRESA", typeof(string));
            //NewboniEnc.Columns.Add("COD_BON", typeof(string));





        }

        private void QUITARS(Object sender, System.EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Parent == null)
                {
                    if (treeView1.SelectedNode.Name != toolStripComboBox1.Text)
                    
                    treeView1.Nodes.Remove(treeView1.SelectedNode);
                }
                else
                {
                    treeView1.SelectedNode.Parent.Nodes.Remove(treeView1.SelectedNode);
                }
            }
        }

        private void NUEVO(Object sender, System.EventArgs e)
        {

            Nueva_bon Nue = new Nueva_bon("","");
            DialogResult res = Nue.ShowDialog();


            
            if (res == DialogResult.OK)
            {
                NewboniEnc.Clear();
                TreeNode destinationNode;
                destinationNode = treeView1.SelectedNode;
               TreeNode NewNode = new TreeNode();
                NewNode.Name = "NEW";
                NewNode.Text = Nueva_bon.Nombre;
                destinationNode.Nodes.Add(NewNode);

                NewboniEnc.Rows.Add(Nueva_bon.Nombre, destinationNode.Text, Nueva_bon.inicio, Nueva_bon.fin,Nueva_bon.comentario);
               
            }
        }
        private void GUARDAR(Object sender, System.EventArgs e)
        {
            if (NewboniEnc.Rows.Count >= 1)
            {
                string nombre = Convert.ToString(NewboniEnc.Rows[0][0]);


                //con.conectar("DM");
                //SqlCommand cmde = new SqlCommand("[CORRECT].[INSERT_PROMOCIONES]", con.condm);
                //cmde.CommandType = CommandType.StoredProcedure;
                //cmde.Parameters.AddWithValue("@NOMBRE", this.comboBox1.Text);
                //cmde.Parameters.AddWithValue("@PROVEEDOR", this.comboBox1.Text);
                //cmde.Parameters.AddWithValue("@FECHAINI", this.comboBox1.Text);
                //cmde.Parameters.AddWithValue("@FECHAFIN", this.comboBox1.Text);
                //cmde.Parameters.AddWithValue("@USUARIO", this.comboBox1.Text);
                //cmde.Parameters.AddWithValue("@EMPRESA", this.comboBox1.Text);
                //cmde.Parameters.AddWithValue("@COMENTARIO", this.comboBox1.Text);
                //cmde.Parameters.AddWithValue("@CORELATIVO", null);

                //cmde.ExecuteNonQuery();






                //con.conectar("DM");
                //SqlCommand cmd = new SqlCommand("[CORRECT].[INSERT_PROMOCIONES_DET]", con.condm);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@ARTICULO", this.comboBox1.Text);
                //cmd.Parameters.AddWithValue("@CANTIDADINI", this.comboBox1.Text);
                //cmd.Parameters.AddWithValue("@CANTIDADFIN", this.comboBox1.Text);
                //cmd.Parameters.AddWithValue("@FECHAINI", this.comboBox1.Text);
                //cmd.Parameters.AddWithValue("@FECHAFIN", this.comboBox1.Text);
                //cmd.Parameters.AddWithValue("@FACTOR", this.comboBox1.Text);
                //cmd.Parameters.AddWithValue("@ARTICULOBON", this.comboBox1.Text);
                //cmd.Parameters.AddWithValue("@UNIDADBON", this.comboBox1.Text);
                //cmd.Parameters.AddWithValue("@EMPRESA", this.comboBox1.Text);
                //cmd.Parameters.AddWithValue("@CORELATIVO", null);
                //cmd.ExecuteNonQuery();

                //con.Desconectar("DM");
            }
        }

        private void nuevo_boni_tb(string nombre)
        {


        }

        private void carga_elementos(string menu_name)
        {
            object O = global::Sinconizacion_EXactus.Properties.Resources.ResourceManager.GetObject("addfile_32x32");
            object e = global::Sinconizacion_EXactus.Properties.Resources.ResourceManager.GetObject("edit_32x32");
            object g = global::Sinconizacion_EXactus.Properties.Resources.ResourceManager.GetObject("guardar");

            toolStrip2.Items.Clear();

            if (menu_name == "Asignaciones")
            {
                
                comencmb.Text = "Item";
                toolStrip2.Items.Add(new ToolStripControlHost(comencmb));
                toolStrip2.Items.Add(cbtipo);

                btguardar.Text = "Guardar";
                btguardar.Image = (Image)g;
                toolStrip2.Items.Add(btguardar);
                btguardar.Enabled = false;
            }
            else
                if (menu_name == "Mantenimiento")
            {
                                
               
                btnuevo.Text = "Nuevo";
                btnuevo.Image = (Image)O;
                toolStrip2.Items.Add(btnuevo);


                btupdate.Text = "Editar";
                btupdate.Image = (Image)e;
                toolStrip2.Items.Add(btupdate);
                btupdate.Enabled = false;

                btguardar.Text = "Guardar";
                btguardar.Image = (Image)g;
                toolStrip2.Items.Add(btguardar);
                btguardar.Enabled = false;

                fecha_finlb.Text = "";
                toolStrip2.Items.Add(new ToolStripControlHost(fecha_finlb));

            }
        }
        private void barEditItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barEditItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barEditItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void navBarItem1_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            menu_names = "Mantenimiento";
            clear_grid();
            carga_promociones();

            if (proveedor.Rows.Count >= 1)
            {
               
                    treeView1.Nodes.Clear();                
              
                carga_treeview();
            }
            carga_elementos(menu_names);
            toolStripComboBox1.Enabled = false;


        }
        private void carga_promociones()
        {
            proveedor.Clear();
            promocionesdt.Clear();
                con.conectar("DM");

                SqlCommand cprob = new SqlCommand("SELECT [PROVEEDOR] ,[NOMBRE],[ALIAS] FROM[EXACTUS].[dismo].[PROVEEDOR] where RUBRO4_PROV <> 'Proveedor Servicios' and PROVEEDOR in (SELECT[PROVEEDOR]  FROM[DM].[CORRECT].[BONIFICACION_x_CLIE_ENC] group by PROVEEDOR)", con.condm);
                SqlDataAdapter dprob = new SqlDataAdapter(cprob);
                dprob.Fill(proveedor);

                SqlCommand cprm = new SqlCommand("SELECT [NOMBRE],[PROVEEDOR],[FECHA_INI],[FECHA_FIN],[USUARIO_CREA],[FECHA_CREA],[USUARIO_UPDATE],[FECHA_UPDATE],[EMPRESA],[COD_BON] FROM [DM].[CORRECT].[BONIFICACION_x_CLIE_ENC] ", con.condm);
                SqlDataAdapter dprm = new SqlDataAdapter(cprm);
                dprm.Fill(promocionesdt);


                con.Desconectar("DM");

            
        }
        private void carga_treeview()
        {
           
            for (int i = 0; i < proveedor.Rows.Count; i++)
            {
               // string menu_id;
                string proveedorid;
                proveedorid = proveedor.Rows[i]["PROVEEDOR"].ToString();

                nodeproveedor = new TreeNode(proveedor.Rows[i]["NOMBRE"].ToString());                
                nodeproveedor.Name = "PROVEEDOR";



                for (int o = 0; o < promocionesdt.Rows.Count; o++)
                {

                    string prov = promocionesdt.Rows[o]["PROVEEDOR"].ToString();

                    if (prov == proveedorid)
                    {
                        TreeNode nodebon;
                        DateTime fecha_cadu = DateTime.Now;
                        DateTime fecha_cpromo = Convert.ToDateTime(promocionesdt.Rows[o]["FECHA_FIN"]);


                        nodebon = new TreeNode(promocionesdt.Rows[o]["COD_BON"].ToString() + " - " + promocionesdt.Rows[o]["NOMBRE"].ToString());
                        nodebon.Name = "BONIFICACION";
                        if (fecha_cpromo >= fecha_cadu)
                        {
                            nodebon.ForeColor = Color.Black;
                        }
                        else
                        {
                            nodebon.ForeColor = Color.Gray;
                        }

                        nodeproveedor.Nodes.Add(nodebon);

                    }

                }




                    treeView1.Nodes.Add(nodeproveedor);
            }
           
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
           

                string node_name = Convert.ToString(treeView1.SelectedNode.Name);
                if (node_name == "BONIFICACION")
                {

                    string nombrenod = Convert.ToString(treeView1.SelectedNode.Text);
                    if (nombrenod.Length >= 8)
                    {
                        string cod = nombrenod.Substring(0, 8);
                        if (estado_promo(cod))
                        {
                            btupdate.Enabled = true;
                        }
                        else
                        {
                            btupdate.Enabled = false;
                        }
                        detalle_ofert(cod);
                    }

                }
                else if (node_name == "NEW")
                {
                    string nombrenod = Convert.ToString(treeView1.SelectedNode.Text);
                    detalle_ofert(nombrenod);
                    dataGridView1.ReadOnly = false;

                    //this.dgrid.Columns["colName"].ReadOnly = true;




                }
                if (node_name == "PROVEEDOR")
                {

                    btnuevo.Enabled = true;

                }
                else
                {
                    btnuevo.Enabled = false;
                }

            nuevo = false;

        }

        private bool estado_promo(string codigo)
        {
            DateTime hoy = DateTime.Now;
            bool activo =false;            
            var query = from p in promocionesdt.AsEnumerable()
                        where p.Field<string>("COD_BON") == codigo
                        select new
                        {
                          fecha = p.Field<DateTime>("FECHA_FIN")
                          
                        };


            foreach (var t in query)
            {
                if (t.fecha == null)
                {

                }
                else
                {
                    if (t.fecha >= hoy)
                    {
                        activo = true;
                    }
                    else
                    {
                        activo = false;
                    }


                }
            }

            return activo;
        }

        private void detalle_ofert(string cod)
        {
            promocionesdetalle.Clear();
            con.conectar("DM");

            SqlCommand sqlcmd = new SqlCommand("SELECT DISTINCT  DET.[ARTICULO] as CODIGO,ART.DESCRIPCION,[CANTIDAD_INICIAL] as 'CANT INICIAL',[CANTIDAD_FINAL] 'CANT FINAL',[FACTOR_BONIF] as 'FACTOR',[ARTICULO_BONIF] as 'COD REGALO',ARTR.DESCRIPCION  as 'DESC BONIFICAR',[UNIDADES_BONIF] as 'CANT REGALO',[FECHA_INI],[FECHA_FIN] FROM [DM].[CORRECT].[BONIFICACION_x_CLIE] as DET LEFT JOIN [DM].[CORRECT].[BONIFICACION_x_CLIE_ENC] as ENC on CAST(ENC.COD_BON as int)  = DET.CORR  LEFT JOIN [EXACTUS].[dismo].[ARTICULO] as ART on DET.ARTICULO = ART.ARTICULO  LEFT JOIN [EXACTUS].[dismo].[ARTICULO] as ARTR on DET.ARTICULO_BONIF = ARTR.ARTICULO  where enc.COD_BON = '" + cod+"' and DET.EMPRESA = 'DISMO'", con.condm);
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);

            da.Fill(promocionesdetalle);

            dataGridView1.DataSource = promocionesdetalle;
            dataGridView1.Refresh();

            con.Desconectar("DM");

        }
        private void ofertas_enc()
        {
            promocionesas.Clear();
            con.conectar("DM");

            SqlCommand sqlcmd = new SqlCommand("SELECT   [COD_BON] ,enc.[NOMBRE] as 'Promocion' ,pro.NOMBRE as 'Proveedor',[FECHA_INI],[FECHA_FIN] FROM [DM].[CORRECT].[BONIFICACION_x_CLIE_ENC] as enc LEFT JOIN [EXACTUS].[dismo].[PROVEEDOR] as pro on enc.proveedor = pro.PROVEEDOR where FECHA_FIN >= GETDATE()", con.condm);
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);

            da.Fill(promocionesas);

            dataGridView1.DataSource = promocionesas;

            con.Desconectar("DM");

        }

        private void navBarItem2_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            treeView1.Nodes.Clear();
            menu_names = "Asignaciones";
            clear_grid();
            carga_elementos(menu_names);
            toolStripComboBox1.Enabled = true;
            promocionesdetalle.Clear();

            ofertas_enc();

        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sqltext= "";
            string bon_det = "";
            tipo_asig = toolStripComboBox1.Text;

            switch (tipo_asig)
            {
                case "VENDEDOR":
                    comencmb.Text = "Vendedor:";
                     sqltext = "SELECT [VENDEDOR]  FROM [EXACTUS].[dismo].[VENDEDOR] where ACTIVO = 'S' and E_MAIL is not null order by VENDEDOR";
                    bon_det = "SELECT CL.VENDEDOR ,[U_NUM_BON],BFE.NOMBRE FROM [EXACTUS].[dismo].[BONIF_ART_X_CLI] as BF  LEFT JOIN [EXACTUS].[dismo].[CLIENTE]  as CL  on BF.CLIENTE = CL.CLIENTE  LEFT JOIN [DM].[CORRECT].[BONIFICACION_x_CLIE_ENC] as BFE  on BF.U_NUM_BON = CAST(BFE.COD_BON as int)  where BF.U_NUM_BON is not null   group by CL.VENDEDOR,BF.U_NUM_BON,BFE.NOMBRE";
                    break;
                case "AREA":
                    comencmb.Text = "Area:";
                    sqltext = "SELECT E_MAIL as AREA  FROM [EXACTUS].[dismo].[VENDEDOR] where  E_MAIL is not null group by E_MAIL";
                    break;
                case "CLIENTE":
                   
                    objetos_cliente_busqueda("add");
                    break;
                case "ZONA":
                    comencmb.Text = "Zona:";
                    sqltext = "SELECT  NOMBRE  FROM [EXACTUS].[dismo].[ZONA]  where NOMBRE <> 'ND'";
                    break;
            };
            if (sqltext != "")
            {

                string strControlVal = "CBuscarbt"; //"SalesToolStripMenuItem" or "invoiceToolStripMenuItem" in your case
                if (toolStrip1.Items.Count > 4)
                {
                    if (toolStrip1.Items[6].Name == strControlVal)
                    {
                        objetos_cliente_busqueda("remove");
                    }
                }

                cbtipo.Text = "";
                cbtipo.Items.Clear();

                con.conectar("EX");
                SqlCommand cm = new SqlCommand(sqltext, con.conex);
                SqlDataReader drc = cm.ExecuteReader();

                while (drc.Read())
                {
                    cbtipo.Items.Add(drc[0]);
                }
                drc.Close();

            con.Desconectar("EX");



                if (bon_det != "")
                {
                    DataTable det_bon = new DataTable();
                    DataTable det_bones = new DataTable();
                    con.conectar("EX");
                    SqlCommand cme = new SqlCommand(bon_det, con.conex);
                    SqlDataAdapter dae = new SqlDataAdapter(cme);
                    dae.Fill(det_bon);


                    SqlCommand cmd2 = new SqlCommand(sqltext, con.conex);
                    SqlDataAdapter dad2 = new SqlDataAdapter(cmd2);
                    dad2.Fill(det_bones);

                    con.Desconectar("EX");

                    find_treeview(det_bones,det_bon, tipo_asig);

                }
            }   
        }
        private void clear_grid()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();

        }

        private void objetos_cliente_busqueda(string accion)
        {
            cbtipo.Text = "";
            if (toolStrip1.Items.Count == 4)
            {
                object O = global::Sinconizacion_EXactus.Properties.Resources.ResourceManager.GetObject("lupa1");
                if (accion == "add")
                {
                    clientes.Text = "Codigo:";
                    toolStrip1.Items.Add(cli);

                    toolStrip1.Items.Add(txt_clie);

                    btbuscar.Text = "Buscar";
                    btbuscar.Name = "CBuscarbt";
                    btbuscar.Image = (Image)O;
                    toolStrip1.Items.Add(btbuscar);
                }

            }
            else
            {
                if (accion == "remove")
                {
                    toolStrip1.Items.Remove(cli);
                    toolStrip1.Items.Remove(txt_clie);
                    toolStrip1.Items.Remove(btbuscar);
                }
                
            }

        }

        private void change_cmb(object sender, EventArgs e)
        {
            carga_boni_por(toolStripComboBox1.Text, cbtipo.Text);
        }

        private void carga_boni_por(string busq_por, string valor)
        {
            tbfind.Clear();
            tbfind.Rows.Clear();
            treeView1.Nodes.Clear();
            string consulta_busq = "";
            switch (busq_por)
            {
                case "VENDEDOR":
                    consulta_busq = "SELECT CL.VENDEDOR ,[U_NUM_BON],BFE.NOMBRE FROM [EXACTUS].[dismo].[BONIF_ART_X_CLI] as BF  LEFT JOIN [EXACTUS].[dismo].[CLIENTE]  as CL  on BF.CLIENTE = CL.CLIENTE  LEFT JOIN [DM].[CORRECT].[BONIFICACION_x_CLIE_ENC] as BFE  on BF.U_NUM_BON = CAST(BFE.COD_BON as int)  where BF.U_NUM_BON is not null and CL.VENDEDOR = '"+valor+"'  group by CL.VENDEDOR,BF.U_NUM_BON,BFE.NOMBRE";
                    break;
                case "AREA":
                    consulta_busq = "SELECT ven.E_MAIL as AREA ,[U_NUM_BON],BFE.NOMBRE FROM [EXACTUS].[dismo].[BONIF_ART_X_CLI] as BF  LEFT JOIN [EXACTUS].[dismo].[CLIENTE]  as CL  on BF.CLIENTE = CL.CLIENTE LEFT JOIN [EXACTUS].[dismo].[VENDEDOR] as ven on CL.VENDEDOR = ven.VENDEDOR LEFT JOIN [DM].[CORRECT].[BONIFICACION_x_CLIE_ENC] as BFE  on BF.U_NUM_BON = CAST(BFE.COD_BON as int)  where BF.U_NUM_BON is not null and ven.E_MAIL = '"+valor+"'  group by BF.U_NUM_BON,BFE.NOMBRE,ven.E_MAIL";
                    break;
                case "ZONA":
                    consulta_busq = "SELECT zon.NOMBRE as AREA ,[U_NUM_BON],BFE.NOMBRE FROM [EXACTUS].[dismo].[BONIF_ART_X_CLI] as BF  LEFT JOIN [EXACTUS].[dismo].[CLIENTE]  as CL  on BF.CLIENTE = CL.CLIENTE LEFT JOIN [EXACTUS].[dismo].[ZONA] as zon on CL.ZONA = zon.ZONA LEFT JOIN [DM].[CORRECT].[BONIFICACION_x_CLIE_ENC] as BFE  on BF.U_NUM_BON = CAST(BFE.COD_BON as int)  where BF.U_NUM_BON is not null   group by BF.U_NUM_BON,BFE.NOMBRE,zon.NOMBRE";
                    break;

            }
            con.conectar("DM");
            SqlCommand cmb = new SqlCommand(consulta_busq, con.conex);
            SqlDataAdapter dap = new SqlDataAdapter(cmb);

            dap.Fill(tbfind);

            con.Desconectar("DM");
            if (tbfind.Rows.Count > 0)
            {
                DataTable tm = new DataTable();
                find_treeview(tbfind,tbfind, "NOMBRE");
            }

        }
        private void find_treeview(DataTable dts,DataTable dts2,string name )
        {
            string valor;
            if (dts.Rows.Count > 0)
            {
                treeView1.Nodes.Clear();
                for (int o = 0; o < dts.Rows.Count; o++)
                {
                    nodefind = new TreeNode(dts.Rows[o][0].ToString());
                    nodefind.Name = name;

                    
                    valor = dts.Rows[o][0].ToString();

                    for (int i = 0; i < dts2.Rows.Count; i++)
                    {
                        string evelu = dts2.Rows[i][0].ToString();

                        if (evelu == valor)
                        {
                            nodefindt = new TreeNode(dts2.Rows[i][2].ToString());
                            nodefindt.Name = "Detalle";

                            nodefind.Nodes.Add(nodefindt);
                        }

                    }

                    treeView1.Nodes.Add(nodefind);

                }
               
            }
            
        }

        private void bn_por()
        {


        }

        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            if (menu_names == "Asignaciones")
            {
                
                if (dragedItemText != "")
                {
                    Point pt;
                    TreeNode destinationNode;
                    pt = treeView1.PointToClient(new Point(e.X, e.Y));
                    destinationNode = treeView1.GetNodeAt(pt);
                    TreeNode dragedNode = new TreeNode();
                    dragedNode.Name = "Boni";
                    dragedNode.Text = dragedItemText;
                    

                    if (NodeExists(destinationNode, dragedItemText))
                    { }
                    else
                    {
                        if (destinationNode != null)
                        {
                            if (destinationNode.Name == "Boni")
                            {
                            }
                            else if (destinationNode.Name == "Detalle")
                            {

                            }
                            else
                            { 
                                destinationNode.Nodes.Add(dragedNode);
                            }
                        }
                    }

                    dragedItemText = "";

                }
            }
        }

        private bool NodeExists(TreeNode node, string key)
        {
            if (node != null)
            {
                foreach (TreeNode subNode in node.Nodes)
                {
                    if (subNode.Text == key)
                    {
                        return true;
                    }
                    //if (node.Nodes.Count > 0)
                    //{
                    //    NodeExists(node, key);
                    //}
                }
            }
           return false;
            
        }

        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            if (menu_names == "Asignaciones")
            {
                e.Effect = DragDropEffects.All;
             
            }
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (menu_names == "Asignaciones")
            {
                int idx;

                if (e.Button == MouseButtons.Left)
                {
                    idx = dataGridView1.CurrentRow.Index;

                    // dragedItemText = dataGridView1.CurrentCell.Value.ToString();
                    dragedItemText = Convert.ToString(dataGridView1.Rows[idx].Cells[1].Value);
                    dataGridView1.DoDragDrop(dragedItemText, DragDropEffects.Copy);


                }
            }
        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                if (menu_names == "Asignaciones")
                {

                    menugrid.Show(treeView1, new Point(e.X, e.Y));
                }
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            
            int idx = dataGridView1.CurrentRow.Index;
            int cidx = dataGridView1.Columns["FECHA_INI"].Index;
            int cidxf = dataGridView1.Columns["FECHA_FIN"].Index;
            int artidx = dataGridView1.Columns["CODIGO"].Index;
            int artdidx = dataGridView1.Columns["DESCRIPCION"].Index;
            int art_bonidx = dataGridView1.Columns["COD REGALO"].Index;
            int art_bondesidx = dataGridView1.Columns["DESC BONIFICAR"].Index;
            int cant_min = dataGridView1.Columns["CANT INICIAL"].Index;
            int cant_max = dataGridView1.Columns["CANT FINAL"].Index;
            int factor = dataGridView1.Columns["FACTOR"].Index;


            if (e.ColumnIndex == artidx)
            {
                string articulo_cod = Convert.ToString(dataGridView1.Rows[idx].Cells[0].Value);

                if (exit_art(articulo_cod))
                {
                    string fini = Convert.ToString(NewboniEnc.Rows[0][2]);
                    string ffin = Convert.ToString(NewboniEnc.Rows[0][3]);

                    dataGridView1.Rows[idx].Cells[cidx].Value = fini;
                    dataGridView1.Rows[idx].Cells[cidxf].Value = ffin;
                    dataGridView1.Rows[idx].Cells[artdidx].Value = nombre_art(articulo_cod);

                    this.dataGridView1.Columns["FECHA_INI"].ReadOnly = true;
                    this.dataGridView1.Columns["FECHA_FIN"].ReadOnly = true;
                   
                }
                else
                {
                    MessageBox.Show("NO existe Articulo");
                }
               dataGridView1.AllowUserToAddRows = false;
            }

            if (e.ColumnIndex == cant_min)
            {
                if (DBNull.Value == dataGridView1.Rows[idx].Cells[cant_min].Value)
                {
                    MessageBox.Show("Este Campo no puede quedar en blanco");
                    dataGridView1.CurrentCell = dataGridView1.Rows[idx].Cells[cant_min];
                  
                    
                }
                else
                {
                    double cantidad_minimia = Convert.ToDouble(dataGridView1.Rows[idx].Cells[cant_min].Value);

                    if (cant_min <= 0)
                    {
                        MessageBox.Show("El Valor de cantidad minima no puede ser 0");
                        dataGridView1.CurrentCell = dataGridView1.Rows[idx].Cells[cant_min];
                      

                    }
                    else
                    {
                        dataGridView1.Rows[idx].Cells[cant_max].Value = 999999;
                        
                    }
                }

            }
            if (e.ColumnIndex == cant_max)
            {
                if (DBNull.Value == dataGridView1.Rows[idx].Cells[cant_max].Value)
                {
                    MessageBox.Show("Este Campo no puede quedar en blanco");
                    dataGridView1.CurrentCell = dataGridView1.Rows[idx].Cells[cant_max];
                   
                }
                else
                {
                    double cantidad_minimia = Convert.ToDouble(dataGridView1.Rows[idx].Cells[cant_min].Value);
                    double cantidad_maxima = Convert.ToDouble(dataGridView1.Rows[idx].Cells[cant_max].Value);

                    if (cantidad_minimia > cantidad_maxima)
                    {
                        MessageBox.Show("La cantidad Minima no puede ser mayor que la maxima");
                        
                    }
                 
                }

                
            }

            if (e.ColumnIndex == factor)
            {
                if (DBNull.Value == dataGridView1.Rows[idx].Cells[factor].Value)
                {
                    MessageBox.Show("Este Campo no puede quedar en blanco");
                    
                }
             
            }
                if (e.ColumnIndex == art_bonidx)
            {
                string articulo_cods = Convert.ToString(dataGridView1.Rows[idx].Cells[art_bonidx].Value);
                if (exit_art(articulo_cods))
                {
                    dataGridView1.Rows[idx].Cells[art_bondesidx].Value = nombre_art(articulo_cods);
                }
                else
                {
                    MessageBox.Show("NO existe Articulo");

                }

            }
        }

        private bool exit_art(string codigo)
        {
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("SELECT COUNT([ARTICULO])  FROM [EXACTUS].[dismo].[ARTICULO] where ARTICULO = @ARTICULO", con.conex);
            cmd.Parameters.AddWithValue("@ARTICULO", codigo);


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

        private string nombre_art(string codigo)
        {
            string nombre = "";
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("SELECT DESCRIPCION  FROM [EXACTUS].[dismo].[ARTICULO] where ARTICULO = '"+codigo+"'", con.conex);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                nombre = Convert.ToString(dr[ "DESCRIPCION"]);
            }
            dr.Close();

            con.Desconectar("EX");

            return nombre;


        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
 

            if (e.KeyData == Keys.Enter)
            {
                if (dataGridView1.CurrentCell.ColumnIndex == dataGridView1.Columns["FECHA_FIN"].Index)
                {
                    if (ValidateGrid(dataGridView1))
                    {
                        MessageBox.Show("Existen Campos que deben ser llenados");
                    }
                    else
                    {
                        dataGridView1.AllowUserToAddRows = true;
                        e.SuppressKeyPress = true;
                        SendKeys.Send("{Tab}");
                        nuevo = true;
                        btguardar.Enabled = true;
                    }
                }
                else
                {

                    e.SuppressKeyPress = true;
                    SendKeys.Send("{Tab}");
                }
            }

        }

        private bool ValidateGrid(DataGridView dgvListas)
        {
            bool IsEmptyCell = false;
            for (int i = 0; i < dgvListas.RowCount ; i++)
            {
                for (int j = 0; j < dgvListas.ColumnCount; j++)
                {
                    if (dgvListas.Rows[i].Cells[j].Value == null || Convert.ToString(dgvListas.Rows[i].Cells[j].Value) == "")
                    {
                        IsEmptyCell = true;
                    }
                }
            }
            return IsEmptyCell;
        }

        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            
               
                //int iColumn = dataGridView1.CurrentCell.ColumnIndex;
                //int iRow = dataGridView1.CurrentCell.RowIndex;
                //if (iColumn == dataGridView1.ColumnCount - 1)
                //{
                //    if (dataGridView1.RowCount > (iRow + 1))
                //    {

                //        dataGridView1.CurrentCell = dataGridView1[1, iRow + 1];
                //    }
                //    else
                //    {
                //        //focus next control
                //    }
                //}
                //else
                //{
                //    dataGridView1.CurrentCell = dataGridView1[iColumn + 1, iRow];
                //}

           
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            

        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
           
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {
            string t = anError.Context.ToString();

            
            if (t == "Parsing, Commit")
            {
                MessageBox.Show("Error Dato Incorrecto Favor Validar");
            }
            if (anError.Context == DataGridViewDataErrorContexts.CurrentCellChange)
            {
                MessageBox.Show("Cell change");
            }
            if (anError.Context == DataGridViewDataErrorContexts.Parsing)
            {
                MessageBox.Show("parsing error");
            }
            if (anError.Context == DataGridViewDataErrorContexts.LeaveControl)
            {
                MessageBox.Show("leave control error");
            }

            if ((anError.Exception) is ConstraintException)
            {
                DataGridView view = (DataGridView)sender;
                view.Rows[anError.RowIndex].ErrorText = "an error";
                view.Rows[anError.RowIndex].Cells[anError.ColumnIndex].ErrorText = "an error";

                anError.ThrowException = false;
            }



        }

        private void groupControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (nuevo)
            {
                MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show("EXISTEN CAMBION PEDIENTES EN LA OFERTA: " + treeView1.SelectedNode.Text +"   DESEA GUARDAR LOS CAMBIOS", "CREACION DE OFERTAS", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {
                    GUARDAR(null, null);

                }
                else
                {
                    btguardar.Enabled = false;
                }

            }

        }
    }
    
}
