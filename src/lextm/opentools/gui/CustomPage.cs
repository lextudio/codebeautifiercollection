// this is custom page, base of all tab pages.
// Copyright (C) 2006  Lex Y. Li
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
using System;

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Lextm.OpenTools.Gui
{
	/// <summary>
	/// Tab page base class.
	/// </summary>
	/// <remarks>Since it is derived from <see cref="UserControl"/>,
	/// components can be edited in Form Designer.</remarks>
	public class CustomPage : System.Windows.Forms.UserControl
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		/// <summary>
		/// Construtor.
		/// </summary>
		public CustomPage()
		{
			// This call is required by the Windows.Forms Designer.
			InitializeComponent();    
		}
		/// <summary>
		/// Loads preferences to UI.
		/// </summary>
		public virtual void PreferencesToUI() {

		}
		/// <summary>
		/// Saves preferences from UI.
		/// </summary>
		public virtual void UIToPreferences() {
        
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
					components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// CustomPage
			// 
			this.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.Name = "CustomPage";
			this.Size = new System.Drawing.Size(576, 418);
		}
		#endregion
	}
}
