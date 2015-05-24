// This is the ota base action class. Ported from SBT.
//		The interface is unchanged but a different implementation is given.
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
using System.Reflection;
using BeWise.Common.Utils;
using Borland.Studio.ToolsAPI;

namespace Lextm.OpenTools.Elements {


	/// <summary>
	/// Base action class.
	/// </summary>
	/// <remarks>
	/// Ported from SBT.
	/// But a few fields imported from removed ActionMenuNode class.
	/// </remarks>
	public abstract class CustomMenuNode: Lextm.Collections.CustomNode, ICustomMenuNode, Lextm.Collections.ICustomNode {

		private  bool enabled;
		private  EventHandler executed;
		private  int shortcut;
		private  string text;
		private  bool visible;
        bool isChecked;
        //private  HelpString help;
		private  string parentName;		
		private  OTAMenuItemLocation parentLocation;

		///<summary>
		///Constructor.
		///</summary>
		/// <param name="parentLocation">Parent location</param>
		/// <param name="parentName">Parent name</param>
		/// <param name="name">Name</param>
		/// <param name="shortcut">Shortcut</param>
		/// <param name="text">Text</param>
		/// <param name="enabled">Enabled</param>
		/// <param name="visible">Visible</param>
		/// <param name="isChecked">Check state</param>
		/// <param name="executed">Execute event handler</param>
		protected CustomMenuNode( MenuItemLocation parentLocation, string parentName, string name, int shortcut, string text, bool enabled, bool visible, bool isChecked, EventHandler executed ): base(name) {
			this.text = text;
			this.enabled = enabled;
			this.visible = visible;
			this.executed += executed;
			this.shortcut = shortcut;
			this.parentName = parentName;
            this.isChecked = isChecked;
			// This.help = new HelpString(this);
			switch(parentLocation) {
				case MenuItemLocation.After:
					this.parentLocation = OTAMenuItemLocation.otamlAfter;
					break;
				case MenuItemLocation.Before:
					this.parentLocation = OTAMenuItemLocation.otamlBefore;
					break;
				case MenuItemLocation.Child:
					this.parentLocation = OTAMenuItemLocation.otamlChild;
					break;
				default:
					this.parentLocation = OTAMenuItemLocation.otamlChild;
					break;
			}
		}		


		///<summary>
		///Creates a menu from node.
		///</summary>
		public override void Register( ) {
			 //Debug.ConsoleOut("\t" + Name + " menu is created.");


		   	 theShortcutList.Add(
		   	 	 OtaUtils.AddMenuItem(
				 	parentName,
				 	parentLocation,
				 	Name,
				 	text,
				 	executed,
				 	visible,
				 	enabled,
                    isChecked,
				 	IO.Path.GetImageFile(Name)
				 ),
				 shortcut
		    );

		}
		
        ///<summary>
		///Parent name.
		///</summary>
		public string ParentName {
           	get {
                return parentName;
            }
        }

//		///<summary>
//		///Parent location.
//		///</summary>
//		public MenuItemLocation ParentLocation {
//			get {
//				return parentLocation;
//			}
//		}
				
		private static ShortcutService theShortcutList = ShortcutService.Instance;
		
#region subclass
        ///// <summary>
        ///// Help string.
        ///// </summary>
        //internal class HelpString {
        //    /// <summary>
        //    /// Construtor.
        //    /// </summary>
        //    /// <param name="action">Action</param>
        //    internal HelpString( ICustomMenuNode action ) {
        //        this.action = action;
        //    }

        //    private ICustomMenuNode action;
        //    /// <summary>
        //    /// Verifies if it is available.
        //    /// </summary>
        //    /// <returns>true if available, false if not.</returns>
        //    public bool IsAvailable() {
        //        return !String.IsNullOrEmpty(Topic);
        //    }

        //    /// <summary>
        //    /// Action.
        //    /// </summary>
        //    private ICustomMenuNode Action {
        //        get {
        //            return action;
        //        }
        //    }
        //    /// <summary>
        //    /// Text.
        //    /// </summary>
        //    public string Text {
        //        get {
        //            Type _Type = typeof(BeWise.SharpBuilderTools.HelpAction);
        //            PropertyInfo _PropertyInfo = _Type.GetProperty(String.Format("{0}HelpText", Action.Name));

        //            if (_PropertyInfo == null) {
        //                return null;
        //            } else {
        //                return (string) _PropertyInfo.GetValue(Action, null);
        //            }
        //        }
        //    }
        //    /// <summary>
        //    /// Title.
        //    /// </summary>
        //    public string Title {
        //        get {
        //            Type _Type = typeof(BeWise.SharpBuilderTools.HelpAction);
        //            PropertyInfo _PropertyInfo = _Type.GetProperty(String.Format("{0}HelpTitle", Action.Name));

        //            if (_PropertyInfo == null) {
        //                return null;
        //            } else {
        //                return (string) _PropertyInfo.GetValue(Action, null);
        //            }
        //        }
        //    }
        //    /// <summary>
        //    /// Topic.
        //    /// </summary>
        //    private string Topic {
        //        get {
        //            Type _Type = typeof(BeWise.SharpBuilderTools.HelpAction);
        //            PropertyInfo _PropertyInfo = _Type.GetProperty(String.Format("{0}HelpTopic", Action.Name));

        //            if (_PropertyInfo == null) {
        //                return null;
        //            } else {
        //                return (string) _PropertyInfo.GetValue(Action, null);
        //            }
        //        }
        //    }
        //}
#endregion
    }
}
