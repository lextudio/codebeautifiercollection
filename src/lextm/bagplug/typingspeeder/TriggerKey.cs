// this is the trigger key class.
//		Valid trigger keys are stored here as well as the event handler.
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
using System.Collections.Generic;
using BeWise.Common.Utils;
using Borland.Studio.ToolsAPI;
using Lextm.Diagnostics;
using Lextm.OpenTools;

namespace Lextm.BagPlug.TypingSpeeder
{
	/// <summary>
	/// TriggerKey is a key analyser based on IOTAEditor.Modified event. It is not accurate enough.
	/// </summary>
	class TriggerKey : IComparable, IKey
	{
		internal TriggerKey(char key) {
			this.key = key;	
		}
		/// <summary>
		/// Accepts a key stroke.
		/// </summary>
		/// <param name="editor">Editor</param>
		/// <param name="key">Key</param>
		/// <returns>true if done, false if not.</returns>
		public void Accept(IOTASourceEditor editor) {
			if (TriggerKeys.ContainsKey(key))
			{
				LoggingService.Info("invoke autocompletion");
				TriggerKeys[key].KeyHandler(editor, new FireEventArgs(/*this.key*/));
			} 
		}
		
		private static Language GetLanguage(string fileName) {
			if (OtaUtils.IsCSFile(fileName)) {
				return Language.CSharp;
			} else if (OtaUtils.IsDelphiFile(fileName)) {
				return Language.Delphi;
			} else if (OtaUtils.IsCFile(fileName)) {
				return Language.CPlusPlus;
			} else {
				return Language.Unknown;
			}
		}
        /*TODO -olextm -creimplement : Make this work in a easy but less buggy way.*/
        //private static void BracketKeyEventHandler(object sender, FireEventArgs e) {
        //    //CompareEndings(sender as IOTASourceEditor, NeutralEndings);
        //    IOTASourceEditor editor = sender as IOTASourceEditor;
        //    if (editor != null)
        //    {
        //        switch(e.Key) {
        //            case '[':
        //                OTAUtils.InsertText(editor, "]");
        //                break;
        //            case '(':
        //                OTAUtils.InsertText(editor, ")");
        //                break;
        //            case '\'':
        //                OTAUtils.InsertText(editor, "\'");
        //                break;
        //            case '"':
        //                OTAUtils.InsertText(editor, "\"");
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //}
        
		private static void SpaceKeyEventHandler(object sender, FireEventArgs e) {
			
			LoggingService.EnterMethod();
			CommandRegistry.DispatchKeywordIn(sender as IOTASourceEditor,
			    GetLanguage((sender as IOTAEditor).FileName));
			LoggingService.LeaveMethod();
			
	    }
		/// <summary>
		/// Valid trigger keys.
		/// </summary>
        private static IDictionary<char, TriggerKey> TriggerKeys = new Dictionary<char, TriggerKey> {
			{' ', new TriggerKey(' ', new FireEventHandler(SpaceKeyEventHandler))}//,
			//new TriggerKey('\'', new FireEventHandler(BracketKeyEventHandler)),
			//new TriggerKey('"', new FireEventHandler(BracketKeyEventHandler)),
			//new TriggerKey('[', new FireEventHandler(BracketKeyEventHandler))//,
			//new TriggerKey('(', new FireEventHandler(BracketKeyEventHandler))
		};
		
    	/// <summary>Compares to.</summary>	
    	/// <param name="obj">Object</param>
    	/// <returns>1 if greater than, 0 if equal to, and -1 if less than.</returns>
		public int CompareTo(object obj) {
			//if (obj is TriggerKey) {
			TriggerKey theKey = obj as TriggerKey;
			if (theKey != null)
			{
				return this.key.CompareTo(theKey.key);
			} else //if (obj is char) {
			{
				LoggingService.Info("this.key is " + this.key.ToString() + " and the key is " + obj.ToString());
				return this.key.CompareTo(obj);
			}
			//throw new ArgumentException("object is not an TriggerKey");
		}
    		
		private char key;

		private FireEventHandler KeyHandler;
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="key">Key</param>
		/// <param name="handler">Handler</param>
		private TriggerKey(char key, FireEventHandler handler) {
			this.key = key;
			this.KeyHandler = handler;		
    	}		
	}
	
	/// <summary>
	/// FireEventHandler delegate.
	/// </summary>
	internal delegate void FireEventHandler(object sender, FireEventArgs e);
	// FireEventArgs: a custom event inherited from EventArgs.
	/// <summary>
	/// Fire event args.
	/// </summary>
	class FireEventArgs: EventArgs {
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="key">Key pressed</param>
		internal FireEventArgs(/*char key*/) {
	        // This.key = key;
	    }
	
	    // The fire event will have two pieces of information-- 
	    // 1) Where the fire is, and 2) how "ferocious" it is.  
	
	    //private char key;
        ///// <summary>
        ///// Key.
        ///// </summary>
        //internal char Key {
        //    get {
        //        return key;
        //    }
        //}
	
	}    //end of class FireEventArgs
}
