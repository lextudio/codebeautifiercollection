// this is the tab node class.
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
using System.Diagnostics;
using System.Windows.Forms;

using Lextm.Collections;

namespace Lextm.OpenTools.Elements {


	///<summary>
	/// Tab node.
	/// </summary>
	public sealed class TabNode : CustomNode {
		/// <summary>
		/// Adds a tab to TreeView.
		/// </summary>
		public override void Register( ) {
			//Trace.Assert(framework.TreeView != null, "null tree view");
			if (Parent == null)
			{ // root
				//framework.TreeView.Nodes.Add(this.treeNode);
			} else {
				((TabNode)Parent).treeNode.Nodes.Add(
				  	this.treeNode);
				//framework.TabControl.Controls.Add(this.treeNode.Tag as TabPage);
			}
		}

		private System.Windows.Forms.TreeNode treeNode;
		/// <summary>
		/// TreeNode instance.
		/// </summary>
		public TreeNode TreeNode {
			get { return treeNode; }
		}
		private string parentName;
		/// <summary>
		/// Parent name.
		/// </summary>
		public string ParentName {
			get {
				return parentName;
			}
		}
		///<summary>
		///Construtor.
		///</summary>
		///<param name="name">Name</param>
        /// <remarks>Page type is set as Gui.CustomPage.</remarks>
		public TabNode( string name)
			: this(name, typeof(Gui.CustomPage)) { }
        /////<summary>
        /////Construtor.
        /////</summary>
        ///// <param name="parent">Parent</param>
        /////<param name="name">Name</param>
        //internal TabNode( string parent, string name): this(parent, name, typeof(Gui.CustomPage)) {}
        ///<summary>
        ///Construtor.
        ///</summary>
        ///<param name="name">Name</param>
        /// <param name="pageType">Tab page type</param>
        internal TabNode(string name, Type pageType)
            : base(name)
        {
            this.parentName = ShareUtils.TabRoot;
            this.treeNode = new System.Windows.Forms.TreeNode(name);
            Trace.Assert(pageType != null);
            this.treeNode.Tag = new TabPageProxy(pageType);
        }
		///<summary>
		///Construtor.
		///</summary>
		/// <param name="parent">Parent</param>
		///<param name="name">Name</param>
		/// <param name="pageType">Tab page type</param>
		public TabNode( string parent, string name, Type pageType ): base(name) {
			this.parentName = parent;
			this.treeNode = new System.Windows.Forms.TreeNode(name);
			Trace.Assert(pageType != null);
			this.treeNode.Tag = new TabPageProxy(pageType);
		}

		/// <summary>
		/// Adds tabs to a TreeView.
		/// </summary>
		/// <param name="root">Root</param>
		/// <param name="frame">Framework</param>
		public static void FillWithNodesOf(TabNode root, Gui.ITreeViewContainer frame)
		{
			Trace.Assert(frame != null, "null framework");
			RegisterWithChildren(root);
			frame.TreeView.BeginUpdate();
			foreach (TreeNode node in root.TreeNode.Nodes)
			{
				frame.TreeView.Nodes.Add(node);// = root.TreeNode.Nodes.to;
			}
			frame.TreeView.EndUpdate();
		}
	}
}
