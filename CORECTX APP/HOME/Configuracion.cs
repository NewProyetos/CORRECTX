using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Npgsql;

namespace Sinconizacion_EXactus
{
    public partial class Configuracion : Form
    {
        public Configuracion()
        {
            InitializeComponent();
        }
        DataTable DT = XMLRW.Readxml("CONFIGURACION");
        DataTable DTSFTP = XMLRW.Readxml("SFTP");

        String SSQLserver;
        String SSQLserverseg;
        String PGSQLserver;
        String SWeb;
        String SDMdatabase;
        String SDBExactus;
        String SDBWeb;
        String pgdb;
        String SSQLogin;
        String SMyqlogin;
        String pglogin;
        String SSqlID;
        String SSqlIDsg;
        String SMsqlID;
        String pgID;
        String SDBSeguridad;
        String SSQLoginsg;

        String HOST;
        String USER;
        String SFTPKEY;
        String AUTOMATICO;
        String PUERTO;


        private void button1_Click(object sender, EventArgs e)
        {

          

            if (comboBox1.Text == "192.168.1.25 [EXACTUS]")
            {
                try {

                 string cad = "data source="+textBox1.Text+";initial catalog="+textBox4.Text+" ;user Id ='"+textBox2.Text+"' ; password = '"+textBox3.Text+"'";
            SqlConnection con = new SqlConnection(cad);

            try
            {
                con.Open();
                MessageBox.Show("Conexion Exitosa");

                con.Close();
                button2.Enabled = true;
            }
            catch
            {

                MessageBox.Show("Error de Conexion");
                button2.Enabled = false;
            
            }



               
                  
                }
                catch
                {

                    MessageBox.Show("Conexion Fallida");
                    button2.Enabled = false;
                
                }
            }
            else
                if (comboBox1.Text == "mysql.dismonline")
                {

                    MySqlConnection mysqlcon = new MySqlConnection();
                    string myscad = "Server = "+textBox1.Text+"; Database = "+textBox4.Text+"; Uid="+textBox2.Text+"; Pwd ="+textBox3.Text+";";
                    try
                    {
                        mysqlcon.ConnectionString = myscad;
                        mysqlcon.Open();                     
                        MessageBox.Show("Conexion Exitosa");
                        button2.Enabled = true;
                       
                      
                    }
                    catch
                    {

                        MessageBox.Show("Conexion Fallida");
                        button2.Enabled = false;

                    }

                }
                else if (comboBox1.Text == "192.168.1.25 [DM]")
                {

                   

                        string cad = "data source=" + textBox1.Text + ";initial catalog=" + textBox4.Text + " ;user Id ='" + textBox2.Text + "' ; password = '" + textBox3.Text + "'";
                        SqlConnection con = new SqlConnection(cad);

                        try
                        {
                            con.Open();
                            MessageBox.Show("Conexion Exitosa");
                            button2.Enabled = true;
                            con.Close();
                        }
                        catch
                        {

                            MessageBox.Show("Error de Conexion");
                            button2.Enabled = false;

                        }

                    
                }

                else if (comboBox1.Text == "192.168.1.11 [ACCESSCONTROL]")
                {



                    string cad = "data source=" + textBox1.Text + ";initial catalog=" + textBox4.Text + " ;user Id ='" + textBox2.Text + "' ; password = '" + textBox3.Text + "'";
                    SqlConnection con = new SqlConnection(cad);

                    try
                    {
                        con.Open();
                        MessageBox.Show("Conexion Exitosa");
                        button2.Enabled = true;
                        con.Close();
                    }
                    catch
                    {

                        MessageBox.Show("Error de Conexion");
                        button2.Enabled = false;

                    }


                }
            else if (comboBox1.Text == "18.216.38.163 [ODOO]")
            {

                NpgsqlConnection pgcon = new NpgsqlConnection("Server="+textBox1.Text+";User Id="+textBox2.Text+"; " + "Password="+textBox3.Text+";Database="+textBox4.Text+";");

               

                try
                {
                    pgcon.Open();
                    MessageBox.Show("Conexion Exitosa");
                    button2.Enabled = true;
                    pgcon.Close();
                }
                catch
                {

                    MessageBox.Show("Error de Conexion");
                    button2.Enabled = false;

                }


            }
            
        }

