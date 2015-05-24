using System.Collections.Generic;
using System.Windows.Forms;


namespace BeWise.SharpBuilderTools.Helpers {
	/// <summary>
	/// Base view helper.
	/// </summary>
    public abstract class BaseViewHelper : IComparer<string> {

        /**************************************************************/
        /*                      Private
        /**************************************************************/

        private IList<string> fItems = new List<string>();

        /**************************************************************/
        /*                      Public
        /**************************************************************/
		/// <summary>
		/// Creates list view item.
		/// </summary>
		/// <param name="item">Item</param>
		/// <param name="parameters">Parameters</param>
		/// <returns></returns>
        protected abstract ListViewItem CreateListViewItem(object item, object[] parameters);
        /// <summary>
        /// Creates list view.
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns></returns>
        public ListViewItem CreateListViewItem(object item) {
            return CreateListViewItem(item, null);
        }
        /// <summary>
        /// Compares.
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <returns></returns>
        public abstract int Compare(string x, string y);
        /// <summary>
        /// Goes to an object.
        /// </summary>
        /// <param name="item">Item</param>
        public abstract void NavigateTo(object item);
        /// <summary>
        /// Verifies if an item is visible.
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <param name="scopeFilter">Scope filter</param>
        /// <param name="item">Item</param>
        /// <returns></returns>
        public abstract bool ItemIsVisible(string filter, string scopeFilter, object item);
        /// <summary>
        /// Loads.
        /// </summary>
        public abstract void Load();
		/// <summary>
		/// Items.
		/// </summary>
        public IList<string> Items {
            get {
                return fItems;
            }
        }
    }
}
