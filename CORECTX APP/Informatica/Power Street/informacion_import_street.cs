using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sinconizacion_EXactus
{
    public partial class informacion_import_street : Form
    {
        public informacion_import_street()
        {
            InitializeComponent();
        }

        private void informacion_import_street_Load(object sender, EventArgs e)
        {
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AllowUserToAddRows = false;

            int consulta = Importador_Street.consulta;


            switch (consulta) 
            {
                case 1:
                    dataGridView1.DataSource = Importador_Street.Encabezados_PED;
                    break;
                case 2:
                    dataGridView1.DataSource = Importador_Street.Detalle_PED;
                    break;
                case 3:
                    dataGridView1.DataSource = Importador_Street.Cobros;
                    break;
                case 4:
                    dataGridView1.DataSource = Importador_Street.clientes;
                    break;
                case 5:
                    dataGridView1.DataSource = Importador_Street.Encabezados_COM;
                    break;
                case 6:
                    dataGridView1.DataSource = Importador_Street.Detalle_COM;
                   break;
                case 7:
                   dataGridView1.DataSource = Importador_Street.Lista_precios;
                   break;
                case 8:
                   dataGridView1.DataSource = Importador_Street.Clientes_Nuevos;
                   break;

                case 9:
                   dataGridView1.DataSource = Importador_Street.Errores;
                   break;
                default:
                   MessageBox.Show("No se encontro Informacion");

                   this.Close();
                   break;
                   
            }

           

           
            
        }
    }
}
