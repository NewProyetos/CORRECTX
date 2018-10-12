using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using System.Data.SqlClient;
 

namespace Sinconizacion_EXactus.CORECTX_APP.COMPRAS.Importador
{
    public partial class odoo : Form
    {
        public odoo()
        {
            InitializeComponent();
        }
        conexionXML con = new conexionXML();
        DataTable solic = new DataTable();
        DataTable solicdet = new DataTable();
        DataTable errores = new DataTable();
        int marcados = 0;
        string Deparmet, usr_autoriza, comentario, usr_solicita,orden_odo;


        private void odoo_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button3.Enabled = false;
            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ReadOnly = false;
            dataGridView1.AllowUserToAddRows = false;

            dataGridView2.Enabled = true;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.ReadOnly = false;
            dataGridView2.AllowUserToAddRows = false;


            errores.Columns.Add("ORDEN_ODOO", typeof(string));
            errores.Columns.Add("ERROR", typeof(string));
            errores.Columns.Add("USUARIO", typeof(string));            
            errores.Columns.Add("FECHA", typeof(DateTime));
        }
              

        private void button1_Click(object sender, EventArgs e)
        {
            solic.Clear();
            con.conectar("ODOO");
            string conm = "SELECT sc.id, sc.name, sc.code, sc.date_start, sc.end_start, sc.requested_by,usr.login as req_id,usr2.login as req_usr,sc.assigned_to, sc.description, state, fl.license_plate, sc.centro_costo,sc.cuenta_contable,case when dp.parent_id is null then  sc.department_id else dp.parent_id end as department_id  , dp.name as deparmet ,sc.create_uid,usr3.login as create_usr, sc.create_date, sc.write_uid,usr4.login as write_usr, sc.write_date  FROM public.sprogroup_purchase_request as sc  LEFT JOIN  public.hr_department as dp on sc.department_id = dp.id  LEFT JOIN public.res_users as usr  on sc.requested_by = usr.id LEFT JOIN public.res_users as usr2  on sc.assigned_to = usr2.id  LEFT JOIN public.res_users as usr3  on sc.create_uid = usr3.id LEFT JOIN public.res_users as usr4  on sc.write_uid = usr4.id LEFT JOIN  public.fleet_vehicle as fl on sc.vehiculo_id = fl.id  where sc.state = 'done' and sc.softland_sc is null;";
            NpgsqlDataAdapter adap = new NpgsqlDataAdapter(conm, con.pgcon);
            adap.Fill(solic);

            con.Desconectar("ODOO");


            if (solic.Rows.Count >= 1)
            {

                if (dataGridView1.Columns.Contains("Importar"))
                {
                }
                else
                {
                    addchekdw();
                }
                dataGridView1.DataSource = solic;

                this.dataGridView1.Columns["id"].Visible = false;
                this.dataGridView1.Columns["requested_by"].Visible = false;
                this.dataGridView1.Columns["req_id"].Visible = false;
                this.dataGridView1.Columns["assigned_to"].Visible = false;
                this.dataGridView1.Columns["state"].Visible = false;
                this.dataGridView1.Columns["license_plate"].Visible = false;
                this.dataGridView1.Columns["centro_costo"].Visible = false;
                this.dataGridView1.Columns["cuenta_contable"].Visible = false;
                this.dataGridView1.Columns["department_id"].Visible = false;
                this.dataGridView1.Columns["deparmet"].Visible = false;
                this.dataGridView1.Columns["create_uid"].Visible = false;
                this.dataGridView1.Columns["write_uid"].Visible = false;
                this.dataGridView1.Columns["write_usr"].Visible = false;
                this.dataGridView1.Columns["write_date"].Visible = false;

                dataGridView1.Refresh();
            }



        }

        private void backgroundcarga_DoWork(object sender, DoWorkEventArgs e)
        {
          
        }

