using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using BeWise.Common.Utils;

namespace Lextm.CBC.Gui {
    /// <summary>
    /// Summary description for WinForm.
    /// </summary>
public class FrmOption : System.Windows.Forms.Form {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtJcfCommandline;
        private System.Windows.Forms.TextBox txtJcfPath;
        private System.Windows.Forms.TabPage tbpAStyle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAStylePath;
        private System.Windows.Forms.Button btnAStylePath;
        private System.Windows.Forms.RadioButton rbADefault;
        private System.Windows.Forms.RadioButton rbACommandLine;
        private System.Windows.Forms.RadioButton rbLinux;
        private System.Windows.Forms.RadioButton rbAnsi;
        private System.Windows.Forms.RadioButton rbJava;
        private System.Windows.Forms.RadioButton rbGnu;
        private System.Windows.Forms.Button btJcfPath;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tbpJCF;
        private System.Windows.Forms.RadioButton rbJCommandline;
        private System.Windows.Forms.RadioButton rbJDefault;
        private System.Windows.Forms.RadioButton rbKernighanRitchie;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtAStyleCommandline;
		private System.Windows.Forms.LinkLabel linkLabel2;
        
        public FrmOption() {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
        
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        
        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FrmOption));
			this.btCancel = new System.Windows.Forms.Button();
			this.btOK = new System.Windows.Forms.Button();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tbpJCF = new System.Windows.Forms.TabPage();
			this.rbJCommandline = new System.Windows.Forms.RadioButton();
			this.rbJDefault = new System.Windows.Forms.RadioButton();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.label1 = new System.Windows.Forms.Label();
			this.btJcfPath = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtJcfCommandline = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtJcfPath = new System.Windows.Forms.TextBox();
			this.tbpAStyle = new System.Windows.Forms.TabPage();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.txtAStyleCommandline = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.rbKernighanRitchie = new System.Windows.Forms.RadioButton();
			this.rbGnu = new System.Windows.Forms.RadioButton();
			this.rbJava = new System.Windows.Forms.RadioButton();
			this.rbAnsi = new System.Windows.Forms.RadioButton();
			this.rbLinux = new System.Windows.Forms.RadioButton();
			this.rbACommandLine = new System.Windows.Forms.RadioButton();
			this.rbADefault = new System.Windows.Forms.RadioButton();
			this.btnAStylePath = new System.Windows.Forms.Button();
			this.txtAStylePath = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.linkLabel2 = new System.Windows.Forms.LinkLabel();
			this.tabControl1.SuspendLayout();
			this.tbpJCF.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tbpAStyle.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// btCancel
			// 
			this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btCancel.Location = new System.Drawing.Point(400, 328);
			this.btCancel.Name = "btCancel";
			this.btCancel.Size = new System.Drawing.Size(88, 23);
			this.btCancel.TabIndex = 2;
			this.btCancel.Text = "Cancel";
			// 
			// btOK
			// 
			this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btOK.Location = new System.Drawing.Point(304, 328);
			this.btOK.Name = "btOK";
			this.btOK.Size = new System.Drawing.Size(88, 23);
			this.btOK.TabIndex = 1;
			this.btOK.Text = "OK";
			this.btOK.Click += new System.EventHandler(this.btOK_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tbpJCF);
			this.tabControl1.Controls.Add(this.tbpAStyle);
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(504, 320);
			this.tabControl1.TabIndex = 3;
			// 
			// tbpJCF
			// 
			this.tbpJCF.Controls.Add(this.rbJCommandline);
			this.tbpJCF.Controls.Add(this.rbJDefault);
			this.tbpJCF.Controls.Add(this.linkLabel1);
			this.tbpJCF.Controls.Add(this.label1);
			this.tbpJCF.Controls.Add(this.btJcfPath);
			this.tbpJCF.Controls.Add(this.groupBox1);
			this.tbpJCF.Controls.Add(this.txtJcfPath);
			this.tbpJCF.Location = new System.Drawing.Point(4, 21);
			this.tbpJCF.Name = "tbpJCF";
			this.tbpJCF.Size = new System.Drawing.Size(496, 295);
			this.tbpJCF.TabIndex = 0;
			this.tbpJCF.Text = "for Delphi-JEDI Code Format";
			// 
			// rbJCommandline
			// 
			this.rbJCommandline.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rbJCommandline.Location = new System.Drawing.Point(132, 72);
			this.rbJCommandline.Name = "rbJCommandline";
			this.rbJCommandline.Size = new System.Drawing.Size(112, 24);
			this.rbJCommandline.TabIndex = 11;
			this.rbJCommandline.Text = "Command Line";
			this.rbJCommandline.CheckedChanged += new System.EventHandler(this.rbJcf_CheckedChanged);
			// 
			// rbJDefault
			// 
			this.rbJDefault.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rbJDefault.Location = new System.Drawing.Point(28, 72);
			this.rbJDefault.Name = "rbJDefault";
			this.rbJDefault.Size = new System.Drawing.Size(64, 24);
			this.rbJDefault.TabIndex = 10;
			this.rbJDefault.Text = "Default";
			this.rbJDefault.CheckedChanged += new System.EventHandler(this.rbJcf_CheckedChanged);
			// 
			// linkLabel1
			// 
			this.linkLabel1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.linkLabel1.LinkArea = new System.Windows.Forms.LinkArea(0, 56);
			this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.linkLabel1.Location = new System.Drawing.Point(180, 40);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(296, 24);
			this.linkLabel1.TabIndex = 9;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "JEDI Code Format: http://jedicodeformat.sourceforge.net/";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(20, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(188, 23);
			this.label1.TabIndex = 7;
			this.label1.Text = "JEDI Code Format (jcf.exe) Path:";
			// 
			// btJcfPath
			// 
			this.btJcfPath.Location = new System.Drawing.Point(452, 16);
			this.btJcfPath.Name = "btJcfPath";
			this.btJcfPath.Size = new System.Drawing.Size(20, 16);
			this.btJcfPath.TabIndex = 13;
			this.btJcfPath.Text = "..";
			this.btJcfPath.Click += new System.EventHandler(this.btJcfPath_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.txtJcfCommandline);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(12, 112);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(472, 169);
			this.groupBox1.TabIndex = 12;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Command Line";
			// 
			// txtJcfCommandline
			// 
			this.txtJcfCommandline.Location = new System.Drawing.Point(160, 40);
			this.txtJcfCommandline.Name = "txtJcfCommandline";
			this.txtJcfCommandline.Size = new System.Drawing.Size(296, 21);
			this.txtJcfCommandline.TabIndex = 1;
			this.txtJcfCommandline.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(128, 23);
			this.label2.TabIndex = 0;
			this.label2.Text = "jcf.exe command line:";
			// 
			// txtJcfPath
			// 
			this.txtJcfPath.Location = new System.Drawing.Point(220, 16);
			this.txtJcfPath.Name = "txtJcfPath";
			this.txtJcfPath.Size = new System.Drawing.Size(224, 21);
			this.txtJcfPath.TabIndex = 8;
			this.txtJcfPath.Text = "";
			// 
			// tbpAStyle
			// 
			this.tbpAStyle.Controls.Add(this.linkLabel2);
			this.tbpAStyle.Controls.Add(this.groupBox2);
			this.tbpAStyle.Controls.Add(this.rbKernighanRitchie);
			this.tbpAStyle.Controls.Add(this.rbGnu);
			this.tbpAStyle.Controls.Add(this.rbJava);
			this.tbpAStyle.Controls.Add(this.rbAnsi);
			this.tbpAStyle.Controls.Add(this.rbLinux);
			this.tbpAStyle.Controls.Add(this.rbACommandLine);
			this.tbpAStyle.Controls.Add(this.rbADefault);
			this.tbpAStyle.Controls.Add(this.btnAStylePath);
			this.tbpAStyle.Controls.Add(this.txtAStylePath);
			this.tbpAStyle.Controls.Add(this.label3);
			this.tbpAStyle.Location = new System.Drawing.Point(4, 21);
			this.tbpAStyle.Name = "tbpAStyle";
			this.tbpAStyle.Size = new System.Drawing.Size(496, 295);
			this.tbpAStyle.TabIndex = 1;
			this.tbpAStyle.Text = "for C/C++/C#-AStyle";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.txtAStyleCommandline);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Location = new System.Drawing.Point(16, 176);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(464, 104);
			this.groupBox2.TabIndex = 10;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Command Line";
			// 
			// txtAStyleCommandline
			// 
			this.txtAStyleCommandline.Location = new System.Drawing.Point(160, 32);
			this.txtAStyleCommandline.Name = "txtAStyleCommandline";
			this.txtAStyleCommandline.Size = new System.Drawing.Size(272, 21);
			this.txtAStyleCommandline.TabIndex = 1;
			this.txtAStyleCommandline.Text = "";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(24, 40);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(136, 23);
			this.label4.TabIndex = 0;
			this.label4.Text = "AStyle Command Line:";
			// 
			// rbKernighanRitchie
			// 
			this.rbKernighanRitchie.Location = new System.Drawing.Point(16, 144);
			this.rbKernighanRitchie.Name = "rbKernighanRitchie";
			this.rbKernighanRitchie.Size = new System.Drawing.Size(168, 24);
			this.rbKernighanRitchie.TabIndex = 9;
			this.rbKernighanRitchie.Text = "Kernighan && Ritchie";
			this.rbKernighanRitchie.CheckedChanged += new System.EventHandler(this.rbAStyle_CheckedChanged);
			// 
			// rbGnu
			// 
			this.rbGnu.Location = new System.Drawing.Point(344, 104);
			this.rbGnu.Name = "rbGnu";
			this.rbGnu.TabIndex = 8;
			this.rbGnu.Text = "GNU";
			this.rbGnu.CheckedChanged += new System.EventHandler(this.rbAStyle_CheckedChanged);
			// 
			// rbJava
			// 
			this.rbJava.Location = new System.Drawing.Point(176, 104);
			this.rbJava.Name = "rbJava";
			this.rbJava.TabIndex = 7;
			this.rbJava.Text = "Java";
			this.rbJava.CheckedChanged += new System.EventHandler(this.rbAStyle_CheckedChanged);
			// 
			// rbAnsi
			// 
			this.rbAnsi.Location = new System.Drawing.Point(16, 104);
			this.rbAnsi.Name = "rbAnsi";
			this.rbAnsi.TabIndex = 6;
			this.rbAnsi.Text = "ANSI";
			this.rbAnsi.CheckedChanged += new System.EventHandler(this.rbAStyle_CheckedChanged);
			// 
			// rbLinux
			// 
			this.rbLinux.Location = new System.Drawing.Point(344, 64);
			this.rbLinux.Name = "rbLinux";
			this.rbLinux.TabIndex = 5;
			this.rbLinux.Text = "Linux";
			this.rbLinux.CheckedChanged += new System.EventHandler(this.rbAStyle_CheckedChanged);
			// 
			// rbACommandLine
			// 
			this.rbACommandLine.Location = new System.Drawing.Point(176, 64);
			this.rbACommandLine.Name = "rbACommandLine";
			this.rbACommandLine.TabIndex = 4;
			this.rbACommandLine.Text = "Command Line";
			this.rbACommandLine.CheckedChanged += new System.EventHandler(this.rbAStyle_CheckedChanged);
			// 
			// rbADefault
			// 
			this.rbADefault.Location = new System.Drawing.Point(16, 64);
			this.rbADefault.Name = "rbADefault";
			this.rbADefault.TabIndex = 3;
			this.rbADefault.Text = "Default";
			this.rbADefault.CheckedChanged += new System.EventHandler(this.rbAStyle_CheckedChanged);
			// 
			// btnAStylePath
			// 
			this.btnAStylePath.Location = new System.Drawing.Point(408, 16);
			this.btnAStylePath.Name = "btnAStylePath";
			this.btnAStylePath.Size = new System.Drawing.Size(24, 16);
			this.btnAStylePath.TabIndex = 2;
			this.btnAStylePath.Text = ".";
			this.btnAStylePath.Click += new System.EventHandler(this.btnAStylePath_Click);
			// 
			// txtAStylePath
			// 
			this.txtAStylePath.Location = new System.Drawing.Point(192, 16);
			this.txtAStylePath.Name = "txtAStylePath";
			this.txtAStylePath.Size = new System.Drawing.Size(200, 21);
			this.txtAStylePath.TabIndex = 1;
			this.txtAStylePath.Text = "";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(32, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(144, 23);
			this.label3.TabIndex = 0;
			this.label3.Text = "AStyle (astyle.exe) Path: ";
			// 
			// linkLabel2
			// 
			this.linkLabel2.Location = new System.Drawing.Point(152, 40);
			this.linkLabel2.Name = "linkLabel2";
			this.linkLabel2.Size = new System.Drawing.Size(304, 23);
			this.linkLabel2.TabIndex = 11;
			this.linkLabel2.TabStop = true;
			this.linkLabel2.Text = "AStyle: http://sourceforge.net/projects/astyle";
			// 
			// FrmOption
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(504, 358);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.btOK);
			this.Controls.Add(this.btCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FrmOption";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "JEDI Code Format Options";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.FrmOption_Load);
			this.tabControl1.ResumeLayout(false);
			this.tbpJCF.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.tbpAStyle.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);
		}
        #endregion

		private void btJcfPath_Click(object sender, System.EventArgs e) {
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
			} else if (aButton == btnAStylePath) {
            	return txtAStylePath;
			}
            else {
                return null;
            }
        }
        
		private void btOK_Click(object sender, System.EventArgs e) {
            //JCF
            Main.Configuration.JcfPath = txtJcfPath.Text;
			Main.Configuration.JcfParams = GetJcfParams();
			Main.Configuration.JcfConfiguration.Style = GetCurrentJcfStyle();
            Main.Configuration.JcfConfiguration.CommandLineParams = txtJcfCommandline.Text;
			 //AStyle
			Main.Configuration.AStylePath = txtAStylePath.Text;
			Main.Configuration.AStyleParams = GetAStyleParams();
			Main.Configuration.AStyleConfiguration.Style = GetCurrentAStyleStyle();
            Main.Configuration.AStyleConfiguration.CommandLineParams = txtAStyleCommandline.Text;
            
        }
        
        private void FrmOption_Load(object sender, System.EventArgs e) {
            //JCF
            txtJcfPath.Text = Main.Configuration.JcfPath;
            txtJcfCommandline.Text = Main.Configuration.JcfConfiguration.CommandLineParams;
            
            switch (Main.Configuration.JcfConfiguration.Style) {
                case JcfStyle.CommandLine:
                rbJCommandline.Checked = true;
                break;
                default:
                rbJDefault.Checked = true;
                break;
            }
            
            rbJcf_CheckedChanged(null, null);
            
            //AStyle
            txtAStylePath.Text = Main.Configuration.AStylePath;
            txtAStyleCommandline.Text = Main.Configuration.AStyleConfiguration.CommandLineParams;
            
            switch (Main.Configuration.AStyleConfiguration.Style) {
                case AStyleStyle.CommandLine:
                rbACommandLine.Checked = true;
                break;
                case AStyleStyle.Ansi:
                rbAnsi.Checked = true;
                break;
                case AStyleStyle.Gnu:
                rbGnu.Checked = true;
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
                rbADefault.Checked = true;
                break;
            }
            
            rbAStyle_CheckedChanged(null, null);
            
        }
        
        //JCF
        private void rbJcf_CheckedChanged(object sender, System.EventArgs e) {
            JcfStyle _JcfStyle = GetCurrentJcfStyle();
            
            switch (_JcfStyle) {
                case JcfStyle.CommandLine:
                groupBox1.Visible = true;
                break;
                default:
                groupBox1.Visible = false;
                break;
            }
        }
        
        private JcfStyle GetCurrentJcfStyle() {
            if (rbJCommandline.Checked) {
                return JcfStyle.CommandLine;
            }
            else {
                return JcfStyle.Default;
            }
        }
        
        private string GetJcfParams() {
            if (rbJCommandline.Checked) {
                return txtJcfCommandline.Text;
            }
            else {
                return Consts.DEFAULT_JCF_PARAMS;
            }
        }
        
        //AStyle
        private void rbAStyle_CheckedChanged(object sender, System.EventArgs e) {
            AStyleStyle _AStyleStyle = GetCurrentAStyleStyle();
            
            switch (_AStyleStyle) {
                case AStyleStyle.CommandLine:
                groupBox2.Visible = true;
                break;
                default:
                groupBox2.Visible = false;
                break;
            }
        }
        
        private AStyleStyle GetCurrentAStyleStyle() {
            if (rbACommandLine.Checked) {
                return AStyleStyle.CommandLine;
            }
            else if (rbGnu.Checked) {
                return AStyleStyle.Gnu;
            }
            else if (rbAnsi.Checked) {
                return AStyleStyle.Ansi;
            }
            else if (rbJava.Checked) {
                return AStyleStyle.Java;
            }
            else if (rbLinux.Checked) {
                return AStyleStyle.Linux;
            }
            else if (rbKernighanRitchie.Checked) {
                return AStyleStyle.KernighanRitchie;
            }
            else
                return AStyleStyle.Default;
                
        }
        
        private string GetAStyleParams() {
            if (rbLinux.Checked) {
                return "--style=ansi";
            }
			else if (rbGnu.Checked) {
                return "--style=gnu";
            }
            else if (rbJava.Checked) {
                return "--style=java";
            }
			else if (rbKernighanRitchie.Checked) {
                return "--style=kr";
            }
            else if (rbACommandLine.Checked) {
                return txtAStyleCommandline.Text;
            }
            else if (rbAnsi.Checked) {
                return "--style=ansi";
            }
            else {
                return Consts.DEFAULT_ASTYLE_PARAMS;
            }
        }
		
		private void btnAStylePath_Click(object sender, System.EventArgs e)
		{
			TextBox _Txt = GetPathTextBoxFromButton((Button) sender);
            
            if (_Txt != null) {
                FolderBrowserDialog _Dlg = new FolderBrowserDialog();
                
                _Dlg.SelectedPath = _Txt.Text;
                
                if (_Dlg.ShowDialog() == DialogResult.OK) {
                    _Txt.Text = _Dlg.SelectedPath;
                }
			}
		}
        
    }
}
