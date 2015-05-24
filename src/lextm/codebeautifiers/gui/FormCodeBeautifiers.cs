// This is the form codebeautifiers. Ported from SBT.
//		Some captions are changed from SBT's to CBC's.
// Copyright (C) 2005-2006  Lex Y. Li
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


namespace Lextm.CodeBeautifiers.Gui {
	/// <summary>
	///Beautifiers form.
	///</summary>
	sealed class FormCodeBeautifiers : System.Windows.Forms.Form {

		private FormCodeBeautifiers() {
			InitializeComponent();
		}

		private static FormCodeBeautifiers instance;
		private System.Windows.Forms.ProgressBar progressBar1;
		private MRG.Controls.UI.LoadingCircle loadingCircle1;

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
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblSubTitle;
        
#region Windows Form Designer generated code
        ///<summary>
        ///Required method for Designer support - do not modify
        ///the contents of this method with the code editor.
        ///</summary>
        private void InitializeComponent()
		{
        	this.lblTitle = new System.Windows.Forms.Label();
        	this.groupBox1 = new System.Windows.Forms.GroupBox();
        	this.progressBar1 = new System.Windows.Forms.ProgressBar();
        	this.lblSubTitle = new System.Windows.Forms.Label();
        	this.loadingCircle1 = new MRG.Controls.UI.LoadingCircle();
        	this.groupBox1.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// lblTitle
        	// 
        	this.lblTitle.BackColor = System.Drawing.Color.White;
        	this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
        	this.lblTitle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.lblTitle.ForeColor = System.Drawing.Color.Black;
        	this.lblTitle.Location = new System.Drawing.Point(0, 0);
        	this.lblTitle.Name = "lblTitle";
        	this.lblTitle.Size = new System.Drawing.Size(424, 24);
        	this.lblTitle.TabIndex = 1;
        	this.lblTitle.Text = "    Beautifying code";
        	this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        	this.lblTitle.Click += new System.EventHandler(this.lblSubTitle_Click);
        	// 
        	// groupBox1
        	// 
        	this.groupBox1.Controls.Add(this.progressBar1);
        	this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.groupBox1.Location = new System.Drawing.Point(0, 24);
        	this.groupBox1.Name = "groupBox1";
        	this.groupBox1.Size = new System.Drawing.Size(424, 64);
        	this.groupBox1.TabIndex = 2;
        	this.groupBox1.TabStop = false;
        	// 
        	// progressBar1
        	// 
        	this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
        	this.progressBar1.Location = new System.Drawing.Point(3, 41);
        	this.progressBar1.Name = "progressBar1";
        	this.progressBar1.Size = new System.Drawing.Size(418, 20);
        	this.progressBar1.TabIndex = 0;
        	// 
        	// lblSubTitle
        	// 
        	this.lblSubTitle.BackColor = System.Drawing.Color.White;
        	this.lblSubTitle.Dock = System.Windows.Forms.DockStyle.Top;
        	this.lblSubTitle.ForeColor = System.Drawing.Color.Black;
        	this.lblSubTitle.Location = new System.Drawing.Point(0, 24);
        	this.lblSubTitle.Name = "lblSubTitle";
        	this.lblSubTitle.Size = new System.Drawing.Size(424, 40);
        	this.lblSubTitle.TabIndex = 3;
        	this.lblSubTitle.Text = "               CBC is performing code formatting";
        	this.lblSubTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        	this.lblSubTitle.Click += new System.EventHandler(this.lblSubTitle_Click);
        	// 
        	// loadingCircle1
        	// 
        	this.loadingCircle1.Active = false;
        	this.loadingCircle1.BackColor = System.Drawing.Color.White;
        	this.loadingCircle1.Color = System.Drawing.Color.Gray;
        	this.loadingCircle1.InnerCircleRadius = 6;
        	this.loadingCircle1.Location = new System.Drawing.Point(56, 32);
        	this.loadingCircle1.Name = "loadingCircle1";
        	this.loadingCircle1.NumberSpoke = 8;
        	this.loadingCircle1.OuterCircleRadius = 7;
        	this.loadingCircle1.RotationSpeed = 80;
        	this.loadingCircle1.Size = new System.Drawing.Size(32, 23);
        	this.loadingCircle1.SpokeThickness = 4;
        	this.loadingCircle1.TabIndex = 4;
        	this.loadingCircle1.Text = "loadingCircle1";
        	// 
        	// FormCodeBeautifiers
        	// 
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
        	this.ClientSize = new System.Drawing.Size(424, 88);
        	this.Controls.Add(this.loadingCircle1);
        	this.Controls.Add(this.lblSubTitle);
        	this.Controls.Add(this.groupBox1);
        	this.Controls.Add(this.lblTitle);
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
        	this.MaximizeBox = false;
        	this.MinimizeBox = false;
        	this.Name = "FormCodeBeautifiers";
        	this.ShowInTaskbar = false;
        	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        	this.Text = "Working...";
        	this.TopMost = true;
        	this.Load += new System.EventHandler(this.FormCodeBeautifiers_Load);
        	this.groupBox1.ResumeLayout(false);
        	this.ResumeLayout(false);
		}
#endregion


		///<summary>
		///Singleton instance of the form.
		///</summary>
		internal static FormCodeBeautifiers getInstance( ) {
			lock(typeof(Lextm.CodeBeautifiers.Gui.FormCodeBeautifiers)){
			    if (instance == null)
			    {
					instance = new Lextm.CodeBeautifiers.Gui.FormCodeBeautifiers();
			    }
		    }
		    return instance;
        }
		
		private void lblSubTitle_Click(object sender, System.EventArgs e)
		{
            this.Hide();
		}
        /// <summary>Sets progress bar.</summary>
        /// <param name="count">Count</param>
		/// <param name="total">Total count</param>
		/// <remarks>Send two numbers and the progress bar value is the ratio.</remarks>
		internal static void SetProgress(int count, int total) {
			getInstance().progressBar1.Value =
				count / total;
		}

		private void FormCodeBeautifiers_Load(object sender, System.EventArgs e)
		{
            this.loadingCircle1.Active = true;			
		}
	}
}
