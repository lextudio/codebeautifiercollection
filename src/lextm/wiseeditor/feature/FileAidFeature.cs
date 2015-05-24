using System;
using Lextm.OpenTools;

namespace Lextm.WiseEditor.Feature {
	
    public class FileAidFeature : CustomFeature {

		private void DoFileAid(object aSender, EventArgs AEventArgs) {
			Gui.FormFileAid form = new Gui.FormFileAid();
			form.ShowDialog();// this form is modal again.
		}

		protected override void IdeRegisterMenus() {

			RegisterMenu(CreateActionMenu(MenuItemLocation.Child,
				ShareUtils.MenuRootDefault,
				"CBCFileAidMenu",
				49237,
				"File Aid",
				new EventHandler(DoFileAid)));

        }
    }
}
