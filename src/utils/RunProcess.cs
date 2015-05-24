using System;
using System.Reflection;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Threading;
using System.Text;
using System.IO;

namespace Lextm.JcfExpert.Utils {

    public class RunProcess {

        // *************************************************************************
        //                             constants
        // *************************************************************************

        const int BUFFER_SIZE = 64;	
        
        // *************************************************************************
		//                                Private
        // *************************************************************************
        
        private Process fP;
		private string fMsg;	
        private byte[] BufferRead = new byte[BUFFER_SIZE];
		private Decoder StreamDecode = Encoding.UTF8.GetDecoder();

		private void DoConsoleOutput(string aOutput) {
			if (OnConsoleOutput != null)
				OnConsoleOutput(aOutput);
		}

		private void DoRunCompleted() {
			if (OnRunCompleted != null)
				OnRunCompleted(this, new EventArgs());
		}

		private void ReadCallBack(IAsyncResult asyncResult) {
            int read = fP.StandardOutput.BaseStream.EndRead(asyncResult);
            
            if (read > 0) {
                char[] charBuffer = new char[BUFFER_SIZE];
                int len = StreamDecode.GetChars(BufferRead,0,BufferRead.Length,charBuffer,0);

                string _Str = Encoding.ASCII.GetString(BufferRead, 0, read);
                
                fMsg += _Str;

                DoConsoleOutput(_Str);
				
                fP.StandardOutput.BaseStream.BeginRead(BufferRead, 0, BUFFER_SIZE,new AsyncCallback(ReadCallBack), null);
            } else {
                fP.StandardOutput.BaseStream.Close();
			}

            return;
        }

		// *************************************************************************
		//                                Public
		// *************************************************************************

		public delegate void ConsoleOutputEvent(string aStr);

		public event ConsoleOutputEvent OnConsoleOutput;
		public event EventHandler OnRunCompleted;

		public string Run(string[] aArgs, string aWorkingDir) {
			if (aArgs.Length == 0)
			{
				return "Error, expected executable";
			}

			try
			{
				// Create the process
				ProcessStartInfo _ProcessStartInfo = new ProcessStartInfo();
				string _Arg = "";

				for (int i = 1; i < aArgs.Length; i++) {
					_Arg += aArgs[i] ;

					if (i != (aArgs.Length -1)) {
						_Arg += " ";
					}
				}

				_ProcessStartInfo.FileName = aArgs[0];
				_ProcessStartInfo.WorkingDirectory = aWorkingDir;
				_ProcessStartInfo.Arguments = _Arg;
				_ProcessStartInfo.RedirectStandardOutput = true;
				_ProcessStartInfo.CreateNoWindow = true;
				_ProcessStartInfo.UseShellExecute = false;

				fP = new Process();

				fP.EnableRaisingEvents = true;
                fP.StartInfo = _ProcessStartInfo;
				fP.Start(); //TODO: 

				fP.StandardOutput.BaseStream.BeginRead(BufferRead, 0,BUFFER_SIZE, new AsyncCallback(ReadCallBack), null);

				fP.WaitForExit();

				DoRunCompleted();
				return fMsg;
			}
            catch (Exception e)
			{
				MessageBox.Show(e.Message + "--" + e.StackTrace); 
                DoConsoleOutput("Error, unable to execute: " + aArgs[0]);
                return "Error, unable to execute: " + aArgs[0];
            }       
        }
    }
}
