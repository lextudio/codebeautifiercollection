// this is the valid file class.
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
namespace Lextm.CodeBeautifiers.IO {

    using System.Diagnostics;
    using System.Globalization;
    using Lextm.Diagnostics;
    using Lextm.OpenTools;

	///<summary>Valid file type.</summary>
	public abstract class ValidFile : CustomFile, IFormattableFile {

		private BeWise.SharpBuilderTools.Tools.BaseTool tool;
		private object[] parameters;

		///<summary>
		///Constructor.
		///</summary>
		protected ValidFile( ) {
		}

		///<summary>
		///Formatter tool.
		///</summary>
		[System.CLSCompliantAttribute(false)]
		protected BeWise.SharpBuilderTools.Tools.BaseTool Tool {
            get {
				return tool;
			}
			set {
				tool = value;
            }
        }

		///<summary>
		///Tool parameters.
		///</summary>
		protected object[] Parameters {
            get {
				return parameters;
			}
			set {
				parameters = value;
            }
        }

        private static void DoRunCompleted( object sender, System.EventArgs e ) {
			MessageService.Show("Done!\n");
        }
		private const string MaskTip =
			"Filename: {0} is formatted by {1}.";

		private void AddTips( ) {
			MessageService.Show(System.String.Format(CultureInfo.InvariantCulture,
				MaskTip,
				FileName,
				tool.GetConsole())
				);
		}

		private void RunTool( ) {
			if (tool.OnRunCompleted == null) {
				tool.OnRunCompleted += new System.EventHandler(DoRunCompleted);
			}
			tool.RunConsole(parameters, null);
		}

		///<summary>
		///Beautifies.
		///</summary>
		public override void Beautify( ) {
			LoggingService.EnterMethod();
			Trace.Assert(tool != null);

			AddTips();
			RunTool();
			LoggingService.LeaveMethod();
		}
	}
}
