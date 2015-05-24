using System;

using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;
using Lextm.OpenTools.Gui;
using Lextm.OpenTools;

namespace Lextm.CodeBeautifierCollection.Gui
{
	/// <summary>
	/// Tab page class for framework.
	/// </summary>
	public class FrameworkPage : CustomPage
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton rbNewMenu;
		private System.Windows.Forms.RadioButton rbUnderTools;
		private System.Windows.Forms.RadioButton rbUnderCnPack;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox cbShowStartUpTime;
		/// <summary>
		/// Framework page.
		/// </summary>
		public FrameworkPage()
		{
			// This call is required by the Windows.Forms Designer.
			InitializeComponent();

            if (ExpertChecker.CNPackInstalled()) {
				this.rbUnderCnPack.Visible = true;
			} else {
				this.rbUnderCnPack.Visible = false;
			}
		}
		/// <summary>
		/// Sets preferences to UI.
		/// </summary>
		public override void PreferencesToUI( ) {
			base.PreferencesToUI();
			// Misc.
			switch ((ParentType)PropertyRegistry.Get("ParentType", ParentType.Default)) {
				case ParentType.Tools:
				rbUnderTools.Checked = true;
				break;
				case ParentType.CNPack:
				rbUnderCnPack.Checked = true;
				break;
				default:
				rbNewMenu.Checked = true;
				break;
			}

            cbShowStartUpTime.Checked = (bool)PropertyRegistry.Get("EntryShowStartUpTime", false);
		}
		
		/// <summary>
		/// Sets UI to preferences.
		/// </summary>
		public override void UIToPreferences( ) {
			base.UIToPreferences();
			// Misc.
			//preferences.ParentType = GetCurrentParentType();
			PropertyRegistry.Set("ParentType", GetCurrentParentType());
            PropertyRegistry.Set("EntryShowStartUpTime", cbShowStartUpTime.Checked);
		}

		private ParentType GetCurrentParentType() {

			if (rbUnderTools.Checked) {
				return ParentType.Tools;
			}
			else if (rbUnderCnPack.Checked) {
				return ParentType.CNPack;
			}
			else {
				return ParentType.Default;
			}
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
			this.rbUnderCnPack = new System.Windows.Forms.RadioButton();
			this.rbUnderTools = new System.Windows.Forms.RadioButton();
			this.rbNewMenu = new System.Windows.Forms.RadioButton();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.cbShowStartUpTime = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.rbUnderCnPack);
			this.groupBox1.Controls.Add(this.rbUnderTools);
			this.groupBox1.Controls.Add(this.rbNewMenu);
			this.groupBox1.Location = new System.Drawing.Point(16, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(184, 152);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Menu positions";
			// 
			// rbUnderCnPack
			// 
			this.rbUnderCnPack.Location = new System.Drawing.Point(24, 120);
			this.rbUnderCnPack.Name = "rbUnderCnPack";
			this.rbUnderCnPack.Size = new System.Drawing.Size(136, 24);
			this.rbUnderCnPack.TabIndex = 2;
			this.rbUnderCnPack.Text = "Under CnPack menu";
			// 
			// rbUnderTools
			// 
			this.rbUnderTools.Location = new System.Drawing.Point(24, 72);
			this.rbUnderTools.Name = "rbUnderTools";
			this.rbUnderTools.Size = new System.Drawing.Size(128, 24);
			this.rbUnderTools.TabIndex = 1;
			this.rbUnderTools.Text = "Under Tools menu";
			// 
			// rbNewMenu
			// 
			this.rbNewMenu.Location = new System.Drawing.Point(24, 24);
			this.rbNewMenu.Name = "rbNewMenu";
			this.rbNewMenu.Size = new System.Drawing.Size(152, 24);
			this.rbNewMenu.TabIndex = 0;
			this.rbNewMenu.Text = "On IDE main menu";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.cbShowStartUpTime);
			this.groupBox2.Location = new System.Drawing.Point(16, 168);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(184, 48);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Startup time";
			// 
			// cbShowStartUpTime
			// 
			this.cbShowStartUpTime.Location = new System.Drawing.Point(16, 17);
			this.cbShowStartUpTime.Name = "cbShowStartUpTime";
			this.cbShowStartUpTime.Size = new System.Drawing.Size(152, 24);
			this.cbShowStartUpTime.TabIndex = 0;
			this.cbShowStartUpTime.Text = "Show in Message Panel";
			// 
			// FrameworkPage
			// 
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Name = "FrameworkPage";
			this.Size = new System.Drawing.Size(384, 224);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		#endregion
		
	}
}
