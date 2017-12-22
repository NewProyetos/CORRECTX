using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace Sinconizacion_EXactus.CORECTX_APP.CREDITOS.IMPUESTO_DISTRIB
{
    public partial class Pago_Impuesto_Distribucion : Form
    {
        public Pago_Impuesto_Distribucion()
        {
            InitializeComponent();
        }
        conexionXML con = new conexionXML();
        DataTable rutas = new DataTable();
        DataTable Entrega = new DataTable();
        DataTable zonas = new DataTable();
        DataTable impuesto = new DataTable();
        public static DataTable direccion = new DataTable();
        String usuario;
        String empresa = Login.empresa;
        String fecha_pago;
        public int idx;
        String ID;
        
        
        //String Nombre;
        private void Pago_Impuesto_Distribucion_Load(object sender, EventArgs e)
        {
            usuario = Login.usuario.ToUpper();
            toolStripButton2.Enabled = false;
            toolStripButton5.Enabled = false;
            button2.Enabled = false;
            //toolStripDropDownButton2.HideDropDown();
         //   toolStripDropDownButton2.Enabled = false;
          fecha_pago = dateTimePicker3.Value.ToString("yyyy-MM-dd");
            //textBox1.Enabled = false;
            //textBox2.Enabled = false;
            //textBox3.Enabled = false;
            //comboBox1.Enabled = false;
            //comboBox2.Enabled = false;
            //comboBox3.Enabled = false;
            //comboBox4.Enabled = false;
            //comboBox5.Enabled = false;

            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;          
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true; 

            groupBox2.Enabled = false;



            con.conectar("EX");

            SqlCommand cm1 = new SqlCommand("SELECT [VENDEDOR],[NOMBRE],[E_MAIL]  FROM [EXACTUS].["+empresa+"].[VENDEDOR]  where VENDEDOR <> 'ND' and VENDEDOR <> 'CXC' and ACTIVO = 'S' and NOMBRE not like '%INACTIVO%'  order by VENDEDOR", con.conex);
            SqlDataAdapter da1 = new SqlDataAdapter(cm1);
             da1.Fill(rutas);

             SqlCommand cm2 = new SqlCommand("SELECT [COBRADOR] ,[NOMBRE] FROM [EXACTUS].[" + empresa + "].[COBRADOR] where COBRADOR like 'E%' or COBRADOR like 'C%' and COBRADOR <> 'CXC' ", con.conex);
             SqlDataAdapter da2 = new SqlDataAdapter(cm2);
             da2.Fill(Entrega);

             SqlCommand cm4 = new SqlCommand("SELECT dpto.NOMBRE_DPTO,zon.ZONA,zon.[NOMBRE] FROM [EXACTUS].[" + empresa + "].[ZONA] as  zon INNER JOIN   [DM].[CORRECT].[DEPARTAMENTOS ELS] as dpto  on LEFT(zon.ZONA,2) = dpto.OTRO where dpto.EMPRESA = '" + empresa + "'", con.conex);
             SqlDataAdapter da4 = new SqlDataAdapter(cm4);
             da4.Fill(zonas);

           




            con.Desconectar("EX");

            con.conectar("DM");

            SqlCommand cm3 = new SqlCommand("SELECT [DESCIPCION] FROM [DM].[CORRECT].[FREC_VISIT_IMP_DIST]", con.condm);
            SqlDataReader dr1 = cm3.ExecuteReader();
            while (dr1.Read())
            {
                comboBox5.Items.Add(dr1["DESCIPCION"]);

            }
            dr1.Close();


            SqlCommand direcm = new SqlCommand("SELECT  distinct TOP 1000 [DIRECCION] FROM [DM].[CORRECT].[IMPUESTO_DISTRIBUCION]", con.conex);
            SqlDataAdapter direcda = new SqlDataAdapter(direcm);
            direcda.Fill(direccion);


           con.Desconectar("DM");

           cargagrid();
            combo(rutas, Entrega, zonas);

            textBox1.AutoCompleteCustomSource = AutocompleteDIREC();
            textBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;




        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {
            groupBox2.Enabled = false;
            toolStripComboBox2.Text = "";
        }

        private void toolStripComboBox2_Click(object sender, EventArgs e)
        {
            groupBox2.Enabled = false;
            toolStripComboBox1.Text = "";
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            groupBox2.Enabled = false;
            toolStripComboBox1.Text = "";
            toolStripComboBox2.Text = "";
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            groupBox2.Enabled = false;


        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Limpiar();
            groupBox2.Enabled = true;
            button2.Enabled = false;
            toolStripButton5.Enabled = true;
            toolStripButton2.Enabled = false;
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           

        }

        private void toolStripComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        public void combo(DataTable dts1, DataTable dts2 ,DataTable dts3)
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();

            var result = from row in dts1.AsEnumerable()
                         group row by row.Field<string>("VENDEDOR") into grp
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
                    comboBox1.Items.Add(t.Vendedor);
                }
            }

            var result2 = from row1 in dts2.AsEnumerable()
                          group row1 by row1.Field<string>("COBRADOR") into ln
                          select new
                          {
                              Cobrador = ln.Key,

                          };
            foreach (var t1 in result2)
            {
                if (t1.Cobrador == null || t1.Cobrador == "")
                {

                }
                else
                {
                    comboBox2.Items.Add(t1.Cobrador);
                }
            }


            var result3 = from row3 in dts3.AsEnumerable()
                          group row3 by row3.Field<string>("NOMBRE_DPTO") into ln
                          select new
                          {
                              Departamento = ln.Key,

                          };
            foreach (var t1 in result3)
            {
                if (t1.Departamento == null || t1.Departamento == "")
                {

                }
                else
                {
                    comboBox3.Items.Add(t1.Departamento);
                }
            }


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                Nombre_Vendedor();
            }
            else
            {
                label11.Text = "";
            }
           
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {

            

        }

        private void comboBox1_KeyUp(object sender, KeyEventArgs e)
        {
           comboBox1.DroppedDown = true;

            //DataView view = new DataView(rutas);
            //view.RowFilter = "VENDEDOR like '" + this.comboBox1.Text + "%'";
            //comboBox1.DataSource = view;
            //comboBox1.DisplayMember = "VENDEDOR";
            
        }

        private void comboBox2_KeyUp(object sender, KeyEventArgs e)
        {
            comboBox2.DroppedDown = true;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            zonas.DefaultView.RowFilter = "NOMBRE_DPTO like '" + this.comboBox3.Text + "%'";
            comboBox4.DataSource = zonas;
            comboBox4.DisplayMember = "NOMBRE";
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == null || comboBox1.Text == "")
            {    
                messagesw("Debe seleccionar un vendedor", "!!Warning!!!",MessageBoxIcon.Warning,MessageBoxButtons.OK);
                comboBox1.Focus();
            }
            else if (comboBox2.Text == null || comboBox2.Text == "")            {

                messagesw("Debe seleccionar un Entregador", "!!Warning!!!", MessageBoxIcon.Warning, MessageBoxButtons.OK);
                comboBox2.Focus();
            }
            else if (comboBox3.Text == null || comboBox3.Text == "")
            {
                messagesw("Debe seleccionar un Departamento", "!!Warning!!!", MessageBoxIcon.Warning, MessageBoxButtons.OK);
                comboBox3.Focus();
            }
            else if (comboBox4.Text == null || comboBox4.Text == "")
            {
                messagesw("Debe seleccionar un Municipio", "!!Warning!!!", MessageBoxIcon.Warning, MessageBoxButtons.OK);
                comboBox4.Focus();
            }
            else if (comboBox5.Text == null || comboBox5.Text == "")
            {
                messagesw("Debe seleccionar una Frecuencia de Visita", "!!Warning!!!", MessageBoxIcon.Warning, MessageBoxButtons.OK);
                comboBox5.Focus();
            }

            else if (textBox1.Text == null || textBox1.Text == "")
            {
                messagesw("Ingrese Direccion", "!!Warning!!!", MessageBoxIcon.Warning, MessageBoxButtons.OK);
                textBox1.Focus();
            }
            else if (textBox4.Text == null || textBox4.Text == "")
            {
                messagesw("Ingrese nombre de persona que recibe el dinero", "!!Warning!!!", MessageBoxIcon.Warning, MessageBoxButtons.OK);
                textBox4.Focus();
            }
            else if (textBox2.Text == "0.0" || textBox2.Text == "")
            {
                messagesw("El valor pagado no puede ser 0.0 ", "!!Warning!!!", MessageBoxIcon.Warning, MessageBoxButtons.OK);
                textBox2.Focus();
            }


            else if (textBox3.Text == "0.0" || textBox3.Text == "")
            {
                messagesw("El valor de la venta no puede ser 0.0 ", "!!Warning!!!", MessageBoxIcon.Warning, MessageBoxButtons.OK);
                textBox3.Focus();
            }
          

            else
            {
                insert();
            
            }

        }


        private void messagesw(string message , string caption,MessageBoxIcon icon, MessageBoxButtons buttons)
        {

          
              
                DialogResult result;

                // Displays the MessageBox.

                result = MessageBox.Show(message, caption,buttons,icon);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {

                    // Closes the parent form.

                    this.Close();

                }

            }

        private void insert()
        {
            string fecha = DateTime.Now.ToString("yyyy-MM-dd");
            

            con.conectar("DM");
            Guid GuD = Guid.NewGuid();
            SqlCommand cmd3 = new SqlCommand();
            cmd3.Connection = con.condm;
            cmd3.CommandText = "INSERT INTO [DM].[CORRECT].[IMPUESTO_DISTRIBUCION]([RUTA_VENTA],[RUTA_ENTREGA],[NOMBRE_VENDEDOR],[NOMBRE_ENTREGADOR],[DIRECCION],[DEPARTAMENTO],[MUNICIPIO],[COD_MUN] ,[COD_DPTO],[FRECUENCIA],[VALOR],[VENTA_SECTOR],[FECHA],[ROWID],[USUARIO_INGRESO],[NOMBRE_RECIBE],[FECHA_PAGO])  VALUES(@RUTA_VENTA,@RUTA_ENTREGA,@NOMBRE_VENDEDOR,@NOMBRE_ENTREGADOR,@DIRECCION,@DEPARTAMENTO,@MUNICIPIO,@COD_MUN,@COD_DPTO,@FRECUENCIA,@VALOR,@VENTA_SECTOR,@FECHA,@ROWID,@USUARIO_INGRESO,@NOMBRE_RECIBE,@FECHA_PAGO)";
            cmd3.Parameters.Add("@RUTA_VENTA", SqlDbType.NVarChar).Value = comboBox1.Text;
            cmd3.Parameters.Add("@RUTA_ENTREGA", SqlDbType.VarChar).Value = comboBox2.Text;
            cmd3.Parameters.Add("@NOMBRE_VENDEDOR", SqlDbType.VarChar).Value = label11.Text;
            cmd3.Parameters.Add("@NOMBRE_ENTREGADOR", SqlDbType.VarChar).Value = label12.Text;
            cmd3.Parameters.Add("@DIRECCION", SqlDbType.VarChar).Value = textBox1.Text.ToUpper();
            cmd3.Parameters.Add("@DEPARTAMENTO", SqlDbType.VarChar).Value = comboBox3.Text;
            cmd3.Parameters.Add("@MUNICIPIO", SqlDbType.VarChar).Value = comboBox4.Text;
            cmd3.Parameters.Add("@COD_MUN", SqlDbType.VarChar).Value = "";
            cmd3.Parameters.Add("@COD_DPTO", SqlDbType.VarChar).Value = "";
            cmd3.Parameters.Add("@FRECUENCIA", SqlDbType.VarChar).Value = comboBox5.Text;
            cmd3.Parameters.Add("@VALOR", SqlDbType.VarChar).Value = textBox2.Text;
            cmd3.Parameters.Add("@VENTA_SECTOR", SqlDbType.NVarChar).Value = textBox3.Text;
            cmd3.Parameters.Add("@FECHA", SqlDbType.NVarChar).Value = fecha;
            cmd3.Parameters.Add("@ROWID", SqlDbType.UniqueIdentifier).Value = GuD;
            cmd3.Parameters.Add("@USUARIO_INGRESO", SqlDbType.NVarChar).Value = usuario;
            cmd3.Parameters.Add("@NOMBRE_RECIBE", SqlDbType.NVarChar).Value = textBox4.Text;
            cmd3.Parameters.Add("@FECHA_PAGO", SqlDbType.NVarChar).Value = fecha_pago;
          




            cmd3.ExecuteNonQuery();
            con.Desconectar("DM");

            MessageBox.Show("Registro Ingresado Exitosamente");

            Limpiar();
            cargagrid();
        }

        private void delete()
        {
            string fecha = DateTime.Now.ToString("yyyy-MM-dd");

            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("[CORRECT].[DEL_IMP_DIST]", con.condm);
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.AddWithValue("@USER", usuario);
            cmd.Parameters.AddWithValue("@FECHA", fecha);
            cmd.Parameters.AddWithValue("@ID_IMP", ID);

            cmd.ExecuteNonQuery();

            con.Desconectar("DM");

            Limpiar();
            cargagrid();
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void Limpiar()
        {
            comboBox1.Text = "";
            comboBox2.Text = "";
            comboBox3.Text = "";
            comboBox4.Text = "";
            comboBox5.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            label11.Text = "";
            label12.Text = "";


        }

        private void button1_Click(object sender, EventArgs e)
        {
            cargagrid();
        }


        private void cargagrid()
        {
            impuesto.Clear();
            
            con.conectar("DM");

            SqlCommand cm5 = new SqlCommand("SELECT [ID_IMP],[FECHA_PAGO],[RUTA_VENTA],[RUTA_ENTREGA],[NOMBRE_VENDEDOR],[NOMBRE_ENTREGADOR],[DIRECCION],[DEPARTAMENTO],[MUNICIPIO],[FRECUENCIA],[VALOR],[VENTA_SECTOR],[NOMBRE_RECIBE],[FECHA] as 'FECHA INGRESO' FROM [DM].[CORRECT].[IMPUESTO_DISTRIBUCION] where DATEADD(dd, 0, DATEDIFF(dd, 0,FECHA)) >= '" + this.dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and DATEADD(dd, 0, DATEDIFF(dd, 0,FECHA)) <= '" + this.dateTimePicker2.Value.ToString("yyyy-MM-dd") + "'", con.condm);
            SqlDataAdapter da5 = new SqlDataAdapter(cm5);
            da5.Fill(impuesto);


            con.Desconectar("DM");           
            dataGridView1.DataSource = impuesto;
            dataGridView1.Refresh();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            toolStripButton5.Enabled = false;
            toolStripComboBox1.Clear();
            string nd = toolStripComboBox1.Text;


            idx = dataGridView1.CurrentRow.Index;

           ID = Convert.ToString(dataGridView1.Rows[idx].Cells[0].Value);

           dateTimePicker1.Text = Convert.ToString(dataGridView1.Rows[idx].Cells[1].Value);
           comboBox1.Text = Convert.ToString(dataGridView1.Rows[idx].Cells[2].Value);
           comboBox2.Text = Convert.ToString(dataGridView1.Rows[idx].Cells[3].Value);
           label11.Text = Convert.ToString(dataGridView1.Rows[idx].Cells[4].Value);
           label12.Text = Convert.ToString(dataGridView1.Rows[idx].Cells[5].Value);
           textBox1.Text= Convert.ToString(dataGridView1.Rows[idx].Cells[6].Value);
           comboBox3.Text = Convert.ToString(dataGridView1.Rows[idx].Cells[7].Value);
           comboBox4.Text = Convert.ToString(dataGridView1.Rows[idx].Cells[8].Value);
           comboBox5.Text = Convert.ToString(dataGridView1.Rows[idx].Cells[9].Value);
           textBox2.Text = Convert.ToString(dataGridView1.Rows[idx].Cells[10].Value);
           textBox3.Text = Convert.ToString(dataGridView1.Rows[idx].Cells[11].Value);
           textBox4.Text = Convert.ToString(dataGridView1.Rows[idx].Cells[12].Value);

           if (ID != null || ID != "")
           {
               toolStripButton2.Enabled = true;
           }

        }

        private void toolStripComboBox1_KeyUp(object sender, KeyEventArgs e)
        {
           

            impuesto.DefaultView.RowFilter = "RUTA_VENTA like '" + this.toolStripComboBox1.Text + "%'";
            dataGridView1.DataSource = impuesto;

        }

        private void toolStripComboBox2_KeyUp(object sender, KeyEventArgs e)
        {
            
            impuesto.DefaultView.RowFilter = "RUTA_ENTREGA like '" + this.toolStripComboBox2.Text + "%'";
            dataGridView1.DataSource = impuesto;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (ID != null || ID != "")
            {


                MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show("REALMENTE DESEA ELIMINAR EL REGISTRO No.: " + ID+ "", "IMPUESTO DE DISTRIBUCION", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {

                    delete();

                }
                else
                {
                   
                   
                }



                
            }
        }


        private void copyall()
        {
            int cellfin;
            cellfin = dataGridView1.ColumnCount;

            dataGridView1.SelectAll();
            DataObject dtobj = dataGridView1.GetClipboardContent();
            if (dtobj != null)
            {
                Clipboard.SetDataObject(dtobj);
            }

        }

        private void sendexcel(DataGridView drg)
        {

            int cellfin;
            cellfin = dataGridView1.ColumnCount;
            copyall();

            Microsoft.Office.Interop.Excel.Application excell;
            Microsoft.Office.Interop.Excel.Workbook workbook;
            Microsoft.Office.Interop.Excel.Worksheet Sheet;
            object miobj = System.Reflection.Missing.Value;
            excell = new Excel.Application();
            excell.Visible = true;


            int incre;

            int Columnas, col;

            col = drg.Columns.Count / 26;

            string Letracol = "ABCDEFEHIJKLMNOPQRSTUVWXYZ";
            string Complementocol;
            //Determinando la letra que se usara despues de la columna 26
            if (col > 0)
            {
                Columnas = drg.Columns.Count - (26 * col);
                Complementocol = Letracol.ToString().Substring(col - 1, 1);
            }
            else
            {
                Columnas = drg.Columns.Count;
                Complementocol = "";
            }

            string ColumnaFinal;

            incre = Encoding.ASCII.GetBytes("A")[0];

            ColumnaFinal = Complementocol.ToString() + Convert.ToChar(incre + Columnas - 1).ToString();


            workbook = excell.Workbooks.Add(miobj);
            Sheet = workbook.Worksheets.get_Item(1);

            Excel.Range rg = Sheet.Cells[5, 1];
            Excel.Range Enc;
            Excel.Range det;
            Excel.Range RN;
            Excel.Range Report;
            Excel.Range Reportxt;
            rg.Select();

            // obtener colummnas de encabezado
           





            for (int c = 0; c < drg.Columns.Count; c++)
            {

                Sheet.Cells[4, c + 1] = String.Format("{0}", impuesto.Columns[c].Caption);
            }


            Sheet.PasteSpecial(rg, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);

            try
            {
                // nombre de la empresa
                RN = Sheet.get_Range("A1", ColumnaFinal + "1");
                RN.Font.Name = "Times New Roman";
                //rango.Font.Color = Color.Blue;
                RN.Font.Size = 14;

                Sheet.Cells[1, 1] = "DISTRIBUIDORA MORAZAN SA DE CV";
                RN.Merge();
                RN.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;


                //Nombre del Reporte 
                Report = Sheet.get_Range("A2", ColumnaFinal + "2");
                Report.Font.Name = "Times New Roman";
                Report.Font.Size = 12;            

                
                Sheet.Cells[2, 1] = "IMPUESTO DE DISTRIBUCION" + " EMISION " + DateTime.Now.ToString();

                Report.Select();
                Report.Merge();
                Report.Font.Bold = true;
                Report.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;




                Reportxt = Sheet.get_Range("A3", ColumnaFinal + "3");
                Reportxt.Font.Name = "Times New Roman";
                Reportxt.Font.Size = 12;



                Sheet.Cells[3, 1] = "DETALLE " + "   DEL " + dateTimePicker1.Value.ToString("dd-MM-yyyy") + "  AL  " + dateTimePicker2.Value.ToString("dd-MM-yyyy") + " ";

                Reportxt.Select();
                Reportxt.Merge();
                Reportxt.Font.Bold = true;
               Reportxt.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
               



                //ENCABEZDO DE COLUMNAS
                Enc = Sheet.get_Range("A4", ColumnaFinal + 4);
                Enc.Font.Name = "Times New Roman";
                Enc.Font.Size = 9;
                Enc.Borders.LineStyle = Excel.XlLineStyle.xlDouble;
                Enc.Font.Bold = true;

                //DETALLE 
                //ENCABEZDO DE COLUMNAS
               

            }
            catch (SystemException exec)
            {
                MessageBoxButtons bt1 = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(exec.ToString(), "!!!!ERROR!!!!", bt1, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);


            }
            
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
                button2.Enabled = true;
            
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            copyall();
            sendexcel(dataGridView1);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textBox2.Text.Contains('.'))
           {
                      if(!char.IsDigit(e.KeyChar))
                      {
                               e.Handled = true;
                       }    

                       if (e.KeyChar == '\b')
                       {
                                e.Handled = false;
                        }
           }
           else
           {
                          if(!char.IsDigit(e.KeyChar))
                          {
                                  e.Handled = true;
                           }

                           if(e.KeyChar=='.' || e.KeyChar=='\b')
                          {
                                   e.Handled = false;
                          }
            }    

            
          
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textBox3.Text.Contains('.'))
            {
                if (!char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }

                if (e.KeyChar == '\b')
                {
                    e.Handled = false;
                }
            }
            else
            {
                if (!char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }

                if (e.KeyChar == '.' || e.KeyChar == '\b')
                {
                    e.Handled = false;
                }
            }    

            

        }

        private void comboBox2_Leave(object sender, EventArgs e)
        {
            if (comboBox2.Text != "")
            {
                Nombre_cobrador();
            }
            else
            {
                label12.Text = "";

            }
            
        }




        private void Nombre_cobrador()
        {
            var results = from myRow in Entrega.AsEnumerable()
                          where myRow.Field<string>("COBRADOR") == comboBox2.Text

                          select new
                          {
                              Nombre = myRow.Field<string>("NOMBRE")
                          };

            foreach (var rs1 in results)
            {
                label12.Text = rs1.Nombre;
            }



        }
        private void Nombre_Vendedor()
        {
            var results = from myRow in rutas.AsEnumerable()
                          where myRow.Field<string>("VENDEDOR") == comboBox1.Text

                          select new
                          {
                              Nombre = myRow.Field<string>("NOMBRE")
                          };

            foreach (var rs1 in results)
            {
                label11.Text = rs1.Nombre;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text != "")
            {
                Nombre_cobrador();
            }
            else
            {
                label12.Text = "";
            
            }
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                Nombre_Vendedor();
            }
            else
            {
                label11.Text = "";
            }
           
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            fecha_pago = dateTimePicker3.Value.ToString("yyyy-MM-dd");
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            Reporte_Impuesto_Distribucion rpt = new Reporte_Impuesto_Distribucion();
            rpt.Show();
        }

        private void toolStripComboBox1_Leave(object sender, EventArgs e)
        {
            toolStripComboBox1.Text = "";
            
        }



       

        public static AutoCompleteStringCollection AutocompleteDIREC()
        {
                      
           

            AutoCompleteStringCollection coleccion = new AutoCompleteStringCollection();
            //recorrer y cargar los items para el autocompletado
            foreach (DataRow row in direccion.Rows)
            {
                coleccion.Add(Convert.ToString(row["DIRECCION"]));
            }

            return coleccion;
        }



        }



    }

