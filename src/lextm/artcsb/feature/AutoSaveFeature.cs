// lextm: this is auto save feature class.
// Copyright (C) 2006  Lex Mark
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
using System.Windows.Forms;
using ArtCSB;
using Lextm.LeXDK;

namespace Lextm.ArtCSB.Feature {
    /// <summary>
    /// AddMany feature.
    /// </summary>
	public class AutoSaveFeature : CustomFeature {

        /// <summary>
        /// Constructor.
        /// </summary>
		public AutoSaveFeature() {
			Common.InitResourceManager();
		}

		private const string MenuItemName = "ArtCSBMenuItem_AutoSave";
        /// <summary>
		/// Registers menus.
        /// </summary>
		protected override void IDERegisterMenus() {
			base.IDERegisterMenus();

			RegisterMenu(
                CreateActionMenu(MenuItemLocation.Child,
                                 Shared.MenuLeXtudio,
								 MenuItemName,
                                 0,
								 "ArtCSB Auto-Save Options...",
								 new EventHandler(MenuItemExecuted)
								));
		}

#region Menus
		private static WinForm_AutoSave wAutoSave=null;

		private static void MenuItemExecuted(object sender, EventArgs e){
			if ((wAutoSave == null) || (wAutoSave.IsDisposed))
			{
				wAutoSave = new WinForm_AutoSave();
			}
			if (wAutoSave.ShowDialog() == DialogResult.OK){
				Utils_AutoSave.SetTimer();
			}

		}    
#endregion
	}
    
}

