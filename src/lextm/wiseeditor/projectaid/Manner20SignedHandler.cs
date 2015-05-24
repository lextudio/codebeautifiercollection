/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/3/30
 * Time: 12:38
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Lextm.OpenTools;

namespace Lextm.WiseEditor.ProjectAid
{
	class Manner20SignedHandler : IHandler
	{
		string _projectName;
		
		internal Manner20SignedHandler(string projectName) {
			_projectName = projectName;
		}
		
		public void Handle()
		{
			MessageService.Show("This project is signed in 2.0 manner: " + _projectName);
		}
	}
}
