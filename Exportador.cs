//ESTA CLASE FUNCIONA ENVIANDO EL DATA TABLE COMO PARAMETRO O
//ENVIANDO EL DATA TABLE,FECHAINICIO REPORTE Y FECHA FIN REPORTE.
//EL NOMBRE DEL REPORTE SE COLOCA EN LA PROPIEDADE NombreReporte de Clase
//El Nombre de La Empresa Si se quiere cambiar se Coloca en la propiedad Empresa.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Sinconizacion_EXactus
{
    class Exportador
    {

        public string Empresa = "DISTRIBUIDORA MORAZAN SA DE CV";
        public string NombreReporte = "";
        int reg;
      

        // Eventos de proceso ----------
        public delegate void ProgressUpdate(int value);
        public event ProgressUpdate OnProgressUpdate;
        public Excel.Application aplicacion;
        private int val;
        

  
                
       

        private void CodigoExcel(DataSet ds, DateTime FechaIni, DateTime FechaFin, int Parametro)
        {
            

            if (ds.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("No hay datos que Mostrar");
            }
            else
            {
               
                Excel.Workbook libros_trabajo;
                aplicacion = new Excel.Application();
               
                Excel.Range rango;
                Excel.Range rango_enc;
                Excel.Range rango_subtitulo;
                Excel.Range rango_cuerpo;

                try
                {
                    libros_trabajo = aplicacion.Workbooks.Add();

                    for (int t = 0; t < ds.Tables.Count; t++)
                    {
                        reg = 0;

                        Excel.Worksheet hoja_trabajo;

                         reg = ds.Tables[t].Columns.Count;

                        int incre;

                        int Columnas, col, Fila;

                        col = ds.Tables[t].Columns.Count / 26;

                        //LE SUMAMOS 3 POR EL ENCABEZADO
                        Fila = ds.Tables[t].Rows.Count + 3;
                        string Letracol = "ABCDEFEHIJKLMNOPQRSTUVWXYZ";
                        string Complementocol;
                        //Determinando la letra que se usara despues de la columna 26


                        if (col > 0)
                        {
                            Columnas = ds.Tables[t].Columns.Count - (26 * col);
                            Complementocol = Letracol.ToString().Substring(col - 1, 1);
                        }
                        else
                        {
                            Columnas = ds.Tables[t].Columns.Count;
                            Complementocol = "";
                        }

                        string ColumnaFinal;

                        incre = Encoding.ASCII.GetBytes("A")[0];

                        ColumnaFinal = Complementocol.ToString() + Convert.ToChar(incre + Columnas - 1).ToString();
                        //+ Fila.ToString()
                        //    MessageBox.Show(ColumnaFinal);
                        int indice = 0;
                        if (t > 0)
                        {
                            indice = t+1;
                            hoja_trabajo = (Excel.Worksheet)libros_trabajo.Worksheets.Add();
                        }
                        else
                        {
                            indice = 1;
                        }

                        //
                        hoja_trabajo = (Excel.Worksheet)libros_trabajo.Worksheets.get_Item(1);
                       
                        hoja_trabajo.Name = ds.Tables[t].TableName;
                        
                        //Colocando Nombre de la Empresa
                        rango = hoja_trabajo.get_Range("A1", ColumnaFinal + "1");
                        rango.Font.Name = "Times New Roman";
                        //rango.Font.Color = Color.Blue;
                        rango.Font.Size = 14;

                        hoja_trabajo.Cells[1, 1] = Empresa;
                        rango.Select();
                        rango.Merge();
                        rango.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;


                        //Nombre del Reporte 
                        rango_subtitulo = hoja_trabajo.get_Range("A2", ColumnaFinal + "2");
                        rango_subtitulo.Font.Name = "Times New Roman";
                        rango_subtitulo.Font.Size = 12;
                        //"DETALLE " + "   DEL " + FechaIni.ToString("dd-MM-yyyy") + "  AL  " + FechaFin.ToString("dd-MM-yyyy") + " ";

                        if (Parametro == 1)
                            hoja_trabajo.Cells[2, 1] = NombreReporte + " EMISION " + DateTime.Now.ToString();
                        else
                            hoja_trabajo.Cells[2, 1] = NombreReporte + " DEL " + FechaIni.ToShortDateString() + " AL " + FechaFin.ToShortDateString() + " EMISION " + DateTime.Now.ToString();

                        rango_subtitulo.Select();
                        rango_subtitulo.Merge();
                        rango_subtitulo.Font.Bold = true;
                        rango_subtitulo.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                        //ENCABEZDO DE COLUMNAS
                        rango_enc = hoja_trabajo.get_Range("A3", ColumnaFinal + "3");
                        rango_enc.Font.Name = "Times New Roman";
                        rango_enc.Font.Size = 9;
                        rango_enc.Borders.LineStyle = Excel.XlLineStyle.xlDouble;
                        rango_enc.Font.Bold = true;
                        rango.Font.Bold = true;

                        rango_cuerpo = hoja_trabajo.get_Range("A4", ColumnaFinal + Fila.ToString());
                        rango_cuerpo.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        rango_cuerpo.Font.Size = 9;

                        //hoja_trabajo.Cells[3, 1] = Convert.ToString(dt.Columns[0].ColumnName);

                        //Horientacion de Papel si son muchas columnas
                        if (ds.Tables[t].Columns.Count >= 10)
                            hoja_trabajo.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
                        else
                            hoja_trabajo.PageSetup.Orientation = Excel.XlPageOrientation.xlPortrait;

                        //Para los títulos de las columnas, encabezado
                        for (int c = 0; c < ds.Tables[t].Columns.Count; c++)
                        {
                            hoja_trabajo.Cells[3, c + 1] = String.Format("{0}", ds.Tables[t].Columns[c].Caption);
                        }



                        for (int i = 0; i < ds.Tables[t].Rows.Count; i++)
                        {
                            // envia la informacion al metodo de proceso
                            changevalue(i, reg);

                            //Cargando registros en hoja de excel
                            for (int j = 0; j < ds.Tables[t].Columns.Count; j++)
                            {

                                hoja_trabajo.Cells[i + 4, j + 1] = ds.Tables[t].Rows[i][j].ToString().Trim();

                            }

                        }

                      
                        //Ajusta Ancho de Columas
                        hoja_trabajo.Columns.AutoFit();
                        hoja_trabajo.Cells[ds.Tables[t].Rows.Count + 4, 1] = "Registros Impresos: " + ds.Tables[t].Rows.Count.ToString();


                    }
                    aplicacion.Visible = true;
                }

                catch (SystemException exect)
                {
                    //    MessageBoxButtons bt1 = MessageBoxButtons.OK;
                    //    DialogResult result = MessageBox.Show(exect.ToString(), "!!!!ERROR!!!!", bt1, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                    MessageBox.Show("Error: " + exect.Message);

                }
                    finally
                    {

                        //mWSheet1 = null;

                        //mWSheet2 = null;

                        //mWorkBook = null;

                        aplicacion.Quit();

                        aplicacion = null;

                        GC.WaitForPendingFinalizers();

                        GC.Collect();

                        GC.WaitForPendingFinalizers();

                        GC.Collect();

                    }
                }
            
        }
    
        public void aExcel(DataSet dt)
        {
            this.CodigoExcel(dt, DateTime.Now, DateTime.Now,1);
        }

        public void aExcel(DataSet dt, DateTime FechaIni, DateTime FechaFin)
        {

            this.CodigoExcel(dt, FechaIni, FechaFin,2);

        }

        // envia valores sobre el proceso
        public int changevalue(int p,int reg)
        {
            for (int j = 0 ; j < reg; j++)
            {
                val = p;
               
                //from here i need to preport the backgroundworker progress
                //eg; backgroundworker1.reportProgress(j);

                if (OnProgressUpdate != null)
                {
                    OnProgressUpdate(p);
                }

            }
            return val;
            
        }

    }
}
