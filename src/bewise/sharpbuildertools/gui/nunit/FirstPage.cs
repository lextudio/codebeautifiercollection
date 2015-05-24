using SMS.Windows.Forms;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BeWise.SharpBuilderTools.Gui.NUnit {
	
	/// <summary>
	/// First page of NUnit Forms.
	/// </summary>
	class FirstPage : ExteriorWizardPage {
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.ComponentModel.IContainer components = null;
		/// <summary>
		/// Constructor.
		/// </summary>
        internal FirstPage() {
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FirstPage));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // m_titleLabel
            //
            this.m_titleLabel.Name = "m_titleLabel";
            this.m_titleLabel.Text = "Welcome to the NUnit Test Fixture Wizard";
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
            this.label1.Size = new System.Drawing.Size(292, 39);
            this.label1.TabIndex = 2;
            this.label1.Text = "This is the TestFixture wizard, which will help you to generate a " +
                               "test fixture class for NUnit.";
            //
            // label2
            //
            this.label2.Location = new System.Drawing.Point(170, 274);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(292, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Click Next to continue.";
            //
            // FirstPage
            //
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FirstPage";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.m_titleLabel, 0);
            this.Controls.SetChildIndex(this.m_watermarkPicture, 0);
            this.ResumeLayout(false);
        }
#endregion
		/// <summary>
		/// OnSetActive handler.
		/// </summary>
		/// <returns></returns>
        protected override bool OnSetActive() {
            if( !base.OnSetActive() )
            {    return false;   }

            // Enable only the Next button on the this page
            Wizard.SetWizardButtons( WizardButton.Next );
            return true;
        }
    }
}

