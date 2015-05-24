// modified by lextm
// some extensions are added.
// lextm extensions are copyrighted by lextm, 2005-2006
// other extensions are copyrighted by its original authors.

using System;

using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using Borland.Studio.ToolsAPI;
using Lextm.Diagnostics;
using Lextm.Win32;
using System.Collections.Generic;
using System.Globalization;

namespace BeWise.Common.Utils {
	/// <summary>
	/// OTA related functions.
	/// </summary>
    public sealed class OtaUtils
    {

        /**************************************************************/
        /*                        Private
        /**************************************************************/
        private static bool CheckExtension(string extension, string[] extensions)
        {
            if (String.IsNullOrEmpty(extension))
            {
                return false;
            }

            string ext;

            if (extension.StartsWith(".", StringComparison.Ordinal))
            {
                ext = extension.Substring(1).ToUpperInvariant();
            }
            else
            {
                ext = extension.ToUpperInvariant();
            }

            foreach (string oneExtension in extensions)
            {

                if (oneExtension.Trim().ToUpperInvariant() == ext)
                {
                    return true;
                }
            }
            return false;
        }

        private const int MAX_READ_SIZE = 9999999;

        /**************************************************************/
        /*                        Public Const
        /**************************************************************/
        /// <summary>
        /// Alt value.
        /// </summary>
        public const int Alt = 32768;  // 1000000000000000 == 0X8000
        /// <summary>
        /// Ctrl value.
        /// </summary>
        public const int Ctrl = 16384;  //  100000000000000 == 0X4000
        /// <summary>
        /// Shift value.
        /// </summary>
        public const int Shift = 8192;   //   10000000000000 == 0X2000

        /**************************************************************/
        /*                       Services
        /**************************************************************/
        /// <summary>
        /// Gets about box service.
        /// </summary>
        /// <returns></returns>
        public static IOTAAboutBoxService GetAboutBoxService()
        {
            return (IOTAAboutBoxService)BorlandIDE.GetService(typeof(IOTAAboutBoxService));
        }
        /// <summary>
        /// Gets action service.
        /// </summary>
        /// <returns></returns>
        public static IOTAActionService GetActionService()
        {
            return (IOTAActionService)BorlandIDE.GetService(typeof(IOTAActionService));
        }
        /// <summary>
        /// Gets gallery category manager.
        /// </summary>
        /// <returns></returns>
        public static IOTAGalleryCategoryManager GetGalleryCategoryManager()
        {
            return (IOTAGalleryCategoryManager)BorlandIDE.GetService(typeof(IOTAGalleryCategoryManager));
        }
        /// <summary>
        /// Gets component install service.
        /// </summary>
        /// <returns></returns>
        public static IOTAComponentInstallService GetComponentInstallService()
        {
            return (IOTAComponentInstallService)BorlandIDE.GetService(typeof(IOTAComponentInstallService));
        }
        /// <summary>
        /// Gets help service.
        /// </summary>
        /// <returns></returns>
        public static IHelpService GetHelpService()
        {
            return (IHelpService)BorlandIDE.GetService(typeof(IHelpService));
        }
        /// <summary>
        /// Gets main menu service.
        /// </summary>
        /// <returns></returns>
        public static IOTAMainMenuService GetMainMenuService()
        {
            return (IOTAMainMenuService)BorlandIDE.GetService(typeof(IOTAMainMenuService));
        }
        /// <summary>
        /// Gets message service.
        /// </summary>
        /// <returns></returns>
        public static IOTAMessageService GetMessageService()
        {
            return (IOTAMessageService)BorlandIDE.GetService(typeof(IOTAMessageService));
        }
        /// <summary>
        /// Gets module service.
        /// </summary>
        /// <returns></returns>
        public static IOTAModuleServices GetModuleServices()
        {
            return (IOTAModuleServices)BorlandIDE.GetService(typeof(IOTAModuleServices));
        }
        /// <summary>
        /// Gets service.
        /// </summary>
        /// <returns></returns>
        public static IOTAService GetService()
        {
            return (IOTAService)BorlandIDE.GetService(typeof(IOTAService));
        }
        /// <summary>
        /// Gets wizard service.
        /// </summary>
        /// <returns></returns>
        public static IOTAWizardService GetWizardService()
        {
            return (IOTAWizardService)BorlandIDE.GetService(typeof(IOTAWizardService));
        }

        /**************************************************************/
        /*                         FileName
        /**************************************************************/
        /// <summary>
        /// Gets project file list.
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static IList<string> GetProjectFileList(string extension)
        {
            IList<string> result = new List<string>();

            IOTAProjectGroup _ProjectGroup = OtaUtils.GetCurrentProjectGroup();
            if (_ProjectGroup == null)
            {
                return result;
            }

            IOTAProject _Project = _ProjectGroup.ActiveProject;
            if (_Project == null)
            {
                return result;
            }
            for (int k = 0; k < _Project.ModuleCount; k++)
            {
                IOTAModuleInfo _ModuleInfo = _Project.GetModuleInfo(k);

                if (_ModuleInfo.FileName != null && _ModuleInfo.FileName.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(_ModuleInfo.FileName);
                }
            }

            return result;
        }
        /// <summary>
        /// Gets current module file name.
        /// </summary>
        /// <returns>null if no file.</returns>
        public static string GetCurrentModuleFileName()
        {
            IOTAModule _Module = OtaUtils.GetCurrentModule();

            if (_Module == null)
            {
                return null;
            }
            else
            {
                return _Module.FileName;
            }
        }
        /// <summary>
        /// Gets current editor file name.
        /// </summary>
        /// <returns>null if no file.</returns>
        public static string GetCurrentEditorFileName()
        {
            IOTAModule _Module = OtaUtils.GetCurrentModule();

            if (_Module == null)
            {
                return null;
            }
            else
            {
                IOTAEditor _Editor = GetCurrentEditor(_Module);

                if (_Editor == null)
                {
                    return null;
                }

                // BUG in the ToolsAPI for the VCL:
                if (String.IsNullOrEmpty(_Editor.FileName))
                {
                    return _Module.FileName;
                }

                return _Editor.FileName;
            }
        }

        /**************************************************************/
        /*                         Project Group
        /**************************************************************/
        /// <summary>
        /// Gets current project group.
        /// </summary>
        /// <returns>null if no group.</returns>
        public static IOTAProjectGroup GetCurrentProjectGroup()
        {
            IOTAModuleServices _ModuleServices = GetModuleServices();

            IOTAProjectGroup _ProjectGroup = _ModuleServices.MainProjectGroup;

            return _ProjectGroup;
        }
        /**************************************************************/
        /*                         Elide
        /**************************************************************/
        /// <summary>
        /// Elides nearest block.
        /// </summary>
        /// <param name="editView">Edit view</param>
        /// <param name="unElide">Unelide flag</param>
        public static void ElideNearestBlock(IOTAEditView editView, bool unElide)
        {
            if (editView != null)
            {
                IOTAElideActions aElideActions = editView as IOTAElideActions;

                if (aElideActions != null)
                {
                    if (unElide)
                    {
                        aElideActions.UnElideNearestBlock();
                    }
                    else
                    {
                        aElideActions.ElideNearestBlock();
                    }
                }
            }
        }

        /**************************************************************/
        /*                         Project
        /**************************************************************/
        /// <summary>
        /// Gets current project.
        /// </summary>
        /// <returns>null if no project.</returns>
        public static IOTAProject GetCurrentProject()
        {
            IOTAProject _Project = null;
            IOTAProjectGroup _ProjectGroup = GetCurrentProjectGroup();

            if (_ProjectGroup != null && _ProjectGroup.ProjectCount > 0)
            {
                _Project = _ProjectGroup.ActiveProject;
            }

            return _Project;
        }

        /// <summary>
        /// Gets module info from project.
        /// </summary>
        /// <param name="project">Project</param>
        /// <param name="fileName">File name</param>
        /// <returns>The module info, or null.</returns>
        public static IOTAModuleInfo GetModuleInfoFromProject(IOTAProject project, string fileName)
        {
            for (int i = 0; i < project.ModuleCount; i++)
            {
                if (project.GetModuleInfo(i).FileName == fileName)
                {
                    return project.GetModuleInfo(i);
                }
            }

            return null;
        }
        //		/// <summary>
        //		/// Removes a file from project.
        //		/// </summary>
        //		/// <remarks>No implemented.</remarks>
        //		/// <param name="project">Project</param>
        //		/// <param name="fileName">File name</param>
        //		public static void RemoveFromProject(IOTAProject project, string fileName) {
        //			Lextm.Windows.Forms.MessageBoxFactory.Warn("Not yet implmented in the Borlant OTA");
        //		}
        /// <summary>
        /// Gets project target name.
        /// </summary>
        /// <param name="project">Project</param>
        /// <returns>Target name or null.</returns>
        public static string GetProjectTarget(IOTAProject project)
        {
            if (project == null)
            {
                return null;
            }
            String targetName = project.ProjectOptions.TargetName;
            if (IsDelphiProject(project))
            {
                string folder = Path.GetDirectoryName(targetName);
                string file = Path.GetFileName(OtaUtils.GetDelphiProjectFileName(project.FileName));
                return Path.ChangeExtension(Path.Combine(folder, file), Path.GetExtension(targetName));
            }
            return targetName;
        }

        /**************************************************************/
        /*                         Modules
        /**************************************************************/
        /// <summary>
        /// Verifies that current module is a C# file.
        /// </summary>
        /// <returns>true if it is, false if not.</returns>
        public static bool CurrentModuleIsCSFile()
        {
            string _FileName = GetCurrentEditorFileName();

            if (String.IsNullOrEmpty(_FileName))
            {
                return false;
            }
            else
            {
                return IsCSFile(_FileName);
            }
        }
        /// <summary>
        /// Verifies that current module is a C/C++ file.
        /// </summary>
        /// <returns>true if it is, false if not.</returns>
        public static bool CurrentModuleIsCFile()
        {
            string _FileName = GetCurrentEditorFileName();

            if (String.IsNullOrEmpty(_FileName))
            {
                return false;
            }
            else
            {
                return IsCFile(_FileName);
            }
        }
        /// <summary>
        /// Verifies that current module is a BASIC file.
        /// </summary>
        /// <returns>true if it is, false if not.</returns>
        public static bool CurrentModuleIsBasicFile()
        {
            string _FileName = GetCurrentEditorFileName();

            if (String.IsNullOrEmpty(_FileName))
            {
                return false;
            }
            else
            {
                return IsBasicFile(_FileName);
            }
        }
        /// <summary>
        /// Verifies that current module is a C#, or Delphi, or C/C++ file.
        /// </summary>
        /// <returns>true if it is, false if not.</returns>
        public static bool CurrentModuleIsCSOrDelphiOrCFile()
        {
            return CurrentModuleIsCSFile() ||
                CurrentModuleIsDelphiFile() ||
                CurrentModuleIsCFile();
        }
        /// <summary>
        /// Verifies that current module is a Delphi file.
        /// </summary>
        /// <returns>true if it is, false if not.</returns>
        public static bool CurrentModuleIsDelphiFile()
        {
            string _FileName = GetCurrentEditorFileName();

            if (String.IsNullOrEmpty(_FileName))
            {
                return false;
            }
            else
            {
                return IsDelphiFile(_FileName);
            }
        }

