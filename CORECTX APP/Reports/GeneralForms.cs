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
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Card;
using DevExpress.Export;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;

namespace Sinconizacion_EXactus.CORECTX_APP.Reports
{
    public partial class GeneralForms : Form
    {

        public GeneralForms(string nombrerep, int id_repot)
        {
            InitializeComponent();
            Reportenombre = nombrerep;
            ID_Reporte = id_repot;
        }
        DateTimePicker datePickerini = new DateTimePicker();
        DateTimePicker datePickerfin = new DateTimePicker();
        ToolStripButton btupdate = new ToolStripButton();
        ToolStripButton btexcel = new ToolStripButton();
        ToolStripButton filterbt = new ToolStripButton();
        DataSet dtfull = new DataSet();
        string empresa;
        int ID_Reporte;
        String Reportenombre;
        String consultasql;
        conexionXML con = new conexionXML();



        private void GeneralForms_Load(object sender, EventArgs e)
        {
            gridView1.SetMasterRowExpandedEx(0, 0, true);
            gridView1.SetMasterRowExpanded(1, true);
            //GridControl grid = gridControl1;
            //GridLevelNode node1 = grid.LevelTree.Nodes.Add("Regalias", gridView1);          
            GridLevelNode node1 = gridControl1.LevelTree.Nodes.Add("Regalias", gridView1);
            GridLevelNode node11 = node1.Nodes.Add("Regalias Detalle", gridView2);
            //node1.Nodes.Add(node11);

         
            //GridLevelNode node2 = grid.LevelTree.Nodes.Add("Traspaso Detalle", gridView3);

            gridView1.OptionsView.ColumnAutoWidth = false;
            gridView1.OptionsView.RowAutoHeight = true;
            gridView1.OptionsBehavior.Editable = true;
            //gridView1.OptionsView.ShowFooter = true;
            gridView1.BestFitColumns();

            empresa = Login.empresa.ToUpper();
            btexcel.Click += new EventHandler(btexcel_click);
            btupdate.Click += new EventHandler(btupdate_click);
            filterbt.Click += new EventHandler(filterbt_click);
            gridView1.OptionsView.ShowFooter = true;

            

            load_ojetos_toostrip();

            //gridView1.CollapseAllDetails(); // works
            //gridView1.SetMasterRowExpanded(0, false); // works


            groupControl1.Text = Reportenombre;
        }

        private void filterbt_click(object sender, EventArgs e)
        {
            if (gridView1.OptionsView.ShowAutoFilterRow)
            {
                gridView1.OptionsView.ShowAutoFilterRow = false;
                filterbt.Image = new Bitmap(Properties.Resources.filter_16x16);
            }
            else
            {
                gridView1.OptionsView.ShowAutoFilterRow = true;
                filterbt.Image = new Bitmap(Properties.Resources.clearfilter_16x16);
            }

        }

