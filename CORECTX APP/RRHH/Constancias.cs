using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Reporting.WinForms;


namespace Sinconizacion_EXactus.CORECTX_APP.RRHH
{
    public partial class Constancias : Form
    {
        public Constancias()
        {
            InitializeComponent();
        }

        conexionXML con = new conexionXML();
        string NOMINA;
        string EMPLEADO;
        double isss;
        Double afp;
        double renta;
        public static DataTable ACCESOS = new DataTable();

        private void button1_Click(object sender, EventArgs e)
        {
            this.reportViewer1.Reset();
            this.recursos_Humanos.CONSTANCIAS_S.Clear();
            this.recursos_Humanos.CONSTANCIAS_DES.Clear();
            this.recursos_Humanos.CARTA_LABORES.Clear();
            NOMINA = "";
            EMPLEADO = "";

            if (radioButton1.Checked)
            {
                this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                this.reportViewer1.LocalReport.ReportPath = @"C:\CORRECT\CORECTX APP\RRHH\Constancia.rdlc";
                ReportDataSource dts1 = new ReportDataSource();

                dts1.Name = "CONSTANCIAS";
                dts1.Value = recursos_Humanos.CONSTANCIAS_S;

                ReportDataSource dts2 = new ReportDataSource();
                dts2.Name = "CONSTANCIAS_DES";
                dts2.Value = recursos_Humanos.CONSTANCIAS_DES;

                this.reportViewer1.LocalReport.DataSources.Add(dts1);
                this.reportViewer1.LocalReport.DataSources.Add(dts2);






                if (textBox1.Text == "" || textBox1.Text == null)
                { }
                else
                {
                    con.conectar("DM");

                    SqlCommand cmd = new SqlCommand("[CORRECT].[CONSTANCIAS_S]", con.condm);
                    // SqlCommand cmd = new SqlCommand("SELECT [NOMBRE] ,[CARNET] ,[TARJETA],[FECHA],[ENTRADA] as 'INGRESO',[SALIDA],[EMPRESA],[PUESTO] FROM [DM].[CORRECT].[TEMP.MARCACION] order by FECHA", con.condm);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NOMINA", null);
                    cmd.Parameters.AddWithValue("@EMPLEADO", textBox1.Text);
                    cmd.Parameters.AddWithValue("@VIATICO", null);
                    cmd.Parameters.AddWithValue("@VARIABLE", null);
                    cmd.Parameters.AddWithValue("@TOTAL", null);
                    cmd.Parameters.AddWithValue("@TOTAL_VAR", null);
                    cmd.Parameters.AddWithValue("@SALARIO", null);
                    cmd.Parameters.AddWithValue("@NOMINADES", null);



                    //cmd.Parameters.AddWithValue("@USUARIO", Usuario);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(this.recursos_Humanos.CONSTANCIAS_S);


                    if (this.recursos_Humanos.CONSTANCIAS_S.Rows.Count >= 1)
                    {
                        DataRow[] result = this.recursos_Humanos.CONSTANCIAS_S.Select("EMPLEADO = '" + textBox1.Text + "'");
                        foreach (DataRow row in result)
                        {
                            NOMINA = row["NOMINADES"].ToString();
                            EMPLEADO = row["EMPLEADO"].ToString();
                        }




                        SqlCommand cmd1 = new SqlCommand("[CORRECT].[CONSTANCIAS_DES]", con.condm);
                        // SqlCommand cmd = new SqlCommand("SELECT [NOMBRE] ,[CARNET] ,[TARJETA],[FECHA],[ENTRADA] as 'INGRESO',[SALIDA],[EMPRESA],[PUESTO] FROM [DM].[CORRECT].[TEMP.MARCACION] order by FECHA", con.condm);
                        cmd1.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@NOMINA", NOMINA);
                        cmd1.Parameters.AddWithValue("@EMPLEADO", EMPLEADO);


                        //cmd.Parameters.AddWithValue("@USUARIO", Usuario);
                        SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                        da1.Fill(this.recursos_Humanos.CONSTANCIAS_DES);

                    }
                    con.Desconectar("DM");
                }
            }
            else if (radioButton2.Checked)
            {
                this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                this.reportViewer1.LocalReport.ReportPath = @"C:\CORRECT\CORECTX APP\RRHH\Carta_Laboral.rdlc";
                ReportDataSource dts1 = new ReportDataSource();
                dts1.Name = "CARTA_LABORAL";
                dts1.Value = recursos_Humanos.CARTA_LABORES;

                this.reportViewer1.LocalReport.DataSources.Add(dts1);


                con.conectar("DM");

                SqlCommand cmd = new SqlCommand("[CORRECT].[CARTA_LABORES]", con.condm);
                // SqlCommand cmd = new SqlCommand("SELECT [NOMBRE] ,[CARNET] ,[TARJETA],[FECHA],[ENTRADA] as 'INGRESO',[SALIDA],[EMPRESA],[PUESTO] FROM [DM].[CORRECT].[TEMP.MARCACION] order by FECHA", con.condm);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;               
                cmd.Parameters.AddWithValue("@EMPLEADO", textBox1.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(this.recursos_Humanos.CARTA_LABORES);

                con.Desconectar("DM");





            }




            //ReportParameter[] param = new ReportParameter[3];
            //param[0] = new ReportParameter("ISSS", isss);
            //param[1] = new ReportParameter("AFP", afp);
            //param[2] = new ReportParameter("RENTA", renta);


            //reportViewer1.LocalReport.SetParameters(param);
            this.reportViewer1.RefreshReport();

            reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
            reportViewer1.ZoomPercent = 150;





        }


       



                
            
           


        

        private void Constancias_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
            con.conectar("EX");
            SqlCommand cm1 = new SqlCommand("SELECT [NOMBRE] FROM [EXACTUS].[dismo].[EMPLEADO] where ACTIVO = 'S' ", con.conex);
            SqlDataAdapter da = new SqlDataAdapter(cm1);
            da.Fill(ACCESOS);
            con.Desconectar("EX");

            textBox2.AutoCompleteCustomSource = Constancias.AutocompleteNOMBRE();
            textBox2.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;


            this.reportViewer1.ProcessingMode =
             Microsoft.Reporting.WinForms.ProcessingMode.Local;
            this.reportViewer1.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.Percent;
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
                con.conectar("EX");
                SqlCommand cm1 = new SqlCommand("SELECT [EMPLEADO] FROM [EXACTUS].[dismo].[EMPLEADO] where NOMBRE =  '" + textBox2.Text + "'", con.conex);
                SqlDataReader dr1 = cm1.ExecuteReader();
                while (dr1.Read())
                {
                    string ID_EMPLEADO = Convert.ToString(dr1["EMPLEADO"]);

                    textBox1.Text = ID_EMPLEADO;

                }
                con.Desconectar("EX");

        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            con.conectar("EX");
            SqlCommand cm1 = new SqlCommand("SELECT [NOMBRE] FROM [EXACTUS].[dismo].[EMPLEADO] where EMPLEADO =  '" + textBox1.Text + "'", con.conex);
            SqlDataReader dr1 = cm1.ExecuteReader();
            while (dr1.Read())
            {
                string EMPLEADO = Convert.ToString(dr1["NOMBRE"]);

                textBox2.Text = EMPLEADO.ToUpper();

            }
            con.Desconectar("EX");
        }
        private static AutoCompleteStringCollection AutocompleteNOMBRE()
        {


            AutoCompleteStringCollection coleccion = new AutoCompleteStringCollection();
            //recorrer y cargar los items para el autocompletado
            foreach (DataRow row in ACCESOS.Rows)
            {
                coleccion.Add(Convert.ToString(row["NOMBRE"]));
            }

            return coleccion;
        }

       
    }
}
