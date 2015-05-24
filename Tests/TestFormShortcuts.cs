
namespace TestFramework
{                                        

	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Data;
	using NUnit.Framework;
	using Lextm.CodeBeautifierCollection;
	using Lextm.CodeBeautifierCollection.Gui;
	using System.Windows.Forms;

	// Test methods for class OtaUtils
	[TestFixture]
	public class TestFormShortcuts
	{
		[SetUp]
		public void SetUp()
		{

		}

		[TearDown]
		public void TearDown()
		{
			
		}

		[Test]
		public void TestShowDialog()
		{
			DialogResult result = (new FormShortcuts()).ShowDialog();
			Assert.AreEqual(DialogResult.OK, result);
		}

	}
}
