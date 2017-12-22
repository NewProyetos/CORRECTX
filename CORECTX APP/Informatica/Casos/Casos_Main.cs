using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.Data.SqlClient;
using System.Windows.Controls;


namespace Sinconizacion_EXactus
{
    public partial class Casos_Main : Form
    {
        public Casos_Main()
        {
            InitializeComponent();
        }

        //Metodo para inicializar el Formulario
        public void Inicializar() 
        {
         //   this.comboBox1.Enabled = false;
            this.comboBox2.Enabled = false;
            this.richTextBox1.Enabled = false;
            this.button1.Enabled   = false;
            this.cbCausas.Enabled  = false;
            this.txtSerie.Enabled  = false;
            this.txtModelo.Enabled = false;
            this.cbDepto.Enabled   = false;
            this.txtModelo.Text = "ND";
            this.txtSerie.Text = "ND";
            this.cbPrioridad.Text = "NORMAL";

            con.conectar("DM");
            //SqlCommand cmd = new SqlCommand("SELECT [NUM_CASO],[USUARIO_REP],[ESTATUS],[NOMBRE],[DET_PROBLEMA],[RUTA] ,[EQUIPO] ,[FECHA_CREA],MODELO,SERIE,CAUSA,DEPTO FROM [DM].[CORRECT].[CASOS]  WHERE RUTA = '" + this.comboBox1.Text + "' ", con.condm);
            SqlCommand cmd = new SqlCommand("SELECT [NUM_CASO],[USUARIO_REP],[ESTATUS],PRIORIDAD,[DET_PROBLEMA],[RUTA] ,[NOMBRE],[EQUIPO] ,[FECHA_CREA],MODELO,SERIE,CAUSA,DEPTO FROM [DM].[CORRECT].[CASOS]  WHERE ESTATUS!='Cerrado' order by PRIORIDAD,FECHA_CREA", con.condm);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            dt.Clear();
            da.Fill(dt);
            
            dataGridView1.DataSource = dt;
            dataGridView1.Refresh();

            con.Desconectar("DM");
        }

        conexionXML con = new conexionXML();
        //Conexion2 conet = new Conexion2();
        public static DataTable dt = new DataTable();
        public static string fecha;
        public static int Caso;
        public static String status;
        public static String mail_detalle;
        public static String mail_equipo;
        public static String mail_Nombre;
        public static String mail_Ruta;
        public static String mail_Fecha;
        public static String mail_Causa;
        public static String mail_Depto;
        public static String mail_Dispositivo;
        public static String mail_Modelo;
        public static String mail_Serie;
        public static String mail_Prioridad;

        public SmtpClient smtp1 = new SmtpClient();

        public MailMessage email = new MailMessage();
       
