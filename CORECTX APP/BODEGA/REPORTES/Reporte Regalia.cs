using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using Microsoft.CSharp;

namespace Sinconizacion_EXactus
{
    public partial class Reporte_Regalia : Form
    {


        public Reporte_Regalia(string report)
        {
            InitializeComponent();
            Tipo_repote = report;
            exp.OnProgressUpdate += exp_Process;
        }
        //Conexion2 conet = new Conexion2();
        conexionXML con = new conexionXML();
        DataTable REGALIAS;
        DataTable agencia = new DataTable();
        String conse_reg;
        DataSet ds = new DataSet();
        Exportador exp = new Exportador();
        public Excel.Application aplicacion;
        public Excel.Workbook libros_trabajo;
        String Tipo_repote;

        private void exp_Process(int value)
        {
            base.Invoke((Action)delegate
            {
                int per = (value + 1) * 100 / REGALIAS.Rows.Count;
                label4.Text = "Cargando Registros  " + Convert.ToString(value + 1);
               // progressBar1.Value = per;
            });

        }


        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            button2.Enabled = false;
        }

        private void Form16_Load(object sender, EventArgs e)
        {
            if (Tipo_repote == "Nuevo")
            {
             //   groupBox1.Enabled = false;
            }

            else
            {
              //  groupBox1.Enabled = true;
            }
            comboBox1.Enabled = false;

            
            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            //dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            button2.Enabled = false;
          //  radioButton1.Checked = true;

            agencias(Login.empresa_id);

            comboBox2.Text = CORECTX_APP.BODEGA.REGALIAS.Regalias.agecia_txt;

            if (Main_Menu.multisucursal == "S")
            {
                comboBox2.Enabled = true;

            }
            else
            {
                comboBox2.Enabled = false;
            }
            
         //   comboBox1.Text = "TODOS";

           // progressBar1.Hide();
        }
        private void cargar_usuarios()
        {
            con.conectar("DM");
            SqlCommand cm2 = new SqlCommand("SELECT[USUARIO]FROM [DM].[CORRECT].[REGALIAS] GROUP BY USUARIO", con.condm);
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
                comboBox1.Items.Add(dr2["USUARIO"]);
            }
            dr2.Close();
            con.Desconectar("DM");
        }


        private void button1_Click(object sender, EventArgs e)
        {
             REGALIAS= new DataTable();

            button2.Enabled = false;
            if (Tipo_repote == "Nuevo")
            {
                //try
                //{

                    if (ds.Tables.Contains("REGALIAS"))
                    {
                        ds.Tables.Remove(REGALIAS);
                    }

                    REGALIAS.Clear();
                    REGALIAS.Columns.Clear();
                    con.conectar("DM");

                    SqlCommand cmd = new SqlCommand("[CORRECT].[REPORTE_DETALLE_NUEVO]", con.condm);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 50;
                    cmd.Parameters.AddWithValue("@fec_ini", Convert.ToDateTime(this.dateTimePicker1.Value.ToString("yyyy/MM/dd")));
                    cmd.Parameters.AddWithValue("@fec_fin", Convert.ToDateTime(this.dateTimePicker2.Value.ToString("yyyy/MM/dd")));
                    cmd.Parameters.AddWithValue("@empresa", Login.empresa);
                    if (comboBox2.Text == "TODOS")
                    {
                        cmd.Parameters.AddWithValue("@reg_suc", null);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@reg_suc", conse_reg);
                    }
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(REGALIAS);


                    con.Desconectar("DM");

                    dataGridView1.DataSource = REGALIAS;

                    if (REGALIAS.Rows.Count > 0)
                    {
                        combo(REGALIAS, "USUARIO", comboBox1);
                    }
                    //  dataGridView1.Refresh();
                    //REGALIAS.Columns["DateOfOrder"].Parce(val => DateTime.Parse(val.ToString()).ToString("dd/MMM/yyyy"));

                    //foreach (DataRow dr in REGALIAS.Rows)
                    //{
                    //    dr["FECHA"] = DateTime.Parse(String.Format("{0}:dd/MM/yyyy", dr["FECHA"]));
                    //}



                    //  exp.NombreReporte = "DETALLE REGALIAS DISMO";



                //}
                //catch (SystemException exec)
                //{
                //    MessageBoxButtons bt1 = MessageBoxButtons.OK;
                //    DialogResult result = MessageBox.Show(exec.ToString(), "!!!!ERROR!!!!", bt1, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                //    con.Desconectar("DM");

                //}




            }

            else
            {

                //if(radioButton1.Checked)
                //{


                //     this.Refresh();
                //     if (Regalias_Vencido.tipo == "R")
                //     {
                //         try
                //         {

                //             if (ds.Tables.Contains("REGALIAS"))
                //             {
                //                 ds.Tables.Remove(REGALIAS);
                //             }

                //             REGALIAS.Clear();
                //             REGALIAS.Columns.Clear();
                //             con.conectar("DM");

                //             SqlCommand cmd = new SqlCommand("[CORRECT].[REPORTE_DETALLE_REGALIA]", con.condm);
                //             cmd.CommandType = CommandType.StoredProcedure;
                //             cmd.CommandTimeout = 50;
                //             cmd.Parameters.AddWithValue("@fec_ini", Convert.ToDateTime(this.dateTimePicker1.Value.ToString("yyyy/MM/dd")));
                //             cmd.Parameters.AddWithValue("@fec_fin", Convert.ToDateTime(this.dateTimePicker2.Value.ToString("yyyy/MM/dd")));
                //             if (comboBox1.Text == "TODOS")
                //             {
                //                 cmd.Parameters.AddWithValue("@usuario", null);
                //             }
                //             else
                //             {

                //                 cmd.Parameters.AddWithValue("@usuario", comboBox1.Text);
                //             }

                //             cmd.ExecuteNonQuery();
                //             SqlDataAdapter da = new SqlDataAdapter(cmd);
                //             da.Fill(REGALIAS);


                //             con.Desconectar("DM");

                //             dataGridView1.DataSource = REGALIAS;
                //           //  dataGridView1.Refresh();
                //           //REGALIAS.Columns["DateOfOrder"].Parce(val => DateTime.Parse(val.ToString()).ToString("dd/MMM/yyyy"));

                //             //foreach (DataRow dr in REGALIAS.Rows)
                //             //{
                //             //    dr["FECHA"] = DateTime.Parse(String.Format("{0}:dd/MM/yyyy", dr["FECHA"]));
                //             //}



                //           //  exp.NombreReporte = "DETALLE REGALIAS DISMO";



                //         }
                //         catch (SystemException exec)
                //         {
                //             MessageBoxButtons bt1 = MessageBoxButtons.OK;
                //             DialogResult result = MessageBox.Show(exec.ToString(), "!!!!ERROR!!!!", bt1, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                //             con.Desconectar("DM");

                //         }

                //     }





                //     }

                //if (radioButton2.Checked)
                //{


                //    this.Refresh();
                //    if (Regalias_Vencido.tipo == "R")
                //    {
                //        try
                //        {

                //            if (ds.Tables.Contains("REGALIAS"))
                //            {
                //                ds.Tables.Remove(REGALIAS);
                //            }

                //            REGALIAS.Clear();
                //            REGALIAS.Columns.Clear();
                //            con.conectar("DM");

                //            SqlCommand cmd = new SqlCommand("[CORRECT].[REPORTE_DETALLE_REGALIA_NP]", con.condm);
                //            cmd.CommandType = CommandType.StoredProcedure;
                //            cmd.CommandTimeout = 50;
                //            cmd.Parameters.AddWithValue("@fec_ini", Convert.ToDateTime(this.dateTimePicker1.Value.ToShortDateString()));
                //            cmd.Parameters.AddWithValue("@fec_fin", Convert.ToDateTime(this.dateTimePicker2.Value.ToShortDateString()));
                //            if (comboBox1.Text == "TODOS")
                //            {
                //                cmd.Parameters.AddWithValue("@usuario", null);
                //            }
                //            else
                //            {

                //                cmd.Parameters.AddWithValue("@usuario", comboBox1.Text);
                //            }

                //            cmd.ExecuteNonQuery();
                //            SqlDataAdapter da = new SqlDataAdapter(cmd);
                //            da.Fill(REGALIAS);


                //            con.Desconectar("DM");

                //            dataGridView1.DataSource = REGALIAS;

                //        }
                //        catch (SystemException exec)
                //        {
                //            MessageBoxButtons bt1 = MessageBoxButtons.OK;
                //            DialogResult result = MessageBox.Show(exec.ToString(), "!!!!ERROR!!!!", bt1, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                //            con.Desconectar("DM");

                //        }



                //    }
                //}


                //         if (radioButton3.Checked)
                //         {

                //             this.Refresh();
                //             if (Regalias_Vencido.tipo == "R")
                //             {
                //                 try
                //                 {


                //                     if (ds.Tables.Contains("REGALIAS"))
                //                     {
                //                         ds.Tables.Remove(REGALIAS);
                //                     }

                //                     REGALIAS.Clear();
                //                     REGALIAS.Columns.Clear();
                //                     con.conectar("DM");

                //                     SqlCommand cmd = new SqlCommand("[CORRECT].[REPORTE_DETALLE_REGALIA_NULAS]", con.condm);
                //                     cmd.CommandType = CommandType.StoredProcedure;
                //                     cmd.CommandTimeout = 50;
                //                     cmd.Parameters.AddWithValue("@fec_ini", Convert.ToDateTime(this.dateTimePicker1.Value.ToShortDateString()));
                //                     cmd.Parameters.AddWithValue("@fec_fin", Convert.ToDateTime(this.dateTimePicker2.Value.ToShortDateString()));
                //                     if (comboBox1.Text == "TODOS")
                //                     {
                //                         cmd.Parameters.AddWithValue("@usuario", null);
                //                     }
                //                     else
                //                     {

                //                         cmd.Parameters.AddWithValue("@usuario", comboBox1.Text);
                //                     }

                //                     cmd.ExecuteNonQuery();
                //                     SqlDataAdapter da = new SqlDataAdapter(cmd);
                //                     da.Fill(REGALIAS);


                //                     con.Desconectar("DM");

                //                     dataGridView1.DataSource = REGALIAS;
                //                 }
                //                 catch (SystemException exec)
                //                 {
                //                     MessageBoxButtons bt1 = MessageBoxButtons.OK;
                //                     DialogResult result = MessageBox.Show(exec.ToString(), "!!!!ERROR!!!!", bt1, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                //                     con.Desconectar("DM");

                //                 }



                //             }



                //         }
            }

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            REGALIAS.TableName = "REGALIAS";

            //ds.Tables.Add(REGALIAS);

            dgtoexcel(REGALIAS);
            
           // exp.aExcel(ds, dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridView1.RowCount >= 1)
            {
                comboBox1.Enabled = true;
                button2.Enabled = true;   

            }
                   
            
        }

        private void dgtoexcel( DataTable dt)
        {
             
            

          Excel.Application excell;
          Excel.Workbook workbook;
          Excel.Worksheet Sheet;
            object miobj = System.Reflection.Missing.Value;
            excell = new Excel.Application();
            excell.Visible = true;

            
            int incre;

            int Columnas, col;

            col = dt.Columns.Count / 26;

            string Letracol = "ABCDEFEHIJKLMNOPQRSTUVWXYZ";
            string Complementocol;
            //Determinando la letra que se usara despues de la columna 26
            if (col > 0)
            {
                Columnas = dt.Columns.Count - (26 * col);
                Complementocol = Letracol.ToString().Substring(col - 1, 1);
            }
            else
            {
                Columnas = dt.Columns.Count;
                Complementocol = "";
            }

            string ColumnaFinal;

            incre = Encoding.ASCII.GetBytes("A")[0];

            ColumnaFinal = Complementocol.ToString() + Convert.ToChar(incre + Columnas - 1).ToString();


            workbook = excell.Workbooks.Add(miobj);
            Sheet = workbook.Worksheets.get_Item(1);

            Excel.Range rg = Sheet.Cells[4, 1];
            Excel.Range Enc;
            Excel.Range RN;
            Excel.Range Report;
            rg.Select();




            for (int c = 0; c < dt.Columns.Count; c++)
            {

                Sheet.Cells[3, c + 1] = String.Format("{0}", dt.Columns[c].Caption);
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
                //"DETALLE " + "   DEL " + FechaIni.ToString("dd-MM-yyyy") + "  AL  " + FechaFin.ToString("dd-MM-yyyy") + " ";


                Sheet.Cells[2, 1] = "REPORTE DE REGALIAS " + " EMISION " + DateTime.Now.ToString();



                Report.Select();
                Report.Merge();
                Report.Font.Bold = true;
                Report.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;



                //ENCABEZDO DE COLUMNAS
                Enc = Sheet.get_Range("A3", ColumnaFinal + 3);
                Enc.Font.Name = "Times New Roman";
                Enc.Font.Size = 9;
                Enc.Borders.LineStyle = Excel.XlLineStyle.xlDouble;
                Enc.Font.Bold = true;

            }
            catch (SystemException exec)
            {
                MessageBoxButtons bt1 = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(exec.ToString(), "!!!!ERROR!!!!", bt1, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);


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

        private void button2_Click(object sender, EventArgs e)
        {
            copyall();

            backgroundWorker1.RunWorkerAsync();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            button2.Enabled = false;
        }


        private void agencias(int empres)
        {

            //coned.con.Open();
            //con.conectar("DM");
            //SqlCommand cm1 = new SqlCommand("SELECT [ID_SUCURSAL] ,[SUCURSAL]  FROM [DM].[CORRECT].[SUCURSALES_EXATUS]", con.condm);
            //SqlDataAdapter da = new SqlDataAdapter(cm1);

            //da.Fill(agencia);



            //con.Desconectar("DM");

            agencia.Clear();
            con.conectar("DM");
            SqlCommand cmd2 = new SqlCommand("SELECT [ID_SUCURSAL] ,[EMPRESA_EXACTUS],[SUCURSAL],[COD_BOD],[COD_RUTA],[CONSE_REGALIA]  FROM [DM].[CORRECT].[SUCURSALES_EXATUS] WHERE EMPRESA_EXACTUS = '" + empres + "'", con.condm);
            SqlDataAdapter da = new SqlDataAdapter(cmd2);
            da.Fill(agencia);

            con.Desconectar("DM");

            if (comboBox2.Items.Count > 1)
            {
                comboBox2.Items.Clear();
            }


            combo(agencia, "SUCURSAL", comboBox2);

        }

        public void combo(DataTable dts, string parametro, ComboBox cbx)
        {
            //toolStripComboBox1.Items.Clear();
            cbx.Items.Clear();
            cbx.Items.Add("TODOS");

            var result = from row in dts.AsEnumerable()
                         group row by row.Field<string>(parametro) into grp
                         select new
                         {
                             familia = grp.Key,

                         };
            foreach (var t in result)
            {
                if (t.familia == null || t.familia == "")
                {

                }
                else
                {
                    cbx.Items.Add(t.familia);
                    

                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text != "TODOS")
            {
                var results = from myRow in agencia.AsEnumerable()
                              where myRow.Field<string>("SUCURSAL") == comboBox2.Text

                              select new
                              {
                                  Nombre = myRow.Field<string>("CONSE_REGALIA")
                              };

                foreach (var rs1 in results)
                {
                    conse_reg = rs1.Nombre;
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
                if (comboBox1.Text == "TODOS")
                {
                if (REGALIAS.Rows.Count > 0)
                {
                    REGALIAS.DefaultView.RowFilter = string.Format("Convert(USUARIO,'System.String') like '%%'", this.comboBox1.Text);
                    dataGridView1.DataSource = REGALIAS;
                }
                }
                else
                {
                if (REGALIAS.Rows.Count > 0)
                {
                    REGALIAS.DefaultView.RowFilter = string.Format("Convert(USUARIO,'System.String') like '%{0}%'", this.comboBox1.Text);
                    dataGridView1.DataSource = REGALIAS;
                }
                }
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
