using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using BeWise.SharpBuilderTools;
using Lextm.NFamily.Feature;
using BeWise.SharpBuilderTools.Tools.NDoc;

namespace BeWise.SharpBuilderTools.Gui {

	class FrmNDoc : Form    {

        /**************************************************************/
        /*                  Constructor/ Destructor
        /**************************************************************/

		internal FrmNDoc() {
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
        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblSubTitle;
		private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cbOutputType;
        private string fDocumenter = null;

        private void FrmNDoc_Load(object sender, System.EventArgs e) {
            cbOutputType.Items.Clear();
            foreach(string name in NDoc.NDocDocumentors) {
            	cbOutputType.Items.Add(name);
            }

            if (fDocumenter != null) {
                cbOutputType.SelectedIndex = cbOutputType.Items.IndexOf(fDocumenter);
            }

            if (cbOutputType.SelectedIndex < 0) {
				cbOutputType.SelectedIndex = cbOutputType.Items.IndexOf(NDoc.DefaultDocumentor);
            }
        }

#region Windows Form Designer generated code
        private void InitializeComponent()
		{
			this.btOk = new System.Windows.Forms.Button();
			this.btCancel = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lblSubTitle = new System.Windows.Forms.Label();
			this.lblTitle = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.cbOutputType = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// btOk
			// 
			this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btOk.Location = new System.Drawing.Point(305, 128);
			this.btOk.Name = "btOk";
			this.btOk.Size = new System.Drawing.Size(96, 24);
			this.btOk.TabIndex = 1;
			this.btOk.Text = "Ok";
			// 
			// btCancel
			// 
			this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btCancel.Location = new System.Drawing.Point(410, 128);
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
			this.groupBox1.Size = new System.Drawing.Size(528, 2);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(19, 79);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(106, 16);
			this.label1.TabIndex = 6;
			this.label1.Text = "Documenters:";
			// 
			// lblSubTitle
			// 
			this.lblSubTitle.BackColor = System.Drawing.Color.White;
			this.lblSubTitle.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblSubTitle.ForeColor = System.Drawing.Color.Black;
			this.lblSubTitle.Location = new System.Drawing.Point(0, 24);
			this.lblSubTitle.Name = "lblSubTitle";
			this.lblSubTitle.Size = new System.Drawing.Size(518, 32);
			this.lblSubTitle.TabIndex = 4;
			this.lblSubTitle.Text = "               Select an output type / Documenter";
			// 
			// lblTitle
			// 
			this.lblTitle.BackColor = System.Drawing.Color.White;
			this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblTitle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTitle.ForeColor = System.Drawing.Color.Black;
			this.lblTitle.Location = new System.Drawing.Point(0, 0);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(518, 24);
			this.lblTitle.TabIndex = 3;
			this.lblTitle.Text = "    NDoc output type";
			this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Location = new System.Drawing.Point(0, 56);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(528, 2);
			this.groupBox2.TabIndex = 5;
			this.groupBox2.TabStop = false;
			// 
			// cbOutputType
			// 
			this.cbOutputType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbOutputType.Items.AddRange(new object[] {
						"JavaDoc",
						"LaTeX",
						"MSDN",
						"Xml"});
			this.cbOutputType.Location = new System.Drawing.Point(125, 74);
			this.cbOutputType.Name = "cbOutputType";
			this.cbOutputType.Size = new System.Drawing.Size(355, 21);
			this.cbOutputType.Sorted = true;
			this.cbOutputType.TabIndex = 0;
			// 
			// FrmNDoc
			// 
			this.AcceptButton = this.btOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.btCancel;
			this.ClientSize = new System.Drawing.Size(518, 158);
			this.Controls.Add(this.cbOutputType);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.lblSubTitle);
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btOk);
			this.Controls.Add(this.btCancel);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MinimumSize = new System.Drawing.Size(322, 152);
			this.Name = "FrmNDoc";
			this.Load += new System.EventHandler(this.FrmNDoc_Load);
			this.ResumeLayout(false);
		}
#endregion

        /**************************************************************/
        /*                        Properties
        /**************************************************************/

        internal string Documenter {
            get {
                if (cbOutputType.Text == String.Empty) {
                    return NDoc.DefaultDocumentor;
                } else {
                    return cbOutputType.Text;
                }
            }

            set {
				fDocumenter = value;
            }
		}
    }
}
