// this is addmany feature class.
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
using ArtCSB;
using BeWise.SharpBuilderTools.Helpers;

namespace Lextm.ArtCSB.Feature {
    /// <summary>
	/// AddMany feature.
    /// </summary>
	public class DesignerNavigatorFeature : Lextm.OpenTools.CustomFeature {

        /// <summary>
        /// Constructor.
        /// </summary>
		public DesignerNavigatorFeature() {
			//Common.InitResourceManager();
		}

		private const string MenuItemName = "ArtCSBMenuItem_DesignerNavigator";

        /// <summary>
		/// Registers menus.
        /// </summary>
		protected override void IdeRegisterMenus() {
			base.IdeRegisterMenus();

            RegisterMenu(
                CreateActionMenu(Lextm.OpenTools.MenuItemLocation.Child,
                                 Lextm.OpenTools.ShareUtils.MenuRootDefault,
								 MenuItemName,
                                 0,
								 "ArtCSB Designer Navigator",
								 new EventHandler(MenuItemExecuted)
								));
		}

#region Menus
        	private static WinForm_DesignerNavigator wDesignerNavigator;
    
    		private static void MenuItemExecuted(object sender, EventArgs e)
			{
				if (!ValidationHelpers.ValidateCurrentModuleNotNull()) {
					return;
                }          				
    			if ((wDesignerNavigator == null) || (wDesignerNavigator.IsDisposed))
    			{
    				wDesignerNavigator = new WinForm_DesignerNavigator();
    			}
    			wDesignerNavigator.Show();
    		}
    
#endregion
	}
    
}

