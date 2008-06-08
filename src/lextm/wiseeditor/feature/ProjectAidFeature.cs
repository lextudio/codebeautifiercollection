using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Gui;
using BeWise.SharpBuilderTools.Helpers;
using BeWise.SharpBuilderTools.Info;
using Borland.Studio.ToolsAPI;
using Lextm.Diagnostics;
using Lextm.OpenTools;
using Lextm.Windows.Forms;
using Lextm.WiseEditor.ProjectAid;

namespace Lextm.WiseEditor.Feature {

	public class ProjectAidFeature : CustomFeatureTool {
		
		const string Name = "Project Aid";
		
		public ProjectAidFeature() {
			IOTAService _Service = OtaUtils.GetService();
			_Service.FileNotification += new FileNotificationHandler(OpenCloseFileNotificationHandler);
		}

		/**************************************************************/
		/*                     Private
        /**************************************************************/
		#region actions
		//        private string fLastPath;

		//private void GetFilesFromDirectory(ArrayList aFileList,  string aDir, string aPatern, bool aRecursive) {
		//    if (aRecursive) {
		//        string[] _DirList = Directory.GetDirectories(aDir);

		//        foreach (string _Dir in _DirList) {
		//            GetFilesFromDirectory(aFileList, _Dir, aPatern, aRecursive);
		//        }
		//    }

		//    string[] _FileList = Directory.GetFiles(aDir, aPatern);

		//    foreach (string _File in _FileList) {
		//        aFileList.Add(_File);
		//    }
		//}

		private static void ExternalViewFile(string aViewerFileName, string aFileName) {
			// Create the process
			ProcessStartInfo _ProcessStartInfo = new ProcessStartInfo();

			_ProcessStartInfo.FileName = aViewerFileName;
			_ProcessStartInfo.Arguments = aFileName;

			System.Diagnostics.Process.Start(_ProcessStartInfo);
		}

		private void DoSignProjectTarget(object sender, EventArgs e)
		{
			IOTAProject proj = OtaUtils.GetCurrentProject();
			if (proj == null)
			{
				MessageService.Show("null project.");
				return;
			}
			if (!OtaUtils.IsDotNetProject(proj))
			{
				MessageService.Show("This operation is only valid for .NET projects.");
				return;
			}
			IHandler handler = HandlerFactory.GetHandlerFor(proj);
			if (handler is Manner20UnsignedHandler && MessageBoxFactory.Confirm(Name, "Do you want to sign this project?", "Signing your projects is recommended by .NET Design Guidelines") == DialogResult.No)
			{
				return;
			}
			handler.Handle();
		}

		bool KeyFileExistInProject(IOTAProject project)
		{
			for (int i = 0; i < project.ModuleCount; i++)
			{
				if (project.GetModuleFileName(i).EndsWith(".snk") || project.GetModuleFileName(i).EndsWith(".pfx"))
				{
					return true;
				}
			}
			return false;
		}
		
		#region activate needed
		private static void OpenCloseFileNotificationHandler(object sender, FileNotificationEventArgs e)
		{
			try {
				LoggingService.Info("file opening is " + e.FileName);
//				if (!OTAUtils.IsSourceFile(e.FileName)) {
//					LoggingService.Warn("not a source file.");
//					return;
//				}
				if (e.NotifyCode == OTAFileNotification.ofnFileOpened) {
					
					IOTAModule _Module = OtaUtils.GetModuleServices().FindModule(e.FileName);
					
					if (_Module != null) {
						IOTASourceEditor _Editor = OtaUtils.GetSourceEditor(_Module);

						if (_Editor != null) {
							_Editor.ViewActivated -= new ViewActivatedEventHandler(ActivateProjectFromFile);
							_Editor.ViewActivated += new ViewActivatedEventHandler(ActivateProjectFromFile);
							LoggingService.Info("view activated added");
						} else {
							LoggingService.Warn("view activated not added");
						}
					} else {
						LoggingService.Warn("null module");
					}
				} else if (e.NotifyCode == OTAFileNotification.ofnFileClosing) {
					IOTAModule _Module = OtaUtils.GetModuleServices().FindModule(e.FileName);

					if (_Module != null) {
						IOTASourceEditor _Editor = OtaUtils.GetSourceEditor(_Module);

						if (_Editor != null) {
							_Editor.ViewActivated -= new ViewActivatedEventHandler(ActivateProjectFromFile);
							LoggingService.Info("view activated removed");
						}
						else {
							LoggingService.Warn("null editor");
						}
					}
				}
			} catch (Exception ex) {
				Lextm.Windows.Forms.MessageBoxFactory.Fatal(Name, ex);
			}
		}
		#endregion

