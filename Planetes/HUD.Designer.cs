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
			this.SuspendLayout();
			// 
			// pbAmmo
			// 
			this.pbAmmo.BackColor = System.Drawing.Color.White;
			this.pbAmmo.ForeColor = System.Drawing.Color.Red;
			this.pbAmmo.Location = new System.Drawing.Point(135, 46);
			this.pbAmmo.Maximum = 150;
			this.pbAmmo.Name = "pbAmmo";
			this.pbAmmo.Size = new System.Drawing.Size(202, 30);
			this.pbAmmo.Step = 1;
			this.pbAmmo.TabIndex = 9;
			// 
			// lblHealth
			// 
			this.lblHealth.AutoSize = true;
			this.lblHealth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
			this.lblHealth.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblHealth.Location = new System.Drawing.Point(15, 10);
			this.lblHealth.Name = "lblHealth";
			this.lblHealth.Size = new System.Drawing.Size(56, 20);
			this.lblHealth.TabIndex = 11;
			this.lblHealth.Text = "Health";
			// 
			// pbHlth
			// 
			this.pbHlth.BackColor = System.Drawing.Color.Red;
			this.pbHlth.ForeColor = System.Drawing.Color.Red;
			this.pbHlth.Location = new System.Drawing.Point(135, 10);
			this.pbHlth.Maximum = 20;
			this.pbHlth.Name = "pbHlth";
			this.pbHlth.Size = new System.Drawing.Size(202, 30);
			this.pbHlth.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.pbHlth.TabIndex = 10;
			// 
			// lblAmmo
			// 
			this.lblAmmo.AutoSize = true;
			this.lblAmmo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
			this.lblAmmo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblAmmo.Location = new System.Drawing.Point(15, 46);
			this.lblAmmo.Name = "lblAmmo";
			this.lblAmmo.Size = new System.Drawing.Size(55, 20);
			this.lblAmmo.TabIndex = 12;
			this.lblAmmo.Text = "Ammo";
			// 
			// lblSpeedX
			// 
			this.lblSpeedX.AutoSize = true;
			this.lblSpeedX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
			this.lblSpeedX.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblSpeedX.Location = new System.Drawing.Point(77, 82);
			this.lblSpeedX.Name = "lblSpeedX";
			this.lblSpeedX.Size = new System.Drawing.Size(27, 20);
			this.lblSpeedX.TabIndex = 13;
			this.lblSpeedX.Text = "10";
			// 
			// lblSpeedY
			// 
			this.lblSpeedY.AutoSize = true;
			this.lblSpeedY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
			this.lblSpeedY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblSpeedY.Location = new System.Drawing.Point(142, 82);
			this.lblSpeedY.Name = "lblSpeedY";
			this.lblSpeedY.Size = new System.Drawing.Size(27, 20);
			this.lblSpeedY.TabIndex = 14;
			this.lblSpeedY.Text = "12";
			// 
			// lblSpeed
			// 
			this.lblSpeed.AutoSize = true;
			this.lblSpeed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
			this.lblSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblSpeed.Location = new System.Drawing.Point(15, 82);
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
			this.lblAcceleration.Location = new System.Drawing.Point(15, 110);
			this.lblAcceleration.Name = "lblAcceleration";
			this.lblAcceleration.Size = new System.Drawing.Size(97, 20);
			this.lblAcceleration.TabIndex = 16;
			this.lblAcceleration.Text = "Acceleration";
			// 
			// lblAccX
			// 
			this.lblAccX.AutoSize = true;
			this.lblAccX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
			this.lblAccX.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblAccX.Location = new System.Drawing.Point(118, 110);
			this.lblAccX.Name = "lblAccX";
			this.lblAccX.Size = new System.Drawing.Size(18, 20);
			this.lblAccX.TabIndex = 17;
			this.lblAccX.Text = "1";
			// 
			// lblAccY
			// 
			this.lblAccY.AutoSize = true;
			this.lblAccY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
			this.lblAccY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblAccY.Location = new System.Drawing.Point(142, 110);
			this.lblAccY.Name = "lblAccY";
			this.lblAccY.Size = new System.Drawing.Size(23, 20);
			this.lblAccY.TabIndex = 18;
			this.lblAccY.Text = "-1";
			// 
			// HUD
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
			this.MaximumSize = new System.Drawing.Size(340, 140);
			this.MinimumSize = new System.Drawing.Size(340, 140);
			this.Name = "HUD";
			this.Size = new System.Drawing.Size(340, 140);
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
	}
}
