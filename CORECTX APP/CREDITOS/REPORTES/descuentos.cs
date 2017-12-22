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
    public partial class descuentos : Form
    {
        public descuentos()
        {
            InitializeComponent();
        }
        conexionXML con = new conexionXML();
        DataTable rutas = new DataTable();
        DataTable DESxFAMILIA = new DataTable();
        DataTable DESxART_CLIENTE = new DataTable();
        DataTable DESxCLIENTE = new DataTable();
        DataSet ds = new DataSet();
        Exportador exp = new Exportador();
        string consulta_0;
        string consulta_1;
        string consulta_2;

        string consulta_10;
        string consulta_11;
        string consulta_12;
        String RUTA;
        private void descuentos_Load(object sender, EventArgs e)
        {
            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            //dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;

            dataGridView2.Enabled = true;
            dataGridView2.RowHeadersVisible = false;
            //dataGridView1.AutoResizeColumns();
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView2.ReadOnly = true;
            dataGridView2.AllowUserToAddRows = false;

            dataGridView3.Enabled = true;
            dataGridView3.RowHeadersVisible = false;
            //dataGridView1.AutoResizeColumns();
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView3.ReadOnly = true;
            dataGridView3.AllowUserToAddRows = false;


            textBox1.Enabled = false;
            comboBox1.Enabled = false;
            carga_rutas();
            consulta_0 = "SELECT DCTO.CLIENTE,CLI.NOMBRE,CLI.ALIAS,CLI.VENDEDOR,CLI.RUTA AS ENTREGA, DCTO.CLASIFICACION ,DCTO.CANTIDAD_INICIAL,DCTO.CANTIDAD_FINAL,DCTO.PORC_DESCUENTO,DCTO.TIPO_DESCUENTO,CLI.LIMITE_CREDITO,CLI.CONDICION_PAGO,FECHA_INICIAL,FECHA_FINAL FROM EXACTUS.DISMO.DCTO_CLAS_X_CLI DCTO INNER JOIN EXACTUS.DISMO.CLIENTE CLI ON DCTO.CLIENTE = CLI.CLIENTE";
             consulta_1 = "SELECT DCTO.CLIENTE,CLI.NOMBRE,CLI.ALIAS,CLI.VENDEDOR,CLI.RUTA AS ENTREGA ,DCTO.ARTICULO,AR.DESCRIPCION,DCTO.CANTIDAD_INICIAL,DCTO.CANTIDAD_FINAL ,DCTO.PORC_DESCUENTO,DCTO.TIPO_DESCUENTO,CLI.LIMITE_CREDITO,CLI.CONDICION_PAGO,FECHA_INICIAL,FECHA_FINAL FROM EXACTUS.DISMO.DCTO_ART_X_CLI DCTO INNER JOIN EXACTUS.DISMO.CLIENTE CLI ON DCTO.CLIENTE = CLI.CLIENTE INNER JOIN EXACTUS.DISMO.ARTICULO AR ON AR.ARTICULO = DCTO.ARTICULO";
             consulta_2 = "SELECT CLI.CLIENTE,CLI.NOMBRE,CLI.ALIAS,CLI.VENDEDOR,CLI.RUTA AS ENTREGA ,CLI.DESCUENTO,CLI.CONDICION_PAGO FROM EXACTUS.DISMO.CLIENTE CLI WHERE DESCUENTO>0";

            comboBox1.Items.Add("TODOS");


        }

        private void desc_por_familia(string consulta, DataTable tabla)
        {
            con.conectar("EX");
            SqlCommand cm2 = new SqlCommand(consulta, con.conex);
            SqlDataAdapter da2 = new SqlDataAdapter(cm2);
            da2.Fill(tabla);
            con.Desconectar("EX");
        }
          

        private void button1_Click(object sender, EventArgs e)
        {
            cargatabla();

          

        }
        private void cargatabla()
        {

            DESxART_CLIENTE.TableName = "DESxART_CLIENTE";
            DESxFAMILIA.TableName = "DESxFAMILIA";
            DESxCLIENTE.TableName = "DESxCLIENTE";

            if (ds.Tables.Contains("DESxART_CLIENTE"))
            {
                ds.Tables.Remove(DESxART_CLIENTE);
            }
            if (ds.Tables.Contains("DESxFAMILIA"))
            {
                ds.Tables.Remove(DESxFAMILIA);
            }
            if (ds.Tables.Contains("DESxCLIENTE"))
            {
                ds.Tables.Remove(DESxCLIENTE);
            }


           
            ds.Tables.Add(DESxFAMILIA);
            ds.Tables.Add(DESxART_CLIENTE);
            ds.Tables.Add(DESxCLIENTE);

            for (int i = 0; i <= ds.Tables.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        
                         desc_por_familia(consulta_0, ds.Tables[i]);
                            dataGridView1.DataSource = ds.Tables[i];
                        
                        break;
                    case 1:
                        desc_por_familia(consulta_1, ds.Tables[i]);
                        dataGridView2.DataSource = ds.Tables[i];
                        break;
                    case 2:
                        desc_por_familia(consulta_2, ds.Tables[i]);
                        dataGridView3.DataSource = ds.Tables[i];
                        break;

                }
            }




        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //exp.aExcel(ds);
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                comboBox1.Enabled = true;
                

            }
            else
            {
                comboBox1.Enabled = false;
            }
        }

        private void carga_rutas()
        {
            con.conectar("EX");

            SqlCommand cm1 = new SqlCommand("SELECT VENDEDOR,[NOMBRE]  FROM [EXACTUS].[dismo].[VENDEDOR]  where VENDEDOR <> 'ND' and VENDEDOR <> 'CXC' and ACTIVO = 'S' and NOMBRE not like '%INACTIVO%'  order by VENDEDOR", con.conex);
            SqlDataAdapter da1 = new SqlDataAdapter(cm1);
            da1.Fill(rutas);


            con.Desconectar("EX");
            combo(rutas);
        }

        private void combo(DataTable dts1)
        {

            comboBox1.Items.Clear();


            var result = from row in dts1.AsEnumerable()
                         group row by row.Field<string>("VENDEDOR") into grp
                         select new
                         {
                             Vendedor = grp.Key,

                         };
            foreach (var t in result)
            {
                if (t.Vendedor == null || t.Vendedor == "")
                {

                }
                else
                {
                    comboBox1.Items.Add(t.Vendedor);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "TODOS")
            {
                DESxART_CLIENTE.DefaultView.RowFilter = string.Format("VENDEDOR like '%{0}%'", "");
                dataGridView1.DataSource = DESxART_CLIENTE;

                DESxFAMILIA.DefaultView.RowFilter = string.Format("VENDEDOR like '%{0}%'", "");
                dataGridView2.DataSource = DESxFAMILIA;

                DESxCLIENTE.DefaultView.RowFilter = string.Format("VENDEDOR like '%{0}%'", "");
                dataGridView3.DataSource = DESxCLIENTE;
            }
            else
            {
                DESxART_CLIENTE.DefaultView.RowFilter = string.Format("VENDEDOR = '" + comboBox1.Text + "'");
                dataGridView2.DataSource = DESxART_CLIENTE;

                DESxFAMILIA.DefaultView.RowFilter = string.Format("VENDEDOR = '" + comboBox1.Text + "'");
                dataGridView1.DataSource = DESxFAMILIA;

                DESxCLIENTE.DefaultView.RowFilter = string.Format("VENDEDOR = '" + comboBox1.Text + "'");
                dataGridView3.DataSource = DESxCLIENTE;

            }
            //consulta_10 = "SELECT DCTO.CLIENTE,CLI.NOMBRE,CLI.ALIAS,CLI.VENDEDOR,CLI.RUTA AS ENTREGA, DCTO.CLASIFICACION ,DCTO.PORC_DESCUENTO,DCTO.TIPO_DESCUENTO,CLI.LIMITE_CREDITO,CLI.CONDICION_PAGO,FECHA_INICIAL,FECHA_FINAL FROM EXACTUS.DISMO.DCTO_CLAS_X_CLI DCTO INNER JOIN EXACTUS.DISMO.CLIENTE CLI ON DCTO.CLIENTE = CLI.CLIENTE where CLI.VENDEDOR = '"+comboBox1.Text+"' ";
            //consulta_11 = "SELECT DCTO.CLIENTE,CLI.NOMBRE,CLI.ALIAS,CLI.VENDEDOR,CLI.RUTA AS ENTREGA ,DCTO.ARTICULO,AR.DESCRIPCION ,DCTO.PORC_DESCUENTO,DCTO.TIPO_DESCUENTO,CLI.LIMITE_CREDITO,CLI.CONDICION_PAGO,FECHA_INICIAL,FECHA_FINAL FROM EXACTUS.DISMO.DCTO_ART_X_CLI DCTO INNER JOIN EXACTUS.DISMO.CLIENTE CLI ON DCTO.CLIENTE = CLI.CLIENTE INNER JOIN EXACTUS.DISMO.ARTICULO AR ON AR.ARTICULO = DCTO.ARTICULO where CLI.VENDEDOR = '" + comboBox1.Text + "' ";
            //consulta_12 = "SELECT CLI.NOMBRE,CLI.ALIAS,CLI.VENDEDOR,CLI.RUTA AS ENTREGA ,CLI.DESCUENTO,CLI.CONDICION_PAGO FROM EXACTUS.DISMO.CLIENTE CLI WHERE DESCUENTO>0 where CLI.VENDEDOR = '" + comboBox1.Text + "' ";
        }

        private void copyall(DataGridView dtg)
        {
            dtg.SelectAll();
            DataObject dtobj = dtg.GetClipboardContent();
            if (dtobj != null)
            {
                Clipboard.SetDataObject(dtobj);
            }

        }
        private void excel(DataTable dt , DataGridView dgrid)
        {
            int cellfin;
            cellfin = dgrid.ColumnCount;
            copyall(dgrid);

            Microsoft.Office.Interop.Excel.Application excell;
            Microsoft.Office.Interop.Excel.Workbook workbook;
            Microsoft.Office.Interop.Excel.Worksheet Sheet;
            object miobj = System.Reflection.Missing.Value;
            excell = new Excel.Application();
            excell.Visible = true;


            int incre;

            int Columnas, col;

            col = dt.Columns.Count / 26;

            string Letracol = "ABCDEFEHIJKLMNOPQRSTUVWXYZ";
            string Complementocol;
            //Determinando la letra que se usara despues de la columna 26
            if (col > 0)
            {
                Columnas = dt.Columns.Count - (26 * col);
                Complementocol = Letracol.ToString().Substring(col - 1, 1);
            }
            else
            {
                Columnas = dt.Columns.Count;
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




            for (int c = 0; c < dt.Columns.Count; c++)
            {

                Sheet.Cells[3, c + 1] = String.Format("{0}", dt.Columns[c].Caption);
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


                Sheet.Cells[2, 1] = "REPORTE DE DESCUENTOS" + " EMISION " + DateTime.Now.ToString();



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

            }
            catch (SystemException exec)
            {
                MessageBoxButtons bt1 = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(exec.ToString(), "!!!!ERROR!!!!", bt1, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);


            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Text == "DESCUENTOS_ ARICULO_ CLIENTE")
            {
                excel(DESxART_CLIENTE, dataGridView2);
            }
            else if (tabControl1.SelectedTab.Text == "DESCUENTO_FAMILIA")
            {
                excel(DESxFAMILIA, dataGridView1);

            }
            else if (tabControl1.SelectedTab.Text == "DESCUENTO_CLIENTE")
            {
                excel(DESxCLIENTE, dataGridView3);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            DESxART_CLIENTE.DefaultView.RowFilter = string.Format("Convert(CLIENTE,'System.String') like '%{0}%'", this.textBox1.Text);
            dataGridView1.DataSource = DESxART_CLIENTE;


            DESxFAMILIA.DefaultView.RowFilter = string.Format("Convert(CLIENTE,'System.String') like '%{0}%'", this.textBox1.Text);
            dataGridView2.DataSource = DESxFAMILIA;

            DESxCLIENTE.DefaultView.RowFilter = string.Format("Convert(CLIENTE,'System.String') like '%{0}%'", this.textBox1.Text);
            dataGridView3.DataSource = DESxCLIENTE;


        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            textBox1.Enabled = true;
        }
    }


    
}
