using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;

namespace Sinconizacion_EXactus
{
    public partial class Update_Clientes_FR : Form
    {
        public Update_Clientes_FR()
        {
            InitializeComponent();
        }

        //conexion conet = new conexion();
        conexionXML con = new conexionXML();
        DataTable clie_dismo = new DataTable();
        DataTable clie_FR = new DataTable();
        DataTable clie_BUSQ = new DataTable();
        String empresa = Login.empresa;
        BackgroundWorker worker = new BackgroundWorker();
        public String Cliente;
        public String Clienteup;
        public String Nombreup;
        private void button1_Click(object sender, EventArgs e)
        {
            clie_dismo.Clear();
            clie_FR.Clear();
            clie_dismo.Columns.Clear();

            con.conectar("EX");

            if (this.radioButton2.Checked)
            {

                SqlCommand cmd = new SqlCommand("SELECT A.[CLIENTE],A.[NOMBRE],A.[ALIAS],A.[DIRECCION],A.[TELEFONO1],A.[CONTRIBUYENTE],A.[PAIS],A.[ZONA],A.[RUTA],A.[E_MAIL],A.[RUBRO1_CLI],A.[RUBRO2_CLI],A.FCH_HORA_ULT_MOD,A.USUARIO_ULT_MOD  FROM [EXACTUS].["+empresa+"].[CLIENTE] A  where  DATEADD(dd, 0, DATEDIFF(dd, 0, FCH_HORA_ULT_MOD)) = '" + this.dateTimePicker1.Value.ToString("yyyy-MM-dd") + " '  and CLIENTE in (SELECT CLIENTE FROM [EXACTUS].[ERPADMIN].[CLIENTE_RT])  and NOMBRE not in (SELECT NOMBRE FROM [EXACTUS].[ERPADMIN].[CLIENTE_RT] where CLIENTE  >'30000' ) and SUBSTRING (NOMBRE,0,31) not in (SELECT NOMBRE FROM [EXACTUS].[ERPADMIN].[CLIENTE_RT] where CLIENTE > '30000') and CLIENTE > '30000'", con.conex);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(clie_dismo);

            }

            else
                if (this.radioButton1.Checked)
                {
                    SqlCommand cmd = new SqlCommand("SELECT [CLIENTE],[NOMBRE],[ALIAS],[DIRECCION],[TELEFONO1],[CONTRIBUYENTE],[PAIS],[ZONA],[RUTA],[E_MAIL],[RUBRO1_CLI],[RUBRO2_CLI],FCH_HORA_ULT_MOD,USUARIO_ULT_MOD  FROM [EXACTUS].["+empresa+"].[CLIENTE]  where  DATEADD(dd, 0, DATEDIFF(dd, 0, FCH_HORA_ULT_MOD)) = ' " + this.dateTimePicker1.Value.ToString("yyyy-MM-dd") + " '  and CLIENTE in (SELECT CLIENTE FROM [EXACTUS].[ERPADMIN].[CLIENTE_RT])  and NOMBRE not in (SELECT NOMBRE FROM [EXACTUS].[ERPADMIN].[CLIENTE_RT]  ) and SUBSTRING (NOMBRE,0,31) not in (SELECT NOMBRE FROM [EXACTUS].[ERPADMIN].[CLIENTE_RT] ) and CLIENTE <'30000'", con.conex);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(clie_dismo);
                
                }
   
            con.Desconectar("EX");

            dataGridView1.DataSource = clie_dismo;
            dataGridView1.Refresh();
            
            
            con.conectar("EX");

            for (int i = 0; i < clie_dismo.Rows.Count; i++)
            {
                DataRow row = clie_dismo.Rows[i];
                Cliente = Convert.ToString(row["CLIENTE"]);



                SqlCommand cmd1 = new SqlCommand("SELECT C.COD_ZON,A.CLIENTE,A.NOMBRE,B.NOM_CTO,B.ALIAS,B.NUM_TEL,C.DIR_CLT FROM [EXACTUS].[ERPADMIN].[CLIENTE_RT] A left JOIN  [EXACTUS].[ERPADMIN].[CLIENTE_CIA] B ON A.CLIENTE = B.COD_CLT  left join [EXACTUS].[ERPADMIN].[CLIENTE] C on A.CLIENTE = C.COD_CLT where A.CLIENTE ='" + Cliente + "' ", con.conex);
                SqlDataReader dr1 = cmd1.ExecuteReader();

                while (dr1.Read())
                {

                    clie_FR.Rows.Add(Convert.ToString(dr1["COD_ZON"]), Convert.ToString(dr1["CLIENTE"]), Convert.ToString(dr1["NOMBRE"]), Convert.ToString(dr1["NOM_CTO"]), Convert.ToString(dr1["ALIAS"]), Convert.ToString(dr1["NUM_TEL"]), Convert.ToString(dr1["DIR_CLT"]));

                
                }

                dr1.Close();

                
            }
            con.Desconectar("EX");
            dataGridView2.DataSource = clie_FR;
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
                string nombre = null;
                if (Nombreup.Length > 30)
                {
                    nombre = Nombreup.Substring(0, 30);
                }
                else
                {
                    nombre = Nombreup;
                }
                
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con.conex;
                cmd.CommandText = "UPDATE [EXACTUS].[ERPADMIN].[CLIENTE_RT] SET NOMBRE=@NOMBRE where CLIENTE ='" + Clienteup + "'";
                cmd.Parameters.Add("@NOMBRE", SqlDbType.NVarChar).Value = nombre;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

            }
            con.Desconectar("EX");
        }

        private void Update_Clientes_FR_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;
            this.radioButton2.Checked = true;
            
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


            clie_FR.Columns.Add("COD_ZON",typeof(string));
            clie_FR.Columns.Add("COD_CLT", typeof(string));
            clie_FR.Columns.Add("NOM_CLT", typeof(string));
            clie_FR.Columns.Add("NOM_CTO", typeof(string));
            clie_FR.Columns.Add("ALIAS", typeof(string));
            clie_FR.Columns.Add("NUM_TEL", typeof(string));
            clie_FR.Columns.Add("DIR_CLT", typeof(string));


            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;

            worker.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            worker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);

        }

        private void dataGridView2_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            int valor = dataGridView2.Rows.Count;
            if (valor > 0)
            {
                button2.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            worker.RunWorkerAsync();
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

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
