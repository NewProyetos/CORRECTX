using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;
using System.IO;
using Microsoft.CSharp;


namespace Sinconizacion_EXactus
{
    public partial class OC : Form
    {
        public OC()
        {
            InitializeComponent();
        }

        DataSet dsdet = new DataSet();
        DataSet dsenc = new DataSet();
        DataSet dsdetv = new DataSet();
        DataSet dsencv = new DataSet();
        DataSet dsclasi = new DataSet();

        DataTable dt = new DataTable();
        DataTable dtalle = new DataTable();
        DataTable dtenc = new DataTable();
        DataTable dtallev = new DataTable();
        DataTable dtencv = new DataTable();
        DataTable dtclasi = new DataTable();
        //Conexion2 conet = new Conexion2();
        conexionXML con =  new conexionXML();

        private void Form14_Load(object sender, EventArgs e)
        {
            label3.Text = DateTime.Now.ToString("dd-MM-yyyy");
            this.radioButton2.Checked = true;
            con.conectar("DM");

            SqlCommand cmd = new SqlCommand("SELECT [RUTA]FROM [DM].[CORRECT].[RUTA_REGALIAS] where RUTA like 'R%'", con.condm);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

          

            con.Desconectar("DM");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(@"C:\CORRECT\Ruta OC+"))
            { 
            
            }
            else

            {

                Directory.CreateDirectory(@"C:\CORRECT\Ruta OC+");
            }

