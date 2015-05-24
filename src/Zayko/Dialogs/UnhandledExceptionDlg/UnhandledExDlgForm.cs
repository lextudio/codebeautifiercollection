using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SendFileTo;
using UnhandledExceptionManager;

namespace Zayko.Dialogs.UnhandledExceptionDlg
{
	public partial class UnhandledExDlgForm: Form, IExceptionInfoManager
	{
		public UnhandledExDlgForm()
		{
			InitializeComponent();
		}

		private void UnhandledExDlgForm_Load(object sender, EventArgs e)
		{
			buttonNotSend.Focus();
			labelExceptionDate.Text = String.Format(CultureInfo.InvariantCulture, 
			                                        labelExceptionDate.Text, 
			                                        DateTime.Now);
			linkLabelData.Left = labelLinkTitle.Right;
		}
		
		ApartmentState apartmentState;
		
		public ApartmentState ApartmentState {
			set {
				apartmentState = value;
			}
		}
		
		TExceptionKind kind;
		[CLSCompliant(false)]
		public TExceptionKind Kind {
			set {
				kind = value;
			}
		}
		
		bool userContinue;
		
		public bool UserContinue {
			set {
				userContinue = value;
			}
		}
		
		bool canContinue;
		
		public bool CanContinue {
			set {
				canContinue = value;
			}
		}
		
		string productInfo;
		
		public string ProductInfo {
			set {
				productInfo = value;
			}
		}
		
		string threadInfo;
		
		public string ThreadInfo {
			set {
				threadInfo = value;
			}
		}
		
		object exceptionObject;
		
		public object ExceptionObject {
			set {
				exceptionObject = value;
			}
		}
		
		string exceptionText;
		
		public string ExceptionText {
			set {
				exceptionText = value;
			}
		}
		
		string exceptionFullText;
		
		public string ExceptionFullText {
			set {
				exceptionFullText = value;
			}
		}
		
		StringCollection loadedAssemblyList;
		
		public StringCollection LoadedAssemblyList {
			set {
				loadedAssemblyList = value;
			}
		}
		
		string failedInfoManagerNames;
		
		public string FailedInfoManagerNames {
			set {
				failedInfoManagerNames = value;
			}
		}
		
		public bool Execute()
		{
			Text = productInfo;
			string str;
			if (kind == TExceptionKind.UnhandledException)
			{
				str = "Unhandled";
			}
			else if (kind == TExceptionKind.ThreadException)
			{
				str = "Thread";
			}
			else
			{
				str = string.Empty;
			}
			StringBuilder builder = new StringBuilder();
			builder.Append(string.Format(CultureInfo.InvariantCulture, 
			                             "Application: {0}\nThread Id / Name: {1}\nKind: {2}\nMessage: {3}\n\nException Text:\n{4}\n", new object[] { productInfo, threadInfo, str, exceptionText, exceptionFullText }));
			builder.Append("Loaded Assemblies:");
			foreach (string assembly in loadedAssemblyList)
			{
				builder.Append(assembly);
			}
			if (failedInfoManagerNames.Length != 0)
			{
				builder.Append("Warning, unable to execute some Info Manager:");
				builder.Append(failedInfoManagerNames);
			}
			totalInfo = builder.ToString();
			Text = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
			labelTitle.Text = String.Format(CultureInfo.InvariantCulture, labelTitle.Text, Text);
			checkBoxRestart.Text = String.Format(CultureInfo.InvariantCulture, checkBoxRestart.Text, Text);
			
			btnIgnore.Visible = canContinue && userContinue;
		
			return ShowDialog() != DialogResult.OK;
		}

		string totalInfo;
        
        void LinkLabelDataLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        	MessageBox.Show(totalInfo);
        }
        
        void ButtonSendClick(object sender, EventArgs e)
        {
        	Hide();
        	string fileName = writeContentToFile(totalInfo);
			MAPI mapi = new MAPI();
			mapi.AddAttachment(fileName);
			mapi.AddRecipientTo("lextudio@gmail.com");
			mapi.SendMailPopup("CBC Unhandled Exception", "See attachment.");

			Restart();
        }
        
        void Restart()
        {
        	if (checkBoxRestart.Checked) {}
        }
        
        static string writeContentToFile(string content)
		{
			string result = Path.GetTempFileName();
			using (StreamWriter writer = new StreamWriter(result))
			{
				writer.Write(content);
				writer.Close();
			}
			return result;
		}
        
        void ButtonNotSendClick(object sender, EventArgs e)
        {
        	Restart();
        }
	}
}