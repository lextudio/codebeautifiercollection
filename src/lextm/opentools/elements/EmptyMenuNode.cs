// This is empty menu class. Ported from SBT ota empty action class.
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

namespace Lextm.OpenTools.Elements {


	/// <summary>
	/// Empty action.
	/// </summary>
	/// <remarks>Tranlated to an menu item wothout actions.</remarks>
    internal class EmptyMenuNode : CustomMenuNode, ICustomMenuNode
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parentLocation">Parent location</param>
        /// <param name="parentName">Parent name</param>
        /// <param name="name">Name</param>
        /// <param name="text">Text</param>
        internal EmptyMenuNode(MenuItemLocation parentLocation, string parentName, string name, string text) :
            base(parentLocation,
                             parentName,
                             name,
                             0,
                             text,
                             true,
                             true,
                             false,
                             null)
        {
        }
    }
}
