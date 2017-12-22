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

namespace Sinconizacion_EXactus.CORECTX_APP.VENTAS
{
    public partial class Visitas_Comentarios : Form
    {
        public Visitas_Comentarios()
        {
            InitializeComponent();
        }

        conexionXML con = new conexionXML();
        DataTable dtfull = new DataTable();
        String empresa;
        DateTimePicker datePickerini = new DateTimePicker();
        DateTimePicker datePickerfin = new DateTimePicker();
        ToolStripButton btupdate = new ToolStripButton();
        ToolStripButton btexcel = new ToolStripButton();
        ToolStripTextBox tbxcomentario = new ToolStripTextBox();


        private void Visitas_Comentarios_Load(object sender, EventArgs e)
        {
            tbxcomentario.KeyPress += new KeyPressEventHandler(tbxcomentario_KeyPress);
            btupdate.Click += new EventHandler(btupdate_click);
            btexcel.Click += new EventHandler(btexcel_click);
            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            //dataGridView2.AutoResizeColumns();
            //dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ReadOnly = true;
            empresa = Login.empresa;


            cargaobjetostoostrip();
            cargadatos();

        }
        private void cargaobjetostoostrip()
        {


            object O = global::Sinconizacion_EXactus.Properties.Resources.ResourceManager.GetObject("refreshpeq1");
            object e = global::Sinconizacion_EXactus.Properties.Resources.ResourceManager.GetObject("excelpeq");

            DateTime DT = DateTime.Now;


           
            datePickerini.Format = DateTimePickerFormat.Short;
            datePickerini.Value = new DateTime(DT.Year, DT.Month, 1);
            datePickerini.Size = new Size(120, 20);


            toolStrip1.Items.Add(new ToolStripControlHost(datePickerini));

            Label lblfechafin = new Label();
            lblfechafin.Text = "Fecha fin";

            toolStrip1.Items.Add(new ToolStripControlHost(lblfechafin));
           
            datePickerfin.Format = DateTimePickerFormat.Short;
            datePickerfin.Size = new Size(120, 20);


            toolStrip1.Items.Add(new ToolStripControlHost(datePickerfin));

            ToolStripSeparator sep1 = new ToolStripSeparator();
            toolStrip1.Items.Add(sep1);



            
            btupdate.Text = "Refrescar";
            btupdate.Image = (Image)O;
            toolStrip1.Items.Add(btupdate);
            ToolStripSeparator sep2 = new ToolStripSeparator();
            toolStrip1.Items.Add(sep2);

            Label buscarlb = new Label();
            buscarlb.Text = "BUSCAR";

            toolStrip1.Items.Add(new ToolStripControlHost(buscarlb));

            ToolStripSeparator sep3 = new ToolStripSeparator();
            toolStrip1.Items.Add(sep3);

            Label comentlb = new Label();
            comentlb.Text = "Comentario:";


           



            tbxcomentario.Size = new Size(150, 25);
            toolStrip1.Items.Add(tbxcomentario);

            btexcel.Text = "Excel";
            btexcel.Image = (Image)e;
            toolStrip1.Items.Add(btexcel);


        }

        private void cargadatos()
        {
            dtfull.Clear();
            con.conectar("EX");
            SqlCommand cmd2 = new SqlCommand();
            if (empresa == "DISMO")
            {
                cmd2 = new SqlCommand("select VS.CLIENTE,VS.RUTA as VENDEDOR,NOMBRE,ALIAS,DIRECCION,VS.NOTAS,VS.INICIO AS FECHA from EXACTUS.ERPADMIN.VISITA VS INNER JOIN EXACTUS." + empresa + ".CLIENTE CL ON VS.CLIENTE = CL.CLIENTE where VS.NOTAS IS NOT NULL AND LEFT(VS.CLIENTE, 1) <> 'G' AND VS.NOTAS <> '' and (DATEADD(dd, 0, DATEDIFF(dd, 0, VS.INICIO)) >= '"+datePickerini.Value.ToString("yyyy-MM-dd") + "') AND (DATEADD(dd, 0, DATEDIFF(dd, 0, VS.INICIO)) <= '"+datePickerfin.Value.ToString("yyyy-MM-dd") + "')", con.condm);
            }
            else
            {
                cmd2 = new SqlCommand("select VS.CLIENTE,VS.RUTA as VENDEDOR,NOMBRE,ALIAS,DIRECCION,VS.NOTAS,VS.INICIO AS FECHA from EXACTUS.ERPADMIN.VISITA VS INNER JOIN EXACTUS."+empresa+ ".CLIENTE CL ON VS.CLIENTE = CL.CLIENTE where VS.NOTAS IS NOT NULL AND LEFT(VS.CLIENTE, 1) = 'G' AND VS.NOTAS <> '' and (DATEADD(dd, 0, DATEDIFF(dd, 0, VS.INICIO)) >= '" + datePickerini.Value.ToString("yyyy-MM-dd") + "') AND (DATEADD(dd, 0, DATEDIFF(dd, 0, VS.INICIO)) <= '" + datePickerfin.Value.ToString("yyyy-MM-dd") + "')", con.condm);
            }

            SqlDataAdapter da1 = new SqlDataAdapter(cmd2);
            da1.Fill(dtfull);
            con.Desconectar("EX");

            dataGridView1.DataSource = dtfull;

        }

       private void tbxcomentario_KeyPress(object O, KeyPressEventArgs e)
        {
            dtfull.DefaultView.RowFilter = string.Format("Convert(NOTAS,'System.String') like '%{0}%'", tbxcomentario.Text);
            dataGridView1.DataSource = dtfull;
            
        }

        private void btupdate_click(object sender, EventArgs e)
        {
            cargadatos();

        }

        private void btexcel_click(object sender, EventArgs e)
        {
            copyall();
            excel(dtfull);

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


        private void excel(DataTable dt)
        {


            String Encabezado = "REPORTES  ENTRADAS y SALIDAS";

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

            col = dt.Columns.Count / 26;

            string Letracol = "ABCDEFEHIJKLMNOPQRSTUVWXYZ";
            string Complementocol;
            //Determinando la letra que se usara despues de la columna 26
            if (col > 0)
            {
                Columnas = dt.Columns.Count - (26 * col);
                Complementocol = Letracol.ToString().Substring(col - 1, 1);
            }
            else
            {
                Columnas = dt.Columns.Count;
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




            for (int c = 0; c < dt.Columns.Count; c++)
            {

                Sheet.Cells[3, c + 1] = String.Format("{0}", dt.Columns[c].Caption);
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
    }
}
