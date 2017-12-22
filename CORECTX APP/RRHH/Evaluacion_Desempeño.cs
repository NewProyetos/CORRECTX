using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Data.SqlClient;
namespace Sinconizacion_EXactus
{
    public partial class Evaluacion_Desempeño : Form
    {
        public Evaluacion_Desempeño(int Tipo_consult,int Id_eva,string Año,string Id_jefe)
        {
            InitializeComponent();
            Tipo_Conulsta = Tipo_consult;
            ID_EVALUACION = Id_eva;
            AÑO = Año;
            ID_JEFE = Id_jefe;            
        }

        conexionXML con = new conexionXML();
        DataTable PreliminarPaso1 = new DataTable();
        DataTable PreliminarPaso2 = new DataTable();
        DataTable PreliminarPaso3 = new DataTable();
        DataTable PreliminarPaso4 = new DataTable();
        DataTable PreliminarPaso5 = new DataTable();


        String Nombre;
        String AÑO;
        String ID_JEFE;
        Int32 ID_EVALUACION ;
        String Departamento;
        String Jefe_Inmediato;
        int CRP;

        String Cargo;
        String Cod_Empleado;
        String objetivo1;
        String objetivo2;
        String objetivo3;
        String objetivo4;
        String objetivo5;
        String objetivo6;
        String objetivo7;
        String objetivo8;
        String objetivo9;
        String objetivo10;
        String Resultado_insert;
        String Resultado1;
        String Resultado2;
        String Resultado3;
        String Resultado4;
        String Resultado5;
        String Resultado6;
        String Resultado7;
        String Resultado8;
        String Resultado9;
        String Resultado10;
        String CORP;
        String COMENT_CORP;

        String CORP_CLIE;
        String COMENT_CORP_CLIE;

        String Calificacion1;
        String Calificacion_Insert;
        String Calificacion2;
        String Calificacion3;
        String Calificacion4;
        String Calificacion5;
        String Calificacion6;
        String Calificacion7;
        String Calificacion8;
        String Calificacion9;
        String Calificacion10;
        String Crecimiento;
        String Organizacion;
        String Responsabilidad;
        String Productividad;
        String Comentario_Jefe;
        String Comentario_Empl;
        String Compentecia_Desarollo;
        Int32 lineasPaso1 = 0;
        String Usuario;        
        Double NotalPaso1= 0.0;
        Double PromedioPaso1 = 0.0;
        Double Notalclasf1 = 0.0;
        Double Notalclasf2 = 0.0;
        Double Notalclasf3 = 0.0;
        Double Notalclasf4 = 0.0;
        Double Notalclasf5 = 0.0;
        Double Notalclasf6 = 0.0;
        Double Notalclasf7 = 0.0;
        Double Notalclasf8 = 0.0;
        Double Notalclasf9 = 0.0;
        Double Notalclasf10 = 0.0;
        String ResultadoCL1;
        String ResultadoCL3;
        String PASO3Evidencia_insert;
        String PASO3Evidencia1;
        String PASO3Evidencia2;
        String PASO3Evidencia3;
        String PASO3Evidencia4;
        String PASO3Evidencia5;
        String PASO3Evidencia6;
        String PASO3Evidencia7;
        String PASO3Evidencia8;
        String PASO3Evidencia9;
        Double PASO3nota1 = 0.0;
        Double PASO3nota2 = 0.0;
        Double PASO3nota3 = 0.0;
        Double PASO3nota4 = 0.0;
        Double PASO3nota5 = 0.0;
        Double PASO3nota6 = 0.0;
        Double PASO3nota7 = 0.0;
        Double PASO3nota8 = 0.0;
        Double PASO3nota9 = 0.0;
        String PASO3calificacion_insert;
        String PASO3calificacion1;
        String PASO3calificacion2;
        String PASO3calificacion3;
        String PASO3calificacion4;
        String PASO3calificacion5;
        String PASO3calificacion6;
        String PASO3calificacion7;
        String PASO3calificacion8;
        String PASO3calificacion9;
        String EVIDENCIA_INSERT;
        String P3CALIFI_INSERT;
        Double TotalPASO3 = 0.0;
        Double PromedioPASO3 = 0.0;
        Int32 LineaPASO3 = 0;
        Double PromedioFinal = 0.0;
        String Resultado_Final;
        Int32 Tipo_Conulsta ;
        DataTable ObjetivosDT = new DataTable();

        private void Evaluacion_Desempeño_Load(object sender, EventArgs e)
        {
            //rectangulo();
            CRP = 0;
            Usuario = Login.usuario.ToUpper();

            //  EnableTab(tabPage2, false);
            tabControl1.TabPages.Remove(tabPage2);
            linkLabel5.Hide();

            groupBox23.Hide();
            groupBox24.Hide();
            //tabControl1.TabPages[1].Text = "PASO";

            if (Tipo_Conulsta == 0)
            {
                groupBox3.Enabled = false;
                groupBox4.Enabled = false;
                groupBox5.Enabled = false;
                groupBox6.Enabled = false;
                groupBox7.Enabled = false;
                groupBox8.Enabled = false;
                groupBox9.Enabled = false;
                groupBox10.Enabled = false;
                groupBox11.Enabled = false;
                groupBox12.Enabled = false;
                groupBox13.Enabled = false;
                groupBox14.Enabled = false;
                groupBox15.Enabled = false;
                groupBox16.Enabled = false;
                groupBox17.Enabled = false;
                groupBox18.Enabled = false;
                groupBox19.Enabled = false;
                groupBox20.Enabled = false;
                groupBox21.Enabled = false;
                groupBox22.Enabled = false;
                

                button2.Enabled = false;

                button1.Enabled = false;

                con.conectar("DM");
                SqlCommand cm2 = new SqlCommand("SELECT  [ID_EVALUACION],[COD_EMPLEADO],[NOMBRE],[DEPARTAMENTO],[CARGO],[JEFE_INMEDIATO],[ESTADO_EVALUACION],[SEMESTRE_INGRESO],[FECHA_INGRESO],[USUARIO_INGRESO],[COMPETENCIAS] FROM [DM].[CORRECT].[EVALUACION_DESEMPEÑO] Where ID_EVALUACION ='"+ID_EVALUACION+"' ", con.condm);
                SqlDataReader dr2 = cm2.ExecuteReader();
                while (dr2.Read())
                {
                    textBox1.Text = Convert.ToString(dr2["NOMBRE"]);
                    textBox2.Text = Convert.ToString(dr2["CARGO"]);
                    textBox3.Text = Convert.ToString(dr2["DEPARTAMENTO"]);
                    textBox4.Text = Convert.ToString(dr2["JEFE_INMEDIATO"]);
                    textBox5.Text = Convert.ToString(dr2["COD_EMPLEADO"]);
                    richTextBox38.Text = Convert.ToString(dr2["COMPETENCIAS"]);
                }
                dr2.Close();
                con.Desconectar("DM");

                
                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
                textBox4.ReadOnly = true;
                textBox5.ReadOnly = true;
               

                carga_objetivos();



                richTextBox2.ReadOnly = true;
                richTextBox3.ReadOnly = true;
                richTextBox4.ReadOnly = true;
                richTextBox5.ReadOnly = true;
                richTextBox6.ReadOnly = true;
                richTextBox7.ReadOnly = true;
                richTextBox8.ReadOnly = true;
                richTextBox9.ReadOnly = true;
                richTextBox10.ReadOnly = true;
                richTextBox11.ReadOnly = true;
                richTextBox12.ReadOnly = true;
                richTextBox13.ReadOnly = true;
                richTextBox14.ReadOnly = true;
                richTextBox15.ReadOnly = true;
                richTextBox16.ReadOnly = true;
                richTextBox17.ReadOnly = true;
                richTextBox18.ReadOnly = true;
                richTextBox19.ReadOnly = true;
                richTextBox20.ReadOnly = true;
                richTextBox21.ReadOnly = true;
                richTextBox22.ReadOnly = true;
                richTextBox23.ReadOnly = true;
                richTextBox24.ReadOnly = true;
                richTextBox25.ReadOnly = true;
                richTextBox26.ReadOnly = true;
                richTextBox27.ReadOnly = true;
                richTextBox28.ReadOnly = true;
                richTextBox29.ReadOnly = true;
                richTextBox30.ReadOnly = true;
                richTextBox31.ReadOnly = true;
                richTextBox32.ReadOnly = true;
                richTextBox33.ReadOnly = true;
                richTextBox34.ReadOnly = true;
                richTextBox35.ReadOnly = true;
                richTextBox36.ReadOnly = true;
                richTextBox37.ReadOnly = true;
                richTextBox38.ReadOnly = true;
                richTextBox39.ReadOnly = true;
                richTextBox40.ReadOnly = true;
                richTextBox41.ReadOnly = true;

               
                



            }
            else
                if (Tipo_Conulsta == 1 )
            {
                
                groupBox3.Enabled = false;
                groupBox4.Enabled = false;
                groupBox5.Enabled = false;
                groupBox6.Enabled = false;
                groupBox7.Enabled = false;
                groupBox8.Enabled = false;
                groupBox9.Enabled = false;
                groupBox10.Enabled = false;
                groupBox11.Enabled = false;
                groupBox12.Enabled = false;
                groupBox13.Enabled = false;
                groupBox14.Enabled = false;
                groupBox15.Enabled = false;
                groupBox16.Enabled = false;
                groupBox17.Enabled = false;
                groupBox18.Enabled = false;
                groupBox19.Enabled = false;
                groupBox20.Enabled = false;
                groupBox21.Enabled = false;
                groupBox22.Enabled = false;

                button2.Enabled = false;

                button1.Enabled = false;

                con.conectar("DM");
                SqlCommand cm2 = new SqlCommand("SELECT  [ID_EVALUACION],[COD_EMPLEADO],[NOMBRE],[DEPARTAMENTO],[CARGO],[JEFE_INMEDIATO],[ESTADO_EVALUACION],[SEMESTRE_INGRESO],[FECHA_INGRESO],[USUARIO_INGRESO],[COMPETENCIAS] FROM [DM].[CORRECT].[EVALUACION_DESEMPEÑO] Where ID_EVALUACION ='" + ID_EVALUACION + "' ", con.condm);
                SqlDataReader dr2 = cm2.ExecuteReader();
                while (dr2.Read())
                {
                    textBox1.Text = Convert.ToString(dr2["NOMBRE"]);
                    textBox2.Text = Convert.ToString(dr2["CARGO"]);
                    textBox3.Text = Convert.ToString(dr2["DEPARTAMENTO"]);
                    textBox4.Text = Convert.ToString(dr2["JEFE_INMEDIATO"]);
                    textBox5.Text = Convert.ToString(dr2["COD_EMPLEADO"]);
                    richTextBox38.Text = Convert.ToString(dr2["COMPETENCIAS"]);
                }
                dr2.Close();
                con.Desconectar("DM");


                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
                textBox4.ReadOnly = true;
                textBox5.ReadOnly = true;

                radioButton75.Checked = true;
                radioButton71.Checked = true;
                radioButton67.Checked = true;
                radioButton63.Checked = true;
                radioButton59.Checked = true;
                radioButton55.Checked = true;
                radioButton51.Checked = true;
                radioButton47.Checked = true;
                radioButton43.Checked = true;


                richTextBox2.ReadOnly = true;
                richTextBox3.ReadOnly = true;
                richTextBox4.ReadOnly = true;
                richTextBox5.ReadOnly = true;
                richTextBox6.ReadOnly = true;
                richTextBox7.ReadOnly = true;
                richTextBox8.ReadOnly = true;
                richTextBox9.ReadOnly = true;
                richTextBox10.ReadOnly = true;
                richTextBox11.ReadOnly = true;
                richTextBox22.ReadOnly = true;
                richTextBox36.ReadOnly = true;
                richTextBox37.ReadOnly = true;
                richTextBox39.ReadOnly = true;
                richTextBox38.ReadOnly = true;


                richTextBox12.ReadOnly = true;
                richTextBox13.ReadOnly = true;
                richTextBox14.ReadOnly = true;
                richTextBox15.ReadOnly = true;
                richTextBox16.ReadOnly = true;
                richTextBox17.ReadOnly = true;
                richTextBox18.ReadOnly = true;
                richTextBox19.ReadOnly = true;
                richTextBox20.ReadOnly = true;
                richTextBox21.ReadOnly = true;
               

                richTextBox23.ReadOnly = false;
                richTextBox24.ReadOnly = false;
                richTextBox25.ReadOnly = false;
                richTextBox26.ReadOnly = false;

                richTextBox27.ReadOnly = false;
                richTextBox28.ReadOnly = false;
                richTextBox29.ReadOnly = false;
                richTextBox30.ReadOnly = false;
                richTextBox31.ReadOnly = false;
                richTextBox32.ReadOnly = false;
                richTextBox33.ReadOnly = false;
                richTextBox34.ReadOnly = false;
                richTextBox35.ReadOnly = false;

                richTextBox40.ReadOnly = false;
                richTextBox41.ReadOnly = false;

                carga_objetivos();
                button1.Enabled = true;
            }
           
          else
                    if (Tipo_Conulsta == 2)
                    {
                        con.conectar("DM");
                        SqlCommand cm2 = new SqlCommand("SELECT  [ID_EVALUACION],[COD_EMPLEADO],[NOMBRE],[DEPARTAMENTO],[CARGO],[JEFE_INMEDIATO],[ESTADO_EVALUACION],[SEMESTRE_INGRESO],[FECHA_INGRESO],[USUARIO_INGRESO],[COMPETENCIAS] ,[CORP] ,[COMENT_CORP] FROM [DM].[CORRECT].[EVALUACION_DESEMPEÑO] Where ID_EVALUACION ='" + ID_EVALUACION + "' ", con.condm);
                        SqlDataReader dr2 = cm2.ExecuteReader();
                        while (dr2.Read())
                        {
                            textBox1.Text = Convert.ToString(dr2["NOMBRE"]);
                            textBox2.Text = Convert.ToString(dr2["CARGO"]);
                            textBox3.Text = Convert.ToString(dr2["DEPARTAMENTO"]);
                            textBox4.Text = Convert.ToString(dr2["JEFE_INMEDIATO"]);
                            textBox5.Text = Convert.ToString(dr2["COD_EMPLEADO"]);
                            richTextBox38.Text = Convert.ToString(dr2["COMPETENCIAS"]);
                            CORP = Convert.ToString(dr2["CORP"]);
                            COMENT_CORP = Convert.ToString(dr2["COMENT_CORP"]);
                }
                        dr2.Close();
                        con.Desconectar("DM");



              


                        richTextBox2.ReadOnly = true;
                        richTextBox3.ReadOnly = true;
                        richTextBox4.ReadOnly = true;
                        richTextBox5.ReadOnly = true;
                        richTextBox6.ReadOnly = true;
                        richTextBox7.ReadOnly = true;
                        richTextBox8.ReadOnly = true;
                        richTextBox9.ReadOnly = true;
                        richTextBox10.ReadOnly = true;
                        richTextBox11.ReadOnly = true;
                        richTextBox12.ReadOnly = true;
                        richTextBox13.ReadOnly = true;
                        richTextBox14.ReadOnly = true;
                        richTextBox15.ReadOnly = true;
                        richTextBox16.ReadOnly = true;
                        richTextBox17.ReadOnly = true;
                        richTextBox18.ReadOnly = true;
                        richTextBox19.ReadOnly = true;
                        richTextBox20.ReadOnly = true;
                        richTextBox21.ReadOnly = true;
                        richTextBox22.ReadOnly = true;
                        richTextBox23.ReadOnly = true;
                        richTextBox24.ReadOnly = true;
                        richTextBox25.ReadOnly = true;
                        richTextBox26.ReadOnly = true;
                        richTextBox27.ReadOnly = true;
                        richTextBox28.ReadOnly = true;
                        richTextBox29.ReadOnly = true;
                        richTextBox30.ReadOnly = true;
                        richTextBox31.ReadOnly = true;
                        richTextBox32.ReadOnly = true;
                        richTextBox33.ReadOnly = true;
                        richTextBox34.ReadOnly = true;
                        richTextBox35.ReadOnly = true;
                        richTextBox36.ReadOnly = true;
                        richTextBox37.ReadOnly = true;
                        richTextBox38.ReadOnly = true;
                        richTextBox39.ReadOnly = true;
                        richTextBox40.ReadOnly = true;
                        richTextBox41.ReadOnly = true;

                        textBox1.ReadOnly = true;
                        textBox2.ReadOnly = true;
                        textBox3.ReadOnly = true;
                        textBox4.ReadOnly = true;
                        textBox5.ReadOnly = true;

                        groupBox3.Enabled = false;
                        groupBox4.Enabled = false;
                        groupBox5.Enabled = false;
                        groupBox6.Enabled = false;
                        groupBox7.Enabled = false;
                        groupBox8.Enabled = false;
                        groupBox9.Enabled = false;
                        groupBox10.Enabled = false;
                        groupBox11.Enabled = false;
                        groupBox12.Enabled = false;
                        groupBox13.Enabled = false;
                        groupBox14.Enabled = false;
                        groupBox15.Enabled = false;
                        groupBox16.Enabled = false;
                        groupBox17.Enabled = false;
                        groupBox18.Enabled = false;
                        groupBox19.Enabled = false;
                        groupBox20.Enabled = false;
                        groupBox21.Enabled = false;
                        groupBox22.Enabled = false;

                        carga_objetivos();
                        carga_preliminar();

                    }
                    else if (Tipo_Conulsta == 3)
                    {
                        con.conectar("DM");
                        SqlCommand cm2 = new SqlCommand("SELECT  [ID_EVALUACION],[COD_EMPLEADO],[NOMBRE],[DEPARTAMENTO],[CARGO],[JEFE_INMEDIATO],[ESTADO_EVALUACION],[SEMESTRE_INGRESO],[FECHA_INGRESO],[USUARIO_INGRESO],[COMPETENCIAS] FROM [DM].[CORRECT].[EVALUACION_DESEMPEÑO] Where ID_EVALUACION ='" + ID_EVALUACION + "' ", con.condm);
                        SqlDataReader dr2 = cm2.ExecuteReader();
                        while (dr2.Read())
                        {
                            textBox1.Text = Convert.ToString(dr2["NOMBRE"]);
                            textBox2.Text = Convert.ToString(dr2["CARGO"]);
                            textBox3.Text = Convert.ToString(dr2["DEPARTAMENTO"]);
                            textBox4.Text = Convert.ToString(dr2["JEFE_INMEDIATO"]);
                            textBox5.Text = Convert.ToString(dr2["COD_EMPLEADO"]);
                            richTextBox38.Text = Convert.ToString(dr2["COMPETENCIAS"]);
                        }
                        dr2.Close();
                        con.Desconectar("DM");

                        button1.Enabled = false;
                        button2.Enabled = false;
                        carga_objetivos();
                        carga_preliminar();


                        richTextBox2.ReadOnly = true;
                        richTextBox3.ReadOnly = true;
                        richTextBox4.ReadOnly = true;
                        richTextBox5.ReadOnly = true;
                        richTextBox6.ReadOnly = true;
                        richTextBox7.ReadOnly = true;
                        richTextBox8.ReadOnly = true;
                        richTextBox9.ReadOnly = true;
                        richTextBox10.ReadOnly = true;
                        richTextBox11.ReadOnly = true;
                        richTextBox12.ReadOnly = true;
                        richTextBox13.ReadOnly = true;
                        richTextBox14.ReadOnly = true;
                        richTextBox15.ReadOnly = true;
                        richTextBox16.ReadOnly = true;
                        richTextBox17.ReadOnly = true;
                        richTextBox18.ReadOnly = true;
                        richTextBox19.ReadOnly = true;
                        richTextBox20.ReadOnly = true;
                        richTextBox21.ReadOnly = true;
                        richTextBox22.ReadOnly = true;
                        richTextBox23.ReadOnly = true;
                        richTextBox24.ReadOnly = true;
                        richTextBox25.ReadOnly = true;
                        richTextBox26.ReadOnly = true;
                        richTextBox27.ReadOnly = true;
                        richTextBox28.ReadOnly = true;
                        richTextBox29.ReadOnly = true;
                        richTextBox30.ReadOnly = true;
                        richTextBox31.ReadOnly = true;
                        richTextBox32.ReadOnly = true;
                        richTextBox33.ReadOnly = true;
                        richTextBox34.ReadOnly = true;
                        richTextBox35.ReadOnly = true;
                        richTextBox36.ReadOnly = true;
                        richTextBox37.ReadOnly = true;
                        richTextBox38.ReadOnly = true;
                        richTextBox39.ReadOnly = true;
                        richTextBox40.ReadOnly = true;
                        richTextBox41.ReadOnly = true;

                        textBox1.ReadOnly = true;
                        textBox2.ReadOnly = true;
                        textBox3.ReadOnly = true;
                        textBox4.ReadOnly = true;
                        textBox5.ReadOnly = true;

                        groupBox3.Enabled = false;
                        groupBox4.Enabled = false;
                        groupBox5.Enabled = false;
                        groupBox6.Enabled = false;
                        groupBox7.Enabled = false;
                        groupBox8.Enabled = false;
                        groupBox9.Enabled = false;
                        groupBox10.Enabled = false;
                        groupBox11.Enabled = false;
                        groupBox12.Enabled = false;
                        groupBox13.Enabled = false;
                        groupBox14.Enabled = false;
                        groupBox15.Enabled = false;
                        groupBox16.Enabled = false;
                        groupBox17.Enabled = false;
                        groupBox18.Enabled = false;
                        groupBox19.Enabled = false;
                        groupBox20.Enabled = false;
                        groupBox21.Enabled = false;
                        groupBox22.Enabled = false;

                      

                    
                    }
                    else if (Tipo_Conulsta == 4)
                    {
                        con.conectar("DM");
                        SqlCommand cm2 = new SqlCommand("SELECT  [ID_EVALUACION],[COD_EMPLEADO],[NOMBRE],[DEPARTAMENTO],[CARGO],[JEFE_INMEDIATO],[ESTADO_EVALUACION],[SEMESTRE_INGRESO],[FECHA_INGRESO],[USUARIO_INGRESO],[COMPETENCIAS] FROM [DM].[CORRECT].[EVALUACION_DESEMPEÑO] Where ID_EVALUACION ='" + ID_EVALUACION + "' ", con.condm);
                        SqlDataReader dr2 = cm2.ExecuteReader();
                        while (dr2.Read())
                        {
                            textBox1.Text = Convert.ToString(dr2["NOMBRE"]);
                            textBox2.Text = Convert.ToString(dr2["CARGO"]);
                            textBox3.Text = Convert.ToString(dr2["DEPARTAMENTO"]);
                            textBox4.Text = Convert.ToString(dr2["JEFE_INMEDIATO"]);
                            textBox5.Text = Convert.ToString(dr2["COD_EMPLEADO"]);
                            richTextBox38.Text = Convert.ToString(dr2["COMPETENCIAS"]);
                        }
                        dr2.Close();
                        con.Desconectar("DM");

                        button1.Enabled = false;
                        button2.Enabled = false;
                        carga_objetivos();
                        carga_final();


                        richTextBox2.ReadOnly = true;
                        richTextBox3.ReadOnly = true;
                        richTextBox4.ReadOnly = true;
                        richTextBox5.ReadOnly = true;
                        richTextBox6.ReadOnly = true;
                        richTextBox7.ReadOnly = true;
                        richTextBox8.ReadOnly = true;
                        richTextBox9.ReadOnly = true;
                        richTextBox10.ReadOnly = true;
                        richTextBox11.ReadOnly = true;
                        richTextBox12.ReadOnly = true;
                        richTextBox13.ReadOnly = true;
                        richTextBox14.ReadOnly = true;
                        richTextBox15.ReadOnly = true;
                        richTextBox16.ReadOnly = true;
                        richTextBox17.ReadOnly = true;
                        richTextBox18.ReadOnly = true;
                        richTextBox19.ReadOnly = true;
                        richTextBox20.ReadOnly = true;
                        richTextBox21.ReadOnly = true;
                        richTextBox22.ReadOnly = true;
                        richTextBox23.ReadOnly = true;
                        richTextBox24.ReadOnly = true;
                        richTextBox25.ReadOnly = true;
                        richTextBox26.ReadOnly = true;
                        richTextBox27.ReadOnly = true;
                        richTextBox28.ReadOnly = true;
                        richTextBox29.ReadOnly = true;
                        richTextBox30.ReadOnly = true;
                        richTextBox31.ReadOnly = true;
                        richTextBox32.ReadOnly = true;
                        richTextBox33.ReadOnly = true;
                        richTextBox34.ReadOnly = true;
                        richTextBox35.ReadOnly = true;
                        richTextBox36.ReadOnly = true;
                        richTextBox37.ReadOnly = true;
                        richTextBox38.ReadOnly = true;
                        richTextBox39.ReadOnly = true;
                        richTextBox40.ReadOnly = true;
                        richTextBox41.ReadOnly = true;

                        textBox1.ReadOnly = true;
                        textBox2.ReadOnly = true;
                        textBox3.ReadOnly = true;
                        textBox4.ReadOnly = true;
                        textBox5.ReadOnly = true;

                        groupBox3.Enabled = false;
                        groupBox4.Enabled = false;
                        groupBox5.Enabled = false;
                        groupBox6.Enabled = false;
                        groupBox7.Enabled = false;
                        groupBox8.Enabled = false;
                        groupBox9.Enabled = false;
                        groupBox10.Enabled = false;
                        groupBox11.Enabled = false;
                        groupBox12.Enabled = false;
                        groupBox13.Enabled = false;
                        groupBox14.Enabled = false;
                        groupBox15.Enabled = false;
                        groupBox16.Enabled = false;
                        groupBox17.Enabled = false;
                        groupBox18.Enabled = false;
                        groupBox19.Enabled = false;
                        groupBox20.Enabled = false;
                        groupBox21.Enabled = false;
                        groupBox22.Enabled = false;

                    }
         
        }

   
       

