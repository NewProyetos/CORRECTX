namespace Sinconizacion_EXactus
{
    partial class Cargas
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
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource3 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource4 = new Microsoft.Reporting.WinForms.ReportDataSource();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Cargas));
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.CargasBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ReporteCC = new Sinconizacion_EXactus.ReporteCC();
            this.Cargas_KITBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cLIENTESCONTADOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cLIENTESCREDITOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CargasBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReporteCC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Cargas_KITBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cLIENTESCONTADOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cLIENTESCREDITOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(118, 27);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(99, 20);
            this.dateTimePicker1.TabIndex = 0;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(7, 27);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(86, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Ruta";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(121, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Fecha";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(237, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(63, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Generar ";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Location = new System.Drawing.Point(12, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(624, 54);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBox2);
            this.groupBox2.Location = new System.Drawing.Point(457, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(159, 43);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Usuario Creacion";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(18, 16);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 21);
            this.comboBox2.TabIndex = 1;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            reportDataSource1.Name = "Cargas";
            reportDataSource1.Value = this.CargasBindingSource;
            reportDataSource2.Name = "kit";
            reportDataSource2.Value = this.Cargas_KITBindingSource;
            reportDataSource3.Name = "LIQUIDACION_CONTADO";
            reportDataSource3.Value = this.cLIENTESCONTADOBindingSource;
            reportDataSource4.Name = "LIQUIDACION_CREDITO";
            reportDataSource4.Value = this.cLIENTESCREDITOBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource3);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource4);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Sinconizacion_EXactus.CORECTX APP.BODEGA.Reporte Cargas Bodega.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(3, 63);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(633, 345);
            this.reportViewer1.TabIndex = 6;
            // 
            // CargasBindingSource
            // 
            this.CargasBindingSource.DataMember = "Cargas";
            this.CargasBindingSource.DataSource = this.ReporteCC;
            // 
            // ReporteCC
            // 
            this.ReporteCC.DataSetName = "ReporteCC";
            this.ReporteCC.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // Cargas_KITBindingSource
            // 
            this.Cargas_KITBindingSource.DataMember = "Cargas_KIT";
            this.Cargas_KITBindingSource.DataSource = this.ReporteCC;
            // 
            // cLIENTESCONTADOBindingSource
            // 
            this.cLIENTESCONTADOBindingSource.DataMember = "CLIENTES_CONTADO";
            this.cLIENTESCONTADOBindingSource.DataSource = this.ReporteCC;
            // 
            // cLIENTESCREDITOBindingSource
            // 
            this.cLIENTESCREDITOBindingSource.DataMember = "CLIENTES_CREDITO";
            this.cLIENTESCREDITOBindingSource.DataSource = this.ReporteCC;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(320, 16);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(117, 30);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "Mostrar \r\nDetalle Documento";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // Cargas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 413);
            this.Controls.Add(this.reportViewer1);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Cargas";
            this.Text = "Cargas";
            this.Load += new System.EventHandler(this.Cargas_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CargasBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReporteCC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Cargas_KITBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cLIENTESCONTADOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cLIENTESCREDITOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource CargasBindingSource;
        private ReporteCC ReporteCC;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.BindingSource Cargas_KITBindingSource;
        private System.Windows.Forms.BindingSource cLIENTESCONTADOBindingSource;
        private System.Windows.Forms.BindingSource cLIENTESCREDITOBindingSource;
        private System.Windows.Forms.CheckBox checkBox1;
        // private Sinconizacion_EXactus.ReporteCCTableAdapters.CargasTableAdapter CargasTableAdapter;
    }
}