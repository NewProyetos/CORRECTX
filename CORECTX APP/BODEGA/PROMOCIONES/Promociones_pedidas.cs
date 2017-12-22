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

namespace Sinconizacion_EXactus.CORECTX_APP.BODEGA
{
    public partial class Promociones_pedidas : Form
    {
        conexionXML con = new conexionXML();
        DataTable articulo_bon = new DataTable();
        String Encabezado;
        public Promociones_pedidas()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            CORECTX_APP.BODEGA.PROMOCIONES.Articulos_Bonificar arbn = new PROMOCIONES.Articulos_Bonificar();
            arbn.ShowDialog();
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            CORECTX_APP.BODEGA.PROMOCIONES.Articulos_Bonificar arbn = new PROMOCIONES.Articulos_Bonificar();
            arbn.ShowDialog();
        }

        private void Promociones_pedidas_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;
            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            //dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;

            cargadata();
        }
        private void cargadata()
        {
            articulo_bon.Clear();
            con.conectar("EX");

            SqlCommand cm2 = new SqlCommand("SELECT  EP.PEDIDO,FECHA_PEDIDO,CLIENTE,NOMBRE_CLIENTE,VENDEDOR,LP.PEDIDO_LINEA,PEDIDO_LINEA_BONIF,LP.ARTICULO,AR.DESCRIPCION, CANTIDAD_PEDIDA,CANTIDAD_BONIFICAD from EXACTUS."+ Login.empresa.ToUpper() + ".PEDIDO EP INNER JOIN EXACTUS."+ Login.empresa.ToUpper() + ".PEDIDO_LINEA LP ON EP.PEDIDO=LP.PEDIDO AND (DATEADD(dd, 0, DATEDIFF(dd, 0, EP.CreateDate)) = DATEADD(dd, 0, DATEDIFF(dd, 0, LP.CreateDate))) INNER JOIN EXACTUS."+Login.empresa.ToUpper()+".ARTICULO AR ON LP.ARTICULO=AR.ARTICULO WHERE (DATEADD(dd, 0, DATEDIFF(dd, 0, EP.FECHA_PEDIDO)) >= '"+dateTimePicker1.Value.ToString("yyyy-MM-dd")+"') and (DATEADD(dd, 0, DATEDIFF(dd, 0, EP.FECHA_PEDIDO)) <= '" + dateTimePicker2.Value.ToString("yyyy-MM-dd")+"') AND EP.ESTADO='N' and LP.ARTICULO in(SELECT [CODIGO] FROM [DM].[CORRECT].[PROMOCIONES_GT]) ORDER BY EP.PEDIDO,PEDIDO_LINEA", con.conex);
            SqlDataAdapter da2 = new SqlDataAdapter(cm2);
            da2.Fill(articulo_bon);

            dataGridView1.DataSource = articulo_bon;


            con.Desconectar("EX");
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            cargadata();
        }

        private void copyall()
        {
            dataGridView1.SelectAll();
            DataObject DTobj = dataGridView1.GetClipboardContent();
            if (DTobj != null)
            {
                Clipboard.SetDataObject(DTobj);
            }

        }

        private void Generando_excel(DataTable DT)
        {

            int cellfin;
            cellfin = dataGridView1.ColumnCount;
            copyall();

            Microsoft.Office.Interop.Excel.Application excell;
            Microsoft.Office.Interop.Excel.Workbook workbook;
            Microsoft.Office.Interop.Excel.Worksheet Sheet;
            object miobj = System.Reflection.Missing.Value;
            excell = new Excel.Application();
            excell.Visible = true;


            int incre;

            int Columnas, col;

            col = DT.Columns.Count / 26;

            string Letracol = "ABCDEFEHIJKLMNOPQRSTUVWXYZ";
            string Complementocol;
            //Determinando la letra que se usara despues de la columna 26
            if (col > 0)
            {
                Columnas = DT.Columns.Count - (26 * col);
                Complementocol = Letracol.ToString().Substring(col - 1, 1);
            }
            else
            {
                Columnas = DT.Columns.Count;
                Complementocol = "";
            }

            string ColumnaFinal;

            incre = Encoding.ASCII.GetBytes("A")[0];

            ColumnaFinal = Complementocol.ToString() + Convert.ToChar(incre + Columnas - 1).ToString();


            workbook = excell.Workbooks.Add(miobj);
            Sheet = workbook.Worksheets.get_Item(1);

            Excel.Range rg = Sheet.Cells[4, 1];
            Excel.Range Enc;
            Excel.Range RN;
            Excel.Range Report;
            rg.Select();




            for (int c = 0; c < DT.Columns.Count; c++)
            {

                Sheet.Cells[3, c + 1] = String.Format("{0}", DT.Columns[c].Caption);
            }


            Sheet.PasteSpecial(rg, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);

            try
            {
                // nombre de la empresa
                RN = Sheet.get_Range("A1", ColumnaFinal + "1");
                RN.Font.Name = "Times New Roman";
                //rango.Font.Color = Color.Blue;
                RN.Font.Size = 14;

                Sheet.Cells[1, 1] = "DISTRIBUIDORA MORAZAN SA DE CV";
                RN.Merge();
                RN.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;



                //Nombre del Reporte 
                Report = Sheet.get_Range("A2", ColumnaFinal + "2");
                Report.Font.Name = "Times New Roman";
                Report.Font.Size = 12;
                //"DETALLE " + "   DEL " + FechaIni.ToString("dd-MM-yyyy") + "  AL  " + FechaFin.ToString("dd-MM-yyyy") + " ";


                Sheet.Cells[2, 1] = Encabezado + " EMISION " + DateTime.Now.ToString();



                Report.Select();
                Report.Merge();
                Report.Font.Bold = true;
                Report.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                Report.Borders.LineStyle = Excel.XlLineStyle.xlLineStyleNone;



                //ENCABEZDO DE COLUMNAS
                Enc = Sheet.get_Range("A3", ColumnaFinal + 3);
                Enc.Font.Name = "Times New Roman";
                Enc.Font.Size = 9;
                Enc.Borders.LineStyle = Excel.XlLineStyle.xlDouble;
                Enc.Font.Bold = true;

            }
            catch (SystemException exec)
            {
                MessageBoxButtons bt1 = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(exec.ToString(), "!!!!ERROR!!!!", bt1, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);


            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            copyall();
            Generando_excel(articulo_bon);
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            button1.Enabled = true;
        }
    }
}
