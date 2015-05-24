// this is the ICustomFeature interface.
// Copyright (C) 2006  Lex Y. Li
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

using System.Collections.Generic;

namespace Lextm.OpenTools
{

    /// <summary>
    /// ICustomFeature interface.
    /// </summary>
    public interface ILoadableFeature
    {
        ///<summary>
        ///Loads preferences.
        ///</summary>
        void LoadPreferences();

        ///<summary>
        ///Refreshes wizard configuration.
        ///</summary>
        void RefreshPreferences();

        ///<summary>
        /// Registers.
        /// </summary>
        void Register();

        ///<summary>
        ///Saves preferences.
        ///</summary>
        void SavePreferences();

        ///<summary>
        ///Sets default configuration.
        ///</summary>
        void SetDefaultPreferences();

        ///<summary>
        ///List of actions.
        ///</summary>
        IList<Elements.ICustomMenuNode> Menus
        {
            get;
        }

        ///<summary>
        ///FormPreferences tabs.
        ///</summary>
        IList<Elements.TabNode> TabNodes
        {
            get;
        }
    }
}