        private void button1_Click(object sender, EventArgs e)
        {
            
            
                if (richTextBox2.Text != "" && richTextBox21.Text == "")
                {

                    MessageBox.Show("Resultado Onjetivo:  "+richTextBox2.Text+",  No ha sido ingresado", "OBJETIVO 1", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    richTextBox21.Focus();
                    tabControl1.SelectedIndex = 0;
                  
                }
                else if (richTextBox21.Text != "" && radioButton1.Checked == false && radioButton2.Checked == false && radioButton3.Checked == false && radioButton4.Checked == false)
                {
                    MessageBox.Show("Establesca una calificación para el objetivo  " + richTextBox2.Text + "");
                    radioButton1.Focus();
                }
                else
                if (richTextBox3.Text != "" && richTextBox20.Text == "")
                {

                    MessageBox.Show("Resultado Onjetivo:  " + richTextBox3.Text + ",  No ha sido ingresado", "OBJETIVO 2", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    richTextBox20.Focus();
                    tabControl1.SelectedIndex = 0;


                }
                else if (richTextBox20.Text != "" && radioButton5.Checked == false && radioButton6.Checked == false && radioButton7.Checked == false && radioButton8.Checked == false)
                {
                    MessageBox.Show("Establesca una calificación para el objetivo  " + richTextBox3.Text + "");
                    radioButton5.Focus();
                }

                else
                    if (richTextBox4.Text != ""  && richTextBox19.Text == "")
                    {
                        MessageBox.Show("Resultado Onjetivo:  " + richTextBox4.Text + ",  No ha sido ingresado", "OBJETIVO 3", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                        richTextBox19.Focus();
                        tabControl1.SelectedIndex = 0;
                    }
                    else if (richTextBox19.Text != "" && radioButton9.Checked == false && radioButton10.Checked == false && radioButton11.Checked == false && radioButton12.Checked == false)
                    {
                        MessageBox.Show("Establesca una calificación para el objetivo  " + richTextBox4.Text + "");
                        radioButton9.Focus();
                    }

                    else
                        if (richTextBox5.Text != "" && richTextBox18.Text == "")
                        {

                            MessageBox.Show("Resultado Onjetivo:  " + richTextBox5.Text + ",  No ha sido ingresado", "OBJETIVO 4", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                            richTextBox18.Focus();
                            tabControl1.SelectedIndex = 0;
                        }

                        else if (richTextBox18.Text != "" && radioButton13.Checked == false && radioButton14.Checked == false && radioButton15.Checked == false && radioButton16.Checked == false)
                        {
                            MessageBox.Show("Establesca una calificación para el objetivo  " + richTextBox5.Text + "");
                            radioButton13.Focus();
                        }
                        else
                            if (richTextBox6.Text != "" && richTextBox17.Text == "")
                            {

                                MessageBox.Show("Resultado Onjetivo:  " + richTextBox6.Text + ",  No ha sido ingresado", "OBJETIVO 5", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                                richTextBox17.Focus();
                                tabControl1.SelectedIndex = 0;
                            }

                            else if (richTextBox17.Text != "" &&  radioButton17.Checked == false && radioButton18.Checked == false && radioButton19.Checked == false && radioButton20.Checked == false)
                            {
                                MessageBox.Show("Establesca una calificación para el objetivo  " + richTextBox6.Text + "");
                                radioButton17.Focus();
                            }

                            else
                                if (richTextBox7.Text != "" && richTextBox16.Text == "")
                                {

                                    MessageBox.Show("Resultado Onjetivo:  " + richTextBox7.Text + ",  No ha sido ingresado", "OBJETIVO 6", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                                    richTextBox16.Focus();
                                    tabControl1.SelectedIndex = 0;
                                }

                                else if (richTextBox16.Text != "" && radioButton21.Checked == false && radioButton22.Checked == false && radioButton23.Checked == false && radioButton24.Checked == false)
                                {
                                    MessageBox.Show("Establesca una calificación para el objetivo  " + richTextBox7.Text + "");
                                    radioButton21.Focus();
                                }


                                else
                                    if (richTextBox8.Text != "" && richTextBox15.Text == "")
                                    {

                                        MessageBox.Show("Resultado Onjetivo:  " + richTextBox8.Text + ",  No ha sido ingresado", "OBJETIVO 7", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                                        richTextBox15.Focus();
                                        tabControl1.SelectedIndex = 0;
                                    }

                                    else if (richTextBox15.Text != "" && radioButton25.Checked == false && radioButton26.Checked == false && radioButton27.Checked == false && radioButton28.Checked == false)
                                    {
                                        MessageBox.Show("Establesca una calificación para el objetivo  " + richTextBox8.Text + "");
                                        radioButton25.Focus();
                                    }

                                    else
                                        if (richTextBox9.Text != "" && richTextBox14.Text == "")
                                        {


                                            MessageBox.Show("Resultado Onjetivo:  " + richTextBox9.Text + ",  No ha sido ingresado", "OBJETIVO 8", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                                            richTextBox14.Focus();
                                            tabControl1.SelectedIndex = 0;
                                        }

                                        else if (richTextBox14.Text != "" && radioButton29.Checked == false && radioButton30.Checked == false && radioButton31.Checked == false && radioButton32.Checked == false)
                                        {
                                            MessageBox.Show("Establesca una calificación para el objetivo  " + richTextBox9.Text + "");
                                            radioButton29.Focus();
                                        }

                                        else
                                            if (richTextBox10.Text != "" && richTextBox13.Text == "")
                                            {
                                                
                                                MessageBox.Show("Resultado Onjetivo:  " + richTextBox10.Text + ",  No ha sido ingresado", "OBJETIVO 9", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                                                richTextBox13.Focus();
                                                tabControl1.SelectedIndex = 0;
                                            }

                                            else if (richTextBox13.Text != "" && radioButton33.Checked == false && radioButton34.Checked == false && radioButton35.Checked == false && radioButton36.Checked == false)
                                            {
                                                MessageBox.Show("Establesca una calificación para el objetivo  " + richTextBox10.Text + "");
                                                radioButton33.Focus();
                                            }

                                            else
                                                if (richTextBox11.Text != "" && richTextBox12.Text == "")
                                                {

                                                    MessageBox.Show("Resultado Onjetivo:  " + richTextBox11.Text + ",  No ha sido ingresado", "OBJETIVO 10", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                                                    richTextBox12.Focus();
                                                    tabControl1.SelectedIndex = 0;
                                                }

                                                else if (richTextBox12.Text != "" && radioButton37.Checked == false && radioButton38.Checked == false && radioButton39.Checked == false && radioButton40.Checked == false)
                                                {
                                                    MessageBox.Show("Establesca una calificación para el objetivo  " + richTextBox11.Text + "");
                                                    radioButton12.Focus();
                                                }

                                          


                                                                //else
                                                                //    if (richTextBox27.Text == "" || richTextBox28.Text == "" || richTextBox29.Text == "" || richTextBox30.Text == "" || richTextBox31.Text == "" || richTextBox32.Text == "" || richTextBox33.Text == "" || richTextBox34.Text == "" || richTextBox35.Text == "")
                                                                //    {
                                                                //        MessageBox.Show("NO pueden quedar en blanco las Evidencias en Codigo de Trabajo", "PASO3", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                                                                //        tabControl1.SelectedIndex = 2;
                                                                //        richTextBox35.Focus();
                                                                        

                                                                //    }

                                                                    else
                                                                        if (richTextBox38.Text == "")
                                                                        {
                                                                            MessageBox.Show("Ingrese Las Competencias y Areas de Desarollo Deseadas", "PASO4", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                                                                            tabControl1.SelectedIndex = 3;
                                                                            richTextBox38.Focus();
                                                                        }

                                                                        else
                                                                            if (richTextBox40.Text == "")
                                                                            {
                                                                                MessageBox.Show("Ingrese Los Comentarios de Jefe Inmediato", "PASO5", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                                                                                tabControl1.SelectedIndex = 4;
                                                                                richTextBox40.Focus();
                                                                            }

                                                                            else
                                                                                if (richTextBox41.Text == "")
                                                                                {
                                                                                    MessageBox.Show("Ingrese Los Comentarios del Colaborador", "PASO5", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                                                                                    tabControl1.SelectedIndex = 4;
                                                                                    richTextBox41.Focus();
                                                                                }
                                                                                else
                                                                                {
                                                                                    DataPaso1();
                                                                                    DataPaso2();
                                                                                    DataPaso3();
                                                                                    Calculos();
                                                                                    Resultado();
                                                                                    DataPaso4();
                                                                                    DataPaso5();


                                                                                    Resultado1 = richTextBox21.Text;
                                                                                    Resultado2 = richTextBox20.Text;
                                                                                    Resultado3 = richTextBox19.Text;
                                                                                    Resultado4 = richTextBox18.Text;
                                                                                    Resultado5 = richTextBox17.Text;
                                                                                    Resultado6 = richTextBox16.Text;
                                                                                    Resultado7 = richTextBox15.Text;
                                                                                    Resultado8 = richTextBox14.Text;
                                                                                    Resultado9 = richTextBox13.Text;
                                                                                    Resultado10 = richTextBox12.Text;


                                                                                    PASO3Evidencia1 = richTextBox35.Text;
                                                                                    PASO3Evidencia2 = richTextBox34.Text;
                                                                                    PASO3Evidencia3 = richTextBox33.Text;
                                                                                    PASO3Evidencia4 = richTextBox32.Text;
                                                                                    PASO3Evidencia5 = richTextBox31.Text;
                                                                                    PASO3Evidencia6 = richTextBox30.Text;
                                                                                    PASO3Evidencia7 = richTextBox29.Text;
                                                                                    PASO3Evidencia8 = richTextBox28.Text;
                                                                                    PASO3Evidencia9 = richTextBox27.Text;

                                                                                    Compentecia_Desarollo = richTextBox38.Text;
                                                                                    Comentario_Jefe = richTextBox40.Text;
                                                                                    Comentario_Empl = richTextBox41.Text;

                                                                                    label41.Text = Convert.ToString(Math.Round(PromedioPaso1, 1));
                                                                                    label42.Text = Convert.ToString(Math.Round(PromedioPASO3, 1));
                                                                                   


                                                                                    tabControl1.SelectedIndex = 4;

                if (Tipo_Conulsta == 2)
                {
                    if (EXISTE_CORP(ID_JEFE, AÑO))
                    {
                        if (Resultado_Final == "CUMPLE ALTAMENTE" || Resultado_Final == "CUMPLE")
                        {
                            groupBox23.Show();

                            MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
                            DialogResult result = MessageBox.Show("EL Empleado " + Nombre + " Cumple con los requisitos para ser emplepado CORP ," + "\n" + "¿Decea seleccionarlo como emplado CORP?", "Empleado CORP", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                            if (result == DialogResult.Yes)
                            {
                                radioButton77.Checked = true;

                            }
                            else
                            {
                                radioButton78.Checked = true;
                            }
                        }
                    }
                    else
                    {

                        groupBox23.Hide();
                        richTextBox42.Text = "";
                        groupBox24.Hide();


                        // groupBox24.Show();
                    }
                }
                else
                {
                    CRP = 3;
                }


            }



        }
        private void DataPaso1()
        {
             Notalclasf1 = 0.0;
            Notalclasf2 = 0.0;
             Notalclasf3 = 0.0;
            Notalclasf4 = 0.0;
             Notalclasf5 = 0.0;
             Notalclasf6 = 0.0;
            Notalclasf7 = 0.0;
            Notalclasf8 = 0.0;
             Notalclasf9 = 0.0;
             Notalclasf10 = 0.0;
             lineasPaso1 = 0;


            if (groupBox3.Enabled == true)
            {
               
                lineasPaso1 = lineasPaso1 + 1;
              
                if (radioButton1.Checked)
                {
                    Calificacion1 = "CA";
                    Notalclasf1 = 4.0;
                  

                     
                }
                else
                    if (radioButton2.Checked)
                    {
                        Calificacion1 = "C";
                        Notalclasf1 = 3.0;
                    }
                    else
                        if (radioButton3.Checked)
                        {
                            Calificacion1 = "ND";
                            Notalclasf1 = 2.0;
                        }
                        else if (radioButton4.Checked)
                        {
                            Calificacion1 = "BD";
                            Notalclasf1 = 1.0;
                        }
            }
           
            if (groupBox5.Enabled == true)
            {
                
                lineasPaso1 = lineasPaso1 + 1;
               

                if (radioButton8.Checked)
                {
                    Calificacion2 = "CA";
                    Notalclasf2 = 4.0;
                }
                else
                    if (radioButton7.Checked)
                    {
                        Calificacion2 = "C";
                        Notalclasf2 = 3.0;
                    }
                    else
                        if (radioButton6.Checked)
                        {
                            Calificacion2 = "ND";
                            Notalclasf2 = 2.0;
                        }
                        else if (radioButton5.Checked)
                        {
                            Calificacion2 = "BD";
                            Notalclasf2 = 1.0;
                        }
            }
           
            if (groupBox6.Enabled == true)
            {
                
                lineasPaso1 = lineasPaso1 + 1;
              

                if (radioButton12.Checked)
                {
                    Calificacion3 = "CA";
                    Notalclasf3 = 4.0;
                }
                else
                    if (radioButton11.Checked)
                    {
                        Calificacion3 = "C";
                        Notalclasf3 = 3.0;
                    }
                    else
                        if (radioButton10.Checked)
                        {
                            Calificacion3 = "ND";
                            Notalclasf3 = 2.0;
                        }
                        else if (radioButton9.Checked)
                        {
                            Calificacion3 = "BD";
                            Notalclasf3 = 1.0;
                        }
            }
        
            if (groupBox7.Enabled == true)
            {
               
                lineasPaso1 = lineasPaso1 + 1;
               
                if (radioButton16.Checked)
                {
                    Calificacion4 = "CA";
                    Notalclasf4 = 4.0;
                }
                else
                    if (radioButton15.Checked)
                    {
                        Calificacion4 = "C";
                        Notalclasf4 = 3.0;
                    }
                    else
                        if (radioButton14.Checked)
                        {
                            Calificacion4 = "ND";
                            Notalclasf4 = 2.0;
                        }
                        else if (radioButton13.Checked)
                        {
                            Calificacion4 = "BD";
                            Notalclasf4 = 1.0;
                        }
            }
          
           
            if (groupBox8.Enabled == true)
            {
                
                lineasPaso1 = lineasPaso1 + 1;
                

                if (radioButton20.Checked)
                {
                    Calificacion5 = "CA";
                    Notalclasf5 = 4.0;
                }
                else
                    if (radioButton19.Checked)
                    {
                        Calificacion5 = "C";
                        Notalclasf5 = 3.0;
                    }
                    else
                        if (radioButton18.Checked)
                        {
                            Calificacion5 = "ND";
                            Notalclasf5 = 2.0;
                        }
                        else if (radioButton17.Checked)
                        {
                            Calificacion5 = "BD";
                            Notalclasf5 = 1.0;
                        }
            }
            if (groupBox9.Enabled == true)
            {
               
                lineasPaso1 = lineasPaso1 + 1;
             

                if (radioButton24.Checked)
                {
                    Calificacion6 = "CA";
                    Notalclasf6 = 4.0;
                }
                else
                    if (radioButton23.Checked)
                    {
                        Calificacion6 = "C";
                        Notalclasf6 = 3.0;
                    }
                    else
                        if (radioButton22.Checked)
                        {
                            Calificacion6 = "ND";
                            Notalclasf6 = 2.0;
                        }
                        else if (radioButton21.Checked)
                        {
                            Calificacion6 = "BD";
                            Notalclasf6 = 1.0;
                        }

            }
            if (groupBox10.Enabled == true)
            {
               
                lineasPaso1 = lineasPaso1 + 1;
              
                if (radioButton28.Checked)
                {
                    Calificacion7 = "CA";
                    Notalclasf7 = 4.0;
                }
                else
                    if (radioButton27.Checked)
                    {
                        Calificacion7 = "C";
                        Notalclasf7 = 3.0;
                    }
                    else
                        if (radioButton26.Checked)
                        {
                            Calificacion7 = "ND";
                            Notalclasf7 = 2.0;
                        }
                        else if (radioButton25.Checked)
                        {
                            Calificacion7 = "BD";
                            Notalclasf7 = 1.0;
                        }
            }
            if (groupBox11.Enabled == true)
            {
               
                lineasPaso1 = lineasPaso1 + 1;
               
                if (radioButton32.Checked)
                {
                    Calificacion8 = "CA";
                    Notalclasf8 = 4.0;
                }
                else
                    if (radioButton31.Checked)
                    {
                        Calificacion8 = "C";
                        Notalclasf8 = 3.0;
                    }
                    else
                        if (radioButton30.Checked)
                        {
                            Calificacion8 = "ND";
                            Notalclasf8 = 2.0;
                        }
                        else if (radioButton29.Checked)
                        {
                            Calificacion8 = "BD";
                            Notalclasf8 = 1.0;
                        }
            }
            if (groupBox12.Enabled == true)
            {
                
                lineasPaso1 = lineasPaso1 + 1;
                

                if (radioButton36.Checked)
                {
                    Calificacion9 = "CA";
                    Notalclasf9 = 4.0;
                }
                else
                    if (radioButton35.Checked)
                    {
                        Calificacion9 = "C";
                        Notalclasf9 = 3.0;
                    }
                    else
                        if (radioButton34.Checked)
                        {
                            Calificacion9 = "ND";
                            Notalclasf9 = 2.0;
                        }
                        else if (radioButton33.Checked)
                        {
                            Calificacion9 = "BD";
                            Notalclasf9 = 1.0;
                        }
            }
            if (groupBox13.Enabled == true)
            {
                
                lineasPaso1 = lineasPaso1 + 1;
               

                if (radioButton40.Checked)
                {
                    Calificacion10 = "CA";
                    Notalclasf10 = 4.0;
                }
                else
                    if (radioButton39.Checked)
                    {
                        Calificacion10 = "C";
                        Notalclasf10 = 3.0;
                    }
                    else
                        if (radioButton38.Checked)
                        {
                            Calificacion10 = "ND";
                            Notalclasf10 = 2.0;
                        }
                        else if (radioButton37.Checked)
                        {
                            Calificacion9 = "BD";
                            Notalclasf10 = 1.0;
                        }
            }
        
        }
        private void DataPaso2()
        {
            if (String.IsNullOrEmpty(richTextBox26.Text))
            {
               
            }
            else
            {
                Crecimiento = richTextBox26.Text;
               
            }

            if (String.IsNullOrEmpty(richTextBox25.Text))
            {
                
            }
            else
            {

                Organizacion = richTextBox25.Text;
            }
            if (String.IsNullOrEmpty(richTextBox24.Text))
            {
               
            }
            else
            {
                Responsabilidad = richTextBox24.Text;
               
            }
            if (String.IsNullOrEmpty(richTextBox23.Text))
            {
               
            }
            else
            {

                Productividad = richTextBox23.Text;
            }

        
        }
        private void Calculos()
        {
            //Clasificacion1 
            PromedioPaso1 = 0.0;
            PromedioPASO3 = 0.0;
            TotalPASO3 = 0.0;
            NotalPaso1 = 0.0;
            LineaPASO3 = 9;
            NotalPaso1 = (Notalclasf1 + Notalclasf2 + Notalclasf3 + Notalclasf4 + Notalclasf5 + Notalclasf6 + Notalclasf7 + Notalclasf8 + Notalclasf9 + Notalclasf10);
            TotalPASO3 = (PASO3nota1 + PASO3nota2 + PASO3nota3 + PASO3nota4 + PASO3nota5 + PASO3nota6 + PASO3nota7 + PASO3nota8 + PASO3nota9);

            
            if (lineasPaso1 > 0)
            {
                PromedioPaso1 = NotalPaso1 / lineasPaso1;
            }
            else
            {
                MessageBox.Show("No se encontraron Objetivos en  PASO 1");
            }

            if (LineaPASO3 > 0)
            {
                PromedioPASO3 = TotalPASO3 / LineaPASO3;

            }
            else
            {
                MessageBox.Show("No se encontro Calificacion en  Evidencias en PASO 3");
                richTextBox35.Focus();
              
            }

         
        }
        private void Resultado()
        {

            //PASO1

            if (PromedioPaso1 >= 0.1 && PromedioPaso1 <= 1.89)
            {
                ResultadoCL1 = "BAJO DESEMPEÑO";
            }
            else if (PromedioPaso1 >= 1.90 && PromedioPaso1 <= 2.89)
            {
                ResultadoCL1 = "NECESITA DESARROLLO";

            }
            else if (PromedioPaso1 >= 2.90 && PromedioPaso1 <= 3.89)
            {
                ResultadoCL1 = "CUMPLE";

            }
            else if (PromedioPaso1 >= 3.9)
            {
                ResultadoCL1 = "CUMPLE ALTAMENTE";

            }

            // -- PASO 3

            if (PromedioPASO3 >= 0.1 && PromedioPASO3 <= 1.89)
            {
                ResultadoCL3 = "BAJO DESEMPEÑO";
            }
            else if (PromedioPASO3 >= 1.90 && PromedioPASO3 <= 2.89)
            {
                ResultadoCL3 = "NECESITA DESARROLLO";

            }
            else if (PromedioPASO3 >= 2.90 && PromedioPASO3 <= 3.89)
            {
                ResultadoCL3 = "CUMPLE";

            }
            else if (PromedioPASO3 >= 3.9)
            {
                ResultadoCL3 = "CUMPLE ALTAMENTE";

            }
            //---
            
            label33.Text = ResultadoCL1;
            label34.Text = ResultadoCL3;

            PromedioFinal = (PromedioPaso1 + PromedioPASO3) / 2;

            label32.Text = Convert.ToString(Math.Round(PromedioFinal,1));

// Promedio Final 

            if (PromedioFinal >= 0.1 && PromedioFinal <= 1.89)
            {
                Resultado_Final= "BAJO DESEMPEÑO";
            }
            else if (PromedioFinal >= 1.90 && PromedioFinal <= 2.89)
            {
                Resultado_Final = "NECESITA DESARROLLO";

            }
            else if (PromedioFinal >= 2.90 && PromedioFinal <= 3.89)
            {
                Resultado_Final = "CUMPLE";

            }
            else if (PromedioFinal >= 3.9)
            {
                Resultado_Final = "CUMPLE ALTAMENTE";

            }


            label35.Text = Resultado_Final;


          

            button2.Enabled = true;

        }
        private void DataPaso3()
        {
            PASO3nota1 = 0.0;
             PASO3nota2 = 0.0;
           PASO3nota3 = 0.0;
           PASO3nota4 = 0.0;
          PASO3nota5 = 0.0;
         PASO3nota7 = 0.0;
             PASO3nota8 = 0.0;
           PASO3nota9 = 0.0;
           LineaPASO3 = 0;

            //if (groupBox22.Enabled == true)
            //{
                
            //    LineaPASO3 = LineaPASO3 + 1;
              
                if (radioButton76.Checked)
                {
                    PASO3calificacion1 = "CA";
                 PASO3nota1 = 4.0;

                }
                else
                    if (radioButton75.Checked)
                    {
                        PASO3calificacion1 = "C";
                        PASO3nota1 = 3.0;
                    }
                    else
                        if (radioButton74.Checked)
                        {
                            PASO3calificacion1 = "ND";
                            PASO3nota1 = 2.0;
                        }
                        else if (radioButton73.Checked)
                        {
                            PASO3calificacion1 = "BD";
                            PASO3nota1 = 1.0;
                        }
            //}


            //if (groupBox21.Enabled == true)
            //{
                
            //    LineaPASO3 = LineaPASO3 + 1;
              
                if (radioButton72.Checked)
                {
                    PASO3calificacion2 = "CA";
                    PASO3nota2 = 4.0;

                }
                else
                    if (radioButton71.Checked)
                    {
                        PASO3calificacion2 = "C";
                        PASO3nota2 = 3.0;
                    }
                    else
                        if (radioButton70.Checked)
                        {
                            PASO3calificacion2 = "ND";
                            PASO3nota2 = 2.0;
                        }
                        else if (radioButton69.Checked)
                        {
                            PASO3calificacion2 = "BD";
                            PASO3nota2 = 1.0;
                        }
            //}


            //if (groupBox20.Enabled == true)
            //{
                
            //    LineaPASO3 = LineaPASO3 + 1;

                if (radioButton68.Checked)
                {
                    PASO3calificacion3 = "CA";
                    PASO3nota3 = 4.0;

                }
                else
                    if (radioButton67.Checked)
                    {
                        PASO3calificacion3 = "C";
                        PASO3nota3 = 3.0;
                    }
                    else
                        if (radioButton66.Checked)
                        {
                            PASO3calificacion3 = "ND";
                            PASO3nota3 = 2.0;
                        }
                        else if (radioButton65.Checked)
                        {
                            PASO3calificacion3 = "BD";
                            PASO3nota3 = 1.0;
                        }
            //}


            //if (groupBox19.Enabled == true)
            //{
                
                //LineaPASO3 = LineaPASO3 + 1;

                if (radioButton64.Checked)
                {
                    PASO3calificacion4 = "CA";
                    PASO3nota4 = 4.0;

                }
                else
                    if (radioButton63.Checked)
                    {
                        PASO3calificacion4 = "C";
                        PASO3nota4 = 3.0;
                    }
                    else
                        if (radioButton62.Checked)
                        {
                            PASO3calificacion4 = "ND";
                            PASO3nota4 = 2.0;
                        }
                        else if (radioButton61.Checked)
                        {
                            PASO3calificacion4 = "BD";
                            PASO3nota4 = 1.0;
                        }
            //}


            // --- 
            //if (groupBox18.Enabled == true)
            //{
              
            //    LineaPASO3 = LineaPASO3 + 1;

                if (radioButton60.Checked)
                {
                    PASO3calificacion5 = "CA";
                    PASO3nota5 = 4.0;

                }
                else
                    if (radioButton59.Checked)
                    {
                        PASO3calificacion5 = "C";
                        PASO3nota5 = 3.0;
                    }
                    else
                        if (radioButton58.Checked)
                        {
                            PASO3calificacion5 = "ND";
                            PASO3nota5 = 2.0;
                        }
                        else if (radioButton57.Checked)
                        {
                            PASO3calificacion5 = "BD";
                            PASO3nota5 = 1.0;
                        }
            //}

            //--17

            //if (groupBox17.Enabled == true)
            //{
              
            //    LineaPASO3 = LineaPASO3 + 1;
              
                if (radioButton56.Checked)
                {
                    PASO3calificacion6 = "CA";
                    PASO3nota6 = 4.0;

                }
                else
                    if (radioButton55.Checked)
                    {
                        PASO3calificacion6 = "C";
                        PASO3nota6 = 3.0;
                    }
                    else
                        if (radioButton54.Checked)
                        {
                            PASO3calificacion6 = "ND";
                            PASO3nota6 = 2.0;
                        }
                        else if (radioButton53.Checked)
                        {
                            PASO3calificacion6 = "BD";
                            PASO3nota6 = 1.0;
                        }
            //}

            //-- 16

            //if (groupBox16.Enabled == true)
            //{
              
            //    LineaPASO3 = LineaPASO3 + 1;
               
                if (radioButton52.Checked)
                {
                    PASO3calificacion7 = "CA";
                    PASO3nota7 = 4.0;

                }
                else
                    if (radioButton51.Checked)
                    {
                        PASO3calificacion7 = "C";
                        PASO3nota7 = 3.0;
                    }
                    else
                        if (radioButton50.Checked)
                        {
                            PASO3calificacion7 = "ND";
                            PASO3nota7 = 2.0;
                        }
                        else if (radioButton49.Checked)
                        {
                            PASO3calificacion7 = "BD";
                            PASO3nota7 = 1.0;
                        }
            //}


            //--15

            //if (groupBox15.Enabled == true)
            //{
               
            //    LineaPASO3 = LineaPASO3 + 1;

                if (radioButton48.Checked)
                {
                    PASO3calificacion8 = "CA";
                    PASO3nota8 = 4.0;

                }
                else
                    if (radioButton47.Checked)
                    {
                        PASO3calificacion8 = "C";
                        PASO3nota8 = 3.0;
                    }
                    else
                        if (radioButton46.Checked)
                        {
                            PASO3calificacion8 = "ND";
                            PASO3nota8 = 2.0;
                        }
                        else if (radioButton45.Checked)
                        {
                            PASO3calificacion8 = "BD";
                            PASO3nota8 = 1.0;
                        }
            //}

            //-- 14 

            //if (groupBox14.Enabled == true)
            //{
              
            //    LineaPASO3 = LineaPASO3 + 1;
                
                if (radioButton44.Checked)
                {
                    PASO3calificacion9 = "CA";
                    PASO3nota9 = 4.0;

                }
                else
                    if (radioButton43.Checked)
                    {
                        PASO3calificacion9 = "C";
                        PASO3nota9 = 3.0;
                    }
                    else
                        if (radioButton42.Checked)
                        {
                            PASO3calificacion9 = "ND";
                            PASO3nota9 = 2.0;
                        }
                        else if (radioButton41.Checked)
                        {
                            PASO3calificacion9 = "BD";
                            PASO3nota9 = 1.0;
                        }
                        //}
        }
        private void DataPaso4()
        {

            if (String.IsNullOrEmpty(richTextBox26.Text))
            {

            }
            else
            {
               Compentecia_Desarollo = richTextBox38.Text;

            }
        
        }
        private void DataPaso5()
        {

            if (String.IsNullOrEmpty(richTextBox40.Text))
            {

            }
            else
            {
                Comentario_Jefe = richTextBox40.Text;

            }

            if (String.IsNullOrEmpty(richTextBox41.Text))
            {

            }
            else
            {
                Comentario_Empl = richTextBox41.Text;

            }

        }

        private void richTextBox35_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox35.Text))
            {
                if (Tipo_Conulsta == 1 || Tipo_Conulsta == 2)
                {
                   
                    LineaPASO3 = LineaPASO3 + 1;
                    groupBox22.Enabled = true;
                }
              
            }

          
            else
            {

                groupBox22.Enabled = true;
                
                radioButton73.Checked = false;
                radioButton74.Checked = false;
                radioButton75.Checked = true;
                radioButton76.Checked = false;

                if (LineaPASO3 != 0)
                {
                    LineaPASO3 = LineaPASO3 - 1;

                }

            }
        }

        private void richTextBox34_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox34.Text))
            {
                if (Tipo_Conulsta == 1 || Tipo_Conulsta == 2)
                {

                    LineaPASO3 = LineaPASO3 + 1;
                    groupBox21.Enabled = true;

                }
            }
          
            else
            {

                groupBox21.Enabled = true;
                radioButton69.Checked = false;
                radioButton70.Checked = false;
                radioButton71.Checked = true;
                radioButton72.Checked = false;
              

                if (LineaPASO3 != 0)
                {
                    LineaPASO3 = LineaPASO3 - 1;

                }
            }

        }

        private void richTextBox33_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox33.Text))
            {
                if (Tipo_Conulsta == 1 || Tipo_Conulsta == 2)
                {

                    LineaPASO3 = LineaPASO3 + 1;
                    groupBox20.Enabled = true;

                }
            }

           
            else
            {

                groupBox20.Enabled = true;
                radioButton65.Checked = false;
                radioButton66.Checked = false;
                radioButton67.Checked = false;
                radioButton68.Checked = false;

                if (LineaPASO3 != 0)
                {
                    LineaPASO3 = LineaPASO3 - 1;

                }
            }
        }

        private void richTextBox32_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox32.Text))
            {
                if (Tipo_Conulsta == 1 || Tipo_Conulsta == 2)
                {

                    LineaPASO3 = LineaPASO3 + 1;
                    groupBox19.Enabled = true;

                }
            }

         
            else
            {

                groupBox19.Enabled = true;
                radioButton61.Checked = false;
                radioButton62.Checked = false;
                radioButton63.Checked = false;
                radioButton64.Checked = false;

                if (LineaPASO3 != 0)
                {
                    LineaPASO3 = LineaPASO3 - 1;

                }

            }
        }

        private void richTextBox31_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox31.Text))
            {
                if (Tipo_Conulsta == 1 || Tipo_Conulsta == 2)
                {

                    LineaPASO3 = LineaPASO3 + 1;
                    groupBox18.Enabled = true;

                }
            }

          
            else
            {

                groupBox18.Enabled = true;
                radioButton60.Checked = false;
                radioButton59.Checked = false;
                radioButton58.Checked = false;
                radioButton57.Checked = false;

                if (LineaPASO3 != 0)
                {
                    LineaPASO3 = LineaPASO3 - 1;

                }
            }
        }

