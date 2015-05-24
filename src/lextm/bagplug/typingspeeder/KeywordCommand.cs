// this is the keyword command class.
//  It defines a keyword and its action.
// Copyright (C) 2005-2007  Lex Y. Li
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
using System.Drawing;

using BeWise.Common.Utils;
using Borland.Studio.ToolsAPI;
using Lextm.Diagnostics;

namespace Lextm.BagPlug.TypingSpeeder
{
	/// <summary>
	/// Keyword that triggers an Auto Complete action.
	/// </summary>
    /// <remarks>It is part of the command pattern.</remarks>
	class KeywordCommand : ICommand {
		
		//private Language language;
		private string text;
		private string complement;
		private Point movement;
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="text">Text</param>
		/// <param name="complement">Complement of the text</param>
		///// <param name="language">Language</param>
		internal KeywordCommand(string text, string complement/*, Language language*/)
			: this(text, complement/*, language*/, new Point()) {}
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="text">Text</param>
		/// <param name="complement">Complement</param>
		///// <param name="language">Language</param>
		/// <param name="movement">Movement</param>
		internal KeywordCommand(string text, string complement, /*Language language,*/ Point movement) {
			this.text = text;
			this.complement = complement;
			// This.language = language;
			this.movement = movement;			
		}
		
		public void Execute(IOTASourceEditor editor) {
			if (!OtaUtils.GetCurrentLine(editor).TrimStart().StartsWith(@"//", StringComparison.Ordinal)
			    && Char.IsWhiteSpace(OtaUtils.GetCharAfterCursor(editor)))
			{
				LoggingService.Debug(this);
				OtaUtils.InsertText(editor, this.complement, movement.X, movement.Y);
			}
		}

		public override string ToString()
		{
			return this.text + " + " + this.complement + " : X " + this.movement.X + " Y " + this.movement.Y;
		}
	}
}

