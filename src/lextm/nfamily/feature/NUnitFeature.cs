using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Gui.NUnit;
using BeWise.SharpBuilderTools.Helpers;
using BeWise.SharpBuilderTools.Tools;
using BeWise.SharpBuilderTools.Wizard;
using Borland.Studio.ToolsAPI;
using Lextm.LeXDK;

namespace Lextm.NFamily.Feature {

    public class NUnitFeature : CustomFeatureTool {
		
		/// <summary>
		/// Constructor.
		/// </summary>
		public NUnitFeature() {}
		
		private const string TabNUnit = "NUnit";
		
		protected override void IDERegisterWizards()
		{
			base.IDERegisterWizards();
			
			IOTAWizardService service = OTAUtils.GetWizardService();			

            IOTAGalleryCategoryManager _GalleryCategoryManager = OTAUtils.GetGalleryCategoryManager();
            IOTAGalleryCategory _Cat = _GalleryCategoryManager.FindCategory(OTAGalleryCategories.sCategoryRoot);
            IOTAGalleryCategory _HelpersCat = _GalleryCategoryManager.AddCategory(_Cat, "#Helpers", "#Helpers", IntPtr.Zero);
            IOTAGalleryCategory _FileCat = _GalleryCategoryManager.AddCategory(_HelpersCat, "#File.Helpers", "#Files", IntPtr.Zero);
			// Test Fixture File
	        TestFixtureFileWizard _TestFixtureFileWizard = new TestFixtureFileWizard(_FileCat);
	        service.AddWizard(_TestFixtureFileWizard);
		}
		///<summary>
		///Registers tabs.
		///</summary>
		///<remarks>
		/// Used to configure tabs on FormPreferences.
		///</remarks>
		protected override void IDERegisterTabs() {
			base.IDERegisterTabs();
			RegisterTab(CreateTabNode(TabNUnit, typeof(Gui.NUnitPage)));
		}
		/**************************************************************/
        /*                        Private
        /**************************************************************/
        // NUnit
        //public string NUnitPath = String.Empty;
        //public bool ShowMessageOnTerminateNUnitConsole = false;
        
        
        private string fAssemblyToTest;
        private BeWise.SharpBuilderTools.Tools.NUnit.NUnit fCurrentNUnit;
        private string fStandardOut = String.Empty;
        //private string fStandardError = "";
        private string fTestFixtureToTest;

        private void OpenNUnitGui() {
            if(!ValidationHelpers.ValidateToolInstalled(CurrentTool)) {
                return;
            }


            AddMessage("==============================================================");
            AddMessage(String.Format("[{0}] Openning " + CurrentNUnit.Name,  DateTime.Now));
            AddMessage("Executable:  " + CurrentNUnit.ToolGuiExe);
            AddMessage("==============================================================");

            CurrentNUnit.OpenGui();
        }

        private void RunNUnitConsole() {
            if(!ValidationHelpers.ValidateToolInstalled(CurrentTool)) {
                return;
            }

            AddMessage("==============================================================");
            AddMessage(String.Format("[{0}] Running " + CurrentNUnit.Name,  DateTime.Now));
            AddMessage("Executable:  " + CurrentNUnit.ToolConsoleExe);
            AddMessage("Assembly: " + fAssemblyToTest);

            if (!Lextm.StringHelper.IsNullOrEmpty(fTestFixtureToTest)) {
                AddMessage("Test Fixture: " + fTestFixtureToTest);
            }

            AddMessage("==============================================================");

            Msg = "";

            CurrentNUnit.RunConsole(new object[] {Utils.QuoteString(fAssemblyToTest), fTestFixtureToTest}, "");
        }


        private void RunNUnitGui() {
            if(!ValidationHelpers.ValidateToolInstalled(CurrentTool)) {
                return;
            }

            AddMessage("==============================================================");
            AddMessage(String.Format("[{0}] Running " + CurrentNUnit.Name,  DateTime.Now));
            AddMessage("Executable:  " + CurrentNUnit.ToolGuiExe);
            AddMessage("Assembly: " + fAssemblyToTest);
            AddMessage("==============================================================");

            CurrentNUnit.RunGui(new object[] {Utils.QuoteString(fAssemblyToTest), ""}, "");
        }

        /**************************************************************/
        /*                       Protected
        /**************************************************************/

