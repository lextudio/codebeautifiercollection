using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace BeWise.Common.Utils {
	/// <summary>
	/// RunProcess class.
	/// </summary>
	/// <remarks>
	/// Processes are run by Run method.
	/// They are run without a window.
	/// <para>Events OnConsoleOutput and OnRunCompleted can be set.</para>
	/// </remarks>
	public sealed class RunProcess: IDisposable {

        // *************************************************************************
        //                             constants
        // *************************************************************************

        const int BUFFER_SIZE = 64;

        // *************************************************************************
        //                                Private
        // *************************************************************************

        private Process fP;
        private string fMsg;
        private byte[] bufferRead = new byte[BUFFER_SIZE];
        //private Decoder streamDecode = Encoding.UTF8.GetDecoder();

        private void DoConsoleOutput(object sender) {
			if (OnConsoleOutput != null) {
				OnConsoleOutput(sender, null);
			}
        }

        private void DoRunCompleted() {
			if (OnRunCompleted != null) {
				OnRunCompleted(this, new EventArgs());
			}
        }

		private void ReadCallBack(IAsyncResult asyncResult) {
            int read = fP.StandardOutput.BaseStream.EndRead(asyncResult);

            if (read > 0) {
                //char[] charBuffer = new char[BUFFER_SIZE];
				//int len = streamDecode.GetChars(bufferRead,0,bufferRead.Length,charBuffer,0);

                string _Str = Encoding.UTF8.GetString(bufferRead, 0, read);

                fMsg += _Str;

                DoConsoleOutput(_Str);

                fP.StandardOutput.BaseStream.BeginRead(bufferRead, 0, BUFFER_SIZE,new AsyncCallback(ReadCallBack), null);
            } else {
                fP.StandardOutput.BaseStream.Close();
            }

            return;
        }

        // *************************************************************************
        //                                Public
        // *************************************************************************
        ///// <summary>
        ///// ConsoleOutputEvent delegate definition.
        ///// </summary>
        //public delegate void ConsoleOutputEventHandler(object sender, EventArgs e);
		/// <summary>
		/// OnConsoleOutput event.
		/// </summary>
        public event EventHandler OnConsoleOutput;
        /// <summary>
        /// OnRunCompleted event.
        /// </summary>
        public event EventHandler OnRunCompleted;
		/// <summary>
		/// Runs the proecess.
		/// </summary>
		/// <param name="args">Arguments</param>
		/// <param name="workingDir">Working place</param>
        public string Run(string[] args, string workingDir) {
            if (args.Length == 0) {
                return "Error, expected executable";
            }

            try {
                // Create the process
                ProcessStartInfo _ProcessStartInfo = new ProcessStartInfo();
                string _Arg = "";

                for (int i = 1; i < args.Length; i++) {
                    _Arg += args[i] ;

                    if (i != (args.Length -1)) {
                        _Arg += " ";
                    }
                }

                _ProcessStartInfo.FileName = args[0];
                _ProcessStartInfo.WorkingDirectory = workingDir;
                _ProcessStartInfo.Arguments = _Arg;
                _ProcessStartInfo.RedirectStandardOutput = true;
                _ProcessStartInfo.CreateNoWindow = true;
                _ProcessStartInfo.UseShellExecute = false;

                fP = new Process();

                fP.EnableRaisingEvents = true;
                fP.StartInfo = _ProcessStartInfo;
                fP.Start();

                fP.StandardOutput.BaseStream.BeginRead(bufferRead, 0,BUFFER_SIZE, new AsyncCallback(ReadCallBack), null);

                fP.WaitForExit();

                DoRunCompleted();
                return fMsg;
            } 
			catch (Win32Exception ex) 
			{
                Lextm.Windows.Forms.MessageBoxFactory.Fatal(null, ex);
                DoConsoleOutput("Error, unable to execute: " + args[0]);
                return "Error, unable to execute: " + args[0];
            }
        }
		/// <summary>
		/// Disposes.
		/// </summary>
		public void Dispose()
		{
			if (!fP.HasExited)
			{
				fP.Kill();
			}
			fP.Dispose();
		}
    }
}