        /// <summary>
        /// Verifies that current module is a source file.
        /// </summary>
        /// <returns>true if it is, false if not.</returns>
        public static bool CurrentModuleIsSourceFile()
        {
            string _FileName = GetCurrentEditorFileName();

            if (String.IsNullOrEmpty(_FileName))
            {
                return false;
            }
            else
            {
                return IsSourceFile(_FileName);
            }
        }

        /// <summary>
        /// Verifies that current module is an XML file.
        /// </summary>
        /// <returns>true if it is, false if not.</returns>
        public static bool CurrentModuleIsXmlFile()
        {
            string _FileName = GetCurrentEditorFileName();

            if (String.IsNullOrEmpty(_FileName))
            {
                return false;
            }
            else
            {
                return IsXmlFile(_FileName);
            }
        }
        /// <summary>
        /// Gets current module.
        /// </summary>
        /// <returns></returns>
        public static IOTAModule GetCurrentModule()
        {
            IOTAModuleServices _ModuleServices = GetModuleServices();

            return _ModuleServices.CurrentModule;
        }
        /// <summary>
        /// Gets project source file list.
        /// </summary>
        /// <param name="project">Project</param>
        /// <returns></returns>
        public static IList<string> GetProjectSourceFiles(IOTAProject project)
        {
            IList<string> result = new List<string>();
            if (project == null)
            {
                return result;
            }

            for (int i = 0; i < project.ModuleCount; i++)
            {
                IOTAModuleInfo _ModuleInfo = project.GetModuleInfo(i);

                if (IsSourceFile(_ModuleInfo.FileName))
                {
                    result.Add(_ModuleInfo.FileName);
                }
            }
            return result;
        }
        /// <summary>
        /// Verifies that a file is a C# file.
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns></returns>
        public static bool IsCSFile(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
            {
                return false;
            }

            return CheckExtension(Path.GetExtension(fileName), GetCSExtensionsFromOptions());
        }

        /// <summary>
        /// Verifies that a file is a Delphi file.
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns></returns>
        public static bool IsDelphiFile(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
            {
                return false;
            }

            return CheckExtension(Path.GetExtension(fileName), GetDelphiExtensionsFromOptions());
        }

        /// <summary>
        /// Verifies that a file is a project file.
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns></returns>
        public static bool IsProjectFile(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
            {
                return false;
            }
            //TODO: handle dproj, cproj, csproj, vbproj.
            return CheckExtension(Path.GetExtension(fileName), new string[] { ".dproj", ".cproj", ".csproj", "vbproj" });
        }

        /// <summary>
        /// Verifies that a file is a project group file.
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns></returns>
        public static bool IsProjectGroupFile(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
            {
                return false;
            }
            //TODO: handle groupproj
            return (Path.GetExtension(fileName).ToUpperInvariant() == ".groupproj".ToUpperInvariant());
        }

        /// <summary>
        /// Verifies that a file is an XML file.
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns></returns>
        public static bool IsXmlFile(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
            {
                return false;
            }

            return CheckExtension(Path.GetExtension(fileName), GetXmlExtensionsFromOptions());
        }