        private void Form9_Load(object sender, EventArgs e)
        {
            
            fecha = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
               
            MaximizeBox = false; ;

            dt.Clear();
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.Refresh();

            Inicializar();
            /*this.comboBox1.Enabled = false;
            this.comboBox2.Enabled = false;
            this.richTextBox1.Enabled = false;
            this.button1.Enabled = false;
            this.cbCausas.Enabled = false;
            this.txtSerie.Enabled = false;
            this.txtModelo.Enabled = false;
            */
            if (Main_Menu.Departamento != "INFORMATICA")
            {
                this.groupBox2.Hide();

                dataGridView1.Size = new Size (556,140);
                dataGridView1.Location = new Point(13, 122);
                pictureBox1.Location = new Point(440, 5);
            }
            else
            {
                con.conectar("DM");
                SqlCommand cm0 = new SqlCommand("SELECT NOMRE FROM [DM].[CORRECT].[EMPRESAS] ORDER BY NOMRE  ", con.condm);
                SqlDataReader dr0 = cm0.ExecuteReader();
                while (dr0.Read())
                {
                    comboBox1.Items.Add(dr0["NOMRE"]);
                }
                dr0.Close();

                con.Desconectar("DM");
              /**This.comboBox1.Items.Add("KOI");
                this.comboBox1.Items.Add("DISMO");
                this.comboBox1.Items.Add("CV+");
                this.comboBox1.Items.Add("SMASH BURGER");
                this.comboBox1.Items.Add("LESA");
                this.comboBox1.Items.Add("IMPACTO");
               **/
            }

            if (Login.usuario.IndexOf("p") >= 0 || Login.usuario.IndexOf("P") >= 0)
            {
                string rutaP = Login.usuario.ToUpper();
                char[] RTchar = { 'P' };
                string rutaR = rutaP.TrimStart(RTchar);

                this.comboBox1.Text = "R" + rutaR;
            }
            else
            {
                this.comboBox1.Text = null;
            }

            con.conectar("DM");

            //Cargando Departamentos
            SqlCommand cmdep = new SqlCommand("SELECT [DEPTO]  FROM [DM].[CORRECT].[DEPARTAMENTO] order by DEPTO ", con.condm);
            SqlDataReader drdep = cmdep.ExecuteReader();
            while (drdep.Read())
            {
                this.cbDepto.Items.Add(drdep["DEPTO"]);
            }
            drdep.Close();

            //Cargando Causas de Soporte
            SqlCommand cmcau = new SqlCommand("SELECT [nombre] FROM [DM].[CORRECT].[CAUSAS] ORDER BY NOMBRE ", con.condm);
            SqlDataReader drcau = cmcau.ExecuteReader();
            while (drcau.Read())
            {
                this.cbCausas.Items.Add(drcau["NOMBRE"]);
            }

            drcau.Close();
            //Cargando Dispositivos
            SqlCommand cmdis = new SqlCommand("SELECT [NOMBRE] FROM [DM].[CORRECT].[DISPOSITIVOS] ORDER BY NOMBRE ", con.condm);
            SqlDataReader drdis = cmdis.ExecuteReader();
            while (drdis.Read())
            {
                comboBox2.Items.Add(drdis["NOMBRE"]);
            }
            drdis.Close();

            SqlCommand cm2 = new SqlCommand("SELECT [RUTA]FROM [DM].[CORRECT].[RUTAS] ORDER BY RUTA  ", con.condm);
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
                comboBox1.Items.Add(dr2["RUTA"]);
            }
            dr2.Close();

            SqlCommand cm3 = new SqlCommand("SELECT [NOMBRE] FROM [DM].[CORRECT].[RUTAS] WHERE HANDHELD = '" +Login.usuario+ "' ", con.condm);
            SqlDataReader dr3 = cm3.ExecuteReader();
            while (dr3.Read())
            {
                this.label4.Text = Convert.ToString(dr3["NOMBRE"]);
            }
            dr3.Close();
/*
            //SqlCommand cmd = new SqlCommand("SELECT [NUM_CASO],[USUARIO_REP],[ESTATUS],[NOMBRE],[DET_PROBLEMA],[RUTA] ,[EQUIPO] ,[FECHA_CREA],MODELO,SERIE,CAUSA,DEPTO FROM [DM].[CORRECT].[CASOS]  WHERE RUTA = '" + this.comboBox1.Text + "' ", con.condm);
            SqlCommand cmd = new SqlCommand("SELECT [NUM_CASO],[USUARIO_REP],[ESTATUS],PRIORIDAD,[DET_PROBLEMA],[RUTA] ,[NOMBRE],[EQUIPO] ,[FECHA_CREA],MODELO,SERIE,CAUSA,DEPTO FROM [DM].[CORRECT].[CASOS]  WHERE ESTATUS!='Cerrado' order by PRIORIDAD,FECHA_CREA", con.condm);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            dataGridView1.DataSource = dt;
            dataGridView1.Refresh();
            */
            con.Desconectar("DM");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dt.Clear();
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            //dataGridView1.Refresh();

             con.conectar("DM");

            SqlCommand cm3 = new SqlCommand("SELECT [NOMBRE] FROM [DM].[CORRECT].[RUTAS] WHERE RUTA = '"+this.comboBox1.Text+"' ", con.condm);
            SqlDataReader dr3 = cm3.ExecuteReader();
            while (dr3.Read())
            {
                this.label4.Text = Convert.ToString(dr3["NOMBRE"]);
            }
            dr3.Close();

