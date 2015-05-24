using System;
using System.Windows.Forms;
using System.IO;
using Borland.Studio.ToolsAPI;
using Lextm.JcfExpert.Utils;

namespace Lextm.JcfExpert
{
	public class OTAJediCodeFormat : BaseOTA
	{

		/**************************************************************/
		/*                        Protected
		/**************************************************************/

		protected string GetJcfExe(){
			string _Path = Main.Configuration.JcfPath;

			if (_Path[_Path.Length -1] != '\\') {
				_Path += @"\";
			}

			_Path += Consts.JCF_EXE_NAME;

			return _Path;
		}

		protected void DoCodeFormat(object aSender, EventArgs AEventArgs)
		{
			if (Main.Configuration.JcfPath == "" || !File.Exists(GetJcfExe())) {
				MessageBox.Show("Please configure correctly JCF Expert!", Consts.MESSAGE_BOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else if (!OTAUtils.CurrentModuleIsPasFile()) {
				MessageBox.Show("JEDI Code Format only apply to Delphi files !", Consts.MESSAGE_BOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else {
				IOTAModuleServices _ModuleServices = (IOTAModuleServices) BorlandIDE.GetService(typeof(IOTAModuleServices));
				IOTAModule _Module = _ModuleServices.CurrentModule;

				IOTAEditor _Editor = OTAUtils.GetEditorWithSourceEditor(_ModuleServices.CurrentModule);

				if (_Editor != null && !_Editor.IsModified) {
					IOTASourceEditor _SourceEditor = OTAUtils.GetSourceEditor(_ModuleServices.CurrentModule);

					RunProcess _RP = new RunProcess();
					string[] _Arr = new string[2];

					_Arr[0] = GetJcfExe();
					_Arr[1] = Main.Configuration.JcfParams + @" """ + _Module.FileName + @"""";

					_RP.Run(_Arr, Path.GetDirectoryName(_Module.FileName));

					IOTAActionService _ActionService = (IOTAActionService) BorlandIDE.GetService(typeof(IOTAActionService));
					_ActionService.ReloadFile(_Module.FileName);

				}
				else {
					MessageBox.Show("Save your file before applying the code beautifier!", Consts.MESSAGE_BOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
		}

		/**************************************************************/
		/*                        Public
		/**************************************************************/

		public override void IDERegister(out string[] aMenuNames, out int[] aMenuShortCuts) {
			IOTAMainMenuService _MenuService = null;
			IOTAMenuItem _MenuItem = null;

			_MenuService = (IOTAMainMenuService) BorlandIDE.GetService(typeof(IOTAMainMenuService));

			// Sep
			_MenuItem = _MenuService.AddMenuItem(Consts.JCF_Expert_MENU_NAME, OTAMenuItemLocation.otamlChild, "SepBeautifierMenu2","-");

            // Code Beautifier
			_MenuItem = _MenuService.AddMenuItem(Consts.JCF_Expert_MENU_NAME, OTAMenuItemLocation.otamlChild, "JEDICodeFormatMenu", "JEDI Code Format");
			_MenuItem.Enabled = true;
			_MenuItem.Executed += new EventHandler(DoCodeFormat);

			aMenuNames = new string[] {"JEDICodeFormatMenu"};
			aMenuShortCuts = new int[] {16452}; // Ctrl D
		}
	}
}
