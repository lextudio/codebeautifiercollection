using System;
using System.Drawing;
using System.Windows.Forms;

namespace BeWise.Common.IconManagers {
	/// <summary>
	/// Tool box icon manager.
	/// </summary>
    public class ToolboxIconManager : BaseIconManager {

        // *********************************************************************
        //                           Public
        // *********************************************************************
        /// <summary>
        /// Constructor.
        /// </summary>
        public ToolboxIconManager() {
            //AddImage("Default", ToolboxBitmapAttribute.Default.GetImage(typeof(Control)));
        }

        // *********************************************************************
        //                           Public
        // *********************************************************************
        /// <summary>
        /// Gets image index.
        /// </summary>
        /// <param name="control">Control</param>
        /// <returns>Index.</returns>
        public int GetImageIndex(Control control) {
            int _Index = base.GetImageIndex(control.GetType().Name);

            if (_Index < 0) {
                return AddImage(control.GetType().Name, ToolboxBitmapAttribute.Default.GetImage(control.GetType()));
            } else {
                return _Index;
            }
        }
		/// <summary>
		/// Image list.
		/// </summary>
        public new ImageList ImageList {
            get {
                return base.ImageList;
            }
        }
    }
}

