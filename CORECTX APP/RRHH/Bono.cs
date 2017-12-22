using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Sinconizacion_EXactus.CORECTX_APP.RRHH
{
    public partial class Bono : Form
    {
        public Bono(string codigo_emp, int COD_EV, int tipo_consulta, string NOM_EMP)
        {
            InitializeComponent();
            cod_emp = codigo_emp;
            Cod_Ev = COD_EV;
            tipo_consult = tipo_consulta;
            nom_emp = NOM_EMP;
        }

        Int32 tipo_consult;
        Int32 cant_amones;
        String cod_emp;
        Int32 Cod_Ev;
        string nom_emp;
        decimal prom_Total;
        conexionXML con = new conexionXML();
        DataTable Amonestaciones = new DataTable();
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            numericUpDown1.Value = trackBar1.Value;
            promedio();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            numericUpDown2.Value = trackBar2.Value;
            promedio();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            numericUpDown3.Value = trackBar3.Value;
            promedio();
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            numericUpDown4.Value = trackBar4.Value;
            promedio();
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            numericUpDown5.Value = trackBar5.Value;
            promedio();
        }

        private void trackBar6_Scroll(object sender, EventArgs e)
        {
            numericUpDown6.Value = trackBar6.Value;
            promedio();
        }

        private void promedio()
        {
            decimal promedio = numericUpDown1.Value + numericUpDown2.Value + numericUpDown3.Value + numericUpDown4.Value + numericUpDown5.Value + numericUpDown6.Value;

            prom_Total = promedio / 6;

            label7.Text = Convert.ToString(Math.Round(prom_Total, 2)) + " %";

            if (prom_Total > 0)
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }

        private void Bono_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;

            con.conectar("DM");

            string consulta = "SELECT TIPO_ACCION,NOTAS,FECHA_RIGE FROM [EXACTUS].[dismo].[EMPLEADO_ACC_PER] where EMPLEADO = '" + cod_emp + "' and (TIPO_ACCION like 'AGRA' OR  TIPO_ACCION like 'ALEV') and ESTADO_ACCION = 'S' ";
            SqlCommand comando = new SqlCommand(consulta, con.condm);

            SqlDataAdapter adap = new SqlDataAdapter(comando);

            adap.Fill(Amonestaciones);
            conteo_amonestaciones(Amonestaciones);


            con.Desconectar("DM");
            cant_amones = Convert.ToInt32(linkLabel1.Text) + Convert.ToInt32(linkLabel2.Text);

            if (cant_amones > 0)
            {
                label10.Text = nom_emp + " \n Cuenta con " + cant_amones + " Amonestaciones No se permite calificacion para Bono ";
                deshabilitar();
            }

        }
        private void conteo_amonestaciones(DataTable dt)
        {
            int count_alev = dt.AsEnumerable()
               .Count(row => row.Field<string>("TIPO_ACCION") == "ALEV");

            int count_agra = dt.AsEnumerable()
               .Count(row => row.Field<string>("TIPO_ACCION") == "AGRA");

            linkLabel1.Text = Convert.ToString(count_alev);
            linkLabel2.Text = Convert.ToString(count_agra);


            if (linkLabel1.Text == "0")
            {
                linkLabel1.Enabled = false;
            }

            else
            {
                linkLabel1.Enabled = true;
            }


            if (linkLabel2.Text == "0")
            {
                linkLabel2.Enabled = false;
            }

            else
            {
                linkLabel2.Enabled = true;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fecha = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            con.conectar("DM");

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con.condm;
            cmd.CommandText = "INSERT INTO [DM].[CORRECT].[EVALUACION_BONO] ([ID_EVALUACION],[MOTIVACION],[PUNTUALIDAD],[PRODUCTIVIDAD],[SOBRE_ESFUERZO],[IDENTIDAD_INSTITUCIONAL],[SERVICIO_SOPORTE],[PROMEDIO_FINAL],[AMONESTACION_LEV],[AMONESTACION_GRA],[FECHA_INGRES],[USUARO_INGRESA]) values (@ID_EVALUACION,@MOTIVACION,@PUNTUALIDAD,@PRODUCTIVIDAD,@SOBRE_ESFUERZO,@IDENTIDAD_INSTITUCIONAL,@SERVICIO_SOPORTE,@PROMEDIO_FINAL,@AMONESTACION_LEV,@AMONESTACION_GRA,@FECHA_INGRES,@USUARO_INGRESA)";
            cmd.Parameters.Add("@ID_EVALUACION", SqlDbType.NVarChar).Value = Cod_Ev;
            cmd.Parameters.Add("@MOTIVACION", SqlDbType.Int).Value = numericUpDown1.Value;
            cmd.Parameters.Add("@PUNTUALIDAD", SqlDbType.Int).Value = numericUpDown2.Value;
            cmd.Parameters.Add("@PRODUCTIVIDAD", SqlDbType.Int).Value = numericUpDown3.Value;
            cmd.Parameters.Add("@SOBRE_ESFUERZO", SqlDbType.Int).Value = numericUpDown4.Value;
            cmd.Parameters.Add("@IDENTIDAD_INSTITUCIONAL", SqlDbType.Int).Value = numericUpDown5.Value;
            cmd.Parameters.Add("@SERVICIO_SOPORTE", SqlDbType.Int).Value = numericUpDown6.Value;
            cmd.Parameters.Add("@PROMEDIO_FINAL", SqlDbType.Decimal).Value = prom_Total;
            cmd.Parameters.Add("@AMONESTACION_LEV", SqlDbType.Int).Value = linkLabel1.Text;
            cmd.Parameters.Add("@AMONESTACION_GRA", SqlDbType.Int).Value = linkLabel2.Text;
            cmd.Parameters.Add("@FECHA_INGRES", SqlDbType.DateTime).Value = fecha;
            cmd.Parameters.Add("@USUARO_INGRESA", SqlDbType.NVarChar).Value = Login.usuario.ToUpper();


            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            //coned.con.Close();
            con.Desconectar("DM");

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            trackBar1.Value = Convert.ToInt32(numericUpDown1.Value);
            promedio();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            trackBar2.Value = Convert.ToInt32(numericUpDown2.Value);
            promedio();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            trackBar3.Value = Convert.ToInt32(numericUpDown3.Value);
            promedio();
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            trackBar4.Value = Convert.ToInt32(numericUpDown4.Value);
            promedio();
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            trackBar5.Value = Convert.ToInt32(numericUpDown5.Value);
            promedio();
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            trackBar6.Value = Convert.ToInt32(numericUpDown6.Value);
            promedio();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RRHH.Amonestaciones amon = new RRHH.Amonestaciones(Amonestaciones);

            amon.ShowDialog();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RRHH.Amonestaciones amon = new RRHH.Amonestaciones(Amonestaciones);

            amon.ShowDialog();

        }

        private void deshabilitar()
        {

            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            groupBox4.Enabled = false;
            groupBox5.Enabled = false;
            groupBox6.Enabled = false;
            button1.Enabled = false;


        }
    }
}
