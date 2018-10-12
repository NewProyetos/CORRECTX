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
    public partial class Reporte_Saldos_Reps : Form
    {
        public Reporte_Saldos_Reps()
        {
            InitializeComponent();
        }
        //conexion myconet = new conexion();
        conexionXML con = new conexionXML();
        DataTable Dismoapp = new DataTable();
        DataTable Exactus = new DataTable();
        DataTable Visitas = new DataTable();
        DataTable Visiatasacum = new DataTable();
        DataTable vends = new DataTable();
        String fechaini;
        String fechafin;
        String Vendedor;
        String Supervisor;
        String Nombre_reporte;
        String horafin = "23:59:00";
        String cmdsup;
        String cmdruta;
        String proc;       
        String Puesto;
        String Filtro = "0";
        DateTime DT = new DateTime();
        String visitacomd;
        String visitacomd2;
        String reportid;
        String supoerv;
        String puesto = Main_Menu.Puesto;
        DataTable General_visitas = new DataTable();
        private void Reporte_Saldos_Reps_Load(object sender, EventArgs e)
        {
            DT = DateTime.Now;


            dateTimePicker1.Value = new DateTime(DT.Year,DT.Month,1);
            //General_visitas.Columns.Add("usuario", typeof(string));
            //General_visitas.Columns.Add("VisitasTotales", typeof(string));
            //General_visitas.Columns.Add("ClientesVisitados", typeof(string));
            //General_visitas.Columns.Add("VisitasEfectivas", typeof(string));
            //General_visitas.Columns.Add("categoria", typeof(string));
           

            button2.Enabled = false;
            button1.Enabled = false;
            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            radioButton1.Hide();
            radioButton2.Hide();
            //dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            checkBox1.Hide();
            checkBox2.Location = new Point(15, 26);
            groupBox5.Hide();
            con.conectar("DM");
            SqlCommand cmd2 = new SqlCommand("SELECT  [REPORT_NAME] FROM [DM].[CORRECT].[DISMOAPP_REPORTS]  where ACTIVO = 'S'" , con.condm);
            SqlDataReader dr = cmd2.ExecuteReader();

             fechaini = dateTimePicker1.Value.ToString("yyyy/MM/dd");
             fechafin = dateTimePicker2.Value.ToString("yyyy/MM/dd");
            while (dr.Read())
            {

                comboBox3.Items.Add(dr["REPORT_NAME"]);
            
            }
            // ---- carga de vendedores------


          //  cmdruta = "SELECT clie.vendedor,vis.usuario FROM dismodb.dmVisitas as vis left join dismodb.dmClientes as clie on vis.cliente = clie.codigo group by vendedor;";
            cmdruta = "SELECT ruta as vendedor , equipo as usuario FROM dismodb.dmRutero group by ruta,vendedor;";
            cargaRUTAS(cmdruta);
            //------------------------------------------------------------------------------------
                        
            //Puesto = Main_Menu.Puesto;//switch (Main_Menu.Puesto)
            switch (puesto)
            {

                case "USR":
                    comboBox1.Enabled = false;
                    comboBox2.Text = "";
                    comboBox1.Text = Main_Menu.USERWEB;
                   // cmdruta = "SELECT clie.RUTA FROM dismodb.ROUTE Rut left join dismodb.CLIENTS clie on Rut.CODCLI = clie.CODCLIE where USR = '" + Main_Menu.USERWEB + "' and RUTA is not null Group by clie.RUTA  ";
                    visitacomd = "CALL REPORTE_VISITAS_ACUMULADO ('" + fechaini + "','" + fechafin + "','" + Main_Menu.USERWEB + "')";
                    visitacomd2 = "CALL REPORTE_VISITAS_DETALLE2 ('" + fechaini + "','" + fechafin + "','" + Main_Menu.USERWEB + "')";
                    //cargaRUTAS(cmdruta);
                    //CARGA_VISITAS(visitacomd, visitacomd2);
                    combo(vends);
                    break;
                case "ADMIN":
                    comboBox1.Text = "Todos";
                    comboBox2.Text = "";
                    cmdsup = "SELECT usuario FROM dismodb.dmUsuarios where perfil <> 'GTMR';";
                    cargaSupervALL(cmdsup);
                    break;
                case "SUPBAT":
                    comboBox1.Text = "Todos";
                    comboBox2.Text = "";
                    cmdsup = "SELECT usuario FROM dismodb.dmUsuarios where perfil = 'TMR';";
                    cargaSupervALL(cmdsup);
                    break;
                case "SUPDM":
                    comboBox1.Text = "Todos";
                    comboBox2.Text = "";
                    cmdsup = "SELECT usuario FROM dismodb.dmUsuarios where perfil = 'DISMO';";
                    cargaSupervALL(cmdsup);
                    break;
                default:
                    comboBox1.Text = "";
                    comboBox1.Enabled = false;
                    comboBox2.Enabled = false;

                    break;

            }


          
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dismoapp.Columns.Clear();
            Dismoapp.Clear();
            proc = null;
            
            dataGridView1.Refresh();
            fechaini = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            fechafin = dateTimePicker2.Value.ToString("yyyy-MM-dd");
            Supervisor = comboBox1.Text;
            Vendedor = comboBox2.Text;
            Puesto = Main_Menu.Puesto;

            if (Puesto == "" || Puesto == null)
            {
                MessageBox.Show("No Tiene Acceso a los Reportes , Favor Consultar con el Departamento de IT", "ERROR");
            }

            if (reportid == "7")
            {

                proc = "CALL REPORTES_DISMOAPP('" + fechaini + "','" + fechafin + "','7',null,null,null,'null')";
                Consultadismoapp(proc);
            }



            else if (comboBox3.Text != "")
            {
                if (checkBox2.Checked)
                {
                    if (textBox1.Text != "" || textBox1.Text != null)
                    {
                        //proc = "CALL REPORTE_SALDOS ('" + fechaini + "','" + fechafin + "',null,null,'" + Puesto + "','" + textBox1.Text + "')";

                        proc = "CALL REPORTES_DISMOAPP('" + fechaini + "','" + fechafin + "','" + reportid + "',4,null,null,'" + textBox1.Text + "')";

                    }
                    else
                    {
                        MessageBox.Show("Ingrese el Codigo de cliente");
                        textBox1.Focus();
                    }
                }

                else if (Supervisor != "Todos")
                {
                    proc = "CALL REPORTES_DISMOAPP('" + fechaini + "','" + fechafin + "','" + reportid + "',2,'" + comboBox1.Text + "',null,null)";

                }

                else if (Vendedor != "")
                {
                    proc = "CALL REPORTES_DISMOAPP('" + fechaini + "','" + fechafin + "','" + reportid + "',3,null,'" + comboBox2.Text + "',null)";
                }
                else
                {
                    proc = "CALL REPORTES_DISMOAPP('" + fechaini + "','" + fechafin + "','" + reportid + "',1,null,null,null)";

                }
                if (reportid == "1")
                {
                    if (Supervisor == "Todos")
                    {
                       proc = "CALL REPORTES_DISMOAPP2('" + fechaini + "','" + fechafin + "','" + reportid + "',1,'" + comboBox1.Text + "',null,null)";
                    }
                    else
                    {
                        proc = "CALL REPORTES_DISMOAPP2('" + fechaini + "','" + fechafin + "','" + reportid + "',2,'" + comboBox1.Text + "',null,null)";
                    }
                }
             
                    Consultadismoapp(proc);
              
            }

         }


       

        private void button2_Click(object sender, EventArgs e)
        {
            string nombre_reporte = "REPORTES DE RESP";
            datagridExcel.Reporte_exel(dataGridView1,nombre_reporte);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Supervisor = comboBox1.Text;

            comboBox2.Text = "";
            comboBox2.Items.Clear();
                        
                combo(vends);

        }

        private void cargaSupervALL(string comd)
        {

            con.conectar("WEB");
            MySqlCommand mcm = new MySqlCommand(comd, con.mysqlconec);
            mcm.CommandTimeout = 1200;
            MySqlDataReader mdr = mcm.ExecuteReader();

            while (mdr.Read())
            {
                comboBox1.Items.Add(mdr["usuario"]);

            }


            con.Desconectar("WEB");
        
        }

        private void cargaRUTAS(string comdrt)
        {
            vends.Clear();
            con.conectar("WEB");
            MySqlCommand mcm = new MySqlCommand(comdrt, con.mysqlconec);
            mcm.CommandTimeout = 1200;
            MySqlDataAdapter da1 = new MySqlDataAdapter(mcm);

            da1.Fill(vends);


            con.Desconectar("WEB");

        }

        private void Consultadismoapp(string comand)
            {

                try
                {
                   
                    con.conectar("WEB");
                    MySqlCommand mcm = new MySqlCommand(comand, con.mysqlconec);
                    MySqlDataAdapter da = new MySqlDataAdapter(mcm);
                    mcm.CommandTimeout = 0;

                    da.Fill(Dismoapp);

                     if (puesto != "ADMIN")
                        {
                            if (puesto == "SUPBAT")
                            {
                                var query =
                             from order in Dismoapp.AsEnumerable()
                             where order.Field<String>("categoria") == "TMR"
                             select order;

                                General_visitas = query.CopyToDataTable();
                                dataGridView1.DataSource = General_visitas;
                            }
                            else if (puesto == "SUPDM")
                            {
                                var query = 
                                from order in Dismoapp.AsEnumerable()
                                where order.Field<String>("categoria") == "Supervisor"
                                select order;

                                General_visitas = query.CopyToDataTable();
                                dataGridView1.DataSource = General_visitas;

                            }
                            else if (puesto == "USR")
                            {
                                var query =
                                    from order in Dismoapp.AsEnumerable()
                                    where order.Field<String>("usuario") == comboBox1.Text
                                    select order;

                                General_visitas = query.CopyToDataTable();
                                dataGridView1.DataSource = General_visitas;

                            }
                        }                      
                   
                    else
                    {
                        dataGridView1.DataSource = Dismoapp;
                    }
                    con.Desconectar("WEB");
                    
                }
                catch (Exception ecx)
                {
                    MessageBox.Show(ecx.ToString(), "ERROR");
                }
             }

        private void DatosExatus()
        {

            
            con.conectar("EX");
            SqlCommand cmd2 = new SqlCommand("SELECT CLIENTE, NOMBRE,ALIAS,CONTACTO,DIRECCION,TELEFONO1,VENDEDOR  FROM EXACTUS.dismo.CLIENTE", con.condm);
            SqlDataAdapter da1 = new SqlDataAdapter(cmd2);
            da1.Fill(Exactus);
            con.Desconectar("EX");
        
        }


        


        public static DateTime TimeStampToDateTime(int TimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(TimeStamp).ToLocalTime();
            return dtDateTime;
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            int reg = dataGridView1.RowCount;

            if (reg > 0)
            {
                button2.Enabled = true;
            }

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;

            con.conectar("DM");
            SqlCommand cmd2 = new SqlCommand("SELECT  [IDENTIFICADOR]  FROM [DM].[CORRECT].[DISMOAPP_REPORTS] where REPORT_NAME = '"+this.comboBox3.Text+"'", con.condm);
            SqlDataReader dr = cmd2.ExecuteReader();

            fechaini = dateTimePicker1.Value.ToString("yyyy/MM/dd");
            fechafin = dateTimePicker2.Value.ToString("yyyy/MM/dd");
            while (dr.Read())
            {

                reportid = Convert.ToString(dr["IDENTIFICADOR"]);

            }

            con.Desconectar("DM");

            if (comboBox3.Text == "Merchandising")
            {
                radioButton1.Hide();
                radioButton2.Hide();
                checkBox1.Show();
                checkBox2.Show();
                checkBox2.Location = new Point(15, 34);
                
            }
            else if (comboBox3.Text == "Visitas")
            {
                checkBox2.Hide();
                radioButton1.Show();
                radioButton2.Show();
                
            }
            else
            {
                checkBox2.Show();
                radioButton1.Hide();
                radioButton2.Hide();
                checkBox1.Hide();
                checkBox2.Location = new Point(15, 26);

                
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                groupBox5.Show();
            }
            else
            {
                groupBox5.Hide();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
               
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
                Filtro = "1";
            }
            else
            {
                dateTimePicker1.Enabled = true;
                dateTimePicker2.Enabled = true;
                Filtro = "0";
            
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            

            switch (comboBox4.Text)
            {
            
                case "ENERO":

                 int Año = Convert.ToInt32(dateTimePicker1.Value.ToString("yyyy"));
                 int Mes = 1;
                 int Pridia = 1;
                 int Ultdia = Convert.ToInt32(DateTime.DaysInMonth(Año, Mes));
                    dateTimePicker1.Value = new DateTime(Año, Mes, Pridia);
                    dateTimePicker2.Value = new DateTime(Año, Mes, Ultdia);

                     fechaini = dateTimePicker1.Value.ToString("yyyy/MM/dd");
                     fechafin = dateTimePicker2.Value.ToString("yyyy/MM/dd");



                    break;
                case "FEBRERO":
                  Año = Convert.ToInt32(dateTimePicker1.Value.ToString("yyyy"));
                  Mes = 2;
                  Pridia = 1;
                  Ultdia = Convert.ToInt32(DateTime.DaysInMonth(Año, Mes));
                    dateTimePicker1.Value = new DateTime(Año, Mes, Pridia);
                    dateTimePicker2.Value = new DateTime(Año, Mes, Ultdia);
                     fechaini = dateTimePicker1.Value.ToString("yyyy/MM/dd");
                    fechafin = dateTimePicker2.Value.ToString("yyyy/MM/dd");
                    break;

                case "MARZO":

                  Año = Convert.ToInt32(dateTimePicker1.Value.ToString("yyyy"));
                  Mes = 3;
                  Pridia = 1;
                  Ultdia = Convert.ToInt32(DateTime.DaysInMonth(Año, Mes));
                    dateTimePicker1.Value = new DateTime(Año, Mes, Pridia);
                    dateTimePicker2.Value = new DateTime(Año, Mes, Ultdia);
                     fechaini = dateTimePicker1.Value.ToString("yyyy/MM/dd");
                     fechafin = dateTimePicker2.Value.ToString("yyyy/MM/dd");

                    break;

                case "ABRIL":

                     Año = Convert.ToInt32(dateTimePicker1.Value.ToString("yyyy"));
                  Mes = 4;
                  Pridia = 1;
                  Ultdia = Convert.ToInt32(DateTime.DaysInMonth(Año, Mes));
                    dateTimePicker1.Value = new DateTime(Año, Mes, Pridia);
                    dateTimePicker2.Value = new DateTime(Año, Mes, Ultdia);
                     fechaini = dateTimePicker1.Value.ToString("yyyy/MM/dd");
                      fechafin = dateTimePicker2.Value.ToString("yyyy/MM/dd");
                    break;

                case "MAYO":
                     Año = Convert.ToInt32(dateTimePicker1.Value.ToString("yyyy"));
                  Mes = 5;
                  Pridia = 1;
                  Ultdia = Convert.ToInt32(DateTime.DaysInMonth(Año, Mes));

                    dateTimePicker1.Value = new DateTime(Año, Mes, Pridia);
                    dateTimePicker2.Value = new DateTime(Año, Mes, Ultdia);
                     fechaini = dateTimePicker1.Value.ToString("yyyy/MM/dd");
                      fechafin = dateTimePicker2.Value.ToString("yyyy/MM/dd");
                    break;

                case "JUNIO":
                     Año = Convert.ToInt32(dateTimePicker1.Value.ToString("yyyy"));
                  Mes = 6;
                  Pridia = 1;
                  Ultdia = Convert.ToInt32(DateTime.DaysInMonth(Año, Mes));
                    dateTimePicker1.Value = new DateTime(Año, Mes, Pridia);
                    dateTimePicker2.Value = new DateTime(Año, Mes, Ultdia);
                     fechaini = dateTimePicker1.Value.ToString("yyyy/MM/dd");
                      fechafin = dateTimePicker2.Value.ToString("yyyy/MM/dd");
                    break;

                case "JULIO":
                     Año = Convert.ToInt32(dateTimePicker1.Value.ToString("yyyy"));
                  Mes = 7;
                  Pridia = 1;
                  Ultdia = Convert.ToInt32(DateTime.DaysInMonth(Año, Mes));
                    dateTimePicker1.Value = new DateTime(Año, Mes, Pridia);
                    dateTimePicker2.Value = new DateTime(Año, Mes, Ultdia);
                     fechaini = dateTimePicker1.Value.ToString("yyyy/MM/dd");
                      fechafin = dateTimePicker2.Value.ToString("yyyy/MM/dd");
                    break;

                case "AGOSTO":

                     Año = Convert.ToInt32(dateTimePicker1.Value.ToString("yyyy"));
                  Mes = 8;
                  Pridia = 1;
                  Ultdia = Convert.ToInt32(DateTime.DaysInMonth(Año, Mes));
                    dateTimePicker1.Value = new DateTime(Año, Mes, Pridia);
                    dateTimePicker2.Value = new DateTime(Año, Mes, Ultdia);
                     fechaini = dateTimePicker1.Value.ToString("yyyy/MM/dd");
                     fechafin = dateTimePicker2.Value.ToString("yyyy/MM/dd");
                    break;

                case "SEPTIEMBRE":

                     Año = Convert.ToInt32(dateTimePicker1.Value.ToString("yyyy"));
                  Mes = 9;
                  Pridia = 1;
                  Ultdia = Convert.ToInt32(DateTime.DaysInMonth(Año, Mes));
                    dateTimePicker1.Value = new DateTime(Año, Mes, Pridia);
                    dateTimePicker2.Value = new DateTime(Año, Mes, Ultdia);
                     fechaini = dateTimePicker1.Value.ToString("yyyy/MM/dd");
                     fechafin = dateTimePicker2.Value.ToString("yyyy/MM/dd");
                    break;
                case "OCTUBRE":

                     Año = Convert.ToInt32(dateTimePicker1.Value.ToString("yyyy"));
                  Mes = 10;
                  Pridia = 1;
                  Ultdia = Convert.ToInt32(DateTime.DaysInMonth(Año, Mes));
                    dateTimePicker1.Value = new DateTime(Año, Mes, Pridia);
                    dateTimePicker2.Value = new DateTime(Año, Mes, Ultdia);
                     fechaini = dateTimePicker1.Value.ToString("yyyy/MM/dd");
                    fechafin = dateTimePicker2.Value.ToString("yyyy/MM/dd");
                    break;
                case "NOVIEMBRE":
                     Año = Convert.ToInt32(dateTimePicker1.Value.ToString("yyyy"));
                  Mes = 11;
                  Pridia = 1;
                  Ultdia = Convert.ToInt32(DateTime.DaysInMonth(Año, Mes));
                    dateTimePicker1.Value = new DateTime(Año, Mes, Pridia);
                    dateTimePicker2.Value = new DateTime(Año, Mes, Ultdia);
                     fechaini = dateTimePicker1.Value.ToString("yyyy/MM/dd");
                     fechafin = dateTimePicker2.Value.ToString("yyyy/MM/dd");
                    break;

                case "DICIEMBRE":
                     Año = Convert.ToInt32(dateTimePicker1.Value.ToString("yyyy"));
                  Mes = 12;
                  Pridia = 1;
                  Ultdia = Convert.ToInt32(DateTime.DaysInMonth(Año, Mes));
                    dateTimePicker1.Value = new DateTime(Año, Mes, Pridia);
                    dateTimePicker2.Value = new DateTime(Año, Mes, Ultdia);
                     fechaini = dateTimePicker1.Value.ToString("yyyy/MM/dd");
                     fechafin = dateTimePicker2.Value.ToString("yyyy/MM/dd");
                    break;

                 

            
            }

            //visitacomd = "CALL REPORTE_VISITAS_ACUMULADO ('" + fechaini + "','" + fechafin + "','" + comboBox1.Text + "')";
            //visitacomd2 = "CALL REPORTE_VISITAS_DETALLE2 ('" + fechaini + "','" + fechafin + "','" + comboBox1.Text + "')";
            //CARGA_VISITAS(visitacomd, visitacomd2);

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        { 
           int Año = Convert.ToInt32(dateTimePicker1.Value.ToString("yyyy"));
           int Mes = Convert.ToInt32(dateTimePicker1.Value.ToString("MM"));
           int Ultdia = Convert.ToInt32(DateTime.DaysInMonth(Año, Mes));
           dateTimePicker2.Value = new DateTime(Año, Mes, Ultdia);


          comboBox4.Text = dateTimePicker1.Value.ToString("MMMM").ToUpper();


        }

        private void CARGA_VISITAS(string comando, string comando2)
        {
            Visiatasacum.Clear();
            Visitas.Clear();
            try
            {
                con.conectar("WEB");
                
                MySqlCommand mcm = new MySqlCommand(comando, con.mysqlconec);
                MySqlDataAdapter da = new MySqlDataAdapter(mcm);

                da.Fill(Visiatasacum);

                label8.Text = Convert.ToString(Visiatasacum.Rows.Count);
                MySqlCommand mcm1 = new MySqlCommand(comando2, con.mysqlconec);
                MySqlDataAdapter da1 = new MySqlDataAdapter(mcm1);

                da1.Fill(Visitas);

                label10.Text = Convert.ToString(Visitas.Rows.Count);

                //dataGridView1.DataSource = Dismoapp;
                con.Desconectar("WEB");
            }
            catch (Exception ecx)
            {
                MessageBox.Show(ecx.ToString(), "ERROR");
            }
        
        }

        public void combo(DataTable dts)
        {
          //  comboBox1.Items.Clear();
          //  comboBox2.Items.Clear();
            if (comboBox1.Text != "Todos")
            {
                IEnumerable<DataRow> venQuery =
                 from vend in dts.AsEnumerable()
                 select vend;
                IEnumerable<DataRow> venruta =
                venQuery.Where(p => p.Field<string>("usuario") == "" + comboBox1.Text + "");


                foreach (DataRow t1 in venruta)
                {

                    comboBox2.Items.Add(t1.Field<string>("vendedor"));

                }
            }
            else
            {

                var result = from row in dts.AsEnumerable()
                             group row by row.Field<string>("vendedor") into grp
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
            
            
            }


        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Vendedor = comboBox2.Text;
        }
    }
}
