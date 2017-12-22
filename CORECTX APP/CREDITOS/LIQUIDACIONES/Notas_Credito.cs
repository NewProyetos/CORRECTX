using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Sinconizacion_EXactus.CORECTX_APP.CREDITOS.LIQUIDACIONES
{
    public partial class Notas_Credito : Form
    {
        public Notas_Credito()
        {
            InitializeComponent();
        }
        conexionXML con = new conexionXML();
        DataTable ncs = new DataTable();
        String fecha_liquidacion;
        String Nota_credito;
        Int32 nota_cred;
        private void button1_Click(object sender, EventArgs e)
        {
            loadnotasdec(dateTimePicker1.Value.ToString("yyyy-MM-dd"), dateTimePicker2.Value.ToString("yyyy-MM-dd"), LIQUIDACIONES.RUTA);

        }

        private void Notas_Credito_Load(object sender, EventArgs e)
        {
            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ReadOnly = false;
            dataGridView1.AllowUserToAddRows = false;

            string ruta = LIQUIDACIONES.RUTA;
            label2.Text = ruta;
            DateTime time = DateTime.Today;

            int dia = Convert.ToInt32(time.DayOfWeek);


            if (dia == 1)
            {
                dateTimePicker1.Value = time.AddDays(-2);

            }
            else
            {
                dateTimePicker1.Value = time.AddDays(-1);

            }

            addchekdw();
            dataGridView1.DataSource = LIQUIDACIONES.nc;
            
            chequear();
           
        }

        private void loadnotasdec(string fechaini,string fechafin,string ruta)
        {
            

            //dataGridView1.Columns.Clear();
            ncs.Clear();
            con.conectar("EX");


            SqlCommand cm4 = new SqlCommand("SELECT [FACTURA] as DEVOLUCION,[FECHA] FROM [EXACTUS].["+Login.empresa+"].[FACTURA]  where  TIPO_DOCUMENTO = 'D'  and RUTA = '" + ruta + "' and (DATEADD(dd, 0, DATEDIFF(dd, 0, FECHA)) >= '" + fechaini + "') AND (DATEADD(dd, 0, DATEDIFF(dd, 0,FECHA)) <= '" + fechafin + "') and ANULADA = 'N' and COMENTARIO_CXC is null order by FECHA", con.conex);
            SqlDataAdapter da = new SqlDataAdapter(cm4);
            da.Fill(ncs);
            con.Desconectar("EX");

           // addchekdw();
            dataGridView1.DataSource = ncs;
            
           

            chequear();

            

        }

        private void addchekdw()
        {
            DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn()
            {
                Name = "Liquidar"                
              
            };
            dataGridView1.Columns.Add(chk);


        }

        private void button2_Click(object sender, EventArgs e)
        {
            

             MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Desea agregar las notas de creditos seleccionadas" , "NOTAS DE CREDITO", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                nota_cred = 0;
                try
                {

                fecha_liquidacion = DateTime.Now.ToString("yyyy-MM-dd");
                //foreach (DataGridViewRow row in dataGridView1.Rows)
                //{

                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                          DataGridViewRow row = dataGridView1.Rows[i];
                    DataGridViewCheckBoxCell cell = row.Cells[0] as DataGridViewCheckBoxCell;
                    if (Convert.ToBoolean(cell.Value) == true)
                    {
                        Nota_credito = Convert.ToString(row.Cells[1].Value);
                        nota_cred = nota_cred + 1;

                        con.conectar("EX");

                        SqlCommand cmd8 = new SqlCommand();
                        cmd8.Connection = con.conex;
                        cmd8.CommandText = "UPDATE[EXACTUS].["+Login.empresa+"].[FACTURA] SET COMENTARIO_CXC = @COMENTARIO_CXC  where  TIPO_DOCUMENTO = 'D'  and FACTURA = @NOTA_CREDITO ";
                        cmd8.Parameters.Add("@COMENTARIO_CXC", SqlDbType.NVarChar).Value = fecha_liquidacion;
                        cmd8.Parameters.Add("@NOTA_CREDITO", SqlDbType.NVarChar).Value = Nota_credito;
                        cmd8.ExecuteNonQuery();

                        con.Desconectar("EX");

                        // MessageBox.Show("Se agregaron las NC a la Liquidacion del dia " + fecha_liquidacion + "");

                       
                    }
                    else
                    { 
                    
                    }
                    

                }
                    loadnotasdec(dateTimePicker1.Value.ToString("yyyy-MM-dd"), dateTimePicker2.Value.ToString("yyyy-MM-dd"), LIQUIDACIONES.RUTA);
                MessageBox.Show("SE AGREGARON: " + nota_cred + "NOTAS DE CREDITO EXITOSAMENTE" );

                }
                catch
                {
                    MessageBox.Show("ERROR AL INGRESAR NOTAS DE CREDITO");
                
                }
            }
        }
        private void chequear()
        {
            for (int i = 0; i < dataGridView1.RowCount ; i++)
            {
                
                    dataGridView1.Rows[i].Cells[0].Value = true;
                
            }
        
        }

       
           

    }
}
