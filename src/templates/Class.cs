using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;

namespace InstallTemplates
{
	/// <summary>
	/// Summary description for Class.
	/// </summary>
	class Class
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			//
			// TODO: Add code to start application here
			//
			InstallXmlTemplates();
			//Console.Read();
		}

		private const string MaskIDERegKey = @"Software\Borland\BDS\{0}.0";

		private static string GetIDERegKey(int version) {
			return String.Format(MaskIDERegKey, version);
		}

		private static void InstallXmlTemplates() {
			// only BDS 4.0 needs
			RegistryKey key = Registry.CurrentUser.OpenSubKey(GetIDERegKey(4));
			if (key != null)
			{
				//foreach(string name in key.GetValueNames()) {
				//Console.WriteLine("a name is " + name);
				//}
				string csharpFolder = (string)key.GetValue(@"RootDir", @"C:\Program Files\CodeGear\RAD Studio\5.0\")
					+ @"Objrepos\code_templates\csharp\";

				string templatesFolder = @".\data\templates\csharp";

				Console.WriteLine("Copy all templates in " + templatesFolder +
				                  " to " + csharpFolder);
				try
				{
					// Determine whether the directory exists.
					if (!Directory.Exists(csharpFolder))
					{
						// Create the directory it does not exist.
						Directory.CreateDirectory(csharpFolder);
					}
					foreach (string file in Directory.GetFiles(templatesFolder)) {
						string dest = csharpFolder + Path.GetFileName(file);
						if (File.Exists(dest)) {
							
							DialogResult command = MessageBox.Show("Template " + dest + " already exists. Replaced?",
							                                       "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
							                                       MessageBoxDefaultButton.Button2);
							if (command == DialogResult.No) {
								continue;
							}
						}
						CopyFile(file, dest);
					}
					Console.WriteLine("Template installation is done.");
				} catch (Exception ex){
					Console.WriteLine(ex.ToString());
				}
			} else {
				Console.WriteLine("Unexpected error.");
			}
		}
		private static void CopyFile(string source, string dest) {
			File.Copy(source, dest, true);
		}
	}
}
