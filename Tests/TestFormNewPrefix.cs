
/// <summary>
/// CSharp NUnit Test Case
/// This unit contains a skeleton test case class generated by the Test Case Wizard.
/// Modify the generated code to correctly setup and call the methods from the unit being tested.
/// </summary>

namespace TestWiseEditor
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using NUnit.Framework;
	using Lextm.WiseEditor.Gui;
	using System.Windows.Forms;



	// Test methods for class FormNewPrefix
	[TestFixture]
	public class TestFormNewPrefix
	{
		
		private FormNewPrefix formNewPrefix;
		
		[SetUp]
		public void SetUp()
		{
			formNewPrefix = new FormNewPrefix();
		}
		
		[TearDown]
		public void TearDown()
		{
			formNewPrefix = null;
		}
		
		[Test]
		public void TestgetInstance()
		{
			FormNewPrefix returnValue;
			returnValue = formNewPrefix;
			string clb = "clb";
			returnValue.lblControlName.Text = "ComboListBox";
			returnValue.txtPrefix.Text = clb;
			DialogResult result = returnValue.ShowDialog();
			// TODO: Validate method results
			Assert.AreEqual("clb", returnValue.txtPrefix.Text);
			Assert.AreEqual(DialogResult.Cancel, result);
		}
	}
}
