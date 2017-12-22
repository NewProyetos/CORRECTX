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
    public partial class Cargas : Form
    {
        public Cargas()
        {
            InitializeComponent();
        }

        conexionXML con = new conexionXML();
        //conexion conet = new conexion();

        DataTable dt = new DataTable();

        public Excel.Application aplicacion;
        public Excel.Workbook libros_trabajo;
        int detalle;
        
        private void Cargas_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = true;

            detalle = 1;

            this.reportViewer1.ProcessingMode =
    Microsoft.Reporting.WinForms.ProcessingMode.Local;

            

            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd-MM-yyyy";
            this.comboBox2.Text = Login.usuario.ToUpper();
             con.conectar("EX");


            SqlCommand cm2 = new SqlCommand("SELECT  [RUTA] FROM [EXACTUS].["+Login.empresa+"].[RUTA]  where RUTA like 'E%' ", con.conex);
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
                comboBox1.Items.Add(dr2["RUTA"]);
            }
            dr2.Close();


            SqlCommand cm4 = new SqlCommand("SELECT USUARIO  FROM [EXACTUS].["+Login.empresa+"].[FACTURA]  Group by USUARIO  ", con.conex);
            SqlDataReader dr4 = cm4.ExecuteReader();
            while (dr4.Read())
            {
                comboBox2.Items.Add(dr4["USUARIO"]);
            }
            dr4.Close();
            
          con.Desconectar("EX");



         
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.reportViewer1.LocalReport.ReportPath = @"C:\CORRECT\CORECTX APP\BODEGA\Reporte Cargas Bodega.rdlc";
            this.ReporteCC.Cargas.Clear();
            this.ReporteCC.Cargas_KIT.Clear();
            this.ReporteCC.CLIENTES_CONTADO.Clear();
            this.ReporteCC.CLIENTES_CREDITO.Clear();
            int cantidad = 0;
            String Ruta = this.comboBox1.Text;
            String fechast = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
            DateTime fecha = Convert.ToDateTime(fechast);
            

          
            //this.CargasTableAdapter.Fill(this.ReporteCC.Cargas,Ruta,fecha);


            con.conectar("EX");


            SqlCommand cm2 = new SqlCommand("SELECT A.[RUTA],B.ARTICULO,C.DESCRIPCION,CAST(SUM(B.CANTIDAD) as decimal (18,2)) as 'TOTAL',C.FACTOR_CONVER_6 as FACTOR  FROM [EXACTUS].[" + Login.empresa+"].[FACTURA] as A  INNER JOIN  [EXACTUS].["+Login.empresa+ "].[FACTURA_LINEA] B on A.PEDIDO = B.PEDIDO  INNER JOIN [EXACTUS].[" + Login.empresa +"].[ARTICULO] C  on B.ARTICULO = C.ARTICULO  where  C.TIPO <> 'K' and RUTA = '" + this.comboBox1.Text + "' and A.ANULADA ='N' and DATEADD(dd, 0, DATEDIFF(dd, 0, A.FECHA)) = '" + this.dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' and USUARIO = '" + this.comboBox2.Text + "' Group by  A.[RUTA],B.ARTICULO,C.DESCRIPCION,A.FECHA_HORA,C.FACTOR_CONVER_6,c.CLASIFICACION_1 order by c.CLASIFICACION_1,C.DESCRIPCION", con.conex);
            SqlDataAdapter da = new SqlDataAdapter(cm2);
            da.Fill(this.ReporteCC.Cargas);

            SqlCommand cm4 = new SqlCommand("SELECT A.[RUTA],B.ARTICULO,C.DESCRIPCION,CAST(SUM(B.CANTIDAD) as decimal (18,2)) as 'TOTAL' FROM [EXACTUS].[" + Login.empresa + "].[FACTURA] as A  INNER JOIN  [EXACTUS].[" + Login.empresa + "].[FACTURA_LINEA] B on A.PEDIDO = B.PEDIDO  INNER JOIN [EXACTUS].[" + Login.empresa +"].[ARTICULO] C  on B.ARTICULO = C.ARTICULO  where  C.TIPO = 'K' and RUTA = '" + this.comboBox1.Text + "' and A.ANULADA ='N' and DATEADD(dd, 0, DATEDIFF(dd, 0, A.FECHA)) = '" + this.dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' and USUARIO = '" + this.comboBox2.Text + "' Group by  A.[RUTA],B.ARTICULO,C.DESCRIPCION,A.FECHA_HORA", con.conex);
            SqlDataAdapter da4 = new SqlDataAdapter(cm4);
            da4.Fill(this.ReporteCC.Cargas_KIT);

            SqlCommand cm5 = new SqlCommand(" SELECT FACTURA as DOCUMENTO ,CLIENTE as CODIGO_CLIENTE,NOMBRE_CLIENTE as NOMBRE ,DIRECCION_FACTURA as DIRECCION,'SS' as DEPARTAMENTO,'SS' as MUNICIPIO, CAST(TOTAL_FACTURA as decimal(18,2)) as MONTO  FROM [EXACTUS].[" + Login.empresa+ "].[FACTURA] as A where A.TIPO_DOCUMENTO = 'F' and A.ANULADA ='N' and CONDICION_PAGO = '01' and A.RUTA = '" + this.comboBox1.Text+ "' and DATEADD(dd, 0, DATEDIFF(dd, 0, A.FECHA)) = '" + this.dateTimePicker1.Value.ToString("yyyy / MM / dd") + "' and A.USUARIO = '"+this.comboBox2.Text+"'", con.conex);
            SqlDataAdapter da5 = new SqlDataAdapter(cm5);
            da5.Fill(this.ReporteCC.CLIENTES_CONTADO);


            SqlCommand cm6 = new SqlCommand(" SELECT FACTURA as DOCUMENTO ,CLIENTE as CODIGO_CLIENTE,NOMBRE_CLIENTE as NOMBRE ,DIRECCION_FACTURA as DIRECCION,'SS' as DEPARTAMENTO,'SS' as MUNICIPIO, CAST(TOTAL_FACTURA as decimal(18,2)) as MONTO  FROM [EXACTUS].[" + Login.empresa + "].[FACTURA] as A where A.TIPO_DOCUMENTO = 'F' and A.ANULADA ='N' and CONDICION_PAGO <> '01' and A.RUTA = '" + this.comboBox1.Text + "' and DATEADD(dd, 0, DATEDIFF(dd, 0, A.FECHA)) = '" + this.dateTimePicker1.Value.ToString("yyyy / MM / dd") + "' and A.USUARIO = '" + this.comboBox2.Text + "'", con.conex);
            SqlDataAdapter da6 = new SqlDataAdapter(cm6);
            da6.Fill(this.ReporteCC.CLIENTES_CREDITO);


            SqlCommand cm3 = new SqlCommand("SELECT COUNT(FACTURA) as cantidad  FROM [EXACTUS].[" + Login.empresa +"].[FACTURA]  WHERE RUTA = '" + this.comboBox1.Text + "' and ANULADA = 'N' and  DATEADD(dd, 0, DATEDIFF(dd, 0, FECHA)) = '" + this.dateTimePicker1.Value.ToString("yyyy/MM/dd") + "'  and USUARIO = '"+this.comboBox2.Text+"'", con.conex);            
            SqlDataReader dr3 = cm3.ExecuteReader();
            while (dr3.Read())
            {
              cantidad=Convert.ToInt32(dr3["cantidad"]);
            }
            dr3.Close();


            con.Desconectar("EX");


           

            



            ReportParameter[] param = new ReportParameter[3];
            param[0] = new ReportParameter("Rutatxt", Ruta);
            param[1] = new ReportParameter("Cantxt", Convert.ToString(cantidad));
            param[2] = new ReportParameter("detalle", Convert.ToString(detalle));
            reportViewer1.LocalReport.SetParameters(param);

            this.reportViewer1.RefreshReport();
          //  dt.Clear();

            
          //  if (this.comboBox1.Text == "")
          //  {
          //      MessageBox.Show("Seleccione una Ruta");
          //  }
          //  else
          //  {
          //   con.conectar("EX");

          //  SqlCommand cmd = new SqlCommand("SELECT A.[RUTA],B.ARTICULO,C.DESCRIPCION,CAST(SUM(B.CANTIDAD_PEDIDA) as decimal (18,2)) as 'TOTAL' FROM [EXACTUS].[dismo].[PEDIDO] as A  INNER JOIN  [EXACTUS].[dismo].[PEDIDO_LINEA] B on A.PEDIDO = B.PEDIDO  INNER JOIN [EXACTUS].[dismo].[ARTICULO] C  on B.ARTICULO = C.ARTICULO  where RUTA = '"+this.comboBox1.Text+"' and A.ESTADO ='F' and DATEADD(dd, 0, DATEDIFF(dd, 0, A.FECHA_PEDIDO)) = '" + this.dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' Group by  A.[RUTA],B.ARTICULO,C.DESCRIPCION,A.FECHA_PEDIDO", con.conex);
          //  SqlDataAdapter da = new SqlDataAdapter(cmd);
          //  da.Fill(dt);

          //con.Desconectar("EX");




          //  Excel.Worksheet hoja_trabajo;

          //  Excel.Range rango;
          //  Excel.Range rango_enc;


          //  aplicacion = new Excel.Application();
          //  libros_trabajo = aplicacion.Workbooks.Add();

          //  hoja_trabajo = (Excel.Worksheet)libros_trabajo.Worksheets.get_Item(1);

          //  hoja_trabajo.Name = "CARGA";

          //  rango = hoja_trabajo.get_Range("A1", "F1");

          //  rango_enc = hoja_trabajo.get_Range("C3", "F3");
          //  rango_enc.Font.Name = "Times New Roman";
          //  rango_enc.Font.Size = 10;
          //  rango_enc.Borders.LineStyle = Excel.XlLineStyle.xlDouble;


          //  rango.Font.Name = "Times New Roman";
          //  rango.Font.Color = Color.Blue;
          //  rango.Font.Size = 15;


          //  rango_enc.Font.Bold = true;
          //  rango.Font.Bold = true;

          //  hoja_trabajo.Cells[1, 2] = "CARGA DE RUTA " + this.comboBox1.Text + "  FECHA " + this.dateTimePicker1.Value.ToString("dd-MM-yyyy") + "";



          //  hoja_trabajo.Cells[3, 3] = Convert.ToString(dt.Columns[0].ColumnName);
          //  hoja_trabajo.Cells[3, 4] = Convert.ToString(dt.Columns[1].ColumnName);
          //  hoja_trabajo.Cells[3, 5] = Convert.ToString(dt.Columns[2].ColumnName);
          //  hoja_trabajo.Cells[3, 6] = Convert.ToString(dt.Columns[3].ColumnName);
           


          //  for (int i = 0; i < dt.Rows.Count; i++)
          //  {

          //      for (int j = 0; j < dt.Columns.Count; j++)
          //      {


          //          hoja_trabajo.Cells[i + 4, j + 3] = dt.Rows[i][j].ToString();
          //      }
          //  }



          //  aplicacion.Visible = true;

          //  }



        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                detalle = 1;
            }
            else
            {
                detalle = 0;
            }
        }
    }
}
