// this is the indate class.
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
using Lextm.Diagnostics;
using System.IO;
using Lextm.OpenTools;
using System.Globalization;

namespace Lextm.Utilities.Feature
{
    /// <summary>
    /// InDateFeature feature, keep up-to-date.
    /// </summary>
    public class InDateFeature: CustomFeature
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
					MenuInDate,
					0,
					MenuTextInDate,
					new EventHandler(DoUpdate)
				)
			);
		}

		/// <summary>
		/// Registers tabs.
		/// </summary>
		protected override void IdeRegisterTabs()
		{
			base.IdeRegisterTabs();

			RegisterTab(CreateTabNode(TabInDate, typeof(Gui.InDatePage)));
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public InDateFeature()
		{
			if ((bool)PropertyRegistry.Get("InDateCheckAtStartup", true)
			    && !CheckedToday())
			{
                DoUpdate(null, null);
			}
		}
		
		private static bool CheckedToday() {
            return DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) == (string)PropertyRegistry.Get("InDateLastCheckDate");
		}
			
		private const string MenuInDate = "CBCInDateMenu";
		private const string MenuOptions = "CBCExpertOptionsMenu";
		private const string MenuTextInDate = "Get Update...";
		private const string TabInDate = "Auto Update";		

		private static void DoUpdate(object sender, EventArgs e)
		{
			string fileName = Path.Combine(Lextm.OpenTools.IO.Path.ProgramFilesFolder, "indate.exe");
			ShellHelper.Execute(fileName);
			PropertyRegistry.Set("InDateLastCheckDate",
                                 DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
		}
	}
}
