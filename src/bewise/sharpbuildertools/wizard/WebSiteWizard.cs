// lextm: this is the website wizard class. Ported from SBT.
//		Constructor changed.
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
using Borland.Studio.ToolsAPI;

namespace BeWise.SharpBuilderTools.Wizard {
	/// <summary>
	/// Web site wizard.
	/// </summary>
	public class WebsiteWizard: IOTAMenuWizard {
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">ID string</param>
		/// <param name="name">Name</param>
		/// <param name="url">URL</param>
		/// <param name="title">URL Title</param>
        public WebsiteWizard (string id, 
		                      string name, 
							  System.Uri url,
							  string title){
			this.id = id;
            this.name = name;
            this.url = url;
			this.title = title;
        }

        private string id;
        private System.Uri url;
        private string title;
		/// <summary>
		/// ID string.
		/// </summary>
        public string IDString {
            get {
                return id;
            }
        }
		/// <summary>
		/// Executes.
		/// </summary>
        public void Execute() {
            Process.Start(url.ToString());
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
		/// <summary>
		/// Menu text.
		/// </summary>
        public string MenuText {
            get {
                return title;
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
