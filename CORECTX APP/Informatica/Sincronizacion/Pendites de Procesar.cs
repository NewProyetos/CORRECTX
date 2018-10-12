using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Sinconizacion_EXactus.CORECTX_APP.Informatica.Sincronizacion
{
    public partial class Pendites_de_Procesar : Form
    {
        public Pendites_de_Procesar(string tipo,string doc_p)
        {
            InitializeComponent();
            Tipo_consl = tipo;
            doc_pro = doc_p;
        }
        String Tipo_consl;
        String doc_pro;
        DataTable datos = new DataTable();
        conexionXML con = new conexionXML();
        string consulta;
        private void Pendites_de_Procesar_Load(object sender, EventArgs e)
        {
            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ReadOnly = false;
            dataGridView1.AllowUserToAddRows = false;
            carga_datos(Tipo_consl);
            button1.Enabled = false;

        }

        private void combo(DataTable dts1)
        {

            comboBox1.Items.Clear();


            var result = from row in dts1.AsEnumerable()
                         group row by row.Field<string>("COD_ZON")
                         into grp
                         orderby grp.Key
                         select new
                         {
                             Vendedor = grp.Key,
                            
                         }  ;
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
        private void carga_datos(string tipo_c)
        {
          
            datos.Clear();

            switch (tipo_c)
            {
                case "pedidos":
                    consulta = "SELECT [COD_CIA],[NUM_PED],[COD_ZON],[COD_CLT],[TIP_DOC], [FEC_PED],[MON_CIV],[NUM_ITM]  FROM [EXACTUS].[ERPADMIN].[alFAC_ENC_PED]  where DOC_PRO "+doc_pro+" and ESTADO <> 'C'";
                    break;
                case "devoluciones":
                    consulta = "SELECT [NUM_DEV],[COD_CIA],[COD_ZON],[COD_CLT],[HOR_INI],[HOR_FIN],[FEC_DEV],[EST_DEV],[MON_SIV],[COD_BOD],[NCF_PREFIJO],[NCF],[NIVEL_PRECIO] FROM [EXACTUS].[ERPADMIN].[alFAC_ENC_DEV] where DOC_PRO " + doc_pro + " and EST_DEV <> 'C'";
                    break;
                case "cobros":
                    consulta = "SELECT [COD_CIA],[NUM_REC],[COD_ZON],[COD_CLT],[FEC_PRO],[MON_DOC_LOC],[HOR_INI],[IMPRESO]  FROM [EXACTUS].[ERPADMIN].[alCXC_DOC_APL]  where DOC_PRO " + doc_pro + " and IND_ANL = 'N'";
                    break;
            }
            

            con.conectar("EX");
            SqlCommand cmd = new SqlCommand(consulta, con.conex);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(datos);

            if (datos.Rows.Count >= 1)
            {
                addchekdw();
                combo(datos);

            }
            dataGridView1.DataSource = datos;

           

            con.Desconectar("EX");

        }
        private void addchekdw()
        {
            DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn()
            {
                Name = "Seleccionar"

            };
            dataGridView1.Columns.Add(chk);


        }
        private void chequear()
        {
            if (dataGridView1.RowCount >= 1)
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {

                    dataGridView1.Rows[i].Cells[0].Value = true;

                }
            }
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn newColumn = dataGridView1.Columns[e.ColumnIndex];
            int marcados = 0;

            if (newColumn.HeaderText == "Seleccionar")
            {

                for (int i = 0; i < dataGridView1.RowCount; i++)
                {

                    DataGridViewRow row = dataGridView1.Rows[i];
                    DataGridViewCheckBoxCell cell = row.Cells[0] as DataGridViewCheckBoxCell;
                    if (Convert.ToBoolean(cell.Value) == true)
                    {
                        dataGridView1.Rows[i].Cells[0].Value = false;
                        marcados = marcados - 1;
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells[0].Value = true;
                        marcados = marcados + 1;
                    }
                }
                if (marcados >= 0)
                {
                    button1.Enabled = true;
                    if (doc_pro == "is null")
                    {
                        
                        button1.Text = "set Docpro = 'P'";
                    }
                    else
                    {
                        button1.Text = "set Docpro = NULL ";

                    }
                }

            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            datos.DefaultView.RowFilter = "COD_ZON like '" + comboBox1.Text + "%'";
            dataGridView1.DataSource = datos;
        }
    }
}
