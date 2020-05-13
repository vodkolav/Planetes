namespace Planetes
{
    partial class Game
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Game));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.pbPl1Ammo = new System.Windows.Forms.ProgressBar();
			this.pbPl1Hlth = new System.Windows.Forms.ProgressBar();
			this.pbPl2Ammo = new System.Windows.Forms.ProgressBar();
			this.pbPl2Hlth = new System.Windows.Forms.ProgressBar();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
			this.LocalGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.humanVsHumanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.humanVsBotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.NetworkGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.hostToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.joinToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.SystemColors.Window;
			this.pictureBox1.Location = new System.Drawing.Point(0, 28);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(1600, 800);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
			this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
			this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
			// 
			// timer1
			// 
			this.timer1.Interval = 10;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// pbPl1Ammo
			// 
			this.pbPl1Ammo.BackColor = System.Drawing.Color.White;
			this.pbPl1Ammo.ForeColor = System.Drawing.Color.Red;
			this.pbPl1Ammo.Location = new System.Drawing.Point(220, 854);
			this.pbPl1Ammo.Maximum = 150;
			this.pbPl1Ammo.Name = "pbPl1Ammo";
			this.pbPl1Ammo.Size = new System.Drawing.Size(202, 30);
			this.pbPl1Ammo.Step = 1;
			this.pbPl1Ammo.TabIndex = 1;
			this.pbPl1Ammo.Visible = false;
			// 
			// pbPl1Hlth
			// 
			this.pbPl1Hlth.BackColor = System.Drawing.Color.Red;
			this.pbPl1Hlth.ForeColor = System.Drawing.Color.Red;
			this.pbPl1Hlth.Location = new System.Drawing.Point(12, 854);
			this.pbPl1Hlth.Maximum = 20;
			this.pbPl1Hlth.Name = "pbPl1Hlth";
			this.pbPl1Hlth.Size = new System.Drawing.Size(202, 30);
			this.pbPl1Hlth.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.pbPl1Hlth.TabIndex = 2;
			this.pbPl1Hlth.Visible = false;
			// 
			// pbPl2Ammo
			// 
			this.pbPl2Ammo.Location = new System.Drawing.Point(554, 854);
			this.pbPl2Ammo.Maximum = 150;
			this.pbPl2Ammo.Name = "pbPl2Ammo";
			this.pbPl2Ammo.Size = new System.Drawing.Size(202, 30);
			this.pbPl2Ammo.TabIndex = 3;
			this.pbPl2Ammo.Visible = false;
			// 
			// pbPl2Hlth
			// 
			this.pbPl2Hlth.Location = new System.Drawing.Point(762, 854);
			this.pbPl2Hlth.Maximum = 20;
			this.pbPl2Hlth.Name = "pbPl2Hlth";
			this.pbPl2Hlth.Size = new System.Drawing.Size(198, 30);
			this.pbPl2Hlth.TabIndex = 4;
			this.pbPl2Hlth.Visible = false;
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(1600, 25);
			this.toolStrip1.TabIndex = 5;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripDropDownButton1
			// 
			this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LocalGameToolStripMenuItem,
            this.NetworkGameToolStripMenuItem});
			this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
			this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
			this.toolStripDropDownButton1.Size = new System.Drawing.Size(78, 22);
			this.toolStripDropDownButton1.Text = "New Game";
			// 
			// LocalGameToolStripMenuItem
			// 
			this.LocalGameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.humanVsHumanToolStripMenuItem,
            this.humanVsBotToolStripMenuItem});
			this.LocalGameToolStripMenuItem.Name = "LocalGameToolStripMenuItem";
			this.LocalGameToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
			this.LocalGameToolStripMenuItem.Text = "Local Game";
			this.LocalGameToolStripMenuItem.Click += new System.EventHandler(this.LocalGameToolStripMenuItem_Click);
			// 
			// humanVsHumanToolStripMenuItem
			// 
			this.humanVsHumanToolStripMenuItem.Name = "humanVsHumanToolStripMenuItem";
			this.humanVsHumanToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
			this.humanVsHumanToolStripMenuItem.Text = "Human vs. Human";
			this.humanVsHumanToolStripMenuItem.Click += new System.EventHandler(this.humanVsHumanToolStripMenuItem_Click);
			// 
			// humanVsBotToolStripMenuItem
			// 
			this.humanVsBotToolStripMenuItem.Name = "humanVsBotToolStripMenuItem";
			this.humanVsBotToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
			this.humanVsBotToolStripMenuItem.Text = "Human vs. Bot";
			this.humanVsBotToolStripMenuItem.Click += new System.EventHandler(this.humanVsBotToolStripMenuItem_Click);
			// 
			// NetworkGameToolStripMenuItem
			// 
			this.NetworkGameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hostToolStripMenuItem,
            this.joinToolStripMenuItem});
			this.NetworkGameToolStripMenuItem.Name = "NetworkGameToolStripMenuItem";
			this.NetworkGameToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
			this.NetworkGameToolStripMenuItem.Text = "Network Game";
			// 
			// hostToolStripMenuItem
			// 
			this.hostToolStripMenuItem.Name = "hostToolStripMenuItem";
			this.hostToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
			this.hostToolStripMenuItem.Text = "Host";
			this.hostToolStripMenuItem.Click += new System.EventHandler(this.hostToolStripMenuItem_Click_1);
			// 
			// joinToolStripMenuItem
			// 
			this.joinToolStripMenuItem.Name = "joinToolStripMenuItem";
			this.joinToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
			this.joinToolStripMenuItem.Text = "Join";
			this.joinToolStripMenuItem.Click += new System.EventHandler(this.joinToolStripMenuItem_Click_1);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label1.Location = new System.Drawing.Point(70, 831);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 20);
			this.label1.TabIndex = 6;
			this.label1.Text = "Health";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label2.Location = new System.Drawing.Point(642, 831);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(55, 20);
			this.label2.TabIndex = 7;
			this.label2.Text = "Ammo";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label3.Location = new System.Drawing.Point(304, 831);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(55, 20);
			this.label3.TabIndex = 8;
			this.label3.Text = "Ammo";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label4.Location = new System.Drawing.Point(851, 831);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(56, 20);
			this.label4.TabIndex = 9;
			this.label4.Text = "Health";
			// 
			// Game
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(1600, 961);
			this.Controls.Add(this.pbPl2Hlth);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.pbPl1Ammo);
			this.Controls.Add(this.pbPl2Ammo);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.pbPl1Hlth);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.toolStrip1);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Game";
			this.Text = "Planetes";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ProgressBar pbPl1Ammo;
        private System.Windows.Forms.ProgressBar pbPl1Hlth;
        private System.Windows.Forms.ProgressBar pbPl2Ammo;
        private System.Windows.Forms.ProgressBar pbPl2Hlth;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem LocalGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem NetworkGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hostToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem joinToolStripMenuItem;
	private System.Windows.Forms.ToolStripMenuItem humanVsHumanToolStripMenuItem;
	private System.Windows.Forms.ToolStripMenuItem humanVsBotToolStripMenuItem;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
	}
}



