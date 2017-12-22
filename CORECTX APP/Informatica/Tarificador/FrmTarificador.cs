using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace Sinconizacion_EXactus
{
    public partial class FrmTarificador : Form
    {
        public string prueba;

        public FrmTarificador()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.Open();
                String inputdata = serialPort1.ReadExisting();
              //  MessageBox.Show("wi"+inputdata);
                richTextBox1.AppendText(inputdata);
                string s = serialPort1.ReadLine().ToString();
                if (s != String.Empty)
                {
                   richTextBox1.AppendText(s);
              //      MessageBox.Show(s.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Primer Catch"+ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
       HyperTerminalAdapter oHyperTerminalAdapter = new HyperTerminalAdapter(); 
	   oHyperTerminalAdapter.Connect(); 
	   //oHyperTerminalAdapter.Write("AT"); 
       string x = oHyperTerminalAdapter.Read();
	   oHyperTerminalAdapter.Disconnect(); 
	   MessageBox.Show("Result of the command was: " + x); 
        }

        private static void DataReceivedHandler(object sender,SerialDataReceivedEventArgs e)
        {
         
        }
        private void PuertoSerie_DataReceived(object sender,System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                richTextBox1.AppendText(serialPort1.ReadLine().ToString());
                richTextBox1.ScrollToCaret();
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("en el segundo catch"+ex.Message);
            }
           
           // MessageBox.Show("ok");
        }
    }
}
