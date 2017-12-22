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


namespace Sinconizacion_EXactus.CORECTX_APP.BODEGA.REGALIAS
{
    public partial class Report_Libro : Form
    {
        public Report_Libro()
        {
            InitializeComponent();
        }
        DataTable bodegas = new DataTable();
        DataTable libro = new DataTable();
        conexionXML con = new conexionXML();
        String Bodlike;
        String fechaini;
        String fechafin;
        String Bodegaini;
        String Bodegafin;
        string empresa;

        private void Report_Libro_Load(object sender, EventArgs e)
        {
            pictureBox1.Hide();
            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            //dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ReadOnly = true;


            empresa = Login.empresa;

            if (empresa == "DISMO")
            {
                Bodlike = "B%";
            }
            else if (empresa == "DISMOGT")
            {
                Bodlike = "G%";
            }


            con.conectar("EX");
            SqlCommand cmd2 = new SqlCommand();
            cmd2 = new SqlCommand("SELECT [BODEGA],[NOMBRE] FROM [EXACTUS].["+empresa+ "].[BODEGA]  where BODEGA like '"+Bodlike+"' order by BODEGA ", con.condm);                      
            SqlDataAdapter da1 = new SqlDataAdapter(cmd2);
            da1.Fill(bodegas);
            con.Desconectar("EX");

            combo(bodegas);
        }

        public void combo(DataTable dts)
        {
            //comboBox1.Items.Clear();
            

            var result = from row in dts.AsEnumerable()
                         group row by row.Field<string>("BODEGA") into grp
                         select new
                         {
                             bodegas = grp.Key,

                         };
            foreach (var t in result)
            {
                if (t.bodegas == null || t.bodegas == "")
                {

                }
                else
                {
                    comboBox1.Items.Add(t.bodegas);
                    comboBox2.Items.Add(t.bodegas);
                }
            }
        }

        public void combolist(DataTable dts)
        {
            


            var result = from row in dts.AsEnumerable()
                         group row by row.Field<string>("familia") into grp
                         select new
                         {
                             bodegas = grp.Key,

                         };
            foreach (var t in result)
            {
                if (t.bodegas == null || t.bodegas == "")
                {

                }
                else
                {
                    toolStripComboBox1.Items.Add(t.bodegas);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataView dtv = new DataView(bodegas);

            dtv.RowFilter = "BODEGA = '" + this.comboBox1.Text + "'";
           string Nombre_bod = dtv[0]["NOMBRE"].ToString();
             label4.Text = Nombre_bod;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int Bodegain = Convert.ToInt32(comboBox1.Text.Substring(1,3));
            int Bodegafi = Convert.ToInt32(comboBox2.Text.Substring(1, 3));

            if (Bodegafi < Bodegain)
            {
                MessageBox.Show("Bodega Final No puede ser Mayor que la Bodega Inicial");
            }
            else
            {


                libro.Clear();
                dataGridView1.DataSource = null;
                dataGridView1.Refresh();
                //dataGridView1.Refresh();


                fechaini = this.dateTimePicker1.Value.ToString("yyyy/MM/dd");
                fechafin = this.dateTimePicker2.Value.ToString("yyyy/MM/dd");
                Bodegaini = comboBox1.Text;
                Bodegafin = comboBox2.Text;
                CargadataWorke.RunWorkerAsync();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataView dtvs = new DataView(bodegas);

            dtvs.RowFilter = "BODEGA = '" + this.comboBox2.Text + "'";
            string Nombre_bod2 = dtvs[0]["NOMBRE"].ToString();
            label6.Text = Nombre_bod2;
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            libro.DefaultView.RowFilter = "Familia like '" + this.toolStripComboBox1.Text + "%'";
            dataGridView1.DataSource = libro;
        }

        private void toolStripTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            libro.DefaultView.RowFilter = "codigo like '" + this.toolStripTextBox1.Text + "%'";
            dataGridView1.DataSource = libro;
        }

        private void toolStripComboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            libro.DefaultView.RowFilter = "Familia like '" + this.toolStripComboBox1.Text + "%'";
            dataGridView1.DataSource = libro;
        }

        private void CargadataWorke_DoWork(object sender, DoWorkEventArgs e)
        {

           
            CargadataWorke.ReportProgress(0, "1");
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("[dismo].[LIBROINVDM]", con.conex);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@artini", DBNull.Value);
            cmd.Parameters.AddWithValue("@artfin", DBNull.Value);
            cmd.Parameters.AddWithValue("@fechaini", fechaini+ " 01:00:00.000");
            cmd.Parameters.AddWithValue("@fechafin", fechafin+ " 23:59:00.000");
            cmd.Parameters.AddWithValue("@BODEGAINI", Bodegaini);
            cmd.Parameters.AddWithValue("@BODEGAFIN", Bodegafin);
            cmd.Parameters.AddWithValue("@EMPRESA", empresa);

            cmd.CommandTimeout = 0;


            cmd.ExecuteNonQuery();
           

            da.Fill(libro);

            con.conectar("EX");
        }

        private void CargadataWorke_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pictureBox1.Hide();
          
            dtfill(libro);
        }

