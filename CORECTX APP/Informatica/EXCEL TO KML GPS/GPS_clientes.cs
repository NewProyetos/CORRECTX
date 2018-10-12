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
using DevExpress.XtraGrid.Views.Grid;
using System.Web.UI.WebControls;
using DevExpress.XtraExport.Helpers;
using DevExpress.Data.Filtering;

namespace Sinconizacion_EXactus
{
    public partial class GPS_clientes : Form
    {
        public GPS_clientes()
        {
            InitializeComponent();
        }

        public static DataTable dt = new DataTable();
        conexionXML con = new conexionXML();
      
        public static string fecha_actual;
        public static string dia;
        public static string semana;
        string RUTA;
        int tip_rep;
        string name_file;


        private void Form8_Load(object sender, EventArgs e)
        {
            groupBox3.Hide();
            
            radioButton1.Checked = true;
            tip_rep = 0;
            dia = "Todos";
            semana = "AB";
            RUTA = "TODAS";
            this.button2.Enabled = false;
            this.button4.Enabled = false;
            this.button3.Enabled = false;

            if (Main_Menu.GeneraKMLcliente == "S")
            {
                button2.Show();
                label6.Show();
               
            }
            else
            {
                button2.Hide();
                label6.Hide();
               
            }

            if (Main_Menu.guardarKMLcliente == "S")
            {
                button4.Show();
                label4.Show();
            }
            else
            {
                button4.Hide();
                label4.Hide();
            }



            if (Main_Menu.solosnGPS == "S")
            {
                checkBox3.Checked = true;
                checkBox3.Enabled = false;
            }
            

            //dataGridView1.Enabled = true;
            //dataGridView1.RowHeadersVisible = false;
            ////dataGridView1.AutoResizeColumns();
            //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            //dataGridView1.ReadOnly = true;




            //  this.comboBox3.Text = "TODOS";
            //  this.comboBox2.Text = "SEMANA(A)";
            //  this.button2.Enabled = false;





            fecha_actual = DateTime.Now.ToString("ddMMyyyy_hhmmss", new System.Globalization.CultureInfo("es-ES"));

            //con.conectar("DM");

            //SqlCommand cm2 = new SqlCommand("SELECT [RUTA]FROM [DM].[dbo].[RUTERO] GROUP BY RUTA ORDER BY RUTA  ", con.condm);
            //SqlDataReader dr2 = cm2.ExecuteReader();
            //while (dr2.Read())
            //{
            //    comboBox1.Items.Add(dr2["RUTA"]);
            //}
            //dr2.Close();
            //con.Desconectar("DM");


        }

