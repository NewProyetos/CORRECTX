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


namespace Sinconizacion_EXactus.CORECTX_APP.SEGURIDAD
{
    public partial class Reporte_Accesos : Form
    {

         


        public Reporte_Accesos()
        {
            InitializeComponent();

            exp.OnProgressUpdate += exp_Process;
            
        }

        conexionXML con = new conexionXML();
        DataTable Accesos = new DataTable();
        DataSet ds = new DataSet();
        Exportador exp = new Exportador();
        DataTable result = new DataTable();
        DataTable rangofecha = new DataTable();
        DataTable empleadoseg = new DataTable();
        public static DataTable emplnomarcados= new DataTable();

        private void exp_Process(int value)
        {
            base.Invoke((Action)delegate
            {
                int per = (value + 1) * 100 / Accesos.Rows.Count;
              //  label3.Text = "Cargando Registros  " + Convert.ToString(value + 1);
               // progressBar1.Value = per;
            });

        }




        private void Reporte_Accesos_Load(object sender, EventArgs e)
        {
            emplnomarcados.Clear();
            rangofecha.Columns.Add("FECHA", typeof(string));

            if (emplnomarcados.Columns.Contains("TARJETA"))
            {

            }
            else
            {
                emplnomarcados.Columns.Add("TARJETA", typeof(string));
            }

            if (emplnomarcados.Columns.Contains("CARNET"))
            {

            }
            else
            {
                emplnomarcados.Columns.Add("CARNET", typeof(string));
            }

            if (emplnomarcados.Columns.Contains("NOMBRE"))
            {

            }
            else
            {
                emplnomarcados.Columns.Add("NOMBRE", typeof(string));
            }

            if (emplnomarcados.Columns.Contains("FECHA"))
            {

            }
            else
            {
                emplnomarcados.Columns.Add("FECHA", typeof(string));
            }

            if (emplnomarcados.Columns.Contains("EMPRESA"))
            {

            }
            else
            {
                emplnomarcados.Columns.Add("EMPRESA", typeof(string));
            }
           
           

            toolStripTextBox1.Enabled = false;
            toolStripTextBox2.Enabled = false;
            toolStripTextBox3.Enabled = false;
            dataGridView1.Enabled = true;
            button2.Enabled = false;
            dataGridView1.RowHeadersVisible = false;
            //dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            Accesos.TableName = "AccesosF";
            result.TableName = "Accesos";
            empleados();
            //progressBar1.Hide();
        }

       
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
             
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fechas();
            toolStripTextBox1.Enabled = false;
            toolStripTextBox2.Enabled = false;
            toolStripTextBox3.Enabled = false;
            string fecini = dateTimePicker1.Value.ToString("yyyy/dd/MM");
            string fecfin = dateTimePicker2.Value.ToString("yyyy/dd/MM");

            Carga_accesos(fecini,fecfin);

            no_marcados();
           // label3.Hide();
            //progressBar1.Hide();
                
            
        }


        private void Carga_accesos(string fecini,string fecfin)
        {
            Accesos.Clear();
            
            con.conectar("SEG");
            SqlCommand cmd = new SqlCommand("[dbo].[Reporte_Accesos]", con.conseg);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@fechaini ", fecini);
            cmd.Parameters.AddWithValue("@fechafin", fecfin);


            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(Accesos);

            dataGridView1.DataSource = Accesos;
           
            con.Desconectar("SEG");
        
        
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            Accesos.DefaultView.RowFilter = string.Format("Convert(NOMBRE,'System.String') like '%{0}%'", this.toolStripTextBox1.Text);
            dataGridView1.DataSource = Accesos;
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            int registros = dataGridView1.RowCount;

            if (registros >= 1)
            {
                toolStripTextBox1.Enabled = true;
                toolStripTextBox2.Enabled = true;
                toolStripTextBox3.Enabled = true;
                button2.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //if (ds.Tables.Contains("Accesos"))
            //{
            //    ds.Tables.Remove(result);
            //}



            //filtro();

            //ds.Tables.Add(result);
            //exp.NombreReporte = "CONTROL DE ENTRADA Y SALIDAS";
            //progressBar1.Show();
            //label3.Show();
            //exp.aExcel(ds, dateTimePicker1.Value, dateTimePicker2.Value);

            copyall();
            Generando_excel();

        }

        private void filtro()
        {
            result.Clear();
            result.Columns.Clear();
           // DataTable dt = new DataTable();
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                result.Columns.Add(col.HeaderText);
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataRow dRow = result.NewRow();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dRow[cell.ColumnIndex] = cell.Value;
                }
                result.Rows.Add(dRow);
            }

        
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {
           // progressBar1.Hide();
            //label3.Hide();
            toolStripTextBox2.Text = "";
            toolStripTextBox3.Text = "";
            
        }

        private void toolStripTextBox2_Click(object sender, EventArgs e)
        {
            //progressBar1.Hide();
            //label3.Hide();
            toolStripTextBox1.Text = "";
            toolStripTextBox3.Text = "";
            
        }

