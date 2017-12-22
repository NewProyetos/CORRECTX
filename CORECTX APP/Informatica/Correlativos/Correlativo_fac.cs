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
    public partial class Correlativo_fac : Form
    {
        public Correlativo_fac()
        {
            InitializeComponent();
        }
        conexionXML con = new conexionXML();
        //conexion conecEXAC =  new conexion();
        //Conexion2 conecDM = new Conexion2();

        public static string Ruta;
        public static string C_ant;
        public static string fecha_actual;
        String empresa = Login.empresa;
        

        private void Form11_Load(object sender, EventArgs e)
        {
            fecha_actual = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
      
            this.textBox1.Enabled = false;
            this.textBox2.Enabled = false;
            this.textBox3.Enabled = false;
            this.linkLabel1.Enabled = false;
            this.button2.Enabled = false;
            this.button3.Enabled = false;

            
            con.conectar("DM");
            
            SqlCommand cm1 = new SqlCommand("SELECT [RUTA] FROM [EXACTUS].["+empresa+"].[RUTA] where DESCRIPCION = 'PRONTA'ORDER BY RUTA", con.condm);
            SqlDataReader dr1 = cm1.ExecuteReader();


            while (dr1.Read())
            {
                comboBox1.Items.Add("R"+dr1["RUTA"]);
            }

            dr1.Close();
            
            con.Desconectar("DM");

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.textBox3.Text = null;
            this.textBox3.Enabled = false;
            this.linkLabel1.Enabled = false;
            this.button2.Enabled = false;
            this.button3.Enabled = false;
            
            Ruta = this.comboBox1.Text;

            con.conectar("DM");

            SqlCommand cm1 = new SqlCommand("SELECT CORRELATIVO FROM [DM].[CORRECT].[HIST_CORELATIVO]  WHERE RUTA ='" + this.comboBox1.Text + "'", con.condm);
            SqlDataReader dr1 = cm1.ExecuteReader();


            while (dr1.Read())
            {
                C_ant = Convert.ToString(dr1["CORRELATIVO"]);
                this.textBox2.Text = C_ant;
            }

            dr1.Close();
            con.Desconectar("DM");

            
            con.conectar("EX");
            
            SqlCommand cm2 = new SqlCommand("SELECT NUM_FAC FROM [EXACTUS].[ERPADMIN].[alSYS_PRM]  where COD_ZON ='" + this.comboBox1.Text + "'", con.conex);
            SqlDataReader dr2 = cm2.ExecuteReader();


            while (dr2.Read())
            {
                this.textBox1.Text = Convert.ToString(dr2["NUM_FAC"]);
            }

            dr2.Close();
            
            con.Desconectar("EX");


            

            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Seleccione una Ruta");
            }

            else
            {
                MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show("Desea Cambiar El correlativo de la Ruta: " + Ruta.ToUpper() + " ", "CAMBIO CORRELATIVO PREIMPRESO", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {

                    this.textBox3.Enabled = true;
                    this.linkLabel1.Enabled = true;
                    this.button2.Enabled = true;
                    this.button3.Enabled = true;

                }
            }
          
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Desea Restablecer El Correlativo Anterior: " +C_ant.ToUpper() + " a la Ruta: "+Ruta.ToUpper()+" ", "CAMBIO CORRELATIVO PREIMPRESO", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {

                this.textBox3.Text = C_ant;

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (this.textBox3.Text == "")
            {
                MessageBox.Show("Ingrese el Nuevo Correlativo");
                textBox3.Focus();
            }

            else
            {

                MessageBoxButtons bt2 = MessageBoxButtons.YesNo;

                DialogResult resulta = MessageBox.Show("ESTA SEGURO DE CAMBIAR EL CORRELATIVO: " + C_ant.ToUpper() + " DE LA RUTA: " + Ruta.ToUpper() + " POR EL CORRELATIVO " + this.textBox3.Text + " ", "CAMBIO CORRELATIVO PREIMPRESO", bt2);
                if (resulta == DialogResult.Yes)
                {
                    if (textBox3.Text == "")
                    {
                        MessageBox.Show("INGRESE NUEVO CORRELATIVO");
                        textBox3.Focus();
                    }
                    else
                    {

                        //MessageBox.Show("HOLA");
                        try
                        {
                            con.conectar("DM");

                            SqlCommand cmd = new SqlCommand("[CORRECT].[CORRELATIVO_UPDATE]", con.condm);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Correlatibo", this.textBox1.Text);
                            cmd.Parameters.AddWithValue("@fecha", fecha_actual);
                            cmd.Parameters.AddWithValue("@Ruta", Ruta);
                            cmd.Parameters.AddWithValue("@Usuario", Login.usuario.ToUpper());

                            cmd.ExecuteNonQuery();

                            con.Desconectar("DM");
                        }
                        catch
                        {
                            MessageBox.Show("No se actualizo Historico");
                        }


                        try
                        {

                            con.conectar("EX");

                            SqlCommand cmd = new SqlCommand("[ERPADMIN].[UPDATE_CORRELATIVOPR]", con.conex);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Correlatibo", this.textBox3.Text);
                            cmd.Parameters.AddWithValue("@Ruta", Ruta);

                            cmd.ExecuteNonQuery();

                            con.Desconectar("EX");

                            MessageBox.Show("CORRELATIVO DE RUTA: " + Ruta + " SE CAMBIO EXITOSAMENTE");
                            textBox3.Text = null;
                        }
                        catch
                        {
                            MessageBox.Show("No se actualizo Correlativo");
                        }



                        comboBox1_SelectedIndexChanged(null, null);

                    }
                }


            }

            
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.textBox3.Text = null;
            this.textBox3.Enabled = false;
            this.linkLabel1.Enabled = false;
            this.button2.Enabled = false;
            this.button3.Enabled = false;
            

        }

        

        
    }
}
