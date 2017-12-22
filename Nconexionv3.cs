using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Sinconizacion_EXactus
{
  public  class conexionv3
    {
        public static SqlConnection ObtenerConexion()
        {
            try
            {
                SqlConnection cn = new SqlConnection("Data Source=192.168.1.25;Initial Catalog=DM;user Id =sa ; password = D!sW0Exactus");
                cn.Open();
                return cn;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
