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
    public partial class Update_PEDIDOS : Form
    {
        public Update_PEDIDOS()
        {
            InitializeComponent();
        }
        //conexion conet = new conexion();
        conexionXML con = new conexionXML();
        DataTable clie_dismo = new DataTable();
        DataTable clie_PED = new DataTable();
        public String Cliente;
        BackgroundWorker worker = new BackgroundWorker();
        public String Clienteup;
        public String Nombreup;
        public String Direccup;
        public String Docup;
        public String Rutaup;
        String empresa = Login.empresa;

        private void Update_PEDIDOS_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;

            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd-MM-yyyy";

            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Refresh();

            dataGridView2.RowHeadersVisible = false;
            dataGridView2.AutoResizeColumns();
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView2.ReadOnly = true;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.Refresh();


            clie_PED.Columns.Add("CLIENTE", typeof(string));
            clie_PED.Columns.Add("PEDIDO", typeof(string));
            clie_PED.Columns.Add("NOMBRE_CLIENTE", typeof(string));
            clie_PED.Columns.Add("EMBARCA_A", typeof(string));
            clie_PED.Columns.Add("DIRECCION_FACTURA", typeof(string));
            clie_PED.Columns.Add("COBRADOR", typeof(string));
            clie_PED.Columns.Add("ESTADO", typeof(string));
            clie_PED.Columns.Add("RUTA", typeof(string));
            clie_PED.Columns.Add("CONDICION_PAGO", typeof(string));
            clie_PED.Columns.Add("ZONA", typeof(string));
            clie_PED.Columns.Add("VENDEDOR", typeof(string));
            clie_PED.Columns.Add("PAIS", typeof(string));
            clie_PED.Columns.Add("DOC_A_GENERAR", typeof(string));

            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;

            worker.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            worker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            clie_dismo.Clear();
            clie_PED.Clear();
            clie_dismo.Columns.Clear();

            con.conectar("EX");

            SqlCommand cmd = new SqlCommand("SELECT CLIE.[CLIENTE],CLIE.[NOMBRE],CLIE.[ALIAS],CLIE.[DIRECCION],CLIE.[DOC_A_GENERAR],CLIE.[TELEFONO1],CLIE.[CONTRIBUYENTE],CLIE.[PAIS],CLIE.[ZONA],CLIE.[RUTA],CLIE.[E_MAIL],CLIE.[RUBRO1_CLI],CLIE.[RUBRO2_CLI],CLIE.FCH_HORA_ULT_MOD,CLIE.USUARIO_ULT_MOD FROM [EXACTUS].["+empresa+"].[PEDIDO] as PED   LEFT JOIN  [EXACTUS].["+empresa+"].[CLIENTE] CLIE  on PED.CLIENTE = CLIE.CLIENTE  where PED.NOMBRE_CLIENTE <> CLIE.NOMBRE and (DATEADD(dd, 0, DATEDIFF(dd, 0, CLIE.FCH_HORA_ULT_MOD)) >=  '" + this.dateTimePicker1.Value.ToString("yyyy-MM-dd") + "'and PED.ESTADO ='N' )", con.conex);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(clie_dismo);
            con.Desconectar("EX");

            dataGridView1.DataSource = clie_dismo;
            dataGridView1.Refresh();


            con.conectar("EX");

        

            for (int i = 0; i < clie_dismo.Rows.Count; i++)
            {
                DataRow row = clie_dismo.Rows[i];
                Cliente = Convert.ToString(row["CLIENTE"]);



                SqlCommand cmd1 = new SqlCommand("SELECT [CLIENTE],[PEDIDO],[NOMBRE_CLIENTE],[EMBARCAR_A],[DIRECCION_FACTURA],[COBRADOR],[ESTADO],[RUTA],[CONDICION_PAGO],[ZONA],[VENDEDOR],[PAIS] FROM [EXACTUS].["+empresa+"].[PEDIDO]  where  ESTADO ='N' and CLIENTE = '"+Cliente+"' ", con.conex);
                SqlDataReader dr1 = cmd1.ExecuteReader();

                while (dr1.Read())
                {

                    clie_PED.Rows.Add(Convert.ToString(dr1["CLIENTE"]), Convert.ToString(dr1["PEDIDO"]), Convert.ToString(dr1["NOMBRE_CLIENTE"]), Convert.ToString(dr1["EMBARCAR_A"]), Convert.ToString(dr1["DIRECCION_FACTURA"]), Convert.ToString(dr1["COBRADOR"]), Convert.ToString(dr1["ESTADO"]), Convert.ToString(dr1["RUTA"]), Convert.ToString(dr1["CONDICION_PAGO"]), Convert.ToString(dr1["ZONA"]), Convert.ToString(dr1["VENDEDOR"]), Convert.ToString(dr1["PAIS"]));


                }

                dr1.Close();


            }
            con.Desconectar("EX");
            dataGridView2.DataSource = clie_PED;
            dataGridView2.Refresh();

            label2.Text = Convert.ToString(dataGridView1.Rows.Count);



        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            con.conectar("EX");
            for (int i = 0; i < clie_dismo.Rows.Count; i++)
            {



                int percentage = (i + 1) * 100 / clie_dismo.Rows.Count;
                worker.ReportProgress(percentage);




                DataRow row = clie_dismo.Rows[i];
                Clienteup = Convert.ToString(row["CLIENTE"]);
                Nombreup = Convert.ToString(row["NOMBRE"]);
                Direccup=Convert.ToString(row["DIRECCION"]);
                Docup = Convert.ToString(row["DOC_A_GENERAR"]);
                Rutaup = Convert.ToString(row["RUTA"]);

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con.conex;
                cmd.CommandText = "UPDATE [EXACTUS].["+empresa+"].[PEDIDO] SET NOMBRE_CLIENTE=@NOMBRE,EMBARCAR_A=@EMBARCAR_A,DIRECCION_FACTURA=@DIRECCION,DOC_A_GENERAR=@DOCUMENTO,RUTA=@RUTA where CLIENTE ='" + Clienteup + "'";
                cmd.Parameters.Add("@NOMBRE", SqlDbType.NVarChar).Value = Nombreup;
                cmd.Parameters.Add("@EMBARCAR_A", SqlDbType.NVarChar).Value = Nombreup;
                cmd.Parameters.Add("@DIRECCION", SqlDbType.NVarChar).Value = Direccup;
                cmd.Parameters.Add("@DOCUMENTO", SqlDbType.NVarChar).Value = Docup;
                cmd.Parameters.Add("@RUTA", SqlDbType.NVarChar).Value = Rutaup;


                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

            }
            con.Desconectar("EX");
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar2.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Actualizacion Realizada");
            progressBar2.Value = 0;

            button1_Click(null, null);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            worker.RunWorkerAsync();
        }

        private void dataGridView2_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            int valor = dataGridView2.Rows.Count;
            if (valor > 0)
            {
                button2.Enabled = true;
            }
        }
    }
}
