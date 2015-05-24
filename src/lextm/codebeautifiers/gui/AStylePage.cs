// This is AStyle tab page.
// Copyright (C) 2006  Lex Y. Li
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
using Lextm.CodeBeautifiers;
using System;

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Lextm.CodeBeautifiers.Feature;
using System.Text;
using System.Diagnostics;
using Lextm.OpenTools;
using Lextm.Diagnostics;

namespace Lextm.CodeBeautifiers.Gui {
    /// <summary>
    /// Tab page for AStyle.
    /// </summary>
    public class AStylePage : Lextm.OpenTools.Gui.CustomPage {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.RadioButton rbJava;
        private System.Windows.Forms.RadioButton rbGNU;
        private System.Windows.Forms.RadioButton rbLinux;
        private System.Windows.Forms.RadioButton rbKernighanRitchie;
        private System.Windows.Forms.RadioButton rbAnsi;
        private System.Windows.Forms.RadioButton rbACommandLine;
        private System.Windows.Forms.GroupBox gbAStylePredifined;
        private System.Windows.Forms.GroupBox gbAStyleCommandLine;
        private System.Windows.Forms.TextBox txtAStyleCommandLine;
        private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox cbIndentNamespaces;
		private System.Windows.Forms.CheckBox cbIndentPreprocessors;
		private System.Windows.Forms.CheckBox cbConvertTabs;
		private System.Windows.Forms.RichTextBox txtPredifinedStyle;
		/// <summary>
		/// Constructor.
		/// </summary>
        public AStylePage() {
            // This call is required by the Windows.Forms Designer.
            InitializeComponent();

            //gbAStylePredifined.Location = gbAStylePredifined.Location;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (components != null)
                {    components.Dispose();}
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
			this.rbJava = new System.Windows.Forms.RadioButton();
			this.rbGNU = new System.Windows.Forms.RadioButton();
			this.rbLinux = new System.Windows.Forms.RadioButton();
			this.rbKernighanRitchie = new System.Windows.Forms.RadioButton();
			this.rbAnsi = new System.Windows.Forms.RadioButton();
			this.rbACommandLine = new System.Windows.Forms.RadioButton();
			this.gbAStylePredifined = new System.Windows.Forms.GroupBox();
			this.txtPredifinedStyle = new System.Windows.Forms.RichTextBox();
			this.gbAStyleCommandLine = new System.Windows.Forms.GroupBox();
			this.txtAStyleCommandLine = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.cbConvertTabs = new System.Windows.Forms.CheckBox();
			this.cbIndentPreprocessors = new System.Windows.Forms.CheckBox();
			this.cbIndentNamespaces = new System.Windows.Forms.CheckBox();
			this.gbAStylePredifined.SuspendLayout();
			this.gbAStyleCommandLine.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// rbJava
			// 
			this.rbJava.Location = new System.Drawing.Point(128, 40);
			this.rbJava.Name = "rbJava";
			this.rbJava.Size = new System.Drawing.Size(72, 24);
			this.rbJava.TabIndex = 27;
			this.rbJava.Text = "Java";
			this.rbJava.CheckedChanged += new System.EventHandler(this.rbAStyle_CheckedChanged);
			// 
			// rbGNU
			// 
			this.rbGNU.Location = new System.Drawing.Point(128, 16);
			this.rbGNU.Name = "rbGNU";
			this.rbGNU.Size = new System.Drawing.Size(56, 24);
			this.rbGNU.TabIndex = 26;
			this.rbGNU.Text = "GNU";
			this.rbGNU.CheckedChanged += new System.EventHandler(this.rbAStyle_CheckedChanged);
			// 
			// rbLinux
			// 
			this.rbLinux.Location = new System.Drawing.Point(8, 40);
			this.rbLinux.Name = "rbLinux";
			this.rbLinux.Size = new System.Drawing.Size(72, 24);
			this.rbLinux.TabIndex = 25;
			this.rbLinux.Text = "Linux";
			this.rbLinux.CheckedChanged += new System.EventHandler(this.rbAStyle_CheckedChanged);
			// 
			// rbKernighanRitchie
			// 
			this.rbKernighanRitchie.Location = new System.Drawing.Point(240, 16);
			this.rbKernighanRitchie.Name = "rbKernighanRitchie";
			this.rbKernighanRitchie.Size = new System.Drawing.Size(144, 24);
			this.rbKernighanRitchie.TabIndex = 24;
			this.rbKernighanRitchie.Text = "Kernighan Ritchie";
			this.rbKernighanRitchie.CheckedChanged += new System.EventHandler(this.rbAStyle_CheckedChanged);
			// 
			// rbAnsi
			// 
			this.rbAnsi.Location = new System.Drawing.Point(8, 16);
			this.rbAnsi.Name = "rbAnsi";
			this.rbAnsi.Size = new System.Drawing.Size(80, 24);
			this.rbAnsi.TabIndex = 23;
			this.rbAnsi.Text = "ANSI";
			this.rbAnsi.CheckedChanged += new System.EventHandler(this.rbAStyle_CheckedChanged);
			// 
			// rbACommandLine
			// 
			this.rbACommandLine.Location = new System.Drawing.Point(240, 40);
			this.rbACommandLine.Name = "rbACommandLine";
			this.rbACommandLine.TabIndex = 21;
			this.rbACommandLine.Text = "Command Line";
			this.rbACommandLine.CheckedChanged += new System.EventHandler(this.rbAStyle_CheckedChanged);
			// 
			// gbAStylePredifined
			// 
			this.gbAStylePredifined.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gbAStylePredifined.Controls.Add(this.txtPredifinedStyle);
			this.gbAStylePredifined.Location = new System.Drawing.Point(16, 152);
			this.gbAStylePredifined.Name = "gbAStylePredifined";
			this.gbAStylePredifined.Size = new System.Drawing.Size(528, 184);
			this.gbAStylePredifined.TabIndex = 30;
			this.gbAStylePredifined.TabStop = false;
			this.gbAStylePredifined.Text = "Style preview";
			// 
			// txtPredifinedStyle
			// 
			this.txtPredifinedStyle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtPredifinedStyle.Location = new System.Drawing.Point(8, 24);
			this.txtPredifinedStyle.Name = "txtPredifinedStyle";
			this.txtPredifinedStyle.ReadOnly = true;
			this.txtPredifinedStyle.Size = new System.Drawing.Size(490, 144);
			this.txtPredifinedStyle.TabIndex = 13;
			this.txtPredifinedStyle.Text = "";
			// 
			// gbAStyleCommandLine
			// 
			this.gbAStyleCommandLine.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gbAStyleCommandLine.Controls.Add(this.txtAStyleCommandLine);
			this.gbAStyleCommandLine.Controls.Add(this.label4);
			this.gbAStyleCommandLine.Location = new System.Drawing.Point(16, 152);
			this.gbAStyleCommandLine.Name = "gbAStyleCommandLine";
			this.gbAStyleCommandLine.Size = new System.Drawing.Size(528, 184);
			this.gbAStyleCommandLine.TabIndex = 29;
			this.gbAStyleCommandLine.TabStop = false;
			this.gbAStyleCommandLine.Text = "Command line";
			// 
			// txtAStyleCommandLine
			// 
			this.txtAStyleCommandLine.Location = new System.Drawing.Point(152, 24);
			this.txtAStyleCommandLine.Name = "txtAStyleCommandLine";
			this.txtAStyleCommandLine.Size = new System.Drawing.Size(232, 21);
			this.txtAStyleCommandLine.TabIndex = 1;
			this.txtAStyleCommandLine.Text = "";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 28);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(136, 36);
			this.label4.TabIndex = 0;
			this.label4.Text = "AStyle Command Line:";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.rbJava);
			this.groupBox1.Controls.Add(this.rbGNU);
			this.groupBox1.Controls.Add(this.rbLinux);
			this.groupBox1.Controls.Add(this.rbKernighanRitchie);
			this.groupBox1.Controls.Add(this.rbAnsi);
			this.groupBox1.Controls.Add(this.rbACommandLine);
			this.groupBox1.Location = new System.Drawing.Point(16, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(528, 72);
			this.groupBox1.TabIndex = 31;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Coding styles";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.cbConvertTabs);
			this.groupBox2.Controls.Add(this.cbIndentPreprocessors);
			this.groupBox2.Controls.Add(this.cbIndentNamespaces);
			this.groupBox2.Location = new System.Drawing.Point(16, 84);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(528, 60);
			this.groupBox2.TabIndex = 32;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Other options";
			// 
			// cbConvertTabs
			// 
			this.cbConvertTabs.Location = new System.Drawing.Point(344, 24);
			this.cbConvertTabs.Name = "cbConvertTabs";
			this.cbConvertTabs.Size = new System.Drawing.Size(160, 24);
			this.cbConvertTabs.TabIndex = 2;
			this.cbConvertTabs.Text = "Convert tabs to spaces";
			// 
			// cbIndentPreprocessors
			// 
			this.cbIndentPreprocessors.Location = new System.Drawing.Point(168, 24);
			this.cbIndentPreprocessors.Name = "cbIndentPreprocessors";
			this.cbIndentPreprocessors.Size = new System.Drawing.Size(152, 24);
			this.cbIndentPreprocessors.TabIndex = 1;
			this.cbIndentPreprocessors.Text = "Indent preprocessors";
			// 
			// cbIndentNamespaces
			// 
			this.cbIndentNamespaces.Location = new System.Drawing.Point(8, 24);
			this.cbIndentNamespaces.Name = "cbIndentNamespaces";
			this.cbIndentNamespaces.Size = new System.Drawing.Size(136, 24);
			this.cbIndentNamespaces.TabIndex = 0;
			this.cbIndentNamespaces.Text = "Indent namespaces";
			// 
			// AStylePage
			// 
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.gbAStylePredifined);
			this.Controls.Add(this.gbAStyleCommandLine);
			this.Name = "AStylePage";
			this.Size = new System.Drawing.Size(552, 352);
			this.gbAStylePredifined.ResumeLayout(false);
			this.gbAStyleCommandLine.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);
		}
