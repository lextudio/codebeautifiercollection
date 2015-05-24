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
	public class FxCopPage : CustomPage
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.LinkLabel linkLabel6;
		private System.Windows.Forms.TextBox txtFxCopPath;
		private System.Windows.Forms.Label lbllabel7;
		private System.Windows.Forms.Button btFxCopPath;

		public FxCopPage()
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
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.linkLabel6 = new System.Windows.Forms.LinkLabel();
			this.txtFxCopPath = new System.Windows.Forms.TextBox();
			this.lbllabel7 = new System.Windows.Forms.Label();
			this.btFxCopPath = new System.Windows.Forms.Button();
			this.groupBox5.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox5
			// 
			this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox5.Controls.Add(this.linkLabel6);
			this.groupBox5.Controls.Add(this.txtFxCopPath);
			this.groupBox5.Controls.Add(this.lbllabel7);
			this.groupBox5.Controls.Add(this.btFxCopPath);
			this.groupBox5.Location = new System.Drawing.Point(16, 8);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(504, 80);
			this.groupBox5.TabIndex = 1;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "FxCop directory";
			// 
			// linkLabel6
			// 
			this.linkLabel6.Location = new System.Drawing.Point(8, 48);
			this.linkLabel6.Name = "linkLabel6";
			this.linkLabel6.Size = new System.Drawing.Size(248, 16);
			this.linkLabel6.TabIndex = 2;
			this.linkLabel6.TabStop = true;
			this.linkLabel6.Text = "http://www.gotdotnet.com/team/fxcop/";
			this.linkLabel6.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel_LinkClicked);
			// 
			// txtFxCopPath
			// 
			this.txtFxCopPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFxCopPath.Location = new System.Drawing.Point(128, 20);
			this.txtFxCopPath.Name = "txtFxCopPath";
			this.txtFxCopPath.Size = new System.Drawing.Size(328, 21);
			this.txtFxCopPath.TabIndex = 0;
			this.txtFxCopPath.Text = "";
			this.txtFxCopPath.TextChanged += new System.EventHandler(this.txtFxCopPath_TextChanged);
			// 
			// lbllabel7
			// 
			this.lbllabel7.Location = new System.Drawing.Point(8, 20);
			this.lbllabel7.Name = "lbllabel7";
			this.lbllabel7.Size = new System.Drawing.Size(120, 17);
			this.lbllabel7.TabIndex = 3;
			this.lbllabel7.Text = "Path of fxcop.exe:";
			// 
			// btFxCopPath
			// 
			this.btFxCopPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btFxCopPath.Location = new System.Drawing.Point(469, 20);
			this.btFxCopPath.Name = "btFxCopPath";
			this.btFxCopPath.Size = new System.Drawing.Size(24, 16);
			this.btFxCopPath.TabIndex = 1;
			this.btFxCopPath.Text = "...";
			this.btFxCopPath.Click += new System.EventHandler(this.btPath_Click);
			// 
			// FxCopPage
			// 
			this.Controls.Add(this.groupBox5);
			this.Name = "FxCopPage";
			this.groupBox5.ResumeLayout(false);
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
            if (aButton == btFxCopPath) {
                return txtFxCopPath;
            } else {
                return null;
            }
		}

//		private Feature.FxCopFeature.Preferences preferences =
//			Feature.FxCopFeature.getInstance().Options as
//			Feature.FxCopFeature.Preferences;

		public override void PreferencesToUI()
		{
			base.PreferencesToUI();
			
			txtFxCopPath.Text = (string)PropertyRegistry.Get("FxCopPath"); //preferences.FxCopPath;
		}		
		public override void UIToPreferences()
		{
			base.UIToPreferences();
			
			//preferences.FxCopPath = txtFxCopPath.Text;
            PropertyRegistry.Set("FxCopPath", txtFxCopPath.Text);
		}
		
		private void txtFxCopPath_TextChanged(object sender, System.EventArgs e)
		{
			CustomFeatureTool.UpdateUIForPathChecking(this.txtFxCopPath.Text,
			                                          BeWise.SharpBuilderTools.Tools.FxCop.FxCop.FXCOP_CONSOLE_EXE_NAME +
			                                          ";" + BeWise.SharpBuilderTools.Tools.FxCop.FxCop.FXCOP_GUI_EXE_NAME,
			                                          lbllabel7);
		}

	}
}
