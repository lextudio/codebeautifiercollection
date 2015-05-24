using SMS.Windows.Forms;
using System;
using System.IO;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Borland.Studio.ToolsAPI;
using BeWise.Common;
using BeWise.Common.Utils;

namespace BeWise.SharpBuilderTools.Gui.NUnit {

	class SecondPage : InteriorWizardPage {
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox namespTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox testClassTextBox;
        private System.Windows.Forms.Button testClassButton;
        private System.Windows.Forms.Label label2;
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox srcFolderTextBox;
        private System.Windows.Forms.Button srcFolderButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox testNameTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox setUpCheckBox;
        private System.Windows.Forms.CheckBox tearDownCheckBox;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private static string TEST_SOURCE_FOLDER;
		private static string TEST_NAMESPACE;
		private static string THE_CLASS_TO_TEST;
		private static string TEST_CLASS_NAME;
		internal static bool IS_SETUP_CHECKED = false;
        internal static bool IS_TEAR_DOWN_CHECKED = false;

		internal SecondPage() {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing ) {
            if( disposing ) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

#region Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SecondPage));
            this.label2 = new System.Windows.Forms.Label();
            this.srcFolderTextBox = new System.Windows.Forms.TextBox();
            this.srcFolderButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.namespTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.testClassTextBox = new System.Windows.Forms.TextBox();
            this.testClassButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.testNameTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.setUpCheckBox = new System.Windows.Forms.CheckBox();
            this.tearDownCheckBox = new System.Windows.Forms.CheckBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
            this.SuspendLayout();
            //
            // m_titleLabel
            //
            this.m_titleLabel.Name = "m_titleLabel";
            this.m_titleLabel.Text = "NUnit Test Fixture";
            //
            // m_subtitleLabel
            //
            this.m_subtitleLabel.Name = "m_subtitleLabel";
            this.m_subtitleLabel.Text = "Select the name of the new NUnit test fixture. Optionally, specify" +
                                        " the class and methods to be tested.";
            //
            // m_headerPanel
            //
            this.m_headerPanel.Name = "m_headerPanel";
            //
            // m_headerPicture
            //
            this.m_headerPicture.Image = ((System.Drawing.Image)(resources.GetObject("m_headerPicture.Image")));
            this.m_headerPicture.Name = "m_headerPicture";
            //
            // m_headerSeparator
            //
            this.m_headerSeparator.Name = "m_headerSeparator";
            //
            // label2
            //
            this.label2.Location = new System.Drawing.Point(8, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Source Folder:";
            //
            // srcFolderTextBox
            //
            this.srcFolderTextBox.Location = new System.Drawing.Point(87, 69);
            this.srcFolderTextBox.Name = "srcFolderTextBox";
            this.srcFolderTextBox.Size = new System.Drawing.Size(332, 21);
            this.srcFolderTextBox.TabIndex = 7;
            this.srcFolderTextBox.Text = "";
            this.srcFolderTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.srcFolderTextBox_Validating);
            //
            // srcFolderButton
            //
            this.srcFolderButton.Location = new System.Drawing.Point(438, 69);
            this.srcFolderButton.Name = "srcFolderButton";
            this.srcFolderButton.Size = new System.Drawing.Size(43, 20);
            this.srcFolderButton.TabIndex = 8;
            this.srcFolderButton.Text = "...";
            this.srcFolderButton.Click += new System.EventHandler(this.srcFolderButton_Click);
            //
            // label1
            //
            this.label1.Location = new System.Drawing.Point(8, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 18);
            this.label1.TabIndex = 9;
            this.label1.Text = "Namespace:";
            //
            // namespTextBox
            //
            this.namespTextBox.Location = new System.Drawing.Point(87, 98);
            this.namespTextBox.Name = "namespTextBox";
            this.namespTextBox.Size = new System.Drawing.Size(333, 21);
            this.namespTextBox.TabIndex = 10;
            this.namespTextBox.Text = "UnitTesting";
            this.namespTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.namespTextBox_Validating);
            //
            // label3
            //
            this.label3.Location = new System.Drawing.Point(7, 146);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 17);
            this.label3.TabIndex = 11;
            this.label3.Text = "Class to test:";
            //
            // testClassTextBox
            //
            this.testClassTextBox.Location = new System.Drawing.Point(86, 144);
            this.testClassTextBox.Name = "testClassTextBox";
            this.testClassTextBox.Size = new System.Drawing.Size(333, 21);
            this.testClassTextBox.TabIndex = 12;
            this.testClassTextBox.Text = "";
            this.testClassTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.testClassTextBox_Validating);
            //
            // testClassButton
            //
            this.testClassButton.Location = new System.Drawing.Point(437, 144);
            this.testClassButton.Name = "testClassButton";
            this.testClassButton.Size = new System.Drawing.Size(43, 20);
            this.testClassButton.TabIndex = 13;
            this.testClassButton.Text = "...";
            this.testClassButton.Click += new System.EventHandler(this.testClassButton_Click);
            //
            // groupBox1
            //
            this.groupBox1.Location = new System.Drawing.Point(10, 130);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(472, 2);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            //
            // groupBox2
            //
            this.groupBox2.Location = new System.Drawing.Point(9, 177);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(472, 2);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            //
            // label4
            //
            this.label4.Location = new System.Drawing.Point(11, 191);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 17);
            this.label4.TabIndex = 16;
            this.label4.Text = "Name:";
            //
            // testNameTextBox
            //
            this.testNameTextBox.Location = new System.Drawing.Point(86, 187);
            this.testNameTextBox.Name = "testNameTextBox";
            this.testNameTextBox.Size = new System.Drawing.Size(333, 21);
            this.testNameTextBox.TabIndex = 17;
            this.testNameTextBox.Text = "";
            this.testNameTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.testNameTextBox_Validating);
            //
            // label5
            //
            this.label5.Location = new System.Drawing.Point(9, 229);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(236, 17);
            this.label5.TabIndex = 18;
            this.label5.Text = "Which method stubs would you like to create?";
            //
            // setUpCheckBox
            //
            this.setUpCheckBox.Location = new System.Drawing.Point(87, 260);
            this.setUpCheckBox.Name = "setUpCheckBox";
            this.setUpCheckBox.Size = new System.Drawing.Size(76, 24);
            this.setUpCheckBox.TabIndex = 19;
            this.setUpCheckBox.Text = "SetUp()";
            this.setUpCheckBox.CheckedChanged += new System.EventHandler(this.setUpCheckBox_CheckedChanged);
            //
            // tearDownCheckBox
            //
            this.tearDownCheckBox.Location = new System.Drawing.Point(209, 260);
            this.tearDownCheckBox.Name = "tearDownCheckBox";
            this.tearDownCheckBox.Size = new System.Drawing.Size(85, 24);
            this.tearDownCheckBox.TabIndex = 20;
            this.tearDownCheckBox.Text = "TearDown()";
            this.tearDownCheckBox.CheckedChanged += new System.EventHandler(this.tearDownCheckBox_CheckedChanged);
            //
            // errorProvider1
            //
            this.errorProvider1.ContainerControl = this;
            this.errorProvider1.DataMember = "";
            //
            // SecondPage
            //
            this.Controls.Add(this.tearDownCheckBox);
            this.Controls.Add(this.setUpCheckBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.testNameTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.testClassButton);
            this.Controls.Add(this.testClassTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.namespTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.srcFolderButton);
            this.Controls.Add(this.srcFolderTextBox);
            this.Controls.Add(this.label2);
            this.Name = "SecondPage";
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.srcFolderTextBox, 0);
            this.Controls.SetChildIndex(this.srcFolderButton, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.namespTextBox, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.testClassTextBox, 0);
            this.Controls.SetChildIndex(this.testClassButton, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.m_headerPanel, 0);
            this.Controls.SetChildIndex(this.m_headerSeparator, 0);
            this.Controls.SetChildIndex(this.m_titleLabel, 0);
            this.Controls.SetChildIndex(this.m_subtitleLabel, 0);
            this.Controls.SetChildIndex(this.m_headerPicture, 0);
            this.Controls.SetChildIndex(this.groupBox2, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.testNameTextBox, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.setUpCheckBox, 0);
            this.Controls.SetChildIndex(this.tearDownCheckBox, 0);
            this.ResumeLayout(false);
        }
