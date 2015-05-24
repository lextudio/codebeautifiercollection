/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/3/31
 * Time: 20:07
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using NUnit.Framework;
using Borland.Studio.ToolsAPI;
using System.IO;
using NMock2;
using Lextm.WiseEditor.ProjectAid;

namespace Lextm.WiseEditor.Tests
{
	/// <summary>
	/// Description of TestHandlerFactory.
	/// </summary>
	[TestFixture]
	public class TestHandlerFactory
	{
		[SetUp]
		public void SetUp()
		{			
			using (BinaryWriter writer = new BinaryWriter(File.Open(Path.Combine(Path.GetTempPath(), "test.dproj"), FileMode.Create)))
			{
				writer.BaseStream.Write(Resources.AddMany, 0, Resources.AddMany.Length);
				writer.Close();
			}
		}
		[TearDown]
		public void TearDown()
		{
			File.Delete(Path.Combine(Path.GetTempPath(), "test.dproj"));
		}
		
		[Test]
		public void TestGetHandlerFor()
		{
			Mockery mocks;
			IOTAProject mockProject;
			IUser mockUser;
			IGenerator mockGenerator;
			mocks = new Mockery();
			mockProject = mocks.NewMock<IOTAProject>();
			mockUser = mocks.NewMock<IUser>();
			mockGenerator = mocks.NewMock<IGenerator>();
			//TODO: use Mock objects.
			string fileName = Path.Combine(Path.GetTempPath(), "test.dproj");
			Expect.AtLeast(3).On(mockProject).GetProperty("FileName").
				Will(Return.Value(fileName));
			Expect.Once.On(mockProject).Method("Save").WithAnyArguments().Will(Return.Value(true));
			Expect.Once.On(mockProject).Method("AddFile").WithAnyArguments().Will(Return.Value(null));
			Expect.Once.On(mockUser).Method("ProvideKeyFile").Will(Return.Value(null));
			Expect.Once.On(mockGenerator).Method("GenerateKey").WithAnyArguments().Will(Return.Value(true));
			IHandler handler = HandlerFactory.GetHandlerFor(mockProject);
			Assert.IsTrue(handler is Manner20UnsignedHandler);
			(handler as Manner20UnsignedHandler).User = mockUser;
			(handler as Manner20UnsignedHandler).Generator = mockGenerator;
			handler.Handle();
			IHandler handler2 = HandlerFactory.GetHandlerFor(mockProject);
			Assert.IsTrue(handler2 is Manner20SignedHandler);
			mocks.VerifyAllExpectationsHaveBeenMet();
		}
		
//		[Test]
//		public void TestGetSignedHandlerFor2()
//		{
//			Rhino.Mocks.MockRepository mocks;
//			IOTAProject mockProject;
//			IUser mockUser;
//			IGenerator mockGenerator;
//			mocks = new Rhino.Mocks.MockRepository();
//			mockProject = mocks.PartialMock<IOTAProject>();
//			mockUser = mocks.CreateMock<IUser>();
//			mockGenerator = mocks.CreateMock<IGenerator>();
//			//TODO: use Mock objects.
//			string fileName = Path.Combine(Path.GetTempPath(), "test.dproj");
//			Rhino.Mocks.Expect.Call(mockProject.FileName).Repeat.Times(3).Return(fileName);
//			//Rhino.Mocks.Expect.Call(mockProject.).Method("AddFile").WithAnyArguments().Will(Return.Value(null));
//			Rhino.Mocks.Expect.Call(mockUser.ProvideKeyFile()).Return(null);
//			Rhino.Mocks.Expect.Call(mockGenerator.GenerateKey("test")).Return(true);
//			mocks.ReplayAll();
//			
//			IHandler handler = HandlerFactory.GetHandlerFor(mockProject);
//			Assert.IsTrue(handler is Manner20UnsignedHandler);
//			(handler as Manner20UnsignedHandler).User = mockUser;
//			(handler as Manner20UnsignedHandler).Generator = mockGenerator;
//			handler.Handle();
//			IHandler handler2 = HandlerFactory.GetHandlerFor(mockProject);
//			Assert.IsTrue(handler2 is Manner20SignedHandler);
//			mocks.VerifyAll();
//		}
	}
}
