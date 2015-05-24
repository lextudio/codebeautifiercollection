// This is the place holder menu node.
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
namespace Lextm.OpenTools.Elements {

	using Lextm.Diagnostics;
	/// <summary>
	/// Place holder menu node.
	/// </summary>
	/// <remarks>The only instance is the menu tree root.</remarks>
	public class PlaceholderMenuNode : CustomMenuNode, ICustomMenuNode {
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">Name</param>
		public PlaceholderMenuNode( string name ):
				base(MenuItemLocation.Child,
					null, name, 0, null, true, true, false, null) {
		}
		/// <summary>
		/// Registers.
		/// </summary>
		/// <remarks>Empty by default.</remarks>
		public override void Register()
		{
			LoggingService.Info("this is place holder.");
		}
	}
}
