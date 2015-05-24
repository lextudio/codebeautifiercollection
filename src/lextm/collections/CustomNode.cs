// this is the custom node class. It implements the ICustomNode interface.
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
namespace Lextm.Collections {

	using Lextm.Diagnostics;
    using System.Collections.Generic;
	///<summary>
	///Basic tree node.
	///</summary>
	/// <remarks>
	///Parent and Children are used in queries.
	///</remarks>
	public abstract class CustomNode : ICustomNode {

		///<summary>
		///Adds a child node.
		///</summary>
		public void AddChild( ICustomNode node ) {
			if (node != null) {
				if (!childrenNames.Contains(node.Name))
				{	
					children.Add(node);
					childrenNames.Add(node.Name);
					node.Parent = this;
				} else {
					LoggingService.Warn("Node is already there: " + node.Name);
				}
			}
		}

		///<summary>
		///Name.
		///</summary>
		public string Name {
            get {
                return name;
            }
        }

		private ICustomNode parent;

		///<summary>
		///Parent node.
		///</summary>
		public ICustomNode Parent {
           	get {
                return parent;
            }
			
			set{
				parent = value;
			}
        }
		
		private string name;
        // why not use a hashtable? Because that will make the nodes out of order.
		private IList<ICustomNode> children = new List<ICustomNode>();
		private IList<string> childrenNames = new List<string>();
		///<summary>
		///Child nodes.
		///</summary>
		public IList<ICustomNode> Children {
            get {
                return children;
            }
        }

		///<summary>
		///Constructor.
		///</summary>
		/// <param name="name">Name</param>
		protected CustomNode( string name ) {
			this.name = name;
		}

		///<summary>
		///Registers a node and all its child nodes.
		///</summary>
		/// <param name="iterator">Iterator</param>
		public static void RegisterWithChildren( ICustomNode iterator ) {
			//TODO : makes non recursive later*/
			if (iterator != null) {
				//Debug.ConsoleOut("iterator is " + iterator.Name);
				iterator.Register();
				if (iterator.Children.Count == 0) {
					return;
				} else {
					foreach(CustomNode node in iterator.Children) {
						CustomNode.RegisterWithChildren(node);
					}
				}
			}
		}

		///<summary>
		///Gets a node.
		///</summary>
		/// <param name="iterator">Iterator</param>
		/// <param name="name">Name</param>
		/// <returns>
		///Node.
		///</returns>
		public static ICustomNode GetChild( ICustomNode iterator, string name ) {
			//TODO : makes non recursive later*/
			
			ICustomNode result = null;
			if (iterator != null) {

				if (iterator.Name == name) {
					result = iterator;
				}
				else {
					if (iterator.Children.Count != 0) {
						foreach(CustomNode node in iterator.Children) {
							result = GetChild(node, name);
							if (result != null) {
								break;
							}
						}
					}
				}
			}
			return result;
		}

		/// <summary>
		/// Registers.
		/// </summary>
		public abstract void Register( );
	}
}
