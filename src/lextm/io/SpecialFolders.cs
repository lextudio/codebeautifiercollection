using System;
using System.Collections.Generic;
using System.Text;
using Lextm.Win32;

namespace Lextm.IO
{
	/// <summary>
	/// Special folders.
	/// </summary>
	public sealed class SpecialFolders
	{
		SpecialFolders() { }

		/// <summary>
		/// Gets .NET SDK 2.0 root folder.
		/// </summary>
		/// <returns></returns>
		public static string DotNetSdkRoot
		{
			get 
			{
				return RegistryHelper.GetValueFromRegKey(BaseKey.LocalMachine, @"Software\Microsoft\.NETFramework", "sdkInstallRootv2.0", @"C:\Program Files\Microsoft.NET\SDK\v2.0\").ToString();
			}
		}
	}
}
