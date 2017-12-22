using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Data.SqlClient;
using System.IO;

namespace Sinconizacion_EXactus
{
    public partial class GPS_clientes : Form
    {
        public GPS_clientes()
        {
            InitializeComponent();
        }

        public static DataTable dt = new DataTable();
        Conexion2 conet = new Conexion2();
      
        public static string fecha_actual;
        public static string dia;
        public static string semana;

        private void Form8_Load(object sender, EventArgs e)
        {
              this.comboBox3.Text = "TODOS";
              this.comboBox2.Text = "SEMANA(A)";
              this.button2.Enabled = false;

         



            fecha_actual = DateTime.Now.ToString("ddMMyyyy", new System.Globalization.CultureInfo("es-ES"));

            conet.con.Open();

            SqlCommand cm2 = new SqlCommand("SELECT [RUTA]FROM [DM].[dbo].[RUTERO] GROUP BY RUTA ORDER BY RUTA  ", conet.con);
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
                comboBox1.Items.Add(dr2["RUTA"]);
            }
            dr2.Close();
            conet.con.Close();
        }

      

      

        private void button1_Click_1(object sender, EventArgs e)
        {
            dt.Clear();
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.Refresh();
            conet.con.Open();




            SqlCommand cmd = new SqlCommand("[CORRECT].[RUTEROGPS]", conet.con);
            cmd.CommandType = CommandType.StoredProcedure;
            if (this.comboBox1.Text == "")
            {

                cmd.Parameters.AddWithValue("@Ruta", null);

            }
            else if (this.comboBox2.Text == "")
            {
                cmd.Parameters.AddWithValue("@Semana", null);
            }
            else if (this.comboBox3.Text == "")
            {
                cmd.Parameters.AddWithValue("@Dia", null);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Ruta", comboBox1.Text);
                cmd.Parameters.AddWithValue("@Semana", semana);
                cmd.Parameters.AddWithValue("@Dia", dia);
            }


            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(dt);


            conet.con.Close();


            dataGridView1.DataSource = dt;
            dataGridView1.Refresh();

            int numRows = dataGridView1.Rows.Count;

            label3.Text = Convert.ToString(numRows - 1);
            this.button2.Enabled = true;


        }

        private void button2_Click_1(object sender, EventArgs e)
        {

            if (Directory.Exists(@"C:\CORRECT\GPS\"))
            {

            }
            else
            {
                Directory.CreateDirectory(@"C:\CORRECT\GPS\");
            }
            

            string nombrefile = @"C:\CORRECT\GPS\" + this.comboBox1.Text + "." + this.comboBox2.Text + "." + this.comboBox3.Text + ".kml";

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
            writer.WriteCData("Puntos de Rutas" + this.comboBox1.Text + "_" + this.comboBox2.Text + "_" + this.comboBox3.Text);
            writer.WriteEndElement();
            writer.WriteElementString("name", this.comboBox1.Text + "." + this.comboBox3.Text);
            writer.WriteElementString("visibility", "0");
            writer.WriteElementString("open", "1");
            writer.WriteStartElement("Folder");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string lat = dt.Rows[i][1].ToString();
                string lon = dt.Rows[i][2].ToString();
                writer.WriteStartElement("Placemark");
                writer.WriteStartElement("description");
                writer.WriteCData("" + " " + dt.Rows[i][6].ToString() + "<br />" + ": " + dt.Rows[i][5].ToString() + "  <br />  " + dt.Rows[i][3].ToString() + " <br />" + dt.Rows[i][4].ToString());
                writer.WriteEndElement();
                writer.WriteElementString("name", dt.Rows[i][0].ToString());
                writer.WriteElementString("visibility", "1");

                writer.WriteStartElement("Style");
                writer.WriteStartElement("IconStyle");
                writer.WriteStartElement("Icon");
                switch (dt.Rows[i][5].ToString())
                {
                    case "LUNES":
                        writer.WriteElementString("href", "http://maps.google.com/mapfiles/kml/pushpin/ylw-pushpin.png");
                        break;
                    case "MARTES":
                        writer.WriteElementString("href", "http://maps.google.com/mapfiles/kml/pushpin/blue-pushpin.png");
                        break;
                    case "MIERCOLES":
                        writer.WriteElementString("href", "http://maps.google.com/mapfiles/kml/pushpin/red-pushpin.png");
                        break;
                    case "JUEVES":
                        writer.WriteElementString("href", "http://maps.google.com/mapfiles/kml/pushpin/grn-pushpin.png");
                        break;
                    case "VIERNES":
                        writer.WriteElementString("href", "http://maps.google.com/mapfiles/kml/pushpin/pink-pushpin.png");
                        break;
                    case "SABADO":
                        writer.WriteElementString("href", "http://maps.google.com/mapfiles/kml/pushpin/wht-pushpin.png");
                        break;
                    case "ESPECIAL":
                        writer.WriteElementString("href", "http://maps.google.com/mapfiles/kml/pushpin/ltblu-pushpin.png");
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


            MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Desea ver los puntos en GoogleEarth", "PUNTEO", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                try
                {
                    try
                    {
                        System.Diagnostics.Process.Start(@"%programfiles%\Google\Google Earth\client\googleearth.exe", nombrefile);
                        
                    }
                    catch
                    {
                        System.Diagnostics.Process.Start(@"C:\Program Files (x86)\Google\Google Earth\client\googleearth.exe", nombrefile);
                    }
                }
                catch
                {
                    MessageBox.Show("ERROR GOOGLE EARTH NO ESTA INSTALADO", "ERROR");
                }
            }
            else 
            {

                MessageBox.Show(nombrefile,"Archivo Guardado Corectamente"  );
            }

        }

        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            switch (this.comboBox2.Text)
            {
                case "SEMANA(A)":
                    semana = "A";
                    break;
                case "SEMANA(B)":
                    semana = "B";
                    break;
                case "SEMANA(AB)":
                    semana = null;
                    break;

                default:
                    semana = null;
                    break;
            }

        }

        private void comboBox3_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            switch (this.comboBox3.Text)
            {
                case "Lunes":
                    dia = "0";
                    break;
                case "Martes":
                    dia = "1";
                    break;
                case "Miercoles":
                    dia = "2";
                    break;
                case "Jueves":
                    dia = "3";
                    break;
                case "Viernes":
                    dia = "4";
                    break;
                case "Sabado":
                    dia = "5";
                    break;

                case "Especial":
                    dia = "6";
                    break;

                case "TODOS":
                    dia = null;
                    break;

                default:
                    dia = null;
                    break;


            }


        }

        private void comboBox3_MouseClick(object sender, MouseEventArgs e)
        {
            this.button2.Enabled = false;
        }

        private void comboBox1_MouseClick(object sender, MouseEventArgs e)
        {
            this.button2.Enabled = false;
        }

        private void comboBox2_MouseClick(object sender, MouseEventArgs e)
        {
            this.button2.Enabled = false;
        }


        
    }
}
