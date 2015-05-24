using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Lextm.LeXDK;

namespace Lextm.WiseEditor.Gui
{
	/// <summary>
	/// Summary description for UserControl.
	/// </summary>
	public class PEVerifyPage : Lextm.LeXDK.Gui.CustomPage
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.GroupBox groupBox7;
		private System.Windows.Forms.Button btPEVerifyPath;
		private System.Windows.Forms.TextBox txtPEVerifyPath;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label1;

		public PEVerifyPage()
		{
			// This call is required by the Windows.Forms Designer.
			InitializeComponent();
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
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btPEVerifyPath = new System.Windows.Forms.Button();
			this.txtPEVerifyPath = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.groupBox7.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox7
			// 
			this.groupBox7.Controls.Add(this.label1);
			this.groupBox7.Controls.Add(this.btPEVerifyPath);
			this.groupBox7.Controls.Add(this.txtPEVerifyPath);
			this.groupBox7.Controls.Add(this.label9);
			this.groupBox7.Location = new System.Drawing.Point(16, 8);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new System.Drawing.Size(496, 72);
			this.groupBox7.TabIndex = 2;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "PEVerify directory";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(280, 16);
			this.label1.TabIndex = 4;
			this.label1.Text = "PEVerify is part of the .NET SDK or Windows SDK";
			// 
			// btPEVerifyPath
			// 
			this.btPEVerifyPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btPEVerifyPath.Location = new System.Drawing.Point(448, 22);
			this.btPEVerifyPath.Name = "btPEVerifyPath";
			this.btPEVerifyPath.Size = new System.Drawing.Size(24, 16);
			this.btPEVerifyPath.TabIndex = 1;
			this.btPEVerifyPath.Text = "...";
			this.btPEVerifyPath.Click += new System.EventHandler(this.btPEVerifyPath_Click);
			// 
			// txtPEVerifyPath
			// 
			this.txtPEVerifyPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtPEVerifyPath.Location = new System.Drawing.Point(152, 20);
			this.txtPEVerifyPath.Name = "txtPEVerifyPath";
			this.txtPEVerifyPath.Size = new System.Drawing.Size(288, 21);
			this.txtPEVerifyPath.TabIndex = 0;
			this.txtPEVerifyPath.Text = "";
			this.txtPEVerifyPath.TextChanged += new System.EventHandler(this.txtPEVerifyPath_TextChanged);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(16, 20);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(136, 17);
			this.label9.TabIndex = 3;
			this.label9.Text = "Path of peverify.exe:";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// PEVerifyPage
			// 
			this.Controls.Add(this.groupBox7);
			this.Name = "PEVerifyPage";
			this.groupBox7.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		#endregion

		private void btPEVerifyPath_Click(object sender, System.EventArgs e)
		{
			Vista_Api.FolderBrowserDialog _Dlg = new Vista_Api.FolderBrowserDialog();

			_Dlg.SelectedPath = txtPEVerifyPath.Text;

			if (_Dlg.ShowDialog() == DialogResult.OK) {
				txtPEVerifyPath.Text = _Dlg.SelectedPath;
			}
		}
//		private Feature.FileManagerFeature.Preferences
//			options = Feature.FileManagerFeature.getInstance().Options
//			as Feature.FileManagerFeature.Preferences;

		public override void PreferencesToUI() {
			base.PreferencesToUI();
			txtPEVerifyPath.Text = (string)PropertyRegistry.Get("PEVerifyPath");//options.PEVerifyPath;
		}

		public override void UIToPreferences() {
			base.UIToPreferences();
            //options.PEVerifyPath = txtPEVerifyPath.Text;
            PropertyRegistry.Set("PEVerifyPath", txtPEVerifyPath.Text);
        }
		
		private void txtPEVerifyPath_TextChanged(object sender, System.EventArgs e)
		{
			CustomFeatureTool.UpdateUIForPathChecking(this.txtPEVerifyPath.Text,
											Tool.PEVerify.PE_CONSOLE_EXE_NAME,
											this.label9);
		}
	}
}
