using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace Sinconizacion_EXactus.CORECTX_APP.BODEGA.REPORTES
{
    public partial class existencias : Form
    {
        public existencias()
        {
            InitializeComponent();
        }

        conexionXML con = new conexionXML();
        DataTable existb = new DataTable();
        String bodega;

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void existencias_Load(object sender, EventArgs e)
        {
            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            //dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;

            groupBox3.Hide();
            DateTime fecini = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime fechafin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            dateTimePicker1.Value = fecini;

            con.conectar("DM");

            SqlCommand cmd = new SqlCommand("SELECT[COD_BOD] FROM [DM].[CORRECT].[SUCURSALES_EXATUS] where  [ID_SUCURSAL] = " + Main_Menu.Agencia + "", con.condm);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
               comboBox1.Items.Add(dr["COD_BOD"]);
            }

            con.Desconectar("DM");
          

        }

        private void backgroundExistencia_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            to_datagrid();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (backgroundExistencia.IsBusy != true)
            {
                bodega = comboBox1.Text;
                groupBox3.Show();
                backgroundExistencia.RunWorkerAsync();

            }
        }
        private void to_datagrid()
        {
            
            dataGridView1.DataSource = existb;
            dataGridView1.Refresh();
            dataGridView1.ReadOnly = true;
        }

        private void backgroundExistencia_DoWork(object sender, DoWorkEventArgs e)
        {
            backgroundExistencia.ReportProgress(0, "Generando Reportes.");

            existb.Clear();

            con.conectar("EX");
            SqlCommand cmdex = new SqlCommand("[DISMOGT].[RepInventario]");
            cmdex.CommandTimeout = 240;
            cmdex.Connection = con.conex;
            cmdex.CommandType = CommandType.StoredProcedure;
            cmdex.Parameters.AddWithValue("@fechai", dateTimePicker1.Value.ToString("yyyy/MM/dd"));
            cmdex.Parameters.AddWithValue("@fechaf", dateTimePicker2.Value.ToString("yyyy/MM/dd"));
            cmdex.Parameters.AddWithValue("@BODEGA", bodega);

            SqlDataAdapter da = new SqlDataAdapter(cmdex);
            da.Fill(existb);



            con.Desconectar("EX");

        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            groupBox3.Hide();
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
            excell = new Microsoft.Office.Interop.Excel.Application();
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

            Microsoft.Office.Interop.Excel.Range rg = Sheet.Cells[4, 1];
            Microsoft.Office.Interop.Excel.Range Enc;
            Microsoft.Office.Interop.Excel.Range RN;
            Microsoft.Office.Interop.Excel.Range Report;
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
                RN.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;



                //Nombre del Reporte 
                Report = Sheet.get_Range("A2", ColumnaFinal + "2");
                Report.Font.Name = "Times New Roman";
                Report.Font.Size = 12;
                //"DETALLE " + "   DEL " + FechaIni.ToString("dd-MM-yyyy") + "  AL  " + FechaFin.ToString("dd-MM-yyyy") + " ";


                Sheet.Cells[2, 1] ="REPORTE EXISTENCIAS BODEGA "+bodega+ " EMISION " + DateTime.Now.ToString();



                Report.Select();
                Report.Merge();
                Report.Font.Bold = true;
                Report.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                Report.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;



                //ENCABEZDO DE COLUMNAS
                Enc = Sheet.get_Range("A3", ColumnaFinal + 3);
                Enc.Font.Name = "Times New Roman";
                Enc.Font.Size = 9;
                Enc.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDouble;
                Enc.Font.Bold = true;

            }
            catch (SystemException exec)
            {
                MessageBoxButtons bt1 = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(exec.ToString(), "!!!!ERROR!!!!", bt1, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);


            }


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

        private void button2_Click(object sender, EventArgs e)
        {
            copyall();
            Generando_excel(existb);
        }
    }
}
