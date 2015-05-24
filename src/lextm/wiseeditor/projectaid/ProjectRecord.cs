/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/3/30
 * Time: 12:38
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using Borland.Studio.ToolsAPI;

namespace Lextm.WiseEditor.ProjectAid
{
	class ProjectRecord
	{
		IList<OptionSet> _options;
		
		public IList<OptionSet> Options {
			get { return _options; }
			set { _options = value; }
		}
		
		IOTAProject _interface;
		
		public IOTAProject Interface
		{
			get { return _interface; }
			set { _interface = value; }
		}
	}
}
