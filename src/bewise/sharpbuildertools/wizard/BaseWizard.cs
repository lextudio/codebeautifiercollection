using System;
using Borland.Studio.ToolsAPI;

namespace BeWise.SharpBuilderTools.Wizard {
	/// <summary>
	/// Base wizard for file wizards.
	/// </summary>
    public class BaseWizard {

        /**************************************************************/
        /*                       Constructor
        /**************************************************************/
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="cat">Category</param>
        public BaseWizard(IOTAGalleryCategory cat) {
            category = cat;
        }

        /**************************************************************/
        /*                       Private
        /**************************************************************/
        private IOTAGalleryCategory category = null;

        /**************************************************************/
        /*                       Public
        /**************************************************************/
		/// <summary>
		/// Destroyed.
		/// </summary>
        public void Destroyed() {
        }
		/// <summary>
		/// Gallery category.
		/// </summary>
        public IOTAGalleryCategory GalleryCategory {
            get {
                return category;
            }
        }
		/// <summary>
		/// Author.
		/// </summary>
        public string Author {
            get {
                return PRODUCT_BASE_NAME;
            }
        }
        // Product
        private const string PRODUCT_BASE_NAME                                   = "Sharp Builder Tools";
		/// <summary>
		/// Designer.
		/// </summary>
        public string Designer {
            get {
                return OTADesignerTypes.dDotNet;
            }
        }
    }
}
