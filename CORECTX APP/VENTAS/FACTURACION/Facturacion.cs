using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Sinconizacion_EXactus.CORECTX_APP.VENTAS.FACTURACION
{
    public partial class Facturacion : Form
    {
        public Facturacion()
        {
            InitializeComponent();
        }
        conexionXML con = new conexionXML();
        DataTable rutas = new DataTable();
        public static String Ruta;
        public static String pedido_update;
        DataTable NCF = new DataTable();
        int correla_fac;
        int correla_ccf;
        string nuevo;
        public static string prefijofac;
        public static string prefijoccf;
        public static String Correlativo_fac;
        public static String Correlativo_ccf;

        public static string update;

        DataTable pedidos = new DataTable();
        int idx;
        
        private void Facturacion_Load(object sender, EventArgs e)
        {
            toolStripButton1.Enabled = false;
            toolStripButton3.Enabled = false;
            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            //dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;

            if (Main_Menu.Puesto == "VEN")
            {
                comboBox1.Text = Login.usuario.Replace('P', 'V');
                Ruta = Login.usuario.Replace('P', 'V');
                correlativos();
                carga_pedidos(comboBox1.Text);
            }
            else
            {
                carga_rutas();
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

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Ingreso_Facturas ingfac = new Ingreso_Facturas();
            ingfac.ShowDialog();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Ruta = comboBox1.Text;
            correlativos();

            carga_pedidos(comboBox1.Text);
        }
        private void correlativos()
        {
            NCF.Clear();
            con.conectar("EX");

            SqlCommand cm1 = new SqlCommand("SELECT PREFIJO,LEFT(PREFIJO,1) as TIPO, [ULTIMO_VALOR] FROM [EXACTUS].[dismo].[NCF_CONSECUTIVO] where RIGHT(PREFIJO,'3')  = '"+comboBox1.Text.Substring(1,3)+"'", con.conex);
            SqlDataAdapter da1 = new SqlDataAdapter(cm1);
            da1.Fill(NCF);


            con.Desconectar("EX");


            var fac = from myRow in NCF.AsEnumerable()
                          where myRow.Field<string>("TIPO") == "F"

                          select new
                          {
                              Nombre = myRow.Field<string>("ULTIMO_VALOR"),
                              prefijo = myRow.Field<string>("PREFIJO")
                          };

            foreach (var rs1 in fac)
            {
                correla_fac = Convert.ToInt32(rs1.Nombre);
                if (correla_fac == 0)
                {
                    Correlativo_fac = "0000000";
                }
                else
                {
                    Correlativo_fac = correlativonuevo(correla_fac);
                 
                }
                prefijofac = rs1.prefijo;
            }

            var ccf = from myRows in NCF.AsEnumerable()
                          where myRows.Field<string>("TIPO") == "C"

                          select new
                          {
                              Nombre = myRows.Field<string>("ULTIMO_VALOR"),
                              prefijo = myRows.Field<string>("PREFIJO")
                          };

            foreach (var rs2 in ccf)
            {
                correla_ccf = Convert.ToInt32(rs2.Nombre);
                if (correla_ccf == 0)
                {
                    Correlativo_ccf = "000000";
                }
                else
               { 
                Correlativo_ccf = correlativonuevo(correla_ccf);
                
                }
                prefijoccf = rs2.prefijo;
            }




          



            label4.Text = Correlativo_fac;
            label5.Text = Correlativo_ccf;
        }


        private string  correlativonuevo(int ultm)
        {
            

            int ceros = Convert.ToString(ultm).Length;
            string newid = Convert.ToString(ultm+1);
            

            switch (ceros)
            {


                case 1:
                    nuevo =  "0000000" + newid;
                    break;
                case 2:
                    nuevo =  "000000" + newid;
                    break;
                case 3:
                    nuevo =  "00000" + newid;
                    break;
                case 4:
                    nuevo =  "0000" + newid;
                    break;
                case 5:
                    nuevo =  "000" + newid;
                    break;
                case 6:
                    nuevo =  "00" + newid;
                    break;
                case 7:
                    nuevo =   "0" + newid;
                    break;
                case 8:
                    nuevo =  newid;
                    break;

                
            }

          return nuevo;
        }
        private void carga_pedidos(string ruta)
        {
            pedidos.Clear();

            con.conectar("DM");

            SqlCommand cm1 = new SqlCommand("SELECT [NUM_DOC_SIS],[TIPO_DOC],ENC.[RUTA],ENC.[VENDEDOR],[ENTREGA],[COD_CLIE],CLIE.NOMBRE,[FECHA_PEDIDO],[MONTO_IMP],[MONTO_SIN_IMP],[MONTO_CON_IMP],[MONTO_DESC_CLIE],[MONTO_DESC_LINEA],[CANT_ITEM],[LISTA_PRECIO],[ESTADO_PEDIDO],[CONDICION_CLIENTE],[BODEGA],[SERIE_DOC],[PROCESADO],[VALOR_DESCUENTO] FROM [DM].[STREET].[ENC_PED_STREET] as ENC LEFT JOIN [EXACTUS].[dismo].[CLIENTE] as CLIE on ENC.COD_CLIE = CLIE.CLIENTE where ENC.RUTA = '"+comboBox1.Text.Replace('V','R')+ "' and (DATEADD(dd, 0, DATEDIFF(dd, 0, ENC.FECHA_PEDIDO)) = '"+dateTimePicker1.Value.ToString("yyyy/MM/dd")+"')", con.condm);
            SqlDataAdapter da1 = new SqlDataAdapter(cm1);
            da1.Fill(pedidos);

            con.Desconectar("DM");



            dataGridView1.DataSource = pedidos;
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

            if (dataGridView1.RowCount > 0)
            {

                if (dataGridView1.Columns.Contains("TIPO_DOC"))
                {
                    dataGridView1.Columns.Remove("TIPO_DOC");
                    dataGridView1.Columns.Remove("RUTA");
                    dataGridView1.Columns.Remove("VENDEDOR");
                    dataGridView1.Columns.Remove("ENTREGA");
                    dataGridView1.Columns.Remove("MONTO_DESC_CLIE");
                    dataGridView1.Columns.Remove("MONTO_DESC_LINEA");
                    dataGridView1.Columns.Remove("LISTA_PRECIO");
                   // dataGridView1.Columns.Remove("ESTADO_PEDIDO");
                    dataGridView1.Columns.Remove("CONDICION_CLIENTE");
                    dataGridView1.Columns.Remove("SERIE_DOC");
                   // dataGridView1.Columns.Remove("PROCESADO");
                    dataGridView1.Columns.Remove("VALOR_DESCUENTO");
                }
            }

        }

        private void dateTimePicker1_TabIndexChanged(object sender, EventArgs e)
        {
            carga_pedidos(comboBox1.Text);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            carga_pedidos(comboBox1.Text);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            idx = dataGridView1.CurrentRow.Index;

          string  idlinea = Convert.ToString(dataGridView1.Rows[idx].Cells[1].Value);
            string estado = Convert.ToString(dataGridView1.Rows[idx].Cells[8].Value);
            string procesado = Convert.ToString(dataGridView1.Rows[idx].Cells[10].Value);
            pedido_update = Convert.ToString(dataGridView1.Rows[idx].Cells[0].Value);

            if (estado != "N" || procesado != "D")
            {
                toolStripButton1.Enabled = false;
                toolStripButton3.Enabled = false;
            }
            else
            {


                toolStripButton1.Enabled = true;
                toolStripButton3.Enabled = true;

            }


        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            update = "SI";

            Ingreso_Facturas ingfac = new Ingreso_Facturas();
            ingfac.Show();

        }
    }
}