		private static void ActivateProjectFromFile(object sender, ViewActivatedEventArgs e) {
			if (!(bool)PropertyRegistry.Get("AutoActivateProject", true)) {
				return;
			}

			if (!ValidationHelpers.ValidateCurrentProjectGroupNotNull() || !ValidationHelpers.ValidateCurrentModuleNotNull()) {
				return;
			}
			
			string fileName = OtaUtils.GetCurrentEditorFileName();
			IOTAProject active = OtaUtils.GetCurrentProject();
			if (active.FileInProject(fileName)) {
				return;
			}

			IOTAModuleServices ms = OtaUtils.GetModuleServices();
			if (ms != null)
			{
				IOTAProject proj = ms.FindProjectFromFileNameEntry(fileName);
				if (proj != null)
				{
					proj.Activate();
				}
			} else {
				LoggingService.Warn("Current file not found in any project!");
			}
		}

		private void DoCopyFile(object aSender, EventArgs AEventArgs) {
			if (!ValidationHelpers.ValidateCurrentProjectNotNull() ||
			    !ValidationHelpers.ValidateCurrentEditorNotNull()) {
				return;
			}

			string _FileName = OtaUtils.GetCurrentEditorFileName();

			if (!ValidationHelpers.ValidateCustom(File.Exists(_FileName), "File does not exists !")) {
				return;
			}

			FrmCopyFileName _Frm = new FrmCopyFileName(_FileName);
			_Frm.Directory = Path.GetDirectoryName(_FileName);

			if (_Frm.ShowDialog() == DialogResult.OK) {
				string _NewFileName = Path.Combine(_Frm.Directory, _Frm.FileName);
				string _Ext = Path.GetExtension(_FileName);

				_NewFileName = Lextm.IO.FileHelper.AddExtension(_NewFileName, _Ext);

				if (File.Exists(_NewFileName)) {
					ValidationHelpers.ShowWarning("Destination filename already exists, select a new name");
					DoCopyFile(null, null);
				} else {
					File.Copy(_FileName, _NewFileName);

					if (_Frm.chkMakeFileWritable.Checked) {
						Lextm.IO.FileHelper.SetReadOnlyFile(_NewFileName, false);
					}

					IOTAProject _Project = OtaUtils.GetCurrentProject();
					_Project.AddFile(_NewFileName);

					IOTAModuleInfo _ModuleInfo = OtaUtils.GetModuleInfoFromProject(_Project, _NewFileName);

					if (_ModuleInfo != null) {
						IOTAModule _Module = _ModuleInfo.OpenModule();

						if (_Module != null) {
							_Module.ShowFileName(_ModuleInfo.FileName);

							_Module.Show();

						}
					}
				}
			}
		}

		private void DoDeleteCurrentModule(object aSender, EventArgs AEventArgs) {
			if (!ValidationHelpers.ValidateCurrentModuleNotNull() ||
			    !ValidationHelpers.ValidateCustom(OtaUtils.CurrentModuleIsCSFile(), "Can only delete C# files !")) {
				return;
			}

			IOTAModule _Module = OtaUtils.GetCurrentModule();
			string _FileName = _Module.FileName;

			if (MessageBoxFactory.Confirm(Name, "Do you really want to delete the file ?", _FileName) == DialogResult.Yes) {
				_Module.Close(false);

				string _ResourceFileName = Path.ChangeExtension(_FileName, ".resx");

				if (File.Exists(_ResourceFileName)) {
					File.Delete(_ResourceFileName);
				}

				File.Delete(_FileName);
			}
		}
		
		private void DoCleanProjectFolder(object aSender, EventArgs AEventArgs) {
			string path = Lextm.OpenTools.IO.Path.GetBundledFile("Clearer.exe");
			Lextm.Diagnostics.ShellHelper.Execute(path);
		}

		private void DoILMergeProject(object sender, EventArgs e) {
			if (!ValidationHelpers.ValidateCurrentProjectNotNull()) {
				return;
			}
			string file = Path.Combine(Lextm.OpenTools.IO.Path.BundledFolder,
			                           "DILMerge.exe");
			Lextm.Diagnostics.ShellHelper.Execute(file,
			                                      Lextm.StringHelper.QuoteString(OtaUtils.GetCurrentProject().FileName));
		}

