// this is the line counter feature.
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
using Lextm.OpenTools;

namespace Lextm.BagPlug.Feature
{
	/// <summary>
	/// Description of LineCounter.
	/// </summary>
	public class LineCounterFeature : CustomFeature
	{
		private Gui.FormLineCounter form;
		
		private void DoViewMethods(object aSender, EventArgs AEventArgs) {
            if ((form == null) || form.IsDisposed) {
            	form = new Gui.FormLineCounter();
            }
            form.Show();
        }

        protected override void IdeRegisterMenus() {
            // View Methods
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
												 ShareUtils.MenuRootDefault,
                                                 "LineCounterMenu",
												 0,  
												 "Line Counter",
                                                 new EventHandler(DoViewMethods)));


        }		
	}
}
