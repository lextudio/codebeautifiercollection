// this is the indate form.
// Copyright (C) 2006  Lex Y. Li
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
using System;
using System.ComponentModel;
using System.Windows.Forms;
using Lextm.OpenTools;
using Lextm.Utilities.InDate;
using System.Threading;

namespace Lextm.Utilities.Gui {
	/// <summary>
	/// InDateFeature form. Compact edition.
	/// </summary>
	public sealed class FormInDate : System.Windows.Forms.Form {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.NotifyIcon notifyIcon;
		private ToolStrip toolStrip1;
		private ToolStripButton tsbtnHide;
		private ToolStripLabel tslblTip;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripSeparator toolStripSeparator2;
		private MRG.Controls.UI.LoadingCircleToolStripMenuItem tslcProgress;
		/// <summary>
		/// Constructor.
		/// </summary>
		public FormInDate() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			//TODO: calculate the height on both XP and Vista here.            
            Width = 340;
            Height = 52;
			Left = Screen.PrimaryScreen.WorkingArea.Right - Width;
			Top = Screen.PrimaryScreen.WorkingArea.Bottom - Height;
			
			context = new UpdateContext(ShareUtils.CoreVersion);
		}
		
		UpdateContext context;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing) {
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInDate));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showDetailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.worker = new System.ComponentModel.BackgroundWorker();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbtnHide = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tslcProgress = new MRG.Controls.UI.LoadingCircleToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tslblTip = new System.Windows.Forms.ToolStripLabel();
            this.contextMenuStrip.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon.BalloonTipText = "InDate is working";
            this.notifyIcon.BalloonTipTitle = "InDate";
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "InDate is working";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.miShow_Click);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.showDetailsToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(150, 70);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.miAbout_Click);
            // 
            // showDetailsToolStripMenuItem
            // 
            this.showDetailsToolStripMenuItem.Name = "showDetailsToolStripMenuItem";
            this.showDetailsToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.showDetailsToolStripMenuItem.Text = "Show details";
            this.showDetailsToolStripMenuItem.Click += new System.EventHandler(this.miShow_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.miClose_Click);
            // 
            // worker
            // 
            this.worker.WorkerReportsProgress = true;
            this.worker.WorkerSupportsCancellation = true;
            this.worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.WorkerDoWork);
            this.worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.WorkerRunWorkerCompleted);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnHide,
            this.toolStripSeparator1,
            this.tslcProgress,
            this.toolStripSeparator2,
            this.tslblTip});
            this.toolStrip1.Location = new System.Drawing.Point(0, 1);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(334, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbtnHide
            // 
            this.tsbtnHide.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnHide.Name = "tsbtnHide";
            this.tsbtnHide.Size = new System.Drawing.Size(39, 22);
            this.tsbtnHide.Text = "Hide";
            this.tsbtnHide.Click += new System.EventHandler(this.btHide_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tslcProgress
            // 
            this.tslcProgress.BackColor = System.Drawing.Color.Transparent;
            // 
            // tslcProgress
            // 
            this.tslcProgress.LoadingCircleControl.AccessibleName = "tslcProgress";
            this.tslcProgress.LoadingCircleControl.Active = true;
            this.tslcProgress.LoadingCircleControl.BackColor = System.Drawing.Color.Transparent;
            this.tslcProgress.LoadingCircleControl.Color = System.Drawing.Color.DarkGray;
            this.tslcProgress.LoadingCircleControl.InnerCircleRadius = 6;
            this.tslcProgress.LoadingCircleControl.Location = new System.Drawing.Point(45, 1);
            this.tslcProgress.LoadingCircleControl.Name = "tslcProgress";
            this.tslcProgress.LoadingCircleControl.NumberSpoke = 9;
            this.tslcProgress.LoadingCircleControl.OuterCircleRadius = 7;
            this.tslcProgress.LoadingCircleControl.RotationSpeed = 100;
            this.tslcProgress.LoadingCircleControl.Size = new System.Drawing.Size(22, 22);
            this.tslcProgress.LoadingCircleControl.SpokeThickness = 4;
            this.tslcProgress.LoadingCircleControl.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.Firefox;
            this.tslcProgress.LoadingCircleControl.TabIndex = 1;
            this.tslcProgress.LoadingCircleControl.Text = "loadingCircleToolStripMenuItem1";
            this.tslcProgress.Name = "tslcProgress";
            this.tslcProgress.Size = new System.Drawing.Size(22, 22);
            this.tslcProgress.Text = "loadingCircleToolStripMenuItem1";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tslblTip
            // 
            this.tslblTip.Name = "tslblTip";
            this.tslblTip.Size = new System.Drawing.Size(96, 22);
            this.tslblTip.Text = "toolStripLabel1";
            // 
            // FormInDate
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(334, 26);
            this.ControlBox = false;
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormInDate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "InDate";
            this.Load += new System.EventHandler(this.FormInDate3_Load);
            this.Closed += new System.EventHandler(this.FormInDate_Closed);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FormInDate3_Closing);
            this.contextMenuStrip.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showDetailsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
		private System.ComponentModel.BackgroundWorker worker;
		#endregion
		#region NotifyIcon menus

		private void miAbout_Click(object sender, System.EventArgs e) {
			Lextm.Windows.Forms.MessageBoxFactory.Info("InDate is working to keep your CBC up-to-date.");
		}

		private void miShow_Click(object sender, System.EventArgs e) {
			WindowState = FormWindowState.Normal;
		}

		private void miClose_Click(object sender, System.EventArgs e) {
			this.Close();
		}

		#endregion

		private void FormInDate3_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if (worker.IsBusy) {
				if (Lextm.Windows.Forms.MessageBoxFactory.Confirm("InDate is still working. Do you want to abort?")
				    == DialogResult.No)
				{
					e.Cancel = true;
				} else {
					worker.CancelAsync();
				}
			}
		}

		private void FormInDate3_Load(object sender, System.EventArgs e) {
			timer.Interval = 5000;
			timer.Tick += new EventHandler(Tick);
			timer.Start();
			
			ShowTip("Installed version is " + ShareUtils.CoreVersion);
		}

		private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
		private System.Windows.Forms.Timer closeTimer = new System.Windows.Forms.Timer();

		private void Tick(object sender, System.EventArgs e) {
			timer.Enabled = false;
			worker.RunWorkerAsync();
		}
		private void TickClose(object sender, System.EventArgs e) {
			closeTimer.Enabled = false;
			Close();
		}

		private void btHide_Click(object sender, System.EventArgs e) {
			this.Hide();
		}
		
		private void FormInDate_Closed(object sender, System.EventArgs e)
		{
			this.notifyIcon.Visible = false;
		}
		#region Backgroundworker related
		void WorkerDoWork(object sender, DoWorkEventArgs e)
		{
			context.RunForward();
		}
		
		void WorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{			
			ShowTip(context.GetTip());
			if (context.Completed) {
				Stop();
			} 
			else 
			{
				context.Transit();
				worker.RunWorkerAsync();
			}
		}
		#endregion

		void Stop()
		{
			Thread.Sleep(2000);
			closeTimer.Interval = 2000;
			closeTimer.Tick += new EventHandler(TickClose);
			closeTimer.Start();
		}		
        
        private void ShowTip(string tip)
		{
			tslblTip.Text = tip;
			if (WindowState == FormWindowState.Minimized)
			{
				notifyIcon.ShowBalloonTip(1500, "Progress", tip, ToolTipIcon.Info);
			}
		}
	}
}

