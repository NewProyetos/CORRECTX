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
    public partial class Regalias_Vencido : Form
    {
        public Regalias_Vencido()
        {
            InitializeComponent();
        }

        public static int modo;
        public static int ID;
        public static string Documento;
        public static string Documento_fisico;
        public static string Bodega;
        public static string Doc_audit;
        public static string tipo;
        public static string Agencia;
        public static string Usuario;
        public static string fecha_actual;
        public static DateTime fechadc;
        //Conexion2 conet = new Conexion2();
        //conexion conetx = new conexion();
        conexionXML con = new conexionXML();
        DataTable dt = new DataTable();
        private ContextMenu menugrid = new ContextMenu();
        private ContextMenu menugrid1 = new ContextMenu();
     
        
        // Evento Carga Grid por primera vez

        private void Form13_Load(object sender, EventArgs e)
        {
            MenuItem ANULAR = new MenuItem("ANULAR", new System.EventHandler(this.ANULAR));
            MenuItem ELIMINAR = new MenuItem("ELIMINAR", new System.EventHandler(this.ELIMINAR));
            menugrid.MenuItems.AddRange(new MenuItem[] { ANULAR });
            menugrid1.MenuItems.AddRange(new MenuItem[] { ELIMINAR });
            Usuario = Login.usuario.ToUpper();
            if (Main_Menu.Departamento == "INFORMATICA")
            {
                actualizarNombresVenedoresToolStripMenuItem.Enabled = true;
            }
            else
            {
                actualizarNombresVenedoresToolStripMenuItem.Enabled = false;
            }

            label10.Hide();
            ID = 0;
            label9.Show();


            con.conectar("EX");

            if (modo == 2 || modo == 1 || modo == 0)
            {

              
                fecha_actual = DateTime.Now.ToString("yyyy/MM/dd");
                this.toolStripButton2.Enabled = false;


                this.textBox1.Enabled = false;
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                dateTimePicker1.CustomFormat = "dd-MM-yyyy";

                dateTimePicker2.Format = DateTimePickerFormat.Custom;
                dateTimePicker2.CustomFormat = "dd-MM-yyyy";
                this.comboBox2.Text = "REGALIA";

                dt.Clear();
                dt.Columns.Clear();
                dataGridView1.RowHeadersVisible = false;
                dataGridView1.AutoResizeColumns();
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.Refresh();


                Usuario = Login.usuario.ToUpper();

                comboBox1.Text = Main_Menu.Agencia;
                textBox1.Text = Usuario;
               

                //SqlCommand cmd = new SqlCommand("SELECT NUM_DOC,RUTA,NOMBRE_VENDEDOR,TIPO,CUENTA,CONCEPTO,CANTIDAD,AGENCIA,FECHA FROM [DM].[CORRECT].[ENC_REGALIA] WHERE USUARIO = '" + Usuario + "' AND  (DATEADD(dd, 0, DATEDIFF(dd, 0, FECHA)) >= '" + this.dateTimePicker1.Value.ToString("yyyy-MM-dd") + "') AND (DATEADD(dd, 0, DATEDIFF(dd, 0, FECHA)) <= '" + this.dateTimePicker2.Value.ToString("yyyy-MM-dd") + "') ", con.condm);
                SqlCommand cmd = new SqlCommand("SELECT A.[APLICACION] as 'TRANSACCION',B.Bodega as 'BODEGA',A.[REFERENCIA] as 'DOCUMENTO',A.[Usuario] as 'USUARIO',A.[FECHA_HORA] ,A.[AUDIT_TRANS_INV] as 'TRANSACCION ID' ,SUM(B.Cantidad) as 'CANTIDAD'  FROM [EXACTUS].[dismo].[AUDIT_TRANS_INV]A INNER JOIN [EXACTUS].[dismo].[SoftlandBI_CI_Movimientos] B  ON A.APLICACION = B.Documento where A.APLICACION like 'REG%' AND A.REFERENCIA not like '%[a-z]%' AND B.Bodega ='B013' and A.USUARIO = '"+Usuario.ToUpper()+"' and (DATEADD(dd, 0, DATEDIFF(dd, 0, FECHA_HORA)) >= '"+this.dateTimePicker1.Value.ToString("yyyy-MM-dd")+"') AND (DATEADD(dd, 0, DATEDIFF(dd, 0, FECHA_HORA)) <= '"+this.dateTimePicker2.Value.ToString("yyyy-MM-dd")+"')  and A.APLICACION not in (SELECT [TRANSACCION] FROM [DM].[CORRECT].[REGALIAS])   Group by A.[AUDIT_TRANS_INV] ,A.[CONSECUTIVO] ,A.[Usuario] ,A.[FECHA_HORA] ,A.[APLICACION] ,A.[REFERENCIA],B.Bodega", con.conex);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();

               

                int linea = dataGridView1.Rows.Count;
                double suma = 0;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    suma += Convert.ToDouble(row.Cells["CANTIDAD"].Value);
                }



                this.label7.Text = Convert.ToString(suma);

                this.label8.Text = Convert.ToString(linea);

            }
            else
                if (modo == 3)
                {
                    fecha_actual = DateTime.Now.ToString("yyyy/MM/dd");
                    this.toolStripButton2.Enabled = false;


                    this.textBox1.Enabled = false;
                    dateTimePicker1.Format = DateTimePickerFormat.Custom;
                    dateTimePicker1.CustomFormat = "dd-MM-yyyy";

                    dateTimePicker2.Format = DateTimePickerFormat.Custom;
                    dateTimePicker2.CustomFormat = "dd-MM-yyyy";
                    this.comboBox2.Text = "VENCIDOS";

                    dt.Clear();
                    dataGridView1.RowHeadersVisible = false;
                    dataGridView1.AutoResizeColumns();
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    dataGridView1.ReadOnly = true;
                    dataGridView1.AllowUserToAddRows = false;
                    dataGridView1.Refresh();


                    comboBox1.Text = Main_Menu.Agencia;



                    SqlCommand cmd = new SqlCommand("SELECT [NUM_DOC],[USUARIO],[RUTA],[NOMBRE_VENDEDOR],[AGENCIA],[CANTIDAD],[FECHA_DOC] FROM [DM].[CORRECT].[ENC_VENCIDO] WHERE USUARIO = '" + Usuario + "' AND  (DATEADD(dd, 0, DATEDIFF(dd, 0, FECHA_DOC)) >= '" + this.dateTimePicker1.Value.ToString("yyyy-MM-dd") + "') AND (DATEADD(dd, 0, DATEDIFF(dd, 0, FECHA_DOC)) <= '" + this.dateTimePicker2.Value.ToString("yyyy-MM-dd") + "') ", con.condm);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;
                    dataGridView1.Refresh();



                    int linea = dataGridView1.Rows.Count;
                    decimal suma = 0;

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        suma += Convert.ToDecimal(row.Cells["CANTIDAD"].Value);
                    }



                    this.label7.Text = Convert.ToString(suma);

                    this.label8.Text = Convert.ToString(linea);






                    SqlCommand cm2 = new SqlCommand("SELECT [USUARIO]FROM [DM].[CORRECT].[USUARIOS] where DEPARTAMENTO = 'OPERACIONES'", con.condm);
                    SqlDataReader dr2 = cm2.ExecuteReader();
                    while (dr2.Read())
                    {

                        toolStripComboBox1.Items.Add(dr2["USUARIO"]);

                    }
                    dr2.Close();




                }
                else
                {
                    con.Desconectar("DM");
                }

           
        }



        private void ANULAR(Object sender, System.EventArgs e)
        {
            MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show("DESEA ANULAR ESTA REGALIA:" + Documento + " ", "ANULACION...", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (result == DialogResult.Yes)
                    {
                        con.Desconectar("EX");
                        con.conectar("EX");

                        SqlCommand cm2 = new SqlCommand("UPDATE[EXACTUS].[dismo].[AUDIT_TRANS_INV] SET  APLICACION = 'N'+APLICACION Where APLICACION = '" + Documento + "'", con.conex);
                        cm2.ExecuteNonQuery();
                        con.Desconectar("EX");

                        MessageBox.Show("ANULACION REALIZADA");
                        carga_data();
                    }
                    
        }


        private void ELIMINAR(Object sender, System.EventArgs e)
        {
            toolStripButton2_Click(null, null);

        }

        
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Documento = "";

            label10.Hide();
            label9.Hide();
            if (comboBox2.Text == "REGALIA")
            {
                this.toolStripButton4.Enabled = false;

                label9.Text = "!!!!!! REGALIAS PENDIENTES!!!!!!";
                fecha_actual = DateTime.Now.ToString("yyyy/MM/dd");
                this.toolStripButton2.Enabled = false;


                this.textBox1.Enabled = false;
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                dateTimePicker1.CustomFormat = "dd-MM-yyyy";

                dateTimePicker2.Format = DateTimePickerFormat.Custom;
                dateTimePicker2.CustomFormat = "dd-MM-yyyy";
               


                dt.Clear();
                dt.Columns.Clear();
                dataGridView1.Refresh();
                

                dataGridView1.RowHeadersVisible = false;
                
                dataGridView1.AutoResizeColumns();
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.DataSource = dt;

                comboBox1.Text = Main_Menu.Agencia;
              
          


                con.conectar("DM");
                dt.Clear();
                SqlCommand cmd = new SqlCommand("SELECT A.[APLICACION] as 'TRANSACCION',B.Bodega as 'BODEGA',A.[REFERENCIA] as 'DOCUMENTO',A.[Usuario] as 'USUARIO',A.[FECHA_HORA] ,A.[AUDIT_TRANS_INV] as 'TRANSACCION ID' ,SUM(B.Cantidad) as 'CANTIDAD'  FROM [EXACTUS].[dismo].[AUDIT_TRANS_INV]A INNER JOIN [EXACTUS].[dismo].[SoftlandBI_CI_Movimientos] B  ON A.APLICACION = B.Documento where A.APLICACION like 'REG%' AND A.REFERENCIA not like '%[a-z]%' AND B.Bodega ='B013' and A.USUARIO = '" + Login.usuario.ToUpper() + "' and A.APLICACION not in (SELECT [TRANSACCION] FROM [DM].[CORRECT].[REGALIAS])   Group by A.[AUDIT_TRANS_INV] ,A.[CONSECUTIVO] ,A.[Usuario] ,A.[FECHA_HORA] ,A.[APLICACION] ,A.[REFERENCIA],B.Bodega", con.conex);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();

                con.Desconectar("DM");

                int linea = dataGridView1.Rows.Count;
                decimal suma = 0;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    suma += Convert.ToDecimal(row.Cells["CANTIDAD"].Value);
                }



                this.label7.Text = Convert.ToString(suma);

                this.label8.Text = Convert.ToString(linea);

            }
            else if (comboBox2.Text == "VENCIDOS")
            {
                
                this.toolStripButton4.Enabled = true;
                fecha_actual = DateTime.Now.ToString("yyyy/MM/dd");
                this.toolStripButton2.Enabled = false;


                this.textBox1.Enabled = false;
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                dateTimePicker1.CustomFormat = "dd-MM-yyyy";

                dateTimePicker2.Format = DateTimePickerFormat.Custom;
                dateTimePicker2.CustomFormat = "dd-MM-yyyy";
                

                dt.Clear();
                dt.Columns.Clear();
                dataGridView1.RowHeadersVisible = false;
                dataGridView1.AutoResizeColumns();
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
              
                dataGridView1.Refresh();


                comboBox1.Text = Main_Menu.Agencia;


                con.conectar("DM");
                dt.Clear();
                SqlCommand cmd = new SqlCommand("SELECT [NUM_DOC],[USUARIO],[RUTA],[NOMBRE_VENDEDOR],[AGENCIA],[CANTIDAD],[FECHA_DOC] FROM [DM].[CORRECT].[ENC_VENCIDO] WHERE USUARIO = '" + Usuario + "' AND  (DATEADD(dd, 0, DATEDIFF(dd, 0, FECHA_DOC)) >= '" + this.dateTimePicker1.Value.ToString("yyyy-MM-dd") + "') AND (DATEADD(dd, 0, DATEDIFF(dd, 0, FECHA_DOC)) <= '" + this.dateTimePicker2.Value.ToString("yyyy-MM-dd") + "') ", con.condm);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();

                con.Desconectar("DM");

                int linea = dataGridView1.Rows.Count;
                decimal suma = 0;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    suma += Convert.ToDecimal(row.Cells["CANTIDAD"].Value);
                }



                this.label7.Text = Convert.ToString(suma);

                this.label8.Text = Convert.ToString(linea);


            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            //if (fecha_actual == this.dateTimePicker1.Value.ToString("yyyy/MM/dd") && fecha_actual == this.dateTimePicker2.Value.ToString("yyyy/MM/dd"))
            //{
            //    this.toolStripButton2.Enabled = true;
            //}
            //else
            //{
            //    this.toolStripButton2.Enabled = false;
            //}
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            
        }

        public  void button1_Click(object sender, EventArgs e)
        {
            carga_data();
            con.Desconectar("DM");
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            modo = 3;
            this.Hide();
            ingreso_reg_ven Fm12 = new ingreso_reg_ven();
            Fm12.Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (this.comboBox2.Text == "REGALIA")
            {

                if (Documento == "" || Documento == null)
                {
                    MessageBox.Show("Seleccione el Documento que quiere Eliminar");

                }
                else
                  
                {
                    MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show("DESEA ELIMINAR ESTE DOCUMENTO:" + Documento + " ", "ELIMINAR", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            con.conectar("DM");
                            SqlCommand cmd = new SqlCommand("DELETE [DM].[CORRECT].[REGALIAS] WHERE TRANSACCION ='" + Documento + "' ", con.condm);
                            

                            cmd.ExecuteNonQuery();
                           
                            con.Desconectar("DM");

                            MessageBox.Show("Documento" + Documento + " Eliminado");

                            carga_data();

                            toolStripButton2.Enabled = false;
                            this.toolStripComboBox1.Text = "";

                        }
                        catch
                        {
                            MessageBox.Show("Error al intentar eliminar Documento" + Documento + "");
                        }



                    }
                    else
                    {

                    }


                }
            }
            else
                if (this.comboBox2.Text == "VENCIDOS")
            {


                    if (Documento == "" || Documento == null)
                {
                    MessageBox.Show("Seleccione el Documento que quiere Eliminar");

                }
                else
                {
                    MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show("DESEA ELIMINAR ESTE DOCUMENTO:" + Documento + " ", "ELIMINAR", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            con.conectar("DM");
                            SqlCommand cmd = new SqlCommand("DELETE FROM [DM].[CORRECT].[DET_VENCIDO] WHERE NUM_DOC ='" + Documento + "' ", con.condm);
                            SqlCommand cmd1 = new SqlCommand("DELETE FROM [DM].[CORRECT].[ENC_VENCIDO] WHERE NUM_DOC ='" + Documento + "' ", con.condm);

                            cmd.ExecuteNonQuery();
                            cmd1.ExecuteNonQuery();
                            con.Desconectar("DM");

                            MessageBox.Show("Documento" + Documento + " Eliminado");

                            button1_Click(null, null);

                            toolStripButton2.Enabled = false;

                        }
                        catch
                        {
                            MessageBox.Show("Error al intentar eliminar Documento" + Documento + "");
                        }


                    }
                    }
                    }        
            
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (this.comboBox2.Text == "REGALIA")
            {
                DataGridViewRow row1 = dataGridView1.CurrentRow;
                Documento = Convert.ToString(row1.Cells["TRANSACCION"].Value);
                fechadc = Convert.ToDateTime(row1.Cells["FECHA_HORA"].Value);
                Documento_fisico = Convert.ToString(row1.Cells["DOCUMENTO"].Value);
                Bodega = Convert.ToString(row1.Cells["BODEGA"].Value);
                Doc_audit = Convert.ToString(row1.Cells["TRANSACCION ID"].Value);


                if (fecha_actual == fechadc.ToString("yyyy/MM/dd") && ID == 1 )
                {
                    this.toolStripButton2.Enabled = true;
                }
                else
                {
                    this.toolStripButton2.Enabled = false;
                }
            }
            else
                 if (this.comboBox2.Text == "VENCIDOS")
            {

                      DataGridViewRow row1 = dataGridView1.CurrentRow;
                Documento = Convert.ToString(row1.Cells["NUM_DOC"].Value);
                DateTime fechadc = Convert.ToDateTime(row1.Cells["FECHA_DOC"].Value);


                if (fecha_actual == fechadc.ToString("yyyy/MM/dd"))
                {
                    this.toolStripButton2.Enabled = true;
                }
                else
                {
                    this.toolStripButton2.Enabled = false;
                }
            }
        }
    

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.comboBox2.Text == "REGALIA" && ID == 0)
            {
                DataGridViewRow row1 = dataGridView1.CurrentRow;
                Documento = Convert.ToString(row1.Cells["TRANSACCION"].Value);
                modo = 2;


                ingreso_reg_ven fm12 = new ingreso_reg_ven();
         // Este codigo obtiene cuando el formulario se cierra  
                fm12.FormClosed += new System.Windows.Forms.FormClosedEventHandler(ingreso_reg_FormClosed);
               
                fm12.ShowDialog();
              

         
            }
            else
                if (this.comboBox2.Text == "VENCIDOS")
            {
                DataGridViewRow row1 = dataGridView1.CurrentRow;
                Documento = Convert.ToString(row1.Cells["NUM_DOC"].Value);
                modo = 4;

                ingreso_reg_ven fm12 = new ingreso_reg_ven();
                fm12.ShowDialog();
            }


        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "REGALIA")
            {
                label9.Hide();
                label10.Show();
                ID = 1;
                if (toolStripComboBox1.Text == "" && toolStripTextBox1.Text == "")
                {
                    MessageBox.Show("Ingrese un parametro para la Busqueda");
                }
                else
                    if (toolStripTextBox1.Text != "")
                    {
                        dt.Clear();
                        dt.Columns.Clear();
                        con.conectar("DM");

                        SqlCommand cmd = new SqlCommand("SELECT [TRANSACCION],[BODEGA],[DOCUMENTO],[USUARIO],[TRANSACCION_AUDIT] as 'TRANSACCION ID',[CANTIDAD],[RUTA],[NONBRE_VENDEDOR],[AGENCIA],[TIPO] ,[CUENTA] ,[CONCEPTO] ,[FECHA] as 'FECHA_HORA' FROM [DM].[CORRECT].[REGALIAS] WHERE [DOCUMENTO] = '" + toolStripTextBox1.Text + "'", con.condm);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);

                        dataGridView1.DataSource = dt;
                        dataGridView1.Refresh();

                        con.Desconectar("DM");

                        int linea = dataGridView1.Rows.Count;
                        decimal suma = 0;

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            suma += Convert.ToDecimal(row.Cells["CANTIDAD"].Value);
                        }



                        this.label7.Text = Convert.ToString(suma);

                        this.label8.Text = Convert.ToString(linea);

                       
                    }
                    else
                        if (toolStripComboBox1.Text != "")
                        {
                            dt.Clear();
                            dt.Columns.Clear();
                            con.conectar("DM");

                            SqlCommand cmd = new SqlCommand("SELECT [TRANSACCION],[BODEGA],[DOCUMENTO],[USUARIO],[TRANSACCION_AUDIT] as 'TRANSACCION ID',[CANTIDAD],[RUTA],[NONBRE_VENDEDOR],[AGENCIA],[TIPO] ,[CUENTA] ,[CONCEPTO] ,[FECHA] as 'FECHA_HORA' FROM [DM].[CORRECT].[REGALIAS] WHERE [USUARIO] = '" + this.toolStripComboBox1.Text + "'", con.condm);
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(dt);

                            dataGridView1.DataSource = dt;
                            dataGridView1.Refresh();

                            con.Desconectar("DM");

                            int linea = dataGridView1.Rows.Count;
                            decimal suma = 0;

                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                suma += Convert.ToDecimal(row.Cells["CANTIDAD"].Value);
                            }



                            this.label7.Text = Convert.ToString(suma);

                            this.label8.Text = Convert.ToString(linea);
                        }
            }
            else
            { 
            
            }

            con.Desconectar("DM");
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {
            toolStripComboBox1.Text = "";
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {
            toolStripTextBox1.Text = "";
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "REGALIA")
            {
                
            }
            else
                if (comboBox2.Text == "VENCIDOS")
            {
                toolStripButton4_Click(null, null);
            }
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton2_Click(null, null);
        }

        private void detalleDeRegaliasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tipo = "R";
            Reporte_Regalia fm16 = new Reporte_Regalia("old");
            fm16.ShowDialog();

        }

        private void detalleVencidoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tipo = "V";
            Reporte_Regalia fm16 = new Reporte_Regalia("old");
            fm16.ShowDialog();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

