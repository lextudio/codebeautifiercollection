// this is the AStyle class. Ported from SBT.
//		Slight changes are brought in.
// Copyright (C) 2005-2006  Lex Y. Li
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
using BeWise.SharpBuilderTools.Helpers;
using BeWise.SharpBuilderTools.Tools;
using Lextm.Diagnostics;

namespace Lextm.CodeBeautifiers.Tool {


	/// <summary>
	///AStyle tool.
	/// </summary>
	internal sealed class AStyle : BaseTool {

		private const string ExeNameAStyle        		    = "astyle.exe";
	   	private const string ToolNameAStyle 					= "Artistic Style";

		/// <summary>
		///Verifies tool file for AStyle.
		/// </summary>
		/// <param name="fileName">File name</param>
        /// <returns>
		///false always for AStyle.
		///</returns>
		public override bool IsToolFile(string fileName) {
			//override just to pass the test
			return false;
		}
        ///<summary>
		///Runs the console.
		///</summary>
		///<param name="parameters">Console parameters</param>
		///<param name="workingDir">Working directory of the console</param>
		public override void RunConsole(object[] parameters, string workingDir) {
			base.RunConsole(parameters, workingDir);
			LoggingService.EnterMethod();
			if (!ValidationHelpers.ValidateToolInstalled(this)) {
				LoggingService.LeaveMethod();
			
				return;
			}

			RunProcess runProcess = new RunProcess();
			string[] paramStrings = new String[2];

			paramStrings[0] = GetConsole();

			if ((parameters != null) && (parameters.Length != 0)) {
				paramStrings[1] = (string) parameters[0];
			}
			LoggingService.Info(String.Format(CultureInfo.InvariantCulture, "The param is {0}", paramStrings[1]));
			runProcess.OnConsoleOutput += OnConsoleOutput;
			runProcess.OnRunCompleted += OnRunCompleted;
			string result = runProcess.Run(paramStrings, workingDir);
			LoggingService.Info(String.Format(CultureInfo.InvariantCulture, "The result is {0}", result));
			LoggingService.LeaveMethod();
			
		}

		///<summary>
		///Gets tool name
		///</summary>
		///<returns>
		///Name of the tool.
		///</returns>
		public override string GetName () {
			return ToolNameAStyle;
		}

		private string path;
		///<summary>
		///Gets the console path.
		///</summary>
		///<returns>
        ///Path of the console.
		///</returns>
		protected override string GetToolPath () {
			// TODO: return (string)Lextm.OpenTools.PropertyRegistry.Get("AStylePath");
			return path;
		}
		///<summary>
		///Gets console name.
		///</summary>
		///<returns>
		///Name of the console.
		///</returns>
		protected override string GetConsoleArgument () {
			return ExeNameAStyle;
		}
   		///<summary>
		///Verifies that the tool is installed.
		///</summary>
		///<returns>
        ///true if the tool is installed, false if not.
		///</returns>
		protected override bool GetInstalled () {
			return File.Exists(GetConsole());
		}

		private AStyle( ) {
		}

		private static AStyle instance;

		///<summary>
		///Gets singleton instance.
		///</summary>
		internal static AStyle getInstance( string path ) {
			lock(typeof(Lextm.CodeBeautifiers.Tool.AStyle)){
			    if (instance == null)
			    {
					instance = new Lextm.CodeBeautifiers.Tool.AStyle();
				}
				instance.path = path;
            }
		    return instance;
		}
	}
}
