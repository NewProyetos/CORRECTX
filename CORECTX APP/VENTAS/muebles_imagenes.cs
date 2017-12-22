using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Windows.Media.Imaging;


namespace Sinconizacion_EXactus.CORECTX_APP.VENTAS
{
    public partial class muebles_imagenes : Form
    {
        public muebles_imagenes()
        {
            InitializeComponent();
        }
       // byte[] foto = new byte[0];
        Int32 convimg;
        conexionXML con = new conexionXML();


        private void muebles_imagenes_Load(object sender, EventArgs e)
        {
            con.conectar("EX");

            SqlCommand cm1 = new SqlCommand("SELECT VENDEDOR FROM [EXACTUS].[dismo].[CLIENTE] where RowPointer in (SELECT [ROW_ID] FROM [EXACTUS].[dismo].[DOC_ADJUNTO])  group by VENDEDOR  ", con.conex);
            SqlDataReader dr1 = cm1.ExecuteReader();
            while (dr1.Read())
            {
                comboBox1.Items.Add(dr1["VENDEDOR"]);

            }
            dr1.Close();



            SqlCommand cm2 = new SqlCommand("SELECT RUBRO3_CLI as CANAL FROM [EXACTUS].[dismo].[CLIENTE] where RowPointer in (SELECT [ROW_ID] FROM [EXACTUS].[dismo].[DOC_ADJUNTO])  group by RUBRO3_CLI", con.conex);
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
                comboBox2.Items.Add(dr2["CANAL"]);

            }
            dr2.Close();


            con.Desconectar("EX");

          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            muebles_merchan.MUEBLES_MECHAN_CLIENTE.Clear();
            cargadata();

            this.reportViewer1.LocalReport.ReportPath = @"C:\CORRECT\CORECTX APP\VENTAS\muebles.rdlc";
            this.reportViewer1.RefreshReport();
        }

        private void cargadata()
        {

            con.conectar("DM");
            SqlCommand cmd1 = new SqlCommand("[CORRECT].[MUEBLES_MECHAN_CLIENTE]", con.condm);
            cmd1.CommandType = CommandType.StoredProcedure;

            if (comboBox1.Text == "")
            {
                cmd1.Parameters.AddWithValue("@vendedor", null);
            }
            else
            {
                cmd1.Parameters.AddWithValue("@vendedor", comboBox1.Text);
            }
            if (comboBox2.Text == "")
            {
                cmd1.Parameters.AddWithValue("@canal", null);
            }
            else
            {
                cmd1.Parameters.AddWithValue("@canal", comboBox2.Text);
            }

            if (textBox1.Text == "")
            {
                cmd1.Parameters.AddWithValue("@cliente", null);
            }
            else
            {
                cmd1.Parameters.AddWithValue("@cliente", textBox1.Text);
            }

            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            DataTable dt = new DataTable();
            da.Fill(dt);


            con.Desconectar("DM");

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                DataRow row = dt.Rows[i];


                string CODIGO = Convert.ToString(row["CODIGO"]);
                string NOMBRE = Convert.ToString(row["NOMBRE"]);
                string DIRECCION = Convert.ToString(row["DIRECCION"]);
                string CANAL = Convert.ToString(row["CANAL"]);
                string DESCRIPCION = Convert.ToString(row["DESCRIPCION"]);
                string NOMBREART = Convert.ToString(row["Nombre Archivo"]);
                byte[] image = compresor.descompirmir(((byte[])(row["CONTENIDO"])));


                muebles_merchan.MUEBLES_MECHAN_CLIENTE.Rows.Add(CODIGO, NOMBRE, DIRECCION, CANAL, DESCRIPCION, NOMBREART, image);

            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            comboBox1.Text = "";
            comboBox2.Text = "";
        }
    }


}
