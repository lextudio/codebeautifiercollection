// This is the custom feature class. Ported from SBT base ota class.
// Copyright (C) 2005-2006  Lex Y. Li
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;

using System.Diagnostics;
using System.Windows.Forms;
using Lextm.Diagnostics;
using System.Collections.Generic;

namespace Lextm.OpenTools
{


    ///<summary>
    ///Base of all feature classes.
    ///</summary>
    public class CustomFeature : ILoadableFeature
    {
        ///<summary>
        ///Loads preferences.
        ///</summary>
        public virtual void LoadPreferences( )
        { }

        ///<summary>
        ///Refreshes wizard configuration.
        ///</summary>
        public virtual void RefreshPreferences( )
        { }

        /// <summary>
        /// Registers.
        /// </summary>
        public void Register( )
        {
            LoadPreferences();
            SetDefaultPreferences();
            IdeRegisterMenus();
            IdeRegisterNotifications();
            IdeRegisterWizards();
            IdeRegisterTabs();
			RefreshPreferences();
        }

        ///<summary>
		///Saves preferences.
        ///</summary>
        public virtual void SavePreferences( )
        {}

        ///<summary>
        ///Sets default configuration.
        ///</summary>
        public virtual void SetDefaultPreferences( )
        {  }

        ///<summary>
        ///List of actions.
        ///</summary>
        public IList<Elements.ICustomMenuNode> Menus
        {
            get
            {
                return menus;
            }
        }

