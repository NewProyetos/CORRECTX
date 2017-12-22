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
    public partial class Accesos : Form
    {
        public Accesos()
        {
            InitializeComponent();
        }

        conexionXML con = new conexionXML();
        //  Conexion2 coned = new Conexion2();
        //  conexion_master cnm = new conexion_master();
        DataTable menu = new DataTable();
        DataTable app = new DataTable();
        DataTable subapp = new DataTable();
        DataTable agencia = new DataTable();
        DataTable empresas = new DataTable();
        String menuapp_id;
        String app_id;
        String sub_app_id;
        String nombre_app;
        Int32 id_sucursal;
        Int32 id_empresa;
        Int32 id_empresa_acc;
        public static String ID_USUARIO;
        String Menu_ID;
        String fecha_actual;
        TreeNode node;
        TreeNode node1;
        TreeNode node2;
        TreeNode node3;
        TreeNode node4;
        TreeNode node5;
        TreeNode node6;
        TreeNode node7;
        TreeNode node8;
        TreeNode node9;
        TreeNode node10;
        TreeNode node11;
        TreeNode node12;
        TreeNode node13;
        TreeNode node14;
        TreeNode node15;
        TreeNode node16;
        public static int info;
        int existinfo;
        String nodoanterio;
        string Nombre;
        public static String USER;
        Int32 Menuacces;
        string DPTO_ID = "0";
        string ACCES = "1";
        //string USER_UPDATE = Login.usuario.ToUpper();
        string USER_UPDATE = "TURCIOSI";

        //ejecuta el metodo de validacion de acceso por usuario 
        Validar_Acceso vacc = new Validar_Acceso();
        private void Accesos_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            comboBox6.Enabled = false;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            carga_usuarios(null, null);
            Usuarios_nuevos(null, null);
            carga_Empresas(null, null);
            carga_Grupo(null, null);
            toolStripButton2.Enabled = false;
            carga_depto(null, null);

            groupBox4.Enabled = false;

            comboBox2.Enabled = false;
            comboBox3.Enabled = false;
            comboBox4.Enabled = false;
            comboBox5.Enabled = false;
            textBox1.Enabled = false;
            toolStripButton6.Enabled = false;
            toolStripButton3.Enabled = false;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = false;
            radioButton1.Checked = false;
            radioButton2.Checked = false;

            nodoanterio = "";

            con.conectar("DM");
            //coned.con.Open();
            SqlCommand cm1 = new SqlCommand("SELECT [USER_ID] ,[USUARIO] FROM [DM].[CORRECT].[USUARIOS] where USUARIO = '" + this.comboBox1.Text + "'", con.condm);
            SqlDataReader dr1 = cm1.ExecuteReader();
            while (dr1.Read())
            {
                ID_USUARIO = (dr1["USER_ID"].ToString());
                USER = (dr1["USUARIO"].ToString());
            }
            dr1.Close();

            SqlCommand cm2 = new SqlCommand("SELECT [NOMBRE]  FROM [EXACTUS].[ERPADMIN].[USUARIO] WHERE USUARIO in (SELECT [USUARIO]  FROM [DM].[CORRECT].[USUARIOS])  AND USUARIO = '" + this.comboBox1.Text + "'", con.condm);
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
                textBox2.Text = (dr2["NOMBRE"].ToString());

            }
            dr2.Close();



            con.Desconectar("DM");
            //coned.con.Close();

            exisinfo(ID_USUARIO);


            string nombre = comboBox1.Text;





            carga_depto_usuario(null, null);

            toolStripButton3.Enabled = true;

            label8.Text = nombre;
            groupBox5.Enabled = true;

            if (existinfo > 0)
            {
                linkLabel1.Text = "Mas Informacion";
            }
            else
            {
                linkLabel1.Text = "Este Usuario no tiene Informacion";
            }
            comboBox6.Enabled = true;

        }
        // valida los usuarios no existentes para CORRECT
        private void Usuarios_nuevos(object sender, EventArgs e)
        {

            con.conectar("MAS");
            SqlCommand cm2 = new SqlCommand("SELECT * FROM sys.server_principals where  name not like 'X%'and name not like '#%' and name not like 'NT%' and name not in (SELECT B.name FROM sys.server_principals B inner join [DM].[CORRECT].[USUARIOS]as A on B.name = A.USUARIO collate MODERN_SPANISH_CI_AS ) and  type_desc <> 'SERVER_ROLE' and type_desc <> 'WINDOWS_GROUP'", con.conmas);
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {

                toolStripComboBox1.Items.Add(dr2["name"]);

            }
            dr2.Close();

            con.Desconectar("MAS");
        }

        private void carga_usuarios(object sender, EventArgs e)
        {
            //coned.con.Open();
            con.conectar("DM");
            SqlCommand cm1 = new SqlCommand("SELECT  [USUARIO] FROM [DM].[CORRECT].[USUARIOS] order by USUARIO", con.condm);
            SqlDataReader dr1 = cm1.ExecuteReader();
            while (dr1.Read())
            {
                comboBox1.Items.Add(dr1["USUARIO"]);

            }
            dr1.Close();

            con.Desconectar("DM");
            //coned.con.Close();

        }

        private void carga_depto(object sender, EventArgs e)
        {
            if (comboBox2.Items.Count >= 1)
            {
                comboBox2.Items.Clear();
            }

            //coned.con.Open();
            con.conectar("DM");
            SqlCommand cm1 = new SqlCommand("SELECT [DEPARTAMENTO] FROM [DM].[CORRECT].[USUARIOS]  group by DEPARTAMENTO", con.condm);
            SqlDataReader dr1 = cm1.ExecuteReader();
            while (dr1.Read())
            {
                comboBox2.Items.Add(dr1["DEPARTAMENTO"]);

            }
            dr1.Close();

            con.Desconectar("DM");










            //coned.con.Close();


        }

        private void carga_depto_usuario(object sender, EventArgs e)
        {

            // coned.con.Open();
            con.conectar("DM");
            SqlCommand cm1 = new SqlCommand("SELECT [USUARIO],[DEPARTAMENTO],SUC.SUCURSAL,[PUESTO],[Principal_ID],[USERWEB],[TIPO_RRHH],[COD_EMPLEADO],EMP.NOMRE as EMPRESA,[LOGO_EMP] FROM [DM].[CORRECT].[USUARIOS] as USR LEFT JOIN [DM].[CORRECT].[EMPRESAS] as EMP ON USR.EMPRESA = EMP.ID LEFT JOIN [DM].[CORRECT].[SUCURSALES_EXATUS] as SUC ON USR.AGENCIA = SUC.ID_SUCURSAL where USUARIO = '" + comboBox1.Text + "'", con.condm);
            SqlDataReader dr1 = cm1.ExecuteReader();
            while (dr1.Read())
            {
                comboBox2.Text = Convert.ToString(dr1["DEPARTAMENTO"]);
                comboBox3.Text = Convert.ToString(dr1["SUCURSAL"]);
                comboBox4.Text = Convert.ToString(dr1["PUESTO"]);
                comboBox5.Text = Convert.ToString(dr1["EMPRESA"]);
                textBox1.Text = Convert.ToString(dr1["COD_EMPLEADO"]);


            }
            dr1.Close();

            con.Desconectar("DM");
            //coned.con.Close();


        }


        // carga el treeview 
        private void carga_menu(object sender, EventArgs e)
        {


            menu.Clear();
            app.Clear();
            subapp.Clear();
            treeView1.Nodes.Clear();


            // carga las datatable de cada una de los proramas existentes

            // tabla de menus 
            //coned.con.Open();
            con.conectar("DM");
            SqlCommand cm = new SqlCommand("SELECT [MENU_ID],[NOMBRE]FROM [DM].[CORRECT].[MENU] ", con.condm);
            SqlDataAdapter da = new SqlDataAdapter(cm);
            da.Fill(menu);

            //Tabalas de APP
            SqlCommand cm1 = new SqlCommand("SELECT [APP_ID],[MENU_ID],[NOMBRE]FROM [DM].[CORRECT].[MENU_APP]  ", con.condm);
            SqlDataAdapter da1 = new SqlDataAdapter(cm1);
            da1.Fill(app);

            // Tablas de SUB_APP
            SqlCommand cm2 = new SqlCommand("SELECT [SUBAPP_ID],[APP_ID],[NOMBRE] FROM [DM].[CORRECT].[MENU_SUB_APP]", con.condm);
            SqlDataAdapter da2 = new SqlDataAdapter(cm2);
            da2.Fill(subapp);
            con.Desconectar("DM");
            //coned.con.Close();





            for (int i = 0; i < menu.Rows.Count; i++)
            {
                string menu_id;
                string nombre_menu;
                menu_id = menu.Rows[i]["MENU_ID"].ToString();

                node = new TreeNode(menu.Rows[i]["NOMBRE"].ToString());
                nombre_menu = menu.Rows[i]["NOMBRE"].ToString();
                node.Name = "MENU";
                node.ForeColor = Color.Gray;
                //valida el acceso y si lo tiene  cambia de color el nodo
                for (int o = 0; o < vacc.Menu_Acces.Rows.Count; o++)
                {
                    string nombre_menu_acc = vacc.Menu_Acces.Rows[o]["NOMBRE"].ToString();
                    if (nombre_menu == nombre_menu_acc)
                    {
                        // establece color negro si  tiene acceso al menu
                        node.ForeColor = Color.Black;

                    }



                }



                // agregando apps al  nodo principal (MENU)
                switch (menu_id)
                {
                    case "1":
                        for (int o = 0; o < app.Rows.Count; o++)
                        {

                            menuapp_id = app.Rows[o]["MENU_ID"].ToString();
                            app_id = app.Rows[o]["APP_ID"].ToString();

                            if (menuapp_id == "1")
                            { // MENU INFORMATICA------
                                node1 = new TreeNode(app.Rows[o]["NOMBRE"].ToString());


                                string app_nombre = app.Rows[o]["NOMBRE"].ToString();

                                for (int j = 0; j < vacc.APP_acces.Rows.Count; j++)
                                {
                                    string nombre_app_acc = vacc.APP_acces.Rows[j]["NOMBRE"].ToString();
                                    //valida si la APP dentro de informatica  tiene Acceso
                                    if (app_nombre == nombre_app_acc)
                                    {
                                        // Agrega color a Negro al nodo  que contenga acceso
                                        node1.ForeColor = Color.Black;


                                    }

                                }

                                // nombre del NODO 1
                                node1.Name = "APP";

                                // valida si existe SUB_APP para agregarlos al nodo 1
                                if (Exist_subapp(app_id))
                                {
                                    for (int p = 0; p < subapp.Rows.Count; p++)
                                    {
                                        string sub_app;
                                        sub_app = subapp.Rows[p]["APP_ID"].ToString();

                                        // valida si la sub app estara dentro de la APP
                                        if (app_id == sub_app)
                                        {
                                            //agrea la sub_app al nodo 6
                                            node6 = new TreeNode(subapp.Rows[p]["NOMBRE"].ToString());
                                            node6.Name = "SUBAPP";

                                            string sub_app_nombre = subapp.Rows[p]["NOMBRE"].ToString();



                                            // validamos si tiene acceso a esta sub_app
                                            for (int k = 0; k < vacc.SUBAPP_acces.Rows.Count; k++)
                                            {
                                                string sub_nombre_app_acc = vacc.SUBAPP_acces.Rows[k]["NOMBRE"].ToString();
                                                if (sub_app_nombre == sub_nombre_app_acc)
                                                {
                                                    // establecemos el color negro al a sub_app con acceso
                                                    node6.ForeColor = Color.Black;


                                                }

                                            }
                                            // agregaramos el nodo6 = subapp al nodo 1 en este caso Informatica
                                            node1.Nodes.Add(node6);
                                        }
                                    }


                                }
                                // agregamos el nodo 1 Informatica al nodo princiapal 
                                node.Nodes.Add(node1);
                            }


                        }
                        break;

                    case "2":
                        for (int o = 0; o < app.Rows.Count; o++)
                        {
                            string menuapp_id;

                            menuapp_id = app.Rows[o]["MENU_ID"].ToString();
                            app_id = app.Rows[o]["APP_ID"].ToString();
                            if (menuapp_id == "2")
                            {
                                node2 = new TreeNode(app.Rows[o]["NOMBRE"].ToString());

                                string app_nombre = app.Rows[o]["NOMBRE"].ToString();

                                for (int j = 0; j < vacc.APP_acces.Rows.Count; j++)
                                {
                                    string nombre_app_acc = vacc.APP_acces.Rows[j]["NOMBRE"].ToString();
                                    if (app_nombre == nombre_app_acc)
                                    {
                                        node2.ForeColor = Color.Black;


                                    }

                                }


                                node2.Name = "APP";



                                if (Exist_subapp(app_id))
                                {
                                    for (int p = 0; p < subapp.Rows.Count; p++)
                                    {

                                        string sub_app;
                                        sub_app = subapp.Rows[p]["APP_ID"].ToString();
                                        if (app_id == sub_app)
                                        {
                                            node7 = new TreeNode(subapp.Rows[p]["NOMBRE"].ToString());
                                            node7.Name = "SUBAPP";


                                            string sub_app_nombre = subapp.Rows[p]["NOMBRE"].ToString();
                                            for (int k = 0; k < vacc.SUBAPP_acces.Rows.Count; k++)
                                            {
                                                string sub_nombre_app_acc = vacc.SUBAPP_acces.Rows[k]["NOMBRE"].ToString();
                                                if (sub_app_nombre == sub_nombre_app_acc)
                                                {
                                                    node7.ForeColor = Color.Black;


                                                }

                                            }

                                            node2.Nodes.Add(node7);


                                        }
                                    }


                                }
                                node.Nodes.Add(node2);
                            }

                        }
                        break;


                    case "3":
                        for (int o = 0; o < app.Rows.Count; o++)
                        {
                            string menuapp_id;

                            menuapp_id = app.Rows[o]["MENU_ID"].ToString();
                            app_id = app.Rows[o]["APP_ID"].ToString();
                            if (menuapp_id == "3")
                            {
                                node3 = new TreeNode(app.Rows[o]["NOMBRE"].ToString());

                                string app_nombre = app.Rows[o]["NOMBRE"].ToString();

                                for (int j = 0; j < vacc.APP_acces.Rows.Count; j++)
                                {
                                    string nombre_app_acc = vacc.APP_acces.Rows[j]["NOMBRE"].ToString();
                                    if (app_nombre == nombre_app_acc)
                                    {
                                        node3.ForeColor = Color.Black;


                                    }

                                }



                                node3.Name = "APP";
                                if (Exist_subapp(app_id))
                                {
                                    for (int p = 0; p < subapp.Rows.Count; p++)
                                    {
                                        string sub_app;
                                        sub_app = subapp.Rows[p]["APP_ID"].ToString();
                                        if (app_id == sub_app)
                                        {
                                            node8 = new TreeNode(subapp.Rows[p]["NOMBRE"].ToString());

                                            node8.Name = "SUBAPP";

                                            string sub_app_nombre = subapp.Rows[p]["NOMBRE"].ToString();
                                            for (int k = 0; k < vacc.SUBAPP_acces.Rows.Count; k++)
                                            {
                                                string sub_nombre_app_acc = vacc.SUBAPP_acces.Rows[k]["NOMBRE"].ToString();
                                                if (sub_app_nombre == sub_nombre_app_acc)
                                                {
                                                    node8.ForeColor = Color.Black;


                                                }

                                            }

                                            node3.Nodes.Add(node8);

                                        }
                                    }


                                }
                                node.Nodes.Add(node3);
                            }

                        }
                        break;
                    case "4":
                        for (int o = 0; o < app.Rows.Count; o++)
                        {
                            string menuapp_id;

                            menuapp_id = app.Rows[o]["MENU_ID"].ToString();
                            app_id = app.Rows[o]["APP_ID"].ToString();
                            if (menuapp_id == "4")
                            {
                                node4 = new TreeNode(app.Rows[o]["NOMBRE"].ToString());


                                string app_nombre = app.Rows[o]["NOMBRE"].ToString();

                                for (int j = 0; j < vacc.APP_acces.Rows.Count; j++)
                                {
                                    string nombre_app_acc = vacc.APP_acces.Rows[j]["NOMBRE"].ToString();
                                    if (app_nombre == nombre_app_acc)
                                    {
                                        node4.ForeColor = Color.Black;


                                    }

                                }

                                node4.Name = "APP";
                                if (Exist_subapp(app_id))
                                {
                                    for (int p = 0; p < subapp.Rows.Count; p++)
                                    {
                                        string sub_app;
                                        sub_app = subapp.Rows[p]["APP_ID"].ToString();
                                        if (app_id == sub_app)
                                        {
                                            node9 = new TreeNode(subapp.Rows[p]["NOMBRE"].ToString());
                                            node4.Nodes.Add(node9);
                                            node9.Name = "SUBAPP";

                                            string sub_app_nombre = subapp.Rows[p]["NOMBRE"].ToString();
                                            for (int k = 0; k < vacc.SUBAPP_acces.Rows.Count; k++)
                                            {
                                                string sub_nombre_app_acc = vacc.SUBAPP_acces.Rows[k]["NOMBRE"].ToString();
                                                if (sub_app_nombre == sub_nombre_app_acc)
                                                {
                                                    node9.ForeColor = Color.Black;


                                                }

                                            }
                                            node4.Nodes.Add(node9);


                                        }
                                    }


                                }
                                node.Nodes.Add(node4);
                            }

                        }
                        break;

                    case "5":
                        for (int o = 0; o < app.Rows.Count; o++)
                        {
                            string menuapp_id;

                            menuapp_id = app.Rows[o]["MENU_ID"].ToString();
                            app_id = app.Rows[o]["APP_ID"].ToString();
                            if (menuapp_id == "5")
                            {
                                node5 = new TreeNode(app.Rows[o]["NOMBRE"].ToString());


                                string app_nombre = app.Rows[o]["NOMBRE"].ToString();

                                for (int j = 0; j < vacc.APP_acces.Rows.Count; j++)
                                {
                                    string nombre_app_acc = vacc.APP_acces.Rows[j]["NOMBRE"].ToString();
                                    if (app_nombre == nombre_app_acc)
                                    {
                                        node5.ForeColor = Color.Black;


                                    }

                                }


                                node5.Name = "APP";



                                if (Exist_subapp(app_id))
                                {
                                    for (int p = 0; p < subapp.Rows.Count; p++)
                                    {
                                        string sub_app;
                                        sub_app = subapp.Rows[p]["APP_ID"].ToString();
                                        if (app_id == sub_app)
                                        {
                                            node10 = new TreeNode(subapp.Rows[p]["NOMBRE"].ToString());




                                            node10.Name = "SUBAPP";

                                            string sub_app_nombre = subapp.Rows[p]["NOMBRE"].ToString();
                                            for (int k = 0; k < vacc.SUBAPP_acces.Rows.Count; k++)
                                            {
                                                string sub_nombre_app_acc = vacc.SUBAPP_acces.Rows[k]["NOMBRE"].ToString();
                                                if (sub_app_nombre == sub_nombre_app_acc)
                                                {
                                                    node10.ForeColor = Color.Black;


                                                }

                                            }
                                            node5.Nodes.Add(node10);

                                        }
                                    }


                                }
                                node.Nodes.Add(node5);
                            }

                        }
                        break;



                    case "6":
                        for (int o = 0; o < app.Rows.Count; o++)
                        {
                            string menuapp_id;

                            menuapp_id = app.Rows[o]["MENU_ID"].ToString();
                            app_id = app.Rows[o]["APP_ID"].ToString();
                            if (menuapp_id == "6")
                            {
                                node6 = new TreeNode(app.Rows[o]["NOMBRE"].ToString());


                                string app_nombre = app.Rows[o]["NOMBRE"].ToString();

                                for (int j = 0; j < vacc.APP_acces.Rows.Count; j++)
                                {
                                    string nombre_app_acc = vacc.APP_acces.Rows[j]["NOMBRE"].ToString();
                                    if (app_nombre == nombre_app_acc)
                                    {
                                        node6.ForeColor = Color.Black;


                                    }

                                }


                                node6.Name = "APP";



                                if (Exist_subapp(app_id))
                                {
                                    for (int p = 0; p < subapp.Rows.Count; p++)
                                    {
                                        string sub_app;
                                        sub_app = subapp.Rows[p]["APP_ID"].ToString();
                                        if (app_id == sub_app)
                                        {
                                            node11 = new TreeNode(subapp.Rows[p]["NOMBRE"].ToString());




                                            node11.Name = "SUBAPP";

                                            string sub_app_nombre = subapp.Rows[p]["NOMBRE"].ToString();
                                            for (int k = 0; k < vacc.SUBAPP_acces.Rows.Count; k++)
                                            {
                                                string sub_nombre_app_acc = vacc.SUBAPP_acces.Rows[k]["NOMBRE"].ToString();
                                                if (sub_app_nombre == sub_nombre_app_acc)
                                                {
                                                    node11.ForeColor = Color.Black;


                                                }

                                            }
                                            node6.Nodes.Add(node11);

                                        }
                                    }


                                }
                                node.Nodes.Add(node6);
                            }

                        }

                        break;

                    case "7":
                        for (int o = 0; o < app.Rows.Count; o++)
                        {
                            string menuapp_id;

                            menuapp_id = app.Rows[o]["MENU_ID"].ToString();
                            app_id = app.Rows[o]["APP_ID"].ToString();
                            if (menuapp_id == "7")
                            {
                                node12 = new TreeNode(app.Rows[o]["NOMBRE"].ToString());


                                string app_nombre = app.Rows[o]["NOMBRE"].ToString();

                                for (int j = 0; j < vacc.APP_acces.Rows.Count; j++)
                                {
                                    string nombre_app_acc = vacc.APP_acces.Rows[j]["NOMBRE"].ToString();
                                    if (app_nombre == nombre_app_acc)
                                    {
                                        node12.ForeColor = Color.Black;


                                    }

                                }


                                node12.Name = "APP";



                                if (Exist_subapp(app_id))
                                {
                                    for (int p = 0; p < subapp.Rows.Count; p++)
                                    {
                                        string sub_app;
                                        sub_app = subapp.Rows[p]["APP_ID"].ToString();
                                        if (app_id == sub_app)
                                        {
                                            node13 = new TreeNode(subapp.Rows[p]["NOMBRE"].ToString());




                                            node13.Name = "SUBAPP";

                                            string sub_app_nombre = subapp.Rows[p]["NOMBRE"].ToString();
                                            for (int k = 0; k < vacc.SUBAPP_acces.Rows.Count; k++)
                                            {
                                                string sub_nombre_app_acc = vacc.SUBAPP_acces.Rows[k]["NOMBRE"].ToString();
                                                if (sub_app_nombre == sub_nombre_app_acc)
                                                {
                                                    node13.ForeColor = Color.Black;


                                                }

                                            }
                                            node12.Nodes.Add(node13);

                                        }
                                    }


                                }
                                node.Nodes.Add(node12);
                            }

                        }
                        break;


                    case "8":
                        for (int o = 0; o < app.Rows.Count; o++)
                        {
                            string menuapp_id;

                            menuapp_id = app.Rows[o]["MENU_ID"].ToString();
                            app_id = app.Rows[o]["APP_ID"].ToString();
                            if (menuapp_id == "8")
                            {
                                node14 = new TreeNode(app.Rows[o]["NOMBRE"].ToString());


                                string app_nombre = app.Rows[o]["NOMBRE"].ToString();

                                for (int j = 0; j < vacc.APP_acces.Rows.Count; j++)
                                {
                                    string nombre_app_acc = vacc.APP_acces.Rows[j]["NOMBRE"].ToString();
                                    if (app_nombre == nombre_app_acc)
                                    {
                                        node14.ForeColor = Color.Black;


                                    }

                                }


                                node14.Name = "APP";



                                if (Exist_subapp(app_id))
                                {
                                    for (int p = 0; p < subapp.Rows.Count; p++)
                                    {
                                        string sub_app;
                                        sub_app = subapp.Rows[p]["APP_ID"].ToString();
                                        if (app_id == sub_app)
                                        {
                                            node15 = new TreeNode(subapp.Rows[p]["NOMBRE"].ToString());




                                            node15.Name = "SUBAPP";

                                            string sub_app_nombre = subapp.Rows[p]["NOMBRE"].ToString();
                                            for (int k = 0; k < vacc.SUBAPP_acces.Rows.Count; k++)
                                            {
                                                string sub_nombre_app_acc = vacc.SUBAPP_acces.Rows[k]["NOMBRE"].ToString();
                                                if (sub_app_nombre == sub_nombre_app_acc)
                                                {
                                                    node15.ForeColor = Color.Black;


                                                }

                                            }
                                            node14.Nodes.Add(node15);

                                        }
                                    }


                                }
                                node.Nodes.Add(node14);
                            }

                        }
                        break;


                }



                treeView1.Nodes.Add(node);


            }



        }


        //se ejecuta al hacer click en el nodo seleccionado
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {



            //radioButton2.Checked = true;
            // radioButton1.Enabled = true;
            nombre_app = treeView1.SelectedNode.Text;





            string nombrenod = Convert.ToString(treeView1.SelectedNode.Name);



            int ac = vacc.Menu_Acces.Rows.Count;



            if (treeView1.SelectedNode.ForeColor == Color.Black)
            {

                radioButton1.Enabled = false;
                radioButton2.Enabled = true;
                radioButton2.Checked = true;

                button1.Enabled = true;

            }


            else
            {

                radioButton1.Enabled = true;
                radioButton2.Enabled = false;
                radioButton2.Checked = false;
                button1.Enabled = true;
            }






            ID_APP(nombrenod);
            Nombre = treeView1.SelectedNode.Name;



        }
        // valida si exite una SubAPP para agregarla al Treeview
        private bool Exist_subapp(string sub_app)
        {
            //coned.con.Open();
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("select COUNT (*) FROM [DM].[CORRECT].[MENU_SUB_APP] where APP_ID = @sub_app  ", con.condm);
            cmd.Parameters.AddWithValue("sub_app", sub_app);

            int contar = Convert.ToInt32(cmd.ExecuteScalar());
            //coned.con.Close();
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
        // valida si tiene acceso en el main menu 
        private bool Exist_acc_main_menu(string menu_id, int empre)
        {
            //coned.con.Open();
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("select COUNT (*) FROM [DM].[CORRECT].[ACCES_MAIN] where MENU_ID = @menu_id and ID_USER = '" + ID_USUARIO + "' and ID_EMPRESA = '" + empre + "' ", con.condm);
            cmd.Parameters.AddWithValue("menu_id", menu_id);

            int contar = Convert.ToInt32(cmd.ExecuteScalar());
            //coned.con.Close();
            con.Desconectar("DM");
            if (contar == 0)
            {
                return true;

            }
            else
            {
                return false;

            }

        }
        // valido si existe acceso a un app
        private bool Exist_acc_app(string app_id)
        {
            //coned.con.Open();
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("select COUNT (*) FROM [DM].[CORRECT].[ACCESO_APP] where APP_ID = @app_id and ID_USER = '" + ID_USUARIO + "' ", con.condm);
            cmd.Parameters.AddWithValue("app_id", app_id);

            int contar = Convert.ToInt32(cmd.ExecuteScalar());
            //coned.con.Close();
            con.Desconectar("DM");
            if (contar == 0)
            {
                return true;

            }
            else
            {
                return false;

            }

        }
        // establece el color azul en el nodo a modificar
        private void color_node(object sender, EventArgs e)
        {
            for (int i = 0; i < menu.Rows.Count; i++)
            {
                string nombre_menus;
                nombre_menus = menu.Rows[i]["NOMBRE"].ToString();

                for (int o = 0; o < vacc.Menu_Acces.Rows.Count; o++)
                {
                    string nombre_menu_acc = vacc.Menu_Acces.Rows[o]["NOMBRE"].ToString();

                    if (nombre_menus == nombre_menu_acc)
                    {
                        node.ForeColor = Color.Blue;
                        nombre_menu_acc = "";
                    }

                }

            }


        }

        private void button1_Click(object sender, EventArgs e)
        {





            if (radioButton1.Checked)
            {
                if (valida_empres_acceso(id_empresa_acc, ID_USUARIO))
                {
                    add_acceso_empresa(id_empresa_acc, ID_USUARIO);
                }


                if (Nombre == "MENU")
                {
                    if (Exist_acc_main_menu(Menu_ID, id_empresa_acc))
                    {
                        access_menu();
                    }
                    else
                    {

                        MessageBox.Show("YA TIENE ACCESO");
                    }
                }


                if (Nombre == "APP")
                {
                    if (Exist_acc_main_menu(Menu_ID, id_empresa_acc))
                    {
                        access_menu();
                        access_APP();
                    }
                    else
                    {


                        access_APP();

                    }

                }
                if (Nombre == "SUBAPP")
                {

                    if (Exist_acc_main_menu(Menu_ID, id_empresa_acc))
                    {
                        access_menu();

                    }


                    if (Exist_acc_app(app_id))
                    {
                        access_APP();
                        access_SUB_APP();
                    }
                    else
                    {
                        access_SUB_APP();

                    }



                }

                comboBox6_SelectedIndexChanged(null, null);


            }

            if (radioButton2.Checked)
            {
                int app_conacc = vacc.APP_acces.Rows.Count;
                int subapp_conacc = vacc.SUBAPP_acces.Rows.Count;

                if (Nombre == "MENU")
                {

                    if (app_conacc > 0)
                        quita_app_menu();



                    if (subapp_conacc > 0)
                    {
                        quitar_subapp_menu();

                    }

                    denegar_menu();


                }

                else if (Nombre == "APP")
                {

                    if (subapp_conacc > 0)
                    {
                        //coned.con.Open();
                        con.conectar("DM");
                        SqlCommand cmd1 = new SqlCommand("DELETE subacpp FROM [DM].[CORRECT].[ACCESO_SUBAPP] subacpp  LEFT JOIN [DM].[CORRECT].[MENU_SUB_APP]subapp  ON subacpp.SUBAPP_ID = subapp.SUBAPP_ID  LEFT JOIN  [DM].[CORRECT].[MENU_APP] App  ON subapp.APP_ID = App.APP_ID where App.APP_ID= '" + app_id + "' and subacpp.ID_USER = '" + ID_USUARIO + "' and ID_EMPRESA = '" + id_empresa_acc + "'", con.condm);

                        cmd1.ExecuteNonQuery();

                        //coned.con.Close();
                        con.Desconectar("DM");

                    }

                    denegar_app();



                }
                else if (Nombre == "SUBAPP")
                {
                    //coned.con.Open();
                    con.conectar("DM");
                    SqlCommand cmd1 = new SqlCommand("DELETE [DM].[CORRECT].[ACCESO_SUBAPP]  where ID_USER = '" + ID_USUARIO + "' and   SUBAPP_ID  = '" + sub_app_id + "' and ID_EMPRESA = '" + id_empresa_acc + "'", con.condm);
                    cmd1.ExecuteNonQuery();
                    con.Desconectar("DM");
                    //coned.con.Close();



                }



                comboBox6_SelectedIndexChanged(null, null);
            }
        }
        // obtiene el ID de la APP solicitada
        private void ID_APP(string nombre)
        {
            switch (nombre)
            {
                case "MENU":
                    //coned.con.Open();
                    con.conectar("DM");
                    SqlCommand cm1 = new SqlCommand("SELECT [MENU_ID] FROM [DM].[CORRECT].[MENU] where NOMBRE = '" + nombre_app + "' ", con.condm);
                    SqlDataReader dr1 = cm1.ExecuteReader();
                    while (dr1.Read())
                    {
                        Menu_ID = (dr1["MENU_ID"].ToString());

                    }
                    dr1.Close();




                    break;

                case "APP":
                    //coned.con.Open();
                    con.conectar("DM");
                    SqlCommand cm2 = new SqlCommand("SELECT [APP_ID],[MENU_ID] FROM [DM].[CORRECT].[MENU_APP] where NOMBRE = '" + nombre_app + "'", con.condm);
                    SqlDataReader dr2 = cm2.ExecuteReader();
                    while (dr2.Read())
                    {
                        app_id = (dr2["APP_ID"].ToString());
                        Menu_ID = (dr2["MENU_ID"].ToString());

                    }
                    dr2.Close();




                    break;
                case "SUBAPP":

                    //coned.con.Open();
                    con.conectar("DM");
                    SqlCommand cm3 = new SqlCommand("SELECT A.[SUBAPP_ID],A.[APP_ID],B.MENU_ID,A.[NOMBRE] FROM [DM].[CORRECT].[MENU_SUB_APP] A   INNER JOIN  [DM].[CORRECT].[MENU_APP] B   ON A.APP_ID = B.APP_ID where A.NOMBRE = '" + nombre_app + "'", con.condm);
                    SqlDataReader dr3 = cm3.ExecuteReader();
                    while (dr3.Read())
                    {
                        sub_app_id = (dr3["SUBAPP_ID"].ToString());
                        app_id = (dr3["APP_ID"].ToString());
                        Menu_ID = (dr3["MENU_ID"].ToString());


                    }
                    dr3.Close();


                    break;
            }
            con.Desconectar("DM");
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

            nodoanterio = treeView1.SelectedNode.Text;

            if (Nombre == "" || Nombre == null)
            {
                button1.Enabled = true;
            }

            else if (radioButton1.Checked)
            {
                treeView1.SelectedNode.ForeColor = Color.Blue;
                button1.Enabled = true;
            }







        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {


        }

        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {


        }

        // inserta acceso a la tabla menu
        private void access_menu()
        {

            //coned.con.Open();
            con.conectar("DM");
            fecha_actual = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con.condm;
            cmd.CommandText = "INSERT INTO [DM].[CORRECT].[ACCES_MAIN] (ID_USER,DPTO_ID,MENU_ID,ACCES,Update_date,User_Update,ID_EMPRESA)values(@ID_USER,@DPTO_ID,@MENU_ID,@ACCES,@Update_date,@User_Update,@ID_EMPRESA)";
            cmd.Parameters.Add("@ID_USER", SqlDbType.NVarChar).Value = ID_USUARIO;
            cmd.Parameters.Add("@DPTO_ID", SqlDbType.NVarChar).Value = DPTO_ID;
            cmd.Parameters.Add("@MENU_ID", SqlDbType.NVarChar).Value = Menu_ID;
            cmd.Parameters.Add("@ACCES", SqlDbType.NVarChar).Value = ACCES;
            cmd.Parameters.Add("@Update_date", SqlDbType.NVarChar).Value = fecha_actual;
            cmd.Parameters.Add("@User_Update", SqlDbType.NVarChar).Value = USER_UPDATE;
            cmd.Parameters.Add("@ID_EMPRESA", SqlDbType.NVarChar).Value = id_empresa_acc;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            con.Desconectar("DM");

            comboBox6_SelectedIndexChanged(null, null);


        }
        // inserta acceso a la tabla APP
        private void access_APP()
        {
            //coned.con.Open();
            con.conectar("DM");
            fecha_actual = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con.condm;
            cmd.CommandText = "INSERT INTO [DM].[CORRECT].[ACCESO_APP] (ID_USER,APP_ID,ACCCESS,Update_date,User_Update,ID_EMPRESA)values(@ID_USER,@APP_ID,@ACCCESS,@Update_date,@User_Update,@ID_EMPRESA)";
            cmd.Parameters.Add("@ID_USER", SqlDbType.NVarChar).Value = ID_USUARIO;
            cmd.Parameters.Add("@APP_ID", SqlDbType.NVarChar).Value = app_id;
            cmd.Parameters.Add("@ACCCESS", SqlDbType.NVarChar).Value = ACCES;
            cmd.Parameters.Add("@Update_date", SqlDbType.NVarChar).Value = fecha_actual;
            cmd.Parameters.Add("@User_Update", SqlDbType.NVarChar).Value = USER_UPDATE;
            cmd.Parameters.Add("@ID_EMPRESA", SqlDbType.NVarChar).Value = id_empresa_acc;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            con.Desconectar("DM");
            comboBox6_SelectedIndexChanged(null, null);

        }
        // inserta acceso a la tabla  SUBAPP
        private void access_SUB_APP()
        {

            //coned.con.Open();
            con.conectar("DM");
            fecha_actual = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con.condm;
            cmd.CommandText = "INSERT INTO [DM].[CORRECT].[ACCESO_SUBAPP] (ID_USER,SUBAPP_ID,ACCESS,Update_date,User_Update,ID_EMPRESA)values(@ID_USER,@SUBAPP_ID,@ACCESS,@Update_date,@User_Update,@ID_EMPRESA)";
            cmd.Parameters.Add("@ID_USER", SqlDbType.NVarChar).Value = ID_USUARIO;
            cmd.Parameters.Add("@SUBAPP_ID", SqlDbType.NVarChar).Value = sub_app_id;
            cmd.Parameters.Add("@ACCESS", SqlDbType.NVarChar).Value = ACCES;
            cmd.Parameters.Add("@Update_date", SqlDbType.NVarChar).Value = fecha_actual;
            cmd.Parameters.Add("@User_Update", SqlDbType.NVarChar).Value = USER_UPDATE;
            cmd.Parameters.Add("@ID_EMPRESA", SqlDbType.NVarChar).Value = id_empresa_acc;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            con.Desconectar("DM");
            comboBox6_SelectedIndexChanged(null, null);


        }

        private void treeView1_Click(object sender, EventArgs e)
        {

        }
        // elimina  registro de acceso de la tabla menu
        private void denegar_menu()
        {


            //coned.con.Open();
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("DELETE FROM  [DM].[CORRECT].[ACCES_MAIN]  WHERE ID_USER = '" + ID_USUARIO + "' and MENU_ID ='" + Menu_ID + "' and ID_EMPRESA = '" + id_empresa_acc + "' ", con.condm);

            cmd.ExecuteNonQuery();

            //coned.con.Close();
            con.Desconectar("DM");

        }

        private void denegar_app()
        {

            //coned.con.Open();
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("DELETE FROM  [DM].[CORRECT].[ACCESO_APP]  WHERE ID_USER = '" + ID_USUARIO + "' and APP_ID ='" + app_id + "' and ID_EMPRESA = '" + id_empresa_acc + "'", con.condm);

            cmd.ExecuteNonQuery();

            //coned.con.Close();
            con.Desconectar("DM");

        }


        // quita los accesos de las app al quitar el menu principal
        private void quita_app_menu()
        {
            //coned.con.Open();
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("DELETE acpp  FROM [DM].[CORRECT].[ACCESO_APP] acpp  LEFT JOIN  [DM].[CORRECT].[MENU_APP] App  ON acpp.APP_ID = App.APP_ID  LEFT join [DM].[CORRECT].[ACCES_MAIN] menu  on App.MENU_ID = menu.MENU_ID  where App.MENU_ID = '" + Menu_ID + "' and acpp.ID_USER = '" + ID_USUARIO + "' and acpp.ID_EMPRESA = '" + id_empresa_acc + "'", con.condm);
            cmd.Connection = con.condm;
            cmd.ExecuteNonQuery();
            //coned.con.Close();
            con.Desconectar("DM");
        }

        // quita los accesos de las sub-app al quitar el menu principal
        private void quitar_subapp_menu()
        {
            //coned.con.Open();
            con.conectar("DM");
            SqlCommand cmd1 = new SqlCommand("DELETE subacpp FROM [DM].[CORRECT].[ACCESO_SUBAPP] subacpp  LEFT JOIN [DM].[CORRECT].[MENU_SUB_APP]subapp  ON subacpp.SUBAPP_ID = subapp.SUBAPP_ID  LEFT JOIN  [DM].[CORRECT].[MENU_APP] App  ON subapp.APP_ID = App.APP_ID  LEFT join [DM].[CORRECT].[ACCES_MAIN] menu  on App.MENU_ID = menu.MENU_ID  where menu.MENU_ID= '" + Menu_ID + "' and subacpp.ID_USER = '" + ID_USUARIO + "' and subacpp.ID_EMPRESA = '" + id_empresa_acc + "'", con.condm);

            cmd1.ExecuteNonQuery();

            //coned.con.Close();
            con.Desconectar("DM");

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "")
            {
                MessageBox.Show("Seleccione un Departamento");
                comboBox2.Focus();
            }

            else if (comboBox3.Text == "")
            {
                MessageBox.Show("Seleccione una Agencia");
                comboBox3.Focus();
            }

            else if (comboBox4.Text == "")
            {
                MessageBox.Show("Seleccione un Grupo");
                comboBox4.Focus();
            }

            else if (comboBox5.Text == "")
            {
                MessageBox.Show("Seleccione una Empresa");
                comboBox5.Focus();
            }
            else if (textBox1.Text == "")
            {
                MessageBox.Show("Ingrese el Codigo de empleado");
                textBox1.Focus();
            }



            else
            {

                //coned.con.Open();
                con.conectar("DM");

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con.condm;
                cmd.CommandText = "INSERT INTO [DM].[CORRECT].[USUARIOS] (USUARIO,DEPARTAMENTO,Principal_ID,PUESTO,COD_EMPLEADO,EMPRESA,AGENCIA,LOGO_EMP)values(@USUARIO,@DEPARTAMENTO,@Principal_ID,@PUESTO,@COD_EMPLEADO,@EMPRESA,@AGENCIA,@LOGO_EMP)";
                cmd.Parameters.Add("@USUARIO", SqlDbType.NVarChar).Value = toolStripComboBox1.Text;
                cmd.Parameters.Add("@DEPARTAMENTO", SqlDbType.NVarChar).Value = comboBox2.Text;
                cmd.Parameters.Add("@PUESTO", SqlDbType.NVarChar).Value = comboBox4.Text;
                cmd.Parameters.Add("@COD_EMPLEADO", SqlDbType.NVarChar).Value = textBox1.Text;
                cmd.Parameters.Add("@EMPRESA", SqlDbType.NVarChar).Value = id_empresa;
                cmd.Parameters.Add("@AGENCIA", SqlDbType.Int).Value = id_sucursal;
                cmd.Parameters.Add("@Principal_ID", SqlDbType.Int).Value = UserIDsystem(toolStripComboBox1.Text);

                switch (comboBox5.Text)
                {

                    case "DISTRIBUIDORA MORAZAN, S.A. DE C.V":
                        cmd.Parameters.Add("@LOGO_EMP", SqlDbType.NVarChar).Value = "1";
                        break;

                    case "LESA, S.A. DE C.V":
                        cmd.Parameters.Add("@LOGO_EMP", SqlDbType.NVarChar).Value = "2";
                        break;
                    case "HAMBURGO,S.A. DE C.V":
                        cmd.Parameters.Add("@LOGO_EMP", SqlDbType.NVarChar).Value = "3";
                        break;
                    case "CORPORACION SIETE, S.A. DE C.V":
                        cmd.Parameters.Add("@LOGO_EMP", SqlDbType.NVarChar).Value = "4";
                        break;
                    case "ZEGNA, S.A. DE C.V":
                        cmd.Parameters.Add("@LOGO_EMP", SqlDbType.NVarChar).Value = "5";
                        break;
                    case "CV+, S.A. DE C.V":
                        cmd.Parameters.Add("@LOGO_EMP", SqlDbType.NVarChar).Value = "6";
                        break;

                    default:
                        cmd.Parameters.Add("@LOGO_EMP", SqlDbType.NVarChar).Value = "1";
                        break;



                }


                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                //coned.con.Close();
                con.Desconectar("DM");

                MessageBox.Show("Usuario " + toolStripComboBox1.Text + " INGRESADO CORRECTAMENTE");
                comboBox1.Text = toolStripComboBox1.Text;
                toolStripComboBox1.Text = "";
                //Accesos_Load(null, null);
                toolStripComboBox1.Items.Clear();
                comboBox1.Items.Clear();
                comboBox2.Items.Clear();
                Usuarios_nuevos(null, null);
                carga_usuarios(null, null);
                comboBox1.Enabled = true;

                //carga_depto(null, null);
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
                comboBox4.Enabled = false;
                comboBox5.Enabled = false;
                textBox1.Enabled = false;
                comboBox1_SelectedIndexChanged(null, null);

            }
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Enabled = true;

            carga_depto(null, null);
            comboBox2.Text = "";



            if (toolStripComboBox1.Text != "")
            {
                toolStripButton2.Enabled = true;
            }

        }

        private int UserIDsystem(string Usuario)
        {
            int login_ID = 0;

            con.conectar("MAS");
            SqlCommand cm1 = new SqlCommand("SELECT principal_id FROM sys.server_principals where name = '" + Usuario + "'", con.conmas);
            SqlDataReader dr1 = cm1.ExecuteReader();
            while (dr1.Read())
            {
                login_ID = Convert.ToInt32(dr1["principal_id"]);

            }
            dr1.Close();

            con.Desconectar("MAS");
            return login_ID;



        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            info = 1;

            groupBox5.Enabled = false;
            groupBox4.Enabled = true;
            comboBox1.Enabled = false;
            comboBox2.Enabled = true;
            comboBox3.Enabled = true;
            comboBox4.Enabled = true;
            comboBox5.Enabled = true;
            textBox1.Enabled = true;
            toolStripButton3.Enabled = false;
            toolStripButton4.Enabled = false;
            comboBox2.Text = "";
            comboBox1.Text = "";
            comboBox3.Text = "";
            comboBox4.Text = "";
            comboBox5.Text = "";
            textBox1.Text = "";




        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            toolStripButton1.Enabled = true;
            toolStripButton4.Enabled = true;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;

            toolStripButton2.Enabled = false;

            groupBox4.Enabled = false;

            comboBox1.Enabled = true;

            comboBox2.Enabled = false;
            comboBox3.Enabled = false;
            comboBox4.Enabled = false;
            comboBox5.Enabled = false;
            textBox1.Enabled = false;

            comboBox2.Text = "";
            comboBox1.Text = "";
            comboBox3.Text = "";
            comboBox4.Text = "";
            comboBox5.Text = "";
            textBox1.Text = "";

        }

        private void carga_Empresas(object sender, EventArgs e)
        {
            if (comboBox5.Items.Count >= 1)
            {
                comboBox5.Items.Clear();
            }
            //coned.con.Open();
            //con.conectar("DM");
            //SqlCommand cm1 = new SqlCommand("SELECT [NOMRE],[Rason_Social]FROM [DM].[CORRECT].[EMPRESAS]", con.condm);
            //SqlDataReader dr1 = cm1.ExecuteReader();
            //while (dr1.Read())
            //{
            //    comboBox5.Items.Add(dr1["Rason_Social"]);

            //}
            //dr1.Close();

            //con.Desconectar("DM");

            empresas.Clear();
            con.conectar("DM");
            SqlCommand cmd2 = new SqlCommand("SELECT [ID],[NOMRE],[Rason_Social] FROM [DM].[CORRECT].[EMPRESAS]   where Estado = 'A'", con.condm);
            SqlDataAdapter da = new SqlDataAdapter(cmd2);
            da.Fill(empresas);

            con.Desconectar("DM");

            combo(empresas, "NOMRE", comboBox5);
            combo(empresas, "NOMRE", comboBox6);
            //coned.con.Close();


        }




        private void carga_Grupo(object sender, EventArgs e)
        {

            //coned.con.Open();
            con.conectar("DM");
            SqlCommand cm1 = new SqlCommand("SELECT [NOMBRE] FROM [DM].[CORRECT].[USUARIO_GRUPO]", con.condm);
            SqlDataReader dr1 = cm1.ExecuteReader();
            while (dr1.Read())
            {
                comboBox4.Items.Add(dr1["NOMBRE"]);

            }
            dr1.Close();

            con.Desconectar("DM");


            //coned.con.Close();


        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            info = 2;
            toolStripButton1.Enabled = false;
            toolStripButton2.Enabled = false;
            toolStripButton4.Enabled = false;
            toolStripButton5.Enabled = true;
            toolStripButton6.Enabled = true;
            comboBox1.Enabled = false;
            groupBox4.Enabled = false;

            comboBox2.Enabled = true;
            comboBox3.Enabled = true;
            comboBox4.Enabled = true;
            comboBox5.Enabled = true;

            textBox1.Enabled = true;



        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "")
            {
                MessageBox.Show("Seleccione un Departamento");
                comboBox2.Focus();
            }

            else if (comboBox3.Text == "")
            {
                MessageBox.Show("Seleccione una Agencia");
                comboBox3.Focus();
            }

            else if (comboBox4.Text == "")
            {
                MessageBox.Show("Seleccione un Grupo");
                comboBox4.Focus();
            }

            else if (comboBox5.Text == "")
            {
                MessageBox.Show("Seleccione una Empresa");
                comboBox5.Focus();
            }
            else if (textBox1.Text == "")
            {
                MessageBox.Show("Ingrese el Codigo de empleado");
                textBox1.Focus();
            }



            else
            {

                con.conectar("DM");
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con.condm;
                cmd.CommandText = "UPDATE [DM].[CORRECT].[USUARIOS] SET DEPARTAMENTO=@DEPARTAMENTO,AGENCIA = @AGENCIA,PUESTO =@PUESTO ,COD_EMPLEADO=@COD_EMPLEADO,EMPRESA=@EMPRESA,LOGO_EMP=@LOGO_EMP WHERE USUARIO= '" + comboBox1.Text + "'";


                cmd.Parameters.Add("@DEPARTAMENTO", SqlDbType.NVarChar).Value = comboBox2.Text;
                cmd.Parameters.Add("@PUESTO", SqlDbType.NVarChar).Value = comboBox4.Text;
                cmd.Parameters.Add("@COD_EMPLEADO", SqlDbType.NVarChar).Value = textBox1.Text;
                cmd.Parameters.Add("@EMPRESA", SqlDbType.NVarChar).Value = id_empresa;
                cmd.Parameters.Add("@AGENCIA", SqlDbType.Int).Value = id_sucursal;


                switch (comboBox5.Text)
                {

                    case "DISTRIBUIDORA MORAZAN, S.A. DE C.V":
                        cmd.Parameters.Add("@LOGO_EMP", SqlDbType.NVarChar).Value = "1";
                        break;

                    case "LESA, S.A. DE C.V":
                        cmd.Parameters.Add("@LOGO_EMP", SqlDbType.NVarChar).Value = "2";
                        break;
                    case "HAMBURGO,S.A. DE C.V":
                        cmd.Parameters.Add("@LOGO_EMP", SqlDbType.NVarChar).Value = "3";
                        break;
                    case "CORPORACION SIETE, S.A. DE C.V":
                        cmd.Parameters.Add("@LOGO_EMP", SqlDbType.NVarChar).Value = "4";
                        break;
                    case "ZEGNA, S.A. DE C.V":
                        cmd.Parameters.Add("@LOGO_EMP", SqlDbType.NVarChar).Value = "5";
                        break;
                    case "CV+, S.A. DE C.V":
                        cmd.Parameters.Add("@LOGO_EMP", SqlDbType.NVarChar).Value = "6";
                        break;

                    default:
                        cmd.Parameters.Add("@LOGO_EMP", SqlDbType.NVarChar).Value = "1";
                        break;



                }



                cmd.ExecuteNonQuery();

                con.Desconectar("DM");

                MessageBox.Show("USUARIO  " + comboBox1.Text + " Actualizado Correctamente");

                toolStripButton5_Click(null, null);

            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {

            MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("DESEA ELIMINAR EL USUARIO  " + comboBox1.Text + " DE CORRECTX ?", "ADMINISTRACION DE USUARIOS ", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {

                con.conectar("DM");
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con.condm;
                cmd.CommandText = "DELETE [DM].[CORRECT].[USUARIOS]  WHERE USUARIO= '" + comboBox1.Text + "'";

                cmd.ExecuteNonQuery();

                MessageBox.Show("USUARIO  " + comboBox1.Text + " Eliminado Correctamente");
                toolStripComboBox1.Items.Clear();
                comboBox1.Items.Clear();
                comboBox2.Items.Clear();
                Usuarios_nuevos(null, null);
                carga_usuarios(null, null);
            }

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (existinfo > 0)
            {
                info = 1;
                CORECTX_APP.Informatica.Control_Accesos.Usuarios_informacion inf = new CORECTX_APP.Informatica.Control_Accesos.Usuarios_informacion();
                inf.ShowDialog();
            }
            else
            {
                info = 0;
                CORECTX_APP.Informatica.Control_Accesos.Usuarios_informacion inf = new CORECTX_APP.Informatica.Control_Accesos.Usuarios_informacion();
                inf.ShowDialog();
            }


        }

        public void exisinfo(string user)
        {

            con.conectar("DM");
            //coned.con.Open();
            SqlCommand cm1 = new SqlCommand("SELECT COUNT ([ID]) as EXT FROM [DM].[CORRECT].[USUARIOS_INFO]  WHERE ID_USUARIO= '" + this.comboBox1.Text + "'", con.condm);
            SqlDataReader dr1 = cm1.ExecuteReader();
            while (dr1.Read())
            {
                existinfo = Convert.ToInt32(dr1["EXT"].ToString());

            }
            dr1.Close();

            con.Desconectar("DM");

        }

        private void agencias(int empres)
        {

            //coned.con.Open();
            //con.conectar("DM");
            //SqlCommand cm1 = new SqlCommand("SELECT [ID_SUCURSAL] ,[SUCURSAL]  FROM [DM].[CORRECT].[SUCURSALES_EXATUS]", con.condm);
            //SqlDataAdapter da = new SqlDataAdapter(cm1);

            //da.Fill(agencia);



            //con.Desconectar("DM");

            agencia.Clear();
            con.conectar("DM");
            SqlCommand cmd2 = new SqlCommand("SELECT [ID_SUCURSAL] ,[EMPRESA_EXACTUS],[SUCURSAL],[COD_BOD],[COD_RUTA]  FROM [DM].[CORRECT].[SUCURSALES_EXATUS] WHERE EMPRESA_EXACTUS = '" + empres + "'", con.condm);
            SqlDataAdapter da = new SqlDataAdapter(cmd2);
            da.Fill(agencia);

            con.Desconectar("DM");

            if (comboBox3.Items.Count > 1)
            {
                comboBox3.Items.Clear();
            }


            combo(agencia, "SUCURSAL", comboBox3);

        }

        public void combo(DataTable dts, string parametro, ComboBox cbx)
        {
            //toolStripComboBox1.Items.Clear();

            var result = from row in dts.AsEnumerable()
                         group row by row.Field<string>(parametro) into grp
                         select new
                         {
                             familia = grp.Key,

                         };
            foreach (var t in result)
            {
                if (t.familia == null || t.familia == "")
                {

                }
                else
                {
                    cbx.Items.Add(t.familia);

                }
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            var results = from myRow in agencia.AsEnumerable()
                          where myRow.Field<string>("SUCURSAL") == comboBox3.Text

                          select new
                          {
                              Nombre = myRow.Field<int>("ID_SUCURSAL")
                          };

            foreach (var rs1 in results)
            {
                id_sucursal = rs1.Nombre;
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            var results = from myRow in empresas.AsEnumerable()
                          where myRow.Field<string>("NOMRE") == comboBox5.Text

                          select new
                          {
                              Nombre = myRow.Field<int>("ID")
                          };

            foreach (var rs1 in results)
            {
                id_empresa = rs1.Nombre;
            }



            agencias(id_empresa);
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = false;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;


            var results = from myRow in empresas.AsEnumerable()
                          where myRow.Field<string>("NOMRE") == comboBox6.Text

                          select new
                          {
                              Nombre = myRow.Field<int>("ID")
                          };

            foreach (var rs1 in results)
            {
                id_empresa_acc = rs1.Nombre;
            }

            vacc.Valida_accion(comboBox1.Text, id_empresa_acc);

            carga_menu(null, null);

            if (valida_empres_acceso(id_empresa_acc, ID_USUARIO))
            {
                button2.Enabled = false;
            }
            else
            {
                button2.Enabled = true;
            }
            

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private bool valida_empres_acceso(int ID_empresa, string ID_empleado)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("SELECT COUNT (*) FROM[DM].[CORRECT].[EMPRESA_PERMISOS]  where USR_ID = '" + ID_empleado + "' and EMP_ID = '" + ID_empresa + "'", con.condm);

            int contar = Convert.ToInt32(cmd.ExecuteScalar());
            con.Desconectar("DM");

            if (contar == 0)
            {
                return true;

            }
            else
            {
                return false;
            }


        }
        private void add_acceso_empresa(int ID_emp , string ID_USR)
        {
            con.conectar("DM");
            fecha_actual = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con.condm;
            cmd.CommandText = " INSERT INTO [DM].[CORRECT].[EMPRESA_PERMISOS] ([USR_ID],[EMP_ID],[ACCESO],[FECHA_CREA],[USUARIO_CREA]) VALUES (@USR_ID,@EMP_ID,@ACCESO,@FECHA_CREA,@USUARIO_CREA)";
            cmd.Parameters.Add("@USR_ID", SqlDbType.NVarChar).Value = ID_USR;
            cmd.Parameters.Add("@EMP_ID", SqlDbType.NVarChar).Value = ID_emp;
            cmd.Parameters.Add("@ACCESO", SqlDbType.NVarChar).Value = "S";
            cmd.Parameters.Add("@FECHA_CREA", SqlDbType.NVarChar).Value = fecha_actual;
            cmd.Parameters.Add("@USUARIO_CREA", SqlDbType.NVarChar).Value = Login.usuario.ToUpper();         
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            con.Desconectar("DM");

            //comboBox6_SelectedIndexChanged(null, null);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBoxButtons bt1 = MessageBoxButtons.OKCancel;
            DialogResult result = MessageBox.Show("SE ELIMINARA TODO ACCESO DE EL USUARIO :" +USER+ " DE LA EMPRESA " +comboBox6.Text+"" , "CARGA FR-ERP", bt1, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.OK)
            {
                quitar_acceso_empresa(id_empresa_acc, ID_USUARIO);
                comboBox6_SelectedIndexChanged(null, null);

            }
        }

        private void quitar_acceso_empresa(int ID_emp, string ID_USR)
        {
            con.conectar("DM");
            SqlCommand cmd4 = new SqlCommand("[CORRECT].[QUITAR_ACCESSO _EMPRESA]", con.condm);
            cmd4.CommandType = CommandType.StoredProcedure;
            cmd4.Parameters.AddWithValue("@ID_USUARIO", ID_USR);
            cmd4.Parameters.AddWithValue("@ID_EMP", ID_emp);
            cmd4.ExecuteNonQuery();
            con.Desconectar("DM");

        }
    }
}
