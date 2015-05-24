using System;
using System.Threading;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Helpers;
using BeWise.SharpBuilderTools.Tools;
using BeWise.SharpBuilderTools.Tools.Expresso;
using Lextm.OpenTools;
using System.Globalization;

namespace Lextm.NFamily.Feature {
    public class ExpressoFeature : CustomFeatureTool {
        /**************************************************************/
        /*                        Private
        /**************************************************************/

        private static Expresso fCurrentExpresso;

//        private void AddCompilerMessages(string[] aMessages) {
//            IOTAMessageService _MessageService = OTAUtils.GetMessageService();
//            _MessageService.ShowMessageView(null);
//            IntPtr _Unknown;
//
//            foreach(string _Msg in aMessages) {
//                _MessageService.AddCompilerMessage("", _Msg, CurrentExpresso.Name, OTAMessageKind.otamkInfo, 0, 0, new IntPtr(0), out _Unknown);
//            }
//        }

        /**************************************************************/
        /*                       Protected
        /**************************************************************/

		private void DoOpenExpresso(object aSender, EventArgs e) {
            Thread _T = new Thread(new ThreadStart(OpenExpressoGui));
            _T.Start();
        }

		private void DoOpenExpressoProject(object aSender, EventArgs e) {
            Thread _T = new Thread(new ThreadStart(RunExpressoGui));
            _T.Start();
        }

        protected override BaseTool GetCurrentTool() {
            if (fCurrentExpresso == null) {
                fCurrentExpresso = new Expresso();
                fCurrentExpresso.OnConsoleOutput += new EventHandler(DoRefreshConsole);
                fCurrentExpresso.OnRunCompleted += new EventHandler(DoRunCompleted);
            }

            return fCurrentExpresso;
        }

		private void OpenExpressoGui() {
        	if(!ValidationHelpers.ValidateToolInstalled(GetCurrentTool())) {
                return;
            }

            AddMessage("==============================================================");
            AddMessage(String.Format(CultureInfo.InvariantCulture, "[{0}] Openning " + CurrentExpresso.GetName(),  DateTime.Now));
            AddMessage("Executable:  " + CurrentExpresso.GetGui());
            AddMessage("==============================================================");

            CurrentExpresso.OpenGui();
        }

		private void RunExpressoGui() {
            string _Filename = GetToolFileFromCurrentProject();

            if(_Filename == null || !ValidationHelpers.ValidateToolInstalled(GetCurrentTool())) {
                return;
            }

            AddMessage("==============================================================");
            AddMessage(String.Format(CultureInfo.InvariantCulture, "[{0}] Openning " + CurrentExpresso.GetName(), DateTime.Now));
            AddMessage("Executable:  " + CurrentExpresso.GetGui());
            AddMessage("Expresso project: " + _Filename);
            AddMessage("==============================================================");

            CurrentExpresso.RunGui(new object[] { StringHelper.QuoteString(_Filename) }, "");
        }

        /**************************************************************/
        /*                        Public
        /**************************************************************/

		protected override void IdeRegisterMenus() {
            base.IdeRegisterMenus();
			// Expresso
			RegisterAction(CreateEmptyMenu(MenuItemLocation.Child,
												 ShareUtils.MenuRootDefault,
												 "ExpressoMenu",
												 "Expresso"));

            // Open Expresso Project
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
                                                 "ExpressoMenu",
                                                 "OpenExpressoProjectMenu",
                                                 0,
                                                 "Open Expresso Project",
                                                 new EventHandler(DoOpenExpressoProject)));

            // Open Expresso Gui
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
                                                 "ExpressoMenu",
                                                 "OpenExpressoGuiMenu",
                                                 0,
                                                 "Open Expresso",
                                                 new EventHandler(DoOpenExpresso)));
        }

        // *************************************************************************
        //                              Properties
        // *************************************************************************

		private Expresso CurrentExpresso {
            get {
        		return (Expresso) GetCurrentTool();
			}
		} 

		protected override void IdeRegisterTabs() {
            RegisterTab(CreateTabNode("Expresso", typeof(Gui.ExpressoPage)));
        }
		/// <summary>
		/// Constructor.
		/// </summary>
        public ExpressoFeature( ) {}
    
	}
}
