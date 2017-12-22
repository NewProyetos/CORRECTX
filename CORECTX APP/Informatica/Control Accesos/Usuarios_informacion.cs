using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.AccessControl;
using System.Data.SqlClient;
 
namespace Sinconizacion_EXactus.CORECTX_APP.Informatica.Control_Accesos
{
    public partial class Usuarios_informacion : Form
    {
        public Usuarios_informacion()
        {
            InitializeComponent();
        }
        conexionXML con = new conexionXML();


        private void Usuarios_informacion_Load(object sender, EventArgs e)
        {
            string pat = @"C:\CORRECT";
            DirectoryInfo dir = new DirectoryInfo(pat);
            comboBox1.Text = Accesos.USER;
            comboBox1.Enabled = false;
            CheckAccess(dir);
            
            comboBox1.Enabled = false;
            comboBox1.Text = Accesos.USER;

            if (Accesos.info > 0)
            {
                
                textBox4.PasswordChar = '*';
                textBox6.PasswordChar = '*';
                textBox5.PasswordChar = '*';

                toolStripButton1.Enabled = true;
                toolStripButton2.Enabled = true;

                con.conectar("DM");

                SqlCommand cm1 = new SqlCommand("SELECT [EMAIL],[USUARIO_WIN],[EMAIL_PAS],[WIN_PASS],[IMPRESOR_ID],[ADMIN_LOCAL],[LOCAL_PASS],[EMAIL_MOBIL],[NUMERO_MOBIL],[NUMERO_EXT],[DIRECION_IP],[EXT_MOBILE],[NOMBRE_PC] FROM [DM].[CORRECT].[USUARIOS_INFO]  where ID_USUARIO = '"+Accesos.USER+"'", con.condm);
                SqlDataReader dr1 = cm1.ExecuteReader();


                while (dr1.Read())
                {
                    //comboBox1.Items.Add("R" + dr1["RUTA"]);
                    textBox3.Text = Convert.ToString(dr1["EMAIL"]);
                    textBox7.Text = Convert.ToString(dr1["USUARIO_WIN"]);
                    textBox5.Text = Encripter.Desencriptar(Convert.ToString(dr1["EMAIL_PAS"]));
                    textBox6.Text = Encripter.Desencriptar(Convert.ToString(dr1["WIN_PASS"]));
                    textBox13.Text = Convert.ToString(dr1["IMPRESOR_ID"]);
                    textBox9.Text = Convert.ToString(dr1["DIRECION_IP"]);
                    textBox8.Text = Convert.ToString(dr1["NOMBRE_PC"]);
                    textBox12.Text = Convert.ToString(dr1["EXT_MOBILE"]);
                    textBox2.Text = Convert.ToString(dr1["ADMIN_LOCAL"]);
                    textBox4.Text = Encripter.Desencriptar(Convert.ToString(dr1["LOCAL_PASS"]));

                    string mob = Convert.ToString(dr1["EMAIL_MOBIL"]);

                    textBox10.Text = Convert.ToString(dr1["NUMERO_MOBIL"]);
                    textBox11.Text = Convert.ToString(dr1["NUMERO_EXT"]);

                    if (mob == "S")
                    {
                        radioButton1.Checked = true;
                    }
                    else
                    {
                        radioButton2.Checked = true;
                    }
                }

                dr1.Close();

                con.Desconectar("DM");

                todobloqueo();


            }
            else
            {
                toolStripButton1.Enabled = false;
                toolStripButton2.Enabled = false;
                textBox4.UseSystemPasswordChar = true;
                textBox6.UseSystemPasswordChar = true;
                textBox5.UseSystemPasswordChar = true;
            }
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }
        private void CheckAccess(DirectoryInfo directory)
        {
            DataTable dt = new DataTable();
            // Get the collection of authorization rules that apply to the current directory
            AuthorizationRuleCollection acl = directory.GetAccessControl().GetAccessRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));