        protected override void AddMessage(string text) {
            fStandardOut += text;

            IOTAMessageService _MessageService = OTAUtils.GetMessageService();
            string _FileName = "";
            int _LineNumber = 0;
            int _ColNumber = 0;
            OTAMessageKind _OTAMessageKind = OTAMessageKind.otamkInfo;
            IntPtr _Out;

            _MessageService.ShowMessageView(null);

            Regex _Regex = new Regex(@"\s*(?<FileName>.+)\((?<Line>\d+),\s*(?<Col>\d+)\)\:",
                                     RegexOptions.IgnoreCase |
                                     RegexOptions.Multiline |
                                     RegexOptions.IgnorePatternWhitespace |
                                     RegexOptions.Compiled);

            Match _Match = _Regex.Match(text);

            if (_Match.Success) {
                _FileName = _Match.Groups[1].Value;
                _LineNumber = int.Parse(_Match.Groups[2].Value);
                _ColNumber = int.Parse(_Match.Groups[3].Value);
            }

            _MessageService.AddCompilerMessage(_FileName,
                                               text,
                                               CurrentNUnit.Name,
                                               _OTAMessageKind,
                                               _LineNumber,
                                               _ColNumber,
                                               new IntPtr(0),
                                               out _Out);
        }

		private void DoOpenNUnitGui(object aSender, EventArgs aEventArgs) {
            Thread _T = new Thread(new ThreadStart(OpenNUnitGui));
            _T.Start();
        }

		private void DoNUnitConsole(object aSender, EventArgs aEventArgs) {
            if(!ValidationHelpers.ValidateCurrentProjectNotNull() ||
                    !ValidationHelpers.ValidateCurrentProjectTargetExists()) {
                return;
            }

            fStandardOut = "";
            fAssemblyToTest = ProjectTargetName;
            fTestFixtureToTest = "";

            Thread _T = new Thread(new ThreadStart(RunNUnitConsole));
            _T.Start();
        }

		private void DoNUnitConsoleOnProjectGroupAssemblies(object aSender, EventArgs aEventArgs) {
            if(!ValidationHelpers.ValidateCurrentProjectGroupNotNull()) {
                return;
            }

            FrmNUnitRunner _Frm = new FrmNUnitRunner();

            if (_Frm.ShowDialog() == DialogResult.OK && _Frm.SelectedAssemblyName != "") {
                fAssemblyToTest = _Frm.SelectedAssemblyName;
                fTestFixtureToTest = "";

                Thread _T = new Thread(new ThreadStart(RunNUnitConsole));
                _T.Start();
            }
        }

		private void DoNUnitGuiWithProjectTarget(object aSender, EventArgs aEventArgs) {
            if(!ValidationHelpers.ValidateCurrentProjectNotNull() ||
               !ValidationHelpers.ValidateCurrentProjectTargetExists()) {
                return;
            }

            fAssemblyToTest = ProjectTargetName;
            fTestFixtureToTest = "";

            Thread _T = new Thread(new ThreadStart(RunNUnitGui));
            _T.Start();
        }

        private void DoOpenNUnitProject(object aSender, EventArgs aEventArgs) {
            string _Filename = GetToolFileInCurrentProject();

            if (_Filename == null || !ValidationHelpers.ValidateToolInstalled(CurrentNUnit)) {
                return;
            }


            AddMessage("==============================================================");
            AddMessage(String.Format("[{0}] Openning " + CurrentNUnit.Name,  DateTime.Now));
            AddMessage("Project:  " + _Filename);
            AddMessage("==============================================================");

            CurrentNUnit.OpenProjectGui(Utils.QuoteString(_Filename), "");
        }

		private void DoNUnitWizard(object aSender, EventArgs aEventArgs) {
            if(!ValidationHelpers.ValidateCurrentProjectNotNull()) {
                return;
            }
			if (!ValidationHelpers.ValidateCurrentProjectIsCSharp())
            {
                return;
            }
            FrmNUnitWizard _Frm = new FrmNUnitWizard();
            _Frm.ShowDialog();
        }

		protected override void DoRunCompleted(object aSender, EventArgs e) {
            DoRefreshConsole(String.Format("Done! ({0})\n", DateTime.Now));

//            if (OptionManager.Configuration.ShowMessageOnTerminateNUnitConsole) {
//                ValidationHelpers.ShowInformation("Run Tests Completed");
//            }
        }

        protected override BaseTool GetCurrentTool() {
            if (fCurrentNUnit == null) {
                fCurrentNUnit = new BeWise.SharpBuilderTools.Tools.NUnit.NUnit();
                fCurrentNUnit.OnConsoleOutput += new RunProcess.ConsoleOutputEvent(DoRefreshConsole);
                fCurrentNUnit.OnRunCompleted += new EventHandler(DoRunCompleted);
            }

            return fCurrentNUnit;
        }

        /**************************************************************/
        /*                       Public
        /**************************************************************/
		private const string MenuLeXtudio = "CBCLeXtudioMenu";
		
