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
        public Cambio(string cod_cliente)
        {
            InitializeComponent();
            cod_cli = cod_cliente;
        }
        String cod_cli;
        conexionXML con = new conexionXML();
        string Ruta;
        string semana;
        string dia;
        string orden;
        string empresa;
        string Agente;
        string Agente2;
        string Vendedor;
        string ultimo_consecutibo;

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
                semana = comboBox3.Text;
                ultimo_orden(Ruta, semana, dia);
                string fecha = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
                
              //  dia = comboBox2.Text;
              

                if (Exists(cod_cli, Ruta, semana, dia))
                {
                    MessageBox.Show("CLIENTE YA EXSISTE EN ESTE DIA");
                }

                else
                {
                    try
                    {
                        con.conectar("DM");
                        SqlCommand cmd1 = new SqlCommand("UPDATE [DM].[dbo].[RUTERO] SET RUTA = '" + Ruta + "' ,DIA = '" + dia + "',ORDEN = '" + orden + "',UpdatedBy = '" + Login.usuario.ToUpper() + "' ,SEMANA = '" + semana + "',FECHACREA = '" + fecha + "',EMPRESA = '" + empresa + "' where CLIENTE = '" + cod_cli + "'", con.condm);


                        cmd1.ExecuteNonQuery();

                        con.Desconectar("DM");


                        con.conectar("EX");
                        SqlCommand cmd4 = new SqlCommand("UPDATE [EXACTUS].[" + empresa + "].[CLIENTE] SET VENDEDOR='" + Agente + "' WHERE CLIENTE ='" + cod_cli + "'", con.conex);


                        cmd4.ExecuteNonQuery();

                        con.Desconectar("EX");

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
    }
}
