namespace Planetes
{
	partial class HUD
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.pbAmmo = new System.Windows.Forms.ProgressBar();
            this.lblHealth = new System.Windows.Forms.Label();
            this.pbHlth = new System.Windows.Forms.ProgressBar();
            this.lblAmmo = new System.Windows.Forms.Label();
            this.lblSpeedX = new System.Windows.Forms.Label();
            this.lblSpeedY = new System.Windows.Forms.Label();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.lblAcceleration = new System.Windows.Forms.Label();
            this.lblAccX = new System.Windows.Forms.Label();
            this.lblAccY = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pbAmmo
            // 
            this.pbAmmo.BackColor = System.Drawing.Color.White;
            this.pbAmmo.ForeColor = System.Drawing.Color.Red;
            this.pbAmmo.Location = new System.Drawing.Point(109, 62);
            this.pbAmmo.Maximum = 150;
            this.pbAmmo.Name = "pbAmmo";
            this.pbAmmo.Size = new System.Drawing.Size(104, 20);
            this.pbAmmo.Step = 1;
            this.pbAmmo.TabIndex = 9;
            // 
            // lblHealth
            // 
            this.lblHealth.AutoSize = true;
            this.lblHealth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lblHealth.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblHealth.Location = new System.Drawing.Point(3, 32);
            this.lblHealth.Name = "lblHealth";
            this.lblHealth.Size = new System.Drawing.Size(100, 20);
            this.lblHealth.TabIndex = 11;
            this.lblHealth.Text = "Health: 1000";
            // 
            // pbHlth
            // 
            this.pbHlth.BackColor = System.Drawing.Color.Red;
            this.pbHlth.ForeColor = System.Drawing.Color.Red;
            this.pbHlth.Location = new System.Drawing.Point(109, 32);
            this.pbHlth.Maximum = 20;
            this.pbHlth.Name = "pbHlth";
            this.pbHlth.Size = new System.Drawing.Size(104, 20);
            this.pbHlth.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbHlth.TabIndex = 10;
            // 
            // lblAmmo
            // 
            this.lblAmmo.AutoSize = true;
            this.lblAmmo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lblAmmo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAmmo.Location = new System.Drawing.Point(4, 62);
            this.lblAmmo.Name = "lblAmmo";
            this.lblAmmo.Size = new System.Drawing.Size(99, 20);
            this.lblAmmo.TabIndex = 12;
            this.lblAmmo.Text = "Ammo: 1000";
            // 
            // lblSpeedX
            // 
            this.lblSpeedX.AutoSize = true;
            this.lblSpeedX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lblSpeedX.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSpeedX.Location = new System.Drawing.Point(92, 93);
            this.lblSpeedX.Name = "lblSpeedX";
            this.lblSpeedX.Size = new System.Drawing.Size(45, 20);
            this.lblSpeedX.TabIndex = 13;
            this.lblSpeedX.Text = "-9.99";
            // 
            // lblSpeedY
            // 
            this.lblSpeedY.AutoSize = true;
            this.lblSpeedY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lblSpeedY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSpeedY.Location = new System.Drawing.Point(168, 93);
            this.lblSpeedY.Name = "lblSpeedY";
            this.lblSpeedY.Size = new System.Drawing.Size(45, 20);
            this.lblSpeedY.TabIndex = 14;
            this.lblSpeedY.Text = "-9.99";
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lblSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSpeed.Location = new System.Drawing.Point(4, 93);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(56, 20);
            this.lblSpeed.TabIndex = 15;
            this.lblSpeed.Text = "Speed";
            // 
            // lblAcceleration
            // 
            this.lblAcceleration.AutoSize = true;
            this.lblAcceleration.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lblAcceleration.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAcceleration.Location = new System.Drawing.Point(23, 123);
            this.lblAcceleration.Name = "lblAcceleration";
            this.lblAcceleration.Size = new System.Drawing.Size(36, 20);
            this.lblAcceleration.TabIndex = 16;
            this.lblAcceleration.Text = "Acc";
            // 
            // lblAccX
            // 
            this.lblAccX.AutoSize = true;
            this.lblAccX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lblAccX.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAccX.Location = new System.Drawing.Point(92, 123);
            this.lblAccX.Name = "lblAccX";
            this.lblAccX.Size = new System.Drawing.Size(45, 20);
            this.lblAccX.TabIndex = 17;
            this.lblAccX.Text = "-9.99";
            // 
            // lblAccY
            // 
            this.lblAccY.AutoSize = true;
            this.lblAccY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lblAccY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAccY.Location = new System.Drawing.Point(168, 123);
            this.lblAccY.Name = "lblAccY";
            this.lblAccY.Size = new System.Drawing.Size(45, 20);
            this.lblAccY.TabIndex = 18;
            this.lblAccY.Text = "-9.99";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(66, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 20);
            this.label1.TabIndex = 19;
            this.label1.Text = "X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(142, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 20);
            this.label2.TabIndex = 20;
            this.label2.Text = "Y";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(142, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 20);
            this.label3.TabIndex = 22;
            this.label3.Text = "Y";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(65, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 20);
            this.label4.TabIndex = 21;
            this.label4.Text = "X";
            // 
            // lblName
            // 
            this.lblName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblName.Location = new System.Drawing.Point(3, 3);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(210, 20);
            this.lblName.TabIndex = 23;
            this.lblName.Text = "Name";
            // 
            // HUD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblAccY);
            this.Controls.Add(this.lblAccX);
            this.Controls.Add(this.lblAcceleration);
            this.Controls.Add(this.lblSpeed);
            this.Controls.Add(this.lblSpeedY);
            this.Controls.Add(this.lblSpeedX);
            this.Controls.Add(this.pbAmmo);
            this.Controls.Add(this.lblHealth);
            this.Controls.Add(this.pbHlth);
            this.Controls.Add(this.lblAmmo);
            this.Name = "HUD";
            this.Size = new System.Drawing.Size(222, 150);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ProgressBar pbAmmo;
		private System.Windows.Forms.Label lblHealth;
		private System.Windows.Forms.ProgressBar pbHlth;
		private System.Windows.Forms.Label lblAmmo;
		private System.Windows.Forms.Label lblSpeedX;
		private System.Windows.Forms.Label lblSpeedY;
		private System.Windows.Forms.Label lblSpeed;
		private System.Windows.Forms.Label lblAcceleration;
		private System.Windows.Forms.Label lblAccX;
		private System.Windows.Forms.Label lblAccY;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblName;
    }
}
