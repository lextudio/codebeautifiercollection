namespace Lextm.OpenTools.Tests
{ 
	using System;
	using NUnit.Framework;
	using Lextm.OpenTools.IO;

	// Test methods for class OtaUtils
	[TestFixture]
	public class TestPath
	{
		[SetUp]
		public void SetUp()
		{

		}

		[TearDown]
		public void TearDown()
		{

		}

		private string GetRoot() {
			return System.IO.Directory.GetParent(GetAssemblyFolder()).Parent.Parent.FullName;
		}
        
		private string GetAssemblyFolder() {
			return System.Reflection.Assembly.GetExecutingAssembly().Location;
		}

		[Test]
		public void TestDocFolderFile()
		{
			string config = String.Format(@"{0}\doc\game.txt", GetRoot());
			Assert.AreEqual(config, Path.GetDocFile("game.txt"));
		}

		[Test]
		public void TestAboutBoxFile()
		{
			string config = String.Format(@"{0}\data\images\LeXtudio36", GetRoot());
			Assert.AreEqual(config, Path.AboutBoxFile);
		}

		[Test]
		public void TestGetPreferencesFile()
		{
			string config = String.Format(@"{0}\data\preferences\lex.ota", GetRoot());
			Assert.AreEqual(config, Path.GetPreferencesFile("lex"));
		}

		[Test]
		public void TestPreferencesFolder()
		{
			string config = String.Format(@"{0}\data\preferences", GetRoot());
			Assert.AreEqual(config, Path.PreferencesFolder);
		}

		[Test]
		public void TestGetDataFile()
		{
			string config = String.Format(@"{0}\data\lex.feature", GetRoot());
			Assert.AreEqual(config, Path.GetDataFile("lex.feature"));
		}

		[Test]
		public void TestConfigFile()
		{
			string config = String.Format(@"{0}\data\config\JCFSettings.cfg", GetRoot());
			Assert.AreEqual(config, Path.DefaultConfigFile);
		}

		[Test]
		public void TestSplashFile()
		{
			string config = String.Format(@"{0}\data\images\LeXtudio24", GetRoot());
			Assert.AreEqual(config, Path.SplashFile);
		}

		[Test]
		public void TestBundledFolder()
		{
			string config = String.Format(@"{0}\bundled", GetRoot());
			Assert.AreEqual(config, Path.BundledFolder);
		}
           
		[Test]
		public void TestGetImageName()
		{
			string config = String.Format(@"{0}\data\images\1", GetRoot());
			Assert.AreEqual(config, Path.GetImageFile("1"));
		} 

	}
}