        protected override void IDERegisterMenus() {
            // NUnit
            RegisterAction(CreateOTACustomAction(MenuItemLocation.Child,
                                                 MenuLeXtudio,
                                                 "NUnitMenu",
                                                 0,
                                                 "NUnit",
                                                 null));

            // NUnit Wizard
            RegisterAction(CreateOTACustomAction(MenuItemLocation.Child,
                                                 "NUnitMenu",
                                                 "NUnitWizardMenu",
                                                 0,
                                                 "New " + CurrentNUnit.Name + " Test Fixture...",
                                                 new EventHandler(DoNUnitWizard)));

            // Sep
            RegisterAction(CreateSeparatorMenu(MenuItemLocation.Child,
                                                  "NUnitMenu"));

            // NUnit Gui on Assembly
            RegisterAction(CreateOTACustomAction(MenuItemLocation.Child,
                                                 "NUnitMenu",
                                                 "NUnitGuiWithAssemblyMenu",
                                                 0,
                                                 "Run " + CurrentNUnit.Name + " (Gui) on Project Target...",
                                                 new EventHandler(DoNUnitGuiWithProjectTarget)));

            // NUnit Gui
            RegisterAction(CreateOTACustomAction(MenuItemLocation.Child,
                                                 "NUnitMenu",
                                                 "NUnitGuiMenu",
                                                 0,
                                                 "Start " + CurrentNUnit.Name + " (Gui)...",
                                                 new EventHandler(DoOpenNUnitGui)));

            // Sep
            RegisterAction(CreateSeparatorMenu(MenuItemLocation.Child,
                                                  "NUnitMenu"));

            // NUnit Console (Select Assembly)
            RegisterAction(CreateOTACustomAction(MenuItemLocation.Child,
                                                 "NUnitMenu",
                                                 "NUnitConsoleProjectGroupAssemblyMenu",
                                                 0,
                                                 "Run " + CurrentNUnit.Name + " (Console) on Project Group Assemblies",
                                                 new EventHandler(DoNUnitConsoleOnProjectGroupAssemblies)));

            // NUnit Console
            RegisterAction(CreateOTACustomAction(MenuItemLocation.Child,
                                                 "NUnitMenu",
                                                 "NUnitConsoleMenu",
                                                 0,
                                                 "Run " + CurrentNUnit.Name + " (Console) on Project Target",
                                                 new EventHandler(DoNUnitConsole)));

            // Sep
            RegisterAction(CreateSeparatorMenu(MenuItemLocation.Child,
                                                  "NUnitMenu"));

            // NUnit Runner
//            RegisterAction(CreateOTACustomAction(OTAMenuItemLocation.otamlChild,
//                                                 "NUnitMenu",
//                                                 "NUnitTestFixtureConsoleMenu",
//                                                 0,
//                                                 CurrentNUnit.Name + " Runner...",
//                                                 new EventHandler(DoNUnitRunner)));


            // Last NUnit Runner
//            RegisterAction(CreateOTACustomAction(OTAMenuItemLocation.otamlChild,
//                                                 "NUnitMenu",
//                                                 "NUnitLastTestFixtureConsoleMenu",
//                                                 0,
//                                                 "Last " + CurrentNUnit.Name + " Runner...",
//                                                 new EventHandler(DoLastNUnitRunner)));


            // Open NUnit Project
            RegisterAction(CreateOTACustomAction(MenuItemLocation.Child,
                                                 "NUnitMenu",
                                                 "OpenNUnitProjectMenu",
                                                 0,
                                                 "Open " + CurrentNUnit.Name + " Project (Gui)...",
                                                 new EventHandler(DoOpenNUnitProject)));


        }

        // *************************************************************************
        //                            Properties
        // *************************************************************************

		private BeWise.SharpBuilderTools.Tools.NUnit.NUnit CurrentNUnit {
            get {
                return (BeWise.SharpBuilderTools.Tools.NUnit.NUnit) CurrentTool;
            }
        }

		private string ProjectTargetName {
            get {
                IOTAProjectGroup _ProjectGroup = OTAUtils.GetCurrentProjectGroup();

                if (_ProjectGroup != null) {
                    IOTAProject _Project = _ProjectGroup.ActiveProject;

                    if (_Project != null) {
                        String _TargetName = _Project.ProjectOptions.TargetName;

						if (!Lextm.StringHelper.IsNullOrEmpty(_TargetName) ||
								File.Exists(_TargetName)) {
                            return _TargetName;
                        } else {
                        	Lextm.Windows.Forms.MessageBoxFactory.Warn("No target assembly found in the active project!");//, Lextm.LeXDK.SharedNames.MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }

                Lextm.Windows.Forms.MessageBoxFactory.Warn("No active project found!");//, SharedNames.MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);

                return null;
            }
        }
							        
    }
}
