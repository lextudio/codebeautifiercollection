using System;
using NUnit.Framework;
using BeWise.Common.Utils;

namespace BeWise.Common.Tests
{
	// Test methods for class OtaUtils
	[TestFixture]
	public class TestOtaUtils
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
		public void TestIsCOrCSFile()
		{
			string fileName;
			bool returnValue;
			fileName = "I.h";
			returnValue = OtaUtils.IsCOrCSFile(fileName);
			Assert.IsTrue(returnValue);

			fileName = "h.c";
			returnValue = OtaUtils.IsCOrCSFile(fileName);
			Assert.IsTrue(returnValue);

			fileName = "h.cpp";
			returnValue = OtaUtils.IsCOrCSFile(fileName);
			Assert.IsTrue(returnValue);

			fileName = "h.hpp";
			returnValue = OtaUtils.IsCOrCSFile(fileName);
			Assert.IsTrue(returnValue);

			fileName = "h.cxx";
			returnValue = OtaUtils.IsCOrCSFile(fileName);
			Assert.IsTrue(returnValue);

			fileName = "h.hxx";
			returnValue = OtaUtils.IsCOrCSFile(fileName);
			Assert.IsTrue(returnValue);

			fileName = "h.cc";
			returnValue = OtaUtils.IsCOrCSFile(fileName);
			Assert.IsTrue(returnValue);

			fileName = "h.hh";
			returnValue = OtaUtils.IsCOrCSFile(fileName);
			Assert.IsTrue(returnValue);

			fileName = "h.cs";
			returnValue = OtaUtils.IsCOrCSFile(fileName);
            Assert.IsTrue(returnValue);

			fileName = "h.b";
			returnValue = OtaUtils.IsCOrCSFile(fileName);
			Assert.IsFalse(returnValue);
		}
	}
}
