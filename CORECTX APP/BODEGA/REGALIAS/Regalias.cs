using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
//using Solicitud_Regalia.REGALIAS;

namespace Sinconizacion_EXactus.CORECTX_APP.BODEGA.REGALIAS
{
    public partial class Regalias : Form
    {
        public Regalias()
        {
            InitializeComponent();
        }
        conexionXML con = new conexionXML();
        DateTimePicker datePickerini = new DateTimePicker();
        DateTimePicker datePickerfin = new DateTimePicker();
        ToolStripButton btupdate = new ToolStripButton();
        ToolStripButton btremover = new ToolStripButton();
        ToolStripButton btrecibir = new ToolStripButton();
        // ToolStripButton btexcel = new ToolStripButton();
        ToolStripTextBox tbxcomentario = new ToolStripTextBox();
        DataTable dt = new DataTable();
        DataTable agencias = new DataTable();
        string agencia_actual;
        public static string agecia_txt;


        string estador;
        public static string regalia;
        public static string traspaso;
        String usuario_rec;


        object O = global::Sinconizacion_EXactus.Properties.Resources.ResourceManager.GetObject("refreshpeq1");
        object e = global::Sinconizacion_EXactus.Properties.Resources.ResourceManager.GetObject("excelpeq");
        object t = global::Sinconizacion_EXactus.Properties.Resources.ResourceManager.GetObject("cancel");
        object r = global::Sinconizacion_EXactus.Properties.Resources.ResourceManager.GetObject("mas1");

        private void Regalias_Load(object sender, EventArgs e)
        {
           


            agencia();
           
                agencia_actual =Main_Menu.Agencia;
                combo_agecnia(comboBox2, agencias, "SUCURSAL");
                agecia_txt= age(agencias, Convert.ToInt32(agencia_actual));

                comboBox2.Text = agecia_txt;
                dataGridView1.Enabled = true;
                dataGridView1.RowHeadersVisible = false;
                dataGridView1.ReadOnly = true;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dataGridView1.AllowUserToAddRows = false;
                tbxcomentario.KeyPress += new KeyPressEventHandler(tbxcomentario_KeyPress);
                btupdate.Click += new EventHandler(btupdate_click);
                btremover.Click += new EventHandler(btremover_click);
                btrecibir.Click += new EventHandler(btrecibir_click);

                label2.Text = Login.usuario.ToUpper();
                // label4.Text = Main_Menu.Agencia.ToUpper();

                estador = "A";
                comboBox1.Text = "ABIERTA";
                cargaobjetostoostrip();
                catgadata(agencia_actual);
            if (Main_Menu.multisucursal == "S")
            {
                comboBox2.Enabled = true;

            }
            else
            {
                comboBox2.Enabled = false;
            }



        }

        private void agencia()
        {
            agencias.Clear();
            con.conectar("DM");
            SqlCommand cmd2 = new SqlCommand("SELECT [ID_SUCURSAL] ,[EMPRESA_EXACTUS],[SUCURSAL],[COD_BOD],[COD_RUTA]  FROM [DM].[CORRECT].[SUCURSALES_EXATUS] WHERE EMPRESA_EXACTUS = '" + Login.empresa_id + "'", con.condm);
            SqlDataAdapter da = new SqlDataAdapter(cmd2);
            da.Fill(agencias);

            con.Desconectar("DM");

        }