        private void Configuracion_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;

            DataRow rowtp = DTSFTP.Rows[0];
            textBox5.Text = Convert.ToString(rowtp["HOST"]);
            textBox6.Text = Convert.ToString(rowtp["USER"]);
            textBox7.Text = Encripter.Desencriptar(Convert.ToString(rowtp["SFTPKEY"]));
            textBox8.Text = Convert.ToString(rowtp["PUERTO"]);
            string automatico = Convert.ToString(rowtp["AUTOMATICO"]);
            if (automatico == "YES")
            {
                checkBox1.Checked = true;
            }
            else
            {
                checkBox1.Checked = false;
            }



        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = false;
              
                    DataRow row = DT.Rows[0];

                    if (comboBox1.Text == "192.168.1.25 [EXACTUS]")
                    {

                        textBox1.Text = Convert.ToString(row["SERVIDORSQL"]);
                        textBox2.Text = Convert.ToString(row["LOGIN"]);
                        textBox4.Text = Convert.ToString(row["DBSQLEX"]);

                    }
                    else
                        if (comboBox1.Text == "mysql.dismonline")
                        {
                            textBox1.Text = Convert.ToString(row["SERVIDORWEB"]);
                            textBox2.Text = Convert.ToString(row["LOGINWEB"]);
                            textBox4.Text = Convert.ToString(row["DBWEB"]);

                        }
                        else if (comboBox1.Text == "192.168.1.25 [DM]")
                        {

                            textBox1.Text = Convert.ToString(row["SERVIDORSQL"]);
                            textBox2.Text = Convert.ToString(row["LOGIN"]);
                            textBox4.Text = Convert.ToString(row["DBSQLDM"]);
                        
                        }
                        else if (comboBox1.Text == "192.168.1.11 [ACCESSCONTROL]")
                        {

                            textBox1.Text = Convert.ToString(row["SERVIDORSQLSEG"]);
                            textBox2.Text = Convert.ToString(row["LOGINSG"]);
                            textBox4.Text = Convert.ToString(row["DBSQLSG"]);

                        }

                        else if (comboBox1.Text == "18.216.38.163 [ODOO]")
                        {

                            textBox1.Text = Convert.ToString(row["SERVIDORPGSQL"]);
                            textBox2.Text = Convert.ToString(row["LOGINPG"]);
                            textBox4.Text = Convert.ToString(row["DBPG"]);

                        }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataRow row = DT.Rows[0];   

