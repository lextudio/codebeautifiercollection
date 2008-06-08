// this is the XMLLex class. Ported from SBT.
//		I reimplements SBT's XML formatting function here.
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
//using MarcClifton.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using BeWise.Common.Utils;
using Lextm.Diagnostics;
using Lextm.OpenTools;

namespace Lextm.CodeBeautifiers.Tool {
	///<summary>
	///XMLLex tool.
	///</summary>
	/// <remarks>
	/// This tool is different from others. 
	/// XMLLex source are included directly into CBC. 
	/// It is originally designed by David Hervieux, and modified by lextm.
	/// Later, maybe a better XML beautifier will take its place.
	///	</remarks>
    internal sealed class XmlLex : BeWise.SharpBuilderTools.Tools.BaseTool {
		/// <summary>
		///Gets tool path.
		/// </summary>
		/// <returns>
		///Path of the tool.
		/// </returns>
		protected override string GetToolPath () {
            return null;
        }
		/// <summary>
		///Verifies the tool is installed.
		/// </summary>
		/// <returns>
		///true if the tool is intalled, false if not.
		/// </returns>
        protected override bool GetInstalled () {
            return true;
        }
		/// <summary>
		///Gets tool name.
		/// </summary>
		/// <returns>
		///Name of the tool.
		/// </returns>
        public override string GetName () {
            return ToolNameXmlLex;
        }
        
		private const string ToolNameXmlLex = "XMLLex: Lextm's XML beautifier";
     	/// <summary>
		///Runs the console.
		/// </summary>
		/// <param name="parameters">Console parameters</param>
		/// <param name="aWorkingDir">Working directory of the console</param>
		/// <remarks>This is an ugly hack to the whole tools architecture David creates because XMLLex is not an external executable.</remarks>
		public override void RunConsole(object[] parameters, string aWorkingDir) {
			base.RunConsole(parameters, aWorkingDir);
            if ((parameters != null) && (parameters.Length != 0)) {
                Beautify((string) parameters[0]);
            }
			if (OnRunCompleted != null) {
                OnRunCompleted(null, null);
            }
        }
		/// <summary>
		///Verifies tool file for AStyle.
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <returns>
		///false always for JCF.
		/// </returns>
        public override bool IsToolFile(string fileName) {
            return BeWise.Common.Utils.OtaUtils.IsXmlFile(fileName);
        }
        
        ///<summary>
		///CBC backup file extension.
        /// </summary>
        private const string ExtensionBackupFile = "bak";
        
        private static bool Backup( string fileName, string backupName ) {

			try {
				File.Copy(fileName, backupName, true);
				return true;
			} 
			catch (IOException ex) {
				Lextm.Windows.Forms.MessageBoxFactory.Fatal(null, ex);
				return false;
			}     	
        }
        
        private static bool Validate( string fileName ) {
        	XmlTextReader reader = null;
			bool result = false;
	     	try {
	       
	        	// Load the reader with the data file and ignore all white space nodes.         
	        	reader = new XmlTextReader(fileName);
	        	reader.WhitespaceHandling = WhitespaceHandling.None;
	
				// Parse the file until exception occurs.
				reader.MoveToContent();
				while (reader.Read()) { }

	        	result = true;
        	} catch (XmlException) { 
				MessageService.Show(fileName + " is not a valid file.");
				result = false;
			} finally {
				if (reader != null) {
	          		reader.Close();
				}				
			}
			
			return result;
        }
        
		private static void Beautify( string fileName ) {
        
			LoggingService.EnterMethod();
            string backupName = Path.ChangeExtension(fileName, ExtensionBackupFile);

			if (Validate(fileName)) {
				if (Backup(fileName, backupName)) {
					// do formatting.
					XmlDocument _XmlDocument = new XmlDocument();

					try
					{
						_XmlDocument.Load(backupName);
					} finally {
						//TODO : uses other coding format.
						XmlTextWriter _XmlTextWriter = new XmlTextWriter(fileName, new UTF8Encoding());
						_XmlTextWriter.Formatting = Formatting.Indented;
						_XmlTextWriter.Indentation = OtaUtils.GetXmlIndentationFromOptions();
						_XmlTextWriter.IndentChar = ' ';

						_XmlDocument.Save(_XmlTextWriter);
						_XmlTextWriter.Close();
					}
				} else {
					LoggingService.Warn("Failed: no backup");
				}
			} else {
				LoggingService.Warn("not a valid file");
			}

			LoggingService.LeaveMethod();
			
		}

		private XmlLex( ) {}
		/// <summary>
		///Gets console name.
		/// </summary>
		/// <returns>
		///Name of the console.
		/// </returns>
		protected override string GetConsoleArgument () {
			return Lextm.Reflection.AssemblyHelper.GetName(System.Reflection.Assembly.GetExecutingAssembly());
		}
        
        private static XmlLex instance;

		/// <summary>
		///Gets singleton instance.
		/// </summary>
        internal static XmlLex getInstance( ) {
			lock (typeof(Lextm.CodeBeautifiers.Tool.XmlLex)) {
                if (instance == null) {
                    instance = new Lextm.CodeBeautifiers.Tool.XmlLex();
                }
            }
            return instance;
        }
    }
}
