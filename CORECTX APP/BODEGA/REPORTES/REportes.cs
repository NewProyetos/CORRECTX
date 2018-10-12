using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Excel = Microsoft.Office.Interop.Excel;



namespace Sinconizacion_EXactus.CORECTX_APP.BODEGA
{
    public partial class REportes : Form
    {
        conexionXML con = new conexionXML();
        DataTable data = new DataTable();
        DataTable tablafil = new DataTable();
        DataTable tablafinal = new DataTable();
        int indx2;
        int incre;
        string Complementocol;
        int Columnas, col;
        // DataTable tablafiltro = new DataTable();
        String ARTICULO;
        String DESCRIPCION;
        String FAMILIA;
        String LINEA;
        String CATEGORIA;
        Double BODEGA_SA;
        Double PRONTA_SA;
        Double BODEGA_SS;
        Double PRONTA_SS;
        Double BODESA;
        Double MOBU;
        Double BODEGA_SM;
        Double PRONTA_SM;
        Double DIAS_INV_SA;
        Double DIAS_INV_SS_BODESA;
        Double SUM_INV_SS_BODESA;
        Double DIAS_INV_SS;
        Double DIAS_INV_SM;
        Double VENTA_SA;
        Double VENTA_SS;
        Double VENTA_SM;
        String empresa = Login.empresa;
        ConvertDT CONVERTDT = new ConvertDT();

        public REportes()
        {
            InitializeComponent();
        }

        private void REportes_Load(object sender, EventArgs e)
        {
            pictureBox1.Hide();
            pictureBox1.BackColor = Color.Transparent;
            radioButton1.Checked = true;
            numericUpDown1.Enabled = false;
            numericUpDown1.Value = 36;
            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            //dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ReadOnly = true; 

            groupBox2.Enabled = false;
            button2.Enabled = false;


            tablafil = new DataTable();
            tablafil.Columns.Add("ARTICULO", typeof(string));
            tablafil.Columns.Add("DESCRIPCION", typeof(string));
            tablafil.Columns.Add("FAMILIA", typeof(string));
            tablafil.Columns.Add("LINEA", typeof(string));
            tablafil.Columns.Add("CATEGORIA", typeof(string));
            tablafil.Columns.Add("BODEGA SA", typeof(double));
            tablafil.Columns.Add("PRONTA SA", typeof(double));
            tablafil.Columns.Add("BODEGA SS", typeof(double));
            tablafil.Columns.Add("PRONTA SS", typeof(double));
            tablafil.Columns.Add("BODESA", typeof(double));
            tablafil.Columns.Add("MOBU", typeof(double));
            tablafil.Columns.Add("BODEGA SM", typeof(double));
            tablafil.Columns.Add("PRONTA SM", typeof(double));
            tablafil.Columns.Add("VENTA DIARIA SA", typeof(double));
            tablafil.Columns.Add("VENTA DIARIA SS", typeof(double));
            tablafil.Columns.Add("VENTA DIARIA SM", typeof(double));
            tablafil.Columns.Add("DIAS INVENTARIO SA", typeof(double));
            tablafil.Columns.Add("DIAS INVENTARIO SS ", typeof(double));
            tablafil.Columns.Add("DIAS INVENTARIO SS + BODESA+MOBU", typeof(double));
            tablafil.Columns.Add("DIAS INVENTARIO SM", typeof(double));




        }

        private void button1_Click(object sender, EventArgs e)
        {
         //   loadWorkerdata.RunWorkerAsync();

           

            if (loadWorkerdata.IsBusy != true)
            {
                // wt = new CORECTX_APP.VENTAS.wait();
                //wt.ShowDialog();
                //clie_ruta.Clear();

                //groupBox5.Show();
               
                if (dataGridView1.Rows.Count >= 0)
                {
                    dgclean();
                }
                loadWorkerdata.RunWorkerAsync();

                pictureBox1.Show();
            }
            else
            {
                if (dataGridView1.Rows.Count >= 0)
                {
                    dgclean();
                }
            }




        }

