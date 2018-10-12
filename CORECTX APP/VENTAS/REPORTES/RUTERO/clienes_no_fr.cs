using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Sinconizacion_EXactus.CORECTX_APP.VENTAS.RUTERO
{
    public partial class clienes_no_fr : Form
    {
        public clienes_no_fr()
        {
            InitializeComponent();
        }
        string nombreC;
        string nombre_cli;
        string cod_clie;
        DataTable dtfull = new DataTable();
        conexionXML con = new conexionXML();
        private void clienes_no_fr_Load(object sender, EventArgs e)
        {
            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            //dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ReadOnly = false;


        }

        private void insert_clientes_ft(string COD_CLIE, string clie_nom)
        {

            if (Exists_FR_cli_rt_(COD_CLIE))
            {



                con.conectar("DM");

                SqlCommand cmd1 = new SqlCommand("[CORRECT].[CREACLIE_FR]", con.condm);
                cmd1.CommandTimeout = 0;
                cmd1.CommandType = CommandType.StoredProcedure;

                cmd1.Parameters.AddWithValue("@TABLA", 1);
                cmd1.Parameters.AddWithValue("@CODCLI", COD_CLIE);
                cmd1.Parameters.AddWithValue("@NOMBRE", clie_nom);
                cmd1.Parameters.AddWithValue("@empresa", Login.empresa);

                cmd1.ExecuteNonQuery();

                con.Desconectar("DM");

            }

            if (Exists_FR_asoc_rt_(COD_CLIE))
            {



                con.conectar("DM");

                SqlCommand cmd2 = new SqlCommand("[CORRECT].[CREACLIE_FR]", con.condm);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.CommandTimeout = 0;
                cmd2.Parameters.AddWithValue("@TABLA", 2);
                cmd2.Parameters.AddWithValue("@CODCLI", COD_CLIE);
                cmd2.Parameters.AddWithValue("@NOMBRE", clie_nom);
                cmd2.Parameters.AddWithValue("@empresa", Login.empresa);

                cmd2.ExecuteNonQuery();

                con.Desconectar("DM");
            }

            if (Exists_clie_DM(COD_CLIE))
            {


                con.conectar("DM");

                SqlCommand cmd2 = new SqlCommand("[CORRECT].[CREACLIE_FR]", con.condm);
                cmd2.CommandType = CommandType.StoredProcedure;

                cmd2.Parameters.AddWithValue("@TABLA", 3);
                cmd2.Parameters.AddWithValue("@CODCLI", COD_CLIE);
                cmd2.Parameters.AddWithValue("@NOMBRE", clie_nom);
                cmd2.Parameters.AddWithValue("@empresa", Login.empresa);

                cmd2.ExecuteNonQuery();

                con.Desconectar("DM");
            }




        }

        private void carga_clientes()
        {
            dtfull.Clear();
            con.conectar("EX");
            SqlCommand cmd2 = new SqlCommand();
            cmd2 = new SqlCommand("SELECT  DISTINCT  RT.[CLIENTE],CLI.NOMBRE  FROM [DM].[dbo].[RUTERO] as RT   LEFT JOIN [EXACTUS].[dismo].[CLIENTE] as CLI  on RT.CLIENTE = CLI.CLIENTE  where RT.CLIENTE NOT IN (SELECT [CLIENTE] FROM [EXACTUS].[ERPADMIN].[CLIENTE_RT])  and EMPRESA = '"+Login.empresa+"'", con.condm);

            SqlDataAdapter da1 = new SqlDataAdapter(cmd2);
            da1.Fill(dtfull);
            con.Desconectar("EX");

            dataGridView1.DataSource = dtfull;

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

        private void button2_Click(object sender, EventArgs e)
        {
            if (dtfull.Rows.Count >= 1)
            {
                for (int i = 0; i < dtfull.Rows.Count; i++)
                {

                    DataRow row = dtfull.Rows[i];
                    cod_clie = row["CLIENTE"].ToString();
                    nombre_cli = row["NOMBRE"].ToString();

                    if (nombre_cli.Length > 79)
                    {
                        nombreC = nombre_cli.Substring(0, 79);
                    }
                    else
                    {
                        nombreC = nombre_cli;
                    }


                    insert_clientes_ft(cod_clie, nombreC);

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            carga_clientes();
        }
    }
}
