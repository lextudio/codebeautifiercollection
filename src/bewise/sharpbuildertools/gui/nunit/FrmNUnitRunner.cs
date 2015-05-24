using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Borland.Studio.ToolsAPI;
using BeWise.Common.Utils;

namespace BeWise.SharpBuilderTools.Gui.NUnit {
	/// <summary>
	/// Form to run NUnit.
	/// </summary>
	class FrmNUnitRunner : Form {

        // *************************************************************************
        //                         Constructor / Destructor
        // *************************************************************************
		/// <summary>
		/// Constructor.
		/// </summary>
		internal FrmNUnitRunner() {
            InitializeComponent();
        }
		/// <summary>
		/// Disposes.
		/// </summary>
		/// <param name="disposing">Disposing</param>
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
        private System.Windows.Forms.ComboBox cbAssembly;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblSubTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cbTestFixture;
        private System.Windows.Forms.Label label3;

        private void cbAssembly_SelectedIndexChanged(object sender, System.EventArgs e) {
            LoadTestFixtureInList();
            EnabledControls();
        }

        private void cbTestFixture_SelectedIndexChanged(object sender, System.EventArgs e) {
            EnabledControls();
        }

        private void InitializeComponent()
		{
			this.btOk = new System.Windows.Forms.Button();
			this.btCancel = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lblSubTitle = new System.Windows.Forms.Label();
			this.lblTitle = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.cbAssembly = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cbTestFixture = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btOk
			// 
			this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btOk.Location = new System.Drawing.Point(319, 169);
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
			this.btCancel.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.btCancel.Location = new System.Drawing.Point(415, 169);
			this.btCancel.Name = "btCancel";
			this.btCancel.Size = new System.Drawing.Size(90, 23);
			this.btCancel.TabIndex = 2;
			this.btCancel.Text = "Cancel";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(0, 159);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(550, 2);
			this.groupBox1.TabIndex = 20;
			this.groupBox1.TabStop = false;
			// 
			// lblSubTitle
			// 
			this.lblSubTitle.BackColor = System.Drawing.Color.White;
			this.lblSubTitle.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblSubTitle.ForeColor = System.Drawing.Color.Black;
			this.lblSubTitle.Location = new System.Drawing.Point(0, 24);
			this.lblSubTitle.Name = "lblSubTitle";
			this.lblSubTitle.Size = new System.Drawing.Size(520, 32);
			this.lblSubTitle.TabIndex = 23;
			this.lblSubTitle.Text = "               Select a Test Fixture";
			// 
			// lblTitle
			// 
			this.lblTitle.BackColor = System.Drawing.Color.White;
			this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblTitle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTitle.ForeColor = System.Drawing.Color.Black;
			this.lblTitle.Location = new System.Drawing.Point(0, 0);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(520, 24);
			this.lblTitle.TabIndex = 22;
			this.lblTitle.Text = "    NUnit Runner";
			this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Location = new System.Drawing.Point(-19, 56);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(549, 2);
			this.groupBox2.TabIndex = 21;
			this.groupBox2.TabStop = false;
			// 
			// cbAssembly
			// 
			this.cbAssembly.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cbAssembly.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbAssembly.Location = new System.Drawing.Point(134, 80);
			this.cbAssembly.Name = "cbAssembly";
			this.cbAssembly.Size = new System.Drawing.Size(375, 21);
			this.cbAssembly.Sorted = true;
			this.cbAssembly.TabIndex = 25;
			this.cbAssembly.SelectedIndexChanged += new System.EventHandler(this.cbAssembly_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(25, 82);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(125, 16);
			this.label1.TabIndex = 24;
			this.label1.Text = "Assembly:";
			// 
			// cbTestFixture
			// 
			this.cbTestFixture.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cbTestFixture.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbTestFixture.Location = new System.Drawing.Point(134, 120);
			this.cbTestFixture.Name = "cbTestFixture";
			this.cbTestFixture.Size = new System.Drawing.Size(375, 21);
			this.cbTestFixture.Sorted = true;
			this.cbTestFixture.TabIndex = 27;
			this.cbTestFixture.SelectedIndexChanged += new System.EventHandler(this.cbTestFixture_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(25, 123);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(125, 16);
			this.label3.TabIndex = 26;
			this.label3.Text = "Test Fixture:";
			// 
			// FrmNUnitRunner
			// 
			this.AcceptButton = this.btOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.btCancel;
			this.ClientSize = new System.Drawing.Size(520, 199);
			this.Controls.Add(this.cbTestFixture);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.cbAssembly);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lblSubTitle);
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btCancel);
			this.Controls.Add(this.btOk);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.ForeColor = System.Drawing.Color.Black;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FrmNUnitRunner";
			this.Load += new System.EventHandler(this.FrmNUnitRunner_Load);
			this.ResumeLayout(false);
		}

