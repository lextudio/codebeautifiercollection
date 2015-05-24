// This is event info class. Ported from SBT event info class.
// Copyright (C) 2007  Lex Y. Li
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
using System;
using System.CodeDom;
using System.Diagnostics;

namespace Lextm.OpenTools.CodeDom
{
	/// <summary>
	/// EventInfo.
	/// </summary>
	public class EventInfo : CodeDomInfo
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="tag">Tag</param>
		/// <param name="className">Class name</param>
		public EventInfo(CodeMemberEvent tag, string className) 
			: base(tag, className)
		{
			Trace.Assert(tag != null);
			name = tag.Name;
			lineNumber = tag.LinePragma.LineNumber;
		}
		private string name;
		private int lineNumber;
		/// <summary>
		/// Generates XML comments.
		/// </summary>
		/// <returns></returns>
		public override string GenerateXmlComments()
		{
			return "<summary>" + Environment.NewLine + "/// " + Name + 
				" event." + Environment.NewLine + "/// </summary>";
		}
		/// <summary>
		/// Code DOM type.
		/// </summary>
		public override string CodeDomType {
			get { return "Event"; }
		}
		/// <summary>
		/// Full name.
		/// </summary>
		public override string FullName {
			get { return ClassName + "." + Name; }
		}
		/// <summary>
		/// Image index.
		/// </summary>
		public override int ImageIndex {
			get { return 0; }
		}
		/// <summary>
		/// Line number.
		/// </summary>
		public override int LineNumber {
			get { return lineNumber; }
		}
		/// <summary>
		/// Name.
		/// </summary>
		public override string Name {
			get { return name; }
		}
	}
}
