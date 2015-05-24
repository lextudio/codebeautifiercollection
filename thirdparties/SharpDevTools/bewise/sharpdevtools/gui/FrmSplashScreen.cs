using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace BeWise.SharpDevTools.Gui {

    public class FrmSplashScreen : System.Windows.Forms.Form {
        // *************************************************************************
        //                              Constructor
        // *************************************************************************
        public FrmSplashScreen() {
            DoInitializeComponent();
        }

        protected override void Dispose (bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        // *************************************************************************
        //                              Private
        // *************************************************************************

        private System.Windows.Forms.PictureBox pictureBox;
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.ContextMenu contextMenu;
        private System.Windows.Forms.MenuItem miClose;


        #region Windows Form Designer generated code
        private void InitializeComponent() {
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.contextMenu = new System.Windows.Forms.ContextMenu();
            this.miClose = new System.Windows.Forms.MenuItem();
            this.lblVersion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                        | System.Windows.Forms.AnchorStyles.Left) 
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox.ContextMenu = this.contextMenu;
            this.pictureBox.Location = new System.Drawing.Point(1, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(719, 536);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // contextMenu
            // 
            this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                        this.miClose});
            //
            // miClose
            // 
            this.miClose.Index = 0;
            this.miClose.Text = "Close";
            this.miClose.Click += new System.EventHandler(this.menuClose_Click);
            // 
            // lblVersion
            // 
            this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVersion.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblVersion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblVersion.ForeColor = System.Drawing.Color.Blue;
            this.lblVersion.Location = new System.Drawing.Point(4, 502);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(709, 23);
            this.lblVersion.TabIndex = 1;
            this.lblVersion.Text = "Version";
            // 
            // FrmSplashScreen
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(717, 534);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.pictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmSplashScreen";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Optosys 2.0";
            this.TopMost = true;
            this.ResumeLayout(false);
        }
        #endregion

        private void menuClose_Click(object sender, System.EventArgs e) {
            //fHideSplashScreen = true;
        }

        private void OnTick(object sender,EventArgs e) {
//            if(fHideSplashScreen == true) {
//                fTimer.Enabled = false;
//                Close();
//            }
        }

        // *************************************************************************
        //                              Protected
        // *************************************************************************
        protected void DoInitializeComponent() {
            InitializeComponent();
//            this.pictureBox.Image = CentralManager.ImageManager.GetCustomImage(Consts.CUSTOM_IMAGE_SPLASH_SCREEN);
            lblVersion.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();

//            fTimer.Enabled = true;
//            fTimer.Interval = 500;
//            fTimer.Tick += new System.EventHandler(this.OnTick);
        }
    }
}
