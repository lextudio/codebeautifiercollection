using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BeWise.Common.IconManager {
	/// <summary>
	/// Base icon manager.
	/// </summary>
    public class BaseIconManager {

        // *********************************************************************
        //                           Private
        // *********************************************************************
        private ImageList imageList = new ImageList();
        private IDictionary<string, int> hashtable = new Dictionary<string, int>();

        // *********************************************************************
        //                           Protected
        // *********************************************************************
        /// <summary>
        /// Adds an image.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="image">Image</param>
        /// <returns></returns>
        protected virtual int AddImage(string key, Image image) {
            return AddImage(key, image, false);
        }
		/// <summary>
		/// Adds an image.
		/// </summary>
		/// <param name="key">Key</param>
		/// <param name="image">Image</param>
		/// <param name="selected">Selected flag</param>
		/// <returns></returns>
        protected virtual int AddImage(string key, Image image, bool selected) {
            string _Key = GetKey(key, selected);
            if (hashtable.ContainsKey(_Key)) {
                throw new Lextm.OpenTools.CoreException("Key " + _Key + " is already loaded");
            } else {
                imageList.Images.Add(image);
				int _Index = imageList.Images.Count - 1;
                hashtable.Add(_Key, _Index);
                return _Index;
            }
        }
		/// <summary>
		/// Gets a key.
		/// </summary>
		/// <param name="key">Key</param>
		/// <param name="selected">Selected flag</param>
		/// <returns>Key selected.</returns>
        protected virtual string GetKey(string key, bool selected) {
            return key + "_" + selected;
        }

        // *********************************************************************
        //                           Public
        // *********************************************************************
        /// <summary>
        /// Gets image index.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Index.</returns>
        public int GetImageIndex(string key) {
            return GetImageIndex(key, false);
        }
		/// <summary>
		/// Gets image index.
		/// </summary>
		/// <param name="key">Key</param>
		/// <param name="selected">Selected flag</param>
		/// <returns>-1 if failed.</returns>
        public virtual int GetImageIndex(string key, bool selected) {
            string _Key = GetKey(key, selected);
            if (hashtable.ContainsKey(_Key)) {
                return (int)hashtable[_Key];
            } else {
                return -1;
            }
        }

        // *********************************************************************
        //                           Properties
        // *********************************************************************
        /// <summary>
        /// Image list.
        /// </summary>
        protected ImageList ImageList {
            get {
                return imageList;
            }
        }
    }
}