        private void FrmNUnitRunner_Load(object sender, System.EventArgs e) {
            LoadAssembliesInList();
            EnabledControls();
        }

        private string[] GetProjectGroupTargets() {
            ArrayList _List = new ArrayList();
            IOTAProjectGroup _ProjectGroup = OTAUtils.GetCurrentProjectGroup();

            if (_ProjectGroup != null) {
                for(int i = 0; i < _ProjectGroup.ProjectCount; i++) {
                    IOTAProject _Project = _ProjectGroup[i];

                    string _Target = OTAUtils.GetProjectTarget(_Project);

                    if (_Target != null) {
                        _List.Add(_Target);
                    }
                }
            }

            return (string[]) _List.ToArray(typeof(string));
        }

        // *************************************************************************
        //                         Protected
        // *************************************************************************
		/// <summary>
		/// Enables controls.
		/// </summary>
        private void EnabledControls() {
            btOk.Enabled = cbAssembly.Text != "" && cbTestFixture.Text != "";
        }
		/// <summary>
		/// Loads assemblies in list.
		/// </summary>
        private void LoadAssembliesInList() {
            cbAssembly.Items.Clear();

            // Load Targets
            string[] _Assemblies = GetProjectGroupTargets();
//TODO: project option
//            for (int j=0; j < OptionManager.ProjectGroupOptions.UnitTestAssemblies.Length; j++) {
//                cbAssembly.Items.Add(_Assemblies[j]);
//            }

            // Load Unit Test Assemblies
//            for (int i=0; i < OptionManager.ProjectGroupOptions.UnitTestAssemblies.Length; i++) {
//                cbAssembly.Items.Add(OptionManager.ProjectGroupOptions.UnitTestAssemblies[i]);
//            }

            if (cbAssembly.Items.Count > 0) {
                cbAssembly.SelectedIndex = 0;
            }
        }

        private void LoadTestFixtureInList() {
            string[] _TestFixtures = BeWise.SharpBuilderTools.Tools.NUnit.NUnit.GetTestFixtures(SelectedAssemblyName);

            cbTestFixture.Items.Clear();

            for (int i=0; i < _TestFixtures.Length; i++) {
                cbTestFixture.Items.Add(_TestFixtures[i]);
            }

            if (cbTestFixture.Items.Count > 0) {
                cbTestFixture.SelectedIndex = 0;
            }
        }

        // *************************************************************************
        //                         Protected
        // *************************************************************************
		/// <summary>
		/// Selected assembly name.
		/// </summary>
        internal string SelectedAssemblyName {
            get {
				return cbAssembly.Text;
            }

            set {
                LoadAssembliesInList();
                cbAssembly.SelectedIndex = cbAssembly.Items.IndexOf(value);
            }
        }
		/// <summary>
		/// Selected test fixture name.
		/// </summary>
        public string SelectedTestFixtureName {
            get {
                return cbTestFixture.Text;
            }
            set {
                cbTestFixture.SelectedIndex = cbTestFixture.Items.IndexOf(value);
            }
        }
    }
}
