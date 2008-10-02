// this is the tab builder class.
//  	It processes nodes to Form Preferences tabs.
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

	using Lextm.Collections;
	using Lextm.Diagnostics;
	using Lextm.OpenTools;
	using Lextm.OpenTools.Elements;
	using Lextm.OpenTools.Gui;
	using System.Windows.Forms;

	///<summary>
	/// Tab builder.
	/// </summary>
	internal sealed class TabBuilder {

		private static TabBuilder instance;

		/// <summary>
		/// Gets singleton instance.
		/// </summary>
		/// <returns>Signleton instance.</returns>
		internal static TabBuilder getInstance( ) {
			lock(typeof(TabBuilder)){
			    if (instance == null)
			    {
					instance = new TabBuilder();
			    }
		    }
		    return instance;
		}
		
		private int nodeCount;
		/// <summary>
		/// Tree node count.
		/// </summary>
		public int NodeCount {
			get {
				return nodeCount;
			}
		}
		
		/// <summary>
		/// Adds a node.
		/// </summary>
		/// <param name="node">Node</param>
		internal void AddNode( TabNode node ) {
		
			LoggingService.EnterMethod();
			if (node != null) {
				if (node.ParentName == root.Name)
				{
					root.AddChild(node);
					nodeCount++;
 					//Debug.ConsoleOut(node.Name + " is added");
				} else {
					ICustomNode parentNode = GetNode(node.ParentName);
					if (parentNode != null) {
						parentNode.AddChild(node);
						nodeCount++;
						//Debug.ConsoleOut(node.Name + " is added");
					} else {
						//Debug.ConsoleOut(node.Name + " has no parent.");
					}
				}
			} else {
                //Debug.ConsoleOut("null node");
			}  
			LoggingService.LeaveMethod();
			
		}
		
		private ICustomNode GetNode( string name ) {
			//System.Diagnostics.Trace.ConsoleOut("get node " + name);
			//LoggingService.AddDebug("get node " + name);
			ICustomNode result = (ICustomNode)Lextm.Collections.CustomNode.GetChild(root, name);
			System.Diagnostics.Trace.Assert(result != null);
			return result;
		}
		/// <summary>
		/// Builds the frame.
		/// </summary>
		internal void Build() {
		
			LoggingService.EnterMethod();
            ITreeViewContainer frame = Gui.FormPreferences.getInstance();
			TabNode.FillWithNodesOf(root, frame);
			LoggingService.LeaveMethod();		
		}

		private TabNode root;

		private TabBuilder( ) {			
			root = new TabNode(ShareUtils.TabRoot);
			nodeCount++;
		}
	}
}