            foreach (var rule in acl)
            {
               
                // do something here
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            string hoy = DateTime.Now.ToString();
            if (Accesos.info > 0)
            {
                con.conectar("DM");
                SqlCommand cmd8 = new SqlCommand();
                cmd8.Connection = con.condm;
                cmd8.CommandText = "UPDATE [DM].[CORRECT].[USUARIOS_INFO] SET EMAIL=@EMAIL,USUARIO_WIN = @USUARIO_WIN,EMAIL_PAS = @EMAIL_PAS,WIN_PASS = @WIN_PASS,IMPRESOR_ID = @IMPRESOR_ID,ADMIN_LOCAL = @ADMIN_LOCAL,LOCAL_PASS = @LOCAL_PASS,EMAIL_MOBIL = @EMAIL_MOBIL,NUMERO_MOBIL =@NUMERO_MOBIL,NUMERO_EXT = @NUMERO_EXT,FECHA_INGRESO = @FECHA_INGRESO,USUARIO_CREA = @USUARIO_CREA,FECHA_UPDATE = @FECHA_UPDATE,DIRECION_IP= @DIRECION_IP,EXT_MOBILE= @EXT_MOBILE,NOMBRE_PC = @NOMBRE_PC   where ID_USUARIO = '" + Accesos.USER + "'";
               
                cmd8.Parameters.Add("@EMAIL", SqlDbType.NVarChar).Value = textBox3.Text;
                cmd8.Parameters.Add("@USUARIO_WIN", SqlDbType.NVarChar).Value = textBox7.Text;
                cmd8.Parameters.Add("@EMAIL_PAS", SqlDbType.NVarChar).Value = Encripter.Encriptar(textBox5.Text);
                cmd8.Parameters.Add("@WIN_PASS", SqlDbType.NVarChar).Value = Encripter.Encriptar(textBox6.Text);
                cmd8.Parameters.Add("@IMPRESOR_ID", SqlDbType.Int).Value = Convert.ToInt32(textBox13.Text);
                cmd8.Parameters.Add("@ADMIN_LOCAL", SqlDbType.NVarChar).Value = textBox2.Text;
                cmd8.Parameters.Add("@LOCAL_PASS", SqlDbType.NVarChar).Value = Encripter.Encriptar(textBox4.Text);
                cmd8.Parameters.Add("@DIRECION_IP", SqlDbType.NVarChar).Value = textBox9.Text;
                cmd8.Parameters.Add("@EXT_MOBILE", SqlDbType.NVarChar).Value = textBox12.Text;
                cmd8.Parameters.Add("@NOMBRE_PC", SqlDbType.NVarChar).Value = textBox8.Text;

                if (radioButton1.Checked)
                {
                    cmd8.Parameters.Add("@EMAIL_MOBIL", SqlDbType.Char).Value = "S";
                }
                else
                    if (radioButton2.Checked)
                    {
                        cmd8.Parameters.Add("@EMAIL_MOBIL", SqlDbType.Char).Value = "N";
                    }
                    else
                    {
                        cmd8.Parameters.Add("@EMAIL_MOBIL", SqlDbType.Char).Value = null;
                    }





                cmd8.Parameters.Add("@NUMERO_MOBIL", SqlDbType.NVarChar).Value = textBox10.Text;

                cmd8.Parameters.Add("@NUMERO_EXT", SqlDbType.Int).Value = Convert.ToInt32(textBox11.Text);
                cmd8.Parameters.Add("@FECHA_INGRESO", SqlDbType.DateTime).Value = DateTime.Now;
                cmd8.Parameters.Add("@USUARIO_CREA", SqlDbType.NVarChar).Value = Login.usuario.ToUpper();
                cmd8.Parameters.Add("@FECHA_UPDATE", SqlDbType.DateTime).Value = DateTime.Now;
               

                cmd8.ExecuteNonQuery();

                con.Desconectar("DM");
            }
            else
            {
                con.conectar("DM");

                SqlCommand cmd3 = new SqlCommand();
                cmd3.Connection = con.condm;
                cmd3.CommandText = "INSERT INTO [DM].[CORRECT].[USUARIOS_INFO]([ID_USUARIO],[EMAIL] ,[USUARIO_WIN] ,[EMAIL_PAS],[WIN_PASS],[IMPRESOR_ID],[ADMIN_LOCAL],[LOCAL_PASS],[EMAIL_MOBIL],[NUMERO_MOBIL],[NUMERO_EXT],[FECHA_INGRESO],[USUARIO_CREA],[FECHA_UPDATE],[DIRECION_IP],[EXT_MOBILE],[NOMBRE_PC])  VALUES(@ID_USUARIO,@EMAIL,@USUARIO_WIN,@EMAIL_PAS,@WIN_PASS,@IMPRESOR_ID,@ADMIN_LOCAL,@LOCAL_PASS,@EMAIL_MOBIL,@NUMERO_MOBIL,@NUMERO_EXT,@FECHA_INGRESO,@USUARIO_CREA,@FECHA_UPDATE,@DIRECION_IP,@EXT_MOBILE,@NOMBRE_PC)";
                cmd3.Parameters.Add("@ID_USUARIO", SqlDbType.NVarChar).Value = comboBox1.Text;
                cmd3.Parameters.Add("@EMAIL", SqlDbType.NVarChar).Value = textBox3.Text;
                cmd3.Parameters.Add("@USUARIO_WIN", SqlDbType.NVarChar).Value = textBox7.Text;
                cmd3.Parameters.Add("@EMAIL_PAS", SqlDbType.NVarChar).Value = Encripter.Encriptar(textBox5.Text);
                cmd3.Parameters.Add("@WIN_PASS", SqlDbType.NVarChar).Value = Encripter.Encriptar(textBox6.Text);
                cmd3.Parameters.Add("@IMPRESOR_ID", SqlDbType.Int).Value = Convert.ToInt32(textBox13.Text);
                cmd3.Parameters.Add("@ADMIN_LOCAL", SqlDbType.NVarChar).Value = textBox2.Text;
                cmd3.Parameters.Add("@LOCAL_PASS", SqlDbType.NVarChar).Value = Encripter.Encriptar(textBox4.Text);
                cmd3.Parameters.Add("@DIRECION_IP", SqlDbType.NVarChar).Value = textBox9.Text;
                cmd3.Parameters.Add("@EXT_MOBILE", SqlDbType.NVarChar).Value = textBox12.Text;
                cmd3.Parameters.Add("@NOMBRE_PC", SqlDbType.NVarChar).Value = textBox8.Text;

                 if (radioButton1.Checked)
                {
                    cmd3.Parameters.Add("@EMAIL_MOBIL", SqlDbType.Char).Value =  "S";
                }
                else
                     if (radioButton2.Checked)
                     {
                         cmd3.Parameters.Add("@EMAIL_MOBIL", SqlDbType.Char).Value = "N";
                     }
                     else
                     {
                         cmd3.Parameters.Add("@EMAIL_MOBIL", SqlDbType.Char).Value = null;
                     }





                 cmd3.Parameters.Add("@NUMERO_MOBIL", SqlDbType.NVarChar).Value = textBox10.Text;

                cmd3.Parameters.Add("@NUMERO_EXT", SqlDbType.Int).Value = Convert.ToInt32(textBox11.Text);
                cmd3.Parameters.Add("@FECHA_INGRESO", SqlDbType.DateTime).Value = DateTime.Now ;
                cmd3.Parameters.Add("@USUARIO_CREA", SqlDbType.NVarChar).Value = Login.usuario.ToUpper();
                cmd3.Parameters.Add("@FECHA_UPDATE", SqlDbType.DateTime).Value = DateTime.Now;
               

                cmd3.ExecuteNonQuery();

                con.Desconectar("DM");

                MessageBox.Show("Datos de Usuario " + Accesos.USER + " Ingresado Correcto");

                todobloqueo();
                toolStripButton1.Enabled = true;
                toolStripButton2.Enabled = true;
                toolStripButton3.Enabled = false;
            }
        }
        private void todobloqueo()
        {

            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            textBox8.Enabled = false;
            textBox9.Enabled = false;
            textBox10.Enabled = false;
            textBox11.Enabled = false;
            textBox12.Enabled = false;
            textBox13.Enabled = false;            

        
        }

