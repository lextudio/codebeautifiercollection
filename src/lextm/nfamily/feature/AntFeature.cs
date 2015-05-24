using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Gui.Ant;
using BeWise.SharpBuilderTools.Helpers;
using BeWise.SharpBuilderTools.Tools;
using BeWise.SharpBuilderTools.Tools.Ant;
using Borland.Studio.ToolsAPI;
using Lextm.LeXDK;

namespace Lextm.NFamily.Feature {
	/// <summary>
	/// Ant feature.
	/// </summary>
	public class AntFeature : CustomFeatureTool {
		
		/// <summary>
		/// Constructor.
		/// </summary>
		public AntFeature() {}
		
		private const string TabAnt = "Ant/NAnt";
		
		///<summary>
		///Registers tabs.
		///</summary>
		///<remarks>
		/// Used to configure tabs on FormPreferences.
		///</remarks>
		protected override void IDERegisterTabs() {
			base.IDERegisterTabs();
			RegisterTab(CreateTabNode(TabAnt, typeof(Gui.AntPage)));
		}
		
        private int fErrorCount;
        private BaseAnt fCurrentAnt = null;
        private FrmNAntMessage fFrmNAntMessage = null;
        private String[] fSelectedTargets;
        private ViewAntMode fLastViewAntMode;
        private int fWarningCount;

        private void DoView(ViewAntMode aViewAntMode) {
            ViewAntProjectStructure(aViewAntMode, null);
        }

        private bool FindCompilationErrorAndWarning(string aText,
                                                    out string aFileName,
                                                    out int aLineNumber,
                                                    out int aColNumber,
                                                    out OTAMessageKind aOTAMessageKind){
            string _RegExpString;

            // Init output
            aFileName = "";
            aLineNumber = 0;
            aColNumber = 0;
            aOTAMessageKind = OTAMessageKind.otamkInfo;

            if ((bool)PropertyRegistry.Get("UseAnt", false)) {
                _RegExpString = @"\[csc\]\s*(?<FileName>.+)\((?<Line>\d+),\s*(?<Col>\d+)\)\:";
            } else {
                _RegExpString = @"\s*(?<FileName>.+)\((?<Line>\d+),\s*(?<Col>\d+)\)\:";
            }

            Regex _Regex = new Regex(_RegExpString,
                                     RegexOptions.IgnoreCase |
                                     RegexOptions.Multiline |
                                     RegexOptions.IgnorePatternWhitespace |
                                     RegexOptions.Compiled);

            Match _Match = _Regex.Match(aText);

            if (_Match.Success) {
                aFileName = _Match.Groups[1].Value;
                aLineNumber = int.Parse(_Match.Groups[2].Value);
                aColNumber = int.Parse(_Match.Groups[3].Value);

                if (aText.IndexOf("error") > -1) {
                    aOTAMessageKind = OTAMessageKind.otamkError;

                    fErrorCount++;
                } else if (aText.IndexOf("warning") > -1) {
                    aOTAMessageKind = OTAMessageKind.otamkWarn;

                    fWarningCount++;
                }

                return true;
            }

            return false;
        }

        private bool FindInternalError(string aText,
                                       out OTAMessageKind aOTAMessageKind){
            string _RegExpString;

            // Init output
            aOTAMessageKind = OTAMessageKind.otamkInfo;
            
            if ((bool)PropertyRegistry.Get("UseAnt", false)) {
                _RegExpString = @"\[csc\]\serror\sCS";
            } else {
                _RegExpString = @"error\sCS";
            }

            Regex _Regex = new Regex(_RegExpString,
                                     RegexOptions.IgnoreCase |
                                     RegexOptions.Multiline |
                                     RegexOptions.IgnorePatternWhitespace |
                                     RegexOptions.Compiled);

            Match _Match = _Regex.Match(aText);

            if (_Match.Success) {
                aOTAMessageKind = OTAMessageKind.otamkError;
                fErrorCount++;

                return true;
            }

            return false;
        }

        private BaseAntProject GetAntProject() {
            // Validations
            if (!ValidationHelpers.ValidateCurrentProjectNotNull()) {
                return null;
            }

            IOTAProjectGroup _ProjectGroup = OTAUtils.GetCurrentProjectGroup();
            IOTAProject _Project = _ProjectGroup.ActiveProject;

            for (int i = 0; i < _Project.ModuleCount; i++) {
                IOTAModuleInfo _ModuleInfo = _Project.GetModuleInfo(i);

                if (!Lextm.StringHelper.IsNullOrEmpty(_ModuleInfo.FileName) && CurrentAnt.IsToolFile(_ModuleInfo.FileName) && File.Exists(_ModuleInfo.FileName)) {
                    return CurrentAnt.CreateAntProject(_ModuleInfo.FileName);
                }
            }

            ValidationHelpers.ShowWarning("No " + CurrentAnt.Name  + " file found in the active project !");
            return null;
        }