        private void btupdate_click(object sender, EventArgs e)
        {
            //  carga_dato();
            carga_enc();
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

        private void load_ojetos_toostrip()
        {

            object O = global::Sinconizacion_EXactus.Properties.Resources.ResourceManager.GetObject("refreshpeq1");
            object e = global::Sinconizacion_EXactus.Properties.Resources.ResourceManager.GetObject("excelpeq");
            object f = global::Sinconizacion_EXactus.Properties.Resources.ResourceManager.GetObject("filter_16x16");

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


            filterbt.Text = "Filtro";
            filterbt.Image = (Image)f;
            toolStrip1.Items.Add(filterbt);
           

        }

        private void carga_dato()
        {

            switch (ID_Reporte)
            {
                case 1:
                    consultasql = "[EXACTUS].[dismo].[DESCUENTOS_DOCUMENTO]";
                    break;

                case 2:
                    consultasql = "[DM].[CORRECT].[REPORTE_REGALIAS]";
                    break;
            }

            dtfull.Clear();
            con.conectar("EX");
            SqlCommand cmd2 = new SqlCommand();

            cmd2 = new SqlCommand(consultasql, con.conex);
            cmd2.CommandType = CommandType.StoredProcedure;

            cmd2.Parameters.AddWithValue("@fechaini", datePickerini.Value.ToString("yyyy/MM/dd"));
            cmd2.Parameters.AddWithValue("@fechafin", datePickerfin.Value.ToString("yyyy/MM/dd"));
            cmd2.Parameters.AddWithValue("@empresa", Login.empresa);

            SqlDataAdapter da1 = new SqlDataAdapter(cmd2);
            da1.Fill(dtfull);
            con.Desconectar("EX");


            carga_enc();


            //gridControl1.Refresh();
            // gridControl1.DataSource = dtfull;

        }
        private void clear_datagrid()
        {
            gridView1.Columns.Clear();
            gridControl1.DataSource = null;
            gridControl1.Refresh();
        }
        private void toExcel()
        {
            XlsxExportOptions opt = new XlsxExportOptions();
                
           opt.ExportMode =  XlsxExportMode.SingleFilePageByPage;
            string FileName = "C:\\CORRECT\\XLS\\" + Reportenombre + ".xlsx";
            opt.SheetName = "NEW";
            
        
            //gridView1.ExportToXlsx(FileName);
            gridView1.OptionsPrint.ExpandAllDetails = true;
            gridView1.OptionsPrint.PrintDetails = true;
            gridView1.OptionsPrint.ExpandAllGroups = true;

            //gridView2.OptionsPrint.ExpandAllDetails = true;
            //gridView2.OptionsPrint.PrintDetails = true;
            gridView1.ZoomView();
            //CompositeLink conp = new CompositeLink();

            var printingSystem = new PrintingSystemBase();
            var compositeLink = new CompositeLinkBase();
            compositeLink.PrintingSystemBase = printingSystem;

            var link1 = new PrintableComponentLinkBase();
            link1.Component = gridControl1;
            //var link2 = new PrintableComponentLinkBase();
            //link2.Component = grid2;

            compositeLink.Links.Add(link1);
           // compositeLink.Links.Add(link2);

            var options = new XlsxExportOptions();
            options.ExportMode = XlsxExportMode.SingleFilePageByPage;

            compositeLink.CreatePageForEachLink();
            compositeLink.ExportToXlsx(FileName, options);








            //conp.ExportToXlsx(FileName, opt);

            //gridView1.ExportToXlsx(FileName,opt);    
            //gridControl1.DefaultView
            Microsoft.Office.Interop.Excel.Application excell;
            Microsoft.Office.Interop.Excel.Workbook workbook;
            excell = new Excel.Application();
            excell.Visible = true;
            workbook = excell.Workbooks.Open(FileName);

            
        }

        private void carga_enc()
        {
            //var list =(from x in defaultDataTable.AsEnumerable()
            //           where x.Field<string>(1) == something
            //           select x.Field<string>(1)).ToList();)


            //con.conectar("DM");
            //SqlCommand cmd2 = new SqlCommand();
            //cmd2 = new SqlCommand(" SELECT DET.[NUM_REG] ,DET.[CLIENTE],CLIE.NOMBRE,DET.[ARTICULO],DET.[CANTIDAD] ,DET.[USUARIO_INGRESO] FROM [DM].[CORRECT].[REGALIAS_SOLICI_DET] as DET LEFT JOIN [EXACTUS].[dismo].[CLIENTE] as CLIE  on DET.CLIENTE = CLIE.CLIENTE  where NUM_REG in ('R175000003','R176000003')", con.condm);
            //SqlDataAdapter da1 = new SqlDataAdapter(cmd2);
            //da1.Fill(dtfull);
            // //con.Desconectar("DM");
            // Muebles_merchan.Encabezado_regaliasDataTable reg = new Muebles_merchan.Encabezado_regaliasDataTable();
            // Muebles_merchan.Detalle_RegaliasDataTable det = new Muebles_merchan.Detalle_RegaliasDataTable();

            // foreach (DataRow dr in dtfull.Tables[0].Rows)
            // {
            //     // (/* some condition */)
            //         reg.Rows.Add(dr.ItemArray);
            // }

            // foreach (DataRow drd in dtfull.Tables[1].Rows)
            // {
            //     // (/* some condition */)
            //     det.Rows.Add(drd.ItemArray);
            // }

            //gridControl1.DataSource = reg;
            
            
            this.reportE_REGALIAS_ENCTableAdapter1.Fill(this.muebles_merchan3.REPORTE_REGALIAS_ENC, datePickerini.Value, datePickerfin.Value,Login.empresa);
            // this.rEPORTE_REGALIAS_ENCTableAdapter.Fill(muebles_merchan.REPORTE_REGALIAS_DET, datePickerini.Value, datePickerfin.Value);
            this.reportE_REGALIAS_DETTableAdapter1.Fill(this.muebles_merchan3.REPORTE_REGALIAS_DET, datePickerini.Value, datePickerfin.Value,Login.empresa);
            this.reportE_REGALIAS_TRASTableAdapter1.Fill(this.muebles_merchan3.REPORTE_REGALIAS_TRAS, datePickerini.Value, datePickerfin.Value,Login.empresa);
            
        }

      

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void dataSetofBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }
    }
}
