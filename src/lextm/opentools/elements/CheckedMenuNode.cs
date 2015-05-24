using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.OpenTools.Elements
{
    class CheckedMenuNode: CustomMenuNode, ICustomMenuNode
    {
        /// <summary>
		/// Construtor.
		/// </summary>
		/// <param name="parentLocation">Parent location</param>
		/// <param name="parentName">Parent name</param>
		/// <param name="name">Name</param>
		/// <param name="shortcut">Shortcut</param>
		/// <param name="text">Text</param>
		/// <param name="enabled">Enabled</param>
		/// <param name="visible">Visible</param>
		/// <param name="isChecked">Check state</param>
		/// <param name="executed">Execute event handler</param>
		internal CheckedMenuNode( MenuItemLocation parentLocation, string parentName, string name, int shortcut, string text, bool enabled, bool visible, bool isChecked, EventHandler executed ): 
			base(parentLocation,
		                      parentName,
		                      name,
		                      shortcut,
		                      text, 
		                      enabled,
		                      visible,
                              isChecked,
		                      executed) {
		}
    }
}
