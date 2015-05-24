using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using NUnit.Framework;
using Lextm.CodeBeautifiers.Tool;
using System.IO;

namespace TestCodeBeautifiers {

	[TestFixture]
	public class TestXmlFormatter {
		private string backup = @"myname.bak";
		private string source = @"myname.xml";
		private string desireExt = @".formatted";
		private string folder = @"G:\Borland Studio Projects\CSharp\CBCBackup\sample\";
//		private string backupB = @"broken.bak";
		private string sourceB = @"broken.xml";


		[SetUp]
		public void SetUp( ) {
		}

		[TearDown]
		public void TearDown( ) {
			File.Copy(folder + backup, folder + source, true);
			File.Delete(folder + backup);
//			File.Copy(folder + backupB, folder + sourceB, true);
//			File.Delete(folder + backupB);
		}

		[Test]
//		[ExpectedException(typeof(System.Xml.XmlException))]
		public void TestRunConsole( ) {
			XmlLex.getInstance().RunConsole(new object[]{folder + source}, folder);
			// Open the file to read from.
			StreamReader sr1 = File.OpenText(folder + source);
			StreamReader sr2 = File.OpenText(folder + source + desireExt);
			string s1 = sr1.ReadToEnd();
			//Debug.ConsoleOut(s1);
			string s2 = sr2.ReadToEnd();
			//Debug.ConsoleOut(s2);
			Assert.AreEqual(s1, s2);
			sr1.Close();
			sr2.Close();

			XmlLex.getInstance().RunConsole(new object[]{folder + sourceB}, folder);

			// Open the file to read from.
			sr1 = File.OpenText(folder + sourceB);
			sr2 = File.OpenText(folder + sourceB + desireExt);
			s1 = sr1.ReadToEnd();
			//Debug.ConsoleOut(s1);
			s2 = sr2.ReadToEnd();
			//Debug.ConsoleOut(s2);
			Assert.AreEqual(s1, s2);

			sr1.Close();
			sr2.Close();

		}
	}
}
