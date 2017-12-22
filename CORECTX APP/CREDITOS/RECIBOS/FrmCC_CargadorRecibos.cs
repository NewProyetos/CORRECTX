using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Sinconizacion_EXactus
{
    public partial class FrmCC_CargadorRecibos : Form
    {
        DataTable dt = new DataTable();
        conexionXML con = new conexionXML();
        String Empresa = Login.empresa;
        
        public FrmCC_CargadorRecibos()
        {       
            InitializeComponent();
        }

        private void BtnGenerar_Click(object sender, EventArgs e)
        {
            
                
            {
                try
                {
                    con.conectar("DM");
                    SqlDataAdapter da = new SqlDataAdapter("[CORRECT].[CARGARECIBOS]", con.condm);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.Add("@FINI", SqlDbType.DateTime);
                    da.SelectCommand.Parameters.Add("@FFIN", SqlDbType.DateTime);
                    da.SelectCommand.Parameters.Add("@Ruta", SqlDbType.NVarChar);
                    da.SelectCommand.Parameters.Add("@empresa", SqlDbType.NVarChar);
                    da.SelectCommand.Parameters["@FINI"].Value = Convert.ToDateTime(FechaIni.Value.ToShortDateString());
                    da.SelectCommand.Parameters["@FFIN"].Value = Convert.ToDateTime(FechaFin.Value.ToShortDateString());
                    da.SelectCommand.Parameters["@Ruta"].Value = Convert.ToString(CBEntrega.SelectedValue.ToString() );
                    da.SelectCommand.Parameters["@empresa"].Value = Empresa;
                    dt.Clear(); 
                    da.Fill(dt);
               
                    this.dataGridView1.DataSource = dt;
                    TxtMonto.Text = dt.Rows[0].ItemArray[3].ToString().Substring(0, dt.Rows[0].ItemArray[3].ToString().Length-6);
                    this.BtnGenerarCSV.Enabled = true;
                    TxtConcepDoc.Text   = "Liquidacion Entrega " + CBEntrega.SelectedValue.ToString();
                    TxtConceptoGral.Text= "Liquidacion Entrega " + CBEntrega.SelectedValue.ToString();
                    LbRegistros.Text = dataGridView1.RowCount.ToString();
                    con.Desconectar("DM");

                //    foreach (DataRow row in dt.Rows)
                //    {
                ////  MessageBox.Show( dt.Rows[0].ItemArray[5].ToString());
                //    }
                   

                //   da.Fill(mibase, "CargadorCC");
                    //RECORRER EL DATA SET

                    //foreach (DataRow renglon in mibase.Tables["MH_ICV"].Rows)
                    //{

                    //    //  textBox1.Text = renglon["NOMBRE"].ToString();
                    //    //  textBox2.Text = renglon["RFC"].ToString();
                    //    //  textBox3.Text = renglon["Telefono"].ToString();
                    //    //  textBox4.Text = renglon["Pagina"].ToString();
                    //}

                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo conectar a la BD Error: " + ex.Message);
                    con.Desconectar("DM");
                }
            }
        }

        private void BtnCSV_Click(object sender, EventArgs e)
        {
            dlGuardar.Filter = "Fichero CSV (*.csv)|*.csv";
            dlGuardar.FileName = "Datos_sqlite";
            dlGuardar.Title = "Exportar a CSV";
            if (dlGuardar.ShowDialog() == DialogResult.OK)
            {
                StringBuilder csvMemoria = new StringBuilder();

                //para los títulos de las columnas, encabezado
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (i == dt.Columns.Count - 1)
                    {
                        csvMemoria.Append(String.Format("{0}",dt.Columns[i].Caption));
                    }
                    else
                    {
                        csvMemoria.Append(String.Format("{0},",dt.Columns[i].Caption));
                    }
                }
                csvMemoria.AppendLine();

                for (int m = 0; m < dt.Rows.Count; m++)
                {
                    for (int n = 0; n < dt.Columns.Count; n++)
                    {     
                        //si es la última columna no poner el ;
                        if (n == dt.Columns.Count - 1)
                        {
                            csvMemoria.Append(String.Format("{0}",dt.Rows[m].ItemArray[n].ToString()));
                        }
                        else
                        {
                            csvMemoria.Append(String.Format("{0},", dt.Rows[m].ItemArray[n].ToString()));
                        }
                    }
                    csvMemoria.AppendLine();
                }
                System.IO.StreamWriter sw =
                    new System.IO.StreamWriter(dlGuardar.FileName, false,
                       System.Text.Encoding.Default);
                sw.Write(csvMemoria.ToString());
                sw.Close(); 
            }
        }
//Donde:
//* dlGuardar: componente de tipo SaveFileDialog.
//* dt: componente de tipo DataGridView con los datos a exportar a fichero.

        

        private void BtnGenerarCSV_Click(object sender, EventArgs e)
        {

            if (TxtConcepDoc.Text.Length.Equals(0) || TxtConceptoGral.Text.Length.Equals(0))
            {
                MessageBox.Show("Los Conceptos no pueden quedar vacios!!");
            }
            else
            {
                //GENERANDO ARCHIVO CSV
                dlGuardar.Filter = "Fichero CSV (*.csv)|*.csv";
                dlGuardar.FileName = "Datos_sqlite";
                dlGuardar.Title = "Exportar a CSV";
                if (dlGuardar.ShowDialog() == DialogResult.OK)
                {
                    StringBuilder csvMemoria = new StringBuilder();

                    //para los títulos de las columnas, encabezado

                    csvMemoria.Append(String.Format("{0}", "TipoDoCB,SubTipoDoCB,NumDoCB,MontoDocCB,Fecha,ConceptoDocCB,TipoCreditopagar,NumDocCC,ConceptoDocCC,ClienteDoCC,CliteOrigDocCC,MontoDocCC,TipoDocCCPagar,NumDocPagar"));
                    csvMemoria.AppendLine();

                    for (int m = 0; m < dt.Rows.Count; m++)
                    {

                        csvMemoria.Append(String.Format("{0},", dt.Rows[m].ItemArray[0].ToString()));
                        csvMemoria.Append(String.Format("{0},", dt.Rows[m].ItemArray[1].ToString()));
                        csvMemoria.Append(String.Format("{0},", "DM"));
                        csvMemoria.Append(String.Format("{0:0.000},", dt.Rows[m].ItemArray[3]));
                        csvMemoria.Append(String.Format("{0},", Calendario.Value.ToShortDateString()));
                        csvMemoria.Append(String.Format("{0},", TxtConceptoGral.Text));
                        csvMemoria.Append(String.Format("{0},", "REC"));
                        csvMemoria.Append(String.Format("{0},", dt.Rows[m].ItemArray[5].ToString()));
                        csvMemoria.Append(String.Format("{0},", TxtConcepDoc.Text));
                        csvMemoria.Append(String.Format("{0},", dt.Rows[m].ItemArray[9].ToString()));
                        csvMemoria.Append(String.Format("{0},", dt.Rows[m].ItemArray[9].ToString()));
                        csvMemoria.Append(String.Format("{0:0.000},", dt.Rows[m].ItemArray[8]));
                        csvMemoria.Append(String.Format("{0},", dt.Rows[m].ItemArray[6].ToString()));
                        csvMemoria.Append(String.Format("{0}", dt.Rows[m].ItemArray[7].ToString()));

                        csvMemoria.AppendLine();
                    }
                    System.IO.StreamWriter sw =
                        new System.IO.StreamWriter(dlGuardar.FileName, false,
                           System.Text.Encoding.Default);
                    sw.Write(csvMemoria.ToString());
                    sw.Close();
                }
            }
        }

        private void FrmCC_CargadorRecibos_Load(object sender, EventArgs e)
        {
           
            {
                try
                {

                    con.conectar("DM");
                    DataTable tablacbo = new DataTable();
                    SqlDataAdapter dac = new SqlDataAdapter("select RUTA,DESCRIPCION AS NOMBRE from EXACTUS."+Empresa+".RUTA RT WHERE LEFT(RT.RUTA,1)='E'", con.condm);
                    //se indica el nombre de la tabla
                    dac.Fill(tablacbo);
                    CBEntrega.DataSource = tablacbo;
                    //se especifica el campo de la tabla
                    CBEntrega.DisplayMember = "RUTA";
                    CBEntrega.ValueMember = "RUTA";

                    FechaIni.Value = DateTime.Now.AddDays(-1);
                    Calendario.Value = DateTime.Now.AddDays(-1);
                    con.Desconectar("DM");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                    con.Desconectar("DM");
                }
            }
        }

        private void TxtMonto_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Validando entra de datos 
            if (e.KeyChar == 8)
            {
                e.Handled = false;
                return;
            }


            bool IsDec = false;
            int nroDec = 0;

            for (int i = 0; i < TxtMonto.Text.Length; i++)
            {
                if (TxtMonto.Text[i] == '.')
                    IsDec = true;

                if (IsDec && nroDec++ >= 2)
                {
                    e.Handled = true;
                    return;
                }


            }

            if (e.KeyChar >= 48 && e.KeyChar <= 57)
                e.Handled = false;
            else if (e.KeyChar == 46)
                e.Handled = (IsDec) ? true : false;
            else
                e.Handled = true;
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
      
    }
}