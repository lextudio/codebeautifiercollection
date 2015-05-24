using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Helpers;

namespace BeWise.SharpBuilderTools.Gui {
	/// <summary>
	/// Form to add a new class file.
	/// </summary>
	class FrmNewClassName : Form {

        /**************************************************************/
        /*                        Private
        /**************************************************************/
		/// <summary>
		/// Constructor.
		/// </summary>
		internal FrmNewClassName() {
            InitializeComponent();
            EnableControls();
        }

        /**************************************************************/
        /*                        Private
        /**************************************************************/

        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtClassName;
        private System.Windows.Forms.Label lblSubTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtNamespace;
        private System.Windows.Forms.Button btDirectory;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDirectory;
        private System.Windows.Forms.TextBox txtNewFileName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;

        private void btDirectory_Click(object sender, System.EventArgs e) {
            Vista_Api.FolderBrowserDialog _Dlg = new Vista_Api.FolderBrowserDialog();

            _Dlg.SelectedPath = txtDirectory.Text;

            if (_Dlg.ShowDialog() == DialogResult.OK) {
                txtDirectory.Text = _Dlg.SelectedPath;
            }
        }

        private void btOk_Click(object sender, System.EventArgs e) {
            if (!String.IsNullOrEmpty(txtDirectory.Text)) {
                if (!System.IO.Directory.Exists(txtDirectory.Text)) {
                    ValidationHelpers.ShowWarning("Specified Directory does not exists !");
                    this.DialogResult = DialogResult.None;
                }
            }
        }

        private void EnableControls() {
            btOk.Enabled = (!String.IsNullOrEmpty(txtClassName.Text)) 
            	&& (!String.IsNullOrEmpty(txtNamespace.Text));
        }

        private void txt_TextChanged(object sender, System.EventArgs e) {
            EnableControls();
        }

