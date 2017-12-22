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
    public partial class Forma_pago_flota : Form
    {
        public Forma_pago_flota()
        {
            InitializeComponent();
           
        }
        DataTable dt1 = new DataTable();
       // Conexion2 coned = new Conexion2();
        conexionXML con = new conexionXML();
        Int32 ID;
        Int32 process;
        String fecha_actual;
        String Usuario;
        private void Forma_pago_flota_Load(object sender, EventArgs e)
        {
            //Usuario = Login.usuario.ToUpper();
            Usuario ="TURCIOSI" ;
            process = 0;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            comboBox1.Enabled = false;
            textBox4.Enabled = false;
            ID = 0;

            textBox1.Text = "";
            textBox2.Text = "";
            comboBox1.Text = "";
            textBox4.Text = "";

            toolStripButton2.Enabled = false;
            toolStripButton3.Enabled = false;
            toolStripButton4.Enabled = false;
            toolStripButton5.Enabled = false;

            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;

            
            con.conectar("DM");
            dt1.Clear();
            string consulta = "SELECT [ID],[TIPO],[NUMERO_TAR],[AGENCIA],[USUARIO],[FECHA] FROM [DM].[CORRECT].[FORMA_PAGO_GAS]";
            SqlCommand comando = new SqlCommand(consulta, con.condm);
            
            SqlDataAdapter adap = new SqlDataAdapter(comando);

            adap.Fill(dt1);
            dataGridView1.DataSource = dt1;
                dataGridView1.Refresh();
                
            con.Desconectar("DM");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            DataGridViewRow row = dataGridView1.CurrentRow;

            ID = Convert.ToInt32(row.Cells[0].Value);
            textBox1.Text = row.Cells[1].Value + "";
            textBox2.Text = row.Cells[2].Value + "";
            comboBox1.Text = row.Cells[3].Value + "";
            textBox4.Text = row.Cells[4].Value + "";
          


            toolStripButton4.Enabled = true;
            toolStripButton2.Enabled = true;
            toolStripButton3.Enabled = true;
            
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            process = 2;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            comboBox1.Enabled = true;
            textBox4.Enabled = true;
            toolStripButton5.Enabled = true;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            process = 1;
            textBox1.Text = "";
            textBox2.Text = "";
            comboBox1.Text = "";
            textBox4.Text = "";

            textBox1.Enabled = true;
            textBox2.Enabled = true;
            comboBox1.Enabled = true;
            textBox4.Enabled = true;
            toolStripButton5.Enabled = true;
            toolStripButton4.Enabled = true;

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Forma_pago_flota_Load(null, null);
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            fecha_actual = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            if (process == 1)
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("Ingrese el TIPO DE PAGO");
                    textBox1.Focus();
                }

                else
                {
                    if (textBox2.Text == "")
                    {
                        MessageBox.Show("Ingrese el NUMERO DE TARJETA");
                        textBox2.Focus();
                    }

                    else
                    {
                        if (comboBox1.Text == "")
                        {
                            MessageBox.Show("Seleccione la Agencia");
                            comboBox1.Focus();
                        }
                        else
                        {
                            if (textBox4.Text == "")
                            {
                                MessageBox.Show("Ingrese El Nombre Completo del Usuario");
                                textBox4.Focus();
                            }
                            else
                            {


                                con.conectar("DM");
                                
                                SqlCommand cmd = new SqlCommand();
                                cmd.Connection = con.condm;
                                cmd.CommandText = "INSERT INTO [DM].[CORRECT].[FORMA_PAGO_GAS] ([TIPO],[NUMERO_TAR],[AGENCIA],[USUARIO],[FECHA],[USUARIO_UPDATE]) VALUES (@TIPO,@NUMERO_TAR,@AGENCIA,@USUARIO,@FECHA,@USUARIO_UPDATE)";
                                cmd.Parameters.Add("@TIPO", SqlDbType.NVarChar).Value = textBox1.Text.ToUpper();
                                cmd.Parameters.Add("@NUMERO_TAR", SqlDbType.NVarChar).Value = textBox2.Text.ToUpper();
                                cmd.Parameters.Add("@AGENCIA", SqlDbType.NVarChar).Value = comboBox1.Text.ToUpper();
                                cmd.Parameters.Add("@USUARIO", SqlDbType.NVarChar).Value = textBox4.Text.ToUpper();
                                cmd.Parameters.Add("@FECHA", SqlDbType.NVarChar).Value = fecha_actual;
                                cmd.Parameters.Add("@USUARIO_UPDATE", SqlDbType.NVarChar).Value = Usuario;
                               


                                cmd.ExecuteNonQuery();
                                con.Desconectar("DM");

                                MessageBox.Show("FORMA DE PAGO " + textBox1.Text + " SE INGRESO CORRECTAMENTE", "INGRESO FORMA DE PAGO", MessageBoxButtons.OK, MessageBoxIcon.Information);


                                Forma_pago_flota_Load(null, null);


                            }
                        }
                    }
                }
            }

            else if(process == 2)
            { 
            
             if (textBox1.Text == "")
                {
                    MessageBox.Show("Ingrese el TIPO DE PAGO");
                    textBox1.Focus();
                }

                else
                {
                    if (textBox2.Text == "")
                    {
                        MessageBox.Show("Ingrese el NUMERO DE TARJETA");
                        textBox2.Focus();
                    }

                    else
                    {
                        if (comboBox1.Text == "")
                        {
                            MessageBox.Show("Seleccione la Agencia");
                            comboBox1.Focus();
                        }
                        else
                        {
                            if (textBox4.Text == "")
                            {
                                MessageBox.Show("Ingrese El Nombre Completo del Usuario");
                                textBox4.Focus();
                            }
                            else
                            {


                                con.conectar("DM");
                                SqlCommand cmd = new SqlCommand("UPDATE [DM].[CORRECT].[FORMA_PAGO_GAS] SET [TIPO] ='" + textBox1.Text.ToUpper() + "' ,[NUMERO_TAR] = '" + textBox2.Text + "',[AGENCIA] = '" + comboBox1.Text + "',[USUARIO] = '" + textBox4.Text.ToUpper()+ "',[FECHA] = '" + fecha_actual+ "',[USUARIO_UPDATE]= '" + Usuario + "' WHERE ID = '"+ID+"' ", con.condm);

                                cmd.ExecuteNonQuery();
                                con.Desconectar("DM");

                                

                                

                                MessageBox.Show("FORMA DE PAGO " + textBox1.Text + " SE ACTUALIZO", "ACTUALIZACION FORMA DE PAGO", MessageBoxButtons.OK, MessageBoxIcon.Information);


                                Forma_pago_flota_Load(null, null);


                            }
                        }
                    }
                }
            
            
            
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (ID == 0)
            {
                MessageBox.Show("SELECCIONE El PROVEEDOR QUE DECEA ELIMINAR");
            }
            else
            {

                con.conectar("DM");
                SqlCommand cmd = new SqlCommand("DELETE [DM].[CORRECT].[FORMA_PAGO_GAS] WHERE ID = '" + ID + "'", con.condm);

                cmd.ExecuteNonQuery();
                con.Desconectar("DM");

                MessageBox.Show("TIPO DE PAGO : " + textBox1.Text + " SE ELIMINO CORRECTAMENTE");
                Forma_pago_flota_Load(null, null);
            }
        }
    }
}


        