// metodo que actualiza la infromacion en el datagrid
        public void carga_data()
        {

            this.toolStripComboBox1.Text = "";
            if (this.comboBox2.Text == "REGALIA")
            {
                label10.Hide();
                label9.Show();
                ID = 0;

                dt.Clear();
                dt.Columns.Clear();
                con.Desconectar("DM");
                this.toolStripButton2.Enabled = false;
                con.conectar("DM");

                
               

                SqlCommand cmd = new SqlCommand("SELECT A.[APLICACION] as 'TRANSACCION',B.Bodega as 'BODEGA',A.[REFERENCIA] as 'DOCUMENTO',A.[Usuario] as 'USUARIO',A.[FECHA_HORA] ,A.[AUDIT_TRANS_INV] as 'TRANSACCION ID' ,SUM(B.Cantidad) as 'CANTIDAD'  FROM [EXACTUS].[dismo].[AUDIT_TRANS_INV]A INNER JOIN [EXACTUS].[dismo].[SoftlandBI_CI_Movimientos] B  ON A.APLICACION = B.Documento where A.APLICACION like 'REG%' AND A.REFERENCIA not like '%[a-z]%' AND B.Bodega ='B013' and A.USUARIO = '" + Usuario.ToUpper() + "' and (DATEADD(dd, 0, DATEDIFF(dd, 0, FECHA_HORA)) >= '"+ this.dateTimePicker1.Value.ToString("yyyy/MM/dd") + "') AND (DATEADD(dd, 0, DATEDIFF(dd, 0, FECHA_HORA)) <= '" + this.dateTimePicker2.Value.ToString("yyyy/MM/dd") + "')  and A.APLICACION not in (SELECT [TRANSACCION] FROM [DM].[CORRECT].[REGALIAS])   Group by A.[AUDIT_TRANS_INV] ,A.[CONSECUTIVO] ,A.[Usuario] ,A.[FECHA_HORA] ,A.[APLICACION] ,A.[REFERENCIA],B.Bodega", con.conex);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();

                con.Desconectar("DM");

                int linea = dataGridView1.Rows.Count;
                decimal suma = 0;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    suma += Convert.ToDecimal(row.Cells["CANTIDAD"].Value);
                }



                this.label7.Text = Convert.ToString(suma);

                this.label8.Text = Convert.ToString(linea);

            }

            else
                if (this.comboBox2.Text == "VENCIDOS")
                {
                    label10.Hide();
                    label9.Hide();
                    this.toolStripButton4.Enabled = true;
                    fecha_actual = DateTime.Now.ToString("yyyy/MM/dd");
                    this.toolStripButton2.Enabled = false;


                    this.textBox1.Enabled = false;
                    dateTimePicker1.Format = DateTimePickerFormat.Custom;
                    dateTimePicker1.CustomFormat = "dd-MM-yyyy";

                    dateTimePicker2.Format = DateTimePickerFormat.Custom;
                    dateTimePicker2.CustomFormat = "dd-MM-yyyy";


                    dt.Clear();
                    dt.Columns.Clear();
                    dataGridView1.RowHeadersVisible = false;
                    dataGridView1.AutoResizeColumns();
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    dataGridView1.ReadOnly = true;
                    dataGridView1.AllowUserToAddRows = false;

                    dataGridView1.Refresh();


                    comboBox1.Text = Main_Menu.Agencia;

                    con.conectar("DM");
                    dt.Clear();
                    SqlCommand cmd = new SqlCommand("SELECT [NUM_DOC],[USUARIO],[RUTA],[NOMBRE_VENDEDOR],[AGENCIA],[CANTIDAD],[FECHA_DOC] FROM [DM].[CORRECT].[ENC_VENCIDO] WHERE USUARIO = '" + Usuario + "' AND  (DATEADD(dd, 0, DATEDIFF(dd, 0, FECHA_DOC)) >= '" + this.dateTimePicker1.Value.ToString("yyyy-MM-dd") + "') AND (DATEADD(dd, 0, DATEDIFF(dd, 0, FECHA_DOC)) <= '" + this.dateTimePicker2.Value.ToString("yyyy-MM-dd") + "') ", con.condm);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;
                    dataGridView1.Refresh();

                    con.Desconectar("DM");

                    int linea = dataGridView1.Rows.Count;
                    decimal suma = 0;

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        suma += Convert.ToDecimal(row.Cells["CANTIDAD"].Value);
                    }



                    this.label7.Text = Convert.ToString(suma);

                    this.label8.Text = Convert.ToString(linea);


                }

        }
  // evento que se ejecuta al cerrar formulario ingreso_reg_ven.cs
        private void ingreso_reg_FormClosed(object sender, FormClosedEventArgs e)
        {
            carga_data(); 
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (ID == 0)
            {
                DataGridView.HitTestInfo info;
                if (e.Button == MouseButtons.Right)
                {
                    info = dataGridView1.HitTest(e.X, e.Y);
                    if (info.Type == DataGridViewHitTestType.Cell)
                    {
                        menugrid.Show(dataGridView1, new Point(e.X, e.Y));
                    }


                }
            }
            else if (ID == 1)
            {
            DataGridView.HitTestInfo info;
            if (e.Button == MouseButtons.Right)
            {
                info = dataGridView1.HitTest(e.X, e.Y);
                if (info.Type == DataGridViewHitTestType.Cell)
                {
                    menugrid1.Show(dataGridView1, new Point(e.X, e.Y));
                }


            }
            }
        }

        private void actualizarNombresVenedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            con.conectar("DM");
            string cmdst = "UPDATE [DM].[CORRECT].[RUTA_REGALIAS] SET NOMBRE = VEN.NOMBRE FROM [DM].[CORRECT].[RUTA_REGALIAS]  as RTD inner join  [EXACTUS].[ERPADMIN].[RUTA_ASIGNADA_RT] as  RT on RTD.RUTA = RT.RUTA  inner join [EXACTUS].[dismo].[VENDEDOR]  as VEN  on RT.AGENTE = VEN.VENDEDOR";
            SqlCommand cmd = new SqlCommand(cmdst);
            cmd.Connection = con.condm;
            cmd.ExecuteNonQuery();
            con.Desconectar("DM");

            MessageBox.Show("Nombres Actualizados");
        }

       

        

    }
}
