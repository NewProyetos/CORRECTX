using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.CSharp;
using Microsoft.Reporting.WinForms;

namespace Sinconizacion_EXactus.CORECTX_APP.RRHH
{
    public partial class Reporte_Ingreso : Form
    {
        public Reporte_Ingreso(int id,string Estado)
        {
            InitializeComponent();
            ID = id;
            estado = Estado;
        }
        conexionXML con = new conexionXML();
        Int32 ID;
        String estado;
        private void Reporte_Ingreso_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'recursos_Humanos.EVALUACION_CALIFICACION_FINAL' Puede moverla o quitarla según sea necesario.
         //   this.eVALUACION_CALIFICACION_FINALTableAdapter.Fill(this.recursos_Humanos.EVALUACION_CALIFICACION_FINAL);
            // TODO: esta línea de código carga datos en la tabla 'recursos_Humanos.EVALUACION_CALIFICACION_PRELIMINAR' Puede moverla o quitarla según sea necesario.
          //  this.eVALUACION_CALIFICACION_PRELIMINARTableAdapter.Fill(this.recursos_Humanos.EVALUACION_CALIFICACION_PRELIMINAR);
          //  estado = Menu_Evaluaciones.Estado;
           // ID = Menu_Evaluaciones.ID_EVALUACION;
           // ID = 4;
            // TODO: esta línea de código carga datos en la tabla 'recursos_Humanos.EVALUACION_DESEMPEÑO' Puede moverla o quitarla según sea necesario.
         //   this.eVALUACION_DESEMPEÑOTableAdapter.Fill(this.recursos_Humanos.EVALUACION_DESEMPEÑO);
            // TODO: esta línea de código carga datos en la tabla 'recursos_Humanos.OBJETIVOS_EVALUACION' Puede moverla o quitarla según sea necesario.
           // this.oBJETIVOS_EVALUACIONTableAdapter.Fill(this.recursos_Humanos.OBJETIVOS_EVALUACION);

            this.ReportHH1.ProcessingMode =
            Microsoft.Reporting.WinForms.ProcessingMode.Local;

            this.ReportHH1.LocalReport.EnableExternalImages = true;
            Carga_reporte();
         
        }

        private void button1_Click(object sender, EventArgs e)
        {
            

        }

