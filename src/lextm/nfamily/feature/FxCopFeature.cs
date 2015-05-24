using System;
using System.Threading;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Helpers;
using System.Globalization;

namespace Lextm.NFamily.Feature {
	
    public class FxCopFeature : Lextm.OpenTools.CustomFeatureTool {
		
		private const string TabFxCop = "FxCop";
		
		///<summary>
		///Registers tabs.
		///</summary>
		///<remarks>
		/// Used to configure tabs on FormPreferences.
		///</remarks>
		protected override void IdeRegisterTabs() {
			base.IdeRegisterTabs();
			RegisterTab(CreateTabNode(TabFxCop, typeof(Gui.FxCopPage)));
		}

		/**************************************************************/
        /*                        Private
        /**************************************************************/
        
        private static BeWise.SharpBuilderTools.Tools.FxCop.FxCop fCurrentFxCop;

//        private void AddCompilerMessages(string[] aMessages) {
//            IOTAMessageService _MessageService = OTAUtils.GetMessageService();
//            _MessageService.ShowMessageView(null);
//            IntPtr _Unknown;
//
//            foreach(string _Msg in aMessages) {
//                _MessageService.AddCompilerMessage("", _Msg, CurrentFxCop.Name, OTAMessageKind.otamkInfo, 0, 0, new IntPtr(0), out _Unknown);
//            }
//        }

        /**************************************************************/
        /*                       Protected
        /**************************************************************/

		private void DoOpenFxCop(object aSender, EventArgs e) {
            Thread _T = new Thread(new ThreadStart(OpenFxCopGui));
            _T.Start();
        }

		private void DoOpenFxCopProject(object aSender, EventArgs e) {
            if (OtaUtils.GetCurrentProject() != null) {
                Thread _T = new Thread(new ThreadStart(RunFxCopGui));
                _T.Start();
            }
        }

		private void DoAnalyse(object aSender, EventArgs e) {
            if (OtaUtils.GetCurrentProject() != null) {
                Thread _T = new Thread(new ThreadStart(RunFxCopConsole));
                _T.Start();
            }
        }

        protected override void DoRunCompleted(object sender, EventArgs e) {
            DoRefreshConsole(String.Format(CultureInfo.InvariantCulture, "Done! ({0})\n", DateTime.Now), null);
        }

        protected override BeWise.SharpBuilderTools.Tools.BaseTool GetCurrentTool() {
            if (fCurrentFxCop == null) {
                fCurrentFxCop = new BeWise.SharpBuilderTools.Tools.FxCop.FxCop();
                fCurrentFxCop.OnConsoleOutput += new EventHandler(DoRefreshConsole);
                fCurrentFxCop.OnRunCompleted += new EventHandler(DoRunCompleted);
            }

            return fCurrentFxCop;
        }

		private void OpenFxCopGui() {
        	if(!ValidationHelpers.ValidateToolInstalled(GetCurrentTool())) {
                return;
            }

            AddMessage("==============================================================");
            AddMessage(String.Format(CultureInfo.InvariantCulture, "[{0}] Openning " + CurrentFxCop.GetName(),  DateTime.Now));
            AddMessage("Executable:  " + CurrentFxCop.GetGui());
            AddMessage("==============================================================");

            CurrentFxCop.OpenGui();
        }

        private void RunFxCopConsole() {
            string _Filename = GetToolFileFromCurrentProject();

            if(_Filename == null || !ValidationHelpers.ValidateToolInstalled(GetCurrentTool())) {
                return;
            }

            AddMessage("==============================================================");
            AddMessage(String.Format(CultureInfo.InvariantCulture, "[{0}] Running " + CurrentFxCop.GetName(),  DateTime.Now));
            AddMessage("Executable:  " + CurrentFxCop.GetConsole());
            AddMessage("FxCop project: " + _Filename);
            AddMessage("==============================================================");

            Msg = "";
            CurrentFxCop.RunConsole(new object[] {StringHelper.QuoteString(_Filename)}, "");
        }

        private void RunFxCopGui() {
            string _Filename = GetToolFileFromCurrentProject();

            if(_Filename == null || !ValidationHelpers.ValidateToolInstalled(GetCurrentTool())) {
                return;
            }

            AddMessage("==============================================================");
            AddMessage(String.Format(CultureInfo.InvariantCulture, "[{0}] Running " + CurrentFxCop.GetName(),  DateTime.Now));
            AddMessage("Executable:  " + CurrentFxCop.GetGui());
            AddMessage("FxCop project: " + _Filename);
            AddMessage("==============================================================");

            CurrentFxCop.RunGui(new object[] { StringHelper.QuoteString(_Filename) }, "");
        }

        /**************************************************************/
        /*                        Public
        /**************************************************************/
		private const string MenuLeXtudio = "CBCLeXtudioMenu";
		
        protected override void IdeRegisterMenus() {
            // FxCop
            RegisterAction(CreateActionMenu(Lextm.OpenTools.MenuItemLocation.Child,
                                                 MenuLeXtudio,
                                                 "FxCopMenu",
                                                 0,
                                                 "FxCop",
                                                 null));

            // Open FxCop Gui
            RegisterAction(CreateActionMenu(Lextm.OpenTools.MenuItemLocation.Child,
                                                 "FxCopMenu",
                                                 "OpenFxCopGuiMenu",
                                                 0,
                                                 "Open FxCop",
                                                 new EventHandler(DoOpenFxCop)));

            // Open FxCop Project
            RegisterAction(CreateActionMenu(Lextm.OpenTools.MenuItemLocation.Child,
                                                 "FxCopMenu",
                                                 "OpenFxCopProjectMenu",
                                                 0,
                                                 "Open FxCop Project",
                                                 new EventHandler(DoOpenFxCopProject)));

            // Run FxCop Project
            RegisterAction(CreateActionMenu(Lextm.OpenTools.MenuItemLocation.Child,
                                                 "FxCopMenu",
                                                 "FxCopAnalyseMenu",
                                                 0,
                                                 "Analyse",
                                                 new EventHandler(DoAnalyse)));
        }

        // *************************************************************************
        //                              Properties
        // *************************************************************************

        private BeWise.SharpBuilderTools.Tools.FxCop.FxCop CurrentFxCop {
            get {
        		return (BeWise.SharpBuilderTools.Tools.FxCop.FxCop) GetCurrentTool();
            }
        }

    }
}
