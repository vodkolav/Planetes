﻿namespace Planetes
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
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
			this.LocalGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.humanVsHumanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.humanVsBotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.NetworkGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.hostToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.joinToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.hudRight = new Planetes.HUD();
			this.hudLeft = new Planetes.HUD();
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
			// hudRight
			// 
			this.hudRight.Location = new System.Drawing.Point(1280, 831);
			this.hudRight.MaximumSize = new System.Drawing.Size(320, 130);
			this.hudRight.MinimumSize = new System.Drawing.Size(320, 130);
			this.hudRight.Name = "hudRight";
			this.hudRight.Size = new System.Drawing.Size(320, 130);
			this.hudRight.TabIndex = 7;
			this.hudRight.Visible = false;
			// 
			// hudLeft
			// 
			this.hudLeft.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.hudLeft.Location = new System.Drawing.Point(0, 831);
			this.hudLeft.MaximumSize = new System.Drawing.Size(320, 130);
			this.hudLeft.MinimumSize = new System.Drawing.Size(320, 130);
			this.hudLeft.Name = "hudLeft";
			this.hudLeft.Size = new System.Drawing.Size(320, 130);
			this.hudLeft.TabIndex = 6;
			this.hudLeft.Visible = false;
			// 
			// Game
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(1600, 961);
			this.Controls.Add(this.hudRight);
			this.Controls.Add(this.hudLeft);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.toolStrip1);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Name = "Game";
			this.Text = "Planetes";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
			this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Game_PreviewKeyDown);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem LocalGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem NetworkGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hostToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem joinToolStripMenuItem;
	private System.Windows.Forms.ToolStripMenuItem humanVsHumanToolStripMenuItem;
	private System.Windows.Forms.ToolStripMenuItem humanVsBotToolStripMenuItem;
		private HUD hudLeft;
		private HUD hudRight;
	}
}



