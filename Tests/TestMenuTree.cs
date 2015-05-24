namespace Lextm.CodeBeautifierCollection.Tests
{                                        

	using System;
	using Lextm.CodeBeautifierCollection.Collections;
	using NUnit.Framework;

	// Test methods for class OtaUtils
	[TestFixture]
	public class TestMenuTree
	{
		private MenuBuilder mt;
		[SetUp]
		public void SetUp()
		{
			mt = MenuBuilder.getInstance();
		}

		[TearDown]
		public void TearDown()
		{

		}

		[Test]
		public void TestRegister()
		{
			mt.Build();
			Console.Out.WriteLine(mt.NodeCount.ToString());
			Assert.AreEqual(1, mt.NodeCount);
		}
	}
}
