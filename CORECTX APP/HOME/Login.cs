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
    public partial class Login : DevExpress.XtraEditors.XtraForm
    {
        public Login()
        {
            InitializeComponent();
            DevExpress.Skins.SkinManager.EnableFormSkins();
        }

        public static String usuario;
        public static String contraseña;
        public static DataTable DTconexion;
        DataTable agencias = new DataTable();

        public static String vercion;
        public static String empresa;
        public static int empresa_id;
        public static String tipo_user;
        public static String vercion_actual;
        string actual;
        string nueva;
        conexionXML cont  = new conexionXML();
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            ingreso();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
           

            MaximizeBox = false;
            try
            {
                DTconexion = XMLRW.Readxml("CONFIGURACION");
            }
            catch(Exception conexerro)
            {
                MessageBox.Show(conexerro.ToString(), "Error en Configuracion XML");
            
            }
            var ensablado = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            vercion_actual = ensablado.ToString();
            label3.Text = vercion_actual;

            cont.conectar("DM");

            SqlCommand cm2 = new SqlCommand("SELECT [VERCION]  FROM [DM].[CORRECT].[VERCION_CORRECTX] WHERE ID_VERCION = '1'", cont.condm);
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
                vercion = Convert.ToString(dr2["VERCION"]);               

            }
            dr2.Close();
            cont.Desconectar("DM");
            int ctn = vercion_actual.Length;
             actual = vercion_actual.Substring((ctn-3),3);

            int ctn2 = vercion.Length;
             nueva = vercion.Substring((ctn2-3),3);

            if (Convert.ToInt32(actual) < Convert.ToInt32(nueva))
            {
                MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show("EXISTE UNA NUEVA VERSION  DE CORRECT   " + vercion + " ¿DESEA REALIZAR LA ACTUALIZACION ?", "ACTUALIZACION SISTEMA  " + vercion_actual + "", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {
                    compiar_archivo();
                    instalar_actualizacion();

                    this.Close();
                }
            }
            else
            {
                  


            }
            cargaempresas();
            combo_empresa(comboBox1, agencias, "NOMRE");

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                //   ingreso();

                comboBox1.Focus();

            }
        }

        private void Configuracion_Click(object sender, EventArgs e)
        {
             usuario = textBox1.Text;
             contraseña = textBox2.Text;
             if (usuario == "Administrador" && contraseña == "D!sW0Adw1u")
             {
                 Configuracion conf = new Configuracion();
                 conf.Show();
             }
             else

             {
                 MessageBox.Show("Debe Ingresar con Usuario Administrador");
             
             }
        }
        private void UpdateVercion()
        {

            cont.conectar("DM");

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cont.condm;
            cmd.CommandText = "UPDATE [DM].[CORRECT].[VERCION_CORRECTX] SET [VERCION]= @VERCION,[ULTIMO_USUARIO]=@ULTIMO_USUARIO,[FECHA]=@FECHA WHERE ID_VERCION = '1' ";
            cmd.Parameters.Add("@VERCION", SqlDbType.NVarChar).Value = vercion_actual;
            cmd.Parameters.Add("@ULTIMO_USUARIO", SqlDbType.NVarChar).Value = usuario;
            cmd.Parameters.Add("@FECHA", SqlDbType.NVarChar).Value = DateTime.Now.ToString("yyyy/MM/dd");

            cmd.ExecuteNonQuery();
            cont.Desconectar("DM");
        
        }
        private void cargaempresas()
        {
            
            agencias.Clear();
            cont.conectar("DM");
            SqlCommand cmd2 = new SqlCommand("SELECT [ID],[NOMRE],[Rason_Social] FROM [DM].[CORRECT].[EMPRESAS]  where Estado = 'A'", cont.condm);
            SqlDataAdapter da = new SqlDataAdapter(cmd2);
            da.Fill(agencias);

            cont.Desconectar("DM");
            
            //cont.conectar("DM");

            //SqlCommand cm2 = new SqlCommand("SELECT [NOMRE]  FROM[DM].[CORRECT].[EMPRESAS]  where[Estado] = 'A'", cont.condm);
            //SqlDataReader dr2 = cm2.ExecuteReader();
            //while (dr2.Read())
            //{

            //    comboBox1.Items.Add(dr2["NOMRE"]);

            //}
            //dr2.Close();
            //cont.Desconectar("DM");



        }

        public void combo_empresa(ComboBox cb, DataTable dts, string parametro)
        {
            cb.Items.Clear();

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
                    cb.Items.Add(t.familia);


                }
            }
        }

        private void datos_empresa_usuario(DataTable dteu)
        {
            var results = from myRow in dteu.AsEnumerable()
                          where myRow.Field<string>("NOMRE") == comboBox1.Text

                          select new
                          {
                              Nombre = myRow.Field<int>("ID"),
                              rason = myRow.Field<string>("Rason_Social")
                          };

            foreach (var rs1 in results)
            {

                empresa_id = rs1.Nombre;



            }

        }



        private void instalar_actualizacion()
        {

            string file = @"C:\CORRECT\UPDATE\" + vercion + ".exe";
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = file;
            // p.StartInfo.Arguments = "login.dbf";

            if (System.IO.File.Exists(file))
            {
                p.Start();
            }
        }

        private void compiar_archivo()
        {

            string fileName = ""+vercion+".exe";
            string origenPath = @"\\192.168.1.3\CORRECT_x86";
            string destinoPath = @"C:\CORRECT\UPDATE";

            string OrigenFile = System.IO.Path.Combine(origenPath, fileName);
            string DestinoFile = System.IO.Path.Combine(destinoPath, fileName);

            if (!System.IO.Directory.Exists(destinoPath))
            {
                System.IO.Directory.CreateDirectory(destinoPath);
            }

            System.IO.File.Copy(OrigenFile, DestinoFile, true);


        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                empresa = comboBox1.Text;
                datos_empresa_usuario(agencias);

            }
        }


        private bool PermisoEmpresa(string user, string emp)
        {
            string comdrt = "SELECT COUNT(EMPR.[USR_ID]) FROM [DM].[CORRECT].[EMPRESA_PERMISOS] EMPR LEFT JOIN [DM].[CORRECT].[EMPRESAS] EMP  on EMPR.EMP_ID = EMP.ID LEFT JOIN [DM].[CORRECT].[USUARIOS] USR ON EMPR.USR_ID = USR.USER_ID  where USR.USUARIO = '"+user+"' and EMP.NOMRE = '"+emp+"' ";
            cont.conectar("DM");
           SqlCommand mcm = new SqlCommand(comdrt, cont.condm);
            int contar = Convert.ToInt32(mcm.ExecuteScalar());
            cont.Desconectar("WEB");
            if (contar == 0)
            {
                return false;

            }
            else
            {
                return true;

            }

        }
        private void ingreso()
        {
            if (empresa != "")
            {


                usuario = textBox1.Text;
                contraseña = textBox2.Text;

                if (PermisoEmpresa(usuario, empresa))
                {
                    if (tipo_Usuario(usuario))
                    {
                        Log_local();
                    }
                    else
                    {
                        //conxion para validar 
                        string cad = "data source=192.168.1.25;initial catalog=EXACTUS ;user Id ='" + usuario + "' ; password = '" + contraseña + "'";
                        SqlConnection con = new SqlConnection(cad);



                        // valida usuarios y contraseña
                        try
                        {
                            con.Open();
                            // MessageBox.Show("Bienvenido"+"  "+usuario);
                            con.Close();

                          //  ID_EMPRESA();
                            if (Convert.ToInt32(actual) > Convert.ToInt32(nueva))
                            {
                                UpdateVercion();
                            }

                            XMLRW.writeLogin(usuario, vercion, empresa, tipo_user);
                            Main_Menu fm6 = new Main_Menu();
                            this.Visible = false;
                            fm6.ShowDialog();
                            fm6.Close();
                            fm6 = null;
                            this.Close();




                        }
                        catch (Exception erg)
                        {
                            string text = erg.ToString();
                            if (text.Contains("inicio de sesión"))
                            {

                                MessageBox.Show("Error en Usuario o Contraseña", "Error Inicio de Sesión ");
                                this.textBox2.Text = null;
                            }
                            else
                            {
                                MessageBox.Show(text, "!!!! Error !!!! ");
                                this.textBox2.Text = null;

                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Actualmente no cuenta con Permisos para la Empresa  " + empresa + "");
                }
            }


        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                ingreso();
            }
        }

        private bool tipo_Usuario(string usrcx)
        {

            cont.conectar("DM");
            
            SqlCommand cm2 = new SqlCommand("SELECT [TIPO_USER] FROM [DM].[CORRECT].[USUARIOS] where usuario ='"+textBox1.Text+"'", cont.condm);
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
                tipo_user = Convert.ToString(dr2["TIPO_USER"]);

            }
            dr2.Close();
            cont.Desconectar("DM");

            if (tipo_user == "L")
            {
                return true;

            }
            else
            {
                return false;
            }
        }
        private void Log_local()
        {

            cont.conectar("DM");
            SqlCommand cm4 = new SqlCommand("SELECT [USUARIO],[PASS_LOCAL],[PUESTO] FROM [DM].[CORRECT].[USUARIOS] where usuario ='" + textBox1.Text + "'", cont.condm);
            SqlDataReader dr = cm4.ExecuteReader();
            while (dr.Read())
            {
                usuario = Convert.ToString(dr["USUARIO"]);
                contraseña = Encripter.Desencriptar(Convert.ToString(dr["PASS_LOCAL"]));
               // puesto = Convert.ToString(dr["PUESTO"]);


            }
            dr.Close();
            cont.Desconectar("DM");

            if (textBox1.Text.ToUpper() == usuario.ToUpper())
            {

                if (contraseña == "" || contraseña == string.Empty)
                {

                    MessageBox.Show("Este usuario no cuenta con contraseña Solicitar  la Creacion a Soporte Tecnico");

                }

                else if (textBox2.Text == contraseña)
                {
                    if (Convert.ToInt32(actual) > Convert.ToInt32(nueva))
                    {
                        UpdateVercion();
                    }

                    Main_Menu fm6 = new Main_Menu();
                    this.Visible = false;
                    fm6.ShowDialog();
                    fm6.Close();
                    fm6 = null;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Error en Contraseña");
                }

            }
            else
            {
                MessageBox.Show("Usuario no Existe");
            }



        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                if (textBox1.Text != "")
                {
                    textBox2.Focus();
                }
            }
        }
    }
}