		private void DoDeleteProjectTarget(object aSender, EventArgs AEventArgs) {
			if (!ValidationHelpers.ValidateCurrentProjectNotNull() ||
			    !ValidationHelpers.ValidateCurrentProjectTargetExists()) {
				return;
			}

			string _Target = OtaUtils.GetProjectTarget(OtaUtils.GetCurrentProject());

			if (MessageBoxFactory.Confirm(Name, "Do you really want to delete the target ?", _Target) == DialogResult.Yes) {
				File.Delete(_Target);
			}
		}

		private void DoGetFileProperties(object aSender, EventArgs AEventArgs) {
			if (!ValidationHelpers.ValidateCurrentEditorNotNull()) {
				return;
			}
			string _FileName = OtaUtils.GetCurrentEditorFileName();

			if (!File.Exists(_FileName)) {
				Lextm.Windows.Forms.MessageBoxFactory.Info(Name, "File not saved", "Please save to see the file properties");
				return;
			}

			FrmFileProperties _Frm = new FrmFileProperties();
			FilePropertyInfo _FilePropertyInfo = new FilePropertyInfo();

			_Frm.propertyGrid.SelectedObject = _FilePropertyInfo;
			_Frm.ShowDialog();
		}

		private void DoMakeFileWritable(object aSender, EventArgs AEventArgs) {
			IOTAEditor _Editor = OtaUtils.GetEditorWithSourceEditor(OtaUtils.GetCurrentModule());

			if (!ValidationHelpers.ValidateCurrentModuleNotNull() ||
			    !ValidationHelpers.ValidateCustom((Lextm.IO.FileHelper.GetFileIsReadOnly(OtaUtils.GetCurrentEditorFileName())),
			                                      "File is not read only!") ||
			    !ValidationHelpers.ValidateCustom((_Editor != null && !_Editor.IsModified),
			                                      "Can change the file attribut if the buffer is modified!")) {
				return;
			}

			string _FileName = OtaUtils.GetCurrentModule().FileName;
			File.SetAttributes(_FileName, File.GetAttributes(_FileName) ^ FileAttributes.ReadOnly);

			IOTAActionService _ActionService = OtaUtils.GetActionService();
			_ActionService.ReloadFile(_FileName);
		}

		//private void DoOrganizeProjects(object aSender, EventArgs e) {
		//    if (!ValidationHelpers.ValidateCurrentProjectGroupNotNull()) {
		//        return;
		//    }

		//    FrmProjectManagement _Frm = new FrmProjectManagement();

		//    if (_Frm.ShowDialog() == DialogResult.OK) {
		//        IOTAProjectGroup _ProjectGroup = OTAUtils.GetCurrentProjectGroup();

		//        for (int i = _ProjectGroup.ProjectCount -1; i >=0 ; i--) {
		//            IOTAProject _Project = _ProjectGroup[i];

		//            _ProjectGroup.RemoveProject(_Project);
		//        }

		//        _ProjectGroup.Save(false, true);

		//        IOTAActionService _ActionService = OTAUtils.GetActionService();

		//        for (int i = 0; i < _Frm.ProjectFileNames.Length; i++) {
		//            _ActionService.OpenProject(_Frm.ProjectFileNames[i], false);
		//        }

		//        _ProjectGroup.Save(false, true);
		//    }
		//}

		private void DoRemoveMissingFiles(object aSender, EventArgs AEventArgs) {
			if (!ValidationHelpers.ValidateCurrentProjectNotNull() ||
			    !ValidationHelpers.ValidateCurrentModuleNotNull()) {
				return;
			}

			IOTAProject _Project = OtaUtils.GetCurrentProject();

			IList<string> _List = new List<string>();
			
			for (int i = 0; i < _Project.ModuleCount; i++) {
				IOTAModuleInfo _ModuleInfo = _Project.GetModuleInfo(i);

				if (!String.IsNullOrEmpty(_ModuleInfo.FileName) && !File.Exists(_ModuleInfo.FileName)) {
					_Project.RemoveFile(_ModuleInfo.FileName);
					_List.Add(_ModuleInfo.FileName);
				}
			}

			if (_List.Count != 0) {
				FrmMissingFiles _FrmMissingFiles = new FrmMissingFiles(_List);

				_FrmMissingFiles.Show();
				//ValidationHelpers.ShowInformation("Removed " + _Count.ToString() + " files from the active project");
			} else {
				ValidationHelpers.ShowWarning("No missing file is found");
			}
		}

