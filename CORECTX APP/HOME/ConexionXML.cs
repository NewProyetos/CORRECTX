﻿ using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using Npgsql;

namespace Sinconizacion_EXactus
{
    class conexionXML
    {


        //DataTable DT = Login.DTconexion;
        DataTable DT = XMLRW.Readxml("CONFIGURACION");
        
        public MySqlConnection mysqlconec = new MySqlConnection();
        public SqlConnection conex;
        public SqlConnection condm;
        public SqlConnection conmas;
        public SqlConnection conseg;
        public NpgsqlConnection pgcon;
        String Sqlserver;
        String Sqlserverseg;
        String Mysqlserver;
        String UserSQL;
        String UserSQLseg;
        String UserMysql;
        String DBEXACTUS;
        String DBSEGURIDAD;
        String DBWEB;
        String DBDM;
        String SQLPass;
        String SQLPSEG;
        String MYsqlPass;
        String PGSQLserver;
        String pgdb;
        String pglogin;
        String pgID;



        public void conectar(string Database)
        {
            credeciales();
            try
            {
                string cadex = "data source=" + Sqlserver + ";initial catalog=" + DBEXACTUS + " ;user Id =" + UserSQL + " ; password = " + SQLPass + "";
                conex = new SqlConnection(cadex);

                string cadm = "data source=" + Sqlserver + ";initial catalog=" + DBDM + " ;user Id =" + UserSQL + " ; password = " + SQLPass + "";
                condm = new SqlConnection(cadm);

                string myscad = "Server = " + Mysqlserver + "; Database = " + DBWEB + "; Uid=" + UserMysql + "; Pwd =" + MYsqlPass + ";";
                mysqlconec.ConnectionString = myscad;

                string cadmas = "data source=" + Sqlserver + ";initial catalog=master ;user Id =" + UserSQL + " ; password = " + SQLPass + "";
                conmas = new SqlConnection(cadmas);

                string cadseg = "data source=" + Sqlserverseg + ";initial catalog="+DBSEGURIDAD+" ;user Id =" + UserSQLseg + " ; password = " + SQLPSEG + "";
                conseg = new SqlConnection(cadseg);

                string pgcad = "Server=" + PGSQLserver + ";User Id=" + pglogin + "; " + "Password=" + pgID + ";Database=" + pgdb + ";";
                 pgcon = new NpgsqlConnection(pgcad);




                if (Database == "EX")
                {

                    conex.Open();

                }
                else if (Database == "DM")
                {
                    condm.Open();


                }
                else if (Database == "WEB")
                {

                    mysqlconec.Open();
                }

                else if (Database == "MAS")
                {

                    conmas.Open();
                }
                else if (Database == "SEG")
                {

                    conseg.Open();
                }

                else if (Database == "ODOO")
                {

                    pgcon.Open();
                }
            }
            catch (Exception error_e)
            {

                MessageBox.Show(error_e.ToString(), "Error de Conecxion");
                
            
            }
        }
        public void Desconectar(string Database)
        {

            if (Database == "EX")
            {

                conex.Close();

            }
            else if (Database == "DM")
            {
                condm.Close();


            }
            else if (Database == "WEB")
            {

                mysqlconec.Close();
            }


            else if (Database == "MAS")
            {

                conmas.Close();
            }
            else if (Database == "SEG")
            {

                conseg.Close();
            }
            else if (Database == "ODOO")
            {

                pgcon.Close();
            }
        }
              
      

        public void credeciales()
        {

            DataRow row = DT.Rows[0];
            
            Sqlserver = Convert.ToString(row["SERVIDORSQL"]);
            PGSQLserver = Convert.ToString(row["SERVIDORPGSQL"]);
            Sqlserverseg = Convert.ToString(row["SERVIDORSQLSEG"]);
            UserSQL = Convert.ToString(row["LOGIN"]);
            UserSQLseg = Convert.ToString(row["LOGINSG"]);
            pglogin = Convert.ToString(row["LOGINPG"]);
            DBEXACTUS = Convert.ToString(row["DBSQLEX"]);
            pgdb = Convert.ToString(row["DBPG"]);
            Mysqlserver = Convert.ToString(row["SERVIDORWEB"]);
            UserMysql = Convert.ToString(row["LOGINWEB"]);
            DBWEB = Convert.ToString(row["DBWEB"]);
            DBDM = Convert.ToString(row["DBSQLDM"]);
            DBSEGURIDAD = Convert.ToString(row["DBSQLSG"]);

            
            SQLPass = Encripter.Desencriptar(Convert.ToString(row["KEYID"]));
            MYsqlPass = Encripter.Desencriptar(Convert.ToString(row["KEYIDWEB"]));
            SQLPSEG = Encripter.Desencriptar(Convert.ToString(row["KEYIDSG"]));
            pgID = Encripter.Desencriptar(Convert.ToString(row["KEYIDPG"]));

        }




    }
}
