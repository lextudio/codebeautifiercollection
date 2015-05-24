namespace ArtCSB
{
	/// <summary>
	/// Summary description for WinForm.
	/// </summary>
	class WinForm_About : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button button_OK;
		private System.Windows.Forms.RichTextBox richTextBox_About;

		internal WinForm_About()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			string  st = Lextm.ArtCSB.ArtCSB_MyResource.ArtCSB_rtf;//Common.MyRes.GetString("ArtCSB.rtf");
			richTextBox_About.Rtf = st;
		}


		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(WinForm_About));
			this.button_OK = new System.Windows.Forms.Button();
			this.richTextBox_About = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// button_OK
			// 
			this.button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button_OK.Location = new System.Drawing.Point(547, 233);
			this.button_OK.Name = "button_OK";
			this.button_OK.Size = new System.Drawing.Size(87, 24);
			this.button_OK.TabIndex = 1;
			this.button_OK.Text = "OK";
			this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
			// 
			// richTextBox_About
			// 
			this.richTextBox_About.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.richTextBox_About.BackColor = System.Drawing.Color.White;
			this.richTextBox_About.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.richTextBox_About.ForeColor = System.Drawing.Color.Black;
			this.richTextBox_About.Location = new System.Drawing.Point(10, 9);
			this.richTextBox_About.Name = "richTextBox_About";
			this.richTextBox_About.ReadOnly = true;
			this.richTextBox_About.Size = new System.Drawing.Size(624, 215);
			this.richTextBox_About.TabIndex = 8;
			this.richTextBox_About.Text = "";
			this.richTextBox_About.MouseDown += new System.Windows.Forms.MouseEventHandler(this.richTextBox_About_MouseDown);
			this.richTextBox_About.TextChanged += new System.EventHandler(this.richTextBox_About_TextChanged);
			this.richTextBox_About.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBox_About_LinkClicked);
			// 
			// WinForm_About
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(645, 267);
			this.Controls.Add(this.richTextBox_About);
			this.Controls.Add(this.button_OK);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "WinForm_About";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "ArtCSB Info";
			this.ResumeLayout(false);
		}
		#endregion

		private void button_OK_Click(object sender, System.EventArgs e) {
			this.Close();
		}

		private void richTextBox_About_LinkClicked(object sender, System.Windows.Forms.LinkClickedEventArgs e) {
			string sTo = System.Web.HttpUtility.UrlEncode(e.LinkText);
			string sSubject = "";
			string sMessage = "";

			System.Diagnostics.Process.Start("mailto:" + sTo + "?subject=" + sSubject + "&body=" + sMessage);
		}

		private void richTextBox_About_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {

		}

		private void richTextBox_About_TextChanged(object sender, System.EventArgs e)
		{

		}
	}
}
