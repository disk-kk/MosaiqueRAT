﻿namespace Serveur.Views
{
    partial class FrmListener
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.chkPopupNotification = new System.Windows.Forms.CheckBox();
            this.chkStartupConnections = new System.Windows.Forms.CheckBox();
            this.txtPort = new System.Windows.Forms.NumericUpDown();
            this.btnListen = new System.Windows.Forms.Button();
            this.lblPort = new System.Windows.Forms.Label();
            this.chkIPv6 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(146, 115);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 27);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(239, 115);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(87, 27);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // chkPopupNotification
            // 
            this.chkPopupNotification.AutoSize = true;
            this.chkPopupNotification.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPopupNotification.Location = new System.Drawing.Point(15, 88);
            this.chkPopupNotification.Name = "chkPopupNotification";
            this.chkPopupNotification.Size = new System.Drawing.Size(265, 19);
            this.chkPopupNotification.TabIndex = 12;
            this.chkPopupNotification.Text = "Show popup notification on new connection.";
            this.chkPopupNotification.UseVisualStyleBackColor = true;
            // 
            // chkStartupConnections
            // 
            this.chkStartupConnections.AutoSize = true;
            this.chkStartupConnections.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkStartupConnections.Location = new System.Drawing.Point(15, 60);
            this.chkStartupConnections.Name = "chkStartupConnections";
            this.chkStartupConnections.Size = new System.Drawing.Size(228, 19);
            this.chkStartupConnections.TabIndex = 11;
            this.chkStartupConnections.Text = "Listen for new connections on startup.";
            this.chkStartupConnections.UseVisualStyleBackColor = true;
            // 
            // txtPort
            // 
            this.txtPort.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPort.Location = new System.Drawing.Point(119, 5);
            this.txtPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.txtPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(68, 23);
            this.txtPort.TabIndex = 10;
            this.txtPort.Value = new decimal(new int[] {
            4444,
            0,
            0,
            0});
            // 
            // btnListen
            // 
            this.btnListen.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnListen.Location = new System.Drawing.Point(199, 5);
            this.btnListen.Name = "btnListen";
            this.btnListen.Size = new System.Drawing.Size(127, 24);
            this.btnListen.TabIndex = 9;
            this.btnListen.Text = "Start listening";
            this.btnListen.UseVisualStyleBackColor = true;
            this.btnListen.Click += new System.EventHandler(this.btnListen_Click);
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPort.Location = new System.Drawing.Point(12, 9);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(97, 15);
            this.lblPort.TabIndex = 8;
            this.lblPort.Text = "Port to listen on :";
            // 
            // chkIPv6
            // 
            this.chkIPv6.AutoSize = true;
            this.chkIPv6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkIPv6.Location = new System.Drawing.Point(15, 35);
            this.chkIPv6.Name = "chkIPv6";
            this.chkIPv6.Size = new System.Drawing.Size(134, 19);
            this.chkIPv6.TabIndex = 15;
            this.chkIPv6.Text = "Enable IPv6 Protocol";
            this.chkIPv6.UseVisualStyleBackColor = true;
            // 
            // FrmListener
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(338, 151);
            this.Controls.Add(this.chkIPv6);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.chkPopupNotification);
            this.Controls.Add(this.chkStartupConnections);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.btnListen);
            this.Controls.Add(this.lblPort);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmListener";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.FrmListener_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.CheckBox chkPopupNotification;
        private System.Windows.Forms.CheckBox chkStartupConnections;
        private System.Windows.Forms.NumericUpDown txtPort;
        private System.Windows.Forms.Button btnListen;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.CheckBox chkIPv6;
    }
}