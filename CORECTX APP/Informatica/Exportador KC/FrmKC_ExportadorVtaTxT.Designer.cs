namespace Sinconizacion_EXactus
{
    partial class FrmKC_ExportadorVtaTxT
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
            this.Generar = new System.Windows.Forms.Button();
            this.FechaFin = new System.Windows.Forms.DateTimePicker();
            this.FechaIni = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.dlGuardar = new System.Windows.Forms.SaveFileDialog();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.BtnSalir = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.BtnFTP = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // Generar
            // 
            this.Generar.Location = new System.Drawing.Point(132, 244);
            this.Generar.Name = "Generar";
            this.Generar.Size = new System.Drawing.Size(121, 41);
            this.Generar.TabIndex = 5;
            this.Generar.Text = "Generar";
            this.Generar.UseVisualStyleBackColor = true;
            this.Generar.Click += new System.EventHandler(this.Generar_Click);
            // 
            // FechaFin
            // 
            this.FechaFin.CustomFormat = "dd/mm/aaaa";
            this.FechaFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.FechaFin.Location = new System.Drawing.Point(241, 195);
            this.FechaFin.Name = "FechaFin";
            this.FechaFin.Size = new System.Drawing.Size(94, 20);
            this.FechaFin.TabIndex = 4;
            // 
            // FechaIni
            // 
            this.FechaIni.CustomFormat = "dd/mm/aaaa";
            this.FechaIni.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.FechaIni.Location = new System.Drawing.Point(111, 195);
            this.FechaIni.Name = "FechaIni";
            this.FechaIni.Size = new System.Drawing.Size(94, 20);
            this.FechaIni.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 199);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Rango a Generar";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(350, 150);
            this.dataGridView1.TabIndex = 0;
            // 
            // BtnSalir
            // 
            this.BtnSalir.Location = new System.Drawing.Point(132, 355);
            this.BtnSalir.Name = "BtnSalir";
            this.BtnSalir.Size = new System.Drawing.Size(121, 41);
            this.BtnSalir.TabIndex = 7;
            this.BtnSalir.Text = "SALIR";
            this.BtnSalir.UseVisualStyleBackColor = true;
            this.BtnSalir.Click += new System.EventHandler(this.BtnSalir_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 162);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(350, 23);
            this.progressBar1.TabIndex = 1;
            // 
            // BtnFTP
            // 
            this.BtnFTP.Location = new System.Drawing.Point(132, 296);
            this.BtnFTP.Name = "BtnFTP";
            this.BtnFTP.Size = new System.Drawing.Size(121, 41);
            this.BtnFTP.TabIndex = 6;
            this.BtnFTP.Text = "Carga Directa FTP";
            this.BtnFTP.UseVisualStyleBackColor = true;
            this.BtnFTP.Click += new System.EventHandler(this.BtnFTP_Click);
            // 
            // button1
            // 
            this.button1.Image = global::Sinconizacion_EXactus.Properties.Resources.excelpeq;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button1.Location = new System.Drawing.Point(298, 244);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(64, 55);
            this.button1.TabIndex = 8;
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FrmKC_ExportadorVtaTxT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 430);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.BtnFTP);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.BtnSalir);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.FechaFin);
            this.Controls.Add(this.FechaIni);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Generar);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmKC_ExportadorVtaTxT";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Exportador para Kimberly Clark";
            this.Load += new System.EventHandler(this.FrmKC_ExportadorVtaTxT_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Generar;
        private System.Windows.Forms.DateTimePicker FechaFin;
        private System.Windows.Forms.DateTimePicker FechaIni;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.SaveFileDialog dlGuardar;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button BtnSalir;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button BtnFTP;
        private System.Windows.Forms.Button button1;
    }
}