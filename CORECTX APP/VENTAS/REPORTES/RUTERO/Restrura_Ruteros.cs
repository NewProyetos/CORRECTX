using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using System.Data.OleDb;
using System.Xml;

namespace Sinconizacion_EXactus.CORECTX_APP.VENTAS.RUTERO
{
    public partial class Restrura_Ruteros : Form
    {
        public Restrura_Ruteros()
        {
            InitializeComponent();
        }

        conexionXML con = new conexionXML();
        DataTable dt = new DataTable();
        DataTable error = new DataTable();
        String Selected_File;
        Int32 EstadoError;
        String RUTA;
        String CLIENTE;
        String DIA;
        Int32 ORDEN;
        String SEMANA;
        String ENTREGA;
        String Estatus;
        String Nombreclient;
        String Escorp;        
        String nombreC;
        String EMPRESA = Login.empresa;
             

        private void Restrura_Ruteros_Load(object sender, EventArgs e)
        {
            this.Text = "REESTRUCTURA   (" + Login.empresa + " ) ";

            button3.Enabled = false;
            button2.Enabled = false;
            comboBox2.Enabled = false;
            textBox1.Enabled = false;
            button1.Enabled = false;
            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            //dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;


            dataGridView2.Enabled = true;
            dataGridView2.RowHeadersVisible = false;
            //dataGridView2.AutoResizeColumns();
            //dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView2.ReadOnly = true;
            dataGridView2.AllowUserToAddRows = false;




            con.conectar("EX");
            
            SqlCommand cm1 = new SqlCommand("SELECT A.[RUTA] as 'RUTA' FROM [EXACTUS].[ERPADMIN].[RUTA_ASIGNADA_RT] A where  COMPANIA = '" + Login.empresa + "' ", con.conex);
            SqlDataReader dr1 = cm1.ExecuteReader();
            while (dr1.Read())
            {
                comboBox1.Items.Add(dr1["RUTA"]);
                comboBox2.Items.Add(dr1["RUTA"]);

            }
            dr1.Close();
            con.Desconectar("EX");


            error.Clear();
            error.Columns.Clear();
            error.Columns.Add("CLIENTE", typeof(string));
            error.Columns.Add("RUTA", typeof(string));
            error.Columns.Add("NOMBRE", typeof(string));
            error.Columns.Add("DIA", typeof(string));
            error.Columns.Add("SEMANA", typeof(string));
            error.Columns.Add("SEC", typeof(string));
            error.Columns.Add("ERROR", typeof(string));

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            textBox1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            Selected_File = string.Empty;
            this.textBox1.Clear();
            openFileDialog1.AutoUpgradeEnabled = false;
            openFileDialog1.InitialDirectory = @"%USERPROFILE%\Documents";
            openFileDialog1.Title = "Select a File";
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.Filter = "Excel 2007 Files|*.xlsx|Excel 2003 Files|*.xls";
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {

            }
            else
            {
                Selected_File = openFileDialog1.FileName;
                this.textBox1.Text = Selected_File;
                button3.Enabled = true;
                Carga_data_xls();
            }
        }

        private void Carga_data_xls()
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = "Hoja1";
            }
           

            string nombre_hoja = textBox2.Text;
            


            if (textBox1.Text == "" || textBox1.Text == null)
            {
                MessageBox.Show("no se a selecionado un archivo de Excel");

            }
            else
            {

                try
                {
                    dt.Clear();
                    dt.Columns.Clear();

                    OleDbConnection cnn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Selected_File + "; Extended Properties=Excel 12.0;");

                    OleDbCommand oconn = new OleDbCommand("select * from [" + nombre_hoja + "$]", cnn);
                    cnn.Open();
                    OleDbDataAdapter adp = new OleDbDataAdapter(oconn);
                   
                    adp.Fill(dt);


                    foreach (DataRow row in dt.Rows)
                    {
                        if (String.IsNullOrEmpty(row["RUTA"].ToString()))

                            row.Delete();
                    }
                    dt.AcceptChanges();

                    dataGridView1.DataSource = dt;
                    dataGridView1.Refresh();

                    button3.Enabled = true;
                }
                catch
                {
                    MessageBox.Show("Error al abrir archivo de Excel");
                }


            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
         

