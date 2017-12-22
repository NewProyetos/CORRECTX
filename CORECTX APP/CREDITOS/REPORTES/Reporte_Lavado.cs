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


namespace Sinconizacion_EXactus.CORECTX_APP.CREDITOS.REPORTES
{
    public partial class Reporte_Lavado : Form
    {
        public Reporte_Lavado()
        {
            InitializeComponent();
        }
        conexionXML con = new conexionXML();
        DataTable Datos = new DataTable();
             
             
        private void Reporte_Lavado_Load(object sender, EventArgs e)
        {
           
            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            //dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;

            DateTime fechaini = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime fechafin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1);
            dateTimePicker1.Value = fechaini;
            dateTimePicker2.Value = fechafin;

            button2.Enabled = false;




        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" || textBox1.Text != string.Empty)
            {
                Load_data();



            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back) && !(char.IsPunctuation(e.KeyChar)))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void Load_data()
        {
            con.conectar("EX");
            SqlCommand cmd2 = new SqlCommand("[dismo].[LAVADO_REPORT]", con.conex);
            cmd2.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da1 = new SqlDataAdapter(cmd2);

            cmd2.Parameters.AddWithValue("@fechaini", dateTimePicker1.Value.ToString("yyyy/MM/dd"));
            cmd2.Parameters.AddWithValue("@fechafin", dateTimePicker2.Value.ToString("yyyy/MM/dd"));
            cmd2.Parameters.AddWithValue("@monto", textBox1.Text);



            cmd2.ExecuteNonQuery();

            da1.Fill(Datos);

            con.Desconectar("EX");
            dataGridView1.DataSource = Datos;


        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridView1.RowCount >= 1)
            {
                button2.Enabled = true;

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

            Excel.Range rg = Sheet.Cells[5, 1];
            Excel.Range Enc;
            Excel.Range RN;
            Excel.Range Report;
            Excel.Range ReportN;
            rg.Select();




            for (int c = 0; c < DT.Columns.Count; c++)
            {

                Sheet.Cells[4, c + 1] = String.Format("{0}", DT.Columns[c].Caption);
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



                Sheet.Cells[2, 1] = "REPORTE TRANSACCIONES REGULADAS MAYORES  A "+textBox1.Text+"";
             


                Report.Select();
                Report.Merge();
                Report.Font.Bold = true;
                Report.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                Report.Borders.LineStyle = Excel.XlLineStyle.xlLineStyleNone;


                ReportN = Sheet.get_Range("A3", ColumnaFinal + "3");
                ReportN.Font.Name = "Times New Roman";
                ReportN.Font.Size = 12;
                //"DETALLE " + "   DEL " + FechaIni.ToString("dd-MM-yyyy") + "  AL  " + FechaFin.ToString("dd-MM-yyyy") + " ";



                Sheet.Cells[3, 1] = "DETALLE " + "   DEL " + dateTimePicker1.Value.ToString("dd-MM-yyyy") + "  AL  " + dateTimePicker2.Value.ToString("dd-MM-yyyy") + "";



                ReportN.Select();
                ReportN.Merge();
                ReportN.Font.Bold = true;
                ReportN.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                ReportN.Borders.LineStyle = Excel.XlLineStyle.xlLineStyleNone;



                //ENCABEZDO DE COLUMNAS
                Enc = Sheet.get_Range("A4", ColumnaFinal + 4);
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

        private void button2_Click(object sender, EventArgs e)
        {
            Generando_excel(Datos);
        }
    }
}
