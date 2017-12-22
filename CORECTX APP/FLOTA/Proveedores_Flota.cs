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
    public partial class Proveedores_Flota : Form
    {
        public Proveedores_Flota()
        {
            InitializeComponent();
        }
        DataTable dt2 = new DataTable();
        DataTable dt1 = new DataTable();
        //Conexion2 coned = new Conexion2();
        conexionXML con = new conexionXML();
        Int32 process;
        String Usuario;
        String fecha_actual;
        int ID;
        String fecha_ingreso;
        private void Proveedores_Flota_Load(object sender, EventArgs e)
        {
            ID = 0;
            Usuario = "TURCIOSI";
            process = 0;
            dt2.Clear();
            dt1.Clear();
          
            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            //dataGridView2.AutoResizeColumns();
            //dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";

            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
           


            con.conectar("DM");

            string consulta = "SELECT * FROM [DM].[CORRECT].[Proveedores] ORDER BY Registro";
            SqlCommand comando = new SqlCommand(consulta, con.condm);

            SqlDataAdapter adap = new SqlDataAdapter(comando);

            adap.Fill(dt2);
            con.Desconectar("DM");
            dataGridView1.DataSource = dt1;
            dataGridView1.Refresh();
            toolStripButton4.Enabled = false;
            toolStripButton2.Enabled = false;
            toolStripButton3.Enabled = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dt2.DefaultView.RowFilter = string.Format("Convert(NOMBRE,'System.String') like '%{0}%'",this.textBox1.Text);
            dataGridView1.DataSource = dt2;
        }

       

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            

            DataGridViewRow row = dataGridView1.CurrentRow;

            ID = Convert.ToInt32(row.Cells[0].Value);
            textBox1.Text = row.Cells[1].Value + "";
            textBox2.Text = row.Cells[2].Value + "";
            textBox3.Text = row.Cells[3].Value + "";
            textBox4.Text = row.Cells[4].Value + "";
            textBox5.Text = row.Cells[5].Value + "";
            textBox6.Text = row.Cells[7].Value + "";
            textBox7.Text = row.Cells[8].Value + "";
            fecha_ingreso = row.Cells[6].Value + "";


            toolStripButton4.Enabled = true;
            toolStripButton2.Enabled = true;
            
            }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            process = 2;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
            textBox6.Enabled = true;
            textBox7.Enabled = true;
            toolStripButton3.Enabled = true;
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            int cont =  dataGridView1.Rows.Count;
            if (cont >= 1)
            {
                
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            process = 1;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
            textBox6.Enabled = true;
            textBox7.Enabled = true;
            toolStripButton3.Enabled = true;
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            Proveedores_Flota_Load(null, null);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (ID == 0)
            {
                MessageBox.Show("SELECCIONE El PROVEEDOR QUE DECEA ELIMINAR");
            }
            else
            {

                con.conectar("DM");
                SqlCommand cmd = new SqlCommand("DELETE[DM].[CORRECT].[Proveedores]WHERE ID = '" + ID + "'", con.condm);

                cmd.ExecuteNonQuery();
                con.Desconectar("DM");

                MessageBox.Show("PROVEEDOR "+textBox1.Text+" SE ELIMINO CORRECTAMENTE");
                Proveedores_Flota_Load(null, null);
            }

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            fecha_actual = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            if (process == 1)
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("Ingrese el Nombre");
                    textBox1.Focus();
                }

                else 
                {
                    if (textBox2.Text == "")
                    {
                    MessageBox.Show("Ingrese el Contacto");
                    textBox2.Focus();
                    }

                    else
                    {
                      if (textBox3.Text == "")
                            {
                        MessageBox.Show("Ingrese Direccion");
                         textBox3.Focus();
                              }
                          else
                            {
                                if (textBox4.Text == "")
                                {
                                    MessageBox.Show("Ingrese Telefono de Proveedor");
                                    textBox4.Focus();
                                }
                                else
                                {
                                    if (textBox5.Text == "")
                                    {
                                        MessageBox.Show("Ingrese E-MAIL de Proveedor");
                                        textBox5.Focus();

                                    }
                                    else
                                    {
                                        if (textBox6.Text == "")
                                        {
                                            MessageBox.Show("Ingrese Numero de Registro Fiscal","ELIMINACION PROVEEDOR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                            textBox6.Focus();
                                        }
                                        else
                                        {
                                            if (textBox7.Text == "")
                                            {
                                                MessageBox.Show("Ingrese Numero de NIT");
                                                textBox7.Focus();
                                            }
                                            else
                                            {




                                                con.conectar("DM");

                                                SqlCommand cmd = new SqlCommand();
                                                cmd.Connection = con.condm;
                                                cmd.CommandText = "INSERT INTO [DM].[CORRECT].[Proveedores] ([Nombre],[Contacto],[Direccion],[Telefono],[Email],[Fecha_Ingreso],[Registro],[NIT],[Usuario_update],[Fecha_Update])  VALUES (@Nombre,@Contacto,@Direccion,@Telefono,@Email,@Fecha_Ingreso,@Registro,@NIT,@Usuario_update,@Fecha_Update)";
                                                    cmd.Parameters.Add("@Nombre",SqlDbType.NVarChar).Value = textBox1.Text.ToUpper();
                                                cmd.Parameters.Add("@Contacto",SqlDbType.NVarChar).Value = textBox2.Text.ToUpper();
                                                cmd.Parameters.Add("@Direccion",SqlDbType.NVarChar).Value = textBox3.Text.ToUpper();
                                                cmd.Parameters.Add("@Telefono",SqlDbType.NVarChar).Value = textBox4.Text.ToUpper();
                                                cmd.Parameters.Add("@Email",SqlDbType.NVarChar).Value = textBox5.Text.ToUpper();
                                                cmd.Parameters.Add("@Fecha_Ingreso",SqlDbType.NVarChar).Value = fecha_actual;
                                                cmd.Parameters.Add("@Registro",SqlDbType.NVarChar).Value = textBox6.Text.ToUpper();
                                                cmd.Parameters.Add("@NIT",SqlDbType.NVarChar).Value = textBox7.Text;
                                                 cmd.Parameters.Add("@Usuario_update",SqlDbType.NVarChar).Value = Usuario;
                                                cmd.Parameters.Add("@Fecha_Update",SqlDbType.NVarChar).Value = fecha_actual;
                                               
                                               

                                                cmd.ExecuteNonQuery();
                                                con.Desconectar("DM");

                                                MessageBox.Show("PROVEEDOR " + textBox1.Text + " SE INGRESO CORRECTAMENTE","INGRESO PROVEEDOR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                Proveedores_Flota_Load(null, null);
                                            
                                            
                                            }
                                        }

                                    }
                                
                                }

                             }
                    
                    }
                }
             



            }
            else
                if (process == 2)
                {

                    if (textBox1.Text == "")
                    {
                        MessageBox.Show("Ingrese el Nombre");
                        textBox1.Focus();
                    }

                    else
                    {
                        if (textBox2.Text == "")
                        {
                            MessageBox.Show("Ingrese el Contacto");
                            textBox2.Focus();
                        }

                        else
                        {
                            if (textBox3.Text == "")
                            {
                                MessageBox.Show("Ingrese Direccion");
                                textBox3.Focus();
                            }
                            else
                            {
                                if (textBox4.Text == "")
                                {
                                    MessageBox.Show("Ingrese Telefono de Proveedor");
                                    textBox4.Focus();
                                }
                                else
                                {
                                    if (textBox5.Text == "")
                                    {
                                        MessageBox.Show("Ingrese E-MAIL de Proveedor");
                                        textBox5.Focus();

                                    }
                                    else
                                    {
                                        if (textBox6.Text == "")
                                        {
                                            MessageBox.Show("Ingrese Numero de Registro Fiscal", "ELIMINACION PROVEEDOR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                            textBox6.Focus();
                                        }
                                        else
                                        {
                                            if (textBox7.Text == "")
                                            {
                                                MessageBox.Show("Ingrese Numero de NIT");
                                                textBox7.Focus();
                                            }
                                            else
                                            {
                                                                                      


                                                con.conectar("DM");
                                                SqlCommand cmd = new SqlCommand("UPDATE [DM].[CORRECT].[Proveedores] SET [Nombre] ='"+textBox1.Text+"' ,[Contacto] = '"+textBox2.Text+"',[Direccion] = '"+textBox3.Text+"',[Telefono] = '"+textBox4.Text+"',[Email] = '"+textBox5.Text+"',[Fecha_Ingreso]= '"+fecha_ingreso+"',[Registro] = '"+textBox6.Text+"',[NIT] = '"+textBox7.Text+"',[Usuario_update] = '"+Usuario+"',[Fecha_Update] = '"+fecha_actual+"' where Registro = '"+textBox6.Text+"' ", con.condm);

                                                cmd.ExecuteNonQuery();
                                                con.Desconectar("DM");


                                                                                     

                                                MessageBox.Show("PROVEEDOR " + textBox1.Text + " SE ACTUALIZO CORRECTAMENTE", "INGRESO PROVEEDOR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                Proveedores_Flota_Load(null, null);


                                            }
                                        }

                                    }

                                }

                            }

                        }
                    }
             

                }
        }

        

      

    }
}
