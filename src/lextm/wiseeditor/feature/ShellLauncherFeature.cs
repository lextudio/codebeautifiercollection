using System;
using System.Diagnostics;
using System.IO;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Helpers;
using Borland.Studio.ToolsAPI;
using Lextm.OpenTools;

namespace Lextm.WiseEditor.Feature {
	
	public class ShellLauncherFeature : CustomFeature
	{
		#region actions
        private static void StartShell(string fileName, string param)
        {
            if (File.Exists(fileName))
            {
                Process.Start(fileName, param);
            }
            else
            {
                MessageService.Show("File does not exist: " + fileName);
            }
        }

		private static void StartShell(string workingDir) {
			Lextm.Diagnostics.ShellHelper.Execute(
				"cmd.exe", String.Empty, workingDir, false);
		}

		private void DoProcessChecker(object aSender, EventArgs AEventArgs) {
			string path = Lextm.OpenTools.IO.Path.GetBundledFile("ProcessChecker.exe");
            Lextm.Diagnostics.ShellHelper.Execute(path);
		}
		/**************************************************************/
		/*                        Public
        /**************************************************************/

		private void DoOpenEventViewer(object aSender, EventArgs AEventArgs) {
			StartShell(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "eventvwr.msc"), "/s");
		}

		private void DoOpenIISManager(object aSender, EventArgs AEventArgs) {
			Process.Start(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "inetsrv"), "iis.msc"));
		}

		private void DoOpenPerformance(object aSender, EventArgs AEventArgs) {
			StartShell(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "perfmon.msc"), "/s");
		}

		private void DoOpenServices(object aSender, EventArgs AEventArgs) {
			StartShell(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "services.msc"), "/s");
		}

		private void DoOpenShell(object aSender, EventArgs AEventArgs) {
			Process.Start(@"cmd.exe");
		}

		private void DoOpenShellInModuleFolder(object aSender, EventArgs AEventArgs) {
			if (!ValidationHelpers.ValidateCurrentModuleNotNull()) {
				return;
			}

			IOTAModule _Module = OtaUtils.GetCurrentModule();
			if (_Module.FileName == "default.htm") {
				return;
			}
			StartShell(Path.GetDirectoryName(_Module.FileName));
		}


		private void DoOpenShellInProjectFolder(object aSender, EventArgs AEventArgs) {
			if (!ValidationHelpers.ValidateCurrentProjectNotNull()) {
				return;
			}

			IOTAProject _Project = OtaUtils.GetCurrentProject();
			StartShell(Path.GetDirectoryName(_Project.FileName));
		}

		private void DoOpenShellInProjectTargetFolder(object aSender, EventArgs AEventArgs) {
			if (!ValidationHelpers.ValidateCurrentProjectTargetNotNull()) {
				return;
			}

			string _Target = OtaUtils.GetProjectTarget(OtaUtils.GetCurrentProject());
			StartShell(Path.GetDirectoryName(_Target));
		}

		private void DoOpenShellInProjectGroupFolder(object aSender, EventArgs AEventArgs) {
			if (!ValidationHelpers.ValidateCurrentProjectGroupNotNull()) {
				return;
			}

			IOTAProjectGroup _ProjectGroup = OtaUtils.GetCurrentProjectGroup();
			StartShell(Path.GetDirectoryName(_ProjectGroup.FileName));
		}

		private void DoOpenModuleFolder(object aSender, EventArgs AEventArgs) {
			if (!ValidationHelpers.ValidateCurrentModuleNotNull()) {
				return;
			}

			IOTAModule _Module = OtaUtils.GetCurrentModule();
			if (_Module.FileName == "default.htm") {
				// When it is startup page, skip.
				return;
			}
			Process.Start(Path.GetDirectoryName(_Module.FileName));
		}

		private void DoOpenProjectFolder(object aSender, EventArgs AEventArgs) {
			if (!ValidationHelpers.ValidateCurrentProjectNotNull()) {
				return;
			}

			IOTAProject _Project = OtaUtils.GetCurrentProject();
			Process.Start(Path.GetDirectoryName(_Project.FileName));
		}

		private void DoOpenProjectTargetFolder(object aSender, EventArgs AEventArgs) {
			if (!ValidationHelpers.ValidateCurrentProjectTargetNotNull()) {
				return;
			}

			string _Target = OtaUtils.GetProjectTarget(OtaUtils.GetCurrentProject());
			Process.Start(Path.GetDirectoryName(_Target));
		}

		private void DoOpenProjectGroupFolder(object aSender, EventArgs AEventArgs) {
			if (!ValidationHelpers.ValidateCurrentProjectGroupNotNull()) {
				return;
			}

			IOTAProjectGroup _ProjectGroup = OtaUtils.GetCurrentProjectGroup();
			Process.Start(Path.GetDirectoryName(_ProjectGroup.FileName));
		}
		#endregion
		/**************************************************************/
		/*                         Public
        /**************************************************************/

		protected override void IdeRegisterMenus() {
			// Shell Menu
			RegisterAction(CreateEmptyMenu(MenuItemLocation.Child,
                                           ShareUtils.MenuRootDefault,
			                               "ShellMenu",
			                               "Shell"
			                              ));
			// Open Shell Menu
			RegisterAction(CreateEmptyMenu(MenuItemLocation.Child,
										   "ShellMenu",
										   "OpenShellMenu",
			                               "Command Prompt"
			                              ));

			// Open Folder Menu
			RegisterAction(CreateEmptyMenu(MenuItemLocation.Child,
			                               "ShellMenu",
			                               "OpenFolderMenu",
			                               "Open Folder"));

			// Administration Menu
			RegisterAction(CreateEmptyMenu(MenuItemLocation.Child,
			                               "ShellMenu",
			                               "AdminToolsMenu",
			                               "Administration"));
			// Sep
			RegisterMenu(CreateSeparatorMenu(MenuItemLocation.Child,
			                                 "ShellMenu"));

			// Shell Project Group
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
												 "OpenShellMenu",
												 "OpenShellProjectGroupMenu",
												 0,
												 "Project Group Folder",
												 new EventHandler(DoOpenShellInProjectGroupFolder)));

			// Shell Project
                        RegisterAction(CreateActionMenu(MenuItemLocation.Child,
												 "OpenShellMenu",
												 "OpenShellProjectMenu",
												 0,
			                                     "Project Folder",
												 new EventHandler(DoOpenShellInProjectFolder)));

			// Shell Project Target
                        RegisterAction(CreateActionMenu(MenuItemLocation.Child,
												 "OpenShellMenu",
												 "OpenShellProjectTargetMenu",
												 0,
												 "Project Target Folder",
												 new EventHandler(DoOpenShellInProjectTargetFolder)));

			// Shell Module
                        RegisterAction(CreateActionMenu(MenuItemLocation.Child,
												 "OpenShellMenu",
												 "OpenShellModuleMenu",
												 0,
												 "Module Folder",
												 new EventHandler(DoOpenShellInModuleFolder)));

			// Shell Default
                        RegisterAction(CreateActionMenu(MenuItemLocation.Child,
												 "OpenShellMenu",
			                                     "OpenShellDefaultMenu",
			                                     0,
			                                     "Default",
			                                     new EventHandler(DoOpenShell)));

			// Open Project Group
                        RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                     "OpenFolderMenu",
			                                     "OpenProjectGroupFolderMenu",
			                                     0,
			                                     "Open Project Group Folder",
			                                     new EventHandler(DoOpenProjectGroupFolder)));

			// Open Project Folder
                        RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                     "OpenFolderMenu",
			                                     "OpenProjectFolderMenu",
			                                     0,
			                                     "Open Project Folder",
			                                     new EventHandler(DoOpenProjectFolder)));

			// Open Project Target Folder
                        RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                     "OpenFolderMenu",
			                                     "OpenProjectTargetFolderMenu",
			                                     0,
			                                     "Open Project Target Folder",
			                                     new EventHandler(DoOpenProjectTargetFolder)));

			// Open Module Folder
                        RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                     "OpenFolderMenu",
			                                     "OpenModuleFolderMenu",
			                                     0,
			                                     "Open Module Folder",
			                                     new EventHandler(DoOpenModuleFolder)));

			// Services
                        RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                     "AdminToolsMenu",
			                                     "ServicesMenu",
			                                     0,
			                                     "Services",
			                                     new EventHandler(DoOpenServices)));

			// Process Checker
                        RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                     "AdminToolsMenu",
												 "ProcessCheckerMenu",
			                                     0,
												 "Process Checker",
			                                     new EventHandler(DoProcessChecker)));

			// Performance
                        RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                     "AdminToolsMenu",
			                                     "PerformanceMenu",
			                                     0,
			                                     "Performance",
			                                     new EventHandler(DoOpenPerformance)));

			// IIS Manager
                        RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                     "AdminToolsMenu",
			                                     "IISManagerMenu",
			                                     0,
			                                     "Internet Information Services",
			                                     new EventHandler(DoOpenIISManager)));

			// Event Viewer
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                     "AdminToolsMenu",
			                                     "EventViewerMenu",
			                                     0,
			                                     "Event Viewer",
			                                     new EventHandler(DoOpenEventViewer)));
		}
	}
}
