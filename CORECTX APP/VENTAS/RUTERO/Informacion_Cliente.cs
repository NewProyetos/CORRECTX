using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace Sinconizacion_EXactus.CORECTX_APP.VENTAS.RUTERO
{
    public partial class Informacion_Cliente : Form
    {
        public Informacion_Cliente()
        {
            InitializeComponent();
        }
        public DataTable zonas = new DataTable();
        DataTable CANAL = new DataTable();
        Int32 detdirect;
        conexionXML con = new conexionXML();
        String empresa = Login.empresa;
        string zonacod;
        String Clase_doc;
        string pais;
        string tipo_contribullente;
        string fecha_proceso;
        DataTable infoclie = new DataTable();
        private void Informacion_Cliente_Load(object sender, EventArgs e)
        {
            textBox1.Text = Rutas.cliente_nom;
            textBox2.Text = Rutas.cliente_nom;
            textBox3.Text = Rutas.cliente_nom;
            Clase_doc = "N";
            textBox2.ForeColor = Color.Gray;
            textBox3.ForeColor = Color.Gray;
            textBox4.ForeColor = Color.Gray;


            comboBox6.Text = Rutas.vendor;

            comboBox2.Text = "UNILEVERDET";
          
            radioButton2.Checked = true;
            tipo_contribullente = "F";
            pais = "FCF";

            if (empresa == "DISMOGT")
            {
                label18.Text = "DPI";
                label4.Text = "Contribuyente:";
                textBox4.Text = "C.F";
                textBox4.ForeColor = Color.Gray;


            }
            else
            {
                label18.Text = "DUI";
                label4.Text = "NIT:";
                textBox4.Text = "0000-000000-000-0";
            }



            con.conectar("EX");

            

            SqlCommand cm4 = new SqlCommand("SELECT dpto.NOMBRE_DPTO,zon.ZONA,zon.[NOMBRE] FROM [EXACTUS].["+empresa+"].[ZONA] as  zon INNER JOIN   [DM].[CORRECT].[DEPARTAMENTOS ELS] as dpto  on LEFT(zon.ZONA,2) = dpto.OTRO where dpto.EMPRESA = '"+empresa+"'", con.conex);
            SqlDataAdapter da4 = new SqlDataAdapter(cm4);
            da4.Fill(zonas);

           


        

            SqlCommand cm3 = new SqlCommand("SELECT [NIVEL_PRECIO] FROM [EXACTUS].["+empresa+"].[NIVEL_PRECIO] where [U_TIPO_USO] = '1'", con.conex);
            SqlDataReader dr1 = cm3.ExecuteReader();
            while (dr1.Read())
            {
                comboBox2.Items.Add(dr1["NIVEL_PRECIO"]);

            }
            dr1.Close();


            SqlCommand cm2 = new SqlCommand("SELECT [VENDEDOR]  FROM [EXACTUS].[" + empresa + "].[VENDEDOR]  where VENDEDOR <> 'ND' and VENDEDOR <> 'CXC' and ACTIVO = 'S' and NOMBRE not like '%INACTIVO%'  order by VENDEDOR", con.conex);
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
                comboBox6.Items.Add(dr2["VENDEDOR"]);

            }
            dr2.Close();


            SqlCommand cm5 = new SqlCommand("SELECT [COBRADOR]  FROM [EXACTUS].[" + empresa + "].[COBRADOR] where COBRADOR  like 'C%'  ", con.conex);
            SqlDataReader dr5 = cm5.ExecuteReader();
            while (dr5.Read())
            {
                comboBox5.Items.Add(dr5["COBRADOR"]);

            }
            dr5.Close();

            SqlCommand cm6 = new SqlCommand("SELECT [RUTA]  FROM [EXACTUS].[" + empresa + "].[RUTA] where  RUTA <> 'CXC' ", con.conex);
            SqlDataReader dr6 = cm6.ExecuteReader();
            while (dr6.Read())
            {
                comboBox4.Items.Add(dr6["RUTA"]);

            }
            dr6.Close();



            SqlCommand cm7 = new SqlCommand("SELECT [DETALLE_DIRECCION] FROM [EXACTUS].["+empresa+"].[CLIENTE] where CLIENTE = '"+Rutas.cliente+"' ", con.conex);
            SqlDataReader dr7 = cm7.ExecuteReader();
            while (dr7.Read())
            {
              detdirect =  Convert.ToInt32(dr7["DETALLE_DIRECCION"]);

            }
            dr7.Close();
            

            con.Desconectar("EX");

            combo(zonas);






            canal_carga();

            cliente_fill(Rutas.cliente);

            cargar_zona_cliente(Rutas.cliente);


        }

        private void cargar_zona_cliente(string cliente)
        {
            con.conectar("EX");

            SqlCommand cm7 = new SqlCommand("SELECT dpto.NOMBRE_DPTO,zon.ZONA,zon.[NOMBRE] FROM [EXACTUS].[dismo].[ZONA] as  zon INNER JOIN   [DM].[CORRECT].[DEPARTAMENTOS ELS] as dpto  on LEFT(zon.ZONA,2) = dpto.OTRO LEFT JOIN [EXACTUS].["+empresa+"].[CLIENTE] as clie on zon.ZONA = clie.ZONA where dpto.EMPRESA = '"+empresa+"'  and clie.CLIENTE = '"+cliente+"'", con.conex);
            SqlDataReader dr7 = cm7.ExecuteReader();
            while (dr7.Read())
            {
                string zonaclie = Convert.ToString(dr7["NOMBRE"]);
                string dpto = Convert.ToString(dr7["NOMBRE_DPTO"]);
                comboBox9.Text = dpto;

                comboBox10.Text = zonaclie;

            }
            dr7.Close();
            con.Desconectar("EX");

            
        }

        private void canal_carga()
        {
            con.conectar("DM");

            SqlCommand cm8 = new SqlCommand("SELECT CANAL.[DESCRIPCION] as CANAL,SUB.[DESCRIPCION] as SUBCANAL FROM [DM].[CORRECT].[CLIENTE_CANAL]  CANAL LEFT JOIN [DM].[CORRECT].[EMPRESAS] as EMP ON  CANAL.EMPRESA_ID = EMP.ID LEFT JOIN [DM].[CORRECT].[CLIENTE_SUBCANAL]as SUB ON  CANAL.ID = SUB.ID_CANAL where EMP.NOMRE = '" + Login.empresa + "'", con.condm);
            SqlDataAdapter da8 = new SqlDataAdapter(cm8);
            da8.Fill(CANAL);

            con.Desconectar("DM");


            combocanal(CANAL);
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            textBox1.ForeColor = Color.Black;
           if(textBox1.Text == "DISPONIBLE")
            {
                textBox1.Text = "";

             }
           
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            textBox2.Text = textBox1.Text;
            textBox3.Text = textBox1.Text;
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            textBox3.Text = textBox2.Text;
        }
        public void combo(DataTable dts1)
        {
            var result3 = from row3 in dts1.AsEnumerable()
                          group row3 by row3.Field<string>("NOMBRE_DPTO") into ln
                          select new
                          {
                              Departamento = ln.Key,

                          };
            foreach (var t1 in result3)
            {
                if (t1.Departamento == null || t1.Departamento == "")
                {

                }
                else
                {
                    comboBox9.Items.Add(t1.Departamento);
                }
            }
        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            zonas.DefaultView.RowFilter = "NOMBRE_DPTO like '" + this.comboBox9.Text + "%'";
            comboBox10.DataSource = zonas;
            comboBox10.DisplayMember = "NOMBRE";

            DataRow dr = zonas.Rows[0];
            string zrnm = Convert.ToString(dr["ZONA"]);
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.SelectionStart = 0;
            textBox1.SelectionLength = textBox1.Text.Length;
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            textBox1.SelectionStart = 0;
            textBox1.SelectionLength = textBox1.Text.Length;
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            textBox4.SelectionStart = 0;
            textBox4.SelectionLength = textBox4.Text.Length;
        }

        private void textBox4_Click(object sender, EventArgs e)
        {
            textBox4.SelectionStart = 0;
            textBox4.SelectionLength = textBox4.Text.Length;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fecha_proceso = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            validar_campos();

           
            
        }

        public void combocanal(DataTable dts1)
        {
            var result3 = from row3 in dts1.AsEnumerable()
                          group row3 by row3.Field<string>("CANAL") into ln
                          select new
                          {
                              canals = ln.Key,

                          };
            foreach (var t1 in result3)
            {
                if (t1.canals == null || t1.canals == "")
                {

                }
                else
                {
                   // comboBox7.Items.Add(t1.canals);
                    comboBox1.Items.Add(t1.canals);

                }
            }
            
        }

       

        private void validar_campos()
        {
            if (textBox1.Text == "DISPONIBLE")
            {
                MessageBox.Show("El Nombre del cliente no puede ser DISPONIBLE");
                textBox1.Focus();
            }
            else if (textBox2.Text == "DISPONIBLE")
            {
                MessageBox.Show("El Alias del cliente no puede ser DISPONIBLE");
                textBox2.Focus();
            }
            else if (textBox3.Text == "DISPONIBLE")
            {
                MessageBox.Show("El Contacto del cliente no puede ser DISPONIBLE");
                textBox3.Focus();

            }
            else if (comboBox4.Text == string.Empty)
            {
                MessageBox.Show("Seleccione una RUTA");
                comboBox4.Focus();

            }

            else if (comboBox5.Text == string.Empty)
            {
                MessageBox.Show("Seleccione un Cobrador");
                comboBox5.Focus();
            }

            else if (textBox7.Text == string.Empty)
            {
                MessageBox.Show("..INGRESE DIRECCON..");
                textBox7.Focus();

            }
            else if (comboBox9.Text == string.Empty)
            {
                MessageBox.Show("Seleccione un Departamento");
                comboBox9.Focus();
            }
            else if (comboBox10.Text == string.Empty)
            {
                MessageBox.Show("Seleccione un Municipio");
                comboBox10.Focus();

            }
           else if (comboBox1.Text == string.Empty)
            {
                MessageBox.Show("Seleccione un Canal");
                comboBox1.Focus();

            }
            else if (comboBox3.Text == string.Empty)
            {
                MessageBox.Show("Seleccione un Sub-Canal");
                comboBox3.Focus();

            }
            if (radioButton1.Checked)
            {
                pais = "CCF";

                if (textBox8.Text == string.Empty)
                {

                    MessageBox.Show("El Cliente es Fiscal  Ingrese el NCR");
                    textBox8.Focus();

                }

                else if (textBox9.Text == string.Empty)
                {
                    MessageBox.Show("El Cliente es Fiscal  Ingrese el GIRO");
                    textBox9.Focus();

                }

                else if (textBox4.Text == string.Empty)
                {
                    MessageBox.Show("El Cliente es Fiscal  Ingrese el Numero de NIT");
                    textBox4.Focus();

                }

                else
                {
                    update_cliente();

                }
            }
            else
            {
                pais = "FCF";
                update_cliente();

            }



        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (textBox5.Text.Contains('-'))
            {
                if (!char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }

                if (e.KeyChar == '\b')
                {
                    e.Handled = false;
                }
            }
            else
            {
                if (!char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }

                if (e.KeyChar == '-' || e.KeyChar == '\b')
                {
                    e.Handled = false;
                }
            }
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textBox8.Text.Contains('-'))
            {
                if (!char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }

                if (e.KeyChar == '\b')
                {
                    e.Handled = false;
                }
            }
            else
            {
                if (!char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }

                if (e.KeyChar == '-' || e.KeyChar == '\b')
                {
                    e.Handled = false;
                }
            }
        }
        private void update_cliente()
        {



            //----------------------- Ingreso de Detalles de Direccion a EXACTUS ---------------------------------
            if (existe_direccion(detdirect))
            {

                con.conectar("EX");
                Guid GuD = Guid.NewGuid();
                SqlCommand cmd7 = new SqlCommand();
                cmd7.Connection = con.conex;
                cmd7.CommandText = "UPDATE [EXACTUS].["+empresa+ "].[DETALLE_DIRECCION] SET [DIRECCION]=@DIRECCION,[CAMPO_1]=@CAMPO_1,[RecordDate]=@RecordDate,[UpdatedBy]=@UpdatedBy where DETALLE_DIRECCION = '"+detdirect+"'";
                
                cmd7.Parameters.Add("@DIRECCION", SqlDbType.VarChar).Value = "ESTANDAR";
                cmd7.Parameters.Add("@CAMPO_1", SqlDbType.VarChar).Value = textBox7.Text +","+comboBox9.Text+","+comboBox10.Text;                
                cmd7.Parameters.Add("@RecordDate", SqlDbType.DateTime).Value = fecha_proceso;                
                cmd7.Parameters.Add("@UpdatedBy", SqlDbType.VarChar).Value = Login.usuario;
                
                            
                cmd7.ExecuteNonQuery();


                con.Desconectar("EX");


            }
            else
            {
                


                con.conectar("EX");
                Guid GuD = Guid.NewGuid();
                SqlCommand cmd3 = new SqlCommand();
                cmd3.Connection = con.conex;
                cmd3.CommandText = "INSERT INTO [EXACTUS].["+empresa+"].[DETALLE_DIRECCION]([DETALLE_DIRECCION],[DIRECCION],[CAMPO_1],[CAMPO_2],[CAMPO_3],[CAMPO_4],[CAMPO_5],[CAMPO_6],[CAMPO_7],[CAMPO_8],[CAMPO_9],[CAMPO_10],[RowPointer],[NoteExistsFlag],[RecordDate],[CreatedBy],[UpdatedBy],[CreateDate])  VALUES(@DETALLE_DIRECCION,@DIRECCION,@CAMPO_1,@CAMPO_2,@CAMPO_3,@CAMPO_4,@CAMPO_5,@CAMPO_6,@CAMPO_7,@CAMPO_8,@CAMPO_9,@CAMPO_10,@RowPointer,@NoteExistsFlag,@RecordDate,@CreatedBy,@UpdatedBy,@CreateDate)";
                cmd3.Parameters.Add("@DETALLE_DIRECCION", SqlDbType.Int).Value = detdirect;
                cmd3.Parameters.Add("@DIRECCION", SqlDbType.VarChar).Value = "ESTANDAR";
                cmd3.Parameters.Add("@CAMPO_1", SqlDbType.VarChar).Value = textBox7.Text + "," + comboBox9.Text + "," + comboBox10.Text;
                cmd3.Parameters.Add("@CAMPO_2", SqlDbType.VarChar).Value = "";
                cmd3.Parameters.Add("@CAMPO_3", SqlDbType.VarChar).Value = "";
                cmd3.Parameters.Add("@CAMPO_4", SqlDbType.VarChar).Value = "";
                cmd3.Parameters.Add("@CAMPO_5", SqlDbType.VarChar).Value = "";
                cmd3.Parameters.Add("@CAMPO_6", SqlDbType.VarChar).Value = "";
                cmd3.Parameters.Add("@CAMPO_7", SqlDbType.VarChar).Value = "";
                cmd3.Parameters.Add("@CAMPO_8", SqlDbType.VarChar).Value = "";
                cmd3.Parameters.Add("@CAMPO_9", SqlDbType.VarChar).Value = "";
                cmd3.Parameters.Add("@CAMPO_10", SqlDbType.NVarChar).Value = "";
                cmd3.Parameters.Add("@RowPointer", SqlDbType.UniqueIdentifier).Value = GuD;
                cmd3.Parameters.Add("@NoteExistsFlag", SqlDbType.TinyInt).Value = 0;
                cmd3.Parameters.Add("@RecordDate", SqlDbType.DateTime).Value = fecha_proceso;
                cmd3.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = Login.usuario;
                cmd3.Parameters.Add("@UpdatedBy", SqlDbType.VarChar).Value = Login.usuario;
                cmd3.Parameters.Add("@CreateDate", SqlDbType.DateTime).Value = fecha_proceso;




                cmd3.ExecuteNonQuery();


                con.Desconectar("EX");
            }



            

            
            
                //----------------------- Ingreso de Detalles de NIT a EXACTUS ---------------------------------
                if (existe_NIT(textBox4.Text))
                {

                }
                else
                {
                    con.conectar("EX");
                    Guid GuD1 = Guid.NewGuid();
                    SqlCommand cmd4 = new SqlCommand();
                    cmd4.Connection = con.conex;
                    cmd4.CommandText = "INSERT INTO [EXACTUS].["+empresa+"].[NIT]([NIT],[RAZON_SOCIAL],[ALIAS],[NOTAS],[TIPO],[DIGITO_VERIFICADOR],[RowPointer],[NoteExistsFlag],[RecordDate],[CreatedBy],[UpdatedBy],[CreateDate])  VALUES(@NIT,@RAZON_SOCIAL,@ALIAS,@NOTAS,@TIPO,@DIGITO_VERIFICADOR,@RowPointer,@NoteExistsFlag,@RecordDate,@CreatedBy,@UpdatedBy,@CreateDate)";
                    cmd4.Parameters.Add("@NIT", SqlDbType.VarChar).Value = textBox4.Text;
                    cmd4.Parameters.Add("@RAZON_SOCIAL", SqlDbType.VarChar).Value = textBox1.Text;
                    cmd4.Parameters.Add("@ALIAS", SqlDbType.VarChar).Value = textBox2.Text;
                    cmd4.Parameters.Add("@NOTAS", SqlDbType.VarChar).Value = "";
                    cmd4.Parameters.Add("@TIPO", SqlDbType.VarChar).Value = "ND";
                    cmd4.Parameters.Add("@DIGITO_VERIFICADOR", SqlDbType.VarChar).Value = "";
                    cmd4.Parameters.Add("@RowPointer", SqlDbType.UniqueIdentifier).Value = GuD1;
                    cmd4.Parameters.Add("@NoteExistsFlag", SqlDbType.TinyInt).Value = 0;
                    cmd4.Parameters.Add("@RecordDate", SqlDbType.DateTime).Value = fecha_proceso;
                    cmd4.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = Login.usuario;
                    cmd4.Parameters.Add("@UpdatedBy", SqlDbType.VarChar).Value = Login.usuario;
                    cmd4.Parameters.Add("@CreateDate", SqlDbType.DateTime).Value = fecha_proceso;

                    cmd4.ExecuteNonQuery();
                
                    con.Desconectar("EX");

                

            }


            /// ---------- ingreso de clientes -------------------------
            /// 
            string contacto = textBox3.Text;
            
            if (contacto.Length > 29)
            {
               contacto =  contacto.Substring(0, 29);
            }

            else
            {

            }
            con.conectar("DM");
            SqlCommand cmd8 = new SqlCommand();
            cmd8.Connection = con.condm;
            cmd8.CommandText = "UPDATE  [EXACTUS].["+empresa+ "].[CLIENTE] SET [NOMBRE]=@NOMBRE,[ALIAS]=@ALIAS,[CONTACTO] = @CONTACTO,[DIRECCION]=@DIRECCION,[TELEFONO1]=@TELEFONO1,[TELEFONO2] = @TELEFONO2,[CONTRIBUYENTE]=@CONTRIBUYENTE,[TIPO_CONTRIBUYENTE]=@TIPO_CONTRIBUYENTE,[NIVEL_PRECIO] = @NIVEL_PRECIO,[ZONA]= @ZONA,[RUTA]=@RUTA,[VENDEDOR]=@VENDEDOR,[COBRADOR]= @COBRADOR,[RUBRO1_CLI]=@RUBRO1_CLI,[RUBRO2_CLI]=@RUBRO2_CLI,[RUBRO3_CLI]=@RUBRO3_CLI,[RUBRO4_CLI]=@RUBRO4_CLI,[RUBRO5_CLI]=@RUBRO5_CLI,[CLASE_DOCUMENTO]= @CLASE_DOCUMENTO , [USUARIO_ULT_MOD]=@USUARIO_ULT_MOD,RecordDate = @RecordDate,PAIS = @PAIS  WHERE CLIENTE = '" + Rutas.cliente+"'";
            cmd8.Parameters.Add("@NOMBRE", SqlDbType.VarChar).Value = textBox1.Text;
            cmd8.Parameters.Add("@ALIAS", SqlDbType.VarChar).Value = textBox2.Text;
            cmd8.Parameters.Add("@CONTACTO", SqlDbType.VarChar).Value = contacto;
            cmd8.Parameters.Add("@DIRECCION", SqlDbType.VarChar).Value = textBox7.Text + "," + comboBox9.Text + "," + comboBox10.Text;
            cmd8.Parameters.Add("@TELEFONO1", SqlDbType.VarChar).Value = textBox5.Text;
            cmd8.Parameters.Add("@TELEFONO2", SqlDbType.VarChar).Value = textBox5.Text;
            cmd8.Parameters.Add("@CONTRIBUYENTE", SqlDbType.VarChar).Value = textBox4.Text;
            cmd8.Parameters.Add("@TIPO_CONTRIBUYENTE", SqlDbType.VarChar).Value = tipo_contribullente;
            cmd8.Parameters.Add("@CLASE_DOCUMENTO", SqlDbType.VarChar).Value = Clase_doc;
            cmd8.Parameters.Add("@NIVEL_PRECIO", SqlDbType.VarChar).Value = comboBox2.Text;
            cmd8.Parameters.Add("@ZONA", SqlDbType.VarChar).Value = zonacod;    
            cmd8.Parameters.Add("@RUTA", SqlDbType.VarChar).Value = comboBox4.Text;
            cmd8.Parameters.Add("@VENDEDOR", SqlDbType.VarChar).Value = comboBox6.Text;
            cmd8.Parameters.Add("@COBRADOR", SqlDbType.VarChar).Value = comboBox5.Text;
            cmd8.Parameters.Add("@RUBRO1_CLI", SqlDbType.VarChar).Value = textBox8.Text;
            cmd8.Parameters.Add("@RUBRO2_CLI", SqlDbType.VarChar).Value = textBox9.Text;
            cmd8.Parameters.Add("@RUBRO3_CLI", SqlDbType.VarChar).Value = comboBox1.Text;
            cmd8.Parameters.Add("@RUBRO4_CLI", SqlDbType.VarChar).Value = comboBox3.Text;
            cmd8.Parameters.Add("@RUBRO5_CLI", SqlDbType.VarChar).Value = textBox10.Text;
            cmd8.Parameters.Add("@USUARIO_ULT_MOD", SqlDbType.VarChar).Value = Login.usuario.ToUpper();
            cmd8.Parameters.Add("@RecordDate", SqlDbType.VarChar).Value = fecha_proceso;
            cmd8.Parameters.Add("@PAIS", SqlDbType.VarChar).Value = pais;



            cmd8.ExecuteNonQuery();

            con.Desconectar("DM");

            this.Close();
            
            ////---------------------------------------------------------
        }

        private bool existe_direccion(int codigo_direccion)
        {
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("SELECT COUNT (*)  FROM [EXACTUS].["+Login.empresa+"].[DETALLE_DIRECCION]  where DETALLE_DIRECCION = '" + codigo_direccion + "'", con.conex);
            cmd.Parameters.AddWithValue("DETALLE_DIRECCIONP", codigo_direccion);


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

        private bool existe_NIT(string NIT)
        {
            con.conectar("EX");
            SqlCommand cmd = new SqlCommand("SELECT COUNT (*)  FROM [EXACTUS].["+Login.empresa+"].[NIT] where NIT = '" + NIT + "'", con.conex);
            cmd.Parameters.AddWithValue("NIT", NIT);


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

        private void comboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            con.conectar("EX");
            SqlCommand cm7 = new SqlCommand("SELECT [ZONA] FROM [EXACTUS].["+empresa+"].[ZONA] where NOMBRE = '"+comboBox10.Text+"' ", con.conex);
            SqlDataReader dr7 = cm7.ExecuteReader();
            while (dr7.Read())
            {
                zonacod = Convert.ToString(dr7["ZONA"]);

            }
            dr7.Close();

            con.Desconectar("EX");

        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {



            if (textBox4.Text.Split('-').Length - 1 > 2)
            {

                if (!char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }

                if (e.KeyChar == '\b')
                {
                    e.Handled = false;
                }

            }

            else
            {
                if (!char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }

                if (e.KeyChar == '-' || e.KeyChar == '\b')
                {
                    e.Handled = false;
                }
            }

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                Clase_doc = "C";
                tipo_contribullente = "O";
               

            }
            else if(radioButton2.Checked)
            {
                Clase_doc = "N";
                tipo_contribullente = "F";
              

            }
        }

        private void cliente_fill(string Cliente)
        {
            con.conectar("EX");
            SqlCommand cm1 = new SqlCommand("SELECT CLIE.[NOMBRE],CLIE.[ALIAS],CLIE.[CONTACTO],CLIE.[DIRECCION],CLIE.[TELEFONO1],CLIE.[TELEFONO2],CLIE.[CONTRIBUYENTE],CLIE.[TIPO_CONTRIBUYENTE],CLIE.[NIVEL_PRECIO],ZON.NOMBRE as city,DEP.NOMBRE_DPTO, CLIE.[RUTA],CLIE.[VENDEDOR],CLIE.[COBRADOR],CLIE.[RUBRO1_CLI],CLIE.[RUBRO2_CLI],CLIE.[RUBRO3_CLI],CLIE.[RUBRO4_CLI],CLIE.[RUBRO5_CLI],CLIE.[CLASE_DOCUMENTO] FROM [EXACTUS].[" + empresa+"].[CLIENTE] as CLIE LEFT JOIN [EXACTUS].["+empresa+"].[ZONA] as ZON  on CLIE.ZONA = ZON.ZONA LEFT JOIN [DM].[CORRECT].[DEPARTAMENTOS ELS] as DEP  on LEFT(ZON.ZONA,2) = DEP.OTRO and dep.EMPRESA = '"+empresa+"' where CLIENTE = '" + Cliente + "'", con.conex);
            SqlDataAdapter da1 = new SqlDataAdapter(cm1);
            da1.Fill(infoclie);
            con.Desconectar("EX");

            if (infoclie.Rows.Count > 0)
            {
                

                DataRow row = infoclie.Rows[0];
                textBox1.Text = Convert.ToString(row["NOMBRE"]);
                textBox2.Text = Convert.ToString(row["ALIAS"]);
                textBox3.Text = Convert.ToString(row["CONTACTO"]);
                textBox7.Text = Convert.ToString(row["DIRECCION"]);
                textBox5.Text = Convert.ToString(row["TELEFONO1"]);
                textBox6.Text = Convert.ToString(row["TELEFONO2"]);
                textBox4.Text = Convert.ToString(row["CONTRIBUYENTE"]);
                comboBox2.Text = Convert.ToString(row["NIVEL_PRECIO"]);
                string city;
                city = Convert.ToString(row["city"]);
                comboBox10.Text = city;
                comboBox9.Text = Convert.ToString(row["NOMBRE_DPTO"]);
                comboBox6.Text = Convert.ToString(row["VENDEDOR"]);
                comboBox5.Text = Convert.ToString(row["COBRADOR"]);
                comboBox4.Text = Convert.ToString(row["RUTA"]);
                textBox8.Text = Convert.ToString(row["RUBRO1_CLI"]);
                textBox9.Text = Convert.ToString(row["RUBRO2_CLI"]);
                string canal;
                canal = Convert.ToString(row["RUBRO3_CLI"]);
                this.comboBox1.Text = canal;
                this.comboBox3.Text = Convert.ToString(row["RUBRO4_CLI"]);
                textBox10.Text = Convert.ToString(row["RUBRO5_CLI"]);

                tipo_contribullente = Convert.ToString(row["TIPO_CONTRIBUYENTE"]);

                if (tipo_contribullente == "O")
                {
                    radioButton1.Checked = true;
                }
                else
                {
                    radioButton2.Checked = true;
                }
                //textBox2.Text = Convert.ToString(row["CLASE_DOCUMENTO"]);


                //  comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;


            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CANAL.DefaultView.RowFilter = "CANAL like '" + this.comboBox1.Text + "%'";
            comboBox3.DataSource = CANAL;
            comboBox3.DisplayMember = "SUBCANAL";
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                Clase_doc = "C";
                tipo_contribullente = "O";
               
            }
            else if (radioButton2.Checked)
            {
                Clase_doc = "N";
                tipo_contribullente = "F";
               
            }
        }
    }
}