#endregion

#region RegisterComponents

		private readonly static string AnsiToolTip =
			"namespace foospace" + Environment.NewLine +
			"{" + Environment.NewLine +
			"int Foo()" + Environment.NewLine +
			"{" + Environment.NewLine +
			"    if (isBar)" + Environment.NewLine +
			"    {" + Environment.NewLine +
			"        bar();" + Environment.NewLine +
			"        return 1;" + Environment.NewLine +
			"    }" + Environment.NewLine +
			"    else" + Environment.NewLine +
			"        return 0;" + Environment.NewLine +
			"}" + Environment.NewLine +
			"}";

		private readonly static string GnuToolTip =
			"namespace foospace" + Environment.NewLine +
			"  {" + Environment.NewLine +
			"    int Foo()" + Environment.NewLine +
			"      {" + Environment.NewLine +
			"        if (isBar)" + Environment.NewLine +
			"          {" + Environment.NewLine +
			"            bar();" + Environment.NewLine +
			"            return 1;" + Environment.NewLine +
			"          }" + Environment.NewLine +
			"        else" + Environment.NewLine +
			"          return 0;" + Environment.NewLine +
			"      }" + Environment.NewLine +
			"  }";

		private readonly static string KRToolTip =
			"namespace foospace {" + Environment.NewLine +
			"int Foo() {" + Environment.NewLine +
			"    if (isBar) {" + Environment.NewLine +
			"        bar();" + Environment.NewLine +
			"        return 1;" + Environment.NewLine +
			"    } else" + Environment.NewLine +
			"        return 0;" + Environment.NewLine +
			"}" + Environment.NewLine +
			"}";

		private readonly static string LinuxToolTip =
			"namespace foospace" + Environment.NewLine +
			"{" + Environment.NewLine +
			"int Foo()" + Environment.NewLine +
			"{" + Environment.NewLine +
			"        if (isBar) {" + Environment.NewLine +
			"                bar();" + Environment.NewLine +
			"                return 1;" + Environment.NewLine +
			"        } else" + Environment.NewLine +
			"                return 0;" + Environment.NewLine +
			"}" + Environment.NewLine +
			"}";
		private readonly static string JavaToolTip =
			"class foospace {" + Environment.NewLine +
			"    int Foo() {" + Environment.NewLine +
			"        if (isBar) {" + Environment.NewLine +
			"            bar();" + Environment.NewLine +
			"            return 1;" + Environment.NewLine +
			"        } else" + Environment.NewLine +
			"            return 0;" + Environment.NewLine +
			"    }" + Environment.NewLine +
			"}";

		private readonly static string[] ArrayAStyleParams = {
					"--style=linux",
					"--style=gnu",
					"--style=java",
					"--style=kr",
					"--style=ansi"
				};

		private string GetAStyleParams() {
			StringBuilder result = new StringBuilder();

			if (!rbACommandLine.Checked) {

				string style = null;

				if (rbLinux.Checked) {
					style = ArrayAStyleParams[0];
				} else if (rbGNU.Checked) {
					style = ArrayAStyleParams[1];
				} else if (rbJava.Checked) {
					style = ArrayAStyleParams[2];
				} else if (rbKernighanRitchie.Checked) {
					style = ArrayAStyleParams[3];
				} else if (rbAnsi.Checked) {
					style = ArrayAStyleParams[4];
				} else {
					LoggingService.Warn("Failed: invalid astyle style");
					style = ArrayAStyleParams[3];
				}

				result.Append(style);

				if ((bool)PropertyRegistry.Get("AStyleIndentNamespace")) {
					result.Append(" --indent-namespaces");
				}
				if ((bool)PropertyRegistry.Get("AStyleConvertTabs")) {
					result.Append(" --convert-tabs");
				}
				if ((bool)PropertyRegistry.Get("AStyleIndentPreprocessor")) {
					result.Append(" --indent-preprocessor");
				}
			} else {
				result.Append(txtAStyleCommandLine.Text);
			}
			LoggingService.Info("astyle params is " + result.ToString());
			return result.ToString();
		}

        private AStyleStyle GetCurrentAStyleStyle() {
            if (rbACommandLine.Checked) {
				return AStyleStyle.CommandLine;
            } else if (rbGNU.Checked) {
				return AStyleStyle.Gnu;
            } else if (rbAnsi.Checked) {
				return AStyleStyle.Ansi;
            } else if (rbJava.Checked) {
				return AStyleStyle.Java;
            } else if (rbLinux.Checked) {
				return AStyleStyle.Linux;
			} else {
				return AStyleStyle.KernighanRitchie;
			} 
		}
		private void rbAStyle_CheckedChanged(object sender, System.EventArgs e) {
		
			LoggingService.EnterMethod();
			AStyleStyle aStyleStyle = GetCurrentAStyleStyle();

			switch (aStyleStyle) {
			case AStyleStyle.Ansi:
				gbAStyleCommandLine.Visible = false;
				gbAStylePredifined.Visible = true;
				txtPredifinedStyle.Text = AnsiToolTip;
				break;
			case AStyleStyle.Gnu:
				gbAStyleCommandLine.Visible = false;
				gbAStylePredifined.Visible = true;
				txtPredifinedStyle.Text = GnuToolTip;
				break;
			case AStyleStyle.Java:
            	gbAStyleCommandLine.Visible = false;
				gbAStylePredifined.Visible = true;
				txtPredifinedStyle.Text = JavaToolTip;
				break;
			case AStyleStyle.KernighanRitchie:
                gbAStyleCommandLine.Visible = false;
				gbAStylePredifined.Visible = true;
				txtPredifinedStyle.Text = KRToolTip;
				break;
			case AStyleStyle.Linux:
                gbAStyleCommandLine.Visible = false;
				gbAStylePredifined.Visible = true;
                txtPredifinedStyle.Text = LinuxToolTip;
				break;
			case AStyleStyle.CommandLine:
				gbAStyleCommandLine.Visible = true;
				gbAStylePredifined.Visible = false;
				break;
			default:
				gbAStyleCommandLine.Visible = false;
				gbAStylePredifined.Visible = true;
				break;
			}
			LoggingService.LeaveMethod();
		
		}  

		/// <summary>
		/// Sets preferences to UI.
        /// </summary>
        public override void PreferencesToUI( ) {
            base.PreferencesToUI();

			// AStyle
			//txtAStyleCommandLine.Text = PropertyService.Get("AStyleCommandLineParams").ToString();

            switch ((AStyleStyle)PropertyRegistry.Get("AStyleStyle")) {
			case AStyleStyle.CommandLine:
                rbACommandLine.Checked = true;
                break;
			case AStyleStyle.Ansi:
                rbAnsi.Checked = true;
                break;
			case AStyleStyle.Gnu:
				rbGNU.Checked = true;
                break;
			case AStyleStyle.Java:
                rbJava.Checked = true;
                break;
			case AStyleStyle.KernighanRitchie:
				rbKernighanRitchie.Checked = true;
				break;
			case AStyleStyle.Linux:
                rbLinux.Checked = true;
                break;
            default:
				rbKernighanRitchie.Checked = true;
                break;
            }

			rbAStyle_CheckedChanged(null, null);

			cbConvertTabs.Checked = (bool)PropertyRegistry.Get("AStyleConvertTabs");
			cbIndentNamespaces.Checked = (bool)PropertyRegistry.Get("AStyleIndentNamespace");
			cbIndentPreprocessors.Checked = (bool)PropertyRegistry.Get("AStyleIndentPreprocessor");
        }
        /// <summary>
        /// Sets UI to preferences.
        /// </summary>
        public override void UIToPreferences( ) {
            base.UIToPreferences();

			// AStyle
			PropertyRegistry.Set("AStyleStyle", GetCurrentAStyleStyle());
			PropertyRegistry.Set("AStyleCommandLineParams", txtAStyleCommandLine.Text);

			PropertyRegistry.Set("AStyleConvertTabs", cbConvertTabs.Checked);
			PropertyRegistry.Set("AStyleIndentNamespace", cbIndentNamespaces.Checked);
			PropertyRegistry.Set("AStyleIndentPreprocessor", cbIndentPreprocessors.Checked);
			PropertyRegistry.Set("AStyleParams", GetAStyleParams());
        }
#endregion

    }
}
