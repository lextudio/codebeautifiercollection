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
	public class AntPage : CustomPage
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox chkAntShowCompletedMessage;
		private System.Windows.Forms.CheckBox chkWarnForAntUnsavedFiles;
		private System.Windows.Forms.CheckBox chkAntClearMessages;
		private System.Windows.Forms.LinkLabel lnklblNAnt;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.Button btNAntPath;
		private System.Windows.Forms.RadioButton rbAnt;
		private System.Windows.Forms.RadioButton rbNAnt;
		private System.Windows.Forms.TextBox txtAntPath;
		private System.Windows.Forms.Label lblNAntPath;
		private System.Windows.Forms.TextBox txtNAntPath;
		private System.Windows.Forms.Label lblAntPath;
		private System.Windows.Forms.Button btAntPath;
		private System.Windows.Forms.GroupBox groupBox2;

		public AntPage()
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lnklblNAnt = new System.Windows.Forms.LinkLabel();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.btNAntPath = new System.Windows.Forms.Button();
			this.rbAnt = new System.Windows.Forms.RadioButton();
			this.rbNAnt = new System.Windows.Forms.RadioButton();
			this.txtAntPath = new System.Windows.Forms.TextBox();
			this.lblNAntPath = new System.Windows.Forms.Label();
			this.txtNAntPath = new System.Windows.Forms.TextBox();
			this.lblAntPath = new System.Windows.Forms.Label();
			this.btAntPath = new System.Windows.Forms.Button();
			this.chkAntShowCompletedMessage = new System.Windows.Forms.CheckBox();
			this.chkWarnForAntUnsavedFiles = new System.Windows.Forms.CheckBox();
			this.chkAntClearMessages = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.lnklblNAnt);
			this.groupBox1.Controls.Add(this.linkLabel1);
			this.groupBox1.Controls.Add(this.btNAntPath);
			this.groupBox1.Controls.Add(this.rbAnt);
			this.groupBox1.Controls.Add(this.rbNAnt);
			this.groupBox1.Controls.Add(this.txtAntPath);
			this.groupBox1.Controls.Add(this.lblNAntPath);
			this.groupBox1.Controls.Add(this.txtNAntPath);
			this.groupBox1.Controls.Add(this.lblAntPath);
			this.groupBox1.Controls.Add(this.btAntPath);
			this.groupBox1.Location = new System.Drawing.Point(16, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(456, 168);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Ant/NAnt directories";
			// 
			// lnklblNAnt
			// 
			this.lnklblNAnt.Location = new System.Drawing.Point(8, 136);
			this.lnklblNAnt.Name = "lnklblNAnt";
			this.lnklblNAnt.Size = new System.Drawing.Size(144, 16);
			this.lnklblNAnt.TabIndex = 8;
			this.lnklblNAnt.TabStop = true;
			this.lnklblNAnt.Text = "http://ant.apache.org";
			this.lnklblNAnt.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel_LinkClicked);
			// 
			// linkLabel1
			// 
			this.linkLabel1.Location = new System.Drawing.Point(8, 80);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(184, 24);
			this.linkLabel1.TabIndex = 7;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "http://nant.sourceforge.net/";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel_LinkClicked);
			// 
			// btNAntPath
			// 
			this.btNAntPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btNAntPath.Location = new System.Drawing.Point(408, 57);
			this.btNAntPath.Name = "btNAntPath";
			this.btNAntPath.Size = new System.Drawing.Size(24, 16);
			this.btNAntPath.TabIndex = 1;
			this.btNAntPath.Text = "...";
			this.btNAntPath.Click += new System.EventHandler(this.btPath_Click);
			// 
			// rbAnt
			// 
			this.rbAnt.Location = new System.Drawing.Point(128, 16);
			this.rbAnt.Name = "rbAnt";
			this.rbAnt.Size = new System.Drawing.Size(184, 24);
			this.rbAnt.TabIndex = 12;
			this.rbAnt.Text = "Use Ant";
			// 
			// rbNAnt
			// 
			this.rbNAnt.Checked = true;
			this.rbNAnt.Location = new System.Drawing.Point(8, 16);
			this.rbNAnt.Name = "rbNAnt";
			this.rbNAnt.Size = new System.Drawing.Size(96, 24);
			this.rbNAnt.TabIndex = 11;
			this.rbNAnt.TabStop = true;
			this.rbNAnt.Text = "Use NAnt";
			// 
			// txtAntPath
			// 
			this.txtAntPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtAntPath.Location = new System.Drawing.Point(120, 112);
			this.txtAntPath.Name = "txtAntPath";
			this.txtAntPath.Size = new System.Drawing.Size(272, 21);
			this.txtAntPath.TabIndex = 2;
			this.txtAntPath.Text = "";
			this.txtAntPath.TextChanged += new System.EventHandler(this.txtAntPath_TextChanged);
			// 
			// lblNAntPath
			// 
			this.lblNAntPath.Location = new System.Drawing.Point(8, 112);
			this.lblNAntPath.Name = "lblNAntPath";
			this.lblNAntPath.Size = new System.Drawing.Size(104, 24);
			this.lblNAntPath.TabIndex = 9;
			this.lblNAntPath.Text = "Path of ant.exe:";
			// 
			// txtNAntPath
			// 
			this.txtNAntPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtNAntPath.Location = new System.Drawing.Point(120, 56);
			this.txtNAntPath.Name = "txtNAntPath";
			this.txtNAntPath.Size = new System.Drawing.Size(272, 21);
			this.txtNAntPath.TabIndex = 0;
			this.txtNAntPath.Text = "";
			this.txtNAntPath.TextChanged += new System.EventHandler(this.txtNAntPath_TextChanged);
			// 
			// lblAntPath
			// 
			this.lblAntPath.Location = new System.Drawing.Point(8, 53);
			this.lblAntPath.Name = "lblAntPath";
			this.lblAntPath.Size = new System.Drawing.Size(112, 24);
			this.lblAntPath.TabIndex = 10;
			this.lblAntPath.Text = "Path of nant.exe:";
			// 
			// btAntPath
			// 
			this.btAntPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btAntPath.Location = new System.Drawing.Point(408, 112);
			this.btAntPath.Name = "btAntPath";
			this.btAntPath.Size = new System.Drawing.Size(24, 16);
			this.btAntPath.TabIndex = 3;
			this.btAntPath.Text = "...";
			this.btAntPath.Click += new System.EventHandler(this.btPath_Click);
			// 
			// chkAntShowCompletedMessage
			// 
			this.chkAntShowCompletedMessage.Location = new System.Drawing.Point(24, 88);
			this.chkAntShowCompletedMessage.Name = "chkAntShowCompletedMessage";
			this.chkAntShowCompletedMessage.Size = new System.Drawing.Size(304, 24);
			this.chkAntShowCompletedMessage.TabIndex = 6;
			this.chkAntShowCompletedMessage.Text = "Show a message when task run is completed";
			// 
			// chkWarnForAntUnsavedFiles
			// 
			this.chkWarnForAntUnsavedFiles.Location = new System.Drawing.Point(24, 56);
			this.chkWarnForAntUnsavedFiles.Name = "chkWarnForAntUnsavedFiles";
			this.chkWarnForAntUnsavedFiles.Size = new System.Drawing.Size(264, 24);
			this.chkWarnForAntUnsavedFiles.TabIndex = 5;
			this.chkWarnForAntUnsavedFiles.Text = "Warn for unsaved files";
			// 
			// chkAntClearMessages
			// 
			this.chkAntClearMessages.Location = new System.Drawing.Point(24, 24);
			this.chkAntClearMessages.Name = "chkAntClearMessages";
			this.chkAntClearMessages.Size = new System.Drawing.Size(296, 24);
			this.chkAntClearMessages.TabIndex = 4;
			this.chkAntClearMessages.Text = "Clear Message before Running NAnt / Ant";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.chkAntShowCompletedMessage);
			this.groupBox2.Controls.Add(this.chkWarnForAntUnsavedFiles);
			this.groupBox2.Controls.Add(this.chkAntClearMessages);
			this.groupBox2.Location = new System.Drawing.Point(16, 192);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(456, 120);
			this.groupBox2.TabIndex = 5;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Options";
			// 
			// AntPage
			// 
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Name = "AntPage";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		#endregion
		private void LinkLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e) {
			Lextm.Diagnostics.ShellHelper.Execute((sender as LinkLabel).Text);
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
            if (aButton == btAntPath) {
                return txtAntPath;
			} else if (aButton == btNAntPath) {
                return txtNAntPath;
			} else {
				return null;
			}
		}
		
