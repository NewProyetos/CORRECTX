using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Sinconizacion_EXactus
{
    public partial class Menu_Evaluaciones : Form
    {
        public Menu_Evaluaciones()
        {
            InitializeComponent();
        }
        public MenuItem VER = new MenuItem("VER:");
        public static String ID_EMPLEADO;
        public static String Departamento;
        public static String Cargo;
        public static String Nombre_Empleado;
        Int32 trimestre;
        public static Int32 Semestre_Actual;
        Int32 Semestre_evaluacion;
        Int32 Evaluacion;
        public static String Estado;
        conexionXML con = new conexionXML();
        private ContextMenu menugrid = new ContextMenu();
        DataTable evaluaciones = new DataTable();
        public static Int32 ID_EVALUACION;
        public static Int32 Tipo_Consulta;

        private void Menu_Evaluaciones_Load(object sender, EventArgs e)
        {
            checkBox1.Hide();
            button2.Hide();
            toolStripButton1.Enabled = false;
            toolStripButton2.Enabled = false;
            toolStripButton3.Enabled = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AllowUserToAddRows = false;



            
      



            textBox2.AutoCompleteCustomSource = AutocompleteRuta.AutocompleteEMPLEADOCOD();
            textBox2.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;

            textBox1.AutoCompleteCustomSource = AutocompleteRuta.AutocompleteEMPLEADONOMBRE();
            textBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;

         trimestre = (DateTime.Now.Month - 1) / 3 + 1;


         if (trimestre >= 1 && trimestre <= 2)
         {
             Semestre_Actual = 1;
         }
         else
             if (trimestre >= 3 && trimestre <= 4)
         {
             toolStripButton1.Enabled = false;             
             //Semestre_Actual = 2;
         }

// quitar despues de la  primera evaluacion....
         //Semestre_Actual = 1;
        // trimestre = 1;

        }

        public void imagen_Empresa(PictureBox imagenes)
        {
            if(Main_Menu.EMPRESA == "DISMO")
            {
                imagenes.Image = Properties.Resources.DM;
            }

            else if(Main_Menu.EMPRESA == "CV+")
            {
            
            }

            imagenes.Refresh();
            imagenes.Visible = true;
        
        }

        private void PRELIMINAR(Object sender, System.EventArgs e)
        {
            Tipo_Consulta = 3;
        //    Evaluacion_Desempeño ev = new Evaluacion_Desempeño(3,ID_EVALUACION);
          //  ev.ShowDialog();
        }

        private void FINAL(Object sender, System.EventArgs e)
        {
            Tipo_Consulta = 4;
           // Evaluacion_Desempeño ev = new Evaluacion_Desempeño(4,ID_EVALUACION);
          //  ev.ShowDialog();
        }

        private void OBJETIVOS(Object sender, System.EventArgs e)
        {
            Tipo_Consulta = 0;
         //   Evaluacion_Desempeño ev = new Evaluacion_Desempeño(0,ID_EVALUACION);
          //  ev.ShowDialog();
        
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {

            button2.Hide();
            con.conectar("EX");
            SqlCommand cm1 = new SqlCommand("SELECT  A.[EMPLEADO],A.[NOMBRE],B.DESCRIPCION as PUESTO ,C.DESCRIPCION as DEPARTAMENTO FROM [EXACTUS].[dismo].[EMPLEADO] A  INNER JOIN [EXACTUS].[dismo].[PUESTO] B  ON A.PUESTO = B.PUESTO INNER JOIN  [EXACTUS].[dismo].[DEPARTAMENTO] C  on A.DEPARTAMENTO = C.DEPARTAMENTO  where A.EMPLEADO = '" + textBox2.Text + "'", con.conex);
            SqlDataReader dr1 = cm1.ExecuteReader();
            while (dr1.Read())
            {
                Nombre_Empleado = Convert.ToString(dr1["NOMBRE"]);
                ID_EMPLEADO = Convert.ToString(dr1["EMPLEADO"]);
                Cargo = Convert.ToString(dr1["PUESTO"]);
                Departamento = Convert.ToString(dr1["DEPARTAMENTO"]);

                textBox1.Text = Nombre_Empleado;
                Cargadatos();

            }
            con.Desconectar("EX");

           


            
        }

        public  void textBox1_Leave(object sender, EventArgs e)
        {
            con.conectar("EX");
            SqlCommand cm1 = new SqlCommand("SELECT  A.[EMPLEADO],A.[NOMBRE],B.DESCRIPCION as PUESTO ,C.DESCRIPCION as DEPARTAMENTO FROM [EXACTUS].[dismo].[EMPLEADO] A  INNER JOIN [EXACTUS].[dismo].[PUESTO] B  ON A.PUESTO = B.PUESTO INNER JOIN  [EXACTUS].[dismo].[DEPARTAMENTO] C  on A.DEPARTAMENTO = C.DEPARTAMENTO  where A.NOMBRE = '" + textBox1.Text + "'", con.conex);
            SqlDataReader dr1 = cm1.ExecuteReader();
            while (dr1.Read())
            {
                Nombre_Empleado = Convert.ToString(dr1["NOMBRE"]);
                ID_EMPLEADO = Convert.ToString(dr1["EMPLEADO"]);
                Cargo = Convert.ToString(dr1["PUESTO"]);
                Departamento = Convert.ToString(dr1["DEPARTAMENTO"]);

                    textBox2.Text = ID_EMPLEADO;
                  
                Cargadatos();
            }
            con.Desconectar("EX");

         
        }

        public void Cargadatos()
        {
            DataTable dt = AutocompleteRuta.EMPLEADOS_CODIGO();

            DataRow[] foundID = dt.Select("SUBORDINADO = '" + ID_EMPLEADO + "'");
                 if (foundID.Length != 0)
                 {

                     evaluaciones.Clear();

                     dataGridView1.Refresh();
                     con.conectar("DM");

                     dataGridView1.DataSource = evaluaciones;

                     SqlCommand cmd = new SqlCommand("SELECT  [ID_EVALUACION] as 'EVALUACION NUMERO',[ESTADO_EVALUACION] as 'ESTADO',[COD_EMPLEADO] as 'CODIGO EMPLEADO' ,[NOMBRE] ,[DEPARTAMENTO],[CARGO] as 'PUESTO',[JEFE_INMEDIATO]  ,[SEMESTRE_INGRESO] as 'SEMESTRE',[FECHA_INGRESO] as 'Fecha de Evaluacion' FROM [DM].[CORRECT].[EVALUACION_DESEMPEÑO] WHERE COD_EMPLEADO = '" + ID_EMPLEADO + "'", con.condm);
                     SqlDataAdapter da = new SqlDataAdapter(cmd);
                     da.Fill(evaluaciones);

                     dataGridView1.DataSource = evaluaciones;
                     dataGridView1.Refresh();


                     con.Desconectar("DM");
        
                 }
                 else
                 {
                    // MessageBox.Show("No tiene Acceso a Ingresar Evaluaciones a este Empleado","!!!Alerta!!!!",MessageBoxIcon.Stop) ;
                 }

            
         
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            int fila = dataGridView1.RowCount  ;

            // pone color  al texto de la celda ------------------------------
            for (int i = 0; i < fila; i++)
            {


                if (dataGridView1[1, i].Value.ToString() == "Cerrado")
                {
                    dataGridView1[1, i].Style.ForeColor = Color.Red;
                }

                else if (dataGridView1[1, i].Value.ToString() == "Abierta")
                {
                    dataGridView1[1, i].Style.ForeColor = Color.Green;
                    toolStripButton1.Enabled = false;
                }
                else if (dataGridView1[1, i].Value.ToString() == "Calificada")
                {
                    dataGridView1[1, i].Style.ForeColor = Color.DarkOrange;
                    toolStripButton1.Enabled = false;
                }

            }
// valida si no exisiste una Evaluacion  
            if (fila < 1)
            {
                checkBox1.Show();
            }
            else
            {
                checkBox1.Hide();
            }

//    Habilita para ingresar Nuevas Evaluaciones segun el trimestre actual 
            
                if (trimestre >= 1 && trimestre <= 2)
                {
                    if (dataGridView1.RowCount == 0)
                    {
                        toolStripButton1.Enabled = true;
                    }
                }
                else
                    if (trimestre >= 3 && trimestre <= 4)
                    {
                        toolStripButton1.Enabled = false;

                    }
            
            if (dataGridView1.RowCount > 0)
            {
                dataGridView1.Enabled = true;
            }
            else
            {
                dataGridView1.Enabled = false;
            }

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row1 = dataGridView1.CurrentRow;
            Semestre_evaluacion = Convert.ToInt32(row1.Cells["SEMESTRE"].Value);
            Evaluacion = Convert.ToInt32(row1.Cells["EVALUACION NUMERO"].Value);
          Estado = Convert.ToString(row1.Cells["ESTADO"].Value);
          ID_EVALUACION = Convert.ToInt32(row1.Cells["EVALUACION NUMERO"].Value);

          button2.Show();

          VER.MenuItems.Clear();     

          if (Estado == "Abierta" )
          {
              toolStripButton2.Enabled = true;
              VER.MenuItems.Add(new MenuItem("OBJETIVOS", new System.EventHandler(this.OBJETIVOS)));
          }
          else
              if (Estado == "Calificada" )
              {
                  toolStripButton1.Enabled = false;
                  toolStripButton3.Enabled = true;
                  toolStripButton2.Enabled = false;
                  VER.MenuItems.Add(new MenuItem("OBJETIVOS", new System.EventHandler(this.OBJETIVOS)));
                  VER.MenuItems.Add(new MenuItem("PRELIMINAR", new System.EventHandler(this.PRELIMINAR)));
              }
              else
                  if (Estado == "Cerrada")
                  {
                      toolStripButton3.Enabled = false;
                      toolStripButton2.Enabled = false;
                      VER.MenuItems.Add(new MenuItem("OBJETIVOS", new System.EventHandler(this.OBJETIVOS)));
                      VER.MenuItems.Add(new MenuItem("PRELIMINAR", new System.EventHandler(this.PRELIMINAR)));
                      VER.MenuItems.Add(new MenuItem("FINAL", new System.EventHandler(this.FINAL)));
                  }

          menugrid.MenuItems.AddRange(new MenuItem[] { VER });
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
           
            Objetivos_Evaluacion objetivo = new Objetivos_Evaluacion(ID_EMPLEADO,2);

           objetivo.FormClosed += new System.Windows.Forms.FormClosedEventHandler(FormClosed);

            objetivo.ShowDialog();



        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) && string.IsNullOrWhiteSpace(textBox2.Text))
            {
                toolStripButton1.Enabled = false;
                dataGridView1.Enabled = false;
            }
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                textBox1.ReadOnly = false;
            }
            else
            {
                textBox1.ReadOnly = true;
            }
           

        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) && string.IsNullOrWhiteSpace(textBox2.Text))
            {
                toolStripButton1.Enabled = false;
                dataGridView1.Enabled = false;
            }
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox2.ReadOnly = false;
            }
            else
            {
                textBox2.ReadOnly = true;
            }
        }

        public void button1_Click(object sender, EventArgs e)
        {
            textBox2.ReadOnly = false;
            textBox1.ReadOnly = false;
            Cargadatos();
        }
