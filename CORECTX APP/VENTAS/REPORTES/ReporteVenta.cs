using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.Utils;
using DevExpress.XtraGrid;
using DevExpress.XtraEditors;


namespace Sinconizacion_EXactus.CORECTX_APP.VENTAS.REPORTES
{
    public partial class ReporteVenta : Form
    {
        public ReporteVenta()
        {
            InitializeComponent();
        }
        conexionXML con = new conexionXML();
        DataTable datos = new DataTable();
        String AREA;
        private void ReporteVenta_Load(object sender, EventArgs e)
        {
            DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;
            groupBox3.Hide();
            button2.Enabled = false;
            con.conectar("DM");
            SqlCommand cm2 = new SqlCommand("SELECT [E_MAIL]  FROM [EXACTUS].[dismo].[VENDEDOR] where ACTIVO = 'S'  group by E_MAIL", con.condm);
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
                comboBox1.Items.Add(dr2["E_MAIL"]);
            }
            dr2.Close();         


            con.Desconectar("DM");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                button2.Enabled = false;
                if (comboBox1.Text == "TODAS")
                {
                    AREA = "TODAS";
                }
                else
                {
                    AREA = comboBox1.Text;
                }
                    if (backgroundPedidos.IsBusy != true)
                {
                    groupBox3.Show();
                    backgroundPedidos.RunWorkerAsync();

                }


                }
            }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            AREA = comboBox1.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            string FileName = "C:\\CORRECT\\XLS\\ Ventas_"+AREA+"_" + dateTimePicker1.Value.ToString("ddddMMyyy") + ".xlsx";
            gridView1.ExportToXlsx(FileName);
            


            Microsoft.Office.Interop.Excel.Application excell;
            Microsoft.Office.Interop.Excel.Workbook workbook;
            excell = new Microsoft.Office.Interop.Excel.Application();
            excell.Visible = true;
            workbook = excell.Workbooks.Open(FileName);

      
    }

        private void gridView1_AsyncCompleted(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }

        private void backgroundPedidos_DoWork(object sender, DoWorkEventArgs e)
        {
            backgroundPedidos.ReportProgress(0, "Generando Reportes.");
            datos.Clear();
            con.conectar("DM");

            SqlCommand cmd = new SqlCommand("[CORRECT].[VENTA_DIARIA_OBJ]", con.condm);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 50;
            cmd.Parameters.AddWithValue("@fecha_venta", Convert.ToDateTime(this.dateTimePicker1.Value.ToString("yyyy/MM/dd")));
            //cmd.Parameters.AddWithValue("@empresa", Login.empresa);

            cmd.Parameters.AddWithValue("@fecha_fin", null);
            cmd.Parameters.AddWithValue("@dias_habiles", null);
            cmd.Parameters.AddWithValue("@frist_day", null);
            cmd.Parameters.AddWithValue("@dia_festivos", null);
            cmd.Parameters.AddWithValue("@Dias_fil", null);
            cmd.Parameters.AddWithValue("@ing_dias", null);
            cmd.Parameters.AddWithValue("@dias_venta", null);

            if (AREA == "TODAS")
            {
                cmd.Parameters.AddWithValue("@AREA", null);
            }
            else
            {
                cmd.Parameters.AddWithValue("@AREA", AREA);
            }


            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(datos);




            con.Desconectar("DM");

        }

        private void backgroundPedidos_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dtfill(datos);

        }

        private void dtfill(DataTable dt)
        {
            gridControl1.Refresh();
            gridControl1.DataSource = datos;
            gridView1.Columns[4].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns[4].DisplayFormat.FormatString = "c2";
            gridView1.Columns[5].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns[5].DisplayFormat.FormatString = "c2";
            gridView1.Columns[6].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns[6].DisplayFormat.FormatString = "c2";
            gridView1.Columns[7].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns[7].DisplayFormat.FormatString = "c2";
            gridView1.Columns[8].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns[8].DisplayFormat.FormatString = "c2";
            gridView1.Columns[9].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns[9].DisplayFormat.FormatString = "c2";
            gridView1.Columns[10].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns[10].DisplayFormat.FormatString = "c2";
            gridView1.Columns[11].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns[11].DisplayFormat.FormatString = "c2";
            gridView1.Columns[12].DisplayFormat.FormatType = FormatType.Numeric;
            gridView1.Columns[12].DisplayFormat.FormatString = "c2";
            gridView1.Columns[13].DisplayFormat.FormatType = FormatType.Numeric;
           // gridView1.Columns[13].DisplayFormat.FormatString = "P";



            GridFormatRule gridFormatRule = new GridFormatRule();
            FormatConditionRuleIconSet formatConditionRuleIconSet = new FormatConditionRuleIconSet();

            FormatConditionIconSet iconSet = formatConditionRuleIconSet.IconSet = new FormatConditionIconSet();

            FormatConditionIconSetIcon icon1 = new FormatConditionIconSetIcon();
            FormatConditionIconSetIcon icon2 = new FormatConditionIconSetIcon();
            FormatConditionIconSetIcon icon3 = new FormatConditionIconSetIcon();
            FormatConditionIconSetIcon icon4 = new FormatConditionIconSetIcon();
            FormatConditionIconSetIcon icon5 = new FormatConditionIconSetIcon();

            //Choose predefined icons.
            icon1.PredefinedName = "Arrows5_1.png";
            icon2.PredefinedName = "Arrows5_2.png";
            icon3.PredefinedName = "Arrows5_3.png";
            icon4.PredefinedName = "Arrows5_4.png";
            icon5.PredefinedName = "Arrows5_5.png";

            //Specify the type of threshold values.
            iconSet.ValueType = FormatConditionValueType.Number;


            //Define ranges to which icons are applied by setting threshold values.
            icon1.Value = 90; // target range: 67% <= value
            icon1.ValueComparison = FormatConditionComparisonType.GreaterOrEqual;
            icon2.Value = 70; // target range: 33% <= value < 67%
            icon2.ValueComparison = FormatConditionComparisonType.GreaterOrEqual;
            icon3.Value = 50; // target range: 0% <= value < 33%
            icon3.ValueComparison = FormatConditionComparisonType.GreaterOrEqual;
            icon4.Value = 25; // target range: 0% <= value < 33%
            icon4.ValueComparison = FormatConditionComparisonType.GreaterOrEqual;
            icon5.Value = 0; // target range: 0% <= value < 33%
            icon5.ValueComparison = FormatConditionComparisonType.GreaterOrEqual;

            //Add icons to the icon set.
            iconSet.Icons.Add(icon1);
            iconSet.Icons.Add(icon2);
            iconSet.Icons.Add(icon3);
            iconSet.Icons.Add(icon4);
            iconSet.Icons.Add(icon5);

            //Specify the rule type.
            gridFormatRule.Rule = formatConditionRuleIconSet;
            //Specify the column to which formatting is applied.
            gridFormatRule.Column = gridView1.Columns["ALCANCE"];
            //Add the formatting rule to the GridView.
            gridView1.FormatRules.Add(gridFormatRule);




            groupBox3.Hide();

            button2.Enabled = true;        


        }

        private void backgroundPedidos_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label3.Text = e.UserState.ToString();

        }
    }
}
