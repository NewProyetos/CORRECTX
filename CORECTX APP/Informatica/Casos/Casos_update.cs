using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;

namespace Sinconizacion_EXactus
{
    public partial class Casos_update : Form
    {
        public Casos_update()
        {
            InitializeComponent();
        }
        conexionXML con = new conexionXML();
       // Conexion2 conet = new Conexion2();
        public static String usuario;
        public static String fecha;
        public static String Solucion;
        public static String Nstatus;

        public SmtpClient smtp1 = new SmtpClient();

        public MailMessage email = new MailMessage();   

        private void Form10_Load(object sender, EventArgs e)
        {
            fecha = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            this.label7.Text = Login.usuario.ToUpper();
            this.label8.Text = fecha;

            if (Main_Menu.Departamento == "INFORMATICA")
            {             

                if (Casos_Main.status == "Cerrado")
                {
                    this.richTextBox1.Enabled = false;
                    this.button1.Hide();
                    this.comboBox2.Enabled = false;
                }
            }
            else
            {
                this.richTextBox1.Enabled = false;
                this.button1.Hide();
                this.comboBox2.Enabled = false;         
            }

            con.conectar("DM");
            
            SqlCommand cm2 = new SqlCommand("SELECT SOLUCION,FECHA_UPDATE,USUARIO_UPDATE  FROM [DM].[CORRECT].[CASOS] WHERE NUM_CASO ='"+Casos_Main.Caso+"'  ", con.condm);
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
                this.richTextBox1.Text = Convert.ToString(dr2["SOLUCION"]);
                usuario = Convert.ToString(dr2["USUARIO_UPDATE"]);

                if (Casos_Main.status != "Abierto")
                {
                    this.label4.Text = Convert.ToString(dr2["FECHA_UPDATE"]);
                    
                }
                else
                {
                    this.label4.Text = "Caso Sin Atender";
                    this.label6.Text = "Caso Sin Atender";
                }
            }
            dr2.Close();
            
           con.Desconectar("DM");
            this.comboBox2.Text = Casos_Main.status;
            
            switch (usuario)
            { 
                case "TURCIOSI":
                    this.label6.Text = "ISAAC TURCIOS";
                    break;

                case "SANTOSM":
                    this.label6.Text = "MISAEL SANTOS";
                    break;

                case "HERCULESC":
                    this.label6.Text = "CARLOS HERCULES";
                    break;
                case "ESTRADAJ":
                    this.label6.Text = "JAVIER ESTRADA";
                    break;
                default:
                    this.label6.Text = usuario;
                    break;

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.comboBox2.Text == "Abierto")
            {
                MessageBox.Show("Debe cambiar el estatus del casos");
             
            }
            else
            {
                if (this.richTextBox1.Text == null || this.richTextBox1.Text == "")
                {
                    MessageBox.Show("Escriba una descripcion de la solucion");

                }

                else
                {
                    Nstatus = this.comboBox2.Text;
                    Solucion = this.richTextBox1.Text;

                    MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show("ESTA SEGURO DE CAMBIAR EL ESTATUS AL CASO No.: " + Casos_Main.Caso + " A  " + this.comboBox2.Text + "  ", "ACTUALIZACION DE CASOS", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            con.conectar("DM");

                            SqlCommand cmd = new SqlCommand("[CORRECT].[UDATE_CASOS]", con.condm);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@estatus", this.comboBox2.Text);
                            cmd.Parameters.AddWithValue("@fecha_update", fecha);
                            cmd.Parameters.AddWithValue("@caso", Casos_Main.Caso);
                            cmd.Parameters.AddWithValue("@usuario_update", Login.usuario.ToUpper());
                            cmd.Parameters.AddWithValue("@solucion", this.richTextBox1.Text);


                            cmd.ExecuteNonQuery();
                            con.Desconectar("DM");

                            MessageBox.Show("Caso No. " + Casos_Main.Caso + " Actualizado Correctamente");
                            try
                            {
                                email.From = new MailAddress("admindm@lamorazan.com");
                                email.To.Add(new MailAddress("carlos_hercules@lamorazan.com"));
                                email.CC.Add(new MailAddress("isaac_turcios@lamorazan.com"));
                                email.CC.Add(new MailAddress("misael_santos@lamorazan.com"));
                                email.CC.Add(new MailAddress("javier_estrada@lamorazan.com"));

                                email.Subject = "USUARIO (" + Login.usuario.ToUpper() + ")  Ha cambiado el Estatus del Caso No. " + Casos_Main.Caso + "";
                                email.Body = "Nuevo estatus del caso: " + Nstatus.ToUpper() + "<br /> Detalle de Reporte: " + Casos_Main.mail_detalle.ToUpper() + " <br />  Ruta/Empresa: " + Casos_Main.mail_Ruta + " <br />  Especialista que Reviso El Caso: " + Login.usuario.ToUpper() + " <br /> Dispositivo: " + " " + Casos_Main.mail_equipo + " <br /> Solucion: " + Solucion + "<br /> Fecha Modificacion: " + fecha + " ";

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
                            catch
                            {
                                MessageBox.Show("NO SE ENVIO CORREO A IT");
                            }

                            this.Close();
                        }
                        catch
                        {
                            MessageBox.Show("ERROR Caso no actualizado");
                        
                        }

                    }
                }
            }
        }
    }
}
