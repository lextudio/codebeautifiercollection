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
	/// Summary description for UserControl1.
	/// </summary>
	public class NDocPage : CustomPage
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.GroupBox gbNDoc;
		private System.Windows.Forms.LinkLabel linkLabel7;
		private System.Windows.Forms.TextBox txtNDocPath;
		private System.Windows.Forms.Label lblNDoc;
		private System.Windows.Forms.Button btNDocPath;

		public NDocPage()
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
            if (aButton == btNDocPath) {
                return txtNDocPath;
            } else {
                return null;
            }
        }
		public override void PreferencesToUI()
		{
			base.PreferencesToUI();
			
			txtNDocPath.Text = (string)PropertyRegistry.Get("NDocPath");//preferences.NDocPath;
		}		
		public override void UIToPreferences()
		{
			base.UIToPreferences();
			
            //preferences.NDocPath = txtNDocPath.Text;
            PropertyRegistry.Set("NDocPath", txtNDocPath.Text);
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
			this.gbNDoc = new System.Windows.Forms.GroupBox();
			this.linkLabel7 = new System.Windows.Forms.LinkLabel();
			this.txtNDocPath = new System.Windows.Forms.TextBox();
			this.lblNDoc = new System.Windows.Forms.Label();
			this.btNDocPath = new System.Windows.Forms.Button();
			this.gbNDoc.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbNDoc
			// 
			this.gbNDoc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gbNDoc.Controls.Add(this.linkLabel7);
			this.gbNDoc.Controls.Add(this.txtNDocPath);
			this.gbNDoc.Controls.Add(this.lblNDoc);
			this.gbNDoc.Controls.Add(this.btNDocPath);
			this.gbNDoc.Location = new System.Drawing.Point(16, 8);
			this.gbNDoc.Name = "gbNDoc";
			this.gbNDoc.Size = new System.Drawing.Size(512, 80);
			this.gbNDoc.TabIndex = 1;
			this.gbNDoc.TabStop = false;
			this.gbNDoc.Text = "NDoc directory";
			// 
			// linkLabel7
			// 
			this.linkLabel7.Location = new System.Drawing.Point(8, 48);
			this.linkLabel7.Name = "linkLabel7";
			this.linkLabel7.Size = new System.Drawing.Size(208, 16);
			this.linkLabel7.TabIndex = 2;
			this.linkLabel7.TabStop = true;
			this.linkLabel7.Text = "http://ndoc.sourceforge.net/";
			this.linkLabel7.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel_LinkClicked);
			// 
			// txtNDocPath
			// 
			this.txtNDocPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtNDocPath.Location = new System.Drawing.Point(120, 20);
			this.txtNDocPath.Name = "txtNDocPath";
			this.txtNDocPath.Size = new System.Drawing.Size(349, 21);
			this.txtNDocPath.TabIndex = 0;
			this.txtNDocPath.Text = "";
			this.txtNDocPath.TextChanged += new System.EventHandler(this.txtNDocPath_TextChanged);
			// 
			// lblNDoc
			// 
			this.lblNDoc.Location = new System.Drawing.Point(8, 20);
			this.lblNDoc.Name = "lblNDoc";
			this.lblNDoc.Size = new System.Drawing.Size(120, 17);
			this.lblNDoc.TabIndex = 3;
			this.lblNDoc.Text = "Path of ndoc exes:";
			// 
			// btNDocPath
			// 
			this.btNDocPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btNDocPath.Location = new System.Drawing.Point(477, 20);
			this.btNDocPath.Name = "btNDocPath";
			this.btNDocPath.Size = new System.Drawing.Size(24, 16);
			this.btNDocPath.TabIndex = 1;
			this.btNDocPath.Text = "...";
			this.btNDocPath.Click += new System.EventHandler(this.btPath_Click);
			// 
			// NDocPage
			// 
			this.Controls.Add(this.gbNDoc);
			this.Name = "NDocPage";
			this.gbNDoc.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		#endregion
		
		private void txtNDocPath_TextChanged(object sender, System.EventArgs e)
		{
			CustomFeatureTool.UpdateUIForPathChecking(this.txtNDocPath.Text,
			                                          BeWise.SharpBuilderTools.Tools.NDoc.NDoc.NDOC_CONSOLE_EXE_NAME + 
			                                          ";" + BeWise.SharpBuilderTools.Tools.NDoc.NDoc.NDOC_GUI_EXE_NAME,
			                                          this.lblNDoc);
		}
	}
}
