// This is a C# port of CnWizards TipOfDay wizard.
//   Since the original code is covered by CnPack IDE Wizards License,
//   I put this piece of software under that license, too.

// Using VCL classes in WinForms is not my idea.
// I read that from a BDN Code Central entry #22691.
// Its author is Bruce McGee. Thanks Bruce.

using System;
using System.Drawing;

using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Borland.Vcl;

namespace Lextm.Utilities.Gui
{
    /// <summary>
    /// Summary description for TipOfTheDayForm.
    /// </summary>
    public class TipOfTheDayForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.PictureBox imgIcon;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Panel panelBack;
        private System.Windows.Forms.Label lblTip;
        private System.Windows.Forms.Panel panelDyk;
        private System.Windows.Forms.Label lblDyk;
        private System.Windows.Forms.Panel panelSeparator;
        private System.Windows.Forms.Button btnPrevious;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;

        public TipOfTheDayForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (ini != null)
                {
                    ini.Free();
                    ((IDisposable)ini).Dispose();
                    ini = null;
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TipOfTheDayForm));
            this.imgIcon = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.panel = new System.Windows.Forms.Panel();
            this.panelBack = new System.Windows.Forms.Panel();
            this.lblTip = new System.Windows.Forms.Label();
            this.panelSeparator = new System.Windows.Forms.Panel();
            this.panelDyk = new System.Windows.Forms.Panel();
            this.lblDyk = new System.Windows.Forms.Label();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.imgIcon)).BeginInit();
            this.panel.SuspendLayout();
            this.panelBack.SuspendLayout();
            this.panelDyk.SuspendLayout();
            this.SuspendLayout();
            // 
            // imgIcon
            // 
            this.imgIcon.Image = ((System.Drawing.Image)(resources.GetObject("imgIcon.Image")));
            this.imgIcon.Location = new System.Drawing.Point(8, 28);
            this.imgIcon.Name = "imgIcon";
            this.imgIcon.Size = new System.Drawing.Size(32, 32);
            this.imgIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgIcon.TabIndex = 0;
            this.imgIcon.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Hint:";
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(200, 192);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 21);
            this.btnNext.TabIndex = 0;
            this.btnNext.Text = "&Next";
            this.btnNext.Click += new System.EventHandler(this.btnNextClick);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(280, 192);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 21);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "&Close";
            this.btnClose.Click += new System.EventHandler(this.btnCloseClick);
            // 
            // panel
            // 
            this.panel.Controls.Add(this.panelBack);
            this.panel.Location = new System.Drawing.Point(48, 24);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(305, 153);
            this.panel.TabIndex = 2;
            // 
            // panelBack
            // 
            this.panelBack.BackColor = System.Drawing.SystemColors.Info;
            this.panelBack.Controls.Add(this.lblTip);
            this.panelBack.Controls.Add(this.panelSeparator);
            this.panelBack.Controls.Add(this.panelDyk);
            this.panelBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBack.Location = new System.Drawing.Point(0, 0);
            this.panelBack.Name = "panelBack";
            this.panelBack.Size = new System.Drawing.Size(305, 153);
            this.panelBack.TabIndex = 0;
            // 
            // lblTip
            // 
            this.lblTip.Font = new System.Drawing.Font("Arial", 9F);
            this.lblTip.Location = new System.Drawing.Point(6, 48);
            this.lblTip.Name = "lblTip";
            this.lblTip.Size = new System.Drawing.Size(298, 102);
            this.lblTip.TabIndex = 0;
            // 
            // panelSeparator
            // 
            this.panelSeparator.BackColor = System.Drawing.SystemColors.ControlText;
            this.panelSeparator.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSeparator.Location = new System.Drawing.Point(0, 41);
            this.panelSeparator.Name = "panelSeparator";
            this.panelSeparator.Size = new System.Drawing.Size(305, 1);
            this.panelSeparator.TabIndex = 1;
            // 
            // panelDyk
            // 
            this.panelDyk.Controls.Add(this.lblDyk);
            this.panelDyk.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDyk.Location = new System.Drawing.Point(0, 0);
            this.panelDyk.Name = "panelDyk";
            this.panelDyk.Size = new System.Drawing.Size(305, 41);
            this.panelDyk.TabIndex = 0;
            // 
            // lblDyk
            // 
            this.lblDyk.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDyk.Location = new System.Drawing.Point(6, 4);
            this.lblDyk.Name = "lblDyk";
            this.lblDyk.Size = new System.Drawing.Size(298, 30);
            this.lblDyk.TabIndex = 0;
            this.lblDyk.Text = "Do you know?";
            // 
            // btnPrevious
            // 
            this.btnPrevious.Location = new System.Drawing.Point(119, 192);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(75, 21);
            this.btnPrevious.TabIndex = 3;
            this.btnPrevious.Text = "&Previous";
            this.btnPrevious.Click += new System.EventHandler(this.btnPreviousClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(8, 176);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(344, 8);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(48, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(304, 8);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            // 
            // TipOfTheDayForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(360, 220);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.imgIcon);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.btnPrevious);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Location = new System.Drawing.Point(404, 256);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TipOfTheDayForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tip of the Day";
            this.Load += new System.EventHandler(this.TipOfTheDayForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.imgIcon)).EndInit();
            this.panel.ResumeLayout(false);
            this.panelBack.ResumeLayout(false);
            this.panelDyk.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new TipOfTheDayForm());
        }

        private int curIndex = -1;
        private TStringList tips;

        private void btnNextClick(object sender, System.EventArgs e)
        {
            curIndex++;
            if (curIndex > tips.Count - 1)
            {
                curIndex = 0;
            }
            ShowTip();
        }

        private void btnCloseClick(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void btnPreviousClick(object sender, System.EventArgs e)
        {
            curIndex--;
            if (curIndex < 0)
            {
                curIndex = tips.Count - 1;
            }
            ShowTip();
        }
        private const string csTipItem = "TipItems";
        private const string csFileName = "TipOfTheDay.ini";

        private TIniFile ini;

        private void TipOfTheDayForm_Load(object sender, System.EventArgs e)
        {
            string fileName = Path.Combine(Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location),
                csFileName);
            if (!File.Exists(fileName))
            {
                Lextm.Windows.Forms.MessageBoxFactory.Warn("No tip file exists: " + fileName);
                this.Close();
            }
            else
            {
                ini = new TIniFile(fileName);
                tips = new TStringList();
                ini.ReadSectionValues(csTipItem, tips);
                curIndex = (new Random()).Next(tips.Count);
                ShowTip();
            }
        }

        private void ShowTip()
        {
            string s;
            if (tips.Count == 0)
            {
                Lextm.Windows.Forms.MessageBoxFactory.Warn("no tip");
                s = String.Empty;
            }
            else
            {
                s = tips.get_ValueFromIndex(curIndex);
            }
            lblTip.Text = s;
        }
    }
}
