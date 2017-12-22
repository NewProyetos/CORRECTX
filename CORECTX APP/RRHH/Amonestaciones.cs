using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sinconizacion_EXactus.CORECTX_APP.RRHH
{
    public partial class Amonestaciones : Form
    {
        public Amonestaciones(DataTable dtr)
        {
            InitializeComponent();
            amon = dtr;
        }
        DataTable amon = new DataTable();
        private void Amonestaciones_Load(object sender, EventArgs e)
        {

            cargagrid();
        }

        private void cargagrid()
        {
            gridControl1.DataSource = amon;
            gridControl1.Refresh();
        }
    }
}