            SqlCommand cmd = new SqlCommand("SELECT [NUM_CASO],[USUARIO_REP],[ESTATUS],PRIORIDAD,[DET_PROBLEMA],[RUTA] ,[NOMBRE],[EQUIPO] ,[FECHA_CREA],MODELO,SERIE,CAUSA,DEPTO FROM [DM].[CORRECT].[CASOS]  WHERE RUTA = '" + this.comboBox1.Text + "' order by ESTATUS,PRIORIDAD,FECHA_CREA ", con.condm);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            dataGridView1.DataSource = dt;
            dataGridView1.Refresh();
            con.Desconectar("DM");
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (this.comboBox2.Text == "" || this.comboBox2.Text == null)
            {
                MessageBox.Show("SELECCIONE EQUIPO");
            }
            else if (this.richTextBox1.Text == "" || this.richTextBox1.Text == null)
            {
                MessageBox.Show("Ingrese Un detalle del Error");

            }
            else
            {

                dt.Clear();
                dataGridView1.RowHeadersVisible = false;
                dataGridView1.AutoResizeColumns();
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dataGridView1.Refresh();

                mail_detalle = this.richTextBox1.Text;
                mail_equipo  = this.comboBox2.Text;
                mail_Ruta = this.comboBox1.Text;
                mail_Nombre = this.label4.Text;
                mail_Fecha  = fecha;
                mail_Causa  = this.cbCausas.Text;
                mail_Serie = this.txtSerie.Text;
                mail_Modelo = this.txtModelo.Text;
                mail_Depto = this.cbDepto.Text;
                mail_Prioridad = this.cbPrioridad.Text;

                try
                {
                     con.conectar("DM");

                    SqlCommand cmd3 = new SqlCommand("[CORRECT].[INSERT_CASOS]", con.condm);
                    cmd3.CommandType = CommandType.StoredProcedure;

                    cmd3.Parameters.AddWithValue("@Usuario_rep", Login.usuario.ToUpper());
                   
                    if (label4.Text == "")
                    {
                        cmd3.Parameters.AddWithValue("@Nombre", null);
                    }
                    else
                    {
                        cmd3.Parameters.AddWithValue("@Nombre", this.label4.Text);
                    }

                    cmd3.Parameters.AddWithValue("@DET_Problema", this.richTextBox1.Text);
                    cmd3.Parameters.AddWithValue("@Solucion", null);
                    cmd3.Parameters.AddWithValue("@Estatus", "Abierto");
                    cmd3.Parameters.AddWithValue("@Ruta", this.comboBox1.Text);
                    cmd3.Parameters.AddWithValue("@Equipo", this.comboBox2.Text);
                    cmd3.Parameters.AddWithValue("@fecha_crea", fecha);
                    cmd3.Parameters.AddWithValue("@Usuario_Update", null);
                    cmd3.Parameters.AddWithValue("@Fecha_Update", null);
                    cmd3.Parameters.AddWithValue("@Causa", this.cbCausas.Text);
                    cmd3.Parameters.AddWithValue("@Modelo", this.txtModelo.Text);
                    cmd3.Parameters.AddWithValue("@Serie", this.txtSerie.Text);
                    cmd3.Parameters.AddWithValue("@Depto", this.cbDepto.Text);
                    cmd3.Parameters.AddWithValue("@Prioridad", this.cbPrioridad.Text);
                    cmd3.ExecuteNonQuery();

                    SqlCommand cmd2 = new SqlCommand("SELECT [NUM_CASO],[USUARIO_REP],[ESTATUS],PRIORIDAD,[DET_PROBLEMA],[RUTA] ,[NOMBRE] ,[EQUIPO] ,[FECHA_CREA],MODELO,SERIE,CAUSA,DEPTO FROM [DM].[CORRECT].[CASOS]   WHERE ESTATUS!='Cerrado' order by ESTATUS,PRIORIDAD,FECHA_CREA ", con.condm);
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);

                    da.Fill(dt);

                    dataGridView1.DataSource = dt;
                    dataGridView1.Refresh();

                    this.comboBox2.Text = null;
                    this.richTextBox1.Text = null;
                    Inicializar();
                    this.button3.Enabled = true;
                }
                catch
                {
                    MessageBox.Show("No paso NAda");
                }

