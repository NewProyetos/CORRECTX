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
    public partial class Rutas : Form
    {
        public Rutas()
        {
            InitializeComponent();
        }


        //conexion conex = new conexion();
        //Conexion2 coned = new Conexion2();
        conexionXML con = new conexionXML();
        public static Int32 repot;
        public String dia;
        public String semana;
        public String semana_act;
        public int secuencia;
        public static String cliente;
        public static String cliente_nom;
        public String clientes;
        public String Hoy;
        public String Agente;
        public String Agente2;
        public String Vendedor;
        public String HoyH;
        public String Estatus = "N";
        public String nombreC;
        public String Ruta;
        public int idx;
        public int exist;
        public string empresa = Login.empresa;
        DataTable dt = new DataTable();
        private ContextMenu menugrid = new ContextMenu();
        public MenuItem BAJA = new MenuItem("BAJA POR:");
        public static string vendor;
        DataTable busqueda = new DataTable();
        DataTable dtfull = new DataTable();
        DataTable dtclienteDM = new DataTable();
        DataTable Disponibles = new DataTable();
        String semanadispo;
        Int32 detalledirec;
        private void Rutas_Load(object sender, EventArgs e)
        {
            radioButton2.Checked = true;

            this.Text = "RUTAS   (" + Login.empresa + " ) ";
            Hoy = DateTime.Now.ToString("yyyy/MM/dd");
            HoyH = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");



            button4.Hide();
            toolStripButton4.Enabled = false;

            toolStripTextBox3.Enabled = false;
            toolStripTextBox4.Enabled = false;
            toolStripButton3.Enabled = false;
            checkBox1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            numericUpDown1.Enabled = false;


            BAJA.MenuItems.Add(new MenuItem("INACTIVIDAD", new System.EventHandler(this.BAJA_IN)));
            BAJA.MenuItems.Add(new MenuItem("MORA", new System.EventHandler(this.BAJA_MORA)));

            MenuItem QUITAR = new MenuItem("QUITAR RUTA", new System.EventHandler(this.QUITAR));

            MenuItem MOVER = new MenuItem("MOVER DE RUTA", new System.EventHandler(this.MOVER));

            MenuItem CLIENTE= new MenuItem("CLIENTE");

            CLIENTE.MenuItems.Add(new MenuItem("AGREGAR INFORMACION", new System.EventHandler(this.CLIENTE_UPDATE)));

            menugrid.MenuItems.AddRange(new MenuItem[] { BAJA, QUITAR,CLIENTE,MOVER});

            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            //dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ReadOnly = false;

            dataGridView2.Enabled = true;
            dataGridView2.RowHeadersVisible = false;
            //dataGridView2.AutoResizeColumns();
            //dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView2.ReadOnly = true;
            con.conectar("EX");

            SqlCommand cm1 = new SqlCommand("SELECT A.[RUTA] as 'RUTA' FROM [EXACTUS].[ERPADMIN].[RUTA_ASIGNADA_RT] A  where COMPANIA = '" + Login.empresa + "' order by A.[RUTA] ", con.conex);
            SqlDataReader dr1 = cm1.ExecuteReader();
            while (dr1.Read())
            {
                comboBox1.Items.Add(dr1["RUTA"]);

            }
            dr1.Close();




            con.Desconectar("EX");



            con.conectar("DM");
            SqlCommand cm4 = new SqlCommand("SELECT SEMANA FROM dm.dbo.SEMANAS where '" + Hoy + "'>=FEINI AND '" + Hoy + "'<=FEFIN", con.condm);
            SqlDataReader dr4 = cm4.ExecuteReader();
            while (dr4.Read())
            {
                semana_act = Convert.ToString(dr4["SEMANA"]);
                this.label7.Text = "SEMANA  " + semana_act;
            }
            dr4.Close();
            con.Desconectar("DM");
            Full_Rutas(null, null);

            this.comboBox2.Text = semana_act;
            this.comboBox3.Text = semana_act;
            groupBox5.Enabled = false;






        }

        private void CLIENTE_UPDATE(object sender, EventArgs e)
        {
            //throw new NotImplementedException();

            
                Sinconizacion_EXactus.CORECTX_APP.VENTAS.RUTERO.Informacion_Cliente cl = new CORECTX_APP.VENTAS.RUTERO.Informacion_Cliente();
                
                DialogResult res = cl.ShowDialog();

                if (res == DialogResult.OK)
                {


                    button4_Click(null, null);

                    Full_Rutas(null, null);

                }
            
        }

        private void Full_Rutas(object sender, EventArgs e)
        {
            dtfull.Clear();
            dataGridView2.Refresh();
            con.conectar("EX");
            SqlCommand cmd2 = new SqlCommand();
            if (empresa == "DISMO")
            {
                cmd2 = new SqlCommand("SELECT CASE WHEN B.RUTA IS NULL THEN 'N/A' ELSE +B.RUTA END as 'RUTA' ,CASE WHEN A.ES_CORPORACION ='N' THEN 'SI' ELSE 'NO' END as MIEMBRO_CORP ,A.[CLIENTE],A.[NOMBRE],A.[ALIAS], CASE WHEN B.SEMANA IS NULL THEN 'N/A' ELSE +B.SEMANA END as 'SEMANA',case B.DIA  WHEN '0' THEN 'LUNES'  WHEN '1' THEN 'MARTES'  WHEN '2' THEN 'MIERCOLES'  WHEN '3' THEN 'JUEVES'  WHEN '4' THEN 'VIERNES'  WHEN '5' THEN 'SABADO'  WHEN '6' THEN 'ESPECIAL' ELSE 'N/A' END  as 'DIA' , case A.ACTIVO WHEN 'N' THEN 'BAJA' WHEN 'S' THEN 'ACTIVO'   END AS 'ESTATUS' ,SALDO FROM [EXACTUS].[dismo].[CLIENTE] A  left join (SELECT CLIENTE,DIA,SEMANA,RUTA FROM [DM].[dbo].[RUTERO] ) B  on A.CLIENTE = B.CLIENTE WHERE A.MOROSO <> 'S' and a.CLIENTE not like 'G%'", con.condm);
            }
            else
            {
                cmd2 = new SqlCommand("SELECT CASE WHEN B.RUTA IS NULL THEN 'N/A' ELSE +B.RUTA END as 'RUTA' ,CASE WHEN A.ES_CORPORACION ='N' THEN 'SI' ELSE 'NO' END as MIEMBRO_CORP ,A.[CLIENTE],A.[NOMBRE],A.[ALIAS], CASE WHEN B.SEMANA IS NULL THEN 'N/A' ELSE +B.SEMANA END as 'SEMANA',case B.DIA  WHEN '0' THEN 'LUNES'  WHEN '1' THEN 'MARTES'  WHEN '2' THEN 'MIERCOLES'  WHEN '3' THEN 'JUEVES'  WHEN '4' THEN 'VIERNES'  WHEN '5' THEN 'SABADO'  WHEN '6' THEN 'ESPECIAL' ELSE 'N/A' END  as 'DIA' , case A.ACTIVO WHEN 'N' THEN 'BAJA' WHEN 'S' THEN 'ACTIVO'   END AS 'ESTATUS' ,SALDO FROM [EXACTUS].["+empresa+"].[CLIENTE] A  left join (SELECT CLIENTE,DIA,SEMANA,RUTA FROM [DM].[dbo].[RUTERO] ) B  on A.CLIENTE = B.CLIENTE WHERE A.MOROSO <> 'S' and a.CLIENTE  like 'G%'", con.condm);
            }

            SqlDataAdapter da1 = new SqlDataAdapter(cmd2);
            da1.Fill(dtfull);
            con.Desconectar("EX");

        }

        private bool Update_Rutero(object sender, EventArgs e)
        {

            MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("DESEA GUARDAR LOS CAMBIOS REALIZADOS EN LA RUTA: " + this.comboBox1.Text + "", "ACTUALIZACION DE RUTAS", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {

                return true;

            }
            else

            {
                return false;
                this.toolStripButton4.Enabled = false;
            }

        }

        private void carga(object sender, EventArgs e)
        {


            toolStripButton4.Enabled = false;
            dt.Clear();
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoResizeColumns();

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Refresh();

            switch (this.toolStripComboBox1.Text)
            {
                case "LUNES":
                    dia = "0";
                    break;
                case "MARTES":
                    dia = "1";
                    break;
                case "MIERCOLES":
                    dia = "2";
                    break;
                case "JUEVES":
                    dia = "3";
                    break;
                case "VIERNES":
                    dia = "4";
                    break;
                case "SABADO":
                    dia = "5";
                    break;
                case "ESPECIAL":
                    dia = "6";
                    break;
                case "TODOS":
                    dia = null;
                    break;

                default:
                    dia = null;
                    break;


            }

            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("[CORRECT].[RUTERO]", con.condm);
            cmd.CommandType = CommandType.StoredProcedure;




            if (this.comboBox2.Text == "AB" || this.comboBox2.Text == "")
            {
                cmd.Parameters.AddWithValue("@Ruta", this.comboBox1.Text);
                cmd.Parameters.AddWithValue("@Semana", null);
                cmd.Parameters.AddWithValue("@Dia", null);
                cmd.Parameters.AddWithValue("@empresa", empresa);
            }
            else if (this.toolStripComboBox1.Text == "TODOS" || this.toolStripComboBox1.Text == "")
            {
                cmd.Parameters.AddWithValue("@Ruta", this.comboBox1.Text);
                cmd.Parameters.AddWithValue("@Semana", semana);
                cmd.Parameters.AddWithValue("@Dia", null);
                cmd.Parameters.AddWithValue("@empresa", empresa);
            }

            else
            {

                cmd.Parameters.AddWithValue("@Ruta", this.comboBox1.Text);
                cmd.Parameters.AddWithValue("@Semana", semana);
                cmd.Parameters.AddWithValue("@Dia", null);
                cmd.Parameters.AddWithValue("@empresa", empresa);

            }

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(dt);


            con.Desconectar("DM");


            dataGridView1.DataSource = dt;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }


            dataGridView1.Refresh();

            int numRows = dataGridView1.Rows.Count;

            toolStripComboBox1_SelectedIndexChanged(null, null);





        }

        private void button1_Click(object sender, EventArgs e)
        {

            button2.Enabled = false;
            button3.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            numericUpDown1.Enabled = false;
            cliente = "0";

            if (toolStripButton4.Enabled == true)
            {
                if (Update_Rutero(null, null))
                {

                    toolStripButton4_Click(null, null);

                    carga(null, null);
                }
                else
                {
                    carga(null, null);


                }
            }
            else
            {
                carga(null, null);

            }

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            cliente = "0";
            semana = this.comboBox2.Text;
            button1_Click(null, null);
            toolStripComboBox1_SelectedIndexChanged(null, null);
            toolStripButton4.Enabled = false;
            this.comboBox3.Text = comboBox2.Text;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            toolStripButton4.Enabled = true;
            dataGridView1.Refresh();
            DataRow newRow = busqueda.NewRow();
            DataRow dr = busqueda.Rows[this.dataGridView1.CurrentRow.Index];

            idx = dataGridView1.CurrentRow.Index;
            if (idx > 0 && idx <= dataGridView1.RowCount - 1)
            {


                newRow.ItemArray = dr.ItemArray;

                busqueda.Rows.RemoveAt(idx);

                busqueda.Rows.InsertAt(newRow, idx - 1);

                dataGridView1.ClearSelection();


                dataGridView1.CurrentCell = dataGridView1.Rows[idx - 1].Cells[0];
                dataGridView1.Rows[idx - 1].Selected = true;

            }


            button4_Click(null, null);
            dataGridView1.Refresh();
            toolStripButton4.Enabled = true;

        }




        private void button3_Click(object sender, EventArgs e)
        {

            toolStripButton4.Enabled = true;
            dataGridView1.Refresh();
            DataRow newRow = busqueda.NewRow();
            DataRow dr = busqueda.Rows[this.dataGridView1.CurrentRow.Index];

            idx = dataGridView1.CurrentRow.Index;
            if (idx >= 0 && idx < dataGridView1.RowCount - 1)
            {


                newRow.ItemArray = dr.ItemArray;

                busqueda.Rows.RemoveAt(idx);

                busqueda.Rows.InsertAt(newRow, idx + 1);

                dataGridView1.ClearSelection();

                dataGridView1.CurrentCell = dataGridView1.Rows[idx + 1].Cells[0];

                dataGridView1.Rows[idx + 1].Selected = true;
            }





            button4_Click(null, null);
            dataGridView1.Refresh();
            toolStripButton4.Enabled = true;
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
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
        private void BAJA_IN(Object sender, System.EventArgs e)

        {
            Decimal SaldoC = 0;
            DataRow[] saldo = dtfull.Select("CLIENTE = '" + cliente + "' AND SALDO >= 0");
            foreach (DataRow rows in saldo)
            {
                SaldoC = Convert.ToDecimal(rows["SALDO"]);

            }
            if (SaldoC > 0 && Main_Menu.Departamento != "INFORMATICA")
            {

                MessageBox.Show("CLIENTE CON SALDO  $" + SaldoC + "   NO SE PUEDE DAR DE BAJA");
            }
            else
            {

                MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show("ESTA SEGURO QUE DESEA DAR DE BAJA A ESTE CLIENTE: " + cliente + " POR INACTIVIDAD", "BAJA DE CLIENTE", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {


                    idx = dataGridView1.CurrentRow.Index;
                    if (idx >= 0)
                    {



                        con.conectar("DM");
                        SqlCommand cmd = new SqlCommand("DELETE FROM  [DM].[dbo].[RUTERO]  WHERE  CLIENTE ='" + cliente + "'", con.condm);


                        cmd.ExecuteNonQuery();

                        con.Desconectar("DM");

                        //----------------------------------------------
                        con.conectar("EX");
                        SqlCommand cmd2 = new SqlCommand("UPDATE [EXACTUS].[dismo].[CLIENTE] SET ACTIVO = 'N',USUARIO_ULT_MOD = '" + Login.usuario.ToUpper() + "',FCH_HORA_ULT_MOD = '" + HoyH + "' where CLIENTE = '" + cliente + "'", con.conex);


                        cmd2.ExecuteNonQuery();

                        con.Desconectar("EX");

                        //---------------------------------------------
                        busqueda.Rows.RemoveAt(idx);


                        toolStripButton4.Enabled = true;
                    }

                    button4_Click(null, null);
                    label3.Text = Convert.ToString(busqueda.Rows.Count);
                    Full_Rutas(null, null);

                }

            }

        }

        private void BAJA_MORA(Object sender, System.EventArgs e)
        {


        }

        private void MOVER(Object sender, System.EventArgs e)
        {

            CORECTX_APP.VENTAS.RUTERO.Cambio mov = new CORECTX_APP.VENTAS.RUTERO.Cambio(cliente);
            DialogResult res = mov.ShowDialog();

            if (res == DialogResult.OK)
            {


                carga(null, null);

            }

        }


        private void QUITAR(Object sender, System.EventArgs e)
        {



            Decimal SaldoC = 0;
            DataRow[] saldo = dtfull.Select("CLIENTE = '" + cliente + "' AND SALDO >= 0");
            foreach (DataRow rows in saldo)
            {
                SaldoC = Convert.ToDecimal(rows["SALDO"]);

            }
            if (exist == 1)
            {

                if (SaldoC > 0 && Main_Menu.Departamento != "INFORMATICA")
                {

                    MessageBox.Show("CLIENTE CON SALDO  $" + SaldoC + "   NO PUEDE SER QUITADO DE LA RUTA");
                }
                else
                {
                    Quitaruta(null, null);
                }

            }
            else if (exist > 1)
            {
                Quitaruta(null, null);

            }


        }

        private void Quitaruta(object sender, EventArgs e)
        {

            if (cliente != "0")
            {
                MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show("ESTA SEGURO QUE DESEA QUITAR ESTE CLIENTE: " + cliente + "", "QUITAR DE RUTA", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {
                    try
                    {

                        idx = dataGridView1.CurrentRow.Index;

                        if (idx >= 0)
                        {



                            con.conectar("EX");
                            SqlCommand cmd = new SqlCommand("DELETE FROM  [DM].[dbo].[RUTERO]  WHERE RUTA = '" + this.comboBox1.Text + "' and CLIENTE ='" + cliente + "' and SEMANA = '" + semana + "' and DIA = '" + dia + "' ", con.conex);


                            cmd.ExecuteNonQuery();

                            con.Desconectar("EX");

                            busqueda.Rows.RemoveAt(idx);


                            toolStripButton4.Enabled = true;
                        }

                        button2.Enabled = false;
                        button3.Enabled = false;
                        button5.Enabled = false;
                        button6.Enabled = false;
                        numericUpDown1.Enabled = false;
                        cliente = "0";

                        button4_Click(null, null);
                        statusStrip1.BackColor = Color.OrangeRed;
                        toolStripStatusLabel2.Text = "Cambios realizados en Ruta :" + this.comboBox1.Text + "   Cliente:  " + cliente + "  ELIMINADO   Por Usuario:  " + Login.usuario.ToUpper() + "";
                        label3.Text = Convert.ToString(busqueda.Rows.Count);
                        Full_Rutas(null, null);
                    }
                    catch
                    {
                        statusStrip1.BackColor = Color.Red;
                        toolStripStatusLabel2.Text = " !!ERROR!!   en Ruta :" + this.comboBox1.Text + "   Cliente:  " + cliente + " NO PUDO SER ELIMINADO  ";
                    }
                }


            }
            else
            {

                MessageBox.Show("NO A SELECCIONADO UN CLIENTE");
            }



        }

        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < busqueda.Rows.Count; i++)
            {

                busqueda.Rows[i]["ORDEN"] = i + 1;
            }
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {

            busqueda.DefaultView.RowFilter = "NOMBRE like '" + this.toolStripTextBox1.Text + "%'";
            dataGridView1.DataSource = busqueda;



        }

        private void toolStripTextBox2_TextChanged(object sender, EventArgs e)
        {

            busqueda.DefaultView.RowFilter = string.Format("Convert(CLIENTE,'System.String') like '%{0}%'", this.toolStripTextBox2.Text);
            dataGridView1.DataSource = busqueda;
        }

        private void dataGridView1_DataBindingComplete_1(object sender, DataGridViewBindingCompleteEventArgs e)
        {



            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[3].ReadOnly = true;
            //dataGridView1.Columns[4].ReadOnly = true;

            groupBox5.Enabled = true;

        }



        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {

            if (object.ReferenceEquals(dataGridView1.CurrentCell.ValueType, typeof(System.Int32)))
            {
                TextBox txt = e.Control as TextBox;

                txt.KeyPress += OnlyNumbers_KeyPress;
            }
        }
        private void OnlyNumbers_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void Cambio_dia(object sender, EventArgs e)
        {

            DataView dv = dt.DefaultView;

            //if (this.toolStripComboBox1.Text == "ESPECIAL")
            //{
            //    dv.RowFilter = "DIA like 'DOMINGO%'";

            //}
            //else {
            if (dt.Columns.Contains("DIA"))
            {
                dv.RowFilter = "DIA like '" + this.toolStripComboBox1.Text + "%'";
                //}




                //busqueda.DefaultView.RowFilter = "DIA like '" + this.toolStripComboBox1.Text + "%'";
                busqueda = dv.ToTable();
                label3.Text = Convert.ToString(busqueda.Rows.Count);

                dataGridView1.DataSource = busqueda;
                toolStripButton4.Enabled = false;
            }
            else
            {

            }

        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cliente = "0";


            switch (this.toolStripComboBox1.Text)
            {
                case "LUNES":
                    dia = "0";
                    break;
                case "MARTES":
                    dia = "1";
                    break;
                case "MIERCOLES":
                    dia = "2";
                    break;
                case "JUEVES":
                    dia = "3";
                    break;
                case "VIERNES":
                    dia = "4";
                    break;
                case "SABADO":
                    dia = "5";
                    break;
                case "ESPECIAL":
                    dia = "6";
                    break;
                case "TODOS":
                    dia = null;
                    break;

                default:
                    dia = null;
                    break;


            }




            if (toolStripButton4.Enabled == true)
            {
                if (Update_Rutero(null, null))
                {

                    toolStripButton4_Click(null, null);
                    Cambio_dia(null, null);
                }
                else
                {

                    Cambio_dia(null, null);


                }
            }
            else

            {
                Cambio_dia(null, null);

            }
        }

        private void toolStripTextBox3_TextChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {

                dtfull.DefaultView.RowFilter = string.Format("RUTA = 'N/A' AND Convert(CLIENTE,'System.String') like '%{0}%'", this.toolStripTextBox3.Text);
                dataGridView2.DataSource = dtfull;


            }

            else
            {
                dtfull.DefaultView.RowFilter = string.Format("Convert(CLIENTE,'System.String') like '%{0}%'", this.toolStripTextBox3.Text);
                dataGridView2.DataSource = dtfull;
            }
        }

        private void toolStripTextBox4_TextChanged(object sender, EventArgs e)
        {

            if (checkBox1.Checked)
            {
                if (radioButton2.Checked)
                {
                    dtfull.DefaultView.RowFilter = "ALIAS like '" + this.toolStripTextBox4.Text + "%' AND RUTA = 'N/A'";
                    dataGridView2.DataSource = dtfull;
                }
                else
                {

                    dtfull.DefaultView.RowFilter = "NOMBRE like '" + this.toolStripTextBox4.Text + "%' AND RUTA = 'N/A'";
                    dataGridView2.DataSource = dtfull;
                }

            }

            else
            {
                if (radioButton2.Checked)
                {
                    dtfull.DefaultView.RowFilter = "ALIAS like '" + this.toolStripTextBox4.Text + "%'";
                    dataGridView2.DataSource = dtfull;
                }
                else
                {
                    dtfull.DefaultView.RowFilter = "NOMBRE like '" + this.toolStripTextBox4.Text + "%'";
                    dataGridView2.DataSource = dtfull;
                }
            }




        }

        private void button5_Click(object sender, EventArgs e)
        {
            toolStripButton4.Enabled = true;
            dataGridView1.Refresh();
            DataRow newRow = busqueda.NewRow();
            DataRow dr = busqueda.Rows[this.dataGridView1.CurrentRow.Index];

            idx = dataGridView1.CurrentRow.Index;
            if (idx > 0 && idx <= dataGridView1.RowCount - 1)
            {


                newRow.ItemArray = dr.ItemArray;

                busqueda.Rows.RemoveAt(idx);

                busqueda.Rows.InsertAt(newRow, 0);

                dataGridView1.ClearSelection();


                dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[0];
                dataGridView1.Rows[0].Selected = true;

            }


            button4_Click(null, null);
            dataGridView1.Refresh();
            toolStripButton4.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            toolStripButton4.Enabled = true;
            dataGridView1.Refresh();
            DataRow newRow = busqueda.NewRow();
            DataRow dr = busqueda.Rows[this.dataGridView1.CurrentRow.Index];
            int ultima_rows = dataGridView1.Rows.Count;
            idx = dataGridView1.CurrentRow.Index;
            if (idx >= 0 && idx <= dataGridView1.RowCount - 1)
            {


                newRow.ItemArray = dr.ItemArray;

                busqueda.Rows.RemoveAt(idx);

                busqueda.Rows.InsertAt(newRow, ultima_rows);

                dataGridView1.ClearSelection();


                dataGridView1.CurrentCell = dataGridView1.Rows[ultima_rows - 1].Cells[0];
                dataGridView1.Rows[ultima_rows - 1].Selected = true;

            }


            button4_Click(null, null);
            dataGridView1.Refresh();
            toolStripButton4.Enabled = true;
        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            //  toolStripButton4.Enabled = true;
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            // toolStripButton4.Enabled = true;
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            statusStrip1.BackColor = Color.Empty;
            toolStripStatusLabel2.Text = "Ready";

            idx = dataGridView1.CurrentRow.Index;
            cliente = Convert.ToString(dataGridView1.Rows[idx].Cells[1].Value);
          cliente_nom = Convert.ToString(dataGridView1.Rows[idx].Cells[2].Value);

            int maximo = dataGridView1.Rows.Count;

            if (dataGridView1.Rows.Count > 0)
            {
                button2.Enabled = true;
                button3.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                numericUpDown1.Enabled = true;
                numericUpDown1.Maximum = maximo;
                numericUpDown1.Minimum = 1;
            }
            else
            {
                button2.Enabled = false;
                button3.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;

            }

            con.conectar("DM");
            SqlCommand cm4 = new SqlCommand("SELECT COUNT(*) as 'exist' FROM [DM].[dbo].[RUTERO]  where CLIENTE = '" + cliente + "'", con.condm);
            SqlDataReader dr4 = cm4.ExecuteReader();
            while (dr4.Read())
            {
                exist = Convert.ToInt32(dr4["exist"]);

            }
            dr4.Close();
            con.Desconectar("DM");
        }



        private void numericUpDown1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                toolStripButton4.Enabled = true;
                dataGridView1.Refresh();
                DataRow newRow = busqueda.NewRow();
                DataRow dr = busqueda.Rows[this.dataGridView1.CurrentRow.Index];
                int valor = Convert.ToInt32(numericUpDown1.Value);
                idx = dataGridView1.CurrentRow.Index;
                if (idx >= 0 && idx <= dataGridView1.RowCount - 1)
                {


                    newRow.ItemArray = dr.ItemArray;

                    busqueda.Rows.RemoveAt(idx);

                    busqueda.Rows.InsertAt(newRow, valor - 1);

                    dataGridView1.ClearSelection();


                    dataGridView1.CurrentCell = dataGridView1.Rows[valor - 1].Cells[0];
                    dataGridView1.Rows[valor - 1].Selected = true;

                }

                button4_Click(null, null);
                dataGridView1.Refresh();
                numericUpDown1.Value = 1;
                toolStripButton4.Enabled = true;

            }
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

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Ruta = this.comboBox1.Text;
            idx = dataGridView2.CurrentRow.Index;

            clientes = Convert.ToString(dataGridView2.Rows[idx].Cells[2].Value);
            string nombre_cli = Convert.ToString(dataGridView2.Rows[idx].Cells[3].Value);


            if (nombre_cli.Length > 79)
            {
                nombreC = nombre_cli.Substring(0, 79);
            }
            else
            {
                nombreC = nombre_cli;
            }





            // string estatus = "N";
            DataRow[] est = dtfull.Select("CLIENTE = '" + clientes + "' AND ESTATUS = 'BAJA'");
            foreach (DataRow rows in est)
            {
                Estatus = Convert.ToString(rows["ESTATUS"]);
            }



            if (Exists(clientes, Ruta, semana, dia))
            {

                MessageBox.Show("CLIENTE YA EXSISTE EN ESTE DIA");
            }


            else

            {
                Carga_cliente_FR(Ruta, dia, semana, nombreC, clientes,1);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            toolStripTextBox3.Enabled = true;
            toolStripTextBox4.Enabled = true;
            checkBox1.Enabled = true;
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.toolStripComboBox1.Text != "" && this.comboBox1.Text != "")
            {
                toolStripButton3.Enabled = true;
            }

        }

        private void toolStripTextBox3_Click(object sender, EventArgs e)
        {
            toolStripTextBox4.Text = "";
        }

        private void toolStripTextBox4_Click(object sender, EventArgs e)
        {
            toolStripTextBox3.Text = "";
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            try
            {
                string secuencia_up;
                string cliente_up;

                con.conectar("DM");
                for (int i = 0; i < busqueda.Rows.Count; i++)
                {

                    DataRow row = busqueda.Rows[i];
                    cliente_up = Convert.ToString(row["CLIENTE"]);
                    secuencia_up = Convert.ToString(row["ORDEN"]);


                    SqlCommand cmd = new SqlCommand("UPDATE  [DM].[dbo].[RUTERO]  set ORDEN = '" + secuencia_up + " ', UpdatedBy = '" + Login.usuario.ToUpper() + "' WHERE RUTA = '" + this.comboBox1.Text + "' and CLIENTE ='" + cliente_up + "' and SEMANA = '" + semana + "' ", con.condm);


                    cmd.ExecuteNonQuery();



                }
                con.Desconectar("DM");

                statusStrip1.BackColor = Color.GreenYellow;
                toolStripStatusLabel2.Text = "Cambios realizados en Ruta :" + this.comboBox1.Text + " Semana  " + semana + "  Por Usuario:  " + Login.usuario.ToUpper() + "                !!EXITOSO!!";
                // MessageBox.Show("Cambios Realizados");
                toolStripButton4.Enabled = false;
            }
            catch
            {
                statusStrip1.BackColor = Color.Red;
                //MessageBox.Show("Error al guardar");
                toolStripStatusLabel2.Text = " !!ERROR!!         Cambios a Ruta :" + this.comboBox1.Text + " NO realizado ";

            }
        }

        private void ruteroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            repot = 1;
            Ruteroreporte report = new Ruteroreporte();
            report.ShowDialog();
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

        private bool Exists_clie_DM(string cliente)
        {
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("select COUNT (*) from [EXACTUS].[ERPADMIN].[CLIENTE_DM] where COD_CLT = @cliente ", con.conex);
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

        private void bajasInactivasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            repot = 2;
            Ruteroreporte report = new Ruteroreporte();
            report.ShowDialog();

        }

        private void bajasPorMoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            repot = 3;
            Ruteroreporte report = new Ruteroreporte();
            report.ShowDialog();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            con.conectar("EX");
            SqlCommand cm2 = new SqlCommand("SELECT A.[AGENTE] as 'AGENTE' ,C.[CODIGO] as 'AGENTE2',B.NOMBRE as 'NOMBRE' FROM [EXACTUS].[ERPADMIN].[RUTA_ASIGNADA_RT] A Inner join  [EXACTUS].[ERPADMIN].[AGENTE_RT] B on A.AGENTE = B.AGENTE  LEFT JOIN [EXACTUS].[ERPADMIN].[AGENTE_ASOC_RT] C on A.AGENTE = C.AGENTE where A.RUTA = '" + this.comboBox1.Text + "'", con.conex);
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
                Agente = Convert.ToString(dr2["AGENTE"]);
                Agente2 = Convert.ToString(dr2["AGENTE2"]);
                Vendedor = Convert.ToString(dr2["NOMBRE"]);

                if (Agente != Agente2)
                {
                    Agente = Agente2;

                }

                label9.Text = Agente;
                label11.Text = Vendedor;
                
            }
            dr2.Close();
            con.Desconectar("EX");
            vendor = label9.Text;

            cliente = "0";
            toolStripComboBox1.Text = "LUNES";
            button2.Enabled = false;
            button3.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            numericUpDown1.Enabled = false;
            Ruta = this.comboBox1.Text;
            if (this.comboBox2.Text != "")

            {
                button1_Click(null, null);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (toolStripTextBox3.Text != "")
            {
                toolStripTextBox3_TextChanged(null, null);
            }
            else if (toolStripTextBox4.Text != "")
            {
                toolStripTextBox4_TextChanged(null, null);
            }
            else if (checkBox1.Checked)
            {
                dtfull.DefaultView.RowFilter = string.Format("RUTA = 'N/A'");
                dataGridView2.DataSource = dtfull;

            }
            else
            {
                dtfull.DefaultView.RowFilter = string.Format(" Convert(CLIENTE,'System.String') like '%{0}%'", this.toolStripTextBox3.Text);
                dataGridView2.DataSource = dtfull;
            }
        }


        private void Carga_cliente_FR(string Rutas, string Dias, string Semanas, string Nombreclie, string Cliente_ing,int proceso)
        {
            string fecha = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            int orden = dataGridView1.Rows.Count + 1;
            //try
            //{
            {
                if (Exists_FR_cli_rt_(Cliente_ing))
                {



                    con.conectar("DM");

                    SqlCommand cmd1 = new SqlCommand("[CORRECT].[CREACLIE_FR]", con.condm);
                    cmd1.CommandTimeout = 0;
                    cmd1.CommandType = CommandType.StoredProcedure;

                    cmd1.Parameters.AddWithValue("@TABLA", 1);
                    cmd1.Parameters.AddWithValue("@CODCLI", Cliente_ing);
                    cmd1.Parameters.AddWithValue("@NOMBRE", Nombreclie);
                    cmd1.Parameters.AddWithValue("@empresa", empresa);

                    cmd1.ExecuteNonQuery();

                    con.Desconectar("DM");

                }

                if (Exists_FR_asoc_rt_(Cliente_ing))
                {



                    con.conectar("DM");

                    SqlCommand cmd2 = new SqlCommand("[CORRECT].[CREACLIE_FR]", con.condm);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.CommandTimeout = 0;
                    cmd2.Parameters.AddWithValue("@TABLA", 2);
                    cmd2.Parameters.AddWithValue("@CODCLI", Cliente_ing);
                    cmd2.Parameters.AddWithValue("@NOMBRE", Nombreclie);
                    cmd2.Parameters.AddWithValue("@empresa", empresa);

                    cmd2.ExecuteNonQuery();

                    con.Desconectar("DM");
                }

                if (Exists_clie_DM(Cliente_ing))
                {


                    con.conectar("DM");

                    SqlCommand cmd2 = new SqlCommand("[CORRECT].[CREACLIE_FR]", con.condm);
                    cmd2.CommandType = CommandType.StoredProcedure;

                    cmd2.Parameters.AddWithValue("@TABLA", 3);
                    cmd2.Parameters.AddWithValue("@CODCLI", Cliente_ing);
                    cmd2.Parameters.AddWithValue("@NOMBRE", Nombreclie);
                    cmd2.Parameters.AddWithValue("@empresa", empresa);

                    cmd2.ExecuteNonQuery();

                    con.Desconectar("DM");
                }




                if (Estatus == "BAJA")
                {
                    con.conectar("EX");
                    SqlCommand cmd2 = new SqlCommand("UPDATE [EXACTUS].[" + empresa + "].[CLIENTE] SET ACTIVO = 'S',USUARIO_ULT_MOD = '" + Login.usuario.ToUpper() + "',FCH_HORA_ULT_MOD = '" + HoyH + "' where CLIENTE = '" + Cliente_ing + "'", con.conex);


                    cmd2.ExecuteNonQuery();

                    con.Desconectar("EX");
                }

                con.conectar("DM");

                SqlCommand cmd3 = new SqlCommand("[CORRECT].[RUTERO_INSERT]", con.condm);
                cmd3.CommandTimeout = 0;
                cmd3.CommandType = CommandType.StoredProcedure;

                cmd3.Parameters.AddWithValue("@RUTA", Rutas);
                cmd3.Parameters.AddWithValue("@CLIENTE", Cliente_ing);
                cmd3.Parameters.AddWithValue("@DIA", Dias);
                cmd3.Parameters.AddWithValue("@ORDEN", orden);
                cmd3.Parameters.AddWithValue("@UpdatedBy", Login.usuario.ToUpper());
                cmd3.Parameters.AddWithValue("@SEMANA", Semanas);
                cmd3.Parameters.AddWithValue("@fecha_crea", fecha);
                cmd3.Parameters.AddWithValue("@empresa", empresa);


                cmd3.ExecuteNonQuery();

                con.Desconectar("DM");

                //if(Agente.Contains("G"))
                //{

                //    Agente = Agente.Replace('G', 'V');

                //}


                con.conectar("EX");
                SqlCommand cmd4 = new SqlCommand("UPDATE [EXACTUS].[" + empresa + "].[CLIENTE] SET VENDEDOR='" + Agente + "' WHERE CLIENTE ='" + Cliente_ing + "'", con.conex);


                cmd4.ExecuteNonQuery();

                con.Desconectar("EX");

                button4_Click(null, null);
                label3.Text = Convert.ToString(busqueda.Rows.Count);
                Full_Rutas(null, null);




                //MessageBox.Show("Cliente Ingresado Correctamente");

                button1_Click(null, null);
                statusStrip1.BackColor = Color.GreenYellow;
                toolStripStatusLabel2.Text = "Cambios realizados en Ruta :" + this.comboBox1.Text + "   Cliente:  " + Cliente_ing + "   Ingresado Correctamente  Por Usuario:  " + Login.usuario.ToUpper() + "";

                // toolStripButton4.Enabled = true;


                cliente = "0";
                clientes = "0";



            }
            if (clientes == "0")
            {
                toolStripButton3.Enabled = false;
            }

            if (proceso == 1)
            {
                dataGridView1.CurrentCell = dataGridView1.Rows[orden - 1].Cells[0];
            }
            

        //}
        //catch
        //{
        //    statusStrip1.BackColor = Color.Red;
        //    toolStripStatusLabel2.Text = "!!ERRROR!! en Ruta :" + this.comboBox1.Text + " No se pudo ingresar Cliente  " + clientes + "";
        //}

   
        
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Disponibles.Clear();
            int cansem = comboBox3.Text.Length;
            switch (this.toolStripComboBox1.Text)
            {
                case "LUNES":
                    dia = "0";
                    break;
                case "MARTES":
                    dia = "1";
                    break;
                case "MIERCOLES":
                    dia = "2";
                    break;
                case "JUEVES":
                    dia = "3";
                    break;
                case "VIERNES":
                    dia = "4";
                    break;
                case "SABADO":
                    dia = "5";
                    break;
                case "ESPECIAL":
                    dia = "6";
                    break;
                case "TODOS":
                    dia = null;
                    break;

                default:
                    dia = null;
                    break;

            }
            //semana = comboBox2.Text;



            if (numericUpDown2.Value == 0)
            {
                MessageBox.Show("La cantidad de Clientes no puede ser 0");
            }

            else if(cansem >=1)
            {

                   MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
              DialogResult result = MessageBox.Show("DESEA AGREGAR "+numericUpDown2.Value+" CLIENTES DISPONIBLES EN LA RUTA: " + this.comboBox1.Text + "", "INGRESO DE DISPONIBLES", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
              if (result == DialogResult.Yes)
              {


                    

                  con.conectar("EX");
                  SqlCommand cmdisp = new SqlCommand("SELECT TOP " + numericUpDown2.Value + " '" + Ruta + "' as 'RUTA', [CLIENTE],NOMBRE ,'" + dia + "' as 'DIA','" + semana + "' as 'SEMANA'  FROM [EXACTUS].["+empresa+"].[CLIENTE]  where nombre like 'DISPONIBLE' and CLIENTE not in (SELECT [CLIENTE]   FROM [DM].[dbo].[RUTERO])", con.conex);
                  SqlDataAdapter dadisp = new SqlDataAdapter(cmdisp);

                  dadisp.Fill(Disponibles);

                  



                  con.Desconectar("EX");



                    for (int j = 0; j < cansem; j++)
                    {
                        if (cansem > 1)
                        {
                            if (j == 0)
                            {
                                semanadispo = "A";
                            }
                            else if (j == 1)
                            {
                                semanadispo = "B";

                            }
                        }
                        else
                        {
                            semanadispo = comboBox3.Text;
                        }

                        if (Disponibles.Rows.Count >= 0)
                        {


                            for (int i = 0; i < Disponibles.Rows.Count; i++)
                            {

                                DataRow row = Disponibles.Rows[i];


                                string Ruta_in = Convert.ToString(row["RUTA"]);
                                string Cod_clie = Convert.ToString(row["CLIENTE"]);
                                string Nombre_crie = Convert.ToString(row["NOMBRE"]);
                                string Dia_in = Convert.ToString(row["DIA"]);
                                // string semana_in = Convert.ToString(row["SEMANA"]);

                                Carga_cliente_FR(Ruta_in, Dia_in, semanadispo, Nombre_crie, Cod_clie, 2);


                            }
                        }
                        else
                        {
                            MessageBox.Show("No Existen clientes Disponibles para agregar");



                        }
                    }
              }
              else

              {
                    MessageBox.Show("Seleccione Semana a la que se le cargaran los Diponibles");
                    comboBox3.Focus();
              
              }

            }
        }

        private void cargadorReestruturasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CORECTX_APP.VENTAS.RUTERO.Restrura_Ruteros rest = new CORECTX_APP.VENTAS.RUTERO.Restrura_Ruteros();

            rest.Show();

        }



        private void crear_disponibles()
        {
            con.conectar("EX");
            SqlCommand comand = new SqlCommand("SELECT TOP 1 [DETALLE_DIRECCION] FROM [EXACTUS].[dismo].[DETALLE_DIRECCION]  order by DETALLE_DIRECCION desc", con.conex);
            SqlDataReader dr1 = comand.ExecuteReader();


            while (dr1.Read())
            {
                detalledirec = Convert.ToInt32(dr1["DETALLE_DIRECCION"]);

            }

            detalledirec = detalledirec + 1;
            dr1.Close();

            con.Desconectar("EX");

        }
        
    }
}