            string Ruta = comboBox1.Text; 
            int Registros = dt.Rows.Count;

            if (Registros > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (Ruta != row["RUTA"].ToString())
                    {
                        EstadoError = 1;
                        break;

                    }
                    else
                    {

                        EstadoError = 0;
                        break;
                    }
                }


                if (EstadoError == 1)
                {

                    MessageBox.Show("NO PUEDES INGRESAR RUTERO DE OTRA SELECCIONADA");
                }
                else
                {

                    MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show("ESTA SEGURO QUE DESEA CARGAR MODIFICAR EL RUTERO DE  LA RUTA: " + this.comboBox1.Text + "" , "REESTRUCTURA ", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (result == DialogResult.Yes)
                    {
                        Borrar_Ruta(this.comboBox1.Text);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            DataRow row = dt.Rows[i];

                            if (DBNull.Value == (row["RUTA"]))
                            {
                                error.Rows.Add(CLIENTE, RUTA, nombreC, DIA, SEMANA, ORDEN, "NO se encontro numero de RUTA");
                                dataGridView2.DataSource = error;
                                dataGridView2.Refresh();
                            }
                            else
                            {
                                if (DBNull.Value == (row["ORDEN"]))
                                {
                                    error.Rows.Add(CLIENTE, RUTA, nombreC, DIA, SEMANA, ORDEN, "NO se encontro numero de SECUENCIA");
                                    dataGridView2.DataSource = error;
                                    dataGridView2.Refresh();
                                }
                                else
                                {
                                    if (DBNull.Value == (row["SEMANA"]))
                                    {
                                        error.Rows.Add(CLIENTE, RUTA, nombreC, DIA, SEMANA, ORDEN, "NO se encontro Semana");
                                        dataGridView2.DataSource = error;
                                        dataGridView2.Refresh();
                                    }
                                    else
                                    {
                                        if (DBNull.Value == (row["ENTREGA"]))
                                        {
                                            error.Rows.Add(CLIENTE, RUTA, nombreC, DIA, SEMANA, ORDEN, "NO se encontro RUTA DE ENTREGA");
                                            dataGridView2.DataSource = error;
                                            dataGridView2.Refresh();
                                        }
                                        else
                                        {
                                            if (DBNull.Value == (row["CLIENTE"]))
                                            {
                                                error.Rows.Add(CLIENTE, RUTA, nombreC, DIA, SEMANA, ORDEN, "NO se encontro NUMERO DE CLIENTE");
                                                dataGridView2.DataSource = error;
                                                dataGridView2.Refresh();
                                            }
                                            else
                                            {
                                                if (DBNull.Value == (row["DIA"]))
                                                {
                                                    error.Rows.Add(CLIENTE, RUTA, nombreC, DIA, SEMANA, ORDEN, "NO se encontro DIA ");
                                                    dataGridView2.DataSource = error;
                                                    dataGridView2.Refresh();
                                                }
                                                else
                                                {
                                                    RUTA = Convert.ToString(row["RUTA"]);
                                                    ORDEN = Convert.ToInt32(row["ORDEN"]);
                                                    SEMANA = Convert.ToString(row["SEMANA"]);
                                                    ENTREGA = Convert.ToString(row["ENTREGA"]);
                                                    CLIENTE = Convert.ToString(row["CLIENTE"]);
                                                    DIA = Convert.ToString(row["DIA"]);
                                                   
                                                    int digitos = CLIENTE.Length;

                                                    switch (digitos)
                                                    {
                                                        case 1:
                                                            CLIENTE = "0000" + CLIENTE;
                                                            break;
                                                        case 2:
                                                            CLIENTE = "000" + CLIENTE;
                                                            break;
                                                        case 3:
                                                            CLIENTE = "00" + CLIENTE;
                                                            break;
                                                        case 4:
                                                            CLIENTE = "0" + CLIENTE;
                                                            break;
                                                    }


                                                    estatus_clie(CLIENTE);

                                                    if (Nombreclient != null || Nombreclient != "")
                                                    {
                                                        if (Nombreclient.Length > 79)
                                                        {
                                                            nombreC = Nombreclient.Substring(0, 79);
                                                        }
                                                        else
                                                        {
                                                            nombreC = Nombreclient;
                                                        }


                                                        switch (DIA.ToUpper())
                                                        {
                                                            case "LUNES":
                                                                DIA = "0";
                                                                break;
                                                            case "MARTES":
                                                                DIA = "1";
                                                                break;
                                                            case "MIERCOLES":
                                                                DIA = "2";
                                                                break;
                                                            case "JUEVES":
                                                                DIA = "3";
                                                                break;
                                                            case "VIERNES":
                                                                DIA = "4";
                                                                break;
                                                            case "SABADO":
                                                                DIA = "5";
                                                                break;
                                                            case "DOMINGO":
                                                                DIA = "6";
                                                                break;

                                                            default:
                                                                error.Rows.Add(CLIENTE, RUTA, nombreC, DIA, SEMANA, ORDEN, "Error en Formato  DIA de VISITA");
                                                                dataGridView2.DataSource = error;
                                                                dataGridView2.Refresh();
                                                                break;
                                                        }




                                                        int diasem = Convert.ToInt32(DIA);

                                                        if (diasem > 6 || diasem < 0)
                                                        {

                                                        }
                                                        else
                                                        {

                                                            if (SEMANA == "AB")
                                                            {
                                                                for (int n = 0; n < 2; n++)
                                                                {

                                                                    if (n == 0)
                                                                    {
                                                                        Ingresar_cliente(CLIENTE, RUTA, "A", DIA, ORDEN, ENTREGA, nombreC);
                                                                        row.Delete();
                                                                        dataGridView1.DataSource = dt;
                                                                        dataGridView1.Refresh();

                                                                    }
                                                                    else if (n == 1)
                                                                    {
                                                                        Ingresar_cliente(CLIENTE, RUTA, "B", DIA, ORDEN, ENTREGA, nombreC);
                                                                        row.Delete();
                                                                        dataGridView1.DataSource = dt;
                                                                        dataGridView1.Refresh();
                                                                    }
                                                                    else
                                                                    {
                                                                    }
                                                                }

                                                            }
                                                            else
                                                            {
                                                                Ingresar_cliente(CLIENTE, RUTA, SEMANA, DIA, ORDEN, ENTREGA, nombreC);
                                                                 row.Delete();
                                                                dataGridView1.DataSource = dt;
                                                                dataGridView1.Refresh();

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
                    Inset_auditoria(Login.usuario.ToUpper(), DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), "Carga Rutero RUTA: " + comboBox1.Text + "");

                    MessageBox.Show("CARGA FINALIZADA");
                }
            }
        }
        private bool Existe_cliente_diferent_RUTA(string cliente, string Ruta)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("select COUNT (*) from [DM].[dbo].[RUTERO] WHERE CLIENTE = '"+CLIENTE+"' and RUTA <> '"+Ruta+"' ", con.condm);


            string contar = Convert.ToString(cmd.ExecuteScalar());
            con.Desconectar("DM");


            if (contar == "0")
            {
                return false;

            }
            else
            {
                return true;
            }

        }

        private void Borrar_cliente(String Cliente)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("DELETE FROM  [DM].[dbo].[RUTERO]  WHERE  CLIENTE ='" + Cliente + "'", con.condm);


            cmd.ExecuteNonQuery();

            con.Desconectar("DM");

        }

        private void Borrar_Ruta(String Ruta)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("DELETE FROM  [DM].[dbo].[RUTERO]  WHERE  RUTA ='" + Ruta + "'", con.condm);


            cmd.ExecuteNonQuery();

            con.Desconectar("DM");

        }

        private bool Exists(string cliente, string ruta, string semana, string dia)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("select COUNT (*) from [DM].[dbo].[RUTERO] where CLIENTE = @cliente and RUTA = @ruta AND SEMANA = @semana AND DIA = @dia", con.condm);
            cmd.Parameters.AddWithValue("cliente", cliente);
            cmd.Parameters.AddWithValue("ruta", ruta);
            cmd.Parameters.AddWithValue("semana", semana);
            cmd.Parameters.AddWithValue("dia", dia);
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
        private void Ingresar_cliente(string cliente,string ruta,string semana,string dia ,int secuencia ,string entrega,string nombrecliente)
        {


            if (Exists(cliente, ruta, semana, dia))
            {

            }
            else if (Existe_cliente_diferent_RUTA(cliente, ruta))
            {

                Borrar_cliente(cliente);

                Ingreso_RUTEROS(ruta, dia, semana, nombrecliente, cliente, secuencia, entrega);

            }
            else
            {

                Ingreso_RUTEROS(ruta, dia, semana, nombrecliente, cliente, secuencia, entrega);
            }
                           

                            
        
        }

        private bool Exists_FR_cli_rt_(string cliente)
        {
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("select COUNT (*) from [EXACTUS].[ERPADMIN].[CLIENTE_RT] where CLIENTE = @cliente ", con.conex);
            cmd.Parameters.AddWithValue("cliente", cliente);

            int contar = Convert.ToInt32(cmd.ExecuteScalar());
            con.Desconectar("EX");

            if (contar == 0)
            {
                return true;

            }
            else
            {
                return false;
            }

        }

        private bool Exists_FR_asoc_rt_(string cliente)
        {
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("select COUNT (*) from [EXACTUS].[ERPADMIN].[CLIENTE_ASOC_RT] where CLIENTE = @cliente ", con.conex);
            cmd.Parameters.AddWithValue("cliente", cliente);

            int contar = Convert.ToInt32(cmd.ExecuteScalar());
            con.Desconectar("EX");

            if (contar == 0)
            {
                return true;

            }
            else
            {
                return false;
            }

        }

        private void Ingreso_RUTEROS(string Rutas, string Dias, string Semanas,string Nombreclie,string Cliente_ing,int orden,string Agente)
        {
            string fecha = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");


         try
            {
            


                
                    if (Exists_FR_cli_rt_(Cliente_ing))
                    {



                        con.conectar("DM");

                        SqlCommand cmd1 = new SqlCommand("[CORRECT].[CREACLIE_FR]", con.condm);
                        cmd1.CommandType = CommandType.StoredProcedure;

                        cmd1.Parameters.AddWithValue("@TABLA", 1);
                        cmd1.Parameters.AddWithValue("@CODCLI", Cliente_ing);
                        cmd1.Parameters.AddWithValue("@NOMBRE", Nombreclie);
                        cmd1.Parameters.AddWithValue("@empresa", EMPRESA);

                    cmd1.ExecuteNonQuery();

                        con.Desconectar("DM");

                    }

                    if (Exists_FR_asoc_rt_(Cliente_ing))
                    {



                        con.conectar("DM");

                        SqlCommand cmd2 = new SqlCommand("[CORRECT].[CREACLIE_FR]", con.condm);
                        cmd2.CommandType = CommandType.StoredProcedure;

                        cmd2.Parameters.AddWithValue("@TABLA", 2);
                        cmd2.Parameters.AddWithValue("@CODCLI", Cliente_ing);
                        cmd2.Parameters.AddWithValue("@NOMBRE", Nombreclie);
                         cmd2.Parameters.AddWithValue("@empresa", EMPRESA);
                    cmd2.ExecuteNonQuery();

                        con.Desconectar("DM");
                    }


                   

                    if (Estatus == "N")
                    {
                        con.conectar("EX");
                        SqlCommand cmd2 = new SqlCommand("UPDATE [EXACTUS].["+EMPRESA+"].[CLIENTE] SET ACTIVO = 'S',USUARIO_ULT_MOD = '" + Login.usuario.ToUpper() + "',FCH_HORA_ULT_MOD = '" + fecha + "' where CLIENTE = '" + Cliente_ing + "'", con.conex);


                        cmd2.ExecuteNonQuery();

                        con.Desconectar("EX");
                    }

                    con.conectar("DM");

                    SqlCommand cmd3 = new SqlCommand("[CORRECT].[RUTERO_INSERT]", con.condm);
                    cmd3.CommandType = CommandType.StoredProcedure;

                    cmd3.Parameters.AddWithValue("@RUTA", Rutas);
                    cmd3.Parameters.AddWithValue("@CLIENTE", Cliente_ing);
                    cmd3.Parameters.AddWithValue("@DIA", Dias);
                    cmd3.Parameters.AddWithValue("@ORDEN", orden);
                    cmd3.Parameters.AddWithValue("@UpdatedBy", Login.usuario.ToUpper());
                    cmd3.Parameters.AddWithValue("@SEMANA", Semanas);
                    cmd3.Parameters.AddWithValue("@fecha_crea", fecha);
                     cmd3.Parameters.AddWithValue("@empresa", EMPRESA);


                cmd3.ExecuteNonQuery();

                    con.Desconectar("DM");



                string cobrador;

                if (Escorp == "N")
                {
                    cobrador = "CXC";
                }
                else
                {
                    cobrador = ENTREGA.Replace('E', 'C');
                }


                    con.conectar("EX");
                    SqlCommand cmd4 = new SqlCommand("UPDATE [EXACTUS].["+EMPRESA+"].[CLIENTE] SET VENDEDOR='" + RUTA.Replace('R','V') + "' , RUTA = '"+ENTREGA+"' , COBRADOR = '"+cobrador+"' WHERE CLIENTE ='" + Cliente_ing + "'", con.conex);


                    cmd4.ExecuteNonQuery();

                    con.Desconectar("EX");

        
               }
             catch
                  {
             
                    }
        
        
        }


        private void estatus_clie(string clientest)
        {

            con.conectar("EX");

            SqlCommand cm1 = new SqlCommand("SELECT NOMBRE,ACTIVO,ES_CORPORACION FROM [EXACTUS].[" + EMPRESA+"].[CLIENTE]  WHERE CLIENTE = '" + clientest + "' ", con.conex);
            SqlDataReader dr1 = cm1.ExecuteReader();
            while (dr1.Read())
            {
                Estatus = Convert.ToString(dr1["ACTIVO"]);
                Nombreclient = Convert.ToString(dr1["NOMBRE"]);
                Escorp = Convert.ToString(dr1["ES_CORPORACION"]);
            }
            dr1.Close();
            con.Desconectar("EX");
        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "")
            {
                MessageBox.Show("SELECCIONE UNA RUTA");
                comboBox2.Focus();
            }
            else
            {
                MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show("ANTES DE  LIMPIAR  LA RUTA: " + this.comboBox2.Text + "  LE SUGERIMOS GENERAR UN RUTERO. \n DESEA GENERARLO ?", "REESTRUCTURA ", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {
                    Ruteroreporte report = new Ruteroreporte();
                    report.ShowDialog();
                }
                else
                {
                    Borrar_Ruta(comboBox2.Text);
                    Inset_auditoria(Login.usuario.ToUpper(), DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), "Proceso de Limpieza  de RUTA " + comboBox2.Text + "");
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {

                comboBox2.Enabled = true;
                button2.Enabled = true;
            }
            else
            {
                comboBox2.Enabled = false;
                button2.Enabled = false;
             
            }
        }
        private void Inset_auditoria(string usuario,string fecha,string proceso)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("INSERT INTO [DM].[CORRECT].[AUDIT_LIMP_RUTERO] (FECHA,USUARIO,PROCESO) VALUES (@FECHA,@USUARIO,@PROCESO)", con.condm);
            cmd.Parameters.AddWithValue("FECHA", fecha);
            cmd.Parameters.AddWithValue("USUARIO", usuario);
            cmd.Parameters.AddWithValue("PROCESO", proceso);
            cmd.ExecuteNonQuery();

            con.Desconectar("DM");
        
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Carga_data_xls();
        }

    }
}
