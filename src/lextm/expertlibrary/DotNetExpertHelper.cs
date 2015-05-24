/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2007/11/3
 * Time: 16:27
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System.Collections.Generic;
using Lextm.IO;

namespace Lextm.ExpertLibrary
{
	public class DotNetExpertHelper
	{
		static DotNetExpertHelper instance;
		
		internal static DotNetExpertHelper Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new DotNetExpertHelper();
				}
				return instance;
			}
		}
		
		DotNetExpertHelper()
		{
			Microsoft.Win32.RegistryKey key =
				Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Borland\BDS\5.0\Known IDE Assemblies");
			if (key != null) {
				foreach(string name in key.GetValueNames()) {
					files.Add(name);// A .NET expert key name is the path.
				}
			}
		}
		
		IList<string> files = new List<string>();
		
		internal bool Exists(string fileName)
		{
			foreach (string file in files)
			{
				if (FileHelper.IsTheSameFile(file, fileName))
				{
					return true;
				}
			}
			return false;
		}
	}
}
