using System.Windows.Forms;
using BeWise.Common.Utils;

namespace BeWise.SharpBuilderTools.Helpers {
	/// <summary>
	/// List view helper.
	/// </summary>
	public sealed class ListViewHelper {

    	private ListViewHelper() { }

        /**************************************************************/
        /*                     Protected
        /**************************************************************/
		/// <summary>
		/// Selects an item.
		/// </summary>
		/// <param name="aListView">List view</param>
		/// <param name="aListViewItem">Item</param>
        private static void SelectItem(Vista_Api.ListView aListView, ListViewItem aListViewItem) {
            if (aListViewItem != null) {
                aListView.SelectedItems.Clear();
                aListViewItem.Selected = true;
                aListViewItem.Focused = true;
                aListViewItem.EnsureVisible();
            }
        }

		/// <summary>
		/// Sends key to list view.
		/// </summary>
		/// <param name="aListView">List view</param>
		/// <param name="e">Key event</param>
        public static void SendKeyToListView(Vista_Api.ListView aListView, System.Windows.Forms.KeyEventArgs e) {
            if ((e.KeyCode == Keys.Up) || (e.KeyCode == Keys.Down) ||
                    (e.KeyCode == Keys.Home) || (e.KeyCode == Keys.End) ||
                    (e.KeyCode == Keys.PageUp) || (e.KeyCode == Keys.PageDown)) {
                if ((!e.Shift) || (e.KeyCode == Keys.Up) || (e.KeyCode == Keys.Down)) {
                    if ((!e.Control) || ((e.KeyCode != Keys.Up) && (e.KeyCode != Keys.Down))) {
                        if (aListView.MultiSelect && e.Control && ((e.KeyCode == Keys.Home) || (e.KeyCode == Keys.End))) {
                            if (e.KeyCode == Keys.Home)
							{    SelectFirstItem(aListView);   }
                            else if (e.KeyCode == Keys.End)
							{    SelectLastItem(aListView);   }
                        } else if (e.KeyCode == Keys.Up) {
                            SelectPreviousItem(aListView);
                        } else if (e.KeyCode == Keys.Down) {
                            SelectNextItem(aListView);
                        } else {
                            WinApi.PostMessage(aListView.Handle, WinApi.WM_KEYDOWN, e.KeyValue, 0);
                        }

                        e.Handled = true;
                    }
                }
            }
        }
		/// <summary>
		/// Selects first item.
		/// </summary>
		/// <param name="aListView">List view</param>
		private static void SelectFirstItem(Vista_Api.ListView aListView) {
            if (aListView.Items.Count > 0) {
                SelectItem(aListView, aListView.Items[0]);
            }
        }
		/// <summary>
		/// Selects last item.
		/// </summary>
		/// <param name="aListView">List view</param>
		private static void SelectLastItem(Vista_Api.ListView aListView) {
            if (aListView.Items.Count > 0) {
                SelectItem(aListView, aListView.Items[aListView.Items.Count-1]);
            }
        }
		/// <summary>
		/// Selects next item.
		/// </summary>
		/// <param name="aListView">List view</param>
		private static void SelectNextItem(Vista_Api.ListView aListView) {
            if (aListView.Items.Count > 0) {
                ListViewItem _Item = null;

                if (aListView.SelectedItems.Count == 0) {
                    _Item = aListView.Items[0];
                } else if (aListView.SelectedIndices[0] < (aListView.Items.Count -1)) {
                    _Item = aListView.Items[aListView.SelectedIndices[0] + 1];
                }

                SelectItem(aListView, _Item);
            }
        }
		/// <summary>
		/// Selects previous item.
		/// </summary>
		/// <param name="aListView">List view</param>
        private static void SelectPreviousItem(Vista_Api.ListView aListView) {
            if (aListView.Items.Count > 0) {
                ListViewItem _Item = null;

                if (aListView.SelectedItems.Count == 0) {
                    _Item = aListView.Items[0];
                } else if (aListView.SelectedIndices[0] != 0) {
                    _Item = aListView.Items[aListView.SelectedIndices[0] - 1];
                }

                SelectItem(aListView, _Item);
            }
        }
    }
}
