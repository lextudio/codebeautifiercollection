/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/3/30
 * Time: 12:38
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using Lextm.OpenTools;

namespace Lextm.WiseEditor.ProjectAid
{
	class Manner20UnsignedHandler : IHandler
	{
		internal Manner20UnsignedHandler(ProjectRecord project) 
		{
			_project = project;
		}
		
		IUser _user = new User();
		
		internal IUser User
		{
			get
			{
				return _user;
			}
			set
			{
				_user = value;
			}
		}
		
		IGenerator _generator = new Generator();
		
		internal IGenerator Generator
		{
			get
			{
				return _generator;
			}
			set
			{
				_generator = value;
			}
		}
		
		ProjectRecord _project;
		
		public void Handle()
		{
			string keyFile = User.ProvideKeyFile();
			if (string.IsNullOrEmpty(keyFile))
			{
				string projectFolder = Path.GetDirectoryName(_project.Interface.FileName);
				keyFile = Path.Combine(projectFolder, "key.snk");
				bool succeeded = Generator.GenerateKey(keyFile);
				if (!succeeded) {
					OptionSet.Clear(_project.Options);
					return;
				}
				_project.Interface.AddFile(keyFile);
				_project.Interface.Save(false, true);
				MessageService.Show("Key file is added to current project: " + keyFile);
			}
			OptionSet.Fill(_project.Options, keyFile);
			MessageService.Show("Porject is signed. Project: " + _project.Interface.FileName + "; Key file: " + keyFile);
		}		
	}
}
