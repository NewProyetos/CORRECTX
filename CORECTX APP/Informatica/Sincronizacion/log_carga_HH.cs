using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace Sinconizacion_EXactus
{
    public partial class log_carga_HH : Form
    {
        public log_carga_HH()
        {
            InitializeComponent();
        }
        int index = 0;
        string stado;
        string select_file;
        string text;
        FileStream fs;

        byte[] bytes;
        private void Form3_Load(object sender, EventArgs e)
        {

            backgroundWorker1.RunWorkerAsync();

            pictureBox1.Show();
            this.richTextBox1.Clear();
           
                      

             //this.richTextBox1.Text = text;

             //string temp = richTextBox1.Text;
             //richTextBox1.Text = string.Empty;
             //richTextBox1.Text = temp;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (stado == "ERROR")
            {
                richTextBox1.Find("ERROR", index, richTextBox1.TextLength, RichTextBoxFinds.None);

                richTextBox1.SelectionBackColor = Color.Red;
                index = richTextBox1.Text.IndexOf("ERROR", index) + 1;



                richTextBox1.Focus();
                richTextBox1.SelectionStart = index;
            }
            else if (stado == "Error")
            {
                richTextBox1.Find("Error", index, richTextBox1.TextLength, RichTextBoxFinds.None);
                richTextBox1.SelectionBackColor = Color.Green;
                index = richTextBox1.Text.IndexOf("Error", index) + 1;

                richTextBox1.Focus();
                richTextBox1.SelectionStart = index;
            
            }
            
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            

           

           
            //try
            //{

            select_file = Sincronizacion.Selected_File;

            fs = new FileStream(select_file, FileMode.Open);

            bytes = new byte[fs.Length];
            fs.Read(bytes, 0, bytes.Length);

                 text = Encoding.ASCII.GetString(bytes);




                if (text.Contains("****ERROR****"))
                {
                    stado = "ERROR";
                    
                                   
                }


                else if (text.Contains("Error"))
                {
                    stado = "Error";                          


                }

                else
                {

                    this.richTextBox1.Text = "No hay Errores";
                }
                fs.Close();
            }
            //catch
            //{
            //    MessageBox.Show("ERROR AL CARGAR ARCHIVO LOG");
        //   }
        //}

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pictureBox1.Hide();
        }

        private void findTextbox()
        {
            if (stado == "ERROR")
            {

                this.richTextBox1.Text = text;


                string temp = richTextBox1.Text;
                richTextBox1.Text = string.Empty;
                richTextBox1.Text = temp;


                while (index < richTextBox1.Text.LastIndexOf("****ERROR****"))
                {

                    richTextBox1.Find("****ERROR****", index, richTextBox1.TextLength, RichTextBoxFinds.None);
                    richTextBox1.SelectionBackColor = Color.Red;
                    index = richTextBox1.Text.IndexOf("****ERROR****", index) + 15;
                    richTextBox1.Focus();
                    richTextBox1.SelectionStart = index;
                }
            }

            else if (stado == "Error")
            {
                this.richTextBox1.Text = text;


                string temp = richTextBox1.Text;
                richTextBox1.Text = string.Empty;
                richTextBox1.Text = temp;


                while (index < richTextBox1.Text.LastIndexOf("Error"))
                {

                    richTextBox1.Find("Error", index, richTextBox1.TextLength, RichTextBoxFinds.None);
                    richTextBox1.SelectionBackColor = Color.Green;
                    index = richTextBox1.Text.IndexOf("Error", index) + 15;
                    richTextBox1.Focus();
                    richTextBox1.SelectionStart = index;



                }

            }
            else
            {

                this.richTextBox1.Text = "No hay Errores";
            }
        
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            findTextbox();
            pictureBox1.Hide();
        }
    }

}
