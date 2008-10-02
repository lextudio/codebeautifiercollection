#pragma warning disable 1591
namespace Lextm.Common.Tests
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using NUnit.Framework;
	using Lextm.Diagnostics;
	using System.Windows.Forms;
	using System.Threading;



	// Test methods for class Stopwatch
	[TestFixture]
	public class TestStopwatch
	{
		
		private Stopwatch stopwatch;
		
		[SetUp]
		public void SetUp()
		{
			stopwatch = new Stopwatch();
		}
		
		[TearDown]
		public void TearDown()
		{
			stopwatch = null;
		}
		
		[Test]
		public void TestAll()
		{
			stopwatch.Start();
			// TODO: Validate method results
			Thread.Sleep(100);
			int first;
			Thread.Sleep(300);
			first = stopwatch.Interval;
			// TODO: Validate method results
			//Console.WriteLine(first);
			int second;
			stopwatch.Suspend();
			Thread.Sleep(500);
			stopwatch.Resume();
			
			Thread.Sleep(800);
			second = stopwatch.Interval;
			//Console.WriteLine(second);

			int all = stopwatch.Value;
			stopwatch.Stop();
			// TODO: Validate method results
			//Console.WriteLine(all);
			Assert.AreEqual(all, first + second);
		}
	}
}
#pragma warning restore 1591