                try
                {
                    email.From = new MailAddress("admindm@lamorazan.com");
                    email.To.Add(new MailAddress("carlos_hercules@lamorazan.com"));
                    email.CC.Add(new MailAddress("isaac_turcios@lamorazan.com"));
                    email.CC.Add(new MailAddress("misael_santos@lamorazan.com"));
                    email.CC.Add(new MailAddress("javier_estrada@lamorazan.com"));

                    email.Subject = "NUEVO Caso Prioridad: "+mail_Prioridad+" Usuario (" + Login.usuario.ToUpper() + ")" ;
                    email.Body = "Se ha agregado un Nuevo Caso con el siguente Contenido:<br />  Ruta/Empresa: " + mail_Ruta + " <br />  Departamento: " + mail_Depto + " <br />  Modelo: " + mail_Modelo + "  Serie: " + mail_Serie + " <br />  Usuario: " + mail_Nombre + " <br /> Dispositivo: " + " " + mail_equipo + " <br />  Causa: " + mail_Causa + " <br /> Detalle: " + mail_detalle + " <br /> Fecha: " + mail_Fecha ;

                    email.IsBodyHtml = true;
                    email.Priority = MailPriority.Normal;

                    smtp1.Host = "smtpout.secureserver.net";
                    smtp1.Port = 25;
                    smtp1.EnableSsl = false;
                    smtp1.UseDefaultCredentials = false;

                    smtp1.Credentials = new NetworkCredential("admindm@lamorazan.com", "Ma1lAdw1uDM");

                    con.Desconectar("DM");

                    smtp1.Send(email);
                    email.Dispose();
                }
                catch
                {
                    MessageBox.Show("NO SE ENVIO CORREO A IT");               
                }
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            int fila = dataGridView1.RowCount;
          

            for (int i = 0; i < fila; i++)
            {
             
                if (dataGridView1[2, i].Value.ToString() == "Cerrado")
                {
                    dataGridView1[2, i].Style.ForeColor = Color.Red;
                }

                else if (dataGridView1[2, i].Value.ToString() == "Abierto")
                {
                    dataGridView1[2, i].Style.ForeColor = Color.Green;
                }
                else if (dataGridView1[2, i].Value.ToString() == "Pendiente")
                {
                    dataGridView1[2, i].Style.ForeColor = Color.DarkOrange;
                }
            }       
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row1 = dataGridView1.CurrentRow;

            string estatus = Convert.ToString(row1.Cells["ESTATUS"].Value);

