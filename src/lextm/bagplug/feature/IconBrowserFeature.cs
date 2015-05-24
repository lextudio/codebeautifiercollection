// this is the icon browser feature.
// Copyright (C) 2007  Lex Y. Li
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

using Lextm.Diagnostics;
using Lextm.OpenTools;

namespace Lextm.BagPlug.Feature
{
	/// <summary>
	/// Icon browser feature.
	/// </summary>
	public class IconBrowserFeature : CustomFeature
	{
		private void DoViewMethods(object aSender, EventArgs AEventArgs) {
			string exe = System.IO.Path.Combine(Lextm.OpenTools.IO.Path.BundledFolder,
			                                    "kerr.iconbrowser.exe");
			ShellHelper.Execute(exe);
        }

        protected override void IdeRegisterMenus() {
            // View Methods
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
												 ShareUtils.MenuRootDefault,
                                                 "IconBrowserMenu",
												 0,  
												 "Icon Browser",
                                                 new EventHandler(DoViewMethods)));


        }		
	}
}
