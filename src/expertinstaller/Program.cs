/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2007/11/3
 * Time: 17:30
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using Lextm.IO;
using Lextm.ExpertLibrary;

namespace expertinstaller
{
	class Program
	{
		public static void Main(string[] args)
		{
			string name = null;
			string fileName = null;
			foreach (string arg in args)
			{
				if (arg.StartsWith("/n:"))
				{
					name = arg.Substring(3);
				}
				if (arg.StartsWith("/f:"))
				{
					fileName = arg.Substring(3);
				}
			}
			if (string.IsNullOrEmpty(fileName))
			{
				return;
			}
			if (string.IsNullOrEmpty(name))
			{
				name = Path.GetFileNameWithoutExtension(fileName);
			}
			IExpert expert;
			if (FileHelper.IsDotNetAssembly(fileName))
			{
				expert = new DotNetExpert(name, fileName);
			} 
			else 
			{
				expert = new Win32Expert(name, fileName);
			}
			expert.Install();
		}
	}
}