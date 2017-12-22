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
    public partial class Direcciones_UPDATE : Form
    {
        public Direcciones_UPDATE()
        {
            InitializeComponent();
        }

        conexionXML con = new conexionXML();
        //conexion conet = new conexion();
        //Conexion2 coned = new Conexion2();
        DataTable pedidos = new DataTable();
        public String Cliente;
        public String Npedido;
        public int registros;
        private void button1_Click(object sender, EventArgs e)
        {
            pedidos.Clear();
            dataGridView1.Refresh();
            
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("SELECT [PEDIDO],[CLIENTE_ORIGEN] ,[CLIENTE],[EMBARCAR_A],[DIRECCION_FACTURA],[COBRADOR],[RUTA],[USUARIO],[CONDICION_PAGO],[BODEGA],[ZONA],[VENDEDOR],[CLIENTE_DIRECCION],[FECHA_PEDIDO] FROM [EXACTUS].["+Login.empresa+"].[PEDIDO]  where  ESTADO = 'N' AND (CLIENTE_DIRECCION like '90%') and IMPRESO = 'N'  and FECHA_PEDIDO = '" + this.dateTimePicker1.Value.ToString("yyyy-MM-dd")+"'", con.conex);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(pedidos);

            
            con.Desconectar("EX");

            dataGridView1.DataSource = pedidos;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                
                con.conectar("DM");

                
                for (int i = 0; i < pedidos.Rows.Count; i++)
                {

                    DataRow row = pedidos.Rows[i];
                    Cliente = Convert.ToString(row["CLIENTE_ORIGEN"]);
                    Npedido = Convert.ToString(row["PEDIDO"]);


                    SqlCommand cmd = new SqlCommand("[CORRECT].[UPDATE_DIRECCION_SUPER]", con.condm);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 50;
                    cmd.Parameters.AddWithValue("@CLIENTE", Cliente);
                    cmd.Parameters.AddWithValue("@PEDIDO", Npedido);
                    cmd.Parameters.AddWithValue("@empresa", Login.empresa);

                    cmd.ExecuteNonQuery();


                }
                
                con.Desconectar("DM");
                MessageBox.Show( Convert.ToString(registros)+ "  PEDIDOS ACTUALIZADOS");
                button2.Enabled = false;
                button1_Click(null, null);
            }
            catch
            {
                MessageBox.Show("ERROR PEDIDOS NO ACTUALIZADOS");
            
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            registros = Convert.ToInt32(dataGridView1.RowCount);
            if (registros > 0)
            {
                this.button2.Enabled = true;
            
            }
        }

        private void Direcciones_UPDATE_Load(object sender, EventArgs e)
        {
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            this.button2.Enabled = false;
        }
    }
}
