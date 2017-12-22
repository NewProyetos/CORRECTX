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

namespace Sinconizacion_EXactus.CORECTX_APP.CREDITOS.LIQUIDACIONES
{
    public partial class LIQUIDACIONES : Form
    {
        public LIQUIDACIONES()
        {
            InitializeComponent();
        }
        public static String RUTA;
        conexionXML con = new conexionXML();
        String Usuario;
        String Acceso_usuarios;
        String Empresa = Login.empresa;
        public static DataTable nc = new DataTable();
        private void LIQUIDACIONES_Load(object sender, EventArgs e)
        {
            groupBox2.Hide();


            Usuario = Login.usuario.ToUpper();
            Acceso_usuarios = Main_Menu.usuario_devol;

            comboBox2.Text = Usuario;
            if (Acceso_usuarios == "S" || Main_Menu.Puesto == "ADMIN")
            {
                comboBox2.Enabled = true;
            }

            else
            {
                comboBox2.Enabled = false;
            }

            this.reportViewer1.ProcessingMode =
          Microsoft.Reporting.WinForms.ProcessingMode.Local;




            con.conectar("EX");
           

            SqlCommand cm2 = new SqlCommand("SELECT  [RUTA] FROM [EXACTUS].["+Empresa+"].[RUTA]  where RUTA like 'E%' ", con.conex);
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
                comboBox1.Items.Add(dr2["RUTA"]);
            }
            dr2.Close();


            SqlCommand cm4 = new SqlCommand("SELECT USUARIO  FROM [EXACTUS].["+Empresa+"].[FACTURA]  Group by USUARIO  ", con.conex);
            SqlDataReader dr4 = cm4.ExecuteReader();
            while (dr4.Read())
            {
                comboBox2.Items.Add(dr4["USUARIO"]);
            }
            dr4.Close();

            con.Desconectar("EX");




            //this.reportViewer1.RefreshReport();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.reportViewer1.LocalReport.ReportPath = @"C:\CORRECT\CORECTX APP\CREDITOS\LIQUIDACIONES\Boleta_liquidacion.rdlc";
            this.reporteCC.LIQUIDACION.Clear();
            
            String Ruta = this.comboBox1.Text;
            String fechaini = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
            String fechafin = this.dateTimePicker2.Value.ToString("yyyy-MM-dd");
            String Usuario = this.comboBox2.Text;
            //DateTime fecha = Convert.ToDateTime(fechast);

            con.conectar("DM");


            SqlCommand cmd = new SqlCommand("[CORRECT].[LIQUIDACION_ENTREGA]", con.condm);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fecha_ini", fechaini);
            cmd.Parameters.AddWithValue("@fecha_fin", fechafin);
            cmd.Parameters.AddWithValue("@ENTREGA", Ruta);
            cmd.Parameters.AddWithValue("@USUARIO", Usuario);
            cmd.Parameters.AddWithValue("@empresa", Empresa);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(reporteCC.LIQUIDACION);


            con.Desconectar("DM");

            ReportParameter[] param = new ReportParameter[1];
            param[0] = new ReportParameter("ENTREGA", comboBox1.Text);
            //param[1] = new ReportParameter("fechaini", this.dateTimePicker1.Value.ToString("dd/MM/yyyy"));
            //param[2] = new ReportParameter("fechafin", this.dateTimePicker2.Value.ToString("dd/MM/yyyy"));


            reportViewer1.LocalReport.SetParameters(param);

            this.reportViewer1.RefreshReport();


            DateTime time = DateTime.Today;

            int dia = Convert.ToInt32(time.DayOfWeek);

            if (dia == 1)
            {
                DateTime dt1 = dateTimePicker1.Value;
                loadnotasdec(dt1.AddDays(-2).ToString("yyyy-MM-dd"), dateTimePicker2.Value.ToString("yyyy-MM-dd"), Ruta);
            }
            else
            {
                DateTime dt1 = dateTimePicker1.Value;
                loadnotasdec(dt1.AddDays(-1).ToString("yyyy-MM-dd"), dateTimePicker2.Value.ToString("yyyy-MM-dd"), Ruta);
            }
            
                linkLabel1.Text ="NC "+Convert.ToString(nc.Rows.Count);
                groupBox2.Show();

           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RUTA = comboBox1.Text;
        }

        private void loadnotasdec(string fechaini, string fechafin, string ruta)
        {
            nc.Clear();
            con.conectar("EX");


            SqlCommand cm4 = new SqlCommand("SELECT [FACTURA] as DEVOLUCION,[FECHA] FROM [EXACTUS].["+Empresa+"].[FACTURA]  where  TIPO_DOCUMENTO = 'D'  and RUTA = '" + ruta + "' and (DATEADD(dd, 0, DATEDIFF(dd, 0, FECHA)) >= '" + fechaini + "') AND (DATEADD(dd, 0, DATEDIFF(dd, 0,FECHA)) <= '" + fechafin + "') and ANULADA = 'N' and COMENTARIO_CXC is null", con.conex);
            SqlDataAdapter da = new SqlDataAdapter(cm4);
            da.Fill(nc);
            con.Desconectar("EX");



        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Notas_Credito nd = new Notas_Credito();
            nd.ShowDialog();
        }
    }
}
