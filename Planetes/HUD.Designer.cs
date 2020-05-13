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
			this.label1 = new System.Windows.Forms.Label();
			this.pbHlth = new System.Windows.Forms.ProgressBar();
			this.label3 = new System.Windows.Forms.Label();
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
			this.pbAmmo.Location = new System.Drawing.Point(0, 79);
			this.pbAmmo.Maximum = 150;
			this.pbAmmo.Name = "pbAmmo";
			this.pbAmmo.Size = new System.Drawing.Size(202, 30);
			this.pbAmmo.Step = 1;
			this.pbAmmo.TabIndex = 9;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label1.Location = new System.Drawing.Point(-1, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 20);
			this.label1.TabIndex = 11;
			this.label1.Text = "Health";
			// 
			// pbHlth
			// 
			this.pbHlth.BackColor = System.Drawing.Color.Red;
			this.pbHlth.ForeColor = System.Drawing.Color.Red;
			this.pbHlth.Location = new System.Drawing.Point(0, 23);
			this.pbHlth.Maximum = 20;
			this.pbHlth.Name = "pbHlth";
			this.pbHlth.Size = new System.Drawing.Size(202, 30);
			this.pbHlth.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.pbHlth.TabIndex = 10;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label3.Location = new System.Drawing.Point(-1, 56);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(55, 20);
			this.label3.TabIndex = 12;
			this.label3.Text = "Ammo";
			// 
			// lblSpeedX
			// 
			this.lblSpeedX.AutoSize = true;
			this.lblSpeedX.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblSpeedX.Location = new System.Drawing.Point(208, 23);
			this.lblSpeedX.Name = "lblSpeedX";
			this.lblSpeedX.Size = new System.Drawing.Size(27, 20);
			this.lblSpeedX.TabIndex = 13;
			this.lblSpeedX.Text = "10";
			// 
			// lblSpeedY
			// 
			this.lblSpeedY.AutoSize = true;
			this.lblSpeedY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblSpeedY.Location = new System.Drawing.Point(257, 23);
			this.lblSpeedY.Name = "lblSpeedY";
			this.lblSpeedY.Size = new System.Drawing.Size(27, 20);
			this.lblSpeedY.TabIndex = 14;
			this.lblSpeedY.Text = "12";
			// 
			// lblSpeed
			// 
			this.lblSpeed.AutoSize = true;
			this.lblSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblSpeed.Location = new System.Drawing.Point(208, 3);
			this.lblSpeed.Name = "lblSpeed";
			this.lblSpeed.Size = new System.Drawing.Size(56, 20);
			this.lblSpeed.TabIndex = 15;
			this.lblSpeed.Text = "Speed";
			// 
			// lblAcceleration
			// 
			this.lblAcceleration.AutoSize = true;
			this.lblAcceleration.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblAcceleration.Location = new System.Drawing.Point(208, 72);
			this.lblAcceleration.Name = "lblAcceleration";
			this.lblAcceleration.Size = new System.Drawing.Size(97, 20);
			this.lblAcceleration.TabIndex = 16;
			this.lblAcceleration.Text = "Acceleration";
			// 
			// lblAccX
			// 
			this.lblAccX.AutoSize = true;
			this.lblAccX.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblAccX.Location = new System.Drawing.Point(208, 92);
			this.lblAccX.Name = "lblAccX";
			this.lblAccX.Size = new System.Drawing.Size(18, 20);
			this.lblAccX.TabIndex = 17;
			this.lblAccX.Text = "1";
			// 
			// lblAccY
			// 
			this.lblAccY.AutoSize = true;
			this.lblAccY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblAccY.Location = new System.Drawing.Point(257, 92);
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
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pbHlth);
			this.Controls.Add(this.label3);
			this.MaximumSize = new System.Drawing.Size(320, 130);
			this.MinimumSize = new System.Drawing.Size(320, 130);
			this.Name = "HUD";
			this.Size = new System.Drawing.Size(320, 130);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ProgressBar pbAmmo;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ProgressBar pbHlth;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblSpeedX;
		private System.Windows.Forms.Label lblSpeedY;
		private System.Windows.Forms.Label lblSpeed;
		private System.Windows.Forms.Label lblAcceleration;
		private System.Windows.Forms.Label lblAccX;
		private System.Windows.Forms.Label lblAccY;
	}
}
