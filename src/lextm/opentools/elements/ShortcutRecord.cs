// This is the shortcut record class.
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

using System;

namespace Lextm.OpenTools.Elements {


	///<summary>
	/// Record for a shortcut.
	/// </summary>
	public class ShortcutRecord {
		
		/// <summary>
		/// Text.
		/// </summary>
		public string Text {
			get {
				return menuItem.Text;
			}
		}

		private Borland.Studio.ToolsAPI.IOTAMenuItem menuItem;

		/// <summary>
		/// Menu item.
		/// </summary>
		internal Borland.Studio.ToolsAPI.IOTAMenuItem MenuItem {
            get {
				return menuItem;
            }
        }

		private int shortcut;

		/// <summary>
		/// value.
		/// </summary>
		/// <remarks>Calculated TShortcut value.</remarks>
        public int Shortcut {
            get {
				return shortcut;
            }
            set {
				shortcut = value;
            }
        }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="item">Menu item</param>
		/// <param name="shortcut">Shortcut value</param>
		/// <returns></returns>
		internal ShortcutRecord( Borland.Studio.ToolsAPI.IOTAMenuItem item, int shortcut ) {
			this.menuItem = item;
			this.shortcut = shortcut;
        }

		private ShortcutRecord( ) {
		}
	}
}
