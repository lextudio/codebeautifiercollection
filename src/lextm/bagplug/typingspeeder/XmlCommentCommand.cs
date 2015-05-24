// this is XML comment command class.
// Copyright (C) 2007  Lex Y. Li
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
using System;
using System.Collections.Generic;
using BeWise.Common.Utils;
using Lextm.OpenTools.CodeDom;

namespace Lextm.BagPlug.TypingSpeeder
{
	/// <summary>
	/// XmlCommentCommand class.
	/// </summary>
	public class XmlCommentCommand : ICommand
	{
		/// <summary>
		/// Executes.
		/// </summary>
		/// <param name="editor">Source editor</param>
		public void Execute(Borland.Studio.ToolsAPI.IOTASourceEditor editor)
		{
			int lineNumber = OtaUtils.GetCurrentLineNumber();
			string last = OtaUtils.GetLineOf(editor, lineNumber - 1);
			if (last == null || last.StartsWith(@"///", StringComparison.Ordinal)) {
				return;
			}

            IList<CodeDomInfo> list = new List<CodeDomInfo>();			
			CodeDomProvider.LoadMethodInfoInto(list, OtaUtils.GetCurrentModule());
			CodeDomProvider.LoadFieldInfoInto(list, OtaUtils.GetCurrentModule());
			CodeDomProvider.LoadEventInfoInto(list, OtaUtils.GetCurrentModule());
			CodeDomProvider.LoadTypeInfoInto(list, OtaUtils.GetCurrentModule());
			for(int i = 0; i < list.Count; i++) {
				CodeDomInfo info = (CodeDomInfo)list[i];
				if (info != null && info.LineNumber == lineNumber + 1) {
					OtaUtils.InsertText(editor, info.GenerateXmlComments());
					return;
				}
			}
		}
	}
}