        /// <summary>
        /// Verifies that a file is a source file.
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns></returns>
        /// <remarks>Only C/C++/C#, Delphi, VB files are source files.</remarks>
        public static bool IsSourceFile(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
            {
                return false;
            }

            return IsCSFile(fileName) || IsBasicFile(fileName) ||
                IsDelphiFile(fileName) || //IsXmlFile(fileName) ||
                IsCFile(fileName);
        }
        /// <summary>
        /// Verifies a project has unsaved files and warns.
        /// </summary>
        /// <param name="project">Project</param>
        /// <param name="warningMessageTitle">Message title</param>
        /// <returns>true if it has, false if not.</returns>
        public static bool WarnForUnsavedFiles(IOTAProject project, string warningMessageTitle)
        {
            if (project == null)
            {
                return false;
            }

            IOTAModuleServices _ModuleServices = GetModuleServices();
            if (_ModuleServices == null)
            {
                return false;
            }

            for (int k = 0; k < project.ModuleCount; k++)
            {
                IOTAModuleInfo _ModuleInfo = project.GetModuleInfo(k);

                IOTAModule _Module = _ModuleServices.FindModule(_ModuleInfo.FileName);

                if (_Module != null)
                {

                    IOTAEditor _Editor = GetEditorWithSourceEditor(_Module);

                    if (_Editor != null && _Editor.IsModified)
                    {
                        if (Lextm.Windows.Forms.MessageBoxFactory.Confirm("The project contains unsaved files" + Environment.NewLine + _Module.FileName + Environment.NewLine + "Do you still want to compile the project?") == DialogResult.No)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /**************************************************************/
        /*                         DotNetModule
        /**************************************************************/
        /// <summary>
        /// Gets active designer for module.
        /// </summary>
        /// <param name="module">Module</param>
        /// <returns>null if not.</returns>
        public static IDesignerHost GetActiveDesignerForModule(IOTAServiceProvider module)
        {
            if (module != null)
            {
                IOTADotNetModule _DotNetModule = (IOTADotNetModule)module.GetService(typeof(IOTADotNetModule));

                if (_DotNetModule != null)
                {
                    _DotNetModule.ShowDesigner();

                    if (_DotNetModule.DesignerActive)
                    {
                        IDesignerHost _Designer = _DotNetModule.DesignerHost;

                        return _Designer;
                    }
                }
            }

            return null;
        }
        /// <summary>
        /// Gets designer from module.
        /// </summary>
        /// <param name="module">Module</param>
        /// <returns>null if not.</returns>
        public static IDesignerHost GetDesignerForModule(IOTAServiceProvider module)
        {
            if (module != null)
            {
                IOTADotNetModule _DotNetModule = (IOTADotNetModule)module.GetService(typeof(IOTADotNetModule));

                if (_DotNetModule != null)
                {
                    IDesignerHost _Designer = _DotNetModule.DesignerHost;

                    return _Designer;
                }
            }

            return null;
        }
        /// <summary>
        /// Verifies designer if active.
        /// </summary>
        /// <param name="module">Module</param>
        /// <returns></returns>
        public static bool GetDesignerIsActive(IOTAServiceProvider module)
        {
            if (module != null)
            {
                IOTADotNetModule _DotNetModule = (IOTADotNetModule)module.GetService(typeof(IOTADotNetModule));

                if (_DotNetModule != null)
                {
                    return _DotNetModule.DesignerActive;
                }
            }

            return false;
        }

        /**************************************************************/
        /*                         Editor
        /**************************************************************/
        /// <summary>
        /// Gets current editor.
        /// </summary>
        /// <param name="module">Module</param>
        /// <returns></returns>
        public static IOTAEditor GetCurrentEditor(IOTAModule module)
        {
            try
            {
                // BUG in the ToolsAPI: The IDE crashes calling IOTAModule.CurrentEditor on the welcome page
                if (module == null || module.FileName == "default.htm")
                {
                    return null;
                }
                else
                {
                    return module.CurrentEditor;
                }
            }
            catch
            {
                // Bug in the ToolsAPI: if nothing visible
                return null;
            }
        }
        /*
    /// <summary>
    /// Gets form editor.
    /// </summary>
    /// <param name="module">Module</param>
    /// <returns>null if null module.</returns>
          public static IOTAFormEditor GetFormEditor(IOTAModule module) {
              return module as IOTAFormEditor;
          }
    //*/
        /// <summary>
        /// Gets editor which must have a source editor.
        /// </summary>
        /// <param name="module">Module</param>
        /// <returns>null if null module.</returns>
        public static IOTAEditor GetEditorWithSourceEditor(IOTAModule module)
        {
            if (module == null)
            {
                return null;
            }

            return GetSourceEditor(module) as IOTAEditor;
        }
        /// <summary>
        /// Gets source editor from editor.
        /// </summary>
        /// <param name="module">Module</param>
        /// <param name="editor">Editor</param>
        /// <returns>null if wrong.</returns>
        public static IOTASourceEditor GetSourceEditorFromEditor(IOTAModule module, IOTAEditor editor)
        {
            if (module == null || editor == null)
            {
                return null;
            }
            IOTASourceEditor result;

            for (int i = 0; i < module.ModuleFileCount; i++)
            {
                IOTAEditor _Editor = module.ModuleFileEditors(i);

                if (_Editor.FileName == editor.FileName)
                {
                    result = _Editor as IOTASourceEditor;
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return null;
        }

        /**************************************************************/
        /*                         Source Editor
        /**************************************************************/
        /// <summary>
        /// Gets current source editor.
        /// </summary>
        /// <returns>null if wrong.</returns>
        public static IOTASourceEditor GetCurrentSourceEditor()
        {
            return GetSourceEditor(GetCurrentModule());
        }
        /// <summary>
        /// Gets source editor.
        /// </summary>
        /// <param name="module">Module</param>
        /// <returns>null if null module.</returns>
        public static IOTASourceEditor GetSourceEditor(IOTAModule module)
        {
            if (module == null)
            {
                return null;
            }
            IOTASourceEditor result;

            if (GetCurrentEditor(module) != null)
            {
                result = GetSourceEditorFromEditor(module, GetCurrentEditor(module));
                if (result != null)
                {
                    return result;
                }
            }
            else
            { // Find a source Editor
                IOTAEditor _Editor;

                for (int i = 0; i < module.ModuleFileCount; i++)
                {
                    _Editor = module.ModuleFileEditors(i);

                    result = _Editor as IOTASourceEditor;
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return null;
        }

        /**************************************************************/
        /*                         EditView
        /**************************************************************/
        /// <summary>
        /// Builds lines.
        /// </summary>
        /// <param name="lines">Lines of string</param>
        /// <param name="addLastNewLine">Add last new line flag</param>
        /// <returns></returns>
        public static string BuildLines(IList<string> lines, bool addLastNewLine)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < lines.Count; i++)
            {
                builder.Append(lines[i]);

                if (lines.Count > 1 && (i != lines.Count - 1 || addLastNewLine))
                {
                    builder.Append(Environment.NewLine);
                }
            }

            return builder.ToString();
        }
        /// <summary>
        /// Moves char position of source editor to buffer position.
        /// </summary>
        /// <param name="sourceEditor">Source editor</param>
        /// <param name="charPos">Char position</param>
        /// <returns></returns>
        public static int CharPosToBufferPos(IOTASourceEditor sourceEditor, OTACharPos charPos)
        {
            IOTAEditView _EditView = GetEditView(sourceEditor);

            if (_EditView != null)
            {
                return _EditView.CharPosToPos(charPos);
            }

            return -1;
        }
        /// <summary>
        /// Moves edit position to buffer position.
        /// </summary>
        /// <param name="sourceEditor">Source editor</param>
        /// <param name="editPos">Edit position</param>
        /// <returns></returns>
        public static int EditPosToBufferPos(IOTASourceEditor sourceEditor, OTAEditPos editPos)
        {
            OTACharPos _CharPos = EditPosToCharPos(sourceEditor, editPos);

            return CharPosToBufferPos(sourceEditor, _CharPos);
        }
        /// <summary>
        /// Moves buffer position to Edit position.
        /// </summary>
        /// <param name="sourceEditor">Source editor</param>
        /// <param name="bufferPos">Buffer position</param>
        /// <returns></returns>
        public static OTAEditPos BufferPosToEditPos(IOTASourceEditor sourceEditor, int bufferPos)
        {
            IOTAEditView _EditView = GetEditView(sourceEditor);

            if (_EditView != null)
            {
                OTACharPos _CharPos = _EditView.PosToCharPos(bufferPos);

                return CharPosToEditPos(sourceEditor, _CharPos);
            }

            return new OTAEditPos();
        }
        /// <summary>
        /// Moves buffer position to char position.
        /// </summary>
        /// <param name="sourceEditor">Source editor</param>
        /// <param name="bufferPos">Buffer position</param>
        /// <returns></returns>
        public static OTACharPos BufferPosToCharPos(IOTASourceEditor sourceEditor, int bufferPos)
        {
            IOTAEditView _EditView = GetEditView(sourceEditor);

            if (_EditView != null)
            {
                return _EditView.PosToCharPos(bufferPos);
            }

            return new OTACharPos();
        }
        /// <summary>
        /// Moves edit position to char position.
        /// </summary>
        /// <param name="sourceEditor">Source editor</param>
        /// <param name="editPos">Edit position</param>
        /// <returns></returns>
        public static OTACharPos EditPosToCharPos(IOTASourceEditor sourceEditor, OTAEditPos editPos)
        {
            IOTAEditView _EditView = GetEditView(sourceEditor);
            OTACharPos _CharPos = new OTACharPos();

            if (_EditView != null)
            {
                _EditView.ConvertPos(true, ref editPos, ref _CharPos);
            }

            return _CharPos;
        }
        /// <summary>
        /// Moves char position to edit position.
        /// </summary>
        /// <param name="sourceEditor">Source editor</param>
        /// <param name="charPos">Char position</param>
        /// <returns>Edit position.</returns>
        public static OTAEditPos CharPosToEditPos(IOTASourceEditor sourceEditor, OTACharPos charPos)
        {
            IOTAEditView _EditView = GetEditView(sourceEditor);
            OTAEditPos _EditPos = new OTAEditPos();

            if (_EditView != null)
            {
                _EditView.ConvertPos(false, ref _EditPos, ref charPos);
            }

            return _EditPos;
        }
        /// <summary>
        /// Creates file from buffer.
        /// </summary>
        /// <param name="sourceEditor">Source editor</param>
        /// <param name="fileName">File name</param>
        public static void CreateFileFromBuffer(IOTASourceEditor sourceEditor, string fileName)
        {
            if (sourceEditor != null)
            {
                string content = GetSourceEditorText(sourceEditor);
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    // Add some text to the file.
                    sw.Write(content);
                }
            }
        }
        /// <summary>
        /// This function remove special char from string and get only one line from the string
        /// It's ensure that the line is UI Ready
        /// </summary>
        /// <param name="str">Dirty string</param>
        public static string CreatePresentationLineFromDirtyString(string str)
        {
            StringReader _Reader = new StringReader(str);

            string _Line = _Reader.ReadLine();

            // Remove Tabs
            if (_Line != null)
            {
                _Line = _Line.Replace("\t", new string(' ', OtaUtils.GetCSIndentationFromOptions()));
            }

            return _Line;
        }
        /// <summary>
        /// Fills buffer from file.
        /// </summary>
        /// <param name="sourceEditor">Source editor</param>
        /// <param name="fileName">File name</param>
        public static void FillBufferFromFile(IOTASourceEditor sourceEditor, string fileName)
        {
            string content = Lextm.IO.FileHelper.GetContentOf(fileName);
            LoggingService.Debug("The file content is " + content);
            FillBufferFromString(sourceEditor, content);
        }
        /// <summary>
        /// Fills buffer from string.
        /// </summary>
        /// <param name="sourceEditor">Source editor</param>
        /// <param name="source">Source string</param>
        public static void FillBufferFromString(IOTASourceEditor sourceEditor, string source)
        {
            if (sourceEditor != null)
            {
                IOTAFileWriter _Writer = sourceEditor.CreateWriter();
                _Writer.CopyTo(0);
                _Writer.DeleteTo(MAX_READ_SIZE);
                _Writer.Insert(source);
                _Writer.Close();
            }
        }
        /// <summary>
        /// Finds selection text.
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="source">Source</param>
        /// <param name="lastPos">Last position</param>
        /// <param name="prev">Previous</param>
        /// <param name="pos">Position</param>
        /// <returns>true if found, false if not.</returns>
        public static bool FindSelectionText(string id, string source, int lastPos, bool prev, out int pos)
        {
            IOTAEditView _EditView = GetCurrentEditView();
            OTAEditPos _EditPos = new OTAEditPos();
            _EditPos.Line = _EditView.Buffer.EditBlock.StartingRow;
            _EditPos.Col = (short)_EditView.Buffer.EditBlock.StartingColumn;

            int _StartPos = EditPosToBufferPos(GetCurrentSourceEditor(), _EditPos);

            _EditPos.Line = _EditView.Buffer.EditBlock.EndingRow;
            _EditPos.Col = (short)_EditView.Buffer.EditBlock.EndingColumn;

            //int _LastPos = EditPosToBufferPos(GetCurrentSourceEditor(), _EditPos);

            //int _Diff = lastPos - _StartPos;
            pos = -1;

            if (id.Length == 0)
            {
                return false;
            }

            id = id.ToUpperInvariant();

            while (Lextm.StringHelper.GoNext(source, ref _StartPos, prev))
            {
                if (Char.ToUpperInvariant(source[_StartPos]) == id[0])
                {
                    if (Lextm.StringHelper.IsStrCaseIns(source, _StartPos, id))
                    {
                        pos = _StartPos;
                        return true;
                    }
                }
            }

            return false;
        }
        /// <summary>
        /// Finds text ident.
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="source">Source</param>
        /// <param name="lastPos">Last position</param>
        /// <param name="prev">Previous</param>
        /// <param name="pos">Position</param>
        /// <returns>true if found, false if not.</returns>
        public static bool FindTextIdent(string id, string source, int lastPos, bool prev, out int pos)
        {
            int _StartPos = GoToBeginingOfWord(source, lastPos);
            int _Diff = lastPos - _StartPos;
            pos = -1;
            if (id.Length == 0)
            {
                return false;
            }

            id = id.ToUpperInvariant();

            while (Lextm.StringHelper.GoNext(source, ref _StartPos, prev))
            {
                if (Char.ToUpperInvariant(source[_StartPos]) == id[0])
                {
                    if (Lextm.StringHelper.IsStrCaseIns(source, _StartPos, id))
                    {
                        if (((_StartPos < 1) || (!IsWordCharacter(source[_StartPos - 1]))) &&
                            ((_StartPos + id.Length > source.Length) || (!IsWordCharacter((source[_StartPos + id.Length])))))
                        {
                            pos = _StartPos + _Diff;
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        /// <summary>
        /// Gets edit view.
        /// </summary>
        /// <param name="sourceEditor">Source editor</param>
        /// <returns></returns>
        public static IOTAEditView GetEditView(IOTASourceEditor sourceEditor)
        {
            if (sourceEditor != null && sourceEditor.EditViewCount > 0)
            {
                return sourceEditor.GetEditView(0);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Gets current edit view.
        /// </summary>
        /// <returns></returns>
        public static IOTAEditView GetCurrentEditView()
        {
            return GetEditView(GetSourceEditor(GetCurrentModule()));
        }
        /// <summary>
        /// Gets current line number.
        /// </summary>
        /// <returns>-1 on error.</returns>
        public static int GetCurrentLineNumber()
        {
            IOTAEditView _EditView = GetCurrentEditView();

            if (_EditView != null)
            {
                return _EditView.Buffer.EditPosition.Row;
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// Goes to beginning of word.
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="startPos">Start position</param>
        /// <returns></returns>
        public static int GoToBeginingOfWord(string source, int startPos)
        {
            int _Pos = startPos;

            if (_Pos >= source.Length)
            {
                return _Pos;
            }

            while ((_Pos > 1) && IsWordCharacter(source[_Pos - 1]))
            {
                _Pos--;
            }
            return _Pos;
        }
        /// <summary>
        /// Goes to end of word.
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="startPos">Start position</param>
        /// <returns></returns>
        public static int GoToEndOfWord(string source, int startPos)
        {
            int _Pos = startPos;
            while ((_Pos > 1) && (_Pos < source.Length) && IsWordCharacter(source[_Pos]))
            {
                _Pos++;
            }
            return _Pos;
        }

        /// <summary>
        /// Gets current selected text.
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentSelectedText()
        {
            return GetSelectedText(OtaUtils.GetCurrentModule());
        }
        /// <summary>
        /// Gets current word.
        /// </summary>
        /// <param name="sourceEditor">Source editor</param>
        /// <returns>String.Empty if no.</returns>
        public static string GetCurrentWord(IOTASourceEditor sourceEditor)
        {
            string _Source = GetSourceEditorText(sourceEditor);
            int _StartPos = GetSourceEditorTextPos(sourceEditor);

            if (_Source.Length == 0 || _StartPos < 0 || _StartPos >= _Source.Length)
            {
                return String.Empty;
            }

            if (IsWordCharacter(_Source[_StartPos]))
            {
                int _Begining = GoToBeginingOfWord(_Source, _StartPos);
                int _End = GoToEndOfWord(_Source, _StartPos);

                return _Source.Substring(_Begining, _End - _Begining);
            }
            else
            {
                return String.Empty;
            }
        }
        /// <summary>
        /// Verifies cursor is at end of line.
        /// </summary>
        /// <param name="sourceEditor">Source editor</param>
        /// <returns></returns>
        /// <remarks>This function is not correctly implemented.</remarks>
        public static bool CursorIsAtEndOfLine(IOTASourceEditor sourceEditor)
        {
            IOTAEditView _EditView = GetEditView(sourceEditor);
            OTAEditPos _OriginalPos = _EditView.CursorPos;
            bool result = false;
            try
            {
                _EditView.Buffer.EditPosition.MoveEOL();

                // Get the End of the Line
                OTAEditPos _EndEditPos = new OTAEditPos();
                _EndEditPos.Col = _EditView.CursorPos.Col;
                _EndEditPos.Line = _EditView.CursorPos.Line;

                result = (_EndEditPos.Col == _OriginalPos.Col);
            }
            finally
            {
                _EditView.CursorPos = _OriginalPos;
            }
            return result;
        }
        /// <summary>
        /// Gets current line.
        /// </summary>
        /// <param name="sourceEditor">Source editor</param>
        /// <returns>String.Empty if no.</returns>
        /// <remarks>
        /// <para>Spaces are kept.</para>
        /// </remarks>
        public static string GetCurrentLine(IOTASourceEditor sourceEditor)
        {
            string _Source = GetSourceEditorText(sourceEditor);

            if (_Source.Length == 0)
            {
                return String.Empty;
            }

            IOTAEditView _EditView = GetEditView(sourceEditor);
            OTAEditPos _OriginalPos = _EditView.CursorPos;
            try
            {
                _EditView.Buffer.EditPosition.MoveEOL();

                // Get the Begin of the Line
                OTAEditPos _BeginEditPos = new OTAEditPos();
                _BeginEditPos.Col = 1;
                _BeginEditPos.Line = _EditView.CursorPos.Line;

                // Get the End of the Line
                OTAEditPos _EndEditPos = new OTAEditPos();
                _EndEditPos.Col = _EditView.CursorPos.Col;
                _EndEditPos.Line = _EditView.CursorPos.Line;

                int _Begining = EditPosToBufferPos(sourceEditor, _BeginEditPos);
                int _End = EditPosToBufferPos(sourceEditor, _EndEditPos);

                return _Source.Substring(_Begining, _End - _Begining);
            }
            finally
            {
                _EditView.CursorPos = _OriginalPos;
            }
        }
        /// <summary>
        /// Gets current edit position.
        /// </summary>
        /// <param name="sourceEditor">Source editor</param>
        /// <returns>new object if wrong.</returns>
        public static OTAEditPos GetCurrentEditPos(IOTASourceEditor sourceEditor)
        {
            if (sourceEditor != null)
            {
                if (sourceEditor.EditViewCount > 0)
                {
                    IOTAEditView _EditView = sourceEditor.GetEditView(0);

                    return _EditView.CursorPos;
                }
            }

            return new OTAEditPos();
        }
        /// <summary>
        /// Gets current edit position.
        /// </summary>
        /// <returns></returns>
        public static OTAEditPos GetCurrentEditPos()
        {
            return GetCurrentEditPos(OtaUtils.GetCurrentSourceEditor());
        }

        /// <summary>
        /// Gets selected text.
        /// </summary>
        /// <param name="module">Module</param>
        /// <returns>String.Empty if wrong.</returns>
        public static string GetSelectedText(IOTAModule module)
        {
            if (module == null)
            {
                return String.Empty;
            }

            IOTASourceEditor _SourceEditor = GetSourceEditor(module);

            if (_SourceEditor != null)
            {
                IOTAEditView _EditView = _SourceEditor.GetEditView(0);

                return _EditView.Buffer.EditBlock.Text;
            }
            else
            {
                return String.Empty;
            }
        }
        /// <summary>
        /// Gets source editor text. All text.
        /// </summary>
        /// <param name="sourceEditor">Source editor</param>
        /// <returns>String.Empty if wrong.</returns>
        public static string GetSourceEditorText(IOTASourceEditor sourceEditor)
        {
            if (sourceEditor == null)
            {
                return String.Empty;
            }

            IOTAFileReader _Reader = sourceEditor.CreateReader();

            if (_Reader == null)
            {
                throw new Lextm.OpenTools.CoreException("No file reader");
            }

            System.Text.UTF8Encoding _Encoding = new System.Text.UTF8Encoding();
            StringBuilder _StringBuilder = new StringBuilder();
            Byte[] _Source = _Reader.Read(1024, 0);

            while (_Source.Length != 0)
            {
                _StringBuilder.Append(_Encoding.GetString(_Source));

                _Source = _Reader.Read(1024, 0);
            }
            //LoggingService.Info("the source is " + _StringBuilder.ToString());
            return _StringBuilder.ToString();
        }
        /// <summary>
        /// Gets source editor text position.
        /// </summary>
        /// <param name="sourceEditor">Source editor</param>
        /// <returns>-1 if wrong.</returns>
        public static int GetSourceEditorTextPos(IOTASourceEditor sourceEditor)
        {
            IOTAEditView _EditView = GetEditView(sourceEditor);

            if (_EditView != null)
            {
                OTAEditPos _EditPos = _EditView.CursorPos;
                OTACharPos _CharPos = new OTACharPos();

                _EditView.ConvertPos(true, ref _EditPos, ref _CharPos);

                return _EditView.CharPosToPos(_CharPos);
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// Gets string before first word.
        /// </summary>
        /// <returns>String.Empty if wrong.</returns>
        public static string GetStringBeforeFirstWord()
        {
            string _Text = GetCurrentSelectedText();

            if (String.IsNullOrEmpty(_Text))
            {
                return String.Empty;
            }

            int _Index = 0;

            for (int i = 0; i < _Text.Length; i++)
            {
                if (_Text[i] != '\t' && _Text[i] != ' ')
                {
                    _Index = i;
                    break;
                }
            }

            return _Text.Substring(0, _Index);
        }
        /// <summary>
        /// Goes to position.
        /// </summary>
        /// <param name="sourceEditor">Source editor</param>
        /// <param name="lineNumber">Line number</param>
        /// <param name="colNumber">column number</param>
        /// <remarks>After calling this, EditView.MoveViewToCursor() and
        ///        EditView.Paint() must be called to move the view.</remarks>
        public static void GoToPosition(IOTASourceEditor sourceEditor, int lineNumber, short colNumber)
        {
            if (sourceEditor != null)
            {
                IOTAEditView _EditView = GetEditView(sourceEditor);

                OTAEditPos _Pos = new OTAEditPos();
                _Pos.Col = colNumber;
                _Pos.Line = lineNumber;

                _EditView.CursorPos = _Pos;
            }
        }
        /// <summary>
        /// Inserts text.
        /// </summary>
        /// <param name="sourceEditor">Source editor</param>
        /// <param name="text">Text</param>
        public static void InsertText(IOTASourceEditor sourceEditor, string text)
        {
            InsertText(sourceEditor, text, 0, 0);
        }
        /// <summary>
        /// Inserts text and moves cursor.
        /// </summary>
        /// <param name="editor">Source editor</param>
        /// <param name="text">Text</param>
        /// <param name="moveRow">Move row</param>
        /// <param name="moveColumn">Move column of cursor</param>
        public static void InsertText(IOTASourceEditor editor, string text, int moveRow, int moveColumn)
        {
            IOTAEditView view = OtaUtils.GetEditView(editor);
            if (view != null)
            {
                IOTAEditPosition pos = view.Position;
                LoggingService.Debug("before insert text, column is " + pos.Column + " and row is " + pos.Row);
                pos.Save(); // save original cursor
                pos.InsertText(text);
                pos.Restore();// back to original cursor
                bool canMove = pos.MoveRelative(moveRow, moveColumn);
                if (!canMove)
                {
                    LoggingService.Error("cannot move cursor");
                }
                view.Paint();
                LoggingService.Debug("after insert text, column is " + pos.Column + " and row is " + pos.Row);
            }
        }
        /// <summary>
        /// Inserts text.
        /// </summary>
        /// <param name="text">Text</param>
        public static void InsertText(string text)
        {
            InsertText(GetCurrentSourceEditor(), text);
        }
        /// <summary>
        /// Verifies it is word char.
        /// </summary>
        /// <param name="character">Char</param>
        /// <returns></returns>
        public static bool IsWordCharacter(char character)
        {
            return Char.IsLetterOrDigit(character) || (character == '_');
        }
        /// <summary>
        /// Replaces inclusive and exclusive.
        /// </summary>
        /// <param name="sourceEditor">Source editor</param>
        /// <param name="editView">Edit view</param>
        /// <param name="isInclusive">Is inclusive flag</param>
        /// <param name="start">Start</param>
        /// <param name="after">After</param>
        /// <param name="text">Text</param>
        private static void ReplaceInclusiveExclusive(IOTASourceEditor sourceEditor,
                                                      IOTAEditView editView,
                                                      bool isInclusive,
                                                      OTACharPos start, OTACharPos
                                                      after, string text)
        {

            if (sourceEditor == null || editView == null)
            {
                return;
            }

            bool _FirstCharInLineDeleted;

            if (!isInclusive)
            {
                _FirstCharInLineDeleted = (after.CharIndex == 1);

                if (after.CharIndex > 0)
                {
                    after.CharIndex -= 1;
                }
            }
            else
            {
                _FirstCharInLineDeleted = false;
            }

            int _StartPos = editView.CharPosToPos(start);
            int _AfterPos = editView.CharPosToPos(after);

            IOTAFileWriter _Writer = sourceEditor.CreateWriter();

            _Writer.CopyTo(_StartPos);
            int _DeleteToPos = _AfterPos;

            if (after.CharIndex == 0 && ((after.Line - start.Line) == 1))
            {
                _DeleteToPos -= 2;

                if (_FirstCharInLineDeleted)
                {
                    _DeleteToPos = +3;
                }
            }
            else
            {
                if (_FirstCharInLineDeleted)
                {
                    _DeleteToPos++;
                }
                else if (after.CharIndex > 0)
                {
                    _DeleteToPos++;
                }
            }

            if (_DeleteToPos > _StartPos)
            {
                _Writer.DeleteTo(_DeleteToPos);
            }

            _Writer.Insert(text);
            _Writer.CopyTo(Int32.MaxValue);
            _Writer.Close();
        }
        /// <summary>
        /// Replaces selected text.
        /// </summary>
        /// <param name="sourceEditor">Source editor</param>
        /// <param name="lines">Lines</param>
        public static void ReplaceSelectedText(IOTASourceEditor sourceEditor, string lines)
        {
            IOTAEditView _EditView = OtaUtils.GetEditView(sourceEditor);

            if (_EditView != null)
            {
                if (_EditView.Block != null && !String.IsNullOrEmpty(_EditView.Block.Text))
                {
                    OTACharPos _Start = sourceEditor.BlockStart;
                    OTACharPos _After = sourceEditor.BlockAfter;

                    if (sourceEditor.BlockType == OTABlockType.btInclusive)
                    {
                        ReplaceInclusiveExclusive(sourceEditor, _EditView, true, _Start, _After, lines);
                    }
                    else if (sourceEditor.BlockType == OTABlockType.btNonInclusive)
                    {
                        ReplaceInclusiveExclusive(sourceEditor, _EditView, false, _Start, _After, lines);
                    }
                    else if (sourceEditor.BlockType == OTABlockType.btColumn)
                    {
                        // Column mode not supported
                        //ReplaceColumns(TOTAEditPos(_Start), TOTAEditPos(_After), Text);
                    }
                    else if (sourceEditor.BlockType == OTABlockType.btLine)
                    {
                        _Start.CharIndex = 0;
                        _After.CharIndex = 1023; // Max line length
                        ReplaceInclusiveExclusive(sourceEditor, _EditView, true, _Start, _After, lines);
                    }
                }
                else
                {
                    InsertText(sourceEditor, lines);
                }
            }
        }
        /// <summary>
        /// Selects line.
        /// </summary>
        /// <param name="sourceEditor">Source editor</param>
        public static void SelectLine(IOTASourceEditor sourceEditor)
        {
            if (sourceEditor != null)
            {
                if (sourceEditor.EditViewCount > 0)
                {
                    IOTAEditView _EditView = sourceEditor.GetEditView(0);

                    OTAEditPos _EditPos = new OTAEditPos();

                    _EditPos.Line = _EditView.Position.Row;
                    _EditPos.Col = 1;
                    _EditView.CursorPos = _EditPos;
                    _EditView.Block.BeginBlock();
                    _EditView.Block.Style = OTABlockType.btNonInclusive;

                    if (_EditView.Position.MoveEOL())
                    {
                        _EditView.MoveViewToCursor();
                    }

                    _EditView.Block.EndBlock();
                }
            }
        }
        /// <summary>
        /// Selects text from position to position.
        /// </summary>
        /// <param name="sourceEditor">Source</param>
        /// <param name="startPos">Start position</param>
        /// <param name="endPos">End position</param>
        public static void SelectTextFromPosToPos(IOTASourceEditor sourceEditor, OTAEditPos startPos, OTAEditPos endPos)
        {
            if (sourceEditor != null)
            {
                if (sourceEditor.EditViewCount > 0)
                {
                    IOTAEditView _EditView = sourceEditor.GetEditView(0);

                    _EditView.CursorPos = startPos;

                    _EditView.Block.BeginBlock();
                    _EditView.Block.Style = OTABlockType.btNonInclusive;

                    _EditView.CursorPos = endPos;
                    _EditView.MoveViewToCursor();
                    _EditView.Block.EndBlock();
                }
            }
        }
        /**************************************************************/
        /*                       Options
        /**************************************************************/

        private const int DefaultIndentation = 4;
        // Reg keys
        private const string RegKeyBorlandCSSourceOptions = @"\Editor\Source Options\Borland.EditOptions.C#";
        private const string RegKeyBorlandDelphiSourceOptions = @"\Editor\Source Options\Borland.EditOptions.Pascal";
        private const string RegKeyBorlandXmlSourceOptions = @"\Editor\Source Options\Borland.EditOptions.XML";

        /// <summary>
        /// Gets BASIC indentation from registry.
        /// </summary>
        public static int GetBasicIndentationFromOptions()
        {
            return (int)RegistryHelper.GetValueFromRegKey(IdeRegKey + RegKeyBorlandBasicSourceOptions,
                                                          @"Block Indent", DefaultIndentation);
        }

        /// <summary>
        /// Gets C# indentation from registry.
        /// </summary>
        public static int GetCSIndentationFromOptions()
        {
            return (int)RegistryHelper.GetValueFromRegKey(IdeRegKey + RegKeyBorlandCSSourceOptions,
                                                          @"Block Indent", DefaultIndentation);
        }
        /// <summary>
        /// Gets C# extensions from registry.
        /// </summary>
        /// <returns></returns>
        public static string[] GetCSExtensionsFromOptions()
        {
            return (RegistryHelper.GetValueFromRegKey(IdeRegKey + RegKeyBorlandCSSourceOptions,
                                                      @"File Extensions", "cs") as string).Split(';');
        }
        /// <summary>
        /// Gets Delphi extensions from registry.
        /// </summary>
        /// <returns></returns>
        public static string[] GetDelphiExtensionsFromOptions()
        {
            return (RegistryHelper.GetValueFromRegKey(IdeRegKey + RegKeyBorlandDelphiSourceOptions,
                                                      @"File Extensions",
                                                      "pas;dpr;dpk;inc;dfm;xfm;nfm;dpkw") as string).Split(';');
        }
        /// <summary>
        /// Gets XML extensions from registry.
        /// </summary>
        /// <returns></returns>
        public static string[] GetXmlExtensionsFromOptions()
        {
            return (RegistryHelper.GetValueFromRegKey(IdeRegKey + RegKeyBorlandXmlSourceOptions,
                                                      @"File Extensions",
                                                      "xml;xsl;xslt;xsd;wml;manifest;resx;config;wsdl;disco;map")
                    as string).Split(';');
        }

        /**************************************************************/
        /*                         Menu
        /**************************************************************/
        /// <summary>
        /// Executes a menu.
        /// </summary>
        /// <param name="menuName">Menu name</param>
        /// <returns></returns>
        public static bool ExecuteMenu(string menuName)
        {
            IOTAMainMenuService _MenuService = GetMainMenuService();

            IOTAMenuItem _MenuItem = _MenuService.GetMenuItem(menuName);

            if (_MenuItem != null)
            {
                _MenuItem.Execute();
                return true;
            }
            else
            {
                return false;
            }
        }

        /**************************************************************/
        /*                         IDE
        /**************************************************************/
        /// <summary>
        /// Gets current IDE version.
        /// </summary>
        /// <remarks>0 if failed, and 1-5 for corresponding IDE versions.</remarks>
        public static int CurrentIdeVersion
        {
            get
            {
                string regular = @"(?<1>\d+.\d+)";

                Regex r = new Regex(regular, RegexOptions.IgnoreCase |
                                    RegexOptions.Singleline | RegexOptions.Compiled);
                Match m = r.Match(IdeRegKey);
                if (m.Success)
                {
                    Version version = new Version(m.Groups[1].Value);
                    return version.Major;
                }
                else
                {
                    return 0;
                }
            }
        }

        private const string IDE_DOT_NET_FRAMEWORK_REG_KEY = @"\DotNetFramework";
        /// <summary>
        /// Gets .NET framework installation path.
        /// </summary>
        /// <returns>null if wrong.</returns>
        public static string GetDotNetFrameworkInstallationPath()
        {
            string fullPath = (string)RegistryHelper.GetValueFromRegKey(IdeRegKey + IDE_DOT_NET_FRAMEWORK_REG_KEY,
                                                                        @"CSharpCompiler", null);
            if (fullPath == null)
            {
                return null;
            }
            else
            {
                return Path.GetDirectoryName(fullPath);
            }
        }

        private static string IdeRegKeyInternal;
        /// <summary>
        /// IDE reg key base.
        /// </summary>
        /// <remarks>No / in the end.</remarks>
        public static string IdeRegKey
        {
            get
            {
                lock (typeof(OtaUtils))
                {
                    if (String.IsNullOrEmpty(IdeRegKeyInternal))
                    {
                        if (GetService() == null)
                        {
                            IdeRegKeyInternal = String.Empty;
                        }
                        else
                        {
                            string temp = GetService().BaseRegistryKey;
                            // walk around BDS 1.0 bug here.
                            if (temp.StartsWith("\\", StringComparison.Ordinal))
                            {
                                IdeRegKeyInternal = temp.Substring(1);
                            }
                            else
                            {
                                IdeRegKeyInternal = temp;
                            }
                        }
                    }
                }
                return IdeRegKeyInternal;
            }
        }
        #region Lextm extensions

        /// <summary>
        /// IDE version info list.
        /// </summary>
        public static IEnumerable<IdeVersionInfo> IdeVersionInfoCollection
        {
            get
            {
                return _ideVersionInfos;
            }
        }

        private readonly static IEnumerable<IdeVersionInfo> _ideVersionInfos = new List<IdeVersionInfo>() {
			new IdeVersionInfo(1, "C#Builder"),
			new IdeVersionInfo(2, "Delphi 8 for .NET"),
			new IdeVersionInfo(3, "Delphi 2005"),
			new IdeVersionInfo(4, "Borland Developer Studio 2006"),
			new IdeVersionInfo(5, "CodeGear RAD Studio 2007")
		};

        private const string MaskIDERegKey = @"Software\Borland\BDS\{0}.0";
        /// <summary>No / in the end.</summary>
        private static string GetIdeRegKey(int version)
        {
            return String.Format(CultureInfo.InvariantCulture, MaskIDERegKey, version);
        }
        private const string IDEEnabledAssembliesRegKey = @"\Known IDE Assemblies";
        private const string IDEDisabledAssembliesRegKey = @"\Disabled IDE Assemblies";

        private const string IDEDisabledPackagesRegKey = @"\Disabled Experts";
        private const string IDEEnabledPackagesRegKey = @"\Experts";
        /// <summary>
        /// Gets all module info of a project.
        /// </summary>
        /// <param name="project">Project</param>
        /// <returns></returns>
        /// <remarks>The real project file (*.dpr or *.dpk) is added, too.</remarks>
        public static IList<IOTAModuleInfo> GetAllModuleInfoOf(IOTAProject project)
        {
            if (project == null || project.ModuleCount == 0)
            {
                return null;
            }
            IList<IOTAModuleInfo> result = new List<IOTAModuleInfo>();
            for (int i = 0; i < project.ModuleCount; i++)
            {
                IOTAModuleInfo info = project.GetModuleInfo(i);
                result.Add(info);
            }
            if (IsDelphiProject(project))
            {
                string projectFile = OtaUtils.GetDelphiProjectFileName(project.FileName);
                if (File.Exists(projectFile))
                {
                    result.Add(new DelphiProjectModuleInfo(projectFile));
                }
            }
            return result;
        }
        /// <summary>
        /// Determines if it is a Delphi project.
        /// </summary>
        /// <param name="project">Project</param>
        /// <returns></returns>
        public static bool IsDelphiProject(IOTAProject project)
        {
            return project.Personality == OTAIDEPersonalities.sDelphiPersonality
                            || project.Personality == OTAIDEPersonalities.sDelphiDotNetPersonality;
        }
        /// <summary>
        /// Determines if it is a .NET project.
        /// </summary>
        /// <param name="project">Project</param>
        /// <returns></returns>
        public static bool IsDotNetProject(IOTAProject project)
        {
            return project.Personality == OTAIDEPersonalities.sCSharpPersonality
                || project.Personality == OTAIDEPersonalities.sDelphiDotNetPersonality
                || project.Personality == OTAIDEPersonalities.sVBPersonality;
        }

        /// <summary>Validates current is a C# project.</summary>
        /// <remarks>Only CodeGear RAD Studio 2007 is supported.</remarks>
        public static bool IsCSharpProject(IOTAProject project)
        {
            return project.Personality == OTAIDEPersonalities.sCSharpPersonality;
        }
        /// <summary>
        /// Validates if current project is VB personality.
        /// </summary>
        /// <remarks>Only CodeGear RAD Studio 2007 is supported.</remarks>
        public static bool IsVisualBasicProject(IOTAProject project)
        {
            return project.Personality == OTAIDEPersonalities.sVBPersonality;
        }

        /// <summary>Validates current is a Delphi for .NET project.</summary>
        /// <remarks>only CodeGear RAD Studio 2007 is supported.</remarks>
        public static bool IsDelphiDotNetProject(IOTAProject project)
        {
            return project.Personality == OTAIDEPersonalities.sDelphiDotNetPersonality;
        }
        /// <summary>
        /// Gets projects of a project group.
        /// </summary>
        /// <param name="solution">Project group</param>
        public static IList<IOTAProject> GetAllProjectsOf(IOTAProjectGroup solution)
        {
            if ((solution == null) || solution.ProjectCount == 0)
            {
                return null;
            }
            IList<IOTAProject> result = new List<IOTAProject>(solution.ProjectCount);
            for (int i = 0; i < solution.ProjectCount; i++)
            {
                result.Add(solution[i]);
            }
            return result;
        }
        /// <summary>
        /// Gets disabled Win32 expert reg key.
        /// </summary>
        /// <param name="version">IDE version</param>
        public static string GetDisabledPackagesRegKey(int version)
        {
            return GetIdeRegKey(version) + IDEDisabledPackagesRegKey;
        }
        /// <summary>
        /// Gets enabled Win32 expert reg key.
        /// </summary>
        /// <param name="version">IDE version</param>
        /// <returns></returns>
        public static string GetEnabledPackagesRegKey(int version)
        {
            return GetIdeRegKey(version) + IDEEnabledPackagesRegKey;
        }
        /// <summary>
        /// Gets disabled .NET expert reg key.
        /// </summary>
        /// <param name="version">IDE version</param>
        /// <returns></returns>
        public static string GetDisabledAssembliesRegKey(int version)
        {
            return GetIdeRegKey(version) + IDEDisabledAssembliesRegKey;
        }
        /// <summary>
        /// Gets enabled .NET expert reg key.
        /// </summary>
        /// <param name="version">IDE version</param>
        /// <returns></returns>
        public static string GetEnabledAssembliesRegKey(int version)
        {
            return GetIdeRegKey(version) + IDEEnabledAssembliesRegKey;
        }
        /// <summary>
        /// Gets IDE root directory.
        /// </summary>
        /// <returns></returns>
        public static string GetIdeRootDir()
        {
            return (string)RegistryHelper.GetValueFromRegKey(IdeRegKey,
                                                             @"RootDir", @"C:\Program Files\CodeGear\RAD Studio\5.0\");
        }
        #endregion

        #region Lextm extensions (partially from C#Builder Goodies)

        private const string MessageGroupName = "CBC2";
        private const string MenuItemCategory = "LeXtudio";
        private const string RegKeyBorlandCSourceOptions = @"\Editor\Source Options\Borland.EditOptions.C";
        private const string RegKeyBorlandBasicSourceOptions = @"\Editor\Source Options\Borland.EditOptions.VisualBasic";

        ///<summary>
        ///Gets IOTASplashScreenService interface.
        ///</summary>
        /// <returns>IOTASplashScreenService</returns>
        public static Borland.Studio.ToolsAPI.IOTASplashScreenService GetSplashScreenService()
        {
            return Borland.Studio.ToolsAPI.BorlandIDE.SplashScreenService;
        }
        ///<summary>
        ///Adds a message to Messages Panel.
        ///</summary>
        ///<param name="text">Message content</param>
        /// <remarks>Adds to CBC2 panel.</remarks>
        public static void AddMessage(string text)
        {
            AddMessage(text, MessageGroupName);
        }

        ///<summary>
        ///Adds a message to Messages Panel.
        ///</summary>
        ///<param name="text">Message content</param>
        /// <param name="messageGroup">Message group</param>
        public static void AddMessage(string text, string messageGroup)
        {
            Trace.Assert(text != null);
            Trace.Assert(messageGroup != null);

            string fileName = null;
            int lineNumber = 0;
            int colNumber = 0;
            string toolName = null;

            Borland.Studio.ToolsAPI.IOTAMessageService messageService = GetMessageService();
            if (messageService != null)
            {
                Borland.Studio.ToolsAPI.IOTAMessageGroup group = messageService.GetGroup(messageGroup);
                if (group == null)
                {
                    group = messageService.AddMessageGroup(messageGroup);
                    if (CurrentIdeVersion > 2)
                    {
                        group.AutoScroll = true;
                    }
                }
                messageService.ShowMessageView(group);

                IntPtr ptr = IntPtr.Zero;

                messageService.AddToolMessage(fileName,
                                              text,
                                              toolName,
                                              lineNumber,
                                              colNumber,
                                              IntPtr.Zero,
                                              out ptr,
                                              group);
            }
        }
        /// <summary>
        /// Is a C file.
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns></returns>
        public static bool IsCFile(string fileName)
        {
            return CheckExtension(Path.GetExtension(fileName), GetCExtensionsFromOptions());
        }
        /// <summary>
        /// Is a BASIC file.
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns></returns>
        public static bool IsBasicFile(string fileName)
        {
            return CheckExtension(Path.GetExtension(fileName), GetBasicExtensionsFromOptions());
        }
        /// <summary>
        /// Is a document file.
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns></returns>
        public static bool IsDocFile(string fileName)
        {
            return CheckExtension(Path.GetExtension(fileName), GetDocExtensionsFromOptions());
        }
        ///<summary>
        ///Verifies a C or CS file.
        ///</summary>
        ///<param name="fileName">Name of the file</param>
        ///<returns>
        ///true if it is a C or CS file, false if not.
        ///</returns>
        public static bool IsCOrCSFile(string fileName)
        {
            Trace.Assert(fileName != null);
            return IsCSFile(fileName) || IsCFile(fileName);
        }

        ///<summary>
        ///Gets C file extensions from registry.
        ///</summary>
        ///<returns>
        ///The string array of preset C file extensions.
        ///</returns>
        public static string[] GetCExtensionsFromOptions()
        {
            return (RegistryHelper.GetValueFromRegKey(IdeRegKey + RegKeyBorlandCSourceOptions,
                                                      @"File Extensions",
                                                      "cpp;c;cc;hpp;h;hh;cxx;hxx;bpkw") as string).Split(';');
        }
        ///<summary>
        ///Gets BASIC file extensions from registry.
        ///</summary>
        ///<returns>
        ///The string array of preset BASIC file extensions.
        ///</returns>
        public static string[] GetBasicExtensionsFromOptions()
        {
            return (RegistryHelper.GetValueFromRegKey(IdeRegKey + RegKeyBorlandBasicSourceOptions,
                                                      @"File Extensions", @"vb") as string).Split(';');
        }
        private static string[] GetDocExtensionsFromOptions()
        {
            string temp = "html;txt;htm;rtf;doc";
            return temp.Split(';');
        }
        ///<summary>
        ///Gets C file indentation from registry.
        ///</summary>
        ///<returns>
        ///Integer number of indentation.
        ///</returns>
        public static int GetCIndentationFromOptions()
        {
            return (int)RegistryHelper.GetValueFromRegKey(IdeRegKey + RegKeyBorlandCSourceOptions,
                                                          @"Block Indent", DefaultIndentation);
        }

        ///<summary>
        ///Gets Delphi file indentation from registry.
        ///</summary>
        ///<returns>
        ///Integer number of indentation.
        ///</returns>
        public static int GetDelphiIndentationFromOptions()
        {
            return (int)RegistryHelper.GetValueFromRegKey(IdeRegKey + RegKeyBorlandDelphiSourceOptions,
                                                          @"Block Indent", DefaultIndentation);
        }
        ///<summary>
        ///Gets XML file indentation from registry.
        ///</summary>
        ///<returns>
        ///Integer number of indentation.
        ///</returns>
        public static int GetXmlIndentationFromOptions()
        {
            return (int)RegistryHelper.GetValueFromRegKey(IdeRegKey + RegKeyBorlandXmlSourceOptions,
                                                          @"Block Indent", DefaultIndentation);
        }

        private static Borland.Studio.ToolsAPI.IOTAMenuItem AddMenuItem(string parentName, Borland.Studio.ToolsAPI.OTAMenuItemLocation location, string name, string text, string imageName)
        {

            LoggingService.EnterMethod();
            Borland.Studio.ToolsAPI.IOTAMainMenuService menuService = GetMainMenuService();
            IOTAMenuItem item = null;

            if (menuService != null)
            {

                IntPtr ptr = Lextm.Drawing.ImageLoader.GetImageDataPtr(imageName);

                if ((String.IsNullOrEmpty(name))
                    || ((menuService.GetMenuItem(name) == null)))
                {
                    // avoid created menus but create separators.
                    item = menuService.AddMenuItem(parentName,
                                                   location,
                                                   name,
                                                   text,
                                                   ptr);

                    if (ptr != IntPtr.Zero)
                    {
                        // only action with a bitmap can be listed in the category
                        item.Category = MenuItemCategory;
                    }
                }
                else
                {
                    LoggingService.Warn(String.Format(CultureInfo.InvariantCulture,
                                                      "existed menu item {0}",
                                                      name));
                }
            }
            else
            {
                LoggingService.Warn("null service");
            }
            LoggingService.LeaveMethod();

            return item;
        }

        ///<summary>Inserts a menu item with bitmap/icon</summary>
        /// <param name="parentName">Name of its parent menu item</param>
        /// <param name="location">Location related to the parent</param>
        /// <param name="name">Name of the menu item</param>
        /// <param name="text">Text shown</param>
        /// <param name="executed">Envent handler</param>
        /// <param name="visible">Flag of visible</param>
        ///<param name="enabled">Flag of enabled</param>
        /// <param name="isChecked">Check state</param>
        /// <param name="imageName">Image name</param>
        public static IOTAMenuItem AddMenuItem(
            string parentName,
            OTAMenuItemLocation location,
            string name,
            string text,
            EventHandler executed,
            bool visible,
            bool enabled,
            bool isChecked,
            string imageName)
        {
            Trace.Assert(parentName != null);
            Trace.Assert(name != null);
            Trace.Assert(text != null);
            Trace.Assert(imageName != null);

            IOTAMenuItem item = AddMenuItem(parentName, location, name, text, imageName);
            if (item != null)
            {
                item.Enabled = enabled;
                item.Visible = visible;
                item.Executed += executed;
                item.Checked = isChecked;
            }
            else
            {
                LoggingService.Warn("null menu item");
            }
            return item;
        }

        ///<summary>
        ///Calculates shortcut value from key code
        ///</summary>
        ///<param name="e">KeyEventArgs</param>
        ///<returns>
        ///Valid shortcut value if the keystroke is valid, 0 if not.
        ///</returns>
        public static int GetShortcutFromKeys(System.Windows.Forms.KeyEventArgs e)
        {
            Trace.Assert(e != null);
            int result = 0;
            try
            {
                if ((e.KeyCode != Keys.Menu) && (e.KeyCode != Keys.ControlKey) &&
                    (e.KeyCode != Keys.ShiftKey))
                {  // when a normal key is down
                    int shortCut = Convert.ToInt32(e.KeyCode, CultureInfo.InvariantCulture);
                    if (e.Alt)
                    {
                        shortCut |= BeWise.Common.Utils.OtaUtils.Alt;
                    }
                    if (e.Shift)
                    {
                        shortCut |= BeWise.Common.Utils.OtaUtils.Shift;
                    }
                    if (e.Control)
                    {
                        shortCut |= BeWise.Common.Utils.OtaUtils.Ctrl;
                    }
                    result = shortCut;
                }
                else
                { // no normal key so invalid combination.
                    result = 0;
                }
            }
            catch (InvalidCastException ex)
            {
                Lextm.Windows.Forms.MessageBoxFactory.Fatal(ex);
            }
            return result;
        }
        ///<summary>
        ///Calculates key text from shortcut value
        ///</summary>
        ///<param name="shortcut">Shortcut value</param>
        ///<returns>
        ///String that stands up the keystroke.
        ///</returns>
        public static string GetKeysTextFromShortcut(int shortcut)
        {
            int keyCode = shortcut;
            System.Text.StringBuilder result = new System.Text.StringBuilder();

            if ((shortcut & BeWise.Common.Utils.OtaUtils.Alt) == BeWise.Common.Utils.OtaUtils.Alt)
            {
                keyCode ^= BeWise.Common.Utils.OtaUtils.Alt;
                result.Insert(0, "Alt + ");
            }
            if ((shortcut & BeWise.Common.Utils.OtaUtils.Shift) == BeWise.Common.Utils.OtaUtils.Shift)
            {
                keyCode ^= BeWise.Common.Utils.OtaUtils.Shift;
                result.Insert(0, "Shift + ");
            }
            if ((shortcut & BeWise.Common.Utils.OtaUtils.Ctrl) == BeWise.Common.Utils.OtaUtils.Ctrl)
            {
                keyCode ^= BeWise.Common.Utils.OtaUtils.Ctrl;
                result.Insert(0, "Ctrl + ");
            }
            // both methods work.
            result.Append(Enum.GetName(typeof(Keys), keyCode));
            //result.Append(Convert.ToChar(keyCode));

            return result.ToString();
        }
        ///<summary>
        /// Registers splash screen service.
        /// </summary>
        /// <param name="assembly">Assembly</param>
        /// <param name="name">Image name</param>
        public static void RegisterSplashScreen(System.Reflection.Assembly assembly, string name)
        {
            Trace.Assert(name != null);
            IOTASplashScreenService splashService = GetSplashScreenService();
            if (splashService != null)
            {
                splashService.AddPluginBitmap(
                        Lextm.Reflection.AssemblyHelper.GetTitle(assembly),
                        Lextm.Drawing.ImageLoader.GetImageDataPtr(name),
                        false,  //isUnregistered
                        Lextm.Reflection.AssemblyHelper.GetConfiguration(assembly),  //licenseStatus
                        "Standard"); //skuName
            }
            else
            {
                LoggingService.Warn("null splash service");
            }
        }

        ///<summary>
        /// Registers about box service by name.
        /// </summary>
        /// <param name="assembly">Assembly</param>
        /// <param name="name">Image name</param>
        /// <exception cref="AddTwiceException">Expert is loaded twice.</exception>
        public static void RegisterAboutBox(System.Reflection.Assembly assembly, string name)
        {

            Trace.Assert(name != null);
            if (PlugInIndex > -1)
            {
                throw new AddTwiceException("You cannot add about box twice.");
            }
            IOTAAboutBoxService aboutBoxService = GetAboutBoxService();
            if (aboutBoxService != null)
            {
                PlugInIndex = aboutBoxService.AddPluginInfo(
                        Lextm.Reflection.AssemblyHelper.GetTitle(assembly),  //Title
                        Lextm.Reflection.AssemblyHelper.GetDescription(assembly),  //Description
                        Lextm.Drawing.ImageLoader.GetImageDataPtr(name),
                        false,  //isUnregistered
                        Lextm.Reflection.AssemblyHelper.GetConfiguration(assembly),  //LicenseStatus
                        "Standard"); //SkuName
            }
            else
            {
                LoggingService.Warn("null about box service");
            }
        }
        /// <summary>
        /// Gets a line string of a certain line number.
        /// </summary>
        /// <param name="number">Line number</param>
        /// <returns>null if wrong.</returns>
        public static string GetLineOf(int number)
        {
            return GetLineOf(GetCurrentSourceEditor(), number);
        }
        /// <summary>
        /// Gets a line string of a certain line number.
        /// </summary>
        /// <param name="sourceEditor">Source editor</param>
        /// <param name="number">Line number</param>
        /// <returns>null if wrong.</returns>
        public static string GetLineOf(IOTASourceEditor sourceEditor, int number)
        {
            if (sourceEditor == null || number < 0 || number >= sourceEditor.LinesInBuffer)
            {
                return null;
            }
            IOTAEditView view = sourceEditor.GetEditView(0);
            if (view == null)
            {
                return null;
            }
            IOTAEditPosition pos = view.Buffer.EditPosition;
            pos.Save();
            pos.GotoLine(number);
            pos.MoveEOL();
            int lastCol = pos.Column;
            pos.MoveBOL();
            int firstCol = pos.Column;
            string result = pos.Read(lastCol - firstCol + 1);
            pos.Restore();
            return result.Split(new char[] { '\r', '\n' })[0];
        }
        //		/// <summary>
        //		/// Gets current line.
        //		/// </summary>
        //		/// <param name="sourceEditor">Source editor</param>
        //		/// <param name="lineNumber">Line number</param>
        //		/// <returns>Current line.</returns>
        //		/// <remarks>
        //		/// <para>Ported from C#Builder Goodies.</para>
        //		/// <para>Spaces are kept.</para>
        //		/// </remarks>
        //		[]
        //		public static string GetCurrentLine(IOTASourceEditor sourceEditor, int lineNumber) {
        //			//TODO : change it to getlineof
        //			if (sourceEditor == null) {
        //				return String.Empty;
        //            }
        //			string source = GetSourceEditorText(sourceEditor);
        //			if (source.Length == 0 || lineNumber < 0 || lineNumber >= sourceEditor.LinesInBuffer)
        //			{//GetTotalLines(sourceEditor)) {
        //				return String.Empty;
        //			} else {
        //				string[] sourceLines = ShareUtils.GetLinesFromString(source);
        //				//LoggingService.Info("There are " + sourceLines.Length.ToString() + " lines.");
        //				//LoggingService.Info("the line is " + sourceLines[0]);
        //				//string theLine = sourceLines[lineNumber - 1];
        //				return sourceLines[lineNumber - 1];
        //			}
        //		}
        /// <summary>
        /// Gets total lines count.
        /// </summary>
        /// <param name="sourceEditor">Source editor</param>
        /// <returns>Last row count or 0.</returns>
        public static int GetTotalLines(IOTASourceEditor sourceEditor)
        {
            Trace.Assert(sourceEditor != null);
            if (sourceEditor.EditViewCount > 0)
            {
                //IOTAEditView editView = sourceEditor.GetEditView(0);
                return sourceEditor.LinesInBuffer;
                //
                //				IOTAEditPosition editPos = editView.Position;
                //				return editPos.LastRow;
            }
            else
            {
                return 0;
            }
        }

        private static int PlugInIndex = -1;
        /// <summary>
        /// Reloads a file.
        /// </summary>
        /// <param name="fileName">File name</param>
        public static void Reload(string fileName)
        {
            Trace.Assert(fileName != null);

            Borland.Studio.ToolsAPI.IOTAActionService _ActionService = OtaUtils.GetActionService();
            if (_ActionService != null)
            {
                _ActionService.ReloadFile(fileName);
            }
        }
        /// <summary>
        /// Saves a file in IDE.
        /// </summary>
        /// <param name="fileName">File name</param>
        public static void Save(string fileName)
        {
            Trace.Assert(fileName != null);

            Borland.Studio.ToolsAPI.IOTAActionService _ActionService = OtaUtils.GetActionService();
            if (_ActionService != null)
            {
                _ActionService.SaveFile(fileName);
            }
        }
        ///<summary>
        ///Validates a menu item.
        ///</summary>
        ///<param name="name">Name</param>
        ///<returns>
        ///true if the menu item exists, false if not.
        ///</returns>
        public static bool MenuExists(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                LoggingService.Warn("null name");
                return true;
                //Why I return true here? wield.
                //There must be some noname items in IDE.
            }
            else
            {

                IOTAMainMenuService _MainMenu = GetMainMenuService();
                if (_MainMenu != null)
                {
                    return (_MainMenu.GetMenuItem(name) != null);
                }
                else
                {
                    LoggingService.Warn("null menu service");
                    return false;
                }
            }
        }
        /// <summary>
        /// Gets char before cursor.
        /// </summary>
        /// <param name="sourceEditor">Source editor</param>
        /// <returns>Char.MinValue if wrong.</returns>
        public static char GetCharBeforeCursor(IOTASourceEditor sourceEditor)
        {

            LoggingService.EnterMethod();
            char result = char.MinValue;

            if (sourceEditor != null)
            {
                if (sourceEditor.EditViewCount > 0)
                {
                    IOTAEditView _EditView = sourceEditor.GetEditView(0);

                    IOTAEditPosition _EditPos = _EditView.Position;
                    _EditPos.Save();

                    bool canMove = _EditPos.MoveRelative(0, -1);
                    if (canMove) // no content
                    {
                        result = _EditPos.Character;
                        LoggingService.Info("get char result is " + result.ToString());
                    }
                    else
                    {
                        LoggingService.Warn("cannot move, may it is the beginning of a line");
                    }
                    _EditPos.Restore();
                }
                else
                {
                    LoggingService.Warn("no view");
                }
            }
            else
            {
                LoggingService.Warn("null editor");
            }
            LoggingService.LeaveMethod();

            return result;
        }
        /// <summary>
        /// Gets char after cursor.
        /// </summary>
        /// <param name="sourceEditor">Source editor</param>
        /// <returns>Char.MinValue if wrong.</returns>
        public static char GetCharAfterCursor(IOTASourceEditor sourceEditor)
        {

            LoggingService.EnterMethod();
            char result = char.MinValue;

            if (sourceEditor != null)
            {
                if (sourceEditor.EditViewCount > 0)
                {
                    IOTAEditView _EditView = sourceEditor.GetEditView(0);

                    IOTAEditPosition _EditPos = _EditView.Position;
                    //_EditPos.Save();

                    //					bool canMove = _EditPos.MoveRelative(0, -1);
                    //					if (canMove) // no content
                    //					{
                    result = _EditPos.Character;
                    LoggingService.Info("get char result is " + result.ToString());
                    //} else {
                    //	LoggingService.Warn("cannot move, may it is the beginning of a line");
                    //}
                    //_EditPos.Restore();
                }
                else
                {
                    LoggingService.Warn("no view");
                }
            }
            else
            {
                LoggingService.Warn("null editor");
            }
            LoggingService.LeaveMethod();

            return result;
        }
        /// <summary>
        /// Invokes Code Insight.
        /// </summary>
        public static void InvokeCodeInsight()
        {
            InvokeCodeInsight(false);
        }
        /// <summary>
        /// Invokes Code Insight.
        /// </summary>
        /// <param name="forced">Forced mode or not</param>
        public static void InvokeCodeInsight(bool forced)
        {

            LoggingService.EnterMethod();
            IOTAEditView _EditView = OtaUtils.GetCurrentEditView();

            if (_EditView != null)
            {
                IOTAEditActions _EditActions = _EditView as IOTAEditActions;

                if (_EditActions != null)
                {
                    try
                    {
                        if (forced)
                        {
                            _EditActions.CodeCompletion(OTACodeCompleteStyle.csManual);
                            LoggingService.Info("manual");
                        }
                        else
                        {
                            _EditActions.CodeCompletion(OTACodeCompleteStyle.csCodelist);
                        }
                        LoggingService.Debug("invoke code insight");
                    }
                    catch
                    {
                        LoggingService.Error("Invoke causes exception");
                    }
                }
                else
                {
                    LoggingService.Warn("null action");
                }
            }
            else
            {
                LoggingService.Warn("null view");
            }
            LoggingService.LeaveMethod();

        }
        /// <summary>
        /// Invokes Live Template system.
        /// </summary>
        public static void InvokeLiveTemplate()
        {

            LoggingService.EnterMethod();

            IOTAEditView _EditView = OtaUtils.GetCurrentEditView();

            if (_EditView != null)
            {
                IOTAEditActions _EditActions = _EditView as IOTAEditActions;

                if (_EditActions != null)
                {
                    try
                    {
                        _EditActions.CodeTemplate();
                        LoggingService.Info("invoke live template");
                    }
                    catch
                    {
                        LoggingService.Error("invoke causes exception");
                    }
                }
                else
                {
                    LoggingService.Warn("null action");
                }
            }
            else
            {
                LoggingService.Warn("null view");
            }
            LoggingService.LeaveMethod();

        }

        //		/// <summary>
        //		/// Gets word before cursor.
        //		/// </summary>
        //		/// <param name="sourceEditor">Source editor</param>
        //		/// <remarks>
        //		/// <para>Special chars ahead are ignored. </para>
        //		/// <para>If space is before cursor return String.Empty.</para>
        //		///  <para>If there are some chars after cursor return String.Empty.</para>
        //		/// </remarks>
        //		/// <returns>String.Empty if wrong.</returns>
        //		[]
        //		public static string GetWordBeforeCursor2(IOTASourceEditor sourceEditor) {
        //			string result = String.Empty;
        //			if (sourceEditor != null) {
        //				if (sourceEditor.EditViewCount > 0) {
        //					IOTAEditView _EditView = sourceEditor.GetEditView(0);
        //
        //					IOTAEditPosition _EditPos = _EditView.Position;
        //					if (Char.IsWhiteSpace(_EditPos.Character)) {
        //						int _ActualColumn = _EditPos.Column;
        //						int _ActualRow = _EditPos.Row;
        //
        //						int _WordLength = 0;
        //
        //						bool canMove = _EditPos.MoveRelative(0, -1);
        //						if (canMove) // no content
        //						{
        //							while (canMove && _EditView.Position.IsWordCharacter) {
        //								_WordLength++;
        //								canMove = _EditPos.MoveRelative(0, -1);
        //							}
        //							result = _EditView.Position.Read(_WordLength + 1).Trim();
        //							_EditView.Position.Move(_ActualRow, _ActualColumn);
        //						} else {
        //							LoggingService.Warn("no content");
        //						}
        //					} else {
        //						LoggingService.Warn("some char on the right");
        //					}
        //				} else {
        //					LoggingService.Warn("no view");
        //				}
        //			} else {
        //				LoggingService.Warn("null editor");
        //			}
        //			return result;
        //		}

        private OtaUtils() { }
        #endregion

        #region CSBuilder Goodies extensions
        /// <summary>
        /// Gets word before cursor.
        /// </summary>
        /// <param name="sourceEditor">Source editor</param>
        /// <remarks>Special chars are included.</remarks>
        /// <returns>String.Empty if wrong.</returns>
        public static string GetWordBeforeCursor(IOTASourceEditor sourceEditor)
        {
            LoggingService.EnterMethod();
            if (sourceEditor != null)
            {
                if (sourceEditor.EditViewCount > 0)
                {
                    IOTAEditView _EditView = sourceEditor.GetEditView(0);

                    IOTAEditPosition _EditPos = _EditView.Position;
                    _EditPos.Save();
                    string _TheWord = String.Empty;
                    int _WordLength = 0;

                    if (!_EditPos.MoveRelative(0, -1)) // no content
                    {
                        _EditPos.Restore();
                        LoggingService.LeaveMethod();
                        return String.Empty;
                    }
                    while (_EditView.Position.IsWhitespace)
                    {
                        if (!_EditPos.MoveRelative(0, -1))
                        {
                            _EditPos.Restore();
                            LoggingService.LeaveMethod();
                            return String.Empty;
                        }
                    } // slip all whitespaces
                    while (_EditPos.IsWordCharacter || _EditPos.IsSpecialCharacter)
                    {
                        _WordLength++;
                        if (!_EditPos.MoveRelative(0, -1))
                        {
                            break;
                        }
                    }
                    _TheWord = _EditPos.Read(_WordLength + 1).Trim();
                    _EditPos.Restore();
                    LoggingService.Info(_TheWord);
                    LoggingService.LeaveMethod();
                    return _TheWord;
                }
            }
            LoggingService.LeaveMethod();
            return String.Empty;
        }
        /// <summary>
        /// Deletes backspace.
        /// </summary>
        /// <param name="sourceEditor">Source editor</param>
        /// <param name="length">Length</param>
        public static void BackspaceDelete(IOTASourceEditor sourceEditor, int length)
        {
            if (sourceEditor != null)
            {
                if (sourceEditor.EditViewCount > 0)
                {
                    IOTAEditView _EditView = sourceEditor.GetEditView(0);

                    IOTAEditPosition _EditPos = _EditView.Position;
                    _EditPos.BackspaceDelete(length);
                }
            }
        }

        #endregion
        #region ArtCSB extensions
        /// <summary>
        /// Gets current editor.
        /// </summary>
        /// <returns>null if no current module or start page is showing.</returns>
        public static IOTAEditor GetCurrentEditor()
        {

            IOTAModule module = GetCurrentModule();
            if ((module == null) || (module.FileName.ToUpperInvariant() == "DEFAULT.HTM"))
            {
                return null;
            }
            else
            {
                return module.CurrentEditor;
            }

        }
        /// <summary>
        /// Get current source editor text.
        /// </summary>
        /// <returns>String.Empty if wrong.</returns>
        public static string GetCurrentSourceEditorText()
        {

            IOTASourceEditor sourceEditor = GetCurrentSourceEditor();
            if (sourceEditor == null)
            { return String.Empty; }

            IOTAFileReader fileReader = sourceEditor.CreateReader();

            if (fileReader == null)
                throw new Lextm.OpenTools.CoreException("No file reader");

            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            StringBuilder stringBuilder = new StringBuilder();
            Byte[] source = fileReader.Read(1024, 0);

            while (source.Length != 0)
            {
                stringBuilder.Append(encoding.GetString(source));

                source = fileReader.Read(1024, 0);
            }

            return stringBuilder.ToString();

        }
        /// <summary>
        /// Gets current edit actions.
        /// </summary>
        /// <returns></returns>
        public static IOTAEditActions GetCurrentEditActions()
        {
            return GetCurrentEditView() as IOTAEditActions;
            //return (IOTAEditActions)BorlandIDE.GetService(typeof(IOTAEditActions));
        }
        /// <summary>Gets current elide actions.</summary>
        public static IOTAElideActions GetCurrentElideActions()
        {
            return GetCurrentEditView() as IOTAElideActions;

        }
        /// <summary>
        /// Gets current .NET module.
        /// </summary>
        /// <returns></returns>
        public static IOTADotNetModule GetCurrentDotNetModule()
        {

            IOTADotNetModule _dotNetModule =
                (IOTADotNetModule)GetModuleServices().CurrentModule.
                GetService(typeof(IOTADotNetModule));
            return _dotNetModule;

        }
        /// <summary>
        /// Gets selection service.
        /// </summary>
        /// <returns></returns>
        /// <remarks>Selection service is related to Designer.</remarks>
        public static ISelectionService GetSelectionService()
        {

            ISelectionService _selectionService =
                (ISelectionService)GetCurrentDotNetModule().DesignerHost.
                RootComponent.Site.GetService(typeof(ISelectionService));
            return _selectionService;

        }
        /// <summary>
        /// Gets toolbox service.
        /// </summary>
        /// <returns></returns>
        public static IToolboxService GetToolboxService()
        {
            IToolboxService _toolboxService = (IToolboxService)BorlandIDE.
                GetService(typeof(IToolboxService));
            return _toolboxService;
        }
        #endregion
        /// <summary>
        /// Gets the real project source file behind dproj.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static string GetDelphiProjectFileName(string project)
        {
            string content = File.ReadAllText(project, Encoding.UTF8);
            Match m = regex.Match(content);
            Debug.Assert(m.Success);
            string file = m.Groups["source_name"].Value;
            return Path.Combine(Path.GetDirectoryName(project), file);
        }

        readonly static Regex regex = new Regex(
              "<DelphiCompile\\s+Include=\"(?<source_name>.+)\">\\s*<MainSo" +
              "urce>MainSource</MainSource>\\s*</DelphiCompile>",
            RegexOptions.CultureInvariant
            | RegexOptions.Compiled
            );
        /// <summary>
        /// Sets menu item check state.
        /// </summary>
        /// <param name="menuName">Menu name</param>
        /// <param name="newState">New state</param>
        public static void SetMenuItemCheckState(string menuName, bool newState)
        {
            IOTAMainMenuService service = OtaUtils.GetMainMenuService();
            if (service != null)
            {
                service.GetMenuItem(menuName).Checked = newState;
            }
        }
    }

	/// <summary>
	/// IDE version info type.
	/// </summary>
	public class IdeVersionInfo
	{
		private int version;
		/// <summary>
		/// IDE version.
		/// </summary>
		public int Version
		{
			get
			{
				return version;
			}
		}
		private string name;
		/// <summary>
		/// IDE title.
		/// </summary>
		public string Name
		{
			get
			{
				return name;
			}
		}
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="version">IDE version</param>
		/// <param name="name">IDE title</param>
		internal IdeVersionInfo(int version, string name)
		{
			this.version = version;
			this.name = name;
		}
	}

	///<summary>
	///Exception thrown when this expert is loaded twice.
	///</summary>
	[System.Serializable]
	public sealed class AddTwiceException : System.Exception
	{

		///<summary>
		///Protected constructor.
		///</summary>
		private AddTwiceException(System.Runtime.Serialization.SerializationInfo si, System.Runtime.Serialization.StreamingContext sc) : base(si, sc) { }

		///<summary>
		///Default constructor
		///</summary>
		public AddTwiceException() { }

		///<summary>
		///Constructor with message
		///</summary>
		///<param name="message">Message</param>
		public AddTwiceException(string message)
			: base(message) { }

		///<summary>
		///Constructor with message and inner exception
		///</summary>
		///<param name="message">Message</param>
		///<param name="inner">Inner exception</param>
		public AddTwiceException(string message, System.Exception inner)
			: base(message, inner) { }
	}
}
