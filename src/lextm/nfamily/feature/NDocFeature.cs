using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Gui;
using BeWise.SharpBuilderTools.Helpers;
using Lextm.LeXDK;

namespace Lextm.NFamily.Feature {

    public class NDocFeature : CustomFeatureTool {
		
		private const string TabNDoc = "NDoc";

		
		///<summary>
		///Registers tabs.
		///</summary>
		///<remarks>
		/// Used to configure tabs on FormPreferences.
		///</remarks>
		protected override void IDERegisterTabs() {
			base.IDERegisterTabs();
			RegisterTab(CreateTabNode(TabNDoc, typeof(Gui.NDocPage)));
		}
       
        private BeWise.SharpBuilderTools.Tools.NDoc.NDoc fCurrentNDoc = null;
        private string fDocumenter = BeWise.SharpBuilderTools.Tools.NDoc.NDoc.DefaultDocumentor;

//        private void AddCompilerMessages(string[] aMessages) {
//            IOTAMessageService _MessageService = OTAUtils.GetMessageService();
//            _MessageService.ShowMessageView(null);
//            IntPtr _Unknown;
//
//            foreach(string _Msg in aMessages) {
//                _MessageService.AddCompilerMessage("", _Msg, CurrentNDoc.Name, OTAMessageKind.otamkInfo, 0, 0, new IntPtr(0), out _Unknown);
//            }
//        }

        private void OpenNDocGui() {
            if (!ValidationHelpers.ValidateToolInstalled(CurrentNDoc)) {
                return;
            }
                               
            AddMessage("==============================================================");
            AddMessage(String.Format("[{0}] Openning " + CurrentNDoc.Name,  DateTime.Now));
            AddMessage("Executable:  " + CurrentNDoc.ToolGuiExe);
            AddMessage("==============================================================");

            CurrentNDoc.OpenGui();
        }

        private void RunNDocConsole() {
            string _Filename = GetToolFileInCurrentProject();

            if (_Filename == null || !ValidationHelpers.ValidateToolInstalled(CurrentNDoc)) {
                return;
            }


            AddMessage("==============================================================");
            AddMessage(String.Format("[{0}] Running " + CurrentNDoc.Name,  DateTime.Now));
            AddMessage("Executable:  " + CurrentNDoc.ToolConsoleExe);
            AddMessage("NDoc project: " + _Filename);
            AddMessage("==============================================================");

            Msg= "";
            string _Documenter = "";

            if (fDocumenter != null) {
                _Documenter = "-documenter=" + fDocumenter;
            }

            CurrentNDoc.RunConsole(new object[] {"-verbose " + _Documenter + " -project=" + Utils.QuoteString(_Filename)},
                                   Path.GetDirectoryName(_Filename));
        }

        private void RunNDocGui() {
            string _Filename = GetToolFileInCurrentProject();

            if (_Filename == null || !ValidationHelpers.ValidateToolInstalled(CurrentNDoc)) {
                return;
            }

            AddMessage("==============================================================");
            AddMessage(String.Format("[{0}] Running " + CurrentNDoc.Name,  DateTime.Now));
            AddMessage("Executable:  " + CurrentNDoc.ToolGuiExe);
            AddMessage("NDoc project: " + _Filename);
            AddMessage("==============================================================");

            CurrentNDoc.RunGui(new object[] {Utils.QuoteString(_Filename)}, Path.GetDirectoryName(_Filename));
        }

        /**************************************************************/
        /*                       Protected
        /**************************************************************/

		private void DoOpenNDoc(object aSender, EventArgs e) {
            Thread _T = new Thread(new ThreadStart(OpenNDocGui));
            _T.Start();
        }

		private void DoOpenNDocProject(object aSender, EventArgs e) {
			if (!ValidationHelpers.ValidateCurrentProjectNotNull()) {
                return;
            }

            Thread _T = new Thread(new ThreadStart(RunNDocGui));
            _T.Start();
        }

		private void DoGenerateDefaultDoc(object aSender, EventArgs e) {
            if (!ValidationHelpers.ValidateCurrentProjectNotNull()) {
                return;
            }

            fDocumenter = null;
            Thread _T = new Thread(new ThreadStart(RunNDocConsole));
            _T.Start();
        }

		private void DoGenerateDoc(object aSender, EventArgs e) {
            if (!ValidationHelpers.ValidateCurrentProjectNotNull()) {
                return;
            }

            FrmNDoc _FrmNDoc = new FrmNDoc();
            _FrmNDoc.Documenter = fDocumenter;

            if (_FrmNDoc.ShowDialog() == DialogResult.OK) {
                fDocumenter = _FrmNDoc.Documenter;

                Thread _T = new Thread(new ThreadStart(RunNDocConsole));
                _T.Start();
            }
        }

		protected override void DoRunCompleted(object aSender, EventArgs e) {
            DoRefreshConsole(String.Format("Done! ({0})\n", DateTime.Now));
        }

        protected override BeWise.SharpBuilderTools.Tools.BaseTool GetCurrentTool() {
            if (fCurrentNDoc == null) {
                fCurrentNDoc = new BeWise.SharpBuilderTools.Tools.NDoc.NDoc();
                fCurrentNDoc.OnConsoleOutput += new RunProcess.ConsoleOutputEvent(DoRefreshConsole);
                fCurrentNDoc.OnRunCompleted += new EventHandler(DoRunCompleted);
            }

            return fCurrentNDoc;
        }

        /**************************************************************/
        /*                        Public
        /**************************************************************/
		private const string MenuLeXtudio = "CBCLeXtudioMenu";
		
		protected override void IDERegisterMenus() {
            // NDoc
            RegisterAction(CreateOTACustomAction(MenuItemLocation.Child,
                                                 MenuLeXtudio,
                                                 "NDocMenu",
                                                 0,
                                                 "NDoc",
                                                 null));

            // Open NDoc Gui
            RegisterAction(CreateOTACustomAction(MenuItemLocation.Child,
                                                 "NDocMenu",
                                                 "OpenNDocGuiMenu",
                                                 0,
                                                 "Open NDoc",
                                                 new EventHandler(DoOpenNDoc)));

            // Open NDoc Project
            RegisterAction(CreateOTACustomAction(MenuItemLocation.Child,
                                                 "NDocMenu",
                                                 "OpenNDocProjectMenu",
                                                 0,
                                                 "Open NDoc Project",
                                                 new EventHandler(DoOpenNDocProject)));

            // Separator
            RegisterAction(CreateSeparatorMenu(MenuItemLocation.Child,
                                                  "NDocMenu"));

            // Generate Default Doc
            RegisterAction(CreateOTACustomAction(MenuItemLocation.Child,
                                                 "NDocMenu",
                                                 "GenerateDefaultDocMenu",
                                                 0,
                                                 "Generate Default Project Doc",
                                                 new EventHandler(DoGenerateDefaultDoc)));

            // Generate Doc
            RegisterAction(CreateOTACustomAction(MenuItemLocation.Child,
                                                 "NDocMenu",
                                                 "GenerateDocMenu",
                                                 0,
                                                 "Generate Doc ...",
                                                 new EventHandler(DoGenerateDoc)));
        }

        // *************************************************************************
        //                              Properties
        // *************************************************************************

		private BeWise.SharpBuilderTools.Tools.NDoc.NDoc CurrentNDoc {
            get {
                return (BeWise.SharpBuilderTools.Tools.NDoc.NDoc) CurrentTool;
            }
        }

    }
}
