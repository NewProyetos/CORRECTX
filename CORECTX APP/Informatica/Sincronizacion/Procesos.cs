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
    public partial class Procesos : Form
    {
        public Procesos()
        {
            InitializeComponent();
        }
        Sincronizacion fecha = new Sincronizacion();
        //conexion conet = new conexion();
        conexionXML con = new conexionXML();
        public string sqlcadena1;
        public string sqlcadena2;
        private void Form2_Load(object sender, EventArgs e)
        {
            
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = " dd-MM-yyyy";

            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = " dd-MM-yyyy";

           // label3.Text= DateTime.Now.ToString("dddd", new System.Globalization.CultureInfo("es-ES"));
            label3.Text = DateTime.Now.ToString("tt");
            con.conectar("EX");

            SqlCommand cm2 = new SqlCommand("SELECT USUARIO FROM dismo.AUDITORIA_DE_PROC GROUP BY USUARIO  ", con.conex);
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
                comboBox1.Items.Add(dr2["USUARIO"]);
            }
            dr2.Close();



            listView1.View = View.Details; //Detalles encabezado
            listView1.GridLines = true; //Lineas tipo cuaderno
            listView1.FullRowSelect = true;
            try
            {
                                
            }

            catch
            {
                MessageBox.Show("conexion Error");
            }
            listView1.Clear();
            listView1.Columns.Add("USUARIO", 80, HorizontalAlignment.Left);
            listView1.Columns.Add("MODULO", 50, HorizontalAlignment.Left);
            listView1.Columns.Add("TABLA", 80, HorizontalAlignment.Left);
            listView1.Columns.Add("PROCESO", 180, HorizontalAlignment.Left);
            listView1.Columns.Add("FECHA / HORA", 180, HorizontalAlignment.Left);

            SqlCommand cm1 = new SqlCommand("SELECT USUARIO,ORIGEN,OPCION,NOTAS,FECHA_HORA FROM dismo.AUDITORIA_DE_PROC WHERE DATEADD(dd, 0, DATEDIFF(dd, 0, FECHA_HORA))>='"+Sincronizacion.fecha_sinc+"' ", con.conex);
            
             SqlDataReader dr1 = cm1.ExecuteReader();
         
                                 
            
           while(dr1.Read())
           {
               
               ListViewItem lvItem = new ListViewItem();
               lvItem.SubItems[0].Text = dr1[0].ToString();
               lvItem.SubItems.Add(dr1[1].ToString());
               lvItem.SubItems.Add(dr1[2].ToString());
               lvItem.SubItems.Add(dr1[3].ToString());
               lvItem.SubItems.Add(dr1[4].ToString());
               listView1.Items.Add(lvItem); 
           }
           dr1.Close();
           con.Desconectar("EX");
          
           }

        private void button1_Click(object sender, EventArgs e)
        {


            con.conectar("EX");
            listView1.Clear();
            listView1.Columns.Add("USUARIO", 80, HorizontalAlignment.Left);
            listView1.Columns.Add("MODULO", 50, HorizontalAlignment.Left);
            listView1.Columns.Add("TABLA", 80, HorizontalAlignment.Left);
            listView1.Columns.Add("PROCESO", 180, HorizontalAlignment.Left);
            listView1.Columns.Add("FECHA / HORA", 180, HorizontalAlignment.Left);

          
                SqlCommand cm3 = new SqlCommand("SELECT USUARIO,ORIGEN,OPCION,NOTAS,FECHA_HORA FROM dismo.AUDITORIA_DE_PROC WHERE USUARIO = '"+this.comboBox1.Text+"' AND DATEADD(dd, 0, DATEDIFF(dd, 0, FECHA_HORA))>='" + this.dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' AND DATEADD(dd, 0, DATEDIFF(dd, 0, FECHA_HORA))<='" + this.dateTimePicker2.Value.ToString("yyyy-MM-dd") + "' ORDER BY FECHA_HORA ASC" , con.conex);
                SqlDataReader dr3 = cm3.ExecuteReader();



                while (dr3.Read())
                {

                    ListViewItem lvItem = new ListViewItem();
                    lvItem.SubItems[0].Text = dr3[0].ToString();
                    lvItem.SubItems.Add(dr3[1].ToString());
                    lvItem.SubItems.Add(dr3[2].ToString());
                    lvItem.SubItems.Add(dr3[3].ToString());
                    lvItem.SubItems.Add(dr3[4].ToString());
                    listView1.Items.Add(lvItem);
                }
                dr3.Close();
                
            con.Desconectar("EX");
        }
   
        
    
    


        }
        }
    

