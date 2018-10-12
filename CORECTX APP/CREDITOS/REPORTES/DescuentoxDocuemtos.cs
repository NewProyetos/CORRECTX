using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using DevExpress.Spreadsheet;

namespace Sinconizacion_EXactus.CORECTX_APP.CREDITOS.REPORTES
{
    public partial class DescuentoxDocuemtos : Form
    {
        public DescuentoxDocuemtos(int id_report,string nombre_rep)
        {
            InitializeComponent();
            id_rep = id_report;
            nombre_reporte = nombre_rep;
        }

        int id_rep;
        DateTimePicker datePickerini = new DateTimePicker();
        DateTimePicker datePickerfin = new DateTimePicker();
        ToolStripButton btupdate = new ToolStripButton();
        ToolStripButton btexcel = new ToolStripButton();
        ToolStripComboBox cbxrt = new ToolStripComboBox();
        string nombre_reporte;
        DataTable dtfull = new DataTable();
        string empresa;
        conexionXML con = new conexionXML();

        private void DescuentoxDocuemtos_Load(object sender, EventArgs e)
        {

            this.Text = nombre_reporte.ToUpper();


            gridView1.OptionsView.ColumnAutoWidth = false;
            gridView1.OptionsView.RowAutoHeight = true;
            gridView1.OptionsBehavior.Editable = true;
            //gridView1.OptionsView.ShowFooter = true;
            gridView1.BestFitColumns();

            empresa = Login.empresa.ToUpper();
            btexcel.Click += new EventHandler(btexcel_click);
            btupdate.Click += new EventHandler(btupdate_click);
            gridView1.OptionsView.ShowFooter = true;

            load_ojetos_toostrip();



        }


        private void btupdate_click(object sender, EventArgs e)
        {
            if (id_rep == 1)
            {
                carga_dato_descuentos();
            }
            else if (id_rep == 2)
            {
                carga_dato_liquidaciones();
            }
            else if (id_rep == 3)
            {
                carga_datos_STICS();
            }
        }

        private void btexcel_click(object sender, EventArgs e)
        {

            string directory = "C:\\CORRECT\\XLS";
            if (Directory.Exists(directory))
            {
                toExcel(groupControl1.Text);

            }
            else
            {
                Directory.CreateDirectory(directory);
                btexcel_click(null, null);
            }



        }

        private void toExcel(string nombre_reporte)
        {
            string FileName = "C:\\CORRECT\\XLS\\"+nombre_reporte+".xlsx";
            gridView1.ExportToXlsx(FileName);


            Microsoft.Office.Interop.Excel.Application excell;
            Microsoft.Office.Interop.Excel.Workbook workbook;
            excell = new Excel.Application();
            excell.Visible = true;
            workbook = excell.Workbooks.Open(FileName);


            //this.gridView1.ExportToXlsx(FileName);
            //DevExpress.Spreadsheet.Workbook workbook = new  Workbook();
            //// Load a workbook from the stream. 
            //using (FileStream stream = new FileStream(FileName, FileMode.Open))
            //{
            //    workbook.LoadDocument(stream, DocumentFormat.OpenXml);
            //}
            //WorksheetHeaderFooterOptions options = workbook.Worksheets[0].HeaderFooterOptions;
            //// Specify that the first page of the worksheet has unique headers and footers.  
            //options.DifferentFirst = true;
            //// Set headers and footers for the first page. 
            //// Insert the workbook name into the header left section. 
            //options.FirstHeader.Left = "&F";

            //// Insert the current date into the footer left section. 
            //options.FirstFooter.Left = "&D";
            //// Insert the current page number into the footer right section. 
            //options.FirstFooter.Right = string.Format("Page {0} of {1}", "&P", "&N");
            //using (FileStream stream = new FileStream("TEST.xlsx",
            //FileMode.Create, FileAccess.ReadWrite))
            //{
            //    workbook.SaveDocument(stream, DocumentFormat.Xlsx);
            //}
            //Process.Start("TEST.xlsx");

        }

        private void cargar_entegas()
        {
            con.conectar("EX");


            SqlCommand cm2 = new SqlCommand("SELECT  [RUTA] FROM [EXACTUS].[" + Login.empresa + "].[RUTA]  where RUTA like 'E%' ", con.conex);
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
                cbxrt.Items.Add(dr2["RUTA"]);
            }
            dr2.Close();
            con.Desconectar("EX");
        }

