using System;
using System.Windows.Forms;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Helpers;
using Borland.Studio.ToolsAPI;
using Lextm.OpenTools;

namespace Lextm.WiseEditor.Feature {
	/// <summary>
	/// Notification related feature.
	/// </summary>
	public class ExtraFeature : CustomFeature {
				
		private const string TabGeneral = "Extra";
	

		///<summary>
		///Registers tabs.
		///</summary>
		///<remarks>
		/// Used to configure tabs on FormPreferences.
		///</remarks>
		protected override void IdeRegisterTabs() {
			base.IdeRegisterTabs();
			RegisterTab(CreateTabNode(TabGeneral, typeof(Gui.NotificationPage)));
		}


        /**************************************************************/
		/*                     Constructor
		/**************************************************************/

		public ExtraFeature() {
			IOTAService _Service = OtaUtils.GetService();
            _Service.FileNotification += new FileNotificationHandler(SetActiveProjectFileNotificationHandler);
            _Service.BeforeCompile += new BeforeCompileHandler(BeforeCompileNotificationHandler);

        }

        /**************************************************************/
        /*                     Private
        /**************************************************************/

        //private static FileNotificationHandler fMenuShortCutNotification;
        private static Timer fTimerActiveProject = new Timer();


        private void BeforeCompileNotificationHandler(object aSender, BeforeCompileArgs aArgs) {
        	if ((bool)PropertyRegistry.Get("WarnOnCompileUnsavedFile", false)) {
                aArgs.Cancel = OtaUtils.WarnForUnsavedFiles(aArgs.Project, ShareUtils.MessageBoxTitle);
            }
        }

        private static void DoTimerActiveProject(object sender, EventArgs e) {
            try {
                fTimerActiveProject.Enabled = false;
                //IOTAProjectGroup _ProjectGroup = OTAUtils.GetCurrentProjectGroup();

//                if (_ProjectGroup != null && OptionManager.ProjectGroupOptions != null) {
//                    for (int i = 0; i < _ProjectGroup.ProjectCount; i ++) {
//                        IOTAProject _Project = _ProjectGroup[i];
// TODO: project option
//                        if (Path.GetFileName(_Project.FileName) == OptionManager.ProjectGroupOptions.DefaultActiveProject) {
//                            _Project.Activate();
//                        }
//                    }
//                }
            } catch (Exception _Exception) {
                Lextm.Windows.Forms.MessageBoxFactory.Fatal(_Exception);
            }
        }

        private static void SetActiveProjectFileNotificationHandler(object aSender, FileNotificationEventArgs aArgs) {
            try {
                // Notification for the Project Group
                if (OtaUtils.IsProjectGroupFile(aArgs.FileName) && aArgs.NotifyCode == OTAFileNotification.ofnFileOpened) {
                    fTimerActiveProject.Interval = 2000;
                    fTimerActiveProject.Tick += new EventHandler(DoTimerActiveProject);
                    fTimerActiveProject.Enabled = true;
                } else if (OtaUtils.IsProjectGroupFile(aArgs.FileName) && aArgs.NotifyCode == OTAFileNotification.ofnFileClosing) {
                    // Reset the configuration//TODO project option
                    //OptionManager.ResetProjectGroupOptions();
                } else if (OtaUtils.IsProjectFile(aArgs.FileName) && aArgs.NotifyCode == OTAFileNotification.ofnActiveProjectChanged) {
//TODO: move to NFamily somehow.
//                    Ant _Ant = Ant.getInstance();
//
//                    if (_Ant != null) {
//                        _Ant.ResetTargets();
//                    }
                }
            } catch (Exception e) {
                Lextm.Windows.Forms.MessageBoxFactory.Fatal(e);
            }
		}
    }
}