        private void RunAnt() {
            // Validations
            if (!ValidationHelpers.ValidateToolNotRunning(this) ||
                    !ValidationHelpers.ValidateToolInstalled(CurrentAnt)) {
                return;
            }

//            if (OptionManager.Configuration.WarnOnRunAntUnsavedFile) {
//                if (OTAUtils.WarnForUnsavedFiles(OTAUtils.GetCurrentProject(), Consts.MESSAGE_BOX_TITLE)) {
//                    return;
//                }
//            }

            IOTAMessageService _MessageService = OTAUtils.GetMessageService();
            _MessageService.ShowMessageView(null);

//            if (OptionManager.Configuration.ClearMessagesBeforeRunningAnt) {
//                _MessageService.ClearCompilerMessages();
//            }

            AddMessage("==============================================================");
            AddMessage(String.Format("[{0}] Running {1} ({2})", DateTime.Now,
                                                                CurrentAnt.Name,
                                                                BaseAnt.TargetsToString(fSelectedTargets)));
            AddMessage("==============================================================");

            Msg = "";
            Running = true;
            fErrorCount = 0;
            fWarningCount = 0;

            CurrentAnt.RunConsole(fSelectedTargets, Path.GetDirectoryName(AntProject.FileName));
        }

        /**************************************************************/
        /*                     Protected
        /**************************************************************/

        protected override void AddMessage(string text) {
            IOTAMessageService _MessageService = OTAUtils.GetMessageService();
            string _FileName = "";
            int _LineNumber = 0;
            int _ColNumber = 0;
            OTAMessageKind _OTAMessageKind = OTAMessageKind.otamkInfo;
            IntPtr _Out;

            _MessageService.ShowMessageView(null);

            if (!FindCompilationErrorAndWarning(text,
                                                out _FileName,
                                                out _LineNumber,
                                                out _ColNumber,
                                                out _OTAMessageKind)) {

                FindInternalError(text,
                                  out _OTAMessageKind);
            }

            _MessageService.AddCompilerMessage(_FileName,
                                               text,
                                               CurrentAnt.Name,
                                               _OTAMessageKind,
                                               _LineNumber,
                                               _ColNumber,
                                               new IntPtr(0),
                                               out _Out);
        }

		private void DoRunLastAntTargets(object aSender, EventArgs aEventArgs) {
            if (fSelectedTargets != null && fSelectedTargets.Length > 0) {
                Thread _T = new Thread(new ThreadStart(RunAnt));
                _T.Start();
            } else {
                DoRunSingleAntTarget(aSender, aEventArgs);
            }
        }

		private void DoRunMultipleAntTargets(object aSender, EventArgs aEventArgs) {
            FrmNAntMultipleRunner _Frm = new FrmNAntMultipleRunner();

            if (AntProject != null) {
                _Frm.Project = AntProject;
                _Frm.CurrentAnt = CurrentAnt;

                if (_Frm.ShowDialog() == DialogResult.OK) {
                    fSelectedTargets = _Frm.SelectedTargets;

                    Thread _T = new Thread(new ThreadStart(RunAnt));
                    _T.Start();
                }
            }
        }

		private void DoRunSingleAntTarget(object aSender, EventArgs aEventArgs) {
            FrmNAntRunner _Frm = new FrmNAntRunner();

            if (AntProject != null) {
                _Frm.Project = AntProject;
                _Frm.CurrentAnt = CurrentAnt;

                if (_Frm.ShowDialog() == DialogResult.OK) {
                    fSelectedTargets = new String[] {_Frm.SelectedTarget};

                    Thread _T = new Thread(new ThreadStart(RunAnt));
                    _T.Start();
                }
            }
        }

		protected override void DoRunCompleted(object aSender, EventArgs e) {
            Running = false;

            DoRefreshConsole(String.Format("Done! ({0})\n", DateTime.Now));

            fFrmNAntMessage = new FrmNAntMessage(CurrentAnt.Name,
                                                 AntProject.FileName,
                                                 fErrorCount,
                                                 fWarningCount,
                                                 fSelectedTargets);

//            if (OptionManager.Configuration.ShowMessageOnTerminateAnt) {
//                ShowAntResults();
//            }
        }

