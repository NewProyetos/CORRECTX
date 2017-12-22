using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Sinconizacion_EXactus.CORECTX_APP.Informatica.Power_Street
{
    public partial class Facturas_No_procesados : Form
    {
        public Facturas_No_procesados()
        {
            InitializeComponent();
        }
        conexionXML con = new conexionXML();
        DataTable Noproc = new DataTable();
        Int32 proceso;
        private void Facturas_No_procesados_Load(object sender, EventArgs e)
        {



        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

      

        private void Facturas_No_procesados_Load_1(object sender, EventArgs e)
        {
            groupBox2.Enabled = false;
            dataGridView1.RowHeadersVisible = false;
            //dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            cargadata();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Seleccione un Caracter a utilizar");
                comboBox1.Focus();
            }
            else
            {
                proceso = 1;
                for (int i = 0; i < Noproc.Rows.Count; i++)
                {
                    DataRow row = Noproc.Rows[i];
                    string NUM_PED = Convert.ToString(row["NUM_DOC_PREIMP"]);

                    if (existe_Documento_exactusERP(NUM_PED))
                    {
                    }
                    else
                    { 
                    MessageBox.Show("Existen Facturas sin problemas. pendientes de Procesar, antes realizar la autilizacion Ejecute el proceso de Carga de Facturas");

                        groupBox2.Enabled = false;
                        proceso = 2;
                        break;
                    }
                    


                }

                if (proceso == 1)
                {

                    con.conectar("DM");
                    //DETALLE DE FACTURA 
                    SqlCommand cmd8 = new SqlCommand();
                    cmd8.Connection = con.condm;
                    cmd8.CommandText = "UPDATE DET SET DET.NUM_DOC = @CARACTER + DET.NUM_DOC FROM [DM].[STREET].[DET_PED_STREET] DET  inner join  [DM].[STREET].[ENC_PED_STREET] ENC  on ENC.NUM_DOC_SIS= DET.NUM_DOC_SIS and ENC.TIPO_DOC = det.TIPO_DOC  where ENC.PROCESADO = 'N' and DATEADD(dd, 0, DATEDIFF(dd, 0, ENC.FECHA_PEDIDO)) >= @FECHAINI and DATEADD(dd, 0, DATEDIFF(dd, 0, ENC.FECHA_PEDIDO)) <= @FECHAFIN";
                    cmd8.Parameters.Add("@CARACTER", SqlDbType.VarChar).Value = comboBox1.Text;
                    cmd8.Parameters.Add("@FECHAINI", SqlDbType.VarChar).Value = dateTimePicker1.Value.ToString("yyyy/MM/dd");
                    cmd8.Parameters.Add("@FECHAFIN", SqlDbType.VarChar).Value = dateTimePicker2.Value.ToString("yyyy/MM/dd");
                    cmd8.ExecuteNonQuery();

                    //ENCABEZADO DE FACTURA

                    SqlCommand cmd9 = new SqlCommand();
                    cmd9.Connection = con.condm;
                    cmd9.CommandText = "UPDATE [DM].[STREET].[ENC_PED_STREET] SET NUM_DOC_PREIMP = @CARACTER + NUM_DOC_PREIMP  where PROCESADO = 'N' and DATEADD(dd, 0, DATEDIFF(dd, 0, FECHA_PEDIDO)) >= @FECHAINI and DATEADD(dd, 0, DATEDIFF(dd, 0, FECHA_PEDIDO)) <= @FECHAFIN";
                    cmd9.Parameters.Add("@CARACTER", SqlDbType.VarChar).Value = comboBox1.Text;
                    cmd9.Parameters.Add("@FECHAINI", SqlDbType.VarChar).Value = dateTimePicker1.Value.ToString("yyyy/MM/dd");
                    cmd9.Parameters.Add("@FECHAFIN", SqlDbType.VarChar).Value = dateTimePicker2.Value.ToString("yyyy/MM/dd");
                    cmd9.ExecuteNonQuery();

                    MessageBox.Show("Se han actualizado " + Convert.ToString(Noproc.Rows.Count) + " FACTURAS");

                    cargadata();


                    con.Desconectar("DM");
                }
            }

        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            label5.Text = Convert.ToString(Noproc.Rows.Count);
            groupBox2.Enabled = true;
        }

        private bool existe_Documento_exactusERP(string factura)
        {
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*)  FROM [EXACTUS].[dismo].[FACTURA]  WHERE FACTURA='" + factura + "'", con.conex);
            cmd.Parameters.AddWithValue("FACTURA", Convert.ToInt32(factura));


            int contar = Convert.ToInt32(cmd.ExecuteScalar());
            con.Desconectar("EX");

            if (contar == 0)
            {
                return false;

            }
            else
            {
                return true;

            }
        }
        private void cargadata()

        {
            Noproc.Clear();
            con.conectar("DM");
            SqlCommand cmd8 = new SqlCommand();
            cmd8.Connection = con.condm;
            cmd8.CommandText = "SELECT [NUM_DOC_SIS],[NUM_DOC_PREIMP],[TIPO_DOC],[RUTA],[VENDEDOR],[ENTREGA],[COD_CLIE],[FECHA_PEDIDO],[HORA_INICIO_PEDIDO],[HORA_FIN_PEDIDO],[FECHA_DESPACHO],[MONTO_IMP],[MONTO_IMP_PERC] ,[MONTO_IMP_RET] ,[MONTO_SIN_IMP] ,[MONTO_CON_IMP],[MONTO_DESC_CLIE],[MONTO_DESC_LINEA],[CANT_ITEM],[LISTA_PRECIO],[ESTADO_PEDIDO],[CONDICION_CLIENTE],[BODEGA],[LATITUD] ,[LONGITUD],[FECHA_CREA],[USUARIO_CREA],[SERIE_DOC] ,[PROCESADO] ,[VALOR_DESCUENTO] FROM [DM].[STREET].[ENC_PED_STREET]  where PROCESADO = 'N' and DATEADD(dd, 0, DATEDIFF(dd, 0, FECHA_PEDIDO)) >= @FECHAINI and DATEADD(dd, 0, DATEDIFF(dd, 0, FECHA_PEDIDO)) <= @FECHAFIN ";
            cmd8.Parameters.Add("@FECHAINI", SqlDbType.VarChar).Value = dateTimePicker1.Value.ToString("yyyy/MM/dd");
            cmd8.Parameters.Add("@FECHAFIN", SqlDbType.VarChar).Value = dateTimePicker2.Value.ToString("yyyy/MM/dd");
            SqlDataAdapter da = new SqlDataAdapter(cmd8);
            da.Fill(Noproc);
            con.Desconectar("DM");

            dataGridView1.DataSource = Noproc;
            dataGridView1.Refresh();
        
        }

    }
}
