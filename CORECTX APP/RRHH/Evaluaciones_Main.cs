using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors;

namespace Sinconizacion_EXactus.CORECTX_APP.RRHH
{
    public partial class Evaluaciones_Main : Form
    {
        public Evaluaciones_Main()
        {
            InitializeComponent();
        }
        string estado;
        string COD_EMPLEADO;
        string NOMBRE_EMPLEADO;
        DataTable amonestaciones = new DataTable();
        conexionXML con = new conexionXML();
        public static int Tipo_Consulta;
        int ID_EVALUACION;
        DataTable dt2 = new DataTable();
        public static DataTable pendiente = new DataTable();
        int tipo_consult;
        string año;
        int tipo_consul_bono;

        private void groupControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
           // Tipo_Consulta = 1;
            Evaluacion_Desempeño ev = new Evaluacion_Desempeño(1,ID_EVALUACION,año,Main_Menu.COD_EMPLEADO);
            ev.FormClosed += new System.Windows.Forms.FormClosedEventHandler(FormClosed);
            ev.ShowDialog();
        }

        private void Evaluaciones_Main_Load(object sender, EventArgs e)
        {
            // oculto el gripo del bono
            ribbonPageGroup6.Visible = false;
            linkLabel2.Hide();
            repositoryItemComboBox3.SelectedIndexChanged += new EventHandler(repositoryItemComboBox3_EditValueChanged);

            repositoryItemComboBox4.SelectedIndexChanged += new EventHandler(repositoryItemComboBox4_EditValueChanged);

            checkEdit1.Checked = true;
            año = DateTime.Now.Year.ToString();
            barEditItem7.EditValue = año;
            
            if (Main_Menu.TIPO_RRHH == "ADMIN")

            {
                // tipo_consult = 1;

            }
            else
            {

                checkEdit1.Hide();
            }
            tipo_consult = 2;
                 //  evapendiente();

            if (pendiente.Rows.Count >= 1)
            {
              //  ribbonGalleryBarItem3.Glyph = new Bitmap(Properties.Resources.show_32x32);
               // hyperlinkLabelControl1.Text = Convert.ToString(pendiente.Rows.Count)+ "  Empleados Pendientes de Evaluar";
            }
            else
            {
             //   ribbonGalleryBarItem3.Glyph = new Bitmap(Properties.Resources.hide_32x32);
            }

            //gridView1.OptionsView.ColumnAutoWidth = false;
            // gridView1.OptionsView.RowAutoHeight = true;

            DataTable dt = new DataTable();
            con.conectar("DM");
            
            SqlCommand cm1s = new SqlCommand("SELECT DATEPART(YEAR,[FECHA_INGRESO]) as AN FROM [DM].[CORRECT].[EVALUACION_DESEMPEÑO] Group by DATEPART(YEAR,[FECHA_INGRESO])", con.condm);
            SqlDataAdapter da = new SqlDataAdapter(cm1s);
            da.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow drw = dt.Rows[i];


                repositoryItemComboBox3.Items.Add(drw["AN"]);
            }

            repositoryItemComboBox3.Items.Add(año);
            //SqlDataReader dr1s = cm1s.ExecuteReader();
            //while (dr1s.Read())
            //{
            //    repositoryItemComboBox3.Items.Add(dr1s["AN"]);

            //}
            //dr1s.Close();


            con.Desconectar("DM");


            barButtonItem8.LargeGlyph = new Bitmap(Properties.Resources.filter_32x32);
         //   barButtonItem12.LargeGlyph = new Bitmap(Properties.Resources.currency_32x32);


            gridView1.OptionsBehavior.Editable = false;
            gridView1.BestFitColumns();
           
