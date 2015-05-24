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
	class Manner1xHandler : IHandler
	{
		string _projectName;
		
		internal Manner1xHandler(string projectName) {
			_projectName = projectName;
		}
		
		public void Handle()
		{
			MessageService.Show("This project is signed in 1.x manner: " + _projectName);
		}
	}
}
