using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Borland.Studio.ToolsAPI;
using BeWise.SharpBuilderTools.Tools.Ant;
using BeWise.Common.Utils;

namespace BeWise.SharpBuilderTools.Gui.Ant {

	class FrmNAntMessage: Form {

        /**************************************************************/
        /*                     Constructor
        /**************************************************************/

		internal FrmNAntMessage(string aAntName, string aAntFile,
							  int aErrorCount, int aWarningCount,
                              string[] aTargets) {
            InitializeComponent();

            fErrorCount = aErrorCount;
            lblTitle.Text = aAntName;
            lblAntFile.Text = Path.GetFileName(aAntFile);
            lblErrors.Text = aErrorCount.ToString();
            lblWarnings.Text = aWarningCount.ToString();

            lblTargets.Text = BaseAnt.TargetsToString(aTargets);
        }

        protected override void Dispose (bool disposing) {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /**************************************************************/
        /*                     Private
        /**************************************************************/

        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblSubTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblErrors;
        private System.Windows.Forms.Label lblTargets;
        private System.Windows.Forms.Label lblAntFile;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblWarnings;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btCancel;
        private int fErrorCount;

#region Windows Form Designer generated code

        private void InitializeComponent()
		{
			this.btOk = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.lblSubTitle = new System.Windows.Forms.Label();
			this.lblTitle = new System.Windows.Forms.Label();
			this.lblErrors = new System.Windows.Forms.Label();
			this.lblTargets = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lblAntFile = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.lblWarnings = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.btCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btOk
			// 
			this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btOk.Location = new System.Drawing.Point(182, 200);
			this.btOk.Name = "btOk";
			this.btOk.Size = new System.Drawing.Size(90, 23);
			this.btOk.TabIndex = 0;
			this.btOk.Text = "Ok";
			this.btOk.Click += new System.EventHandler(this.btGoToError_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Location = new System.Drawing.Point(0, 48);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(528, 2);
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			// 
			// lblSubTitle
			// 
			this.lblSubTitle.BackColor = System.Drawing.Color.White;
			this.lblSubTitle.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblSubTitle.ForeColor = System.Drawing.Color.Black;
			this.lblSubTitle.Location = new System.Drawing.Point(0, 24);
			this.lblSubTitle.Name = "lblSubTitle";
			this.lblSubTitle.Size = new System.Drawing.Size(537, 24);
			this.lblSubTitle.TabIndex = 2;
			this.lblSubTitle.Text = "               You can view log in the message view";
			// 
			// lblTitle
			// 
			this.lblTitle.BackColor = System.Drawing.Color.White;
			this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblTitle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTitle.ForeColor = System.Drawing.Color.Black;
			this.lblTitle.Location = new System.Drawing.Point(0, 0);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(537, 24);
			this.lblTitle.TabIndex = 1;
			this.lblTitle.Text = "    Ant Result";
			this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblErrors
			// 
			this.lblErrors.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblErrors.Location = new System.Drawing.Point(163, 160);
			this.lblErrors.Name = "lblErrors";
			this.lblErrors.Size = new System.Drawing.Size(365, 24);
			this.lblErrors.TabIndex = 11;
			this.lblErrors.Text = "0";
			this.lblErrors.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblTargets
			// 
			this.lblTargets.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblTargets.Location = new System.Drawing.Point(163, 94);
			this.lblTargets.Name = "lblTargets";
			this.lblTargets.Size = new System.Drawing.Size(365, 24);
			this.lblTargets.TabIndex = 7;
			this.lblTargets.Text = "Clean Dist";
			this.lblTargets.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label3.Location = new System.Drawing.Point(10, 160);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(144, 24);
			this.label3.TabIndex = 10;
			this.label3.Text = "Compilation Errors";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label2.Location = new System.Drawing.Point(10, 94);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(144, 24);
			this.label2.TabIndex = 6;
			this.label2.Text = "Targets";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(10, 191);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(528, 2);
			this.groupBox1.TabIndex = 12;
			this.groupBox1.TabStop = false;
			// 
			// lblAntFile
			// 
			this.lblAntFile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblAntFile.Location = new System.Drawing.Point(163, 62);
			this.lblAntFile.Name = "lblAntFile";
			this.lblAntFile.Size = new System.Drawing.Size(365, 24);
			this.lblAntFile.TabIndex = 5;
			this.lblAntFile.Text = "build.xml";
			this.lblAntFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label6
			// 
			this.label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label6.Location = new System.Drawing.Point(10, 62);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(144, 24);
			this.label6.TabIndex = 3;
			this.label6.Text = "File";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblWarnings
			// 
			this.lblWarnings.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblWarnings.Location = new System.Drawing.Point(163, 128);
			this.lblWarnings.Name = "lblWarnings";
			this.lblWarnings.Size = new System.Drawing.Size(365, 24);
			this.lblWarnings.TabIndex = 9;
			this.lblWarnings.Text = "0";
			this.lblWarnings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label5
			// 
			this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label5.Location = new System.Drawing.Point(10, 128);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(144, 24);
			this.label5.TabIndex = 8;
			this.label5.Text = "Compilation Warnings";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btCancel
			// 
			this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btCancel.Location = new System.Drawing.Point(278, 200);
			this.btCancel.Name = "btCancel";
			this.btCancel.Size = new System.Drawing.Size(90, 23);
			this.btCancel.TabIndex = 13;
			this.btCancel.Text = "Cancel";
			this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
			// 
			// FrmNAntMessage
			// 
			this.AcceptButton = this.btOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.btOk;
			this.ClientSize = new System.Drawing.Size(537, 231);
			this.Controls.Add(this.btCancel);
			this.Controls.Add(this.lblWarnings);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.lblAntFile);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.lblErrors);
			this.Controls.Add(this.lblTargets);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.lblSubTitle);
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this.btOk);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FrmNAntMessage";
			this.Load += new System.EventHandler(this.FrmNAntMessage_Load);
			this.ResumeLayout(false);
		}
#endregion

        private void btGoToError_Click(object sender, System.EventArgs e) {
            if (fErrorCount > 0) {
                IOTAMessageService _MessageService = OTAUtils.GetMessageService();

                _MessageService.NextErrorMessage(true, true);
            }
        }
        
        private void FrmNAntMessage_Load(object sender, System.EventArgs e) {
            if (fErrorCount > 0) {
                btOk.Text = "Go to error";
            }
        }
		
		private void btCancel_Click(object sender, System.EventArgs e)
		{

		}
    }
}
