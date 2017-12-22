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
    public partial class FrmPromociones : Form
    {
        DataTable dt = new DataTable();
        conexionXML con = new conexionXML();
        public FrmPromociones()
        {
            InitializeComponent();
           
                try
                {
                    con.conectar("DM");
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [CORRECT].[BONIFICACION_x_CLIE]", con.condm);
                    //da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    //da.SelectCommand.Parameters.Add("@FINI", SqlDbType.DateTime);
                    //da.SelectCommand.Parameters.Add("@FFIN", SqlDbType.DateTime);

                    //da.SelectCommand.Parameters["@FINI"].Value = Convert.ToDateTime(FechaIni.Value.ToShortDateString());
                    //da.SelectCommand.Parameters["@FFIN"].Value = Convert.ToDateTime(FechaFin.Value.ToShortDateString());

                    dt.Clear();
                    da.Fill(dt);

                    this.dataGridView1.DataSource = dt;

                    con.Desconectar("DM");

                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se Pudo conectar a la BD Error: " + ex.Message);
                }
            
        }

        private void bONIFICACION_x_CLIEBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bONIFICACION_x_CLIEBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dataSetof);

        }

        private void FrmPromociones_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'dataSetof.DCTO_ART_X_CLIENTE' Puede moverla o quitarla según sea necesario.
            this.dCTO_ART_X_CLIENTETableAdapter.Fill(this.dataSetof.DCTO_ART_X_CLIENTE);
            // TODO: esta línea de código carga datos en la tabla 'dataSetof.BONIFICACION_x_CLIE' Puede moverla o quitarla según sea necesario.
            this.bONIFICACION_x_CLIETableAdapter.Fill(this.dataSetof.BONIFICACION_x_CLIE);

        }
    }
}
