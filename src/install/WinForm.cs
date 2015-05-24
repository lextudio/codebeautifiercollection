using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace Install
{
	/// <summary>
	/// Summary description for WinForm.
	/// </summary>
	public class WinForm : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label1;

		public WinForm()
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
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(296, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Code Beautfiier Collection is now installing...";
			// 
			// WinForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(320, 62);
			this.ControlBox = false;
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "WinForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Install";
			this.Load += new System.EventHandler(this.WinForm_Load);
			this.ResumeLayout(false);
		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.Run(new WinForm());
		}

		private void WinForm_Load(object sender, System.EventArgs e)
		{
//			Process[] processes = Process.GetProcessesByName("bds");
//
//			if (processes.Length > 0) {
//				if (MessageBox.Show("bds is running. Close it ?", "Warning",
//					MessageBoxButtons.OKCancel) == DialogResult.OK)
//				{
//					foreach (Process process in processes)
//					{
//						process.CloseMainWindow();
//					}
//					Execute(".\\setup.exe", null);
//				}
//			} else {
			//MessageBox.Show("no bds is running");
			Execute(".\\setup.exe", null);
//			}
			Close();
		}
		// These are the Win32 error code for file not found or access denied.
		const int ERROR_FILE_NOT_FOUND =2;
		const int ERROR_ACCESS_DENIED = 5;

		private static void Execute(string fileName, string arg) {
			Process myProcess = new Process();

			try
			{
				// Get the path that stores user documents.
				myProcess.StartInfo.FileName = fileName;
				myProcess.StartInfo.Arguments = arg;
				myProcess.Start();
			}
			catch (Win32Exception e)
			{
				if(e.NativeErrorCode == ERROR_FILE_NOT_FOUND)
				{
					Console.WriteLine(e.Message + ". Check the path.");
				}

				else if (e.NativeErrorCode == ERROR_ACCESS_DENIED)
				{
					// Note that if your word processor might generate exceptions
					// such as this, which are handled first.
					Console.WriteLine(e.Message +
					                  ". You do not have permission to print this file.");
				}
			}
		}
	}
}
