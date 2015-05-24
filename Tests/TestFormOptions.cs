namespace TestFramework
{                                        

	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Data;
	using NUnit.Framework;
	using Lextm.CodeBeautifierCollection.Gui;
	using System.Windows.Forms;

	// Test methods for class OtaUtils
	[TestFixture]
	public class TestFormOptions
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
			DialogResult result = FormPreferences.getInstance().ShowDialog();
			Assert.AreEqual(DialogResult.OK, result);
		}

	}
}
