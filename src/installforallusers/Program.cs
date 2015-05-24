using System;
using System.IO;
using System.Reflection;

using Lextm.Diagnostics;
using Lextm.Win32;
using Lextm.Windows.Forms;
using Microsoft.Win32;

namespace installforallusers
{
	static class Program
	{
		readonly static string expertRegistry = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "expertregistry.exe");
		readonly static string framework = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Lextm.CodeBeautifierCollection.Framework.dll");
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				// install
				// first check if expertregistry exists
				if (File.Exists(expertRegistry))
				{
					if (MessageBoxFactory.Confirm("Install Code Beautifier Collection for all users? (This is an experimental feature. Click No if you don't want to paticipate in the trial.)") == System.Windows.Forms.DialogResult.Yes)
					{
						//for all users
						RegistryHelper.SetValueToRegKey(BaseKey.LocalMachine, @"Software\LeXtudio\CodeBeautifierCollection", "InstallForAllUsers", 1);
						// register cbc using expertregstry.exe
						ShellHelper.Execute(expertRegistry, "/n:\"Code Beautifier Collection\" /f:\"" + framework + "\"", true);
						RegistryHelper.SetValueToRegKey(@"Software\Borland\BDS\5.0\Known IDE Assemblies", framework, "Code Beautifier Collection");
						FakeLauncher();
						InstallDllToGac();
						return;
					}
				}
				//for current user only
				RegistryHelper.SetValueToRegKey(BaseKey.LocalMachine, @"Software\LeXtudio\CodeBeautifierCollection", "InstallForAllUsers", 0);
				RegistryHelper.SetValueToRegKey(@"Software\Borland\BDS\5.0\Known IDE Assemblies", framework, "Code Beautifier Collection");

				InstallDllToGac();
			}
			else if (args.Length == 1)
			{
				if (args[0] == "/u") {
					// uninstall
					RegistryHelper.RemoveValueFromRegKey(@"Software\Borland\BDS\5.0\Known IDE Assemblies", framework);
					if ((int)RegistryHelper.GetValueFromRegKey(BaseKey.LocalMachine, @"Software\LeXtudio\CodeBeautifierCollection", "InstallForAllUsers", 0) == 1)
					{
						// unregister using expertregistry.exe
						ShellHelper.Execute(expertRegistry, "/u /f:\"" + framework + "\"", true);
						UnfakeLauncher();
					}
					// remove this so CBC is uninstalled
					RegistryHelper.RemoveValueFromRegKey(BaseKey.LocalMachine, @"Software\LeXtudio\CodeBeautifierCollection", "InstallForAllUsers");
					UninstallDllFromGac();
				} else if (args[0] == "/i")
				{
					//for current user only
					RegistryHelper.SetValueToRegKey(BaseKey.LocalMachine, @"Software\LeXtudio\CodeBeautifierCollection", "InstallForAllUsers", 0);
					RegistryHelper.SetValueToRegKey(@"Software\Borland\BDS\5.0\Known IDE Assemblies", framework, "Code Beautifier Collection");

					InstallDllToGac();
				}
			}
		}

		private static void UninstallDllFromGac()
		{
			string exe = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "gacutil.exe");
			Lextm.Diagnostics.ShellHelper.Execute(exe, "-u UnhandledExceptionManager", true);
		}

		private static void InstallDllToGac()
		{
			string fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "UnhandledExceptionManager.dll");
			string exe = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "gacutil.exe");
			Lextm.Diagnostics.ShellHelper.Execute(exe, String.Format("-i \"{0}\"", fileName), true);
		}
		
		static void UnfakeLauncher()
		{
			string bds = Path.Combine(Path.Combine(GetBDSRootDir(), "bin"),
			                          "bds.real.exe");
			string fake = Path.Combine(Path.Combine(GetBDSRootDir(), "bin"),
			                           "bds.exe");
			File.Copy(bds, fake, true);
		}
		
		static void FakeLauncher()
		{
			string bds = Path.Combine(Path.Combine(GetBDSRootDir(), "bin"),
			                          "bds.exe");
			string fake = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "bds.exe");
			File.Copy(bds, Path.ChangeExtension(bds, ".real.exe"), true);
			File.Copy(fake, bds, true);
		}
		
		static string GetBDSRootDir()
		{
			return GetValueFromRegKey(BaseKey.LocalMachine,
			                          @"Software\Borland\BDS\5.0",
			                          @"RootDir",
			                          @"C:\Program Files\CodeGear\RAD Studio\5.0\").ToString();
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
}
