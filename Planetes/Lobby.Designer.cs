namespace Planetes
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
            this.iDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hostDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.healthDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maxHealthDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ammoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maxAmmoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.enemyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jetDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.keyShootDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.gameStateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.actionMappingDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isDeadDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.playerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnStart = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvwPlayers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.playerBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvwPlayers
            // 
            this.dgvwPlayers.AutoGenerateColumns = false;
            this.dgvwPlayers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvwPlayers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iDDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.hostDataGridViewCheckBoxColumn,
            this.healthDataGridViewTextBoxColumn,
            this.maxHealthDataGridViewTextBoxColumn,
            this.ammoDataGridViewTextBoxColumn,
            this.maxAmmoDataGridViewTextBoxColumn,
            this.enemyDataGridViewTextBoxColumn,
            this.jetDataGridViewTextBoxColumn,
            this.keyShootDataGridViewCheckBoxColumn,
            this.gameStateDataGridViewTextBoxColumn,
            this.actionMappingDataGridViewTextBoxColumn,
            this.isDeadDataGridViewCheckBoxColumn});
            this.dgvwPlayers.DataSource = this.playerBindingSource;
            this.dgvwPlayers.Location = new System.Drawing.Point(22, 12);
            this.dgvwPlayers.Name = "dgvwPlayers";
            this.dgvwPlayers.Size = new System.Drawing.Size(1061, 362);
            this.dgvwPlayers.TabIndex = 0;
            // 
            // iDDataGridViewTextBoxColumn
            // 
            this.iDDataGridViewTextBoxColumn.DataPropertyName = "ID";
            this.iDDataGridViewTextBoxColumn.HeaderText = "ID";
            this.iDDataGridViewTextBoxColumn.Name = "iDDataGridViewTextBoxColumn";
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            // 
            // hostDataGridViewCheckBoxColumn
            // 
            this.hostDataGridViewCheckBoxColumn.DataPropertyName = "Host";
            this.hostDataGridViewCheckBoxColumn.HeaderText = "Host";
            this.hostDataGridViewCheckBoxColumn.Name = "hostDataGridViewCheckBoxColumn";
            // 
            // healthDataGridViewTextBoxColumn
            // 
            this.healthDataGridViewTextBoxColumn.DataPropertyName = "Health";
            this.healthDataGridViewTextBoxColumn.HeaderText = "Health";
            this.healthDataGridViewTextBoxColumn.Name = "healthDataGridViewTextBoxColumn";
            // 
            // maxHealthDataGridViewTextBoxColumn
            // 
            this.maxHealthDataGridViewTextBoxColumn.DataPropertyName = "MaxHealth";
            this.maxHealthDataGridViewTextBoxColumn.HeaderText = "MaxHealth";
            this.maxHealthDataGridViewTextBoxColumn.Name = "maxHealthDataGridViewTextBoxColumn";
            // 
            // ammoDataGridViewTextBoxColumn
            // 
            this.ammoDataGridViewTextBoxColumn.DataPropertyName = "Ammo";
            this.ammoDataGridViewTextBoxColumn.HeaderText = "Ammo";
            this.ammoDataGridViewTextBoxColumn.Name = "ammoDataGridViewTextBoxColumn";
            // 
            // maxAmmoDataGridViewTextBoxColumn
            // 
            this.maxAmmoDataGridViewTextBoxColumn.DataPropertyName = "MaxAmmo";
            this.maxAmmoDataGridViewTextBoxColumn.HeaderText = "MaxAmmo";
            this.maxAmmoDataGridViewTextBoxColumn.Name = "maxAmmoDataGridViewTextBoxColumn";
            // 
            // enemyDataGridViewTextBoxColumn
            // 
            this.enemyDataGridViewTextBoxColumn.DataPropertyName = "Enemy";
            this.enemyDataGridViewTextBoxColumn.HeaderText = "Enemy";
            this.enemyDataGridViewTextBoxColumn.Name = "enemyDataGridViewTextBoxColumn";
            // 
            // jetDataGridViewTextBoxColumn
            // 
            this.jetDataGridViewTextBoxColumn.DataPropertyName = "Jet";
            this.jetDataGridViewTextBoxColumn.HeaderText = "Jet";
            this.jetDataGridViewTextBoxColumn.Name = "jetDataGridViewTextBoxColumn";
            // 
            // keyShootDataGridViewCheckBoxColumn
            // 
            this.keyShootDataGridViewCheckBoxColumn.DataPropertyName = "KeyShoot";
            this.keyShootDataGridViewCheckBoxColumn.HeaderText = "KeyShoot";
            this.keyShootDataGridViewCheckBoxColumn.Name = "keyShootDataGridViewCheckBoxColumn";
            // 
            // gameStateDataGridViewTextBoxColumn
            // 
            this.gameStateDataGridViewTextBoxColumn.DataPropertyName = "GameState";
            this.gameStateDataGridViewTextBoxColumn.HeaderText = "GameState";
            this.gameStateDataGridViewTextBoxColumn.Name = "gameStateDataGridViewTextBoxColumn";
            // 
            // actionMappingDataGridViewTextBoxColumn
            // 
            this.actionMappingDataGridViewTextBoxColumn.DataPropertyName = "actionMapping";
            this.actionMappingDataGridViewTextBoxColumn.HeaderText = "actionMapping";
            this.actionMappingDataGridViewTextBoxColumn.Name = "actionMappingDataGridViewTextBoxColumn";
            // 
            // isDeadDataGridViewCheckBoxColumn
            // 
            this.isDeadDataGridViewCheckBoxColumn.DataPropertyName = "isDead";
            this.isDeadDataGridViewCheckBoxColumn.HeaderText = "isDead";
            this.isDeadDataGridViewCheckBoxColumn.Name = "isDeadDataGridViewCheckBoxColumn";
            this.isDeadDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // playerBindingSource
            // 
            this.playerBindingSource.DataSource = typeof(GameObjects.Player);
            this.playerBindingSource.DataSourceChanged += new System.EventHandler(this.playerBindingSource_DataSourceChanged);
            this.playerBindingSource.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.playerBindingSource_ListChanged);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(22, 392);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(187, 392);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "button2";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // Lobby
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1106, 453);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.dgvwPlayers);
            this.Name = "Lobby";
            ((System.ComponentModel.ISupportInitialize)(this.dgvwPlayers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.playerBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvwPlayers;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn hostDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn healthDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn maxHealthDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ammoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn maxAmmoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn enemyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn jetDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn keyShootDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn gameStateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn actionMappingDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isDeadDataGridViewCheckBoxColumn;
        private System.Windows.Forms.BindingSource playerBindingSource;
    }
}