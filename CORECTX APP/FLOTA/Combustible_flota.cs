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
    public partial class Combustible_flota : Form
    {
        public Combustible_flota()
        {
            InitializeComponent();
        }
        //Conexion2 coned = new Conexion2();
        conexionXML con = new conexionXML();
        Int32 tipo_proc = 0;
        String fecha_actual;
        Int32 PLACA;
        Double KILOMETRAJE;
        Int32 FACTURA;
        Double TOTAL;
        Double GALONES;
        String PROVEEDOR;
        String REGISTRO;
        String AGENCIA;
        String FORMA_PAGO;
        String FECHA;
        String ESTATUS;
        Int32 Año;
        Int32 mes;
        Double IVA;
        Double FOVIAL;
        Double COTRANS;
        Double TOTAL_IMPUESTO;
        Double COMPRA_GRAVADA;
        Double COMPRA_NETA;
        Double COSTO_GALON;
        Int32 consulta ;
        Int32 valida;
        Double ultimoKLM;
        Int32 Canfac;
        Int32 ID;
        string USUARIO;

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            import_fac_flota impf = new import_fac_flota();
            impf.Show();
        }

        private void Combustible_flota_Load(object sender, EventArgs e)
        {
            ID = 0;
            consulta = 0;
            tipo_proc = 0;


            //USUARIO = Login.usuario.ToUpper();

           
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy/MM/dd";

            label16.Text = "0.00";
            label17.Text = "0.00";
            label18.Text = "0.00";
            label19.Text = "0.00";
            label20.Text = "0.00";
            label21.Text = "0.00";
            textBox6.Text = "0.00";
            //comboBox1.Items.Clear();
            //comboBox2.Items.Clear();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            
          
                                  
            textBox6.Text = "";
           
            comboBox4.Text = "";
            label27.Text = "";
            
           
            //comboBox3.Items.Clear();
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            textBox1.Enabled = false;
            textBox3.Enabled = false;
           
            textBox6.Enabled = false;
            toolStripButton3.Enabled = false;
            toolStripButton4.Enabled = false;
            toolStripButton2.Enabled = false;
            button1.Enabled = true;
            comboBox3.Enabled = false;
            comboBox4.Enabled = false;
          
             

                comboBox1.DataSource = AutocompleteRuta.AutocompleteRutas.PLACAS();
                comboBox1.DisplayMember = "PLACA";
                comboBox1.ValueMember = "PLACA";
               
                comboBox1.AutoCompleteCustomSource = AutocompleteRuta.AutocompleteRutas.AutocompletePLACA();
                comboBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
                comboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
                comboBox1.Text = "";
                comboBox2.DataSource = AutocompleteRuta.AutocompleteRutas.FORMA_PAGO();
                comboBox2.DisplayMember = "TIPO";
                comboBox2.ValueMember = "TIPO";

               
                comboBox2.AutoCompleteCustomSource = AutocompleteRuta.AutocompleteRutas.AutocompleteFPAGO();
                comboBox2.AutoCompleteMode = AutoCompleteMode.Suggest;
                comboBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;

                comboBox2.Text = "";

                comboBox3.DataSource = AutocompleteRuta.AutocompleteRutas.REGISTRO();
                comboBox3.DisplayMember = "Registro";
                comboBox3.ValueMember = "Registro";
                
                comboBox3.AutoCompleteCustomSource = AutocompleteRuta.AutocompleteRutas.AutocompleteREGISTRO();
                comboBox3.AutoCompleteMode = AutoCompleteMode.Suggest;
                comboBox3.AutoCompleteSource = AutoCompleteSource.CustomSource;


                comboBox3.Text = "";
                label24.Text = "";
                label28.Text = "";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (consulta == 0)
            {
                
                con.conectar("DM");
                SqlCommand cm1 = new SqlCommand("SELECT[CECO] FROM [DM].[CORRECT].[VEHICULOS] where PLACA ='" + comboBox1.Text + "' ", con.condm);
                SqlDataReader dr1 = cm1.ExecuteReader();
                while (dr1.Read())
                {
                    label28.Text = Convert.ToString(dr1["CECO"]);

                }
                dr1.Close();



                
               con.Desconectar("DM");
            }
        


        }

        private void button1_Click(object sender, EventArgs e)
        {
            consulta = 1;
            if (textBox2.Text == "")
            {
                MessageBox.Show("DIGITE EL NUMERO DE FACTURA");
            }

            else if (Exists_fac(this.textBox2.Text))
            {
                if (Canfac < 2)
                {
                    string ag;
                    string st;
                    con.conectar("DM");
                    SqlCommand cm1 = new SqlCommand("SELECT [ID],[PLACA],[KILOMETRAJE],[FACTURA],[TOTAL],[GALONES],[PROVEEDOR],[REGISTRO],[AGENCIA],[FORMA_PAGO],[FECHA],[IVA],[FOVIAL],[COTRANS],[COMPRA_GRAVADA],[COMPRA_NETA],[COSTO_GALON],[ESTATUS] FROM [DM].[CORRECT].[FAC_FLOTA]  where FACTURA = '" + textBox2.Text + "'", con.condm);
                    SqlDataReader dr1 = cm1.ExecuteReader();

                    while (dr1.Read())
                    {
                        ID = Convert.ToInt32(dr1["ID"]);
                        comboBox1.Text = Convert.ToString(dr1["PLACA"]);
                        comboBox2.Text = Convert.ToString(dr1["FORMA_PAGO"]);
                        textBox1.Text = Convert.ToString(dr1["KILOMETRAJE"]);
                        textBox3.Text = Convert.ToString(dr1["GALONES"]);
                        comboBox3.Text = Convert.ToString(dr1["REGISTRO"]);
                        textBox6.Text = Convert.ToString(dr1["TOTAL"]);
                        label19.Text = Convert.ToString(dr1["COMPRA_GRAVADA"]);
                        label20.Text = Convert.ToString(dr1["COMPRA_NETA"]);
                        label21.Text = Convert.ToString(dr1["COSTO_GALON"]);
                        label16.Text = Convert.ToString(dr1["COTRANS"]);
                        label17.Text = Convert.ToString(dr1["FOVIAL"]);
                        label18.Text = Convert.ToString(dr1["IVA"]);
                        label24.Text = Convert.ToString(dr1["PROVEEDOR"]);
                        dateTimePicker1.Value = Convert.ToDateTime(dr1["FECHA"]);
                        ag = Convert.ToString(dr1["AGENCIA"]);
                        st = Convert.ToString(dr1["ESTATUS"]);
                        switch (ag)
                        {
                            case "SS":
                                comboBox4.Text = "SAN SALVADOR";
                                break;
                            case "SA":
                                comboBox4.Text = "SANTA ANA";
                                break;
                            case "SM":
                                comboBox4.Text = "SAN MIEGUEL";
                                break;
                            default:
                                comboBox4.Text = ag;
                                break;

                        }

                        switch (st)
                        {
                            case "A":
                                label27.Text = "ACTIVA";
                                break;
                            case "N":
                                label27.Text = "ANULADA";
                                label27.ForeColor = Color.Red;
                                break;
                            default:
                                label27.Text = "NO ESTATUS";
                                break;
                        }



                    }
                    dr1.Close();



                    SqlCommand cm2 = new SqlCommand("SELECT[CECO] FROM [DM].[CORRECT].[VEHICULOS] where PLACA ='" + comboBox1.Text + "' ", con.condm);
                    SqlDataReader dr2 = cm2.ExecuteReader();
                    while (dr2.Read())
                    {
                        label28.Text = Convert.ToString(dr2["CECO"]);

                    }
                    dr2.Close();


                    SqlCommand cm3 = new SqlCommand("SELECT [Nombre] FROM [DM].[CORRECT].[Proveedores] where Registro ='" + comboBox3.Text + "' ", con.condm);
                    SqlDataReader dr3 = cm3.ExecuteReader();
                    while (dr3.Read())
                    {
                        label24.Text = Convert.ToString(dr3["Nombre"]);

                    }
                    dr3.Close();


                    toolStripButton3.Enabled = true;
                    toolStripButton4.Enabled = true;
                    toolStripButton2.Enabled = true;







                   con.Desconectar("DM");
                }
                else
                {
                    MessageBox.Show("Existen 2 Documentos con el numero :"+FACTURA+"");
                }


            }
            else
            {
                MessageBox.Show("No Existe Factura");
                
            }

        }
        private bool Exists_fac(string factura)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("select COUNT (*) from [DM].[CORRECT].[FAC_FLOTA] where FACTURA = @Factura ", con.condm);
            cmd.Parameters.AddWithValue("Factura",factura);

            int contar = Convert.ToInt32(cmd.ExecuteScalar());
           con.Desconectar("DM");

            if (contar == 0)
            {
                return false;

            }
            else
            {
                return true;
                Canfac = contar;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (tipo_proc == 0)
            {
                if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back) && (e.KeyChar != (char)Keys.Enter))
                {
                    MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    e.Handled = true;
                    return;
                }
                else if (e.KeyChar == Convert.ToChar(Keys.Enter))
                {

                    button1_Click(null, null);
                }

                 
               }


            else 
            {
                if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back) && (e.KeyChar != (char)Keys.Enter))
                {
                    MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    e.Handled = true;
                    return;
                }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            tipo_proc = 2;
            button1.Enabled = false;
            comboBox1.Enabled = true;
            comboBox2.Enabled = false;
            textBox1.Enabled = true;
            textBox3.Enabled = true;
            comboBox3.Enabled = true;
            comboBox4.Enabled = true;
            comboBox2.Enabled = true;
            textBox6.Enabled = true;

            toolStripButton4.Enabled = true;
            toolStripButton3.Enabled = true;

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            tipo_proc = 1;
            button1.Enabled = false;
            comboBox1.Text = "";
            comboBox2.Text = "";
            comboBox3.Text = "";
            comboBox4.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
         
            textBox6.Text = "";
            label16.Text = "0.00";
            label17.Text = "0.00";
            label18.Text = "0.00";
            label19.Text = "0.00";
            label20.Text = "0.00";
            label21.Text = "0.00";
            textBox6.Text = "0.00";
            label24.Text = "";
            comboBox4.Text = "";
            label27.Text = "ACTIVA";
            

            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            textBox1.Enabled = true;
            textBox3.Enabled = true;
            comboBox3.Enabled = true;
            comboBox4.Enabled = true;
          
            textBox6.Enabled = true;
           
            toolStripButton4.Enabled = true;


        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            Combustible_flota_Load(null, null);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            fecha_actual = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            mes = Convert.ToInt32(DateTime.Now.ToString("MM"));
            Año = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            FECHA = dateTimePicker1.Value.ToString("yyyy/MM/dd");

           
            
            if (tipo_proc == 1)
            {
                ESTATUS = "A";


                validacion_info();

                if (valida == 1)
                {





                    if (existe_factura(FACTURA, REGISTRO))
                    {
                        MessageBox.Show("FACTURA YA EXISTE");
                    }
                    else
                    {

                        con.conectar("DM");
                        SqlCommand cm1 = new SqlCommand("select TOP 1 (KILOMETRAJE) from [DM].[CORRECT].[FAC_FLOTA] where PLACA ='" + PLACA + "' AND (DATEADD(dd, 0, DATEDIFF(dd, 0, FECHA)) <= '" + fecha_actual + " ' AND ESTATUS = 'A')", con.condm);
                                SqlDataReader dr1 = cm1.ExecuteReader();
                                while (dr1.Read())
                                {
                                    ultimoKLM = Convert.ToInt32(dr1["KILOMETRAJE"]);

                                }
                                dr1.Close();

                        

                        if (KILOMETRAJE > ultimoKLM)
                        {
                            string agen = "";

                            SqlCommand cmd1 = new SqlCommand("[CORRECT].[INSERT_FAC_FLOTA]", con.condm);
                            cmd1.CommandType = CommandType.StoredProcedure;

                            cmd1.Parameters.AddWithValue("@PLACA", PLACA);
                            cmd1.Parameters.AddWithValue("@KILOMETRAJE", KILOMETRAJE);
                            cmd1.Parameters.AddWithValue("@FACTURA", FACTURA);
                            cmd1.Parameters.AddWithValue("@TOTAL", TOTAL);
                            cmd1.Parameters.AddWithValue("@GALONES", GALONES);
                            cmd1.Parameters.AddWithValue("@PROVEEDOR", PROVEEDOR);
                            cmd1.Parameters.AddWithValue("@REGISTRO", REGISTRO);

                            switch (AGENCIA)
                            {
                                case "SAN SALVADOR":
                                    agen = "SS";
                                    break;
                                case "SAN MIGUEL":
                                    agen = "SM";
                                    break;
                                case "SANTA ANA":
                                    agen = "SA";
                                    break;
                             }

                             cmd1.Parameters.AddWithValue("@AGENCIA", agen);
                            cmd1.Parameters.AddWithValue("@FORMA_PAGO", FORMA_PAGO);
                            cmd1.Parameters.AddWithValue("@FECHA",Convert.ToDateTime(FECHA));
                            cmd1.Parameters.AddWithValue("@MES", mes);
                            cmd1.Parameters.AddWithValue("@AÑO", Año);
                            cmd1.Parameters.AddWithValue("@IVA", IVA);
                            cmd1.Parameters.AddWithValue("@FOVIAL", FOVIAL);
                            cmd1.Parameters.AddWithValue("@COTRANS", COTRANS);
                            cmd1.Parameters.AddWithValue("@TOTAL_IMPUESTO", TOTAL_IMPUESTO);
                            cmd1.Parameters.AddWithValue("@COMPRA_GRAVADA", COMPRA_GRAVADA);
                            cmd1.Parameters.AddWithValue("@COMPRA_NETA", COMPRA_NETA);
                            cmd1.Parameters.AddWithValue("@COSTO_GALON", COSTO_GALON);
                            cmd1.Parameters.AddWithValue("@fecha_crea",Convert.ToDateTime(fecha_actual));
                            cmd1.Parameters.AddWithValue("@USUARIO", USUARIO);
                            cmd1.Parameters.AddWithValue("@ESTATUS", ESTATUS);


                            cmd1.ExecuteNonQuery();
                            MessageBox.Show("FACTURA N0. " + FACTURA + " INGRESADA");
                           con.Desconectar("DM");
                            Combustible_flota_Load(null, null);
                            
                        }
                        else
                        {
                            MessageBox.Show( "KILOMETRAJE ANTERIOR " + ultimoKLM + " ES MAYOR QUE EL  KILOMETRAJE INGRESADO : " + KILOMETRAJE + "");
                           con.Desconectar("DM");
                        }
                        
                    }

                }



            
            }
            else if (tipo_proc == 2)
            {

                 MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("DESEA ACTUALIZAR LA  FACTURA : " +FACTURA+ " ", "FACTURACION DE COMBUSTIBLES", bt1, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                ESTATUS = "A";
                validacion_info();
                Calcular();

                con.conectar("DM");
                SqlCommand cm1 = new SqlCommand("select TOP 1 (KILOMETRAJE) from [DM].[CORRECT].[FAC_FLOTA] where PLACA ='" + PLACA + "' AND (DATEADD(dd, 0, DATEDIFF(dd, 0, FECHA)) <= '" + fecha_actual + " ' AND ESTATUS = 'A')", con.condm);
                SqlDataReader dr1 = cm1.ExecuteReader();
                while (dr1.Read())
                {
                    ultimoKLM = Convert.ToInt32(dr1["KILOMETRAJE"]);

                }
                dr1.Close();



                if (KILOMETRAJE >= ultimoKLM)
                {
                    string agen = "";

                    SqlCommand cmd1 = new SqlCommand("[CORRECT].[UPDATE_FAC_FLOTA]", con.condm);
                    cmd1.CommandType = CommandType.StoredProcedure;

                    cmd1.Parameters.AddWithValue("@ID", ID);
                    cmd1.Parameters.AddWithValue("@PLACA", PLACA);
                    cmd1.Parameters.AddWithValue("@KILOMETRAJE", KILOMETRAJE);
                    cmd1.Parameters.AddWithValue("@FACTURA", FACTURA);
                    cmd1.Parameters.AddWithValue("@TOTAL", TOTAL);
                    cmd1.Parameters.AddWithValue("@GALONES", GALONES);
                    cmd1.Parameters.AddWithValue("@PROVEEDOR", PROVEEDOR);
                    cmd1.Parameters.AddWithValue("@REGISTRO", REGISTRO);

                    switch (AGENCIA)
                    {
                        case "SAN SALVADOR":
                            agen = "SS";
                            break;
                        case "SAN MIGUEL":
                            agen = "SM";
                            break;
                        case "SANTA ANA":
                            agen = "SA";
                            break;
                    }

                    cmd1.Parameters.AddWithValue("@AGENCIA", agen);
                    cmd1.Parameters.AddWithValue("@FORMA_PAGO", FORMA_PAGO);
                    cmd1.Parameters.AddWithValue("@FECHA",Convert.ToDateTime( FECHA));
                    cmd1.Parameters.AddWithValue("@MES", mes);
                    cmd1.Parameters.AddWithValue("@AÑO", Año);
                    cmd1.Parameters.AddWithValue("@IVA", IVA);
                    cmd1.Parameters.AddWithValue("@FOVIAL", FOVIAL);
                    cmd1.Parameters.AddWithValue("@COTRANS", COTRANS);
                    cmd1.Parameters.AddWithValue("@TOTAL_IMPUESTO", TOTAL_IMPUESTO);
                    cmd1.Parameters.AddWithValue("@COMPRA_GRAVADA", COMPRA_GRAVADA);
                    cmd1.Parameters.AddWithValue("@COMPRA_NETA", COMPRA_NETA);
                    cmd1.Parameters.AddWithValue("@COSTO_GALON", COSTO_GALON);
                    cmd1.Parameters.AddWithValue("@fecha_crea", Convert.ToDateTime( fecha_actual));
                    cmd1.Parameters.AddWithValue("@USUARIO", USUARIO);
                    cmd1.Parameters.AddWithValue("@ESTATUS", ESTATUS);


                    cmd1.ExecuteNonQuery();
                    MessageBox.Show("FACTURA N0. " + FACTURA + " ACTUALIZADA");
                   con.Desconectar("DM");
                    Combustible_flota_Load(null, null);

                }
                else
                {
                    MessageBox.Show("KILOMETRAJE ANTERIOR " + ultimoKLM + " ES MAYOR QUE EL  KILOMETRAJE INGRESADO : " + KILOMETRAJE + "");
                   con.Desconectar("DM");
                }



            }
            }
        }

        private void validacion_info()
        {
            if (comboBox1.Text == "" || comboBox1.Text == null)
            {
                
                MessageBox.Show("Seleccione Numero de Placa");
                comboBox1.Focus();
                valida = 0;

            }
            else
            {
                PLACA = Convert.ToInt32(comboBox1.Text);
                valida = 1;
                if (textBox1.Text == "" || textBox1.Text == null)
                {
                    MessageBox.Show("Ingrese el Kilometraje");
                    textBox1.Focus();

                    valida = 0;
                }
                else
                {
                    valida = 1;
                    KILOMETRAJE = Convert.ToDouble(textBox1.Text);

                    if (textBox2.Text == "" || textBox2.Text == null)
                    {
                        valida = 0;
                        MessageBox.Show("Ingrese el Numero de Factura");
                        textBox2.Focus();
                    }
                    else
                    {
                        valida = 1;
                        FACTURA = Convert.ToInt32(textBox2.Text);

                        if (textBox3.Text == "" || textBox3.Text == null)
                        {
                            valida = 0;
                            MessageBox.Show("Ingrese los Galones");
                            textBox3.Focus();
                        }
                        else
                        {
                            valida = 1;
                            GALONES = Convert.ToDouble(textBox3.Text);

                            if (textBox6.Text == "" || textBox6.Text == null || textBox6.Text == "0.00")
                            {
                                valida = 0;
                                MessageBox.Show("Ingrese el total de la Factura");
                                textBox6.Focus();
                            }
                            else
                            {
                                valida = 1;
                                TOTAL = Convert.ToDouble(textBox6.Text);

                                if (comboBox3.Text == "" || comboBox3.Text == null)
                                {
                                    valida = 0;
                                    MessageBox.Show("Seleccione el Numero de Registro del Cliente");
                                    comboBox3.Focus();
                                }
                                else
                                {
                                    valida = 1;
                                    REGISTRO = comboBox3.Text;
                                    PROVEEDOR = label24.Text;
                                    if (comboBox2.Text == "" || comboBox2.Text == null)
                                    {
                                        valida = 0;
                                        MessageBox.Show("Seleccione Forma de Pago");
                                        comboBox2.Focus();
                                    }
                                    else
                                    {
                                        valida = 1;
                                        FORMA_PAGO = comboBox2.Text;


                                        if (comboBox4.Text == "" || comboBox4.Text == null)
                                        {
                                            valida = 0;
                                            MessageBox.Show("Seleccione Agencia");
                                            comboBox4.Focus();
                                        }
                                        else
                                        {
                                            valida = 1;
                                            AGENCIA = comboBox4.Text;

                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
           
         
         

          


           




        
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (consulta == 0)
            {
                con.conectar("DM");
                SqlCommand cm2 = new SqlCommand("SELECT [Nombre] FROM [DM].[CORRECT].[Proveedores] where Registro ='" + comboBox3.Text + "' ", con.condm);
                SqlDataReader dr1 = cm2.ExecuteReader();
                while (dr1.Read())
                {
                    label24.Text = Convert.ToString(dr1["Nombre"]);

                }
                dr1.Close();




               con.Desconectar("DM");
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back) && (e.KeyChar != (char)Keys.Enter) && (Convert.ToString(e.KeyChar) != "."))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
            
            
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back) && (e.KeyChar != (char)Keys.Enter) && (Convert.ToString(e.KeyChar) != "."))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }



                if (e.KeyChar == Convert.ToChar(Keys.Enter)&& Convert.ToDouble(textBox6.Text)>0)
                {
                    TOTAL = Convert.ToDouble(textBox6.Text);
                    Calcular();

                    label15.Text = Convert.ToString(COMPRA_GRAVADA);
                    label16.Text = Convert.ToString(COMPRA_NETA);
                    label17.Text = Convert.ToString(COSTO_GALON);
                    label18.Text = Convert.ToString(COTRANS);
                    label19.Text = Convert.ToString(FOVIAL);
                    label20.Text = Convert.ToString(IVA);


                    
                }


        }

        private void Calcular()
        {

            if (textBox3.Text == "")
            {
                
                textBox3.Focus();
                FOVIAL = 0.0;
                COTRANS = 0.0;
                TOTAL_IMPUESTO = 0.0;
                IVA = 0.0;
                COMPRA_GRAVADA = 0.0;
                COMPRA_NETA = 0.0;
                COSTO_GALON = 0.0;
                
                MessageBox.Show("INGRESE CANTIDAD DE GALONES");
            }
            else if (Convert.ToDouble(textBox3.Text) > 0)
            {
                GALONES = Convert.ToDouble(textBox3.Text);
                FOVIAL = Math.Round(GALONES * 0.2, 2);
                COTRANS = Math.Round(GALONES * 0.1, 2);
                TOTAL_IMPUESTO = Math.Round(FOVIAL + COTRANS,2);

                if (Convert.ToDouble(textBox6.Text) > 0)
                {

                    IVA = Math.Round((TOTAL - TOTAL_IMPUESTO) - (TOTAL - TOTAL_IMPUESTO) / 1.13, 2);
                    COMPRA_GRAVADA = Math.Round(TOTAL - TOTAL_IMPUESTO - IVA,2);
                    COMPRA_NETA = Math.Round(COMPRA_GRAVADA + TOTAL_IMPUESTO,2);
                    COSTO_GALON = Math.Round(COMPRA_NETA / GALONES, 2);
                }
            }
            else if (Convert.ToDouble(textBox3.Text) <= 0)
            {
                textBox3.Focus();
                FOVIAL = 0.0;
                COTRANS = 0.0;
                TOTAL_IMPUESTO = 0.0;
                IVA = 0.0;
                COMPRA_GRAVADA = 0.0;
                COMPRA_NETA = 0.0;
                COSTO_GALON = 0.0;

                MessageBox.Show("CANTIDAD DE GALONES NO PUEDE SER = 0");
            
            }
        
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back) && (e.KeyChar != (char)Keys.Enter) && (Convert.ToString(e.KeyChar) != "."))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private bool existe_factura(int factura,string registro)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("select COUNT (*) from [DM].[CORRECT].[FAC_FLOTA] where FACTURA ='"+factura+"' and REGISTRO = '"+registro+"'and ESTATUS ='A'", con.condm);
            cmd.Parameters.AddWithValue("factura", Convert.ToInt32(factura));
            cmd.Parameters.AddWithValue("registro", registro);

            int contar = Convert.ToInt32(cmd.ExecuteScalar());
           con.Desconectar("DM");

            if (contar == 0)
            {
                return false;

            }
            else
            {
                return true;
               
            }

        
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            MessageBoxButtons bt1 = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("SE ELIMINARA FACTURA: " + textBox2.Text+ " ", "FACTURACION DE COMBUSTIBLES", bt1, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                fecha_actual = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                if (textBox2.Text == "" || textBox2.Text == null || comboBox3.Text == "")
                {
                    MessageBox.Show("NO SE ENCONTRO NUMERO DE FACTURA");
                }
                else
                {
                    if (label27.Text == "ACTIVA")
                    {

                        if (existe_factura(Convert.ToInt32(textBox2.Text), comboBox3.Text))
                        {
                            con.conectar("DM");

                            SqlCommand cmd = new SqlCommand("DELETE [DM].[CORRECT].[FAC_FLOTA]  WHERE ID = '" + ID + "'", con.condm);

                            cmd.ExecuteNonQuery();
                           con.Desconectar("DM");

                            MessageBox.Show("EL DOCUMENTO FUE ELIMINADO");
                            Combustible_flota_Load(null, null);

                           
                        }
                    }
                }
            }
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            { }
            else
            {
                if (existe_Placa(Convert.ToInt32(comboBox1.Text)))
                {

                    toolStripButton4.Enabled = true;
                }

                else
                {
                    MessageBox.Show("NUMERO DE PLACA NO EXISTE");
                    comboBox1.Focus();
                    toolStripButton4.Enabled = false;
                }
            }
        }

        private bool existe_Placa(int placa)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("select COUNT (*) from [DM].[CORRECT].[VEHICULOS] where PLACA ='" + placa + "'", con.condm);
            cmd.Parameters.Add("@ID", SqlDbType.Binary);
            cmd.Parameters.AddWithValue("placa", Convert.ToInt32(placa));
          

            int contar = Convert.ToInt32(cmd.ExecuteScalar());
           con.Desconectar("DM");

            if (contar == 0)
            {
                return false;

            }
            else
            {
                return true;

            }


        }

        private bool existe_registro(string Registro)
        {
            con.conectar("DM");
            SqlCommand cmd = new SqlCommand("select COUNT (*) from [DM].[CORRECT].[Proveedores] where Registro ='" + Registro + "'", con.condm);
            cmd.Parameters.AddWithValue("Registro", Registro);


            int contar = Convert.ToInt32(cmd.ExecuteScalar());
           con.Desconectar("DM");

            if (contar == 0)
            {
                return false;

            }
            else
            {
                return true;

            }


        }

        private void comboBox3_Leave(object sender, EventArgs e)
        {
            if (comboBox3.Text == "")
            {

            }

            else
            {
                if (existe_registro(comboBox3.Text))
                {
                    toolStripButton4.Enabled = true;
                }
                else
                {
                    MessageBox.Show("NUMERO DE REGISTRO NO EXISTE");
                    comboBox3.Focus();
                    toolStripButton4.Enabled = false;
                }
            }
        }

        private void proveedorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Proveedores_Flota prov = new Proveedores_Flota();
            prov.ShowDialog(this);
        }

        private void vehiculosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Control_Vehiculos c_v = new Control_Vehiculos();
            c_v.ShowDialog(this);
        }

        private void formasDePagoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forma_pago_flota fp_f = new Forma_pago_flota();
            fp_f.ShowDialog(this);
        }

    }
      
}
