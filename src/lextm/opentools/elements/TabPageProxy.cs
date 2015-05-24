// this is tab page record class.
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
using System.Diagnostics;

namespace Lextm.OpenTools.Elements
{
	/// <summary>
	/// Tab page record.
	/// </summary>
	/// <remarks>Used to delay-initialize a tab page.</remarks>
	public class TabPageProxy
	{
		private Type type;
		private Gui.CustomPage instance;
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="type">Page type</param>
		/// <param name="instance">Page instance</param>
		private TabPageProxy( Type type, Gui.CustomPage instance)
		{
			Trace.Assert(type != null);
			this.type = type;
			this.instance = instance;
		}
		/// <summary>
		/// Page instance.
		/// </summary>
		/// <remarks>Instance is created when needed.</remarks>
		public Gui.CustomPage Instance {
			get {
				lock(typeof(TabPageProxy)) {
					if (instance == null)
					{
						instance = Activator.CreateInstance(type) as Gui.CustomPage;
					}
				}
				return instance;
            }
        }
		/// <summary>
		/// Construtor.
		/// </summary>
		/// <param name="pageType">Page type</param>
		public TabPageProxy(Type pageType) : this(pageType, null) {}
	}
}
