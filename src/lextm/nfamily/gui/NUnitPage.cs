using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Lextm.LeXDK.Gui;
using Lextm.LeXDK;

namespace Lextm.NFamily.Gui
{
	/// <summary>
	/// Summary description for UserControl.
	/// </summary>
	public class NUnitPage : CustomPage
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.GroupBox gbNUnit;
		private System.Windows.Forms.CheckBox chkShowNUnitMessageOnTernminate;
		private System.Windows.Forms.LinkLabel linkLabel4;
		private System.Windows.Forms.TextBox txtNUnitPath;
		private System.Windows.Forms.Label lblNUnit;
		private System.Windows.Forms.Button btNUnitPath;
		private System.Windows.Forms.GroupBox groupBox1;

		public NUnitPage()
		{
			// This call is required by the Windows.Forms Designer.
			InitializeComponent();


		}
        private void LinkLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e) {
            System.Diagnostics.Process.Start((sender as LinkLabel).Text);
		}
			
		private void btPath_Click(object sender, System.EventArgs e) {
            TextBox _Txt = GetPathTextBoxFromButton((Button) sender);

            if (_Txt != null) {
                FolderBrowserDialog _Dlg = new FolderBrowserDialog();

                _Dlg.SelectedPath = _Txt.Text;

                if (_Dlg.ShowDialog() == DialogResult.OK) {
                    _Txt.Text = _Dlg.SelectedPath;
                }
            }
        }
        
        private TextBox GetPathTextBoxFromButton(Button aButton) {
			if (aButton == btNUnitPath) {
                return txtNUnitPath;
            } else {
                return null;
            }
        }
        
		public override void PreferencesToUI()
		{
			base.PreferencesToUI();
			
			txtNUnitPath.Text = (string)PropertyRegistry.Get("NUnitPath");//preferences.NUnitPath;
			chkShowNUnitMessageOnTernminate.Checked =
				//preferences.ShowMessageOnTerminateNUnitConsole;
				(bool)PropertyRegistry.Get("ShowMessageOnTerminateNUnitConsole", true);
		}		
		public override void UIToPreferences()
		{
			base.UIToPreferences();
			
			//preferences.NUnitPath = txtNUnitPath.Text;
            //preferences.ShowMessageOnTerminateNUnitConsole = chkShowNUnitMessageOnTernminate.Checked;
            PropertyRegistry.Set("NUnitPath", txtNUnitPath.Text);
            PropertyRegistry.Set("ShowMessageOnTerminateNUnitConsole", 
                                chkShowNUnitMessageOnTernminate.Checked);
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
			this.gbNUnit = new System.Windows.Forms.GroupBox();
			this.linkLabel4 = new System.Windows.Forms.LinkLabel();
			this.txtNUnitPath = new System.Windows.Forms.TextBox();
			this.lblNUnit = new System.Windows.Forms.Label();
			this.btNUnitPath = new System.Windows.Forms.Button();
			this.chkShowNUnitMessageOnTernminate = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.gbNUnit.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbNUnit
			// 
			this.gbNUnit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gbNUnit.Controls.Add(this.linkLabel4);
			this.gbNUnit.Controls.Add(this.txtNUnitPath);
			this.gbNUnit.Controls.Add(this.lblNUnit);
			this.gbNUnit.Controls.Add(this.btNUnitPath);
			this.gbNUnit.Location = new System.Drawing.Point(16, 8);
			this.gbNUnit.Name = "gbNUnit";
			this.gbNUnit.Size = new System.Drawing.Size(496, 80);
			this.gbNUnit.TabIndex = 1;
			this.gbNUnit.TabStop = false;
			this.gbNUnit.Text = "NUnit directory";
			// 
			// linkLabel4
			// 
			this.linkLabel4.Location = new System.Drawing.Point(16, 48);
			this.linkLabel4.Name = "linkLabel4";
			this.linkLabel4.Size = new System.Drawing.Size(184, 24);
			this.linkLabel4.TabIndex = 2;
			this.linkLabel4.TabStop = true;
			this.linkLabel4.Text = "http://nunit.org/default.htm";
			this.linkLabel4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel_LinkClicked);
			// 
			// txtNUnitPath
			// 
			this.txtNUnitPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtNUnitPath.Location = new System.Drawing.Point(128, 20);
			this.txtNUnitPath.Name = "txtNUnitPath";
			this.txtNUnitPath.Size = new System.Drawing.Size(320, 21);
			this.txtNUnitPath.TabIndex = 0;
			this.txtNUnitPath.Text = "";
			this.txtNUnitPath.TextChanged += new System.EventHandler(this.txtNUnitPath_TextChanged);
			// 
			// lblNUnit
			// 
			this.lblNUnit.Location = new System.Drawing.Point(8, 20);
			this.lblNUnit.Name = "lblNUnit";
			this.lblNUnit.Size = new System.Drawing.Size(128, 17);
			this.lblNUnit.TabIndex = 4;
			this.lblNUnit.Text = "Path of nunit exes:";
			// 
			// btNUnitPath
			// 
			this.btNUnitPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btNUnitPath.Location = new System.Drawing.Point(461, 20);
			this.btNUnitPath.Name = "btNUnitPath";
			this.btNUnitPath.Size = new System.Drawing.Size(24, 16);
			this.btNUnitPath.TabIndex = 1;
			this.btNUnitPath.Text = "...";
			this.btNUnitPath.Click += new System.EventHandler(this.btPath_Click);
			// 
			// chkShowNUnitMessageOnTernminate
			// 
			this.chkShowNUnitMessageOnTernminate.Location = new System.Drawing.Point(24, 24);
			this.chkShowNUnitMessageOnTernminate.Name = "chkShowNUnitMessageOnTernminate";
			this.chkShowNUnitMessageOnTernminate.Size = new System.Drawing.Size(384, 24);
			this.chkShowNUnitMessageOnTernminate.TabIndex = 3;
			this.chkShowNUnitMessageOnTernminate.Text = "Show a message when unit tests execution are completed";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.chkShowNUnitMessageOnTernminate);
			this.groupBox1.Location = new System.Drawing.Point(16, 96);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(496, 64);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Options";
			// 
			// NUnitPage
			// 
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.gbNUnit);
			this.Name = "NUnitPage";
			this.gbNUnit.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		#endregion
		
		private void txtNUnitPath_TextChanged(object sender, System.EventArgs e)
		{
			CustomFeatureTool.UpdateUIForPathChecking(this.txtNUnitPath.Text,
			                                          BeWise.SharpBuilderTools.Tools.NUnit.NUnit.NUNIT_CONSOLE_EXE_NAME +
			                                          ";" + BeWise.SharpBuilderTools.Tools.NUnit.NUnit.NUNIT_GUI_EXE_NAME,
			                                          this.lblNUnit);
		}
	}
}
