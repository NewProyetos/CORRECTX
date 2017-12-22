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
    public partial class Reporte_Carera_XLS : Form
    {
        public Reporte_Carera_XLS()
        {
            InitializeComponent();
            exp.OnProgressUpdate += exp_Process;
        }
        DataTable CARTERA = new DataTable();
        DataSet ds = new DataSet();
        //conexion conec = new conexion();
        conexionXML con = new conexionXML();
        private Exportador exp = new Exportador();
        String empresa = Login.empresa;

        string Ruta;
        string cliente;
        //string status;

        private void exp_Process(int value)
        {
            base.Invoke((Action)delegate
            {
                //this.toolStripStatusLabel1.ForeColor = Color.Blue;
                //this.toolStripStatusLabel1.Text = "GENERANDO..";
                int per = (value + 1) * 100 / CARTERA.Rows.Count;
                label4.Text = "Cargando Registros  " + Convert.ToString(value + 1);
               // progressBar1.Value = per;
            });

        }
       
        private void Reporte_Carera_XLS_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;
            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            //dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ReadOnly = true; 
            
            this.comboBox1.Text = "Todos";
            con.conectar("EX");

            SqlCommand cm1 = new SqlCommand("SELECT Vendedor FROM [EXACTUS].["+empresa+"].[SoftlandBI_CC_SaldoClientes] GROUP BY Vendedor", con.conex);
            SqlDataReader dr1 = cm1.ExecuteReader();


            while (dr1.Read())
            {
                comboBox1.Items.Add(dr1["Vendedor"]);
            }

            dr1.Close();
           con.Desconectar("EX");
        }

        private void comboBox1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "Todos")
            {
                this.textBox1.Enabled = false;
            }
            else
            {
                this.textBox1.Enabled = true;
            }

        }

        private void textBox1_Click(object sender, EventArgs e)
        {
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Todos" || comboBox1.Text == "CXC" || comboBox1.Text == "ND")
            {
                label5.Text = comboBox1.Text;
            }
            else
            {

                con.conectar("EX");

                SqlCommand cm2 = new SqlCommand("SELECT NOMBRE FROM [EXACTUS].["+empresa+"].[VENDEDOR] Where VENDEDOR = '" + this.comboBox1.Text + "'", con.conex);
                SqlDataReader dr2 = cm2.ExecuteReader();


                while (dr2.Read())
                {
                    label5.Text = Convert.ToString(dr2["NOMBRE"]);
                }

                dr2.Close();
               con.Desconectar("EX");
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textBox1.Text.Trim().Length == 0 )
            {
                comboBox1.Enabled = true;
            }
            else
            {
                comboBox1.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CARTERA.Clear();
            dataGridView1.Refresh();
            Ruta = comboBox1.Text;
            cliente = textBox1.Text;
            this.toolStripStatusLabel1.ForeColor = Color.OrangeRed;
            this.toolStripStatusLabel1.Text = "CONECTANDO";
            label4.Text = "";
           
          
           backgroundWorker1.RunWorkerAsync();

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
           

            try
            {
               
                con.conectar("EX");


                SqlCommand cmd = new SqlCommand("[" + empresa + "].[ReporteCC]", con.conex);
                cmd.CommandTimeout = 120;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                if (cliente == "")
                {
                    cmd.Parameters.AddWithValue("@cliente", null);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@cliente", cliente);

                }

                if (Ruta == "Todos")
                {
                    cmd.Parameters.AddWithValue("@Ruta", null);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Ruta", Ruta);
                }

                SqlDataAdapter da1 = new SqlDataAdapter(cmd);
                da1.Fill(CARTERA);
               con.Desconectar("EX");
            }
            catch (SystemException exec)
            {
                MessageBoxButtons bt1 = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(exec.ToString(), "!!!!ERROR!!!!", bt1, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);


            }



        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            ds.Tables.Add(CARTERA);
            exp.NombreReporte = "REPORTE CARTERA CREDITO DISMO";
            exp.aExcel(ds);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           // backgroundWorker2.RunWorkerAsync();
            dataGridView1.Refresh();
            this.toolStripStatusLabel1.Text = "COMPLETADO";
            dataGridView1.DataSource = CARTERA;
        

           
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
           
        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            this.toolStripStatusLabel1.Text = "COMPLETADO";
        }
        private void copyall()
        {
            dataGridView1.SelectAll();
            DataObject dtobj = dataGridView1.GetClipboardContent();
            if (dtobj != null)
            {
                Clipboard.SetDataObject(dtobj);
            }
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int cellfin ;
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

            col = CARTERA.Columns.Count / 26;

               string Letracol = "ABCDEFEHIJKLMNOPQRSTUVWXYZ";
                string Complementocol;
                //Determinando la letra que se usara despues de la columna 26
                if (col > 0)
                {
                    Columnas = CARTERA.Columns.Count - (26 * col);
                    Complementocol = Letracol.ToString().Substring(col - 1, 1);
                }
                else
                {
                    Columnas = CARTERA.Columns.Count;
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

           
                      

            for (int c = 0; c < CARTERA.Columns.Count; c++)
            {

                Sheet.Cells[3, c + 1] = String.Format("{0}", CARTERA.Columns[c].Caption);
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

               
                    Sheet.Cells[2, 1] = "REPORTE DE CARTERA CREDITO"+ ""+ empresa + ""+ " EMISION " + DateTime.Now.ToString();
              
                    

                Report.Select();
                Report.Merge();
                Report.Font.Bold = true;
                Report.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;



                //ENCABEZDO DE COLUMNAS
                Enc = Sheet.get_Range("A3", ColumnaFinal + 3 );
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

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridView1.RowCount > 1)
            {

                button1.Enabled = true;
            }
        }
    }
}
