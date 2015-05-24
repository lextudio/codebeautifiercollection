// this is the readme feature.
// It is original made by Liz, as "Liz's Readme viewer",
// which is a BDN Code Central entry.
// I port it to C# and make it part of the WiseEditor Plus.
// Copyright (C) 2006  Lex Y. Li
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
    /// Readme feature.
    /// </summary>
    public class ReadmeNotifierFeature : CustomFeature {
    	
        public ReadmeNotifierFeature() {
            IOTAService service1 = OtaUtils.GetService();
            service1.FileNotification += new FileNotificationHandler(this.ReadmeFileNotificationHandler);
        }
		
		private void ReadmeFileNotificationHandler(object aSender, FileNotificationEventArgs aArgs) {
            try
            {
				if ((aArgs.NotifyCode == OTAFileNotification.ofnFileOpened) && OtaUtils.IsProjectFile(aArgs.FileName)) {
					IOTAProject project1 = OtaUtils.GetCurrentProject();
					if (project1 != null) {
						for (int i = 0; i < project1.ModuleCount; i++) {
							IOTAModuleInfo info1 = project1.GetModuleInfo(i);

							string text2 = info1.FileName;
							if (String.IsNullOrEmpty(text2) || !OtaUtils.IsDocFile(text2)) {
								continue;
							}

							if (System.IO.Path.GetFileName(text2).IndexOf("README", StringComparison.OrdinalIgnoreCase) > -1) {
								System.Diagnostics.Process.Start(text2);
							} else {
								LoggingService.Warn("Readme: no readme file in this project.");
							}

						}
					} else {
						LoggingService.Warn("Readme: IOTAProject is null even though a project file is opened.");
					}
				}	
            }
			catch (Exception ex)
			{
				LoggingService.Error("Readme: bug is here." + ex.GetType() + ex.Message + ex.StackTrace);
            	throw;
			}
        }
    }
}
