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
    public partial class Objetivos_Evaluacion : Form
    {
        public Objetivos_Evaluacion(string id_emp,int tipo_ingreso)
        {
            InitializeComponent();
            ID_EMPLEADO = id_emp;
            tipo_ingre = tipo_ingreso;
        }
        Int32 tipo_ingre;
        String ID_EMPLEADO;
        String NOMBRE ;
        String Departamento ;
        String cargo  ;
        String JefeInmediato;
        Int32 Numero_Objetivo;
        String Objetivo1;
        String Objetivo2;
        String Objetivo3;
        String Objetivo4;
        String Objetivo5;
        String Objetivo6;
        String Objetivo7;
        String Objetivo8;
        String Objetivo9;
        String Objetivo10;
        String Objetivo_insert;
        String Competencias;
        Int32 Vacias;
        String Usuario;
        String ESTADO;
        Int32 ID_EVALUACION;
        Int32 Semestre =Menu_Evaluaciones.Semestre_Actual;
        conexionXML con = new conexionXML();

        Menu_Evaluaciones ev = new Menu_Evaluaciones();


        private void Objetivos_Evaluacion_Load(object sender, EventArgs e)
        {
            Usuario = Login.usuario.ToUpper();
            //Usuario = "Pruebas";
            ESTADO = "Abierta";
            if (tipo_ingre == 1)
            {
                textBox1.Enabled =true;
                textBox2.Enabled = true;
                textBox3.Enabled = false;
                textBox5.Enabled = false;
                textBox4.Enabled = false;

                richTextBox2.Enabled = true;
                richTextBox3.Enabled = true;
                richTextBox4.Enabled = true;
                richTextBox5.Enabled = true;
                richTextBox6.Enabled = true;
                richTextBox7.Enabled = true;
                richTextBox8.Enabled = true;
                richTextBox9.Enabled = true;
                richTextBox10.Enabled = true;
               // Infomarcionload(ID_EMPLEADO);
            }
            else
            {
               
                textBox4.Enabled = false;

                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                textBox5.Enabled = false;

                richTextBox2.Enabled = false;
                richTextBox3.Enabled = false;
                richTextBox4.Enabled = false;
                richTextBox5.Enabled = false;
                richTextBox6.Enabled = false;
                richTextBox7.Enabled = false;
                richTextBox8.Enabled = false;
                richTextBox9.Enabled = false;
                richTextBox10.Enabled = false;
            }

            Infomarcionload(ID_EMPLEADO);
            textBox1.Text = ID_EMPLEADO;
            textBox2.Text = NOMBRE;
            textBox3.Text = Departamento;
            textBox4.Text = JefeInmediato;
            textBox5.Text = cargo;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            validacion();
            
            
           
        }

        private void validacion()
        {

            Numero_Objetivo = 0;
            Vacias = 0;



            if (!string.IsNullOrWhiteSpace(richTextBox1.Text))
            {
                Objetivo1 = richTextBox1.Text;
                Numero_Objetivo = Numero_Objetivo + 1;
            }
            else
            {
                Vacias = Vacias + 1;
            }
            if (!string.IsNullOrWhiteSpace(richTextBox2.Text))
            {
                Objetivo2 = richTextBox2.Text;
                Numero_Objetivo = Numero_Objetivo + 1;
            }
            else
            {
                Vacias = Vacias + 1;
            }
            if (!string.IsNullOrWhiteSpace(richTextBox3.Text))
            {
                Objetivo3 = richTextBox3.Text;
                Numero_Objetivo = Numero_Objetivo + 1;
            }
            else
            {
                Vacias = Vacias + 1;
            }

            if (!string.IsNullOrWhiteSpace(richTextBox4.Text))
            {
                Objetivo4 = richTextBox4.Text;
                Numero_Objetivo = Numero_Objetivo + 1;
            }
            else
            {
                Vacias = Vacias + 1;
            }
            if (!string.IsNullOrWhiteSpace(richTextBox5.Text))
            {
                Objetivo5 = richTextBox5.Text;
                Numero_Objetivo = Numero_Objetivo + 1;
            }
            else
            {
                Vacias = Vacias + 1;
            }
            if (!string.IsNullOrWhiteSpace(richTextBox6.Text))
            {
                Objetivo6 = richTextBox6.Text;
                Numero_Objetivo = Numero_Objetivo + 1;
            }
            else
            {
                Vacias = Vacias + 1;
            }
            if (!string.IsNullOrWhiteSpace(richTextBox7.Text))
            {
                Objetivo7 = richTextBox7.Text;
                Numero_Objetivo = Numero_Objetivo + 1;
            }
            else
            {
                Vacias = Vacias + 1;
            }
            if (!string.IsNullOrWhiteSpace(richTextBox8.Text))
            {
                Objetivo8 = richTextBox8.Text;
                Numero_Objetivo = Numero_Objetivo + 1;
            }
            else
            {
                Vacias = Vacias + 1;
            }
            if (!string.IsNullOrWhiteSpace(richTextBox9.Text))
            {
                Objetivo9 = richTextBox9.Text;
                Numero_Objetivo = Numero_Objetivo + 1;
            }
            else
            {
                Vacias = Vacias + 1;
            }

            if (!string.IsNullOrWhiteSpace(richTextBox10.Text))
            {
                Objetivo10 = richTextBox10.Text;
                Numero_Objetivo = Numero_Objetivo + 1;
            }
            else
            {
                Vacias = Vacias + 1;
            }



            if (string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Ingrese Nombre del jefe inmediato", "EVALUACION", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
                textBox4.Focus();
            }
            else
                JefeInmediato = textBox4.Text;
            if (Vacias >= 10)
            {
                MessageBox.Show("Ingrese Objetivos", "EVALUACION", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
                {
                    tabControl1.SelectedIndex = 0;
                    richTextBox1.Focus();


                }

            }
            else
                if (Numero_Objetivo == 0)
                {
                    MessageBox.Show("Ingrese al menos un Objetivo", "EVALUACION", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
                }
                else

                if (!string.IsNullOrWhiteSpace(richTextBox11.Text))
                {
                    Competencias = richTextBox11.Text;

                    ingreso_datos();
                }
                else
                {
                    MessageBox.Show("Ingrese Competencias y áreas de desarrollo deseado", "EVALUACION", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
                    {

                        tabControl1.SelectedIndex = 1;
                        richTextBox11.Focus();

                    }
                }

            
        
        }

        private void ingreso_datos()
        {
            DateTime Fecha_crea;
            Fecha_crea = DateTime.Now;

            try
            {

                con.conectar("DM");
                SqlCommand cm2 = new SqlCommand("SELECT IDENT_CURRENT ('[DM].[CORRECT].[EVALUACION_DESEMPEÑO]') as ID_EVALUACION", con.condm);
                SqlDataReader dr2 = cm2.ExecuteReader();
                while (dr2.Read())
                {
                    ID_EVALUACION = Convert.ToInt32(dr2["ID_EVALUACION"]);
                }
                dr2.Close();

                SqlCommand cmd1 = new SqlCommand();
                cmd1.Connection = con.condm;
                cmd1.CommandText = "INSERT INTO [DM].[CORRECT].[EVALUACION_DESEMPEÑO]([COD_EMPLEADO],[NOMBRE],[DEPARTAMENTO],[CARGO],[JEFE_INMEDIATO],[ESTADO_EVALUACION],[SEMESTRE_INGRESO],[COMPETENCIAS],[FECHA_INGRESO],[USUARIO_INGRESO])  VALUES(@COD_EMPLEADO,@NOMBRE,@DEPARTAMENTO,@CARGO,@JEFE_INMEDIATO,@ESTADO_EVALUACION,@SEMESTRE_INGRESO,@COMPETENCIAS,@FECHA_INGRESO,@USUARIO_INGRESO)";
                cmd1.Parameters.Add("@COD_EMPLEADO", SqlDbType.NVarChar).Value = ID_EMPLEADO;
                cmd1.Parameters.Add("@NOMBRE", SqlDbType.NVarChar).Value = NOMBRE;
                cmd1.Parameters.Add("@DEPARTAMENTO", SqlDbType.NVarChar).Value = Departamento;
                cmd1.Parameters.Add("@CARGO", SqlDbType.NVarChar).Value = cargo;
                cmd1.Parameters.Add("@JEFE_INMEDIATO", SqlDbType.NVarChar).Value = JefeInmediato;
                cmd1.Parameters.Add("@ESTADO_EVALUACION", SqlDbType.NVarChar).Value = ESTADO;
                cmd1.Parameters.Add("@SEMESTRE_INGRESO", SqlDbType.NVarChar).Value = Semestre;
                cmd1.Parameters.Add("@COMPETENCIAS", SqlDbType.NVarChar).Value = Competencias;
                cmd1.Parameters.Add("@USUARIO_INGRESO", SqlDbType.NVarChar).Value = Usuario;
                cmd1.Parameters.Add("@FECHA_INGRESO", SqlDbType.DateTime).Value = Fecha_crea;



                cmd1.ExecuteNonQuery();

                con.Desconectar("DM");

                for (int i = 1; i <= Numero_Objetivo; i++)
                {
                    con.conectar("DM");
                    int Numero_Obj_insert = i;
                    Objetivo_insert = null;
                    switch (i)
                    {
                        case 1:
                            Objetivo_insert = Objetivo1;
                            break;
                        case 2:
                            if (!string.IsNullOrWhiteSpace(Objetivo2))
                            {
                                Objetivo_insert = Objetivo2;

                            }
                            else
                            {
                                Objetivo_insert = null;
                                goto case 3;
                            }
                            break;
                        case 3:
                             if (!string.IsNullOrWhiteSpace(Objetivo3))
                            {
                                Objetivo_insert = Objetivo3;

                            }
                            else
                            {
                                Objetivo_insert = null;
                                goto case 4;
                            }
                            break;
                            
                        case 4:
                             if (!string.IsNullOrWhiteSpace(Objetivo4))
                            {
                                Objetivo_insert = Objetivo4;

                            }
                            else
                            {
                                Objetivo_insert = null;
                                goto case 5;
                            }
                            break;
                        case 5:
                             if (!string.IsNullOrWhiteSpace(Objetivo5))
                            {
                                Objetivo_insert = Objetivo5;

                            }
                            else
                            {
                                Objetivo_insert = null;
                                goto case 6;
                            }
                            break;
                        case 6:
                              if (!string.IsNullOrWhiteSpace(Objetivo6))
                            {
                                Objetivo_insert = Objetivo6;

                            }
                            else
                            {
                                Objetivo_insert = null;
                                goto case 7;
                            }
                            break;
                        case 7:
                              if (!string.IsNullOrWhiteSpace(Objetivo7))
                            {
                                Objetivo_insert = Objetivo7;

                            }
                            else
                            {
                                Objetivo_insert = null;
                                goto case 8;
                            }
                            break;
                        case 8:
                              if (!string.IsNullOrWhiteSpace(Objetivo8))
                            {
                                Objetivo_insert = Objetivo8;

                            }
                            else
                            {
                                Objetivo_insert = null;
                                goto case 9;
                            }
                            break;
                        case 9:
                              if (!string.IsNullOrWhiteSpace(Objetivo9))
                            {
                                Objetivo_insert = Objetivo9;

                            }
                            else
                            {
                                Objetivo_insert = null;
                                goto case 10;
                            }
                            break;
                        case 10:
                              if (!string.IsNullOrWhiteSpace(Objetivo10))
                            {
                                Objetivo_insert = Objetivo10;

                            }
                            else
                            {
                                Objetivo_insert = null;
                                break;
                            }
                            break;
                           

                    }
                    SqlCommand cmd3 = new SqlCommand();
                    cmd3.Connection = con.condm;
                    cmd3.CommandText = "INSERT INTO [DM].[CORRECT].[OBJETIVOS_EVALUACION]([ID_EVALUACION],[OBJETIVO_NUMERO],[OBJETIVO],[FECHA_INGRESO],[USUARIO_INGRESO])  VALUES( @ID_EVALUACION,@OBJETIVO_NUMERO,@OBJETIVO,@FECHA_INGRESO,@USUARIO_INGRESO)";
                    cmd3.Parameters.Add("@ID_EVALUACION", SqlDbType.NVarChar).Value = ID_EVALUACION + 1;
                    cmd3.Parameters.Add("@OBJETIVO_NUMERO", SqlDbType.NVarChar).Value = Numero_Obj_insert;
                    cmd3.Parameters.Add("@OBJETIVO", SqlDbType.NVarChar).Value = Objetivo_insert;
                    cmd3.Parameters.Add("@USUARIO_INGRESO", SqlDbType.NVarChar).Value = Usuario;
                    cmd3.Parameters.Add("@FECHA_INGRESO", SqlDbType.DateTime).Value = Fecha_crea;



                    cmd3.ExecuteNonQuery();

                    con.Desconectar("DM");
                  
                    
                }
                int evaluacion_int = ID_EVALUACION + 1;
                MessageBox.Show("Evaluacion Numero: "+evaluacion_int+" Ingresada Exitosamente");

                this.Close();
            }
            catch
            {
                MessageBox.Show("NO SE INGRESO EVALUACION", "ERROR EVALUACION", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
            }
        
        }

        private void Objetivos_Evaluacion_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }

        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox1.Text))
            {
                richTextBox2.Enabled = true;
            }
            else
            {
                richTextBox2.Enabled = false;
            
            }
        }

        private void richTextBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox2.Text))
            {
                richTextBox3.Enabled = true;
            }
            else
            {
                richTextBox3.Enabled = false;

            }
        }

        private void richTextBox3_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox3.Text))
            {
                richTextBox4.Enabled = true;
            }
            else
            {
                richTextBox4.Enabled = false;

            }
        }

        private void richTextBox4_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox4.Text))
            {
                richTextBox5.Enabled = true;
            }
            else
            {
                richTextBox5.Enabled = false;

            }
        }

        private void richTextBox5_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox5.Text))
            {
                richTextBox6.Enabled = true;
            }
            else
            {
                richTextBox6.Enabled = false;

            }
        }

        private void richTextBox6_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox6.Text))
            {
                richTextBox7.Enabled = true;
            }
            else
            {
                richTextBox7.Enabled = false;

            }
        }

        private void richTextBox7_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox7.Text))
            {
                richTextBox8.Enabled = true;
            }
            else
            {
                richTextBox8.Enabled = false;

            }
        }

        private void richTextBox8_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox8.Text))
            {
                richTextBox9.Enabled = true;
            }
            else
            {
                richTextBox9.Enabled = false;

            }
        }

        private void richTextBox9_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox9.Text))
            {
                richTextBox10.Enabled = true;
            }
            else
            {
                richTextBox10.Enabled = false;

            }
        }

        private void Infomarcionload(string ID_EMP)
        {
            con.conectar("EX");
            SqlCommand cm1 = new SqlCommand("SELECT  A.[EMPLEADO],A.[NOMBRE],B.DESCRIPCION as PUESTO ,C.DESCRIPCION as DEPARTAMENTO,JER.NOMBRE_JEFE FROM [EXACTUS].[dismo].[EMPLEADO] A  INNER JOIN [EXACTUS].[dismo].[PUESTO] B  ON A.PUESTO = B.PUESTO INNER JOIN  [EXACTUS].[dismo].[DEPARTAMENTO] C  on A.DEPARTAMENTO = C.DEPARTAMENTO  LEFT JOIN  (SELECT JER.SUBORDINADO,JER.[SUPERIOR],EMP.NOMBRE as NOMBRE_JEFE   FROM [EXACTUS].[dismo].[EMPLEADO_JERARQUIA] as JER  LEFT JOIN [EXACTUS].[dismo].[EMPLEADO] as EMP  on JER.SUPERIOR = EMP.EMPLEADO   where EMP.ACTIVO = 'S'  Group by JER.SUPERIOR,JER.SUBORDINADO,EMP.NOMBRE     ) as JER on A.EMPLEADO = JER.SUBORDINADO 	 where A.ACTIVO = 'S' and A.EMPLEADO = '" + ID_EMP+"'", con.conex);
            SqlDataReader dr1 = cm1.ExecuteReader();
            while (dr1.Read())
            {
                NOMBRE = Convert.ToString(dr1["NOMBRE"]);
               JefeInmediato = Convert.ToString(dr1["NOMBRE_JEFE"]);
                cargo = Convert.ToString(dr1["PUESTO"]);
                Departamento = Convert.ToString(dr1["DEPARTAMENTO"]);

               
            }
            con.Desconectar("EX");
        }


        private void infosub()
        {
            con.conectar("DM");

            string consulta = "SELECT  [ID_EVALUACION] as 'EVALUACION NUMERO',[ESTADO_EVALUACION] as 'ESTADO',EV.[COD_EMPLEADO] as 'CODIGO EMPLEADO' ,EV.[NOMBRE] ,EV.[DEPARTAMENTO],[CARGO] as 'PUESTO',[JEFE_INMEDIATO]  ,[SEMESTRE_INGRESO] as 'SEMESTRE',EV.[FECHA_INGRESO] as 'Fecha de Evaluacion' FROM [DM].[CORRECT].[EVALUACION_DESEMPEÑO] as EV LEFT JOIN [EXACTUS].[dismo].[EMPLEADO] as  EM  on EV.COD_EMPLEADO = EM.EMPLEADO INNER JOIN [EXACTUS].[dismo].[EMPLEADO_JERARQUIA]  as EJ on EJ.SUBORDINADO = EM.EMPLEADO  INNER JOIN [DM].[CORRECT].[USUARIOS] as DMUSER  ON EJ.SUPERIOR = DMUSER.COD_EMPLEADO where DMUSER.COD_EMPLEADO = '" + Main_Menu.COD_EMPLEADO + "'";
            SqlCommand comando = new SqlCommand(consulta, con.condm);

            SqlDataAdapter adap = new SqlDataAdapter(comando);

          //  adap.Fill(dt2);
            con.Desconectar("DM");
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty || textBox1.Text == "")
            {
            }
            else
            {
                ID_EMPLEADO = textBox1.Text;
                Infomarcionload(textBox1.Text);
                textBox1.Text = ID_EMPLEADO;
                textBox2.Text = NOMBRE;
                textBox3.Text = Departamento;
                textBox4.Text = JefeInmediato;
                textBox5.Text = cargo;
            }
        }
    }
}