		private void DoSaveFileList(object aSender, EventArgs AEventArgs) {
			IOTAModuleServices _ModuleServices = OtaUtils.GetModuleServices();
			int _Count = _ModuleServices.ModuleCount;
			FrmSaveFiles _FrmSaveFiles =  new FrmSaveFiles();

			// Get the modified files
			for (int i = 0; i < _Count; i++) {
				IOTAModule _Module = _ModuleServices.GetModule(i);

				if (_Module != null ) {
					try {
						IOTAEditor _Editor = OtaUtils.GetEditorWithSourceEditor(_Module);


						if (_Editor != null && _Editor.IsModified) {
							_FrmSaveFiles.Modules.Add(_Module);
						}
					}
					// Workaround for unexpected error with project module
					catch {

					}
				}
			}

			if (_FrmSaveFiles.Modules.Count > 0) {
				if (_FrmSaveFiles.ShowDialog() == DialogResult.OK) {
					IOTAModule[] _Modules = _FrmSaveFiles.GetModuleToSave();

					foreach (IOTAModule _Module in _Modules) {
						_Module.Save(false, true);
					}
				}
			} else {
				Lextm.Windows.Forms.MessageBoxFactory.Info(Name, "No file found", "There is no file to save");
			}
		}

		private void DoPEVerifyProjectTarget(object aSender, EventArgs AEventArgs) {
			if (!ValidationHelpers.ValidateCurrentProjectTargetExists()) {
				return;
			}
			if (!ValidationHelpers.ValidateToolInstalled(CurrentPEVerify))
			{
				return;
			}

			string _Target = StringHelper.QuoteString(OtaUtils.GetProjectTarget(OtaUtils.GetCurrentProject()));
			try
			{
				AddMessage("PEVerify project target: " + _Target);
				CurrentPEVerify.RunConsole(new object[] {_Target, String.Empty}, String.Empty);
			} catch (Exception ex)
			{
				Lextm.Windows.Forms.MessageBoxFactory.Fatal(Name, ex);
			}
		}

		protected override void AddMessage(string text) {
			OtaUtils.AddMessage(text);
		}

		//private void DoVSExportActiveProject(object aSender, EventArgs e) {
		//    if (!ValidationHelpers.ValidateCurrentProjectNotNull()) {
		//        AddMessage("No project to export.");
		//        return;
		//    }
		//    if (OtaUtils.CurrentIdeVersion == 1) {
		//        AddMessage("C#Builder 1.0 is not supported.");
		//        return;
		//    }
		//    string bdsFile = OtaUtils.GetCurrentProject().FileName;
		//    string vsFile;
		//    if (ValidationHelpers.ValidateCurrentProjectIsCSharp())
		//    {
		//        vsFile = Path.ChangeExtension(bdsFile, "csproj");
		//    } else if (ValidationHelpers.ValidateCurrentProjectIsVB()) {
		//        vsFile = Path.ChangeExtension(bdsFile, "vbproj");
		//    } else {
		//        AddMessage("Only C# and VB projects can be exported. Current project is " + bdsFile);
		//        return;
		//    }

		//    string xsltFile = GetXsltFile();
		//    AddMessage("Visual Studio Conversion Tool");
		//    AddMessage("Using transform document: " + xsltFile);
		//    AddMessage("Input file: " + bdsFile);
		//    AddMessage("Output file: " + vsFile);

		//    try {
		//        Lextm.Xml.XmlConvertor.ConvertXmlUsingXsl(bdsFile,
		//                                xsltFile,
		//                                vsFile);
		//        AddMessage("Done.");
		//    } catch (Exception ex) {
		//        Lextm.Windows.Forms.MessageBoxFactory.Fatal(ex);
		//    }

		//}
		
		//private static string GetXsltFile() {
		//    return OtaUtils.GetIdeRootDir() + @"Bin\VSExport.xsl";
		//}
		
//		protected void DoStartProjectTarget(object aSender, EventArgs e) {
//			if (!ValidationHelpers.ValidateCurrentProjectTargetExists()) {
//				return;
//			}
//
//			string _Target = OTAUtils.GetProjectTarget(OTAUtils.GetCurrentProject());
//			Process.Start(_Target);
//		}

