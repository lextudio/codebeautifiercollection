using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace BeWise.SharpBuilderTools.Info {
	/// <summary>
	/// Component info.
	/// </summary>
    public class ComponentInfo {
        /**************************************************************/
        /*                        Constructor
        /**************************************************************/
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="component">Component</param>
        public ComponentInfo(IComponent component) {
            this.component = component;
        }

        /**************************************************************/
        /*                        Private
        /**************************************************************/

        private IComponent component;
        private IList<IComponent> componentList = new List<IComponent>();

        /**************************************************************/
        /*                     Properties
        /**************************************************************/
		/// <summary>
		/// Component list.
		/// </summary>
        public IList<IComponent> ComponentList {
            get {
                return componentList;
            }
        }
		/// <summary>
		/// Component.
		/// </summary>
        public IComponent Component {
            get {
                return component;
            }
        }
		/// <summary>
		/// Name.
		/// </summary>
        public string Name {
            get {
                return (component as Control).Name;
            }
        }
		/// <summary>
		/// Type.
		/// </summary>
        public string TypeName {
            get {
                return (component as Control).GetType().Name;
            }
        }
    }
}
