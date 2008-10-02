
namespace Lextm.Common.Tests {
    using System;
    using System.Collections;
    using System.ComponentModel;
    using NUnit.Framework;
    using Lextm.Windows.Forms;
    using System.Windows.Forms;
    using System.Threading;
    
    
    
    // Test methods for class AlertForm
    [TestFixture]
    public class TestAlertForm {
    
        //private AlertForm alertForm;
        
        [SetUp]
		public void SetUp() {
			//string tip = "this is a very very very very very very long tip.";
			//alertForm = new AlertForm(tip);
        }
        
        [TearDown]
		public void TearDown() {
            //alertForm = null;
        }
        
        [Test]
        public void TestShow() {
			string tip = "this is a very very very very very very long tip.";
			AlertForm.ShowForm(tip);
			System.Threading.Thread.Sleep(1000);
			
			Assert.AreEqual(DialogResult.Yes,
				MessageBox.Show("do you see the alert",
					"??", MessageBoxButtons.YesNo));
        }
    }
}
