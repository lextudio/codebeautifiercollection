// this is doc preview feature class.
// Copyright (C) 2006-2007  Lex Y. Li
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
using Lextm.OpenTools;

namespace Lextm.CsbGoodies.Feature
{
	/// <summary>
	/// Wrapper for C#Builder Goodies.
	/// </summary>
	public class DocPreviewFeature: CustomFeature
	{

		/// <summary>
		/// Constructor.
		/// </summary>
    	public DocPreviewFeature() { }
    	
		private const int DefaultViewDocShortcut = ShareUtils.Alt + (int)Keys.Q;//32849; // Alt + Q
		
		private const string MenuViewDoc = "CSBuilderGoodiesViewDocMenu";
		private const string MenuTextViewDoc = "View Doc";	
		/// <summary>
		/// Registers menus.
		/// </summary>
		protected override void IdeRegisterMenus()
		{
			base.IdeRegisterMenus();

            // View Doc
            RegisterMenu(
                CreateActionMenu(
					MenuItemLocation.Child,
                    ShareUtils.MenuRootDefault,
                    MenuViewDoc,
					DefaultViewDocShortcut,
                    MenuTextViewDoc,
              		new EventHandler(DoViewDoc)
              	)
            );
		}
		
		private static void DoViewDoc(object sender, EventArgs e) {
			CsbGoodies.QuickdocViewer.Activate();
		}

	}
}
