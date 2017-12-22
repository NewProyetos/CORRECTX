using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace Sinconizacion_EXactus
{
    public partial class Ruteroreporte : Form
    {
        conexionXML con = new conexionXML();
        //Conexion2 coned = new Conexion2();
        DataTable RUTERO = new DataTable();
        DataSet ds = new DataSet();
        private Exportador exp = new Exportador();
        public string HoyH;

        public bool stado;
        public string Ruta;

        public Ruteroreporte()
        {
            InitializeComponent();

            exp.OnProgressUpdate += exp_Process;

        }


        private void exp_Process(int value)
        {
            base.Invoke((Action)delegate
            {
                int per = (value + 1) * 100 / RUTERO.Rows.Count;
                label1.Text = "Cargando Registros  "+Convert.ToString(value+1);
                progressBar1.Value = per;
            });

        }

        
        private void Ruteroreporte_Load(object sender, EventArgs e)
        {

            this.Text = " REPORTE RUTEROS  (" + Login.empresa + " ) ";
            this.label1.Hide();
            HoyH = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            
            comboBox1.Text = "";
           con.conectar("DM");
            SqlCommand cm1 = new SqlCommand("SELECT  RUTA FROM [DM].[dbo].[RUTERO] where EMPRESA = '"+Login.empresa+"' group by RUTA order by RUTA", con.condm);
            SqlDataReader dr1 = cm1.ExecuteReader();
            while (dr1.Read())
            {
                comboBox1.Items.Add(dr1["RUTA"]);
            }
            dr1.Close();
            con.Desconectar("DM");
        }

        private void filtro(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                comboBox1.Text = "";
                comboBox1.Enabled = false;

            
            }
            else if (radioButton2.Checked)
            {

                comboBox1.Enabled = true;
            }
        
        
        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            filtro(null, null);
           

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            filtro(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "0";
            label1.Show();
            RUTERO.Clear();
            Ruta = this.comboBox1.Text;

            if (ds.Tables.Contains("RUTERO"))
            {
                ds.Tables.Remove(RUTERO);
            }

            if (this.radioButton1.Checked)
            {
                stado = true;
               
                backgroundWorker1.RunWorkerAsync();
            }
            else if (this.comboBox1.Text != "")
            {
                stado = false;
                backgroundWorker1.RunWorkerAsync();
            }

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
        

            if (Rutas.repot == 1)
            {
                if (stado == true)
                {
                   con.conectar("DM");
                    SqlCommand cmd2 = new SqlCommand("SELECT A.[RUTA],A.[ORDEN],A.[CLIENTE],B.NOMBRE,case A.DIA  WHEN '0' THEN 'LUNES'  WHEN '1' THEN 'MARTES'  WHEN '2' THEN 'MIERCOLES'  WHEN '3' THEN 'JUEVES'  WHEN '4' THEN 'VIERNES'  WHEN '5' THEN 'SABADO'  WHEN '6' THEN 'ESPECIAL'  ELSE 'N/A'  END as 'DIA' ,A.[SEMANA]  FROM [DM].[dbo].[RUTERO] A  LEFT JOIN (SELECT NOMBRE,CLIENTE  FROM [EXACTUS].["+Login.empresa+ "].[CLIENTE])B  ON A.CLIENTE = B.CLIENTE  where EMPRESA ='" + Login.empresa + "' order by  A.RUTA,A.DIA,A.SEMANA,A.ORDEN", con.condm);
                    SqlDataAdapter da1 = new SqlDataAdapter(cmd2);
                    da1.Fill(RUTERO);
                    con.Desconectar("DM");

                    RUTERO.TableName = "RUTERO";
                    ds.Tables.Add(RUTERO);

                    exp.NombreReporte = "RUTERO COMPLETO DISMO";
                    exp.aExcel(ds);
                }
                else
                {
                   con.conectar("DM");
                    SqlCommand cmd2 = new SqlCommand("SELECT A.[RUTA],A.[ORDEN],A.[CLIENTE],B.NOMBRE,case A.DIA  WHEN '0' THEN 'LUNES'  WHEN '1' THEN 'MARTES'  WHEN '2' THEN 'MIERCOLES'  WHEN '3' THEN 'JUEVES'  WHEN '4' THEN 'VIERNES'  WHEN '5' THEN 'SABADO'  WHEN '6' THEN 'ESPECIAL'  ELSE 'N/A'  END as 'DIA' ,A.[SEMANA]  FROM [DM].[dbo].[RUTERO] A  LEFT JOIN (SELECT NOMBRE,CLIENTE  FROM [EXACTUS].["+Login.empresa+"].[CLIENTE])B  ON A.CLIENTE = B.CLIENTE  WHERE RUTA = '" + Ruta + "' and EMPRESA ='"+Login.empresa+"' order by  A.RUTA,A.DIA,A.SEMANA,A.ORDEN", con.condm);
                    SqlDataAdapter da1 = new SqlDataAdapter(cmd2);
                    da1.Fill(RUTERO);
                    con.Desconectar("DM");
                    ds.Tables.Add(RUTERO);
                    exp.NombreReporte = "RUTERO COMPLETO RUTA  " + Ruta + "";
                    exp.aExcel(ds);
                }

            }

            else if (Rutas.repot == 2)
            { 
            

            }

            else if (Rutas.repot == 3)
            { 
            
            
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
          
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
         // label1.Text += Convert.ToString(e.ProgressPercentage);


        }
    }
}
