using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.CSharp;
using Microsoft.Reporting.WinForms;

namespace Sinconizacion_EXactus.CORECTX_APP.RRHH
{
    public partial class Boleta_Marcacion : Form
    {
        public Boleta_Marcacion()
        {
            InitializeComponent();
        }
       
        conexionXML con = new conexionXML();
        ReportDataSource reportDataSource = new ReportDataSource();
       public static DataTable ACCESOS = new DataTable();
        private void Boleta_Marcacion_Load(object sender, EventArgs e)
        {

            con.conectar("SEG");


            string consulta = "SELECT UPPER(RTRIM(LTRIM(ISNULL(FIRSTNAME,''))))+' '+ UPPER(RTRIM(LTRIM(isnull(MIDNAME,''))))+' '+UPPER(RTRIM(LTRIM(ISNULL(LASTNAME,''))))  AS NOMBRE  ,[SSNO] AS CARNET  FROM [ACCESSCONTROL].[dbo].[EMP] EMPLE";
            SqlCommand comando = new SqlCommand(consulta, con.conseg);

            SqlDataAdapter adap = new SqlDataAdapter(comando);

            adap.Fill(ACCESOS);
            con.Desconectar("SEG");


            textBox2.AutoCompleteCustomSource = AutocompleteNOMBRE();
            textBox2.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;

            textBox1.AutoCompleteCustomSource = AutocompleteCARNET();
            textBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;

            this.reportViewer1.ProcessingMode =
        Microsoft.Reporting.WinForms.ProcessingMode.Local;
           this. reportViewer1.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.Percent;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.reportViewer1.LocalReport.ReportPath = @"C:\CORRECT\CORECTX APP\RRHH\Boleta_Marcacion.rdlc";
            this.recursos_Humanos.Accesos.Clear();

            String CARNET = this.textBox1.Text;
            String fechaini = this.dateTimePicker1.Value.ToString("yyyy-dd-MM");
            String fechafin = this.dateTimePicker2.Value.ToString("yyyy-dd-MM");
            String Usuario = Login.usuario.ToUpper();
           

            //con.conectar("DM");
            con.conectar("SEG");

            SqlCommand cmd = new SqlCommand("[dbo].[Reporte_Accesos_mod]", con.conseg);
           // SqlCommand cmd = new SqlCommand("SELECT [NOMBRE] ,[CARNET] ,[TARJETA],[FECHA],[ENTRADA] as 'INGRESO',[SALIDA],[EMPRESA],[PUESTO] FROM [DM].[CORRECT].[TEMP.MARCACION] order by FECHA", con.condm);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fechaini", fechaini);
            cmd.Parameters.AddWithValue("@fechafin", fechafin);
            cmd.Parameters.AddWithValue("@carnet", CARNET);
            //cmd.Parameters.AddWithValue("@USUARIO", Usuario);



            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(this.recursos_Humanos.Accesos);


            con.Desconectar("SEG");
            //con.Desconectar("DM");


            ReportParameter[] param = new ReportParameter[3];
            param[0] = new ReportParameter("Usuario", Usuario);
            param[1] = new ReportParameter("fechaini", fechaini);
            param[2] = new ReportParameter("fechafin", fechafin);


            reportViewer1.LocalReport.SetParameters(param);
            this.reportViewer1.RefreshReport();
            
            reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
            reportViewer1.ZoomPercent = 150;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            textBox2.Text = "";
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            textBox1.Text = "";
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            DataRow[] result = ACCESOS.Select("CARNET = '"+ textBox1.Text +"'");
            foreach (DataRow row in result)
            {
                textBox2.Text=row["NOMBRE"].ToString();
            
            }
        }


     

        public static AutoCompleteStringCollection AutocompleteNOMBRE()
        { 
            
            
            AutoCompleteStringCollection coleccion = new AutoCompleteStringCollection();
            //recorrer y cargar los items para el autocompletado
            foreach (DataRow row in ACCESOS.Rows)
            {
                coleccion.Add(Convert.ToString(row["NOMBRE"]));
            }

            return coleccion;
        }

        public static AutoCompleteStringCollection AutocompleteCARNET()
        {
            

            AutoCompleteStringCollection coleccion = new AutoCompleteStringCollection();
            //recorrer y cargar los items para el autocompletado
            foreach (DataRow row in ACCESOS.Rows)
            {
                coleccion.Add(Convert.ToString(row["CARNET"]));
            }

            return coleccion;
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            DataRow[] result = ACCESOS.Select("NOMBRE = '" + textBox2.Text + "'");
            foreach (DataRow row in result)
            {
                textBox1.Text = row["CARNET"].ToString();

            }
        }

    }
}
