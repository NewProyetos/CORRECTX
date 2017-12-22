using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Diagnostics;

namespace Sinconizacion_EXactus.CORECTX_APP.Informatica
{
    public partial class Administracion_Empresas : Form
    {
        public Administracion_Empresas()
        {
            InitializeComponent();
        }
        conexionXML con = new conexionXML();
        String Selected_File_carga;
        Byte[] bindata = new byte[0];
        Int32 numero = 0;
        Int32 convimg;

        private void Administracion_Empresas_Load(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            linkLabel1.Enabled = false;

            //textBox2.AutoCompleteCustomSource = AutocompleteRuta.AutocompleteEMPRESAS();
            //textBox2.AutoCompleteMode = AutoCompleteMode.Suggest;
            //textBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            linkLabel1.Enabled = true;
            toolStripButton2.Enabled = false;
            toolStripButton3.Enabled = false;

            con.conectar("DM");
            SqlCommand cm1 = new SqlCommand("SELECT TOP (1) [ID]  FROM [DM].[CORRECT].[EMPRESAS]  Order by ID Desc", con.condm);
            SqlDataReader dr1 = cm1.ExecuteReader();
            while (dr1.Read())
            {
               textBox1.Text = Convert.ToString(dr1["ID"]);             

            }
            con.Desconectar("");



        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog openFileDialog3 = new OpenFileDialog();

            openFileDialog3.AutoUpgradeEnabled = false;


            Selected_File_carga = string.Empty;

            string usuario = SystemInformation.UserName;
            string direccion = @"C:\CORRECT";


            openFileDialog3.InitialDirectory = direccion;
            openFileDialog3.Title = "Select a File";
            openFileDialog3.FileName = string.Empty;
            openFileDialog3.Filter = "JPG|*.jpg|JPEG|*.jpeg|BMP|*.bmp";
            if (openFileDialog3.ShowDialog() == DialogResult.Cancel)
            {

            }
            else
            {
                Selected_File_carga = openFileDialog3.FileName;

                pictureBox1.Image = Image.FromFile(Selected_File_carga);

                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (Selected_File_carga != null)
            {
                FileStream stream = new FileStream(Selected_File_carga, FileMode.Open, FileAccess.Read);
                bindata = new byte[stream.Length];

                stream.Read(bindata, 0, Convert.ToInt32(stream.Length));
            }
            else
            {
                bindata = BitConverter.GetBytes(numero);

            }

            String ID = textBox1.Text;
            String NOMBRE = textBox2.Text;
            String DIRECCION = richTextBox1.Text;
            String NIT = textBox4.Text;
            String REGISTRO = textBox5.Text;
            String RAZON = textBox3.Text;
            String fecha = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");


            con.conectar("DM");
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con.condm;
            cmd.CommandText = "insert into [DM].[CORRECT].[EMPRESAS](NOMRE,Direccion,NIT,Registro,Logo,Fecha_ingreso,Rason_Social) values (@NOMBRE,@Direccion,@NIT,@Registro,@Logo,@Fecha_ingreso,@Rason_Social)";

           // cmd.Parameters.Add("@ID", SqlDbType.NVarChar).Value = ID;
            cmd.Parameters.Add("@NOMBRE", SqlDbType.NVarChar).Value = NOMBRE;
            cmd.Parameters.Add("@Direccion", SqlDbType.NVarChar).Value = DIRECCION;
            cmd.Parameters.Add("@NIT", SqlDbType.NVarChar).Value = NIT;
            cmd.Parameters.Add("@Registro", SqlDbType.NVarChar).Value = REGISTRO;
            cmd.Parameters.Add("@Rason_Social", SqlDbType.NVarChar).Value = RAZON;
            cmd.Parameters.AddWithValue("@Logo", bindata);
            cmd.Parameters.Add("@Fecha_ingreso", SqlDbType.NVarChar).Value = fecha;
            

            cmd.ExecuteNonQuery();

            con.Desconectar("DM");



        }


        private void Carga_datos()
        {

            con.conectar("DM");


            SqlCommand cm1 = new SqlCommand("SELECT  [ID],[NOMRE],[Direccion],[NIT],[Registro],[fecha_ingreso],[Rason_Social]  FROM [DM].[CORRECT].[EMPRESAS] Where NOMRE ='" + textBox2.Text + "' ", con.condm);
            SqlDataReader dr1 = cm1.ExecuteReader();
            while (dr1.Read())
            {
                
                textBox1.Text = Convert.ToString(dr1["ID"]);
                textBox3.Text = Convert.ToString(dr1["Rason_Social"]);
                textBox4.Text = Convert.ToString(dr1["NIT"]);
                textBox5.Text = Convert.ToString(dr1["Registro"]);
                richTextBox1.Text = Convert.ToString(dr1["Direccion"]);
                
            }


            dr1.Close();


            SqlCommand cm2 = new SqlCommand("SELECT [Logo] FROM [DM].[CORRECT].[EMPRESAS] WHERE NOMRE = '" + textBox2.Text + "'", con.condm);
            SqlDataAdapter da = new SqlDataAdapter(cm2);
            DataSet ds = new DataSet("Logo");
            da.Fill(ds, "Logo");


            if (toolStripButton2.Enabled == false)
            {

            }
            else
            {

                int cantidad = (ds.Tables["Logo"].Columns.Count);

                if (cantidad >= 1)
                {

                    DataRow dr = ds.Tables["Logo"].Rows[0];



                    foreach (DataColumn colum in dr.Table.Columns)
                    {
                        if (!dr.IsNull(colum))
                        {

                            byte[] foto = new byte[0];
                            foto = (byte[])dr["Logo"];


                            MemoryStream ms = new MemoryStream(foto);

                            convimg = BitConverter.ToInt32(foto, 0);

                            if (convimg == 0)
                            {

                            }
                            else
                            {
                                pictureBox1.Image = System.Drawing.Bitmap.FromStream(ms);
                                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                            }
                        }
                        else
                        {

                        }
                    }
                }

                con.Desconectar("DM");


            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if(textBox2.Text == "") 
            {

            }
            else
            {
            Carga_datos();
            }
        }



    }
}
