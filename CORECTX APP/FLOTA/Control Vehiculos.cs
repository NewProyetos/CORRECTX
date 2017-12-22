using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Globalization;



namespace Sinconizacion_EXactus
{
    public partial class Control_Vehiculos : Form
    {
        public Control_Vehiculos()
        {
            InitializeComponent();
        }

        Int32 tipo;
        conexionXML con = new conexionXML();
       // Conexion2 coned = new Conexion2();
        DataTable dt = new DataTable();
        String Selected_File_carga;
        String fecha_actual;
        String ID;
        String PLACA, AGENCIA, TIPO_UNIDAD, TIPO_COBUSTIBLE, MARCA, COLOR, CLASE, TIPO, AÑO, MODELO, CAPACIDAD, PROPIDAD, ASEGURADORA, CHASIS, MOTOR, RASTREO, CECO, fecha_update, Usuario_update,Alias,Tiempo_Manto,Mes_Refrenda,Distribucion,Comentarios;
        Byte[] bindata = new byte[0];
        byte[] foto = new byte[0];
        Int32 numero = 0;
        Int32 convimg;
        public String Activo;


        public bool vacio;

        private void Control_Vehiculos_Load(object sender, EventArgs e)
        {
            CultureInfo currentUICulture = CultureInfo.CreateSpecificCulture("es-ES");
            //var months = System.Globalization.DateTimeFormatInfo.InvariantInfo.MonthNames;
           var months = currentUICulture.DateTimeFormat.MonthNames;
            comboBox5.Items.AddRange(months);
            
            toolStripButton4.Enabled = false;
            groupBox4.Hide();

            deshabiliar();

            carga_placas();

            toolStripButton3.Enabled = false;

            if (tipo == 0)
            {
                if (comboBox1.Text == "")
                {

                    toolStripButton2.Enabled = false;
                }

            }
            else
            {
               comboBox1.Enabled = true;
            
            }
        }


