// this is the ICustomNode interface which defines what functions a node should provide.
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
using System.Collections.Generic;

namespace Lextm.Collections
{
	/// <summary>
	/// ICustomNode interface.
	/// </summary>
	public interface ICustomNode
	{
		/// <summary>
		/// Name.
		/// </summary>
		string Name {
			get;
		}
		/// <summary>
		/// Parent.
		/// </summary>
		ICustomNode Parent {
			get;
			set;
		}
		/// <summary>
		/// Children.
		/// </summary>
		IList<ICustomNode> Children {
			get;
		}
		/// <summary>
		/// Adds a child.
		/// </summary>
		void AddChild(ICustomNode node);
		/// <summary>
		/// Registers.
		/// </summary>
		void Register();
	}
}
