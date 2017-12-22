using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Sinconizacion_EXactus.CORECTX_APP.VENTAS.FACTURACION
{
    public partial class Articulos : Form
    {
        public Articulos()
        {
            InitializeComponent();
        }

        public string Cod_art;
        public string Nom_art;
        public string orden;
        int idx;

        DataTable articulo = new DataTable();
        conexionXML con = new conexionXML();
        private void Articulos_Load(object sender, EventArgs e)
        {
            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;


            carga_articulo();

            combo(articulo);


        }

        private void carga_articulo()

        {
            con.conectar("EX");
            SqlCommand cm2 = new SqlCommand("SELECT CAST(ART.[U_ID_FAC] as int) as  ORDEN,ART.[ARTICULO] as CODIGO,ART.[DESCRIPCION] ,ART.CLASIFICACION_1 +' '+ CLAS.DESCRIPCION as FAMILIA FROM [EXACTUS].[dismo].[ARTICULO] ART  LEFT JOIN (SELECT [CLASIFICACION] ,[DESCRIPCION]   FROM [EXACTUS].[dismo].[CLASIFICACION]  where AGRUPACION = '1') as CLAS  on ART.CLASIFICACION_1 = CLAS.CLASIFICACION where ART.CLASIFICACION_1 = '73'", con.conex);
            SqlDataAdapter da2 = new SqlDataAdapter(cm2);
            da2.Fill(articulo);
            con.Desconectar("EX");
        }

        public void combo(DataTable dts1)
        {
            comboBox1.Items.Clear();


            var result = from row in dts1.AsEnumerable()
                         group row by row.Field<string>("FAMILIA") into grp
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
            articulo.DefaultView.RowFilter = " FAMILIA = '" + this.comboBox1.Text + "'";
           
            dataGridView1.DataSource = articulo;

            this.dataGridView1.Columns["FAMILIA"].Visible = false;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            idx = dataGridView1.CurrentRow.Index;
            orden = Convert.ToString(dataGridView1.Rows[idx].Cells[0].Value);
            Cod_art = Convert.ToString(dataGridView1.Rows[idx].Cells[1].Value);
            Nom_art = Convert.ToString(dataGridView1.Rows[idx].Cells[2].Value);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            articulo.DefaultView.RowFilter = string.Format("Convert(DESCRIPCION,'System.String') like '%{0}%'", this.textBox1.Text);
            dataGridView1.DataSource = articulo;
        }
    }
}
