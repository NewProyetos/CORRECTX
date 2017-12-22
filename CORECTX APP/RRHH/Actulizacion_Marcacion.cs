using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Sinconizacion_EXactus.CORECTX_APP.RRHH
{
    public partial class Actulizacion_Marcacion : Form
    {
        public Actulizacion_Marcacion()
        {
            InitializeComponent();
        }
        conexionXML con = new conexionXML();
        DataTable data_empl_marca = new DataTable();
        DataTable empleasos_exac = new DataTable();
        DataTable sucursal_ = new DataTable();
        DataTable puesto_ = new DataTable();
        DataTable empresa_ = new DataTable();
        DataTable divicion_ = new DataTable();
        DataTable departamento_ = new DataTable();

        String empresa_mar;
        String Primer_Nom_mar;
        String Segundo_Nom_mar;
        String Apellido_mar;
        String cod_emp_mar;
        String depart_mar;
        String puesto_mar;
        String sucursal;
        String divicion;
                         
        int id_emple;
        int id_puesto;
        int id_dep;
        int id_empre;
        int id_divicion;
        int id_sucursal;


        private void Actulizacion_Marcacion_Load(object sender, EventArgs e)
        {

            dataGridView1.RowHeadersVisible = false;
            //dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;


            dataGridView2.RowHeadersVisible = false;
            //dataGridView1.AutoResizeColumns();
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView2.ReadOnly = true;
            dataGridView2.AllowUserToAddRows = false;

            fill_tablas();
            carga_data_parcacion();

            combo(comboBox1, empresa_, "NAME");
            combo(comboBox2, departamento_, "NAME");
           // combo(toolStripComboBox1, departamento_, "NAME");
           toolstripcombo(toolStripComboBox1, departamento_, "NAME");
            toolstripcombo(toolStripComboBox3, empresa_, "NAME");
            combo(comboBox3, puesto_, "NAME");
            combo(comboBox4, sucursal_, "NAME");
            combo(comboBox5, divicion_, "NAME");


            groupBox2.Enabled = false;
        }
        private void fill_tablas()
        {
            sucursal_.Clear();
            puesto_.Clear();
            empresa_.Clear();
            divicion_.Clear();
            departamento_.Clear();
            con.conectar("SEG");

            SqlCommand cmd2 = new SqlCommand("SELECT [ID],[NAME] FROM [ACCESSCONTROL].[dbo].[LOCATION]", con.conseg);
            SqlDataAdapter da = new SqlDataAdapter(cmd2);
            da.Fill(sucursal_);
                       
            
            SqlCommand cmd3 = new SqlCommand("SELECT [ID],[NAME] FROM [ACCESSCONTROL].[dbo].[TITLE]", con.conseg);
            SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
            da3.Fill(puesto_);

           
           
            SqlCommand cmd4 = new SqlCommand("SELECT [ID],[NAME] FROM [ACCESSCONTROL].[dbo].[BUILDING]", con.conseg);
            SqlDataAdapter da4 = new SqlDataAdapter(cmd4);
            da4.Fill(empresa_);


            SqlCommand cmd5 = new SqlCommand("SELECT [ID],[NAME] FROM [ACCESSCONTROL].[dbo].[DIVISION]", con.conseg);
            SqlDataAdapter da5 = new SqlDataAdapter(cmd5);
            da5.Fill(divicion_);


            SqlCommand cmd6 = new SqlCommand("SELECT [ID],[NAME] FROM [ACCESSCONTROL].[dbo].[DEPT]", con.conseg);
            SqlDataAdapter da6 = new SqlDataAdapter(cmd6);
            da6.Fill(departamento_);


            con.Desconectar("SEG");

        }
        private void carga_data_parcacion()
        {
            data_empl_marca.Clear();
            empleasos_exac.Clear();
            con.conectar("SEG");

            SqlCommand cmd = new SqlCommand("[dbo].[Reporte_Empleados]", con.conseg);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter datap = new SqlDataAdapter(cmd);

            datap.Fill(data_empl_marca);

            dataGridView1.DataSource = data_empl_marca;



                con.Desconectar("SEG");
            comoboempresas();

            con.conectar("DM");

            SqlCommand cmd1 = new SqlCommand("[CORRECT].[EMPLEADOS]", con.condm);
            cmd1.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter datap1 = new SqlDataAdapter(cmd1);

            datap1.Fill(empleasos_exac);

            dataGridView2.DataSource = empleasos_exac;



            con.Desconectar("DM");



            //comobodepartamento();
            //comobodepuestos();
            //comobosucursal();


        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {

            data_empl_marca.DefaultView.RowFilter = string.Format("Convert(CODIGO_EMP,'System.String') like '%{0}%'", this.toolStripTextBox1.Text);
            dataGridView1.DataSource = data_empl_marca;
        }

        private void toolStripTextBox2_TextChanged(object sender, EventArgs e)
        {
            data_empl_marca.DefaultView.RowFilter = string.Format("Convert(NOMBRE,'System.String') like '%{0}%'", this.toolStripTextBox2.Text);
            dataGridView1.DataSource = data_empl_marca;

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
          int  idx = dataGridView1.CurrentRow.Index;

            Primer_Nom_mar = Convert.ToString(dataGridView1.Rows[idx].Cells[3].Value);
            Segundo_Nom_mar = Convert.ToString(dataGridView1.Rows[idx].Cells[4].Value);
            Apellido_mar = Convert.ToString(dataGridView1.Rows[idx].Cells[2].Value);
            cod_emp_mar =  Convert.ToString(dataGridView1.Rows[idx].Cells[5].Value);
            puesto_mar = Convert.ToString(dataGridView1.Rows[idx].Cells[7].Value);
            depart_mar = Convert.ToString(dataGridView1.Rows[idx].Cells[8].Value);
            empresa_mar = Convert.ToString(dataGridView1.Rows[idx].Cells[9].Value);
            id_emple = Convert.ToInt32(dataGridView1.Rows[idx].Cells[0].Value);
            sucursal = Convert.ToString(dataGridView1.Rows[idx].Cells[10].Value);
            divicion = Convert.ToString(dataGridView1.Rows[idx].Cells[11].Value);

            textBox4.Text = cod_emp_mar;
            textBox1.Text = Primer_Nom_mar;
            textBox2.Text = Segundo_Nom_mar;
            textBox3.Text = Apellido_mar;
            comboBox1.Text = empresa_mar;
            comboBox2.Text = depart_mar;
            comboBox3.Text = puesto_mar;
            comboBox4.Text = sucursal;
            comboBox5.Text = divicion;
            label9.Text = Convert.ToString(id_emple);

            groupBox2.Enabled = true;
            dato_selec(empresa_mar, depart_mar, puesto_mar, sucursal, divicion);



            empleasos_exac.DefaultView.RowFilter = string.Format("Convert(PNOMBRE,'System.String') like '%{0}%' and Convert(SNOMBRE,'System.String') like '%{1}%' ", Primer_Nom_mar,Segundo_Nom_mar);
            dataGridView2.DataSource = empleasos_exac;

        }
        public void comoboempresas()
        {
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.Items.Clear();
            }
            

            con.conectar("SEG");

            //string textResultado = Regex.Replace(comboBox3.Text, @"[^a-zA-z0-9 ]+", "");

            SqlCommand emprecm = new SqlCommand("SELECT [NAME] FROM [ACCESSCONTROL].[dbo].[BUILDING]", con.conseg);
            SqlDataReader empread = emprecm.ExecuteReader();

            while (empread.Read())
            {

                comboBox1.Items.Add(empread["NAME"]);


            }

            empread.Close();
            con.Desconectar("SEG");


            


        }

       

        private void toolStripComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            data_empl_marca.DefaultView.RowFilter = string.Format("Convert(EMPRESA,'System.String') like '%{0}%'", this.toolStripComboBox3.Text);
            dataGridView1.DataSource = data_empl_marca;

        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            data_empl_marca.DefaultView.RowFilter = string.Format("Convert(DEPARTAMENTO,'System.String') = '{0}'", this.toolStripComboBox1.Text);
            dataGridView1.DataSource = data_empl_marca;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (id_emple > 0)
            {
                Update_EMP(id_emple,textBox1.Text,textBox2.Text,textBox3.Text,textBox4.Text);
            }
        }

        private void Update_EMP(int ID_EMP,string fname,string mname,string lname,string ssno)
        {
            DateTime fechahoy = DateTime.Now;
            con.conectar("SEG");
            SqlCommand cmd8 = new SqlCommand();
            cmd8.Connection = con.conseg;
            cmd8.CommandText = "UPDATE [ACCESSCONTROL].[dbo].[EMP] SET [LASTNAME]=@LASTNAME,[FIRSTNAME] = @FIRSTNAME,[MIDNAME] = @MIDNAME,[SSNO] = @SSNO,[LASTCHANGED] = @FECHAUPDATE where ID = '"+ID_EMP+"'";
            cmd8.Parameters.Add("@LASTNAME", SqlDbType.NVarChar).Value = lname;
            cmd8.Parameters.Add("@FIRSTNAME", SqlDbType.NVarChar).Value = fname;
            cmd8.Parameters.Add("@MIDNAME", SqlDbType.NVarChar).Value = mname;
            cmd8.Parameters.Add("@SSNO", SqlDbType.NVarChar).Value = ssno;
            cmd8.Parameters.Add("@FECHAUPDATE", SqlDbType.DateTime).Value = fechahoy;
           
            cmd8.ExecuteNonQuery();

            con.Desconectar("SEG");

            Update_UEMP(ID_EMP, comboBox1.Text, comboBox3.Text, id_puesto, id_dep, id_divicion, id_empre,id_sucursal);

            carga_data_parcacion();

            MessageBox.Show("Usuario Actualizado Exitosamente");

        }
        private void Update_UEMP(int ID ,string ADDR1 , string CITY,int TITLE ,int DEPT,int divic, int building , int locat)
        {
            DateTime fechahoy = DateTime.Now;
            con.conectar("SEG");
            SqlCommand cmd8 = new SqlCommand();
            cmd8.Connection = con.conseg;
            cmd8.CommandText = "UPDATE  [ACCESSCONTROL].[dbo].[UDFEMP] SET [ADDR1] = @ADDR1 , [CITY] = @CITY , [TITLE] = @TITLE ,[DEPT] = @DEPT,[DIVISION] = @DIVISION,[BUILDING] = @BUILDING , [LOCATION] = @LOCATION where ID = @ID";
            cmd8.Parameters.Add("@ID", SqlDbType.NVarChar).Value = ID ;
            cmd8.Parameters.Add("@ADDR1", SqlDbType.NVarChar).Value = ADDR1;
            cmd8.Parameters.Add("@CITY", SqlDbType.NVarChar).Value = CITY;
            cmd8.Parameters.Add("@TITLE", SqlDbType.Int).Value = TITLE;
            cmd8.Parameters.Add("@DEPT", SqlDbType.Int).Value = DEPT;
            cmd8.Parameters.Add("@DIVISION", SqlDbType.Int).Value = divic;
            cmd8.Parameters.Add("@BUILDING", SqlDbType.Int).Value = building;
            cmd8.Parameters.Add("@LOCATION", SqlDbType.Int).Value = locat;

            cmd8.ExecuteNonQuery();

            con.Desconectar("SEG");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            id_empre = combo_select(empresa_, comboBox1.Text);



        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)

        {

            id_puesto = combo_select(puesto_, comboBox3.Text);

            //con.conectar("SEG");

            //string textResultado = Regex.Replace(comboBox3.Text, @"[^a-zA-z0-9 ]+", "");

            //SqlCommand emprecm = new SqlCommand("SELECT  [ID] FROM [ACCESSCONTROL].[dbo].[TITLE] where NAME = '"+textResultado+"'",con.conseg);
            //SqlDataReader empread = emprecm.ExecuteReader();

            //while (empread.Read())
            //{
            //    id_puesto = Convert.ToInt32(empread["ID"]);


            //}

            //empread.Close();
            //con.Desconectar("SEG");


        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            

        }

        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                int idx = dataGridView1.CurrentRow.Index;

                Primer_Nom_mar = Convert.ToString(dataGridView1.Rows[idx].Cells[3].Value);
                Segundo_Nom_mar = Convert.ToString(dataGridView1.Rows[idx].Cells[4].Value);
                Apellido_mar = Convert.ToString(dataGridView1.Rows[idx].Cells[2].Value);
                cod_emp_mar = Convert.ToString(dataGridView1.Rows[idx].Cells[5].Value);
                puesto_mar = Convert.ToString(dataGridView1.Rows[idx].Cells[7].Value);
                depart_mar = Convert.ToString(dataGridView1.Rows[idx].Cells[8].Value);
                empresa_mar = Convert.ToString(dataGridView1.Rows[idx].Cells[9].Value);
                id_emple = Convert.ToInt32(dataGridView1.Rows[idx].Cells[0].Value);
                sucursal = Convert.ToString(dataGridView1.Rows[idx].Cells[10].Value);
                divicion = Convert.ToString(dataGridView1.Rows[idx].Cells[11].Value);

                textBox4.Text = cod_emp_mar;
                textBox1.Text = Primer_Nom_mar;
                textBox2.Text = Segundo_Nom_mar;
                textBox3.Text = Apellido_mar;
                comboBox1.Text = empresa_mar;
                comboBox2.Text = depart_mar;
                comboBox3.Text = puesto_mar;
                label9.Text = Convert.ToString(id_emple);

                groupBox2.Enabled = true;

                dato_selec(empresa_mar, depart_mar, puesto_mar, sucursal, divicion);

                empleasos_exac.DefaultView.RowFilter = string.Format("Convert(PNOMBRE,'System.String') like '%{0}%' and Convert(SNOMBRE,'System.String') like '%{1}%' ", Primer_Nom_mar, Segundo_Nom_mar);
                dataGridView2.DataSource = empleasos_exac;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

           id_dep =  combo_select(departamento_, comboBox2.Text);

            

        }

        public void combo(ComboBox cb, DataTable dts, string parametro)
        {
            cb.Items.Clear();

            var result = from row in dts.AsEnumerable()
                         group row by row.Field<string>(parametro) into grp
                         select new
                         {
                             familia = grp.Key,

                         };
            foreach (var t in result)
            {
                if (t.familia == null || t.familia == "")
                {

                }
                else
                {
                    cb.Items.Add(t.familia);

                }
            }
        }
        public void toolstripcombo(ToolStripComboBox cb, DataTable dts, string parametro)
        {
            cb.Items.Clear();

            var result = from row in dts.AsEnumerable()
                         group row by row.Field<string>(parametro) into grp
                         select new
                         {
                             familia = grp.Key,

                         };
            foreach (var t in result)
            {
                if (t.familia == null || t.familia == "")
                {

                }
                else
                {
                    cb.Items.Add(t.familia);

                }
            }
        }

        private void dato_selec( string emp , string dep ,string pues ,string agen , string div)
        {

            //---------------------- empresa ---------------------------------

            
            var results = from myRow in empresa_.AsEnumerable()
                          where myRow.Field<string>("NAME") == emp

                          select new
                          {
                              Nombre = myRow.Field<int>("ID")
                          };

            foreach (var rs1 in results)
            {
                id_empre = rs1.Nombre;
            }

            //-------------------- departamento ----------------------------------


            var results1 = from myRow in departamento_.AsEnumerable()
                          where myRow.Field<string>("NAME") == dep

                          select new
                          {
                              Nombre = myRow.Field<int>("ID")
                          };

            foreach (var rs1 in results1)
            {
                id_dep = rs1.Nombre;
            }

            //----------------------- puesto ----------------------------------------

            var results2 = from myRow in puesto_.AsEnumerable()
                           where myRow.Field<string>("NAME") == pues

                           select new
                           {
                               Nombre = myRow.Field<int>("ID")
                           };

            foreach (var rs1 in results2)
            {
                id_puesto = rs1.Nombre;
            }

            //------------------------ agencia --------------------------------------

            var results3 = from myRow in sucursal_.AsEnumerable()
                           where myRow.Field<string>("NAME") == agen

                           select new
                           {
                               Nombre = myRow.Field<int>("ID")
                           };

            foreach (var rs1 in results3)
            {
               id_sucursal = rs1.Nombre;
            }


            // --------------------- divicion ---------------------------------

            var results4 = from myRow in divicion_.AsEnumerable()
                           where myRow.Field<string>("NAME") == div

                           select new
                           {
                               Nombre = myRow.Field<int>("ID")
                           };

            foreach (var rs1 in results4)
            {
                id_divicion = rs1.Nombre;
            }


        }


        private int combo_select(DataTable dt, string valor)
        {
            int resultado = 0;

            var results = from myRow in dt.AsEnumerable()
                          where myRow.Field<string>("NAME") == valor

                          select new
                          {
                              Nombre = myRow.Field<int>("ID")
                          };

            foreach (var rs1 in results)
            {
                resultado = rs1.Nombre;
               
            }

            return resultado;
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            id_sucursal = combo_select(sucursal_, comboBox4.Text);
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
           id_divicion = combo_select(divicion_, comboBox5.Text);
        }

        private void toolStripTextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
           empleasos_exac.DefaultView.RowFilter = string.Format("Convert(EMPLEADO,'System.String') like '%{0}%'", this.toolStripTextBox3.Text);
            dataGridView2.DataSource = empleasos_exac;
        }

        private void toolStripComboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            empleasos_exac.DefaultView.RowFilter = string.Format("Convert(PNOMBRE,'System.String') like '%{0}%'", this.toolStripTextBox4.Text);
            dataGridView2.DataSource = empleasos_exac;
        }

        private void toolStripComboBox1_KeyUp(object sender, KeyEventArgs e)
        {
            data_empl_marca.DefaultView.RowFilter = string.Format("Convert(DEPARTAMENTO,'System.String') = '{0}'", this.toolStripComboBox1.Text);
            dataGridView1.DataSource = data_empl_marca;

        }

        private void toolStripComboBox3_KeyUp(object sender, KeyEventArgs e)
        {
            data_empl_marca.DefaultView.RowFilter = string.Format("Convert(EMPRESA,'System.String') like '%{0}%'", this.toolStripComboBox3.Text);
            dataGridView1.DataSource = data_empl_marca;
        }

        private void toolStripTextBox4_KeyUp(object sender, KeyEventArgs e)
        {
            empleasos_exac.DefaultView.RowFilter = string.Format("Convert(PNOMBRE,'System.String') like '%{0}%'", this.toolStripTextBox4.Text);
            dataGridView2.DataSource = empleasos_exac;
        }
    }

}
