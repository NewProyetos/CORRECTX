using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace Sinconizacion_EXactus.CORECTX_APP.BODEGA.REGALIAS
{
    public partial class Traspasos : Form
    {
        public Traspasos()
        {
            InitializeComponent();
        }
        DataTable dt = new DataTable();
        conexionXML con = new conexionXML();
        String Trasspaso;
        String fecha_aplicacion;
        private void Traspasos_Load(object sender, EventArgs e)
        {
            linkLabel1.Hide();
            button2.Enabled = false;
            dataGridView1.Enabled = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AllowUserToAddRows = false;
            addchekdw();
          

            Usuarios();
            // cargadata();

            if (Main_Menu.Puesto == "BOD")
            {
                comboBox1.Text = Login.usuario.ToUpper();
                cargadata(comboBox1.Text);
            }

        }
        private void addchekdw()
        {

            DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn()

            {
                Name = "Asignar"

            };
            dataGridView1.Columns.Add(chk);


        }


        private void UpdateCellValue(int CurrentRowIndex)
        {
            if (CurrentRowIndex < 0)
                return;
            dataGridView1.Rows[CurrentRowIndex].Cells[0].Value = true;
            dataGridView1.EndEdit();
            if (CurrentRowIndex > -1)
            {
                for (int row = 0; row < dataGridView1.Rows.Count; row++)
                {
                    if (CurrentRowIndex != row)
                        dataGridView1.Rows[row].Cells[0].Value = false;
                }

                valor_rowchek();
                button2.Enabled = true;
                linkLabel1.Show();
            }
           
        }

        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            UpdateCellValue(e.RowIndex);
        }
        private void cargadata(string usuario)
        {
            dt.Clear();

            con.conectar("DM");
            SqlCommand cmd2 = new SqlCommand("SELECT [APLICACION] ,[AUDIT_TRANS_INV] ,[USUARIO] ,[FECHA_HORA] ,[REFERENCIA] FROM [EXACTUS].["+Login.empresa+"].[AUDIT_TRANS_INV]   where APLICACION not in (SELECT [REGALIA] FROM [DM].[CORRECT].[REGALIAS_SOLICI_ENC]  where REGALIA is not null and ESTADO = 'L') and USUARIO = '" + usuario + "' and APLICACION like 'REGALIA%'  and (DATEADD(dd, 0, DATEDIFF(dd, 0, FECHA_HORA )) >= '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "') AND (DATEADD(dd, 0, DATEDIFF(dd, 0, FECHA_HORA )) <= '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "') ", con.condm);
            SqlDataAdapter da = new SqlDataAdapter(cmd2);
            da.Fill(dt);

            dataGridView1.DataSource = dt;
            dataGridView1.Refresh();

            // combo(dt);
            con.Desconectar("DM");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != string.Empty || comboBox1.Text != "" || comboBox1.Text != null)
            {
                cargadata(comboBox1.Text);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            fecha_aplicacion = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            string Tras_insert;
            Tras_insert = valor_rowchek();

            try
            {


                con.conectar("DM");

                SqlCommand cmd8 = new SqlCommand();
                cmd8.Connection = con.condm;
                cmd8.CommandText = "UPDATE [DM].[CORRECT].[REGALIAS_SOLICI_ENC] SET USUARIO_APLICA= @USUARIO_APLICA ,FECHA_APLIC=@FECHA_APLIC,REGALIA = @REGALIA,ESTADO = @ESTADO   where NUM_REG ='" + Regalias.regalia + "'";
                cmd8.Parameters.Add("@USUARIO_APLICA", SqlDbType.NVarChar).Value = Login.usuario.ToUpper();
                cmd8.Parameters.Add("@FECHA_APLIC", SqlDbType.NVarChar).Value = fecha_aplicacion;
                cmd8.Parameters.Add("@REGALIA", SqlDbType.NVarChar).Value = Tras_insert;
                cmd8.Parameters.Add("@ESTADO", SqlDbType.Char).Value = 'L';


                cmd8.ExecuteNonQuery();

                con.Desconectar("DM");

                Update_traspaso(Tras_insert, Regalias.regalia);

                // MessageBox.Show("Se agregaron las NC a la Liquidacion del dia " + fecha_liquidacion + "");
                this.Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != string.Empty || comboBox1.Text != "" || comboBox1.Text != null)
            {

                cargadata(comboBox1.Text);
            }
            else
            {

            }
        }
        private void Usuarios()
        {

            con.conectar("DM");

            SqlCommand cm1 = new SqlCommand("SELECT [USUARIO] FROM [DM].[CORRECT].[USUARIOS] where PUESTO = 'BOD'", con.condm);
            SqlDataReader dr1 = cm1.ExecuteReader();
            while (dr1.Read())
            {
                comboBox1.Items.Add(dr1["USUARIO"]);

            }
            dr1.Close();
            con.Desconectar("DM");
        }

        private void Update_traspaso(string trasspaso, string regalia)
        {

            string fecha_aplicacion = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
           

            con.conectar("EX");
            SqlCommand cmd9 = new SqlCommand();
            cmd9.Connection = con.conex;
            cmd9.CommandText = "UPDATE [EXACTUS].["+Login.empresa+"].[AUDIT_TRANS_INV] SET REFERENCIA = '" + regalia + " Asignadad por : " + Login.usuario.ToUpper() + " FECHA: " + fecha_aplicacion + "'  where APLICACION = '" + trasspaso + "'";
            cmd9.ExecuteNonQuery();
            con.Desconectar("EX");





        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

           
        }




        private void anular_traspaso(string traspas)
        {
            MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("SE ANULARA LA REGALIA #: " + traspas + " ESTA SEGURO QUE DECEA ANULAR", "ANULACION DE TRASPASO", bt1, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {

                string fecha_aplicacion = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                con.conectar("EX");
                SqlCommand cmd9 = new SqlCommand();
                cmd9.Connection = con.conex;
                cmd9.CommandText = "UPDATE [EXACTUS].["+Login.empresa+"].[AUDIT_TRANS_INV] SET REFERENCIA = 'ANULACION REGALIA: " + traspas + " por USUARIO: " + Login.usuario.ToUpper() + " FECHA: " + fecha_aplicacion + "' , APLICACION = 'N'+APLICACION where APLICACION = '" + traspas + "'";
                cmd9.ExecuteNonQuery();
                con.Desconectar("EX");

            }

        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
            anular_traspaso(valor_rowchek());
        }

        private string valor_rowchek()
        {
            //string valor;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow row = dataGridView1.Rows[i];
                DataGridViewCheckBoxCell cell = row.Cells[0] as DataGridViewCheckBoxCell;
                if (Convert.ToBoolean(cell.Value) == true)
                {
                   
                    Trasspaso = Convert.ToString(row.Cells[1].Value);

                }
            }
            return Trasspaso;

          
        }

        private void dataGridView1_DataBindingComplete_1(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridView1.RowCount >= 1)
            {
                
                
               
            }
        }
    }
}
