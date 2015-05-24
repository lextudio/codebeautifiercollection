/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2007/11/3
 * Time: 16:27
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using Lextm.IO;

namespace Lextm.ExpertLibrary
{
	class Win32ExpertHelper
	{
		static Win32ExpertHelper instance;
		
		internal static Win32ExpertHelper Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new Win32ExpertHelper();
				}
				return instance;
			}
		}
		
		Win32ExpertHelper()
		{
			Microsoft.Win32.RegistryKey key =
				Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Borland\BDS\5.0\Experts");
			if (key != null) {
				foreach(string name in key.GetValueNames()) {
					files.Add(key.GetValue(name).ToString());// A Win32 expert key value is the path.
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