        public void combo_agecnia(ComboBox cb, DataTable dts, string parametro)
        {
            cb.Items.Clear();

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
                    cb.Items.Add(t.familia);

                }
            }
        }


        private string age(DataTable dts, int parametro)
        {
            string agena = "";

            var results = from myRow in dts.AsEnumerable()
                          where myRow.Field<int>("ID_SUCURSAL") == parametro

                          select new
                          {
                              Nombre = myRow.Field<string>("SUCURSAL")
                          };

            foreach (var rs1 in results)
            {
                agena = rs1.Nombre.ToUpper();
            }
            return agena;
        }
    

        private void btrecibir_click(object sender, EventArgs e)
        {
            MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("SE RECIBIRA REGALIA No.: " + regalia + " DESEA APLICAR LA RECEPCION DE DOCUMENTO ", "RECIBIR", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                recivir_regalia(regalia);

            }
            
        }

        private void btupdate_click(object sender, EventArgs e)
        {
            catgadata(agencia_actual);
        }


        

        private void cargaobjetostoostrip()
        {




            DateTime DT = DateTime.Now;




            datePickerini.Format = DateTimePickerFormat.Short;
            datePickerini.Value = new DateTime(DT.Year, DT.Month, 1);
            datePickerini.Size = new Size(120, 20);


            toolStrip1.Items.Add(new ToolStripControlHost(datePickerini));

            Label lblfechafin = new Label();
            lblfechafin.Text = "Fecha fin";

            toolStrip1.Items.Add(new ToolStripControlHost(lblfechafin));

            datePickerfin.Format = DateTimePickerFormat.Short;
            datePickerfin.Size = new Size(120, 20);


            toolStrip1.Items.Add(new ToolStripControlHost(datePickerfin));

            ToolStripSeparator sep1 = new ToolStripSeparator();
            toolStrip1.Items.Add(sep1);




            btupdate.Text = "Refrescar";
            btupdate.Image = (Image)O;
            toolStrip1.Items.Add(btupdate);
            ToolStripSeparator sep2 = new ToolStripSeparator();
            toolStrip1.Items.Add(sep2);

            Label buscarlb = new Label();
            buscarlb.Text = "BUSCAR";

            toolStrip1.Items.Add(new ToolStripControlHost(buscarlb));

            ToolStripSeparator sep3 = new ToolStripSeparator();
            toolStrip1.Items.Add(sep3);


          

            Label comentlb = new Label();
            comentlb.Text = "Comentario:";



            tbxcomentario.Size = new Size(120, 25);
            toolStrip1.Items.Add(tbxcomentario);

            btrecibir.Text = "Recibir";
            btrecibir.Image = (Image)r;
            btrecibir.TextImageRelation = TextImageRelation.ImageBeforeText;
            toolStrip1.Items.Add(btrecibir);
            btrecibir.Enabled = false;

            //btexcel.Text = "Excel";
            //btexcel.Image = (Image)e;
            //toolStrip1.Items.Add(btexcel);


        }

        private void catgadata(string agencia)
        {
            switch (comboBox1.Text)
            {
                case "ABIERTA":
                    estador = "A";
                        break;
                case "ANULADA":
                    estador = "N";
                    break;
                case "PROCESADA":
                    estador = "L";
                    break;
                case "RECIBIDA":
                    estador = "R";
                    break;

                default:
                    estador = "A";
                    break;
            }



            dt.Clear();

            con.conectar("DM");
            SqlCommand  cmd2 = new SqlCommand("SELECT [ID],[ESTADO],[NUM_REG],[RUTA],[VENDEDOR],[CANTIDAD],[FECHA_CREA] ,[COMENTARIO],[FECHA_RECIBIDO_BOD] as FECHA_RECIBIDO,[USUARIO_RECIBIO] ,[REGALIA],FECHA_APLIC,USUARIO_APLICA FROM [DM].[CORRECT].[REGALIAS_SOLICI_ENC] as ENC LEFT JOIN [DM].[CORRECT].[USUARIOS] as USR on ENC.RUTA = CASE USR.PUESTO WHEN 'VEN' THEN REPLACE(USR.USUARIO,'P','R') ELSE USR.USUARIO END  where  ESTADO = '" + estador + "' and (DATEADD(dd, 0, DATEDIFF(dd, 0, FECHA_CREA )) >= '" + datePickerini.Value.ToString("yyyy-MM-dd") + "') AND (DATEADD(dd, 0, DATEDIFF(dd, 0, FECHA_CREA )) <= '" + datePickerfin.Value.ToString("yyyy-MM-dd") + "') and USR.EMPRESA = '"+Login.empresa_id+"' and USR.AGENCIA = '"+ agencia + "'", con.condm);
            SqlDataAdapter da = new SqlDataAdapter(cmd2);
            da.Fill(dt);

            dataGridView1.DataSource = dt;
            dataGridView1.Refresh();

            combo(dt);
            con.Desconectar("DM");

        }

        public void combo(DataTable dts)
        {
            toolStripComboBox1.Items.Clear();

            var result = from row in dts.AsEnumerable()
                         group row by row.Field<string>("RUTA") into grp
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
                    toolStripComboBox1.Items.Add(t.familia);
                }
            }
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dt.DefaultView.RowFilter = string.Format("Convert(RUTA,'System.String') like '%{0}%'", this.toolStripComboBox1.Text);
            dataGridView1.DataSource = dt;
        }
        private void tbxcomentario_KeyPress(object O, KeyPressEventArgs e)
        {
            dt.DefaultView.RowFilter = string.Format("Convert(NUM_REG,'System.String') like '%{0}%'", tbxcomentario.Text);
            dataGridView1.DataSource = dt;

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Main_Menu.Puesto == "BOD" || Main_Menu.Puesto == "ADMIN")
            {
                int idx = dataGridView1.CurrentRow.Index;
                regalia = Convert.ToString(dataGridView1.Rows[idx].Cells[2].Value);
                string rstado = Convert.ToString(dataGridView1.Rows[idx].Cells[1].Value);

                if (rstado == "R")
                {
                    Traspasos tras = new Traspasos();
                    tras.Show();
                }
                else
                {
                    MessageBox.Show("EL ESTADO DE LA REGALIA DEBE SER EN RECIBIDA PARA PODER SER LIQUIDADA","ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);

                }

            }
            else
            {

            }

        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //int fila = dataGridView1.RowCount;


            //for (int i = 0; i < fila; i++)
            //{

            //    if (dataGridView1[1, i].Value.ToString() == "N")
            //    {
            //        dataGridView1[1, i].Style.ForeColor = Color.Red;
            //    }

            //    else if (dataGridView1[1, i].Value.ToString() == "A")
            //    {
            //        dataGridView1[1, i].Style.ForeColor = Color.Green;
            //    }
            //    else if (dataGridView1[1, i].Value.ToString() == "P")
            //    {
            //        dataGridView1[1, i].Style.ForeColor = Color.DarkOrange;
            //    }
            //}
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "PROCESADA")
            {

                btremover.Text = "quitar";
                btremover.Image = (Image)t;
                toolStrip1.Items.Add(btremover);

            }
            else
            {
                if (toolStrip1.Items.Contains(btremover))
                {
                    toolStrip1.Items.Remove(btremover);
                }

            }
            
            catgadata(agencia_actual);
        }
        private void abrir_poryec()
        {
          
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

           


        }

       

        private void desvicular_regalia(string trasspaso_anular, string regalia_anular)
        {
            string fecha_aplicacion = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            con.conectar("DM");
            SqlCommand cmd8 = new SqlCommand();
            cmd8.Connection = con.condm;
            cmd8.CommandText = "UPDATE [DM].[CORRECT].[REGALIAS_SOLICI_ENC] SET UPDATE_USER= '" + Login.usuario.ToUpper() + "' ,DATE_UPDATE='" + fecha_aplicacion + "',REGALIA = '',ESTADO = 'R'   where NUM_REG ='" + regalia_anular + "'";
            // cmd8.Parameters.Add("@UPDATE_USER", SqlDbType.NVarChar).Value = Login.usuario.ToUpper();
            //cmd8.Parameters.Add("@DATE_UPDATE", SqlDbType.NVarChar).Value = fecha_aplicacion;
            //cmd8.Parameters.Add("@REGALIA", SqlDbType.NVarChar).Value = trasspaso_anular;
            //cmd8.Parameters.Add("@ESTADO", SqlDbType.Char).Value = 'A';
            cmd8.ExecuteNonQuery();
            con.Desconectar("DM");


            con.conectar("EX");
            SqlCommand cmd9 = new SqlCommand();
            cmd9.Connection = con.conex;
            cmd9.CommandText = "UPDATE [EXACTUS].[dismo].[AUDIT_TRANS_INV] SET REFERENCIA = ''  where APLICACION = '" + trasspaso_anular + "'";
            cmd9.ExecuteNonQuery();
            con.Desconectar("EX");

        }

        private void btremover_click(object sender, EventArgs e)
        {
            MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("DESEA QUITAR EL TRASPASO :" + traspaso + " DE LA REGALIA: " + regalia + "", "QUITAR", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                desvicular_regalia(traspaso,regalia);

            }
        }

        private void regliasLiquidadasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CORECTX_APP.Reports.GeneralForms rep = new Reports.GeneralForms("Regalias Procesadas",2);
            rep.Show();

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int idx = dataGridView1.CurrentRow.Index;
            //regalia = Convert.ToString(dataGridView1.Rows[idx].Cells[2].Value);
            string rstado = Convert.ToString(dataGridView1.Rows[idx].Cells[1].Value);
           
            if (rstado == "L")
            {
                traspaso = Convert.ToString(dataGridView1.Rows[idx].Cells[10].Value);
                
            }
            regalia = Convert.ToString(dataGridView1.Rows[idx].Cells[2].Value);
            usuario_rec = Convert.ToString(dataGridView1.Rows[idx].Cells[9].Value);
        

            if (usuario_rec == "" || usuario_rec == null || usuario_rec == string.Empty)
            {
                if (regalia == "" || regalia == null || regalia == string.Empty)
                {

                }
                else
                {
                    btrecibir.Enabled = true;
                }
            }
            else
    
            {
                btrecibir.Enabled = false ;
            }
        }
        public void recivir_regalia(string regalia_recibir)
        {
            string fecha_recibido = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            con.conectar("DM");
            SqlCommand cmd8 = new SqlCommand();
            cmd8.Connection = con.condm;
            cmd8.CommandText = "UPDATE [DM].[CORRECT].[REGALIAS_SOLICI_ENC] SET USUARIO_RECIBIO= '" + Login.usuario.ToUpper() + "' ,FECHA_RECIBIDO_BOD='" + fecha_recibido + "' , ESTADO = 'R' where NUM_REG ='" + regalia_recibir + "'";
           
            cmd8.ExecuteNonQuery();
            con.Desconectar("DM");
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var results = from myRow in agencias.AsEnumerable()
                          where myRow.Field<string>("SUCURSAL") == comboBox2.Text

                          select new
                          {
                              Nombre = myRow.Field<int>("ID_SUCURSAL")
                          };

            foreach (var rs1 in results)
            {
                agencia_actual = Convert.ToString(rs1.Nombre);
            }

            catgadata(agencia_actual);
            agecia_txt = age(agencias, Convert.ToInt32(agencia_actual));
        }

        private void regalisLiquidadasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reporte_Regalia regrep = new Reporte_Regalia("Nuevo");
            regrep.ShowDialog();

        }
    }
}