        private void carga_datos()
        {
            
            
            con.conectar("DM");

            
            SqlCommand cm1 = new SqlCommand("SELECT [COD_ID],[AGENCIA],[TIPO_UNIDAD],[TIPO_COBUSTIBLE],[MARCA],[COLOR],[CLASE],[TIPO],[AÑO],[MODELO],[CAPACIDAD],[PROPIDAD],[ASEGURADORA],[CHASIS],[MOTOR],[RASTREO],[CECO],[Imagen],[Alias],[Tiempo_Manto],[Mes_Refrenda],[Distribucion],[Comentarios],[Activo] FROM [DM].[CORRECT].[VEHICULOS] WHERE PLACA = '" + comboBox1.Text + "'", con.condm);
            SqlDataReader dr1 = cm1.ExecuteReader();
            while (dr1.Read())
            {
                
                ID = Convert.ToString(dr1["COD_ID"]);
                comboBox4.Text = Convert.ToString(dr1["AGENCIA"]);
                textBox2.Text = Convert.ToString(dr1["COLOR"]);
                textBox3.Text = Convert.ToString(dr1["MODELO"]);
                textBox4.Text = Convert.ToString(dr1["AÑO"]);
                textBox5.Text = Convert.ToString(dr1["TIPO"]);
                textBox6.Text = Convert.ToString(dr1["CLASE"]);
                textBox7.Text = Convert.ToString(dr1["MARCA"]);
                textBox9.Text = Convert.ToString(dr1["TIPO_UNIDAD"]);
                textBox10.Text = Convert.ToString(dr1["CAPACIDAD"]);
                textBox11.Text = Convert.ToString(dr1["PROPIDAD"]);
                textBox12.Text = Convert.ToString(dr1["ASEGURADORA"]);
                textBox13.Text = Convert.ToString(dr1["CHASIS"]);
                textBox14.Text = Convert.ToString(dr1["MOTOR"]);
                textBox15.Text = Convert.ToString(dr1["CECO"]);
                textBox16.Text = Convert.ToString(dr1["Alias"]);
                textBox1.Text = Convert.ToString(dr1["Tiempo_Manto"]);

                comboBox3.Text = Convert.ToString(dr1["TIPO_COBUSTIBLE"]);
                comboBox2.Text = Convert.ToString(dr1["RASTREO"]);
                comboBox5.Text = Convert.ToString(dr1["Mes_Refrenda"]);
                comboBox6.Text = Convert.ToString(dr1["Distribucion"]);
                richTextBox1.Text = Convert.ToString(dr1["Comentarios"]);
                Activo = Convert.ToString(dr1["Activo"]);

                if (Activo == "S")
                {
                    checkBox1.Checked = true;
                }
                else if (Activo == "N")
                {
                    checkBox1.Checked = false;
                }
             }
           

            dr1.Close();


            SqlCommand cm2 = new SqlCommand("SELECT [Imagen] FROM [DM].[CORRECT].[VEHICULOS] WHERE PLACA = '" + comboBox1.Text + "'", con.condm);
            SqlDataAdapter da = new SqlDataAdapter(cm2);
            DataSet ds = new DataSet("Imagen");
            da.Fill(ds, "Imagen");
            

            
            
            
           int cantidad =  ds.Tables["Imagen"].Columns.Count;

           if (cantidad >= 1)
           {
               DataRow dr = ds.Tables["Imagen"].Rows[0];



               foreach (DataColumn colum in dr.Table.Columns)
               {
                   if (!dr.IsNull(colum))
                   {

                      
                       foto = (byte[])dr["Imagen"];
                      // bindata = (byte[])dr["Imagen"];

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

        private void carga_placas()
        {
            con.conectar("DM");

            SqlCommand cm2 = new SqlCommand("SELECT [PLACA] FROM [DM].[CORRECT].[VEHICULOS]", con.condm);
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
                comboBox1.Items.Add(dr2["PLACA"]);


            }
            dr2.Close();
            con.Desconectar("DM");
           
            //comboBox1.DataSource = AutocompleteRuta.AutocompleteRutas.PLACAS();
            //comboBox1.DisplayMember = "PLACA";
            //comboBox1.ValueMember = "PLACA";
            comboBox1.Text = "";
            comboBox1.AutoCompleteCustomSource = AutocompleteRuta.AutocompleteRutas.AutocompletePLACA();
            comboBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            
            

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            carga_datos();
            deshabiliar();
           
            if (comboBox1.Text == "")
            {

                toolStripButton2.Enabled = false;
                
            }
            else 
            {
                toolStripButton2.Enabled = true;
                toolStripButton4.Enabled = true;
            }
        }

        private void deshabiliar()
        {
            button2.Enabled = false;
            //comboBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            textBox9.Enabled = false;
            textBox10.Enabled = false;
            textBox11.Enabled = false;
            textBox12.Enabled = false;
            textBox13.Enabled = false;
            textBox14.Enabled = false;
            textBox15.Enabled = false;
            linkLabel1.Enabled = false;

            comboBox2.Enabled = false;
            comboBox3.Enabled = false;
            comboBox4.Enabled = false;
            comboBox5.Enabled = false;
        
        
        }

        private void habiliar()
        {
            button2.Enabled = true;
            comboBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
            textBox6.Enabled = true;
            textBox7.Enabled = true;
            textBox9.Enabled = true;
            textBox10.Enabled = true;
            textBox11.Enabled = true;
            textBox12.Enabled = true;
            textBox13.Enabled = true;
            textBox14.Enabled = true;
            textBox15.Enabled = true;
            linkLabel1.Enabled = true;
            comboBox2.Enabled = true;
            comboBox3.Enabled = true;


        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            tipo = 1;
            pictureBox1.Image = null;    
            habiliar();

            groupBox4.Show();

            //comboBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            
            textBox9.Clear();
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();
            textBox14.Clear();
            textBox15.Clear();

            comboBox1.Text = "";
            comboBox1.Enabled = false;
            toolStripButton4.Enabled = false;
            comboBox2.Text = "";
            comboBox3.Text = "";
            bindata = null;
            
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            tipo = 2;
            if (comboBox1.Text == "")
            {

            }
            else
            {
                habiliar();
               
                comboBox1.Enabled = false;
                toolStripButton3.Enabled = true;
                textBox8.Text = comboBox1.Text;
                bindata = null;
            }

            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
             

            OpenFileDialog openFileDialog3 = new OpenFileDialog();

            openFileDialog3.AutoUpgradeEnabled = false;


            Selected_File_carga = string.Empty;
          
           string  usuario = SystemInformation.UserName;
           string direccion = @"C:\";

            
            openFileDialog3.InitialDirectory = direccion;
            openFileDialog3.Title = "Select a File";
            openFileDialog3.FileName = string.Empty;
            openFileDialog3.Filter = "JPG|*.jpg|JPEG|*.jpeg|BMP|*.bmp";
            if (openFileDialog3.ShowDialog() == DialogResult.Cancel)
            {
                Selected_File_carga = "";
            }
            else
            {
                Selected_File_carga = openFileDialog3.FileName;

                pictureBox1.Image = Image.FromFile(Selected_File_carga);
                
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            toolStripButton3.Enabled = true;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
           

        }

        private void button1_Click(object sender, EventArgs e)
        {
            deshabiliar();
            groupBox4.Hide();
            
            comboBox1.Enabled = true;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
           
            
            fecha_actual = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
           
            PLACA = textBox8.Text;
            AGENCIA = comboBox1.Text;
            TIPO_UNIDAD = textBox9.Text;
            TIPO_COBUSTIBLE = comboBox3.Text;
            MARCA = textBox7.Text;
            COLOR = textBox2.Text;
            CLASE = textBox6.Text;
            TIPO = textBox5.Text;
            MODELO = textBox3.Text;
            AÑO = textBox4.Text;
            CAPACIDAD = textBox10.Text;
            PROPIDAD = textBox11.Text;
            ASEGURADORA = textBox12.Text;
            CHASIS = textBox13.Text;
            MOTOR = textBox14.Text;
            RASTREO = comboBox2.Text;
            CECO = textBox15.Text;
            fecha_update = fecha_actual;
            if (checkBox1.Checked)
            {
                Activo = "S";
            }

            else
            {
                Activo = "N";
            }

            Alias = textBox16.Text;
            Tiempo_Manto = textBox1.Text;
            Mes_Refrenda = comboBox5.Text;
            Distribucion = comboBox6.Text;
            Comentarios = richTextBox1.Text;

            //Usuario_update = Login.usuario.ToUpper();
            Usuario_update = Login.usuario.ToUpper();


            if (string.IsNullOrEmpty(Selected_File_carga) )
            {
               // bindata = BitConverter.GetBytes(numero);
                bindata = foto;

                
            }
            else
            {
                FileStream stream = new FileStream(Selected_File_carga, FileMode.Open, FileAccess.Read);
                bindata = new byte[stream.Length];
                stream.Read(bindata, 0, Convert.ToInt32(stream.Length));
            
            }

           
                if (tipo == 1)
                {
                    if (existe_Placa(Convert.ToInt32(PLACA)))
                    {
                        MessageBox.Show("PLACA YA EXISTE");
                    }
                    else
                    {


                    {
                        MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
                        DialogResult result = MessageBox.Show("SE INGRESARA NUEVO VEHICULO PLACA: " + PLACA + " ", "INGRESO DE VEHICULOS", bt1, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        if (result == DialogResult.Yes)
                        {
                            if (textBox8.Text == "")
                            {
                                MessageBox.Show("INGRESE NUEVO NUMERO DE PLACA");
                                textBox8.Focus();


                            }


                            else
                            {


                                con.conectar("DM");
                                SqlCommand cmd = new SqlCommand();
                                cmd.Connection = con.condm;
                                cmd.CommandText = "insert into [DM].[CORRECT].[VEHICULOS](PLACA,AGENCIA,TIPO_UNIDAD,TIPO_COBUSTIBLE,MARCA,COLOR,CLASE,TIPO,AÑO,MODELO,CAPACIDAD,PROPIDAD,ASEGURADORA,CHASIS,MOTOR,RASTREO,CECO,Imagen,fecha_update,Usuario_update,Alias,Tiempo_Manto,Mes_Refrenda,Distribucion,Comentarios,Activo) values (@PLACA,@AGENCIA,@TIPO_UNIDAD,@TIPO_COBUSTIBLE,@MARCA,@COLOR,@CLASE,@TIPO,@AÑO,@MODELO,@CAPACIDAD,@PROPIDAD,@ASEGURADORA,@CHASIS,@MOTOR,@RASTREO,@CECO,@Imagen,@fecha_update,@Usuario_update,@Alias,@Tiempo_Manto,@Mes_Refrenda,@Distribucion,@Comentarios,@Activo)";

                                cmd.Parameters.Add("@PLACA", SqlDbType.NVarChar).Value = PLACA;
                                cmd.Parameters.Add("@AGENCIA", SqlDbType.NVarChar).Value = AGENCIA;
                                cmd.Parameters.Add("@TIPO_UNIDAD", SqlDbType.NVarChar).Value = TIPO_UNIDAD;
                                cmd.Parameters.Add("@TIPO_COBUSTIBLE", SqlDbType.NVarChar).Value = TIPO_COBUSTIBLE;
                                cmd.Parameters.Add("@MARCA", SqlDbType.NVarChar).Value = MARCA;
                                cmd.Parameters.Add("@COLOR", SqlDbType.NVarChar).Value = COLOR;
                                cmd.Parameters.Add("@CLASE", SqlDbType.NVarChar).Value = CLASE;
                                cmd.Parameters.Add("@TIPO", SqlDbType.NVarChar).Value = TIPO;
                                cmd.Parameters.Add("@AÑO", SqlDbType.NVarChar).Value = AÑO;
                                cmd.Parameters.Add("@MODELO", SqlDbType.NVarChar).Value = MODELO;
                                cmd.Parameters.Add("@CAPACIDAD", SqlDbType.NVarChar).Value = CAPACIDAD;
                                cmd.Parameters.Add("@PROPIDAD", SqlDbType.NVarChar).Value = PROPIDAD;
                                cmd.Parameters.Add("@ASEGURADORA", SqlDbType.NVarChar).Value = ASEGURADORA;
                                cmd.Parameters.Add("@CHASIS", SqlDbType.NVarChar).Value = CHASIS;
                                cmd.Parameters.Add("@MOTOR", SqlDbType.NVarChar).Value = MOTOR;
                                cmd.Parameters.Add("@RASTREO", SqlDbType.NVarChar).Value = RASTREO;
                                cmd.Parameters.Add("@CECO", SqlDbType.NVarChar).Value = CECO;
                                cmd.Parameters.Add("@Alias", SqlDbType.NVarChar).Value = Alias;
                                cmd.Parameters.Add("@Tiempo_Manto", SqlDbType.NVarChar).Value = Tiempo_Manto;
                                cmd.Parameters.Add("@Mes_Refrenda", SqlDbType.NVarChar).Value = Mes_Refrenda;
                                cmd.Parameters.Add("@Distribucion", SqlDbType.NVarChar).Value = Distribucion;
                                cmd.Parameters.Add("@Comentarios", SqlDbType.NVarChar).Value = Comentarios;
                                cmd.Parameters.Add("@Activo", SqlDbType.NVarChar).Value = Activo;
                                
                            
                                cmd.Parameters.AddWithValue("@Imagen", bindata);
                                
                                cmd.Parameters.Add("@fecha_update", SqlDbType.NVarChar).Value = fecha_update;
                                cmd.Parameters.Add("@Usuario_update", SqlDbType.NVarChar).Value = Usuario_update;

                                cmd.ExecuteNonQuery();

                                con.Desconectar("DM");

                                Control_Vehiculos_Load(null, null);
                            }
                        }
                    }
                }
            }
            if (tipo == 2)
            { 
               MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show("SE CAMBIARAN DATOS DE VEHICULO PLACA: " + PLACA + " DESEA REALIZARLO ", "INGRESO DE VEHICULOS", bt1, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {
                    if (textBox8.Text == "")
                    {
                        MessageBox.Show("INGRESE NUEVO NUMERO DE PLACA");
                        textBox8.Focus();
                    }
                    else
                    {

                        con.conectar("DM");
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = con.condm;
                        cmd.CommandText = "UPDATE [DM].[CORRECT].[VEHICULOS] SET PLACA=@PLACA,AGENCIA = @AGENCIA,TIPO_UNIDAD =@TIPO_UNIDAD ,TIPO_COBUSTIBLE =@TIPO_COBUSTIBLE,MARCA=@MARCA,COLOR=@COLOR,CLASE=@CLASE,TIPO=@TIPO,AÑO=@AÑO,MODELO=@MODELO,CAPACIDAD=@CAPACIDAD,PROPIDAD=@PROPIDAD,ASEGURADORA=@ASEGURADORA,CHASIS=@CHASIS,MOTOR=@MOTOR,RASTREO=@RASTREO,CECO=@CECO,Imagen=@Imagen,fecha_update=@fecha_update,Usuario_update=@Usuario_update, Alias = @Alias,Tiempo_Manto = @Tiempo_Manto,Mes_Refrenda=@Mes_Refrenda,Distribucion = @Distribucion,Comentarios= @Comentarios,Activo=@Activo    WHERE COD_ID = '" + ID + "'";

                        cmd.Parameters.Add("@PLACA", SqlDbType.NVarChar).Value = PLACA;
                        cmd.Parameters.Add("@AGENCIA", SqlDbType.NVarChar).Value = AGENCIA;
                        cmd.Parameters.Add("@TIPO_UNIDAD", SqlDbType.NVarChar).Value = TIPO_UNIDAD;
                        cmd.Parameters.Add("@TIPO_COBUSTIBLE", SqlDbType.NVarChar).Value = TIPO_COBUSTIBLE;
                        cmd.Parameters.Add("@MARCA", SqlDbType.NVarChar).Value = MARCA;
                        cmd.Parameters.Add("@COLOR", SqlDbType.NVarChar).Value = COLOR;
                        cmd.Parameters.Add("@CLASE", SqlDbType.NVarChar).Value = CLASE;
                        cmd.Parameters.Add("@TIPO", SqlDbType.NVarChar).Value = TIPO;
                        cmd.Parameters.Add("@AÑO", SqlDbType.NVarChar).Value = AÑO;
                        cmd.Parameters.Add("@MODELO", SqlDbType.NVarChar).Value = MODELO;
                        cmd.Parameters.Add("@CAPACIDAD", SqlDbType.NVarChar).Value = CAPACIDAD;
                        cmd.Parameters.Add("@PROPIDAD", SqlDbType.NVarChar).Value = PROPIDAD;
                        cmd.Parameters.Add("@ASEGURADORA", SqlDbType.NVarChar).Value = ASEGURADORA;
                        cmd.Parameters.Add("@CHASIS", SqlDbType.NVarChar).Value = CHASIS;
                        cmd.Parameters.Add("@MOTOR", SqlDbType.NVarChar).Value = MOTOR;
                        cmd.Parameters.Add("@RASTREO", SqlDbType.NVarChar).Value = RASTREO;
                        cmd.Parameters.Add("@CECO", SqlDbType.NVarChar).Value = CECO;
                        cmd.Parameters.Add("@Alias", SqlDbType.NVarChar).Value = Alias;
                        cmd.Parameters.Add("@Tiempo_Manto", SqlDbType.NVarChar).Value = Tiempo_Manto;
                        cmd.Parameters.Add("@Mes_Refrenda", SqlDbType.NVarChar).Value = Mes_Refrenda;
                        cmd.Parameters.Add("@Distribucion", SqlDbType.NVarChar).Value = Distribucion;
                        cmd.Parameters.Add("@Comentarios", SqlDbType.NVarChar).Value = Comentarios;
                        cmd.Parameters.Add("@Activo", SqlDbType.NVarChar).Value = Activo;

                      
                            //cmd.Parameters.AddWithValue("@Imagen", compresor.comprimir(bindata));
                      


                        cmd.Parameters.Add("@fecha_update", SqlDbType.NVarChar).Value = fecha_update;
                        cmd.Parameters.Add("@Usuario_update", SqlDbType.NVarChar).Value = Usuario_update;

                        cmd.ExecuteNonQuery();

                        con.Desconectar("DM");

                        Control_Vehiculos_Load(null, null);

                    }
                }
            


            }



        }


        private void textvalida(object sender, EventArgs e)
        {

            if (comboBox1.Text == "")
            {
                MessageBox.Show("Ingrese Agencia");
                
            }

            if (textBox2.Text == "")
            {
                MessageBox.Show("Ingrese Color de Vehiculo");
            }
            if (textBox3.Text == "")
            {
                MessageBox.Show("Ingrese Modelo de Vehiculo");
            }
            if (textBox4.Text == "")
            {
                MessageBox.Show("Ingrese Año de Vehiculo");
            }
            if (textBox5.Text == "")
            {
                MessageBox.Show("Ingrese Tipo de Vehiculo");
                
            }
            if (textBox6.Text == "")
            {
                MessageBox.Show("Ingrese Clase de Vehiculo");

            }

            if (textBox7.Text == "")
            {
                MessageBox.Show("Ingrese la Marca del Vehiculo");

            }
            if (textBox8.Text == "")
            {
                MessageBox.Show("Ingrese El numero de Placa");

            }
            if (textBox9.Text == "")
            {
                MessageBox.Show("Ingrese Tipo de Unidad");

            }



        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            Selected_File_carga = null;
        }
        private bool existe_Placa(int placa)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("select COUNT (*) from [DM].[CORRECT].[VEHICULOS] where PLACA ='" + placa + "'", con.condm);
            cmd.Parameters.AddWithValue("placa", Convert.ToInt32(placa));


            int contar = Convert.ToInt32(cmd.ExecuteScalar());
            con.Desconectar("DM");

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