        private void richTextBox30_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox30.Text))
            {
                if (Tipo_Conulsta == 1 || Tipo_Conulsta == 2)
                {

                    LineaPASO3 = LineaPASO3 + 1;

                    groupBox17.Enabled = true;
                } 
            }

          
            else
            {

                groupBox17.Enabled = true;
                radioButton56.Checked = false;
                radioButton55.Checked = false;
                radioButton54.Checked = false;
                radioButton53.Checked = false;
                if (LineaPASO3 != 0)
                {
                    LineaPASO3 = LineaPASO3 - 1;

                }
            }
        }

        private void richTextBox29_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox29.Text))
            {
                if (Tipo_Conulsta == 1 || Tipo_Conulsta == 2)
                {

                    LineaPASO3 = LineaPASO3 + 1;
                    groupBox16.Enabled = true;
                }
               
            }

         
            else
            {

                groupBox16.Enabled = true;
                radioButton52.Checked = false;
                radioButton51.Checked = false;
                radioButton50.Checked = false;
                radioButton49.Checked = false;

                if (LineaPASO3 != 0)
                {
                    LineaPASO3 = LineaPASO3 - 1;

                }

            }
        }


        private void richTextBox28_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox28.Text))
            {
                if (Tipo_Conulsta == 1 || Tipo_Conulsta == 2)
                {

                    LineaPASO3 = LineaPASO3 + 1;
                    groupBox15.Enabled = true;
                }
               
            }

          
            else
            {

                groupBox15.Enabled = true;
                radioButton48.Checked = false;
                radioButton47.Checked = false;
                radioButton46.Checked = false;
                radioButton45.Checked = false;

                if (LineaPASO3 != 0)
                {
                    LineaPASO3 = LineaPASO3 - 1;

                }

            }
        }

        private void richTextBox27_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox27.Text))
            {
                if (Tipo_Conulsta == 1 || Tipo_Conulsta == 2)
                {

                    LineaPASO3 = LineaPASO3 + 1;
                    groupBox14.Enabled = true;

                }
            }


           
            else
            {
                groupBox14.Enabled = true;
                radioButton44.Checked = false;
                radioButton43.Checked = false;
                radioButton42.Checked = false;
                radioButton41.Checked = false;

                if (LineaPASO3 != 0)
                {
                    LineaPASO3 = LineaPASO3 - 1;

                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tabControl1.SelectedIndex = 3;
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tabControl1.SelectedIndex = 4;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (CRP == 3)
            {
                CORP_CLIE = "N/A";
                COMENT_CORP_CLIE = "";
                insercion();


            }

            if (CRP == 0)
            {
                CORP_CLIE = "N";
                COMENT_CORP_CLIE = "";
                insercion();


            }

            else

                if (CRP == 1)
            {
                if (richTextBox42.Text == "" || richTextBox42.Text == string.Empty || richTextBox42.Text == null)
                {
                    MessageBox.Show("Debe escribir un comentario para el empleado CORP", "Empleado CORP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    CORP_CLIE = "S";
                    COMENT_CORP_CLIE = richTextBox42.Text;
                    insercion();
                }

            }
            else if (CRP == 2)
            {
                CORP_CLIE = "N";
                COMENT_CORP_CLIE = "";
                insercion();
            }


        }


        private void Limpieza_datos()
        {
            Nombre = "";
            ID_EVALUACION = 0;
            Departamento = "";
            Jefe_Inmediato = "";
            Cargo = "";
            Cod_Empleado = "";

            objetivo1 = ""; Calificacion1 = ""; PASO3calificacion1 = "";
            objetivo2 = ""; Calificacion2 = ""; PASO3calificacion2 = "";
            objetivo3 = ""; Calificacion3 = ""; PASO3calificacion3 = "";
            objetivo4 = ""; Calificacion4 = ""; PASO3calificacion4 = "";
            objetivo5 = ""; Calificacion5 = ""; PASO3calificacion5 = "";
            objetivo6 = ""; Calificacion6 = ""; PASO3calificacion6 = "";
            objetivo7 = ""; Calificacion7 = ""; PASO3calificacion7 = "";
            objetivo8 = ""; Calificacion8 = ""; PASO3calificacion8 = "";
            objetivo9 = ""; Calificacion9 = ""; PASO3calificacion9 = "";
            objetivo10 = ""; Calificacion10 = "";
            Resultado1 = ""; Notalclasf1 = 0; EVIDENCIA_INSERT = "";
            Resultado2 = ""; Notalclasf2 = 0; P3CALIFI_INSERT = "";
            Resultado3 = ""; Notalclasf3 = 0; TotalPASO3 = 0.0;
            Resultado4 = ""; Notalclasf4 = 0; PromedioFinal = 0.0;
            Resultado5 = ""; Notalclasf5 = 0; Resultado_Final = "";
            Resultado6 = ""; Notalclasf6 = 0;
            Resultado7 = ""; Notalclasf7 = 0;
            Resultado8 = ""; Notalclasf8 = 0;
            Resultado9 = ""; Notalclasf9 = 0;
            Resultado10 = "";Notalclasf10 = 0;

            Crecimiento = ""; ResultadoCL1 = ""; PASO3Evidencia8 = ""; PASO3nota8 = 0.0;
            Organizacion = ""; ResultadoCL3 = ""; PASO3Evidencia9 = ""; PASO3nota9 = 0.0;
            Responsabilidad = ""; PASO3Evidencia1 = ""; PASO3nota1 = 0.0;
            Productividad = ""; PASO3Evidencia2 = ""; PASO3nota2 = 0.0;
            Comentario_Jefe = ""; PASO3Evidencia3 = ""; PASO3nota3 = 0.0;
            Comentario_Empl = ""; PASO3Evidencia4 = ""; PASO3nota4 = 0.0;
            Compentecia_Desarollo = ""; PASO3Evidencia5 = ""; PASO3nota5 = 0.0;
            lineasPaso1 = 0; PASO3Evidencia6 = ""; PASO3nota6 = 0.0;
            NotalPaso1 = 0.0; PASO3Evidencia7 = ""; PASO3nota7 = 0.0;

            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";

            richTextBox2.Text = ""; richTextBox3.Text = ""; richTextBox3.Text = ""; richTextBox4.Text = ""; richTextBox5.Text = ""; richTextBox6.Text = ""; richTextBox7.Text = ""; richTextBox8.Text = ""; richTextBox9.Text = ""; richTextBox10.Text = "";
            richTextBox11.Text = ""; richTextBox12.Text = ""; richTextBox13.Text = ""; richTextBox14.Text = ""; richTextBox15.Text = ""; richTextBox16.Text = ""; richTextBox17.Text = ""; richTextBox18.Text = ""; richTextBox19.Text = ""; richTextBox20.Text = "";
            richTextBox21.Text = ""; richTextBox23.Text = ""; richTextBox24.Text = ""; richTextBox25.Text = ""; richTextBox26.Text = ""; richTextBox27.Text = ""; richTextBox28.Text = ""; richTextBox29.Text = ""; richTextBox30.Text = ""; richTextBox31.Text = "";
            richTextBox32.Text = ""; richTextBox33.Text = ""; richTextBox34.Text = ""; richTextBox35.Text = ""; richTextBox38.Text = ""; richTextBox40.Text = ""; richTextBox41.Text = "";
            label32.Text = ""; label33.Text = ""; label34.Text = ""; label35.Text="";



    
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            con.conectar("EX");
            SqlCommand cm1 = new SqlCommand("SELECT  A.[EMPLEADO],A.[NOMBRE],B.DESCRIPCION as PUESTO ,C.DESCRIPCION as DEPARTAMENTO FROM [EXACTUS].[dismo].[EMPLEADO] A  INNER JOIN [EXACTUS].[dismo].[PUESTO] B  ON A.PUESTO = B.PUESTO INNER JOIN  [EXACTUS].[dismo].[DEPARTAMENTO] C  on A.DEPARTAMENTO = C.DEPARTAMENTO  where A.NOMBRE = '"+textBox1.Text+"'", con.conex);
            SqlDataReader dr1 = cm1.ExecuteReader();
            while (dr1.Read())
            {
                textBox5.Text = Convert.ToString(dr1["EMPLEADO"]);
                textBox3.Text = Convert.ToString(dr1["DEPARTAMENTO"]);
                textBox2.Text = Convert.ToString(dr1["PUESTO"]);

            }
            con.Desconectar("EX");

        }

        private void textBox5_Leave(object sender, EventArgs e)
        {

            con.conectar("EX");
            SqlCommand cm1 = new SqlCommand("SELECT  A.[EMPLEADO],A.[NOMBRE],B.DESCRIPCION as PUESTO ,C.DESCRIPCION as DEPARTAMENTO FROM [EXACTUS].[dismo].[EMPLEADO] A  INNER JOIN [EXACTUS].[dismo].[PUESTO] B  ON A.PUESTO = B.PUESTO INNER JOIN  [EXACTUS].[dismo].[DEPARTAMENTO] C  on A.DEPARTAMENTO = C.DEPARTAMENTO  where A.EMPLEADO = '" + textBox5.Text + "'", con.conex);
            SqlDataReader dr1 = cm1.ExecuteReader();
            while (dr1.Read())
            {
                textBox1.Text = Convert.ToString(dr1["NOMBRE"]);
                textBox3.Text = Convert.ToString(dr1["DEPARTAMENTO"]);
                textBox2.Text = Convert.ToString(dr1["PUESTO"]);

            }
            con.Desconectar("EX");
        }



        private void carga_objetivos()
        {

            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("SELECT  [ID_EVALUACION],[OBJETIVO_NUMERO],[OBJETIVO],[FECHA_INGRESO],[USUARIO_INGRESO] FROM [DM].[CORRECT].[OBJETIVOS_EVALUACION] where [ID_EVALUACION] = '"+ID_EVALUACION+"'", con.condm);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ObjetivosDT);

            con.Desconectar("DM");

            for (int i = 0; i < ObjetivosDT.Rows.Count; i++)
            {
                int id_objetivo;
                DataRow row = ObjetivosDT.Rows[i];

                id_objetivo = Convert.ToInt32(row["OBJETIVO_NUMERO"]);

                switch (id_objetivo)
                {
                
                    case 1:
                        richTextBox2.Text = Convert.ToString(row["OBJETIVO"]);
                        if (Tipo_Conulsta == 1 || Tipo_Conulsta == 2)
                        {
                            richTextBox21.ReadOnly = false;
                        }
                    

                        break;
                   case 2:
                        richTextBox3.Text = Convert.ToString(row["OBJETIVO"]);

                        if (Tipo_Conulsta == 1 || Tipo_Conulsta == 2)
                        {
                            richTextBox20.ReadOnly = false;
                        }
                      

                        break;
                     case 3:
                        richTextBox4.Text = Convert.ToString(row["OBJETIVO"]);

                        if (Tipo_Conulsta == 1 || Tipo_Conulsta == 2)
                        {
                            richTextBox19.ReadOnly = false;
                        }
                        else
                        {
                            richTextBox19.ReadOnly = true;
                        }
                        break;
                     case 4:
                        richTextBox5.Text = Convert.ToString(row["OBJETIVO"]);

                        if (Tipo_Conulsta == 1 || Tipo_Conulsta == 2)
                        {
                            richTextBox18.ReadOnly = false;
                        }
                        else
                        {
                            richTextBox18.ReadOnly = true;
                        }
                        break;
                     case 5:
                        richTextBox6.Text = Convert.ToString(row["OBJETIVO"]);

                        if (Tipo_Conulsta == 1 || Tipo_Conulsta == 2)
                        {
                            richTextBox17.ReadOnly = false;
                        }
                        else
                        {
                            richTextBox17.ReadOnly = true;
                        }
                        break;
                     case 6:
                        richTextBox7.Text = Convert.ToString(row["OBJETIVO"]);

                        if (Tipo_Conulsta == 1 || Tipo_Conulsta == 2)
                        {
                            richTextBox16.ReadOnly = false;
                        }
                        else
                        {
                            richTextBox16.ReadOnly = true;
                        }
                        break;
                     case 7:
                        richTextBox8.Text = Convert.ToString(row["OBJETIVO"]);

                        if (Tipo_Conulsta == 1 || Tipo_Conulsta == 2)
                        {
                            richTextBox15.ReadOnly = false;
                        }
                        else
                        {
                            richTextBox15.ReadOnly = true;
                        }
                        break;
                     case 8:
                        richTextBox9.Text = Convert.ToString(row["OBJETIVO"]);

                        if (Tipo_Conulsta == 1 || Tipo_Conulsta == 2)
                        {
                            richTextBox14.ReadOnly = false;
                        }
                        else
                        {
                            richTextBox14.ReadOnly = true;
                        }
                        break;
                     case 9:
                        richTextBox10.Text = Convert.ToString(row["OBJETIVO"]);

                        if (Tipo_Conulsta == 1 || Tipo_Conulsta == 2)
                        {
                            richTextBox13.ReadOnly = false;
                        }
                        else
                        {
                            richTextBox13.ReadOnly = true;
                        }
                        break;
                     case 10:
                        richTextBox11.Text = Convert.ToString(row["OBJETIVO"]);

                        if (Tipo_Conulsta == 1 || Tipo_Conulsta == 2)
                        {
                            richTextBox12.ReadOnly = false;
                        }
                        else
                        {
                            richTextBox12.ReadOnly = true;
                        }
                        break;
                
                }


            }




        
        }

        private void carga_preliminar()
        { 
        
            //PASO 1 -------


            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("SELECT [ID_EVALUACION],[ID_OBJETIVO],[DESCRIPCION],[CALIFICACION],[USUARIO_CREA],[FECHA_CREA] FROM [DM].[CORRECT].[RESULTADOS_EVALUACION_PRELIMINAR] where [ID_EVALUACION] = '" + ID_EVALUACION + "'", con.condm);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(PreliminarPaso1);

            con.Desconectar("DM");

            for (int i = 0; i < PreliminarPaso1.Rows.Count; i++)
            {
                int id_Preliminar;
                string calilific_preliminar;
                DataRow row = PreliminarPaso1.Rows[i];

                id_Preliminar = Convert.ToInt32(row["ID_OBJETIVO"]);
                calilific_preliminar = Convert.ToString(row["CALIFICACION"]);
                switch (id_Preliminar)
                {

                    case 1:
                        richTextBox21.Text = Convert.ToString(row["DESCRIPCION"]);
                        richTextBox21.Enabled = true;
                        groupBox3.Enabled = true;
                        switch (calilific_preliminar)
                        { 
                            case "CA":
                                radioButton1.Checked=true;
                                break;
                            case "C":
                                radioButton2.Checked = true;
                                break;
                            case "ND":
                                radioButton3.Checked = true;
                                break;
                            case "BD":
                                radioButton4.Checked = true;
                                break;
                        }
                        break;

                    case 2:
                        richTextBox20.Text = Convert.ToString(row["DESCRIPCION"]);
                       
                            groupBox5.Enabled = true;
                            richTextBox20.Enabled = true;
                       
                     
                        switch (calilific_preliminar)
                        { 
                            case "CA":
                                radioButton8.Checked=true;
                                break;
                            case "C":
                                radioButton7.Checked = true;
                                break;
                            case "ND":
                                radioButton6.Checked = true;
                                break;
                            case "BD":
                                radioButton5.Checked = true;
                                break;
                        }

                        break;
                    case 3:
                      richTextBox19.Text = Convert.ToString(row["DESCRIPCION"]);
                        groupBox6.Enabled = true;
                        richTextBox19.Enabled = true;
                        switch (calilific_preliminar)
                        { 
                            case "CA":
                                radioButton12.Checked=true;
                                break;
                            case "C":
                                radioButton11.Checked = true;
                                break;
                            case "ND":
                                radioButton10.Checked = true;
                                break;
                            case "BD":
                                radioButton9.Checked = true;
                                break;
                        }

                        break;
                    case 4:
                        richTextBox18.Text = Convert.ToString(row["DESCRIPCION"]);
                        groupBox7.Enabled = true;
                        richTextBox18.Enabled = true;
                        switch (calilific_preliminar)
                        { 
                            case "CA":
                                radioButton16.Checked=true;
                                break;
                            case "C":
                                radioButton15.Checked = true;
                                break;
                            case "ND":
                                radioButton14.Checked = true;
                                break;
                            case "BD":
                                radioButton13.Checked = true;
                                break;
                        }

                        break;
                    case 5:
                       richTextBox17.Text = Convert.ToString(row["DESCRIPCION"]);
                        groupBox8.Enabled = true;
                        richTextBox18.Enabled = true;
                        switch (calilific_preliminar)
                        { 
                            case "CA":
                                radioButton20.Checked=true;
                                break;
                            case "C":
                                radioButton19.Checked = true;
                                break;
                            case "ND":
                                radioButton18.Checked = true;
                                break;
                            case "BD":
                                radioButton17.Checked = true;
                                break;
                        }

                        break;
                    case 6:
                       richTextBox16.Text = Convert.ToString(row["DESCRIPCION"]);
                        groupBox9.Enabled = true;
                        richTextBox16.Enabled = true;
                        switch (calilific_preliminar)
                        { 
                            case "CA":
                                radioButton24.Checked=true;
                                break;
                            case "C":
                                radioButton23.Checked = true;
                                break;
                            case "ND":
                                radioButton22.Checked = true;
                                break;
                            case "BD":
                                radioButton21.Checked = true;
                                break;
                        }

                        break;
                    case 7:
                      richTextBox15.Text = Convert.ToString(row["DESCRIPCION"]);
                        groupBox10.Enabled = true;
                        richTextBox15.Enabled = true;
                        switch (calilific_preliminar)
                        { 
                            case "CA":
                                radioButton28.Checked=true;
                                break;
                            case "C":
                                radioButton27.Checked = true;
                                break;
                            case "ND":
                                radioButton26.Checked = true;
                                break;
                            case "BD":
                                radioButton25.Checked = true;
                                break;
                        }
                        break;
                    case 8:
                        richTextBox14.Text = Convert.ToString(row["DESCRIPCION"]);
                        groupBox11.Enabled = true;
                        richTextBox14.Enabled = true;
                        switch (calilific_preliminar)
                        { 
                            case "CA":
                                radioButton32.Checked=true;
                                break;
                            case "C":
                                radioButton31.Checked = true;
                                break;
                            case "ND":
                                radioButton30.Checked = true;
                                break;
                            case "BD":
                                radioButton29.Checked = true;
                                break;
                        }
                        break;
                    case 9:
                        richTextBox13.Text = Convert.ToString(row["DESCRIPCION"]);
                        groupBox12.Enabled = true;
                        richTextBox13.Enabled = true;
                        switch (calilific_preliminar)
                        { 
                            case "CA":
                                radioButton36.Checked=true;
                                break;
                            case "C":
                                radioButton35.Checked = true;
                                break;
                            case "ND":
                                radioButton34.Checked = true;
                                break;
                            case "BD":
                                radioButton33.Checked = true;
                                break;
                        }
                        break;
                    case 10:
                       richTextBox12.Text = Convert.ToString(row["DESCRIPCION"]);
                        groupBox13.Enabled = true;
                        richTextBox12.Enabled = true;
                        switch (calilific_preliminar)
                        { 
                            case "CA":
                                radioButton40.Checked=true;
                                break;
                            case "C":
                                radioButton39.Checked = true;
                                break;
                            case "ND":
                                radioButton38.Checked = true;
                                break;
                            case "BD":
                                radioButton37.Checked = true;
                                break;
                        }
                        break;

                }


            }
            





            //PASO2--------------------------

            con.conectar("DM");
            SqlCommand cmd1 = new SqlCommand("SELECT  [ID_EVALUACION],[CRECIMIENTO],[ORGANIZCION],[RESPONSABILIDAD],[PRODUCTIVIDAD],[FECHA_CREA],[USUARIO_CREA]  FROM [DM].[CORRECT].[RESULTADO_ESTRATEGIA_PRELIMINAR] where [ID_EVALUACION] = '" + ID_EVALUACION + "'", con.condm);
            SqlDataReader dr = cmd1.ExecuteReader();

            while (dr.Read())
            {
                richTextBox26.Text = Convert.ToString(dr["CRECIMIENTO"]);
                richTextBox25.Text = Convert.ToString(dr["ORGANIZCION"]);
                richTextBox24.Text = Convert.ToString(dr["RESPONSABILIDAD"]);
                richTextBox23.Text = Convert.ToString(dr["PRODUCTIVIDAD"]);


            }
            dr.Close();
            con.Desconectar("DM");
            richTextBox26.ReadOnly = false;
            richTextBox25.ReadOnly = false;
            richTextBox24.ReadOnly = false;
            richTextBox23.ReadOnly = false;
          
            // PASO 3----------------------------

               con.conectar("DM");
            SqlCommand cmd2 = new SqlCommand("SELECT [ID_EVALUACION],[COD_TRABAJO],[EVIDENCIA],[CALIFICACION],[FECHA_INGRESO],[USUARIO_INGRESO] FROM [DM].[CORRECT].[EVALUACION_COD_TRABAJO_PRELIMINAR] where [ID_EVALUACION] = '" + ID_EVALUACION + "'", con.condm);
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            da2.Fill(PreliminarPaso3);

            con.Desconectar("DM");

            for (int i = 0; i < PreliminarPaso3.Rows.Count; i++)
            {
                int id_Preliminar3;
                string calilific_preliminar3;
                DataRow row = PreliminarPaso3.Rows[i];

                id_Preliminar3 = Convert.ToInt32(row["COD_TRABAJO"]);
                calilific_preliminar3 = Convert.ToString(row["CALIFICACION"]);
                switch (id_Preliminar3)
                {

                    case 1:
                        richTextBox35.Text = Convert.ToString(row["EVIDENCIA"]);
                        richTextBox35.ReadOnly = false;
                        groupBox22.Enabled = true;
                        switch (calilific_preliminar3)
                        {
                            case "CA":
                                radioButton76.Checked = true;
                                break;
                            case "C":
                                radioButton75.Checked = true;
                                break;
                            case "ND":
                                radioButton74.Checked = true;
                                break;
                            case "BD":
                                radioButton73.Checked = true;
                                break;
                        }
                        break;
                    case 2:
                        richTextBox34.Text = Convert.ToString(row["EVIDENCIA"]);
                        richTextBox34.ReadOnly = false;
                        groupBox21.Enabled = true;
                        switch (calilific_preliminar3)
                        {
                            case "CA":
                                radioButton72.Checked = true;
                                break;
                            case "C":
                                radioButton71.Checked = true;
                                break;
                            case "ND":
                                radioButton70.Checked = true;
                                break;
                            case "BD":
                                radioButton69.Checked = true;
                                break;
                        }
                        break;

                    case 3:
                        richTextBox33.Text = Convert.ToString(row["EVIDENCIA"]);
                        richTextBox33.ReadOnly = false;
                        groupBox20.Enabled = true;
                        switch (calilific_preliminar3)
                        {
                            case "CA":
                                radioButton68.Checked = true;
                                break;
                            case "C":
                                radioButton67.Checked = true;
                                break;
                            case "ND":
                                radioButton66.Checked = true;
                                break;
                            case "BD":
                                radioButton65.Checked = true;
                                break;
                        }
                        break;

                    case 4:
                        richTextBox32.Text = Convert.ToString(row["EVIDENCIA"]);
                        richTextBox32.ReadOnly = false;
                        groupBox19.Enabled = true;
                        switch (calilific_preliminar3)
                        {
                            case "CA":
                                radioButton64.Checked = true;
                                break;
                            case "C":
                                radioButton63.Checked = true;
                                break;
                            case "ND":
                                radioButton62.Checked = true;
                                break;
                            case "BD":
                                radioButton61.Checked = true;
                                break;
                        }
                        break;

                    case 5:
                        richTextBox31.Text = Convert.ToString(row["EVIDENCIA"]);
                        richTextBox31.ReadOnly = false;
                        groupBox18.Enabled = true;
                        switch (calilific_preliminar3)
                        {
                            case "CA":
                                radioButton60.Checked = true;
                                break;
                            case "C":
                                radioButton59.Checked = true;
                                break;
                            case "ND":
                                radioButton58.Checked = true;
                                break;
                            case "BD":
                                radioButton57.Checked = true;
                                break;
                        }
                        break;
                    case 6:
                        richTextBox30.Text = Convert.ToString(row["EVIDENCIA"]);
                        richTextBox30.ReadOnly = false;
                        groupBox17.Enabled = true;
                        switch (calilific_preliminar3)
                        {
                            case "CA":
                                radioButton56.Checked = true;
                                break;
                            case "C":
                                radioButton55.Checked = true;
                                break;
                            case "ND":
                                radioButton54.Checked = true;
                                break;
                            case "BD":
                                radioButton53.Checked = true;
                                break;
                        }
                        break;

                    case 7:
                        richTextBox29.Text = Convert.ToString(row["EVIDENCIA"]);
                        richTextBox29.ReadOnly = false;
                        groupBox16.Enabled = true;
                        switch (calilific_preliminar3)
                        {
                            case "CA":
                                radioButton52.Checked = true;
                                break;
                            case "C":
                                radioButton51.Checked = true;
                                break;
                            case "ND":
                                radioButton50.Checked = true;
                                break;
                            case "BD":
                                radioButton49.Checked = true;
                                break;
                        }
                        break;

                    case 8:
                        richTextBox28.Text = Convert.ToString(row["EVIDENCIA"]);
                        richTextBox28.ReadOnly = false;
                        groupBox15.Enabled = true;
                        switch (calilific_preliminar3)
                        {
                            case "CA":
                                radioButton48.Checked = true;
                                break;
                            case "C":
                                radioButton47.Checked = true;
                                break;
                            case "ND":
                                radioButton46.Checked = true;
                                break;
                            case "BD":
                                radioButton45.Checked = true;
                                break;
                        }
                        break;

                    case 9:
                        richTextBox27.Text = Convert.ToString(row["EVIDENCIA"]);
                        richTextBox27.ReadOnly = false;
                        groupBox14.Enabled = true;
                        switch (calilific_preliminar3)
                        {
                            case "CA":
                                radioButton44.Checked = true;
                                break;
                            case "C":
                                radioButton43.Checked = true;
                                break;
                            case "ND":
                                radioButton42.Checked = true;
                                break;
                            case "BD":
                                radioButton41.Checked = true;
                                break;
                        }
                        break;

                }


                }

            // PASO 5 ----------------------
            richTextBox40.ReadOnly = false;
            richTextBox41.ReadOnly = false;
            con.conectar("DM");
            SqlCommand cmd4 = new SqlCommand("SELECT  [ID_EVALUACION],[COMENTARIO_JEFE],[COMENTARIO_COLAB],[FECHA_INGRESO],[USUARIO_INGRESO] FROM [DM].[CORRECT].[EVALUACION_RETROALIMENTACION_PRELIMINAR] where [ID_EVALUACION] = '" + ID_EVALUACION + "'", con.condm);
            SqlDataReader dr1 = cmd4.ExecuteReader();

            while (dr1.Read())
            {
                richTextBox40.Text = Convert.ToString(dr1["COMENTARIO_JEFE"]);
                richTextBox41.Text = Convert.ToString(dr1["COMENTARIO_COLAB"]);

            }
            dr1.Close();
            con.Desconectar("DM");

            // NOTA ----

            if(Tipo_Conulsta !=1 || Tipo_Conulsta != 2)
            {
                con.conectar("DM");
                SqlCommand cmd5 = new SqlCommand("SELECT  [ID_EVALUACION],[PROMEDIO],[PASO1],[PASO2],[NOTA_FINAL],[FECHA_INGRESO],[USUARIO_INGRESO],[PROMEDIO_PASO1] ,[PROMEDIO_PASO2]  FROM [DM].[CORRECT].[EVALUACION_CALIFICACION_PRELIMINAR] where [ID_EVALUACION] = '" + ID_EVALUACION + "'", con.condm);
                SqlDataReader dr2 = cmd5.ExecuteReader();

                while (dr2.Read())
                {

                    
                    label33.Text = Convert.ToString(dr2["PASO1"]);
                    label34.Text = Convert.ToString(dr2["PASO2"]);
                    label35.Text = Convert.ToString(dr2["NOTA_FINAL"]);
                    label41.Text = Convert.ToString(Math.Round(Convert.ToDouble(dr2["PROMEDIO_PASO1"]), 1));
                    label42.Text = Convert.ToString(Math.Round(Convert.ToDouble(dr2["PROMEDIO_PASO2"]), 1));
                    label32.Text = Convert.ToString(Math.Round(Convert.ToDouble(dr2["PROMEDIO"]), 1));


                }
                dr2.Close();
                con.Desconectar("DM");

            
            
            }
    

        }
        private void carga_final()
        {

            //PASO 1 -------


            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("SELECT [ID_EVALUACION],[ID_OBJETIVO],[DESCRIPCION],[CALIFICACION],[USUARIO_CREA],[FECHA_CREA] FROM [DM].[CORRECT].[RESULTADOS_EVALUACION_FINAL] where [ID_EVALUACION] = '" + ID_EVALUACION + "'", con.condm);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(PreliminarPaso1);

            con.Desconectar("DM");

            for (int i = 0; i < PreliminarPaso1.Rows.Count; i++)
            {
                int id_Preliminar;
                string calilific_preliminar;
                DataRow row = PreliminarPaso1.Rows[i];

                id_Preliminar = Convert.ToInt32(row["ID_OBJETIVO"]);
                calilific_preliminar = Convert.ToString(row["CALIFICACION"]);
                switch (id_Preliminar)
                {

                    case 1:
                        richTextBox21.Text = Convert.ToString(row["DESCRIPCION"]);
                        richTextBox21.Enabled = true;
                        groupBox3.Enabled = true;
                        switch (calilific_preliminar)
                        {
                            case "CA":
                                radioButton1.Checked = true;
                                break;
                            case "C":
                                radioButton2.Checked = true;
                                break;
                            case "ND":
                                radioButton3.Checked = true;
                                break;
                            case "BD":
                                radioButton4.Checked = true;
                                break;
                        }
                        break;

                    case 2:
                        richTextBox20.Text = Convert.ToString(row["DESCRIPCION"]);

                        groupBox5.Enabled = true;
                        richTextBox20.Enabled = true;


                        switch (calilific_preliminar)
                        {
                            case "CA":
                                radioButton8.Checked = true;
                                break;
                            case "C":
                                radioButton7.Checked = true;
                                break;
                            case "ND":
                                radioButton6.Checked = true;
                                break;
                            case "BD":
                                radioButton5.Checked = true;
                                break;
                        }

                        break;
                    case 3:
                        richTextBox19.Text = Convert.ToString(row["DESCRIPCION"]);
                        groupBox6.Enabled = true;
                        richTextBox19.Enabled = true;
                        switch (calilific_preliminar)
                        {
                            case "CA":
                                radioButton12.Checked = true;
                                break;
                            case "C":
                                radioButton11.Checked = true;
                                break;
                            case "ND":
                                radioButton10.Checked = true;
                                break;
                            case "BD":
                                radioButton9.Checked = true;
                                break;
                        }

                        break;
                    case 4:
                        richTextBox18.Text = Convert.ToString(row["DESCRIPCION"]);
                        groupBox7.Enabled = true;
                        richTextBox18.Enabled = true;
                        switch (calilific_preliminar)
                        {
                            case "CA":
                                radioButton16.Checked = true;
                                break;
                            case "C":
                                radioButton15.Checked = true;
                                break;
                            case "ND":
                                radioButton14.Checked = true;
                                break;
                            case "BD":
                                radioButton13.Checked = true;
                                break;
                        }

                        break;
                    case 5:
                        richTextBox17.Text = Convert.ToString(row["DESCRIPCION"]);
                        groupBox8.Enabled = true;
                        richTextBox18.Enabled = true;
                        switch (calilific_preliminar)
                        {
                            case "CA":
                                radioButton20.Checked = true;
                                break;
                            case "C":
                                radioButton19.Checked = true;
                                break;
                            case "ND":
                                radioButton18.Checked = true;
                                break;
                            case "BD":
                                radioButton17.Checked = true;
                                break;
                        }

                        break;
                    case 6:
                        richTextBox16.Text = Convert.ToString(row["DESCRIPCION"]);
                        groupBox9.Enabled = true;
                        richTextBox16.Enabled = true;
                        switch (calilific_preliminar)
                        {
                            case "CA":
                                radioButton24.Checked = true;
                                break;
                            case "C":
                                radioButton23.Checked = true;
                                break;
                            case "ND":
                                radioButton22.Checked = true;
                                break;
                            case "BD":
                                radioButton21.Checked = true;
                                break;
                        }

                        break;
                    case 7:
                        richTextBox15.Text = Convert.ToString(row["DESCRIPCION"]);
                        groupBox10.Enabled = true;
                        richTextBox15.Enabled = true;
                        switch (calilific_preliminar)
                        {
                            case "CA":
                                radioButton28.Checked = true;
                                break;
                            case "C":
                                radioButton27.Checked = true;
                                break;
                            case "ND":
                                radioButton26.Checked = true;
                                break;
                            case "BD":
                                radioButton25.Checked = true;
                                break;
                        }
                        break;
                    case 8:
                        richTextBox14.Text = Convert.ToString(row["DESCRIPCION"]);
                        groupBox11.Enabled = true;
                        richTextBox14.Enabled = true;
                        switch (calilific_preliminar)
                        {
                            case "CA":
                                radioButton32.Checked = true;
                                break;
                            case "C":
                                radioButton31.Checked = true;
                                break;
                            case "ND":
                                radioButton30.Checked = true;
                                break;
                            case "BD":
                                radioButton29.Checked = true;
                                break;
                        }
                        break;
                    case 9:
                        richTextBox13.Text = Convert.ToString(row["DESCRIPCION"]);
                        groupBox12.Enabled = true;
                        richTextBox13.Enabled = true;
                        switch (calilific_preliminar)
                        {
                            case "CA":
                                radioButton36.Checked = true;
                                break;
                            case "C":
                                radioButton35.Checked = true;
                                break;
                            case "ND":
                                radioButton34.Checked = true;
                                break;
                            case "BD":
                                radioButton33.Checked = true;
                                break;
                        }
                        break;
                    case 10:
                        richTextBox12.Text = Convert.ToString(row["DESCRIPCION"]);
                        groupBox13.Enabled = true;
                        richTextBox12.Enabled = true;
                        switch (calilific_preliminar)
                        {
                            case "CA":
                                radioButton40.Checked = true;
                                break;
                            case "C":
                                radioButton39.Checked = true;
                                break;
                            case "ND":
                                radioButton38.Checked = true;
                                break;
                            case "BD":
                                radioButton37.Checked = true;
                                break;
                        }
                        break;

                }


            }






            //PASO2--------------------------

            con.conectar("DM");
            SqlCommand cmd1 = new SqlCommand("SELECT  [ID_EVALUACION],[CRECIMIENTO],[ORGANIZCION],[RESPONSABILIDAD],[PRODUCTIVIDAD],[FECHA_CREA],[USUARIO_CREA]  FROM [DM].[CORRECT].[RESULTADO_ESTRATEGIA_FINAL] where [ID_EVALUACION] = '" + ID_EVALUACION + "'", con.condm);
            SqlDataReader dr = cmd1.ExecuteReader();

            while (dr.Read())
            {
                richTextBox26.Text = Convert.ToString(dr["CRECIMIENTO"]);
                richTextBox25.Text = Convert.ToString(dr["ORGANIZCION"]);
                richTextBox24.Text = Convert.ToString(dr["RESPONSABILIDAD"]);
                richTextBox23.Text = Convert.ToString(dr["PRODUCTIVIDAD"]);


            }
            dr.Close();
            con.Desconectar("DM");
            richTextBox26.ReadOnly = false;
            richTextBox25.ReadOnly = false;
            richTextBox24.ReadOnly = false;
            richTextBox23.ReadOnly = false;

            // PASO 3----------------------------

            con.conectar("DM");
            SqlCommand cmd2 = new SqlCommand("SELECT [ID_EVALUACION],[COD_TRABAJO],[EVIDENCIA],[CALIFICACION],[FECHA_INGRESO],[USUARIO_INGRESO] FROM [DM].[CORRECT].[EVALUACION_COD_TRABAJO_FINAL] where [ID_EVALUACION] = '" + ID_EVALUACION + "'", con.condm);
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            da2.Fill(PreliminarPaso3);

            con.Desconectar("DM");

            for (int i = 0; i < PreliminarPaso3.Rows.Count; i++)
            {
                int id_Preliminar3;
                string calilific_preliminar3;
                DataRow row = PreliminarPaso3.Rows[i];

                id_Preliminar3 = Convert.ToInt32(row["COD_TRABAJO"]);
                calilific_preliminar3 = Convert.ToString(row["CALIFICACION"]);
                switch (id_Preliminar3)
                {

                    case 1:
                        richTextBox35.Text = Convert.ToString(row["EVIDENCIA"]);
                        richTextBox35.ReadOnly = false;
                        groupBox22.Enabled = true;
                        switch (calilific_preliminar3)
                        {
                            case "CA":
                                radioButton76.Checked = true;
                                break;
                            case "C":
                                radioButton75.Checked = true;
                                break;
                            case "ND":
                                radioButton74.Checked = true;
                                break;
                            case "BD":
                                radioButton73.Checked = true;
                                break;
                        }
                        break;
                    case 2:
                        richTextBox34.Text = Convert.ToString(row["EVIDENCIA"]);
                        richTextBox34.ReadOnly = false;
                        groupBox21.Enabled = true;
                        switch (calilific_preliminar3)
                        {
                            case "CA":
                                radioButton72.Checked = true;
                                break;
                            case "C":
                                radioButton71.Checked = true;
                                break;
                            case "ND":
                                radioButton70.Checked = true;
                                break;
                            case "BD":
                                radioButton69.Checked = true;
                                break;
                        }
                        break;

                    case 3:
                        richTextBox33.Text = Convert.ToString(row["EVIDENCIA"]);
                        richTextBox33.ReadOnly = false;
                        groupBox20.Enabled = true;
                        switch (calilific_preliminar3)
                        {
                            case "CA":
                                radioButton68.Checked = true;
                                break;
                            case "C":
                                radioButton67.Checked = true;
                                break;
                            case "ND":
                                radioButton66.Checked = true;
                                break;
                            case "BD":
                                radioButton65.Checked = true;
                                break;
                        }
                        break;

                    case 4:
                        richTextBox32.Text = Convert.ToString(row["EVIDENCIA"]);
                        richTextBox32.ReadOnly = false;
                        groupBox19.Enabled = true;
                        switch (calilific_preliminar3)
                        {
                            case "CA":
                                radioButton64.Checked = true;
                                break;
                            case "C":
                                radioButton63.Checked = true;
                                break;
                            case "ND":
                                radioButton62.Checked = true;
                                break;
                            case "BD":
                                radioButton61.Checked = true;
                                break;
                        }
                        break;

                    case 5:
                        richTextBox31.Text = Convert.ToString(row["EVIDENCIA"]);
                        richTextBox31.ReadOnly = false;
                        groupBox18.Enabled = true;
                        switch (calilific_preliminar3)
                        {
                            case "CA":
                                radioButton60.Checked = true;
                                break;
                            case "C":
                                radioButton59.Checked = true;
                                break;
                            case "ND":
                                radioButton58.Checked = true;
                                break;
                            case "BD":
                                radioButton57.Checked = true;
                                break;
                        }
                        break;
                    case 6:
                        richTextBox30.Text = Convert.ToString(row["EVIDENCIA"]);
                        richTextBox30.ReadOnly = false;
                        groupBox17.Enabled = true;
                        switch (calilific_preliminar3)
                        {
                            case "CA":
                                radioButton56.Checked = true;
                                break;
                            case "C":
                                radioButton55.Checked = true;
                                break;
                            case "ND":
                                radioButton54.Checked = true;
                                break;
                            case "BD":
                                radioButton53.Checked = true;
                                break;
                        }
                        break;

                    case 7:
                        richTextBox29.Text = Convert.ToString(row["EVIDENCIA"]);
                        richTextBox29.ReadOnly = false;
                        groupBox16.Enabled = true;
                        switch (calilific_preliminar3)
                        {
                            case "CA":
                                radioButton52.Checked = true;
                                break;
                            case "C":
                                radioButton51.Checked = true;
                                break;
                            case "ND":
                                radioButton50.Checked = true;
                                break;
                            case "BD":
                                radioButton49.Checked = true;
                                break;
                        }
                        break;

                    case 8:
                        richTextBox28.Text = Convert.ToString(row["EVIDENCIA"]);
                        richTextBox28.ReadOnly = false;
                        groupBox15.Enabled = true;
                        switch (calilific_preliminar3)
                        {
                            case "CA":
                                radioButton48.Checked = true;
                                break;
                            case "C":
                                radioButton47.Checked = true;
                                break;
                            case "ND":
                                radioButton46.Checked = true;
                                break;
                            case "BD":
                                radioButton45.Checked = true;
                                break;
                        }
                        break;

                    case 9:
                        richTextBox27.Text = Convert.ToString(row["EVIDENCIA"]);
                        richTextBox27.ReadOnly = false;
                        groupBox14.Enabled = true;
                        switch (calilific_preliminar3)
                        {
                            case "CA":
                                radioButton44.Checked = true;
                                break;
                            case "C":
                                radioButton43.Checked = true;
                                break;
                            case "ND":
                                radioButton42.Checked = true;
                                break;
                            case "BD":
                                radioButton41.Checked = true;
                                break;
                        }
                        break;

                }


            }

            // PASO 5 ----------------------
            richTextBox40.ReadOnly = false;
            richTextBox41.ReadOnly = false;
            con.conectar("DM");
            SqlCommand cmd4 = new SqlCommand("SELECT  [ID_EVALUACION],[COMENTARIO_JEFE],[COMENTARIO_COLAB],[FECHA_INGRESO],[USUARIO_INGRESO] FROM [DM].[CORRECT].[EVALUACION_RETROALIMENTACION_FINAL] where [ID_EVALUACION] = '" + ID_EVALUACION + "'", con.condm);
            SqlDataReader dr1 = cmd4.ExecuteReader();

            while (dr1.Read())
            {
                richTextBox40.Text = Convert.ToString(dr1["COMENTARIO_JEFE"]);
                richTextBox41.Text = Convert.ToString(dr1["COMENTARIO_COLAB"]);

            }
            dr1.Close();
            con.Desconectar("DM");

            // NOTA ----

            if (Tipo_Conulsta != 1 || Tipo_Conulsta != 2)
            {
                con.conectar("DM");
                SqlCommand cmd5 = new SqlCommand("SELECT  [ID_EVALUACION],[PROMEDIO],[PASO1],[PASO2],[NOTA_FINAL],[FECHA_INGRESO],[USUARIO_INGRESO],[PROMEDIO_PASO1] ,[PROMEDIO_PASO2]  FROM [DM].[CORRECT].[EVALUACION_CALIFICACION_FINAL] where [ID_EVALUACION] = '" + ID_EVALUACION + "'", con.condm);
                SqlDataReader dr2 = cmd5.ExecuteReader();

                while (dr2.Read())
                {

                    label33.Text = Convert.ToString(dr2["PASO1"]);
                    label34.Text = Convert.ToString(dr2["PASO2"]);
                    label35.Text = Convert.ToString(dr2["NOTA_FINAL"]); 
                    label41.Text = Convert.ToString(Math.Round(Convert.ToDouble(dr2["PROMEDIO_PASO1"]), 1));
                    label42.Text = Convert.ToString(Math.Round(Convert.ToDouble(dr2["PROMEDIO_PASO2"]), 1));
                    label32.Text = Convert.ToString(Math.Round(Convert.ToDouble(dr2["PROMEDIO"]), 1));


                 
                    



                }
                dr2.Close();
                con.Desconectar("DM");



            }


        }

        private void richTextBox21_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox21.Text))
            {
                if (Tipo_Conulsta == 1 || Tipo_Conulsta == 2)
                {


                    groupBox3.Enabled = true;
                }
            }

            else
            {
                groupBox3.Enabled = false;
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton3.Checked = false;
                radioButton4.Checked = false;
            
            }
        }

        private void richTextBox20_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox20.Text))
            {
                if (Tipo_Conulsta == 1 || Tipo_Conulsta == 2)
                {
                    groupBox5.Enabled = true;
                }
            }

            else
            {
                groupBox5.Enabled = false;
                radioButton5.Checked = false;
                radioButton6.Checked = false;
                radioButton7.Checked = false;
                radioButton8.Checked = false;

            }
        }

        private void richTextBox19_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox19.Text))
            {
                if (Tipo_Conulsta == 1 || Tipo_Conulsta == 2)
                {
                    groupBox6.Enabled = true;
                }
            }

            else
            {
                groupBox6.Enabled = false;
                radioButton9.Checked = false;
                radioButton10.Checked = false;
                radioButton11.Checked = false;
                radioButton12.Checked = false;

            }
        }

        private void richTextBox18_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox18.Text))
            {
                if (Tipo_Conulsta == 1 || Tipo_Conulsta == 2)
                {
                    groupBox7.Enabled = true;
                }
            }

            else
            {
                groupBox7.Enabled = false;
                radioButton13.Checked = false;
                radioButton14.Checked = false;
                radioButton15.Checked = false;
                radioButton16.Checked = false;

            }
        }

        private void richTextBox17_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox17.Text))
            {
                if (Tipo_Conulsta == 1 || Tipo_Conulsta == 2)
                {
                    groupBox8.Enabled = true;
                }
            }

            else
            {
                groupBox8.Enabled = false;
                radioButton17.Checked = false;
                radioButton18.Checked = false;
                radioButton19.Checked = false;
                radioButton20.Checked = false;

            }
        }

        private void richTextBox16_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox16.Text))
            {
                if (Tipo_Conulsta == 1 || Tipo_Conulsta == 2)
                {
                    groupBox9.Enabled = true;
                }
            }

            else
            {
                groupBox9.Enabled = false;
                radioButton21.Checked = false;
                radioButton22.Checked = false;
                radioButton23.Checked = false;
                radioButton24.Checked = false;

            }
        }

        private void richTextBox15_MouseUp(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox15.Text))
            {
                if (Tipo_Conulsta == 1 || Tipo_Conulsta == 2)
                {
                    groupBox10.Enabled = true;
                }
            }

            else
            {
                groupBox10.Enabled = false;
                radioButton25.Checked = false;
                radioButton26.Checked = false;
                radioButton27.Checked = false;
                radioButton28.Checked = false;

            }
        }

        private void richTextBox14_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox14.Text))
            {
                if (Tipo_Conulsta == 1 || Tipo_Conulsta == 2)
                {
                    groupBox11.Enabled = true;
                }
            }

            else
            {
                groupBox11.Enabled = false;
                radioButton29.Checked = false;
                radioButton30.Checked = false;
                radioButton31.Checked = false;
                radioButton32.Checked = false;

            }
        }

        private void richTextBox13_MouseUp(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox13.Text))
            {
                if (Tipo_Conulsta == 1 || Tipo_Conulsta == 2)
                {
                    if (Tipo_Conulsta == 1 || Tipo_Conulsta == 2)
                    {
                        groupBox12.Enabled = true;
                    }
                }
            }

            else
            {
                groupBox12.Enabled = false;
                radioButton33.Checked = false;
                radioButton34.Checked = false;
                radioButton35.Checked = false;
                radioButton36.Checked = false;

            }
        }

        private void richTextBox12_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox12.Text))
            {
                if (Tipo_Conulsta == 1 || Tipo_Conulsta == 2)
                {
                    groupBox13.Enabled = true;
                }

            }

            else
            {
                groupBox13.Enabled = false;
                radioButton37.Checked = false;
                radioButton38.Checked = false;
                radioButton39.Checked = false;
                radioButton40.Checked = false;

            }
        }


        //PRELIMINAR
        private bool Exist_resultado_obj_prelimi(int ID_EVALUACION)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("select COUNT (*) from [DM].[CORRECT].[RESULTADOS_EVALUACION_PRELIMINAR] where ID_EVALUACION = @ID_EVALUACION ", con.condm);
            cmd.Parameters.AddWithValue("ID_EVALUACION", ID_EVALUACION);

            int contar = Convert.ToInt32(cmd.ExecuteScalar());
            con.Desconectar("DM");

            if (contar == 0)
            {
                return true;

            }
            else
            {
                return false;
            }

        }

        private bool EXIST_EVALUACION_COD_TRABAJO_PRELIMINAR(int ID_EVALUACION)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("select COUNT (*) from [DM].[CORRECT].[EVALUACION_COD_TRABAJO_PRELIMINAR] where ID_EVALUACION = @ID_EVALUACION ", con.condm);
            cmd.Parameters.AddWithValue("ID_EVALUACION", ID_EVALUACION);

            int contar = Convert.ToInt32(cmd.ExecuteScalar());
            con.Desconectar("DM");

            if (contar == 0)
            {
                return true;

            }
            else
            {
                return false;
            }

        }


        private bool EXIST_RESULTADO_ESTRATEGIA_PRELIMINAR(int ID_EVALUACION)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("select COUNT (*) from [DM].[CORRECT].[RESULTADO_ESTRATEGIA_PRELIMINAR] where ID_EVALUACION = @ID_EVALUACION ", con.condm);
            cmd.Parameters.AddWithValue("ID_EVALUACION", ID_EVALUACION);

            int contar = Convert.ToInt32(cmd.ExecuteScalar());
            con.Desconectar("DM");

            if (contar == 0)
            {
                return true;

            }
            else
            {
                return false;
            }

        }

        private bool EXIST_EVALUACION_RETROALIMENTACION_PRELIMINAR(int ID_EVALUACION)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("select COUNT (*) from [DM].[CORRECT].[EVALUACION_RETROALIMENTACION_PRELIMINAR] where ID_EVALUACION = @ID_EVALUACION ", con.condm);
            cmd.Parameters.AddWithValue("ID_EVALUACION", ID_EVALUACION);

            int contar = Convert.ToInt32(cmd.ExecuteScalar());
            con.Desconectar("DM");

            if (contar == 0)
            {
                return true;

            }
            else
            {
                return false;
            }

        }

        private bool EXIST_EVALUACION_CALIFICACION_PRELIMINAR(int ID_EVALUACION)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("select COUNT (*) from [DM].[CORRECT].[EVALUACION_CALIFICACION_PRELIMINAR] where ID_EVALUACION = @ID_EVALUACION ", con.condm);
            cmd.Parameters.AddWithValue("ID_EVALUACION", ID_EVALUACION);

            int contar = Convert.ToInt32(cmd.ExecuteScalar());
            con.Desconectar("DM");

            if (contar == 0)
            {
                return true;

            }
            else
            {
                return false;
            }

        }

       // FINAL
        private bool Exist_resultado_obj_final(int ID_EVALUACION)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("select COUNT (*) from [DM].[CORRECT].[RESULTADOS_EVALUACION_FINAL] where ID_EVALUACION = @ID_EVALUACION ", con.condm);
            cmd.Parameters.AddWithValue("ID_EVALUACION", ID_EVALUACION);

            int contar = Convert.ToInt32(cmd.ExecuteScalar());
            con.Desconectar("DM");

            if (contar == 0)
            {
                return true;

            }
            else
            {
                return false;
            }

        }

        private bool EXIST_EVALUACION_COD_TRABAJO_FINAL(int ID_EVALUACION)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("select COUNT (*) from [DM].[CORRECT].[EVALUACION_COD_TRABAJO_FINAL] where ID_EVALUACION = @ID_EVALUACION ", con.condm);
            cmd.Parameters.AddWithValue("ID_EVALUACION", ID_EVALUACION);

            int contar = Convert.ToInt32(cmd.ExecuteScalar());
            con.Desconectar("DM");

            if (contar == 0)
            {
                return true;

            }
            else
            {
                return false;
            }

        }


        private bool EXIST_RESULTADO_ESTRATEGIA_FINAL(int ID_EVALUACION)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("select COUNT (*) from [DM].[CORRECT].[RESULTADO_ESTRATEGIA_FINAL] where ID_EVALUACION = @ID_EVALUACION ", con.condm);
            cmd.Parameters.AddWithValue("ID_EVALUACION", ID_EVALUACION);

            int contar = Convert.ToInt32(cmd.ExecuteScalar());
            con.Desconectar("DM");

            if (contar == 0)
            {
                return true;

            }
            else
            {
                return false;
            }

        }

        private bool EXIST_EVALUACION_RETROALIMENTACION_FINAL(int ID_EVALUACION)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("select COUNT (*) from [DM].[CORRECT].[EVALUACION_RETROALIMENTACION_FINAL] where ID_EVALUACION = @ID_EVALUACION ", con.condm);
            cmd.Parameters.AddWithValue("ID_EVALUACION", ID_EVALUACION);

            int contar = Convert.ToInt32(cmd.ExecuteScalar());
            con.Desconectar("DM");

            if (contar == 0)
            {
                return true;

            }
            else
            {
                return false;
            }

        }

        private bool EXIST_EVALUACION_CALIFICACION_FINAL(int ID_EVALUACION)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("select COUNT (*) from [DM].[CORRECT].[EVALUACION_CALIFICACION_FINAL] where ID_EVALUACION = @ID_EVALUACION ", con.condm);
            cmd.Parameters.AddWithValue("ID_EVALUACION", ID_EVALUACION);

            int contar = Convert.ToInt32(cmd.ExecuteScalar());
            con.Desconectar("DM");

            if (contar == 0)
            {
                return true;

            }
            else
            {
                return false;
            }

        }

        private bool EXISTE_CORP(string JEFE,string Año )
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("SELECT COUNT([ID_EVALUACION]) FROM [DM].[CORRECT].[EVALUACION_DESEMPEÑO] as EV LEFT JOIN [EXACTUS].[dismo].[EMPLEADO] as  EM  on EV.COD_EMPLEADO = EM.EMPLEADO INNER JOIN [EXACTUS].[dismo].[EMPLEADO_JERARQUIA]  as EJ on EJ.SUBORDINADO = EM.EMPLEADO  INNER JOIN [DM].[CORRECT].[USUARIOS] as DMUSER  ON EJ.SUPERIOR = DMUSER.COD_EMPLEADO where DMUSER.COD_EMPLEADO = '"+JEFE+"' and DATEPART(YEAR,EV.FECHA_INGRESO) = '"+Año+"'and EV.CORP = 'S'", con.condm);
            //cmd.Parameters.AddWithValue("ID_EVALUACION", ID_EVALUACION);

            int contar = Convert.ToInt32(cmd.ExecuteScalar());
            con.Desconectar("DM");

            if (contar == 0)
            {
                return true;

            }
            else
            {
                return false;
            }

        }


        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        public static void EnableTab(TabPage page, bool enable)
        {
            foreach (Control ctl in page.Controls) ctl.Enabled = enable;
        }

        private void radioButton77_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton77.Checked)
            {
                CRP = 1;
                groupBox24.Show();

            }
            else if (radioButton78.Checked)
            {
                CRP = 2;
                groupBox24.Hide();
            }
        }

        private void radioButton78_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton77.Checked)
            {
                CRP = 1; 
                groupBox24.Show();
            }
            else if (radioButton78.Checked)
            {
                CRP = 2;
                groupBox24.Hide();
            }
        }

        private void insercion()
        {
            DateTime Fecha_crea;
            Fecha_crea = DateTime.Now;

            if (Tipo_Conulsta == 1)

            {
                if (Exist_resultado_obj_prelimi(ID_EVALUACION))
                {


                    for (int i = 1; i <= ObjetivosDT.Rows.Count; i++)
                    {
                        con.conectar("DM");
                        int Numero_Obj_insert = i;

                        switch (i)
                        {
                            case 1:
                                Resultado_insert = Resultado1;
                                Calificacion_Insert = Calificacion1;
                                break;
                            case 2:
                                if (!string.IsNullOrWhiteSpace(Resultado2))
                                {
                                    Resultado_insert = Resultado2;
                                    Calificacion_Insert = Calificacion2;

                                }
                                else
                                {
                                    goto case 3;
                                }
                                break;
                            case 3:
                                if (!string.IsNullOrWhiteSpace(Resultado3))
                                {
                                    Resultado_insert = Resultado3;
                                    Calificacion_Insert = Calificacion3;

                                }
                                else
                                {
                                    goto case 4;
                                }
                                break;

                            case 4:
                                if (!string.IsNullOrWhiteSpace(Resultado4))
                                {
                                    Resultado_insert = Resultado4;
                                    Calificacion_Insert = Calificacion4;

                                }
                                else
                                {
                                    goto case 5;
                                }
                                break;
                            case 5:
                                if (!string.IsNullOrWhiteSpace(Resultado5))
                                {
                                    Resultado_insert = Resultado5;
                                    Calificacion_Insert = Calificacion5;

                                }
                                else
                                {
                                    goto case 6;
                                }
                                break;
                            case 6:
                                if (!string.IsNullOrWhiteSpace(Resultado6))
                                {
                                    Resultado_insert = Resultado6;
                                    Calificacion_Insert = Calificacion6;

                                }
                                else
                                {
                                    goto case 7;
                                }
                                break;
                            case 7:
                                if (!string.IsNullOrWhiteSpace(Resultado7))
                                {
                                    Resultado_insert = Resultado7;
                                    Calificacion_Insert = Calificacion7;

                                }
                                else
                                {
                                    goto case 8;
                                }
                                break;
                            case 8:
                                if (!string.IsNullOrWhiteSpace(Resultado8))
                                {
                                    Resultado_insert = Resultado8;
                                    Calificacion_Insert = Calificacion8;

                                }
                                else
                                {
                                    goto case 9;
                                }
                                break;
                            case 9:
                                if (!string.IsNullOrWhiteSpace(Resultado9))
                                {
                                    Resultado_insert = Resultado9;
                                    Calificacion_Insert = Calificacion9;

                                }
                                else
                                {
                                    goto case 10;
                                }
                                break;
                            case 10:
                                if (!string.IsNullOrWhiteSpace(Resultado10))
                                {
                                    Resultado_insert = Resultado10;
                                    Calificacion_Insert = Calificacion10;
                                }
                                else
                                {

                                }
                                break;


                        }


                        SqlCommand cmd3 = new SqlCommand();
                        cmd3.Connection = con.condm;
                        cmd3.CommandText = "INSERT INTO [DM].[CORRECT].[RESULTADOS_EVALUACION_PRELIMINAR]([ID_EVALUACION],[ID_OBJETIVO],[DESCRIPCION],[CALIFICACION],[USUARIO_CREA],[FECHA_CREA])  VALUES( @ID_EVALUACION,@ID_OBJETIVO,@DESCRIPCION,@CALIFICACION,@USUARIO_CREA,@FECHA_CREA)";
                        cmd3.Parameters.Add("@ID_EVALUACION", SqlDbType.NVarChar).Value = ID_EVALUACION;
                        cmd3.Parameters.Add("@ID_OBJETIVO", SqlDbType.NVarChar).Value = Numero_Obj_insert;
                        cmd3.Parameters.Add("@DESCRIPCION", SqlDbType.NVarChar).Value = Resultado_insert;
                        cmd3.Parameters.Add("@CALIFICACION", SqlDbType.NVarChar).Value = Calificacion_Insert;
                        cmd3.Parameters.Add("@USUARIO_CREA", SqlDbType.NVarChar).Value = Usuario;
                        cmd3.Parameters.Add("@FECHA_CREA", SqlDbType.DateTime).Value = Fecha_crea;



                        cmd3.ExecuteNonQuery();

                        con.Desconectar("DM");

                    }
                }


                if (EXIST_RESULTADO_ESTRATEGIA_PRELIMINAR(ID_EVALUACION))
                {
                    con.conectar("DM");
                    SqlCommand cmd4 = new SqlCommand();
                    cmd4.Connection = con.condm;
                    cmd4.CommandText = "INSERT INTO [DM].[CORRECT].[RESULTADO_ESTRATEGIA_PRELIMINAR]([ID_EVALUACION],[CRECIMIENTO],[ORGANIZCION],[RESPONSABILIDAD],[PRODUCTIVIDAD],[FECHA_CREA],[USUARIO_CREA])  VALUES(@ID_EVALUACION,@CRECIMIENTO,@ORGANIZCION,@RESPONSABILIDAD,@PRODUCTIVIDAD,@FECHA_CREA,@USUARIO_CREA)";
                    cmd4.Parameters.Add("@ID_EVALUACION", SqlDbType.NVarChar).Value = ID_EVALUACION;


                    if (!string.IsNullOrEmpty(Crecimiento))
                    {
                        cmd4.Parameters.Add("@CRECIMIENTO", SqlDbType.NVarChar).Value = Crecimiento;
                    }
                    else
                    {
                        cmd4.Parameters.Add("@CRECIMIENTO", SqlDbType.NVarChar).Value = "No aplica";
                    }

                    if (!string.IsNullOrEmpty(Organizacion))
                    {
                        cmd4.Parameters.Add("@ORGANIZCION", SqlDbType.NVarChar).Value = Organizacion;
                    }
                    else
                    {
                        cmd4.Parameters.Add("@ORGANIZCION", SqlDbType.NVarChar).Value = "No aplica";
                    }

                    if (!string.IsNullOrEmpty(Responsabilidad))
                    {
                        cmd4.Parameters.Add("@RESPONSABILIDAD", SqlDbType.NVarChar).Value = Responsabilidad;
                    }
                    else
                    {
                        cmd4.Parameters.Add("@RESPONSABILIDAD", SqlDbType.NVarChar).Value = "No aplica";
                    }

                    if (!string.IsNullOrEmpty(Productividad))
                    {
                        cmd4.Parameters.Add("@PRODUCTIVIDAD", SqlDbType.NVarChar).Value = Productividad;
                    }
                    else
                    {
                        cmd4.Parameters.Add("@PRODUCTIVIDAD", SqlDbType.NVarChar).Value = "No aplica";
                    }
                    cmd4.Parameters.Add("@USUARIO_CREA", SqlDbType.NVarChar).Value = Usuario;
                    cmd4.Parameters.Add("@FECHA_CREA", SqlDbType.DateTime).Value = Fecha_crea;


                    cmd4.ExecuteNonQuery();

                    con.Desconectar("DM");
                }

                if (EXIST_EVALUACION_COD_TRABAJO_PRELIMINAR(ID_EVALUACION))
                {
                    for (int j = 1; j <= 9; j++)
                    {
                        con.conectar("DM");
                        int Numero_Insert = j;

                        switch (j)
                        {
                            case 1:
                                PASO3Evidencia_insert = PASO3Evidencia1;
                                PASO3calificacion_insert = PASO3calificacion1;
                                break;
                            case 2:
                                if (!string.IsNullOrWhiteSpace(PASO3Evidencia2))
                                {
                                    PASO3Evidencia_insert = PASO3Evidencia2;
                                    PASO3calificacion_insert = PASO3calificacion2;

                                }
                                else
                                {
                                    goto case 3;
                                }
                                break;
                            case 3:
                                if (!string.IsNullOrWhiteSpace(PASO3Evidencia3))
                                {
                                    PASO3Evidencia_insert = PASO3Evidencia3;
                                    PASO3calificacion_insert = PASO3calificacion3;

                                }
                                else
                                {
                                    goto case 4;
                                }
                                break;

                            case 4:
                                if (!string.IsNullOrWhiteSpace(PASO3Evidencia4))
                                {
                                    PASO3Evidencia_insert = PASO3Evidencia4;
                                    PASO3calificacion_insert = PASO3calificacion4;

                                }
                                else
                                {
                                    goto case 5;
                                }
                                break;
                            case 5:
                                if (!string.IsNullOrWhiteSpace(PASO3Evidencia5))
                                {
                                    PASO3Evidencia_insert = PASO3Evidencia5;
                                    PASO3calificacion_insert = PASO3calificacion5;

                                }
                                else
                                {
                                    goto case 6;
                                }
                                break;
                            case 6:
                                if (!string.IsNullOrWhiteSpace(PASO3Evidencia6))
                                {
                                    PASO3Evidencia_insert = PASO3Evidencia6;
                                    PASO3calificacion_insert = PASO3calificacion6;

                                }
                                else
                                {
                                    goto case 7;
                                }
                                break;
                            case 7:
                                if (!string.IsNullOrWhiteSpace(PASO3Evidencia7))
                                {
                                    PASO3Evidencia_insert = PASO3Evidencia7;
                                    PASO3calificacion_insert = PASO3calificacion7;

                                }
                                else
                                {
                                    goto case 8;
                                }
                                break;
                            case 8:
                                if (!string.IsNullOrWhiteSpace(PASO3Evidencia8))
                                {
                                    PASO3Evidencia_insert = PASO3Evidencia8;
                                    PASO3calificacion_insert = PASO3calificacion8;


                                }
                                else
                                {
                                    goto case 9;
                                }
                                break;
                            case 9:
                                if (!string.IsNullOrWhiteSpace(PASO3Evidencia9))
                                {
                                    PASO3Evidencia_insert = PASO3Evidencia9;
                                    PASO3calificacion_insert = PASO3calificacion9;

                                }
                                else
                                {

                                }
                                break;




                        }

                        SqlCommand cmd5 = new SqlCommand();
                        cmd5.Connection = con.condm;
                        cmd5.CommandText = "INSERT INTO [DM].[CORRECT].[EVALUACION_COD_TRABAJO_PRELIMINAR]([ID_EVALUACION],[COD_TRABAJO],[EVIDENCIA],[CALIFICACION],[FECHA_INGRESO],[USUARIO_INGRESO])  VALUES(@ID_EVALUACION,@COD_TRABAJO,@EVIDENCIA,@CALIFICACION,@FECHA_INGRESO,@USUARIO_INGRESO)";
                        cmd5.Parameters.Add("@ID_EVALUACION", SqlDbType.NVarChar).Value = ID_EVALUACION;
                        cmd5.Parameters.Add("@COD_TRABAJO", SqlDbType.NVarChar).Value = Numero_Insert;
                        if (!string.IsNullOrEmpty(PASO3Evidencia_insert))
                        {
                            cmd5.Parameters.Add("@EVIDENCIA", SqlDbType.NVarChar).Value = PASO3Evidencia_insert;
                        }
                        else
                        {
                            cmd5.Parameters.Add("@EVIDENCIA", SqlDbType.NVarChar).Value = "Ninguna";
                        }

                        cmd5.Parameters.Add("@CALIFICACION", SqlDbType.NVarChar).Value = PASO3calificacion_insert;
                        cmd5.Parameters.Add("@USUARIO_INGRESO", SqlDbType.NVarChar).Value = Usuario;
                        cmd5.Parameters.Add("@FECHA_INGRESO", SqlDbType.DateTime).Value = Fecha_crea;


                        cmd5.ExecuteNonQuery();

                        con.Desconectar("DM");


                    }
                }
                if (EXIST_EVALUACION_RETROALIMENTACION_PRELIMINAR(ID_EVALUACION))
                {
                    con.conectar("DM");
                    SqlCommand cmd6 = new SqlCommand();
                    cmd6.Connection = con.condm;
                    cmd6.CommandText = "INSERT INTO [DM].[CORRECT].[EVALUACION_RETROALIMENTACION_PRELIMINAR]([ID_EVALUACION],[COMENTARIO_JEFE],[COMENTARIO_COLAB],[FECHA_INGRESO],[USUARIO_INGRESO])  VALUES(@ID_EVALUACION,@COMENTARIO_JEFE,@COMENTARIO_COLAB,@FECHA_INGRESO,@USUARIO_INGRESO)";
                    cmd6.Parameters.Add("@ID_EVALUACION", SqlDbType.NVarChar).Value = ID_EVALUACION;
                    cmd6.Parameters.Add("@COMENTARIO_JEFE", SqlDbType.NVarChar).Value = Comentario_Jefe;
                    cmd6.Parameters.Add("@COMENTARIO_COLAB", SqlDbType.NVarChar).Value = Comentario_Empl;
                    cmd6.Parameters.Add("@USUARIO_INGRESO", SqlDbType.NVarChar).Value = Usuario;
                    cmd6.Parameters.Add("@FECHA_INGRESO", SqlDbType.DateTime).Value = Fecha_crea;

                    cmd6.ExecuteNonQuery();
                    con.Desconectar("DM");
                }
                if (EXIST_EVALUACION_CALIFICACION_PRELIMINAR(ID_EVALUACION))
                {
                    con.conectar("DM");
                    SqlCommand cmd7 = new SqlCommand();
                    cmd7.Connection = con.condm;
                    cmd7.CommandText = "INSERT INTO [DM].[CORRECT].[EVALUACION_CALIFICACION_PRELIMINAR]([ID_EVALUACION],[PROMEDIO],[PASO1],[PASO2],[NOTA_FINAL],[FECHA_INGRESO],[USUARIO_INGRESO],[PROMEDIO_PASO1],[PROMEDIO_PASO2])  VALUES(@ID_EVALUACION,@PROMEDIO,@PASO1,@PASO2,@NOTA_FINAL,@FECHA_INGRESO,@USUARIO_INGRESO,@PROMEDIO_PASO1,@PROMEDIO_PASO2)";
                    cmd7.Parameters.Add("@ID_EVALUACION", SqlDbType.NVarChar).Value = ID_EVALUACION;
                    cmd7.Parameters.Add("@PROMEDIO", SqlDbType.NVarChar).Value = Convert.ToString(Math.Round(PromedioFinal, 2));
                    cmd7.Parameters.Add("@PROMEDIO_PASO1", SqlDbType.NVarChar).Value = Convert.ToString(Math.Round(PromedioPaso1, 2));
                    cmd7.Parameters.Add("@PROMEDIO_PASO2", SqlDbType.NVarChar).Value = Convert.ToString(Math.Round(PromedioPASO3, 2));
                    cmd7.Parameters.Add("@PASO1", SqlDbType.NVarChar).Value = ResultadoCL1;
                    cmd7.Parameters.Add("@PASO2", SqlDbType.NVarChar).Value = ResultadoCL3;
                    cmd7.Parameters.Add("@NOTA_FINAL", SqlDbType.NVarChar).Value = Resultado_Final;
                    cmd7.Parameters.Add("@USUARIO_INGRESO", SqlDbType.NVarChar).Value = Usuario;
                    cmd7.Parameters.Add("@FECHA_INGRESO", SqlDbType.DateTime).Value = Fecha_crea;

                    cmd7.ExecuteNonQuery();

                    con.Desconectar("DM");

                }

                con.conectar("DM");

                SqlCommand cmd8 = new SqlCommand();
                cmd8.Connection = con.condm;
                cmd8.CommandText = "UPDATE [DM].[CORRECT].[EVALUACION_DESEMPEÑO] SET ESTADO_EVALUACION =@ESTADO where ID_EVALUACION = '" + ID_EVALUACION + "' ";
                cmd8.Parameters.Add("@ESTADO", SqlDbType.NVarChar).Value = "Calificada";
                cmd8.ExecuteNonQuery();

                con.Desconectar("DM");

                MessageBox.Show("Calificacion Ingresada...");
                this.Close();
            }
            if (Tipo_Conulsta == 2)
            {


                if (Exist_resultado_obj_final(ID_EVALUACION))
                {


                    for (int i = 1; i <= ObjetivosDT.Rows.Count; i++)
                    {
                        con.conectar("DM");
                        int Numero_Obj_insert = i;

                        switch (i)
                        {
                            case 1:
                                Resultado_insert = Resultado1;
                                Calificacion_Insert = Calificacion1;
                                break;
                            case 2:
                                if (!string.IsNullOrWhiteSpace(Resultado2))
                                {
                                    Resultado_insert = Resultado2;
                                    Calificacion_Insert = Calificacion2;

                                }
                                else
                                {
                                    goto case 3;
                                }
                                break;
                            case 3:
                                if (!string.IsNullOrWhiteSpace(Resultado3))
                                {
                                    Resultado_insert = Resultado3;
                                    Calificacion_Insert = Calificacion3;

                                }
                                else
                                {
                                    goto case 4;
                                }
                                break;

                            case 4:
                                if (!string.IsNullOrWhiteSpace(Resultado4))
                                {
                                    Resultado_insert = Resultado4;
                                    Calificacion_Insert = Calificacion4;

                                }
                                else
                                {
                                    goto case 5;
                                }
                                break;
                            case 5:
                                if (!string.IsNullOrWhiteSpace(Resultado5))
                                {
                                    Resultado_insert = Resultado5;
                                    Calificacion_Insert = Calificacion5;

                                }
                                else
                                {
                                    goto case 6;
                                }
                                break;
                            case 6:
                                if (!string.IsNullOrWhiteSpace(Resultado6))
                                {
                                    Resultado_insert = Resultado6;
                                    Calificacion_Insert = Calificacion6;

                                }
                                else
                                {
                                    goto case 7;
                                }
                                break;
                            case 7:
                                if (!string.IsNullOrWhiteSpace(Resultado7))
                                {
                                    Resultado_insert = Resultado7;
                                    Calificacion_Insert = Calificacion7;

                                }
                                else
                                {
                                    goto case 8;
                                }
                                break;
                            case 8:
                                if (!string.IsNullOrWhiteSpace(Resultado8))
                                {
                                    Resultado_insert = Resultado8;
                                    Calificacion_Insert = Calificacion8;

                                }
                                else
                                {
                                    goto case 9;
                                }
                                break;
                            case 9:
                                if (!string.IsNullOrWhiteSpace(Resultado9))
                                {
                                    Resultado_insert = Resultado9;
                                    Calificacion_Insert = Calificacion9;

                                }
                                else
                                {
                                    goto case 10;
                                }
                                break;
                            case 10:
                                if (!string.IsNullOrWhiteSpace(Resultado10))
                                {
                                    Resultado_insert = Resultado10;
                                    Calificacion_Insert = Calificacion10;
                                }
                                else
                                {

                                }
                                break;


                        }



                        SqlCommand cmd3 = new SqlCommand();
                        cmd3.Connection = con.condm;
                        cmd3.CommandText = "INSERT INTO [DM].[CORRECT].[RESULTADOS_EVALUACION_FINAL]([ID_EVALUACION],[ID_OBJETIVO],[DESCRIPCION],[CALIFICACION],[USUARIO_CREA],[FECHA_CREA]) VALUES (@ID_EVALUACION,@ID_OBJETIVO,@DESCRIPCION,@CALIFICACION,@USUARIO_CREA,@FECHA_CREA)";
                        // cmd3.CommandText = "INSERT INTO [DM].[CORRECT].[RESULTADOS_EVALUACION_FINAL]([ID_EVALUACION],[ID_OBJETIVO],[DESCRIPCION],[CALIFICACION],[USUARIO_CREA],[FECHA_CREA]) VALUES ('" + ID_EVALUACION + "','" + Numero_Obj_insert + "','" + Resultado_insert + "','" + Calificacion_Insert + "','" + Usuario + "','" + Fecha_crea + "')";
                        cmd3.Parameters.Add("@ID_EVALUACION", SqlDbType.NVarChar).Value = ID_EVALUACION;
                        cmd3.Parameters.Add("@ID_OBJETIVO", SqlDbType.NVarChar).Value = Numero_Obj_insert;
                        cmd3.Parameters.Add("@DESCRIPCION", SqlDbType.NVarChar).Value = Resultado_insert;
                        cmd3.Parameters.Add("@CALIFICACION", SqlDbType.NVarChar).Value = Calificacion_Insert;
                        cmd3.Parameters.Add("@USUARIO_CREA", SqlDbType.NVarChar).Value = Usuario;
                        cmd3.Parameters.Add("@FECHA_CREA", SqlDbType.DateTime).Value = Fecha_crea;



                        cmd3.ExecuteNonQuery();

                        con.Desconectar("DM");

                    }
                }


                if (EXIST_RESULTADO_ESTRATEGIA_FINAL(ID_EVALUACION))
                {
                    con.conectar("DM");
                    SqlCommand cmd4 = new SqlCommand();
                    cmd4.Connection = con.condm;
                    cmd4.CommandText = "INSERT INTO [DM].[CORRECT].[RESULTADO_ESTRATEGIA_FINAL]([ID_EVALUACION],[CRECIMIENTO],[ORGANIZCION],[RESPONSABILIDAD],[PRODUCTIVIDAD],[FECHA_CREA],[USUARIO_CREA])  VALUES(@ID_EVALUACION,@CRECIMIENTO,@ORGANIZCION,@RESPONSABILIDAD,@PRODUCTIVIDAD,@FECHA_CREA,@USUARIO_CREA)";
                    cmd4.Parameters.Add("@ID_EVALUACION", SqlDbType.NVarChar).Value = ID_EVALUACION;
                    cmd4.Parameters.Add("@CRECIMIENTO", SqlDbType.NVarChar).Value = Crecimiento;
                    cmd4.Parameters.Add("@ORGANIZCION", SqlDbType.NVarChar).Value = Organizacion;
                    cmd4.Parameters.Add("@RESPONSABILIDAD", SqlDbType.NVarChar).Value = Responsabilidad;
                    cmd4.Parameters.Add("@PRODUCTIVIDAD", SqlDbType.NVarChar).Value = Productividad;
                    cmd4.Parameters.Add("@USUARIO_CREA", SqlDbType.NVarChar).Value = Usuario;
                    cmd4.Parameters.Add("@FECHA_CREA", SqlDbType.DateTime).Value = Fecha_crea;


                    cmd4.ExecuteNonQuery();

                    con.Desconectar("DM");
                }

                if (EXIST_EVALUACION_COD_TRABAJO_FINAL(ID_EVALUACION))
                {
                    for (int j = 1; j <= 9; j++)
                    {
                        con.conectar("DM");
                        int Numero_Insert = j;

                        switch (j)
                        {
                            case 1:
                                PASO3Evidencia_insert = PASO3Evidencia1;
                                PASO3calificacion_insert = PASO3calificacion1;
                                break;
                            case 2:
                                if (!string.IsNullOrWhiteSpace(PASO3Evidencia2))
                                {
                                    PASO3Evidencia_insert = PASO3Evidencia2;
                                    PASO3calificacion_insert = PASO3calificacion2;

                                }
                                else
                                {
                                    goto case 3;
                                }
                                break;
                            case 3:
                                if (!string.IsNullOrWhiteSpace(PASO3Evidencia3))
                                {
                                    PASO3Evidencia_insert = PASO3Evidencia3;
                                    PASO3calificacion_insert = PASO3calificacion3;

                                }
                                else
                                {
                                    goto case 4;
                                }
                                break;

                            case 4:
                                if (!string.IsNullOrWhiteSpace(PASO3Evidencia4))
                                {
                                    PASO3Evidencia_insert = PASO3Evidencia4;
                                    PASO3calificacion_insert = PASO3calificacion4;

                                }
                                else
                                {
                                    goto case 5;
                                }
                                break;
                            case 5:
                                if (!string.IsNullOrWhiteSpace(PASO3Evidencia5))
                                {
                                    PASO3Evidencia_insert = PASO3Evidencia5;
                                    PASO3calificacion_insert = PASO3calificacion5;

                                }
                                else
                                {
                                    goto case 6;
                                }
                                break;
                            case 6:
                                if (!string.IsNullOrWhiteSpace(PASO3Evidencia6))
                                {
                                    PASO3Evidencia_insert = PASO3Evidencia6;
                                    PASO3calificacion_insert = PASO3calificacion6;

                                }
                                else
                                {
                                    goto case 7;
                                }
                                break;
                            case 7:
                                if (!string.IsNullOrWhiteSpace(PASO3Evidencia7))
                                {
                                    PASO3Evidencia_insert = PASO3Evidencia7;
                                    PASO3calificacion_insert = PASO3calificacion7;

                                }
                                else
                                {
                                    goto case 8;
                                }
                                break;
                            case 8:
                                if (!string.IsNullOrWhiteSpace(PASO3Evidencia8))
                                {
                                    PASO3Evidencia_insert = PASO3Evidencia8;
                                    PASO3calificacion_insert = PASO3calificacion8;


                                }
                                else
                                {
                                    goto case 9;
                                }
                                break;
                            case 9:
                                if (!string.IsNullOrWhiteSpace(PASO3Evidencia9))
                                {
                                    PASO3Evidencia_insert = PASO3Evidencia9;
                                    PASO3calificacion_insert = PASO3calificacion9;

                                }
                                else
                                {

                                }
                                break;




                        }

                        SqlCommand cmd5 = new SqlCommand();
                        cmd5.Connection = con.condm;
                        cmd5.CommandText = "INSERT INTO [DM].[CORRECT].[EVALUACION_COD_TRABAJO_FINAL]([ID_EVALUACION],[COD_TRABAJO],[EVIDENCIA],[CALIFICACION],[FECHA_INGRESO],[USUARIO_INGRESO])  VALUES(@ID_EVALUACION,@COD_TRABAJO,@EVIDENCIA,@CALIFICACION,@FECHA_INGRESO,@USUARIO_INGRESO)";
                        cmd5.Parameters.Add("@ID_EVALUACION", SqlDbType.NVarChar).Value = ID_EVALUACION;
                        cmd5.Parameters.Add("@COD_TRABAJO", SqlDbType.NVarChar).Value = Numero_Insert;
                        cmd5.Parameters.Add("@EVIDENCIA", SqlDbType.NVarChar).Value = PASO3Evidencia_insert;
                        cmd5.Parameters.Add("@CALIFICACION", SqlDbType.NVarChar).Value = PASO3calificacion_insert;
                        cmd5.Parameters.Add("@USUARIO_INGRESO", SqlDbType.NVarChar).Value = Usuario;
                        cmd5.Parameters.Add("@FECHA_INGRESO", SqlDbType.DateTime).Value = Fecha_crea;


                        cmd5.ExecuteNonQuery();

                        con.Desconectar("DM");


                    }
                }
                if (EXIST_EVALUACION_RETROALIMENTACION_FINAL(ID_EVALUACION))
                {
                    con.conectar("DM");
                    SqlCommand cmd6 = new SqlCommand();
                    cmd6.Connection = con.condm;
                    cmd6.CommandText = "INSERT INTO [DM].[CORRECT].[EVALUACION_RETROALIMENTACION_FINAL]([ID_EVALUACION],[COMENTARIO_JEFE],[COMENTARIO_COLAB],[FECHA_INGRESO],[USUARIO_INGRESO])  VALUES(@ID_EVALUACION,@COMENTARIO_JEFE,@COMENTARIO_COLAB,@FECHA_INGRESO,@USUARIO_INGRESO)";
                    cmd6.Parameters.Add("@ID_EVALUACION", SqlDbType.NVarChar).Value = ID_EVALUACION;
                    cmd6.Parameters.Add("@COMENTARIO_JEFE", SqlDbType.NVarChar).Value = Comentario_Jefe;
                    cmd6.Parameters.Add("@COMENTARIO_COLAB", SqlDbType.NVarChar).Value = Comentario_Empl;
                    cmd6.Parameters.Add("@USUARIO_INGRESO", SqlDbType.NVarChar).Value = Usuario;
                    cmd6.Parameters.Add("@FECHA_INGRESO", SqlDbType.DateTime).Value = Fecha_crea;

                    cmd6.ExecuteNonQuery();
                    con.Desconectar("DM");
                }
                if (EXIST_EVALUACION_CALIFICACION_FINAL(ID_EVALUACION))
                {
                    con.conectar("DM");
                    SqlCommand cmd7 = new SqlCommand();
                    cmd7.Connection = con.condm;
                    cmd7.CommandText = "INSERT INTO [DM].[CORRECT].[EVALUACION_CALIFICACION_FINAL]([ID_EVALUACION],[PROMEDIO],[PASO1],[PASO2],[NOTA_FINAL],[FECHA_INGRESO],[USUARIO_INGRESO],[PROMEDIO_PASO1],[PROMEDIO_PASO2])  VALUES(@ID_EVALUACION,@PROMEDIO,@PASO1,@PASO2,@NOTA_FINAL,@FECHA_INGRESO,@USUARIO_INGRESO,@PROMEDIO_PASO1,@PROMEDIO_PASO2)";
                    cmd7.Parameters.Add("@ID_EVALUACION", SqlDbType.NVarChar).Value = ID_EVALUACION;
                    cmd7.Parameters.Add("@PROMEDIO", SqlDbType.NVarChar).Value = Convert.ToString(Math.Round(PromedioFinal, 2));
                    cmd7.Parameters.Add("@PROMEDIO_PASO1", SqlDbType.NVarChar).Value = Convert.ToString(Math.Round(PromedioPaso1, 2));
                    cmd7.Parameters.Add("@PROMEDIO_PASO2", SqlDbType.NVarChar).Value = Convert.ToString(Math.Round(PromedioPASO3, 2));
                    cmd7.Parameters.Add("@PASO1", SqlDbType.NVarChar).Value = ResultadoCL1;
                    cmd7.Parameters.Add("@PASO2", SqlDbType.NVarChar).Value = ResultadoCL3;
                    cmd7.Parameters.Add("@NOTA_FINAL", SqlDbType.NVarChar).Value = Resultado_Final;
                    cmd7.Parameters.Add("@USUARIO_INGRESO", SqlDbType.NVarChar).Value = Usuario;
                    cmd7.Parameters.Add("@FECHA_INGRESO", SqlDbType.DateTime).Value = Fecha_crea;

                    cmd7.ExecuteNonQuery();

                    con.Desconectar("DM");

                }

                con.conectar("DM");

                SqlCommand cmd8 = new SqlCommand();
                cmd8.Connection = con.condm;
                cmd8.CommandText = "UPDATE [DM].[CORRECT].[EVALUACION_DESEMPEÑO] SET ESTADO_EVALUACION =@ESTADO,[CORP] = @CORP,[COMENT_CORP] = @COMENT where ID_EVALUACION = '" + ID_EVALUACION + "' ";
                cmd8.Parameters.Add("@ESTADO", SqlDbType.NVarChar).Value = "Cerrada";
                cmd8.Parameters.Add("@CORP", SqlDbType.NVarChar).Value = CORP_CLIE;
                if (COMENT_CORP_CLIE != "")
                {
                    cmd8.Parameters.Add("@COMENT", SqlDbType.NVarChar).Value = COMENT_CORP_CLIE;
                }
                else
                {
                    cmd8.Parameters.Add("@COMENT", SqlDbType.NVarChar).Value = "NULL";
                }
                cmd8.ExecuteNonQuery();

                con.Desconectar("DM");

                MessageBox.Show("Evaluacion Finalizada...");
                this.Close();






            }
        }
    }
}