		private void DoShowLastAntResults(object aSender, EventArgs AEventArgs) {
            ShowAntResults();
        }

		private void DoViewAntFile(object aSender, EventArgs AEventArgs) {
            DoView(ViewAntMode.All);
        }

		private void DoViewAntProperties(object aSender, EventArgs AEventArgs) {
            DoView(ViewAntMode.Properties);
        }

		private void DoViewAntTargets(object aSender, EventArgs AEventArgs) {
            DoView(ViewAntMode.Targets);
        }

		private void DoViewLast(object aSender, EventArgs AEventArgs) {
            DoView(fLastViewAntMode);
        }

        protected override BaseTool GetCurrentTool() {
            if ((bool)PropertyRegistry.Get("UseAnt", false)) {
                if (fCurrentAnt == null || fCurrentAnt is NAnt) {
                    fCurrentAnt = new Ant();
                    fCurrentAnt.OnConsoleOutput += new RunProcess.ConsoleOutputEvent(DoRefreshConsole);
                    fCurrentAnt.OnRunCompleted += new EventHandler(DoRunCompleted);
                }
        	} else {
        		if (fCurrentAnt == null || fCurrentAnt is BeWise.SharpBuilderTools.Tools.Ant.Ant) {
                    fCurrentAnt = new NAnt();
                    fCurrentAnt.OnConsoleOutput += new RunProcess.ConsoleOutputEvent(DoRefreshConsole);
                    fCurrentAnt.OnRunCompleted += new EventHandler(DoRunCompleted);
                }
            }

            return fCurrentAnt;
        }

        private void ShowAntResults() {
            if (fFrmNAntMessage == null) {
                Lextm.Windows.Forms.MessageBoxFactory.Info(String.Format("No {0} results available!", CurrentAnt.Name));
                return;
            }

            fFrmNAntMessage.ShowDialog();
        }

        /**************************************************************/
        /*                        Public
        /**************************************************************/
		private const string MenuLeXtudio = "CBCLeXtudioMenu";
		
