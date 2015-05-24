// This is JCF tab page.
// Copyright (C) 2006-2007  Lex Y. Li
// 
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
using System;
using System.Windows.Forms;
using Lextm.IO;
using Lextm.OpenTools;
using Lextm.Windows.Forms;
using System.Globalization;

namespace Lextm.CodeBeautifiers.Gui
{
    /// <summary>
    /// Tab page for JCF.
    /// </summary>
    public class JcfPage : Lextm.OpenTools.Gui.CustomPage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.RadioButton rbUserDefined;
        private System.Windows.Forms.RadioButton rbJCommandLine;
        private System.Windows.Forms.RadioButton rbJDefault;
        private System.Windows.Forms.GroupBox gbJcfCommandLine;
        private System.Windows.Forms.TextBox txtJcfCommandLine;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        /// <summary>
        /// Constructor.
        /// </summary>
        public JcfPage()
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
            this.rbUserDefined = new System.Windows.Forms.RadioButton();
            this.rbJCommandLine = new System.Windows.Forms.RadioButton();
            this.rbJDefault = new System.Windows.Forms.RadioButton();
            this.gbJcfCommandLine = new System.Windows.Forms.GroupBox();
            this.txtJcfCommandLine = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnConfigure = new System.Windows.Forms.Button();
            this.gbCustomStyle = new System.Windows.Forms.GroupBox();
            this.gbJcfCommandLine.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbCustomStyle.SuspendLayout();
            this.SuspendLayout();
            // 
            // rbUserDefined
            // 
            this.rbUserDefined.Location = new System.Drawing.Point(184, 24);
            this.rbUserDefined.Name = "rbUserDefined";
            this.rbUserDefined.Size = new System.Drawing.Size(95, 24);
            this.rbUserDefined.TabIndex = 23;
            this.rbUserDefined.Text = "Custom Style";
            this.rbUserDefined.CheckedChanged += new System.EventHandler(this.rbJcf_CheckedChanged);
            // 
            // rbJCommandLine
            // 
            this.rbJCommandLine.Location = new System.Drawing.Point(83, 24);
            this.rbJCommandLine.Name = "rbJCommandLine";
            this.rbJCommandLine.Size = new System.Drawing.Size(104, 24);
            this.rbJCommandLine.TabIndex = 22;
            this.rbJCommandLine.Text = "Command Line";
            this.rbJCommandLine.CheckedChanged += new System.EventHandler(this.rbJcf_CheckedChanged);
            // 
            // rbJDefault
            // 
            this.rbJDefault.Checked = true;
            this.rbJDefault.Location = new System.Drawing.Point(17, 24);
            this.rbJDefault.Name = "rbJDefault";
            this.rbJDefault.Size = new System.Drawing.Size(80, 24);
            this.rbJDefault.TabIndex = 21;
            this.rbJDefault.TabStop = true;
            this.rbJDefault.Text = "Borland";
            this.rbJDefault.CheckedChanged += new System.EventHandler(this.rbJcf_CheckedChanged);
            // 
            // gbJcfCommandLine
            // 
            this.gbJcfCommandLine.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbJcfCommandLine.Controls.Add(this.txtJcfCommandLine);
            this.gbJcfCommandLine.Controls.Add(this.label4);
            this.gbJcfCommandLine.Location = new System.Drawing.Point(16, 74);
            this.gbJcfCommandLine.Name = "gbJcfCommandLine";
            this.gbJcfCommandLine.Size = new System.Drawing.Size(528, 92);
            this.gbJcfCommandLine.TabIndex = 25;
            this.gbJcfCommandLine.TabStop = false;
            this.gbJcfCommandLine.Text = "Command line";
            // 
            // txtJcfCommandLine
            // 
            this.txtJcfCommandLine.Location = new System.Drawing.Point(128, 32);
            this.txtJcfCommandLine.Name = "txtJcfCommandLine";
            this.txtJcfCommandLine.Size = new System.Drawing.Size(368, 21);
            this.txtJcfCommandLine.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(37, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 23);
            this.label4.TabIndex = 1;
            this.label4.Text = "Command line:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.rbUserDefined);
            this.groupBox1.Controls.Add(this.rbJCommandLine);
            this.groupBox1.Controls.Add(this.rbJDefault);
            this.groupBox1.Location = new System.Drawing.Point(16, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(528, 63);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Coding styles";
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(199, 34);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 26;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.BtnExportClick);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(118, 34);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 25;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImportClick);
            // 
            // btnConfigure
            // 
            this.btnConfigure.Location = new System.Drawing.Point(37, 34);
            this.btnConfigure.Name = "btnConfigure";
            this.btnConfigure.Size = new System.Drawing.Size(75, 23);
            this.btnConfigure.TabIndex = 24;
            this.btnConfigure.Text = "Configure";
            this.btnConfigure.UseVisualStyleBackColor = true;
            this.btnConfigure.Click += new System.EventHandler(this.BtnConfigureClick);
            // 
            // gbCustomStyle
            // 
            this.gbCustomStyle.Controls.Add(this.btnExport);
            this.gbCustomStyle.Controls.Add(this.btnConfigure);
            this.gbCustomStyle.Controls.Add(this.btnImport);
            this.gbCustomStyle.Location = new System.Drawing.Point(16, 77);
            this.gbCustomStyle.Name = "gbCustomStyle";
            this.gbCustomStyle.Size = new System.Drawing.Size(528, 89);
            this.gbCustomStyle.TabIndex = 2;
            this.gbCustomStyle.TabStop = false;
            this.gbCustomStyle.Text = "Custom Style";
            // 
            // JcfPage
            // 
            this.Controls.Add(this.gbJcfCommandLine);
            this.Controls.Add(this.gbCustomStyle);
            this.Controls.Add(this.groupBox1);
            this.Name = "JcfPage";
            this.gbJcfCommandLine.ResumeLayout(false);
            this.gbJcfCommandLine.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.gbCustomStyle.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.GroupBox gbCustomStyle;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnConfigure;
        #endregion

        /// <summary>
        /// Sets preferences to UI.
        /// </summary>
        public override void PreferencesToUI()
        {
            base.PreferencesToUI();

            // JCF
            txtJcfCommandLine.Text = (string)PropertyRegistry.Get("JcfCommandLineParams");
            //txtUserDefined.Text = (string)PropertyRegistry.Get("JcfSettingsFileName");

            switch ((JcfStyle)PropertyRegistry.Get("JcfStyle"))
            {
                case JcfStyle.CommandLine:
                    rbJCommandLine.Checked = true;
                    break;
                case JcfStyle.UserDefined:
                    if (FileHelper.IsTheSameFile(PropertyRegistry.Get("JcfSettingsFileName").ToString(),
                                                 Lextm.OpenTools.IO.Path.DefaultConfigFile))
                    {
                        rbJDefault.Checked = true;
                    }
                    else
                    {
                        rbUserDefined.Checked = true;
                    }
                    break;
                default:
                    rbJDefault.Checked = true;
                    break;
            }

            rbJcf_CheckedChanged(null, null);
        }

        private const string MaskJcfParams = "-config={0} -clarify -backup -f -y";

        private string GetJcfParams()
        {
            if (rbJCommandLine.Checked)
            {
                return txtJcfCommandLine.Text;
            }
            else
            {
                return String.Format(CultureInfo.InvariantCulture, MaskJcfParams,
                                     StringHelper.QuoteString(
                                        GetCurrentSettingsFileName()));
            }
        }

        private string GetCurrentSettingsFileName()
        {
            JcfStyle _Style = GetCurrentJcfStyle();
            if (_Style == JcfStyle.CommandLine)
            {
                return String.Empty;
            }
            else if (_Style == JcfStyle.UserDefined)
            {
                return Lextm.OpenTools.IO.Path.UserConfigFile;//txtUserDefined.Text;
            }
            else
            {
                return Lextm.OpenTools.IO.Path.DefaultConfigFile;
            }
        }
        private void rbJcf_CheckedChanged(object sender, System.EventArgs e)
        {
            JcfStyle jcfStyle = GetCurrentJcfStyle();
            gbJcfCommandLine.Visible = jcfStyle == JcfStyle.CommandLine;
            gbCustomStyle.Visible = jcfStyle == JcfStyle.UserDefined;
        }

        private JcfStyle GetCurrentJcfStyle()
        {
            if (rbJCommandLine.Checked)
            {
                return JcfStyle.CommandLine;
            }
            else if (rbUserDefined.Checked)
            {
                return JcfStyle.UserDefined;
            }
            else
            {
                return JcfStyle.Borland;
            }
        }

        /// <summary>
        /// Sets UI to preferences.
        /// </summary>
        public override void UIToPreferences()
        {
            base.UIToPreferences();

            // JCF
            PropertyRegistry.Set("JcfStyle", GetCurrentJcfStyle());
            PropertyRegistry.Set("JcfCommandLineParams", txtJcfCommandLine.Text);
            PropertyRegistry.Set("JcfSettingsFileName", GetCurrentSettingsFileName());
            PropertyRegistry.Set("JcfParams", GetJcfParams());

        }

        void BtnConfigureClick(object sender, EventArgs e)
        {
            if (!Lextm.Diagnostics.ShellHelper.IsProcessRunning(OpenTools.IO.Path.JcfStyler))
            {
                Lextm.Diagnostics.ShellHelper.Execute(OpenTools.IO.Path.JcfStyler, true);
            }
            else
            {
                MessageBoxFactory.Warn("JCF Format Settings is already running.");
            }
        }

        void BtnImportClick(object sender, EventArgs e)
        {
            if (!Lextm.Diagnostics.ShellHelper.IsProcessRunning(OpenTools.IO.Path.JcfStyler))
            {
                Vista_Api.OpenFileDialog dialog = new Vista_Api.OpenFileDialog();
                dialog.Filter = "configuration files (*.cfg)|*.cfg";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (!FileHelper.IsTheSameFile(dialog.FileName, OpenTools.IO.Path.UserConfigFile))
                    {
                        System.IO.File.Copy(dialog.FileName, OpenTools.IO.Path.UserConfigFile);
                    }
                }
            }
            else
            {
                MessageBoxFactory.Warn("Please close JCF Format Settings.");
            }
        }

        void BtnExportClick(object sender, EventArgs e)
        {
            Vista_Api.SaveFileDialog dialog = new Vista_Api.SaveFileDialog();
            dialog.Filter = "configuration files (*.cfg)|*.cfg";
            dialog.DefaultExt = "cfg";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (!FileHelper.IsTheSameFile(OpenTools.IO.Path.UserConfigFile, dialog.FileName))
                {
                    System.IO.File.Copy(OpenTools.IO.Path.UserConfigFile, dialog.FileName);
                }
            }
        }
    }
}
