using System;
using System.Drawing;
using System.Windows.Forms;

namespace BeWise.SharpDevTools.Gui {

    public class FrmErrorDialog : System.Windows.Forms.Form{
        // *************************************************************************
        //                         Constructor
        // *************************************************************************

        public FrmErrorDialog():base(){
            InitializeComponent();
            this.Height= GetSmallHeight();
            pbIcon.Image = SystemIcons.Error.ToBitmap();
        }

        // *************************************************************************
        //                         Private
        // *************************************************************************

        private const int DEFAULT_SMALL_HEIGHT = 104;
        private const int DEFAULT_DETAIL_HEIGHT = 344;
        private const int DEFAULT_TITLE_BAR_HEIGHT = 19;

        public System.Windows.Forms.Label lblText;
        private System.Windows.Forms.Button butOk;
        private System.Windows.Forms.Button butDetails;
        private System.Windows.Forms.PictureBox pbIcon;
        private System.Windows.Forms.TextBox txtMessage;

        private int GetTitleBarHeightDelta() {
            return SystemInformation.CaptionHeight - DEFAULT_TITLE_BAR_HEIGHT;
        }

        private int GetDefaultTitleBarHeight() {
            return DEFAULT_TITLE_BAR_HEIGHT;
        }

        private int GetDetailHeight() {
            return DEFAULT_DETAIL_HEIGHT + GetTitleBarHeightDelta();
        }

        private int GetSmallHeight() {
            return DEFAULT_SMALL_HEIGHT + GetTitleBarHeightDelta();
        }

        private void butDetails_Click(object sender, System.EventArgs e) {
            if (this.Height < GetDetailHeight()) {
                this.Height = GetDetailHeight();
            } else {
                this.Height = GetSmallHeight();
            }
        }

        private void InitializeComponent() {
            this.lblText = new System.Windows.Forms.Label();
            this.butOk = new System.Windows.Forms.Button();
            this.butDetails = new System.Windows.Forms.Button();
            this.pbIcon = new System.Windows.Forms.PictureBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblText
            // 
            this.lblText.Location = new System.Drawing.Point(88, 15);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(432, 21);
            this.lblText.TabIndex = 0;
            this.lblText.Text = "Unexpected error, contact your administrator";
            // 
            // butOk
            // 
            this.butOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.butOk.Location = new System.Drawing.Point(368, 51);
            this.butOk.Name = "butOk";
            this.butOk.Size = new System.Drawing.Size(75, 22);
            this.butOk.TabIndex = 1;
            this.butOk.Text = "Ok";
            // 
            // butDetails
            // 
            this.butDetails.Location = new System.Drawing.Point(448, 51);
            this.butDetails.Name = "butDetails";
            this.butDetails.Size = new System.Drawing.Size(75, 22);
            this.butDetails.TabIndex = 2;
            this.butDetails.Text = "Details";
            this.butDetails.Click += new System.EventHandler(this.butDetails_Click);
            // 
            // pbIcon
            // 
            this.pbIcon.Location = new System.Drawing.Point(22, 12);
            this.pbIcon.Name = "pbIcon";
            this.pbIcon.Size = new System.Drawing.Size(40, 46);
            this.pbIcon.TabIndex = 3;
            this.pbIcon.TabStop = false;
            // 
            // txtMessage
            // 
            this.txtMessage.AutoSize = false;
            this.txtMessage.Location = new System.Drawing.Point(16, 80);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMessage.Size = new System.Drawing.Size(504, 216);
            this.txtMessage.TabIndex = 4;
            this.txtMessage.Text = "";
            this.txtMessage.WordWrap = false;
            // 
            // FrmErrorDialog
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(536, 79);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.pbIcon);
            this.Controls.Add(this.butDetails);
            this.Controls.Add(this.butOk);
            this.Controls.Add(this.lblText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FrmErrorDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.ResumeLayout(false);
        }

        // *************************************************************************
        //                              Public
        // *************************************************************************
        public static DialogResult ShowDialog(Exception aException, string aTitle){
            FrmErrorDialog _FrmErrorDialog = new FrmErrorDialog();

            _FrmErrorDialog.Text = aTitle;
            _FrmErrorDialog.txtMessage.Text = aException.Message + "\r\n\r\n" + aException.StackTrace;

            return _FrmErrorDialog.ShowDialog();
        }

        public static DialogResult ShowDialog(Exception aException, string aMessage, string aTitle){
            FrmErrorDialog _FrmErrorDialog = new FrmErrorDialog();
            _FrmErrorDialog.lblText.Text = aMessage;
            _FrmErrorDialog.Text = aTitle;
            _FrmErrorDialog.txtMessage.Text = aException.Message + "\r\n\r\n" + aException.StackTrace;

            return _FrmErrorDialog.ShowDialog();
        }
    }
}


