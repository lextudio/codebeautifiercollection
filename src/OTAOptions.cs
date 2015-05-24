using System;
using System.Windows.Forms;
using Borland.Studio.ToolsAPI;
using Lextm.JcfExpert.Gui;

namespace Lextm.JcfExpert
{
	public class OTAJcfExpertOptions : BaseOTA
	{
		/**************************************************************/
		/*                       Protected
		/**************************************************************/

		protected void DoJcfExpertOptions(object aSender, EventArgs AEventArgs)
		{
			FrmOptions _Frm = new FrmOptions();
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
			_MenuItem = _MenuService.AddMenuItem(Consts.JCF_Expert_MENU_NAME,
												 OTAMenuItemLocation.otamlChild,
												 "SepOptionMenu",
												 "-");
			// Options
			_MenuItem = _MenuService.AddMenuItem(Consts.JCF_Expert_MENU_NAME,
												 OTAMenuItemLocation.otamlChild,
												 "JcfExpertOptionsMenu",
												 "JCF Expert Options...");
			_MenuItem.Enabled = true;
			_MenuItem.Executed += new EventHandler(DoJcfExpertOptions);

			aMenuNames = new string[] {"JcfExpertOptionMenu"};
			aMenuShortCuts = new int[] {0};
		}
	}
}
