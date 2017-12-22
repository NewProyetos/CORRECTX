using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace Sinconizacion_EXactus.CORECTX_APP.VENTAS.FACTURACION
{
    public partial class carga_digicel : Form
    {
        public carga_digicel()
        {
            InitializeComponent();
        }
        conexionXML con = new conexionXML();
        DataTable rutas = new DataTable();
        DataTable Encabezados_PED_Ex = new DataTable();
        DataTable Detalle_PED_Ex = new DataTable();
        private void carga_digicel_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
            comboBox1.Enabled = false;
            carga_rutas();
        }

        private void carga_rutas()
        {
            con.conectar("EX");

            SqlCommand cm1 = new SqlCommand("SELECT VENDEDOR,[NOMBRE]  FROM [EXACTUS].[dismo].[VENDEDOR]  where VENDEDOR <> 'ND' and VENDEDOR <> 'CXC' and ACTIVO = 'S' and NOMBRE not like '%INACTIVO%'  order by VENDEDOR", con.conex);
            SqlDataAdapter da1 = new SqlDataAdapter(cm1);
            da1.Fill(rutas);

            con.Desconectar("EX");
            combo(rutas);
        }
        private void combo(DataTable dts1)
        {

            comboBox1.Items.Clear();


            var result = from row in dts1.AsEnumerable()
                         group row by row.Field<string>("VENDEDOR") into grp
                         select new
                         {
                             Vendedor = grp.Key,

                         };
            foreach (var t in result)
            {
                if (t.Vendedor == null || t.Vendedor == "")
                {

                }
                else
                {
                    comboBox1.Items.Add(t.Vendedor);
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                comboBox1.Enabled = true;
            }
            else if (radioButton2.Checked)
            {
                comboBox1.Enabled = false;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                comboBox1.Enabled = true;
            }
            else if (radioButton2.Checked)
            {
                comboBox1.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            con.conectar("DM");
            SqlCommand cmd1 = new SqlCommand("SELECT 'DISMO' as COD_CIA ,enc.[NUM_DOC_PREIMP] as NCF,enc.NUM_DOC_SIS as NUM_PED ,enc.[RUTA] as COD_ZON ,enc.[COD_CLIE] as COD_CLT ,enc.TIPO_DOC ,enc.[HORA_FIN_PEDIDO] as HORA_FIN ,enc.[FECHA_PEDIDO] as FEC_PED ,enc.[FECHA_DESPACHO] as FEC_DES , enc.[HORA_INICIO_PEDIDO] as HORA_INI  ,enc.[MONTO_IMP] as MON_IMP_VT ,0.00 as MON_IMP_CS ,enc.[MONTO_CON_IMP] as MON_CIV ,enc.[MONTO_SIN_IMP] as MON_SIV ,enc.[MONTO_DESC_LINEA] as MON_DSC ,enc.[CANT_ITEM] as NUM_ITM ,enc.[LISTA_PRECIO] as LST_PRE,enc.[ESTADO_PEDIDO] as ESTADO ,enc.CONDICION_CLIENTE as CONDICION_PAGO ,enc.[BODEGA],clie.PAIS ,clie.CLASE_DOCUMENTO ,'ND' as DIR_ENT ,clie.DESCUENTO as DESC1 ,0.0000000 as DESC2 ,enc.[MONTO_DESC_CLIE] as MONT_DESC2 ,'N' as DESCUENTO_CASCADA  ,'S' as IMPRESO	,'N' as CONSIGNACION,enc.[SERIE_DOC] as NCF_PREFIJO ,NULL as FACTURA_UNICA ,pr.NIVEL_PRECIO  ,'L' as MONEDA ,NEWID() as RowPointer ,0 as NoteExistsFlag ,enc.[FECHA_CREA] as RecordDate,enc.[USUARIO_CREA] as CreatedBy ,enc.[USUARIO_CREA] as UpdatedBy,enc.[FECHA_CREA] as CreateDate FROM [DM].[STREET].[ENC_PED_STREET] as enc  INNER JOIN [EXACTUS].[dismo].[CLIENTE] clie  on enc.COD_CLIE = clie.CLIENTE LEFT JOIN [EXACTUS].[ERPADMIN].[NIVEL_LISTA] pr on enc.LISTA_PRECIO = pr.LISTA  where enc.PROCESADO = 'D'", con.condm);
           // cmd1.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            da.Fill(Encabezados_PED_Ex);
            con.Desconectar("DM");
            for (int i = 0; i < Encabezados_PED_Ex.Rows.Count; i++)
            {




                DataRow row = Encabezados_PED_Ex.Rows[i];


                string COD_CIA = Convert.ToString(row["COD_CIA"]);
                string NUM_PED = Convert.ToString(row["NUM_PED"]);
                string COD_ZON = Convert.ToString(row["COD_ZON"]);
                string COD_CLT = Convert.ToString(row["COD_CLT"]);
                string TIPO_DOC = Convert.ToString(row["TIPO_DOC"]);
                DateTime HORA_FIN = Convert.ToDateTime(row["HORA_FIN"]);
                DateTime FEC_PED = Convert.ToDateTime(row["FEC_PED"]);
                DateTime FEC_DES = Convert.ToDateTime(row["FEC_DES"]);
                DateTime HORA_INI = Convert.ToDateTime(row["HORA_INI"]);
                string MON_IMP_VT = Convert.ToString(row["MON_IMP_VT"]);
                string MON_IMP_CS = Convert.ToString(row["MON_IMP_CS"]);
                string MON_CIV = Convert.ToString(row["MON_CIV"]);
                string MON_SIV = Convert.ToString(row["MON_SIV"]);
                string MON_DSC = Convert.ToString(row["MON_DSC"]);
                string NUM_ITM = Convert.ToString(row["NUM_ITM"]);
                string LST_PRE = Convert.ToString(row["LST_PRE"]);
                string ESTADO = Convert.ToString(row["ESTADO"]);

                string CONDICION_PAGO = Convert.ToString(row["CONDICION_PAGO"]);
                string BODEGA = Convert.ToString(row["BODEGA"]);
                string PAIS = Convert.ToString(row["PAIS"]);
                string CLASE_DOCUMENTO = Convert.ToString(row["CLASE_DOCUMENTO"]);
                string DIR_ENT = Convert.ToString(row["DIR_ENT"]);
                string DESC1 = Convert.ToString(row["DESC1"]);
                string DESC2 = Convert.ToString(row["DESC2"]);
                string MONT_DESC2 = Convert.ToString(row["MONT_DESC2"]);
                string DESCUENTO_CASCADA = Convert.ToString(row["DESCUENTO_CASCADA"]);
                string IMPRESO = Convert.ToString(row["IMPRESO"]);
                string CONSIGNACION = Convert.ToString(row["CONSIGNACION"]);
                string NCF_PREFIJO = Convert.ToString(row["NCF_PREFIJO"]);
                string NCF = Convert.ToString(row["NCF"]);
                string NIVEL_PRECIO = Convert.ToString(row["NIVEL_PRECIO"]);

                string MONEDA = Convert.ToString(row["MONEDA"]);

                DateTime RecordDate = Convert.ToDateTime(row["RecordDate"]);
                string CreatedBy = Convert.ToString(row["CreatedBy"]);
                string UpdatedBy = Convert.ToString(row["UpdatedBy"]);
                DateTime CreateDate = Convert.ToDateTime(row["CreateDate"]);



                Detalle_PED_Ex.Clear();

                con.conectar("DM");
                SqlCommand cmd2 = new SqlCommand("SELECT [NUMERO_LINEA],[NUM_DOC_SIS] as NUM_DOC,'DISMO' as COD_CIA ,[COD_ART] ,'0' as ART_BON ,[PRECIO_UNIT] as MON_PRC_MN,[ART_BON],[LINEA_ART_BON],CASE  WHEN MONTO_DESC_ART <> 0.0 THEN ROUND(((MONTO_DESC_ART/(PRECIO_UNIT*CATIDAD))*100 ),1) ELSE MONTO_DESC_ART END as POR_DSC_AP,[SUBTOTAL_LINEA] as MON_TOT ,[MONTO_DESC_ART] as MON_DSC,[PRECIO_UNIT] as MON_PRC_MX , CAST([CATIDAD]as decimal(18,8)) as CNT_MAX,0.00000000 as CNT_MIN ,[LISTA_PRECIO] as LST_PRE,NEWID() as RowPointer ,'0' as NoteExistsFlag,[FECHA_CREA] as RecordDate  ,[USUARIO] as CreatedBy,[USUARIO] as UpdatedBy,[FECHA_CREA] as CreateDate  FROM [DM].[STREET].[DET_PED_STREET]  WHERE [NUM_DOC_SIS]= '" + NUM_PED+"'  and PROCESADO = 'D'", con.condm);
                //cmd2.CommandType = CommandType.StoredProcedure;
                //cmd2.Parameters.AddWithValue("@NUM_DOC", NUM_PED);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                da2.Fill(Detalle_PED_Ex);
                con.Desconectar("DM");





                if (existe_Documento_exactus(NUM_PED))
                {
                }

                else
                {


                    con.conectar("EX");
                    SqlCommand cmd5 = new SqlCommand("[dismo].[STREET_PED]", con.conex);
                    cmd5.CommandType = CommandType.StoredProcedure;

                    cmd5.Parameters.AddWithValue("@COD_CIA", COD_CIA);
                    cmd5.Parameters.AddWithValue("@NUM_PED", NUM_PED);
                    cmd5.Parameters.AddWithValue("@COD_ZON", COD_ZON);
                    cmd5.Parameters.AddWithValue("@COD_CLT", COD_CLT);
                    cmd5.Parameters.AddWithValue("@TIP_DOC", "1");
                    cmd5.Parameters.AddWithValue("@HOR_FIN", HORA_FIN);
                    cmd5.Parameters.AddWithValue("@FEC_PED", FEC_PED);
                    cmd5.Parameters.AddWithValue("@FEC_DES", FEC_DES);
                    cmd5.Parameters.AddWithValue("@HOR_INI", HORA_INI);
                    cmd5.Parameters.AddWithValue("@MON_IMP_VT", MON_IMP_VT);
                    cmd5.Parameters.AddWithValue("@MON_IMP_CS", MON_IMP_CS);
                    cmd5.Parameters.AddWithValue("@MON_CIV", MON_CIV);
                    cmd5.Parameters.AddWithValue("@MON_SIV", MON_SIV);
                    cmd5.Parameters.AddWithValue("@MON_DSC", MONT_DESC2);
                    cmd5.Parameters.AddWithValue("@NUM_ITM", NUM_ITM);
                    cmd5.Parameters.AddWithValue("@LST_PRE", LST_PRE);
                    cmd5.Parameters.AddWithValue("@ESTADO", "N");
                    cmd5.Parameters.AddWithValue("@COD_CND", CONDICION_PAGO);
                    cmd5.Parameters.AddWithValue("@COD_BOD", COD_ZON.Replace("R", "B"));
                    cmd5.Parameters.AddWithValue("@COD_PAIS", PAIS);
                    cmd5.Parameters.AddWithValue("@CLASE", CLASE_DOCUMENTO);
                    cmd5.Parameters.AddWithValue("@DESC1", DESC1);
                    cmd5.Parameters.AddWithValue("@DESC2", DESC2);
                    cmd5.Parameters.AddWithValue("@MONT_DESC1", MON_DSC);
                    cmd5.Parameters.AddWithValue("@MONT_DESC2", MON_DSC);
                    cmd5.Parameters.AddWithValue("@IMPRESO", "N");
                    cmd5.Parameters.AddWithValue("@NCF_PREFIJO", NCF_PREFIJO);
                    cmd5.Parameters.AddWithValue("@NCF", NCF);
                    cmd5.Parameters.AddWithValue("@NIVEL_PRECIO", NIVEL_PRECIO);
                    cmd5.Parameters.AddWithValue("@RecordDate", FEC_PED);
                    cmd5.Parameters.AddWithValue("@CreatedBy", "sa");
                    cmd5.Parameters.AddWithValue("@UpdatedBy", "sa");
                    cmd5.Parameters.AddWithValue("@CreateDate", FEC_PED);


                    cmd5.ExecuteNonQuery();
                    con.Desconectar("EX");

                    for (int j = 0; j < Detalle_PED_Ex.Rows.Count; j++)
                    {
                        DataRow row2 = Detalle_PED_Ex.Rows[j];
                        string ART_BON;


                        string LINEA_NUM = Convert.ToString(row2["NUMERO_LINEA"]);
                        string LINEA_BON = Convert.ToString(row2["LINEA_ART_BON"]);

                        string NUM_DOC = Convert.ToString(row2["NUM_DOC"]);
                        string COD_ART = Convert.ToString(row2["COD_ART"]);

                        string MON_TOT = Convert.ToString(row2["MON_TOT"]);



                        if ((Convert.ToDouble(MON_TOT)) <= 0.00)
                        {
                            ART_BON = "B";
                        }
                        else
                        {
                            ART_BON = Convert.ToString(row2["ART_BON"]);
                        }




                        string MON_PRC_MN = Convert.ToString(row2["MON_PRC_MN"]);
                        string POR_DSC_AP = Convert.ToString(row2["POR_DSC_AP"]);

                        string MON_DSC_DET = Convert.ToString(row2["MON_DSC"]);
                        string MON_PRC_MX = Convert.ToString(row2["MON_PRC_MX"]);
                        string CNT_MAX = Convert.ToString(row2["CNT_MAX"]);
                        string CNT_MIN = Convert.ToString(row2["CNT_MIN"]);
                        string LST_PR_DET = Convert.ToString(row2["LST_PRE"]);

                        string MON_DSCL = Convert.ToString(row2["MON_DSC"]);





                        if (existe_Documento_exactus(NUM_DOC))
                        {
                            con.conectar("EX");
                            SqlCommand cmd6 = new SqlCommand("[dismo].[STREET_DET_PED]", con.conex);
                            cmd6.CommandType = CommandType.StoredProcedure;

                            cmd6.Parameters.AddWithValue("@NUM_LN", LINEA_NUM);
                            cmd6.Parameters.AddWithValue("@NUM_PED", NUM_DOC);
                            cmd6.Parameters.AddWithValue("@COD_CIA", COD_CIA);
                            cmd6.Parameters.AddWithValue("@COD_ART", COD_ART);
                            cmd6.Parameters.AddWithValue("@ART_BON", ART_BON);
                            cmd6.Parameters.AddWithValue("@MON_DSC_MN", MON_PRC_MN);
                            cmd6.Parameters.AddWithValue("@POR_DESC_AP", POR_DSC_AP);
                            cmd6.Parameters.AddWithValue("@MON_TOT", MON_TOT);
                            cmd6.Parameters.AddWithValue("@MON_DSC", MON_DSCL);
                            cmd6.Parameters.AddWithValue("@MON_PRC_MX", MON_PRC_MX);
                            cmd6.Parameters.AddWithValue("@CNT_MAX", CNT_MAX);
                            cmd6.Parameters.AddWithValue("@CNT_MIN", CNT_MIN);
                            cmd6.Parameters.AddWithValue("@COD_ART_RFR", LINEA_BON);
                            cmd6.Parameters.AddWithValue("@LST_PRE", LST_PR_DET);
                            // cmd6.Parameters.AddWithValue("@TOPE", COD_CIA);
                            cmd6.Parameters.AddWithValue("@RecordDate", CreateDate);
                            cmd6.Parameters.AddWithValue("@CreatedBy", CreatedBy);



                            cmd6.ExecuteNonQuery();




                            



                            con.Desconectar("EX");


                            con.conectar("DM");
                            SqlCommand cmd8 = new SqlCommand();
                            cmd8.Connection = con.condm;
                            cmd8.CommandText = "UPDATE [DM].[STREET].[ENC_PED_STREET]SET PROCESADO = 'S' WHERE NUM_DOC_PREIMP = @DOC_PREIM";
                            cmd8.Parameters.Add("@DOC_PREIM", SqlDbType.VarChar).Value = NUM_PED;

                            cmd8.ExecuteNonQuery();

                            con.Desconectar("DM");

                        }
                        else
                        {
                        }


                    }

                }


            }


        }
        private bool existe_Documento_exactus(string factura)
        {
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*)  FROM [EXACTUS].[ERPADMIN].[alFAC_ENC_PED]  WHERE NUM_PED='" + factura + "'", con.conex);
            cmd.Parameters.AddWithValue("NUM_PED", factura);


            int contar = Convert.ToInt32(cmd.ExecuteScalar());
            con.Desconectar("EX");

            if (contar == 0)
            {
                return false;

            }
            else
            {
                return true;

            }
        }

    }
    }
