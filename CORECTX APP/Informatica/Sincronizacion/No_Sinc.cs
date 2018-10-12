using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;
using System.Reflection;

namespace Sinconizacion_EXactus
{
    
    public partial class No_Sinc : Form
    {
        public No_Sinc()
        {
            InitializeComponent();
        }
       // conexion conet = new conexion();
        conexionXML con = new conexionXML();
        DataTable ven_name = new DataTable();
        private ListViewColumnSorter lvwColumnSorter;

        private void Form4_Load(object sender, EventArgs e)
        {
            ven_name.Clear();
            
            
        listView1.View = View.Details; //Detalles
            listView1.GridLines = true; //Lineas
            listView1.FullRowSelect = true;


            listView1.Clear();
            listView1.Columns.Add("Ruta", 50, HorizontalAlignment.Left);
            listView1.Columns.Add("Nombre", 120, HorizontalAlignment.Left);


            show_nosinc( Join_tablas(Sincronizacion.No_sincronizadas,ven_name));


            con.conectar("EX");
            SqlCommand cm1 = new SqlCommand("SELECT [HANDHELD] as RUTA,VEN.NOMBRE FROM [EXACTUS].[ERPADMIN].[RUTA_ASIGNADA_RT] RT LEFT JOIN [EXACTUS].["+Login.empresa+"].[VENDEDOR] as VEN on RT.AGENTE = VEN.VENDEDOR  where RT.COMPANIA = '"+Login.empresa+"' and VEN.ACTIVO = 'S'", con.conex);
            SqlDataAdapter da = new SqlDataAdapter(cm1);
            da.Fill(ven_name);

            con.Desconectar("EX");


            show_nosinc(Join_tablas(Sincronizacion.No_sincronizadas, ven_name));

            //if (Sincronizacion.time == "Tarde")
            //{
            //    try
            //    {
            //        listView1.Clear();
            //        listView1.Columns.Add("Ruta", 50, HorizontalAlignment.Left);
            //        listView1.Columns.Add("Nombre", 120, HorizontalAlignment.Left);

            //        con.conectar("EX");
            //        SqlCommand cm1 = new SqlCommand("SELECT A. HANDHELD,B.NOMBRE FROM  ERPADMIN.RUTA_ASIGNADA_RT A INNER JOIN ERPADMIN.AGENTE_RT B ON A.AGENTE = B.AGENTE WHERE A.COMPANIA = '" + Login.empresa + "' AND A. HANDHELD NOT IN (SELECT PDA FROM [EXACTUS].[ERPADMIN].[EMM_SYNCINFO] WHERE  CONDUIT = '" + Sincronizacion.conduit + "' AND  SYNC_START BETWEEN '" + Sincronizacion.fecha_sinc + " 12:00:00' AND '" + Sincronizacion.fecha_sinc + " 23:59:00')AND B.NOMBRE <> 'OFFLINE'  GROUP BY A.HANDHELD,B.NOMBRE", con.conex);
            //        SqlDataReader dr1 = cm1.ExecuteReader();
            //        while (dr1.Read())
            //        {
            //            ListViewItem lvItem = new ListViewItem();
            //            lvItem.SubItems[0].Text = dr1[0].ToString();
            //            lvItem.SubItems.Add(dr1[1].ToString());

            //            listView1.Items.Add(lvItem);
            //        }
            //        dr1.Close();
            //        con.Desconectar("EX");
            //    }
            //    catch
            //    {
            //        MessageBox.Show("Error Carga de Rutas");
            //        con.Desconectar("EX");
            //    }
            //}
            //else

            //    if (Sincronizacion.time == "Mañana")
            //    {

            //        try
            //        {
            //            listView1.Clear();
            //            listView1.Columns.Add("Ruta", 50, HorizontalAlignment.Left);
            //            listView1.Columns.Add("Nombre", 120, HorizontalAlignment.Left);

            //            con.conectar("EX");
            //            SqlCommand cm1 = new SqlCommand("SELECT A. HANDHELD,B.NOMBRE FROM  ERPADMIN.RUTA_ASIGNADA_RT A INNER JOIN ERPADMIN.AGENTE_RT B ON A.AGENTE = B.AGENTE WHERE A.COMPANIA = '"+Login.empresa+"' AND A. HANDHELD NOT IN (SELECT PDA FROM [EXACTUS].[ERPADMIN].[EMM_SYNCINFO] WHERE A.COMPANIA = '" + Login.empresa + "' AND  CONDUIT = '" + Sincronizacion.conduit + "' AND PDA LIKE 'P%' AND  SYNC_START BETWEEN '" + Sincronizacion.fecha_sinc + " 1:00:00' AND '" + Sincronizacion.fecha_sinc + " 12:00:00')AND B.NOMBRE <> 'OFFLINE' GROUP BY A.HANDHELD,B.NOMBRE", con.conex);
            //            SqlDataReader dr1 = cm1.ExecuteReader();
            //            while (dr1.Read())
            //            {
            //                ListViewItem lvItem = new ListViewItem();
            //                lvItem.SubItems[0].Text = dr1[0].ToString();
            //                lvItem.SubItems.Add(dr1[1].ToString());

            //                listView1.Items.Add(lvItem);
            //            }
            //            dr1.Close();
            //            con.Desconectar("EX");
            //        }
            //        catch
            //        {
            //            MessageBox.Show("Error Carga de Rutas");
            //            con.Desconectar("EX");
            //        }
            //    }


            //    else if (Sincronizacion.time == "Todos")
            //    {
            //        //try
            //        //{
            //            listView1.Clear();
            //            listView1.Columns.Add("Ruta", 50, HorizontalAlignment.Left);
            //            listView1.Columns.Add("Nombre", 120, HorizontalAlignment.Left);

            //            con.conectar("EX");
            //            SqlCommand cm1 = new SqlCommand("SELECT A. HANDHELD,B.NOMBRE FROM  ERPADMIN.RUTA_ASIGNADA_RT A INNER JOIN ERPADMIN.AGENTE_RT B ON A.AGENTE = B.AGENTE WHERE  A.COMPANIA = '" + Login.empresa+"' AND A. HANDHELD NOT IN (SELECT PDA FROM [EXACTUS].[ERPADMIN].[EMM_SYNCINFO] WHERE A.COMPANIA = '" + Login.empresa + "' AND  CONDUIT = '" + Sincronizacion.conduit + "' AND PDA LIKE 'P%' AND  SYNC_START BETWEEN '" + Sincronizacion.fecha_sinc + " 1:00:00' AND '" + Sincronizacion.fecha_sinc + " 23:59:59')AND B.NOMBRE <> 'OFFLINE' GROUP BY A.HANDHELD,B.NOMBRE", con.conex);
            //            SqlDataReader dr1 = cm1.ExecuteReader();
            //            while (dr1.Read())
            //            {
            //                ListViewItem lvItem = new ListViewItem();
            //                lvItem.SubItems[0].Text = dr1[0].ToString();
            //                lvItem.SubItems.Add(dr1[1].ToString());

            //                listView1.Items.Add(lvItem);
            //            }
            //            dr1.Close();
            //            con.Desconectar("EX");
            //        //}
            //        //catch
            //        //{
            //        //    MessageBox.Show("Error Carga de Rutas");
            //        //    con.Desconectar("EX");
            //        //}


            //}
        }

