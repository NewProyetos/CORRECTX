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
    public partial class Cambio_fecha_Pedido : Form
    {
        public Cambio_fecha_Pedido()
        {
            InitializeComponent();
        }

        conexionXML con = new conexionXML();
        DataTable Pedido = new DataTable();
        String N_pedido;
        private void Cambio_fecha_Pedido_Load(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            textBox1.Text = Sincronizacion.PedidoN;

        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            Pedido.Clear();
            
            
            con.conectar("EX");

            SqlCommand cmd = new SqlCommand("SELECT [PEDIDO],[ESTADO],[FECHA_PEDIDO],[FECHA_PROMETIDA],[FECHA_PROX_EMBARQU],[FECHA_ULT_EMBARQUE]  FROM [EXACTUS].["+Login.empresa+"].[PEDIDO]  where PEDIDO = '"+this.textBox1.Text+"'",con.conex);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(Pedido);

            dataGridView1.DataSource = Pedido;
            dataGridView1.Refresh();


            con.Desconectar("EX");
            groupBox1.Enabled = false;

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int idx;
            String Estado;
            
            idx = dataGridView1.CurrentRow.Index;
            Estado = Convert.ToString(dataGridView1.Rows[idx].Cells[1].Value);
            N_pedido = Convert.ToString(dataGridView1.Rows[idx].Cells[0].Value);
            if (Estado == "N")
            {
                groupBox1.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                con.conectar("EX");
                SqlCommand update = new SqlCommand("UPDATE [EXACTUS].["+Login.empresa+"].[PEDIDO] SET [FECHA_PEDIDO] = '" + this.dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' ,[FECHA_PROMETIDA] = '" + this.dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' ,[FECHA_PROX_EMBARQU] = '" + this.dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' ,[FECHA_ORDEN] = '" + this.dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' where PEDIDO = '" + N_pedido + "'", con.conex);
                update.ExecuteNonQuery();
                con.Desconectar("EX");

                MessageBox.Show("Canbio de fecha Realizado");
            }
            catch
            {
                MessageBox.Show("No se ha podido cambiar las fechas");
            }
        }
    }
}
