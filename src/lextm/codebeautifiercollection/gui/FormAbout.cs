// this is the form about class. Ported from SBT.
//		Captions are modified.
// Copyright (C) 2005-2007  Lex Y. Li
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
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using BeWise.SharpDevTools.Component;
using Lextm.Diagnostics;
using Lextm.OpenTools;
using System.Globalization;
using PSTaskDialog;

namespace Lextm.CodeBeautifierCollection.Gui {
	/// <summary>
	/// About form.
	///</summary>
	sealed class FormAbout : System.Windows.Forms.Form {
		
		/// <summary>
		/// Constructor.
		/// </summary>
		internal FormAbout() {
			InitializeComponent();
			fSecretKeyManager.RegisterKeySequence(new SecretKeyManager.HiddenKeys("team", "show team", new EventHandler(ShowTeam)));
		}
#region Secret key related
		private void ShowTeam(object sender, EventArgs e) {
			fSecretKeyManager.ShowHelp();
        }

        private PictureBox pictureBox1;

		private SecretKeyManager fSecretKeyManager =
			new TeamKeyManager();

		private void FormAbout_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
			if (e.Alt) {
				LoggingService.Info("key is " + e.KeyCode.ToString());
				fSecretKeyManager.SendKey(e.KeyCode);
			}
		}
#endregion
		///<summary>
		///Cleans up any resources being used.
		///</summary>
		protected override void Dispose (bool disposing) {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btLicense;
		private System.Windows.Forms.Button btClose;
		private System.Windows.Forms.RichTextBox tbInfo;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbProduct;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.PictureBox pictureBox2;

#region Windows Form Designer generated code
        ///<summary>
        ///Required method for Designer support - do not modify
        ///the contents of this method with the code editor.
        ///</summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.tbInfo = new System.Windows.Forms.RichTextBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbProduct = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btClose = new System.Windows.Forms.Button();
            this.btLicense = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Coral;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(466, 24);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "Beautify Your Code, Delight Your Mind";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitle.DoubleClick += new System.EventHandler(this.lblTitle_DoubleClick);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
           
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.linkLabel1);
            this.panel1.Controls.Add(this.tbInfo);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.tbProduct);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(2);
            this.panel1.Size = new System.Drawing.Size(466, 200);
            this.panel1.TabIndex = 3;
            // 
            // linkLabel1
            // 
           
            this.linkLabel1.Location = new System.Drawing.Point(245, 168);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(100, 23);
            this.linkLabel1.TabIndex = 4;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Product Homepage";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel1LinkClicked);
            // 
            // tbInfo
            // 
            
            this.tbInfo.Location = new System.Drawing.Point(104, 80);
            this.tbInfo.Name = "tbInfo";
            this.tbInfo.ReadOnly = true;
            this.tbInfo.Size = new System.Drawing.Size(336, 80);
            this.tbInfo.TabIndex = 7;
            this.tbInfo.Text = "";
            // 
            // pictureBox2
            // 
            
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(16, 72);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(80, 112);
            this.pictureBox2.TabIndex = 6;
            this.pictureBox2.TabStop = false;
            // 
            // label2
            // 
            
            this.label2.Location = new System.Drawing.Point(104, 168);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 5;
            this.label2.Text = "Standard";
            // 
            // tbProduct
            // 
            
            this.tbProduct.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbProduct.Location = new System.Drawing.Point(16, 8);
            this.tbProduct.Multiline = true;
            this.tbProduct.Name = "tbProduct";
            this.tbProduct.ReadOnly = true;
            this.tbProduct.Size = new System.Drawing.Size(408, 40);
            this.tbProduct.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            
            this.label1.Location = new System.Drawing.Point(16, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Product Information:";
            // 
            // btClose
            // 
            this.btClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btClose.BackColor = System.Drawing.SystemColors.Control;
            this.btClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btClose.Location = new System.Drawing.Point(360, 232);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(75, 23);
            this.btClose.TabIndex = 0;
            this.btClose.Text = "Close";
            this.btClose.UseVisualStyleBackColor = false;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // btLicense
            // 
            this.btLicense.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btLicense.BackColor = System.Drawing.SystemColors.Control;
            this.btLicense.Location = new System.Drawing.Point(24, 232);
            this.btLicense.Name = "btLicense";
            this.btLicense.Size = new System.Drawing.Size(75, 23);
            this.btLicense.TabIndex = 1;
            this.btLicense.Text = "License";
            this.btLicense.UseVisualStyleBackColor = false;
            this.btLicense.Click += new System.EventHandler(this.btLicese_Click);
            // 
            // pictureBox1
            // 
            
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(351, 163);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(89, 32);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.btnDonate_Click);
            // 
            // FormAbout
            // 
            this.AcceptButton = this.btClose;
            this.CancelButton = this.btClose;
            this.ClientSize = new System.Drawing.Size(466, 264);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btClose);
            this.Controls.Add(this.btLicense);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAbout";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About Code Beautifier Collection";
            this.Load += new System.EventHandler(this.FormAbout_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormAbout_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

		}
		private System.Windows.Forms.LinkLabel linkLabel1;
#endregion

		private const string Product = "Code Beautifier Collection for CodeGear RAD Studio Version: {0} Copyright (C) 2005-2007 Lex Y. Li and other contributors. All Rights Reserved.";

		private readonly static string Info = 
			"Code Beautifier Collection Standard is copyrighted by Lex Y. Li and other contributors."
            + Environment.NewLine + Environment.NewLine
			+ "It consists of a bundle of assemblies. There is a Framework assembly, LeXDK assemblies, minuses, and pluses."
			+ Environment.NewLine + Environment.NewLine
			+ "Each assembly/plug-in is licensed under its respective license terms. "
			+ Environment.NewLine + Environment.NewLine
			+ "If there is something wrong or you have a problem, please contact me by sending a mail to lextudio@gmail.com."
			+ Environment.NewLine + Environment.NewLine
			+ "Click the License button to see details.";

		private void FormAbout_Load(object sender, System.EventArgs e)
		{
            tbProduct.Text = String.Format(CultureInfo.InvariantCulture, Product,
				ShareUtils.CoreVersion);
            tbInfo.Text = Info;
		}

		private void btLicese_Click(object sender, System.EventArgs e)
		{
			string file = Lextm.OpenTools.IO.Path.GetDocFile("License.pdf");
			Lextm.Diagnostics.ShellHelper.Execute(file);
			this.Close();
		}
		
		private void btClose_Click(object sender, System.EventArgs e)
		{
            this.Close();
		}
		
		private void lblTitle_DoubleClick(object sender, System.EventArgs e)
		{
            throw new CoreException("This is a test for UnhandledExceptionManager. Press 'Continue' to go on.");
		}

		
		void LinkLabel1LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string homepage = (string)PropertyRegistry.Get("CBCHomepage", "http://code.google.com/p/lextudio");
			ShellHelper.Execute(homepage);
		}

        private void btnDonate_Click(object sender, EventArgs e)
        {
            ShellHelper.Execute("http://sourceforge.net/donate/index.php?group_id=170248");//><img src="http://images.sourceforge.net/images/project-support.jpg" 
        }
	}
}
