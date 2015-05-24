using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using BeWise.SharpBuilderTools.Tools.Ant;

namespace BeWise.SharpBuilderTools.Gui.Ant {

	class FrmNAntRunner: Form {

        // *************************************************************************
        //                         Constructor / Destructor
        // *************************************************************************

		internal FrmNAntRunner() {
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
        //                             Private
        // *************************************************************************
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBox;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private System.ComponentModel.Container components = null;

        private BaseAnt fCurrentAnt;
        private BaseAntProject fProject;
        private string fSelectedTarget;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubTitle;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.RichTextBox txtFileName;

        private void InitializeComponent()
		{
			this.btOk = new System.Windows.Forms.Button();
			this.btCancel = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.lblTitle = new System.Windows.Forms.Label();
			this.lblSubTitle = new System.Windows.Forms.Label();
			this.lblFileName = new System.Windows.Forms.Label();
			this.comboBox = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtFileName = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// btOk
			// 
			this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btOk.Location = new System.Drawing.Point(285, 146);
			this.btOk.Name = "btOk";
			this.btOk.Size = new System.Drawing.Size(90, 23);
			this.btOk.TabIndex = 1;
			this.btOk.Text = "Ok";
			// 
			// btCancel
			// 
			this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btCancel.Location = new System.Drawing.Point(381, 146);
			this.btCancel.Name = "btCancel";
			this.btCancel.Size = new System.Drawing.Size(90, 23);
			this.btCancel.TabIndex = 2;
			this.btCancel.Text = "Cancel";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Location = new System.Drawing.Point(-10, 56);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(738, 2);
			this.groupBox2.TabIndex = 5;
			this.groupBox2.TabStop = false;
			// 
			// lblTitle
			// 
			this.lblTitle.BackColor = System.Drawing.Color.White;
			this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblTitle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTitle.ForeColor = System.Drawing.Color.Black;
			this.lblTitle.Location = new System.Drawing.Point(0, 0);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(504, 24);
			this.lblTitle.TabIndex = 3;
			this.lblTitle.Text = "    Ant Runner";
			this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblSubTitle
			// 
			this.lblSubTitle.BackColor = System.Drawing.Color.White;
			this.lblSubTitle.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblSubTitle.ForeColor = System.Drawing.Color.Black;
			this.lblSubTitle.Location = new System.Drawing.Point(0, 24);
			this.lblSubTitle.Name = "lblSubTitle";
			this.lblSubTitle.Size = new System.Drawing.Size(504, 32);
			this.lblSubTitle.TabIndex = 4;
			this.lblSubTitle.Text = "               Select a target to run";
			// 
			// lblFileName
			// 
			this.lblFileName.Location = new System.Drawing.Point(19, 64);
			this.lblFileName.Name = "lblFileName";
			this.lblFileName.Size = new System.Drawing.Size(125, 16);
			this.lblFileName.TabIndex = 6;
			this.lblFileName.Text = "Build file name:";
			// 
			// comboBox
			// 
			this.comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox.Location = new System.Drawing.Point(173, 104);
			this.comboBox.Name = "comboBox";
			this.comboBox.Size = new System.Drawing.Size(299, 21);
			this.comboBox.Sorted = true;
			this.comboBox.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(19, 107);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(125, 16);
			this.label1.TabIndex = 7;
			this.label1.Text = "Target:";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(0, 134);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(738, 2);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			this.groupBox1.SizeChanged += new System.EventHandler(this.groupBox1_SizeChanged);
			// 
			// txtFileName
			// 
			this.txtFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFileName.Location = new System.Drawing.Point(173, 61);
			this.txtFileName.Name = "txtFileName";
			this.txtFileName.ReadOnly = true;
			this.txtFileName.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.txtFileName.Size = new System.Drawing.Size(299, 20);
			this.txtFileName.TabIndex = 9;
			this.txtFileName.Text = "";
			// 
			// FrmNAntRunner
			// 
			this.AcceptButton = this.btOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.btCancel;
			this.ClientSize = new System.Drawing.Size(504, 179);
			this.Controls.Add(this.txtFileName);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.lblFileName);
			this.Controls.Add(this.comboBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lblSubTitle);
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.btCancel);
			this.Controls.Add(this.btOk);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.ForeColor = System.Drawing.Color.Black;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(512, 208);
			this.Name = "FrmNAntRunner";
			this.ResumeLayout(false);
		}

        // *************************************************************************
        //                         Protected
        // *************************************************************************

		private void LoadProjectInList() {
            comboBox.Items.Clear();

            for (int i=0; i < Project.Targets.Length; i++) {
                comboBox.Items.Add(Project.Targets[i].Name);
            }

            if (fSelectedTarget != null) {
                comboBox.SelectedIndex = comboBox.Items.IndexOf(fSelectedTarget);
            } else if (comboBox.Items.Count > 0) {
                comboBox.SelectedIndex = 0;
            }
        }

        // *************************************************************************
        //                            Public Properties
        // *************************************************************************

		internal BaseAnt CurrentAnt {
            get {
                return fCurrentAnt;
            }

            set {
				fCurrentAnt = value;

                if (value != null) {
                    lblTitle.Text = CurrentAnt.Name + " Runner";
                }
            }
        }

		internal BaseAntProject Project {
			get {
                return fProject;
            }
            set {
                fProject = value;
                LoadProjectInList();
                txtFileName.Text = fProject.FileName;
            }
        }

        internal string SelectedTarget {
            get {
                return comboBox.Text;
            }
            set {
                fSelectedTarget = value;
            }
        }

		private void groupBox1_SizeChanged(object sender, System.EventArgs e)
		{
            this.groupBox1.Height = 2;
		}
    }
}
