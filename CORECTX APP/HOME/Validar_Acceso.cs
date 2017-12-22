using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
namespace Sinconizacion_EXactus
{
   
    
    class Validar_Acceso
    
    {
       public DataTable Menu_Acces = new DataTable();
       public DataTable APP_acces= new DataTable();
       public DataTable SUBAPP_acces= new DataTable();
      
      conexionXML con = new conexionXML();

        private void consulta(string Usuario, int id_empresa)
        {

            Menu_Acces.Clear();
            APP_acces.Clear();
            SUBAPP_acces.Clear();

            con.conectar("DM");
            SqlCommand cm = new SqlCommand("SELECT A.ACCES,C.NOMBRE,A.MENU_ID FROM [DM].[CORRECT].[ACCES_MAIN] A  INNER JOIN [DM].[CORRECT].[USUARIOS] B  ON A.ID_USER = B.USER_ID    INNER JOIN [DM].[CORRECT].[MENU]C  on A.MENU_ID = C.MENU_ID  WHERE B.USUARIO = '" + Usuario + "' and ID_EMPRESA = '"+id_empresa+"' ", con.condm);
            SqlDataAdapter da = new SqlDataAdapter(cm);
            da.Fill(Menu_Acces);


            SqlCommand cm1 = new SqlCommand("SELECT A.[ACCCESS],C.NOMBRE,A.APP_ID FROM [DM].[CORRECT].[ACCESO_APP] A INNER JOIN [DM].[CORRECT].[USUARIOS] B ON A.ID_USER = B.USER_ID  INNER JOIN [DM].[CORRECT].[MENU_APP]C on A.APP_ID = C.APP_ID  WHERE B.USUARIO = '" + Usuario + "' and ID_EMPRESA = '" + id_empresa + "' ", con.condm);
            SqlDataAdapter da1 = new SqlDataAdapter(cm1);
            da1.Fill(APP_acces);


            SqlCommand cm2 = new SqlCommand("SELECT A.SUBAPP_ID,A.ACCESS,C.NOMBRE FROM [DM].[CORRECT].[ACCESO_SUBAPP] A INNER JOIN [DM].[CORRECT].[USUARIOS] B ON A.ID_USER = B.USER_ID    INNER JOIN [DM].[CORRECT].[MENU_SUB_APP]C on A.SUBAPP_ID = C.SUBAPP_ID  WHERE B.USUARIO ='" + Usuario + "' and ID_EMPRESA = '" + id_empresa + "' ", con.condm);
            SqlDataAdapter da2 = new SqlDataAdapter(cm2);
            da2.Fill(SUBAPP_acces);
            con.Desconectar("DM");



        }

        public void Valida_accion(string nombre , int id_empresa)
        { 
        
        consulta(nombre,id_empresa);
        }
    }
}
