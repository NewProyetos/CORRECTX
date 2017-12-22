using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Sinconizacion_EXactus.CORECTX_APP.VENTAS.FACTURACION
{
    public partial class Ingreso_Facturas : Form
    {
        public Ingreso_Facturas()
        {
            InitializeComponent();
        }
        String cod_art;
        String Nom_art;
        String NIVEL_PRECIO_CLIE;
        int LISTA_PRECIO_NUM;
        string CLIENTE_CLASE_DOC;
        decimal precioart;
        string prefijo;
        double valor_sin_iva;
        double total_impuesto;
        double valor_con_iva;
        double cantidad_total;
        double cesct;
        string credito_clie;
        string clase_doc;

        conexionXML con = new conexionXML();
        DataTable articulos = new DataTable();
        DataTable precioslist = new DataTable();
        DataTable clientfiltro = new DataTable();
        DataTable Detalles_fac = new DataTable();
        DataTable Detalles_fac_up = new DataTable();
        DataTable Encabezado_fac = new DataTable();
        DataTable tabupdate = new DataTable();
        int idx;
        string idlinea;
        String Correlativo,articulo_linea;
        int impuesto,subtotal,total, total_linea,linea_numero;
        ConvertDT CONVERDT = new ConvertDT();
       public static DataTable Clientes = new DataTable();

        private void toolStrip1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Articulos art = new Articulos();
            DialogResult res = art.ShowDialog();

            if (res == DialogResult.OK)
            {
                cod_art = art.Cod_art;
                Nom_art = art.Nom_art;


                toolStripTextBox1.Text = cod_art;
              //  toolStripLabel2.Text = Nom_art;
            }
        }

        private void Ingreso_Facturas_Load(object sender, EventArgs e)
        {
            textBox3.Focus();
            toolStripButton3.Enabled = false;
            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            //dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            
            toolStripButton1.Enabled = false;
            toolStripButton2.Enabled = false;
            carga_articulo();
            carga_clientes();
            columnas_tables();
            textBox1.AutoCompleteCustomSource = Autocompletecodclie();
            textBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;

            textBox2.AutoCompleteCustomSource = Autocompletenomclie();
            textBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;

            textBox3.AutoCompleteCustomSource = Autocompletelefono();
            textBox3.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBox3.AutoCompleteSource = AutoCompleteSource.CustomSource;

            if (Facturacion.update == "SI")
            {
                load_update(Facturacion.pedido_update);
            }

         
                
        }

        private void columnas_tables()
        {
            Detalles_fac.Columns.Add("NUMERO FACTURA", typeof(string));
            Detalles_fac.Columns.Add("NUMERO LINEA", typeof(string));
            Detalles_fac.Columns.Add("TIPO_DOC", typeof(string));
            Detalles_fac.Columns.Add("COD ARTICULO", typeof(string));
            Detalles_fac.Columns.Add("ARTICULO", typeof(string));
            Detalles_fac.Columns.Add("CANTIDAD", typeof(double));
            Detalles_fac.Columns.Add("PRECIO UNITARIO", typeof(string));
            Detalles_fac.Columns.Add("IVA", typeof(double));
            Detalles_fac.Columns.Add("CESC", typeof(double));           
            Detalles_fac.Columns.Add("MONTO_DESC_ART", typeof(string));
            Detalles_fac.Columns.Add("MONTO_DESC_FAM", typeof(string));
            Detalles_fac.Columns.Add("SUBTOTAL", typeof(double));
            Detalles_fac.Columns.Add("TIPO_UNIDA", typeof(string));
            Detalles_fac.Columns.Add("LISTA PRECIO", typeof(string));
            Detalles_fac.Columns.Add("COSTO_ART", typeof(string));
            Detalles_fac.Columns.Add("FECHA_CREA", typeof(string));
            Detalles_fac.Columns.Add("USUARIO", typeof(string));
            Detalles_fac.Columns.Add("SERIE_DOC", typeof(string));
            Detalles_fac.Columns.Add("NUM_DOC", typeof(string));
            Detalles_fac.Columns.Add("ART_BON", typeof(string));
            Detalles_fac.Columns.Add("LINEA_ART_BON", typeof(string));
            

        }

        private void carga_articulo()

        {
            con.conectar("EX");
            SqlCommand cm2 = new SqlCommand("SELECT CAST(ART.[U_ID_FAC] as int) as  ORDEN,ART.[ARTICULO] as CODIGO,ART.[DESCRIPCION] ,ART.CLASIFICACION_1 +' '+ CLAS.DESCRIPCION as FAMILIA FROM [EXACTUS].[dismo].[ARTICULO] ART  LEFT JOIN (SELECT [CLASIFICACION] ,[DESCRIPCION]   FROM [EXACTUS].[dismo].[CLASIFICACION]  where AGRUPACION = '1') as CLAS  on ART.CLASIFICACION_1 = CLAS.CLASIFICACION where ART.CLASIFICACION_1 = '73'", con.conex);
            SqlDataAdapter da2 = new SqlDataAdapter(cm2);
            da2.Fill(articulos);
            con.Desconectar("EX");
        }

        private void carga_clientes()
        {
            Clientes.Clear();
            con.conectar("EX");
            SqlCommand cm2 = new SqlCommand("SELECT CLIE.[CLIENTE],CLIE.[NOMBRE],CLIE.[CONDICION_PAGO],CLIE.[ALIAS],CLIE.[DIRECCION],CLIE.[TELEFONO1],CLIE.[TELEFONO2],CLIE.[SALDO],CLIE.[NIVEL_PRECIO],CLIE.[COBRADOR],CLIE.[RUTA],LIST.LISTA ,CLIE.CLASE_DOCUMENTO FROM [EXACTUS].[dismo].[CLIENTE] as CLIE LEFT JOIN [EXACTUS].[ERPADMIN].[NIVEL_LISTA]as LIST on CLIE.NIVEL_PRECIO = LIST.NIVEL_PRECIO where CLIE.vendedor = '" + Facturacion.Ruta+"'", con.conex);
            SqlDataAdapter da2 = new SqlDataAdapter(cm2);
            da2.Fill(Clientes);
            con.Desconectar("EX");


        }


        private void toolStripTextBox1_DoubleClick(object sender, EventArgs e)
        {
            Articulos art = new Articulos();
            DialogResult res = art.ShowDialog();

            if (res == DialogResult.OK)
            {
                cod_art = art.Cod_art;
                Nom_art = art.Nom_art;


                toolStripLabel2.Text = art.Nom_art;             
            }
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
                return;
            }
           
        }

        private void toolStripTextBox1_Leave(object sender, EventArgs e)
        {
            string artic = toolStripTextBox1.Text;
            toolStripLabel2.Text = "";


            if (artic != "" || artic != string.Empty ||  artic != null)
            {
                articulo_nombre("ORDEN", Convert.ToInt32(artic));

            }
            else
            {
               
            }
        }

        public static AutoCompleteStringCollection Autocompletecodclie()
        {


            AutoCompleteStringCollection coleccion = new AutoCompleteStringCollection();
            //recorrer y cargar los items para el autocompletado
            foreach (DataRow row in Clientes.Rows)
            {
                coleccion.Add(Convert.ToString(row["CLIENTE"]));
            }

            return coleccion;
        }

        public static AutoCompleteStringCollection Autocompletenomclie()
        {

            AutoCompleteStringCollection coleccion = new AutoCompleteStringCollection();
            //recorrer y cargar los items para el autocompletado
            foreach (DataRow row in Clientes.Rows)
            {
                coleccion.Add(Convert.ToString(row["NOMBRE"]));
            }

            return coleccion;
        }

        public static AutoCompleteStringCollection Autocompletelefono()
        {

            AutoCompleteStringCollection coleccion = new AutoCompleteStringCollection();
            //recorrer y cargar los items para el autocompletado
            foreach (DataRow row in Clientes.Rows)
            {
                coleccion.Add(Convert.ToString(row["TELEFONO1"]));
            }

            return coleccion;
        }

        private void Nombre_cliente(string parametro,string dato)
        {
            var results = from myRow in Clientes.AsEnumerable()
                          where myRow.Field<string>(parametro) == dato

                          select new
                          {
                              Nombre = myRow.Field<string>("NOMBRE")
                          };

            foreach (var rs1 in results)
            {
               textBox2.Text = rs1.Nombre;
            }
        }
        private void Codigo_cliente(string parametro, string dato)
        {
            var results = from myRow in Clientes.AsEnumerable()
                          where myRow.Field<string>(parametro) == dato

                          select new
                          {
                              Nombre = myRow.Field<string>("CLIENTE")
                          };

            foreach (var rs1 in results)
            {
               textBox1.Text = rs1.Nombre;
            }
        }

        private void Telefono_cliente(string parametro, string dato)
        {
            var results = from myRow in Clientes.AsEnumerable()
                          where myRow.Field<string>(parametro) == dato

                          select new
                          {
                              Nombre = myRow.Field<string>("TELEFONO1")
                          };

            foreach (var rs1 in results)
            {
                textBox3.Text = rs1.Nombre;
            }
        }

        private void Direccion_cliente(string parametro, string dato)
        {
            var results = from myRow in Clientes.AsEnumerable()
                          where myRow.Field<string>(parametro) == dato

                          select new
                          {
                              Nombre = myRow.Field<string>("DIRECCION")
                          };

            foreach (var rs1 in results)
            {

                richTextBox1.Text = rs1.Nombre;
            }
        }

        private void Credito(string parametro, string dato)
        {
            var results = from myRow in Clientes.AsEnumerable()
                          where myRow.Field<string>(parametro) == dato

                          select new
                          {
                              Nombre = myRow.Field<string>("CONDICION_PAGO")
                          };

            foreach (var rs1 in results)
            {
                if (rs1.Nombre == "01")
                {
                    credito_clie = rs1.Nombre;
                    radioButton1.Checked = true;
                    radioButton2.Enabled = false;
                }
                else
                {
                    credito_clie = rs1.Nombre;
                    radioButton2.Checked = true;
                    radioButton2.Enabled = true;

                }
            }
        }

        private void precio_num(string parametro, string dato)
        {
            var results = from myRow in Clientes.AsEnumerable()
                          where myRow.Field<string>(parametro) == dato

                          select new
                          {
                              Nombre = myRow.Field<int>("LISTA")
                          };

            foreach (var rs1 in results)
            {
                LISTA_PRECIO_NUM = rs1.Nombre;
                
               
            }
        }


        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter))
            {
                if (textBox3.Text != string.Empty || textBox3.Text != "")
                {
                    toolStripTextBox1.Focus();
                }
            }
        }

        private void toolStripTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void toolStripTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter))
            {
                if (toolStripTextBox2.Text != string.Empty || toolStripTextBox2.Text != "")
                {
                    if (toolStripLabel2.Text != "" || toolStripLabel2.Text != ".")
                    {
                        toolStripButton2.Enabled = true;
                        toolStripButton2_Click(null, null);
                    }
                    else
                    {
                        toolStripButton2.Enabled = false;
                        toolStripTextBox1.Focus();
                    }
                }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Ingreso_detalle();
            Calculos();
                
        }

        private void Ingreso_detalle()

        {
            string fecha = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            decimal cantidad;
            decimal subtotal;
            decimal impuesto = 0.13m;
            decimal cesc = 0.05m;
            decimal total_impuesto;
            decimal total_cesc;
            decimal precio_art;
           

            precioart = precioarticulo(articulo_linea);
            cantidad = Convert.ToDecimal(toolStripTextBox2.Text);
            subtotal = cantidad * precioart;
            total_impuesto = subtotal * impuesto;
            total_cesc = subtotal * cesc;
            string articulo = toolStripLabel2.Text;


            if (CLIENTE_CLASE_DOC == "N")
            {
                prefijo = Facturacion.prefijofac;

            }
            else if (CLIENTE_CLASE_DOC == "C")
            {
                prefijo = Facturacion.prefijoccf;
            }
            if (Detalles_fac.Rows.Count > 0)
            {
                linea_numero = Detalles_fac.Rows.Count + 1;

            }
            else
            {
                linea_numero = 1;
            }


            Detalles_fac.Rows.Add(Facturacion.Correlativo_fac, linea_numero, CLIENTE_CLASE_DOC, articulo_linea,articulo, cantidad, precioart, total_impuesto,total_cesc,"0.00", "0.00", subtotal, "1", NIVEL_PRECIO_CLIE, "0.00", fecha, Login.usuario.ToUpper(), prefijo, Facturacion.Correlativo_fac, null, null);


            dataGridView1.DataSource = Detalles_fac;
            dataGridView1.Refresh();


        }

        private void toolStripTextBox2_Leave(object sender, EventArgs e)
        {
          //  toolStripTextBox2.Text = "";
        }

        private void articulo_nombre(string parametro, int dato)
        {
            var results = from myRow in articulos.AsEnumerable()
                          where myRow.Field<int>(parametro) == dato

                          select new
                          {
                              Nombre = myRow.Field<string>("DESCRIPCION"),
                              codigo = myRow.Field<string>("CODIGO"),
                          };

            foreach (var rs1 in results)
            {

                this.toolStripLabel2.Text = rs1.Nombre;
                articulo_linea = rs1.codigo;

            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty || textBox1.Text != "")
            {
                Nombre_cliente("CLIENTE",textBox1.Text);
                Telefono_cliente("CLIENTE",textBox1.Text);
                Direccion_cliente("CLIENTE", textBox1.Text);
                Credito("CLIENTE", textBox1.Text);
                precio_num("CLIENTE", textBox1.Text);
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text != string.Empty || textBox2.Text != "")
            {
                Codigo_cliente("NOMBRE",textBox2.Text);
                Telefono_cliente("NOMBRE",textBox2.Text);
                Direccion_cliente("NOMBRE", textBox2.Text);
                Credito("NOMBRE", textBox2.Text);
                precio_num("NOMBRE", textBox2.Text);
            }

        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            toolStripTextBox1.Text = string.Empty;
            toolStripTextBox2.Text = string.Empty;
            toolStripLabel2.Text = "";
            toolStripButton2.Enabled = false;

            toolStripTextBox1.Focus();
            groupBox1.Enabled = false;

            if (dataGridView1.ColumnCount == 21)
            {
               // dataGridView1.Columns.Remove("MONTO_IMP2");
                dataGridView1.Columns.Remove("TIPO_DOC");
                dataGridView1.Columns.Remove("MONTO_DESC_ART");
                dataGridView1.Columns.Remove("MONTO_DESC_FAM");
                dataGridView1.Columns.Remove("TIPO_UNIDA");
                dataGridView1.Columns.Remove("COSTO_ART");
                dataGridView1.Columns.Remove("FECHA_CREA");
                dataGridView1.Columns.Remove("USUARIO");
                dataGridView1.Columns.Remove("SERIE_DOC");
                dataGridView1.Columns.Remove("NUM_DOC");
                dataGridView1.Columns.Remove("ART_BON");
                dataGridView1.Columns.Remove("LINEA_ART_BON");
            }
            if (dataGridView1.RowCount >= 1)
            {
                toolStripButton3.Enabled = true;
            }
            else
            {
                toolStripButton3.Enabled = false;
            }



        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            idx = dataGridView1.CurrentRow.Index;

            idlinea = Convert.ToString(dataGridView1.Rows[idx].Cells[1].Value);

            toolStripButton1.Enabled = true;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            for (int i = Detalles_fac.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = Detalles_fac.Rows[i];
               string lord = Convert.ToString(dr["NUMERO LINEA"]);

                if (lord == idlinea)
                {
                    
                    Detalles_fac.Rows.Remove(dr);
                }
            }

            dataGridView1.DataSource = Detalles_fac;
            dataGridView1.Refresh();

            if (dataGridView1.RowCount < 1)
            {
                toolStripButton1.Enabled = false;
                groupBox1.Enabled = true;
                label9.Text = "0.00";
                label10.Text = "0.00";
                label11.Text = "0.00";
                label13.Text = "0";
                label14.Text = "0.00";

            }

            else
            {
                Calculos();
            }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (textBox3.Text != string.Empty || textBox3.Text != "")
            {
                Codigo_cliente("TELEFONO1", textBox3.Text);
                Nombre_cliente("TELEFONO1", textBox3.Text);
                Direccion_cliente("TELEFONO1", textBox3.Text);
                Credito("TELEFONO1", textBox3.Text);
                cliente_seleccion();
                Precios(NIVEL_PRECIO_CLIE);
                precio_num("TELEFONO1", textBox3.Text);
               
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (Facturacion.update == "SI")
            {
                update_ped(Facturacion.pedido_update); 
            }
            else
            {
                Insertenc_pedido();
            }
            
        }

        private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)

        {
           //


            if (e.KeyCode.Equals(Keys.Enter))
            {
                toolStripTextBox1_Leave(null, null);

                if (toolStripLabel2.Text == "" || toolStripLabel2.Text == ".")
                {
                    MessageBox.Show("No se encontro articulo");
                    
                }
                else
                {
                    toolStripTextBox2.Focus();
                }
            }
        }

        private void cliente_seleccion()
        {
            clientfiltro.Clear();
            var results = from table1 in Clientes.AsEnumerable()
                          where Convert.ToString(table1["CLIENTE"]) == textBox1.Text
                          orderby (table1["CLIENTE"]) descending

                          select new
                          {
                              CLIENTE = (string)Convert.ToString(table1["CLIENTE"]),
                              NOMBRE = (string)Convert.ToString(table1["NOMBRE"]),
                              ALIAS = (string)Convert.ToString(table1["ALIAS"]),
                              DIRECCION = (string)Convert.ToString(table1["DIRECCION"]),
                              TELEFONO1 = (string)Convert.ToString(table1["TELEFONO1"]),
                              TELEFONO2 = (string)Convert.ToString(table1["TELEFONO2"]),
                              SALDO = (string)Convert.ToString(table1["SALDO"]),
                              NIVEL_PRECIO = (string)Convert.ToString(table1["NIVEL_PRECIO"]),
                              COBRADOR = (string)Convert.ToString(table1["COBRADOR"]),
                              RUTA = (string)Convert.ToString(table1["RUTA"]),
                              LISTA = (string)Convert.ToString(table1["LISTA"]),
                              CLASE_DOCUMENTO = (string)Convert.ToString(table1["CLASE_DOCUMENTO"])
                          };

            //string[] nivelprecio = results.Select(n => n.NIVEL_PRECIO).ToArray(); 

            //foreach (string nivel_precio in nivelprecio)
            //{
            //    NIVEL_PRECIO_CLIE = nivel_precio;

            //}

            clientfiltro = CONVERDT.ConvertToDataTable(results);

            DataRow row = clientfiltro.Rows[0];

            NIVEL_PRECIO_CLIE = Convert.ToString(row["NIVEL_PRECIO"]);
            CLIENTE_CLASE_DOC = Convert.ToString(row["CLASE_DOCUMENTO"]);



        }

        
        #region artuculos

        private decimal precioarticulo(string articulo)
        {
            var results = from myRow in precioslist.AsEnumerable()
                          where myRow.Field<string>("ARTICULO") == articulo

                          select new
                          {
                              Nombre = myRow.Field<decimal>("PRECIO")
                          };

            foreach (var rs1 in results)
            {

               precioart = Convert.ToDecimal(rs1.Nombre);
            }
            return precioart;
        }
        private void Precios(string lista)
        {

            con.conectar("EX");
            SqlCommand cm2 = new SqlCommand("SELECT ART.ARTICULO,PRECIO FROM [EXACTUS].[dismo].[VERSION_NIVEL]as NIVEL  LEFT JOIN [EXACTUS].[dismo].[ARTICULO_PRECIO] as ART   on NIVEL.NIVEL_PRECIO = ART.NIVEL_PRECIO and NIVEL.VERSION = ART.VERSION  where NIVEL.NIVEL_PRECIO = '" + lista + "'", con.conex);
            SqlDataAdapter da2 = new SqlDataAdapter(cm2);
            da2.Fill(precioslist);
            con.Desconectar("EX");






        }


        #endregion articulos

        #region InsertPedido

        private void Insertenc_pedido()
        {
            string NUM_DOC_SIS = Facturacion.Ruta.Replace("V", "P") + Facturacion.Correlativo_fac;

            string FECHA = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            if (existe_Documento_sys(NUM_DOC_SIS))
            { }

            else
            {
                int cantidad_item = Detalles_fac.Rows.Count;

                con.conectar("DM");
                SqlCommand cmd1 = new SqlCommand("[STREET].[INSERT_ENC_PED_STREET]", con.condm);
                cmd1.CommandType = CommandType.StoredProcedure;

                cmd1.Parameters.AddWithValue("@NUM_DOC_SIS", NUM_DOC_SIS);
                cmd1.Parameters.AddWithValue("@NUM_DOC_PREIMP", Facturacion.Correlativo_fac);
                cmd1.Parameters.AddWithValue("@TIPO_DOC", "F");
                cmd1.Parameters.AddWithValue("@RUTA", Facturacion.Ruta.Replace("V", "R"));
                cmd1.Parameters.AddWithValue("@VENDEDOR", Facturacion.Ruta);
                cmd1.Parameters.AddWithValue("@ENTREGA", Facturacion.Ruta.Replace("V", "E"));
                cmd1.Parameters.AddWithValue("@COD_CLIE", textBox1.Text);
                cmd1.Parameters.AddWithValue("@COD_CLIE_ALT", textBox1.Text);
                cmd1.Parameters.AddWithValue("@FECHA_PEDIDO", FECHA);
                cmd1.Parameters.AddWithValue("@HORA_INICIO_PEDIDO", FECHA);
                cmd1.Parameters.AddWithValue("@HORA_FIN_PEDIDO", FECHA);
                cmd1.Parameters.AddWithValue("@FECHA_DESPACHO", FECHA);
                cmd1.Parameters.AddWithValue("@MONTO_IMP", total_impuesto);
                cmd1.Parameters.AddWithValue("@MONTO_IMP_PERC", "0.00000");
                cmd1.Parameters.AddWithValue("@MONTO_IMP_RET", "0.00000");
                cmd1.Parameters.AddWithValue("@MONTO_SIN_IMP", valor_sin_iva);
                cmd1.Parameters.AddWithValue("@MONTO_CON_IMP", valor_con_iva);
                cmd1.Parameters.AddWithValue("@MONTO_DESC_CLIE", "0.0000");
                cmd1.Parameters.AddWithValue("@MONTO_DESC_LINEA", "0.0000");
                cmd1.Parameters.AddWithValue("@CANT_ITEM", cantidad_item);
                cmd1.Parameters.AddWithValue("@LISTA_PRECIO", LISTA_PRECIO_NUM);
                cmd1.Parameters.AddWithValue("@ESTADO_PEDIDO", 'N');
                cmd1.Parameters.AddWithValue("@CONDICION_CLIENTE", credito_clie);
                cmd1.Parameters.AddWithValue("@BODEGA", Facturacion.Ruta.Replace("V", "B"));
                cmd1.Parameters.AddWithValue("@LATITUD", "");
                cmd1.Parameters.AddWithValue("@LONGITUD", "");
                cmd1.Parameters.AddWithValue("@FECHA_CREA", FECHA);
                cmd1.Parameters.AddWithValue("@USUARIO_CREA", Login.usuario.ToUpper());

                if (CLIENTE_CLASE_DOC == "N")
                {
                    cmd1.Parameters.AddWithValue("@SERIE_DOC", "F" + Facturacion.Ruta.Substring(1, 3));
                }
                else if (CLIENTE_CLASE_DOC == "C")
                {
                    cmd1.Parameters.AddWithValue("@SERIE_DOC", "C" + Facturacion.Ruta.Substring(1, 3));
                }
                cmd1.Parameters.AddWithValue("@PROCESADO", "D");

                cmd1.ExecuteNonQuery();

                if (existe_Documento_sys(NUM_DOC_SIS))
                {
                    con.conectar("DM");
                    for (int i = 0; i < Detalles_fac.Rows.Count; i++)
                    {
                        DataRow row = Detalles_fac.Rows[i];

                        string factura = Convert.ToString(row["NUMERO FACTURA"]);
                        string linea = Convert.ToString(row["NUMERO LINEA"]);
                        string Tipodoc = Convert.ToString(row["TIPO_DOC"]);
                        string Articulo = Convert.ToString(row["COD ARTICULO"]);
                        string cantidad = Convert.ToString(row["CANTIDAD"]);
                        string precio_lista = Convert.ToString(row["PRECIO UNITARIO"]);
                        string Impuesto1 = Convert.ToString(row["IVA"]);
                        string Impuesto2 = Convert.ToString(row["CESC"]);
                        string Descuento = Convert.ToString(row["MONTO_DESC_ART"]);
                        string Monto_sin_iva = Convert.ToString(row["SUBTOTAL"]);
                        string tipo_unidad = Convert.ToString(row["TIPO_UNIDA"]);
                        string Lista_precio = Convert.ToString(row["LISTA PRECIO"]);
                        string costo = Convert.ToString(row["COSTO_ART"]);
                        string fecha_proceso = Convert.ToString(row["FECHA_CREA"]);
                        string Usuario = Convert.ToString(row["USUARIO"]);
                        string serie = Convert.ToString(row["SERIE_DOC"]);
                        string COD_DOCUMENTO = Convert.ToString(row["NUM_DOC"]);
                        string Bonificada = Convert.ToString(row["ART_BON"]);
                        string linea_bonificar = Convert.ToString(row["LINEA_ART_BON"]);
                       //  int linea_bonificar = Convert.ToInt32(row["SUBTOTAL"]);

                        
                        


                        SqlCommand cmd2 = new SqlCommand("[STREET].[INSERT_DET_PED_STREET]", con.condm);
                        cmd2.CommandType = CommandType.StoredProcedure;

                        cmd2.Parameters.AddWithValue("@NUM_DOC_SIS", NUM_DOC_SIS);
                        cmd2.Parameters.AddWithValue("@NUMERO_LINEA", linea);
                        cmd2.Parameters.AddWithValue("@TIPO_DOC", Tipodoc.ToUpper());
                        cmd2.Parameters.AddWithValue("@COD_ART", Articulo);
                        cmd2.Parameters.AddWithValue("@CATIDAD", cantidad);
                        cmd2.Parameters.AddWithValue("@PRECIO_UNIT", precio_lista);
                        cmd2.Parameters.AddWithValue("@MONTO_IMP1", Impuesto1);
                        cmd2.Parameters.AddWithValue("@MONTO_IMP2", Impuesto2);
                        cmd2.Parameters.AddWithValue("@MONTO_DESC_ART", Descuento);
                        cmd2.Parameters.AddWithValue("@MONTO_DESC_FAM", Descuento);
                        cmd2.Parameters.AddWithValue("@SUBTOTAL_LINEA", Monto_sin_iva);
                        cmd2.Parameters.AddWithValue("@TIPO_UNIDA", tipo_unidad);
                        cmd2.Parameters.AddWithValue("@LISTA_PRECIO", LISTA_PRECIO_NUM);
                        cmd2.Parameters.AddWithValue("@COSTO_ART", costo);
                        cmd2.Parameters.AddWithValue("@FECHA_CREA", fecha_proceso);
                        cmd2.Parameters.AddWithValue("@USUARIO", Usuario);
                        cmd2.Parameters.AddWithValue("@SERIE_DOC", serie);
                        cmd2.Parameters.AddWithValue("@PROCESADO", "D");
                        cmd2.Parameters.AddWithValue("@NUM_DOC", COD_DOCUMENTO);
                        cmd2.Parameters.AddWithValue("@ART_BON", Bonificada);
                        cmd2.Parameters.AddWithValue("@LINEA_ART_BON", linea_bonificar);
                       
                        cmd2.ExecuteNonQuery();
                       

                        

                    }
                    con.Desconectar("DM");
                }


               
            }
            con.Desconectar("DM");
        }


        #endregion InsertPedido

        private void Calculos()
        {
           
            object total;
            object cant_total;
            object imp;
            object csc;


            total = Detalles_fac.Compute("SUM(SUBTOTAL)", "");
            imp = Detalles_fac.Compute("SUM(IVA)", "");
            cant_total = Detalles_fac.Compute("SUM(CANTIDAD)", "");
            csc = Detalles_fac.Compute("SUM(CESC)", "");
            valor_sin_iva = Convert.ToDouble(total);
            total_impuesto = Convert.ToDouble(imp);           
            cantidad_total = Convert.ToDouble(cant_total);
            cesct = Convert.ToDouble(csc);
            valor_con_iva = valor_sin_iva + total_impuesto + cesct;

            label9.Text =  "$ "+Convert.ToString(Math.Round(valor_sin_iva,3));
            label14.Text = "$ "+Convert.ToString(Math.Round(total_impuesto,3));
            label10.Text = "$ " + Convert.ToString(Math.Round(cesct, 3));
            label11.Text = "$ "+Convert.ToString(Math.Round(valor_con_iva,3));
            label13.Text = Convert.ToString(cant_total);
        }

        
        private bool existe_Documento_sys(string factura)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("SELECT COUNT (*)  FROM [DM].[STREET].[ENC_PED_STREET]  where NUM_DOC_SIS = @NUM_DOC_SIS", con.condm);
            cmd.Parameters.AddWithValue("@NUM_DOC_SIS",factura);


            int contar = Convert.ToInt32(cmd.ExecuteScalar());
            con.Desconectar("DM");

            if (contar == 0)
            {
                return false;

            }
            else
            {
                return true;

            }
        }

        private bool existe_det_sys(string factura)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("SELECT COUNT (*)  FROM [DM].[STREET].[DET_PED_STREET]   where NUM_DOC_SIS = @NUM_DOC_SIS", con.condm);
            cmd.Parameters.AddWithValue("@NUM_DOC_SIS", factura);


            int contar = Convert.ToInt32(cmd.ExecuteScalar());
            con.Desconectar("DM");

            if (contar == 0)
            {
                return false;

            }
            else
            {
                return true;

            }
        }


        private void load_update(string pedidos)
        {

            tabupdate.Clear();
            con.conectar("EX");
            SqlCommand cm2 = new SqlCommand("SELECT ENC.[COD_CLIE],CLIE.TELEFONO1,CLIE.NOMBRE,CLIE.DIRECCION	FROM [DM].[STREET].[ENC_PED_STREET] as ENC LEFT JOIN [EXACTUS].[dismo].[CLIENTE] as CLIE  on ENC.COD_CLIE = CLIE.CLIENTE  where NUM_DOC_SIS =  '"+pedidos+"'", con.conex);
            SqlDataAdapter da2 = new SqlDataAdapter(cm2);
            da2.Fill(tabupdate);

            for (int i = tabupdate.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = tabupdate.Rows[i];
                textBox3.Text = Convert.ToString(dr["TELEFONO1"]);
                textBox2.Text = Convert.ToString(dr["NOMBRE"]);
                textBox1.Text = Convert.ToString(dr["COD_CLIE"]);
                richTextBox1.Text = Convert.ToString(dr["DIRECCION"]);
                
            }



            Nombre_cliente("CLIENTE", textBox1.Text);
            Telefono_cliente("CLIENTE", textBox1.Text);
            Direccion_cliente("CLIENTE", textBox1.Text);
            Credito("CLIENTE", textBox1.Text);
            precio_num("CLIENTE", textBox1.Text);
            cliente_seleccion();
            Precios(NIVEL_PRECIO_CLIE);


            Detalles_fac.Clear();
            
            SqlCommand cm3 = new SqlCommand("SELECT [NUM_DOC_SIS] as 'NUMERO FACTURA',[NUMERO_LINEA] as 'NUMERO LINEA',[TIPO_DOC],[COD_ART] AS 'COD ARTICULO',art.DESCRIPCION as 'ARTICULO',[CATIDAD]as 'CANTIDAD',[PRECIO_UNIT] as 'PRECIO UNITARIO',[MONTO_IMP1] as 'IVA',[MONTO_IMP2] as 'CESC',[MONTO_DESC_ART],[MONTO_DESC_FAM],[SUBTOTAL_LINEA] as 'SUBTOTAL',[TIPO_UNIDA],[LISTA_PRECIO] as 'LISTA PRECIO',[COSTO_ART],[FECHA_CREA],[USUARIO],[SERIE_DOC],[NUM_DOC],[ART_BON],[LINEA_ART_BON] FROM [DM].[STREET].[DET_PED_STREET] AS det left join [EXACTUS].[dismo].[ARTICULO] AS art on det.COD_ART = art.ARTICULO  where NUM_DOC_SIS =  '" + pedidos+"'", con.conex);
            SqlDataAdapter da3 = new SqlDataAdapter(cm3);
            da3.Fill(Detalles_fac);
            con.Desconectar("EX");






            dataGridView1.DataSource = Detalles_fac;

            Calculos();


           
        }


        private void update_ped(string pedido)
        {
            string NUM_DOC_SIS = pedido;
            string FECHA = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            int cantidad_item = Detalles_fac.Rows.Count;

            con.conectar("DM");
            SqlCommand cmd1 = new SqlCommand("UPDATE [DM].[STREET].[ENC_PED_STREET] SET  [NUM_DOC_SIS] = @NUM_DOC_SIS ,[NUM_DOC_PREIMP] = @NUM_DOC_PREIMP ,[TIPO_DOC] = @TIPO_DOC   ,[RUTA] = @RUTA ,[VENDEDOR] = @VENDEDOR ,[ENTREGA] = @ENTREGA ,[COD_CLIE] = @COD_CLIE ,[COD_CLIE_ALT] = @COD_CLIE_ALT,[FECHA_PEDIDO] = @FECHA_PEDIDO,[HORA_INICIO_PEDIDO] = @HORA_INICIO_PEDIDO,[HORA_FIN_PEDIDO] = @HORA_FIN_PEDIDO ,[FECHA_DESPACHO] = @FECHA_DESPACHO,[MONTO_IMP] = @MONTO_IMP,[MONTO_IMP_PERC] = @MONTO_IMP_PERC ,[MONTO_IMP_RET] = @MONTO_IMP_RET,[MONTO_SIN_IMP] = @MONTO_SIN_IMP ,[MONTO_CON_IMP] = @MONTO_CON_IMP,[MONTO_DESC_CLIE] = @MONTO_DESC_CLIE ,[MONTO_DESC_LINEA] = @MONTO_DESC_LINEA ,[CANT_ITEM] = @CANT_ITEM ,[LISTA_PRECIO] = @LISTA_PRECIO ,[ESTADO_PEDIDO] = @ESTADO_PEDIDO ,[CONDICION_CLIENTE] = @CONDICION_CLIENTE ,[BODEGA] = @BODEGA ,[LATITUD] = @LATITUD ,[LONGITUD] = @LONGITUD ,[FECHA_CREA] = @FECHA_CREA ,[USUARIO_CREA] = @USUARIO_CREA ,[SERIE_DOC] = @SERIE_DOC  ,[PROCESADO] = @PROCESADO  WHERE NUM_DOC_SIS = '"+pedido+"'", con.condm);
           // cmd1.CommandType = CommandType.StoredProcedure;

            cmd1.Parameters.AddWithValue("@NUM_DOC_SIS", NUM_DOC_SIS);
            cmd1.Parameters.AddWithValue("@NUM_DOC_PREIMP", Facturacion.Correlativo_fac);
            cmd1.Parameters.AddWithValue("@TIPO_DOC", "F");
            cmd1.Parameters.AddWithValue("@RUTA", Facturacion.Ruta.Replace("V", "R"));
            cmd1.Parameters.AddWithValue("@VENDEDOR", Facturacion.Ruta);
            cmd1.Parameters.AddWithValue("@ENTREGA", Facturacion.Ruta.Replace("V", "E"));
            cmd1.Parameters.AddWithValue("@COD_CLIE", textBox1.Text);
            cmd1.Parameters.AddWithValue("@COD_CLIE_ALT", textBox1.Text);
            cmd1.Parameters.AddWithValue("@FECHA_PEDIDO", FECHA);
            cmd1.Parameters.AddWithValue("@HORA_INICIO_PEDIDO", FECHA);
            cmd1.Parameters.AddWithValue("@HORA_FIN_PEDIDO", FECHA);
            cmd1.Parameters.AddWithValue("@FECHA_DESPACHO", FECHA);
            cmd1.Parameters.AddWithValue("@MONTO_IMP", total_impuesto);
            cmd1.Parameters.AddWithValue("@MONTO_IMP_PERC", "0.00000");
            cmd1.Parameters.AddWithValue("@MONTO_IMP_RET", "0.00000");
            cmd1.Parameters.AddWithValue("@MONTO_SIN_IMP", valor_sin_iva);
            cmd1.Parameters.AddWithValue("@MONTO_CON_IMP", valor_con_iva);
            cmd1.Parameters.AddWithValue("@MONTO_DESC_CLIE", "0.0000");
            cmd1.Parameters.AddWithValue("@MONTO_DESC_LINEA", "0.0000");
            cmd1.Parameters.AddWithValue("@CANT_ITEM", cantidad_item);
            cmd1.Parameters.AddWithValue("@LISTA_PRECIO", LISTA_PRECIO_NUM);
            cmd1.Parameters.AddWithValue("@ESTADO_PEDIDO", 'N');
            cmd1.Parameters.AddWithValue("@CONDICION_CLIENTE", credito_clie);
            cmd1.Parameters.AddWithValue("@BODEGA", Facturacion.Ruta.Replace("V", "B"));
            cmd1.Parameters.AddWithValue("@LATITUD", "");
            cmd1.Parameters.AddWithValue("@LONGITUD", "");
            cmd1.Parameters.AddWithValue("@FECHA_CREA", FECHA);
            cmd1.Parameters.AddWithValue("@USUARIO_CREA", Login.usuario.ToUpper());

            if (CLIENTE_CLASE_DOC == "N")
            {
                cmd1.Parameters.AddWithValue("@SERIE_DOC", "F" + Facturacion.Ruta.Substring(1, 3));
            }
            else if (CLIENTE_CLASE_DOC == "C")
            {
                cmd1.Parameters.AddWithValue("@SERIE_DOC", "C" + Facturacion.Ruta.Substring(1, 3));
            }
            cmd1.Parameters.AddWithValue("@PROCESADO", "D");


            cmd1.ExecuteNonQuery();

            // ELIMINA DETALLE ANTERIOR------------------------------------------------------------------


            if (existe_det_sys(NUM_DOC_SIS))
            {
                con.conectar("DM");
                SqlCommand cmd3 = new SqlCommand("DELETE [DM].[STREET].[DET_PED_STREET]  where NUM_DOC_SIS = '" + NUM_DOC_SIS + "'", con.condm);
                cmd3.ExecuteNonQuery();
                con.Desconectar("DM");
            }

            // INSERTA NUEVO  DETALLE DE FACTURAS -----------------------------------------------------------
           if (existe_Documento_sys(NUM_DOC_SIS))
            {
                con.conectar("DM");
                for (int i = 0; i < Detalles_fac.Rows.Count; i++)
                {
                    DataRow row = Detalles_fac.Rows[i];

                    string factura = Convert.ToString(row["NUMERO FACTURA"]);
                    string linea = Convert.ToString(row["NUMERO LINEA"]);
                    string Tipodoc = Convert.ToString(row["TIPO_DOC"]);
                    string Articulo = Convert.ToString(row["COD ARTICULO"]);
                    string cantidad = Convert.ToString(row["CANTIDAD"]);
                    string precio_lista = Convert.ToString(row["PRECIO UNITARIO"]);
                    string Impuesto1 = Convert.ToString(row["IVA"]);
                    string Impuesto2 = Convert.ToString(row["CESC"]);
                    string Descuento = Convert.ToString(row["MONTO_DESC_ART"]);
                    string Monto_sin_iva = Convert.ToString(row["SUBTOTAL"]);
                    string tipo_unidad = Convert.ToString(row["TIPO_UNIDA"]);
                    string Lista_precio = Convert.ToString(row["LISTA PRECIO"]);
                    string costo = Convert.ToString(row["COSTO_ART"]);
                    DateTime fecha_proceso =Convert.ToDateTime(row["FECHA_CREA"]);
                    string Usuario = Convert.ToString(row["USUARIO"]);
                    string serie = Convert.ToString(row["SERIE_DOC"]);
                    string COD_DOCUMENTO = Convert.ToString(row["NUM_DOC"]);
                    string Bonificada = Convert.ToString(row["ART_BON"]);
                    string linea_bonificar = Convert.ToString(row["LINEA_ART_BON"]);
                    //  int linea_bonificar = Convert.ToInt32(row["SUBTOTAL"]);





                    SqlCommand cmd2 = new SqlCommand("[STREET].[INSERT_DET_PED_STREET]", con.condm);
                    cmd2.CommandType = CommandType.StoredProcedure;

                    cmd2.Parameters.AddWithValue("@NUM_DOC_SIS", NUM_DOC_SIS);
                    cmd2.Parameters.AddWithValue("@NUMERO_LINEA", linea);
                    cmd2.Parameters.AddWithValue("@TIPO_DOC", Tipodoc.ToUpper());
                    cmd2.Parameters.AddWithValue("@COD_ART", Articulo);
                    cmd2.Parameters.AddWithValue("@CATIDAD", cantidad);
                    cmd2.Parameters.AddWithValue("@PRECIO_UNIT", precio_lista);
                    cmd2.Parameters.AddWithValue("@MONTO_IMP1", Impuesto1);
                    cmd2.Parameters.AddWithValue("@MONTO_IMP2", Impuesto2);
                    cmd2.Parameters.AddWithValue("@MONTO_DESC_ART", Descuento);
                    cmd2.Parameters.AddWithValue("@MONTO_DESC_FAM", Descuento);
                    cmd2.Parameters.AddWithValue("@SUBTOTAL_LINEA", Monto_sin_iva);
                    cmd2.Parameters.AddWithValue("@TIPO_UNIDA", tipo_unidad);
                    cmd2.Parameters.AddWithValue("@LISTA_PRECIO", LISTA_PRECIO_NUM);
                    cmd2.Parameters.AddWithValue("@COSTO_ART", costo);
                    cmd2.Parameters.AddWithValue("@FECHA_CREA", fecha_proceso);
                    cmd2.Parameters.AddWithValue("@USUARIO", Usuario);
                    cmd2.Parameters.AddWithValue("@SERIE_DOC", serie);
                    cmd2.Parameters.AddWithValue("@PROCESADO", "D");
                    cmd2.Parameters.AddWithValue("@NUM_DOC", COD_DOCUMENTO);
                    cmd2.Parameters.AddWithValue("@ART_BON", Bonificada);
                    cmd2.Parameters.AddWithValue("@LINEA_ART_BON", linea_bonificar);

                    cmd2.ExecuteNonQuery();




                }
                con.Desconectar("DM");

            }
            }
        }
}
