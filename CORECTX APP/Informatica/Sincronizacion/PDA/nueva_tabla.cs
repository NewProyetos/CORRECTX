using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;



namespace Sinconizacion_EXactus.CORECTX_APP.Informatica.Sincronizacion.PDA
{
    public partial class nueva_tabla : Form
    {
        public nueva_tabla()
        {
            InitializeComponent();
        }
        conexionXML con = new conexionXML();
       

        public static string Tabla;
        public static string consulta;
        int nuevo_num = 0;
        DataTable intermedia = new DataTable();


        private void nueva_tabla_Load(object sender, EventArgs e)
        {

            label1.Hide();
            textBox2.Hide();
            button2.Hide();

        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (richTextBox1.TextLength == 0)
            {
                button1.Text = "Salir";

                label1.Hide();
                textBox2.Hide();
                button2.Hide();
            }
            else
            {
                label1.Show();
                textBox2.Show();
                button2.Show();

                button1.Text = "Guardar/Salir";

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Salir")
            {
                this.Close();
            }
            else
            {

                int paso = ultimo_numero();
                Tabla = textBox1.Text;
                if (Tabla == "" || Tabla == string.Empty)
                {
                    MessageBox.Show("Ingrese Nombre de Tabla FR");
                }
                else
                {
                    consulta = richTextBox1.Text;
                    string usuario = "TURCIOSI";
                    if (consulta == "" || consulta == string.Empty)
                    {
                        consulta = null;
                    }

                    string tipo_sinc = comboBox1.Text;
                    if (tipo_sinc == "" || tipo_sinc == string.Empty)
                    {
                        MessageBox.Show("Seleccione el Tipo de sincronizacion para esta consulta");
                    }
                    else
                    {
                        int parametros = 0;
                        if (checkBox1.Checked)
                        {
                            parametros = 1;

                        }

                        string fecha = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");

                        con.conectar("DM");

                        SqlCommand cmd = new SqlCommand("INSERT INTO [DM].[CORRECT].[Sincronizacion_DM] ([PASO],[TABLA],[CONSULTA],[ESTADO],[USUARIO_CREA],[FECHA_CREA],[TIPO_SINC],[PARAMETROS])  VALUES (@PASO,@TABLA,@CONSULTA,@ESTADO,@USUARIO_CREA,@FECHA_CREA,@TIPO_SINC,@PARAMETROS)", con.condm);
                        cmd.Parameters.AddWithValue("@PASO", paso);
                        cmd.Parameters.AddWithValue("@TABLA", Tabla);
                        cmd.Parameters.AddWithValue("@CONSULTA", consulta);
                        cmd.Parameters.AddWithValue("@ESTADO", "A");
                        cmd.Parameters.AddWithValue("@USUARIO_CREA", usuario);
                        cmd.Parameters.AddWithValue("@FECHA_CREA", fecha);
                        cmd.Parameters.AddWithValue("@TIPO_SINC", tipo_sinc);
                        cmd.Parameters.AddWithValue("@PARAMETROS", parametros);
                        cmd.ExecuteNonQuery();

                        con.Desconectar("DM");

                        this.Close();
                    }
                }
            }
        }


        private int  ultimo_numero()
        {
            int numero_ultimo = 0;
            con.conectar("DM");

            SqlCommand cmd = new SqlCommand("SELECT TOP (1) PASO  FROM [DM].[CORRECT].[Sincronizacion_DM] order by PASO desc", con.condm);
            SqlDataReader dr = cmd.ExecuteReader();


            while (dr.Read())
            {
                numero_ultimo = Convert.ToInt32(dr["PASO"]);
            }
            dr.Close();
            con.Desconectar("DM");
            
            nuevo_num = numero_ultimo + 1;

            return nuevo_num;
        }

        private void v(object sender, EventArgs e)
        {

        }

        private void nueva_tabla_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Escape)
            {

                MessageBoxButtons bt2 = MessageBoxButtons.YesNo;

                DialogResult resulta = MessageBox.Show("ESTA SEGURO QUE DESEA SALIR SE PERDERA LA INFORMACION", "INGRESO TABLA SINCRONIZACION", bt2);
                if (resulta == DialogResult.Yes)
                {
                    this.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int parametros = 0;
            string parametro_value;

            if (checkBox1.Checked)
            {
                parametros = 1;
            }

            if (parametros >= 1)
            {
                parametro_value = textBox2.Text;

                if (parametro_value == string.Empty || parametro_value == null || parametro_value == "")
                {
                    MessageBox.Show("Debe Ingresar un valor en los  parametros");
                }
                else
                {
                    prueba(richTextBox1.Text, textBox2.Text,parametros);

                }
            }

            else
            {
                prueba(richTextBox1.Text, null,0);
            }
        }

        public void prueba(string consula, string param, int parametros)
        {

                con.conectar("EX");
                SqlCommand cmd = new SqlCommand(consula, con.conex);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                if (parametros >= 1)
                {
                    cmd.Parameters.AddWithValue("@Handheld", param);
                }

                da.Fill(intermedia);
                dataGridView1.DataSource = intermedia;
                con.Desconectar("EX");

              
            

        }
    }
}
