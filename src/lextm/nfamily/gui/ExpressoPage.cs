using System;

using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Lextm.OpenTools.Gui;
using Lextm.OpenTools;

namespace Lextm.NFamily.Gui
{
	/// <summary>
	/// Summary description for UserControl.
	/// </summary>
	public class ExpressoPage : CustomPage
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.LinkLabel linkLabel5;
		private System.Windows.Forms.TextBox txtExpressoPath;
		private System.Windows.Forms.Label lblNExpresso;
		private System.Windows.Forms.Button btExpressoPath;

		public ExpressoPage()
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
                Vista_Api.FolderBrowserDialog _Dlg = new Vista_Api.FolderBrowserDialog();

                _Dlg.SelectedPath = _Txt.Text;

                if (_Dlg.ShowDialog() == DialogResult.OK) {
                    _Txt.Text = _Dlg.SelectedPath;
                }
            }
        }
        private TextBox GetPathTextBoxFromButton(Button aButton) {
			if (aButton == btExpressoPath) {
                return txtExpressoPath;
            } else {
                return null;
            }
		}

//		private Feature.ExpressoFeature.Preferences preferences =
//			Feature.ExpressoFeature.getInstance().Options as
//			Feature.ExpressoFeature.Preferences;

		public override void PreferencesToUI() {
			base.PreferencesToUI();
			txtExpressoPath.Text = (string)PropertyRegistry.Get("ExpressoPath");//preferences.ExpressoPath;
		}

		public override void UIToPreferences() {
			base.UIToPreferences();
            //preferences.ExpressoPath = txtExpressoPath.Text;
            PropertyRegistry.Set("ExpressoPath", txtExpressoPath.Text);
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
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.linkLabel5 = new System.Windows.Forms.LinkLabel();
			this.txtExpressoPath = new System.Windows.Forms.TextBox();
			this.lblNExpresso = new System.Windows.Forms.Label();
			this.btExpressoPath = new System.Windows.Forms.Button();
			this.groupBox4.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox4
			// 
			this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox4.Controls.Add(this.linkLabel5);
			this.groupBox4.Controls.Add(this.txtExpressoPath);
			this.groupBox4.Controls.Add(this.lblNExpresso);
			this.groupBox4.Controls.Add(this.btExpressoPath);
			this.groupBox4.Location = new System.Drawing.Point(16, 8);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(496, 80);
			this.groupBox4.TabIndex = 1;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Expresso directory";
			// 
			// linkLabel5
			// 
			this.linkLabel5.Location = new System.Drawing.Point(8, 48);
			this.linkLabel5.Name = "linkLabel5";
			this.linkLabel5.Size = new System.Drawing.Size(160, 16);
			this.linkLabel5.TabIndex = 2;
			this.linkLabel5.TabStop = true;
			this.linkLabel5.Text = "http://www.ultrapico.com";
			this.linkLabel5.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel_LinkClicked);
			// 
			// txtExpressoPath
			// 
			this.txtExpressoPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtExpressoPath.Location = new System.Drawing.Point(152, 20);
			this.txtExpressoPath.Name = "txtExpressoPath";
			this.txtExpressoPath.Size = new System.Drawing.Size(296, 21);
			this.txtExpressoPath.TabIndex = 0;
			this.txtExpressoPath.Text = "";
			this.txtExpressoPath.TextChanged += new System.EventHandler(this.txtExpressoPath_TextChanged);
			// 
			// lblNExpresso
			// 
			this.lblNExpresso.Location = new System.Drawing.Point(8, 20);
			this.lblNExpresso.Name = "lblNExpresso";
			this.lblNExpresso.Size = new System.Drawing.Size(144, 17);
			this.lblNExpresso.TabIndex = 3;
			this.lblNExpresso.Text = "Path of Expresso.exe:";
			// 
			// btExpressoPath
			// 
			this.btExpressoPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btExpressoPath.Location = new System.Drawing.Point(461, 20);
			this.btExpressoPath.Name = "btExpressoPath";
			this.btExpressoPath.Size = new System.Drawing.Size(24, 16);
			this.btExpressoPath.TabIndex = 1;
			this.btExpressoPath.Text = "...";
			this.btExpressoPath.Click += new System.EventHandler(this.btPath_Click);
			// 
			// ExpressoPage
			// 
			this.Controls.Add(this.groupBox4);
			this.Name = "ExpressoPage";
			this.groupBox4.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		#endregion
		
		private void txtExpressoPath_TextChanged(object sender, System.EventArgs e)
		{
			CustomFeatureTool.UpdateUIForPathChecking(this.txtExpressoPath.Text,
											BeWise.SharpBuilderTools.Tools.Expresso.Expresso.EXPRESSO_GUI_EXE_NAME,
											this.lblNExpresso);			
		}
	}
}