        private void load_ojetos_toostrip()
        {

            object O = global::Sinconizacion_EXactus.Properties.Resources.ResourceManager.GetObject("refreshpeq1");
            object e = global::Sinconizacion_EXactus.Properties.Resources.ResourceManager.GetObject("excelpeq");

            DateTime DT = DateTime.Now;



            Label lblfechaini = new Label();
            lblfechaini.Text = "Fecha Inicio";

            toolStrip1.Items.Add(new ToolStripControlHost(lblfechaini));

            datePickerini.Format = DateTimePickerFormat.Short;
            // datePickerini.Value = new DateTime(DT.Year, DT.Month, 1);
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






            ToolStripSeparator sep3 = new ToolStripSeparator();
            toolStrip1.Items.Add(sep3);

            btupdate.Text = "Refrescar";
            btupdate.Image = (Image)O;
            toolStrip1.Items.Add(btupdate);
            ToolStripSeparator sep2 = new ToolStripSeparator();
            toolStrip1.Items.Add(sep2);


            ToolStripSeparator sep4 = new ToolStripSeparator();
            toolStrip1.Items.Add(sep4);



            if (id_rep == 2)
            {
                Label comencmb = new Label();
                comencmb.Text = "RUTA ENTREGA:";
                toolStrip1.Items.Add(new ToolStripControlHost(comencmb));
                toolStrip1.Items.Add(cbxrt);

                ToolStripSeparator sep5 = new ToolStripSeparator();
                toolStrip1.Items.Add(sep5);

                cargar_entegas();
            }

            Label comentlb = new Label();
            comentlb.Text = "Enviar a Excel:";


            btexcel.Text = "Excel";
            btexcel.Image = (Image)e;
            toolStrip1.Items.Add(btexcel);


        }

        private void carga_dato_descuentos()
        {
            dtfull.Clear();
            con.conectar("EX");
            SqlCommand cmd2 = new SqlCommand();

            cmd2 = new SqlCommand("[dismo].[DESCUENTOS_DOCUMENTO]", con.conex);
            cmd2.CommandType = CommandType.StoredProcedure;

            cmd2.Parameters.AddWithValue("@fechaini", datePickerini.Value.ToString("yyyy/MM/dd"));
            cmd2.Parameters.AddWithValue("@fechafin", datePickerfin.Value.ToString("yyyy/MM/dd"));
            cmd2.Parameters.AddWithValue("@empresa", Login.empresa);


            SqlDataAdapter da1 = new SqlDataAdapter(cmd2);
            da1.Fill(dtfull);
            con.Desconectar("EX");

            gridControl1.Refresh();
            gridControl1.DataSource = dtfull;

            

            groupControl1.Text = "DESCUENTOS";
       


        }

        private void carga_dato_liquidaciones()
        {

            dtfull.Clear();
            con.conectar("DM");
            SqlCommand cmd2 = new SqlCommand();
            cmd2 = new SqlCommand("[CORRECT].[LIQUIDACION_ENTREGA_REPORT]", con.condm);
            cmd2.CommandType = CommandType.StoredProcedure;

            cmd2.Parameters.AddWithValue("@fecha_ini", datePickerini.Value.ToString("yyyy/MM/dd"));
            cmd2.Parameters.AddWithValue("@fecha_fin", datePickerfin.Value.ToString("yyyy/MM/dd"));
            if (cbxrt.Text == "" || cbxrt.Text == null)
            {
                cmd2.Parameters.AddWithValue("@ENTREGA",null);
            }
            else
            {
                cmd2.Parameters.AddWithValue("@ENTREGA", cbxrt.Text);
            }
            cmd2.Parameters.AddWithValue("@USUARIO",null);
            cmd2.Parameters.AddWithValue("@empresa", Login.empresa.ToUpper()); 


            SqlDataAdapter da1 = new SqlDataAdapter(cmd2);
            da1.Fill(dtfull);
            con.Desconectar("DM");

            gridControl1.Refresh();
            gridControl1.DataSource = dtfull;

            groupControl1.Text = "LIQUIDACIONES";
        }
        
        private void carga_datos_STICS()
        {
            dtfull.Clear();
            con.conectar("DM");
            SqlCommand cmd2 = new SqlCommand();

            cmd2 = new SqlCommand("[CORRECT].[REPORTE_STCS]", con.condm);
            cmd2.CommandType = CommandType.StoredProcedure;

            cmd2.Parameters.AddWithValue("@FINI", datePickerini.Value.ToString("yyyy/MM/dd"));
            cmd2.Parameters.AddWithValue("@FFIN", datePickerfin.Value.ToString("yyyy/MM/dd"));
            


            SqlDataAdapter da1 = new SqlDataAdapter(cmd2);
            da1.Fill(dtfull);
            con.Desconectar("DM");

            gridControl1.Refresh();
            gridControl1.DataSource = dtfull;



            groupControl1.Text = "STCS";
        }


        private void clear_datagrid()
        {
            gridView1.Columns.Clear();
            gridControl1.DataSource = null;
            gridControl1.Refresh();
        }

    }
}
