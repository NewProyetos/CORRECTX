using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace Sinconizacion_EXactus
{
    class conexion
    {


        DataTable DT = XMLRW.Readxml("CONFIGURACION");
        //MySqlConnection mysqlcon = new MySqlConnection();
        private string cad = "data source=192.168.1.25;initial catalog=EXACTUS ;user Id =sa ; password = D!sW0Exactus";
        public SqlConnection con;

        private string myscad = "Server = mysql.dismonline.info; Database = dismodb; Uid=admindm; Pwd =dm0nlin314;";
        public MySqlConnection mysqlcon = new MySqlConnection();


        public void conectar()
        {
            con = new SqlConnection(cad);
        
           mysqlcon.ConnectionString = myscad;


        }
        //public SqlCommand comando;
         
        public conexion()
        
        {
        conectar();
    
        }



    }
}
