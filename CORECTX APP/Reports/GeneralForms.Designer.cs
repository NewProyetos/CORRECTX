namespace Sinconizacion_EXactus.CORECTX_APP.Reports
{
    partial class GeneralForms
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode2 = new DevExpress.XtraGrid.GridLevelNode();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GeneralForms));
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.muebles_merchan3 = new Sinconizacion_EXactus.Muebles_merchan();
            this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.reportE_REGALIAS_ENCTableAdapter1 = new Sinconizacion_EXactus.Muebles_merchanTableAdapters.REPORTE_REGALIAS_ENCTableAdapter();
            this.reportE_REGALIAS_DETTableAdapter1 = new Sinconizacion_EXactus.Muebles_merchanTableAdapters.REPORTE_REGALIAS_DETTableAdapter();
            this.reportE_REGALIAS_TRASTableAdapter1 = new Sinconizacion_EXactus.Muebles_merchanTableAdapters.REPORTE_REGALIAS_TRASTableAdapter();
            this.colNUM_REG = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRUTA = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colVENDEDOR = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCANTIDAD = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colESTADO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFECHA_CREA = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFECHA_APLIC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUSUARIO_CREA = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUSUARIO_APLICA = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCOMENTARIO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUPDATE_USER = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDATE_UPDATE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colREGALIA = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCONCEPTO = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.muebles_merchan3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridView2
            // 
            this.gridView2.GridControl = this.gridControl1;
            this.gridView2.Name = "gridView2";
            // 
            // gridControl1
            // 
            this.gridControl1.DataMember = "REPORTE_REGALIAS_ENC";
            this.gridControl1.DataSource = this.muebles_merchan3;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            gridLevelNode1.LevelTemplate = this.gridView2;
            gridLevelNode1.RelationName = "DETALLE REGALIA";
            gridLevelNode2.LevelTemplate = this.gridView3;
            gridLevelNode2.RelationName = "TRASPASO EXACTUS";
            this.gridControl1.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1,
            gridLevelNode2});
            this.gridControl1.Location = new System.Drawing.Point(2, 21);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1000, 580);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView3,
            this.gridView1,
            this.gridView2});
            // 
            // muebles_merchan3
            // 
            this.muebles_merchan3.DataSetName = "Muebles_merchan";
            this.muebles_merchan3.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // gridView3
            // 
            this.gridView3.GridControl = this.gridControl1;
            this.gridView3.Name = "gridView3";
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colNUM_REG,
            this.colRUTA,
            this.colVENDEDOR,
            this.colCANTIDAD,
            this.colESTADO,
            this.colFECHA_CREA,
            this.colFECHA_APLIC,
            this.colUSUARIO_CREA,
            this.colUSUARIO_APLICA,
            this.colCOMENTARIO,
            this.colUPDATE_USER,
            this.colDATE_UPDATE,
            this.colREGALIA,
            this.colCONCEPTO});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            // 
            // groupControl1
            // 
            this.groupControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupControl1.Controls.Add(this.gridControl1);
            this.groupControl1.Location = new System.Drawing.Point(2, 28);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1004, 603);
            this.groupControl1.TabIndex = 2;
            this.groupControl1.Text = "Nombre de Reporte";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1018, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // reportE_REGALIAS_ENCTableAdapter1
            // 
            this.reportE_REGALIAS_ENCTableAdapter1.ClearBeforeFill = true;
            // 
            // reportE_REGALIAS_DETTableAdapter1
            // 
            this.reportE_REGALIAS_DETTableAdapter1.ClearBeforeFill = true;
            // 
            // reportE_REGALIAS_TRASTableAdapter1
            // 
            this.reportE_REGALIAS_TRASTableAdapter1.ClearBeforeFill = true;
            // 
            // colNUM_REG
            // 
            this.colNUM_REG.FieldName = "NUM_REG";
            this.colNUM_REG.Name = "colNUM_REG";
            this.colNUM_REG.Visible = true;
            this.colNUM_REG.VisibleIndex = 0;
            // 
            // colRUTA
            // 
            this.colRUTA.FieldName = "RUTA";
            this.colRUTA.Name = "colRUTA";
            this.colRUTA.Visible = true;
            this.colRUTA.VisibleIndex = 1;
            // 
            // colVENDEDOR
            // 
            this.colVENDEDOR.FieldName = "VENDEDOR";
            this.colVENDEDOR.Name = "colVENDEDOR";
            this.colVENDEDOR.Visible = true;
            this.colVENDEDOR.VisibleIndex = 2;
            // 
            // colCANTIDAD
            // 
            this.colCANTIDAD.FieldName = "CANTIDAD";
            this.colCANTIDAD.Name = "colCANTIDAD";
            this.colCANTIDAD.Visible = true;
            this.colCANTIDAD.VisibleIndex = 3;
            // 
            // colESTADO
            // 
            this.colESTADO.FieldName = "ESTADO";
            this.colESTADO.Name = "colESTADO";
            this.colESTADO.Visible = true;
            this.colESTADO.VisibleIndex = 4;
            // 
            // colFECHA_CREA
            // 
            this.colFECHA_CREA.FieldName = "FECHA_CREA";
            this.colFECHA_CREA.Name = "colFECHA_CREA";
            this.colFECHA_CREA.Visible = true;
            this.colFECHA_CREA.VisibleIndex = 5;
            // 
            // colFECHA_APLIC
            // 
            this.colFECHA_APLIC.FieldName = "FECHA_APLIC";
            this.colFECHA_APLIC.Name = "colFECHA_APLIC";
            this.colFECHA_APLIC.Visible = true;
            this.colFECHA_APLIC.VisibleIndex = 6;
            // 
            // colUSUARIO_CREA
            // 
            this.colUSUARIO_CREA.FieldName = "USUARIO_CREA";
            this.colUSUARIO_CREA.Name = "colUSUARIO_CREA";
            this.colUSUARIO_CREA.Visible = true;
            this.colUSUARIO_CREA.VisibleIndex = 7;
            // 
            // colUSUARIO_APLICA
            // 
            this.colUSUARIO_APLICA.FieldName = "USUARIO_APLICA";
            this.colUSUARIO_APLICA.Name = "colUSUARIO_APLICA";
            this.colUSUARIO_APLICA.Visible = true;
            this.colUSUARIO_APLICA.VisibleIndex = 8;
            // 
            // colCOMENTARIO
            // 
            this.colCOMENTARIO.FieldName = "COMENTARIO";
            this.colCOMENTARIO.Name = "colCOMENTARIO";
            this.colCOMENTARIO.Visible = true;
            this.colCOMENTARIO.VisibleIndex = 9;
            // 
            // colUPDATE_USER
            // 
            this.colUPDATE_USER.FieldName = "UPDATE_USER";
            this.colUPDATE_USER.Name = "colUPDATE_USER";
            this.colUPDATE_USER.Visible = true;
            this.colUPDATE_USER.VisibleIndex = 10;
            // 
            // colDATE_UPDATE
            // 
            this.colDATE_UPDATE.FieldName = "DATE_UPDATE";
            this.colDATE_UPDATE.Name = "colDATE_UPDATE";
            this.colDATE_UPDATE.Visible = true;
            this.colDATE_UPDATE.VisibleIndex = 11;
            // 
            // colREGALIA
            // 
            this.colREGALIA.FieldName = "REGALIA";
            this.colREGALIA.Name = "colREGALIA";
            this.colREGALIA.Visible = true;
            this.colREGALIA.VisibleIndex = 12;
            // 
            // colCONCEPTO
            // 
            this.colCONCEPTO.FieldName = "CONCEPTO";
            this.colCONCEPTO.Name = "colCONCEPTO";
            this.colCONCEPTO.Visible = true;
            this.colCONCEPTO.VisibleIndex = 13;
            // 
            // GeneralForms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1018, 643);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.groupControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GeneralForms";
            this.Text = "GeneralForms";
            this.Load += new System.EventHandler(this.GeneralForms_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.muebles_merchan3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private Muebles_merchan muebles_merchan1;
        private Muebles_merchan muebles_merchan2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private Muebles_merchan muebles_merchan3;
        private Muebles_merchanTableAdapters.REPORTE_REGALIAS_ENCTableAdapter reportE_REGALIAS_ENCTableAdapter1;
        private Muebles_merchanTableAdapters.REPORTE_REGALIAS_DETTableAdapter reportE_REGALIAS_DETTableAdapter1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView3;
        private Muebles_merchanTableAdapters.REPORTE_REGALIAS_TRASTableAdapter reportE_REGALIAS_TRASTableAdapter1;
        private DevExpress.XtraGrid.Columns.GridColumn colNUM_REG;
        private DevExpress.XtraGrid.Columns.GridColumn colRUTA;
        private DevExpress.XtraGrid.Columns.GridColumn colVENDEDOR;
        private DevExpress.XtraGrid.Columns.GridColumn colCANTIDAD;
        private DevExpress.XtraGrid.Columns.GridColumn colESTADO;
        private DevExpress.XtraGrid.Columns.GridColumn colFECHA_CREA;
        private DevExpress.XtraGrid.Columns.GridColumn colFECHA_APLIC;
        private DevExpress.XtraGrid.Columns.GridColumn colUSUARIO_CREA;
        private DevExpress.XtraGrid.Columns.GridColumn colUSUARIO_APLICA;
        private DevExpress.XtraGrid.Columns.GridColumn colCOMENTARIO;
        private DevExpress.XtraGrid.Columns.GridColumn colUPDATE_USER;
        private DevExpress.XtraGrid.Columns.GridColumn colDATE_UPDATE;
        private DevExpress.XtraGrid.Columns.GridColumn colREGALIA;
        private DevExpress.XtraGrid.Columns.GridColumn colCONCEPTO;
    }
}