#endregion

        protected override bool OnSetActive() {
            if( !base.OnSetActive() )
			{    return false;       }

            // Setup the folder textbox
            IOTAProject _Project = OTAUtils.GetCurrentProject();
            if (_Project != null) {
                this.srcFolderTextBox.Text = Path.GetDirectoryName(_Project.FileName);
            } else {
                Lextm.Windows.Forms.MessageBoxFactory.Warn("Please open or create a project before using this NUnit TestFixture wizard.");
                // Close wizard
                this.Wizard.Close();
            }

            IOTAModule _Module = OTAUtils.GetCurrentModule();
            if (_Module != null) {
                this.testClassTextBox.Text = _Module.FileName;
                THE_CLASS_TO_TEST = _Module.FileName;
                SetupTestClassName();
            }

            // Enable both the Next and Back buttons on this page
            Wizard.SetWizardButtons( WizardButton.Back | WizardButton.Next );
            return true;
        }

        protected override string OnWizardNext() {
            TEST_SOURCE_FOLDER = this.srcFolderTextBox.Text;
            TEST_NAMESPACE = this.namespTextBox.Text;
            THE_CLASS_TO_TEST = this.testClassTextBox.Text;
            TEST_CLASS_NAME = this.testNameTextBox.Text;
            return "ThirdPage";
        }

        // DONE2 -ovkyr -cwpage2 Added text entry field validation
        protected override bool OnKillActive() {
            bool bValidFolder    = ValidateFolder();
            bool bValidNamespace = ValidateNamespace();
            bool bValidTestclass = ValidateTestclass();
            bool bValidTestname  = ValidateTestname();

            if (bValidFolder && bValidNamespace && bValidTestclass && bValidTestname)
                return true;
            else
                return false;
        }

        private void srcFolderButton_Click(object sender, System.EventArgs e) {
            if (srcFolderTextBox != null) {
                FolderBrowserDialog _FBDlg = new FolderBrowserDialog();

                _FBDlg.SelectedPath = srcFolderTextBox.Text;

                if (_FBDlg.ShowDialog() == DialogResult.OK) {
                    srcFolderTextBox.Text = _FBDlg.SelectedPath;
                    TEST_SOURCE_FOLDER = srcFolderTextBox.Text;
                }
            }
        }

        private void testClassButton_Click(object sender, System.EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string filter = "CSharp file (*.cs)|*.cs";
            openFileDialog.Filter = filter;
            openFileDialog.Title = "Select a CSharp File";
            IOTAModule _Module = OTAUtils.GetCurrentModule();
            if (_Module != null) {
                openFileDialog.InitialDirectory = _Module.FileSystem;
                openFileDialog.FileName = _Module.FileName;
                THE_CLASS_TO_TEST = _Module.FileName;
            } else {
                openFileDialog.InitialDirectory = srcFolderTextBox.Text;
                openFileDialog.FileName = testClassTextBox.Text;
                THE_CLASS_TO_TEST = testClassTextBox.Text;
            }
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                switch (openFileDialog.FilterIndex) {
                case 1:
                    testClassTextBox.Text = openFileDialog.FileName;
                    THE_CLASS_TO_TEST = testClassTextBox.Text;
                    SetupTestClassName();
                    break;
                }
            }
            THE_CLASS_TO_TEST = testClassTextBox.Text;
        }

        private void setUpCheckBox_CheckedChanged(object sender, System.EventArgs e) {
            if (setUpCheckBox.Checked == true)
                IS_SETUP_CHECKED = true;
            else
                IS_SETUP_CHECKED = false;
        }

        private void tearDownCheckBox_CheckedChanged(object sender, System.EventArgs e) {
            if (tearDownCheckBox.Checked == true)
			{    IS_TEAR_DOWN_CHECKED = true;
			}else  {
				IS_TEAR_DOWN_CHECKED = false;  }
        }


        private void srcFolderTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
            ValidateFolder();
        }

		private bool ValidateFolder() {
            bool bStatus = true;
            try {
                if ( srcFolderTextBox.Text.Length == 0 ) {
                    bStatus = false;
                    throw new Exception( "Please enter or select a project directory path." );
                } else if (!Directory.Exists(srcFolderTextBox.Text)) {
                    bStatus = false;
                    throw new Exception( "Please enter or select a valid project directory path." );
				} else  {
					errorProvider1.SetError( srcFolderTextBox, "" ); }
			} catch ( Exception ex ) {
                errorProvider1.SetError( srcFolderTextBox, ex.Message );
            }
            return bStatus;
        }

        private void namespTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
            ValidateNamespace();
        }

        private bool ValidateNamespace() {
            bool bStatus = true;
            try {
                if ( namespTextBox.Text.Length == 0 ) {
                    bStatus = false;
                    throw new Exception( "Please enter a namespace." );
				} else {
					errorProvider1.SetError( namespTextBox, "" );  }
            } catch( Exception ex ) {
				errorProvider1.SetError( namespTextBox, ex.Message );
            }
            return bStatus;
        }

        private void testClassTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
            ValidateTestclass();
        }

        private bool ValidateTestclass() {
            bool bStatus = true;
            try {
                if (testClassTextBox.Text.Length == 0) {
                    bStatus = false;
                    throw new Exception( "Please enter or select a project class file path." );
                } else if (!File.Exists(testClassTextBox.Text)) {
                    bStatus = false;
                    throw new Exception( "Please enter or select an existing project class file." );
                } else if (!testClassTextBox.Text.ToUpper().EndsWith(CommonConsts.CS_EXTENSION.ToUpper())) {
                    bStatus = false;
                    throw new Exception( "The file must contains a C# class." );
                } else if (OTAUtils.GetModuleInfoFromProject(OTAUtils.GetCurrentProject() ,SecondPage.GetClassToTest()) == null){
                    bStatus = false;
                    throw new Exception( "The file must be in the project to generate the unit test." );
				} else  {
                    errorProvider1.SetError(testClassTextBox, "");  }
            } catch( Exception ex ) {
				errorProvider1.SetError( testClassTextBox, ex.Message );
            }
            return bStatus;
        }

        private void testNameTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
            ValidateTestname();
        }

        private bool ValidateTestname() {
            bool bStatus = true;
            try {
                if (testNameTextBox.Text.Length == 0) {
                    bStatus = false;
                    throw new Exception( "Please enter a name for the test fixture\r\nclass to generate." );
				} else {
                    errorProvider1.SetError(testNameTextBox, "");   }
            } catch (Exception ex) {
                errorProvider1.SetError(testNameTextBox, ex.Message);
            }
            return bStatus;
        }

		internal static string GetTestSourceFolder() {
            return TEST_SOURCE_FOLDER;
        }

		internal static string GetTestNamespace() {
            return TEST_NAMESPACE;
        }

		internal static string GetClassToTest() {
            return THE_CLASS_TO_TEST;
        }

		internal static string GetTestClassName() {
            return TEST_CLASS_NAME;
        }

        private void SetupTestClassName() {
            TEST_CLASS_NAME = Path.GetFileNameWithoutExtension(THE_CLASS_TO_TEST)+"Test";
            this.testNameTextBox.Text = TEST_CLASS_NAME;
        }

        public static bool IsSetUpChecked() {
            return IS_SETUP_CHECKED;
        }

        public static bool IsTearDownChecked() {
            return IS_TEAR_DOWN_CHECKED;
        }


    }
}





