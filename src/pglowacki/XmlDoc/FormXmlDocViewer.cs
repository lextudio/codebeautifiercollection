// original file is copyrighted by Pawel Glowacki.
// The file can be found on the web at
// http://codecentral.borland.com/codecentral/ccweb.exe/listing?id=20544

// lextm modifies a few lines and removes something he does not need.
using System;
using System.Drawing;

using System.ComponentModel;
using System.Windows.Forms;

namespace pglowacki.XmlDoc
{
    /// <summary>
    /// Summary description for WinForm1.
    /// </summary>
    public class FormXmlDocViewer : System.Windows.Forms.Form
    {
        /// <summary>
		/// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        public FormXmlDocViewer()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            InitializeForm();
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
        	this.webBrows = new System.Windows.Forms.WebBrowser();
        	this.SuspendLayout();
        	// 
        	// webBrows
        	// 
        	this.webBrows.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.webBrows.Location = new System.Drawing.Point(0, 0);
        	this.webBrows.MinimumSize = new System.Drawing.Size(20, 20);
        	this.webBrows.Name = "webBrows";
        	this.webBrows.Size = new System.Drawing.Size(636, 398);
        	this.webBrows.TabIndex = 0;
        	this.webBrows.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.WebBrowsNavigating);
        	// 
        	// FormXmlDocViewer
        	// 
        	this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
        	this.ClientSize = new System.Drawing.Size(636, 398);
        	this.Controls.Add(this.webBrows);
        	this.MaximizeBox = false;
        	this.MinimizeBox = false;
        	this.MinimumSize = new System.Drawing.Size(250, 150);
        	this.Name = "FormXmlDocViewer";
        	this.ShowInTaskbar = false;
        	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        	this.Text = "XML Documentation";
        	this.Deactivate += new System.EventHandler(this.FormXmlDocViewer_Deactivate);
        	this.ResumeLayout(false);
		}
        private System.Windows.Forms.WebBrowser webBrows;
#endregion

        /// <summary>
        /// Clears the content of WebBrowser and makes its Document property not null
        /// </summary>
        private void NavigateBlank()
        {
            webBrows.Navigate("about:blank");
        }

     	/// <summary>
        /// Displays arbitrary HTML string inside the WebBrowser.
        /// </summary>
        public void DisplayHtml(string s)
        {
        	HtmlDocument hdoc = webBrows.Document.OpenNew(true); // clears current doc
            hdoc.Write(s);
        }
        /// <summary>
        /// Displays arbitrary HTML string inside the WebBrowser.
        /// </summary>        
		public static void ShowViewer( string html ) {
			FormXmlDocViewer viewer = new FormXmlDocViewer();
			viewer.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			viewer.Text = "Doc Preview";
			viewer.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			viewer.ClientSize = new System.Drawing.Size(420, 294);
			viewer.StartPosition = FormStartPosition.Manual;
			viewer.MaximizeBox = false;
			viewer.MinimizeBox = false;
			viewer.TopMost = true;
			viewer.DisplayHtml(html);
			viewer.Left = Screen.PrimaryScreen.WorkingArea.Right
					- viewer.Width;
			viewer.Top = Screen.PrimaryScreen.WorkingArea.Bottom
					- viewer.Height;
			viewer.Show();
		}
        
        private void InitializeForm()
        {
            NavigateBlank();
        }

        private void FormXmlDocViewer_Deactivate(object sender, System.EventArgs e)
        {
            Close();
		}		
       
        void WebBrowsNavigating(object sender, WebBrowserNavigatingEventArgs e)
        {
        	if (e.Url.ToString() == "about:blank") {
                e.Cancel = true;
            }        	
        }
    }
}