        private void backgroundcarga_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           

        }

        private void addchekdw()
        {
            
            DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn()
            {
                Name = "Importar"

            };
            dataGridView1.Columns.Add(chk);


        }
        private void chequear()
        {
            marcados = 0;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (button3.Text == "Marcar")
                {
                    dataGridView1.Rows[i].Cells[0].Value = true;
                    marcados = marcados + 1;
                }
                else
                {
                    dataGridView1.Rows[i].Cells[0].Value = false;
                    if (marcados >= 1)
                    {
                        marcados = marcados - 1;
                    }
                }        
                    
               
            }
            if (marcados >= 1)
            {
                button3.Text = "Desmarcar";
            }
            else
            {
                button3.Text = "Marcar";
            }

            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            chequear();
        }

        private void contar_marcados()
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                DataGridViewRow row = dataGridView1.Rows[i];
                DataGridViewCheckBoxCell cell = row.Cells[0] as DataGridViewCheckBoxCell;
                if (Convert.ToBoolean(cell.Value) == true)
                {
                    marcados = marcados + 1;
                }
                else
                {
                    if (marcados >= 1)
                    {
                        marcados = marcados - 1;
                    }
                    

                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            estrurar_datos();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            contar_marcados();
        }

        private void backgroundinsert_DoWork(object sender, DoWorkEventArgs e)
        {
            
        }

        private void estrurar_datos()
        {
            errores.Clear();

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                DataGridViewRow row = dataGridView1.Rows[i];
                DataGridViewCheckBoxCell cell = row.Cells[0] as DataGridViewCheckBoxCell;
                if (Convert.ToBoolean(cell.Value) == true)
                {

                   


                    string Placa = "";

                    orden_odo = Convert.ToString(row.Cells["name"].Value);
                    string code_orden = Convert.ToString(row.Cells["code"].Value);
                    string id = Convert.ToString(row.Cells["id"].Value);
                    string odoo_orden = orden_odo + "-" + code_orden;
                    Deparmet = dpto_id(Convert.ToString(row.Cells["department_id"].Value));
                    usr_autoriza = usuarios_ex(Convert.ToString(row.Cells["assigned_to"].Value), Convert.ToString(row.Cells["requested_by"].Value)).Item2;
                    usr_solicita = usuarios_ex(Convert.ToString(row.Cells["requested_by"].Value), Convert.ToString(row.Cells["requested_by"].Value)).Item1;
                    DateTime fecha_sol = Convert.ToDateTime(Convert.ToString(row.Cells["date_start"].Value));
                    DateTime fecha_req = Convert.ToDateTime(Convert.ToString(row.Cells["end_start"].Value));
                    DateTime fecha_auto = Convert.ToDateTime(Convert.ToString(row.Cells["write_date"].Value));
                    DateTime fecha_hora = Convert.ToDateTime(Convert.ToString(row.Cells["create_date"].Value));
                    if (DBNull.Value == row.Cells["license_plate"].Value)
                    {
                        Placa = null;
                    }
                    else
                    {
                        Placa = Convert.ToString(row.Cells["license_plate"].Value);
                       
                    }
                    string comentario = Convert.ToString(row.Cells["description"].Value);
                    string ID = Convert.ToString(row.Cells["id"].Value);
                    string centro_c = Convert.ToString(row.Cells["centro_costo"].Value);
                    string cuenta_c = Convert.ToString(row.Cells["cuenta_contable"].Value);
                    int lineas = 0;

                   if (exist_odoo_orden(odoo_orden))
                    {

                    }
                   else
                    { 


                        con.conectar("ODOO");
                    string conln = "SELECT COUNT(id) as lineas FROM public.sprogroup_purchase_request_line where request_id = '" + id + "';";
                    NpgsqlCommand comln = new NpgsqlCommand(conln,con.pgcon);
                    NpgsqlDataReader dr = comln.ExecuteReader();

                    while (dr.Read())
                    {
                        lineas = Convert.ToInt32(dr["lineas"]);
                    }
                    dr.Close();
                    con.Desconectar("ODOOO");






                    if (DBNull.Value == row.Cells["centro_costo"].Value)
                    {
                        errores.Rows.Add(orden_odo+"-"+code_orden, "Centro de Costo  no puede estar en blanco", usr_solicita, fecha_sol);
                    }
                    else
                        if (DBNull.Value == row.Cells["cuenta_contable"].Value)
                    {
                        errores.Rows.Add(orden_odo +"-"+code_orden, "Cuenta Contable  no puede estar en blanco", usr_solicita, fecha_sol);
                    }
                    else
                        if (contable(cuenta_c, centro_c).Item1)
                    {
                        errores.Rows.Add(orden_odo + "-" + code_orden, "Cuenta Contable  no existe en EXACTUS", usr_solicita, fecha_sol);
                    }
                    else
                       if (contable(cuenta_c, centro_c).Item2)
                    {
                        errores.Rows.Add(orden_odo + "-" + code_orden, "Centro de Costo  no existe en EXACTUS", usr_solicita, fecha_sol);
                    }
                    
                    else if (lineas <=0)
                     {
                        errores.Rows.Add(orden_odo + "-" + code_orden, "No se encontraron Lineas para esta  Solicitud", usr_solicita, fecha_sol);

                     }
                    else if (errores.Rows.Count >= 1)
                        {
                            dataGridView2.DataSource = errores;
                        }
                        else
                        {

                            con.conectar("DM");
                            SqlCommand cmd = new SqlCommand("[CORRECT].[SOLICITUD_COMPRA]", con.condm);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@departamento", Deparmet);
                            cmd.Parameters.AddWithValue("@fecha_solicitud", fecha_sol);
                            cmd.Parameters.AddWithValue("@fecha_requerida", fecha_req);
                            cmd.Parameters.AddWithValue("@autorizada_por", usr_autoriza);
                            cmd.Parameters.AddWithValue("@fecha_aurotiza", fecha_auto);
                            cmd.Parameters.AddWithValue("@placa", Placa);
                            cmd.Parameters.AddWithValue("@oder_odoo", odoo_orden);
                            cmd.Parameters.AddWithValue("@comentario", comentario);
                            cmd.Parameters.AddWithValue("@fecha_hora", fecha_hora);
                            cmd.Parameters.AddWithValue("@usuario", usr_solicita);
                            cmd.Parameters.AddWithValue("@linea_noasignada", lineas);


                            cmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);
                            cmd.Parameters.Add("@codigo_sc", SqlDbType.VarChar, 100);

                            cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                            cmd.Parameters["@codigo_sc"].Direction = ParameterDirection.Output;

                            cmd.ExecuteNonQuery();
                            string retunvalue = (string)cmd.Parameters["@msg"].Value;
                            string cod_sc = (string)cmd.Parameters["@codigo_sc"].Value;

                            con.Desconectar("DM");



                            insert_detalle(ID, centro_c, cuenta_c, cod_sc, fecha_req);

                            update_sc_odoo(cod_sc, ID, DateTime.Now);

                        }
                    }
                }
                if (errores.Rows.Count >= 1)
                {
                    dataGridView2.DataSource = errores;
                }
                button1_Click(null, null);
            }
        }
        private bool exist_odoo_orden(string odoo_orden)
        {

            con.conectar("EX");

            SqlCommand cmd = new SqlCommand("SELECT COUNT([SOLICITUD_OC]) FROM [EXACTUS].[dismo].[SOLICITUD_OC] where RUBRO1 = @SOLIC", con.conex);
            cmd.Parameters.AddWithValue("SOLIC", odoo_orden);
            int contar = Convert.ToInt32(cmd.ExecuteScalar());

            if (contar > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

            con.Desconectar("EX");
           
        }
        private Tuple<bool,int> exist_sc(string solic)
        {
            bool exist = false;
            int lincant = 0;

            con.conectar("EX");

            SqlCommand cmd = new SqlCommand("SELECT COUNT([SOLICITUD_OC]) FROM [EXACTUS].[dismo].[SOLICITUD_OC] where SOLICITUD_OC = @SOLIC", con.conex);
            cmd.Parameters.AddWithValue("SOLIC", solic);
            int contar = Convert.ToInt32(cmd.ExecuteScalar());

            if (contar > 0)
            {
                exist = false;
            }
            else
            {
                exist = true;
            }



            SqlCommand cmd2 = new SqlCommand("SELECT COUNT([SOLICITUD_OC]) FROM [EXACTUS].[dismo].[SOLICITUD_OC_LINEA] where SOLICITUD_OC = @SOLIC", con.conex);
            cmd2.Parameters.AddWithValue("SOLIC", solic);
            int contarc = Convert.ToInt32(cmd2.ExecuteScalar());

            if (contarc > 0)
            {
                lincant = contarc;
            }



            con.Desconectar("EX");



            return Tuple.Create(exist, lincant);


        }


        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridView1.RowCount >= 1)
            {
                button2.Enabled = true;
                button3.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
                button3.Enabled = false;
            }
        }

        private void insert_detalle(string id, string centro_costo, string cuenta_contable, string cod_sc, DateTime fecha_req)
        {
            if (exist_sc(cod_sc).Item1)
            {
            }
            else
            {
                if ((exist_sc(cod_sc).Item2) == 0)
                {
                    solicdet.Clear();
                    con.conectar("ODOO");
                    string conln = "SELECT id, message_last_post, product_id, name,  product_qty,description, specifications, request_state FROM public.sprogroup_purchase_request_line where request_id = '" + id + "';";
                    NpgsqlDataAdapter adapln = new NpgsqlDataAdapter(conln, con.pgcon);
                    adapln.Fill(solicdet);
                    con.Desconectar("ODOO");

                    for (int i = 0; i < solicdet.Rows.Count; i++)
                    {
                        DataRow row = solicdet.Rows[i];

                        int num_lin = i + 1;
                        double cantidad = Convert.ToDouble(row["product_qty"]);

                        string cod_art = "SERVCONTA";
                        string descripcion = Convert.ToString(row["name"]);


                        con.conectar("DM");
                        SqlCommand cmd = new SqlCommand("[CORRECT].[SOLICITUD_COMPRA_DET]", con.condm);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@sig_solic", cod_sc);
                        cmd.Parameters.AddWithValue("@fecha_solicitud", fecha_req);
                        cmd.Parameters.AddWithValue("@articulo", cod_art);
                        cmd.Parameters.AddWithValue("@descripcion", descripcion);
                        cmd.Parameters.AddWithValue("@linea", num_lin);
                        cmd.Parameters.AddWithValue("@cantidad", cantidad);
                        cmd.Parameters.AddWithValue("@centro_costo", centro_costo);
                        cmd.Parameters.AddWithValue("@cuenta_contable", cuenta_contable);
                        cmd.Parameters.AddWithValue("@usuario", usr_solicita);

                        cmd.Parameters.Add("@msg", SqlDbType.VarChar, 100);

                        cmd.Parameters["@msg"].Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();
                        string retunvalue = (string)cmd.Parameters["@msg"].Value;


                        con.Desconectar("DM");

                    }

                }
            }
        


            

         }

        private void button4_Click(object sender, EventArgs e)
        {
            update_sc_odoo("SC0000090", "4", DateTime.Now);
        }

        private Tuple<bool, bool> contable(string cuenta, string centro_costo)
        {
            bool cuet = true;
            bool cen_c = true;
            con.conectar("EX");


            SqlCommand cmd = new SqlCommand("SELECT COUNT([CUENTA_CONTABLE]) FROM [EXACTUS].[dismo].[CUENTA_CONTABLE]  where CUENTA_CONTABLE = @cuenta", con.conex);
            cmd.Parameters.AddWithValue("cuenta", cuenta);            
            int contar = Convert.ToInt32(cmd.ExecuteScalar());           

            if (contar > 0)
            {
                cuet = false;
            }



            SqlCommand cmd2 = new SqlCommand("SELECT COUNT([CENTRO_COSTO]) FROM [EXACTUS].[dismo].[CENTRO_COSTO] where CENTRO_COSTO = @centroc", con.conex);
            cmd2.Parameters.AddWithValue("centroc", centro_costo);
            int contarc = Convert.ToInt32(cmd2.ExecuteScalar());

            if (contarc > 0)
            {
                cen_c = false;
            }



            con.Desconectar("EX");



            return Tuple.Create(cuet, cen_c);

        }

        private string dpto_id(string depart)
        {
            string dpt = "";

            con.conectar("EX");

            SqlCommand dptcmd = new SqlCommand("SELECT [DEPARTAMENTO] FROM [EXACTUS].[dismo].[DEPARTAMENTO] where U_ODOOID = '"+depart+"'", con.conex);
            SqlDataReader drdpt = dptcmd.ExecuteReader();

            while (drdpt.Read())
            {
                dpt = Convert.ToString(drdpt["DEPARTAMENTO"]);
            }

            drdpt.Close();

            con.Desconectar("EX");


            return dpt;



        }

        private Tuple<string,string> usuarios_ex(string u_sol,string u_ap)
        {
            string usuario_so = "", usuario_ap= "";

            con.conectar("DM");


            SqlCommand uscmd = new SqlCommand("SELECT [USUARIO] FROM [DM].[CORRECT].[USUARIOS] where [ODOO_ID] = '"+u_sol+"'", con.condm);
            SqlDataReader usdr= uscmd.ExecuteReader();

            while (usdr.Read())
            {
                usuario_so = Convert.ToString(usdr["USUARIO"]);

            }

            usdr.Close();

            SqlCommand uapcmd = new SqlCommand("SELECT [USUARIO] FROM [DM].[CORRECT].[USUARIOS] where [ODOO_ID] = '" + u_ap + "'", con.condm);
            SqlDataReader uapdr = uapcmd.ExecuteReader();

            while (uapdr.Read())
            {
                usuario_ap = Convert.ToString(uapdr["USUARIO"]);

            }

            uapdr.Close();


            con.Desconectar("DM");



            return Tuple.Create(usuario_so, usuario_ap);

        }

        private void update_sc_odoo(string cod_sc , string id,DateTime fecha)
        {
            con.conectar("ODOO");
          
            NpgsqlCommand comand = new NpgsqlCommand("UPDATE public.sprogroup_purchase_request SET softland_sc='"+cod_sc+"',fecha_softland_sc ='"+fecha.ToString("yyyy-MM-dd")+"' where id = "+id+"; ", con.pgcon);
            comand.ExecuteNonQuery();
            con.Desconectar("ODOO");
          
        }

    }
}