            if (comboBox1.Text == "192.168.1.25 [EXACTUS]")
            {
                SSQLserver = textBox1.Text;
                SWeb = Convert.ToString(row["SERVIDORWEB"]);
                SDMdatabase = Convert.ToString(row["DBSQLDM"]); 
                SDBExactus = textBox4.Text;
                SDBWeb = Convert.ToString(row["DBWEB"]);
                SSQLogin = textBox2.Text;
                SMyqlogin = Convert.ToString(row["LOGINWEB"]);                 
                SSqlID = Encripter.Encriptar(textBox3.Text);
                SMsqlID = Convert.ToString(row["KEYIDWEB"]);
                SSQLserverseg = Convert.ToString(row["SERVIDORSQLSEG"]);
                SDBSeguridad = Convert.ToString(row["DBSQLSG"]);
                SSQLoginsg = Convert.ToString(row["LOGINSG"]);
                SSqlIDsg = Convert.ToString(row["KEYIDSG"]);
                PGSQLserver = Convert.ToString(row["SERVIDORPGSQL"]);
                pgdb = Convert.ToString(row["DBPG"]);
                pglogin = Convert.ToString(row["LOGINPG"]);
                pgID = Convert.ToString(row["KEYIDPG"]);

                XMLRW.write("CONFIGURACION", SSQLserver, SWeb, SSQLserverseg,PGSQLserver, SDMdatabase, SDBExactus, SDBSeguridad, pgdb,SDBWeb, SSQLogin, SSQLoginsg, SMyqlogin,pglogin, SSqlID, SSqlIDsg, SMsqlID,pgID,"","","","","");
                
            }
            else
                if (comboBox1.Text == "mysql.dismonline")
                {
                    SSQLserver = Convert.ToString(row["SERVIDORSQL"]);
                    SWeb = textBox1.Text;
                    SDMdatabase = Convert.ToString(row["DBSQLDM"]);
                    SDBExactus = Convert.ToString(row["DBSQLEX"]);
                    SDBWeb = textBox4.Text;
                    SSQLogin = Convert.ToString(row["LOGIN"]);
                    SMyqlogin = textBox2.Text;
                    SSqlID = Convert.ToString(row["KEYID"]); 
                    SMsqlID =  Encripter.Encriptar(textBox3.Text);
                    SSQLserverseg = Convert.ToString(row["SERVIDORSQLSEG"]);
                    SDBSeguridad = Convert.ToString(row["DBSQLSG"]);
                    SSQLoginsg = Convert.ToString(row["LOGINSG"]);
                    SSqlIDsg = Convert.ToString(row["KEYIDSG"]);
                PGSQLserver = Convert.ToString(row["SERVIDORPGSQL"]);
                pgdb = Convert.ToString(row["DBPG"]);
                pglogin = Convert.ToString(row["LOGINPG"]);
                pgID = Convert.ToString(row["KEYIDPG"]);

                XMLRW.write("CONFIGURACION", SSQLserver, SWeb, SSQLserverseg, PGSQLserver, SDMdatabase, SDBExactus, SDBSeguridad, pgdb, SDBWeb, SSQLogin, SSQLoginsg, SMyqlogin, pglogin, SSqlID, SSqlIDsg, SMsqlID, pgID, "", "", "", "", "");

            }
                else if (comboBox1.Text == "192.168.1.25 [DM]")
                {
                    SSQLserver = textBox1.Text;
                    SWeb = Convert.ToString(row["SERVIDORWEB"]);
                    SDMdatabase = textBox4.Text;
                    SDBExactus = Convert.ToString(row["DBSQLEX"]);
                    SDBWeb = Convert.ToString(row["DBWEB"]);
                    SSQLogin = textBox2.Text ;
                    SMyqlogin = Convert.ToString(row["LOGINWEB"]); 
                    SSqlID = Encripter.Encriptar(textBox3.Text);
                    SMsqlID = Convert.ToString(row["KEYIDWEB"]);
                    SSQLserverseg = Convert.ToString(row["SERVIDORSQLSEG"]);
                    SDBSeguridad = Convert.ToString(row["DBSQLSG"]);
                    SSQLoginsg = Convert.ToString(row["LOGINSG"]);
                    SSqlIDsg = Convert.ToString(row["KEYIDSG"]);
                PGSQLserver = Convert.ToString(row["SERVIDORPGSQL"]);
                pgdb = Convert.ToString(row["DBPG"]);
                pglogin = Convert.ToString(row["LOGINPG"]);
                pgID = Convert.ToString(row["KEYIDPG"]);

                XMLRW.write("CONFIGURACION", SSQLserver, SWeb, SSQLserverseg, PGSQLserver, SDMdatabase, SDBExactus, SDBSeguridad, pgdb, SDBWeb, SSQLogin, SSQLoginsg, SMyqlogin, pglogin, SSqlID, SSqlIDsg, SMsqlID, pgID, "", "", "", "", "");

            }
                else if (comboBox1.Text == "192.168.1.11 [ACCESSCONTROL]")
                {
                    SSQLserver = Convert.ToString(row["SERVIDORSQL"]);
                    SSQLserverseg = textBox1.Text;
                    SWeb = Convert.ToString(row["SERVIDORWEB"]);
                    SDMdatabase = Convert.ToString(row["DBSQLDM"]);
                    SDBSeguridad = textBox4.Text;
                    SDBExactus = Convert.ToString(row["DBSQLEX"]);
                    SDBWeb = Convert.ToString(row["DBWEB"]);
                    SSQLoginsg = textBox2.Text;
                    SSQLogin = Convert.ToString(row["LOGIN"]);
                    SMyqlogin = Convert.ToString(row["LOGINWEB"]);
                    SSqlID = Convert.ToString(row["KEYID"]); 
                    SSqlIDsg = Encripter.Encriptar(textBox3.Text);
                    SMsqlID = Convert.ToString(row["KEYIDWEB"]);


                PGSQLserver = Convert.ToString(row["SERVIDORPGSQL"]);
                pgdb = Convert.ToString(row["DBPG"]);
                pglogin = Convert.ToString(row["LOGINPG"]);
                pgID = Convert.ToString(row["KEYIDPG"]);

                XMLRW.write("CONFIGURACION", SSQLserver, SWeb, SSQLserverseg, PGSQLserver, SDMdatabase, SDBExactus, SDBSeguridad, pgdb, SDBWeb, SSQLogin, SSQLoginsg, SMyqlogin, pglogin, SSqlID, SSqlIDsg, SMsqlID, pgID, "", "", "", "", "");

                  }

