// lextm: this is the help wizard class. Ported from SBT.
//		More flexibilities are added by providing a constructor.
//      Enhancement added to make sure the help file is there.
// Copyright (C) 2005-2006  Lex Mark
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
using System.Diagnostics;
using System.IO;
using Borland.Studio.ToolsAPI;
using BeWise.Common.Utils;
using Lextm.LeXDK;

namespace BeWise.SharpBuilderTools.Wizard {
	/// <summary>
	/// Help wizard.
	/// </summary>
    public class HelpWizard: IOTAMenuWizard {
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">ID string</param>
		/// <param name="helpFileName">Help file name</param>
		/// <param name="name">Name</param>
		/// <param name="menuText">Menu text</param>
		public HelpWizard(string id,
		                  string helpFileName,
		                  string name,
		                  string menuText) {
			this.id = id;
			this.helpFileName = helpFileName;
			this.name = name;
			this.menuText = menuText;
		}
		
		private string id;
		/// <summary>
		/// ID string.
		/// </summary>
        public string IDString {
            get {
                return id;
            }
        }
		
		private string helpFileName;
		/// <summary>
		/// Executes.
		/// </summary>
        public void Execute() {
			if (File.Exists(helpFileName)) {
            	Lextm.Diagnostics.ShellHelper.Execute(helpFileName);
			} else {
				MessageService.Show("The manual is not here.");
			}
        }
		
		private string name;
		/// <summary>
		/// Name.
		/// </summary>
        public string Name {
            get {
                return name;
            }
        }
		/// <summary>
		/// Destroys.
		/// </summary>
		public void Destroyed() {
            /* nothing */
        }
		
		private string menuText;
		/// <summary>
		/// Menu text.
		/// </summary>
        public string MenuText {
            get {
                return menuText;
            }
        }
		/// <summary>
		/// Checked.
		/// </summary>
		public bool Checked {
			get {
                return false;
            }
        }
		/// <summary>
		/// Enabled.
		/// </summary>
        public bool Enabled {
            get {
				return true;
            }
        }
    }
}
