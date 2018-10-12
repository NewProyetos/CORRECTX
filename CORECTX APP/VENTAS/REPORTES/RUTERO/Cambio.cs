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
    public partial class Cambio : Form
    {
        public Cambio(string cod_cliente, string ruta_old)
        {
            InitializeComponent();
            cod_cli = cod_cliente;
            Ruta_old = ruta_old;
        }
        String cod_cli;
        conexionXML con = new conexionXML();
        string Ruta;
        string Ruta_old;
        string semana;
        string dia;
        string orden;
        string empresa;
        string Agente;
        string Agente2;
        string Vendedor;
        string ultimo_consecutibo;
        int semanas;
        DataTable dt = new DataTable();

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Cambio_Load(object sender, EventArgs e)
        {
            label5.Text = cod_cli;

            empresa = Login.empresa;
            con.conectar("EX");

            SqlCommand cm1 = new SqlCommand("SELECT A.[RUTA] as 'RUTA' FROM [EXACTUS].[ERPADMIN].[RUTA_ASIGNADA_RT] A  where COMPANIA = '" + empresa + "' order by A.[RUTA] ", con.conex);
            SqlDataReader dr1 = cm1.ExecuteReader();
            while (dr1.Read())
            {
                comboBox1.Items.Add(dr1["RUTA"]);

            }
            dr1.Close();




            con.Desconectar("EX");



        }

        private void button1_Click(object sender, EventArgs e)
        {


            if (comboBox1.Text == "" || comboBox1.Text == null || comboBox1.Text == string.Empty)
            {
                MessageBox.Show("debe seleccionar una ruta");
                comboBox1.Focus();
            }
            else
             if (comboBox2.Text == "" || comboBox2.Text == null || comboBox2.Text == string.Empty)
            {
                MessageBox.Show("debe seleccionar un dia ");
                comboBox2.Focus();
            }

            else if (comboBox3.Text == "" || comboBox3.Text == null || comboBox3.Text == string.Empty)
            {
                MessageBox.Show("debe seleccionar una semana ");
                comboBox3.Focus();
            }


            else
            {


                Ruta = comboBox1.Text;
                if (comboBox3.Text == "AB")
                {
                    semana = "A";
                }
                else
                {
                    semana = comboBox3.Text;
                }
               

                string fecha = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
                Eliminar(cod_cli, Ruta_old);

                //  dia = comboBox2.Text;


                if (Exists(cod_cli, Ruta, semana, dia))
                {
                    MessageBox.Show("CLIENTE YA EXSISTE EN ESTE DIA");
                }

                else
                {
                    try
                    {

                        if (comboBox3.Text == "AB")
                        {
                            semanas = 2;
                        }
                        else
                        {
                            semanas = 1;
                        }

                        for (int i = 0; i < semanas ; i++)
                        {

                            ultimo_orden(Ruta, semana, dia);

                            con.conectar("DM");

                            SqlCommand cmd3 = new SqlCommand("[CORRECT].[RUTERO_INSERT]", con.condm);
                            cmd3.CommandTimeout = 0;
                            cmd3.CommandType = CommandType.StoredProcedure;

                            cmd3.Parameters.AddWithValue("@RUTA", Ruta);
                            cmd3.Parameters.AddWithValue("@CLIENTE", cod_cli);
                            cmd3.Parameters.AddWithValue("@DIA", dia);
                            cmd3.Parameters.AddWithValue("@ORDEN", orden);
                            cmd3.Parameters.AddWithValue("@UpdatedBy", Login.usuario.ToUpper());
                            cmd3.Parameters.AddWithValue("@SEMANA", semana);
                            cmd3.Parameters.AddWithValue("@fecha_crea", fecha);
                            cmd3.Parameters.AddWithValue("@empresa", empresa);


                            cmd3.ExecuteNonQuery();

                            con.Desconectar("DM");


                            con.conectar("EX");
                            SqlCommand cmd4 = new SqlCommand("UPDATE [EXACTUS].[" + empresa + "].[CLIENTE] SET VENDEDOR='" + Agente + "' WHERE CLIENTE ='" + cod_cli + "'", con.conex);


                            cmd4.ExecuteNonQuery();

                            con.Desconectar("EX");

                           

                            if (semanas == 2 && semana == "A")
                            {
                                semana = "B";
                            }

                            

                        }
                        this.Close();
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(Convert.ToString(ex));
                    }

               
             
               }
    
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

              

            }
            dr2.Close();
            con.Desconectar("EX");
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox2.Text)
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
        }

        private void ultimo_orden(string ruta, string semana,string dia)
        {
            con.conectar("EX");
            SqlCommand cm2 = new SqlCommand("SELECT TOP 1 [ORDEN] as ORDEN FROM [DM].[dbo].[RUTERO] where RUTA = '"+ruta+"' and SEMANA = '"+semana+ "'  and DIA = '"+dia+"'  order by ORDEN desc", con.conex);
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
               ultimo_consecutibo = Convert.ToString(dr2["ORDEN"]);
               
                
            }


            if (ultimo_consecutibo == "" || ultimo_consecutibo == null || ultimo_consecutibo == string.Empty)
            { }
            else
            {
                int ordenado = 0;
               ordenado = Convert.ToInt32(ultimo_consecutibo) + 1;
                orden = Convert.ToString(ordenado);
            }

            dr2.Close();
            con.Desconectar("EX");

        }

        private void Eliminar(string COD_CLIE,string RUTA)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("DELETE FROM  [DM].[dbo].[RUTERO]  WHERE RUTA = '" + RUTA + "' and CLIENTE ='" + COD_CLIE + "'", con.condm);

            cmd.ExecuteNonQuery();

            con.Desconectar("DM");

        }
    }
}
