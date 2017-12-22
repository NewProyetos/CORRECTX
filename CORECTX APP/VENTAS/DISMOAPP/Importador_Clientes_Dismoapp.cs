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
using System.Data.OleDb;
using System.Xml;
using MySql.Data.MySqlClient;
using System.Net;

namespace Sinconizacion_EXactus
{
    public partial class Importador_Clientes_Dismoapp : Form
    {
        public Importador_Clientes_Dismoapp()
        {
            InitializeComponent();
        }
        String Selected_File;
        DataTable dt = new DataTable() ;
        Int32 ID;
        conexionXML con = new conexionXML();
        Int32 Registros;
        String UserWeb;
        String Usuario = Rutas_Trade_Reps.Usuario_selected;

        String RUTA;
        String SUPERVISOR;
        Int32 ORDEN;
        String DIA;        
        String CODIGO;
        String NOMBRE;
        String ALIAS;
        String DIRECCION;
        String TELEFONO;
        String CELULAR;
        String ENTREGA;
        String VENDEDOR;
        String COBRADOR;
        String DOCUMENTO;
        String DUI;
        String NIT;
        String REGISTRO;
        String RUBRO;
        String CONDICION_PAGO;
        String LIMITE_CREDITO;
        String LATITUD;
        String LONGITUD;
        int operador;




        Int32 EstadoError;
        

        private void Importador_Clientes_Dismoapp_Load(object sender, EventArgs e)
        {
            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            String cat = Rutas_Trade_Reps.categoria;
            button3.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = "Hoja1";

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
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Carga_data_xls();
        }

        private void Carga_data_xls()
        {
            string nombre_hoja = textBox2.Text;
           // button2.Text = "Importar";


            if (textBox1.Text == "" || textBox1.Text == null)
            {
                MessageBox.Show("no se a selecionado un archivo de Excel");

            }
            else
            {

                try
                {


                    OleDbConnection cnn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Selected_File + "; Extended Properties=Excel 12.0;");

                    OleDbCommand oconn = new OleDbCommand("select * from [" + nombre_hoja + "$]", cnn);
                    cnn.Open();
                    OleDbDataAdapter adp = new OleDbDataAdapter(oconn);
                    dt.Clear();
                    dt.Columns.Clear();
                    adp.Fill(dt);


                    foreach (DataRow row in dt.Rows)
                    {
                        if (String.IsNullOrEmpty(row["CLIENTE"].ToString()))

                            row.Delete();
                    }
                    dt.AcceptChanges();

                    dataGridView1.DataSource = dt;
                    dataGridView1.Refresh();


                }
                //catch 
                //{
                //    MessageBox.Show("Error al abrir archivo de Excel");
                //}

                catch ( WebException e)
                {
                    MessageBox.Show(Convert.ToString(e));
                }

            }

        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            button3.Enabled = true;
            label4.Text =Convert.ToString(dt.Rows.Count);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            operador = 0;

            string fecha = DateTime.Now.ToString("yyyy-MM-dd");
          

            Registros = dt.Rows.Count;