// Evaluacion visor Objetivos
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
// Evaluacion Nota Preliminar
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Tipo_Consulta = 1;
            //Evaluacion_Desempeño ev = new Evaluacion_Desempeño(1,ID_EVALUACION);
            //ev.FormClosed += new System.Windows.Forms.FormClosedEventHandler(FormClosed);
            //ev.ShowDialog();
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo info;
            if (e.Button == MouseButtons.Right)
            {
                info = dataGridView1.HitTest(e.X, e.Y);
                if (info.Type == DataGridViewHitTestType.Cell)
                {
                    menugrid.Show(dataGridView1, new Point(e.X, e.Y));
                }


            }
        }

        private bool Exist_resultado_obj_prelimi(int ID_EVALUACION)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("select COUNT (*) from [DM].[CORRECT].[RESULTADOS_EVALUACION_PRELIMINAR] where ID_EVALUACION = @ID_EVALUACION ", con.condm);
            cmd.Parameters.AddWithValue("ID_EVALUACION", ID_EVALUACION);

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

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Tipo_Consulta = 2;
           // Evaluacion_Desempeño ev = new Evaluacion_Desempeño(2,ID_EVALUACION);
            //ev.FormClosed += new System.Windows.Forms.FormClosedEventHandler(FormClosed);
            //ev.ShowDialog();
        }

        private void textBox2_MouseClick(object sender, MouseEventArgs e)
        {
          
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void FormClosed(object sender, FormClosedEventArgs e)
        {
            toolStripButton1.Enabled = false;
            toolStripButton2.Enabled = false;
            toolStripButton3.Enabled = false;
            Cargadatos();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                toolStripButton1.Enabled = true;
            }
            else
            {
                toolStripButton1.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CORECTX_APP.RRHH.Reporte_Ingreso rp = new CORECTX_APP.RRHH.Reporte_Ingreso(ID_EVALUACION,Estado);
            rp.ShowDialog();

        }

    }
}
