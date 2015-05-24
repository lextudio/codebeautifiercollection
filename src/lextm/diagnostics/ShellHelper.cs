// this is shell helper class.
//		It provides some convenient ways to run other executables.
// Copyright (C) 2006-2007  Lex Y. Li
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace Lextm.Diagnostics
{
	/// <summary>
	/// Shell.
	/// </summary>
	/// <remarks>At this moment, a normal file, such as "readme.txt" cannot be executed 
	/// by calling ShellExecute("readme.txt"). </remarks>
	public sealed class ShellHelper
	{
		private ShellHelper()
		{
		}
		/// <summary>
		/// Verifies if the executable is already running.
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <returns></returns>
		public static bool IsProcessRunning(string fileName)
		{
			Process[] processes = Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(fileName));
			if (processes.Length > 0)
			{
				foreach (Process p in processes)
				{
					if (Lextm.IO.FileHelper.IsTheSameFile(p.MainModule.FileName, fileName))
					{
						return true;
					}
				}
			}
			return false;
		}
        // These are the Win32 error code for file not found or access denied.
        private const int ERROR_FILE_NOT_FOUND =2;
        private const int ERROR_ACCESS_DENIED = 5;
		private const int ERROR_NO_ASSOCIATION = 1155;
        /// <summary>
		/// Executes an executable.
		/// </summary>
		/// <param name="fileName">Executable name</param>
		/// <remarks>Better than Process.Start because it handles exceptions.</remarks>
		public static void Execute(string fileName) {
			if (StringHelper.IsHttpUri(fileName)) {
				Help.ShowHelp(null, fileName);
			} else {
				Execute(fileName, String.Empty);
			}
		}
        /// <summary>
		/// Executes an executable.
		/// </summary>
		/// <param name="fileName">Executable name</param>
		/// <param name="parameters">Parameters</param>
		public static void Execute(string fileName, string parameters) {
			Execute(fileName, parameters, System.IO.Path.GetDirectoryName(fileName), false);
        }
		/// <summary>
		/// Executes an executable.
		/// </summary>
		/// <param name="fileName">Executable name</param>
		/// <param name="parameters">Parameters</param>
		/// <param name="waitTillReturn">Wait till return</param>	
		public static void Execute(string fileName, string parameters, bool waitTillReturn) {
			Execute(fileName, parameters, System.IO.Path.GetDirectoryName(fileName), waitTillReturn);			
		}
		/// <summary>
		/// Executes an executable.
		/// </summary>
		/// <param name="fileName">Executable name</param>
		/// <param name="parameters">Parameters</param>
		/// <param name="workingDir">Working directory</param>	
		/// <param name="waitTillReturn">Wait till return</param>	
		public static void Execute(string fileName, string parameters, string workingDir, bool waitTillReturn) {
            System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
			bool started = false;
            try
            {
				myProcess.StartInfo.FileName = fileName;
				myProcess.StartInfo.Arguments = parameters;
				myProcess.StartInfo.WorkingDirectory = workingDir;
				myProcess.StartInfo.UseShellExecute = true;
                myProcess.Start();
                started = true;
                if (waitTillReturn) {
                	myProcess.WaitForExit();
                }
            }
            catch (Win32Exception e)
            {
                if(e.NativeErrorCode == ERROR_FILE_NOT_FOUND)
                {
					Lextm.Windows.Forms.MessageBoxFactory.Error("Check if the path is valid. " + fileName);
				}
				else if (e.NativeErrorCode == ERROR_ACCESS_DENIED)
				{
					Lextm.Windows.Forms.MessageBoxFactory.Error(
						"You do not have permission to run this file. " + fileName);
				}
				else if (e.NativeErrorCode == ERROR_NO_ASSOCIATION)
				{
					Lextm.Windows.Forms.MessageBoxFactory.Error(
						"You must install necessary software to see this file. " + fileName);
				}
				else {
					Lextm.Windows.Forms.MessageBoxFactory.Error(e.Message + " " + fileName);
                }
			}
            finally {
            	if (waitTillReturn) 
            	{
            		if (started && !myProcess.HasExited)
                	{
                		myProcess.Kill();
            		}
                    myProcess.Close();
                }
            }
        }
        /// <summary>
        /// Runs an executable.
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <param name="waitTillReturn">Wait till return flag</param>
        public static void Execute(string fileName, bool waitTillReturn)
        {
            Execute(fileName, String.Empty, System.IO.Path.GetDirectoryName(fileName), waitTillReturn);
        }
    }
}