            if (Registros > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (Usuario != row["SUPERVISOR"].ToString())
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

                    MessageBox.Show("NO PUEDES INGRESAR RUTA DE OTRO SUPERVISOR SELECIONADO");
                }
                else
                {
                   // Elimar_Ruta();
                  //  Reindex();
                   // Ultimo_ID();
             
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow row = dt.Rows[i];

                        RUTA = Convert.ToString(row["VENDEDOR"]);
                        DIA = Convert.ToString(row["DIA_VISITA"]);
                        SUPERVISOR = Convert.ToString(row["SUPERVISOR"]);
                        ORDEN = Convert.ToInt32(row["ORDEN"]);

                        if (i == 0 && DIA != "CUSTOM" )
                        {
                            operador = 1;
                        }
                        else
                        {
                            operador = 0;
                        }

                      

                        CODIGO = Convert.ToString(row["CLIENTE"]);

                        int digitos = CODIGO.Length;

                        switch (digitos)
                        {
                            case 1:
                                CODIGO = "0000" + CODIGO;
                                break;
                            case 2:
                                CODIGO = "000" + CODIGO;
                                break;
                            case 3:
                                CODIGO = "00" + CODIGO;
                                break;
                            case 4:
                                CODIGO = "0" + CODIGO;
                                break;                        
                        }


                      






                        if (Exite_cliente(CODIGO, SUPERVISOR))
                        {
                            MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
                            DialogResult result = MessageBox.Show("CLIENTE " + CODIGO + " YA EXISTE EN OTRA RUTA NO SE AGREGARA A RUTA \n ¿DESEA FINALIZAR LA IMPORTACION?", "CARGA CLIENTES DISMOAPP", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                            if (result == DialogResult.Yes)
                            {
                                break;
                            }
                        }
                        else



                            datos_clie(CODIGO);



                            if (DBNull.Value == (row["CLIENTE"]))
                            {

                            }
                            else
                                if (DBNull.Value == (row["VENDEDOR"]))
                                {

                                }
                                else
                                    if (DBNull.Value == (row["DIA_VISITA"]))
                                    {

                                    }
                                    else
                                        if (DBNull.Value == (row["SUPERVISOR"]))
                                        {


                                        }

                                        else
                                        {
                                            try
                                            {
                                                con.conectar("WEB");

                                                ID = ID + 1;
                                                MySqlCommand cmd = new MySqlCommand();
                                                cmd.Connection = con.mysqlconec;
                                                //cmd.CommandText = "INSERT INTO dismodb.dmRutero (ruta,codcli,dia,orden,updateby,fechaupdate,equipo) VALUES (@ruta,@codcli,@dia,@orden,@updateby,@fechaupdate,@equipo)";
                                                //cmd.CommandText = "CALL UPDATE_CLIENTESYRUTERO (@porden, @pdia, @pcliente,@pnombre,@palias,@pdireccion, @ptel , @pcel , @pruta ,@pvendedor,@pentrega , @pcobrador ,@ptipodoc,@pdui,@pnit , @pregistro,@pgiro,@pcondicion,@plimite,@platitud, @plongitud,@pusuario , @pupdateby , @operador);";
                                                cmd.CommandText = "SET SQL_SAFE_UPDATES=0; CALL UPDATE_CLIENTESYRUTERO('" + ORDEN + "','" + DIA + "','" + CODIGO + "','" + NOMBRE + "','" + ALIAS + "','" + DIRECCION + "','" + TELEFONO + "','" + CELULAR + "','" + RUTA + "','" + VENDEDOR + "','" + ENTREGA + "','" + COBRADOR + "','" + DOCUMENTO + "','" + DUI + "','" + NIT + "','" + REGISTRO + "','" + RUBRO + "','" + CONDICION_PAGO + "','" + LIMITE_CREDITO + "','" + LATITUD + "','" + LONGITUD + "','" + Usuario+ "','" + Login.usuario.ToUpper() + "','"+operador+"')";
                                                cmd.ExecuteNonQuery();
                                                con.Desconectar("WEB");
                                         
                                              

                                               


                                               
                                                row.Delete();
                                                dataGridView1.DataSource = dt;
                                                dataGridView1.Refresh();
                                            }
                                            catch
                                            {

                                                MessageBox.Show("ERROR NO SE ENGRESARON REGISTROS");
                                            }

                                        }




                    }


                   // Reindex();

                    MessageBox.Show("CARGA DE CLIENTES EXITOSA");
                    button3.Enabled = false;

                }

            }
        }

        private void Ultimo_ID()
        {

            string comdrt = "SELECT MAX(ID) as ID FROM ROUTE";
            con.conectar("WEB");
            MySqlCommand mcm = new MySqlCommand(comdrt, con.mysqlconec);
            MySqlDataReader mdr = mcm.ExecuteReader();

            while (mdr.Read())
            {
                ID = Convert.ToInt32(mdr["ID"]);

            }


            con.Desconectar("WEB");
        
        }

        private void Elimar_Ruta()
        {

            string comdrt = "DELETE FROM dismodb.dmRutero Where equipo = '" + Usuario + "'";
            
            con.conectar("WEB");
            MySqlCommand mcm = new MySqlCommand(comdrt, con.mysqlconec);
           
            mcm.ExecuteNonQuery();
          
            con.Desconectar("WEB");

        }
        private void Reindex()
        {
            con.conectar("WEB");

            string comp1 = " SELECT Reindex() ";

          
            MySqlCommand mcm3 = new MySqlCommand(comp1, con.mysqlconec);
          
            mcm3.ExecuteNonQuery();
            con.Desconectar("WEB");
        }
      

        private bool Exite_cliente(string cliente, string Ruta)
        {
            string comdrt = "SELECT COUNT(*) FROM dismodb.dmRutero as rt LEFT JOIN dismodb.dmUsuarios as us on rt.equipo = us.equipo WHERE rt.codcli = '"+cliente+"' and rt.equipo <> '"+SUPERVISOR+"'  and us.perfil = '"+Rutas_Trade_Reps.perfil+"' and us.categoria = '"+Rutas_Trade_Reps.categoria+"'  ;";
            con.conectar("WEB");
            MySqlCommand mcm = new MySqlCommand(comdrt, con.mysqlconec);
            int contar = Convert.ToInt32(mcm.ExecuteScalar());
            con.Desconectar("WEB");
            if (contar == 0)
            {
                return false;

            }
            else
            {
                return true;

            }
        
        }

        private void datos_clie(string cliente)
        {
            RUTA = "";
           // CODIGO = "";
            NOMBRE = "";
            ALIAS = "";
            DIRECCION = "";
            TELEFONO = "";
            CELULAR = "";
            ENTREGA = "";
            VENDEDOR = "";
            COBRADOR = "";
            DOCUMENTO = "";
            DUI = "";
            NIT = "";
            REGISTRO = "";
            RUBRO = "";
            CONDICION_PAGO = "";
            LIMITE_CREDITO = "";
            LATITUD = "";
            LONGITUD = "";




            con.conectar("DM");
            SqlCommand cm2 = new SqlCommand("SELECT clie.[NOMBRE],clie.[ALIAS],clie.[DIRECCION],clie.[TELEFONO1],clie.[TELEFONO2],REPLACE(clie.VENDEDOR,'V','R') as RUTA,clie.[RUTA] as ENTREGA,clie.[VENDEDOR],clie.[COBRADOR],clie.[PAIS],clie.[RUBRO5_CLI],clie.[CONTRIBUYENTE],clie.[RUBRO1_CLI],clie.[RUBRO2_CLI],clie.[CONDICION_PAGO],clie.[LIMITE_CREDITO],ubi.LATITUD,ubi.LONGITUD  FROM [EXACTUS].[dismo].[CLIENTE] as clie left join [EXACTUS].[ERPADMIN].[CLIENTE_UBICACION] as ubi on clie.CLIENTE = ubi.CLIENTE where clie.CLIENTE = '"+cliente+"'", con.condm);
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {

                //toolStripComboBox1.Items.Add(dr2["RUTA"]);



                NOMBRE = Convert.ToString(dr2["NOMBRE"]);
                ALIAS = Convert.ToString(dr2["ALIAS"]);
                DIRECCION = Convert.ToString(dr2["DIRECCION"]);
                TELEFONO = Convert.ToString(dr2["TELEFONO1"]);
                CELULAR = Convert.ToString(dr2["TELEFONO2"]);
                RUTA = Convert.ToString(dr2["RUTA"]);
                ENTREGA = Convert.ToString(dr2["ENTREGA"]);
                VENDEDOR = Convert.ToString(dr2["VENDEDOR"]);
                COBRADOR = Convert.ToString(dr2["COBRADOR"]);
                DOCUMENTO = Convert.ToString(dr2["PAIS"]);
                DUI = Convert.ToString(dr2["RUBRO5_CLI"]);
                NIT = Convert.ToString(dr2["CONTRIBUYENTE"]);
                REGISTRO = Convert.ToString(dr2["RUBRO1_CLI"]);
                RUBRO = Convert.ToString(dr2["RUBRO2_CLI"]);
                CONDICION_PAGO = Convert.ToString(dr2["CONDICION_PAGO"]);
                LIMITE_CREDITO = Convert.ToString(dr2["LIMITE_CREDITO"]);

                

                if (DBNull.Value == (dr2["LATITUD"]) || DBNull.Value == (dr2["LONGITUD"]))
                {
                    LATITUD = "0";
                    LONGITUD = "0";
                }
                else
                {
                    LATITUD = Convert.ToString(dr2["LATITUD"]);
                    LONGITUD = Convert.ToString(dr2["LONGITUD"]);
                }


               
                    
               


            }
            dr2.Close();
            con.Desconectar("DM");

        
        }
            
    }
}
