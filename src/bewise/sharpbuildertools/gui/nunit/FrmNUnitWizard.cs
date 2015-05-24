using SMS.Windows.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BeWise.SharpBuilderTools.Gui.NUnit {
	/// <summary>
	/// Form of NUnit wizard
	/// </summary>
	class FrmNUnitWizard : WizardForm {
		
		/// <summary>
		/// Constructor.
		/// </summary>
        internal FrmNUnitWizard() {
            InitializeComponent();
            Controls.AddRange( new Control[] {
                                   new FirstPage(),
                                   new SecondPage(),
                                   new ThirdPage(),
                                   new FourthPage()
                               } );
        }

#region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            //
            // m_backButton
            //
            this.m_backButton.Name = "m_backButton";
            //
            // m_nextButton
            //
            this.m_nextButton.Name = "m_nextButton";
            //
            // m_cancelButton
            //
            this.m_cancelButton.Name = "m_cancelButton";
            //
            // m_finishButton
            //
            this.m_finishButton.Name = "m_finishButton";
            //
            // m_separator
            //
            this.m_separator.Name = "m_separator";
            //
            // FrmNUnitWizard
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(499, 362);
            this.Name = "FrmNUnitWizard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New TestFixture Wizard";
        }
#endregion

    }
}

