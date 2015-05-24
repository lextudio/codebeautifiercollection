using System;

using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;
using Lextm.OpenTools.Gui;
using Lextm.OpenTools;

namespace Lextm.Utilities.Gui
{
	/// <summary>
	/// Tab page class for indate.
	/// </summary>
	public class InDatePage : CustomPage
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox cbIncludeRC;
		private System.Windows.Forms.CheckBox cbAutoInstall;
		private System.Windows.Forms.CheckBox cbCheckAtStartup;
		/// <summary>
		/// InDateFeature page.
		/// </summary>
		public InDatePage()
		{
			// This call is required by the Windows.Forms Designer.
			InitializeComponent();

		}
		/// <summary>
		/// Sets preferences to UI.
		/// </summary>
		public override void PreferencesToUI( ) {
			base.PreferencesToUI();
			// Misc.
			cbCheckAtStartup.Checked = (bool)PropertyRegistry.Get("InDateCheckAtStartup", true);
			cbIncludeRC.Checked = (bool)PropertyRegistry.Get("InDateIncludeRC", false);//preferences.IncludeRC;
			cbAutoInstall.Checked = (bool)PropertyRegistry.Get("InDateAutoInstall", false);//preferences.AutoInstall;
		}
		
		/// <summary>
		/// Sets UI to preferences.
		/// </summary>
		public override void UIToPreferences( ) {
			base.UIToPreferences();
			// Misc.
			PropertyRegistry.Set("InDateCheckAtStartup", cbCheckAtStartup.Checked);
			PropertyRegistry.Set("InDateIncludeRC", cbIncludeRC.Checked);
			PropertyRegistry.Set("InDateAutoInstall", cbAutoInstall.Checked);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cbAutoInstall = new System.Windows.Forms.CheckBox();
			this.cbIncludeRC = new System.Windows.Forms.CheckBox();
			this.cbCheckAtStartup = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.cbCheckAtStartup);
			this.groupBox1.Controls.Add(this.cbAutoInstall);
			this.groupBox1.Controls.Add(this.cbIncludeRC);
			this.groupBox1.Location = new System.Drawing.Point(16, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(296, 120);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Options";
			// 
			// cbAutoInstall
			// 
			this.cbAutoInstall.Location = new System.Drawing.Point(24, 80);
			this.cbAutoInstall.Name = "cbAutoInstall";
			this.cbAutoInstall.Size = new System.Drawing.Size(184, 24);
			this.cbAutoInstall.TabIndex = 1;
			this.cbAutoInstall.Text = "Auto download and install";
			// 
			// cbIncludeRC
			// 
			this.cbIncludeRC.Location = new System.Drawing.Point(24, 52);
			this.cbIncludeRC.Name = "cbIncludeRC";
			this.cbIncludeRC.Size = new System.Drawing.Size(184, 24);
			this.cbIncludeRC.TabIndex = 0;
			this.cbIncludeRC.Text = "Include unstable versions";
			// 
			// cbCheckAtStartup
			// 
			this.cbCheckAtStartup.Location = new System.Drawing.Point(24, 24);
			this.cbCheckAtStartup.Name = "cbCheckAtStartup";
			this.cbCheckAtStartup.Size = new System.Drawing.Size(240, 24);
			this.cbCheckAtStartup.TabIndex = 2;
			this.cbCheckAtStartup.Text = "Check for updates when IDE starts";
			// 
			// InDatePage
			// 
			this.Controls.Add(this.groupBox1);
			this.Name = "InDatePage";
			this.Size = new System.Drawing.Size(384, 224);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		#endregion
		
	}
}
