using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;

namespace Sinconizacion_EXactus.CORECTX_APP.Informatica.Procesos
{
    public partial class Procesos_Exactus : Form
    {
        public Procesos_Exactus()
        {
            InitializeComponent();
        }
        conexionXML con = new conexionXML();
        string empresa;
        string empresasel;
        string Ruta;
        string Bodega;
        DataTable dtfull = new DataTable();
        private void Procesos_Exactus_Load(object sender, EventArgs e)
        {

           

            radioButton2.Checked = true;
            empresa = Login.empresa;
            //  comboBox2.Text = empresa;

            if (radioButton2.Checked)
            {
                comboBox2.Focus();
               
                checkBox3.Enabled = false;
                checkBox4.Enabled = false;
                checkBox5.Enabled = false;
               
            }
            else
                if (radioButton1.Checked)
            {
                groupBox1.Enabled = false;
                groupBox3.Enabled = false;
                comboBox2.Focus();
                checkBox2.Enabled = false;
                checkBox3.Enabled = false;
                checkBox4.Enabled = false;
                checkBox5.Enabled = false;
              

            }
           
            
        }

        private void carga_ruta(string emp)
        {
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.Items.Clear();
            }

            con.conectar("EX");

            SqlCommand cm1 = new SqlCommand("SELECT A.[RUTA] as 'RUTA' FROM [EXACTUS].[ERPADMIN].[RUTA_ASIGNADA_RT] A  where COMPANIA = '"+ emp + "' order by A.[RUTA] ", con.conex);
            SqlDataReader dr1 = cm1.ExecuteReader();
            while (dr1.Read())
            {
                comboBox1.Items.Add(dr1["RUTA"]);

            }
            dr1.Close();
            con.Desconectar("EX");


        }
        private void bodega_ruta(string ruta)
        {
            con.conectar("EX");

            SqlCommand cm1 = new SqlCommand("SELECT [BODEGA]  FROM [EXACTUS].[ERPADMIN].[RUTA_ASIGNADA_RT]  where RUTA = '"+ruta+"'", con.conex);
            SqlDataReader dr1 = cm1.ExecuteReader();
            while (dr1.Read())
            {
                Bodega = Convert.ToString(dr1["BODEGA"]);

            }
            dr1.Close();
            con.Desconectar("EX");
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            empresasel = comboBox2.Text;
            carga_ruta(comboBox2.Text);
            limpiar_imagen();



        } 

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "" || comboBox2.Text == string.Empty || comboBox2.Text == null)
            {
                MessageBox.Show("seleccione una EMPRESA");
                comboBox1.Focus();

            }
            else


            if (comboBox1.Text == "" || comboBox1.Text == string.Empty || comboBox1.Text == null)
            {
                MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show("No se ha seleccionado una Ruta para la Empresa " + comboBox2.Text + " Ejecutar el proceso para todas las Rutas?: " + this.comboBox1.Text + "", "PROCESO CARGA FR", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {

                }
                else
                {
                   // MessageBox.Show("seleccione una Ruta");
                    comboBox1.Focus();
                }
                  
                

            }
            else

                validachek();

            //    if (checkBox1.Checked)
            //    {
            //        Clientes_Worker.RunWorkerAsync();
            //    }
            //if (checkBox2.Checked)
            //{

            //    Invetario_Worker.RunWorkerAsync();
            //}
        }

        private void Clientes_Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
          //  progressBar1.Value = e.ProgressPercentage;
            //label1.Text = Convert.ToString(e.ProgressPercentage);
        }

        private void Clientes_Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("[CORRECT].[PROCESO_CLIENTES_FR_V2]", con.condm);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@RUTA", Ruta);
            cmd.Parameters.AddWithValue("@EMPRESA", empresasel);
            cmd.Parameters.Add("@msg", SqlDbType.VarChar,100);
            cmd.Parameters["@msg"].Direction = ParameterDirection.Output;

            // Clientes_Worker.ReportProgress(50);

            pictureBox5.Image = Properties.Resources.ajaxloaderc;

           // pictureBox1.Refresh();

            cmd.ExecuteNonQuery();
            string retunvalue = (string)cmd.Parameters["@msg"].Value;

            //  Thread.Sleep(10000);

            //Clientes_Worker.ReportProgress(100);
            //MessageBox.Show(retunvalue +" EN RUTA" + Ruta);
            if (retunvalue == "OK")
            {
                e.Result = "OK";
            }

            else
            {
                e.Result = "NO";
            }

        }

        private void Clientes_Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            //  label1.Text = Ruta + " OK ";
            if (e.Result == "OK")
            {
                pictureBox5.Image = Properties.Resources.chqeueverde;
            }
            else
            {
                pictureBox5.Image = Properties.Resources.chequerojo;
            }

          

            checkBox1.Checked = false;

            if (radioButton2.Checked)
            {
                validachek();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Ruta = comboBox1.Text;
            bodega_ruta(Ruta);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            groupBox3.Enabled = true;
            checkBox1.Enabled = true;
            checkBox2.Enabled = true;
            checkBox3.Enabled = true;
            checkBox4.Enabled = true;
            checkBox5.Enabled = true;
            checkBox6.Enabled = true;
            checkBox7.Enabled = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = true;
            groupBox3.Enabled = true;


            checkBox3.Enabled = false;
            checkBox4.Enabled = false;
            checkBox5.Enabled = false;
            checkBox6.Enabled = false;
            checkBox6.Enabled = false;

        }

        private void Invetario_Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (Bodega == string.Empty || Bodega == null || Bodega == "")
            {
                MessageBox.Show("No se encontro Bodega para Ruta");
            }
            else
            {

                pictureBox1.Image = Properties.Resources.ajaxloaderc;
                con.conectar("DM");
                SqlCommand cmd = new SqlCommand("[CORRECT].[PROCESO_FR_INVETARIO]", con.condm);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@bodega", Bodega);
                cmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
             //   Clientes_Worker.ReportProgress(50);

                cmd.ExecuteNonQuery();
                string retunvalue = (string)cmd.Parameters["@msg"].Value;

            //    Clientes_Worker.ReportProgress(100);
                //MessageBox.Show(retunvalue + " EN RUTA" + Ruta);

                if (retunvalue == "OK")
                {
                    e.Result = "OK";
                }

                else
                {
                    e.Result = "NO";
                }
            }
        }

        private void Invetario_Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //progressBar1.Value = 100;
            //label1.Text = Ruta + " OK ";
            if (e.Result == "OK")
            {
                pictureBox1.Image = Properties.Resources.chqeueverde;
            }
            else
            {
                pictureBox1.Image = Properties.Resources.chequerojo;
            }




            checkBox2.Checked = false;
            if (radioButton2.Checked)
            {
                validachek();
            }
        }

        private void validachek()
        {
            //progressBar1.Style = ProgressBarStyle.Marquee;
            //progressBar1.MarqueeAnimationSpeed = 50;

            if (checkBox1.Checked)
            {
                Clientes_Worker.RunWorkerAsync();
                
            }
            else
                if (checkBox2.Checked)
            {
                Invetario_Worker.RunWorkerAsync();

            }
            else if (checkBox3.Checked)
            {
             
            }
            else if (checkBox4.Checked)
            {
            }
            else if (checkBox5.Checked)
            {
            }
            else if (checkBox6.Checked)
            {
                Globales_Worker.RunWorkerAsync();
            }
            else if (checkBox7.Checked)
            {
                CXC_Worker.RunWorkerAsync();
            }

        }

        private void Precios_Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            con.conectar("DM");

            pictureBox2.Image = Properties.Resources.ajaxloaderc;

            SqlCommand cmd = new SqlCommand("[CORRECT].[PROCESO_FR_LISTAPRECIO]", con.condm);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
            cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();


            string retunvalue = (string)cmd.Parameters["@msg"].Value;

            if (retunvalue == "OK")
            {
                e.Result = "OK";
            }

            else
            {
                e.Result = "NO";
            }



        }

        private void Precios_Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result == "OK")
            {
                pictureBox2.Image = Properties.Resources.chqeueverde;
            }
            else
            {
                pictureBox2.Image = Properties.Resources.chequerojo;
            }




            checkBox3.Checked = false;
            if (radioButton2.Checked)
            {
                validachek();
            }
        }

        private void CXC_Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            

            pictureBox7.Image = Properties.Resources.ajaxloaderc;

            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("[CORRECT].[PROCESO_FR_CXC]", con.condm);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ruta", Ruta);
            cmd.Parameters.AddWithValue("@empresa", empresasel);
            cmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
            cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();


            string retunvalue = (string)cmd.Parameters["@msg"].Value;

            if (retunvalue == "OK")
            {
                e.Result = "OK";
            }

            else
            {
                e.Result = "NO";
            }

        }

        private void CXC_Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result == "OK")
            {
                pictureBox7.Image = Properties.Resources.chqeueverde;
            }
            else
            {
                pictureBox7.Image = Properties.Resources.chequerojo;
            }




            checkBox7.Checked = false;
            if (radioButton2.Checked)
            {
                validachek();
            }
        }

        private void Globales_Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            con.conectar("DM");

            pictureBox6.Image = Properties.Resources.ajaxloaderc;

            SqlCommand cmd = new SqlCommand("[CORRECT].[PROCESO_FR_GLOBALES]", con.condm);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
            cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();


            string retunvalue = (string)cmd.Parameters["@msg"].Value;

            if (retunvalue == "OK")
            {
                e.Result = "OK";
            }

            else
            {
                e.Result = "NO";
            }

        }

        private void Globales_Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result == "OK")
            {
                pictureBox6.Image = Properties.Resources.chqeueverde;
            }
            else
            {
                pictureBox6.Image = Properties.Resources.chequerojo;
            }




            checkBox7.Checked = false;
            if (radioButton2.Checked)
            {
                validachek();
            }
        }

        private void limpiar_imagen()
        {
            pictureBox1.Image = Properties.Resources.transparente;
            pictureBox2.Image = Properties.Resources.transparente;
            pictureBox3.Image = Properties.Resources.transparente;
            pictureBox4.Image = Properties.Resources.transparente;
            pictureBox5.Image = Properties.Resources.transparente;
            pictureBox6.Image = Properties.Resources.transparente;
            pictureBox7.Image = Properties.Resources.transparente;
            

        }

    }
}