        ///<summary>
        ///FormPreferences tabs.
        ///</summary>
        public IList<Elements.TabNode> TabNodes {
            get
            {
                return tabNodes;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected CustomFeature( )
        { }

        ///<summary>
        ///Creates an action menu.
        ///</summary>
        ///<param name="parentLocation">Parent location</param>
        ///<param name="parentName">Parent name</param>
        ///<param name="name">Name</param>
        ///<param name="shortcut">Shortcut</param>
        ///<param name="text">Text</param>
        ///<param name="executeEvent">Event handler</param>
        ///<returns>
        ///Created ActionMenu object.
        ///</returns>
        protected static Elements.ICustomMenuNode CreateActionMenu(MenuItemLocation parentLocation,
                string parentName,
                string name,
                int shortcut,
                string text,
                EventHandler executeEvent)
        {
            Trace.Assert(parentName != null);
            Trace.Assert(name != null);
            Trace.Assert(text != null);

            LoggingService.EnterMethod();
            Elements.ActionMenuNode actionMenuNode =
                new Elements.ActionMenuNode(parentLocation,
                                               parentName,
                                               name,
                                               shortcut,
                                               text,
                                               true,
                                               true,
                                               executeEvent);

            LoggingService.LeaveMethod();

            return actionMenuNode;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentLocation"></param>
        /// <param name="parentName"></param>
        /// <param name="name"></param>
        /// <param name="shortcut"></param>
        /// <param name="text"></param>
        /// <param name="isChecked"></param>
        /// <param name="executeEvent"></param>
        /// <returns></returns>
        protected static Elements.ICustomMenuNode CreateCheckedMenu(MenuItemLocation parentLocation,
                string parentName,
                string name,
                int shortcut,
                string text,
                bool isChecked,
                EventHandler executeEvent)
        {
            Trace.Assert(parentName != null);
            Trace.Assert(name != null);
            Trace.Assert(text != null);

            LoggingService.EnterMethod();
            Elements.CheckedMenuNode result =
                new Elements.CheckedMenuNode(parentLocation,
                                               parentName,
                                               name,
                                               shortcut,
                                               text,
                                               true,
                                               true,
                                               isChecked,
                                               executeEvent);

            LoggingService.LeaveMethod();

            return result;
        }
        ///<summary>
        ///Creates an empty menu.
        ///</summary>
        ///<param name="parentLocation">Parent location</param>
        ///<param name="parentName">Parent name</param>
        ///<param name="name">Name</param>
        ///<param name="text">Text</param>
        ///<returns>
        ///Created EmptyMenu object.
        ///</returns>
        protected static Elements.ICustomMenuNode CreateEmptyMenu(
            MenuItemLocation parentLocation,
            string parentName,
            string name,
            string text)
        {
            Trace.Assert(parentName != null);
            Trace.Assert(name != null);
            Trace.Assert(text != null);

            LoggingService.EnterMethod();
            Elements.EmptyMenuNode emptyMenuNode = new Elements.EmptyMenuNode(parentLocation, parentName, name, text);
            LoggingService.LeaveMethod();

            return emptyMenuNode;
        }

        ///// <summary>
        ///// Creates a menu object.
        ///// </summary>
        ///// <param name="parentLocation"></param>
        ///// <param name="parentName"></param>
        ///// <param name="name"></param>
        ///// <param name="shortcut"></param>
        ///// <param name="text"></param>
        ///// <param name="executeEvent"></param>
        ///// <returns></returns>
        //protected static Elements.ICustomMenuNode CreateOTACustomAction(MenuItemLocation parentLocation,
        //        string parentName,
        //        string name,
        //        int shortcut,
        //        string text,
        //        EventHandler executeEvent)
        //{
        //    return CreateActionMenu(parentLocation, parentName, name, shortcut, text, executeEvent);
        //}

        ///<summary>
        ///Creates a separator menu.
        ///</summary>
        ///<param name="parentLocation">Parent location</param>
        ///<param name="parentName">Parent name</param>
        ///<returns>
        ///Created SeparatorMenu object.
        ///</returns>
        protected static Elements.ICustomMenuNode CreateSeparatorMenu(MenuItemLocation parentLocation,
                string parentName)
        {
            Trace.Assert(parentName != null);

            LoggingService.EnterMethod();
            Elements.SeparatorMenuNode separator = new Elements.SeparatorMenuNode(parentLocation, parentName);
            LoggingService.LeaveMethod();

            return separator;
        }

        ///<summary>
        ///Creates a tab node.
        ///</summary>
        /// <param name="parent">Parent name</param>
        ///<param name="name">Name</param>
        /// <param name="pageType">Tab page type</param>
        ///<returns>
        ///Created TabNode object.
        ///</returns>
        protected static Elements.TabNode CreateTabNode( string parent, string name, Type pageType )
        {

            Trace.Assert(parent != null);
            Trace.Assert(name != null);

            LoggingService.EnterMethod();
            Elements.TabNode tabNode = new Elements.TabNode(parent, name, pageType);

            LoggingService.LeaveMethod();

            return tabNode;
        }

        ///<summary>
        ///Creates a tab node.
        ///</summary>
        ///<param name="name">Name</param>
        /// <param name="pageType">Tab page type</param>
        ///<returns>
        ///Created TabNode object.
        ///</returns>
        protected static Elements.TabNode CreateTabNode( string name, Type pageType )
        {
            Trace.Assert(name != null);

            LoggingService.EnterMethod();
            Elements.TabNode tabNode = new Elements.TabNode(name, pageType);
            LoggingService.LeaveMethod();

            return tabNode;
        }

        ///<summary>
        ///Registers the wizard actions.
        ///</summary>
        protected virtual void IdeRegisterMenus( )
        { }

        ///<summary>
        ///Registers notification.
        ///</summary>
        protected virtual void IdeRegisterNotifications( )
        { }

        ///<summary>
        ///Registers tab nodes.
        ///</summary>
        protected virtual void IdeRegisterTabs( )
        { }

        ///<summary>
        ///Registers wizard class.
        ///</summary>
        protected virtual void IdeRegisterWizards( )
        { }

        /// <summary>
        /// Registers a menu.
        /// </summary>
        /// <param name="menu">Menu</param>
        protected void RegisterAction(Elements.ICustomMenuNode menu )
        {
            RegisterMenu(menu);
        }

        ///<summary>
        ///Registers an action.
        ///</summary>
        ///<param name="menu">Menu</param>
        protected void RegisterMenu( Elements.ICustomMenuNode menu )
        {

            LoggingService.EnterMethod();
            if (menu != null)
            {
                Menus.Add(menu);
            }
            LoggingService.LeaveMethod();

        }

        ///<summary>
        ///Registers a tab node.
        ///</summary>
        ///<param name="node">Tab node</param>
        protected void RegisterTab( Elements.TabNode node )
        {
            LoggingService.EnterMethod();
            if (node != null)
            {
                TabNodes.Add(node);
            }
            LoggingService.LeaveMethod();

        }

        private IList<Elements.ICustomMenuNode> menus = new List<Elements.ICustomMenuNode>();
        private IList<Elements.TabNode> tabNodes = new List<Elements.TabNode>();
    }
}

