// this is the menu builder.
//		It processes all nodes to menu items.
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
namespace Lextm.CodeBeautifierCollection.Collections {

    using Lextm.Diagnostics;
    using Lextm.OpenTools;
    using Lextm.OpenTools.Elements;
    using System.Globalization;
	///<summary>
	///Menu builder.
	///</summary>
	internal sealed class MenuBuilder {
		
		private int nodeCount;
		/// <summary>
		/// Tree node count.
		/// </summary>
		public int NodeCount {
			get {
				return nodeCount;
			}
		}

		///<summary>
		///Adds an action menu node.
		///</summary>
		/// <param name="node">Node</param>
		internal void AddNode( ICustomMenuNode node ) {
        	
			LoggingService.EnterMethod();
			
			if (node != null) {
				ICustomMenuNode parent = GetNode(node.ParentName);
				
				if (parent != null) { // must have a father
					//Debug.ConsoleOut(parent.Name + " is the father.");
					parent.AddChild(node);
					nodeCount++;
					//Debug.ConsoleOut("add node " + node.Name);
				} else {
					LoggingService.Warn(System.String.Format(CultureInfo.InvariantCulture,
					    "{0} Failed: null parent {1}.", 
					    this.GetType(),
					    node.ParentName));
				}
			}
			LoggingService.LeaveMethod();
			
		}

		private PlaceholderMenuNode root;
		
		///<summary>
		///Creates all menus.
		///</summary>
		internal void Build( ) {
			
			LoggingService.EnterMethod();
			
			CustomMenuNode.RegisterWithChildren(root); // creates all menus

			ShortcutService.LoadShortcuts(); // loads shortcuts
			ShortcutService.RefreshShortcuts();

			LoggingService.LeaveMethod();
		
		}

		private ICustomMenuNode GetNode( string name ) {
			//System.Diagnostics.Trace.ConsoleOut("get node " + name);
			//LoggingService.AddDebug("get node " + name);
			ICustomMenuNode result = (ICustomMenuNode)Lextm.Collections.CustomNode.GetChild(root, name);
			System.Diagnostics.Trace.Assert(result != null);
			return result;
		}

		private static MenuBuilder instance;

		///<summary>
		///Gets singleton instance.
		///</summary>
		/// <returns>
		///Instance.
		///</returns>
		internal static MenuBuilder getInstance( ) {
			lock (typeof(MenuBuilder)) {
				if (instance == null) {
					instance = new MenuBuilder();					
				}
			}
			return instance;
		}

		private MenuBuilder( ) {
			
			LoggingService.EnterMethod();

			root = new PlaceholderMenuNode(Root);
			nodeCount++;
			
			LoggingService.LeaveMethod();
			
		}

		// this name should not be an existed menu item.
		private const string Root = "Root";
	}
}
