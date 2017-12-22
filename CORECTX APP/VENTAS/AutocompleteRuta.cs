using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms;


namespace Sinconizacion_EXactus
{
    class AutocompleteRuta
    {


        public static class AutocompleteRutas
        {


            public static DataTable RUTAS()
            {
                DataTable dt = new DataTable();

                Conexion2 coned = new Conexion2();
                coned.con.Open();

                string consulta = "SELECT [RUTA] FROM [DM].[CORRECT].[RUTA_REGALIAS] ORDER BY RUTA";
                SqlCommand comando = new SqlCommand(consulta, coned.con);

                SqlDataAdapter adap = new SqlDataAdapter(comando);

                adap.Fill(dt);
                coned.con.Close();
                return dt;


            }

            public static AutoCompleteStringCollection AutocompleteRT()
            {
                DataTable dt = RUTAS();

                AutoCompleteStringCollection coleccion = new AutoCompleteStringCollection();
                //recorrer y cargar los items para el autocompletado
                foreach (DataRow row in dt.Rows)
                {
                    coleccion.Add(Convert.ToString(row["RUTA"]));
                }

                return coleccion;
            }


            public static DataTable PRODUCTO()
            {
                DataTable dt1 = new DataTable();

                conexion coned = new conexion();
                coned.con.Open();

                string consulta = "SELECT [ARTICULO],[DESCRIPCION]FROM [EXACTUS].[dismo].[ARTICULO]";
                SqlCommand comando = new SqlCommand(consulta, coned.con);

                SqlDataAdapter adap = new SqlDataAdapter(comando);

                adap.Fill(dt1);
                coned.con.Close();
                return dt1;


            }

            public static AutoCompleteStringCollection AutocompletePRART()
            {
                DataTable dt1 = PRODUCTO();

                AutoCompleteStringCollection Codigo = new AutoCompleteStringCollection();

                //recorrer y cargar los items para el autocompletado
                foreach (DataRow row in dt1.Rows)
                {
                    Codigo.Add(Convert.ToString(row["ARTICULO"]));

                }

                return Codigo;

            }
            public static AutoCompleteStringCollection AutocompletePRDESC()
            {
                DataTable dt1 = PRODUCTO();

                AutoCompleteStringCollection Desc = new AutoCompleteStringCollection();
                //recorrer y cargar los items para el autocompletado
                foreach (DataRow row in dt1.Rows)
                {

                    Desc.Add(Convert.ToString(row["DESCRIPCION"]));
                }

                return Desc;
            }

            public static DataTable PLACAS()
            {
                DataTable dt2 = new DataTable();

                Conexion2 coned = new Conexion2();
                coned.con.Open();

                string consulta = "SELECT PLACA FROM [DM].[CORRECT].[VEHICULOS] ORDER BY PLACA";
                SqlCommand comando = new SqlCommand(consulta, coned.con);

                SqlDataAdapter adap = new SqlDataAdapter(comando);

                adap.Fill(dt2);
                coned.con.Close();
                return dt2;

            }

            public static AutoCompleteStringCollection AutocompletePLACA()
            {
                DataTable dt2 = PLACAS();

                AutoCompleteStringCollection coleccion = new AutoCompleteStringCollection();
                //recorrer y cargar los items para el autocompletado
                foreach (DataRow row in dt2.Rows)
                {
                    coleccion.Add(Convert.ToString(row["PLACA"]));
                }

                return coleccion;
            }


            public static DataTable FORMA_PAGO()
            {
                DataTable dt2 = new DataTable();

                Conexion2 coned = new Conexion2();
                coned.con.Open();

                string consulta = "SELECT TIPO FROM [DM].[CORRECT].[FORMA_PAGO_GAS] ORDER BY TIPO";
                SqlCommand comando = new SqlCommand(consulta, coned.con);

                SqlDataAdapter adap = new SqlDataAdapter(comando);

                adap.Fill(dt2);
                coned.con.Close();
                return dt2;

            }

            public static AutoCompleteStringCollection AutocompleteFPAGO()
            {
                DataTable dt2 = FORMA_PAGO();

                AutoCompleteStringCollection coleccion = new AutoCompleteStringCollection();
                //recorrer y cargar los items para el autocompletado
                foreach (DataRow row in dt2.Rows)
                {
                    coleccion.Add(Convert.ToString(row["TIPO"]));
                }

                return coleccion;
            }





            public static DataTable REGISTRO()
            {
                DataTable dt2 = new DataTable();

                Conexion2 coned = new Conexion2();
                coned.con.Open();

                string consulta = "SELECT Registro FROM [DM].[CORRECT].[Proveedores] ORDER BY Registro";
                SqlCommand comando = new SqlCommand(consulta, coned.con);

                SqlDataAdapter adap = new SqlDataAdapter(comando);

                adap.Fill(dt2);
                coned.con.Close();
                return dt2;

            }

            public static AutoCompleteStringCollection AutocompleteREGISTRO()
            {
                DataTable dt2 = REGISTRO();

                AutoCompleteStringCollection coleccion = new AutoCompleteStringCollection();
                //recorrer y cargar los items para el autocompletado
                foreach (DataRow row in dt2.Rows)
                {
                    coleccion.Add(Convert.ToString(row["Registro"]));
                }

                return coleccion;
            }

