using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Windows.Controls;
using System.Data.SqlClient;

namespace Sinconizacion_EXactus.CORECTX_APP.RRHH
{
    public partial class Reportes_Evaluaciones : Form
    {
        public Reportes_Evaluaciones()
        {
            InitializeComponent();
            exp.OnProgressUpdate += exp_Process;
        }
        Exportador exp = new Exportador();
        DataTable EVALUACIONES = new DataTable();
        DataTable PASO3 = new DataTable();
        DataTable PASO1 = new DataTable();
        DataTable ESTRATEGIA = new DataTable();
        DataTable RETROALIMENACION = new DataTable();
        DataSet ds = new DataSet();
        conexionXML con = new conexionXML();

        private void exp_Process(int value)
        {
            base.Invoke((Action)delegate
            {
                int per = (value + 1) * 100 / ((EVALUACIONES.Rows.Count + PASO1.Rows.Count +PASO3.Rows.Count));
                //label4.Text = "Cargando Registros  " + Convert.ToString(value + 1);
                progressBar1.Value = per;
            });

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Reportes_Evaluaciones_Load(object sender, EventArgs e)
        {
                textBox1.Enabled = false;
                textBox2.Enabled =false;
                
                button3.Enabled = false;
                textBox2.AutoCompleteCustomSource = AutocompleteRuta.AutocompleteEMPLEADOCOD();
                textBox2.AutoCompleteMode = AutoCompleteMode.Suggest;
                textBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;

                textBox1.AutoCompleteCustomSource = AutocompleteRuta.AutocompleteEMPLEADONOMBRE();
                textBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
                textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
           

                for (int i = 2015; i <= DateTime.UtcNow.Year; i++)
                {
                    comboBox1.Items.Add(i.ToString());
                }

            comboBox1.Text = DateTime.UtcNow.Year.ToString();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;

         
            }
            else
            {
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox1.Text = "";
                textBox2.Text = "";

            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button3.Enabled = true;
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            { }
            else
            {
                con.conectar("DM");
                SqlCommand cm1 = new SqlCommand("SELECT  A.[NOMBRE] FROM [EXACTUS].[dismo].[EMPLEADO] A  INNER JOIN [EXACTUS].[dismo].[PUESTO] B  ON A.PUESTO = B.PUESTO INNER JOIN  [EXACTUS].[dismo].[DEPARTAMENTO] C  on A.DEPARTAMENTO = C.DEPARTAMENTO  where A.EMPLEADO = '" + textBox2.Text + "'", con.condm);
                SqlDataReader dr1 = cm1.ExecuteReader();
                while (dr1.Read())
                {
                    String Nombre_Empleado = Convert.ToString(dr1["NOMBRE"]);


                    textBox1.Text = Nombre_Empleado;


                }
                con.Desconectar("DM");
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            { }
            else
            {
                con.conectar("DM");
                SqlCommand cm1 = new SqlCommand("SELECT  A.[EMPLEADO] FROM [EXACTUS].[dismo].[EMPLEADO] A  INNER JOIN [EXACTUS].[dismo].[PUESTO] B  ON A.PUESTO = B.PUESTO INNER JOIN  [EXACTUS].[dismo].[DEPARTAMENTO] C  on A.DEPARTAMENTO = C.DEPARTAMENTO  where A.NOMBRE = '" + textBox1.Text + "'", con.condm);
                SqlDataReader dr1 = cm1.ExecuteReader();
                while (dr1.Read())
                {
                    String Codigo = Convert.ToString(dr1["EMPLEADO"]);


                    textBox2.Text = Codigo;


                }
                con.Desconectar("DM");
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            textBox2.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //refresh
            this.Refresh();
            if (checkBox1.Checked)
            {
                if (textBox2.Text == "")
                {
                    MessageBox.Show("Debe Especificar un Empleado");
                    textBox2.Focus();
                }
                else
                {
                    cargardt();
                    backgroundReport.RunWorkerAsync();

                }

            }
            else
            {
                cargardt();
                backgroundReport.RunWorkerAsync();
            }


           
        }

        private void cargardt()
        {
            if (ds.Tables.Contains("EVALUACIONES"))
            {
                ds.Tables.Remove(EVALUACIONES);
            }

            if (ds.Tables.Contains("PASO3"))
            {
                ds.Tables.Remove(PASO3);
            }

            if (ds.Tables.Contains("PASO1"))
            {
                ds.Tables.Remove(PASO1);
            }

            if (ds.Tables.Contains("ESTRATEGIA"))
            {
                ds.Tables.Remove(ESTRATEGIA);
            }
            if (ds.Tables.Contains("RETROALIMENACION"))
            {
                ds.Tables.Remove(RETROALIMENACION);
            }

            EVALUACIONES.Clear();
            EVALUACIONES.Columns.Clear();
            con.conectar("DM");


            SqlCommand cmd = new SqlCommand("[CORRECT].[REPORTE_EVALUACIONES]", con.condm);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 50;

            cmd.Parameters.AddWithValue("@año", comboBox1.Text);

            if (radioButton1.Checked)
            {
                cmd.Parameters.AddWithValue("@EV_Estado", "Preliminar");
                exp.NombreReporte = "EVALUACION DESEMPEÑO PRELIMINAR ";
            }
            else if (radioButton2.Checked)
            {

                cmd.Parameters.AddWithValue("@EV_Estado", "Final");
                exp.NombreReporte = "EVALUACION DESEMPEÑO FINAL ";
            }

            else if (radioButton3.Checked)
            {
                cmd.Parameters.AddWithValue("@EV_Estado", null);
                exp.NombreReporte = "EVALUACION DESEMPEÑO ";
            }

            if (checkBox1.Checked)
            {
                cmd.Parameters.AddWithValue("@cod_empleado", textBox2.Text);

            }
            else
            {
                cmd.Parameters.AddWithValue("@cod_empleado", null);
            }


            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(EVALUACIONES);
            

            if (checkBox2.Checked)
            {
                // ESTRATEGIA ==============================================

                ESTRATEGIA.Clear();
                ESTRATEGIA.Columns.Clear();

                SqlCommand cmd3 = new SqlCommand("[CORRECT].[REPORTE_ESTRATEGIA]", con.condm);
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.CommandTimeout = 50;

                cmd3.Parameters.AddWithValue("@año", comboBox1.Text);

                if (checkBox1.Checked)
                {
                    cmd3.Parameters.AddWithValue("@cod_empleado", textBox2.Text);

                }
                else
                {
                    cmd3.Parameters.AddWithValue("@cod_empleado", null);
                }


                cmd3.ExecuteNonQuery();
                SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                da3.Fill(ESTRATEGIA);

                ESTRATEGIA.TableName = "ESTRATEGIA";

                ds.Tables.Add(ESTRATEGIA);

        // RETROALIMENTACION ======================================================================

                RETROALIMENACION.Clear();
                RETROALIMENACION.Columns.Clear();

                SqlCommand cmd4 = new SqlCommand("[CORRECT].[REPORTE_RETROALIMENTACION]", con.condm);
                cmd4.CommandType = CommandType.StoredProcedure;
                cmd4.CommandTimeout = 50;

                cmd4.Parameters.AddWithValue("@año", comboBox1.Text);

                if (checkBox1.Checked)
                {
                    cmd4.Parameters.AddWithValue("@cod_empleado", textBox2.Text);

                }
                else
                {
                    cmd4.Parameters.AddWithValue("@cod_empleado", null);
                }


                cmd4.ExecuteNonQuery();
                SqlDataAdapter da4 = new SqlDataAdapter(cmd4);
                da4.Fill(RETROALIMENACION);

                RETROALIMENACION.TableName = "RETROALIMENACION";

                ds.Tables.Add(RETROALIMENACION);

        // PASO 3 ===================================================================
                PASO3.Clear();
                PASO3.Columns.Clear();

                SqlCommand cmd1 = new SqlCommand("[CORRECT].[REPORTE_EVALUACIONES_PASO3]", con.condm);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandTimeout = 50;

                cmd1.Parameters.AddWithValue("@año", comboBox1.Text);

                if (checkBox1.Checked)
                {
                    cmd1.Parameters.AddWithValue("@cod_empleado", textBox2.Text);

                }
                else
                {
                    cmd1.Parameters.AddWithValue("@cod_empleado", null);
                }


                cmd1.ExecuteNonQuery();
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                da1.Fill(PASO3);

                PASO3.TableName = "PASO3";

                ds.Tables.Add(PASO3);


        // PASO 1  ==============================================

                PASO1.Clear();
                PASO1.Columns.Clear();

                SqlCommand cmd2 = new SqlCommand("[CORRECT].[REPORTE_EVALUACIONES_PASO1]", con.condm);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.CommandTimeout = 50;

                cmd2.Parameters.AddWithValue("@año", comboBox1.Text);

                if (checkBox1.Checked)
                {
                    cmd2.Parameters.AddWithValue("@cod_empleado", textBox2.Text);

                }
                else
                {
                    cmd2.Parameters.AddWithValue("@cod_empleado", null);
                }


                cmd2.ExecuteNonQuery();
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                da2.Fill(PASO1);

                PASO1.TableName = "PASO1";

                ds.Tables.Add(PASO1);







            }


            EVALUACIONES.TableName = "EVALUACIONES";
            ds.Tables.Add(EVALUACIONES);
           
           
            con.Desconectar("DM");        

            

        
        
        }

        private void backgroundReport_DoWork(object sender, DoWorkEventArgs e)
        {
            exp.aExcel(ds);
        }
    }
}
