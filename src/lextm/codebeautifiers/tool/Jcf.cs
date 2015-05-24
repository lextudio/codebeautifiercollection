// this is the JCF class.
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
	///JCF tool.
	/// </summary>
	internal sealed class Jcf : BaseTool {

		private const string ExeNameJcf 						= "Jcf.exe";

		private const string ToolNameJcf						= "JEDI Code Format";

		/// <summary>
		///Verifies tool file for AStyle.
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <returns>
		///false always for JCF.
		/// </returns>
		public override bool IsToolFile(string fileName) {
			//just to complete the class
			return false;
		}
     	/// <summary>
		///Runs the console.
		/// </summary>
		/// <param name="aParams">Console parameters</param>
		/// <param name="aWorkingDir">Working directory of the console</param>
		public override void RunConsole(object[] aParams, string aWorkingDir) {
			base.RunConsole(aParams, aWorkingDir);
			LoggingService.EnterMethod();
			if (!ValidationHelpers.ValidateToolInstalled(this)) {
				LoggingService.LeaveMethod();
				
				return;
			}

			RunProcess _RunProcess = new RunProcess();
			string[] _Params = new String[2];

			_Params[0] = GetConsole();

			if ((aParams != null) && (aParams.Length != 0)) {
				_Params[1] = (string) aParams[0];
			}
			LoggingService.Info(String.Format(CultureInfo.InvariantCulture, "The param is {0}", _Params[1]));

			_RunProcess.OnConsoleOutput += OnConsoleOutput;
			_RunProcess.OnRunCompleted += OnRunCompleted;
			string result = _RunProcess.Run(_Params, aWorkingDir);
			LoggingService.Info(String.Format(CultureInfo.InvariantCulture, "The result is {0}", result));
			LoggingService.LeaveMethod();
			
		}

		/// <summary>
		///Gets tool name.
		/// </summary>
		/// <returns>
		///Name of the tool.
		/// </returns>
		public override string GetName () {
			return ToolNameJcf;
		}

		private string path;
		/// <summary>
		///Gets tool path.
		/// </summary>
		/// <returns>
		///Path of the tool.
		/// </returns>
		protected override string GetToolPath () {
			// TODO: return (string)Lextm.OpenTools.PropertyRegistry.Get("JcfPath");
			return path;
		}

		/// <summary>
		///Gets console name.
		/// </summary>
		/// <returns>
		///Name of the console.
		/// </returns>
		protected override string GetConsoleArgument () {
			return ExeNameJcf;
		}

		/// <summary>
		///Verifies the tool is installed.
		/// </summary>
		/// <returns>
		///true if the tool is intalled, false if not.
		/// </returns>
		protected override bool GetInstalled () {
			return File.Exists(GetConsole());
		}

		private Jcf( ) {
		}

		private static Jcf instance;

		/// <summary>
		///Gets singleton instance.
		/// </summary>
		internal static Jcf getInstance( string path ) {
			lock(typeof(Lextm.CodeBeautifiers.Tool.Jcf)){
			    if (instance == null)
			    {
					instance = new Lextm.CodeBeautifiers.Tool.Jcf();
				}
				instance.path = path;
			}
		    return instance;
        }
	}
}
