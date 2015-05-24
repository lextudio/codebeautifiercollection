using System;
using System.Windows.Forms;
using Borland.Studio.ToolsAPI;
using Lextm.CBC.Gui;

namespace Lextm.CBC {
    public class OTACBCOptions : BaseOTA {
        /**************************************************************/
        /*                       Protected
        /**************************************************************/

        protected void DoCBCOptions(object aSender, EventArgs AEventArgs) {
            FrmOption _Frm = new FrmOption();

            if (_Frm.ShowDialog() == DialogResult.OK) {
                Main.SaveConfiguration();
            }   
        }

        /**************************************************************/
        /*                       Public
        /**************************************************************/

        public override void IDERegister(out string[] aMenuNames, out int[] aMenuShortCuts) {
            IOTAMainMenuService _MenuService = null;
            IOTAMenuItem _MenuItem = null;

            _MenuService = (IOTAMainMenuService) BorlandIDE.GetService(typeof(IOTAMainMenuService));

            // Sep1
			_MenuItem = _MenuService.AddMenuItem(Consts.CBC_MENU_NAME,
												 OTAMenuItemLocation.otamlChild,
												 "SepOptionMenu",
												 "-");
			// Options
            _MenuItem = _MenuService.AddMenuItem(Consts.CBC_MENU_NAME,
                                                 OTAMenuItemLocation.otamlChild,
                                                 "CBCOptionsMenu",
                                                 "CBC Options...");
            _MenuItem.Enabled = true;
            _MenuItem.Executed += new EventHandler(DoCBCOptions);

            aMenuNames = new string[] {"CBCOptionMenu"};
            aMenuShortCuts = new int[] {0};
        }
    }
}
