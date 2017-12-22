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

namespace Sinconizacion_EXactus.CORECTX_APP.CREDITOS.REPORTES
{
    public partial class DescuentoxDocuemtos : Form
    {
        public DescuentoxDocuemtos()
        {
            InitializeComponent();
        }
        DateTimePicker datePickerini = new DateTimePicker();
        DateTimePicker datePickerfin = new DateTimePicker();
        ToolStripButton btupdate = new ToolStripButton();
        ToolStripButton btexcel = new ToolStripButton();
        DataTable dtfull = new DataTable();
        string empresa;
        conexionXML con = new conexionXML();

        private void DescuentoxDocuemtos_Load(object sender, EventArgs e)
        {
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
            carga_dato();
        }

        private void btexcel_click(object sender, EventArgs e)
        {
            string directory = "C:\\CORRECT\\XLS";
            if (Directory.Exists(directory))
            {
                toExcel();

            }
            else
            {
                Directory.CreateDirectory(directory);
                btexcel_click(null, null);
            }



        }

        private void toExcel()
        {
            string FileName = "C:\\CORRECT\\XLS\\descuento_documento.xlsx";
            gridView1.ExportToXlsx(FileName);


            Microsoft.Office.Interop.Excel.Application excell;
            Microsoft.Office.Interop.Excel.Workbook workbook;
            excell = new Excel.Application();
            excell.Visible = true;
            workbook = excell.Workbooks.Open(FileName);

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


            Label comentlb = new Label();
            comentlb.Text = "Enviar a Excel:";


            btexcel.Text = "Excel";
            btexcel.Image = (Image)e;
            toolStrip1.Items.Add(btexcel);


        }

        private void carga_dato()
        {
            dtfull.Clear();
            con.conectar("EX");
            SqlCommand cmd2 = new SqlCommand();

            cmd2 = new SqlCommand("[dismo].[DESCUENTOS_DOCUMENTO]", con.conex);
            cmd2.CommandType = CommandType.StoredProcedure;

            cmd2.Parameters.AddWithValue("@fechaini", datePickerini.Value.ToString("yyyy/MM/dd"));
            cmd2.Parameters.AddWithValue("@fechafin", datePickerfin.Value.ToString("yyyy/MM/dd"));
            

            SqlDataAdapter da1 = new SqlDataAdapter(cmd2);
            da1.Fill(dtfull);
            con.Desconectar("EX");

            gridControl1.Refresh();
            gridControl1.DataSource = dtfull;


            //total_arti = gridView1.GroupSummary.Add(SummaryItemType.Sum, "UNIDADES PEDIDAS", null, "Total Unidades ={0:N2}");
            //total_valor = gridView1.GroupSummary.Add(SummaryItemType.Sum, "VALOR PEDIDO", null, "Total Valor ={0:c2}");
            //Total_peso = gridView1.GroupSummary.Add(SummaryItemType.Sum, "PESO_BRUTO", null, "Total Peso ={0:N2}");
            //Total_volume = gridView1.GroupSummary.Add(SummaryItemType.Sum, "VOLUMEN", null, "Total Volume ={0:N2}");


            //gridView1.Columns["VALOR PEDIDO"].Summary.Add(DevExpress.Data.SummaryItemType.Average, "VALOR PEDIDO", "Avg={0:n2}");
            //gridView1.Columns["VALOR PEDIDO"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "VALOR PEDIDO", "Sum={0}");
            //GridColumnSummaryItem item = new GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Max, "VALOR PEDIDO", "Max={0}");
            //gridView1.Columns["VALOR PEDIDO"].Summary.Add(item);

            //gridView1.Columns["UNIDADES PEDIDAS"].Summary.Add(DevExpress.Data.SummaryItemType.Average, "UNIDADES PEDIDAS", "Avg={0:n2}");
            //gridView1.Columns["UNIDADES PEDIDAS"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "UNIDADES PEDIDAS", "Sum={0}");
            //GridColumnSummaryItem item2 = new GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Max, "UNIDADES PEDIDAS", "Max={0}");
            //gridView1.Columns["UNIDADES PEDIDAS"].Summary.Add(item2);

            //gridView1.Columns["PESO_BRUTO"].Summary.Add(DevExpress.Data.SummaryItemType.Average, "PESO_BRUTO", "Avg={0:n2}");
            //gridView1.Columns["PESO_BRUTO"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "PESO_BRUTO", "Sum={0}");
            //GridColumnSummaryItem item3 = new GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Max, "PESO_BRUTO", "Max={0}");
            //gridView1.Columns["PESO_BRUTO"].Summary.Add(item3);

            //gridView1.Columns["VOLUMEN"].Summary.Add(DevExpress.Data.SummaryItemType.Average, "VOLUMEN", "Avg={0:n2}");
            //gridView1.Columns["VOLUMEN"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "VOLUMEN", "Sum={0}");
            //GridColumnSummaryItem item4 = new GridColumn SummaryItem(DevExpress.Data.SummaryItemType.Max, "VOLUMEN", "Max={0}");
            //gridView1.Columns["VOLUMEN"].Summary.Add(item4);



            groupControl1.Text = "DESCUENTOS";
         //   barStaticItem1.Caption = Convert.ToString(gridView1.RowCount) + " Registros";


        }




        private void clear_datagrid()
        {
            gridView1.Columns.Clear();
            gridControl1.DataSource = null;
            gridControl1.Refresh();
        }

    }
}
