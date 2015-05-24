using System;
using System.Windows.Forms;
using Lextm.OpenTools;

namespace BeWise.SharpBuilderTools.Gui.AssemblyInfo {

    class FrmAssemblyInfoBuilderRunner : Form {

        // *************************************************************************
        //                            Constructor
        // *************************************************************************

		internal FrmAssemblyInfoBuilderRunner() {
            InitializeComponent();
        }

        // *************************************************************************
        //                            Destructor
        // *************************************************************************

        protected override void Dispose( bool disposing ) {
            if( disposing ) {
                if(components != null) {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

        // *************************************************************************
        //                            Private
        // *************************************************************************
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.Label lblSubTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbLanguage;

        private void btOk_Click(object sender, System.EventArgs e) {
            Close();
        }
        
        private void FrmAssemblyInfoBuilderRunner_Load(object sender, System.EventArgs e) {
            cbLanguage.SelectedIndex = 0;
        }

#region Windows Form Designer generated code

        private void InitializeComponent()
		{
			this.btOk = new System.Windows.Forms.Button();
			this.btCancel = new System.Windows.Forms.Button();
			this.lblSubTitle = new System.Windows.Forms.Label();
			this.lblTitle = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cbLanguage = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// btOk
			// 
			this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btOk.Location = new System.Drawing.Point(310, 169);
			this.btOk.Name = "btOk";
			this.btOk.Size = new System.Drawing.Size(90, 23);
			this.btOk.TabIndex = 6;
			this.btOk.Text = "Ok";
			this.btOk.Click += new System.EventHandler(this.btOk_Click);
			// 
			// btCancel
			// 
			this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btCancel.Location = new System.Drawing.Point(406, 169);
			this.btCancel.Name = "btCancel";
			this.btCancel.Size = new System.Drawing.Size(90, 23);
			this.btCancel.TabIndex = 7;
			this.btCancel.Text = "Cancel";
			this.btCancel.Click += new System.EventHandler(this.btOk_Click);
			// 
			// lblSubTitle
			// 
			this.lblSubTitle.BackColor = System.Drawing.Color.White;
			this.lblSubTitle.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblSubTitle.ForeColor = System.Drawing.Color.Black;
			this.lblSubTitle.Location = new System.Drawing.Point(0, 24);
			this.lblSubTitle.Name = "lblSubTitle";
			this.lblSubTitle.Size = new System.Drawing.Size(511, 32);
			this.lblSubTitle.TabIndex = 9;
			this.lblSubTitle.Text = "               Select the language used to generate the Assembly " +  
				"Info";
			// 
			// lblTitle
			// 
			this.lblTitle.BackColor = System.Drawing.Color.White;
			this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblTitle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTitle.ForeColor = System.Drawing.Color.Black;
			this.lblTitle.Location = new System.Drawing.Point(0, 0);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(511, 24);
			this.lblTitle.TabIndex = 8;
			this.lblTitle.Text = "    AssemblyInfo Builder Runner";
			this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox2
			// 
			this.groupBox2.Location = new System.Drawing.Point(-10, 56);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(711, 3);
			this.groupBox2.TabIndex = 10;
			this.groupBox2.TabStop = false;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox1.Location = new System.Drawing.Point(-10, 160);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(711, 3);
			this.groupBox1.TabIndex = 14;
			this.groupBox1.TabStop = false;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(29, 96);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 23);
			this.label1.TabIndex = 15;
			this.label1.Text = "Language:";
			// 
			// cbLanguage
			// 
			this.cbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbLanguage.Items.AddRange(new object[] {
						"C#",
						"Delphi"});
			this.cbLanguage.Location = new System.Drawing.Point(163, 96);
			this.cbLanguage.Name = "cbLanguage";
			this.cbLanguage.Size = new System.Drawing.Size(288, 21);
			this.cbLanguage.TabIndex = 16;
			// 
			// FrmAssemblyInfoBuilderRunner
			// 
			this.AcceptButton = this.btOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.btCancel;
			this.ClientSize = new System.Drawing.Size(511, 199);
			this.Controls.Add(this.cbLanguage);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.lblSubTitle);
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.btCancel);
			this.Controls.Add(this.btOk);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.ForeColor = System.Drawing.Color.Black;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FrmAssemblyInfoBuilderRunner";
			this.Load += new System.EventHandler(this.FrmAssemblyInfoBuilderRunner_Load);
			this.ResumeLayout(false);
		}
#endregion


        // *************************************************************************
        //                            Properties
        // *************************************************************************

        internal Language Language {
            get {
                if (cbLanguage.SelectedIndex == 0) {
                    return Language.CSharp;
                } else {
                    return Language.Delphi;
                }
            }
        }
    }
}