        private void InitializeComponent()
		{
			this.btOk = new System.Windows.Forms.Button();
			this.btCancel = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtClassName = new System.Windows.Forms.TextBox();
			this.lblSubTitle = new System.Windows.Forms.Label();
			this.lblTitle = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtNamespace = new System.Windows.Forms.TextBox();
			this.btDirectory = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.txtDirectory = new System.Windows.Forms.TextBox();
			this.txtNewFileName = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.SuspendLayout();
			// 
			// btOk
			// 
			this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btOk.Location = new System.Drawing.Point(422, 254);
			this.btOk.Name = "btOk";
			this.btOk.Size = new System.Drawing.Size(90, 23);
			this.btOk.TabIndex = 5;
			this.btOk.Text = "Ok";
			this.btOk.Click += new System.EventHandler(this.btOk_Click);
			// 
			// btCancel
			// 
			this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btCancel.Location = new System.Drawing.Point(518, 254);
			this.btCancel.Name = "btCancel";
			this.btCancel.Size = new System.Drawing.Size(90, 23);
			this.btCancel.TabIndex = 6;
			this.btCancel.Text = "Cancel";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(0, 241);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(624, 2);
			this.groupBox1.TabIndex = 12;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "New Filename";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(19, 208);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(96, 16);
			this.label1.TabIndex = 4;
			this.label1.Text = "Class name";
			// 
			// txtClassName
			// 
			this.txtClassName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtClassName.Location = new System.Drawing.Point(125, 200);
			this.txtClassName.Name = "txtClassName";
			this.txtClassName.Size = new System.Drawing.Size(365, 21);
			this.txtClassName.TabIndex = 4;
			this.txtClassName.Text = "";
			this.txtClassName.TextChanged += new System.EventHandler(this.txt_TextChanged);
			// 
			// lblSubTitle
			// 
			this.lblSubTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblSubTitle.BackColor = System.Drawing.Color.White;
			this.lblSubTitle.ForeColor = System.Drawing.Color.Black;
			this.lblSubTitle.Location = new System.Drawing.Point(0, 24);
			this.lblSubTitle.Name = "lblSubTitle";
			this.lblSubTitle.Size = new System.Drawing.Size(624, 32);
			this.lblSubTitle.TabIndex = 8;
			this.lblSubTitle.Text = "               Enter the new file name";
			// 
			// lblTitle
			// 
			this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblTitle.BackColor = System.Drawing.Color.White;
			this.lblTitle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTitle.ForeColor = System.Drawing.Color.Black;
			this.lblTitle.Location = new System.Drawing.Point(0, 0);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(624, 24);
			this.lblTitle.TabIndex = 7;
			this.lblTitle.Text = "    New class";
			this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Location = new System.Drawing.Point(-29, 56);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(672, 2);
			this.groupBox2.TabIndex = 15;
			this.groupBox2.TabStop = false;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(19, 176);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(96, 16);
			this.label3.TabIndex = 19;
			this.label3.Text = "Namespace";
			// 
			// txtNamespace
			// 
			this.txtNamespace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtNamespace.Location = new System.Drawing.Point(125, 168);
			this.txtNamespace.Name = "txtNamespace";
			this.txtNamespace.Size = new System.Drawing.Size(451, 21);
			this.txtNamespace.TabIndex = 3;
			this.txtNamespace.Text = "";
			this.txtNamespace.TextChanged += new System.EventHandler(this.txt_TextChanged);
			// 
			// btDirectory
			// 
			this.btDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btDirectory.Location = new System.Drawing.Point(586, 72);
			this.btDirectory.Name = "btDirectory";
			this.btDirectory.Size = new System.Drawing.Size(28, 16);
			this.btDirectory.TabIndex = 1;
			this.btDirectory.Text = "...";
			this.btDirectory.Click += new System.EventHandler(this.btDirectory_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(19, 80);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(96, 16);
			this.label4.TabIndex = 9;
			this.label4.Text = "Dir";
			// 
			// txtDirectory
			// 
			this.txtDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtDirectory.Location = new System.Drawing.Point(125, 72);
			this.txtDirectory.Name = "txtDirectory";
			this.txtDirectory.Size = new System.Drawing.Size(451, 21);
			this.txtDirectory.TabIndex = 0;
			this.txtDirectory.Text = "";
			this.txtDirectory.TextChanged += new System.EventHandler(this.txt_TextChanged);
			// 
			// txtNewFileName
			// 
			this.txtNewFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtNewFileName.Location = new System.Drawing.Point(125, 104);
			this.txtNewFileName.Name = "txtNewFileName";
			this.txtNewFileName.Size = new System.Drawing.Size(365, 21);
			this.txtNewFileName.TabIndex = 2;
			this.txtNewFileName.Text = "";
			this.txtNewFileName.TextChanged += new System.EventHandler(this.txt_TextChanged);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(19, 112);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(96, 16);
			this.label5.TabIndex = 10;
			this.label5.Text = "File name";
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Location = new System.Drawing.Point(0, 152);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(624, 2);
			this.groupBox3.TabIndex = 11;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "New Filename";
			// 
			// FrmNewClassName
			// 
			this.AcceptButton = this.btOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.btCancel;
			this.ClientSize = new System.Drawing.Size(624, 286);
			this.Controls.Add(this.btDirectory);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtDirectory);
			this.Controls.Add(this.txtNewFileName);
			this.Controls.Add(this.txtNamespace);
			this.Controls.Add(this.txtClassName);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lblSubTitle);
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btOk);
			this.Controls.Add(this.btCancel);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MaximumSize = new System.Drawing.Size(800, 312);
			this.MinimumSize = new System.Drawing.Size(0, 312);
			this.Name = "FrmNewClassName";
			this.ResumeLayout(false);
		}

        /**************************************************************/
        /*                        Protected
        /**************************************************************/
		/// <summary>
		/// Disposes.
		/// </summary>
		/// <param name="disposing">Disposing</param>
        protected override void Dispose (bool disposing) {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /**************************************************************/
        /*                        Properties
        /**************************************************************/
		/// <summary>
		/// Class name.
		/// </summary>
		internal string ClassName {
            get {
				return txtClassName.Text;
            }
        }
		/// <summary>
		/// Directory.
		/// </summary>
		internal string Directory {
            get {
                return txtDirectory.Text;
            }

            set {
                txtDirectory.Text = value;
            }
        }
		/// <summary>
		/// File name.
		/// </summary>
		internal string FileName {
            get {
                return System.IO.Path.Combine(txtDirectory.Text,
				                              txtNewFileName.Text);
            }
        }
		/// <summary>
		/// Namespace.
		/// </summary>
        internal string Namespace {
            get {
                return txtNamespace.Text;
            }
        }
    }

}
