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

namespace Sinconizacion_EXactus
{
    public partial class Reporte_Pedidos_facturas : Form
    {
        DataTable DT = new DataTable();
        String Encabezado;
        String empresa = Login.empresa;
        //conexion conet = new conexion();
        conexionXML con = new conexionXML();
        public Reporte_Pedidos_facturas()
        {
            InitializeComponent();
        }

        private void Reporte_Pedidos_facturas_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;
            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            //dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            
            button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DT.Columns.Clear();
            DT.Clear();
            dataGridView1.Refresh();


            switch (comboBox1.Text)
            {
                case "Tiempos de Impresion de Facturas":
                    con.conectar("DM");

                    SqlCommand cm1 = new SqlCommand("SELECT ped.VENDEDOR,COUNT(ped.[PEDIDO]) as 'PEDIDOS' , CONVERT(varchar,CAST(MAX(CAST(ped.CreateDate as FLOAT))as Datetime),8) as 'Hora de Proceso',COUNT(fac.FACTURA) as 'Facturados',CONVERT(varchar,CAST(MAX(CAST(fac.CreateDate as FLOAT))as Datetime),8) as 'Hora de Facturacion',DATEDIFF(MINUTE,CONVERT(varchar,CAST(MAX(CAST(ped.CreateDate as FLOAT))as Datetime),8),CONVERT(varchar,CAST(MAX(CAST(fac.CreateDate as FLOAT))as Datetime),8)) as 'Minutos Transcurridos'FROM [EXACTUS].[" + empresa + "].[PEDIDO] ped  left join   [EXACTUS].[" + empresa + "].[FACTURA] fac on ped.PEDIDO =fac.PEDIDO  where  ped.FECHA_PEDIDO = '" + dateTimePicker1.Value.ToString("yyyy/MM/dd") + "' and ped.RUTA like 'E%'  Group by ped.VENDEDOR order by ped.VENDEDOR", con.condm);
                    SqlDataAdapter da = new SqlDataAdapter(cm1);
                    da.Fill(DT);

                    con.Desconectar("DM");
                    dataGridView1.DataSource = DT;
                    dataGridView1.Refresh();
                    Encabezado = "REPORTE TIEMPOS DE IMPRESION FACTURAS";

                    break;

                case "Tiempos de Sincronizacion":

                    con.conectar("DM");
                    SqlCommand cmd2 = new SqlCommand("[CORRECT].[SINCRONIZACION]", con.condm);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da1 = new SqlDataAdapter(cmd2);

                    cmd2.Parameters.AddWithValue("@fechaini", dateTimePicker1.Value.ToString("yyyy/MM/dd"));
                    cmd2.Parameters.AddWithValue("@fechafin", dateTimePicker2.Value.ToString("yyyy/MM/dd"));

                    cmd2.ExecuteNonQuery();

                    da1.Fill(DT);

                    con.Desconectar("DM");
                    dataGridView1.DataSource = DT;
                    dataGridView1.Refresh();
                    Encabezado = "REPORTE TIEMPOS DE SINCRONIZACION";
                    break;

                case "Articulos Bonificados por REP":
                    con.conectar("DM");
                    SqlCommand cmd3 = new SqlCommand("[CORRECT].[ATICULOS_BONIFICADOS]", con.condm);
                    cmd3.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da3 = new SqlDataAdapter(cmd3);


                    cmd3.Parameters.AddWithValue("@fechaini", dateTimePicker1.Value.ToString("yyyy/MM/dd"));
                    cmd3.Parameters.AddWithValue("@fechafin", dateTimePicker2.Value.ToString("yyyy/MM/dd"));
                    cmd3.Parameters.AddWithValue("@empresa", Login.empresa);
                    cmd3.ExecuteNonQuery();

                    da3.Fill(DT);

                    con.Desconectar("DM");
                    dataGridView1.DataSource = DT;
                    dataGridView1.Refresh();
                    Encabezado = "Articulos Bonificados por REP";


                    break;
            

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

        private void Generando_excel()
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

            Generando_excel();



        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            int registros = dataGridView1.RowCount;
            if (registros <= 0)
            {
            }
            else
            {
                button1.Enabled = true;
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Tiempos de Impresion de Facturas")
            {
                label4.Hide();
                dateTimePicker2.Hide();
            }
            else if (comboBox1.Text == "Tiempos de Sincronizacion")
            {
                label4.Show();
                dateTimePicker2.Show();
            
            }
        }




    }
}
