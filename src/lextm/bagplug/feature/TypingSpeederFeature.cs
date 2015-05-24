// this is the typing speeder (autocompletion) class. Ported from SBT.
//		Moved out from notification and don't need a timer.
//		Functions popped when Space is pressed.
//		More functions added. And whole algorithm is changes.
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
using BeWise.Common.Utils;
using Borland.Studio.ToolsAPI;
using Lextm.Diagnostics;
using Lextm.OpenTools;

namespace Lextm.BagPlug.Feature {
	/// <summary>
	/// AutoCompletion feature.
	/// </summary>
	public class TypingSpeederFeature : CustomFeature {
		
		//private const string Name = "AutoCompletion";
		//private const string TabGeneral = "AutoCompletion";
		/**************************************************************/
		/*                     Constructor
        /**************************************************************/
		/// <summary>
		/// Constructor.
		/// </summary>
		public TypingSpeederFeature() {
			
			LoggingService.EnterMethod();
			if (!ExpertChecker.CastaliaInstalled()) {
				IOTAService _Service = OtaUtils.GetService();
				_Service.FileNotification += new FileNotificationHandler(OpenCloseFileNotificationHandler);
			} else {
				LoggingService.Warn("Castalia is installed, so disable Typing Speeder.");
			}
			LoggingService.LeaveMethod();			
		}

		private static IOTAEditor fEditor;
		
		private static void KeyDispatcher(object sender, EventArgs e) {
			LoggingService.EnterMethod();
			
			fEditor = sender as IOTAEditor;
			
			try {
				IOTAModule _Module = OtaUtils.GetCurrentModule();
				IOTAEditor _Editor = OtaUtils.GetEditorWithSourceEditor(_Module);

				if ((fEditor != null) && (_Editor != null) &&
				    _Editor.FileName == fEditor.FileName) 
				{
					IOTASourceEditor _SourceEditor = OtaUtils.GetCurrentSourceEditor();

					if (IsValidForAutoCompletion(_Module, _SourceEditor)) {
						//TODO: the above fails some time when a file is open without a project.
						TypingSpeeder.KeyChecker.GetKeyFrom(_SourceEditor).Accept(_SourceEditor);
					}
					else {
						LoggingService.Warn("not valid for AC");
					}
				} else {
					LoggingService.Warn("empty editor or wrong file");
				}
			} catch (Exception ex) {
				Lextm.Windows.Forms.MessageBoxFactory.Fatal(ex);
				throw;
			} finally {
				fEditor = null;
			}
			LoggingService.LeaveMethod();
		}
		
		private static bool IsValidForAutoCompletion(IOTAModule aModule, IOTASourceEditor aSourceEditor) {
			return (aSourceEditor != null) &&
				(String.IsNullOrEmpty(OtaUtils.GetSelectedText(aModule)));
		}

		private static void OpenCloseFileNotificationHandler(object sender, FileNotificationEventArgs e) {
			try {
				LoggingService.Info("file opening is " + e.FileName);
				if (!OtaUtils.IsSourceFile(e.FileName)) {
					LoggingService.Warn("not a source file.");
					return;
				}
				if (e.NotifyCode == OTAFileNotification.ofnFileOpened) {
					
					IOTAModule _Module = OtaUtils.GetModuleServices().FindModule(e.FileName);
					
					if (_Module != null) {
						IOTAEditor _Editor = OtaUtils.GetEditorWithSourceEditor(_Module);

						if (_Editor != null) {
							_Editor.Modified -= new EventHandler(KeyDispatcher);
							_Editor.Modified += new EventHandler(KeyDispatcher);
							LoggingService.Info("AC trigger added");
						} else {
							LoggingService.Warn("AC trigger not added");
						}
					} else {
						LoggingService.Warn("null module");
					}
				} else if (e.NotifyCode == OTAFileNotification.ofnFileClosing) {
					IOTAModule _Module = OtaUtils.GetModuleServices().FindModule(e.FileName);

					if (_Module != null) {
						// Remove the Auto Completion Handler
						IOTAEditor _Editor = OtaUtils.GetEditorWithSourceEditor(_Module);

						if (_Editor != null) {
							_Editor.Modified -= new EventHandler(KeyDispatcher);
							LoggingService.Info("AC trigger removed");
						}
						else {
							LoggingService.Warn("null editor");
						}
					}
				}
			} catch (Exception ex) {
				Lextm.Windows.Forms.MessageBoxFactory.Fatal(ex);
				throw;
			}
		}
		
	}
}