            public static DataTable ACTIVIDADES()
            {
                DataTable dt5 = new DataTable();

                Conexion2 coned = new Conexion2();
                coned.con.Open();

                string consulta = "SELECT NOMBRE FROM [DM].[CORRECT].[ACTIVIDADES_BAT] order by ID desc";
                SqlCommand comando = new SqlCommand(consulta, coned.con);

                SqlDataAdapter adap = new SqlDataAdapter(comando);

                adap.Fill(dt5);
                coned.con.Close();
                return dt5;

            }
            public static AutoCompleteStringCollection AutocompleteACTIVIDAD()
            {
                DataTable dt5 = ACTIVIDADES();

                AutoCompleteStringCollection coleccion = new AutoCompleteStringCollection();
                //recorrer y cargar los items para el autocompletado
                foreach (DataRow row in dt5.Rows)
                {
                    coleccion.Add(Convert.ToString(row["NOMBRE"]));
                }

                return coleccion;

            }
            public static DataTable CUENTA_BAT()
            {
                DataTable dt5 = new DataTable();

                Conexion2 coned = new Conexion2();
                coned.con.Open();

                string consulta = "SELECT CUENTA FROM [DM].[CORRECT].[ACTIVIDADES_BAT] order by ID desc";
                SqlCommand comando = new SqlCommand(consulta, coned.con);

                SqlDataAdapter adap = new SqlDataAdapter(comando);

                adap.Fill(dt5);
                coned.con.Close();
                return dt5;

            }
            public static AutoCompleteStringCollection AutocompleteCUENTA_BAT()
            {
                DataTable dt5 = CUENTA_BAT();

                AutoCompleteStringCollection coleccion = new AutoCompleteStringCollection();
                //recorrer y cargar los items para el autocompletado
                foreach (DataRow row in dt5.Rows)
                {
                    coleccion.Add(Convert.ToString(row["CUENTA"]));
                }

                return coleccion;

            }
        }

        public static DataTable EMPLEADOS_CODIGO()
        {

            if (Main_Menu.TIPO_RRHH == "ADMIN")
            {

                DataTable dt2 = new DataTable();

                Conexion2 coned = new Conexion2();
                coned.con.Open();

                string consulta = "SELECT EJ.[SUBORDINADO] FROM [EXACTUS].[dismo].[EMPLEADO_JERARQUIA] as  EJ INNER JOIN  [EXACTUS].[dismo].[EMPLEADO] as EM on EJ.SUBORDINADO = EM.EMPLEADO  INNER JOIN [DM].[CORRECT].[USUARIOS] as DMUSER  ON EJ.SUPERIOR = DMUSER.COD_EMPLEADO ";
                SqlCommand comando = new SqlCommand(consulta, coned.con);

                SqlDataAdapter adap = new SqlDataAdapter(comando);

                adap.Fill(dt2);
                coned.con.Close();
                return dt2;
            
            
            }
            else
            {


                DataTable dt2 = new DataTable();

                Conexion2 coned = new Conexion2();
                coned.con.Open();

                string consulta = "SELECT EJ.[SUBORDINADO] FROM [EXACTUS].[dismo].[EMPLEADO_JERARQUIA] as  EJ INNER JOIN  [EXACTUS].[dismo].[EMPLEADO] as EM on EJ.SUBORDINADO = EM.EMPLEADO  INNER JOIN [DM].[CORRECT].[USUARIOS] as DMUSER  ON EJ.SUPERIOR = DMUSER.COD_EMPLEADO  where DMUSER.COD_EMPLEADO ='" + Main_Menu.COD_EMPLEADO + "'";
                SqlCommand comando = new SqlCommand(consulta, coned.con);

                SqlDataAdapter adap = new SqlDataAdapter(comando);

                adap.Fill(dt2);

                if (Main_Menu.autoev == "S")
                {
                    if (dt2.Rows.Count > 0)
                    {
                        DataRow Row = dt2.NewRow();
                        Row["SUBORDINADO"] = Main_Menu.COD_EMPLEADO;
                        dt2.Rows.Add(Row);
                    }
                }
                coned.con.Close();
                return dt2;
            }
        
        }

        public static AutoCompleteStringCollection AutocompleteEMPLEADOCOD()
        {
            DataTable dt5 = EMPLEADOS_CODIGO();

            AutoCompleteStringCollection coleccion = new AutoCompleteStringCollection();
            //recorrer y cargar los items para el autocompletado
            foreach (DataRow row in dt5.Rows)
            {
                coleccion.Add(Convert.ToString(row["SUBORDINADO"]));
            }

            return coleccion;

        
        }


