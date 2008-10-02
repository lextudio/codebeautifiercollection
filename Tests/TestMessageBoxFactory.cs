using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Lextm.Windows.Forms;
using System.Windows.Forms;

namespace Lextm.Common.Tests
{
	[TestFixture]
	public class TestMessageBoxFactory
	{
		[SetUp]
		public void Setup()
		{
			//temp = PSTaskDialog.cTaskDialog.NoParentForm;
			//PSTaskDialog.cTaskDialog.NoParentForm = true;
		}
		
		//bool temp;
		
		[TearDown]
		public void TearDown()
		{
			//PSTaskDialog.cTaskDialog.NoParentForm = temp;
		}
		
		[Test]
		public void TestMethod()
		{
			// TODO: Add your test.
//			MessageBoxFactory.Debug("Debug");
//			MessageBoxFactory.Info("Info");
//			MessageBoxFactory.Warn("Warn");
//			MessageBoxFactory.Error("Error");
//			MessageBoxFactory.Fatal("Fatal");
			//Assert.AreEqual(DialogResult.Yes, MessageBoxFactory.Confirm("Say Yes"));
			//Assert.AreEqual(DialogResult.No, MessageBoxFactory.Confirm("Say No"));
		}
	}
}
