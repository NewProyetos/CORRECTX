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
using System.Data.OleDb;
using System.Xml;


namespace Sinconizacion_EXactus
{
    public partial class ExceltoKML : Form
    {
        public ExceltoKML()
        {
            InitializeComponent();
        }
        public static String Selected_File;
        DataTable dt = new DataTable();
        DataTable exact = new DataTable();
        DataTable Rutas = new DataTable();
        DataTable Areas = new DataTable();
        DataTable resultado = new DataTable();
        //conexion conex = new conexion();
        conexionXML con = new conexionXML();
        BackgroundWorker worker = new BackgroundWorker();
        public static int registros;

        public Thread trd;

        private void ExceltoKML_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button1.Enabled = false;
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;


            label1.Hide();
            textBox2.Text = "100";

            resultado.Columns.Add("LATITUD", typeof(string));
            resultado.Columns.Add("LONGITUD", typeof(string));
            resultado.Columns.Add("CLIENTEK", typeof(string));
            resultado.Columns.Add("CLIENTEXACT", typeof(string));
            resultado.Columns.Add("METROS", typeof(double));
            resultado.Columns.Add("RUTA", typeof(string));
            resultado.Columns.Add("NOMBREK", typeof(string));
            resultado.Columns.Add("DIRECCION", typeof(string));


            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
       
            worker.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            worker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);

            
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("SELECT A.[RUTA],C.E_MAIL FROM [EXACTUS].[ERPADMIN].[CLIENTE_UBICACION] A LEFT JOIN [EXACTUS].[ERPADMIN].[RUTA_ASIGNADA_RT] B on A.RUTA = B.RUTA INNER JOIN [EXACTUS].[dismo].[VENDEDOR] C on B.AGENTE = C.VENDEDOR Group by A.[RUTA],C.E_MAIL", con.conex);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(Rutas);

            this.comboBox1.DataSource = Rutas;
            this.comboBox1.DisplayMember = "RUTA";

            SqlCommand cmd1 = new SqlCommand("SELECT [E_MAIL] as 'AREA' FROM [EXACTUS].[dismo].[VENDEDOR]  group by E_MAIL", con.conex);
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            da1.Fill(Areas);

            this.comboBox2.DataSource = Areas;
            this.comboBox2.DisplayMember = "AREA";
            
            con.Desconectar("EX");

            this.comboBox1.Text = "TODAS";
            this.comboBox2.Text = "TODAS";

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;


            double dtr;
            double rtd;
            double cl = 0;
            dtr = Math.PI / 180;
            rtd = 180 / Math.PI;

            double result;



            //try
            //{