		private void DoViewModuleHexDump(object aSender, EventArgs AEventArgs) {
			if (!ValidationHelpers.ValidateCurrentEditorNotNull()) {
				return;
			}
			// TODO: make it non-modal.
			string _FileName = OtaUtils.GetCurrentEditorFileName();

			FrmViewHexaDump _Frm = new FrmViewHexaDump();

			ByteViewer _Ctr = new ByteViewer();
			_Ctr.SetDisplayMode(DisplayMode.Hexdump);
			_Ctr.SetFile(_FileName);
			_Ctr.Dock = DockStyle.Fill;
			_Frm.pnlHost.Controls.Add(_Ctr);
			_Frm.ShowDialog();
		}

		private void DoViewProjectSource(object aSender, EventArgs AEventArgs) {
			if (!ValidationHelpers.ValidateCurrentProjectNotNull()) {
				return;
			}

			ExternalViewFile(ExternalSourceViewer, OtaUtils.GetCurrentProject().FileName);
		}

		private const string ExternalSourceViewer = "notepad.exe";

		private void DoViewProjectGroupSource(object aSender, EventArgs AEventArgs)
		{
			if (!ValidationHelpers.ValidateCurrentProjectGroupNotNull())
			{
				return;
			}
			string fileName = OtaUtils.GetCurrentProjectGroup().FileName;
			if (File.Exists(fileName))
			{
				ExternalViewFile(ExternalSourceViewer, fileName);
			}
		}
		private void DoViewWritableFiles(object aSender, EventArgs AEventArgs) {
			if (!ValidationHelpers.ValidateCurrentProjectNotNull() ||
			    !ValidationHelpers.ValidateCurrentModuleNotNull()) {
				return;
			}

			IOTAProject _Project = OtaUtils.GetCurrentProject();

			IList<string> _List = new List<string>();

			for (int i = 0; i < _Project.ModuleCount; i++) {
				IOTAModuleInfo _ModuleInfo = _Project.GetModuleInfo(i);

				if (!String.IsNullOrEmpty(_ModuleInfo.FileName) && File.Exists(_ModuleInfo.FileName) && !Lextm.IO.FileHelper.GetFileIsReadOnly(_ModuleInfo.FileName))
				{
					_List.Add(_ModuleInfo.FileName);
				}
			}

			if (_List.Count != 0) {
				FrmFileList _FrmFileList = new FrmFileList(_List);
				_FrmFileList.lblTitle.Text = "Writable files";
				_FrmFileList.lblSubTitle.Text = "Show all the writable file found in the current project";
				_FrmFileList.Show();
			} else {
				ValidationHelpers.ShowWarning("No writable file is found");
			}
		}

		private void DoAutoActivateProject(object sender, EventArgs e)
		{
			bool state = (bool)PropertyRegistry.Get("AutoActivateProject", true);
			bool newState = !state;
			OtaUtils.SetMenuItemCheckState(MenuAutoActivateProject, newState);
			PropertyRegistry.Set("AutoActivateProject", newState);
		}

		#endregion
		/**************************************************************/
		/*                        Public
        /**************************************************************/
		private const string MenuProject = "ProjectSubMenu";
		const string MenuAutoActivateProject = "AutoActivateProjectMenu";
		
		protected override void IdeRegisterMenus() {
			// Project
			RegisterAction(CreateEmptyMenu(MenuItemLocation.Child,
			                               ShareUtils.MenuRootDefault,
			                               MenuProject,
			                               "Project Aid"));

			RegisterAction(CreateCheckedMenu(MenuItemLocation.Child,
			                                 MenuProject,
			                                 MenuAutoActivateProject,
			                                 0,
			                                 "Auto Activate Project",
			                                 (bool)PropertyRegistry.Get("AutoActivateProject", true),
			                                 new EventHandler(DoAutoActivateProject)
			                                ));
			// View Project Group Source
			RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                MenuProject,
			                                "ViewProjectGroupSourceMenu",
			                                0,
			                                "View Project Group Source...",
			                                new EventHandler(DoViewProjectGroupSource)));

