/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2007/11/3
 * Time: 15:58
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.IO;
using System.Text;

using Microsoft.Win32;

namespace Launcher
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("This is CodeGear RAD Studio Expert Manager, please don't close.");
			string expertMaintainer = Path.Combine(GetCbcFolder(), "expertmaintainer.exe");
			if (File.Exists(expertMaintainer)) {
				Execute(expertMaintainer, true);
			}
			Execute("bds.real.exe", MergeParameters(args), true);
		}
		
		static string MergeParameters(string[] args)
		{
			StringBuilder result = new StringBuilder();
			foreach (string arg in args)
			{
				result.Append(arg + " ");
			}
			return result.ToString();
		}
		// These are the Win32 error code for file not found or access denied.
		private const int ERROR_FILE_NOT_FOUND =2;
		private const int ERROR_ACCESS_DENIED = 5;
		private const int ERROR_NO_ASSOCIATION = 1155;
		/// <summary>
		/// Executes an executable.
		/// </summary>
		/// <param name="fileName">Executable name</param>
		/// <param name="parameters">Parameters</param>
		/// <param name="workingDir">Working directory</param>
		/// <param name="waitTillReturn">Wait till return</param>
		static void Execute(string fileName, string parameters, string workingDir, bool waitTillReturn) {
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
					//Lextm.Windows.Forms.MessageBoxFactory.Error("Check if the path is valid. " + fileName);
				}
				else if (e.NativeErrorCode == ERROR_ACCESS_DENIED)
				{
					//Lextm.Windows.Forms.MessageBoxFactory.Error(
					//	"You do not have permission to run this file. " + fileName);
				}
				else if (e.NativeErrorCode == ERROR_NO_ASSOCIATION)
				{
					//Lextm.Windows.Forms.MessageBoxFactory.Error(
					//	"You must install necessary software to see this file. " + fileName);
				}
				else {
					//Lextm.Windows.Forms.MessageBoxFactory.Error(e.Message + " " + fileName);
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
		static void Execute(string fileName, bool waitTillReturn)
		{
			Execute(fileName, String.Empty, System.IO.Path.GetDirectoryName(fileName), waitTillReturn);
		}
		/// <summary>
		/// Executes an executable.
		/// </summary>
		/// <param name="fileName">Executable name</param>
		/// <param name="parameters">Parameters</param>
		/// <param name="waitTillReturn">Wait till return</param>	
		static void Execute(string fileName, string parameters, bool waitTillReturn) {
			Execute(fileName, parameters, System.IO.Path.GetDirectoryName(fileName), waitTillReturn);			
		}		
		static string GetCbcFolder()
		{
			// HKLM\Software\Microsoft\Windows\CurrentVersion\Uninstall\{F768F6BA-F164-4599-BC26-DCCFC2F76855}_is1
			// InstallLocation
			return GetValueFromRegKey(BaseKey.LocalMachine,
			                          @"Software\Microsoft\Windows\CurrentVersion\Uninstall\{F768F6BA-F164-4599-BC26-DCCFC2F76855}_is1",
			                          "InstallLocation",
			                          @"C:\Program Files\Code Beautifier Collection").ToString();
		}

		static object GetValueFromRegKey(BaseKey keyBase, string keyName, string valueName, object defaultValue) {
			
			RegistryKey _Key = (keyBase == BaseKey.Default) ?
				Registry.CurrentUser.OpenSubKey(keyName) : Registry.LocalMachine.OpenSubKey(keyName);

			if ( _Key != null && Array.IndexOf(_Key.GetValueNames(), valueName) > -1) {
				return _Key.GetValue(valueName);
			} else {
				return defaultValue;
			}
		}
	}
	/// <summary>
	/// Base key.
	/// </summary>
	enum BaseKey {
		/// <summary>
		/// Default, current user.
		/// </summary>
		Default = 0,
		/// <summary>
		/// Local machine.
		/// </summary>
		LocalMachine
	};
}