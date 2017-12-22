using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Data.SqlClient;

namespace Sinconizacion_EXactus.CORECTX_APP.CREDITOS.IMPUESTO_DISTRIB
{
    public partial class Reporte_Impuesto_Distribucion : Form
    {
        public Reporte_Impuesto_Distribucion()
        {
            InitializeComponent();
            exp.OnProgressUpdate += exp_Process;
        }
        Exportador exp = new Exportador();
        conexionXML con = new conexionXML();
        DataSet liqds = new DataSet();
        DataTable dt1 = new DataTable();
        DataTable dt2 = new DataTable();
        DataTable dt3 = new DataTable();
        DataTable dt4 = new DataTable();
        DataTable dt5 = new DataTable();

        private void button1_Click(object sender, EventArgs e)
        {

            if (liqds.Tables.Contains("ZONA"))
            {
                liqds.Tables.Remove(dt1);
            }

            if (liqds.Tables.Contains("DEPARTAMENTO"))
            {
                liqds.Tables.Remove(dt2);
            }
            if (liqds.Tables.Contains("MUNICIPIO"))
            {
                liqds.Tables.Remove(dt3);
            }
            if (liqds.Tables.Contains("DEP_MUN_ENT"))
            {
                liqds.Tables.Remove(dt4);
            }
            if (liqds.Tables.Contains("DEP_MUN_ENT_MES_ANO"))
            {
                liqds.Tables.Remove(dt5);
            }
            string fechaini = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string fechafin = dateTimePicker2.Value.ToString("yyyy-MM-dd");

            if (checkBox1.Checked)
            {
                dt1.Clear();
                con.conectar("DM");

                SqlCommand cmd2 = new SqlCommand("[CORRECT].[REPORTE_IMP_DIST]", con.condm);
                cmd2.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dta = new SqlDataAdapter(cmd2);

                cmd2.Parameters.AddWithValue("@TABLA", 4);
                cmd2.Parameters.AddWithValue("@fechaini", fechaini);
                cmd2.Parameters.AddWithValue("@fechafin", fechafin);
                    
                //cmd2.ExecuteNonQuery();
                dta.Fill(dt1);

                con.Desconectar("DM");

                dt1.TableName = "ZONA";

                liqds.Tables.Add(dt1);
            }
            if (checkBox2.Checked)
            {
                dt2.Clear();
                con.conectar("DM");

                SqlCommand cmd2 = new SqlCommand("[CORRECT].[REPORTE_IMP_DIST]", con.condm);
                cmd2.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dta = new SqlDataAdapter(cmd2);

                cmd2.Parameters.AddWithValue("@TABLA", 3);
                cmd2.Parameters.AddWithValue("@fechaini", fechaini);
                cmd2.Parameters.AddWithValue("@fechafin", fechafin);

                //cmd2.ExecuteNonQuery();
                dta.Fill(dt2);
                con.Desconectar("DM");
                dt2.TableName = "DEPARTAMENTO";
                liqds.Tables.Add(dt2);

            }
            if (checkBox3.Checked)
            {
                dt3.Clear();
                con.conectar("DM");

                SqlCommand cmd2 = new SqlCommand("[CORRECT].[REPORTE_IMP_DIST]", con.condm);
                cmd2.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dta = new SqlDataAdapter(cmd2);
                cmd2.Parameters.AddWithValue("@TABLA", 2);
                cmd2.Parameters.AddWithValue("@fechaini", fechaini);
                cmd2.Parameters.AddWithValue("@fechafin", fechafin);

                //cmd2.ExecuteNonQuery();
                dta.Fill(dt3);
                con.Desconectar("DM");
                dt3.TableName = "MUNICIPIO";
                liqds.Tables.Add(dt3);
            }
            if (checkBox4.Checked)
            {
                dt4.Clear();
                con.conectar("DM");

                SqlCommand cmd2 = new SqlCommand("[CORRECT].[REPORTE_IMP_DIST]", con.condm);
                cmd2.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dta = new SqlDataAdapter(cmd2);
                cmd2.Parameters.AddWithValue("@TABLA", 1);
                cmd2.Parameters.AddWithValue("@fechaini", fechaini);
                cmd2.Parameters.AddWithValue("@fechafin", fechafin);

                //cmd2.ExecuteNonQuery();
                dta.Fill(dt4);
                dt4.TableName = "DEP_MUN_ENT";
                con.Desconectar("DM");
                liqds.Tables.Add(dt4);
            
            }

            if (checkBox5.Checked)
            {
                dt5.Clear();
                con.conectar("DM");

                SqlCommand cmd2 = new SqlCommand("[CORRECT].[REPORTE_IMP_DIST]", con.condm);
                cmd2.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dta = new SqlDataAdapter(cmd2);
                cmd2.Parameters.AddWithValue("@TABLA", 5);
                cmd2.Parameters.AddWithValue("@fechaini", fechaini);
                cmd2.Parameters.AddWithValue("@fechafin", fechafin);

                //cmd2.ExecuteNonQuery();
                dta.Fill(dt5);
                dt5.TableName = "DEP_MUN_ENT_MES_ANO";
                con.Desconectar("DM");
                liqds.Tables.Add(dt5);

            }

            backgroundReport.RunWorkerAsync();
            
        }
        private void exp_Process(int value)
        {
            base.Invoke((Action)delegate
            {
                int per = (value + 1) * 100 / ((dt1.Rows.Count + dt2.Rows.Count + dt3.Rows.Count + dt4.Rows.Count+dt5.Rows.Count));
                //label4.Text = "Cargando Registros  " + Convert.ToString(value + 1);
                progressBar1.Value = per;
            });

        }

        private void backgroundReport_DoWork(object sender, DoWorkEventArgs e)
        {

            exp.aExcel(liqds);
        }
    }
}
