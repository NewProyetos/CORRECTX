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

namespace Sinconizacion_EXactus.CORECTX_APP.Informatica
{
    public partial class descargaarchivos : Form
    {
        public descargaarchivos()
        {
            InitializeComponent();
        }

        string Selected_File;
        conexionXML con = new conexionXML();
        byte[] archi;
        byte[] compres;
        private void descargaarchivos_Load(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.AutoUpgradeEnabled = false;
            openFileDialog1.InitialDirectory = @"%USERPROFILE%\Documents";
            openFileDialog1.Title = "Select a File";
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.Filter = "TXT|*.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {

            }
            else
            {
                Selected_File = openFileDialog1.FileName;             
               

                archi = File.ReadAllBytes(Selected_File);
                compres = compresor.comprimir(archi);


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            con.conectar("DM");
            SqlCommand cm2 = new SqlCommand("SELECT CONTENIDO FROM [EXACTUS].[dismo].[DOC_ADJUNTO] where NOMBRE = 'and.txt' ", con.condm);
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {

                archi = compresor.descompirmir(((byte[])(dr2["CONTENIDO"])));
            }
            dr2.Close();
            con.Desconectar("DM");

            string direc = @"C:\CORRECT\GPS\miart.txt";

            File.WriteAllBytes(direc, archi);
        }
    }
}
