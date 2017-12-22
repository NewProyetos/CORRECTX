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

namespace Sinconizacion_EXactus.CORECTX_APP.RRHH
{
    public partial class Reporte_Evaluaciones : Form
    {
        public Reporte_Evaluaciones()
        {
            InitializeComponent();
        }

        conexionXML con = new conexionXML();
        private void Reporte_Evaluaciones_Load(object sender, EventArgs e)
        {
            

            this.reportViewer1.RefreshReport();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Sinconizacion_EXactus.Recursos_Humanos.EVALUACION_DESEMPEÑODataTable  EV = new Recursos_Humanos.EVALUACION_DESEMPEÑODataTable();

            con.conectar("EX");


           // SqlCommand cm2 = new SqlCommand("SELECT A.[RUTA],B.ARTICULO,C.DESCRIPCION,CAST(SUM(B.CANTIDAD) as decimal (18,2)) as 'TOTAL' FROM [EXACTUS].[dismo].[FACTURA] as A  INNER JOIN  [EXACTUS].[dismo].[FACTURA_LINEA] B on A.PEDIDO = B.PEDIDO  INNER JOIN [EXACTUS].[dismo].[ARTICULO] C  on B.ARTICULO = C.ARTICULO  where RUTA = '" + this.comboBox1.Text + "' and A.ANULADA ='N' and DATEADD(dd, 0, DATEDIFF(dd, 0, A.FECHA_HORA)) = '" + this.dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' and USUARIO = '" + this.comboBox2.Text + "' Group by  A.[RUTA],B.ARTICULO,C.DESCRIPCION,A.FECHA_HORA", con.conex);
           // SqlDataAdapter da = new SqlDataAdapter(cm2);
           // da.Fill(EV);
            

            
        }
    }
}
