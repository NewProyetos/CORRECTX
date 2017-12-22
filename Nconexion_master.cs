using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
namespace Sinconizacion_EXactus
{
    class conexion_master
    {
         private string cad = "data source=192.168.1.25;initial catalog=master ;user Id =sa ; password = D!sW0Exactus";
        public SqlConnection con;


        public void conectar()
        {
            con = new SqlConnection(cad);
            

        }
        //public SqlCommand comando;
         
        public conexion_master()
        
        {
        conectar();
    
        }
    }
}
