// this is the plus manager class.
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

using System;
using Lextm.OpenTools;

namespace Lextm.Utilities.Feature
{

    /// <summary>
    /// Help wizards.
    /// </summary>
    public class PlusManagerFeature: CustomFeature
    {
        /// <summary>
        /// Registers menus.
        /// </summary>
        protected override void IdeRegisterMenus()
        {
            base.IdeRegisterMenus();

            RegisterMenu(
                CreateActionMenu(
                    MenuItemLocation.Child,
                    MenuOptions,
                    MenuPlusManager,
                    0,
                    MenuTextPlusManager,
                    new EventHandler(DoManage)
                )
            );
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PlusManagerFeature( )
        { }

        private const string MenuOptions = "CBCExpertOptionsMenu";
        private const string MenuPlusManager = "CBCPlusManagerMenu";
        private const string MenuTextPlusManager = "Plus Manager";

        private static void DoManage(object sender, EventArgs e)
		{
            string fileName = System.IO.Path.Combine(Lextm.OpenTools.IO.Path.ProgramFilesFolder, "plusmanager.exe");
            Lextm.Diagnostics.ShellHelper.Execute(fileName);  
        }
    }
}


