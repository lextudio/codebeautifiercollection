// this is the expert manager class. Ported from SBT.
//   Adds Win32 expert supports.
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
using System.IO;
using Lextm.Diagnostics;
using Lextm.OpenTools;

namespace Lextm.Utilities.Feature
{
    /// <summary>
    /// Help wizards.
    /// </summary>
    public class ExpertManagerFeature: CustomFeature
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
                    MenuExpertManager,
                    0,
                    MenuTextExpertManager,
                    new EventHandler(DoManage)
                )
            );
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExpertManagerFeature( )
        { }

        private const string MenuExpertManager = "CBCExpertManagerMenu";
        private const string MenuOptions = "CBCExpertOptionsMenu";
        private const string MenuTextExpertManager = "Expert Manager";

        private static void DoManage(object sender, EventArgs e)
        {
			string fileName = Path.Combine(Lextm.OpenTools.IO.Path.ProgramFilesFolder, "expertmanager.exe");
			ShellHelper.Execute(fileName);
        }
    }
}

