using System;
using System.IO;
using System.Threading;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Helpers;
using BeWise.SharpBuilderTools.Tools;
using Borland.Studio.ToolsAPI;
using Lextm.OpenTools;

namespace Lextm.WiseEditor.Feature {

	/// <summary>WinMerge feature.</summary>
	/// <remarks>This may be a useful feature before.
	/// However, since IDE now ships History Manager,
	/// which is a much more convenient tool,
	/// you may not need this feature any more.</remarks>
	public class WinMergeFeature : CustomFeature {

		private const string TabWinMerge = "WinMerge";
		
		///<summary>
		///Registers tabs.
		///</summary>
		///<remarks>
		/// Used to configure tabs on FormPreferences.
		///</remarks>
		protected override void IdeRegisterTabs() {
			base.IdeRegisterTabs();
			RegisterTab(CreateTabNode(TabWinMerge, typeof(Gui.WinMergePage)));
		}

		/**************************************************************/
		/*                        Private
        /**************************************************************/

		private BeWise.SharpBuilderTools.Tools.WinMerge.WinMerge fCurrentWinMerge;

		private void OpenWinMergeGui() {
			if (!ValidationHelpers.ValidateToolInstalled(CurrentWinMerge)) {
				return;
			}

			MessageService.Show("Running " + CurrentWinMerge.GetName());
			MessageService.Show("Executable:  " + CurrentWinMerge.GetGui());
			CurrentWinMerge.OpenGui();
		}

		private void CompareSourceFile() {
			IOTASourceEditor _SourceEditor = OtaUtils.GetCurrentSourceEditor();

			if (_SourceEditor != null) {
				string _SourceFile = OtaUtils.GetCurrentEditorFileName();
				string _TempFile = Path.Combine(Path.GetTempPath(),
				                                Path.GetFileName(_SourceFile) + ".Buffer");
				OtaUtils.CreateFileFromBuffer(_SourceEditor, _TempFile);
                CurrentWinMerge.RunConsole(new object[] { StringHelper.QuoteString(_SourceFile), StringHelper.QuoteString(_TempFile) }, "");
				File.Delete(_TempFile);
			}
		}

		/**************************************************************/
		/*                       Protected
        /**************************************************************/

		private void DoOpenWinMerge(object aSender, EventArgs e) {
			Thread _T = new Thread(new ThreadStart(OpenWinMergeGui));
			_T.Start();
		}

		private void DoCompareFile(object aSender, EventArgs e) {
			if (!ValidationHelpers.ValidateToolInstalled(CurrentWinMerge) ||
			    !ValidationHelpers.ValidateCurrentProjectNotNull() ||
			    !ValidationHelpers.ValidateCurrentSourceEditorNotNull()) {
				return;
			}


			Thread _T = new Thread(new ThreadStart(CompareSourceFile));
			_T.Start();
		}

		BaseTool CurrentWinMerge {
			get {
				if (fCurrentWinMerge == null) {
					fCurrentWinMerge = new BeWise.SharpBuilderTools.Tools.WinMerge.WinMerge();
				}
				return fCurrentWinMerge;
			}
		}

		/**************************************************************/
		/*                        Public
        /**************************************************************/

		protected override void IdeRegisterMenus() {
			base.IdeRegisterMenus();
			// WinMerge
			RegisterAction(CreateEmptyMenu(MenuItemLocation.Child,
			                               ShareUtils.MenuRootDefault,
			                               "WinMergeMenu",
			                               "WinMerge"));

			// Open WinMerge Gui
			RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                "WinMergeMenu",
			                                "OpenWinMergeMenu",
			                                0,
			                                "Open WinMerge ...",
			                                new EventHandler(DoOpenWinMerge)));

			// Compare file
			RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                "WinMergeMenu",
			                                "CompareFileMenu",
			                                0,
			                                "Compare file ...",
			                                new EventHandler(DoCompareFile)));
		}
	}
}