			// View Project Source
			RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                MenuProject,
			                                "ViewProjectSourceMenu",
			                                0,
			                                "View Project Source...",
			                                new EventHandler(DoViewProjectSource)));
			// PEVerify Project target
			RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                MenuProject,
			                                "PEVerifyProjectTargetMenu",
			                                0,
			                                "PEVerify Project Target",
			                                new EventHandler(DoPEVerifyProjectTarget)));

			// Start Project target
			RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                MenuProject,
			                                "SignProjectMenu",
			                                0,
			                                "Create Key File",
			                                new EventHandler(DoSignProjectTarget)));

			// Organize Project
			/*
            Bug with Remove Project
            RegisterAction(CreateActionMenu(OTAMenuItemLocation.otamlChild,
                                                 Consts.SHARP_BUILDER_TOOLS_PROJECT_MENU_NAME,
                                                 "OrganizeProjectMenu",
                                                 0,
                                                 "Change Project order...",
                                                 new EventHandler(DoOrganizeProjects)));
			 */
			// Clean Project Folder
			RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                MenuProject,
			                                "CleanProjectFolderMenu",
			                                0,
			                                "Clean Project Folder...",
			                                new EventHandler(DoCleanProjectFolder)));

			// ILMerge Project
			RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                MenuProject,
			                                "ILMergeProjectMenu",
			                                0,
			                                "ILMerge Project...",
			                                new EventHandler(DoILMergeProject)));


			// Delete Project Target
			RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                MenuProject,
			                                "DeleteProjectTargetMenu",
			                                0,
			                                "Delete Project Target...",
			                                new EventHandler(DoDeleteProjectTarget)));

			// Separator
			RegisterAction(CreateSeparatorMenu(MenuItemLocation.Child,
			                                   MenuProject));

			// View Writable Files
			RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                MenuProject,
			                                "ViewWritableFilesMenu",
			                                0,
			                                "View Writable Files...",
			                                new EventHandler(DoViewWritableFiles)));

			// View Missing Files
			RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                MenuProject,
			                                "RemoveMissingFilesMenu",
			                                0,
			                                "View Missing Files...",
			                                new EventHandler(DoRemoveMissingFiles)));

			// View Current File Hexa Dump
			RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                MenuProject,
			                                "ViewModuleHexaDumpMenu",
			                                0,
			                                "View Current File Hexa Dump...",
			                                new EventHandler(DoViewModuleHexDump)));

			// Save Files
			RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                MenuProject,
			                                "SaveFilesMenu",
			                                0,
			                                "Save Files...",
			                                new EventHandler(DoSaveFileList)));

			// Make File Writable
			RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                MenuProject,
			                                "MakeFileWritableMenu",
			                                0,
			                                "Make File Writable",
			                                new EventHandler(DoMakeFileWritable)));

			//// File Properties
			//RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			//                                     MenuProject,
			//                                     "GetFilePropertiesMenu",
			//                                     0,
			//                                     "File Properties...",
			//                                     new EventHandler(DoGetFileProperties)));


			//TODO: make stronger if necessary. otherwise this function is not that important.
			//// Delete Current File
			//RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			//                                     MenuProject,
			//                                     "DeleteCurrentModuleMenu",
			//                                     0,
			//                                     "Delete Current File...",
			//                                     new EventHandler(DoDeleteCurrentModule)));

			// Copy File as
			RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			                                MenuProject,
			                                "CopyFileMenu",
			                                0,
			                                "Copy File as...",
			                                new EventHandler(DoCopyFile)));


			// Add File Tree  // use AddMany instead.
			//            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
			//                                                 MenuProject,
			//                                                 "AddFileTreeMenu",
			//                                                 0,
			//                                                 "Add File Tree...",
			//                                                 new EventHandler(DoAddFileTree)));

			//Activate Current File Project  // changed to auto activator.
			//			RegisterAction(CreateActionMenu(MenuItemLocation.Child,
//			                                     MenuProject,
//			                                     "ActivateCurrentModuleProjectMenu",
//			                                     0,
//			                                     "Activate Current File Project",
//			                                     new EventHandler(DoActivateCurrentModuleProject)));

		}

		private Tool.PEVerify fCurrentPEVerify;

		protected override BeWise.SharpBuilderTools.Tools.BaseTool GetCurrentTool() {
			if (fCurrentPEVerify == null) {
				fCurrentPEVerify = new Tool.PEVerify();
				fCurrentPEVerify.OnConsoleOutput += new EventHandler(DoRefreshConsole);
			}

			return fCurrentPEVerify;
		}

		private Tool.PEVerify CurrentPEVerify {
			get {
				return (Tool.PEVerify) GetCurrentTool();
			}
		}
	}
}
