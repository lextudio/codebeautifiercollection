using System;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Helpers;
using Borland.Studio.ToolsAPI;
using Lextm.OpenTools;
using System.Collections.Generic;

namespace Lextm.WiseEditor.Feature {
	
	public class ToolboxManagerFeature : CustomFeature {

#region actions
//		private ToolboxItem[] GetToolboxItemsFromCategory(string aCategoryName) {
//			ArrayList result = new ArrayList();
//
//			IOTAComponentInstallService  _IOTAComponentInstallService = OTAUtils.GetComponentInstallService();
//
//			for (int i = 0; i < _IOTAComponentInstallService.InstalledComponents.Length; i++) {
//				ToolboxItem _Item = _IOTAComponentInstallService.InstalledComponents[i].ToolboxItem;
//
//				if (_IOTAComponentInstallService.InstalledComponents[i].CategoryName == aCategoryName) {
//					result.Add(_Item);
//				}
//			}
//
//			return (ToolboxItem[]) result.ToArray(typeof(ToolboxItem));
//		}

		private class ToolboxItemComparer : IComparer<IOTAInstalledToolboxItem> {

            static ToolboxItemComparer instance;

            internal static ToolboxItemComparer Instance
            {
                get
                {
                    lock (typeof(ToolboxItemComparer))
                    {
                        if (instance == null)
                        {
                            instance = new ToolboxItemComparer();
                        }
                    }
                    return instance;
                }
            }

			ToolboxItemComparer() {}

            public virtual int Compare(IOTAInstalledToolboxItem x, IOTAInstalledToolboxItem y)
            {
                return String.Compare(x.ToolboxItem.DisplayName,
                    y.ToolboxItem.DisplayName,
                    StringComparison.Ordinal);
            }
		}

		/**************************************************************/
		/*                     Protected
        /**************************************************************/

		private void DoSortToolBox(object aSender, EventArgs aEventArgs) {
			IOTAComponentInstallService _IOTAComponentInstallService = OtaUtils.GetComponentInstallService();
			try {
				List<IOTAInstalledToolboxItem> _List = new List<IOTAInstalledToolboxItem>();

				foreach (IOTAInstalledToolboxItem _IOTAInstalledToolboxItem in _IOTAComponentInstallService.InstalledComponents) {
					_List.Add(_IOTAInstalledToolboxItem);
				}

				_IOTAComponentInstallService.Clear();

				_List.Sort(ToolboxItemComparer.Instance);

				foreach (IOTAInstalledToolboxItem _Item in _List) {
					_IOTAComponentInstallService.Add(_Item.ToolboxItem, _Item.CategoryName);
				}

				_IOTAComponentInstallService.SaveState();
			} catch (Exception e) {
				Lextm.Windows.Forms.MessageBoxFactory.Fatal(e);
				_IOTAComponentInstallService.LoadState();
			}
		}
#endregion

		protected override void IdeRegisterMenus() {

            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
                                                 ShareUtils.MenuRootDefault,
			                                     "SortToolBoxMenu",
			                                     0,
			                                     "Sort ToolBox Items",
			                                     new EventHandler(DoSortToolBox)));

		}

	}
}
