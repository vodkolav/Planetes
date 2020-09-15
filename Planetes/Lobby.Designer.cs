﻿namespace Planetes
{
    partial class Lobby
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
            this.dgvwPlayers = new System.Windows.Forms.DataGridView();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.playerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.iDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maxHealthDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maxAmmoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jetDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.dgvwPlayers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.playerBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvwPlayers
            // 
            this.dgvwPlayers.AllowUserToAddRows = false;
            this.dgvwPlayers.AllowUserToDeleteRows = false;
            this.dgvwPlayers.AutoGenerateColumns = false;
            this.dgvwPlayers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvwPlayers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iDDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.maxHealthDataGridViewTextBoxColumn,
            this.maxAmmoDataGridViewTextBoxColumn,
            this.jetDataGridViewTextBoxColumn});
            this.dgvwPlayers.DataSource = this.playerBindingSource;
            this.dgvwPlayers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvwPlayers.Location = new System.Drawing.Point(0, 0);
            this.dgvwPlayers.Name = "dgvwPlayers";
            this.dgvwPlayers.Size = new System.Drawing.Size(561, 375);
            this.dgvwPlayers.TabIndex = 0;
            this.dgvwPlayers.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvwPlayers_CellValueChanged);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(3, 3);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(70, 60);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(79, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 60);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // playerBindingSource
            // 
            this.playerBindingSource.DataSource = typeof(GameObjects.Player);
            this.playerBindingSource.DataSourceChanged += new System.EventHandler(this.playerBindingSource_DataSourceChanged);
            this.playerBindingSource.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.playerBindingSource_ListChanged);
            // 
            // iDDataGridViewTextBoxColumn
            // 
            this.iDDataGridViewTextBoxColumn.DataPropertyName = "ID";
            this.iDDataGridViewTextBoxColumn.HeaderText = "ID";
            this.iDDataGridViewTextBoxColumn.Name = "iDDataGridViewTextBoxColumn";
            this.iDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            // 
            // maxHealthDataGridViewTextBoxColumn
            // 
            this.maxHealthDataGridViewTextBoxColumn.DataPropertyName = "MaxHealth";
            this.maxHealthDataGridViewTextBoxColumn.HeaderText = "MaxHealth";
            this.maxHealthDataGridViewTextBoxColumn.Name = "maxHealthDataGridViewTextBoxColumn";
            this.maxHealthDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // maxAmmoDataGridViewTextBoxColumn
            // 
            this.maxAmmoDataGridViewTextBoxColumn.DataPropertyName = "MaxAmmo";
            this.maxAmmoDataGridViewTextBoxColumn.HeaderText = "MaxAmmo";
            this.maxAmmoDataGridViewTextBoxColumn.Name = "maxAmmoDataGridViewTextBoxColumn";
            this.maxAmmoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // jetDataGridViewTextBoxColumn
            // 
            this.jetDataGridViewTextBoxColumn.DataPropertyName = "Color";
            this.jetDataGridViewTextBoxColumn.HeaderText = "Jet";
            this.jetDataGridViewTextBoxColumn.Name = "jetDataGridViewTextBoxColumn";
            this.jetDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvwPlayers);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnCancel);
            this.splitContainer1.Panel2.Controls.Add(this.btnStart);
            this.splitContainer1.Size = new System.Drawing.Size(561, 453);
            this.splitContainer1.SplitterDistance = 375;
            this.splitContainer1.TabIndex = 3;
            // 
            // Lobby
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 453);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Lobby";
            ((System.ComponentModel.ISupportInitialize)(this.dgvwPlayers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.playerBindingSource)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvwPlayers;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.BindingSource playerBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn maxHealthDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn maxAmmoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn jetDataGridViewTextBoxColumn;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}