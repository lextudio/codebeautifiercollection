using SMS.Windows.Forms;
using System;
using System.CodeDom;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Borland.Studio.ToolsAPI;
using BeWise.Common.Info;
using BeWise.Common.Utils;
using Lextm.LeXDK.CodeDom;

namespace BeWise.SharpBuilderTools.Gui.NUnit {

	class ThirdPage : InteriorWizardPage {
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox methodCheckedListBox;
        private System.Windows.Forms.Button selectAllButton;
        private System.Windows.Forms.Button deselectAllButton;
        private System.Windows.Forms.Label classNameLabel;
        private static ArrayList TEST_METHOD_LIST;

		internal ThirdPage() {
            InitializeComponent();
        }

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
        private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ThirdPage));
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.label1 = new System.Windows.Forms.Label();
			this.methodCheckedListBox = new System.Windows.Forms.CheckedListBox();
			this.selectAllButton = new System.Windows.Forms.Button();
			this.deselectAllButton = new System.Windows.Forms.Button();
			this.classNameLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_titleLabel
			// 
			this.m_titleLabel.Name = "m_titleLabel";
			this.m_titleLabel.Text = "NUnit Test Methods";
			// 
			// m_subtitleLabel
			// 
			this.m_subtitleLabel.Name = "m_subtitleLabel";
			this.m_subtitleLabel.Text = "Select methods for which test method stubs should be generated.";
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
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			this.errorProvider1.DataMember = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 80);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 16);
			this.label1.TabIndex = 5;
			this.label1.Text = "Available methods for:";
			// 
			// methodCheckedListBox
			// 
			this.methodCheckedListBox.CheckOnClick = true;
			this.methodCheckedListBox.Location = new System.Drawing.Point(16, 112);
			this.methodCheckedListBox.Name = "methodCheckedListBox";
			this.methodCheckedListBox.Size = new System.Drawing.Size(368, 132);
			this.methodCheckedListBox.TabIndex = 6;
			// 
			// selectAllButton
			// 
			this.selectAllButton.Location = new System.Drawing.Point(400, 120);
			this.selectAllButton.Name = "selectAllButton";
			this.selectAllButton.Size = new System.Drawing.Size(88, 23);
			this.selectAllButton.TabIndex = 7;
			this.selectAllButton.Text = "Select All";
			this.selectAllButton.Click += new System.EventHandler(this.selectAllButton_Click);
			// 
			// deselectAllButton
			// 
			this.deselectAllButton.Location = new System.Drawing.Point(400, 160);
			this.deselectAllButton.Name = "deselectAllButton";
			this.deselectAllButton.Size = new System.Drawing.Size(88, 23);
			this.deselectAllButton.TabIndex = 8;
			this.deselectAllButton.Text = "Deselect All";
			this.deselectAllButton.Click += new System.EventHandler(this.deselectAllButton_Click);
			// 
			// classNameLabel
			// 
			this.classNameLabel.BackColor = System.Drawing.SystemColors.Control;
			this.classNameLabel.Location = new System.Drawing.Point(144, 80);
			this.classNameLabel.Name = "classNameLabel";
			this.classNameLabel.Size = new System.Drawing.Size(320, 16);
			this.classNameLabel.TabIndex = 9;
			this.classNameLabel.Text = "classxyz";
			// 
			// ThirdPage
			// 
			this.Controls.Add(this.classNameLabel);
			this.Controls.Add(this.deselectAllButton);
			this.Controls.Add(this.selectAllButton);
			this.Controls.Add(this.methodCheckedListBox);
			this.Controls.Add(this.label1);
			this.Name = "ThirdPage";
			this.Controls.SetChildIndex(this.label1, 0);
			this.Controls.SetChildIndex(this.methodCheckedListBox, 0);
			this.Controls.SetChildIndex(this.selectAllButton, 0);
			this.Controls.SetChildIndex(this.deselectAllButton, 0);
			this.Controls.SetChildIndex(this.classNameLabel, 0);
			this.Controls.SetChildIndex(this.m_headerPanel, 0);
			this.Controls.SetChildIndex(this.m_headerSeparator, 0);
			this.Controls.SetChildIndex(this.m_titleLabel, 0);
			this.Controls.SetChildIndex(this.m_subtitleLabel, 0);
			this.Controls.SetChildIndex(this.m_headerPicture, 0);
			this.ResumeLayout(false);
		}
