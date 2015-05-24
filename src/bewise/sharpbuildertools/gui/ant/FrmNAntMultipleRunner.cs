using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using BeWise.SharpBuilderTools.Tools.Ant;

namespace BeWise.SharpBuilderTools.Gui.Ant {

	class FrmNAntMultipleRunner: Form {

        // *************************************************************************
        //                            Constructor
        // *************************************************************************

		internal FrmNAntMultipleRunner() {
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btAdd;
        private System.Windows.Forms.Button btRemove;
        private System.Windows.Forms.ListBox lbSource;
        private System.Windows.Forms.ListBox lbToRun;
        private System.Windows.Forms.Button btDown;
        private System.Windows.Forms.Button btUp;

        private System.ComponentModel.Container components = null;

        private BaseAnt fCurrentAnt;
        private BaseAntProject fProject;
        private string[] fSelectedTargets;
        private System.Windows.Forms.Label lblSubTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblFileName;

#region Windows Form Designer generated code

        private void InitializeComponent()
		{
			this.btOk = new System.Windows.Forms.Button();
			this.btCancel = new System.Windows.Forms.Button();
			this.lblSubTitle = new System.Windows.Forms.Label();
			this.lblTitle = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.btDown = new System.Windows.Forms.Button();
			this.btUp = new System.Windows.Forms.Button();
			this.btRemove = new System.Windows.Forms.Button();
			this.btAdd = new System.Windows.Forms.Button();
			this.lbToRun = new System.Windows.Forms.ListBox();
			this.lbSource = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lblFileName = new System.Windows.Forms.Label();
			this.txtFileName = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.SuspendLayout();
			// 
			// btOk
			// 
			this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btOk.Location = new System.Drawing.Point(454, 378);
			this.btOk.Name = "btOk";
			this.btOk.Size = new System.Drawing.Size(90, 23);
			this.btOk.TabIndex = 6;
			this.btOk.Text = "Ok";
			// 
			// btCancel
			// 
			this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btCancel.Location = new System.Drawing.Point(550, 378);
			this.btCancel.Name = "btCancel";
			this.btCancel.Size = new System.Drawing.Size(90, 23);
			this.btCancel.TabIndex = 7;
			this.btCancel.Text = "Cancel";
			// 
			// lblSubTitle
			// 
			this.lblSubTitle.BackColor = System.Drawing.Color.White;
			this.lblSubTitle.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblSubTitle.ForeColor = System.Drawing.Color.Black;
			this.lblSubTitle.Location = new System.Drawing.Point(0, 24);
			this.lblSubTitle.Name = "lblSubTitle";
			this.lblSubTitle.Size = new System.Drawing.Size(655, 32);
			this.lblSubTitle.TabIndex = 9;
			this.lblSubTitle.Text = "               Select targets to run (targets will in the selecte" +  
				"d order)";
			// 
			// lblTitle
			// 
			this.lblTitle.BackColor = System.Drawing.Color.White;
			this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblTitle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTitle.ForeColor = System.Drawing.Color.Black;
			this.lblTitle.Location = new System.Drawing.Point(0, 0);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(655, 24);
			this.lblTitle.TabIndex = 8;
			this.lblTitle.Text = "    Ant Runner";
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
			// btDown
			// 
			this.btDown.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btDown.Location = new System.Drawing.Point(298, 208);
			this.btDown.Name = "btDown";
			this.btDown.Size = new System.Drawing.Size(57, 24);
			this.btDown.TabIndex = 4;
			this.btDown.Text = "Down";
			// 
			// btUp
			// 
			this.btUp.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btUp.Location = new System.Drawing.Point(298, 176);
			this.btUp.Name = "btUp";
			this.btUp.Size = new System.Drawing.Size(57, 24);
			this.btUp.TabIndex = 3;
			this.btUp.Text = "Up";
			// 
			// btRemove
			// 
			this.btRemove.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btRemove.Location = new System.Drawing.Point(298, 144);
			this.btRemove.Name = "btRemove";
			this.btRemove.Size = new System.Drawing.Size(57, 24);
			this.btRemove.TabIndex = 2;
			this.btRemove.Text = "-";
			// 
			// btAdd
			// 
			this.btAdd.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btAdd.Location = new System.Drawing.Point(298, 112);
			this.btAdd.Name = "btAdd";
			this.btAdd.Size = new System.Drawing.Size(57, 24);
			this.btAdd.TabIndex = 1;
			this.btAdd.Text = "+";
			// 
			// lbToRun
			// 
			this.lbToRun.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left)));
			this.lbToRun.HorizontalScrollbar = true;
			this.lbToRun.Location = new System.Drawing.Point(365, 115);
			this.lbToRun.Name = "lbToRun";
			this.lbToRun.Size = new System.Drawing.Size(278, 225);
			this.lbToRun.TabIndex = 5;
			this.lbToRun.DoubleClick += new System.EventHandler(this.lbToRun_DoubleClick);
			// 
			// lbSource
			// 
			this.lbSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left)));
			this.lbSource.HorizontalScrollbar = true;
			this.lbSource.Location = new System.Drawing.Point(10, 115);
			this.lbSource.Name = "lbSource";
			this.lbSource.Size = new System.Drawing.Size(278, 225);
			this.lbSource.Sorted = true;
			this.lbSource.TabIndex = 0;
			this.lbSource.DoubleClick += new System.EventHandler(this.lbSource_DoubleClick);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(10, 96);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(124, 16);
			this.label1.TabIndex = 13;
			this.label1.Text = "Available targets:";
			// 
			// lblFileName
			// 
			this.lblFileName.Location = new System.Drawing.Point(10, 72);
			this.lblFileName.Name = "lblFileName";
			this.lblFileName.Size = new System.Drawing.Size(124, 16);
			this.lblFileName.TabIndex = 11;
			this.lblFileName.Text = "Build file name:";
			// 
			// txtFileName
			// 
			this.txtFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFileName.Location = new System.Drawing.Point(144, 72);
			this.txtFileName.Name = "txtFileName";
			this.txtFileName.ReadOnly = true;
			this.txtFileName.Size = new System.Drawing.Size(499, 21);
			this.txtFileName.TabIndex = 12;
			this.txtFileName.TabStop = false;
			this.txtFileName.Text = "";
			// 
			// groupBox1
			// 
			this.groupBox1.Location = new System.Drawing.Point(-10, 368);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(711, 3);
			this.groupBox1.TabIndex = 14;
			this.groupBox1.TabStop = false;
			// 
			// FrmNAntMultipleRunner
			// 
			this.AcceptButton = this.btOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.btCancel;
			this.ClientSize = new System.Drawing.Size(655, 408);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btDown);
			this.Controls.Add(this.btUp);
			this.Controls.Add(this.btRemove);
			this.Controls.Add(this.btAdd);
			this.Controls.Add(this.lbToRun);
			this.Controls.Add(this.lbSource);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lblFileName);
			this.Controls.Add(this.txtFileName);
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
			this.Name = "FrmNAntMultipleRunner";
			this.ResumeLayout(false);
		}
