using System;
using System.IO;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using Borland.Studio.ToolsAPI;
//using BeWise.SharpBuilderTools.Tools.Ant;
using BeWise.Common.Utils;
using System.Collections.Generic;

namespace BeWise.SharpBuilderTools.Gui.AssemblyInfo {

	class FrmAssemblyInfoMessage : Form {

        /**************************************************************/
        /*                     Constructor
        /**************************************************************/

        internal FrmAssemblyInfoMessage(List<string> aMessages) {
            InitializeComponent();

            txtResults.Lines = aMessages.ToArray();
        }

        protected override void Dispose (bool disposing) {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /**************************************************************/
        /*                     Private
        /**************************************************************/

        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblSubTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtResults;
        private System.Windows.Forms.Button btCancel;

#region Windows Form Designer generated code

        private void InitializeComponent()
		{
			this.btOk = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.lblSubTitle = new System.Windows.Forms.Label();
			this.lblTitle = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtResults = new System.Windows.Forms.TextBox();
			this.btCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btOk
			// 
			this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btOk.Location = new System.Drawing.Point(305, 207);
			this.btOk.Name = "btOk";
			this.btOk.Size = new System.Drawing.Size(90, 23);
			this.btOk.TabIndex = 0;
			this.btOk.Text = "Ok";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Location = new System.Drawing.Point(0, 48);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(499, 2);
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			// 
			// lblSubTitle
			// 
			this.lblSubTitle.BackColor = System.Drawing.Color.White;
			this.lblSubTitle.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblSubTitle.ForeColor = System.Drawing.Color.Black;
			this.lblSubTitle.Location = new System.Drawing.Point(0, 24);
			this.lblSubTitle.Name = "lblSubTitle";
			this.lblSubTitle.Size = new System.Drawing.Size(508, 24);
			this.lblSubTitle.TabIndex = 2;
			this.lblSubTitle.Text = "               Creation results ";
			// 
			// lblTitle
			// 
			this.lblTitle.BackColor = System.Drawing.Color.White;
			this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblTitle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTitle.ForeColor = System.Drawing.Color.Black;
			this.lblTitle.Location = new System.Drawing.Point(0, 0);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(508, 24);
			this.lblTitle.TabIndex = 1;
			this.lblTitle.Text = "    Assembly Info Creator";
			this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(10, 198);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(499, 2);
			this.groupBox1.TabIndex = 12;
			this.groupBox1.TabStop = false;
			// 
			// txtResults
			// 
			this.txtResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtResults.Location = new System.Drawing.Point(10, 56);
			this.txtResults.Multiline = true;
			this.txtResults.Name = "txtResults";
			this.txtResults.ReadOnly = true;
			this.txtResults.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtResults.Size = new System.Drawing.Size(489, 135);
			this.txtResults.TabIndex = 13;
			this.txtResults.Text = "";
			// 
			// btCancel
			// 
			this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btCancel.Location = new System.Drawing.Point(401, 207);
			this.btCancel.Name = "btCancel";
			this.btCancel.Size = new System.Drawing.Size(90, 23);
			this.btCancel.TabIndex = 14;
			this.btCancel.Text = "Cancel";
			// 
			// FrmAssemblyInfoMessage
			// 
			this.AcceptButton = this.btOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.btOk;
			this.ClientSize = new System.Drawing.Size(508, 238);
			this.Controls.Add(this.btCancel);
			this.Controls.Add(this.txtResults);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.lblSubTitle);
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this.btOk);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MinimumSize = new System.Drawing.Size(0, 267);
			this.Name = "FrmAssemblyInfoMessage";
			this.ResumeLayout(false);
		}
#endregion

    }
}
