// this is the key checker class.
//		With IOTASourceEditor interface it tries to determine what key is presed by user.
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
using System.Drawing;
using BeWise.Common.Utils;
using Borland.Studio.ToolsAPI;
using Lextm.Diagnostics;

namespace Lextm.BagPlug.TypingSpeeder
{

	/// <summary>Key checker.</summary>
	/// <remarks>
	/// <para>Use this class to check what key is down
	/// when source editor is modified.</para>
	/// <para>However, now it is only partially okay.
	/// Sometimes it gives the wrong key.</para>
	/// <para>If CodeGear provides an alternative API, things can be much
	/// easier.</para>
	/// </remarks>
	sealed class KeyChecker {

		private KeyChecker() {}

		private static Point lastPoint;
		private static bool initialized;
		
		internal static IKey GetKeyFrom(Borland.Studio.ToolsAPI.IOTASourceEditor editor) {
			// IsBackSpace: backspace is pressed.
			// IsEnter:		row is changed, and Enter is pressed.
			// IsBlockEdit:	IDE AutoCompletion or Code Insight fills, or
			//			Cut/Copy,or Indent/unindent, or block edit.
			// IsError:		GetCharBeforeCursor error.
			//bool IsBackSpace = false;
			//bool IsEnter = false;
			//bool IsBlockEdit = false;
			//bool IsError = false;
			IOTAEditView view = OtaUtils.GetEditView(editor);
			if (view == null) {
				LoggingService.Warn("null view");
				return InvalidKey.Instance;
			}
			
			if (!initialized) {
				LoggingService.Info("initial row");
				LoggingService.Info("initial column");
				SavePoint(view);
				initialized = true;
				// initial state, no action.
				return InvalidKey.Instance;
			}
			char result = OtaUtils.GetCharBeforeCursor(editor);
			// check key.
			// TODO: what is 9, seems it leads to bug.
			if (result == char.MinValue) {
				LoggingService.Info("min char");
				SavePoint(view);
				return InvalidKey.Instance;
			}
			
			if (lastPoint.Y != view.LastEditRow) {
				//LoggingService.AddDebug("Row changed. reget column.");
//					if (view.LastEditRow == lastPoint.Row + 1) {
//						IsEnter = true;
//						LoggingService.Info("Enter next row.");
//					} else {
//						IsBlockEdit = true;
//						LoggingService.Info("Move to other row.");
//					}
				SavePoint(view);
				return InvalidKey.Instance;
			}
			
			// same row.
			if (view.LastEditColumn == lastPoint.X) {
				LoggingService.Info("initial column");
			} else if (view.LastEditColumn == lastPoint.X + 1) {
				LoggingService.Info("Normal user input");
			} else if (view.LastEditColumn == lastPoint.X + 2) {
				//if (LastIsEnter())
				LoggingService.Info("Enter after XML tag");
			} else if (view.LastEditColumn == lastPoint.X - 1) {
				//IsBackSpace = true;
				SavePoint(view);
				return InvalidKey.Instance;
				// normally the left is greater than or equal to the right.
			} else {
				//IsBlockEdit = true;
				LoggingService.Info("movement is " +
				                    (view.LastEditColumn - lastPoint.X));
				SavePoint(view);
				return InvalidKey.Instance;
			}			
			SavePoint(view);
			return new TriggerKey(result);
		}
		
		private static void SavePoint(IOTAEditView view) {
			lastPoint.X = view.LastEditColumn;
			lastPoint.Y = view.LastEditRow;
		}
	}
}
