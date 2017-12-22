namespace Sinconizacion_EXactus.CORECTX_APP.RRHH
{
    partial class Reporte_Evaluaciones
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
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.recursos_Humanos = new Sinconizacion_EXactus.Recursos_Humanos();
            this.oBJETIVOSEVALUACIONBindingSource = new System.Windows.Forms.BindingSource(this.components);
            //this.oBJETIVOS_EVALUACIONTableAdapter = new Sinconizacion_EXactus.Recursos_HumanosTableAdapters.OBJETIVOS_EVALUACIONTableAdapter();
            this.eVALUACIONBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.eVALUACIONTableAdapter = new Sinconizacion_EXactus.Recursos_HumanosTableAdapters.EVALUACIONTableAdapter();
            this.button1 = new System.Windows.Forms.Button();
            this.OBJETIVOS_EVALUACIONBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.recursos_Humanos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.oBJETIVOSEVALUACIONBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eVALUACIONBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OBJETIVOS_EVALUACIONBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.OBJETIVOS_EVALUACIONBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Sinconizacion_EXactus.CORECTX APP.RRHH.Report1.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(12, 56);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(713, 316);
            this.reportViewer1.TabIndex = 0;
            // 
            // recursos_Humanos
            // 
            this.recursos_Humanos.DataSetName = "Recursos_Humanos";
            this.recursos_Humanos.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // oBJETIVOSEVALUACIONBindingSource
            // 
            this.oBJETIVOSEVALUACIONBindingSource.DataMember = "OBJETIVOS_EVALUACION";
            this.oBJETIVOSEVALUACIONBindingSource.DataSource = this.recursos_Humanos;
            // 
            // oBJETIVOS_EVALUACIONTableAdapter
            // 
          //  this.oBJETIVOS_EVALUACIONTableAdapter.ClearBeforeFill = true;
            // 
            // eVALUACIONBindingSource
            // 
            this.eVALUACIONBindingSource.DataMember = "EVALUACION";
            this.eVALUACIONBindingSource.DataSource = this.recursos_Humanos;
            // 
            // eVALUACIONTableAdapter
            // 
            //this.eVALUACIONTableAdapter.ClearBeforeFill = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(263, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // OBJETIVOS_EVALUACIONBindingSource
            // 
            this.OBJETIVOS_EVALUACIONBindingSource.DataMember = "OBJETIVOS_EVALUACION";
            this.OBJETIVOS_EVALUACIONBindingSource.DataSource = this.recursos_Humanos;
            // 
            // Reporte_Evaluaciones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 384);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.reportViewer1);
            this.Name = "Reporte_Evaluaciones";
            this.Text = "Reporte_Evaluaciones";
            this.Load += new System.EventHandler(this.Reporte_Evaluaciones_Load);
            ((System.ComponentModel.ISupportInitialize)(this.recursos_Humanos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.oBJETIVOSEVALUACIONBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eVALUACIONBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OBJETIVOS_EVALUACIONBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private Recursos_Humanos recursos_Humanos;
        private System.Windows.Forms.BindingSource oBJETIVOSEVALUACIONBindingSource;
       // private Recursos_HumanosTableAdapters.OBJETIVOS_EVALUACIONTableAdapter oBJETIVOS_EVALUACIONTableAdapter;
        private System.Windows.Forms.BindingSource eVALUACIONBindingSource;
        private Recursos_HumanosTableAdapters.EVALUACIONTableAdapter eVALUACIONTableAdapter;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.BindingSource OBJETIVOS_EVALUACIONBindingSource;
    }
}