		private const string MenuAnt = "AntMenu";
		/// <summary>
		/// Registers menus.
		/// </summary>
        protected override void IDERegisterMenus() {
            // Ant
            RegisterMenu(CreateActionMenu(MenuItemLocation.Child,
                                                 MenuLeXtudio,
                                                 MenuAnt,
                                                 0,
                                                 CurrentAnt.Name,
                                                 null));

            // Separator
            RegisterMenu(CreateSeparatorMenu(MenuItemLocation.Child,
                                                  MenuAnt));

            // Single Ant
            RegisterMenu(CreateActionMenu(MenuItemLocation.Child,
                                                 MenuAnt,
                                                 "SingleAntMenu",
                                                 16505, // Ctrl F10
                                                 "Run " + CurrentAnt.Name + " Target...",
                                                 new EventHandler(DoRunSingleAntTarget)));

            // Multiple Ant
            RegisterMenu(CreateActionMenu(MenuItemLocation.Child,
                                                 MenuAnt,
                                                 "MultipleAntMenu",
                                                 8313, // Shift F10
                                                 "Run Multiple " + CurrentAnt.Name + " Target...",
                                                 new EventHandler(DoRunMultipleAntTargets)));

            // Last Ant
            RegisterMenu(CreateActionMenu(MenuItemLocation.Child,
                                                 MenuAnt,
                                                 "LastAntMenu",
                                                 121, // F10
                                                 "Run Last " + CurrentAnt.Name + " Target...",
                                                 new EventHandler(DoRunLastAntTargets)));

            // Last Ant Result
            RegisterMenu(CreateActionMenu(MenuItemLocation.Child,
                                                 MenuAnt,
                                                 "ShowLastAntResultMenu",
                                                 0, // F10
                                                 "Show Last " + CurrentAnt.Name + " Result...",
                                                 new EventHandler(DoShowLastAntResults)));

            // Separator
            RegisterMenu(CreateSeparatorMenu(MenuItemLocation.Child,
                                                  MenuAnt));

            // Properties
            RegisterMenu(CreateActionMenu(MenuItemLocation.Child,
                                                 MenuAnt,
                                                 "ViewAntPropertiesMenu",
                                                 0,
                                                 "View " + CurrentAnt.Name + " Properties...",
                                                 new EventHandler(DoViewAntProperties)));

            // Targets
            RegisterMenu(CreateActionMenu(MenuItemLocation.Child,
                                                 MenuAnt,
                                                 "ViewAntTargetsMenu",
                                                 0,
                                                 "View " + CurrentAnt.Name + " Targets...",
                                                 new EventHandler(DoViewAntTargets)));

            // All
            RegisterMenu(CreateActionMenu(MenuItemLocation.Child,
                                                 MenuAnt,
                                                 "ViewAntFileMenu",
                                                 0,
                                                 "View " + CurrentAnt.Name + " file...",
                                                 new EventHandler(DoViewAntFile)));

            // Ant Structure
            RegisterMenu(CreateActionMenu(MenuItemLocation.Child,
                                                 MenuAnt,
                                                 "ViewAntStructureMenu",
                                                 0,
                                                 "View Last " + CurrentAnt.Name + " Structure...",
                                                 new EventHandler(DoViewLast)));

            // Separator
            RegisterMenu(CreateSeparatorMenu(MenuItemLocation.Child,
                                                  MenuAnt));

        }
		/// <summary>
		/// Refreshes preferences.
		/// </summary>
        public override void RefreshPreferences() {
            IOTAMainMenuService _MenuService = OTAUtils.GetMainMenuService();
            IOTAMenuItem _MenuItem = null;

            _MenuItem = _MenuService.GetMenuItem(MenuAnt);

            if (_MenuItem != null) {
                _MenuItem.Text = CurrentAnt.Name;
            }

            _MenuItem = _MenuService.GetMenuItem("SingleAntMenu");

            if (_MenuItem != null) {
                _MenuItem.Text = "Run " + CurrentAnt.Name + " Target...";
            }

            _MenuItem = _MenuService.GetMenuItem("MultipleAntMenu");

            if (_MenuItem != null) {
                _MenuItem.Text = "Run Multiple " + CurrentAnt.Name + " Target...";
            }

            _MenuItem = _MenuService.GetMenuItem("LastAntMenu");

            if (_MenuItem != null) {
                _MenuItem.Text = "Run Last " + CurrentAnt.Name + " Target...";
            }

            _MenuItem = _MenuService.GetMenuItem("ViewAntPropertiesMenu");

            if (_MenuItem != null) {
                _MenuItem.Text = "View " + CurrentAnt.Name + " Properties...";
            }

            _MenuItem = _MenuService.GetMenuItem("ViewAntTargetsMenu");

            if (_MenuItem != null) {
                _MenuItem.Text = "View " + CurrentAnt.Name + " Targets...";
            }

            _MenuItem = _MenuService.GetMenuItem("ViewAntFileMenu");

            if (_MenuItem != null) {
                _MenuItem.Text = "View " + CurrentAnt.Name + " File...";
            }

            _MenuItem = _MenuService.GetMenuItem("ViewAntStructureMenu");

            if (_MenuItem != null) {
                _MenuItem.Text = "View Last " + CurrentAnt.Name + " Info...";
            }
            
            _MenuItem = _MenuService.GetMenuItem("ShowLastAntResultMenu");
            
            if (_MenuItem != null) {
            	_MenuItem.Text = "Show Last " + CurrentAnt.Name + " Result...";
            }
        }
		/// <summary>
		/// Resets targets.
		/// </summary>
		private void ResetTargets() {
            fSelectedTargets = null;
        }
		/// <summary>
		/// View Ant project structure.
		/// </summary>
		/// <param name="viewAntMode">Mode</param>
		/// <param name="project">Project</param>
		private void ViewAntProjectStructure(ViewAntMode viewAntMode, BaseAntProject project) {
            if (project == null) {
                project = GetAntProject();
            }

            if (project != null) {
                FrmViewAnt _Frm = new FrmViewAnt(project, CurrentAnt);
                _Frm.ViewAntMode = viewAntMode;
                _Frm.ShowDialog();
                fLastViewAntMode = _Frm.ViewAntMode;
            }
        }

        // *************************************************************************
        //                            Properties
        // *************************************************************************
		/// <summary>
		/// Ant project.
		/// </summary>
		private BaseAntProject AntProject {
            get {
                return GetAntProject();
            }
        }
		/// <summary>
		/// Current Ant.
		/// </summary>
        private BaseAnt CurrentAnt {
            get {
                return (BaseAnt) CurrentTool;
            }
        }

    }
}
