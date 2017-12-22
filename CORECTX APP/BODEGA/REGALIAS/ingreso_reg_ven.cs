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
    public partial class ingreso_reg_ven : Form
    {
        public ingreso_reg_ven()
        {
            InitializeComponent();
        }
        public static string fecha_actual;
       
        public static string Agencia;
        public static string Usuario;
        public int editmode;
        DataTable dt = new DataTable();


        conexionXML con = new conexionXML();
       // Conexion2 coned = new Conexion2();
        //conexion conet = new conexion();
        String cuenta_Actividad;
        String tipo_ruta;

        DataTable dt2 = AutocompleteRuta.AutocompleteRutas.PRODUCTO();
        private void Form12_Load(object sender, EventArgs e)
        {
            label16.Hide();
            comboBox5.Hide();
            editmode = 0;
          
            toolStripButton5.Enabled = false;
            if (Regalias_Vencido.modo == 2)
            {
                dt.Clear();
                dataGridView1.Enabled = true;
                dataGridView1.RowHeadersVisible = false;
                dataGridView1.AutoResizeColumns();
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dataGridView1.ReadOnly = true;
                dataGridView1.Refresh();
                con.conectar("DM");
                pictureBox1.Hide();
                pictureBox2.Hide();
                this.dateTimePicker1.Value = Regalias_Vencido.fechadc;
                this.toolStripButton1.Enabled = false;
                this.toolStripButton2.Enabled = false;
                this.toolStripButton5.Enabled = true;
                this.textBox1.Text = Regalias_Vencido.Documento_fisico;
                this.textBox1.Enabled = false;
                
                
                SqlCommand cmd = new SqlCommand("SELECT Articulo as 'ARTICULO',DescripcionArticulo as 'DESCRIPCION',CAST(Cantidad as decimal (18,2))as 'CANTIDAD' FROM [EXACTUS].[dismo].[SoftlandBI_CI_Movimientos]  where Bodega = 'B013' and Naturaleza = 'Entrada' and Documento = '"+Regalias_Vencido.Documento+"' ", con.condm);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);


                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();



                
               con.Desconectar("DM");

            }
            else
                if (Regalias_Vencido.modo == 3)
                {
                    dataGridView1.ColumnCount = 3;
                    dataGridView1.Columns[0].Name = "CODIGO";
                    dataGridView1.Columns[1].Name = "DESCRIPCION";
                    dataGridView1.Columns[2].Name = "CANTIDAD";

                    this.label5.Hide();
                    this.label1.Hide();
                    this.label2.Hide();
                    this.comboBox1.Hide();
                    this.comboBox3.Hide();
                    this.richTextBox1.Hide();
                    this.label10.Text = "Fecha de Vencimiento";
                    this.Text = "INGRESO DE VENCIDO";

                }
                else
                    if (Regalias_Vencido.modo == 4)
                    {
                        dataGridView1.Enabled = false;
                        dataGridView1.ReadOnly = true;
                        this.toolStripButton1.Enabled = false;
                        this.toolStripButton5.Enabled = false;
                        this.toolStripButton3.Enabled = false;
                        this.label5.Hide();
                        this.label1.Hide();
                        this.label2.Hide();
                        this.comboBox1.Hide();
                        this.comboBox3.Hide();
                        this.richTextBox1.Hide();
                        this.label10.Text = "Fecha de Vencimiento";
                        this.Text = "INGRESO DE VENCIDO";

                        dt.Clear();
                       
                        dataGridView1.RowHeadersVisible = false;
                        dataGridView1.AutoResizeColumns();
                        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                      
                        dataGridView1.Refresh();
                        con.conectar("DM");

                       

                        SqlCommand cmd = new SqlCommand("select COD_PROD as 'CODIGO',DESCRIPCION,CANTIDAD FROM [DM].[CORRECT].[DET_VENCIDO]	WHERE NUM_DOC ='" + Regalias_Vencido.Documento + "' ", con.condm);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);


                        dataGridView1.DataSource = dt;
                        dataGridView1.Refresh();

                        SqlCommand cmd2 = new SqlCommand("SELECT A.NUM_DOC ,A.RUTA,A.AGENCIA ,B.FECHA_VEN FROM [DM].[CORRECT].[ENC_VENCIDO] A  LEFT JOIN [DM].[CORRECT].[DET_VENCIDO] B  ON A.NUM_DOC = B.NUM_DOC WHERE A.NUM_DOC ='"+Regalias_Vencido.Documento+"'GROUP BY A.[NUM_DOC] ,A.[RUTA],A.[AGENCIA],B.FECHA_VEN", con.condm);
                        SqlDataReader dr2 = cmd2.ExecuteReader();
                        
                         while (dr2.Read())
                        {
                            this.comboBox2.Text = Convert.ToString(dr2["RUTA"]);
                            this.textBox1.Text = Convert.ToString(dr2["NUM_DOC"]);
                            this.comboBox4.Text = Convert.ToString(dr2["AGENCIA"]);
                            this.dateTimePicker1.Value = (DateTime)dr2["FECHA_VEN"];
                        }

                        dr2.Close();






                       con.Desconectar("DM");
                  


                    }
                    else
                    {
                        dataGridView1.ColumnCount = 3;
                        dataGridView1.Columns[0].Name = "CODIGO";
                        dataGridView1.Columns[1].Name = "DESCRIPCION";
                        dataGridView1.Columns[2].Name = "CANTIDAD";
                        pictureBox1.Hide();
                        pictureBox2.Hide();
                    
                    
                    }



            toolStripButton3.Enabled = false;
            ControlBox = false;
            this.ActiveControl = comboBox2;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd-MM-yyyy";
           
           
            fecha_actual = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            dataGridView1.Enabled = false;

       
             Usuario = Login.usuario.ToUpper();
             label8.Text = Usuario;

             comboBox4.Text = Main_Menu.Agencia;
         
             comboBox2.DataSource = AutocompleteRuta.AutocompleteRutas.RUTAS();
             comboBox2.DisplayMember = "RUTA";
             comboBox2.ValueMember = "RUTA";

             comboBox2.AutoCompleteCustomSource = AutocompleteRuta.AutocompleteRutas.AutocompleteRT();
             comboBox2.AutoCompleteMode = AutoCompleteMode.Suggest;
             comboBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;

             comboBox5.DataSource = AutocompleteRuta.AutocompleteRutas.ACTIVIDADES();
             comboBox5.DisplayMember = "NOMBRE";
             comboBox5.ValueMember = "NOMBRE";

             comboBox5.AutoCompleteCustomSource = AutocompleteRuta.AutocompleteRutas.AutocompleteACTIVIDAD();
             comboBox5.AutoCompleteMode = AutoCompleteMode.Suggest;
             comboBox5.AutoCompleteSource = AutoCompleteSource.CustomSource;

             comboBox6.DataSource = AutocompleteRuta.AutocompleteRutas.CUENTA_BAT();
             comboBox6.DisplayMember = "CUENTA";
             comboBox6.ValueMember = "CUENTA";

             comboBox6.AutoCompleteCustomSource = AutocompleteRuta.AutocompleteRutas.AutocompleteCUENTA_BAT();
             comboBox6.AutoCompleteMode = AutoCompleteMode.Suggest;
             comboBox6.AutoCompleteSource = AutoCompleteSource.CustomSource;


             comboBox6.Hide();
             label17.Hide();

        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {



        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
                     


            if (dataGridView1.Enabled == true)
            {

                int icolumn = dataGridView1.CurrentCell.ColumnIndex;
                int irow = dataGridView1.CurrentCell.RowIndex;

         

            if (e.KeyChar == Convert.ToChar(Keys.Enter)|| e.KeyChar == Convert.ToChar(Keys.Tab))
            {
                                  



                if (icolumn == dataGridView1.Columns.Count - 1)
                {


                    MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show("DESEA AGREGAR OTRO PRODUCTO:", "INGRESO DE REGALIA", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (result == DialogResult.Yes)
                    {

                        dataGridView1.Rows.Add();
                        dataGridView1.CurrentCell = dataGridView1[0, irow + 1];




                    }
                    else
                    {
                        dataGridView1.AllowUserToAddRows = false;

                    }

                }
                else if (icolumn != dataGridView1.Columns.Count - 1)
                {

                    dataGridView1.CurrentCell = dataGridView1[icolumn + 1, irow];
                    dataGridView1.AllowUserToAddRows = false;


                }

             

            }
            }

           
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            toolStripButton5.Enabled = true;
            if (Regalias_Vencido.modo == 2)
            {
                dataGridView1.Enabled = true;

            }
            else
            {
                dataGridView1.Enabled = true;
                dataGridView1.AllowUserToAddRows = true;
            }
        }

       

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            DataGridViewRow row1 = dataGridView1.CurrentRow;
            string estatus = Convert.ToString(row1.Cells["CODIGO"].Value);


            if (row1 != null && estatus != "")
            {
                int linea = dataGridView1.Rows.Count;
                if (linea == 1)
                {
                    dataGridView1.CurrentRow.Cells[0].Value = null;
                    dataGridView1.CurrentRow.Cells[1].Value = null;
                    dataGridView1.CurrentRow.Cells[2].Value = null;
                    this.label7.Text = "0";
                    this.label14.Text = "0";
                    dataGridView1.CurrentCell = dataGridView1.CurrentRow.Cells[0];
                   
                }
                else
                {

                    MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show("DESEA ElIMINAR LA LINEA:", "INGRESO DE REGALIA", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (result == DialogResult.Yes)
                    {
                        int linea2 = dataGridView1.Rows.Count;

                        dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                       
                        decimal suma = 0;

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            suma += Convert.ToDecimal(row.Cells["CANTIDAD"].Value);
                        }

                        this.label7.Text = Convert.ToString(suma);

                        this.label14.Text = Convert.ToString(linea2-1);
                    }

                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Regalias_Vencido.modo == 2 || Regalias_Vencido.modo == 3)
            {
                                
                this.Close();
            }

            else
            {
                this.Close();
            
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            con.conectar("DM");
            SqlCommand cm2 = new SqlCommand("SELECT [NOMBRE],[TIPO] FROM [DM].[CORRECT].[RUTA_REGALIAS] WHERE RUTA = '"+comboBox2.Text+"'", con.condm);
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
                label6.Text= dr2["NOMBRE"].ToString();
                tipo_ruta = dr2["TIPO"].ToString();


            }
            dr2.Close();

                     
           con.Desconectar("DM");

            if (tipo_ruta == "BAT")
            {
                label16.Show();
                comboBox5.Show();
               
            }
            else
            {
                label16.Hide();
                comboBox5.Hide();
            }

           




        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            toolStripButton3.Enabled = true;




        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox Art = e.Control as TextBox;
            TextBox desc = e.Control as TextBox;
            TextBox Cant = e.Control as TextBox;


            if (dataGridView1.CurrentCell == dataGridView1.CurrentRow.Cells[1])
            {

                if (desc != null)
                {
                    desc.AutoCompleteMode = AutoCompleteMode.Suggest;
                    desc.AutoCompleteCustomSource = AutocompleteRuta.AutocompleteRutas.AutocompletePRDESC();
                    desc.AutoCompleteSource = AutoCompleteSource.CustomSource;

                }


            }
            else
            {


                if (dataGridView1.CurrentCell == dataGridView1.CurrentRow.Cells[0])
                {

                    if (Art != null)
                    {
                        Art.AutoCompleteMode = AutoCompleteMode.Suggest;
                        Art.AutoCompleteCustomSource = AutocompleteRuta.AutocompleteRutas.AutocompletePRART();
                        Art.AutoCompleteSource = AutoCompleteSource.CustomSource;

                    }

                }
                else
                {
                    Art.AutoCompleteCustomSource = null;
                }

                if (dataGridView1.CurrentCell.ColumnIndex == 2)
                {
                    
                    if (Cant != null)
                    {
                        Cant.KeyPress -= new KeyPressEventHandler(dataGridView1_KeyPress);
                        Cant.KeyPress += new KeyPressEventHandler(dataGridView1_KeyPress);

                    }

                }
               


            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells[0].Value == null)
            {

                if ((dataGridView1.CurrentRow.Cells[1].Value != null))
                {
                    string desdp = Convert.ToString(dataGridView1.CurrentRow.Cells[1].Value);


                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        DataRow row = dt2.Rows[i];

                        if (desdp == Convert.ToString(row["DESCRIPCION"]))
                        {
                            string valor = Convert.ToString(row["ARTICULO"]);

                            dataGridView1.CurrentRow.Cells[0].Value = valor;

                        }

                    }


                }

            }

            else
                if ((dataGridView1.CurrentRow.Cells[1].Value == null))
                {


                    string codigoP = Convert.ToString(dataGridView1.CurrentRow.Cells[0].Value);


                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        DataRow row = dt2.Rows[i];

                        if (codigoP == Convert.ToString(row["ARTICULO"]))
                        {
                            string valor = Convert.ToString(row["DESCRIPCION"]);

                           
                            dataGridView1.CurrentRow.Cells[1].Value = valor;

                        }


                    }

                }

        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
           
           

        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            this.toolStripButton3.Enabled = true;
            if (dataGridView1.CurrentCell == dataGridView1.CurrentRow.Cells[0])
            {
                dataGridView1.CurrentRow.Cells[1].Value = null;
            }
            else if (dataGridView1.CurrentCell == dataGridView1.CurrentRow.Cells[1])
            {
                dataGridView1.CurrentRow.Cells[0].Value = null;
            }
           
        }

        private void dataGridView1_CellEndEdit_1(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.AllowUserToAddRows = false;
            if (dataGridView1.CurrentCell == dataGridView1.CurrentRow.Cells[2])
            {
                int linea = dataGridView1.Rows.Count;
                decimal suma = 0;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    suma += Convert.ToDecimal(row.Cells["CANTIDAD"].Value);
                }

                this.label7.Text = Convert.ToString(suma);

                this.label14.Text = Convert.ToString(linea);

                

            
            }

        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            string actividad;

            if (Regalias_Vencido.modo == 2 && editmode == 0)
            {
                int linea = dataGridView1.Rows.Count;

                if (textBox1.Text == "")
                {
                    MessageBox.Show("ingrese numero de Regalia");


                }
                else if (comboBox3.Text == "")
                {
                    MessageBox.Show("seleccione una cuenta");
                }
                else if (comboBox1.Text == "")
                {

                    MessageBox.Show("seleccione una Tipo de Regalia");
                }

                else if (richTextBox1.Text == "")
                {

                    MessageBox.Show("Ingrese un concepto de Regalia");
                }


                else if (label7.Text == "" || label7.Text == "0" || linea < 1)
                {
                    MessageBox.Show("Ingrese un Producto");
                }

              

                else
                {

                    if (comboBox5.Text == ""|| comboBox5.Text == "NA")
                    {
                        actividad = null;
                        cuenta_Actividad = null;
                    }
                    else
                    {
                        actividad = comboBox5.Text;
                    }

                    if (comboBox6.Text == "" || comboBox6.Text == "N")
                    {
                        cuenta_Actividad = null;
                    }
                    else
                    {
                        cuenta_Actividad = comboBox6.Text;
                    }

                    try
                    {
                        con.conectar("DM");

                        SqlCommand cmd3 = new SqlCommand("[CORRECT].[INSERT_REGALIA]", con.condm);
                        cmd3.CommandType = CommandType.StoredProcedure;
                        cmd3.Parameters.AddWithValue("@TRANSACCION ", Regalias_Vencido.Documento);
                        cmd3.Parameters.AddWithValue("@BODEGA ", Regalias_Vencido.Bodega);
                        cmd3.Parameters.AddWithValue("@DOCUMENTO ", Convert.ToInt32(this.textBox1.Text));
                        cmd3.Parameters.AddWithValue("@USUARIO ", Usuario);
                        cmd3.Parameters.AddWithValue("@TRANSACCION_AUDIT ", Regalias_Vencido.Doc_audit);
                        cmd3.Parameters.AddWithValue("@CANTIDAD ", Convert.ToDecimal(this.label7.Text));
                        cmd3.Parameters.AddWithValue("@RUTA ", this.comboBox2.Text);
                        cmd3.Parameters.AddWithValue("@NOMBRE_VENDEDOR ", this.label6.Text);
                        cmd3.Parameters.AddWithValue("@AGENCIA ", this.comboBox4.Text);
                        cmd3.Parameters.AddWithValue("@TIPO ", this.comboBox1.Text);
                        cmd3.Parameters.AddWithValue("@CUENTA ", this.comboBox3.Text);
                        cmd3.Parameters.AddWithValue("@CONCEPTO ", this.richTextBox1.Text);
                        cmd3.Parameters.AddWithValue("@FECHA ", Regalias_Vencido.fechadc);
                        cmd3.Parameters.AddWithValue("@ACTIVIDAD ", actividad);
                        cmd3.Parameters.AddWithValue("@BATCUENTA ", cuenta_Actividad);

                        cmd3.ExecuteNonQuery();

                        MessageBox.Show("Documento Guardado");

                    }
                    catch
                    {
                        MessageBox.Show("Error Ingresando Regalia");
                       con.Desconectar("DM");
                    }



                   





                   con.Desconectar("DM");




                }
            }

            else
                if (Regalias_Vencido.modo == 2 && editmode == 1)
                {
                    int linea = dataGridView1.Rows.Count;

                    if (textBox1.Text == "")
                    {
                        MessageBox.Show("ingrese numero de Regalia");


                    }
                    else if (comboBox3.Text == "")
                    {
                        MessageBox.Show("seleccione una cuenta");
                    }
                    else if (comboBox1.Text == "")
                    {

                        MessageBox.Show("seleccione una Tipo de Regalia");
                    }

                    else if (richTextBox1.Text == "")
                    {

                        MessageBox.Show("Ingrese un concepto de Regalia");
                    }


                    else if (label7.Text == "" || label7.Text == "0" || linea < 1)
                    {
                        MessageBox.Show("Ingrese un Producto");
                    }



                    else
                    {

                        if (comboBox5.Text == "" || comboBox5.Text == "NA")
                        {
                            actividad = null;
                            cuenta_Actividad = null;
                        }
                        else
                        {
                            actividad = comboBox5.Text;
                        }

                        if (comboBox6.Text == "" || comboBox6.Text == "N")
                        {
                            cuenta_Actividad = null;
                        }
                        else
                        {
                            cuenta_Actividad = comboBox6.Text;
                        }

                        try
                        {
                            con.conectar("DM");

                            SqlCommand cmd3 = new SqlCommand("[CORRECT].[INSERT_REGALIA]", con.condm);
                            cmd3.CommandType = CommandType.StoredProcedure;
                            cmd3.Parameters.AddWithValue("@TRANSACCION ", Regalias_Vencido.Documento);
                            cmd3.Parameters.AddWithValue("@BODEGA ", Regalias_Vencido.Bodega);
                            cmd3.Parameters.AddWithValue("@DOCUMENTO ", Convert.ToInt32(this.textBox1.Text));
                            cmd3.Parameters.AddWithValue("@USUARIO ", Usuario);
                            cmd3.Parameters.AddWithValue("@TRANSACCION_AUDIT ", Regalias_Vencido.Doc_audit);
                            cmd3.Parameters.AddWithValue("@CANTIDAD ", Convert.ToDecimal(this.label7.Text));
                            cmd3.Parameters.AddWithValue("@RUTA ", this.comboBox2.Text);
                            cmd3.Parameters.AddWithValue("@NOMBRE_VENDEDOR ", this.label6.Text);
                            cmd3.Parameters.AddWithValue("@AGENCIA ", this.comboBox4.Text);
                            cmd3.Parameters.AddWithValue("@TIPO ", this.comboBox1.Text);
                            cmd3.Parameters.AddWithValue("@CUENTA ", this.comboBox3.Text);
                            cmd3.Parameters.AddWithValue("@CONCEPTO ", this.richTextBox1.Text);
                            cmd3.Parameters.AddWithValue("@FECHA ", Regalias_Vencido.fechadc);
                            cmd3.Parameters.AddWithValue("@ACTIVIDAD ", actividad);
                            cmd3.Parameters.AddWithValue("@BATCUENTA ", cuenta_Actividad);
                            cmd3.ExecuteNonQuery();



                        }
                        catch
                        {
                            MessageBox.Show("Error Ingresando Regalia");
                           con.Desconectar("DM");
                        }

                       con.Desconectar("DM");

                        try
                        {
                            con.conectar("EX");
                            SqlCommand cmd = new SqlCommand("UPDATE  [EXACTUS].["+Login.empresa+"].[AUDIT_TRANS_INV] SET REFERENCIA = '"+this.textBox1.Text+"' where APLICACION = '"+Regalias_Vencido.Documento+"'", con.conex);
                            cmd.ExecuteNonQuery();
                            con.Desconectar("EX");
                        }

                        catch
                        {
                            MessageBox.Show("ERROR AL ACTUALIZAR EN EXACTUS FR");
                            con.Desconectar("EX");
                        }


                        MessageBox.Show("Documento Guardado");


                        this.Close();



                    }
                }





            else
                if (Regalias_Vencido.modo == 3)
                {


                    try
                    {
                        con.conectar("DM");


                        SqlCommand cmd6 = new SqlCommand();
                        cmd6.Connection = con.condm;
                        cmd6.CommandText = "insert into[DM].[CORRECT].[ENC_VENCIDO](NUM_DOC,USUARIO,RUTA,NOMBRE_VENDEDOR,AGENCIA,CANTIDAD,FECHA_DOC)values(@NUM_DOC,@USUARIO,@RUTA,@NOMBRE_VENDEDOR,@AGENCIA,@CANTIDAD,@FECHA_DOC)";
                        cmd6.Parameters.Add("@NUM_DOC", SqlDbType.NVarChar).Value = Convert.ToInt32(this.textBox1.Text);
                        cmd6.Parameters.Add("@USUARIO", SqlDbType.NVarChar).Value = Usuario;
                        cmd6.Parameters.Add("@RUTA", SqlDbType.NVarChar).Value = this.comboBox2.Text;
                        cmd6.Parameters.Add("@NOMBRE_VENDEDOR", SqlDbType.NVarChar).Value = this.label6.Text;
                        cmd6.Parameters.Add("@AGENCIA", SqlDbType.NVarChar).Value = this.comboBox4.Text;
                        cmd6.Parameters.Add("@CANTIDAD", SqlDbType.Decimal).Value = Convert.ToDecimal(this.label7.Text);
                        cmd6.Parameters.Add("@FECHA_DOC", SqlDbType.NVarChar).Value = fecha_actual;
                        

                        cmd6.CommandType = CommandType.Text;
                        cmd6.ExecuteNonQuery();
                                                                   


                    }
                    catch
                    {
                        MessageBox.Show("Error ingreso de Regalia ENCABEZDO");
                       con.Desconectar("DM");
                    }



                    try
                    {
                        
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {


                            SqlCommand cmd6 = new SqlCommand();
                            cmd6.Connection = con.condm;
                            cmd6.CommandText = "insert into[DM].[CORRECT].[DET_VENCIDO](NUM_DOC,USUARIO,RUTA,COD_PROD,DESCRIPCION,CANTIDAD,FECHA,FECHA_VEN)values(@NUM_DOC,@USUARIO,@RUTA,@COD_PROD,@DESCRIPCION,@CANTIDAD,@FECHA,@FECHA_VEN)";
                            cmd6.Parameters.Add("@NUM_DOC", SqlDbType.NVarChar).Value = Convert.ToInt32(this.textBox1.Text);
                            cmd6.Parameters.Add("@USUARIO", SqlDbType.NVarChar).Value = Usuario;
                            cmd6.Parameters.Add("@RUTA", SqlDbType.NVarChar).Value =this.comboBox2.Text;
                            cmd6.Parameters.Add("@COD_PROD", SqlDbType.NVarChar).Value = Convert.ToString(row.Cells["CODIGO"].Value);
                            cmd6.Parameters.Add("@DESCRIPCION", SqlDbType.NVarChar).Value = Convert.ToString(row.Cells["DESCRIPCION"].Value);
                            cmd6.Parameters.Add("@CANTIDAD", SqlDbType.Decimal).Value = Convert.ToDecimal(row.Cells["CANTIDAD"].Value);
                            cmd6.Parameters.Add("@FECHA", SqlDbType.NVarChar).Value = fecha_actual;
                            cmd6.Parameters.Add("@FECHA_VEN", SqlDbType.NVarChar).Value = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
                          
                            cmd6.CommandType = CommandType.Text;
                            cmd6.ExecuteNonQuery();
                                                                                 

                        }

                       con.Desconectar("DM");
                        MessageBox.Show("Documento Ingresado");
                        this.textBox1.Text = "";
                        this.comboBox3.Text = "";
                        this.comboBox1.Text = "";
                        this.richTextBox1.Text = "";
                        this.dataGridView1.Rows.Clear();
                        this.label7.Text = "";
                        this.label14.Text = "";



                    }
                    catch
                    {
                        MessageBox.Show("Error ingreso de Regalia DETALLE");
                       con.Desconectar("DM");
                    }




                   con.Desconectar("DM");

                    this.Close();
                }
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

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            dataGridView1.Enabled = true;
          
            
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            int linea = dataGridView1.Rows.Count -1;
            decimal suma = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                suma += Convert.ToDecimal(row.Cells["CANTIDAD"].Value);
            }

            this.label7.Text = Convert.ToString(suma);

            this.label14.Text = Convert.ToString(linea);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("ESTA SEGURO DE CAMBIAR EL NUMERO DE DOCUMENTO ", "CAMBIAR", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                textBox1.Enabled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            editmode = 1;
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox5.Text != "NA" || comboBox5.Text != "")
            {

                comboBox6.Show();
                label17.Show();

            }

            else
            {
               
                comboBox6.Hide();
                label17.Hide();
            }

            
            //con.conectar("DM");
            //SqlCommand cm2 = new SqlCommand("SELECT [CUENTA] FROM [DM].[CORRECT].[ACTIVIDADES_BAT]  WHERE NOMBRE = '"+comboBox4.Text+"'", con.condm);
            //SqlDataReader dr2 = cm2.ExecuteReader();
            //while (dr2.Read())
            //{
            //    cuenta_Actividad = dr2["CUENTA"].ToString();


            //}
            //dr2.Close();
            //con.conectar("DM");
        }

       

     
        
       

        }

        

       

        

        

        
      

      
       
    }

    