            else if (comboBox1.Text == "18.216.38.163 [ODOO]")
            {
                SSQLserver = Convert.ToString(row["SERVIDORSQL"]);               
                SWeb = Convert.ToString(row["SERVIDORWEB"]);
                SDMdatabase = Convert.ToString(row["DBSQLDM"]);               
                SDBExactus = Convert.ToString(row["DBSQLEX"]);
                SDBWeb = Convert.ToString(row["DBWEB"]);              
                SSQLogin = Convert.ToString(row["LOGIN"]);
                SMyqlogin = Convert.ToString(row["LOGINWEB"]);
                SSqlID = Convert.ToString(row["KEYID"]);               
                SMsqlID = Convert.ToString(row["KEYIDWEB"]);

                SSQLserverseg = Convert.ToString(row["SERVIDORSQLSEG"]);
                SDBSeguridad = Convert.ToString(row["DBSQLSG"]);
                SSQLoginsg = Convert.ToString(row["LOGINSG"]);
                SSqlIDsg = Convert.ToString(row["KEYIDSG"]);

                PGSQLserver = textBox1.Text;
                pgdb = textBox4.Text;
                pglogin = textBox2.Text;
                pgID = Encripter.Encriptar(textBox3.Text);


                XMLRW.write("CONFIGURACION", SSQLserver, SWeb, SSQLserverseg, PGSQLserver, SDMdatabase, SDBExactus, SDBSeguridad, pgdb, SDBWeb, SSQLogin, SSQLoginsg, SMyqlogin, pglogin, SSqlID, SSqlIDsg, SMsqlID, pgID, "", "", "", "", "");

            }




        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataRow rowtp = DTSFTP.Rows[0];
            

            if (textBox5.Text == string.Empty)
            {
                 HOST = Convert.ToString(rowtp["HOST"]); 
            }
            else
            {
                HOST = textBox5.Text;

            }
            if (textBox6.Text == string.Empty)
            {
                USER = Convert.ToString(rowtp["USER"]);
            }
            else
            {
                USER = textBox6.Text;
            }
            if (textBox7.Text == string.Empty)
            {
               SFTPKEY = Encripter.Encriptar(Convert.ToString(rowtp["SFTPKEY"]));
            }
            else
            {
                SFTPKEY = Encripter.Encriptar(textBox7.Text);
            }

            if (textBox8.Text == string.Empty)
            {
                PUERTO = Convert.ToString(rowtp["PUERTO"]);
            }
            else
            {
                PUERTO = textBox8.Text;
            }
            if (checkBox1.Checked)
            {
                AUTOMATICO = "YES";
            }
            else
            {
                AUTOMATICO = "NO";
            }

            XMLRW.write("SFTP", "", "", "","", "","", "", "", "", "", "", "", "","","","","", HOST, USER, SFTPKEY, PUERTO, AUTOMATICO);
        }
    }
}
