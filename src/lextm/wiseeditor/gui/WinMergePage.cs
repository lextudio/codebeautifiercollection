using System;

using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Lextm.OpenTools;

namespace Lextm.WiseEditor.Gui
{
	/// <summary>
	/// Summary description for UserControl.
	/// </summary>
	public class WinMergePage : Lextm.OpenTools.Gui.CustomPage
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.GroupBox groupBox7;
		private System.Windows.Forms.Button btWinMergePath;
		private System.Windows.Forms.TextBox txtWinMergePath;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.LinkLabel linkLabel8;

		public WinMergePage()
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
			this.btWinMergePath = new System.Windows.Forms.Button();
			this.txtWinMergePath = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.linkLabel8 = new System.Windows.Forms.LinkLabel();
			this.groupBox7.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox7
			// 
			this.groupBox7.Controls.Add(this.btWinMergePath);
			this.groupBox7.Controls.Add(this.txtWinMergePath);
			this.groupBox7.Controls.Add(this.label9);
			this.groupBox7.Controls.Add(this.linkLabel8);
			this.groupBox7.Location = new System.Drawing.Point(16, 8);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new System.Drawing.Size(488, 72);
			this.groupBox7.TabIndex = 1;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "WinMerge directory";
			// 
			// btWinMergePath
			// 
			this.btWinMergePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btWinMergePath.Location = new System.Drawing.Point(448, 22);
			this.btWinMergePath.Name = "btWinMergePath";
			this.btWinMergePath.Size = new System.Drawing.Size(24, 16);
			this.btWinMergePath.TabIndex = 1;
			this.btWinMergePath.Text = "...";
			this.btWinMergePath.Click += new System.EventHandler(this.btPath_Click);
			// 
			// txtWinMergePath
			// 
			this.txtWinMergePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtWinMergePath.Location = new System.Drawing.Point(152, 20);
			this.txtWinMergePath.Name = "txtWinMergePath";
			this.txtWinMergePath.Size = new System.Drawing.Size(288, 21);
			this.txtWinMergePath.TabIndex = 0;
			this.txtWinMergePath.Text = "";
			this.txtWinMergePath.TextChanged += new System.EventHandler(this.txtWinMergePath_TextChanged);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(16, 20);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(136, 17);
			this.label9.TabIndex = 3;
			this.label9.Text = "Path of winmerge.exe:";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// linkLabel8
			// 
			this.linkLabel8.Location = new System.Drawing.Point(16, 48);
			this.linkLabel8.Name = "linkLabel8";
			this.linkLabel8.Size = new System.Drawing.Size(208, 16);
			this.linkLabel8.TabIndex = 2;
			this.linkLabel8.TabStop = true;
			this.linkLabel8.Text = "http://winmerge.sourceforge.net/";
			this.linkLabel8.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel_LinkClicked);
			// 
			// WinMergePage
			// 
			this.Controls.Add(this.groupBox7);
			this.Name = "WinMergePage";
			this.Size = new System.Drawing.Size(576, 296);
			this.groupBox7.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		#endregion

		private void LinkLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e) {
			System.Diagnostics.Process.Start((sender as LinkLabel).Text);
		}
		private void btPath_Click(object sender, System.EventArgs e) {
			TextBox _Txt = GetPathTextBoxFromButton((Button) sender);

			if (_Txt != null) {
                Vista_Api.FolderBrowserDialog _Dlg = new Vista_Api.FolderBrowserDialog();

                _Dlg.SelectedPath = _Txt.Text;

                if (_Dlg.ShowDialog() == DialogResult.OK) {
					_Txt.Text = _Dlg.SelectedPath;
                }
			}
		}
        
		private TextBox GetPathTextBoxFromButton(Button aButton) {
			if (aButton == btWinMergePath) {
				return txtWinMergePath;
            } else {
                return null;
			}
		}

		public override void PreferencesToUI()
		{
			base.PreferencesToUI();
			txtWinMergePath.Text = (string)PropertyRegistry.Get("WinMergePath"); 
		}
		public override void UIToPreferences()
		{
			base.UIToPreferences();

			PropertyRegistry.Set("WinMergePath", txtWinMergePath.Text);
		}
		
		private void txtWinMergePath_TextChanged(object sender, System.EventArgs e)
		{
			CustomFeatureTool.UpdateUIForPathChecking(this.txtWinMergePath.Text,
						  BeWise.SharpBuilderTools.Tools.WinMerge.WinMerge.WIN_MERGE_GUI_EXE_NAME,
						  this.label9);
		}

	}
}
