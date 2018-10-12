using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using Microsoft.CSharp;
using Microsoft.Reporting.WinForms;


namespace Sinconizacion_EXactus
{
    public partial class Cobro_bod : Form
    {
        public Cobro_bod()
        {
            InitializeComponent();
        }
        conexionXML con = new conexionXML();
       
        private void Cobro_bod_Load(object sender, EventArgs e)
        {
            

            
            this.reportViewer1.ProcessingMode =
    Microsoft.Reporting.WinForms.ProcessingMode.Local;

           

            con.conectar("EX");


            SqlCommand cm2 = new SqlCommand("SELECT  [RUTA] FROM [EXACTUS].[" + Login.empresa + "].[RUTA]  where RUTA like 'E%' ", con.conex);
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
                comboBox1.Items.Add(dr2["RUTA"]);
            }
            dr2.Close();
            con.Desconectar("EX");
           // this.reportViewer1.RefreshReport();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReporteCC.RECIBOS.Clear();
             
            
            this.reportViewer1.LocalReport.ReportPath = @"C:\CORRECT\CORECTX APP\BODEGA\RECIBOS.rdlc";
           
            
            carga_data();

         



        }
        private void carga_data()
        {

            int cantidad = 0;
            String Ruta = this.comboBox1.Text;
            String fechaini = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
            String fechafin = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
            DateTime fechai = Convert.ToDateTime(fechaini);
            DateTime fechaf = Convert.ToDateTime(fechafin);


            con.conectar("DM");

            // SqlCommand cm2 = new SqlCommand("SELECT A.[RUTA],B.ARTICULO,C.DESCRIPCION,CAST(SUM(B.CANTIDAD) as decimal (18,2)) as 'TOTAL',C.FACTOR_CONVER_6 as FACTOR  FROM [EXACTUS].[" + Login.empresa+"].[FACTURA] as A  INNER JOIN  [EXACTUS].["+Login.empresa+ "].[FACTURA_LINEA] B on A.PEDIDO = B.PEDIDO  INNER JOIN [EXACTUS].[" + Login.empresa +"].[ARTICULO] C  on B.ARTICULO = C.ARTICULO  where  C.TIPO <> 'K' and RUTA = '" + this.comboBox1.Text + "' and A.ANULADA ='N' and DATEADD(dd, 0, DATEDIFF(dd, 0, A.FECHA)) = '" + this.dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' and USUARIO = '" + this.comboBox2.Text + "' Group by  A.[RUTA],B.ARTICULO,C.DESCRIPCION,A.FECHA_HORA,C.FACTOR_CONVER_6,c.CLASIFICACION_1 order by c.CLASIFICACION_1,C.DESCRIPCION", con.conex);
            SqlCommand cm2 = new SqlCommand("[CORRECT].[RECIBOS]", con.condm);
            cm2.CommandType = CommandType.StoredProcedure;
            cm2.Parameters.AddWithValue("@EMPRESA", Login.empresa.ToUpper());
            cm2.Parameters.AddWithValue("@Ruta", comboBox1.Text);
            cm2.Parameters.AddWithValue("@fechaini", this.dateTimePicker1.Value.ToString("yyyy/MM/dd"));
            cm2.Parameters.AddWithValue("@fechafin", this.dateTimePicker2.Value.ToString("yyyy/MM/dd"));


            SqlDataAdapter da = new SqlDataAdapter(cm2);

            //da.TableMappings.Add("Table", "RECIBOS");
            //da.TableMappings.Add("Table1", "DOC_CC");

            da.Fill(ReporteCC.RECIBOS);

            con.Desconectar("DM");

            //System.Drawing.Printing.PageSettings pg = new System.Drawing.Printing.PageSettings();
            //pg.Margins.Top = 10;
            //pg.Margins.Bottom = 0;
            //pg.Margins.Left = 0;
            //pg.Margins.Right = 0;

            ////pg.PaperSize = new System.Drawing.Printing.PaperSize("Recibos", 800, 450);

            //System.Drawing.Printing.PaperSize size = new System.Drawing.Printing.PaperSize();
            //size.PaperName = "Recibos";
            //size.Width = 800;
            //size.Height = 450;
            //pg.PaperSize = size;

            //pg.Landscape = false;


            //this.reportViewer1.SetPageSettings(pg);
           
            this.reportViewer1.RefreshReport();

          

        }

        private void reportViewer1_PrintingBegin(object sender, ReportPrintEventArgs e)
        {
            e.PrinterSettings.DefaultPageSettings.Landscape = false;
        }
    }
}
