// this is the tipof the day class.
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
using Lextm.OpenTools;

namespace Lextm.Utilities.Feature
{

    /// <summary>
    /// Help wizards.
    /// </summary>
    public class TipOfTheDayFeature: CustomFeature
    {
        /// <summary>
        /// Registers wizards.
        /// </summary>
        protected override void IdeRegisterMenus()
        {
            base.IdeRegisterMenus();

            RegisterMenu(
                CreateActionMenu(
                    MenuItemLocation.Child,
                    MenuOptions,
                    MenuTips,
                    0,
                    MenuTextTips,
                    new EventHandler(DoTips)
                )
            );
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TipOfTheDayFeature( )
        { }

        private const string MenuOptions = "CBCExpertOptionsMenu";
        private const string MenuTextTips = "Tip of the Day...";
        private const string MenuTips = "CBCTipsMenu";
        private const string TipsExeName = "TipOfTheDay.exe";

        private static Gui.TipOfTheDayForm form;
        
        private static void DoTips(object sender, EventArgs e)
        {
        	if (form == null || form.IsDisposed)
        	{
        		form = new Gui.TipOfTheDayForm();
        	}
        	form.Show();
        }
    }
}


