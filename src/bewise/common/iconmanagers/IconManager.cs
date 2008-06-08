using System;
using System.Drawing;
using System.Windows.Forms;

namespace BeWise.Common.IconManagers {
	/// <summary>
	/// Icon manager.
	/// </summary>
    public class IconManager : BaseIconManager {
        // *********************************************************************
        //                           Public
        // *********************************************************************
        /// <summary>
        /// Adds an image.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="image">Image</param>
        /// <returns>Index.</returns>
        public new int AddImage(string key, Image image) {
            return base.AddImage(key, image, false);
        }
		/// <summary>
		/// Adds an image.
		/// </summary>
		/// <param name="key">Key</param>
		/// <param name="image">Image</param>
		/// <param name="selected">Selected</param>
		/// <returns>Index.</returns>
        public new int AddImage(string key, Image image, bool selected) {
            return base.AddImage(key, image, selected);
        }

        // *********************************************************************
        //                           Properties
        // *********************************************************************
        /// <summary>
        /// Image list.
        /// </summary>
        public new ImageList ImageList;
    }
}