        public static DataTable EMPLEADO_NOMBRE()
        {

            if (Main_Menu.TIPO_RRHH == "ADMIN")
            {
                DataTable dt2 = new DataTable();

                Conexion2 coned = new Conexion2();
                coned.con.Open();

                string consulta = "SELECT EM.NOMBRE  FROM [EXACTUS].[dismo].[EMPLEADO_JERARQUIA] as  EJ  INNER JOIN  [EXACTUS].[dismo].[EMPLEADO] as EM  on EJ.SUBORDINADO = EM.EMPLEADO  INNER JOIN [DM].[CORRECT].[USUARIOS] as DMUSER  ON EJ.SUPERIOR = DMUSER.COD_EMPLEADO ";
                SqlCommand comando = new SqlCommand(consulta, coned.con);

                SqlDataAdapter adap = new SqlDataAdapter(comando);

                adap.Fill(dt2);
                coned.con.Close();
                return dt2;

            }
            else
            {

                DataTable dt2 = new DataTable();

                Conexion2 coned = new Conexion2();
                coned.con.Open();

                string consulta = "SELECT EM.NOMBRE  FROM [EXACTUS].[dismo].[EMPLEADO_JERARQUIA] as  EJ  INNER JOIN  [EXACTUS].[dismo].[EMPLEADO] as EM  on EJ.SUBORDINADO = EM.EMPLEADO  INNER JOIN [DM].[CORRECT].[USUARIOS] as DMUSER  ON EJ.SUPERIOR = DMUSER.COD_EMPLEADO  where DMUSER.COD_EMPLEADO = '" + Main_Menu.COD_EMPLEADO + "'";
                SqlCommand comando = new SqlCommand(consulta, coned.con);

                SqlDataAdapter adap = new SqlDataAdapter(comando);

                adap.Fill(dt2);
                coned.con.Close();
                return dt2;
            }
        }
        public static AutoCompleteStringCollection AutocompleteEMPLEADONOMBRE()
        {
            DataTable dt5 = EMPLEADO_NOMBRE();

            AutoCompleteStringCollection coleccion = new AutoCompleteStringCollection();
            //recorrer y cargar los items para el autocompletado
            foreach (DataRow row in dt5.Rows)
            {
                coleccion.Add(Convert.ToString(row["NOMBRE"]));
            }

            return coleccion;

        }


        public static DataTable EMPRESAS()
        {
            DataTable dt9 = new DataTable();

            Conexion2 coned = new Conexion2();
            coned.con.Open();

            string consulta = "SELECT NOMRE FROM [DM].[CORRECT].[EMPRESAS] order by ID desc";
            SqlCommand comando = new SqlCommand(consulta, coned.con);

            SqlDataAdapter adap = new SqlDataAdapter(comando);

            adap.Fill(dt9);
            coned.con.Close();
            return dt9;

        }
        public static AutoCompleteStringCollection AutocompleteEMPRESAS()
        {
            DataTable dt9 = EMPRESAS();
            int cuandto = dt9.Rows.Count;
            AutoCompleteStringCollection coleccion = new AutoCompleteStringCollection();
            //recorrer y cargar los items para el autocompletado
            foreach (DataRow row in dt9.Rows)
            {
                coleccion.Add(Convert.ToString(row["NOMBRE"]));
            }

            return coleccion;

        }

        public static DataTable EMPLEADOS_ACCESO()
        {
            DataTable dt9 = new DataTable();

            conexionXML con = new conexionXML();
            con.conectar("SEG");

            string consulta = "SELECT UPPER(RTRIM(LTRIM(ISNULL(FIRSTNAME,''))))+' '+ UPPER(RTRIM(LTRIM(isnull(MIDNAME,''))))+' '+UPPER(RTRIM(LTRIM(ISNULL(LASTNAME,''))))  AS NOMBRE,  CARDT.ID as CARNET  FROM [ACCESSCONTROL].[dbo].[EMP] EMPLE LEFT JOIN [ACCESSCONTROL].[dbo].[BADGE] CARDT  on EMPLE.ID = CARDT.EMPID	   where CARDT.STATUS = '1'";
            SqlCommand comando = new SqlCommand(consulta, con.conseg);

            SqlDataAdapter adap = new SqlDataAdapter(comando);

            adap.Fill(dt9);
            con.Desconectar("SEG");
            return dt9;

        }
        public static AutoCompleteStringCollection AutocompleteEMPLEADO_ACCESO()
        {
            DataTable dt9 = EMPLEADOS_ACCESO();
            int cuandto = dt9.Rows.Count;
            AutoCompleteStringCollection colec_Nombre = new AutoCompleteStringCollection();
           

            //recorrer y cargar los items para el autocompletado
            foreach (DataRow row in dt9.Rows)
            {
                colec_Nombre.Add(Convert.ToString(row["NOMBRE"]));
               
            }

            return colec_Nombre;
            

        }
        public static AutoCompleteStringCollection AutocompleteCARNET_ACCESO()
        {
            DataTable dt9 = EMPLEADOS_ACCESO();
            int cuandto = dt9.Rows.Count;
           
            AutoCompleteStringCollection colec_Carnet = new AutoCompleteStringCollection();

            //recorrer y cargar los items para el autocompletado
            foreach (DataRow row in dt9.Rows)
            {
             
                colec_Carnet.Add(Convert.ToString(row["CARNET"]));
            }

            
            return colec_Carnet;

        }

    }
}
