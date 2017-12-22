using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sinconizacion_EXactus.CORECTX_APP.VENTAS
{
    public partial class wait : Form
    {
        public wait()
        {
            InitializeComponent();
        }

        public void button1_Click(object sender, EventArgs e)
        {

            Rutas_Trade_Reps rt = new Rutas_Trade_Reps();
            //rt.cancelprocess();

            if (rt.backgroundRutero.IsBusy)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("El proceso de aun esta ejecutandoce...");
            }
        }

        private void wait_Load(object sender, EventArgs e)
        {

        }
    }
}
