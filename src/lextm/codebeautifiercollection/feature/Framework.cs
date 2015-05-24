// this is the ota framework class.
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
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Helpers;
using Borland.Studio.ToolsAPI;
using Lextm.Diagnostics;
using Lextm.OpenTools;
using Lextm.OpenTools.Elements;

namespace Lextm.CodeBeautifierCollection.Feature
{

	///<summary>
	///Expert skeleton.
	///</summary>
	sealed class Framework : CustomFeature, ILoadableFeature
	{
		/// <summary>
		///Registers actions.
		/// </summary>
		protected override void IdeRegisterMenus()
		{
			base.IdeRegisterMenus();

			ValidateRootMenu();
			// Registers a ToolsMenu but should be no action,
			// because it is already a IDE menu.
			RegisterMenu(CreateEmptyMenu(
				MenuItemLocation.Child,
				Root,
				MenuTools,
				String.Empty)
			            );

			string parent = null;
			MenuItemLocation _Location;

			switch ((ParentType)PropertyRegistry.Get("ParentType", ParentType.Default))
			{
				case ParentType.Tools:
					parent = MenuTools;
					_Location = MenuItemLocation.Child;
					break;
				case ParentType.CNPack:
					parent = MenuCNPack;
					_Location = MenuItemLocation.Child;
					break;
				default:
					parent = MenuTools;
					_Location = MenuItemLocation.Before;
					break;
			}

			if ((ParentType)PropertyRegistry.Get("ParentType", ParentType.Default) != ParentType.Default)
			{
				// Separator
				RegisterMenu(CreateSeparatorMenu(
					_Location,
					parent)
				            );
			}
			
			AddCommonMenus(_Location, parent);
		}

		/// <summary>
		/// Registers notifications.
		/// </summary>
		protected override void IdeRegisterNotifications()
		{
			base.IdeRegisterNotifications();

			IOTAService _Service = OtaUtils.GetService();
			if (_Service != null)
			{
				_Service.FileNotification += new FileNotificationHandler(ShowWarningsFileNotificationHandler);
			}

		}