//		private Feature.AntFeature.Preferences preferences =
//			Feature.AntFeature.getInstance().Options as
//			Feature.AntFeature.Preferences;

		/// <summary>
		/// To UI.
		/// </summary>
		public override void PreferencesToUI()
		{
			base.PreferencesToUI();

			chkWarnForAntUnsavedFiles.Checked = (bool)PropertyRegistry.Get("WarnOnRunAntUnsavedFile", false);
			chkAntShowCompletedMessage.Checked = (bool)PropertyRegistry.Get("ShowMessageOnTerminateAnt", true);

			txtNAntPath.Text = (string)PropertyRegistry.Get("NAntPath");
			txtAntPath.Text = (string)PropertyRegistry.Get("AntPath");
			rbAnt.Checked = (bool)PropertyRegistry.Get("UseAnt", false);
			chkAntClearMessages.Checked = (bool)PropertyRegistry.Get("ClearMessagesBeforeRunningAnt", true);
		}
		/// <summary>
		/// From UI.
		/// </summary>
		public override void UIToPreferences()
		{
			base.UIToPreferences();

			PropertyRegistry.Set("WarnOnRunAntUnsavedFile", chkWarnForAntUnsavedFiles.Checked);
			PropertyRegistry.Set("ShowMessageOnTerminateAnt", chkAntShowCompletedMessage.Checked);

			PropertyRegistry.Set("NAntPath", txtNAntPath.Text);
			PropertyRegistry.Set("AntPath", txtAntPath.Text);
			PropertyRegistry.Set("ClearMessagesBeforeRunningAnt", chkAntClearMessages.Checked);
			PropertyRegistry.Set("UseAnt", rbAnt.Checked);
		}
		
		private void txtNAntPath_TextChanged(object sender, System.EventArgs e)
		{
			CustomFeatureTool.UpdateUIForPathChecking(this.txtNAntPath.Text,
											BeWise.SharpBuilderTools.Tools.Ant.NAnt.NANT_CONSOLE_EXE_NAME,
											this.lblNAntPath);
		}
		
		private void txtAntPath_TextChanged(object sender, System.EventArgs e)
		{
			CustomFeatureTool.UpdateUIForPathChecking(this.txtAntPath.Text,
											BeWise.SharpBuilderTools.Tools.Ant.Ant.ANT_CONSOLE_EXE_NAME,
											this.lblAntPath);			
		}
	}
}
