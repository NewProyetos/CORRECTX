using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraGrid;

namespace Sinconizacion_EXactus.CORECTX_APP.COMPRAS.Reportes
{
    public partial class Reporte_Form : Form
    {
        public Reporte_Form()
        {
            InitializeComponent();
        }
        string Tipo_repot;
        conexionXML con = new conexionXML();
        DataTable datos = new DataTable();
        object[] totals = new object[] { 0, 0, 0, 0 };


        private void Reporte_Form_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;
            DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;
            gridView1.OptionsView.ColumnAutoWidth = false;
            gridView1.OptionsView.RowAutoHeight = true;
            gridView1.OptionsBehavior.Editable = false;
            gridView1.BestFitColumns();
            radioButton1.Checked = true;
            Tipo_repot = "R";

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                Tipo_repot = "R";
            }
            else {
                Tipo_repot = "D";
            }

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                Tipo_repot = "D";
            }
            else
            {
                Tipo_repot = "R";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            datos.Clear();
            if (datos.Columns.Count > 0)
            {
                datos.Columns.Clear();
            }

            if (gridView1.Columns.Count > 0)
            {
                gridView1.Columns.Clear();
                gridControl1.DataSource = null;
                gridControl1.Refresh();

            }

            string fechalim = this.dateTimePicker1.Value.ToString("yyyy/MM/dd");

            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("[CORRECT].[REPORTE_SALDOS_PROVEEDORES]", con.condm);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 50;
            cmd.Parameters.AddWithValue("@Tipo_Rep", Tipo_repot);
            cmd.Parameters.AddWithValue("@fecha_lim", fechalim + " 23:59:59.000");        
            
         

            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(datos);

            gridControl1.DataSource = datos;

            if (gridView1.RowCount > 0)
            {
                button2.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
            }
            con.Desconectar("DM");


        }

        private void gridView1_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            
        }
        bool IsValid(object val)
        {
            try
            {
                int res = Convert.ToInt32(val);
                if (res >= 0 && res < totals.Length - 1)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void gridView1_CustomSummaryCalculate_1(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            if ((int)(e.Item as GridSummaryItem).Tag == 1)
            {

                if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
                {
                    e.TotalValue = totals;
                }
                if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate && IsValid(e.FieldValue))
                {
                    totals[(int)e.FieldValue] = (int)totals[(int)e.FieldValue] + 1;
                    totals[3] = (int)totals[3] + 1;
                }
                if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
                {
                    e.TotalValue = String.Format("Total: {3:N} Late: {0:N}, Closed: {1:N}, Open: {2:N}", totals);
                    totals = new object[] { 0, 0, 0, 0 };
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string nombre_report;
            if (Tipo_repot == "R")
            {
                nombre_report = "Resumen";
            }
            else
            {
                nombre_report = "Detalle";
            }

            string FileName = "C:\\CORRECT\\XLS\\ Compras_" + nombre_report + "_" + dateTimePicker1.Value.ToString("ddddMMyyy") + ".xlsx";
            gridView1.ExportToXlsx(FileName);



            Microsoft.Office.Interop.Excel.Application excell;
            Microsoft.Office.Interop.Excel.Workbook workbook;
            excell = new Microsoft.Office.Interop.Excel.Application();
            excell.Visible = true;
            workbook = excell.Workbooks.Open(FileName);

        }
    }
}
