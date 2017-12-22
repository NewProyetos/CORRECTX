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
    public partial class Correlativo_process : Form
    {
        public Correlativo_process()
        {
            InitializeComponent();
        }

        conexionXML con = new conexionXML();
       // conexion conecEXAC = new conexion();
        //Conexion2 conecDM = new Conexion2();
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();

        public static string Ruta;
        public static string C_ant;
        public static string C_act;
        public Int32 idx;
        public String corelativo_select;
        public static string fecha_actual;

        private void Correlativo_process_Load(object sender, EventArgs e)
        {
            label5.Hide();
            textBox2.Hide();
            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            //dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ReadOnly = true;


            dataGridView2.Enabled = true;
            dataGridView2.RowHeadersVisible = false;
            //dataGridView1.AutoResizeColumns();
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView2.ReadOnly = true;

            fecha_actual = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            label2.Text = "Correlativo Actual";
            textBox1.ForeColor = Color.Gray;
            button2.Enabled = false;
            
            con.conectar("EX");
            
            SqlCommand cm1 = new SqlCommand("SELECT [RUTA] FROM [EXACTUS].["+Login.empresa+"].[RUTA] where DESCRIPCION = 'PRONTA'ORDER BY RUTA", con.conex);
            SqlDataReader dr1 = cm1.ExecuteReader();


            while (dr1.Read())
            {
                comboBox1.Items.Add("R" + dr1["RUTA"]);
            }

            dr1.Close();
            
            con.Desconectar("EX");
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {


            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Seleccione una Ruta", "ALERTA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                comboBox1.Focus();
            }
            
            else if (this.textBox1.Text == "")
            {
                MessageBox.Show("Ingrese el Correlativo Inicial", "ALERTA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBox1.Focus();
            }

            else  if (this.textBox2.Text == "")
            {
                MessageBox.Show("Ingrese el Correlativo Final","ALERTA",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                textBox2.Focus();
            }
            else
            {
                if (Exists_Correlativo(textBox1.Text, textBox2.Text)) 
                {
                    MessageBox.Show("YA EXISTEN FACTURAS CON ESTOS CORRELATIVOS","ERROR",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                }
                else
                {

                MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show("ESTA SEGURO DE CAMBIAR EL CORRELATIVO: " + C_act.ToUpper() + " DE LA RUTA: " + Ruta.ToUpper() + " POR EL CORRELATIVO " + this.textBox1.Text + " ", "CAMBIO CORRELATIVO PREIMPRESO", bt1, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {
                    if (textBox1.Text == "")
                    {
                        MessageBox.Show("INGRESE NUEVO CORRELATIVO");
                        textBox1.Focus();
                    }
                    else
                    {
                        if (Exists_rt_(Ruta))
                        {
                            try
                            {
                                con.conectar("DM");
                                SqlCommand cmd1 = new SqlCommand("[CORRECT].[INSERT_CORRELATIVO_PROC]", con.condm);
                                cmd1.CommandType = CommandType.StoredProcedure;

                                cmd1.Parameters.AddWithValue("@RUTA", Ruta);
                                cmd1.Parameters.AddWithValue("@CORRELATIVO", textBox1.Text);
                                cmd1.Parameters.AddWithValue("@PROCESS", 0);
                                cmd1.Parameters.AddWithValue("@FECHA_CREA", fecha_actual);
                                cmd1.Parameters.AddWithValue("@USUARIO", Login.usuario.ToUpper());

                                cmd1.ExecuteNonQuery();

                                con.Desconectar("DM");

                                comboBox1_SelectedIndexChanged(null, null);
                            }
                            catch
                            {
                                MessageBox.Show("ERROR AL INGRESAR CORRELATIVO");

                            }

                            try
                            {
                                con.conectar("DM");

                                SqlCommand cmd = new SqlCommand("[CORRECT].[CORRELATIVO_UPDATE]", con.condm);
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@Correlatibo", C_act);
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





                        }
                        else
                        {

                            MessageBoxButtons bt2 = MessageBoxButtons.OK;
                            DialogResult result2 = MessageBox.Show("YA EXISTE UNA SOLICITUD DE CORRELATIVO PARA ESTA RUTA ", "CAMBIO CORRELATIVO PREIMPRESO", bt2, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);


                        }
                    
                    }
                }
                }
            }
        }
        

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Desea Restablecer El Correlativo Anterior: " + C_ant.ToUpper() + " a la Ruta: " + Ruta.ToUpper() + " ", "CAMBIO CORRELATIVO PREIMPRESO", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {

                this.textBox1.Text = C_ant;

            }



        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text = "";
            label5.Hide();
            textBox2.Hide();
            textBox1.ForeColor = Color.Gray;
            label2.Text = "Correlativo Actual";
            Ruta = this.comboBox1.Text;
            dt.Clear();
            dt1.Clear();
            button2.Enabled = false;
            con.conectar("DM");

            SqlCommand cm1 = new SqlCommand("SELECT CORRELATIVO FROM [DM].[CORRECT].[HIST_CORELATIVO]  WHERE RUTA ='" + this.comboBox1.Text + "'", con.condm);
            SqlDataReader dr1 = cm1.ExecuteReader();


            while (dr1.Read())
            {
                C_ant = Convert.ToString(dr1["CORRELATIVO"]);
                label4.Text = C_ant;
            }

            dr1.Close();
            con.Desconectar("DM");

            
            con.conectar("EX");
            
            SqlCommand cm2 = new SqlCommand("SELECT NUM_FAC FROM [EXACTUS].[ERPADMIN].[alSYS_PRM]  where COD_ZON ='" + this.comboBox1.Text + "'", con.conex);
            SqlDataReader dr2 = cm2.ExecuteReader();


            while (dr2.Read())
            {
                C_act = Convert.ToString(dr2["NUM_FAC"]);
                this.textBox1.Text = C_act;
            }

            dr2.Close();


            SqlCommand cm3 = new SqlCommand("SELECT [RUTA],[CORRELATIVO],[FECHA_CREA],[USUARIO] FROM [DM].[CORRECT].[CORRELATIVO_PROC]where PROCESS = '0' and  RUTA = '"+Ruta+"'", con.conex);

            SqlDataAdapter da = new SqlDataAdapter(cm3);

            da.Fill(dt);

            dataGridView1.DataSource = dt;




            SqlCommand cm4 = new SqlCommand("SELECT [RUTA],[CORRELATIVO],[PROCESS],[FECHA_CREA],[USUARIO],[FECHA_UPDATE] FROM [DM].[CORRECT].[CORRELATIVO_PROC]  Where RUTA = '"+Ruta+"'  and PROCESS = '1'", con.conex);

            SqlDataAdapter da1 = new SqlDataAdapter(cm4);

            da1.Fill(dt1);

            dataGridView2.DataSource = dt1;

            
            con.Desconectar("EX");










        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox1.ForeColor = Color.Black;
            label2.Text = "Nuevo Inicial";

            label5.Show();
            textBox2.Show();
        }


        private bool Exists_rt_(string Ruta)
        {

            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("select COUNT (*) from [DM].[CORRECT].[CORRELATIVO_PROC] where RUTA = @ruta and PROCESS = '0'", con.condm);
            cmd.Parameters.AddWithValue("Ruta", Ruta);

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

        private void button2_Click(object sender, EventArgs e)
        {
               MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show("ESTA SEGURO DE ELIMINAR LA SOLICITUD DEL CORRELATIVO: " + corelativo_select + " DE LA RUTA: " + Ruta.ToUpper() + " ", "CAMBIO CORRELATIVO PREIMPRESO", bt1, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {


                    if (corelativo_select == "" && Ruta == "")
                    {
                        MessageBox.Show("SELECCIONE UN CORRELATIVO");
                    }
                    else
                    {

                        con.conectar("DM");
                        SqlCommand cmd = new SqlCommand(" DELETE  [DM].[CORRECT].[CORRELATIVO_PROC] where RUTA = '" + Ruta + "' AND CORRELATIVO = '" + corelativo_select + "' AND PROCESS = '0' ", con.condm);
                        cmd.ExecuteNonQuery();
                        con.Desconectar("DM");

                        comboBox1_SelectedIndexChanged(null, null);
                    }

                }
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
           
           

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            idx = dataGridView1.CurrentRow.Index;
            corelativo_select = Convert.ToString(dataGridView1.Rows[idx].Cells[1].Value);

            if (corelativo_select == "")
            {
                button2.Enabled = false;
            }
            else
            {
                button2.Enabled = true;
            }
        }


        private bool Exists_Correlativo(string inicial , string final)
        {

            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("SELECT COUNT (NUM_PED)  FROM [EXACTUS].[ERPADMIN].[alFAC_ENC_PED]  where NUM_PED between @inicial and @final", con.conex);
            cmd.Parameters.AddWithValue("inicial", inicial);
            cmd.Parameters.AddWithValue("final", final);

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

    
    }
}