        private void show_nosinc(DataTable dt)
            {

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                ListViewItem listitem = new ListViewItem();
                listitem.SubItems[0].Text = dr[0].ToString();
                listitem.SubItems.Add(dr["NOMBRE"].ToString());
            
                listView1.Items.Add(listitem);
            }
        }


       public class ListViewItemComparer : IComparer
        {
            private int col;
            public ListViewItemComparer()
            {
                col = 0;
            }
            public ListViewItemComparer(int column)
            {
                col = column;
            }
            public int Compare(object x, object y)
            {
                int returnVal = -1;
                returnVal = String.Compare(((ListViewItem)x).SubItems[col].Text,
                ((ListViewItem)y).SubItems[col].Text);
                return returnVal;
            }
        }
        private ColumnHeader SortingColumn = null;
        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            ColumnHeader new_sorting_column = listView1.Columns[e.Column];
            ColumnHeader new_sort = listView1.Columns[e.Column];
            // Figure out the new sorting order.
            System.Windows.Forms.SortOrder sort_order;

            if (SortingColumn == null)
            {
                // New column. Sort ascending.
                sort_order = System.Windows.Forms.SortOrder.Ascending;
            }
            else
            {
                // See if this is the same column.
                if (new_sorting_column == SortingColumn)
                {
                    // Same column. Switch the sort order.
                    if (SortingColumn.Text.StartsWith("> "))
                    {
                        sort_order = System.Windows.Forms.SortOrder.Descending;
                    }
                    else
                    {
                        sort_order = System.Windows.Forms.SortOrder.Ascending;
                    }
                }
                else
                {
                    // New column. Sort ascending.
                    sort_order = System.Windows.Forms.SortOrder.Ascending;
                }

                // Remove the old sort indicator.
                SortingColumn.Text = SortingColumn.Text.Substring(2);
            }

            // Display the new sort order.
            SortingColumn = new_sorting_column;
            if (sort_order == System.Windows.Forms.SortOrder.Ascending)
            {
                SortingColumn.Text = "> " + SortingColumn.Text;
            }
            else
            {
                SortingColumn.Text = "< " + SortingColumn.Text;
            }

            // Create a comparer.
           listView1.ListViewItemSorter =
                new ListViewItemComparer(e.Column);

            // Sort.
            listView1.Sort();

        }

        private DataTable Join_tablas(DataTable dt1, DataTable dt2)
        {



            var result = from x in dt1.AsEnumerable()
                         join y in dt2.AsEnumerable() on (string)x["RUTA"] equals (string)y["RUTA"]
                         select new
                         {
                             RUTA = (string)x["RUTA"],
                             NOMBRE = (string)y["NOMBRE"]
                         };
            return LINQToDataTable(result);
            
        }



        public DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();

            // column names 
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others 
              //  will follow
                 if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }



    }
}
