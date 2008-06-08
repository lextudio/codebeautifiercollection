using System;

using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;
using BeWise.Common.Utils;
using Lextm.OpenTools;
using Lextm.Diagnostics;

namespace Lextm.WiseEditor.Gui
{
	/// <summary>
	/// Summary description for UserControl.
	/// </summary>
	public class NotificationPage : Lextm.OpenTools.Gui.CustomPage
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox chkWarnOnCompileUnsavedFile;
		private System.Windows.Forms.TextBox txtProjectStartupDirectory;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button btDefaultDirPath;

		public NotificationPage()
		{
			// This call is required by the Windows.Forms Designer.
			InitializeComponent();


		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.chkWarnOnCompileUnsavedFile = new System.Windows.Forms.CheckBox();
			this.txtProjectStartupDirectory = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.btDefaultDirPath = new System.Windows.Forms.Button();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.chkWarnOnCompileUnsavedFile);
			this.groupBox2.Controls.Add(this.txtProjectStartupDirectory);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.btDefaultDirPath);
			this.groupBox2.Location = new System.Drawing.Point(16, 8);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(400, 112);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Options";
			// 
			// chkWarnOnCompileUnsavedFile
			// 
			this.chkWarnOnCompileUnsavedFile.Location = new System.Drawing.Point(16, 72);
			this.chkWarnOnCompileUnsavedFile.Name = "chkWarnOnCompileUnsavedFile";
			this.chkWarnOnCompileUnsavedFile.Size = new System.Drawing.Size(216, 24);
			this.chkWarnOnCompileUnsavedFile.TabIndex = 2;
			this.chkWarnOnCompileUnsavedFile.Text = "Warn on compile unsaved file";
			// 
			// txtProjectStartupDirectory
			// 
			this.txtProjectStartupDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtProjectStartupDirectory.Location = new System.Drawing.Point(16, 40);
			this.txtProjectStartupDirectory.Name = "txtProjectStartupDirectory";
			this.txtProjectStartupDirectory.Size = new System.Drawing.Size(328, 21);
			this.txtProjectStartupDirectory.TabIndex = 0;
			this.txtProjectStartupDirectory.TextChanged += new System.EventHandler(this.txtProjectStartupDirectory_TextChanged);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(16, 20);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(192, 32);
			this.label5.TabIndex = 0;
			this.label5.Text = "Default projects directory:";
			// 
			// btDefaultDirPath
			// 
			this.btDefaultDirPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btDefaultDirPath.Location = new System.Drawing.Point(352, 40);
			this.btDefaultDirPath.Name = "btDefaultDirPath";
			this.btDefaultDirPath.Size = new System.Drawing.Size(24, 16);
			this.btDefaultDirPath.TabIndex = 1;
			this.btDefaultDirPath.Text = "...";
			this.btDefaultDirPath.Click += new System.EventHandler(this.btPath_Click);
			// 
			// NotificationPage
			// 
			this.Controls.Add(this.groupBox2);
			this.Name = "NotificationPage";
			this.Size = new System.Drawing.Size(576, 360);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);
		}
		#endregion
//		private static Feature.ExtraFeature.Preferences preferences =
//		    Feature.ExtraFeature.getInstance().Options as
//			Feature.ExtraFeature.Preferences;
//		
		// Reg keys
        private const string REG_KEY_BORLAND_GLOBAL                              = @"\Globals";
		/// <summary>
		/// Preferences to UI.
		/// </summary>
		public override void PreferencesToUI()
		{
		
			LoggingService.EnterMethod();
			base.PreferencesToUI();
			
            RegistryKey _Key = Registry.CurrentUser.OpenSubKey(OtaUtils.IdeRegKey + REG_KEY_BORLAND_GLOBAL);

            if ( _Key != null && Array.IndexOf(_Key.GetValueNames(), @"DefaultProjectsDirectory") > -1) {

                txtProjectStartupDirectory.Text = _Key.GetValue(@"DefaultProjectsDirectory").ToString();
                LoggingService.Info("the text is " + txtProjectStartupDirectory.Text);
            } else {
            	LoggingService.Warn("notification: null key");
            }
            chkWarnOnCompileUnsavedFile.Checked = (bool)PropertyRegistry.Get("WarnOnCompileUnsavedFile", false);//preferences.WarnOnCompileUnsavedFile;
 			LoggingService.LeaveMethod();						
			
		}
		/// <summary>
		/// UI to preferences.
		/// </summary>
		public override void UIToPreferences()
		{
			base.UIToPreferences();
            // Save Project Startup Dir
            if (!string.IsNullOrEmpty(txtProjectStartupDirectory.Text)) {
                RegistryKey _Key = Registry.CurrentUser.OpenSubKey(OtaUtils.IdeRegKey + REG_KEY_BORLAND_GLOBAL, true);

                if ( _Key == null ) {
                    _Key = Registry.CurrentUser.CreateSubKey(OtaUtils.IdeRegKey + REG_KEY_BORLAND_GLOBAL);
                }

                _Key.SetValue(@"DefaultProjectsDirectory", txtProjectStartupDirectory.Text);
            }

            if (fProjectStartupChanged) {
            	Lextm.Windows.Forms.MessageBoxFactory.Info(null, "Change saved", "The new Default projects directory will be available next time you start the application");
            }
            
            //preferences.WarnOnCompileUnsavedFile = chkWarnOnCompileUnsavedFile.Checked;
            PropertyRegistry.Set("WarnOnCompileUnsavedFile", chkWarnOnCompileUnsavedFile.Checked);
		}
		
		private TextBox GetPathTextBoxFromButton(Button aButton) {
			if (aButton == btDefaultDirPath) {
				return txtProjectStartupDirectory;
        	} else {
				return null;
			}
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
		private bool fProjectStartupChanged;
		
        private void txtProjectStartupDirectory_TextChanged(object sender, System.EventArgs e) {
            fProjectStartupChanged = true;
        }		
	}
}
