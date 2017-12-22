using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sinconizacion_EXactus.CORECTX_APP.RRHH
{
    public partial class No_evaluados : Form
    {
        public No_evaluados(int tipocon)
        {
            InitializeComponent();
            CONSULT = tipocon;
        }
        DataTable clientes = new DataTable();
        int CONSULT;
       public static string COD_EMP;
        private void No_evaluados_Load(object sender, EventArgs e)
        {
            gridView1.OptionsBehavior.Editable = false;
            gridView1.OptionsView.ShowAutoFilterRow = true;

          
          gridControl1.DataSource = Evaluaciones_Main.pendiente;

            if (Evaluaciones_Main.pendiente.Rows.Count >= 1)
            {
                linkLabel1.Show();
            }
            else
            {
                linkLabel1.Hide();
            }
        }

        private void cargar_datos()
        {

        }

        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            //string COD_EMP;
            //foreach (int i in gridView1.GetSelectedRows())
            //{
            //    DataRow row = gridView1.GetDataRow(i);

            //    COD_EMP = Convert.ToString(row[0]);

            //}

        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            // GridView view = (GridView)sender;
            Point pt = gridView1.GridControl.PointToClient(Control.MousePosition);
            DoRowDoubleClick(gridView1, pt);
        }

        private  void DoRowDoubleClick(GridView view, Point pt)
        {
            if (CONSULT == 1)
            {

            }
            else
            {
                GridHitInfo info = view.CalcHitInfo(pt);
                if (info.InRow || info.InRowCell)
                {
                    string colCaption = info.Column == null ? "N/A" : info.Column.GetCaption();
                    object valor = view.GetRowCellValue(info.RowHandle, "CODIGO");

                    COD_EMP = Convert.ToString(valor);

                }

                Objetivos_Evaluacion obj = new Objetivos_Evaluacion(COD_EMP, 1);

                this.Visible = false;
                obj.ShowDialog();
                obj.FormClosed += new System.Windows.Forms.FormClosedEventHandler(FormClosed);
                obj.Close();
                obj = null;
                //  this.Close();
            }
          
        }

        private void FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string FileName = "C:\\CORRECT\\RRHH_NOEVALUADOS.xls";
            gridView1.ExportToXls(FileName);

            Microsoft.Office.Interop.Excel.Application excell;
            Microsoft.Office.Interop.Excel.Workbook workbook;
            excell = new Excel.Application();
            excell.Visible = true;
            workbook = excell.Workbooks.Open(FileName);
        }
    }
}
    