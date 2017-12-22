using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Globalization;

namespace Sinconizacion_EXactus.CORECTX_APP.Informatica
{
    public partial class festivos : Form
    {
        public festivos()
        {
            InitializeComponent();
        }
        String Fecha;
        String Descricion;
        conexionXML con = new conexionXML();
        String fecha_seleccionada;
        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            Fecha = monthCalendar1.SelectionRange.Start.ToString("yyyy/MM/dd");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                                  con.conectar("DM");
                                SqlCommand cmd = new SqlCommand();
                                cmd.Connection = con.condm;
                                cmd.CommandText = "insert into [DM].[CORRECT].[Festivos](Nombre,Fecha) values (@Nombre,@Fecha)";

                                cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = textBox1.Text;
                                cmd.Parameters.Add("@Fecha", SqlDbType.NVarChar).Value = Fecha;

                                cmd.ExecuteNonQuery();
                                con.Desconectar("DM");
                                fechas_festivos();
                                monthCalendar1.Refresh();
                                MessageBox.Show("Festivo Ingresado");
            }
            catch
            {
                MessageBox.Show("Error Ingresando Festivo");
              
            }

        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            textBox1.Text = "";
            fecha_seleccionada = monthCalendar1.SelectionRange.Start.ToString("yyyy-MM-dd");


            con.conectar("DM");

            SqlCommand cm2 = new SqlCommand("SELECT [Nombre] FROM [DM].[CORRECT].[Festivos]  where Fecha = '" + fecha_seleccionada + "'", con.condm);
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
               
            textBox1.Text= (Convert.ToString(dr2["Nombre"]));
                               

            }
            dr2.Close();
            con.Desconectar("DM");

        }

        private void festivos_Load(object sender, EventArgs e)
        {
            fechas_festivos();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                con.conectar("DM");

                SqlCommand cm2 = new SqlCommand("DELETE [DM].[CORRECT].[Festivos]  where Fecha = '" + fecha_seleccionada + "' ", con.condm);
                cm2.ExecuteNonQuery();

                con.Desconectar("DM");
                monthCalendar1.RemoveBoldedDate(Convert.ToDateTime(fecha_seleccionada));
                textBox1.Text = "";
                monthCalendar1.Refresh();
                MessageBox.Show("Fecha eliminada exitosamente");

            }
            catch
            {
                MessageBox.Show("No se pudo anular la fecha seleccionada");
            
            }
        }

        private void fechas_festivos()
        {
            con.conectar("DM");

            SqlCommand cm2 = new SqlCommand("SELECT [Fecha] FROM [DM].[CORRECT].[Festivos] ", con.condm);
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
                DateTime datetime = new DateTime();
                datetime = (Convert.ToDateTime(dr2["Fecha"]));

                monthCalendar1.AddBoldedDate(datetime);
               
                monthCalendar1.Refresh();

            }
            dr2.Close();
            con.Desconectar("DM");
        }
    }
}
