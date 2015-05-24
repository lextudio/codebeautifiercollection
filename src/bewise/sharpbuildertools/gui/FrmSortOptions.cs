using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

namespace BeWise.SharpBuilderTools.Gui {

	class FrmSortOptions : Form {

        /**************************************************************/
        /*                  Constructor / Destructor
        /**************************************************************/

		internal FrmSortOptions() {
            InitializeComponent();
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
        /*                        Private
        /**************************************************************/

        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbAscending;
        private System.Windows.Forms.RadioButton rbDescending;
        private System.Windows.Forms.CheckBox chkCaseSensitive;
        private System.Windows.Forms.Label lblSubTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox groupBox2;

#region Windows Form Designer generated code

        private void InitializeComponent()
		{
			this.btOk = new System.Windows.Forms.Button();
			this.btCancel = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.chkCaseSensitive = new System.Windows.Forms.CheckBox();
			this.rbDescending = new System.Windows.Forms.RadioButton();
			this.rbAscending = new System.Windows.Forms.RadioButton();
			this.lblSubTitle = new System.Windows.Forms.Label();
			this.lblTitle = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.SuspendLayout();
			// 
			// btOk
			// 
			this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btOk.Location = new System.Drawing.Point(214, 153);
			this.btOk.Name = "btOk";
			this.btOk.Size = new System.Drawing.Size(96, 24);
			this.btOk.TabIndex = 3;
			this.btOk.Text = "Ok";
			// 
			// btCancel
			// 
			this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btCancel.Location = new System.Drawing.Point(319, 153);
			this.btCancel.Name = "btCancel";
			this.btCancel.Size = new System.Drawing.Size(96, 24);
			this.btCancel.TabIndex = 4;
			this.btCancel.Text = "Cancel";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(0, 143);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(422, 2);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			// 
			// chkCaseSensitive
			// 
			this.chkCaseSensitive.Location = new System.Drawing.Point(192, 72);
			this.chkCaseSensitive.Name = "chkCaseSensitive";
			this.chkCaseSensitive.Size = new System.Drawing.Size(125, 16);
			this.chkCaseSensitive.TabIndex = 1;
			this.chkCaseSensitive.Text = "Case sensitive";
			// 
			// rbDescending
			// 
			this.rbDescending.Location = new System.Drawing.Point(19, 104);
			this.rbDescending.Name = "rbDescending";
			this.rbDescending.Size = new System.Drawing.Size(125, 24);
			this.rbDescending.TabIndex = 2;
			this.rbDescending.Text = "Descending";
			// 
			// rbAscending
			// 
			this.rbAscending.Checked = true;
			this.rbAscending.Location = new System.Drawing.Point(19, 72);
			this.rbAscending.Name = "rbAscending";
			this.rbAscending.Size = new System.Drawing.Size(125, 24);
			this.rbAscending.TabIndex = 0;
			this.rbAscending.TabStop = true;
			this.rbAscending.Text = "Ascending";
			// 
			// lblSubTitle
			// 
			this.lblSubTitle.BackColor = System.Drawing.Color.White;
			this.lblSubTitle.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblSubTitle.ForeColor = System.Drawing.Color.Black;
			this.lblSubTitle.Location = new System.Drawing.Point(0, 24);
			this.lblSubTitle.Name = "lblSubTitle";
			this.lblSubTitle.Size = new System.Drawing.Size(424, 32);
			this.lblSubTitle.TabIndex = 6;
			this.lblSubTitle.Text = "               Select the sorting options";
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
			this.lblTitle.TabIndex = 5;
			this.lblTitle.Text = "    Sort Text";
			this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Location = new System.Drawing.Point(-52, 56);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(528, 2);
			this.groupBox2.TabIndex = 24;
			this.groupBox2.TabStop = false;
			// 
			// FrmSortOptions
			// 
			this.AcceptButton = this.btOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.btCancel;
			this.ClientSize = new System.Drawing.Size(424, 184);
			this.Controls.Add(this.lblSubTitle);
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.chkCaseSensitive);
			this.Controls.Add(this.rbDescending);
			this.Controls.Add(this.rbAscending);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btOk);
			this.Controls.Add(this.btCancel);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FrmSortOptions";
			this.ResumeLayout(false);
		}
#endregion

		/// <summary>Ascending.</summary>
        internal bool Ascending {
            get {
				return rbAscending.Checked == true;
            }
        }
        /// <summary>Case sensitive.</summary>
		internal bool CaseSensitive {
            get {
                return chkCaseSensitive.Checked == true;
            }
        }

    }
}