        private void button1_KeyDown(object sender, KeyEventArgs e)
        {

            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox4.PasswordChar == '*')
            {
                textBox4.PasswordChar = '\0';
                textBox6.PasswordChar = '\0';
                textBox5.PasswordChar = '\0';
            }
            else
            {
                textBox4.PasswordChar = '*';
                textBox6.PasswordChar = '*';
                textBox5.PasswordChar = '*';
            
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
            textBox6.Enabled = true;
            textBox7.Enabled = true;
            textBox8.Enabled = true;
            textBox9.Enabled = true;
            textBox10.Enabled = true;
            textBox11.Enabled = true;
            textBox12.Enabled = true;
            textBox13.Enabled = true;   
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            MessageBoxButtons bt2 = MessageBoxButtons.YesNo;

            DialogResult resulta = MessageBox.Show("ESTA SEGURO DESEA ELIMINAR LA INFORMACION DEL USUARIO: " + Accesos.USER , "INFORMACION DE USUARIO", bt2);
                if (resulta == DialogResult.Yes)
                {
                    con.conectar("DM");

                    SqlCommand cmd3 = new SqlCommand();
                    cmd3.Connection = con.condm;
                    cmd3.CommandText = "DELETE [DM].[CORRECT].[USUARIOS_INFO] where ID_USUARIO = '" + Accesos.USER + "'";
                    cmd3.ExecuteNonQuery();

                    con.Desconectar("DM");


                }
        
         

        }

       

    }
}
