using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace Sinconizacion_EXactus.CORECTX_APP.BODEGA
{
    public partial class join_tablas : Form
    {
        public join_tablas()
        {
            InitializeComponent();
        }
        conexionXML con = new conexionXML();
        DataTable tabla1 = new DataTable();
        DataTable tabla2 = new DataTable();
        DataTable tabla3 = new DataTable();
        DataTable Bodega = new DataTable();
        ConvertDT CONVERTDT = new ConvertDT();
        private void join_tablas_Load(object sender, EventArgs e)
        {
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("SELECT ART.[ARTICULO],ART.[DESCRIPCION],FAM.DESCRIPCION as 'Familia'  ,CTN.DESCRIPCION as 'Categoria'  FROM [EXACTUS].[dismo].[ARTICULO] as ART LEFT JOIN [EXACTUS].[dismo].[ARTICULO_CUENTA] as CTN on ART.ARTICULO_CUENTA = CTN.ARTICULO_CUENTA LEFT JOIN (SELECT [CLASIFICACION] ,[DESCRIPCION] FROM[EXACTUS].[dismo].[CLASIFICACION]where AGRUPACION = '1') FAM on ART.CLASIFICACION_1 = FAM.CLASIFICACION LEFT JOIN (SELECT [CLASIFICACION] ,[DESCRIPCION] FROM[EXACTUS].[dismo].[CLASIFICACION]where AGRUPACION = '2') LINEA  on ART.CLASIFICACION_2 = LINEA.CLASIFICACION where ART.TIPO = 'T'  and ACTIVO = 'S'  order by ART.ARTICULO", con.conex);
            SqlDataAdapter principal = new SqlDataAdapter(cmd);
            principal.Fill(tabla1);

            con.Desconectar("EX");

           Bodega.Columns.Add("BODEGA", typeof(string));

            Bodega.Rows.Add("B100");
            Bodega.Rows.Add("B200");

            inventario();

        }
        private void inventario()
        {
            


            for (int c = 0; c < Bodega.Rows.Count; c++)
            {

                tabla2.Clear();
                DataRow row = Bodega.Rows[c];

                string bod = Convert.ToString(row["BODEGA"]);

                con.conectar("EX");
                SqlCommand cmd2 = new SqlCommand("SELECT  SS.[Articulo],SUM(SS.[CantidadDisponible]) as 'Santa Ana "+bod+"' FROM [EXACTUS].[dismo].[SoftlandBI_CI_Existencias] as SS   where Bodega = '"+bod+"' and ss.tipo = 'Teminado' group by SS.Articulo,ss.DescripcionArticulo", con.conex);
                SqlDataAdapter inventraio = new SqlDataAdapter(cmd2);
                inventraio.Fill(tabla2);
                con.Desconectar("EX");

                join(tabla1, tabla2);

            }


            dataGridView1.DataSource = tabla3;
        }

        private void join(DataTable tabla1, DataTable tabla2)
        {
            
            if (tabla3.Columns.Count == 0)
            {
                tabla3 = tabla1.Clone();
            }
            else
            {
               
            }
            
            var dt2Columns = tabla2.Columns.OfType<DataColumn>().Select(dc =>
            new DataColumn(dc.ColumnName, dc.DataType, dc.Expression, dc.ColumnMapping));
            var dt2FinalColumns = from dc in dt2Columns.AsEnumerable()
                                  where tabla3.Columns.Contains(dc.ColumnName) == false
                                  select dc;

            tabla3.Columns.AddRange(dt2FinalColumns.ToArray());




            var results = from table1 in tabla1.AsEnumerable()
                          join table2 in tabla2.AsEnumerable() on (string)Convert.ToString(table1["ARTICULO"]) equals (string)Convert.ToString(table2["Articulo"])
                          //  where Convert.ToString(table2["ESTADO"]) == var_estado
                        //  select table1.ItemArray.Concat(table2.ItemArray).ToArray();
            select table1.ItemArray.Concat(table2.ItemArray.Where(r2 => table1.ItemArray.Contains(r2) == false)).ToArray();


            //Add row data to dtblResult
            foreach (object[] values in results)
            {
                tabla3.Rows.Add(values);
            }
            // tabla3 = CONVERTDT.ConvertToDataTable(results);
           

        }
    }
}
