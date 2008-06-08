using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO;
#pragma warning disable 1591
namespace Lextm.Common.Tests
{
	[TestFixture]
	public class TestFileHelper
	{
		[Test]
		public void TestIsDotNetAssembly()
		{
			string dotNet = Path.Combine(
				Environment.ExpandEnvironmentVariables("%WinDir%"),
				@"Microsoft.NET\Framework\v2.0.50727\System.dll");
			string win32 = Path.Combine(
				Environment.ExpandEnvironmentVariables("%WinDir%"), 
				@"system32\user32.dll");
			string win32Exe = Path.Combine(
				Environment.ExpandEnvironmentVariables("%WinDir%"),
				"notepad.exe");
			string dotNetExe = Path.Combine(
				Environment.ExpandEnvironmentVariables("%WinDir%"),
				@"Microsoft.NET\Framework\v2.0.50727\MSBuild.exe");
			
			Assert.IsTrue(Lextm.IO.FileHelper.IsDotNetAssembly(dotNet));
			Assert.IsFalse(Lextm.IO.FileHelper.IsDotNetAssembly(win32));
			Assert.IsFalse(Lextm.IO.FileHelper.IsDotNetAssembly(win32Exe));
			Assert.IsTrue(Lextm.IO.FileHelper.IsDotNetAssembly(dotNetExe));
			
//			Assert.IsTrue(Lextm.IO.FileHelper.IsDotNetAssembly3(dotNet));
//			Assert.IsFalse(Lextm.IO.FileHelper.IsDotNetAssembly3(win32));
//			Assert.IsFalse(Lextm.IO.FileHelper.IsDotNetAssembly3(win32Exe));
//			Assert.IsTrue(Lextm.IO.FileHelper.IsDotNetAssembly3(dotNetExe));
		}
	}
}
#pragma warning restore 1591