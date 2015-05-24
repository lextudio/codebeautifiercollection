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
using System.Windows.Forms;
using Lextm.OpenTools;

namespace Lextm.AddMany.Feature {
    /// <summary>
    /// AddMany feature.
    /// </summary>
    public class AddManyFeature : CustomFeature {

        private const string AddManyItem = "ProjectAddManyItem";
        private const string AddProjectItem = "ProjectAddProjectItem";
        private const string MenuLeXtudio = "CBCLeXtudioMenu";

        private const int AddManyItemShortcut = ShareUtils.Ctrl +
                                                ShareUtils.Alt + (int)Keys.A;//49217; // Ctrl + Alt + A

		private const string AddProjectItemText = "Add &Project Files to Project...";
        private const string AddManyItemText = "Add &Many to Project...";

        private TIDEPlugin idePlugin;

        /// <summary>
        /// Constructor.
        /// </summary>
        public AddManyFeature() {
            idePlugin = new TIDEPlugin();
        }

        /// <summary>
        /// Registers menus.
        /// </summary>
        protected override void IdeRegisterMenus() {
            base.IdeRegisterMenus();

			// Add Project
            RegisterMenu(
                CreateActionMenu(MenuItemLocation.Child,
                                 MenuLeXtudio,
                                 AddProjectItem,
                                 0,
                                 AddProjectItemText,
                                 new EventHandler(OnMenuExecutedAddProject)
                                ));
            // Add Many
            RegisterMenu(
                CreateActionMenu(MenuItemLocation.Child,
                                 MenuLeXtudio,
                                 AddManyItem,
                                 AddManyItemShortcut,
                                 AddManyItemText,
                                 new EventHandler(OnMenuExecutedAddMany)
                                ));

        }


        private void OnMenuExecutedAddMany(object sender, EventArgs e) {
            idePlugin.Add(false);
        }

        private void OnMenuExecutedAddProject(object sender, EventArgs e) {
            idePlugin.Add(true);
        }
	}
}


