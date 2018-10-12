using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Sinconizacion_EXactus.CORECTX_APP.BODEGA.PROMOCIONES
{
    public partial class Nueva_bon : Form
    {
        public Nueva_bon(string cod_pr , string nombre)
        {
            InitializeComponent();
        }
        public static string Nombre;
        public static string inicio;
        public static string fin;
        public static string comentario;
        public static DataTable proveedor = new DataTable();
        conexionXML con = new conexionXML();

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Nueva_bon_Load(object sender, EventArgs e)
        {
            carga_prov();

            textBox3.AutoCompleteCustomSource = Autocompletecodclie();
            textBox3.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBox3.AutoCompleteSource = AutoCompleteSource.CustomSource;

            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox1.Text == string.Empty)
            {
                this.DialogResult = DialogResult.No;
            }
            else
            {
                Nombre = textBox1.Text;
                inicio = dateTimePicker1.Value.ToString("yyyy/MM/dd");
                fin = dateTimePicker2.Value.ToString("yyyy/MM/dd");
                comentario = textBox2.Text;
            }
        }
        public static AutoCompleteStringCollection Autocompletecodclie()
        {


            AutoCompleteStringCollection coleccion = new AutoCompleteStringCollection();
            //recorrer y cargar los items para el autocompletado
            foreach (DataRow row in proveedor.Rows)
            {
                coleccion.Add(Convert.ToString(row["PROVEEDOR"]));
            }

            return coleccion;
        }
        public void carga_prov()
        {
            con.conectar("EX");

            SqlCommand cmd = new SqlCommand("SELECT [PROVEEDOR],[NOMBRE],ALIAS FROM [EXACTUS].[dismo].[PROVEEDOR]", con.conex);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(proveedor);

            con.Desconectar("EX");
                
        }
        public string nombre_prov(string cod)
        {
            string nombre= "";
         
            var results = from myRow in proveedor.AsEnumerable()
                          where myRow.Field<string>("PROVEEDOR") == cod

                          select new
                          {
                              Nombre = myRow.Field<string>("NOMBRE")
                          };

            foreach (var rs1 in results)
            {
              nombre   = rs1.Nombre.ToUpper();
            }
            return nombre;
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            label6.Text = nombre_prov(textBox3.Text);
        }
    }
}
