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
using System.IO;
using Microsoft.CSharp;

namespace Sinconizacion_EXactus
{
    public partial class Reporte_venta_cobro : Form
    {
        public Reporte_venta_cobro()
        {
            InitializeComponent();
        }
        
        conexionXML con = new conexionXML();
        DataTable dt = new DataTable();

        public Excel.Application aplicacion;
        public Excel.Workbook libros_trabajo;
        private void Form15_Load(object sender, EventArgs e)
        {
            ControlBox = false;
            this.button2.Enabled = false;
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd-MM-yyyy";

            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "dd-MM-yyyy";

            dt.Clear();
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Refresh();

            con.conectar("EX");
            SqlCommand cmd2 = new SqlCommand("SELECT [VENDEDOR] FROM  [EXACTUS].[dismo].[FACTURA] GROUP BY [VENDEDOR]ORDER BY [VENDEDOR]", con.conex);
                SqlDataReader dr1 = cmd2.ExecuteReader();

                while (dr1.Read())
                {

                    comboBox1.Items.Add(dr1["VENDEDOR"]);
                }

                dr1.Close();
                con.Desconectar("EX");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value > this.dateTimePicker2.Value)
            {
                MessageBox.Show("Corregir Fechas");
            }

            else
            {
                dt.Clear();
                dataGridView1.Refresh();

                con.conectar("EX");

                SqlCommand cmd = new SqlCommand("[dismo].[DETALLE_VENTA]", con.conex);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@fechaini", this.dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@fechafin", this.dateTimePicker2.Value.ToString("yyyy-MM-dd"));
                if (this.comboBox1.Text == "")
                {
                    cmd.Parameters.AddWithValue("@ruta", null);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ruta", this.comboBox1.Text);
                }
                cmd.ExecuteNonQuery();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();

                con.Desconectar("EX");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
         
            Excel.Worksheet hoja_trabajo;

            Excel.Range rango;
            Excel.Range rango_enc;


            aplicacion = new Excel.Application();
            libros_trabajo = aplicacion.Workbooks.Add();

            hoja_trabajo = (Excel.Worksheet)libros_trabajo.Worksheets.get_Item(1);

            hoja_trabajo.Name = "DETALLE";

            rango = hoja_trabajo.get_Range("A1", "F1");

            rango_enc = hoja_trabajo.get_Range("A3", "F3");
            rango_enc.Font.Name = "Times New Roman";
            rango_enc.Font.Size = 10;
            rango_enc.Borders.LineStyle = Excel.XlLineStyle.xlDouble;


            rango.Font.Name = "Times New Roman";
            rango.Font.Color = Color.Blue;
            rango.Font.Size = 15;
            rango.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            rango_enc.Font.Bold = true;
            rango.Font.Bold = true;

            hoja_trabajo.Cells[1, 2] = "DETALLE DE VENTA RUTAS";
            hoja_trabajo.Cells[2, 2] = "FECHAS  "+ this.dateTimePicker1.Value.ToString("dd-MM-yyyy")+ "  A  "+ this.dateTimePicker2.Value.ToString("dd-MM-yyyy") +" ";


            hoja_trabajo.Cells[3, 1] = Convert.ToString(dt.Columns[0].ColumnName);
            hoja_trabajo.Cells[3, 2] = Convert.ToString(dt.Columns[1].ColumnName);
            hoja_trabajo.Cells[3, 3] = Convert.ToString(dt.Columns[2].ColumnName);
            hoja_trabajo.Cells[3, 4] = Convert.ToString(dt.Columns[3].ColumnName);
            hoja_trabajo.Cells[3, 5] = Convert.ToString(dt.Columns[4].ColumnName);
            hoja_trabajo.Cells[3, 6] = Convert.ToString(dt.Columns[5].ColumnName);


            for (int i = 0; i < dt.Rows.Count; i++)
            {

                for (int j = 0; j < dt.Columns.Count; j++)
                {


                    hoja_trabajo.Cells[i + 4, j + 1] = dt.Rows[i][j].ToString();
                }
            }



            aplicacion.Visible = true;
            

        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            button2.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            button2.Enabled = false;
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            button2.Enabled = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = false;
        }

    }
}
