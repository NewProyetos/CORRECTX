using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace Sinconizacion_EXactus.CORECTX_APP.BODEGA.PROMOCIONES
{
    public partial class Articulos_Bonificar : Form
    {
        conexionXML con = new conexionXML();
        public static DataTable articulo = new DataTable();
        public static DataTable articulo_bon = new DataTable();
        private ContextMenu menugrid = new ContextMenu();
        string codigo;
        
        public Articulos_Bonificar()
        {
            InitializeComponent();
        }

        private void Articulos_Bonificar_Load(object sender, EventArgs e)
        {

            MenuItem QUITAR = new MenuItem("ELIMINAR", new System.EventHandler(this.eliminar));
            menugrid.MenuItems.AddRange(new MenuItem[] { QUITAR});
            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            //dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;

            carga_articulos();

           
            carga_bon();
          

        }

        private void eliminar(Object sender, System.EventArgs e)

        {

            con.conectar("DM");
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con.condm;
            cmd.CommandText = "DELETE [DM].[CORRECT].[PROMOCIONES_GT] WHERE CODIGO = '"+codigo+"'";
            cmd.ExecuteNonQuery();
            con.Desconectar("DM");
            carga_bon();
            carga_articulos();

        }

        private void carga_bon()
        {
            articulo_bon.Clear();

            con.conectar("EX");

            SqlCommand cm2 = new SqlCommand("SELECT PR.[CODIGO],AR.DESCRIPCION,PR.FECHA_CREA as FECHA_INGRESO FROM [DM].[CORRECT].[PROMOCIONES_GT] as PR  LEFT JOIN EXACTUS."+ Login.empresa.ToUpper() + ".ARTICULO AR ON PR.CODIGO=AR.ARTICULO", con.conex);
            SqlDataAdapter da2 = new SqlDataAdapter(cm2);
            da2.Fill(articulo_bon);

            dataGridView1.DataSource = articulo_bon;


            con.Desconectar("EX");

            textBox1.AutoCompleteCustomSource = Autocompletearticulo();
            textBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;

            textBox2.AutoCompleteCustomSource = Autocompletearticulodes();
            textBox2.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;

        }
        private void carga_articulos()
        {
            articulo.Clear();
            con.conectar("EX");

            SqlCommand cm2 = new SqlCommand("SELECT [ARTICULO],[DESCRIPCION] FROM [EXACTUS].["+ Login.empresa.ToUpper() + "].[ARTICULO] where ARTICULO not in (SELECT [CODIGO] FROM [DM].[CORRECT].[PROMOCIONES_GT])", con.conex);
            SqlDataAdapter da2 = new SqlDataAdapter(cm2);
            da2.Fill(articulo);           


            con.Desconectar("EX");
        }

        public static AutoCompleteStringCollection Autocompletearticulo()
        {

            AutoCompleteStringCollection coleccion = new AutoCompleteStringCollection();
            //recorrer y cargar los items para el autocompletado
            foreach (DataRow row in articulo.Rows)
            {
                coleccion.Add(Convert.ToString(row["ARTICULO"]));
            }

            return coleccion;
        }

        public static AutoCompleteStringCollection Autocompletearticulodes()
        {

            AutoCompleteStringCollection coleccion = new AutoCompleteStringCollection();
            //recorrer y cargar los items para el autocompletado
            foreach (DataRow row in articulo.Rows)
            {
                coleccion.Add(Convert.ToString(row["DESCRIPCION"]));
            }

            return coleccion;
        }

        private void Nombre_Articulo()
        {
            var results = from myRow in articulo.AsEnumerable()
                          where myRow.Field<string>("ARTICULO") == textBox1.Text

                          select new
                          {
                              Nombre = myRow.Field<string>("DESCRIPCION")
                          };

            foreach (var rs1 in results)
            {
                textBox2.Text = rs1.Nombre;
            }
        }

        private void cod_Articulo()
        {
            var results = from myRow in articulo.AsEnumerable()
                          where myRow.Field<string>("DESCRIPCION") == textBox2.Text

                          select new
                          {
                              Nombre = myRow.Field<string>("ARTICULO")
                          };

            foreach (var rs1 in results)
            {
                textBox1.Text = rs1.Nombre;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty || textBox1.Text != "")
            {
                Nombre_Articulo();
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text != string.Empty || textBox2.Text != "")
            {
                cod_Articulo();
                //if (textBox1.Text == string.Empty || textBox1.Text == "")
                //{
                //    cod_Articulo();
                //}
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            con.conectar("DM");
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con.condm;
            cmd.CommandText = "INSERT INTO [DM].[CORRECT].[PROMOCIONES_GT] ([CODIGO],[FECHA_CREA],[USUARIO_CREA]) VALUES (@CODIGO,@FECHA_CREA,@USUARIO_CREA)";

            cmd.Parameters.Add("@CODIGO", SqlDbType.NVarChar).Value = textBox1.Text;
            cmd.Parameters.Add("@FECHA_CREA", SqlDbType.DateTime).Value = DateTime.Now.ToString();
            cmd.Parameters.Add("@USUARIO_CREA", SqlDbType.NVarChar).Value = Login.usuario.ToUpper();

            cmd.ExecuteNonQuery();
            con.Desconectar("DM");
            carga_bon();

            textBox1.Text = "";
            textBox2.Text = "";
            carga_articulos();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int idx = dataGridView1.CurrentRow.Index;
            codigo = Convert.ToString(dataGridView1.Rows[idx].Cells[0].Value);
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
         
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo info;
            if (e.Button == MouseButtons.Right)
            {
                info = dataGridView1.HitTest(e.X, e.Y);
                if (info.Type == DataGridViewHitTestType.Cell)
                {
                    menugrid.Show(dataGridView1, new Point(e.X, e.Y));
                }


            }
        }
    }
}
