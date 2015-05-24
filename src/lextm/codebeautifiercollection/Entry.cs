// this is the Main class. Ported from SBT.
//  Implementation changes.
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
using System.Globalization;
using System.IO;
using BeWise.Common.Utils;
using Lextm.Diagnostics;
using Lextm.OpenTools;
using UnhandledExceptionManager;
using Zayko.Dialogs.UnhandledExceptionDlg;

namespace Lextm.CodeBeautifierCollection {
    /// <summary>
    ///The entry class.
    /// </summary>
    public sealed class Entry { 
    	private static readonly string logFile =
    		Lextm.OpenTools.IO.Path.GetDataFile("cbc2.exe.config");
		/// <summary>
        ///Entry point of this assembly, called by IDE.
        /// </summary>
        public static void IDERegister() {
			Stopwatch watch = new Stopwatch();
			try { 
				watch.Start();
                RegisterSplashScreen();
                
				LoggingService.Start(logFile);
				LoggingService.EnterMethod();

                TExceptionManager.DefaultInfoManagerType = typeof(UnhandledExDlgForm);
				TExceptionManager.Initialize();
				TExceptionManager.UserContinue = true;
                System.Diagnostics.Debug.Assert(TExceptionManager.DefaultInfoManagerType == typeof(UnhandledExDlgForm));

                if (!ShareUtils.InstalledForCurrentUser)
                {
                    string fileName = Path.Combine(OpenTools.IO.Path.ProgramFilesFolder, "installforcurrentuser.exe");
                    ShellHelper.Execute(fileName, null, null, true);
                } 

                Collections.FeatureBuilder.Register();                
            }
			catch (Exception ex) {
				TExceptionManager.UnhandledExceptionHandler(null,new UnhandledExceptionEventArgs(ex,true));			
				throw new CoreException("CBC is not loaded. Press Continue/OK to continue launching IDE.",
				                              ex);
			}
			finally {
				int result = watch.Value;
				watch.Stop();
				LoggingService.LeaveMethod();
				LoggingService.Info(String.Format(CultureInfo.InvariantCulture, "Loading consumes {0}-ms.", result));
				if ((bool)PropertyRegistry.Get("EntryShowStartUpTime", false))
				{
					MessageService.Show(String.Format(CultureInfo.InvariantCulture, "CBC2 loading consumes {0}-ms.", result));
                }
				RegisterAboutBox();
            }
        }

        private Entry() {}
        
		private static void RegisterAboutBox() {
			OtaUtils.RegisterAboutBox(System.Reflection.Assembly.GetExecutingAssembly(), 
        	                          Lextm.OpenTools.IO.Path.AboutBoxFile);
        }

        private static void RegisterSplashScreen() {
			OtaUtils.RegisterSplashScreen(System.Reflection.Assembly.GetExecutingAssembly(), 
        	                              Lextm.OpenTools.IO.Path.SplashFile);
        }
	}
}