#endregion

        protected override bool OnSetActive() {
            if( !base.OnSetActive() )
            {    return false;}

            string theClass = SecondPage.GetClassToTest();
            theClass = Path.GetFileNameWithoutExtension(theClass);
            this.classNameLabel.Text = theClass;

            ArrayList _List = null;
            _List = new ArrayList();

            // Determines the actual project.
            IOTAProject _Project = OTAUtils.GetCurrentProject();

            if (_Project != null) {
                // Loads the given modul via the OTAUtils.
                IOTAModuleInfo _ModuleInfo = OTAUtils.GetModuleInfoFromProject(_Project ,SecondPage.GetClassToTest());
				if (_ModuleInfo == null)
				{
                    return false;
                }
				IOTAModule _Module = _ModuleInfo.OpenModule();

                // Loads the methods into the array list for the given module
                // and sets up the method checked list box.
                CodeDomProvider.LoadMethodInfoInto(_List, _Module);
                this.methodCheckedListBox.Items.Clear();
                for (int i = 0; i < _List.Count; i++) {
                    MethodInfo _MethodInfo = (MethodInfo) _List[i];
                    //TODO: this .Name may not be correct.
                    this.methodCheckedListBox.Items.Add(_MethodInfo.Name);
                }

                // Enable both the Next and Back buttons on this page
                Wizard.SetWizardButtons( WizardButton.Back | WizardButton.Next );
            } else {
                Lextm.Windows.Forms.MessageBoxFactory.Warn("Please open or create a project before using this NUnit TestFixture wizard.");
                // Close wizard
                this.Wizard.Close();
            }
            return true;
        }

        protected override string OnWizardNext() {
            DetermineSelectedMethods();
            return "FourthPage";
        }

        private void selectAllButton_Click(object sender, System.EventArgs e) {
            // Determine if there are any items.
            if(methodCheckedListBox.Items.Count != 0) {
                // If so, loop through all checked items and check them.
                for(int x = 0; x <= methodCheckedListBox.Items.Count - 1 ; x++) {
                    methodCheckedListBox.SetItemChecked(x, true);
                }
            }
        }

        private void deselectAllButton_Click(object sender, System.EventArgs e) {
            // Determine if there are any items.
            if(methodCheckedListBox.Items.Count != 0) {
                // If so, loop through all checked items and uncheck them.
                for(int x = 0; x <= methodCheckedListBox.Items.Count - 1 ; x++) {
                    methodCheckedListBox.SetItemChecked(x, false);
                }
            }
        }

        private void DetermineSelectedMethods() {
            TEST_METHOD_LIST = null;
            TEST_METHOD_LIST = new ArrayList();
            // Determine if there are any items checked.
            if(methodCheckedListBox.CheckedItems.Count != 0) {
                // If so, loop through all checked items and print results.
                for(int x = 0; x <= methodCheckedListBox.CheckedItems.Count - 1 ; x++) {
                    TEST_METHOD_LIST.Add(methodCheckedListBox.CheckedItems[x].ToString());
                }
            }
            if (SecondPage.IS_SETUP_CHECKED)
			{    TEST_METHOD_LIST.Add("SetUp");    }
            if (SecondPage.IS_TEAR_DOWN_CHECKED)
            {    TEST_METHOD_LIST.Add("TearDown");   }
        }
		/// <summary>
		/// Gets selected methods.
		/// </summary>
		/// <returns></returns>
        internal static ArrayList GetSelectedMethods() {
            return TEST_METHOD_LIST;
        }

    }
}