            if (Main_Menu.Departamento == "INFORMATICA")
            {
                Caso = Convert.ToInt32(row1.Cells["NUM_CASO"].Value);
                status=Convert.ToString (row1.Cells["ESTATUS"].Value);
                mail_detalle = Convert.ToString(row1.Cells["DET_PROBLEMA"].Value);
                mail_Ruta = Convert.ToString(row1.Cells["RUTA"].Value);
                mail_equipo = Convert.ToString(row1.Cells["EQUIPO"].Value);

                Casos_update fm10 = new Casos_update();
                fm10.FormClosed += new System.Windows.Forms.FormClosedEventHandler(form10_FormClosed);
                fm10.ShowDialog();
            }
            else
            {

                if (row1 != null && estatus != "Abierto")
                {
                    Caso = Convert.ToInt32(row1.Cells["NUM_CASO"].Value);
                    status = Convert.ToString(row1.Cells["ESTATUS"].Value);
                    Casos_update fm10 = new Casos_update();
                    fm10.FormClosed += new System.Windows.Forms.FormClosedEventHandler(form10_FormClosed);
                    fm10.ShowDialog();
                }
            }

        }
        private void form10_FormClosed(object sender, FormClosedEventArgs e)
        {
            dt.Clear();
            dataGridView1.Refresh();
             con.conectar("DM");

            SqlCommand cmd = new SqlCommand("SELECT [NUM_CASO],[USUARIO_REP],[ESTATUS],PRIORIDAD,[DET_PROBLEMA],[RUTA] ,[NOMBRE],[EQUIPO] ,[FECHA_CREA],MODELO,SERIE,CAUSA,DEPTO FROM [DM].[CORRECT].[CASOS]   WHERE ESTATUS!='Cerrado' order by ESTATUS,PRIORIDAD,FECHA_CREA ", con.condm);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            dataGridView1.DataSource = dt;
            dataGridView1.Refresh();

            con.Desconectar("DM");
        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.comboBox3.Text == null || this.comboBox3.Text == "")
            {
                dt.Clear();
                dataGridView1.Refresh();
                 con.conectar("DM");

                 SqlCommand cmd = new SqlCommand("SELECT [NUM_CASO],[USUARIO_REP],[ESTATUS],PRIORIDAD,[DET_PROBLEMA],[RUTA] ,[NOMBRE] ,[EQUIPO] ,[FECHA_CREA],MODELO,SERIE,CAUSA,DEPTO FROM [DM].[CORRECT].[CASOS]  WHERE NUM_CASO = '" + this.textBox1.Text + "' order by ESTATUS,PRIORIDAD,FECHA_CREA ", con.condm);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();

                con.Desconectar("DM");

            }
            else
            {

                if (this.textBox1.Text == "" || this.textBox1.Text == null)
                {
                    dt.Clear();
                    dataGridView1.Refresh();
                     con.conectar("DM");

                     SqlCommand cmd = new SqlCommand("SELECT [NUM_CASO],[USUARIO_REP],[ESTATUS],PRIORIDAD,[DET_PROBLEMA],[RUTA] ,[NOMBRE],[EQUIPO] ,[FECHA_CREA],MODELO,SERIE,CAUSA,DEPTO FROM [DM].[CORRECT].[CASOS]  WHERE ESTATUS = '" + this.comboBox3.Text + "' order by ESTATUS,PRIORIDAD,FECHA_CREA ", con.condm);
                     SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;
                    dataGridView1.Refresh();

                    con.Desconectar("DM");
                }
                else
                {

                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            DataGridViewRow row1 = dataGridView1.CurrentRow;
            this.comboBox1.Text = Convert.ToString(row1.Cells["RUTA"].Value);
            //this.comboBox2.Text = Convert.ToString(row1.Cells["EQUIPO"].Value);
           //this.richTextBox1.Text = Convert.ToString(row1.Cells["DET_PROBLEMA"].Value);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.comboBox1.Enabled = true;
            this.comboBox2.Enabled = true;
            this.richTextBox1.Enabled = true;
            this.button1.Enabled = true;
            this.button3.Enabled = false;
            this.txtSerie.Enabled = true;
            this.txtModelo.Enabled = true;
            this.cbCausas.Enabled = true;
            this.cbDepto.Enabled = true;  
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            DialogResult x = MessageBox.Show("Seguro que Desea Salir?", "Correct ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (x == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Inicializar();
            this.button3.Enabled = true;
        }

        private void btnAgregaDisp_Click(object sender, EventArgs e)
        {
            DialogResult x = MessageBox.Show("Desea Agregar este nuevo Dispositivo?", "Correct ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (x == DialogResult.Yes)
            {
                con.conectar("DM");
                SqlCommand cm3 = new SqlCommand("INSERT INTO [DM].[CORRECT].[DISPOSITIVOS] (NOMBRE) VALUES('" + this.comboBox2.Text + "')", con.condm);
                cm3.ExecuteNonQuery();
                con.Desconectar("DM");
            }
        }

        private void btnAgregaCausa_Click(object sender, EventArgs e)
        {
            DialogResult x = MessageBox.Show("Desea Agregar esta nueva Causa?", "Correct ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (x == DialogResult.Yes)
            {
                con.conectar("DM");
                SqlCommand cm3 = new SqlCommand("INSERT INTO [DM].[CORRECT].[CAUSAS] (NOMBRE) VALUES('" + this.cbCausas.Text + "')", con.condm);
                cm3.ExecuteNonQuery();
                con.Desconectar("DM");
            }
        }
    }
}