		///<summary>
		///Registers tabs.
		///</summary>
		protected override void IdeRegisterTabs()
		{
			base.IdeRegisterTabs();

			RegisterTab(CreateTabNode(TabFramework, typeof(Gui.FrameworkPage)));
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		internal Framework( )
		{ }

		private const string MenuAbout = "CBCAboutMenu";
		private const string MenuCNPack = "CnPackMenu";
		private const string MenuLeXtudio = "CBCLeXtudioMenu";
		private const string MenuOptions = "CBCExpertOptionsMenu";
		private const string MenuPreferences = "CBCPreferencesMenu";
		private const string MenuPreferencesReload = "CBCPreferencesReloadMenu";
		private const string MenuShortcuts = "CBCShortcutsMenu";
		private const string MenuShortcutsReload = "CBCShortcutsReloadMenu";
		private const string MenuTextAbout = "About...";
		private const string MenuTextLeXtudio = "LeXt&udio";
		private const string MenuTextOptions = "Options";
		private const string MenuTextPreferences = "Preferences...";
		private const string MenuTextPreferencesReload = "Reload Preferences";
		private const string MenuTextShortcuts = "Shortcuts...";
		private const string MenuTextShortcutsReload = "Refresh Shortcuts";
		private const string MenuTools = "ToolsMenu";
		private const string Root = "Root"; // the same with the one in MenuTree.
		private const string TabFramework = "Framework";
		private static Gui.FormAbout about;
		private static Gui.FormShortcuts form;

		private void AddCommonMenus( MenuItemLocation _Location, string parent ) {
			RegisterMenu(CreateEmptyMenu(
				_Location,
				parent,
				MenuLeXtudio,
				MenuTextLeXtudio)
			            );
			RegisterMenu(CreateEmptyMenu(
				MenuItemLocation.Child,
				MenuLeXtudio,
				MenuOptions,
				MenuTextOptions)
			            );
			RegisterMenu(CreateSeparatorMenu(MenuItemLocation.Before,
			                                 MenuOptions));
			RegisterMenu(CreateActionMenu(MenuItemLocation.Child,
			                              MenuOptions,
			                              MenuPreferencesReload,
			                              0,
			                              MenuTextPreferencesReload,
			                              new EventHandler(DoReloadPreferences)));
			RegisterMenu(CreateActionMenu(MenuItemLocation.Child,
			                              MenuOptions,
			                              MenuShortcutsReload,
			                              0,
			                              MenuTextShortcutsReload,
			                              new EventHandler(DoReloadShortcuts)));
			RegisterMenu(CreateSeparatorMenu(MenuItemLocation.Child,
			                                 MenuOptions));
			RegisterMenu(CreateActionMenu(MenuItemLocation.Child,
			                              MenuOptions,
			                              MenuShortcuts,
			                              0,
			                              MenuTextShortcuts,
			                              new EventHandler(DoShortcuts)));
			RegisterMenu(CreateActionMenu(MenuItemLocation.Child,
			                              MenuOptions,
			                              MenuPreferences,
			                              0,
			                              MenuTextPreferences,
			                              new EventHandler(DoPreferences)));
			RegisterMenu(
				CreateActionMenu(
					MenuItemLocation.After,
					MenuOptions,
					MenuAbout,
					0,
					MenuTextAbout,
					new EventHandler(DoAbout)
				)
			);
			RegisterMenu(
				CreateActionMenu(
					MenuItemLocation.After,
					MenuOptions,
					"CBCSendComments",
					0,
					"Send Your Comments",
					new EventHandler(DoSendComments)
				)
			);
			RegisterMenu(
				CreateActionMenu(
					MenuItemLocation.After,
					MenuOptions,
					"CBCManual",
					0,
					"User Manual",
					new EventHandler(DoManual)
				)
			);
		}
		
		private static void DoManual(object sender, EventArgs e)
		{
			ShellHelper.Execute(OpenTools.IO.Path.GetDocFile("Manual.pdf"));
		}
		
		private static void DoSendComments(object sender, EventArgs e)
		{
			ShellHelper.Execute("http://code.google.com/p/lextudio/issues/entry");
		}
		
		private static void DoAbout(object sender, EventArgs e)
		{
			if ((about == null) || (about.IsDisposed))
			{
				about = new Gui.FormAbout();
			}
			about.ShowDialog();
		}

		private static void DoPreferences(object sender, EventArgs e)
		{

			LoggingService.EnterMethod();

			Gui.FormPreferences.getInstance().ShowDialog();

			LoggingService.LeaveMethod();

		}

		private static void DoReloadPreferences(object sender, EventArgs e)
		{
			LoggingService.EnterMethod();

			Collections.FeatureBuilder.ReloadPreferences();

			LoggingService.LeaveMethod();

		}

		private static void DoReloadShortcuts(object sender, EventArgs e)
		{

			LoggingService.EnterMethod();

			ShortcutService.RefreshShortcuts();

			LoggingService.LeaveMethod();

		}

		private static void DoShortcuts(object sender, EventArgs e)
		{

			LoggingService.EnterMethod();

			if ((form == null) || (form.IsDisposed))
			{
				form = new Gui.FormShortcuts();
			}
			form.Show();

			LoggingService.LeaveMethod();

		}

		private static void ShowWarningsFileNotificationHandler(object aSender, FileNotificationEventArgs aArgs)
		{
			// Notification for the Project Group
			if (aArgs.NotifyCode == OTAFileNotification.ofnFileOpened)
			{
				MessageService.ShowAll();
			}
		}

		///<summary>
		/// Check for CNPackMenu
		/// </summary>
		private void ValidateRootMenu( )
		{

			if (((ParentType)PropertyRegistry.Get("ParentType", ParentType.Default) == ParentType.CNPack) &&
			    !(ExpertChecker.CNPackInstalled()))
			{
				PropertyRegistry.Set("ParentType", ParentType.Default);
				this.SavePreferences();
			}
		}
	}
}