#endregion

        private void btAdd_Click(object sender, System.EventArgs e) {
            if (lbSource.SelectedItem != null) {
                lbToRun.Items.Add(lbSource.SelectedItem);
                lbSource.Items.Remove(lbSource.SelectedItem);
            }
        }

        private void btRemove_Click(object sender, System.EventArgs e) {
            if (lbToRun.SelectedItem != null) {
                lbSource.Items.Add(lbToRun.SelectedItem);
                lbToRun.Items.Remove(lbToRun.SelectedItem);
            }
        }

        private void btUp_Click(object sender, System.EventArgs e) {
            if (lbToRun.SelectedItem != null && lbToRun.SelectedIndex != 0) {
                int _Sel = lbToRun.SelectedIndex;

                lbToRun.Items.Insert(lbToRun.SelectedIndex -1, lbToRun.SelectedItem);
                lbToRun.Items.RemoveAt(lbToRun.SelectedIndex);

                lbToRun.SelectedIndex = _Sel - 1;
            }
        }

        private void btDown_Click(object sender, System.EventArgs e) {
            if (lbToRun.SelectedItem != null && lbToRun.SelectedIndex != (lbToRun.Items.Count -1)) {
                lbToRun.Items.Insert(lbToRun.SelectedIndex, lbToRun.Items[lbToRun.SelectedIndex + 1]);
                lbToRun.Items.RemoveAt(lbToRun.SelectedIndex + 1);
            }
        }

        private void lbSource_DoubleClick(object sender, System.EventArgs e) {
            btAdd_Click(sender, null);
        }

        private void lbToRun_DoubleClick(object sender, System.EventArgs e) {
            btRemove_Click(sender, null);
        }

        private void LoadProjectInList() {
            lbSource.Items.Clear();

            for (int i = 0; i < Project.Targets.Length; i++) {
                lbSource.Items.Add(Project.Targets[i].Name);
            }
        }
        // *************************************************************************
        //                            Properties
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

        internal string[] SelectedTargets {
            get {
                ArrayList _Arr = new ArrayList();

                for (int i = 0; i < lbToRun.Items.Count; i++) {
                    _Arr.Add(lbToRun.Items[i]);
                }

                string[] _res = new string[_Arr.Count];

                for (int j = 0; j < _Arr.Count; j ++) {
                    _res[j] = _Arr[j] as string;
                }

                return _res;
            }
            set {
                fSelectedTargets = value;
            }
        }
    }
}