        private void tbl_Dismo()
        {


            //for (int i = 0; i < data.Rows.Count; i++)
            //{
            //    BODEGA_SA = 0.0;
            //    PRONTA_SA = 0.0;
            //    BODEGA_SS = 0.0;
            //    BODESA = 0.0;
            //    PRONTA_SS = 0.0;
            //    BODEGA_SM = 0.0;
            //    PRONTA_SM = 0.0;
            //    VENTA_SA = 0.0;
            //    VENTA_SS = 0.0;
            //    VENTA_SM = 0.0;
            //    DIAS_INV_SA = 0.0;
            //    DIAS_INV_SS = 0.0;
            //    DIAS_INV_SM = 0.0;
            //    DIAS_INV_SS_BODESA = 0.0;

            //    DataRow row = data.Rows[i];
            //    ARTICULO = Convert.ToString(row["ARTICULO"]);
            //    DESCRIPCION = Convert.ToString(row["DESCRIPCION"]);
            //    FAMILIA = Convert.ToString(row["Familia"]);
            //    LINEA = Convert.ToString(row["Linea"]);
            //    CATEGORIA = Convert.ToString(row["Categoria"]);
            //    BODEGA_SA = Convert.ToDouble(row["Bodega SA"]);
            //    PRONTA_SA = Convert.ToDouble(row["Bodega Pronta SA"]);

            //    if (DBNull.Value == row["MOBU"])
            //    {
            //        MOBU = 0.0;
            //    }
            //    else
            //    {
            //        MOBU = Convert.ToDouble(row["MOBU"]);
            //    }

            //    if (DBNull.Value == row["Bodega SS"])
            //    {
            //        BODEGA_SS = 0.0;
            //    }
            //    else
            //    {
            //        BODEGA_SS = Convert.ToDouble(row["Bodega SS"]);
            //    }

            //    if (DBNull.Value == row["BODESA"]) 
            //    {

            //        BODESA = 0.0;
            //    }
            //    else
            //    {

            //    BODESA = Convert.ToDouble(row["BODESA"]);

            //    }
            //    PRONTA_SS = Convert.ToDouble(row["Bodegas Pronta SS"]);
            //    BODEGA_SM = Convert.ToDouble(row["Bodega SM"]);
            //    PRONTA_SM = Convert.ToDouble(row["Bodegas Pronta SM"]);
            //    VENTA_SA = Convert.ToDouble(row["VENTA DIARIA SA"]);
            //    if (DBNull.Value == row["VENTA DIARIA SS"])
            //    {
            //        VENTA_SS = 0.0;
            //    }
            //    else
            //    {
            //        VENTA_SS = Convert.ToDouble(row["VENTA DIARIA SS"]);
            //    }
            //    VENTA_SM = Convert.ToDouble(row["VENTA DIARIA SM"]);


            //    if (BODEGA_SA == 0.0 || VENTA_SA == 0.0)
            //    {
            //        DIAS_INV_SM = 0.0;
            //    }
            //    else
            //    {
            //        DIAS_INV_SA = Math.Round(BODEGA_SA / VENTA_SA);
            //    }

            //    if (VENTA_SS == 0.0)
            //    {
            //        DIAS_INV_SS_BODESA = 0.0;
            //        DIAS_INV_SS = 0.0;
            //    }
            //    else
            //    {
            //        DIAS_INV_SS = Math.Round(BODEGA_SS / VENTA_SS);



            //            SUM_INV_SS_BODESA = (BODEGA_SS + BODESA+ MOBU);
            //            DIAS_INV_SS_BODESA = Math.Round(SUM_INV_SS_BODESA / VENTA_SS);


            //    }

            //    if (BODEGA_SM == 0.0 || VENTA_SM == 0.0)
            //    {
            //        DIAS_INV_SM = 0.0;
            //    }
            //    else
            //    {
            //        DIAS_INV_SM = Math.Round(BODEGA_SM / VENTA_SM);
            //    }

            //   // if (BODEGA_SA + BODEGA_SS + BODEGA_SM + BODESA + MOBU> 0)
            //   // {
            //        tablafil.Rows.Add(ARTICULO, DESCRIPCION, FAMILIA, LINEA, CATEGORIA, BODEGA_SA, PRONTA_SA, BODEGA_SS, PRONTA_SS, BODESA,MOBU, BODEGA_SM, PRONTA_SM, VENTA_SA, VENTA_SS, VENTA_SM, DIAS_INV_SA, DIAS_INV_SS, DIAS_INV_SS_BODESA, DIAS_INV_SM);


            //    //}

            //}

          
        
        }
        private void join(DataTable tabla1 , DataTable tabla2,string campo)
        {
            var results = from table1 in tabla1.AsEnumerable()
                          join table2 in tabla2.AsEnumerable() on (int)Convert.ToInt32(table1["CLV"]) equals (int)Convert.ToInt32(table2["CVE_CLIE"])
                          //  where Convert.ToString(table2["ESTADO"]) == var_estado
                          orderby (table2["FECHA_IN"]) descending

                          select new
                          {
                              //ID = table1,
                              NUM_ORDEN = (string)Convert.ToString(table2["COD_ORDEN"]),
                              NUM_PEDIDO = (string)Convert.ToString(table2["PEDIDO_SAE"]),
                              NUM_CAJA = (string)Convert.ToString(table2["NUM_CAJA"]),
                              NOMBRE = (string)Convert.ToString(table1["NOMBRE"]),
                              PACIENTE = (string)Convert.ToString(table2["PACIENTE"]),
                              FECHA_IN = (string)Convert.ToString(table2["FECHA_IN"]),
                              ESTADO = (string)Convert.ToString(table2["ESTADO"]),
                              ESTADO_PROCESO = (string)Convert.ToString(table2["ESTADO_LAB"]),
                              PROCESO_LABO = (string)Convert.ToString(table2["ESTACIONES_NAME"]),
                              INGRESO_LAB = (string)Convert.ToString(table2["ING_LAB"]),
                              FECHA_FACT = (string)Convert.ToString(table2["FECHA_FACTURA"]),
                              COD_CLIE = (string)Convert.ToString(table2["CVE_CLIE"]),
                              VENDEDOR = (string)Convert.ToString(table2["VENDEDOR"]),
                              USUARIO = (string)Convert.ToString(table2["USUARIO_MOD"])
                          };

            tablafinal = CONVERTDT.ConvertToDataTable(results);
        
            
    }

       
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                numericUpDown1.Enabled = false;
            }
            else
                if (radioButton2.Checked)
                {
                numericUpDown1.Enabled = true;
                }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                numericUpDown1.Enabled = false;
            }
            else
                if (radioButton2.Checked)
                {
                    numericUpDown1.Enabled = true;
                }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
                data.DefaultView.RowFilter = "Linea like '%" + this.comboBox2.Text + "%'";
                dataGridView1.DataSource = data;
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
                data.DefaultView.RowFilter = "Familia like '%" + this.comboBox1.Text + "%'";
                dataGridView1.DataSource = data;

                combolinea(data, comboBox1.Text);
                comobocategoria(data, comboBox1.Text);


           
          //  comboBox2.DataSource = data;
           // comboBox2.DisplayMember = "Linea";
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
           
                data.DefaultView.RowFilter = "Familia like '%" + this.comboBox1.Text + "%'";
                dataGridView1.DataSource = data;
                combolinea(data, comboBox1.Text);
                comobocategoria(data, comboBox1.Text);
            
        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
           
                data.DefaultView.RowFilter = "Linea like '%" + this.comboBox2.Text + "%'";
                dataGridView1.DataSource = data;
            
        }

        public void combolinea(DataTable dts, string filtro)
        {
            // string[] variable;

            //var result2 = variable;
            comboBox2.Items.Clear();

            if (filtro == null || filtro == string.Empty || filtro == "")
            {
                var result2 = from row1 in dts.AsEnumerable()                             
                              group row1 by row1.Field<string>("Linea")

                                              into ln
                              select new
                              {

                                  Linea = ln.Key,

                              };
                foreach (var t1 in result2)
                {
                    if (t1.Linea == null || t1.Linea == "")
                    {

                    }
                    else
                    {
                        comboBox2.Items.Add(t1.Linea);
                    }
                }

            }
            else
            {
                var result2 = from row1 in dts.AsEnumerable()
                              where row1.Field<string>("familia") == filtro
                              group row1 by row1.Field<string>("Linea")

                              into ln
                              select new
                              {

                                  Linea = ln.Key,

                              };
                foreach (var t1 in result2)
                {
                    if (t1.Linea == null || t1.Linea == "")
                    {

                    }
                    else
                    {
                        comboBox2.Items.Add(t1.Linea);
                    }
                }


            }
           


        }

        public void comobocategoria(DataTable dts, string familia)
        {
            comboBox3.Items.Clear();

            if (familia == null || familia == string.Empty || familia == "")
            {
                var result2 = from row1 in dts.AsEnumerable()
                              group row1 by row1.Field<string>("categoria")

                                              into ln
                              select new
                              {

                                  Linea = ln.Key,

                              };
                foreach (var t1 in result2)
                {
                    if (t1.Linea == null || t1.Linea == "")
                    {

                    }
                    else
                    {
                        comboBox3.Items.Add(t1.Linea);
                    }
                }

            }
            else
            {
                var result2 = from row1 in dts.AsEnumerable()
                              where row1.Field<string>("familia") == familia
                              group row1 by row1.Field<string>("categoria")

                              into ln
                              select new
                              {

                                  Linea = ln.Key,

                              };
                foreach (var t1 in result2)
                {
                    if (t1.Linea == null || t1.Linea == "")
                    {

                    }
                    else
                    {
                        comboBox3.Items.Add(t1.Linea);
                    }
                }


            }

        }

        public void combofam (DataTable dts)
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();

            var result = from row in dts.AsEnumerable()
                         group row by row.Field<string>("Familia") into grp
                         select new
                         {
                             familia = grp.Key,

                         };
            foreach (var t in result)
            {
                if (t.familia == null || t.familia == "")
                {

                }
                else
                {
                    comboBox1.Items.Add(t.familia);
                }
            }

          

            var result3 = from row2 in dts.AsEnumerable()
                          group row2 by row2.Field<string>("categoria") into lns
                          select new
                          {
                              categoria = lns.Key,
                              
                          };
            foreach (var t2 in result3)
            {
                if (t2.categoria == null || t2.categoria == "")
                {

                }
                else
                {
                    comboBox3.Items.Add(t2.categoria);
                }
            }
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

        private void button2_Click(object sender, EventArgs e)
        {
            envexcel(data);
        }
        private void envexcel( DataTable dtex)
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


           

           
            int fila = dtex.Rows.Count + 3;
           
                col = dtex.Columns.Count / 26;
           
            string Letracol = "ABCDEFEHIJKLMNOPQRSTUVWXYZ";
           
            //Determinando la letra que se usara despues de la columna 26


            string activecell = "A1";
            string activecell2 = Regex.Replace(activecell, @"[^\d]", "");
            int cant_lt = (activecell.Replace("$", "").Length) - activecell2.Length;
            string letra = activecell.Substring(0, cant_lt);


            int indx = Letracol.IndexOf(letra.Substring(0, 1));

            if (col > 0)
            {
                Columnas = dtex.Columns.Count - (26 * col);
                Complementocol = Letracol.ToString().Substring(col - 1, 1);
            }
            if (cant_lt > 1)
            {
                indx2 = Letracol.IndexOf(letra.Substring(1, 1));
                if (cant_lt == 2)
                {
                    if (dtex.Columns.Count + indx > 26)
                    {
                        Columnas = dtex.Columns.Count - 1;

                        Complementocol = Letracol.ToString().Substring(indx2 + 1, 1);
                        incre = Encoding.ASCII.GetBytes(letra.Substring(0, 1))[0];

                        incre = incre - dtex.Columns.Count;
                    }
                    else
                    {
                        Columnas = dtex.Columns.Count;
                        Complementocol = Letracol.ToString().Substring(indx2, 1);
                        incre = Encoding.ASCII.GetBytes(letra.Substring(1, 1))[0];


                    }
                }
            }
            else
           if (dtex.Columns.Count + indx > 26)
            {

                Columnas = dtex.Columns.Count - 1;
                Complementocol = Letracol.ToString().Substring(0, 1);
                incre = Encoding.ASCII.GetBytes(Complementocol)[0];


            }
            else
            {
                Columnas = dtex.Columns.Count;
                Complementocol = "";
                incre = Encoding.ASCII.GetBytes(letra)[0];
            }
            string ColumnaFinal;







            ColumnaFinal = Complementocol.ToString() + Convert.ToChar((incre + Columnas) - 1).ToString();

            //if (col > 0)
            //{
            //    Columnas = dtex.Columns.Count - (26 * col);
            //    Complementocol = Letracol.ToString().Substring(col - 1, 1);
            //}
            //else
            //{
            //    Columnas = dtex.Columns.Count;
            //    Complementocol = "";
            //}
            //string ColumnaFinal;

            //incre = Encoding.ASCII.GetBytes("A")[0];

            //ColumnaFinal = Complementocol.ToString() + Convert.ToChar(incre + Columnas - 1).ToString();


            workbook = excell.Workbooks.Add(miobj);
            Sheet = workbook.Worksheets.get_Item(1);

            Excel.Range rg = Sheet.Cells[4, 1];
            Excel.Range Enc;
            Excel.Range RN;
            Excel.Range Report;
            rg.Select();




            for (int c = 0; c < dtex.Columns.Count; c++)
            {

                Sheet.Cells[3, c + 1] = String.Format("{0}", dtex.Columns[c].Caption);
            }


            Sheet.PasteSpecial(rg, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);

            //try
            //{
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


                Sheet.Cells[2, 1] = "DIAS INVETARIO" + " EMISION " + DateTime.Now.ToString();



                Report.Select();
                Report.Merge();
                Report.Font.Bold = true;
                Report.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;



                //ENCABEZDO DE COLUMNAS
                Enc = Sheet.get_Range("A3", ColumnaFinal + 3);
                Enc.Font.Name = "Times New Roman";
                Enc.Font.Size = 9;
                Enc.Borders.LineStyle = Excel.XlLineStyle.xlDouble;
                Enc.Font.Bold = true;

            //}
            //catch (SystemException exec)
            //{
            //    MessageBoxButtons bt1 = MessageBoxButtons.OK;
            //    DialogResult result = MessageBox.Show(exec.ToString(), "!!!!ERROR!!!!", bt1, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);


            //}
            
        }


        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
                button2.Enabled = true;
                groupBox2.Enabled = true;
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            data.DefaultView.RowFilter = "categoria like '%" + this.comboBox3.Text + "%'";
            dataGridView1.DataSource = data;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            DateTime fecha = DateTime.Now;
            data.Clear();

            tablafil.Clear();

            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("[" + empresa + "].[REPORTE_ANALISIS_INVENTARIO]", con.conex);
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;

            if (radioButton1.Checked)
            {

                cmd.Parameters.AddWithValue("@Filtro", 1);
                cmd.Parameters.AddWithValue("@ing_dias", DBNull.Value);
            }
            else if (radioButton2.Checked)
            {

                cmd.Parameters.AddWithValue("@Filtro", 2);
                cmd.Parameters.AddWithValue("@ing_dias", numericUpDown1.Value.ToString());
            }

            cmd.Parameters.AddWithValue("@Dias_fil", DBNull.Value);
            cmd.Parameters.AddWithValue("@fecha_ini", DBNull.Value);
            cmd.Parameters.AddWithValue("@fecha_fin", fecha);
            cmd.Parameters.AddWithValue("@dias_habiles", DBNull.Value);
            cmd.Parameters.AddWithValue("@frist_day", DBNull.Value);
            cmd.Parameters.AddWithValue("@dia_festivos", DBNull.Value);




           // cmd.CommandTimeout = 50;


            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(data);



            con.conectar("EX");

        }

        private void loadWorkerdata_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            combofam(data);
            combolinea(data, "");
            comobocategoria(data, "");


           // groupBox2.Enabled = true;

            //tbl_Dismo();
            dtfill(data);
            pictureBox1.Hide();

        }

        private void loadWorkerdata_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void dtfill(DataTable dt)
        {
            dataGridView1.Refresh();
            dataGridView1.DataSource = dt;


        }

        private void dgclean()
        {

            //clie_ruta.Clear();
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();

        }
    }
}
