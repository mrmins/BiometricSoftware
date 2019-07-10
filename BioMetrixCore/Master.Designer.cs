namespace BioMetrixCore
{
    partial class Master
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnPingDevice = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnBeep = new System.Windows.Forms.Button();
            this.btnDownloadFingerPrint = new System.Windows.Forms.Button();
            this.btnPullData = new System.Windows.Forms.Button();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.cmbUbicacion = new System.Windows.Forms.ComboBox();
            this.dgvRecords = new System.Windows.Forms.DataGridView();
            this.btnPowerOff = new System.Windows.Forms.Button();
            this.btnRestartDevice = new System.Windows.Forms.Button();
            this.btnGetDeviceTime = new System.Windows.Forms.Button();
            this.btnEnableDevice = new System.Windows.Forms.Button();
            this.btnDisableDevice = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btn_eventos = new System.Windows.Forms.Button();
            this.btnSync = new System.Windows.Forms.Button();
            this.btnSyncAll = new System.Windows.Forms.Button();
            this.btnSincronizarHora = new System.Windows.Forms.Button();
            this.lblDeviceInfo = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecords)).BeginInit();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Ubicación de marcador";
            // 
            // btnPingDevice
            // 
            this.btnPingDevice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPingDevice.Location = new System.Drawing.Point(641, 3);
            this.btnPingDevice.Name = "btnPingDevice";
            this.btnPingDevice.Size = new System.Drawing.Size(75, 48);
            this.btnPingDevice.TabIndex = 5;
            this.btnPingDevice.Text = "Ping";
            this.btnPingDevice.UseVisualStyleBackColor = true;
            this.btnPingDevice.Click += new System.EventHandler(this.btnPingDevice_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStatus.Location = new System.Drawing.Point(0, 483);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.lblStatus.Size = new System.Drawing.Size(1153, 25);
            this.lblStatus.TabIndex = 6;
            this.lblStatus.Text = "label3";
            // 
            // btnBeep
            // 
            this.btnBeep.Location = new System.Drawing.Point(280, 3);
            this.btnBeep.Name = "btnBeep";
            this.btnBeep.Size = new System.Drawing.Size(59, 48);
            this.btnBeep.TabIndex = 5;
            this.btnBeep.Text = "Beep";
            this.btnBeep.UseVisualStyleBackColor = true;
            this.btnBeep.Click += new System.EventHandler(this.btnBeep_Click);
            // 
            // btnDownloadFingerPrint
            // 
            this.btnDownloadFingerPrint.Location = new System.Drawing.Point(3, 3);
            this.btnDownloadFingerPrint.Name = "btnDownloadFingerPrint";
            this.btnDownloadFingerPrint.Size = new System.Drawing.Size(77, 48);
            this.btnDownloadFingerPrint.TabIndex = 9;
            this.btnDownloadFingerPrint.Text = "Usuarios registrados";
            this.btnDownloadFingerPrint.UseVisualStyleBackColor = true;
            this.btnDownloadFingerPrint.Click += new System.EventHandler(this.btnDownloadFingerPrint_Click);
            // 
            // btnPullData
            // 
            this.btnPullData.Location = new System.Drawing.Point(86, 3);
            this.btnPullData.Name = "btnPullData";
            this.btnPullData.Size = new System.Drawing.Size(90, 48);
            this.btnPullData.TabIndex = 10;
            this.btnPullData.Text = "Obtener marcaciones en terminal";
            this.btnPullData.UseVisualStyleBackColor = true;
            this.btnPullData.Click += new System.EventHandler(this.btnPullData_Click);
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Controls.Add(this.cmbUbicacion);
            this.pnlHeader.Controls.Add(this.label1);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1153, 37);
            this.pnlHeader.TabIndex = 712;
            this.pnlHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlHeader_Paint);
            // 
            // cmbUbicacion
            // 
            this.cmbUbicacion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbUbicacion.FormattingEnabled = true;
            this.cmbUbicacion.Location = new System.Drawing.Point(143, 9);
            this.cmbUbicacion.Name = "cmbUbicacion";
            this.cmbUbicacion.Size = new System.Drawing.Size(998, 21);
            this.cmbUbicacion.TabIndex = 9;
            this.cmbUbicacion.SelectedIndexChanged += new System.EventHandler(this.cmbUbicacion_SelectedIndexChanged);
            // 
            // dgvRecords
            // 
            this.dgvRecords.AllowUserToAddRows = false;
            this.dgvRecords.AllowUserToDeleteRows = false;
            this.dgvRecords.AllowUserToOrderColumns = true;
            this.dgvRecords.AllowUserToResizeColumns = false;
            this.dgvRecords.AllowUserToResizeRows = false;
            this.dgvRecords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRecords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRecords.Location = new System.Drawing.Point(0, 54);
            this.dgvRecords.Name = "dgvRecords";
            this.dgvRecords.Size = new System.Drawing.Size(1129, 360);
            this.dgvRecords.TabIndex = 883;
            // 
            // btnPowerOff
            // 
            this.btnPowerOff.Location = new System.Drawing.Point(570, 3);
            this.btnPowerOff.Name = "btnPowerOff";
            this.btnPowerOff.Size = new System.Drawing.Size(65, 48);
            this.btnPowerOff.TabIndex = 885;
            this.btnPowerOff.Text = "Apagar";
            this.btnPowerOff.UseVisualStyleBackColor = true;
            this.btnPowerOff.Click += new System.EventHandler(this.btnPowerOff_Click);
            // 
            // btnRestartDevice
            // 
            this.btnRestartDevice.Location = new System.Drawing.Point(499, 3);
            this.btnRestartDevice.Name = "btnRestartDevice";
            this.btnRestartDevice.Size = new System.Drawing.Size(65, 48);
            this.btnRestartDevice.TabIndex = 886;
            this.btnRestartDevice.Text = "Reiniciar terminal";
            this.btnRestartDevice.UseVisualStyleBackColor = true;
            this.btnRestartDevice.Click += new System.EventHandler(this.btnRestartDevice_Click);
            // 
            // btnGetDeviceTime
            // 
            this.btnGetDeviceTime.Location = new System.Drawing.Point(182, 3);
            this.btnGetDeviceTime.Name = "btnGetDeviceTime";
            this.btnGetDeviceTime.Size = new System.Drawing.Size(92, 48);
            this.btnGetDeviceTime.TabIndex = 887;
            this.btnGetDeviceTime.Text = "Obtener hora del dispositivo";
            this.btnGetDeviceTime.UseVisualStyleBackColor = true;
            this.btnGetDeviceTime.Click += new System.EventHandler(this.btnGetDeviceTime_Click);
            // 
            // btnEnableDevice
            // 
            this.btnEnableDevice.Location = new System.Drawing.Point(345, 3);
            this.btnEnableDevice.Name = "btnEnableDevice";
            this.btnEnableDevice.Size = new System.Drawing.Size(65, 48);
            this.btnEnableDevice.TabIndex = 889;
            this.btnEnableDevice.Text = "Habilitar terminal";
            this.btnEnableDevice.UseVisualStyleBackColor = true;
            this.btnEnableDevice.Click += new System.EventHandler(this.btnEnableDevice_Click);
            // 
            // btnDisableDevice
            // 
            this.btnDisableDevice.Location = new System.Drawing.Point(416, 3);
            this.btnDisableDevice.Name = "btnDisableDevice";
            this.btnDisableDevice.Size = new System.Drawing.Size(77, 48);
            this.btnDisableDevice.TabIndex = 890;
            this.btnDisableDevice.Text = "Desabilitar terminal";
            this.btnDisableDevice.UseVisualStyleBackColor = true;
            this.btnDisableDevice.Click += new System.EventHandler(this.btnDisableDevice_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.dgvRecords);
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Location = new System.Drawing.Point(12, 68);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1129, 414);
            this.panel1.TabIndex = 891;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.btnDownloadFingerPrint);
            this.flowLayoutPanel1.Controls.Add(this.btnPullData);
            this.flowLayoutPanel1.Controls.Add(this.btnGetDeviceTime);
            this.flowLayoutPanel1.Controls.Add(this.btnBeep);
            this.flowLayoutPanel1.Controls.Add(this.btnEnableDevice);
            this.flowLayoutPanel1.Controls.Add(this.btnDisableDevice);
            this.flowLayoutPanel1.Controls.Add(this.btnRestartDevice);
            this.flowLayoutPanel1.Controls.Add(this.btnPowerOff);
            this.flowLayoutPanel1.Controls.Add(this.btnPingDevice);
            this.flowLayoutPanel1.Controls.Add(this.btn_eventos);
            this.flowLayoutPanel1.Controls.Add(this.btnSync);
            this.flowLayoutPanel1.Controls.Add(this.btnSyncAll);
            this.flowLayoutPanel1.Controls.Add(this.btnSincronizarHora);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1129, 54);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // btn_eventos
            // 
            this.btn_eventos.Location = new System.Drawing.Point(722, 3);
            this.btn_eventos.Name = "btn_eventos";
            this.btn_eventos.Size = new System.Drawing.Size(75, 48);
            this.btn_eventos.TabIndex = 895;
            this.btn_eventos.Text = "Visor de eventos";
            this.btn_eventos.UseVisualStyleBackColor = true;
            this.btn_eventos.Click += new System.EventHandler(this.btn_eventos_Click);
            // 
            // btnSync
            // 
            this.btnSync.Location = new System.Drawing.Point(803, 3);
            this.btnSync.Name = "btnSync";
            this.btnSync.Size = new System.Drawing.Size(85, 48);
            this.btnSync.TabIndex = 894;
            this.btnSync.Text = "Sincronizar terminal actual";
            this.btnSync.UseVisualStyleBackColor = true;
            this.btnSync.Click += new System.EventHandler(this.btnSync_Click);
            // 
            // btnSyncAll
            // 
            this.btnSyncAll.Location = new System.Drawing.Point(894, 3);
            this.btnSyncAll.Name = "btnSyncAll";
            this.btnSyncAll.Size = new System.Drawing.Size(126, 48);
            this.btnSyncAll.TabIndex = 896;
            this.btnSyncAll.Text = "Sincronizar marcaciones de todas las terminales";
            this.btnSyncAll.UseVisualStyleBackColor = true;
            this.btnSyncAll.Click += new System.EventHandler(this.btnSyncAll_Click);
            // 
            // btnSincronizarHora
            // 
            this.btnSincronizarHora.Location = new System.Drawing.Point(1026, 3);
            this.btnSincronizarHora.Name = "btnSincronizarHora";
            this.btnSincronizarHora.Size = new System.Drawing.Size(100, 48);
            this.btnSincronizarHora.TabIndex = 897;
            this.btnSincronizarHora.Text = "Sincronizar hora en todas las terminales";
            this.btnSincronizarHora.UseVisualStyleBackColor = true;
            this.btnSincronizarHora.Click += new System.EventHandler(this.btnSincronizarHora_Click);
            // 
            // lblDeviceInfo
            // 
            this.lblDeviceInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDeviceInfo.Location = new System.Drawing.Point(11, 45);
            this.lblDeviceInfo.Name = "lblDeviceInfo";
            this.lblDeviceInfo.Size = new System.Drawing.Size(1130, 19);
            this.lblDeviceInfo.TabIndex = 892;
            this.lblDeviceInfo.Text = "Información del dispositivo : --";
            // 
            // Master
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1153, 508);
            this.Controls.Add(this.lblDeviceInfo);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.lblStatus);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(615, 425);
            this.Name = "Master";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Administración de Sistemas Biométricos";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecords)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnPingDevice;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnBeep;
        private System.Windows.Forms.Button btnDownloadFingerPrint;
        private System.Windows.Forms.Button btnPullData;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.DataGridView dgvRecords;
        private System.Windows.Forms.Button btnPowerOff;
        private System.Windows.Forms.Button btnRestartDevice;
        private System.Windows.Forms.Button btnGetDeviceTime;
        private System.Windows.Forms.Button btnEnableDevice;
        private System.Windows.Forms.Button btnDisableDevice;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label lblDeviceInfo;
        private System.Windows.Forms.ComboBox cmbUbicacion;
        private System.Windows.Forms.Button btnSync;
        private System.Windows.Forms.Button btn_eventos;
        private System.Windows.Forms.Button btnSyncAll;
        private System.Windows.Forms.Button btnSincronizarHora;
    }
}

