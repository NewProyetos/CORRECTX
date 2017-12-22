namespace Sinconizacion_EXactus.CORECTX_APP.VENTAS
{
    partial class muebles_imagenes
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource3 = new Microsoft.Reporting.WinForms.ReportDataSource();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(muebles_imagenes));
            this.MUEBLES_MECHAN_CLIENTEBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.muebles_merchan = new Sinconizacion_EXactus.Muebles_merchan();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.mUEBLESMECHANCLIENTEBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.mUEBLES_MECHAN_CLIENTETableAdapter = new Sinconizacion_EXactus.Muebles_merchanTableAdapters.MUEBLES_MECHAN_CLIENTETableAdapter();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.MUEBLES_MECHAN_CLIENTEBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.muebles_merchan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mUEBLESMECHANCLIENTEBindingSource)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MUEBLES_MECHAN_CLIENTEBindingSource
            // 
            this.MUEBLES_MECHAN_CLIENTEBindingSource.DataMember = "MUEBLES_MECHAN_CLIENTE";
            this.MUEBLES_MECHAN_CLIENTEBindingSource.DataSource = this.muebles_merchan;
            // 
            // muebles_merchan
            // 
            this.muebles_merchan.DataSetName = "Muebles_merchan";
            this.muebles_merchan.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            reportDataSource3.Name = "muebles";
            reportDataSource3.Value = this.MUEBLES_MECHAN_CLIENTEBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource3);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Sinconizacion_EXactus.CORECTX APP.VENTAS.muebles.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 71);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(791, 389);
            this.reportViewer1.TabIndex = 0;
            // 
            // mUEBLESMECHANCLIENTEBindingSource
            // 
            this.mUEBLESMECHANCLIENTEBindingSource.DataMember = "MUEBLES_MECHAN_CLIENTE";
            this.mUEBLESMECHANCLIENTEBindingSource.DataSource = this.muebles_merchan;
            // 
            // mUEBLES_MECHAN_CLIENTETableAdapter
            // 
            this.mUEBLES_MECHAN_CLIENTETableAdapter.ClearBeforeFill = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.comboBox2);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(0, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(791, 62);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(551, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Generar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(407, 21);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 5;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(349, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "CLIENTE";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(243, 20);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(61, 21);
            this.comboBox2.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(192, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "CANAL:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(86, 20);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(61, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "RUTA:";
            // 
            // muebles_imagenes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 460);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.reportViewer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "muebles_imagenes";
            this.Text = "muebles_imagenes";
            this.Load += new System.EventHandler(this.muebles_imagenes_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MUEBLES_MECHAN_CLIENTEBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.muebles_merchan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mUEBLESMECHANCLIENTEBindingSource)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource mUEBLESMECHANCLIENTEBindingSource;
        private Muebles_merchan muebles_merchan;
        private Muebles_merchanTableAdapters.MUEBLES_MECHAN_CLIENTETableAdapter mUEBLES_MECHAN_CLIENTETableAdapter;
        private System.Windows.Forms.BindingSource MUEBLES_MECHAN_CLIENTEBindingSource;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;

    }
}