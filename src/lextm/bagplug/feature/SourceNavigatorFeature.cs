using System;
using Lextm.OpenTools;

namespace Lextm.BagPlug.Feature {

    public class SourceNavigatorFeature : CustomFeature {

		private Gui.FormSourceNavigator form;
		
		private void DoViewMethods(object aSender, EventArgs AEventArgs) {
            //DoView(ViewCodeDomMode.Methods);
            if ((form == null) || form.IsDisposed) {
            	form = new Gui.FormSourceNavigator();
            }
            form.Show();
        }

        protected override void IdeRegisterMenus() {
            // View Methods
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
												 ShareUtils.MenuRootDefault,
                                                 "ViewMethodsMenu",
												 ShareUtils.Ctrl + ShareUtils.Shift + (int)
												 System.Windows.Forms.Keys.Q,  // Ctrl G
												 "Source Navigator",
                                                 new EventHandler(DoViewMethods)));


        }
    }
}