            barButtonItem1.Enabled = false;

           
            barButtonItem2.Enabled = false;
            barButtonItem3.Enabled = false;
            barButtonItem12.Enabled = false;
            barButtonItem10.Enabled = false;
            evaluar_pendiente();
            cargagrid();

            
        }
        private void repositoryItemComboBox3_EditValueChanged(object sender, EventArgs e)
        {
            ComboBoxEdit edit = (ComboBoxEdit)sender;
            object editValue = edit.EditValue;

            año = Convert.ToString(editValue);
            evaluar_pendiente();
            cargagrid();

        }

        private void repositoryItemComboBox4_EditValueChanged(object sender, EventArgs e)
        {
            ComboBoxEdit edit = (ComboBoxEdit)sender;
            object editValue = edit.EditValue;

            año = Convert.ToString(editValue);
           


        }


        private void evaluar_pendiente()
        {
            evapendiente();
            int noevaluados = pendiente.Rows.Count;
            if (noevaluados >= 1)
            {
                
                barButtonItem5.Enabled = true;
                linkLabel1.Text = Convert.ToString(noevaluados) + " Evaluaciones Pendientes";
            }
            else
            {
                barButtonItem5.Enabled = false;
                linkLabel1.Text = "";
            }
        }

        private DataTable datos()
        {
            if (tipo_consult == 1)
            {
                dt2.Clear();

                //  Conexion2 coned = new Conexion2();
                con.conectar("DM");

                string consulta = "SELECT  [ID_EVALUACION] as 'EVALUACION NUMERO',[ESTADO_EVALUACION] as 'ESTADO',EV.[COD_EMPLEADO] as 'CODIGO EMPLEADO' ,EV.[NOMBRE] ,EV.[DEPARTAMENTO],[CARGO] as 'PUESTO',[JEFE_INMEDIATO]  ,[SEMESTRE_INGRESO] as 'SEMESTRE',EV.[FECHA_INGRESO] as 'Fecha de Evaluacion',EV.CORP FROM [DM].[CORRECT].[EVALUACION_DESEMPEÑO] as EV LEFT JOIN [EXACTUS].[dismo].[EMPLEADO] as  EM  on EV.COD_EMPLEADO = EM.EMPLEADO INNER JOIN [EXACTUS].[dismo].[EMPLEADO_JERARQUIA]  as EJ on EJ.SUBORDINADO = EM.EMPLEADO  INNER JOIN [DM].[CORRECT].[USUARIOS] as DMUSER  ON EJ.SUPERIOR = DMUSER.COD_EMPLEADO where DATEPART(YEAR,EV.FECHA_INGRESO) = '" + año+"'";
                SqlCommand comando = new SqlCommand(consulta, con.condm);

                SqlDataAdapter adap = new SqlDataAdapter(comando);

                adap.Fill(dt2);
                con.Desconectar("DM");
                return dt2;

            }
            else
            {

                dt2.Clear();

                con.conectar("DM");

                string consulta = "SELECT  [ID_EVALUACION] as 'EVALUACION NUMERO',[ESTADO_EVALUACION] as 'ESTADO',EV.[COD_EMPLEADO] as 'CODIGO EMPLEADO' ,EV.[NOMBRE] ,EV.[DEPARTAMENTO],[CARGO] as 'PUESTO',[JEFE_INMEDIATO]  ,[SEMESTRE_INGRESO] as 'SEMESTRE',EV.[FECHA_INGRESO] as 'Fecha de Evaluacion',EV.CORP FROM [DM].[CORRECT].[EVALUACION_DESEMPEÑO] as EV LEFT JOIN [EXACTUS].[dismo].[EMPLEADO] as  EM  on EV.COD_EMPLEADO = EM.EMPLEADO INNER JOIN [EXACTUS].[dismo].[EMPLEADO_JERARQUIA]  as EJ on EJ.SUBORDINADO = EM.EMPLEADO  INNER JOIN [DM].[CORRECT].[USUARIOS] as DMUSER  ON EJ.SUPERIOR = DMUSER.COD_EMPLEADO where DMUSER.COD_EMPLEADO = '" + Main_Menu.COD_EMPLEADO+ "' and DATEPART(YEAR,EV.FECHA_INGRESO) = '"+año+"'";
                SqlCommand comando = new SqlCommand(consulta, con.condm);

                SqlDataAdapter adap = new SqlDataAdapter(comando);

                adap.Fill(dt2);
                con.Desconectar("DM");
                return dt2;
            }

            
        }

        private void gridView1_RowCellClick_1(object sender, RowCellClickEventArgs e)
        {
            foreach (int i in gridView1.GetSelectedRows())
            {
                DataRow row = gridView1.GetDataRow(i);

              estado = Convert.ToString(row[1]);
                ID_EVALUACION = Convert.ToInt32(row[0]);
                COD_EMPLEADO = Convert.ToString(row[2]);
                NOMBRE_EMPLEADO = Convert.ToString(row[3]);

                amonestaciones_load(COD_EMPLEADO);

                if (estado == "Abierta")
                {
                    barButtonItem1.Enabled = false;
                    barButtonItem2.Enabled = true;
                    barButtonItem3.Enabled = false;
                    barButtonItem10.Enabled = true;
                    barButtonItem12.Enabled = false;
                }
                else if (estado == "Calificada")
                {
                    barButtonItem1.Enabled = false;
                    barButtonItem2.Enabled = false;
                    barButtonItem3.Enabled = true;
                    barButtonItem10.Enabled = true;
                    barButtonItem12.Enabled = false;
                }
                else if (estado == "Cerrada")
                {
                    barButtonItem10.Enabled = true;
                    barButtonItem2.Enabled = false;
                    barButtonItem3.Enabled = false;
                    barButtonItem12.Enabled = true;
                    if (Evaluaciones_pendiente(COD_EMPLEADO))
                    {
                        barButtonItem1.Enabled = false;
                    }
                    else
                    {
                        barButtonItem1.Enabled = true;
                    }

                }
                else
                {
                    barButtonItem1.Enabled = false;
                    barButtonItem2.Enabled = false;
                    barButtonItem3.Enabled = false;
                    barButtonItem10.Enabled = false;
                    barButtonItem12.Enabled = false;
                }

                }
        }

        private void amonestaciones_load(string cod_emp)
        {
            con.conectar("EX");

            string consulta = "SELECT TIPO_ACCION,NOTAS,FECHA_RIGE FROM [EXACTUS].[dismo].[EMPLEADO_ACC_PER] where EMPLEADO = '" + cod_emp + "' and (TIPO_ACCION like 'AGRA' OR  TIPO_ACCION like 'ALEV') and ESTADO_ACCION = 'S' ";
            SqlCommand comando = new SqlCommand(consulta, con.conex);

            SqlDataAdapter adap = new SqlDataAdapter(comando);

            adap.Fill(amonestaciones);
            conteo_amonestaciones(amonestaciones);


            con.Desconectar("EX");
            
        }

        private void conteo_amonestaciones(DataTable dt)
        {
            int count_alev = dt.AsEnumerable()
              .Count(row => row.Field<string>("TIPO_ACCION") == "ALEV");

            int count_agra = dt.AsEnumerable()
               .Count(row => row.Field<string>("TIPO_ACCION") == "AGRA");

            if (count_alev > 0 || count_agra > 0)
            {
                linkLabel2.Show();
                linkLabel2.Text = "Empleado cuenta con :" + Convert.ToString(count_alev) + "Acciones Leves  " + Convert.ToString(count_agra) + " Acciones Grabes";
            }
            else
            {
                linkLabel2.Hide();
            }

        }

        private void barEditItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Objetivos_Evaluacion objetivo = new Objetivos_Evaluacion(COD_EMPLEADO,2);

            objetivo.FormClosed += new System.Windows.Forms.FormClosedEventHandler(FormClosed);

            objetivo.ShowDialog();
        }

        private void FormClosed(object sender, FormClosedEventArgs e)
        {
            evaluar_pendiente();

            barButtonItem1.Enabled = false;
            barButtonItem2.Enabled = false;
            barButtonItem3.Enabled = false;
            cargagrid();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //Tipo_Consulta = 2;
            Evaluacion_Desempeño ev = new Evaluacion_Desempeño(2,ID_EVALUACION, año, Main_Menu.COD_EMPLEADO);
            ev.FormClosed += new System.Windows.Forms.FormClosedEventHandler(FormClosed);
            ev.ShowDialog();
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CORECTX_APP.RRHH.Reporte_Ingreso rp = new CORECTX_APP.RRHH.Reporte_Ingreso(ID_EVALUACION,estado);
            rp.ShowDialog();
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string FileName = "C:\\CORRECT\\RRHH.xls";
            gridView1.ExportToXls(FileName);

            Microsoft.Office.Interop.Excel.Application excell;
            Microsoft.Office.Interop.Excel.Workbook workbook;
            excell = new Excel.Application();
            excell.Visible = true;
            workbook = excell.Workbooks.Open(FileName);
        }
        private void cargagrid()
        {
            gridControl1.DataSource = datos();
            gridControl1.Refresh();
        }
        private bool Evaluaciones_pendiente(string COD_EMP)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("SELECT COUNT (*)  FROM [DM].[CORRECT].[EVALUACION_DESEMPEÑO]  where COD_EMPLEADO = @COD_EMP and (ESTADO_EVALUACION = 'Abierta' or ESTADO_EVALUACION = 'Calificada')", con.condm);
            cmd.Parameters.AddWithValue("COD_EMP", COD_EMP);

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

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit1.Checked)
            {
                tipo_consult = 2;
                cargagrid();
                evaluar_pendiente();

            }
            else
            {
                tipo_consult = 1;
                cargagrid();
                evaluar_pendiente();
            }
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //Objetivos_Evaluacion objt = new Objetivos_Evaluacion(Main_Menu.COD_EMPLEADO, 1);
            //objt.ShowDialog();

            if (checkEdit1.Checked)
            {
                tipo_consult = 2;
            }
            else
            {
                tipo_consult = 1;
            }

            No_evaluados nev = new No_evaluados(tipo_consult);
            nev.Show();
            nev.FormClosed += new System.Windows.Forms.FormClosedEventHandler(FormClosed);
        }

        private void evapendiente()
        {
            pendiente.Clear();

            if (checkEdit1.Checked)
            {
                //  Conexion2 coned = new Conexion2();
                con.conectar("DM");

                string consulta = "SELECT JER.SUBORDINADO as CODIGO,EMP.NOMBRE FROM [EXACTUS].[dismo].[EMPLEADO_JERARQUIA] as JER  LEFT JOIN [EXACTUS].[dismo].[EMPLEADO] as EMP  on JER.SUBORDINADO = EMP.EMPLEADO   where EMP.ACTIVO = 'S' and JER.SUBORDINADO not in (SELECT COD_EMPLEADO FROM [DM].[CORRECT].[EVALUACION_DESEMPEÑO] where DATEPART(YEAR,FECHA_INGRESO) = '" + año + "' group by COD_EMPLEADO) and JER.SUPERIOR ='" + Main_Menu.COD_EMPLEADO + "'  Group by JER.SUPERIOR,EMP.NOMBRE,JER.SUBORDINADO ";
                SqlCommand comando = new SqlCommand(consulta, con.condm);

                SqlDataAdapter adap = new SqlDataAdapter(comando);

                adap.Fill(pendiente);
                con.Desconectar("DM");
            }
            else
            {
                con.conectar("DM");

                string consulta = "SELECT JER.SUBORDINADO as CODIGO,EMP.NOMBRE,SUP.NOMBRE as JEFE FROM [EXACTUS].[dismo].[EMPLEADO_JERARQUIA] as JER  LEFT JOIN [EXACTUS].[dismo].[EMPLEADO] as EMP  on JER.SUBORDINADO = EMP.EMPLEADO LEFT JOIN [EXACTUS].[dismo].[EMPLEADO] as SUP on JER.SUPERIOR = SUP.EMPLEADO   where EMP.ACTIVO = 'S' and JER.SUBORDINADO not in (SELECT COD_EMPLEADO FROM [DM].[CORRECT].[EVALUACION_DESEMPEÑO] where DATEPART(YEAR,FECHA_INGRESO) = '"+año+"' group by COD_EMPLEADO)  Group by JER.SUPERIOR,EMP.NOMBRE,JER.SUBORDINADO,SUP.NOMBRE ";
                SqlCommand comando = new SqlCommand(consulta, con.condm);

                SqlDataAdapter adap = new SqlDataAdapter(comando);

                adap.Fill(pendiente);
                con.Desconectar("DM");
            }
            

        }

        private void hyperlinkLabelControl1_Click(object sender, EventArgs e)
        {
            if (checkEdit1.Checked)
            {
                tipo_consult = 2;               
            }
            else
            {
                tipo_consult = 1;               
            }

            No_evaluados nev = new No_evaluados(tipo_consult);
            nev.Show();
        }

        private void ribbonStatusBar1_Click(object sender, EventArgs e)
        {

        }

        private void ribbonGalleryBarItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barEditItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridView1.OptionsView.ShowAutoFilterRow)
            {
                Image fil = new Bitmap(Properties.Resources.filter_32x32); 
                gridView1.OptionsView.ShowAutoFilterRow = false;
                barButtonItem8.LargeGlyph = fil;
                
            }
            else
            {
                Image fil = new Bitmap(Properties.Resources.clearfilter_32x32);
                gridView1.OptionsView.ShowAutoFilterRow = true;
                barButtonItem8.LargeGlyph = fil;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            barButtonItem5_ItemClick(null, null);
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (existe_bono(ID_EVALUACION))
            {
                tipo_consul_bono = 2; // mostara
            }
            else
            {
                tipo_consul_bono = 1; // crear 
            }

            Bono bn = new Bono(COD_EMPLEADO,ID_EVALUACION,tipo_consul_bono,NOMBRE_EMPLEADO);
            bn.Show();

        }

        private bool existe_bono(int ID)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("SELECT COUNT (*)  FROM [DM].[CORRECT].[EVALUACION_BONO] where ID_EVALUACION = @ID_EVALUACION ", con.condm);
            cmd.Parameters.AddWithValue("@ID_EVALUACION", ID);

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

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Amonestaciones amon = new Amonestaciones(amonestaciones);
            amon.ShowDialog();
        }
    }
}
