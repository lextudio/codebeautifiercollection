using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using BeWise.SharpBuilderTools;

namespace BeWise.SharpBuilderTools.Gui {
	/// <summary>
	/// Region creation form.
	/// </summary>
	class FrmCreateRegion : Form    {

        /**************************************************************/
        /*                  Constructor/ Destructor
        /**************************************************************/
		/// <summary>Constructor</summary>
		internal FrmCreateRegion() {
            InitializeComponent();
        }
        /// <summary>Disposes</summary>
        protected override void Dispose (bool disposing) {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /**************************************************************/
        /*                        Private
        /**************************************************************/

        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCreateRegion;
        private System.Windows.Forms.Label lblSubTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox groupBox2;

#region Windows Form Designer generated code

        private void InitializeComponent()
		{
			this.btOk = new System.Windows.Forms.Button();
			this.btCancel = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtCreateRegion = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lblSubTitle = new System.Windows.Forms.Label();
			this.lblTitle = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.SuspendLayout();
			// 
			// btOk
			// 
			this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btOk.Location = new System.Drawing.Point(147, 128);
			this.btOk.Name = "btOk";
			this.btOk.Size = new System.Drawing.Size(96, 24);
			this.btOk.TabIndex = 1;
			this.btOk.Text = "Ok";
			// 
			// btCancel
			// 
			this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btCancel.Location = new System.Drawing.Point(252, 128);
			this.btCancel.Name = "btCancel";
			this.btCancel.Size = new System.Drawing.Size(96, 24);
			this.btCancel.TabIndex = 2;
			this.btCancel.Text = "Cancel";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(0, 120);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(370, 2);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			// 
			// txtCreateRegion
			// 
			this.txtCreateRegion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtCreateRegion.Location = new System.Drawing.Point(96, 80);
			this.txtCreateRegion.Name = "txtCreateRegion";
			this.txtCreateRegion.Size = new System.Drawing.Size(250, 21);
			this.txtCreateRegion.TabIndex = 0;
			this.txtCreateRegion.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(19, 80);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(67, 16);
			this.label1.TabIndex = 7;
			this.label1.Text = "Region:";
			// 
			// lblSubTitle
			// 
			this.lblSubTitle.BackColor = System.Drawing.Color.White;
			this.lblSubTitle.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblSubTitle.ForeColor = System.Drawing.Color.Black;
			this.lblSubTitle.Location = new System.Drawing.Point(0, 24);
			this.lblSubTitle.Name = "lblSubTitle";
			this.lblSubTitle.Size = new System.Drawing.Size(360, 32);
			this.lblSubTitle.TabIndex = 4;
			this.lblSubTitle.Text = "               Enter the region name";
			// 
			// lblTitle
			// 
			this.lblTitle.BackColor = System.Drawing.Color.White;
			this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblTitle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTitle.ForeColor = System.Drawing.Color.Black;
			this.lblTitle.Location = new System.Drawing.Point(0, 0);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(360, 24);
			this.lblTitle.TabIndex = 3;
			this.lblTitle.Text = "    Create a region";
			this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Location = new System.Drawing.Point(0, 56);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(370, 2);
			this.groupBox2.TabIndex = 5;
			this.groupBox2.TabStop = false;
			// 
			// FrmCreateRegion
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(360, 158);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.lblSubTitle);
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this.txtCreateRegion);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btOk);
			this.Controls.Add(this.btCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MaximumSize = new System.Drawing.Size(500, 184);
			this.MinimumSize = new System.Drawing.Size(368, 184);
			this.Name = "FrmCreateRegion";
			this.ResumeLayout(false);
		}
#endregion

        /**************************************************************/
        /*                        Properties
        /**************************************************************/
        /// <summary>Region name</summary>
        internal string RegionText {
            get {
        		if (string.IsNullOrEmpty(txtCreateRegion.Text)) {
                    return "Undefined region name";
                } else {
                    return txtCreateRegion.Text;
                }
            }
        }
    }
}
