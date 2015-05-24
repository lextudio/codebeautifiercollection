/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2007/11/3
 * Time: 16:27
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Lextm.Diagnostics;

namespace Lextm.ExpertLibrary
{
	public class Win32Expert : IExpert
	{
		string name;
		string fileName;
		
		Win32Expert() {}
		public Win32Expert(string name, string fileName)
		{
			this.name = name;
			this.fileName = fileName;
		}
		public override string ToString()
		{
			return name + ";" + fileName;
		}
		
		public void CheckInstall()
		{
			if (!Exists())
			{
//				ShellHelper.Execute("expertinstaller.exe", 
//				                    "/n:" + name + " /f:\"" + fileName + "\"",  
//				                    true);
				Install();
			}
		}
		
		bool Exists()
		{
			return Win32ExpertHelper.Instance.Exists(fileName);
		}
		
		public void Install()
		{
			ExpertRegistry.Install(name, fileName);
		}
		
		public string FileName {
			get {
				return fileName;
			}
		}
	}
}
