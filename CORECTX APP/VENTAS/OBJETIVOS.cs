using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Sinconizacion_EXactus.CORECTX_APP.VENTAS
{
    public partial class OBJETIVOS : Form
    {
        public OBJETIVOS()
        {
            InitializeComponent();
        }
        conexionXML con = new conexionXML();
        DataTable dt = new DataTable();
        int ID;
        decimal monto_up;
        decimal volume_up;
        string familia_up;
        private void OBJETIVOS_Load(object sender, EventArgs e)
        {
            comboBox1.Text = "VENDEDOR";
            carga_anos();
            gridView1.OptionsView.ColumnAutoWidth = false;
            gridView1.OptionsView.RowAutoHeight = true;
            gridView1.OptionsBehavior.Editable = false;
            gridView1.BestFitColumns();

            toolStripButton4.Enabled = false;
            toolStripButton1.Enabled = false;
            toolStripButton2.Enabled = false;
            toolStripButton3.Enabled = false;

        }

        private void cargaobjetivos(string tipo, string anio, string MES)
        {
            if (tipo == "VENDEDOR")
            {
                dt.Clear();
                con.conectar("EX");
                SqlCommand cmd = new SqlCommand("set language español SELECT  [ID], [RUTA] ,[FAMILIA] ,[SUB_FAMILIA] ,[LINEA] ,[MONTO],[VOLUMEN]  FROM [DM].[CORRECT].[OBJETIVOS_DISMO]where DATEPART(YEAR,FECHA) = '" + anio + "' and UPPER(DATENAME(MONTH,FECHA)) = '" + MES + "'", con.conex);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            else if (tipo == "CLIENTE")
            {

                dt.Clear();
                con.conectar("EX");
                SqlCommand cmd = new SqlCommand("set language español SELECT  [ID], [RUTA] ,[FAMILIA] ,[SUB_FAMILIA] ,[LINEA] ,[MONTO],[VOLUMEN]  FROM [DM].[CORRECT].[OBJETIVOS_DISMO]where DATEPART(YEAR,FECHA) = '" + anio + "' and UPPER(DATENAME(MONTH,FECHA)) = '" + MES + "'", con.conex);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }

            gridControl1.DataSource = dt;

            if (dt.Rows.Count >= 1)
            {
                toolStripButton4.Enabled = true;
                toolStripButton1.Enabled = true;
                toolStripButton2.Enabled = true;
                toolStripButton3.Enabled = true;
            }
            else
            {
                toolStripButton4.Enabled = false;
                toolStripButton1.Enabled = false;
                toolStripButton2.Enabled = false;
                toolStripButton3.Enabled = false;
            }
       }
    
        
        private void carga_anos()
        {
            dt.Clear();
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("SELECT DATEPART(YEAR,FECHA) as anio FROM [DM].[CORRECT].[OBJETIVOS_DISMO] GROUP by DATEPART(YEAR,FECHA)", con.conex);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
                {

                comboBox4.Items.Add(dr["anio"]);

                }
            dr.Close();

        }
        private void carga_meses(string anio)
        {
            dt.Clear();
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("set language español SELECT UPPER(DATENAME(MONTH,FECHA))as meses FROM [DM].[CORRECT].[OBJETIVOS_DISMO] where DATEPART(YEAR,FECHA) = '"+anio+ "' group by DATENAME(MONTH,FECHA)  order by DATENAME(MONTH,FECHA)", con.conex);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {

                comboBox2.Items.Add(dr["meses"]);

            }
            dr.Close();

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            cargaobjetivos(comboBox1.Text,comboBox4.Text, comboBox2.Text);
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            carga_meses(comboBox4.Text);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            gridView1.AddNewRow();
            gridView1.OptionsBehavior.Editable = true;
            toolStripButton2.Enabled = false;
            toolStripButton4.Enabled = false;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            gridView1.OptionsBehavior.Editable = true;
           
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (gridView1.OptionsView.ShowAutoFilterRow)
            { 
                
                Image fil = new Bitmap(Sinconizacion_EXactus.Properties.Resources.filter_32x32);
                gridView1.OptionsView.ShowAutoFilterRow = false;
                toolStripButton3.Image = fil;

            }
            else
            {
                Image fil = new Bitmap(Sinconizacion_EXactus.Properties.Resources.clearfilter_32x32);
                gridView1.OptionsView.ShowAutoFilterRow = true;
                toolStripButton3.Image = fil;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cargaobjetivos(comboBox1.Text,comboBox4.Text, comboBox2.Text);
            gridView1.OptionsBehavior.Editable = false;
        }

        private void gridView1_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
           
            int[] i = gridView1.GetSelectedRows();
            foreach (int o in gridView1.GetSelectedRows())
            {
                DataRow row = gridView1.GetDataRow(o);
             monto_up =  Convert.ToDecimal(row[5]);
            }
                
        }

        private void cargadorObjetvosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CORECTX_APP.VENTAS.importador_objetivos imp = new importador_objetivos();
            imp.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
