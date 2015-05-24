using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Win32;
using Lextm.JcfExpert.Utils;
using System.IO;

namespace Lextm.JcfExpert.Gui {
    public class FrmOptions : System.Windows.Forms.Form {

        // *************************************************************************
        //                       Constructor / Destructor
        // *************************************************************************

        public FrmOptions() {
            InitializeComponent();
        }


        protected override void Dispose( bool disposing ) {
            if( disposing ) {
                if(components != null) {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

        // *************************************************************************
        //                              Private
        // *************************************************************************

        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtJcfPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtJcfCommandLine;
		private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpJcf;
        private System.Windows.Forms.Button btJcfPath;
        private System.Windows.Forms.RadioButton rbDefault;
        private System.Windows.Forms.RadioButton rbCommandLine;
        private System.Windows.Forms.RadioButton rbCustom;
        private System.Windows.Forms.RadioButton rbAnsi;
        private System.Windows.Forms.RadioButton rbKernighanRitchie;
        private System.Windows.Forms.GroupBox gbJcfPredifined;
        private System.Windows.Forms.TextBox txtPredifinedStyle;
        private System.Windows.Forms.GroupBox gbJcfCommandLine;
        private System.Windows.Forms.RadioButton rbLinux;
        private System.Windows.Forms.RadioButton rbGNU;
        private System.Windows.Forms.RadioButton rbJava;
        private System.Windows.Forms.Button Help;

        private void btOk_Click(object sender, System.EventArgs e) {
            Main.Configuration.JcfPath = txtJcfPath.Text;
            Main.Configuration.JcfParams = GetJcfParams();

			Main.Configuration.JcfConfiguration.Style = GetCurrentJcfStyle();
            Main.Configuration.JcfConfiguration.CommandLineParams = txtJcfCommandLine.Text;
        }

		private void FrmOptions_Load(object sender, System.EventArgs e) {
			txtJcfPath.Text = Main.Configuration.JcfPath;
            txtJcfCommandLine.Text = Main.Configuration.JcfConfiguration.CommandLineParams;

            switch (Main.Configuration.JcfConfiguration.Style) {
            case JcfStyle.Ansi:
                rbAnsi.Checked = true;
                break;
            case JcfStyle.KernighanRitchie:
                rbKernighanRitchie.Checked = true;
                break;
            case JcfStyle.Linux:
                rbLinux.Checked = true;
                break;
            case JcfStyle.GNU:
                rbGNU.Checked = true;
                break;
            case JcfStyle.Java:
                rbJava.Checked = true;
                break;
            case JcfStyle.CommandLine:
                rbCommandLine.Checked = true;
                break;
            default:
                rbDefault.Checked = true;
                break;
            }

            rbJcf_CheckedChanged(null, null);
        }

        private void InitializeComponent()
		{
			this.btCancel = new System.Windows.Forms.Button();
			this.btOk = new System.Windows.Forms.Button();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tpJcf = new System.Windows.Forms.TabPage();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.rbJava = new System.Windows.Forms.RadioButton();
			this.rbGNU = new System.Windows.Forms.RadioButton();
			this.rbLinux = new System.Windows.Forms.RadioButton();
			this.rbKernighanRitchie = new System.Windows.Forms.RadioButton();
			this.rbAnsi = new System.Windows.Forms.RadioButton();
			this.rbCustom = new System.Windows.Forms.RadioButton();
			this.rbCommandLine = new System.Windows.Forms.RadioButton();
			this.rbDefault = new System.Windows.Forms.RadioButton();
			this.txtJcfPath = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.btJcfPath = new System.Windows.Forms.Button();
			this.gbJcfCommandLine = new System.Windows.Forms.GroupBox();
			this.txtJcfCommandLine = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.gbJcfPredifined = new System.Windows.Forms.GroupBox();
			this.txtPredifinedStyle = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.Help = new System.Windows.Forms.Button();
			this.tabControl.SuspendLayout();
			this.tpJcf.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.gbJcfCommandLine.SuspendLayout();
			this.gbJcfPredifined.SuspendLayout();
			this.SuspendLayout();
			// 
			// btCancel
			// 
			this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btCancel.Location = new System.Drawing.Point(600, 448);
			this.btCancel.Name = "btCancel";
			this.btCancel.Size = new System.Drawing.Size(96, 24);
			this.btCancel.TabIndex = 2;
			this.btCancel.Text = "Cancel";
			// 
			// btOk
			// 
			this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btOk.Location = new System.Drawing.Point(496, 448);
			this.btOk.Name = "btOk";
			this.btOk.Size = new System.Drawing.Size(96, 24);
			this.btOk.TabIndex = 1;
			this.btOk.Text = "Ok";
			this.btOk.Click += new System.EventHandler(this.btOk_Click);
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.tpJcf);
			this.tabControl.Location = new System.Drawing.Point(10, 9);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(700, 430);
			this.tabControl.TabIndex = 0;
			// 
			// tpJcf
			// 
			this.tpJcf.Controls.Add(this.groupBox3);
			this.tpJcf.Location = new System.Drawing.Point(4, 21);
			this.tpJcf.Name = "tpJcf";
			this.tpJcf.Size = new System.Drawing.Size(692, 405);
			this.tpJcf.TabIndex = 2;
			this.tpJcf.Text = "JEDI Code Format ";
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.rbJava);
			this.groupBox3.Controls.Add(this.rbGNU);
			this.groupBox3.Controls.Add(this.rbLinux);
			this.groupBox3.Controls.Add(this.rbKernighanRitchie);
			this.groupBox3.Controls.Add(this.rbAnsi);
			this.groupBox3.Controls.Add(this.rbCustom);
			this.groupBox3.Controls.Add(this.rbCommandLine);
			this.groupBox3.Controls.Add(this.rbDefault);
			this.groupBox3.Controls.Add(this.txtJcfPath);
			this.groupBox3.Controls.Add(this.label3);
			this.groupBox3.Controls.Add(this.btJcfPath);
			this.groupBox3.Controls.Add(this.gbJcfCommandLine);
			this.groupBox3.Controls.Add(this.gbJcfPredifined);
			this.groupBox3.Location = new System.Drawing.Point(10, 9);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(681, 388);
			this.groupBox3.TabIndex = 5;
			this.groupBox3.TabStop = false;
			// 
			// rbJava
			// 
			this.rbJava.Enabled = false;
			this.rbJava.Location = new System.Drawing.Point(528, 86);
			this.rbJava.Name = "rbJava";
			this.rbJava.Size = new System.Drawing.Size(125, 26);
			this.rbJava.TabIndex = 20;
			this.rbJava.Text = "Java";
			this.rbJava.Visible = false;
			this.rbJava.CheckedChanged += new System.EventHandler(this.rbJcf_CheckedChanged);
			// 
			// rbGNU
			// 
			this.rbGNU.Enabled = false;
			this.rbGNU.Location = new System.Drawing.Point(403, 86);
			this.rbGNU.Name = "rbGNU";
			this.rbGNU.Size = new System.Drawing.Size(125, 26);
			this.rbGNU.TabIndex = 19;
			this.rbGNU.Text = "GNU";
			this.rbGNU.Visible = false;
			this.rbGNU.CheckedChanged += new System.EventHandler(this.rbJcf_CheckedChanged);
			// 
			// rbLinux
			// 
			this.rbLinux.Enabled = false;
			this.rbLinux.Location = new System.Drawing.Point(278, 86);
			this.rbLinux.Name = "rbLinux";
			this.rbLinux.Size = new System.Drawing.Size(125, 26);
			this.rbLinux.TabIndex = 18;
			this.rbLinux.Text = "Linux";
			this.rbLinux.Visible = false;
			this.rbLinux.CheckedChanged += new System.EventHandler(this.rbJcf_CheckedChanged);
			// 
			// rbKernighanRitchie
			// 
			this.rbKernighanRitchie.Enabled = false;
			this.rbKernighanRitchie.Location = new System.Drawing.Point(125, 86);
			this.rbKernighanRitchie.Name = "rbKernighanRitchie";
			this.rbKernighanRitchie.Size = new System.Drawing.Size(134, 26);
			this.rbKernighanRitchie.TabIndex = 15;
			this.rbKernighanRitchie.Text = "Kernighan Ritchie";
			this.rbKernighanRitchie.Visible = false;
			this.rbKernighanRitchie.CheckedChanged += new System.EventHandler(this.rbJcf_CheckedChanged);
			// 
			// rbAnsi
			// 
			this.rbAnsi.Enabled = false;
			this.rbAnsi.Location = new System.Drawing.Point(19, 86);
			this.rbAnsi.Name = "rbAnsi";
			this.rbAnsi.Size = new System.Drawing.Size(125, 26);
			this.rbAnsi.TabIndex = 14;
			this.rbAnsi.Text = "Ansi";
			this.rbAnsi.Visible = false;
			this.rbAnsi.CheckedChanged += new System.EventHandler(this.rbJcf_CheckedChanged);
			// 
			// rbCustom
			// 
			this.rbCustom.Enabled = false;
			this.rbCustom.Location = new System.Drawing.Point(278, 52);
			this.rbCustom.Name = "rbCustom";
			this.rbCustom.Size = new System.Drawing.Size(125, 26);
			this.rbCustom.TabIndex = 13;
			this.rbCustom.Text = "Custom";
			this.rbCustom.Visible = false;
			this.rbCustom.CheckedChanged += new System.EventHandler(this.rbJcf_CheckedChanged);
			// 
			// rbCommandLine
			// 
			this.rbCommandLine.Location = new System.Drawing.Point(125, 52);
			this.rbCommandLine.Name = "rbCommandLine";
			this.rbCommandLine.Size = new System.Drawing.Size(125, 26);
			this.rbCommandLine.TabIndex = 12;
			this.rbCommandLine.Text = "Command Line";
			this.rbCommandLine.CheckedChanged += new System.EventHandler(this.rbJcf_CheckedChanged);
			// 
			// rbDefault
			// 
			this.rbDefault.Checked = true;
			this.rbDefault.Location = new System.Drawing.Point(19, 52);
			this.rbDefault.Name = "rbDefault";
			this.rbDefault.Size = new System.Drawing.Size(96, 26);
			this.rbDefault.TabIndex = 11;
			this.rbDefault.TabStop = true;
			this.rbDefault.Text = "Default";
			this.rbDefault.CheckedChanged += new System.EventHandler(this.rbJcf_CheckedChanged);
			// 
			// txtJcfPath
			// 
			this.txtJcfPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtJcfPath.Location = new System.Drawing.Point(144, 20);
			this.txtJcfPath.Name = "txtJcfPath";
			this.txtJcfPath.Size = new System.Drawing.Size(485, 21);
			this.txtJcfPath.TabIndex = 7;
			this.txtJcfPath.Text = "";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(10, 25);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(134, 18);
			this.label3.TabIndex = 6;
			this.label3.Text = "JCF Commandline Path:";
			// 
			// btJcfPath
			// 
			this.btJcfPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btJcfPath.Location = new System.Drawing.Point(638, 24);
			this.btJcfPath.Name = "btJcfPath";
			this.btJcfPath.Size = new System.Drawing.Size(29, 17);
			this.btJcfPath.TabIndex = 5;
			this.btJcfPath.Text = "...";
			this.btJcfPath.Click += new System.EventHandler(this.btPath_Click);
			// 
			// gbJcfCommandLine
			// 
			this.gbJcfCommandLine.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gbJcfCommandLine.Controls.Add(this.txtJcfCommandLine);
			this.gbJcfCommandLine.Controls.Add(this.label4);
			this.gbJcfCommandLine.Location = new System.Drawing.Point(17, 121);
			this.gbJcfCommandLine.Name = "gbJcfCommandLine";
			this.gbJcfCommandLine.Size = new System.Drawing.Size(653, 259);
			this.gbJcfCommandLine.TabIndex = 21;
			this.gbJcfCommandLine.TabStop = false;
			this.gbJcfCommandLine.Text = "Command Line";
			// 
			// txtJcfCommandLine
			// 
			this.txtJcfCommandLine.Location = new System.Drawing.Point(182, 26);
			this.txtJcfCommandLine.Name = "txtJcfCommandLine";
			this.txtJcfCommandLine.Size = new System.Drawing.Size(279, 21);
			this.txtJcfCommandLine.TabIndex = 11;
			this.txtJcfCommandLine.Text = "";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(19, 30);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(163, 39);
			this.label4.TabIndex = 10;
			this.label4.Text = "JCF Command Line:";
			// 
			// gbJcfPredifined
			// 
			this.gbJcfPredifined.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gbJcfPredifined.Controls.Add(this.txtPredifinedStyle);
			this.gbJcfPredifined.Location = new System.Drawing.Point(19, 121);
			this.gbJcfPredifined.Name = "gbJcfPredifined";
			this.gbJcfPredifined.Size = new System.Drawing.Size(653, 259);
			this.gbJcfPredifined.TabIndex = 16;
			this.gbJcfPredifined.TabStop = false;
			this.gbJcfPredifined.Text = "Predifined Style";
			// 
			// txtPredifinedStyle
			// 
			this.txtPredifinedStyle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtPredifinedStyle.Location = new System.Drawing.Point(13, 15);
			this.txtPredifinedStyle.Multiline = true;
			this.txtPredifinedStyle.Name = "txtPredifinedStyle";
			this.txtPredifinedStyle.ReadOnly = true;
			this.txtPredifinedStyle.Size = new System.Drawing.Size(627, 236);
			this.txtPredifinedStyle.TabIndex = 12;
			this.txtPredifinedStyle.Text = "";
			// 
			// groupBox1
			// 
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(0, 0);
			this.label2.Name = "label2";
			this.label2.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.TabIndex = 0;
			// 
			// Help
			// 
			this.Help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.Help.Enabled = false;
			this.Help.Location = new System.Drawing.Point(392, 448);
			this.Help.Name = "Help";
			this.Help.Size = new System.Drawing.Size(96, 26);
			this.Help.TabIndex = 3;
			this.Help.Text = "Help";
			this.Help.Visible = false;
			this.Help.Click += new System.EventHandler(this.button1_Click);
			// 
			// FrmOptions
			// 
			this.AcceptButton = this.btOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.CancelButton = this.btCancel;
			this.ClientSize = new System.Drawing.Size(722, 482);
			this.Controls.Add(this.Help);
			this.Controls.Add(this.btOk);
			this.Controls.Add(this.btCancel);
			this.Controls.Add(this.tabControl);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MinimumSize = new System.Drawing.Size(512, 304);
			this.Name = "FrmOptions";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "JCF Expert- Options";
			this.Load += new System.EventHandler(this.FrmOptions_Load);
			this.tabControl.ResumeLayout(false);
			this.tpJcf.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.gbJcfCommandLine.ResumeLayout(false);
			this.gbJcfPredifined.ResumeLayout(false);
			this.ResumeLayout(false);
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
            if (aButton == btJcfPath) {
                return txtJcfPath;
            } else {
                return null;
            }
        }

        private void rbJcf_CheckedChanged(object sender, System.EventArgs e) {
            JcfStyle _JcfStyle = GetCurrentJcfStyle();

            switch (_JcfStyle) {
            case JcfStyle.Ansi:
            case JcfStyle.KernighanRitchie:
            case JcfStyle.Linux:
            case JcfStyle.GNU:
            case JcfStyle.Java:
                gbJcfCommandLine.Visible = false;
                gbJcfPredifined.Visible = true;
                txtPredifinedStyle.Lines = GetPredifinedStyleExample(_JcfStyle);
                break;
            case JcfStyle.CommandLine:
                gbJcfCommandLine.Visible = true;
                gbJcfPredifined.Visible = false;
                break;
            default:
                gbJcfCommandLine.Visible = false;
                gbJcfPredifined.Visible = false;
                break;
            }
        }

        private JcfStyle GetCurrentJcfStyle() {
            if (rbLinux.Checked) {
                return JcfStyle.Linux;
            } else if (rbGNU.Checked) {
                return JcfStyle.GNU;
            } else if (rbJava.Checked) {
                return JcfStyle.Java;
            } else if (rbKernighanRitchie.Checked) {
                return JcfStyle.KernighanRitchie;
            } else if (rbCommandLine.Checked) {
                return JcfStyle.CommandLine;
            } else if (rbCustom.Checked) {
                return JcfStyle.Custom;
            } else if (rbAnsi.Checked) {
                return JcfStyle.Ansi;
            } else {
                return JcfStyle.Default;
            }
        }

        private string GetJcfParams() {
            if (rbLinux.Checked) {
                return "--style=ansi";
            } else if (rbGNU.Checked) {
                return "--style=gnu";
            } else if (rbJava.Checked) {
                return "--style=java";
            } else if (rbKernighanRitchie.Checked) {
                return "--style=kr";
            } else if (rbCommandLine.Checked) {
                return txtJcfCommandLine.Text;
            } else if (rbAnsi.Checked) {
                return "--style=ansi";
            } else {
                return Consts.DEFAULT_JCF_PARAMS;
            }
        }

        private string[] GetPredifinedStyleExample(JcfStyle aJcfStyle) {
            string[] _Lines;

            switch (aJcfStyle) {
            case JcfStyle.Ansi:
                _Lines = new string[] {
                             "--style=ansi",
                             "ANSI style formatting/indenting.",
                             "",
                             "namespace foospace",
                             "{",
                             "    int Foo()",
                             "    {",
                             "        if (isBar)",
                             "        {",
                             "            bar();",
                             "            return 1;",
                             "        }",
                             "        else",
                             "            return 0;",
                             "    }",
                             "}"};

                break;

            case JcfStyle.KernighanRitchie:
                _Lines = new string[] {
                             "--style=kr",
                             "Kernighan&Ritchie style formatting/indenting.",
                             "",
                             "namespace foospace {",
                             "	int Foo() {",
                             "		if (isBar) {",
                             "			bar();",
                             "			return 1;",
                             "		} else",
                             "			return 0;",
                             "	}",
                             "}"};

                break;

            case JcfStyle.Linux:
                _Lines = new string[] {
                             "--style=linux",
                             "Linux style formatting/indenting (brackets are broken apart from class/function declarations, but connected to command lines, and indents are set to 8 spaces).",
                             "",
                             "namespace foospace",
                             "{",
                             "		int Foo()",
                             "		{",
                             "				if (isBar) {",
                             "						bar();",
                             "						return 1;",
                             "				} else",
                             "						return 0;",
                             "		}",
                             "}"};

                break;

            case JcfStyle.GNU:
                _Lines = new string[] {
                             "--style=gnu",
                             "GNU style formatting/indenting.",
                             "",
                             "namespace foospace",
                             "  {",
                             "	int Foo()",
                             "	  {",
                             "		if (isBar)",
                             "		  {",
                             "			bar();",
                             "			return 1;",
                             "		  }",
                             "		else",
                             "		  return 0;",
                             "	  }",
                             "}"};
                break;

            case JcfStyle.Java:
                _Lines = new string[] {
                             "--style=java",
                             "Java style formatting/indenting.",
                             "",
                             "class foospace {",
                             "    int Foo() {",
                             "        if (isBar) {",
                             "            bar();",
                             "            return 1;",
                             "        } else",
                             "            return 0;",
                             "    }",
                             "}"};
                break;

            default:
                _Lines = new string[] {};
                break;
            }

            return _Lines;
		}

//        private void GetHelp() {
//            RunProcess _RP = new RunProcess();
//			string[] _Arr = new string[2];
//			string _Path = Main.Configuration.JcfPath;
//
//			if (_Path[_Path.Length -1] != '\\') {
//				_Path += @"\";
//			}
//
//			_Arr[0] = "winhlp32.exe";
//			_Arr[1] = _Path + @" """ + "CodeFormat.hlp" + @"""";
//
//			_RP.Run(_Arr, Path.GetDirectoryName(_Arr[1]));
//
//		}

		private void button1_Click(object sender, System.EventArgs e) {
//			this.GetHelp();
        }
    }
}