            for (int i = 1; i < dt.Rows.Count; i++)
            {
                registros = i;



                int percentage = (i + 1) * 100 / dt.Rows.Count;
                worker.ReportProgress(percentage);



                result = 0;
                for (int j = 1; j < exact.Rows.Count; j++)
                {
                    DataRow row = dt.Rows[i];
                    DataRow row2 = exact.Rows[j];
                    double latitud1 = Convert.ToDouble(row["LATITUD"]);
                    double latitud2 = Convert.ToDouble(row2["LATITUD"]);
                    double longitud1 = Convert.ToDouble(row["LONGITUD"]);
                    double longitud2 = Convert.ToDouble(row2["LONGITUD"]);

                    cl = Math.Acos((Math.Sin(latitud1 * dtr) * Math.Sin(latitud2 * dtr)) + (Math.Cos(latitud1 * dtr) * Math.Cos(latitud2 * dtr) * Math.Cos((longitud1 - longitud2) * dtr))) * rtd * 111.302 * 1000;

                    if (cl < Convert.ToDouble(this.textBox2.Text))
                    {

                        result = cl;
                        resultado.Rows.Add(Convert.ToString(row["LATITUD"]), Convert.ToString(row["LONGITUD"]), Convert.ToString(row["Codigo"]), Convert.ToString(row2["CLIENTE"]), result, Convert.ToString(row2["RUTA"]), Convert.ToString(row["Nombre Clientes"]), Convert.ToString(row["DIRECCION"]));



                    }
                    else
                    {

                    }

                }



            }


        


          
                     

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label1.Text = (e.ProgressPercentage.ToString() + "%");
            this.progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Processo Cancelado");

            }
            else if (e.Error != null)
            {
                MessageBox.Show("Error en el processo" + e.Error.ToString());
            }
            else
            {
                fill_DataGrid();
                this.progressBar1.Value = 0;
                label1.Text = "0%";
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            button2.Enabled = true;
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            exact.Clear();
            exact.Columns.Clear();

            string latitud = Convert.ToString(dt.Columns[0].ColumnName);
            string longitud = Convert.ToString(dt.Columns[1].ColumnName);
            string codigo = Convert.ToString(dt.Columns[2].ColumnName);
            string Nombre_Clientes = Convert.ToString(dt.Columns[3].ColumnName);
            string Direccion = Convert.ToString(dt.Columns[4].ColumnName);
            string Icon = Convert.ToString(dt.Columns[5].ColumnName);


            if (latitud == "Latitud" || latitud == "latitud" || latitud == "LATITUD" || longitud == "Longitud" || longitud == "longitud" || longitud == "LONGITUD" || codigo == "Codigo" || codigo == "codigo" || codigo == "CODIGO" || Nombre_Clientes == "Nombre Clientes" || Nombre_Clientes == "nombre clientes" || Nombre_Clientes == "NOMBRE CLIENTES" || Direccion == "Direccion" || Direccion == "direccion" || Direccion == "DIRECCION" || Icon == "icon" || Icon == "Icon" || Icon == "ICON")
            {


                if (this.comboBox1.Text == "TODAS" && this.comboBox2.Text == "TODAS")
                {


                    con.conectar("EX");
                    SqlCommand cmd = new SqlCommand("SELECT A.[LATITUD],A.[LONGITUD],A.[RUTA],A.[CLIENTE] FROM [EXACTUS].[ERPADMIN].[CLIENTE_UBICACION] A LEFT JOIN [EXACTUS].[ERPADMIN].[RUTA_ASIGNADA_RT] B on A.RUTA = B.RUTA INNER JOIN [EXACTUS].[dismo].[VENDEDOR] C on B.AGENTE = C.VENDEDOR ", con.conex);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(exact);
                    con.Desconectar("EX");




                    label1.Show();

                    worker.RunWorkerAsync();
                }

                else
                    if (this.comboBox1.Text == "TODAS" && this.comboBox2.Text != "TODAS")
                    {
                        con.conectar("EX");
                        SqlCommand cmd = new SqlCommand("SELECT A.[LATITUD],A.[LONGITUD],A.[RUTA],A.[CLIENTE] FROM [EXACTUS].[ERPADMIN].[CLIENTE_UBICACION] A LEFT JOIN [EXACTUS].[ERPADMIN].[RUTA_ASIGNADA_RT] B on A.RUTA = B.RUTA INNER JOIN [EXACTUS].[dismo].[VENDEDOR] C on B.AGENTE = C.VENDEDOR where C.E_MAIL ='" + this.comboBox2.Text + "'", con.conex);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(exact);
                        con.Desconectar("EX");



                        label1.Show();
                        worker.RunWorkerAsync();
                    }


                    else if (this.comboBox2.Text == "TODAS" && this.comboBox1.Text != "TODAS")
                    {
                        con.conectar("EX");
                        SqlCommand cmd = new SqlCommand("SELECT A.[LATITUD],A.[LONGITUD],A.[RUTA],A.[CLIENTE] FROM [EXACTUS].[ERPADMIN].[CLIENTE_UBICACION] A LEFT JOIN [EXACTUS].[ERPADMIN].[RUTA_ASIGNADA_RT] B on A.RUTA = B.RUTA INNER JOIN [EXACTUS].[dismo].[VENDEDOR] C on B.AGENTE = C.VENDEDOR where A.RUTA ='" + this.comboBox1.Text + "'", con.conex);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(exact);
                        con.Desconectar("EX");


                        label1.Show();
                        worker.RunWorkerAsync();

                    }
            }



            else
            {
                MessageBox.Show("Estructura de Archivo de Excel Incorrecto Revise los Encabezados");
            }
        }

        private void fill_DataGrid()
        {
            dataGridView1.Columns.Clear();
            dataGridView2.DataSource = resultado;
            dataGridView2.Refresh();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.comboBox2.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
          

            button2.Enabled = false;

            if (textBox1.Text == "" || textBox1.Text == null)
            {
                MessageBox.Show("no se a selecionado un archivo de Excel");

            }
            else
            {
                if (radioButton2.Checked)
                {
                    try
                    {


                        OleDbConnection cnn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Selected_File + "; Extended Properties=Excel 12.0;");



                        OleDbCommand oconn = new OleDbCommand("select * from [Sheet1$]", cnn);


                        cnn.Open();
                        OleDbDataAdapter adp = new OleDbDataAdapter(oconn);
                        dt.Clear();
                        dt.Columns.Clear();
                        adp.Fill(dt);
                        dataGridView1.DataSource = dt;
                        dataGridView1.Refresh();
                    }
                    catch
                    {
                        MessageBox.Show("Archivo de Excel en Español, Cambie de Idioma");
                    }
                }

                if (radioButton1.Checked)
                {
                    try
                    {
                        OleDbConnection cnn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Selected_File + "; Extended Properties=Excel 12.0;");



                        OleDbCommand oconn = new OleDbCommand("select * from [Hoja1$]", cnn);


                        cnn.Open();
                        OleDbDataAdapter adp = new OleDbDataAdapter(oconn);
                        dt.Clear();
                        dt.Columns.Clear();
                        adp.Fill(dt);
                        dataGridView1.DataSource = dt;
                        dataGridView1.Refresh();
                    }
                    catch
                    {
                        MessageBox.Show("Archivo de Excel en Ingles Cambie, de Idioma");

                    }
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Selected_File = string.Empty;
            this.textBox1.Clear();
            openFileDialog1.AutoUpgradeEnabled = false;
            openFileDialog1.InitialDirectory = @"%USERPROFILE%\Documents";
            openFileDialog1.Title = "Select a File";
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.Filter = "Excel 2007 Files|*.xlsx| Excel 2003 Files|*.xls";
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {

            }
            else
            {
                Selected_File = openFileDialog1.FileName;
                this.textBox1.Text = Selected_File;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {



            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Kml File|*.Kml";
            saveFileDialog1.Title = "Save an Kml File";
            saveFileDialog1.FileName = "Archivo Goole Hearth.kml";
            saveFileDialog1.ShowDialog();



            string nombrefile = saveFileDialog1.FileName;



            //Definimos el archivo XML
            XmlTextWriter writer = new
            XmlTextWriter(nombrefile, Encoding.UTF8);

            // Empezamos a escribir
            writer.WriteStartDocument();
            writer.WriteStartElement("kml");
            writer.WriteAttributeString("xmlns", "http://earth.google.com/kml/2.0");
            writer.WriteStartElement("Folder");
            writer.WriteStartElement("description");

            //Descripcion del Conjunto de Datos,puede ser texto o HTML
            writer.WriteCData("Puntos" + nombrefile + "");
            writer.WriteEndElement();
            writer.WriteElementString("name", nombrefile);
            writer.WriteElementString("visibility", "0");
            writer.WriteElementString("open", "1");
            writer.WriteStartElement("Folder");

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                
                string lat = dt.Rows[i][0].ToString();
                string lon = dt.Rows[i][1].ToString();
                writer.WriteStartElement("Placemark");
                writer.WriteStartElement("description");
                writer.WriteCData("Boleta" + " " + dt.Rows[i][5].ToString() + "<br />" + "Correlativo: " + dt.Rows[i][2].ToString() + "<br />" + "Telefono: " + dt.Rows[i][7].ToString() + "<br />" + "DIRECCION: " + dt.Rows[i][4].ToString() + "<br />" + "Departamento: " + dt.Rows[i][8].ToString() + "<br />" + "Ubicacion: " + dt.Rows[i][9].ToString() + "<br />" + "GPS: " + dt.Rows[i][6].ToString());
                writer.WriteEndElement();
                writer.WriteElementString("name", dt.Rows[i][3].ToString());
                writer.WriteElementString("visibility", "1");

                writer.WriteStartElement("Style");
                writer.WriteStartElement("IconStyle");
                writer.WriteStartElement("Icon");
                switch (dt.Rows[i][10].ToString())
                {
                    case "100":
                        writer.WriteElementString("href", "http://maps.google.com/mapfiles/kml/pushpin/purple-pushpin.png");
                        break;
                    case "111":
                        writer.WriteElementString("href", "http://maps.google.com/mapfiles/kml/pushpin/ylw-pushpin.png");
                        break;
                    case "112":
                        writer.WriteElementString("href", "http://maps.google.com/mapfiles/kml/pushpin/blue-pushpin.png");
                        break;
                    case "113":
                        writer.WriteElementString("href", "http://maps.google.com/mapfiles/kml/pushpin/grn-pushpin.png");
                        break;
                    case "114":
                        writer.WriteElementString("href", "http://maps.google.com/mapfiles/kml/pushpin/pink-pushpin.png");
                        break;
                    case "115":
                        writer.WriteElementString("href", "http://maps.google.com/mapfiles/kml/pushpin/wht-pushpin.png");
                        break;
                    case "116":
                        writer.WriteElementString("href", "http://maps.google.com/mapfiles/kml/shapes/caution.png");
                        break;
                    default:
                        writer.WriteElementString("href", "http://maps.google.com/mapfiles/kml/pushpin/ylw-pushpin.png");
                        break;
                }
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();


                writer.WriteStartElement("LookAt");
                writer.WriteElementString("longitude", lon);
                writer.WriteElementString("latitude", lat);
                writer.WriteElementString("range", "3000");
                writer.WriteElementString("tilt", "60");
                writer.WriteElementString("heading", "0");
                writer.WriteEndElement();
                writer.WriteStartElement("Point");
                writer.WriteElementString("extrude", "1");
                writer.WriteElementString("altitudeMode", "relativeToGround");
                writer.WriteElementString("coordinates", lon + "," + lat + ",3");
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();


            
        }

        
    }
}