        private void toolStripTextBox2_TextChanged(object sender, EventArgs e)
        {
            Accesos.DefaultView.RowFilter = string.Format("Convert(CARNET,'System.String') like '%{0}%'", this.toolStripTextBox2.Text);
            dataGridView1.DataSource = Accesos;
        }

        private void toolStripTextBox3_Click(object sender, EventArgs e)
        {
            //progressBar1.Hide();
            //label3.Hide();
            toolStripTextBox1.Text = "";
            toolStripTextBox2.Text = "";
        }

        private void toolStripTextBox3_TextChanged(object sender, EventArgs e)
        {
            Accesos.DefaultView.RowFilter = string.Format("Convert(TARJETA,'System.String') like '%{0}%'", this.toolStripTextBox3.Text);
            dataGridView1.DataSource = Accesos;
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
            String Encabezado = "REPORTES  ENTRADAS y SALIDAS";

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

            col = Accesos.Columns.Count / 26;

            string Letracol = "ABCDEFEHIJKLMNOPQRSTUVWXYZ";
            string Complementocol;
            //Determinando la letra que se usara despues de la columna 26
            if (col > 0)
            {
                Columnas = Accesos.Columns.Count - (26 * col);
                Complementocol = Letracol.ToString().Substring(col - 1, 1);
            }
            else
            {
                Columnas = Accesos.Columns.Count;
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




            for (int c = 0; c < Accesos.Columns.Count; c++)
            {

                Sheet.Cells[3, c + 1] = String.Format("{0}", Accesos.Columns[c].Caption);
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

        private void fechas()
        {
            rangofecha.Clear();
            DateTime fechaini = dateTimePicker1.Value.Date;
            DateTime fechafin = dateTimePicker2.Value.Date;

            TimeSpan listfecha = fechafin.Subtract(fechaini);


            for (int i = 0; i<listfecha.TotalDays+1  ; i++)
            {
                DateTime resultdate = fechaini.AddDays(i);

                rangofecha.Rows.Add(resultdate.ToString("dd/MM/yyyy"));

             //   MessageBox.Show("ss");
            }
        
        }

        public void empleados()
        {
            empleadoseg.Clear();
            con.conectar("SEG");
            SqlCommand cmd = new SqlCommand("SELECT acces.[ID] as TARJETA,[EMPID] ,EMPLE.SSNO as CARNET,UPPER(RTRIM(LTRIM(ISNULL(EMPLE.FIRSTNAME,''))))+' '+ UPPER(RTRIM(LTRIM(isnull(EMPLE.MIDNAME,''))))+' '+UPPER(RTRIM(LTRIM(ISNULL(EMPLE.LASTNAME,''))))  AS NOMBRE , CASE WHEN DIREC.BUILDING =0 THEN DIREC.ADDR1 ELSE (SELECT NAME FROM [dbo].[BUILDING] BL WHERE BL.ID= BUILDING )  END AS EMPRESA  FROM [ACCESSCONTROL].[dbo].[BADGE] acces  inner join  [ACCESSCONTROL].[dbo].[EMP] EMPLE  on acces.EMPID = EMPLE.ID  LEFT JOIN [ACCESSCONTROL].[dbo].[UDFEMP] DIREC  on  EMPLE.ID = DIREC.ID  where STATUS = '1'", con.conseg);
            SqlDataAdapter daemp = new SqlDataAdapter(cmd);
            daemp.Fill(empleadoseg);            
            con.Desconectar("SEG");
        
        }


        private void no_marcados()
        {
            emplnomarcados.Clear();

            for (int i = 0; i < rangofecha.Rows.Count;++i)
            {
                DataRow row = rangofecha.Rows[i];

                DateTime fec = Convert.ToDateTime(row["FECHA"]);
                string fecha = fec.ToString("dd/MM/yyyy hh:mm:ss tt");
                
                 for (int j = 0; j < empleadoseg.Rows.Count; j++)
                     {
                       DataRow rowem = empleadoseg.Rows[j];
                         string tarjeta = Convert.ToString(rowem["TARJETA"]);
                         string Nombre = Convert.ToString(rowem["NOMBRE"]);
                         string carnet = Convert.ToString(rowem["CARNET"]);
                         string empresa = Convert.ToString(rowem["EMPRESA"]);

                 DataRow[] foundfecha = Accesos.Select("FECHA = '" + fecha + "'AND TARJETA = '"+tarjeta+"'");
                 if (foundfecha.Length != 0)
                 {
                                           
                 }
                 else

                 {
                        DataRow[] exist_usr = emplnomarcados.Select("TARJETA = '" + tarjeta + "'");
                        if (foundfecha.Length != 0)
                        {
                        }
                        else
                        {
                            emplnomarcados.Rows.Add(tarjeta, carnet, Nombre, fecha, empresa);
                        }
                 }
                
            }

                 int nomarcados = emplnomarcados.Rows.Count;

                 if (nomarcados >= 1)
                 {
                     linkLabel1.Text = "EMPLEADOS SIN MARCACION : " + " " + nomarcados.ToString();

                 }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            No_Marcados nm = new No_Marcados();
            nm.ShowDialog();
               
        }
    }
}