        private void CargadataWorke_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState.ToString() == "1")
            {
                pictureBox1.Show();
            }
        }
        private void dtfill(DataTable dt)
        {
            
            dataGridView1.DataSource = dt;
            dataGridView1.Refresh();
            combolist(libro);

        }

        private void copyall()
        {
            int cellfin;
            cellfin = dataGridView1.ColumnCount;

            dataGridView1.SelectAll();
            DataObject dtobj = dataGridView1.GetClipboardContent();
            if (dtobj != null)
            {
                Clipboard.SetDataObject(dtobj);
            }

        }



        private void sendexcel(DataGridView drg)
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

            col = drg.Columns.Count / 26;

            string Letracol = "ABCDEFEHIJKLMNOPQRSTUVWXYZ";
            string Complementocol;
            //Determinando la letra que se usara despues de la columna 26
            if (col > 0)
            {
                Columnas = drg.Columns.Count - (26 * col);
                Complementocol = Letracol.ToString().Substring(col - 1, 1);
            }
            else
            {
                Columnas = drg.Columns.Count;
                Complementocol = "";
            }

            string ColumnaFinal;

            incre = Encoding.ASCII.GetBytes("A")[0];

            ColumnaFinal = Complementocol.ToString() + Convert.ToChar(incre + Columnas - 1).ToString();


            workbook = excell.Workbooks.Add(miobj);
            Sheet = workbook.Worksheets.get_Item(1);

            Excel.Range rg = Sheet.Cells[5, 1];
            Excel.Range Enc;
            Excel.Range det;
            Excel.Range RN;
            Excel.Range Report;
            Excel.Range Reportxt;
            rg.Select();

            // obtener colummnas de encabezado






            for (int c = 0; c < drg.Columns.Count; c++)
            {

                Sheet.Cells[4, c + 1] = String.Format("{0}", libro.Columns[c].Caption);
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


                Sheet.Cells[2, 1] = "LIBRO INVENTARIO" + " RANGO FECHA " +fechaini+ " a "+fechafin;

                Report.Select();
                Report.Merge();
                Report.Font.Bold = true;
                Report.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;




                Reportxt = Sheet.get_Range("A3", ColumnaFinal + "3");
                Reportxt.Font.Name = "Times New Roman";
                Reportxt.Font.Size = 12;



                Sheet.Cells[3, 1] = "BODEGAS "+Bodegaini+"  a  "+Bodegafin+" ";

                Reportxt.Select();
                Reportxt.Merge();
                Reportxt.Font.Bold = true;
                Reportxt.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;




                //ENCABEZDO DE COLUMNAS
                Enc = Sheet.get_Range("A4", ColumnaFinal + 4);
                Enc.Font.Name = "Times New Roman";
                Enc.Font.Size = 9;
                Enc.Borders.LineStyle = Excel.XlLineStyle.xlDouble;
                Enc.Font.Bold = true;

                //DETALLE 
                //ENCABEZDO DE COLUMNAS


            }
            catch (SystemException exec)
            {
                MessageBoxButtons bt1 = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(exec.ToString(), "!!!!ERROR!!!!", bt1, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);


            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            copyall();
            sendexcel(dataGridView1);
        }
    }
}