        private void Carga_reporte()
        {
            
            //this.ReportHH1.LocalReport.ReportPath = @"C:\CORRECT\CORECTX APP\RRHH\RRHHR.rdlc";
            this.ReportHH1.LocalReport.ReportPath = @"C:\CORRECT\CORECTX APP\RRHH\Evaluciones.rdlc";
            
            this.recursos_Humanos.EVALUACION_DESEMPEÑO.Clear();
            this.recursos_Humanos.EVALUACION_CALIFICACION_PRELIMINAR.Clear();
            this.recursos_Humanos.EVALUACION_CALIFICACION_FINAL.Clear();
            this.recursos_Humanos.OBJETIVOS_EVALUACION.Clear();
            recursos_Humanos.REPORTE_RETROALIMENTACION.Clear();
              



            con.conectar("DM");
            
            SqlCommand cm2 = new SqlCommand("SELECT  [ID_EVALUACION],[COD_EMPLEADO],[NOMBRE],[DEPARTAMENTO],[CARGO],[JEFE_INMEDIATO],[ESTADO_EVALUACION],[SEMESTRE_INGRESO],[FECHA_INGRESO],[USUARIO_INGRESO],[COMPETENCIAS] FROM [DM].[CORRECT].[EVALUACION_DESEMPEÑO] where ID_EVALUACION ='" + ID+ "'", con.condm);
            SqlDataAdapter da = new SqlDataAdapter(cm2);
            da.Fill(recursos_Humanos.EVALUACION_DESEMPEÑO);

            SqlCommand cm3 = new SqlCommand("SELECT [ID_EVALUACION],[OBJETIVO_NUMERO],[OBJETIVO],[FECHA_INGRESO],[USUARIO_INGRESO] FROM [DM].[CORRECT].[OBJETIVOS_EVALUACION]  where ID_EVALUACION =  '" + ID + "'", con.condm);
            SqlDataAdapter da1 = new SqlDataAdapter(cm3);
            da1.Fill(recursos_Humanos.OBJETIVOS_EVALUACION);


            SqlCommand cm4 = new SqlCommand("SELECT [ID_EVALUACION],[PROMEDIO],[PASO1],[PASO2],[NOTA_FINAL],[FECHA_INGRESO],[USUARIO_INGRESO],[PROMEDIO_PASO1],[PROMEDIO_PASO2] FROM [DM].[CORRECT].[EVALUACION_CALIFICACION_PRELIMINAR] where ID_EVALUACION =  '" + ID + "'", con.condm);
            SqlDataAdapter da2 = new SqlDataAdapter(cm4);
            da2.Fill(recursos_Humanos.EVALUACION_CALIFICACION_PRELIMINAR);

            SqlCommand cm5 = new SqlCommand("SELECT [ID_EVALUACION],[PROMEDIO],[PASO1],[PASO2],[NOTA_FINAL],[FECHA_INGRESO],[USUARIO_INGRESO],[PROMEDIO_PASO1],[PROMEDIO_PASO2] FROM [DM].[CORRECT].[EVALUACION_CALIFICACION_FINAL] where ID_EVALUACION =  '" + ID + "'", con.condm);
            SqlDataAdapter da3 = new SqlDataAdapter(cm5);
            da3.Fill(recursos_Humanos.EVALUACION_CALIFICACION_FINAL);


            SqlCommand cm6= new SqlCommand("SELECT PR.ID_EVALUACION,PR.COMENTARIO_JEFE as 'COMENTARIO_JEFE_PRELIMINAR',PR.COMENTARIO_COLAB	as 'COMENTARIO_EMPLEADO_PRELIMINAR',FN.[COMENTARIO_JEFE] as 'COMENTARIO_JEFE_FINAL',FN.[COMENTARIO_COLAB] as 'COMENTARIO_EMPLEADO_FINAL' FROM  [DM].[CORRECT].[EVALUACION_DESEMPEÑO] as EV RIGHT JOIN  [DM].[CORRECT].[EVALUACION_RETROALIMENTACION_PRELIMINAR] as PR ON EV.ID_EVALUACION = PR.ID_EVALUACION LEFT JOIN [DM].[CORRECT].[EVALUACION_RETROALIMENTACION_FINAL] as FN on PR.ID_EVALUACION = FN.ID_EVALUACION where EV.ID_EVALUACION = '"+ID+"'", con.condm);
            SqlDataAdapter da4 = new SqlDataAdapter(cm6);
            da4.Fill(recursos_Humanos.REPORTE_RETROALIMENTACION);



            SqlCommand cm7 = new SqlCommand("SELECT  EV.[ID_EVALUACION],EV.[OBJETIVO_NUMERO],EV.[OBJETIVO],REP.DESCRIPCION as 'Resultado_Preliminar',REP.CALIFICACION as 'Nota_Preliminar',REF.DESCRIPCION as 'Resultado_Final',REF.CALIFICACION as 'Nota_Final' FROM [DM].[CORRECT].[OBJETIVOS_EVALUACION]EV LEFT JOIN [DM].[CORRECT].[EVALUACION_DESEMPEÑO] EVD on EV.ID_EVALUACION = EVD.ID_EVALUACION LEFT JOIN [DM].[CORRECT].[RESULTADOS_EVALUACION_PRELIMINAR] REP on  ev.ID_EVALUACION=REP.ID_EVALUACION and  EV.OBJETIVO_NUMERO = REP.ID_OBJETIVO LEFT JOIN [DM].[CORRECT].[RESULTADOS_EVALUACION_FINAL] REF on ev.ID_EVALUACION = REF.ID_EVALUACION and EV.OBJETIVO_NUMERO=REF.ID_OBJETIVO where ev.ID_EVALUACION ='"+ID+"'", con.condm);
            SqlDataAdapter da5 = new SqlDataAdapter(cm7);
            da5.Fill(recursos_Humanos.RESULTADO_PASO1);


            SqlCommand cm8 = new SqlCommand("SELECT PR.[ID_EVALUACION],PR.[CRECIMIENTO] as  CRECIMIENTO_PRELIMINAR,PR.[ORGANIZCION] as  ORGANIZACION_PRELIMINAR,PR.[RESPONSABILIDAD] as  RESPONSABILIDAD_PRELIMINAR,PR.[PRODUCTIVIDAD] as   PRODUCTIVIDAD_PRELIMINAR,FN.[CRECIMIENTO] as  CRECIMIENTO_FINAL,FN.[ORGANIZCION] as  ORGANIZACION_FINAL,FN.[RESPONSABILIDAD] as  RESPONSABILIDAD_FINAL,FN.[PRODUCTIVIDAD] as   PRODUCTIVIDAD_FINAL  FROM [DM].[CORRECT].[EVALUACION_DESEMPEÑO] as EV RIGHT JOIN [DM].[CORRECT].[RESULTADO_ESTRATEGIA_PRELIMINAR] as PR on EV.ID_EVALUACION = PR.ID_EVALUACION LEFT JOIN [DM].[CORRECT].[RESULTADO_ESTRATEGIA_FINAL] as FN on PR.ID_EVALUACION = FN.ID_EVALUACION where EV.ID_EVALUACION = '"+ID+"'", con.condm);
            SqlDataAdapter da6 = new SqlDataAdapter(cm8);
            da6.Fill(recursos_Humanos.RESULTADO_PASO2);


            SqlCommand cm9 = new SqlCommand("SELECT EV.[ID_EVALUACION],ECT.ID_CODIGO,ECT.DESCRIPCION as 'CODIGO_DE_TRABAJO',ECTP.EVIDENCIA as 'EVIDENCIA_PRELIMINAR',ECTP.CALIFICACION as 'CALIFICACION_PRELIMINAR',TM.EVIDENCIA as 'EVIDENCIA_FINAL',TM.CALIFICACION as 'CALIFICACION_FINAL'   FROM [DM].[CORRECT].[EVALUACION_DESEMPEÑO] EV LEFT JOIN [DM].[CORRECT].[EVALUACION_COD_TRABAJO_PRELIMINAR] ECTP ON EV.ID_EVALUACION = ECTP.ID_EVALUACION  LEFT JOIN [DM].[CORRECT].[EVALUACION_CODIGO_TRABAJO] ECT on  ECTP.COD_TRABAJO = ECT.ID_CODIGO  LEFT JOIN (SELECT EV.[ID_EVALUACION], ECTF.COD_TRABAJO, ECTF.EVIDENCIA, ECTF.CALIFICACION  FROM [DM].[CORRECT].[EVALUACION_DESEMPEÑO] EV LEFT JOIN [DM].[CORRECT].[EVALUACION_COD_TRABAJO_FINAL] ECTF ON EV.ID_EVALUACION = ECTF.ID_EVALUACION  LEFT JOIN [DM].[CORRECT].[EVALUACION_CODIGO_TRABAJO] ECT on  ECTF.COD_TRABAJO = ECT.ID_CODIGO  Group by EV.ID_EVALUACION,ECTF.COD_TRABAJO,ECTF.EVIDENCIA,ECTF.CALIFICACION) TM on EV.ID_EVALUACION = TM.ID_EVALUACION and ECT.ID_CODIGO = TM.COD_TRABAJO WHERE EV.ID_EVALUACION = '"+ID+"' Group by EV.ID_EVALUACION,ECT.ID_CODIGO,ECTP.EVIDENCIA,ECTP.CALIFICACION,TM.EVIDENCIA,TM.CALIFICACION,ECT.DESCRIPCION order by ID_EVALUACION", con.condm);
            SqlDataAdapter da7 = new SqlDataAdapter(cm9);
            da7.Fill(recursos_Humanos.RESULTADOS_PASO_3);

            con.Desconectar("DM");



            string empresa = Main_Menu.EMPRESA;
            int logo = Main_Menu.logo;

            //string empresa = "DISMO, S.A. de C.V.";
            //ReportParameter[] param = new ReportParameter[3];

            //switch (logo)
            //{
            //    case 1 :
            //        param[0] = new ReportParameter("Directorio", @"file:C:\CORRECT\DM.jpg");
            //        break;
            //    case 2:
            //        param[0] = new ReportParameter("Directorio", @"file:C:\CORRECT\KOI.jpg");
            //        break;
            //    case 3:
            //        param[0] = new ReportParameter("Directorio", @"file:C:\CORRECT\smash.png");
            //        break;
            //    case 4:
            //        param[0] = new ReportParameter("Directorio", @"file:C:\CORRECT\cv.jpg");
            //        break;
            //    case 5:
            //        param[0] = new ReportParameter("Directorio", @"file:C:\CORRECT\C7.jpg");
            //        break;
            //    case 6:
            //        param[0] = new ReportParameter("Directorio", @"file:C:\CORRECT\lesa.jpg");
            //        break;
            //    case 7:
            //        param[0] = new ReportParameter("Directorio", @"file:C:\CORRECT\zegna.jpg");
            //        break;
            
            //}

           
            //param[1] = new ReportParameter("Empresa", empresa);
            //param[2] = new ReportParameter("Estado", estado);
            //ReportHH1.LocalReport.SetParameters(param);
            

            this.ReportHH1.RefreshReport();


            ReportHH1.SetDisplayMode(DisplayMode.PrintLayout);
            
           
        
        
        }


    }
}
