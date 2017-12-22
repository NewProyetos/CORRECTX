using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Sinconizacion_EXactus.CORECTX_APP.BODEGA
{
    public partial class costos_articulo : Form
    {
        public costos_articulo()
        {
            InitializeComponent();
        }

        conexionXML con = new conexionXML();
        double COSTO_PROM_LOC;
        double COSTO_PROM_DOL;
        double COSTO_STD_LOC;
        double COSTO_STD_DOL;
        double COSTO_ULT_LOC;
        double COSTO_ULT_DOL;

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                busqueda_costo(textBox1.Text);
                fill_box();
            }

        }

        private void busqueda_costo(string articulo)
        {
            con.conectar("EX");


            SqlCommand cm4 = new SqlCommand("SELECT [COSTO_PROM_LOC] ,[COSTO_PROM_DOL],[COSTO_STD_LOC],[COSTO_STD_DOL],[COSTO_ULT_LOC],[COSTO_ULT_DOL] FROM [EXACTUS].["+Login.empresa+"].[ARTICULO] where ARTICULO = '"+articulo+"'", con.conex);
            SqlDataReader dr = cm4.ExecuteReader();
            while (dr.Read())
            {
                COSTO_PROM_LOC = Convert.ToDouble(dr["COSTO_PROM_LOC"]);
                COSTO_PROM_DOL = Convert.ToDouble(dr["COSTO_PROM_DOL"]);
                COSTO_STD_LOC = Convert.ToDouble(dr["COSTO_STD_LOC"]);
                COSTO_STD_DOL = Convert.ToDouble(dr["COSTO_STD_DOL"]);
                COSTO_ULT_LOC = Convert.ToDouble(dr["COSTO_ULT_LOC"]);
                COSTO_ULT_DOL = Convert.ToDouble(dr["COSTO_ULT_DOL"]);


            }
            dr.Close();


            con.Desconectar("EX");

        }

        private void fill_box()
        {
            if (COSTO_PROM_LOC > 0.0)
            {
                textBox2.Enabled = false;
                textBox2.Text = Convert.ToString(COSTO_PROM_LOC);
            }
            else

            {
                textBox2.Enabled = true;
                textBox2.Text = Convert.ToString(COSTO_PROM_LOC);

            }

            if (COSTO_PROM_DOL > 0.0)
            {
                textBox3.Enabled = false;
                textBox3.Text = Convert.ToString(COSTO_PROM_DOL);
            }
            else

            {
                textBox3.Enabled = true;
                textBox3.Text = Convert.ToString(COSTO_PROM_DOL);

            }


            if (COSTO_STD_LOC > 0.0)
            {
                textBox4.Enabled = false;
                textBox4.Text = Convert.ToString(COSTO_STD_LOC);
            }
            else

            {
                textBox4.Enabled = true;
                textBox4.Text = Convert.ToString(COSTO_STD_LOC);

            }


            if (COSTO_STD_DOL > 0.0)
            {
                textBox5.Enabled = false;
                textBox5.Text = Convert.ToString(COSTO_STD_DOL);
            }
            else

            {
                textBox5.Enabled = true;
                textBox5.Text = Convert.ToString(COSTO_STD_DOL);

            }

            if (COSTO_ULT_LOC > 0.0)
            {
                textBox6.Enabled = false;
                textBox6.Text = Convert.ToString(COSTO_ULT_LOC);
            }
            else

            {
                textBox6.Enabled = true;
                textBox6.Text = Convert.ToString(COSTO_ULT_LOC);

            }

            if (COSTO_ULT_DOL > 0.0)
            {
                textBox7.Enabled = false;
                textBox7.Text = Convert.ToString(COSTO_ULT_DOL);
            }
            else

            {
                textBox7.Enabled = true;
                textBox7.Text = Convert.ToString(COSTO_ULT_DOL);

            }



        }

        private void costos_articulo_Load(object sender, EventArgs e)
        {
           // button1.Enabled = false;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            keypress(textBox2,e);

        }

        private void keypress(TextBox txt, KeyPressEventArgs e)
        {
            if (txt.Text.Contains('.'))
            {
                if (!char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }

                if (e.KeyChar == '\b')
                {
                    e.Handled = false;
                }
            }

            else
            {
                if (!char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }

                if (e.KeyChar == '+' || e.KeyChar == '.' || e.KeyChar == '\b')
                {
                    e.Handled = false;
                }
            }

        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            keypress(textBox4, e);
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            keypress(textBox5, e);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            keypress(textBox3, e);
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            keypress(textBox6, e);
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            keypress(textBox7, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (valida_())
            {
                acutalizar(textBox1.Text);
            }
            else
            {

            }
        }

        private void acutalizar(string arti)
        {
            con.conectar("EX");


            SqlCommand cm4 = new SqlCommand("UPDATE[EXACTUS].["+Login.empresa+"].[ARTICULO] SET[COSTO_PROM_LOC] = '"+textBox2.Text+"',[COSTO_PROM_DOL] = '"+textBox3.Text+"',[COSTO_STD_LOC] = '"+textBox4.Text+"',[COSTO_STD_DOL] = '"+textBox5.Text+"',[COSTO_ULT_LOC] = '"+textBox6.Text+"',[COSTO_ULT_DOL] = '"+textBox7.Text+"' where ARTICULO = '"+arti+"' ", con.conex);
            cm4.ExecuteNonQuery();

            con.Desconectar("EX");


            MessageBox.Show("Costos Actualizados Exitosamente");

            busqueda_costo(textBox1.Text);
            fill_box();
        }

        private bool valida_()
        {
            if (textBox2.Text != Convert.ToString(COSTO_PROM_LOC))
            {
                
                return true;
            }
            else if (textBox3.Text != Convert.ToString(COSTO_PROM_DOL))
            {
                return true;

            }
            else if (textBox4.Text != Convert.ToString(COSTO_STD_LOC))
            {
                return true;
            }
            else if (textBox5.Text != Convert.ToString(COSTO_STD_DOL))
            {
                return true;
            }

            else if (textBox6.Text != Convert.ToString(COSTO_ULT_LOC))
            {
                return true;
            }
            else if (textBox7.Text != Convert.ToString(COSTO_ULT_DOL))
            {
                return true;
            }
            else

            {
                return false;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            busqueda_costo(textBox1.Text);
            fill_box();
        }
    }
}
