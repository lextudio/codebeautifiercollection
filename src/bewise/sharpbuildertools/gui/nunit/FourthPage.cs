using SMS.Windows.Forms;
using System;
using System.IO;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Borland.Studio.ToolsAPI;
using BeWise.SharpBuilderTools.Wizard.Creator;
using BeWise.Common.Utils;

namespace BeWise.SharpBuilderTools.Gui.NUnit {
	/// <summary>
	/// Fourth page of NUnit Forms.
	/// </summary>
	class FourthPage : ExteriorWizardPage {
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.ComponentModel.IContainer components = null;
		/// <summary>
		/// Constructor.
		/// </summary>
		internal FourthPage() {
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FourthPage));
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // m_titleLabel
            //
            this.m_titleLabel.Name = "m_titleLabel";
            this.m_titleLabel.Text = "You almost finished the Test Fixture Wizard";
            //
            // m_watermarkPicture
            //
            this.m_watermarkPicture.Image = ((System.Drawing.Image)(resources.GetObject("m_watermarkPicture.Image")));
            this.m_watermarkPicture.Name = "m_watermarkPicture";
            this.m_watermarkPicture.Size = new System.Drawing.Size(152, 312);
            //
            // label1
            //
            this.label1.Location = new System.Drawing.Point(170, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(292, 55);
            this.label1.TabIndex = 2;
            this.label1.Text = "This is the last final generation step of the Test Fixture Wizard." +
                               "  In order to change any of your settings use the < Back button.";
            //
            // label3
            //
            this.label3.Location = new System.Drawing.Point(170, 274);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(292, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Click Finish to generate the test fixture.";
            //
            // FourthPage
            //
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Name = "FourthPage";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.m_titleLabel, 0);
            this.Controls.SetChildIndex(this.m_watermarkPicture, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.ResumeLayout(false);
        }
#endregion
		/// <summary>
		/// OnSetActive handler.
		/// </summary>
		/// <returns></returns>
        protected override bool OnSetActive() {
            if( !base.OnSetActive() )
                return false;

            // Enable both the Back and Finish (enabled/disabled) buttons on this page
            Wizard.SetWizardButtons( WizardButton.Back | WizardButton.Finish );
            return true;
        }
		/// <summary>
		/// OnWizardFinish handler.
		/// </summary>
		/// <returns></returns>
        protected override bool OnWizardFinish() {
            // Finish the wizard
            GenerateTestFixture();
            return true;
        }

        private void GenerateTestFixture() {
            IOTAModuleServices lModuleServices;
            IOTAModule lModule;
            IOTAModule lActiveProject = null;

            lModuleServices = OTAUtils.GetModuleServices();

            if (lModuleServices.MainProjectGroup != null) {
                if (lModuleServices.MainProjectGroup.ActiveProject != null) {
                    lActiveProject = lModuleServices.MainProjectGroup.ActiveProject;
                    lModule = lModuleServices.CreateModule(new TestFixtureModuleCreator(GenerateTestFixtureCode(),lActiveProject));
                } else {
                    Lextm.Windows.Forms.MessageBoxFactory.Warn("Please open or create a project before using this NUnit TestFixture wizard.");
                }
            } else {
                Lextm.Windows.Forms.MessageBoxFactory.Warn("Please open or create a project before using this NUnit TestFixture wizard.");
            }
        }

        private string GenerateTestFixtureCode() {
            string codeHeader = "using System;\r\nusing NUnit.Framework;\r\n\r\n"
                                + "namespace " + SecondPage.GetTestNamespace() + "\r\n{\r\n"
                                + "\t[TestFixture]\r\n\tpublic class " + Path.GetFileNameWithoutExtension(SecondPage.GetTestClassName())
                                + "\r\n\t{\r\n";

            string codeBody = "";
            foreach (string str in ThirdPage.GetSelectedMethods()) {
                if (str.Equals("SetUp"))
				{    codeBody += "\t\t[SetUp]\r\n\t\tpublic void " + str + "()\r\n\t\t{\r\n\r\n\t\t}\r\n\r\n";
				}else if (str.Equals("TearDown"))
				{    codeBody += "\t\t[TearDown]\r\n\t\tpublic void " + str + "()\r\n\t\t{\r\n\r\n\t\t}\r\n\r\n";
				}else {
                    codeBody += "\t\t[Test]\r\n\t\tpublic void " + str + "()\r\n\t\t{\r\n\r\n\t\t}\r\n\r\n";
				}
			}

            string codeTail = "\t}\r\n}\r\n";

            return codeHeader+codeBody+codeTail;
        }
		/// <summary>
		/// Whether to show a ''file exists'' message box.
		/// </summary>
        internal static bool ShowFileExistsMsgBox = true;
    }
}



