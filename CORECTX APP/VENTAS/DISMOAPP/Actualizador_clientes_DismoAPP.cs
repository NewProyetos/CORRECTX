using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Collections;
namespace Sinconizacion_EXactus
{
    public partial class Actualizador_clientes_DismoAPP : Form
    {
        public Actualizador_clientes_DismoAPP()
        {
            InitializeComponent();
        }
        conexionXML con = new conexionXML();
        DataTable web = new DataTable();
        DataTable exactus = new DataTable();
        DataTable result = new DataTable();
        DataTable compara = new DataTable();
        String tipo;
        private void Actualizador_clientes_DismoAPP_Load(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            //dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
          //  button2.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();


        }

        private void button2_Click(object sender, EventArgs e)
        {



            con.conectar("WEB");
            for (int i = 0; i < result.Rows.Count; i++)
            {
                DataRow row = result.Rows[i];
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con.mysqlconec;
                cmd.CommandText = "INSERT INTO dismodb.CLIENTS (TIMEST,CODCLIE,NOMBRE,DIRECCION,RUTA,AREA,Mueble,MUECANT,COMUNI,ALAS,NEC,TEL,CUMP,LAT,LON) VALUES (@TIMEST,@CODCLIE,@NOMBRE,@DIRECCION,@RUTA,@AREA,@Mueble,@MUECANT,@COMUNI,@ALAS,@NEC,@TEL,@CUMP,@LAT,@LON)";
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@TIMEST", 0);
                cmd.Parameters.AddWithValue("@CODCLIE", Convert.ToString(row["CODCLIE"]));
                cmd.Parameters.AddWithValue("@NOMBRE", Convert.ToString(row["NOMBRE"]));
                cmd.Parameters.AddWithValue("@DIRECCION", Convert.ToString(row["DIRECCION"]));
                cmd.Parameters.AddWithValue("@RUTA", Convert.ToString(row["VENDEDOR"]));
                cmd.Parameters.AddWithValue("@AREA", Convert.ToString(row["AREA"]));
                cmd.Parameters.AddWithValue("@Mueble", "0");
                cmd.Parameters.AddWithValue("@MUECANT", "0");
                cmd.Parameters.AddWithValue("@COMUNI", "0");
                cmd.Parameters.AddWithValue("@ALAS", Convert.ToString(row["ALIAS"]));
                cmd.Parameters.AddWithValue("@NEC", "0");
                cmd.Parameters.AddWithValue("@TEL", Convert.ToString(row["TEL"]));
                cmd.Parameters.AddWithValue("@CUMP", "0");
                cmd.Parameters.AddWithValue("@LAT", Convert.ToString(row["LAT"]));
                cmd.Parameters.AddWithValue("@LON", Convert.ToString(row["LON"]));

                cmd.ExecuteNonQuery();

            }
            con.Desconectar("WEB");
        }

        public DataTable RemoveDuplicateRows(DataTable dTable, string colName)
        {

            Hashtable hTable = new Hashtable();
            ArrayList duplicateList = new ArrayList();

            //Add list of all the unique item value to hashtable, which stores combination of key, value pair.
            //And add duplicate item value in arraylist.
            foreach (DataRow drow in dTable.Rows)
            {
                if (hTable.Contains(drow[colName]))
                    duplicateList.Add(drow);
                else
                    hTable.Add(drow[colName], string.Empty);
            }

            //Removing a list of duplicate items from datatable.
            foreach (DataRow dRow in duplicateList)
                dTable.Rows.Remove(dRow);

            //Datatable which contains unique records will be return as output.
            return dTable;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            this.timer1.Interval = (60000);
            this.timer1.Start();

            cargaimagen("Load");
           
            con.conectar("WEB");

            string comd = "SELECT CODCLIE FROM dismodb.CLIENTS;";
            MySqlCommand mcm = new MySqlCommand(comd, con.mysqlconec);
            MySqlDataAdapter da = new MySqlDataAdapter(mcm);
            da.Fill(web);


            con.Desconectar("WEB");


            con.conectar("EX");
            string domdx = "SELECT cast((fac.CLIENTE_ORIGEN)as int) as 'CODCLIE',fac.NOMBRE_CLIENTE as 'NOMBRE',fac.DIRECCION_FACTURA as 'DIRECCION',fac.VENDEDOR as 'VENDEDOR',ven.E_MAIL as 'AREA',clie.ALIAS as 'ALIAS',clie.TELEFONO1 as 'TEL',gps.LATITUD as 'LAT',gps.LONGITUD as 'LON'  FROM [EXACTUS].[dismo].[FACTURA] fac  left join [EXACTUS].[dismo].[FACTURA_LINEA] facline  on fac.FACTURA = facline.FACTURA   left join [EXACTUS].[dismo].[VENDEDOR] ven  on fac.VENDEDOR = ven.VENDEDOR   left join [EXACTUS].[dismo].[CLIENTE] clie  on fac.CLIENTE = clie.CLIENTE  left join [EXACTUS].[ERPADMIN].[CLIENTE_RT] gps  on fac.CLIENTE = gps.CLIENTE  where facline.ARTICULO between '1001' and '1851' and fac.FECHA_ORDEN>= dateadd (mm, -7, getdate()) and fac.ANULADA <> 'S'   group by fac.CLIENTE_ORIGEN,fac.NOMBRE_CLIENTE,fac.DIRECCION_FACTURA,fac.VENDEDOR,ven.E_MAIL,clie.ALIAS,clie.TELEFONO1,gps.LATITUD,gps.LONGITUD";
            SqlCommand cmd = new SqlCommand(domdx, con.conex);
            SqlDataAdapter dax = new SqlDataAdapter(cmd);
            dax.Fill(exactus);


            con.Desconectar("EX");

            //button2.Enabled = true;



          
                compara = exactus.AsEnumerable()

                 .Where(r =>

                !web.AsEnumerable().Any(w =>

                w.Field<int>("CODCLIE") == r.Field<int>("CODCLIE")))

                 .CopyToDataTable<DataRow>();


               // result = RemoveDuplicateRows(compara, "CODCLIE");

                //label1.Text = Convert.ToString(result.Rows.Count);
                //dataGridView1.DataSource = compara;


              //  MessageBox.Show("NO se encotraron registros");



        }

        public void imagen_enviando(PictureBox imagenes)
        {
            imagenes.Image = Properties.Resources.informaticaweb;

            imagenes.Refresh();
            imagenes.Visible = true;


        }

        public void imagen_consulta(PictureBox imagenes)
        {
            imagenes.Image = Properties.Resources.trnasfer;
            imagenes.Refresh();
            imagenes.Visible = true;

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

         
        }

        private void cargaimagen(string tipos)
        {
            if (tipos== "Load")
            {

               // imagen_consulta(pictureBox1);

            }
            else

            { 
            
            }
        
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            cargaimagen(tipo);
           
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            result = RemoveDuplicateRows(compara, "CODCLIE");

            label1.Text = Convert.ToString(result.Rows.Count);
            dataGridView1.DataSource = compara;
        }
    }
}