        public void combo(DataTable dts1,string valor,ComboBox cbx)
        {
            cbx.Items.Clear();


            var result = from row in dts1.AsEnumerable()
                         group row by row.Field<string>(valor) into grp
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
                    cbx.Items.Add(t.Vendedor);
                }
            }

        }



        private void button1_Click_1(object sender, EventArgs e)
        {
            dt.Clear();

            if (dt.Columns.Count >= 1)
                
            {
                dt.Columns.Clear();
            }
            if (gridView1.Columns.Count >= 1)
            {
                gridView1.Columns.Clear();
            }


            gridControl1.DataSource = null;

          
         //  gridControl1.Refresh();


            if (tip_rep == 2)
            {
                string fechaini = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                string fechafin = dateTimePicker2.Value.ToString("yyyy-MM-dd");

                con.conectar("DM");
                SqlCommand cmd = new SqlCommand("[CORRECT].[VISITA_DISTANCIA]", con.condm);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@empresa", Login.empresa);
                cmd.Parameters.AddWithValue("@fecha_ini", fechaini+" 00:00:00.000");
                cmd.Parameters.AddWithValue("@fecha_fin", fechafin+" 23:59:59.000");


                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);


                con.Desconectar("DM");

                gridControl1.DataSource = dt;

                int numRows = gridView1.RowCount;


                label3.Text = Convert.ToString(numRows - 1);
                if (numRows - 1 > 0)
                {
                    button3.Enabled = true;
                }


              }
          else
            {
                con.conectar("DM");
                SqlCommand cmd = new SqlCommand("[CORRECT].[RUTEROGPS]", con.condm);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@empresa", Login.empresa);

                if (checkBox3.Checked)
                {
                    cmd.Parameters.AddWithValue("@geo", 0);
                }
                else

                {
                    cmd.Parameters.AddWithValue("@geo", 1);

                }
                cmd.Parameters.AddWithValue("@tipo_rep", tip_rep);



                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);


                con.Desconectar("DM");

                gridControl1.DataSource = dt;



                int numRows = gridView1.RowCount;


                label3.Text = Convert.ToString(numRows - 1);
                if (numRows - 1 > 0)
                {

                    if (checkBox3.Checked)
                    {
                        this.button2.Enabled = false;
                        this.button4.Enabled = false;
                    }
                    else
                    {
                        this.button2.Enabled = true;
                        this.button4.Enabled = true;
                    }
                    this.button3.Enabled = true;


                }
                else
                {

                    this.button2.Enabled = false;
                    this.button4.Enabled = false;
                    this.button3.Enabled = false;
                }


            }
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


           string nombrefiles = @"C:\CORRECT\GPS\puntos_" + fecha_actual + ".kml";
            createkml(nombrefiles,"ver");


        }

        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            //semana = comboBox2.Text;

            //dt.DefaultView.RowFilter = "SEMANA = '" + semana + "'";
            //dataGridView1.DataSource = dt;
        }

        private void comboBox3_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            //dia = comboBox3.Text;

            //dt.DefaultView.RowFilter = "DIAVISITA = '" + dia + "'";
            //dataGridView1.DataSource = dt;

        }

        private void comboBox3_MouseClick(object sender, MouseEventArgs e)
        {
          //  this.button2.Enabled = false;
        }

        private void comboBox1_MouseClick(object sender, MouseEventArgs e)
        {
          //  this.button2.Enabled = false;
        }

        private void comboBox2_MouseClick(object sender, MouseEventArgs e)
        {
        //    this.button2.Enabled = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //if (checkBox1.Checked)
            //{
            //    //dt.DefaultView.RowFilter = "ACTIVO = 'N'";
            //    //dataGridView1.DataSource = dt;
            //}
            //else
            //{
            //    //dt.DefaultView.RowFilter = "ACTIVO = 'S'";
            //    //dataGridView1.DataSource = dt;
            //}
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //RUTA = comboBox1.Text;

            //dt.DefaultView.RowFilter = "RUTA = '" + RUTA + "'";
            //dataGridView1.DataSource = dt;


        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            //dt.DefaultView.RowFilter = "RUTA = ''";
            //dataGridView1.DataSource = dt;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            //if (checkBox2.Checked)
            //{
                
            //}
            //else
            //{
            //    dt.DefaultView.RowFilter = "NOMBRE <> 'DISPONIBLE'";
            //}

            //dataGridView1.DataSource = dt;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                button2.Enabled = false;

            }
            else
            {
                button2.Enabled = true;
            }

            button1_Click_1(null, null);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            

            string FileName = "C:\\CORRECT\\XLS\\ "+ name_file + "_" + fecha_actual + ".xlsx";
            gridView1.ExportToXlsx(FileName);



            Microsoft.Office.Interop.Excel.Application excell;
            Microsoft.Office.Interop.Excel.Workbook workbook;
            excell = new Microsoft.Office.Interop.Excel.Application();
            excell.Visible = true;
            workbook = excell.Workbooks.Open(FileName);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                tip_rep = 0;
                name_file = "GPS_RUTERO";
            }
            else
            {
                tip_rep = 1;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                tip_rep = 1;
                name_file = "GPS_CLIENTES";
            }
            else
            {
                tip_rep = 0;

            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            saveFileDialog1.InitialDirectory = @"C:\CORRECT";
            saveFileDialog1.Title = "Guardar KML";
            saveFileDialog1.DefaultExt = "kml";
            saveFileDialog1.Filter = "kml files (*.kml)|*.kml";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
               string  nombrefiles = saveFileDialog1.FileName;
                createkml(nombrefiles,"guardar");
            }
        }

      

        private void createkml(string nombrefile,string tipo_eje)
        {
             

            string ad = "RUTERO COMPLETO";        
               
            

            //Definimos el archivo XML
            XmlTextWriter writer = new
            XmlTextWriter(nombrefile, Encoding.UTF8);

            // Empezamos a escribir
            writer.WriteStartDocument();
            writer.WriteStartElement("kml");
            writer.WriteAttributeString("xmlns", "http://earth.google.com/kml/2.0");
            writer.WriteStartElement("Folder");
            writer.WriteStartElement("description");


            CriteriaOperator op = gridView1.ActiveFilterCriteria;
            string filterStnring = DevExpress.Data.Filtering.CriteriaToWhereClauseHelper.GetDataSetWhere(op);
            //  string filterStnring2 = DevExpress.Data.Filtering.Crite;

            if (filterStnring == "")
            { }
            else
            {
                ad = op.LegacyToString();
            }





            //Descripcion del Conjunto de Datos,puede ser texto o HTML
            writer.WriteCData("Puntos" + ad + "");
            writer.WriteEndElement();
            writer.WriteElementString("name", ad);
            writer.WriteElementString("visibility", "0");
            writer.WriteElementString("open", "1");
            writer.WriteStartElement("Folder");


            //   for (int i = 0; i < gridView1.DataRowCount; i++)
            //   {
            //       string v = "";

            //       v = gridView1.GetRowCellValue(i, "LATITUD").ToString();

            ////do something
            //     }



            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                // DataRow dtr = dt.Rows[i];


                string lat = gridView1.GetRowCellValue(i, "LATITUD").ToString();
                string lon = gridView1.GetRowCellValue(i, "LONGITUD").ToString();
                writer.WriteStartElement("Placemark");
                writer.WriteStartElement("description");

                writer.WriteCData("" + "RUTA:" + gridView1.GetRowCellValue(i, "RUTA").ToString() + "<br />" + "SEMANA: " + gridView1.GetRowCellValue(i, "SEMANA").ToString() + "  <br />  " + "DIA: " + gridView1.GetRowCellValue(i, "DIAVISITA").ToString() + " <br />" + "NOMBRE: " + gridView1.GetRowCellValue(i, "NOMBRE").ToString() + " <br />" + "DIRECCION: " + gridView1.GetRowCellValue(i, "DIRECCION").ToString());

                writer.WriteEndElement();
                writer.WriteElementString("name", gridView1.GetRowCellValue(i, "CLIENTE").ToString());
                writer.WriteElementString("visibility", "1");

                writer.WriteStartElement("Style");
                writer.WriteStartElement("IconStyle");
                writer.WriteStartElement("Icon");
                switch (gridView1.GetRowCellValue(i, "DIAVISITA").ToString())
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




            if (tipo_eje == "ver")
            {
                System.Diagnostics.Process.Start(nombrefile);
            }
            else
            {
                MessageBox.Show(nombrefile, "Archivo Guardado Corectamente");
            }

            //MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
            //DialogResult result = MessageBox.Show("Desea ver los puntos en GoogleEarth", "PUNTEO", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            //if (result == DialogResult.Yes)
            //{
            //    try
            //    {
            //        try
            //        {
            //            System.Diagnostics.Process.Start(nombrefile);

            //        }
            //        catch
            //        {

            //        }
            //    }
            //    catch
            //    {
            //        MessageBox.Show("ERROR GOOGLE EARTH NO ESTA INSTALADO", "ERROR");
            //    }
            //}
            //else
            //{

            //    MessageBox.Show(nombrefile, "Archivo Guardado Corectamente");
            //}






        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                tip_rep = 2;
                groupBox3.Show();
                groupBox4.Hide();
                checkBox3.Enabled = false;
                name_file = "VISITAS";
            }

            else
            {
                groupBox3.Hide();
                groupBox4.Show();
                checkBox3.Enabled = true;
            }

        }
    }
}
