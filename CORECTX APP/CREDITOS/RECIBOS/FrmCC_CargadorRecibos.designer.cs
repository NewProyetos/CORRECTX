namespace Sinconizacion_EXactus
{
    partial class FrmCC_CargadorRecibos
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BtnGenerar = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.BtnCSV = new System.Windows.Forms.Button();
            this.dlGuardar = new System.Windows.Forms.SaveFileDialog();
            this.BtnGenerarCSV = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtConceptoGral = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Calendario = new System.Windows.Forms.DateTimePicker();
            this.TxtConcepDoc = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.CBEntrega = new System.Windows.Forms.ComboBox();
            this.FechaIni = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.FechaFin = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.TxtMonto = new System.Windows.Forms.TextBox();
            this.BtnSalir = new System.Windows.Forms.Button();
            this.LbRegistros = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnGenerar
            // 
            this.BtnGenerar.Location = new System.Drawing.Point(154, 324);
            this.BtnGenerar.Name = "BtnGenerar";
            this.BtnGenerar.Size = new System.Drawing.Size(121, 34);
            this.BtnGenerar.TabIndex = 18;
            this.BtnGenerar.Text = "Generar Información";
            this.BtnGenerar.UseVisualStyleBackColor = true;
            this.BtnGenerar.Click += new System.EventHandler(this.BtnGenerar_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(10, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(390, 137);
            this.dataGridView1.TabIndex = 0;
            // 
            // BtnCSV
            // 
            this.BtnCSV.Enabled = false;
            this.BtnCSV.Location = new System.Drawing.Point(287, 329);
            this.BtnCSV.Name = "BtnCSV";
            this.BtnCSV.Size = new System.Drawing.Size(124, 23);
            this.BtnCSV.TabIndex = 21;
            this.BtnCSV.Text = "Crear CSV Dinamico";
            this.BtnCSV.UseVisualStyleBackColor = true;
            this.BtnCSV.Visible = false;
            this.BtnCSV.Click += new System.EventHandler(this.BtnCSV_Click);
            // 
            // BtnGenerarCSV
            // 
            this.BtnGenerarCSV.Enabled = false;
            this.BtnGenerarCSV.Location = new System.Drawing.Point(154, 364);
            this.BtnGenerarCSV.Name = "BtnGenerarCSV";
            this.BtnGenerarCSV.Size = new System.Drawing.Size(121, 30);
            this.BtnGenerarCSV.TabIndex = 19;
            this.BtnGenerarCSV.Text = "Crear CSV";
            this.BtnGenerarCSV.UseVisualStyleBackColor = true;
            this.BtnGenerarCSV.Click += new System.EventHandler(this.BtnGenerarCSV_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 270);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Concepto General:";
            // 
            // TxtConceptoGral
            // 
            this.TxtConceptoGral.Location = new System.Drawing.Point(121, 267);
            this.TxtConceptoGral.Name = "TxtConceptoGral";
            this.TxtConceptoGral.Size = new System.Drawing.Size(269, 20);
            this.TxtConceptoGral.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 241);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Fecha de Abono:";
            // 
            // Calendario
            // 
            this.Calendario.CustomFormat = "dd/mm/aaaa";
            this.Calendario.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.Calendario.Location = new System.Drawing.Point(121, 241);
            this.Calendario.Name = "Calendario";
            this.Calendario.Size = new System.Drawing.Size(94, 20);
            this.Calendario.TabIndex = 13;
            // 
            // TxtConcepDoc
            // 
            this.TxtConcepDoc.Location = new System.Drawing.Point(121, 293);
            this.TxtConcepDoc.Name = "TxtConcepDoc";
            this.TxtConcepDoc.Size = new System.Drawing.Size(269, 20);
            this.TxtConcepDoc.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 296);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Concepto de Recibo:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 191);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Entrega:";
            // 
            // CBEntrega
            // 
            this.CBEntrega.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBEntrega.FormattingEnabled = true;
            this.CBEntrega.Location = new System.Drawing.Point(121, 187);
            this.CBEntrega.Name = "CBEntrega";
            this.CBEntrega.Size = new System.Drawing.Size(65, 21);
            this.CBEntrega.TabIndex = 7;
            // 
            // FechaIni
            // 
            this.FechaIni.CustomFormat = "dd/mm/aaaa";
            this.FechaIni.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.FechaIni.Location = new System.Drawing.Point(121, 213);
            this.FechaIni.Name = "FechaIni";
            this.FechaIni.Size = new System.Drawing.Size(94, 20);
            this.FechaIni.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 215);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Pagar Desde:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(246, 216);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Hasta:";
            // 
            // FechaFin
            // 
            this.FechaFin.CustomFormat = "dd/mm/aaaa";
            this.FechaFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.FechaFin.Location = new System.Drawing.Point(290, 213);
            this.FechaFin.Name = "FechaFin";
            this.FechaFin.Size = new System.Drawing.Size(94, 20);
            this.FechaFin.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(244, 162);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Monto:";
            // 
            // TxtMonto
            // 
            this.TxtMonto.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtMonto.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.TxtMonto.Location = new System.Drawing.Point(290, 158);
            this.TxtMonto.Name = "TxtMonto";
            this.TxtMonto.ReadOnly = true;
            this.TxtMonto.Size = new System.Drawing.Size(100, 20);
            this.TxtMonto.TabIndex = 5;
            this.TxtMonto.Text = "0.0";
            this.TxtMonto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TxtMonto.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtMonto_KeyPress);
            // 
            // BtnSalir
            // 
            this.BtnSalir.Location = new System.Drawing.Point(154, 400);
            this.BtnSalir.Name = "BtnSalir";
            this.BtnSalir.Size = new System.Drawing.Size(121, 36);
            this.BtnSalir.TabIndex = 20;
            this.BtnSalir.Text = "SALIR";
            this.BtnSalir.UseVisualStyleBackColor = true;
            this.BtnSalir.Click += new System.EventHandler(this.BtnSalir_Click);
            // 
            // LbRegistros
            // 
            this.LbRegistros.AutoSize = true;
            this.LbRegistros.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbRegistros.Location = new System.Drawing.Point(125, 162);
            this.LbRegistros.Name = "LbRegistros";
            this.LbRegistros.Size = new System.Drawing.Size(0, 13);
            this.LbRegistros.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 162);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Total Registros:";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Location = new System.Drawing.Point(10, 152);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(388, 31);
            this.panel1.TabIndex = 1;
            // 
            // FrmCC_CargadorRecibos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 453);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.LbRegistros);
            this.Controls.Add(this.BtnSalir);
            this.Controls.Add(this.TxtMonto);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.FechaFin);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.FechaIni);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.CBEntrega);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TxtConcepDoc);
            this.Controls.Add(this.Calendario);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TxtConceptoGral);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnGenerarCSV);
            this.Controls.Add(this.BtnCSV);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.BtnGenerar);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCC_CargadorRecibos";
            this.Text = "Cargador dePagos de Contado";
            this.Load += new System.EventHandler(this.FrmCC_CargadorRecibos_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnGenerar;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button BtnCSV;
        private System.Windows.Forms.SaveFileDialog dlGuardar;
        private System.Windows.Forms.Button BtnGenerarCSV;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtConceptoGral;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker Calendario;
        private System.Windows.Forms.TextBox TxtConcepDoc;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox CBEntrega;
        private System.Windows.Forms.DateTimePicker FechaIni;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker FechaFin;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox TxtMonto;
        private System.Windows.Forms.Button BtnSalir;
        private System.Windows.Forms.Label LbRegistros;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel1;
    }
}