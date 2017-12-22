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
    public partial class No_Sinc : Form
    {
        public No_Sinc()
        {
            InitializeComponent();
        }
       // conexion conet = new conexion();
        conexionXML con = new conexionXML();

        private void Form4_Load(object sender, EventArgs e)
        {
            listView1.View = View.Details; //Detalles
            listView1.GridLines = true; //Lineas
            listView1.FullRowSelect = true;


            if (Sincronizacion.time == "Tarde")
            {
                try
                {
                    listView1.Clear();
                    listView1.Columns.Add("Ruta", 50, HorizontalAlignment.Left);
                    listView1.Columns.Add("Nombre", 120, HorizontalAlignment.Left);

                    con.conectar("EX");
                    SqlCommand cm1 = new SqlCommand("SELECT A. HANDHELD,B.NOMBRE FROM  ERPADMIN.RUTA_ASIGNADA_RT A INNER JOIN ERPADMIN.AGENTE_RT B ON A.AGENTE = B.AGENTE WHERE A.COMPANIA = '" + Login.empresa + "' AND A. HANDHELD NOT IN (SELECT PDA FROM [EXACTUS].[ERPADMIN].[EMM_SYNCINFO] WHERE  CONDUIT = '" + Sincronizacion.conduit + "' AND  SYNC_START BETWEEN '" + Sincronizacion.fecha_sinc + " 12:00:00' AND '" + Sincronizacion.fecha_sinc + " 23:59:00')AND B.NOMBRE <> 'OFFLINE'  GROUP BY A.HANDHELD,B.NOMBRE", con.conex);
                    SqlDataReader dr1 = cm1.ExecuteReader();
                    while (dr1.Read())
                    {
                        ListViewItem lvItem = new ListViewItem();
                        lvItem.SubItems[0].Text = dr1[0].ToString();
                        lvItem.SubItems.Add(dr1[1].ToString());

                        listView1.Items.Add(lvItem);
                    }
                    dr1.Close();
                    con.Desconectar("EX");
                }
                catch
                {
                    MessageBox.Show("Error Carga de Rutas");
                    con.Desconectar("EX");
                }
            }
            else
            
                if (Sincronizacion.time == "Mañana")
                {

                    try
                    {
                        listView1.Clear();
                        listView1.Columns.Add("Ruta", 50, HorizontalAlignment.Left);
                        listView1.Columns.Add("Nombre", 120, HorizontalAlignment.Left);

                        con.conectar("EX");
                        SqlCommand cm1 = new SqlCommand("SELECT A. HANDHELD,B.NOMBRE FROM  ERPADMIN.RUTA_ASIGNADA_RT A INNER JOIN ERPADMIN.AGENTE_RT B ON A.AGENTE = B.AGENTE WHERE A.COMPANIA = '"+Login.empresa+"' AND A. HANDHELD NOT IN (SELECT PDA FROM [EXACTUS].[ERPADMIN].[EMM_SYNCINFO] WHERE A.COMPANIA = '" + Login.empresa + "' AND  CONDUIT = '" + Sincronizacion.conduit + "' AND PDA LIKE 'P%' AND  SYNC_START BETWEEN '" + Sincronizacion.fecha_sinc + " 1:00:00' AND '" + Sincronizacion.fecha_sinc + " 12:00:00')AND B.NOMBRE <> 'OFFLINE' GROUP BY A.HANDHELD,B.NOMBRE", con.conex);
                        SqlDataReader dr1 = cm1.ExecuteReader();
                        while (dr1.Read())
                        {
                            ListViewItem lvItem = new ListViewItem();
                            lvItem.SubItems[0].Text = dr1[0].ToString();
                            lvItem.SubItems.Add(dr1[1].ToString());

                            listView1.Items.Add(lvItem);
                        }
                        dr1.Close();
                        con.Desconectar("EX");
                    }
                    catch
                    {
                        MessageBox.Show("Error Carga de Rutas");
                        con.Desconectar("EX");
                    }
                }


                else if (Sincronizacion.time == "Todos")
                {
                    //try
                    //{
                        listView1.Clear();
                        listView1.Columns.Add("Ruta", 50, HorizontalAlignment.Left);
                        listView1.Columns.Add("Nombre", 120, HorizontalAlignment.Left);

                        con.conectar("EX");
                        SqlCommand cm1 = new SqlCommand("SELECT A. HANDHELD,B.NOMBRE FROM  ERPADMIN.RUTA_ASIGNADA_RT A INNER JOIN ERPADMIN.AGENTE_RT B ON A.AGENTE = B.AGENTE WHERE  A.COMPANIA = '" + Login.empresa+"' AND A. HANDHELD NOT IN (SELECT PDA FROM [EXACTUS].[ERPADMIN].[EMM_SYNCINFO] WHERE A.COMPANIA = '" + Login.empresa + "' AND  CONDUIT = '" + Sincronizacion.conduit + "' AND PDA LIKE 'P%' AND  SYNC_START BETWEEN '" + Sincronizacion.fecha_sinc + " 1:00:00' AND '" + Sincronizacion.fecha_sinc + " 23:59:59')AND B.NOMBRE <> 'OFFLINE' GROUP BY A.HANDHELD,B.NOMBRE", con.conex);
                        SqlDataReader dr1 = cm1.ExecuteReader();
                        while (dr1.Read())
                        {
                            ListViewItem lvItem = new ListViewItem();
                            lvItem.SubItems[0].Text = dr1[0].ToString();
                            lvItem.SubItems.Add(dr1[1].ToString());

                            listView1.Items.Add(lvItem);
                        }
                        dr1.Close();
                        con.Desconectar("EX");
                    //}
                    //catch
                    //{
                    //    MessageBox.Show("Error Carga de Rutas");
                    //    con.Desconectar("EX");
                    //}
                
                
                }
        }
    }
}
