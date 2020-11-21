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
            this.pbxWorld = new System.Windows.Forms.PictureBox();
            this.timerDraw = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.LocalGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.humanVsHumanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.humanVsBotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.botVsBotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NetworkGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hostToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.joinToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.flpOtherPlayers = new System.Windows.Forms.FlowLayoutPanel();
            this.hudLeft = new Planetes.HUD();
            ((System.ComponentModel.ISupportInitialize)(this.pbxWorld)).BeginInit();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbxWorld
            // 
            this.pbxWorld.BackColor = System.Drawing.SystemColors.Window;
            this.pbxWorld.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbxWorld.Location = new System.Drawing.Point(0, 0);
            this.pbxWorld.Name = "pbxWorld";
            this.pbxWorld.Size = new System.Drawing.Size(1734, 820);
            this.pbxWorld.TabIndex = 0;
            this.pbxWorld.TabStop = false;
            this.pbxWorld.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbxWorld_MouseDown);
            this.pbxWorld.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbxWorld_MouseMove);
            this.pbxWorld.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbxWorld_MouseUp);
            // 
            // timerDraw
            // 
            this.timerDraw.Interval = 20;
            this.timerDraw.Tick += new System.EventHandler(this.timerDraw_Tick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1734, 25);
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
            this.humanVsBotToolStripMenuItem,
            this.botVsBotToolStripMenuItem});
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
            // botVsBotToolStripMenuItem
            // 
            this.botVsBotToolStripMenuItem.Name = "botVsBotToolStripMenuItem";
            this.botVsBotToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.botVsBotToolStripMenuItem.Text = "Bot vs. Bot";
            this.botVsBotToolStripMenuItem.Click += new System.EventHandler(this.botVsBotToolStripMenuItem_Click);
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
            this.joinToolStripMenuItem.Click += new System.EventHandler(this.JoinToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pbxWorld);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1734, 936);
            this.splitContainer1.SplitterDistance = 820;
            this.splitContainer1.TabIndex = 8;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.hudLeft);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.flpOtherPlayers);
            this.splitContainer2.Size = new System.Drawing.Size(1734, 112);
            this.splitContainer2.SplitterDistance = 221;
            this.splitContainer2.TabIndex = 8;
            // 
            // flpOtherPlayers
            // 
            this.flpOtherPlayers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpOtherPlayers.Location = new System.Drawing.Point(0, 0);
            this.flpOtherPlayers.Name = "flpOtherPlayers";
            this.flpOtherPlayers.Size = new System.Drawing.Size(1509, 112);
            this.flpOtherPlayers.TabIndex = 0;
            // 
            // hudLeft
            // 
            this.hudLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hudLeft.Location = new System.Drawing.Point(0, 0);
            this.hudLeft.Name = "hudLeft";
            this.hudLeft.Size = new System.Drawing.Size(221, 112);
            this.hudLeft.TabIndex = 6;
            this.hudLeft.Visible = false;
            // 
            // Game
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1734, 961);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "Game";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Game_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Game_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Game_KeyUp);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Game_PreviewKeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pbxWorld)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbxWorld;
        private System.Windows.Forms.Timer timerDraw;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem LocalGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem NetworkGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hostToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem joinToolStripMenuItem;
	    private System.Windows.Forms.ToolStripMenuItem humanVsHumanToolStripMenuItem;
	    private System.Windows.Forms.ToolStripMenuItem humanVsBotToolStripMenuItem;
		private HUD hudLeft;
		private System.Windows.Forms.ToolStripMenuItem botVsBotToolStripMenuItem;
		private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.FlowLayoutPanel flpOtherPlayers;
    }
}



