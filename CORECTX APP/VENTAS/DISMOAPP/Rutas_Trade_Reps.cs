using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sinconizacion_EXactus
{
    public partial class Rutas_Trade_Reps : Form
    {
        public Rutas_Trade_Reps()
        {
            InitializeComponent();
        }
        conexionXML con = new conexionXML();
        DataTable clie_ruta = new DataTable();
        DataTable CLIENTES = new DataTable();
        public static DataTable Rutassup = new DataTable();
        DataSet ds = new DataSet();
        String cmdruta;
        String cmdsup;
        String Puesto;
        // Int32 idx;
        String CODCLIE;
        Int32 ID;
        Int32 RO;
        String TYPE;
        public static String categoria;
        public static String perfil;
        public static String Usuario_selected;
        String Usuario;
        string query = "";
        String Rutaname;
        public CORECTX_APP.VENTAS.wait wt;
        private static MySqlCommand cmruta;
        DataTable clieruta = new DataTable();
        DataTable cliefull = new DataTable();

        private void Rutas_Trade_Reps_Load(object sender, EventArgs e)
        {
            groupBox5.Hide();
            toolStripButton5.Enabled = false;
            radioButton1.Hide();
            radioButton2.Hide();
            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;

            dataGridView2.Enabled = true;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.ReadOnly = true;
            dataGridView2.AllowUserToAddRows = false;
            toolStripButton1.Enabled = false;
            toolStripButton2.Enabled = false;

            backgroundRutero.WorkerSupportsCancellation = true;

            con.conectar("DM");
            SqlCommand cm2 = new SqlCommand("SELECT [RUTA] FROM [DM].[dbo].[RUTERO]  group by RUTA  order by RUTA", con.condm);
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {

                toolStripComboBox1.Items.Add(dr2["RUTA"]);
            }
            dr2.Close();
            con.Desconectar("DM");


            clientesfull(null, null, null);
            clientesrt();
            clientemix(CLIENTES, clieruta);
            // Usuario = "TURCIOSI";
            String Usuario = Main_Menu.USERWEB;

            Puesto = Main_Menu.Puesto;
            //String Puesto = "ADMIN";
            switch (Puesto)
            {
                case "USR":
                    comboBox1.Enabled = false;
                    comboBox2.Text = "Todos";
                    comboBox1.Text = Usuario;
                    label4.Text = comboBox1.Text;
                    cmdruta = "SELECT dia FROM dismodb.dmRutero where equipo = '" + Usuario + "' Group by dia;";
                    cargaRUTAS(cmdruta);
                    //comboBox1_SelectedIndexChanged(null,null);


                    break;
                case "ADMIN":
                    cmdsup = "SELECT usuario FROM dismodb.dmUsuarios;";
                    cargaSupervALL(cmdsup);
                    comboBox1.Text = "Todos";
                    comboBox2.Text = "Todos";
                    break;
                case "SUPBAT":
                   
                    cmdsup = "SELECT usuario FROM dismodb.dmUsuarios where perfil = 'TMR';";
                    cargaSupervALL(cmdsup);
                   // comboBox1.Text = "Todos";
                   // comboBox2.Text = "Todos";
                    break;
                case "SUPDM":
                  
                    cmdsup = "SELECT usuario FROM dismodb.dmUsuarios where perfil = 'DISMO';";
                    cargaSupervALL(cmdsup);
                    //comboBox1.Text = "Todos";
                    //comboBox2.Text = "Todos";
                    break;
                default:
                    comboBox1.Text = "";
                    comboBox1.Enabled = false;
                    comboBox2.Enabled = false;

                    break;

            }
        }
        private void cargaRUTAS(string comdrt)
        {
            Rutassup.Clear();
            con.conectar("WEB");
            MySqlCommand mcm = new MySqlCommand(comdrt, con.mysqlconec);
            // MySqlDataReader mdr = mcm.ExecuteReader();
            MySqlDataAdapter msda = new MySqlDataAdapter(mcm);
            msda.Fill(Rutassup);


            //while (mdr.Read())
            //{
            //    comboBox2.Items.Add(mdr["dia"]);

            //}
            con.Desconectar("WEB");

            combo(Rutassup);

        }
        private void cargaSupervALL(string comd)
        {

            con.conectar("WEB");
            MySqlCommand mcm = new MySqlCommand(comd, con.mysqlconec);
            MySqlDataReader mdr = mcm.ExecuteReader();

            while (mdr.Read())
            {
                comboBox1.Items.Add(mdr["usuario"]);

            }

            con.Desconectar("WEB");


        }
        private void categoriasup(string Superv)
        {

            con.conectar("WEB");
            MySqlCommand mcm = new MySqlCommand("SELECT categoria,perfil FROM dismodb.dmUsuarios where usuario = '" + Superv + "';", con.mysqlconec);
            MySqlDataReader mdr = mcm.ExecuteReader();

            while (mdr.Read())
            {
                categoria = Convert.ToString(mdr["categoria"]);
                perfil = Convert.ToString(mdr["perfil"]);

            }

            con.Desconectar("WEB");


        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripButton5.Enabled = false;
            Usuario_selected = comboBox1.Text;
            comboBox2.Items.Clear();





            if (comboBox1.Text == "Todos")
            {
                if (Puesto == "ADMIN")
                {
                    radioButton1.Show();
                    radioButton2.Show();
                    radioButton1.Enabled = true;
                    radioButton2.Enabled = true;
                    cmdruta = "SELECT dia FROM dismodb.dmRutero Group by dia;";
                   
                    cargaRUTAS(cmdruta);
                    //Cargaclientesruta();
                }
                else if (Puesto == "SUPBAT")
                {
                    radioButton1.Hide();
                    radioButton2.Hide();

                    cmdruta = "SELECT rt.dia FROM dismodb.dmRutero rt inner join dismodb.dmUsuarios us on rt.equipo = us.equipo group by rt.dia and us.categoria = 'TMR'; ";
                    cargaRUTAS(cmdruta);
                    Cargaclientesruta();
                }

                else if (Puesto == "SUPDM")
                {
                    radioButton1.Hide();
                    radioButton2.Hide();
                    cmdruta = "SELECT rt.dia FROM dismodb.dmRutero rt inner join dismodb.dmUsuarios us on rt.equipo = us.equipo group by rt.dia and us.perfil = 'DISMO';";
                    cargaRUTAS(cmdruta);
                    Cargaclientesruta();
                }
            }
            else
            {
                radioButton1.Hide();
                radioButton2.Hide();
                cmdruta = "SELECT dia FROM dismodb.dmRutero where equipo = '" + comboBox1.Text + "' group by dia;";
                cargaRUTAS(cmdruta);
                categoriasup(comboBox1.Text);
                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
                Cargaclientesruta();

            }

            label4.Text = comboBox1.Text;

        }

        private void toolStripSplitButton1_Click(object sender, EventArgs e)
        {
            Importador_Clientes_Dismoapp imp = new Importador_Clientes_Dismoapp();
            DialogResult res = imp.ShowDialog();

            if (res == DialogResult.OK)
            {
                Cargaclientesruta();
            }
        }

        private void toolStrip4_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ROID();
            clie_ruta.DefaultView.RowFilter = "dia = '" + this.comboBox2.Text + "'";
            dataGridView1.DataSource = clie_ruta;
            label7.Text = comboBox2.Text;
            label8.Text = Convert.ToString(dataGridView1.RowCount);

        }

        private void clientesfull(string ruta, string semana, string dia)
        {
            con.conectar("DM");

            SqlCommand cmd = new SqlCommand("[CORRECT].[APP_RUTEROCLIENTES]", con.condm);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 50;
            cmd.Parameters.AddWithValue("@ruta", ruta);
            cmd.Parameters.AddWithValue("@semana", semana);
            cmd.Parameters.AddWithValue("@dia", dia);

            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(CLIENTES);


            con.Desconectar("DM");



        }
        private void clientesrt()
        {

            con.conectar("WEB");
            string comdrt = "SELECT CODCLI as CLIENTE,equipo as ASIGNADO FROM dismodb.dmRutero;";
            MySqlCommand msq = new MySqlCommand(comdrt, con.mysqlconec);
            MySqlDataAdapter msda = new MySqlDataAdapter(msq);
            msda.Fill(clieruta);
            con.Desconectar("WEB");

        }
        private void clientemix(DataTable dm, DataTable web)
        {
            //var result = from t1 in dm.AsEnumerable()
            //             join t2 in web.AsEnumerable()
            //             on t1.Field<string>("CLIENTE") equals t2.Field<string>("CLIENTE")
            //             select new { t1, t2 };
            //cliefull.Columns.Add("CLIENTE", typeof(string));
            //cliefull.Columns.Add("ASIGNADO", typeof(string));

            //foreach (var dr in result)
            //{
            //    DataRow nrow = cliefull.NewRow();
            //    nrow["CLIENTE"] = dr.t1.Field<string>("CLIENTE");
            //    nrow["ASIGNADO"] = dr.t2.Field<string>("ASIGNADO");

            //    cliefull.Rows.Add(nrow);
            //}


            var values = from rowLeft in dm.AsEnumerable()
                         join rowRight in web.AsEnumerable() on rowLeft["CLIENTE"] equals rowRight["CLIENTE"] into gj
                         from subRight in gj.DefaultIfEmpty()
                         select  rowLeft.ItemArray.Concat((subRight == null) ? (web.NewRow().ItemArray) : subRight.ItemArray).ToArray() ;

            cliefull.Columns.Add("RUTA", typeof(string));
            cliefull.Columns.Add("CLIENTE", typeof(string));
            cliefull.Columns.Add("ORDEN", typeof(string));
            cliefull.Columns.Add("NOMDIA", typeof(string));
            cliefull.Columns.Add("NOMBRE", typeof(string));
            cliefull.Columns.Add("ALIAS", typeof(string));
            cliefull.Columns.Add("DIRECCION", typeof(string));
            cliefull.Columns.Add("TELEFONO1", typeof(string));
            cliefull.Columns.Add("TELEFONO2", typeof(string));
            cliefull.Columns.Add("ENTREGA", typeof(string));
            cliefull.Columns.Add("VENDEDOR", typeof(string));
            cliefull.Columns.Add("COBRADOR", typeof(string));
            cliefull.Columns.Add("TIPDOC", typeof(string));
            cliefull.Columns.Add("DUI", typeof(string));
            cliefull.Columns.Add("NIT", typeof(string));
            cliefull.Columns.Add("REGISTRO", typeof(string));
            cliefull.Columns.Add("GIRO", typeof(string));
            cliefull.Columns.Add("CONDICION", typeof(string));
            cliefull.Columns.Add("LIMITE", typeof(string));
            cliefull.Columns.Add("SALDO", typeof(string));
            cliefull.Columns.Add("LATITUD", typeof(string));
            cliefull.Columns.Add("LONGITUD", typeof(string));
            cliefull.Columns.Add("CLIE2", typeof(string));
            cliefull.Columns.Add("ASIGNADO", typeof(string));

            //Add row data to dtblResult
            foreach (object[] valores in values)
                cliefull.Rows.Add(valores);
            cliefull.Columns.Remove("CLIE2");
            


            //cliefull = values.CopyToDataTable();

        }




        private void toolStripTextBox3_TextChanged(object sender, EventArgs e)
        {
            if (toolStripTextBox3.Text != "")
            {
                cliefull.DefaultView.RowFilter = "CLIENTE like  '" + this.toolStripTextBox3.Text + "%'";                
                dataGridView2.DataSource = cliefull;
            }

        }

        private void toolStripTextBox4_TextChanged(object sender, EventArgs e)
        {
            cliefull.DefaultView.RowFilter = "NOMBRE like '" + this.toolStripTextBox4.Text + "%'";            
            dataGridView2.DataSource = cliefull;
        }

        private void toolStripTextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                // MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {

            clie_ruta.DefaultView.RowFilter = "CLIENTE like '" + this.toolStripTextBox1.Text + "%'";
            dataGridView1.DataSource = clie_ruta;


        }
        private void Cargaclientesruta()
        {
            // clie_ruta.Clear();


            if (comboBox1.Text == "Todos")
            {
                if (Puesto == "ADMIN")
                {
                    if (radioButton1.Checked)
                    {
                        query = "SELECT rut.codcli as CLIENTE,clie.nombre as NOMBRE,clie.nomnegocio,clie.direccion,rut.ruta,clie.entrega,rut.dia,rut.orden,rut.equipo FROM dismodb.dmRutero as rut left join dismodb.dmClientes as clie on rut.codcli = clie.codigo LEFT JOIN dismodb.dmUsuarios us on rut.equipo = us.usuario where us.categoria = 'TMR'";
                    }
                    else if (radioButton2.Checked)
                    {
                        query = "SELECT rut.codcli as CLIENTE,clie.nombre as NOMBRE,clie.nomnegocio,clie.direccion,rut.ruta,clie.entrega,rut.dia,rut.orden,rut.equipo FROM dismodb.dmRutero as rut left join dismodb.dmClientes as clie on rut.codcli = clie.codigo LEFT JOIN dismodb.dmUsuarios us on rut.equipo = us.usuario where us.categoria = 'Supervisor'";
                    }
                    else
                    {

                    }
                }
                else if (Puesto == "SUPBAT")
                {
                    query = "SELECT rut.codcli as CLIENTE,clie.nombre as NOMBRE,clie.nomnegocio,clie.direccion,rut.ruta,clie.entrega,rut.dia,rut.orden,rut.equipo FROM dismodb.dmRutero as rut left join dismodb.dmClientes as clie on rut.codcli = clie.codigo LEFT JOIN dismodb.dmUsuarios us on rut.equipo = us.usuario where us.categoria = 'TMR'";

                }

                else if (Puesto == "SUPDM")
                {
                    query = "SELECT rut.codcli as CLIENTE,clie.nombre as NOMBRE,clie.nomnegocio,clie.direccion,rut.ruta,clie.entrega,rut.dia,rut.orden,rut.equipo FROM dismodb.dmRutero as rut left join dismodb.dmClientes as clie on rut.codcli = clie.codigo LEFT JOIN dismodb.dmUsuarios us on rut.equipo = us.usuario where us.categoria = 'supervisor'";

                }
            }
            else
            {

                query = "SELECT rut.codcli as CLIENTE,clie.nombre as NOMBRE,clie.nomnegocio,clie.direccion,rut.ruta,clie.entrega,rut.dia,rut.orden,rut.equipo FROM dismodb.dmRutero as rut left join dismodb.dmClientes as clie on rut.codcli = clie.codigo where rut.equipo = '" + comboBox1.Text + "';";

            }


            if (backgroundRutero.IsBusy != true)
            {
                // wt = new CORECTX_APP.VENTAS.wait();
                //wt.ShowDialog();
                //clie_ruta.Clear();

                groupBox5.Show();
                if (dataGridView1.Rows.Count >= 0)
                {
                    dgclean();
                }
                backgroundRutero.RunWorkerAsync();


            }
            else
            {
                if (dataGridView1.Rows.Count >= 0)
                {
                    dgclean();
                }
            }






            //con.Desconectar("WEB");

            label5.Text = Convert.ToString(clie_ruta.Rows.Count);
        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                // MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void toolStripTextBox2_TextChanged(object sender, EventArgs e)
        {
            clie_ruta.DefaultView.RowFilter = "NOMBRE like '" + this.toolStripTextBox2.Text + "%'";
            dataGridView1.DataSource = clie_ruta;
        }


        private void Quitaruta(object sender, EventArgs e)
        {

            if (Convert.ToInt32(CODCLIE) != 0)
            {
                MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show("ESTA SEGURO QUE DESEA QUITAR ESTE CLIENTE: " + Convert.ToString(CODCLIE) + "", "QUITAR DE RUTA", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {
                    //try
                    //{

                    int idx = dataGridView1.CurrentRow.Index;
                    string Ruta = Convert.ToString(dataGridView1.Rows[idx].Cells[5].Value);
                    if (idx >= 0)
                    {
                        con.conectar("WEB");
                        string comdrt = "SET SQL_SAFE_UPDATES=0; DELETE FROM dismodb.dmRutero where equipo= '" + this.comboBox1.Text + "' and   CODCLI = '" + Convert.ToString(CODCLIE) + "';";
                        MySqlCommand msq = new MySqlCommand(comdrt, con.mysqlconec);
                        msq.ExecuteNonQuery();
                        con.Desconectar("WEB");



                        //clie_ruta.Rows.RemoveAt(idx);

                        //dataGridView1.Refresh();
                        // Reindex();
                        Cargaclientesruta();
                    }





                    statusStrip1.BackColor = Color.OrangeRed;
                    toolStripStatusLabel1.Text = "Cambios realizados en Ruta :" + this.comboBox1.Text + "   Cliente:  " + Convert.ToString(CODCLIE) + "  ELIMINADO   Por Usuario:  " + Usuario + "";
                    CODCLIE = "0";

                    //}
                    //catch
                    //{
                    //    statusStrip1.BackColor = Color.Red;
                    //    toolStripStatusLabel1.Text = " !!ERROR!!   en Ruta :" + this.comboBox1.Text + "   Cliente:  " + Convert.ToString(CODCLIE) + " NO PUDO SER ELIMINADO  ";
                    //    CODCLIE = 0;
                    //}
                }


            }
            else
            {

                MessageBox.Show("NO A SELECCIONADO UN CLIENTE");
            }



        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            statusStrip1.BackColor = Color.Empty;
            toolStripStatusLabel1.Text = "Ready";

            int idx = dataGridView1.CurrentRow.Index;
            CODCLIE = Convert.ToString(dataGridView1.Rows[idx].Cells[0].Value);
            if (Convert.ToInt32(CODCLIE) != 0)
            {
                toolStripButton1.Enabled = true;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            Quitaruta(null, null);


        }
        private void Reindex()
        {
            con.conectar("WEB");

            string comp1 = " SELECT Reindex() ";


            MySqlCommand mcm3 = new MySqlCommand(comp1, con.mysqlconec);

            mcm3.ExecuteNonQuery();
            con.Desconectar("WEB");
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text != "Todos")
            {
                //TYPER();
                //ROID();
                int idxt = dataGridView2.CurrentRow.Index;

                agregar_clientedgv(idxt, 1);

                Cargaclientesruta();
                dataGridView1.Refresh();
                statusStrip1.BackColor = Color.Green;
                toolStripStatusLabel1.Text = "Cambios realizados en Ruta :" + this.comboBox1.Text + "   Cliente:  " + Convert.ToString(CODCLIE) + "  AGREGADO   Por Usuario:  " + Login.usuario.ToUpper() + "";
                CODCLIE = "0";



            }
            else
            {

                MessageBox.Show("Seleccione una Ruta para Asignar el Cliente");
                comboBox2.Focus();
            }
        }

        private bool Exite_cliente(string cliente, string Superv)
        {
            string comdrt = "SELECT COUNT(*) FROM dismodb.dmRutero as rt LEFT JOIN dismodb.dmUsuarios as us on rt.equipo = us.equipo WHERE rt.codcli = '" + cliente + "' and rt.equipo <> '" + Superv + "'and categoria = '" + categoria + "' ;";
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
        private void ROID()
        {
            if (comboBox1.Text != "todos" || comboBox1.Text != "")
            {
                Reindex();
                string comdrt = "SELECT MAX(RO) as RO FROM ROUTE where RONAME = '" + this.comboBox2.Text + "' and USR = '" + this.comboBox1.Text + "';";
                con.conectar("WEB");
                MySqlCommand mcm = new MySqlCommand(comdrt, con.mysqlconec);
                MySqlDataReader mdr = mcm.ExecuteReader();

                while (mdr.Read())
                {
                    RO = Convert.ToInt32(mdr["RO"]);

                }


                con.Desconectar("WEB");

            }
        }

        private void TYPER()
        {

            if (comboBox1.Text != "todos" || comboBox1.Text != " ")
            {
                string comdrt = "SELECT MAX(TYPE) as TYPE FROM ROUTE where  USR = '" + this.comboBox1.Text + "';";
                con.conectar("WEB");
                MySqlCommand mcm = new MySqlCommand(comdrt, con.mysqlconec);
                MySqlDataReader mdr = mcm.ExecuteReader();

                while (mdr.Read())
                {
                    TYPE = Convert.ToString(mdr["TYPE"]);

                }


                con.Desconectar("WEB");

            }

        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            int idx = dataGridView2.CurrentRow.Index;
            CODCLIE = Convert.ToString(dataGridView2.Rows[idx].Cells[1].Value);
            if (Convert.ToInt32(CODCLIE) != 0)
            {
                toolStripButton2.Enabled = true;
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (ds.Tables.Contains("RUTERO"))
            {
                ds.Tables.Remove(clie_ruta);
            }

            Exportador ex = new Exportador();
            DateTime hoy = DateTime.Today;
            clie_ruta.TableName = "RUTERO";
            ds.Tables.Add(clie_ruta);
            ex.NombreReporte = "RUTERO";
            ex.aExcel(ds, hoy, hoy);
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripComboBox2.Text = "Todos";
            toolStripComboBox3.Text = "Todos";

            CLIENTES.DefaultView.RowFilter = "RUTA like '" + this.toolStripComboBox1.Text + "%'";
            dataGridView2.DataSource = CLIENTES;


        }

        private void toolStripComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (toolStripComboBox3.Text != "Todos")
            {
                if (toolStripComboBox1.Text != "")
                {
                    CLIENTES.DefaultView.RowFilter = " RUTA like '" + this.toolStripComboBox1.Text + "%' AND  NOMDIA like '" + this.toolStripComboBox3.Text + "%'";
                    dataGridView2.DataSource = CLIENTES;
                }
            }

        }

        private void toolStripComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripComboBox2.Text != "Todos")
            {
                if (toolStripComboBox1.Text != "" && toolStripComboBox3.Text != "")
                {
                    string diasemana = this.toolStripComboBox3.Text + this.toolStripComboBox2.Text;
                    CLIENTES.DefaultView.RowFilter = " RUTA like '" + this.toolStripComboBox1.Text + "%' AND  NOMDIA = '" + diasemana + "'";
                    dataGridView2.DataSource = CLIENTES;
                }
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            //CORECTX_APP.Informatica.Power_Street.filtro_carga filtro = new CORECTX_APP.Informatica.Power_Street.filtro_carga();
            //DialogResult res = filtro.ShowDialog();

            //if (res == DialogResult.OK)
            //{
            //    Rutaname = filtro.ROUTENAME;
            //}


            if (toolStripComboBox1.Text != "")
            {

                if (toolStripComboBox2.Text != "")
                {


                    if (toolStripComboBox3.Text != "")
                    {
                        MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
                        DialogResult result = MessageBox.Show("SE CARGARAN TODOS LOS CLIENTES DE LA RUTA DE VENTA : " + toolStripComboBox1.Text + " \n DIA : " + toolStripComboBox3.Text + " \n SEMANA: " + toolStripComboBox2.Text + "  \n CONTINUAR CON LA CARGA?", "CARGAR RUTA", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        if (result == DialogResult.Yes)
                        {

                            for (int i = 0; i < dataGridView2.RowCount; i++)
                            {
                                int idxt = i;
                                agregar_clientedgv(idxt, 2);

                            }


                        }

                    }


                }


            }
            else
            {
                MessageBox.Show("Seleccione un Vendedor");
                toolStripComboBox1.Focus();

            }



        }



        public void combo(DataTable dts)
        {
            //comboBox1.Items.Clear();
            comboBox2.Items.Clear();




            var result = from row in dts.AsEnumerable()
                         group row by row.Field<string>("dia") into grp
                         select new
                         {
                             ruta = grp.Key,

                         };
            foreach (var t in result)
            {
                if (t.ruta == null || t.ruta == "")
                {

                }
                else
                {
                    comboBox2.Items.Add(t.ruta);

                }

            }

            if (comboBox2.Items.Contains("CUSTOM"))
            {

            }
            else
            {
                comboBox2.Items.Add("CUSTOM");
            }


        }

        public void agregar_clientedgv(int idx, int tipoing)
        {
            string DIA = "";

            string RUTA = Convert.ToString(dataGridView2.Rows[idx].Cells[0].Value);
            string CODIGO = Convert.ToString(dataGridView2.Rows[idx].Cells[1].Value);
            string ORDEN = Convert.ToString(dataGridView2.Rows[idx].Cells[2].Value);
            // string DIA = Convert.ToString(dataGridView2.Rows[idx].Cells[3].Value);
            if (tipoing == 1)
            {
                DIA = comboBox2.Text;
            }
            else if (tipoing == 2)
            {
                DIA = Convert.ToString(dataGridView2.Rows[idx].Cells[3].Value);
            }

            string NOMBRE = Convert.ToString(dataGridView2.Rows[idx].Cells[4].Value);
            string ALIAS = Convert.ToString(dataGridView2.Rows[idx].Cells[5].Value);
            string DIRECCION = Convert.ToString(dataGridView2.Rows[idx].Cells[6].Value);
            string TELEFONO = Convert.ToString(dataGridView2.Rows[idx].Cells[7].Value);
            string CELULAR = Convert.ToString(dataGridView2.Rows[idx].Cells[8].Value);
            string ENTREGA = Convert.ToString(dataGridView2.Rows[idx].Cells[9].Value);
            string VENDEDOR = Convert.ToString(dataGridView2.Rows[idx].Cells[10].Value);
            string COBRADOR = Convert.ToString(dataGridView2.Rows[idx].Cells[11].Value);
            string DOCUMENTO = Convert.ToString(dataGridView2.Rows[idx].Cells[12].Value);
            string DUI = Convert.ToString(dataGridView2.Rows[idx].Cells[13].Value);
            string NIT = Convert.ToString(dataGridView2.Rows[idx].Cells[14].Value);
            string REGISTRO = Convert.ToString(dataGridView2.Rows[idx].Cells[15].Value);
            string RUBRO = Convert.ToString(dataGridView2.Rows[idx].Cells[16].Value);
            string CONDICION_PAGO = Convert.ToString(dataGridView2.Rows[idx].Cells[17].Value);
            string LIMITE_CREDITO = Convert.ToString(dataGridView2.Rows[idx].Cells[18].Value);
            string LATITUD = Convert.ToString(dataGridView2.Rows[idx].Cells[20].Value);
            string LONGITUD = Convert.ToString(dataGridView2.Rows[idx].Cells[21].Value);
            string usuariort = comboBox1.Text;
            int operador = 2;

            if (LATITUD == "" || LONGITUD == "")
            {
                LATITUD = "0";
                LONGITUD = "0";
            }
            // ID = ID + 1;


            if (Exite_cliente((CODCLIE), comboBox1.Text))
            {
                MessageBox.Show("CLIENTE YA ESTA ASIGNADO EN OTRA RUTA");

            }
            else
            {


                con.conectar("WEB");



                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con.mysqlconec;
                //cmd.CommandText = "INSERT INTO dismodb.dmRutero (ruta,codcli,dia,orden,updateby,fechaupdate,equipo) VALUES (@ruta,@codcli,@dia,@orden,@updateby,@fechaupdate,@equipo)";
                //cmd.CommandText = "CALL UPDATE_CLIENTESYRUTERO (@porden, @pdia, @pcliente,@pnombre,@palias,@pdireccion, @ptel , @pcel , @pruta ,@pvendedor,@pentrega , @pcobrador ,@ptipodoc,@pdui,@pnit , @pregistro,@pgiro,@pcondicion,@plimite,@platitud, @plongitud,@pusuario , @pupdateby , @operador);";
                cmd.CommandText = "SET SQL_SAFE_UPDATES=0; CALL UPDATE_CLIENTESYRUTERO('" + ORDEN + "','" + DIA + "','" + CODIGO + "','" + NOMBRE + "','" + ALIAS + "','" + DIRECCION + "','" + TELEFONO + "','" + CELULAR + "','" + RUTA + "','" + VENDEDOR + "','" + ENTREGA + "','" + COBRADOR + "','" + DOCUMENTO + "','" + DUI + "','" + NIT + "','" + REGISTRO + "','" + RUBRO + "','" + CONDICION_PAGO + "','" + LIMITE_CREDITO + "','" + LATITUD + "','" + LONGITUD + "','" + usuariort + "','" + Login.usuario.ToUpper() + "','" + operador + "')";
                cmd.ExecuteNonQuery();
                con.Desconectar("WEB");

            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                // clie_ruta.Clear();
                // dataGridView1.DataSource = clie_ruta;
                Cargaclientesruta();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                //  clie_ruta.Clear();
                //dataGridView1.DataSource = clie_ruta;
                Cargaclientesruta();
            }
        }

        private void backgroundRutero_DoWork(object sender, DoWorkEventArgs e)
        {
            backgroundRutero.ReportProgress(0, "Obteniendo Datos.");


            clie_ruta.Clear();
            string comdrt = query;
            con.conectar("WEB");
            comando = new MySqlCommand(comdrt, con.mysqlconec);
            comando.CommandTimeout = 0;
            MySqlDataAdapter da = new MySqlDataAdapter(comando);
            
            
            da.Fill(clie_ruta);


            con.Desconectar("WEB");

        
    
        }

        private void backgroundRutero_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            dtfill(clie_ruta);
            //dataGridView1.DataSource = clie_ruta;
            //dataGridView1.Refresh();
            // wt.Close();
            groupBox5.Hide();
           
        }
        


       
        

        public void cancelprocess()
        {
            backgroundRutero.CancelAsync();
            
           
        }

        public static MySqlCommand comando
        {
            get { return cmruta; }
            set { cmruta = value; }
        }

        private void backgroundRutero_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label9.Text = e.UserState.ToString();
        }

        private void dtfill(DataTable dt)
        {
            dataGridView1.Refresh();
            dataGridView1.DataSource = dt;
            

        }
        private void dgclean()
        {

            //clie_ruta.Clear();
            dataGridView1.DataSource = null;
           dataGridView1.Refresh();
            
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
                toolStripButton5.Enabled = true;
            }
        }


        private void copyall()
        {
            int cellfin;
            cellfin = dataGridView1.ColumnCount;

            dataGridView1.SelectAll();
            DataObject dtobj = dataGridView1.GetClipboardContent();
            if (dtobj != null)
            {
                Clipboard.SetDataObject(dtobj);
            }

        }

        private void sendexcel(DataGridView drg)
        {

            int cellfin;
            cellfin = dataGridView1.ColumnCount;
            copyall();

            Microsoft.Office.Interop.Excel.Application excell;
            Microsoft.Office.Interop.Excel.Workbook workbook;
            Microsoft.Office.Interop.Excel.Worksheet Sheet;
            object miobj = System.Reflection.Missing.Value;
            excell = new Excel.Application();
            excell.Visible = true;


            int incre;

            int Columnas, col;

            col = drg.Columns.Count / 26;

            string Letracol = "ABCDEFEHIJKLMNOPQRSTUVWXYZ";
            string Complementocol;
            //Determinando la letra que se usara despues de la columna 26
            if (col > 0)
            {
                Columnas = drg.Columns.Count - (26 * col);
                Complementocol = Letracol.ToString().Substring(col - 1, 1);
            }
            else
            {
                Columnas = drg.Columns.Count;
                Complementocol = "";
            }

            string ColumnaFinal;

            incre = Encoding.ASCII.GetBytes("A")[0];

            ColumnaFinal = Complementocol.ToString() + Convert.ToChar(incre + Columnas - 1).ToString();


            workbook = excell.Workbooks.Add(miobj);
            Sheet = workbook.Worksheets.get_Item(1);

            Excel.Range rg = Sheet.Cells[5, 1];
            Excel.Range Enc;
            Excel.Range det;
            Excel.Range RN;
            Excel.Range Report;
            Excel.Range Reportxt;
            rg.Select();

            // obtener colummnas de encabezado






            for (int c = 0; c < drg.Columns.Count; c++)
            {

                Sheet.Cells[4, c + 1] = String.Format("{0}", cliefull.Columns[c].Caption);
            }


            Sheet.PasteSpecial(rg, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);

            try
            {
                // nombre de la empresa
                RN = Sheet.get_Range("A1", ColumnaFinal + "1");
                RN.Font.Name = "Times New Roman";
                //rango.Font.Color = Color.Blue;
                RN.Font.Size = 14;

                Sheet.Cells[1, 1] = "DISTRIBUIDORA MORAZAN SA DE CV";
                RN.Merge();
                RN.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;


                //Nombre del Reporte 
                Report = Sheet.get_Range("A2", ColumnaFinal + "2");
                Report.Font.Name = "Times New Roman";
                Report.Font.Size = 12;


                Sheet.Cells[2, 1] = "IMPUESTO DE DISTRIBUCION" + " EMISION " + DateTime.Now.ToString();

                Report.Select();
                Report.Merge();
                Report.Font.Bold = true;
                Report.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;




                Reportxt = Sheet.get_Range("A3", ColumnaFinal + "3");
                Reportxt.Font.Name = "Times New Roman";
                Reportxt.Font.Size = 12;



                Sheet.Cells[3, 1] = "CLIENTES ASIGNADOS ";

                Reportxt.Select();
                Reportxt.Merge();
                Reportxt.Font.Bold = true;
                Reportxt.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;




                //ENCABEZDO DE COLUMNAS
                Enc = Sheet.get_Range("A4", ColumnaFinal + 4);
                Enc.Font.Name = "Times New Roman";
                Enc.Font.Size = 9;
                Enc.Borders.LineStyle = Excel.XlLineStyle.xlDouble;
                Enc.Font.Bold = true;

                //DETALLE 
                //ENCABEZDO DE COLUMNAS


            }
            catch (SystemException exec)
            {
                MessageBoxButtons bt1 = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(exec.ToString(), "!!!!ERROR!!!!", bt1, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);


            }

        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            copyall();
            sendexcel(dataGridView1);
        }
    }

}