            if (radioButton1.Checked == true)
            {

                string fecha = DateTime.Now.ToString("dd-MM-yyyy");
                try
                {
                    Directory.CreateDirectory(@"C:\CORRECT\Ruta OC+\" + fecha + "");

                    for (int c = 0; c < dt.Rows.Count; c++)
                    {


                        string Ruta = Convert.ToString(dt.Rows[c][0]);


                        if (File.Exists(@"\\192.168.1.5\c$\OC\" + Ruta + @"\CLASIFIC.xml"))
                        {




                            //--- tabla detalle
                            XmlDocument doc = new XmlDocument();
                            doc.Load(@"\\192.168.1.5\c$\OC\" + Ruta + @"\DET_PRECIOS.xml");
                            XmlNodeReader rd = new XmlNodeReader(doc);
                            dsdet.ReadXml(rd);

                            dtalle = dsdet.Tables[0];

                            //--- tabla encabezasos 

                            rd.Close();

                            XmlDocument doc1 = new XmlDocument();
                            doc1.Load(@"\\192.168.1.5\c$\OC\" + Ruta + @"\ENC_PRECIO.xml");
                            XmlNodeReader rd1 = new XmlNodeReader(doc1);
                            dsenc.ReadXml(rd1);

                            dtenc = dsenc.Tables[0];

                            rd1.Close();

                            //-- enc volulem
                            XmlDocument doc2 = new XmlDocument();
                            doc2.Load(@"\\192.168.1.5\c$\OC\" + Ruta + @"\ENC_VOLUME.xml");
                            XmlNodeReader rd2 = new XmlNodeReader(doc2);
                            dsencv.ReadXml(rd2);

                            dtencv = dsencv.Tables[0];

                            rd2.Close();
                            // -- det Volule

                            XmlDocument doc3 = new XmlDocument();
                            doc3.Load(@"\\192.168.1.5\c$\OC\" + Ruta + @"\DET_VOLUME.xml");
                            XmlNodeReader rd3 = new XmlNodeReader(doc3);
                            dsdetv.ReadXml(rd3);

                            dtallev = dsdetv.Tables[0];

                            rd3.Close();

                            //-- clasificacion


                            XmlDocument doc4 = new XmlDocument();
                            doc4.Load(@"\\192.168.1.5\c$\OC\" + Ruta + @"\CLASIFIC.xml");
                            XmlNodeReader rd4 = new XmlNodeReader(doc4);
                            dsclasi.ReadXml(rd4);

                            dtclasi = dsclasi.Tables[0];

                            rd4.Close();

                        }
                    }
                    Excel.Application aplicacion;
                    Excel.Workbook libros_trabajo;
                    Excel.Worksheet hoja_trabajo;
                    Excel.Worksheet hoja_trabajo2;
                    Excel.Worksheet hoja_trabajo3;
                    Excel.Worksheet hoja_trabajo4;
                    Excel.Worksheet hoja_trabajo5;

                    Excel.Range rango_enc;
                    Excel.Range rango_name;
                    Excel.Range rango_det;
                    Excel.Range rango_encb;
                    Excel.Range rango_namev;
                    Excel.Range rango_detv;
                    Excel.Range rango_namedv;
                    Excel.Range rango_encbv;

                    Excel.Range rango_nameclasi;
                    Excel.Range rango_enclasi;



                    aplicacion = new Excel.Application();
                    libros_trabajo = aplicacion.Workbooks.Add();

                    Excel.Worksheet newWorksheet;
                    newWorksheet = (Excel.Worksheet)aplicacion.Worksheets.Add();

                    Excel.Worksheet newWorksheet2;
                    newWorksheet2 = (Excel.Worksheet)aplicacion.Worksheets.Add();


                    hoja_trabajo = (Excel.Worksheet)libros_trabajo.Worksheets.get_Item(1);
                    hoja_trabajo2 = (Excel.Worksheet)libros_trabajo.Worksheets.get_Item(2);
                    hoja_trabajo3 = (Excel.Worksheet)libros_trabajo.Worksheets.get_Item(3);
                    hoja_trabajo4 = (Excel.Worksheet)libros_trabajo.Worksheets.get_Item(4);
                    hoja_trabajo5 = (Excel.Worksheet)libros_trabajo.Worksheets.get_Item(5);

                    hoja_trabajo.Name = "DET_PRECIOS";
                    hoja_trabajo2.Name = "ENC_PRECIOS";
                    hoja_trabajo3.Name = "ENC_VOLUME";
                    hoja_trabajo4.Name = "DET_VOLUME";
                    hoja_trabajo5.Name = "CLASIFICACION";

                    rango_det = hoja_trabajo2.get_Range("A1", "G1");
                    rango_enc = hoja_trabajo.get_Range("A1", "G1");
                    rango_encbv = hoja_trabajo3.get_Range("A1", "G1");
                    rango_detv = hoja_trabajo4.get_Range("A1", "G1");
                    rango_enclasi = hoja_trabajo5.get_Range("A1", "I1");


                    rango_encb = hoja_trabajo2.get_Range("A3", "G3");
                    rango_name = hoja_trabajo.get_Range("A3", "G3");
                    rango_namev = hoja_trabajo3.get_Range("A3", "G3");
                    rango_namedv = hoja_trabajo4.get_Range("A3", "G3");
                    rango_nameclasi = hoja_trabajo5.get_Range("A3", "I3");



                    rango_enc.Font.Name = "Times New Roman";
                    rango_enc.Font.Size = 15;
                    rango_enc.Borders.LineStyle = Excel.XlLineStyle.xlDouble;

                    rango_encbv.Font.Name = "Times New Roman";
                    rango_encbv.Font.Size = 15;
                    rango_encbv.Borders.LineStyle = Excel.XlLineStyle.xlDouble;

                    rango_detv.Font.Name = "Times New Roman";
                    rango_detv.Font.Size = 15;
                    rango_detv.Borders.LineStyle = Excel.XlLineStyle.xlDouble;


                    rango_det.Font.Name = "Times New Roman";
                    rango_det.Font.Size = 15;
                    rango_det.Borders.LineStyle = Excel.XlLineStyle.xlDouble;

                    rango_enclasi.Font.Name = "Times New Roman";
                    rango_enclasi.Font.Size = 15;
                    rango_enclasi.Borders.LineStyle = Excel.XlLineStyle.xlDouble;


                    rango_name.Font.Name = "Times New Roman";
                    rango_name.Font.Color = Color.Blue;
                    rango_name.Font.Size = 10;
                    rango_name.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                    rango_namev.Font.Name = "Times New Roman";
                    rango_namev.Font.Color = Color.Blue;
                    rango_namev.Font.Size = 10;
                    rango_namev.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                    rango_namedv.Font.Name = "Times New Roman";
                    rango_namedv.Font.Color = Color.Blue;
                    rango_namedv.Font.Size = 10;
                    rango_namedv.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;


                    rango_encb.Font.Name = "Times New Roman";
                    rango_encb.Font.Color = Color.Blue;
                    rango_encb.Font.Size = 10;
                    rango_encb.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                    rango_nameclasi.Font.Name = "Times New Roman";
                    rango_nameclasi.Font.Color = Color.Blue;
                    rango_nameclasi.Font.Size = 10;
                    rango_nameclasi.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                    rango_enc.Font.Bold = true;
                    rango_encb.Font.Bold = true;
                    rango_det.Font.Bold = true;
                    rango_name.Font.Bold = true;
                    rango_namev.Font.Bold = true;
                    rango_namedv.Font.Bold = true;
                    rango_nameclasi.Font.Bold = true;

                    hoja_trabajo.Cells[1, 2] = "REPORTE DETALLE_PRECIOS DE OC+";
                    hoja_trabajo2.Cells[1, 2] = "REPORTE ENCABEZADO_PRECIOS DE OC+";
                    hoja_trabajo3.Cells[1, 2] = "REPORTE ENCABEZADO_VOLUMES DE OC+";
                    hoja_trabajo4.Cells[1, 2] = "REPORTE DETALLE_VOLUME DE OC+";
                    hoja_trabajo5.Cells[1, 4] = "REPORTE CLASIFICIONES DE OC+";










                    hoja_trabajo.Cells[3, 1] = Convert.ToString(dtalle.Columns[0].ColumnName);
                    hoja_trabajo.Cells[3, 2] = Convert.ToString(dtalle.Columns[1].ColumnName);
                    hoja_trabajo.Cells[3, 3] = Convert.ToString(dtalle.Columns[2].ColumnName);
                    hoja_trabajo.Cells[3, 4] = Convert.ToString(dtalle.Columns[3].ColumnName);
                    hoja_trabajo.Cells[3, 5] = Convert.ToString(dtalle.Columns[4].ColumnName);
                    hoja_trabajo.Cells[3, 6] = Convert.ToString(dtalle.Columns[5].ColumnName);
                    hoja_trabajo.Cells[3, 7] = Convert.ToString(dtalle.Columns[6].ColumnName);


                    hoja_trabajo4.Cells[3, 1] = Convert.ToString(dtallev.Columns[0].ColumnName);
                    hoja_trabajo4.Cells[3, 2] = Convert.ToString(dtallev.Columns[1].ColumnName);
                    hoja_trabajo4.Cells[3, 3] = Convert.ToString(dtallev.Columns[2].ColumnName);
                    hoja_trabajo4.Cells[3, 4] = Convert.ToString(dtallev.Columns[3].ColumnName);
                    hoja_trabajo4.Cells[3, 5] = Convert.ToString(dtallev.Columns[4].ColumnName);
                    hoja_trabajo4.Cells[3, 6] = Convert.ToString(dtallev.Columns[5].ColumnName);
                    hoja_trabajo4.Cells[3, 7] = Convert.ToString(dtallev.Columns[6].ColumnName);


                    hoja_trabajo2.Cells[3, 1] = Convert.ToString(dtencv.Columns[0].ColumnName);
                    hoja_trabajo2.Cells[3, 2] = Convert.ToString(dtencv.Columns[1].ColumnName);
                    hoja_trabajo2.Cells[3, 3] = Convert.ToString(dtencv.Columns[2].ColumnName);
                    hoja_trabajo2.Cells[3, 4] = Convert.ToString(dtencv.Columns[3].ColumnName);

                    hoja_trabajo3.Cells[3, 1] = Convert.ToString(dtenc.Columns[0].ColumnName);
                    hoja_trabajo3.Cells[3, 2] = Convert.ToString(dtenc.Columns[1].ColumnName);
                    hoja_trabajo3.Cells[3, 3] = Convert.ToString(dtenc.Columns[2].ColumnName);
                    hoja_trabajo3.Cells[3, 4] = Convert.ToString(dtenc.Columns[3].ColumnName);



                    hoja_trabajo5.Cells[3, 1] = Convert.ToString(dtclasi.Columns[0].ColumnName);
                    hoja_trabajo5.Cells[3, 2] = Convert.ToString(dtclasi.Columns[1].ColumnName);
                    hoja_trabajo5.Cells[3, 3] = Convert.ToString(dtclasi.Columns[2].ColumnName);
                    hoja_trabajo5.Cells[3, 4] = Convert.ToString(dtclasi.Columns[3].ColumnName);
                    hoja_trabajo5.Cells[3, 5] = Convert.ToString(dtclasi.Columns[4].ColumnName);
                    hoja_trabajo5.Cells[3, 6] = Convert.ToString(dtclasi.Columns[5].ColumnName);
                    hoja_trabajo5.Cells[3, 7] = Convert.ToString(dtclasi.Columns[6].ColumnName);
                    hoja_trabajo5.Cells[3, 8] = Convert.ToString(dtclasi.Columns[7].ColumnName);
                    hoja_trabajo5.Cells[3, 9] = Convert.ToString(dtclasi.Columns[8].ColumnName);


                    //Recorremos el DataGridView rellenando la hoja de trabajo


                    for (int i = 0; i < dtalle.Rows.Count; i++)
                    {

                        for (int j = 0; j < dtalle.Columns.Count; j++)
                        {


                            hoja_trabajo.Cells[i + 4, j + 1] = dtalle.Rows[i][j].ToString();
                        }
                    }

                    for (int i = 0; i < dtenc.Rows.Count; i++)
                    {

                        for (int j = 0; j < dtenc.Columns.Count; j++)
                        {


                            hoja_trabajo2.Cells[i + 4, j + 1] = dtenc.Rows[i][j].ToString();
                        }
                    }


                    for (int i = 0; i < dtencv.Rows.Count; i++)
                    {

                        for (int j = 0; j < dtencv.Columns.Count; j++)
                        {


                            hoja_trabajo3.Cells[i + 4, j + 1] = dtencv.Rows[i][j].ToString();
                        }
                    }

                    for (int i = 0; i < dtallev.Rows.Count; i++)
                    {

                        for (int j = 0; j < dtallev.Columns.Count; j++)
                        {


                            hoja_trabajo4.Cells[i + 4, j + 1] = dtallev.Rows[i][j].ToString();
                        }
                    }

                    for (int i = 0; i < dtclasi.Rows.Count; i++)
                    {

                        for (int j = 0; j < dtclasi.Columns.Count; j++)
                        {


                            hoja_trabajo5.Cells[i + 4, j + 1] = dtclasi.Rows[i][j].ToString();
                        }
                    }







                    //aplicacion.Visible = true;

                    if (Directory.Exists(@"C:\CORRECT\Ruta OC+\" + fecha + ""))
                    {

                        libros_trabajo.SaveAs(@"C:\CORRECT\Ruta OC+\" + fecha + "/" + "OC+");

                        libros_trabajo.Close(true);
                        aplicacion.Quit();
                    }

                    else
                    {
                        Directory.CreateDirectory(@"C:\CORRECT\Ruta OC+\" + fecha + "");

                    }
                    //}
                    //else
                    //{

                    //}

                }
                catch
                {
                    MessageBox.Show("*****ERRRORRR****");
                }

                this.Close();
            }

            else
                if (radioButton2.Checked == true)
                {

                    string fecha = DateTime.Now.ToString("dd-MM-yyyy");
                    try
                    {
                        Directory.CreateDirectory(@"C:\CORRECT\Ruta OC+\" + fecha + "");

                        for (int c = 0; c < dt.Rows.Count; c++)
                        {


                            string Ruta = Convert.ToString(dt.Rows[c][0]);


                            if (File.Exists(@"\\192.168.1.5\c$\OC\" + Ruta + @"\DET_PRECIOS.xml"))
                            {
                                
                                dsdet.Clear();
                                dtalle.Clear();
                                dtallev.Clear();
                                dtclasi.Clear();
                                dtenc.Clear();
                                dtencv.Clear();
                                

                                //--- tabla detalle
                                XmlDocument doc = new XmlDocument();
                                doc.Load(@"\\192.168.1.5\c$\OC\" + Ruta + @"\DET_PRECIOS.xml");
                                XmlNodeReader rd = new XmlNodeReader(doc);
                                dsdet.ReadXml(rd);

                                dtalle = dsdet.Tables[0];

                                //--- tabla encabezasos 

                                rd.Close();

                                XmlDocument doc1 = new XmlDocument();
                                doc1.Load(@"\\192.168.1.5\c$\OC\" + Ruta + @"\ENC_PRECIO.xml");
                                XmlNodeReader rd1 = new XmlNodeReader(doc1);
                                dsenc.ReadXml(rd1);

                                dtenc = dsenc.Tables[0];

                                rd1.Close();

                                //-- enc volulem
                                XmlDocument doc2 = new XmlDocument();
                                doc2.Load(@"\\192.168.1.5\c$\OC\" + Ruta + @"\ENC_VOLUME.xml");
                                XmlNodeReader rd2 = new XmlNodeReader(doc2);
                                dsencv.ReadXml(rd2);

                                dtencv = dsencv.Tables[0];

                                rd2.Close();
                                // -- det Volule

                                XmlDocument doc3 = new XmlDocument();
                                doc3.Load(@"\\192.168.1.5\c$\OC\" + Ruta + @"\DET_VOLUME.xml");
                                XmlNodeReader rd3 = new XmlNodeReader(doc3);
                                dsdetv.ReadXml(rd3);

                                dtallev = dsdetv.Tables[0];

                                rd3.Close();

                                //-- clasificacion


                                XmlDocument doc4 = new XmlDocument();
                                doc4.Load(@"\\192.168.1.5\c$\OC\" + Ruta + @"\CLASIFIC.xml");
                                XmlNodeReader rd4 = new XmlNodeReader(doc4);
                                dsclasi.ReadXml(rd4);

                                dtclasi = dsclasi.Tables[0];

                                rd4.Close();


                                Excel.Application aplicacion;
                                Excel.Workbook libros_trabajo;
                                Excel.Worksheet hoja_trabajo;
                                Excel.Worksheet hoja_trabajo2;
                                Excel.Worksheet hoja_trabajo3;
                                Excel.Worksheet hoja_trabajo4;
                                Excel.Worksheet hoja_trabajo5;

                                Excel.Range rango_enc;
                                Excel.Range rango_name;
                                Excel.Range rango_det;
                                Excel.Range rango_encb;
                                Excel.Range rango_namev;
                                Excel.Range rango_detv;
                                Excel.Range rango_namedv;
                                Excel.Range rango_encbv;

                                Excel.Range rango_nameclasi;
                                Excel.Range rango_enclasi;



                                aplicacion = new Excel.Application();
                                libros_trabajo = aplicacion.Workbooks.Add();

                                Excel.Worksheet newWorksheet;
                                newWorksheet = (Excel.Worksheet)aplicacion.Worksheets.Add();

                                Excel.Worksheet newWorksheet2;
                                newWorksheet2 = (Excel.Worksheet)aplicacion.Worksheets.Add();


                                hoja_trabajo = (Excel.Worksheet)libros_trabajo.Worksheets.get_Item(1);
                                hoja_trabajo2 = (Excel.Worksheet)libros_trabajo.Worksheets.get_Item(2);
                                hoja_trabajo3 = (Excel.Worksheet)libros_trabajo.Worksheets.get_Item(3);
                                hoja_trabajo4 = (Excel.Worksheet)libros_trabajo.Worksheets.get_Item(4);
                                hoja_trabajo5 = (Excel.Worksheet)libros_trabajo.Worksheets.get_Item(5);

                                hoja_trabajo.Name = "DET_PRECIOS";
                                hoja_trabajo2.Name = "ENC_PRECIOS";
                                hoja_trabajo3.Name = "ENC_VOLUME";
                                hoja_trabajo4.Name = "DET_VOLUME";
                                hoja_trabajo5.Name = "CLASIFICACION";

                                rango_det = hoja_trabajo2.get_Range("A1", "G1");
                                rango_enc = hoja_trabajo.get_Range("A1", "G1");
                                rango_encbv = hoja_trabajo3.get_Range("A1", "G1");
                                rango_detv = hoja_trabajo4.get_Range("A1", "G1");
                                rango_enclasi = hoja_trabajo5.get_Range("A1", "I1");


                                rango_encb = hoja_trabajo2.get_Range("A3", "G3");
                                rango_name = hoja_trabajo.get_Range("A3", "G3");
                                rango_namev = hoja_trabajo3.get_Range("A3", "G3");
                                rango_namedv = hoja_trabajo4.get_Range("A3", "G3");
                                rango_nameclasi = hoja_trabajo5.get_Range("A3", "I3");



                                rango_enc.Font.Name = "Times New Roman";
                                rango_enc.Font.Size = 15;
                                rango_enc.Borders.LineStyle = Excel.XlLineStyle.xlDouble;

                                rango_encbv.Font.Name = "Times New Roman";
                                rango_encbv.Font.Size = 15;
                                rango_encbv.Borders.LineStyle = Excel.XlLineStyle.xlDouble;

                                rango_detv.Font.Name = "Times New Roman";
                                rango_detv.Font.Size = 15;
                                rango_detv.Borders.LineStyle = Excel.XlLineStyle.xlDouble;


                                rango_det.Font.Name = "Times New Roman";
                                rango_det.Font.Size = 15;
                                rango_det.Borders.LineStyle = Excel.XlLineStyle.xlDouble;

                                rango_enclasi.Font.Name = "Times New Roman";
                                rango_enclasi.Font.Size = 15;
                                rango_enclasi.Borders.LineStyle = Excel.XlLineStyle.xlDouble;


                                rango_name.Font.Name = "Times New Roman";
                                rango_name.Font.Color = Color.Blue;
                                rango_name.Font.Size = 10;
                                rango_name.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                                rango_namev.Font.Name = "Times New Roman";
                                rango_namev.Font.Color = Color.Blue;
                                rango_namev.Font.Size = 10;
                                rango_namev.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                                rango_namedv.Font.Name = "Times New Roman";
                                rango_namedv.Font.Color = Color.Blue;
                                rango_namedv.Font.Size = 10;
                                rango_namedv.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;


                                rango_encb.Font.Name = "Times New Roman";
                                rango_encb.Font.Color = Color.Blue;
                                rango_encb.Font.Size = 10;
                                rango_encb.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                                rango_nameclasi.Font.Name = "Times New Roman";
                                rango_nameclasi.Font.Color = Color.Blue;
                                rango_nameclasi.Font.Size = 10;
                                rango_nameclasi.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                                rango_enc.Font.Bold = true;
                                rango_encb.Font.Bold = true;
                                rango_det.Font.Bold = true;
                                rango_name.Font.Bold = true;
                                rango_namev.Font.Bold = true;
                                rango_namedv.Font.Bold = true;
                                rango_nameclasi.Font.Bold = true;

                                hoja_trabajo.Cells[1, 2] = "REPORTE DETALLE_PRECIOS DE OC+";
                                hoja_trabajo2.Cells[1, 2] = "REPORTE ENCABEZADO_PRECIOS DE OC+";
                                hoja_trabajo3.Cells[1, 2] = "REPORTE ENCABEZADO_VOLUMES DE OC+";
                                hoja_trabajo4.Cells[1, 2] = "REPORTE DETALLE_VOLUME DE OC+";
                                hoja_trabajo5.Cells[1, 4] = "REPORTE CLASIFICIONES DE OC+";

                                                                


                                hoja_trabajo.Cells[3, 1] = Convert.ToString(dtalle.Columns[0].ColumnName);
                                hoja_trabajo.Cells[3, 2] = Convert.ToString(dtalle.Columns[1].ColumnName);
                                hoja_trabajo.Cells[3, 3] = Convert.ToString(dtalle.Columns[2].ColumnName);
                                hoja_trabajo.Cells[3, 4] = Convert.ToString(dtalle.Columns[3].ColumnName);
                                hoja_trabajo.Cells[3, 5] = Convert.ToString(dtalle.Columns[4].ColumnName);
                                hoja_trabajo.Cells[3, 6] = Convert.ToString(dtalle.Columns[5].ColumnName);
                                hoja_trabajo.Cells[3, 7] = Convert.ToString(dtalle.Columns[6].ColumnName);


                                hoja_trabajo4.Cells[3, 1] = Convert.ToString(dtallev.Columns[0].ColumnName);
                                hoja_trabajo4.Cells[3, 2] = Convert.ToString(dtallev.Columns[1].ColumnName);
                                hoja_trabajo4.Cells[3, 3] = Convert.ToString(dtallev.Columns[2].ColumnName);
                                hoja_trabajo4.Cells[3, 4] = Convert.ToString(dtallev.Columns[3].ColumnName);
                                hoja_trabajo4.Cells[3, 5] = Convert.ToString(dtallev.Columns[4].ColumnName);
                                hoja_trabajo4.Cells[3, 6] = Convert.ToString(dtallev.Columns[5].ColumnName);
                                hoja_trabajo4.Cells[3, 7] = Convert.ToString(dtallev.Columns[6].ColumnName);


                                hoja_trabajo2.Cells[3, 1] = Convert.ToString(dtencv.Columns[0].ColumnName);
                                hoja_trabajo2.Cells[3, 2] = Convert.ToString(dtencv.Columns[1].ColumnName);
                                hoja_trabajo2.Cells[3, 3] = Convert.ToString(dtencv.Columns[2].ColumnName);
                                hoja_trabajo2.Cells[3, 4] = Convert.ToString(dtencv.Columns[3].ColumnName);

                                hoja_trabajo3.Cells[3, 1] = Convert.ToString(dtenc.Columns[0].ColumnName);
                                hoja_trabajo3.Cells[3, 2] = Convert.ToString(dtenc.Columns[1].ColumnName);
                                hoja_trabajo3.Cells[3, 3] = Convert.ToString(dtenc.Columns[2].ColumnName);
                                hoja_trabajo3.Cells[3, 4] = Convert.ToString(dtenc.Columns[3].ColumnName);



                                hoja_trabajo5.Cells[3, 1] = Convert.ToString(dtclasi.Columns[0].ColumnName);
                                hoja_trabajo5.Cells[3, 2] = Convert.ToString(dtclasi.Columns[1].ColumnName);
                                hoja_trabajo5.Cells[3, 3] = Convert.ToString(dtclasi.Columns[2].ColumnName);
                                hoja_trabajo5.Cells[3, 4] = Convert.ToString(dtclasi.Columns[3].ColumnName);
                                hoja_trabajo5.Cells[3, 5] = Convert.ToString(dtclasi.Columns[4].ColumnName);
                                hoja_trabajo5.Cells[3, 6] = Convert.ToString(dtclasi.Columns[5].ColumnName);
                                hoja_trabajo5.Cells[3, 7] = Convert.ToString(dtclasi.Columns[6].ColumnName);
                                hoja_trabajo5.Cells[3, 8] = Convert.ToString(dtclasi.Columns[7].ColumnName);
                                hoja_trabajo5.Cells[3, 9] = Convert.ToString(dtclasi.Columns[8].ColumnName);


                                //Recorremos el DataGridView rellenando la hoja de trabajo


                                for (int i = 0; i < dtalle.Rows.Count; i++)
                                {

                                    for (int j = 0; j < dtalle.Columns.Count; j++)
                                    {


                                        hoja_trabajo.Cells[i + 4, j + 1] = dtalle.Rows[i][j].ToString();
                                    }
                                }

                                for (int i = 0; i < dtenc.Rows.Count; i++)
                                {

                                    for (int j = 0; j < dtenc.Columns.Count; j++)
                                    {


                                        hoja_trabajo2.Cells[i + 4, j + 1] = dtenc.Rows[i][j].ToString();
                                    }
                                }


                                for (int i = 0; i < dtencv.Rows.Count; i++)
                                {

                                    for (int j = 0; j < dtencv.Columns.Count; j++)
                                    {


                                        hoja_trabajo3.Cells[i + 4, j + 1] = dtencv.Rows[i][j].ToString();
                                    }
                                }

                                for (int i = 0; i < dtallev.Rows.Count; i++)
                                {

                                    for (int j = 0; j < dtallev.Columns.Count; j++)
                                    {


                                        hoja_trabajo4.Cells[i + 4, j + 1] = dtallev.Rows[i][j].ToString();
                                    }
                                }

                                for (int i = 0; i < dtclasi.Rows.Count; i++)
                                {

                                    for (int j = 0; j < dtclasi.Columns.Count; j++)
                                    {


                                        hoja_trabajo5.Cells[i + 4, j + 1] = dtclasi.Rows[i][j].ToString();
                                    }
                                }







                                //aplicacion.Visible = true;

                                if (Directory.Exists(@"C:\CORRECT\Ruta OC+\" + fecha + ""))
                                {

                                    libros_trabajo.SaveAs(@"C:\CORRECT\Ruta OC+\" + fecha + "/" + Ruta + "");

                                    libros_trabajo.Close(true);
                                    aplicacion.Quit();
                                }

                                else
                                {
                                    Directory.CreateDirectory(@"C:\CORRECT\Ruta OC+\" + fecha + "");

                                }


                            }


                        }

                    }
                    catch
                    {
                        MessageBox.Show("*****ERRRORRR****");
                    }

                    this.Close();
                }
                else
                {

                }

        }
    }
}
