using System;
using System.CodeDom;
using System.ComponentModel.Design;
using System.IO;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using Borland.Studio.ToolsAPI;
using BeWise.Common;
using BeWise.Common.Info;

namespace BeWise.Common.Utils {

    public enum BDSVersion {v10, v20, v30}

    public class OTAUtils {
		private static string ideRegKey = "";

        /**************************************************************/
        /*                        Private
        /**************************************************************/

        private static bool CheckExtension(string aExt, string[] aExtensions) {
            if (aExt == null || aExt == "") {
                return false;
            }

            string _Ext;

			if (aExt.StartsWith(".")) {
                _Ext = aExt.Substring(1).ToUpper();
            } else {
                _Ext = aExt.ToUpper();
            }

            foreach (string _Str in aExtensions) {

                if (_Str.Trim().ToUpper() == _Ext) {
                    return true;
                }
            }

            return false;
		}

        private const int MAX_READ_SIZE = 9999999;
        
        /**************************************************************/
        /*                        Public Const
        /**************************************************************/

        public const int ALT   = 32768;  // 1000000000000000 == 0X8000
        public const int CTRL  = 16384;  //  100000000000000 == 0X4000
        public const int SHIFT = 8192;   //   10000000000000 == 0X2000

        /**************************************************************/
        /*                       Services
        /**************************************************************/

        public static IOTAAboutBoxService GetAboutBoxService() {
            return (IOTAAboutBoxService) BorlandIDE.GetService(typeof(IOTAAboutBoxService));
        }

        public static IOTAActionService GetActionService() {
            return (IOTAActionService) BorlandIDE.GetService(typeof(IOTAActionService));
        }

		public static IOTAGalleryCategoryManager GetGalleryCategoryManager() {
			return (IOTAGalleryCategoryManager) BorlandIDE.GetService(typeof(IOTAGalleryCategoryManager));
		}

		public static IOTAComponentInstallService GetComponentInstallService() {
			return (IOTAComponentInstallService) BorlandIDE.GetService(typeof(IOTAComponentInstallService));
		}

		public static IHelpService GetHelpService() {
			return (IHelpService) BorlandIDE.GetService(typeof(IHelpService));
		}

		public static IOTAMainMenuService GetMainMenuService() {
			return (IOTAMainMenuService) BorlandIDE.GetService(typeof(IOTAMainMenuService));
		}

		public static IOTAMessageService GetMessageService() {
			return (IOTAMessageService) BorlandIDE.GetService(typeof(IOTAMessageService));
		}

		public static IOTAModuleServices GetModuleServices() {
			return (IOTAModuleServices) BorlandIDE.GetService(typeof(IOTAModuleServices));
		}

		public static IOTAService GetService() {
			return  (IOTAService) BorlandIDE.GetService(typeof(IOTAService));
        }

        public static IOTAWizardService GetWizardService() {
            return  (IOTAWizardService) BorlandIDE.GetService(typeof(IOTAWizardService));
        }

        /**************************************************************/
        /*                         FileName
        /**************************************************************/

        public static string[] GetProjectFileList(string aExtension) {
            ArrayList _List = new ArrayList();

            IOTAProjectGroup _ProjectGroup = OTAUtils.GetCurrentProjectGroup();
            IOTAProject _Project = _ProjectGroup.ActiveProject;

            for (int k = 0; k < _Project.ModuleCount; k++) {
                IOTAModuleInfo _ModuleInfo = _Project.GetModuleInfo(k);

                if (_ModuleInfo.FileName != null && _ModuleInfo.FileName.ToUpper().EndsWith(aExtension.ToUpper())) {
                    _List.Add(_ModuleInfo.FileName);
                }
            }

            return (string[]) _List.ToArray(typeof(string));
		}

        public static string GetCurrentModuleFileName() {
            IOTAModule _Module = OTAUtils.GetCurrentModule();

            if (_Module == null) {
                return null;
            } else {
                return _Module.FileName;
            }
        }

        public static string GetCurrentEditorFileName() {
            IOTAModule _Module = OTAUtils.GetCurrentModule();

			if (_Module == null){
				return null;
            } else {
                IOTAEditor _Editor = GetCurrentEditor(_Module);

                if (_Editor == null) {
                    return null;
                }

                // BUG in the ToolsAPI for the VCL:
                if (Utils.StringIsNullOrEmpty(_Editor.FileName)) {
                    return _Module.FileName;
                }

                return _Editor.FileName;
            }
        }

        /**************************************************************/
        /*                         Project Group
        /**************************************************************/

        public static IOTAProjectGroup GetCurrentProjectGroup() {
            IOTAModuleServices _ModuleServices = GetModuleServices();

            IOTAProjectGroup _ProjectGroup = _ModuleServices.MainProjectGroup;

            return _ProjectGroup;
        }

		/**************************************************************/
		/*                         Elide
        /**************************************************************/
        public static void ElideNearestBlock(IOTAEditView aEditView, bool aUnElide) {
            if (aEditView != null) {
                IOTAElideActions aElideActions = aEditView as IOTAElideActions;

                if (aElideActions != null) {
                    if (aUnElide) {
                        aElideActions.UnElideNearestBlock();
                    } else {
                        aElideActions.ElideNearestBlock();
                    }
                }
            }
		}

        /**************************************************************/
        /*                         Project
        /**************************************************************/

        public static IOTAProject GetCurrentProject() {
            IOTAProject _Project = null;
            IOTAProjectGroup _ProjectGroup = GetCurrentProjectGroup();

            if (_ProjectGroup != null && _ProjectGroup.ProjectCount > 0) {
                _Project = _ProjectGroup.ActiveProject;
            }

            return _Project;
        }

        public static IOTAModuleInfo GetModuleInfoFromProject(IOTAProject aProject, string aFileName) {
            for (int i = 0; i < aProject.ModuleCount; i++) {
                if (aProject.GetModuleInfo(i).FileName == aFileName) {
                    return aProject.GetModuleInfo(i);
                }
            }

            return null;
        }

        public static void RemoveFromProject(IOTAProject aProject, string aFileName) {
            MessageBox.Show("Not yet implmented in the Borlant OTA");
        }

		public static string GetProjectTarget(IOTAProject aProject) {
            if (aProject != null) {
                String _TargetName = aProject.ProjectOptions.TargetName;

                return _TargetName;
            }

            return null;
        }

        /**************************************************************/
        /*                         Modules
        /**************************************************************/

		public static bool CurrentModuleIsCSFile() {
			string _FileName = GetCurrentEditorFileName();

            if (Utils.StringIsNullOrEmpty(_FileName)) {
                return false;
            } else {
                return IsCSFile(_FileName);
            }
        }

        public static bool CurrentModuleIsDelphiFile() {
            string _FileName = GetCurrentEditorFileName();

            if (Utils.StringIsNullOrEmpty(_FileName)) {
                return false;
            } else {
                return IsDelphiFile(_FileName);
            }
        }

        public static bool CurrentModuleIsSourceFile() {
            string _FileName = GetCurrentEditorFileName();

            if (Utils.StringIsNullOrEmpty(_FileName)) {
                return false;
            } else {
                return IsSourceFile(_FileName);
            }
        }

		public static bool CurrentModuleIsXmlFile() {
			string _FileName = GetCurrentEditorFileName();

            if (Utils.StringIsNullOrEmpty(_FileName)) {
                return false;
            } else {
                return IsXmlFile(_FileName);
            }
        }

        public static IOTAModule GetCurrentModule() {
            IOTAModuleServices _ModuleServices = GetModuleServices();

            return _ModuleServices.CurrentModule;
        }

		public static string[] GetProjectSourceFiles(IOTAProject aProject) {
            if (aProject == null) {
                return null;
            } else {
                ArrayList _List = new ArrayList();

                for (int i = 0; i < aProject.ModuleCount; i++) {
                    IOTAModuleInfo _ModuleInfo = aProject.GetModuleInfo(i);

                    if (IsSourceFile(_ModuleInfo.FileName)) {
                        _List.Add(_ModuleInfo.FileName);
                    }
                }

                return (string[]) _List.ToArray(typeof(string));
            }
        }

        public static bool IsCSFile(string aFileName) {
            if (Utils.StringIsNullOrEmpty(aFileName)) {
                return false;
            }

            return CheckExtension(Path.GetExtension(aFileName), GetCsExtensionsFromOptions());
        }

        public static bool IsDelphiFile(string aFileName) {
            if (Utils.StringIsNullOrEmpty(aFileName)) {
                return false;
			}

            return CheckExtension(Path.GetExtension(aFileName), GetDelphiExtensionsFromOptions());
        }

        public static bool IsProjectFile(string aFileName) {
            if (Utils.StringIsNullOrEmpty(aFileName)) {
                return false;
            }

            return (Path.GetExtension(aFileName).ToUpper() == CommonConsts.PROJECT_EXTENSION.ToUpper());
        }

        public static bool IsProjectGroupFile(string aFileName) {
            if (Utils.StringIsNullOrEmpty(aFileName)) {
				return false;
			}

            return (Path.GetExtension(aFileName).ToUpper() == CommonConsts.PROJECT_GROUP_EXTENSION.ToUpper());
        }

        public static bool IsXmlFile(string aFileName) {
            if (Utils.StringIsNullOrEmpty(aFileName)) {
                return false;
            }

            return CheckExtension(Path.GetExtension(aFileName), GetXmlExtensionsFromOptions());
        }

        public static bool IsSourceFile(string aFileName) {
            if (Utils.StringIsNullOrEmpty(aFileName)) {
                return false;
            }

            return IsCSFile(aFileName) || IsDelphiFile(aFileName);
        }

        public static bool WarnForUnsavedFiles(IOTAProject aProject, string aWarningMessageTitle) {
            try {
                if (aProject == null) {
                    return false;
                }

                IOTAModuleServices _ModuleServices = GetModuleServices();

				for (int k = 0; k < aProject.ModuleCount; k++) {
					IOTAModuleInfo _ModuleInfo = aProject.GetModuleInfo(k);

                    IOTAModule _Module = _ModuleServices.FindModule(_ModuleInfo.FileName);

                    if (_Module != null) {

                        IOTAEditor _Editor = GetEditorWithSourceEditor(_Module);

                        if (_Editor != null && _Editor.IsModified) {
                            if (MessageBox.Show("The project contains unsaved files\n" + _Module.FileName + "\nDo you still want to compile the project?",
                                                aWarningMessageTitle,
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Information) == DialogResult.No) {

								return true;
							}
                        }
                    }
                }
            } catch (Exception e) {
                MessageBox.Show(e.Message + "--" + e.StackTrace);
            }

            return false;
        }

        /**************************************************************/
        /*                         DotNetModule
        /**************************************************************/

        public static IDesignerHost GetActiveDesignerForModule(IOTAModule aModule) {
            if (aModule != null) {
                IOTADotNetModule _DotNetModule = (IOTADotNetModule) aModule.GetService(typeof(IOTADotNetModule));

                if (_DotNetModule != null) {
                    _DotNetModule.ShowDesigner();

                    if (_DotNetModule.DesignerActive) {
                        IDesignerHost _Designer = _DotNetModule.DesignerHost;

                        return _Designer;
                    }
                }
            }

			return null;
        }

        public static IDesignerHost GetDesignerForModule(IOTAModule aModule) {
            if (aModule != null) {
                IOTADotNetModule _DotNetModule = (IOTADotNetModule) aModule.GetService(typeof(IOTADotNetModule));

                if (_DotNetModule != null) {
                    IDesignerHost _Designer = _DotNetModule.DesignerHost;

                    return _Designer;
                }
            }

			return null;
		}

        public static bool GetDesignerIsActive(IOTAModule aModule) {
            if (aModule != null) {
                IOTADotNetModule _DotNetModule = (IOTADotNetModule) aModule.GetService(typeof(IOTADotNetModule));

                if (_DotNetModule != null) {
                    return _DotNetModule.DesignerActive;
                }
            }

            return false;
        }

        /**************************************************************/
        /*                         Editor
        /**************************************************************/

        public static IOTAEditor GetCurrentEditor(IOTAModule aModule) {
            try {
                // BUG in the ToolsAPI: The IDE crashes calling IOTAModule.CurrentEditor on the welcome page
                if (aModule == null || aModule.FileName == "default.htm") {
                    return null;
                } else {
                    return aModule.CurrentEditor;
                }
            } catch {
                // Bug in the ToolsAPI: if nothing visible
                return null;
			}
		}

        #if V20
        public static IOTAFormEditor GetFormEditor(IOTAModule aModule) {
            return aModule as IOTAFormEditor;
        }
        #endif

        public static IOTAEditor GetEditorWithSourceEditor(IOTAModule aModule) {
            if (aModule == null) {
                return null;
            }

            return (IOTAEditor) GetSourceEditor(aModule);
		}

        public static IOTASourceEditor GetSourceEditorFromEditor(IOTAModule aModule, IOTAEditor aEditor) {
            if (aModule == null || aEditor == null) {
                return null;
            }

            IOTAEditor _Editor;

            for (int i = 0; i < aModule.ModuleFileCount; i++) {
                _Editor = aModule.ModuleFileEditors(i);

                if (_Editor.FileName == aEditor.FileName) {
                    if (_Editor is IOTASourceEditor) {
                        return (_Editor as IOTASourceEditor);
                    } else {
                        return null;
                    }
                }
            }

            return null;
        }

        /**************************************************************/
        /*                         Source Editor
        /**************************************************************/

        public static IOTASourceEditor GetCurrentSourceEditor() {
            IOTAModule _Module = GetCurrentModule();

			return GetSourceEditor(_Module);
        }

        public static IOTASourceEditor GetSourceEditor(IOTAModule aModule) {
            if (aModule == null) {
                return null;
            }

            if (GetCurrentEditor(aModule) != null &&
                    GetSourceEditorFromEditor(aModule, GetCurrentEditor(aModule)) != null) {
                return (IOTASourceEditor) GetSourceEditorFromEditor(aModule, GetCurrentEditor(aModule));
            } else { // Find a source Editor
                IOTAEditor _Editor;

				for (int i = 0; i < aModule.ModuleFileCount; i++) {
					_Editor = aModule.ModuleFileEditors(i);

                    if (_Editor is IOTASourceEditor) {
                        return (_Editor as IOTASourceEditor);
                    }
                }
            }

            return null;
        }

        /**************************************************************/
        /*                         EditView
        /**************************************************************/

        public static string BuildLines(string [] aLines, bool aAddLastNewLine) {
            string _Lines = "";

            for (int i = 0; i < aLines.Length; i++) {
                _Lines = _Lines + aLines[i];

                if (aLines.Length > 1 && (i != aLines.Length -1 || aAddLastNewLine)) {
                    _Lines = _Lines + "\r\n";
                }
            }

            return _Lines;
        }

		public static int CharPosToBufferPos(IOTASourceEditor aSourceEditor, OTACharPos aCharPos) {
			IOTAEditView _EditView = GetEditView(aSourceEditor);

            if (_EditView != null) {
                return _EditView.CharPosToPos(aCharPos);
            }

            return -1;
        }

        public static int EditPosToBufferPos(IOTASourceEditor aSourceEditor, OTAEditPos aEditPos) {
            OTACharPos _CharPos = EditPosToCharPos(aSourceEditor, aEditPos);

            return CharPosToBufferPos(aSourceEditor, _CharPos);
        }

		public static OTAEditPos BufferPosToEditPos(IOTASourceEditor aSourceEditor, int aBufferPos) {
            IOTAEditView _EditView = GetEditView(aSourceEditor);

            if (_EditView != null) {
                OTACharPos _CharPos = _EditView.PosToCharPos(aBufferPos);

                return CharPosToEditPos(aSourceEditor, _CharPos);
            }

            return new OTAEditPos();
        }

        public static OTACharPos BufferPosToCharPos(IOTASourceEditor aSourceEditor, int aBufferPos) {
            IOTAEditView _EditView = GetEditView(aSourceEditor);

            if (_EditView != null) {
                return _EditView.PosToCharPos(aBufferPos);
            }

            return new OTACharPos();
        }

        public static OTACharPos EditPosToCharPos(IOTASourceEditor aSourceEditor, OTAEditPos aEditPos) {
            IOTAEditView _EditView = GetEditView(aSourceEditor);
            OTACharPos _CharPos = new OTACharPos();

            if (_EditView != null) {
                _EditView.ConvertPos(true, ref aEditPos, ref _CharPos);
            }

			return _CharPos;
        }

        public static OTAEditPos CharPosToEditPos(IOTASourceEditor aSourceEditor, OTACharPos aCharPos) {
            IOTAEditView _EditView = GetEditView(aSourceEditor);
            OTAEditPos _EditPos = new OTAEditPos();

            if (_EditView != null) {
                _EditView.ConvertPos(false, ref _EditPos, ref aCharPos);
            }

            return _EditPos;
        }

		public static void CreateFileFromBuffer(IOTASourceEditor aSourceEditor, string aFileName) {
			if (aSourceEditor != null) {
                IOTAFileReader _Reader = aSourceEditor.CreateReader();

                Byte[] _Source = _Reader.Read(MAX_READ_SIZE, 0);

                FileStream _FileStream = new FileStream(aFileName, FileMode.Create);
                try {
                    _FileStream.Write(_Source, 0, _Source.Length);
                } finally {
                    _FileStream.Close();
                }
            }
        }

        // This function remove special char from string and get only one line from the string
        // It's ensure that the line is UI Ready
        public static string CreatePresentationLineFromDirtyString(string aString) {
            StringReader  _Reader = new StringReader(aString);

            string _Line = _Reader.ReadLine();

            // Remove Tabs
            if (_Line != null) {
                _Line = _Line.Replace("\t", new string(' ', OTAUtils.GetCsIndentationFromOptions()));
            }

            return _Line;
        }

		public static void FillBufferFromFile(IOTASourceEditor aSourceEditor, string aFileName) {
			if (aSourceEditor != null) {
                IOTAFileWriter _Writer = aSourceEditor.CreateWriter();

                FileStream _FileStream = File.OpenRead(aFileName);
                try {
                    byte[] _Source = new byte[0];
                    _FileStream.Read(_Source, 0, MAX_READ_SIZE);

                    _Writer.DeleteTo(0);
                    _Writer.Insert(_Source, 0, _Source.Length);
                } finally {
                    _FileStream.Close();
                }
            }
		}

        public static void FillBufferFromString(IOTASourceEditor aSourceEditor, string aSource) {
            if (aSourceEditor != null) {
                IOTAFileWriter _Writer = aSourceEditor.CreateWriter();

                _Writer.DeleteTo(0);
                _Writer.Insert(aSource);
            }
        }

        public static bool FindSelectionText(string aId, string aSource, int aLastPos, bool aPrev, out int aPos) {
            IOTAEditView _EditView = GetCurrentEditView();
            OTAEditPos _EditPos = new OTAEditPos();
            _EditPos.Line = _EditView.Buffer.EditBlock.StartingRow;
            _EditPos.Col = (short) _EditView.Buffer.EditBlock.StartingColumn;

            int _StartPos = EditPosToBufferPos(GetCurrentSourceEditor(), _EditPos);

            _EditPos.Line = _EditView.Buffer.EditBlock.EndingRow;
            _EditPos.Col = (short) _EditView.Buffer.EditBlock.EndingColumn;

            int _LastPos = EditPosToBufferPos(GetCurrentSourceEditor(),_EditPos);

            int _Diff = aLastPos - _StartPos;
            aPos = -1;

            if (aId == string.Empty) {
                return false;
            }

			aId = aId.ToUpper();

            while (GoNext(aSource, ref _StartPos, aPrev)) {
                if (Char.ToUpper(aSource[_StartPos]) == aId[0]) {
                    if (Utils.IsStrCaseIns(aSource, _StartPos, aId)) {
                            aPos = _StartPos;
                            return true;
                    }
                }
            }

            return false;
        }

		public static bool FindTextIdent(string aId, string aSource, int aLastPos, bool aPrev, out int aPos) {
			int _StartPos = GoToBeginingOfWord(aSource, aLastPos);
            int _Diff = aLastPos - _StartPos;
            aPos = -1;
            if (aId == string.Empty) {
                return false;
            }

            aId = aId.ToUpper();

            while (GoNext(aSource, ref _StartPos, aPrev)) {
                if (Char.ToUpper(aSource[_StartPos]) == aId[0]) {
                    if (Utils.IsStrCaseIns(aSource, _StartPos, aId)) {
                        if (((_StartPos < 1) || (! IsWordCharacter(aSource[_StartPos - 1]))) &&
                                ((_StartPos + aId.Length > aSource.Length) || (!IsWordCharacter((aSource[_StartPos + aId.Length]))))) {
                            aPos = _StartPos + _Diff;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static IOTAEditView GetEditView(IOTASourceEditor aSourceEditor) {
            if (aSourceEditor != null && aSourceEditor.EditViewCount > 0) {
                return aSourceEditor.GetEditView(0);
            } else {
                return null;
			}
		}

        public static IOTAEditView GetCurrentEditView() {
            return GetEditView(GetSourceEditor(GetCurrentModule()));
        }

        public static int GetCurrentLineNumber() {
            IOTAEditView _EditView = GetCurrentEditView();

            if (_EditView != null) {
                return _EditView.Buffer.EditPosition.Row;
            } else {
                return -1;
            }
		}

        public static int GoToBeginingOfWord(string aSource, int aStartPos) {
            int _Pos = aStartPos;

            if (_Pos >= aSource.Length) {
                return _Pos;
            }

            while ((_Pos > 1) && IsWordCharacter(aSource[_Pos-1])) {
                _Pos--;
            }
            return _Pos;
        }

        public static int GoToEndOfWord(string aSource, int aStartPos) {
            int _Pos = aStartPos;
            while ((_Pos > 1) && (_Pos < aSource.Length) && IsWordCharacter(aSource[_Pos])) {
                _Pos++;
            }
            return _Pos;
        }

        public static bool GoNext(string aSource, ref int aStartPos, bool aPrev) {
            if (aPrev) {
                aStartPos--;
            } else {
                aStartPos++;
            }
            return (aStartPos >= 0) && (aStartPos < aSource.Length);
		}

        public static string GetCurrentSelectedText() {
            return GetSelectedText(OTAUtils.GetCurrentModule());
        }

        public static string GetCurrentWord(IOTASourceEditor aSourceEditor) {
            string _Source = GetSourceEditorText(aSourceEditor);
            int _StartPos = GetSourceEditorTextPos(aSourceEditor);

            if (_Source == "" || _StartPos < 0 || _StartPos >= _Source.Length) {
                return "";
            }

            if (IsWordCharacter(_Source[_StartPos])) {
				int _Begining = GoToBeginingOfWord(_Source, _StartPos);
				int _End = GoToEndOfWord(_Source, _StartPos);

                return _Source.Substring(_Begining, _End - _Begining);
            } else {
                return "";
            }
        }

        public static bool CursorIsAtEndOfLine(IOTASourceEditor aSourceEditor) {
            IOTAEditView _EditView = GetEditView(aSourceEditor);
            OTAEditPos _OriginalPos = _EditView.CursorPos;
            try {
                _EditView.Buffer.EditPosition.MoveEOL();

                // Get the End of the Line
                OTAEditPos _EndEditPos = new OTAEditPos();
                _EndEditPos.Col = _EditView.CursorPos.Col;
                _EndEditPos.Line = _EditView.CursorPos.Line;

                return (_EndEditPos.Col == _OriginalPos.Col);
            } finally {
                _EditView.CursorPos = _OriginalPos;
            }
        }
        
        public static string GetCurrentLine(IOTASourceEditor aSourceEditor) {
            string _Source = GetSourceEditorText(aSourceEditor);

            if (_Source == "") {
				return "";
			}

            IOTAEditView _EditView = GetEditView(aSourceEditor);
            OTAEditPos _OriginalPos = _EditView.CursorPos;
            try {
                _EditView.Buffer.EditPosition.MoveEOL();

                // Get the Begin of the Line
                OTAEditPos _BeginEditPos = new OTAEditPos();
                _BeginEditPos.Col = 1;
                _BeginEditPos.Line = _EditView.CursorPos.Line;

                // Get the End of the Line
                OTAEditPos _EndEditPos = new OTAEditPos();
				_EndEditPos.Col = _EditView.CursorPos.Col;
				_EndEditPos.Line = _EditView.CursorPos.Line;

                int _Begining = EditPosToBufferPos(aSourceEditor, _BeginEditPos);
                int _End = EditPosToBufferPos(aSourceEditor, _EndEditPos);

                return _Source.Substring(_Begining, _End - _Begining);
            } finally {
                _EditView.CursorPos = _OriginalPos;
            }
        }

        public static OTAEditPos GetCurrentEditPos(IOTASourceEditor aSourceEditor) {
            if (aSourceEditor != null) {
                if (aSourceEditor.EditViewCount > 0) {
                    IOTAEditView _EditView = aSourceEditor.GetEditView(0);

                    return _EditView.CursorPos;
                }
            }

            return new OTAEditPos();
        }

        public static OTAEditPos GetCurrentEditPos() {
            return GetCurrentEditPos(OTAUtils.GetCurrentSourceEditor());
        }

        public static string[] GetLinesFromString(string aString) {
            if (aString != null) {
				StringReader  _Reader = new StringReader(aString);
				ArrayList _List = new ArrayList();

                string _Line = _Reader.ReadLine();

                while (_Line != null) {
                    _List.Add(_Line);
                    _Line = _Reader.ReadLine();
                }

                return (string[]) _List.ToArray(typeof(string));
            } else {
                return new string[0];
            }
        }

		public static string GetSelectedText(IOTAModule aModule) {
            if (aModule == null) {
                return null;
            }

            IOTASourceEditor _SourceEditor = GetSourceEditor(aModule);

            if (_SourceEditor != null) {
                IOTAEditView _EditView = _SourceEditor.GetEditView(0);

                return _EditView.Buffer.EditBlock.Text;
            } else {
                return null;
            }
        }

        public static string GetSourceEditorText(IOTASourceEditor aSourceEditor) {
            if (aSourceEditor == null)
                return "";
                
            IOTAFileReader _Reader = aSourceEditor.CreateReader();

            if (_Reader == null)
                throw new Exception("No file reader");

            System.Text.ASCIIEncoding _Encoding = new System.Text.ASCIIEncoding();
            StringBuilder _StringBuilder = new StringBuilder();
            Byte[] _Source = _Reader.Read(1024, 0);
            
			while (_Source.Length != 0) {
				_StringBuilder.Append(_Encoding.GetString(_Source));

                _Source = _Reader.Read(1024, 0);
            }

            return _StringBuilder.ToString();
        }

        public static int GetSourceEditorTextPos(IOTASourceEditor aSourceEditor) {
            IOTAEditView _EditView = GetEditView(aSourceEditor);

            if (_EditView != null) {
                OTAEditPos _EditPos = _EditView.CursorPos;
                OTACharPos _CharPos = new OTACharPos();

				_EditView.ConvertPos(true, ref _EditPos, ref _CharPos);

                return _EditView.CharPosToPos(_CharPos);
            } else {
                return -1;
            }
        }

        public static string GetStringBeforeFirstWord() {
            string _Text = GetCurrentSelectedText();

            if (Utils.StringIsNullOrEmpty(_Text)) {
                return "";
            }

            int _Index = 0;

            for (int i = 0; i < _Text.Length; i++) {
                if (_Text[i] != '\t' && _Text[i] != ' ') {
                    _Index = i;
                    break;
                }
            }

            return _Text.Substring(0, _Index);
        }

        public static void GoToPosition(IOTASourceEditor aSourceEditor, int aLineNumber, short aColNumber) {
            if (aSourceEditor != null) {
				IOTAEditView _EditView = GetEditView(aSourceEditor);

                OTAEditPos _Pos = new OTAEditPos();
                _Pos.Col = aColNumber;
                _Pos.Line = aLineNumber;

                _EditView.CursorPos = _Pos;
            }
        }

        public static void InsertText(IOTASourceEditor aSourceEditor, string aLines) {
            IOTAEditView _EditView = OTAUtils.GetEditView(aSourceEditor);

            if (_EditView != null) {
                _EditView.Position.InsertText(aLines);
			}
		}

        public static bool IsWordCharacter(char aChar) {
            return Char.IsLetterOrDigit(aChar) || (aChar == '_');
        }

        private static void ReplaceInclusiveExclusive(IOTASourceEditor aSourceEditor,
                IOTAEditView aEditView,
                bool aIsInclusive,
                OTACharPos _Start, OTACharPos
                _After, string aText) {

            if (aSourceEditor == null || aEditView == null) {
                return;
            }

            bool _FirstCharInLineDeleted;

            if (aIsInclusive == false )  {
                _FirstCharInLineDeleted = (_After.CharIndex == 1);

                if (_After.CharIndex > 0) {
                    _After.CharIndex -= 1;
                }
            } else
                _FirstCharInLineDeleted = false;

            int _StartPos = aEditView.CharPosToPos(_Start);
            int _AfterPos = aEditView.CharPosToPos(_After);

			IOTAFileWriter _Writer = aSourceEditor.CreateWriter();

            _Writer.CopyTo(_StartPos);
            int _DeleteToPos = _AfterPos;

            if (_After.CharIndex == 0 && ((_After.Line - _Start.Line) == 1)) {
                _DeleteToPos -= 2;

                if (_FirstCharInLineDeleted) {
                    _DeleteToPos =+ 3;
                }
            } else  {
                if (_FirstCharInLineDeleted) {
                    _DeleteToPos++;
				} else if (_After.CharIndex > 0) {
					_DeleteToPos++;
                }
            }

            if (_DeleteToPos > _StartPos) {
                _Writer.DeleteTo(_DeleteToPos);
            }

            _Writer.Insert(aText);
            _Writer.CopyTo(Int32.MaxValue);
            _Writer.Close();
        }

        public static void ReplaceSelectedText(IOTASourceEditor aSourceEditor, string aLines) {
            IOTAEditView _EditView = OTAUtils.GetEditView(aSourceEditor);

            if (_EditView != null) {
                if (_EditView.Block != null && !Utils.StringIsNullOrEmpty(_EditView.Block.Text)) {
                    OTACharPos _Start = aSourceEditor.BlockStart;
                    OTACharPos _After = aSourceEditor.BlockAfter;

                    if (aSourceEditor.BlockType == OTABlockType.btInclusive) {
                        ReplaceInclusiveExclusive(aSourceEditor, _EditView, true, _Start, _After, aLines);
                    } else if (aSourceEditor.BlockType == OTABlockType.btNonInclusive) {
                        ReplaceInclusiveExclusive(aSourceEditor, _EditView, false, _Start, _After, aLines);
                    } else if (aSourceEditor.BlockType == OTABlockType.btColumn) {
                        // Column mode not supported
                        //ReplaceColumns(TOTAEditPos(_Start), TOTAEditPos(_After), Text);
                    }
					else if (aSourceEditor.BlockType == OTABlockType.btLine) {
						_Start.CharIndex = 0;
                        _After.CharIndex = 1023; // Max line length
                        ReplaceInclusiveExclusive(aSourceEditor, _EditView, true, _Start, _After, aLines);
                    }
                } else {
                    InsertText(aSourceEditor, aLines);
                }
            }
        }

        public static bool SearchText(string aText,
                                      string aSource,
                                      int aCurrentPos,
                                      bool aPrev,
									  out int aFoundPos) {
			int _StartPos = aCurrentPos;
            aFoundPos = -1;

            if (aText == string.Empty) {
                return false;
            }

            aText = aText.ToUpper();

            while (GoNext(aSource, ref _StartPos, aPrev)) {
                //if (Char.ToUpper(aSource[_StartPos]) == aText[0]) {
                if (Utils.IsStrCaseIns(aSource, _StartPos, aText)) {
                    aFoundPos = _StartPos;
                    return true;
                }
                //}
            }

            return false;
        }

        public static void SelectLine(IOTASourceEditor aSourceEditor) {
            if (aSourceEditor != null) {
                if (aSourceEditor.EditViewCount > 0) {
                    IOTAEditView _EditView = aSourceEditor.GetEditView(0);

                    OTAEditPos _EditPos = new OTAEditPos();

                    _EditPos.Line = _EditView.Position.Row;
					_EditPos.Col = 1;
					_EditView.CursorPos = _EditPos;
                    _EditView.Block.BeginBlock();
                    _EditView.Block.Style = OTABlockType.btNonInclusive;

                    if (_EditView.Position.MoveEOL()) {
                        _EditView.MoveViewToCursor();
                    }

                    _EditView.Block.EndBlock();
                }
            }
        }

        public static void SelectTextFromPosToPos(IOTASourceEditor aSourceEditor, OTAEditPos aStartPos, OTAEditPos aEndPos) {
			if (aSourceEditor != null) {
				if (aSourceEditor.EditViewCount > 0) {
                    IOTAEditView _EditView = aSourceEditor.GetEditView(0);

                    _EditView.CursorPos = aStartPos;

                    _EditView.Block.BeginBlock();
                    _EditView.Block.Style = OTABlockType.btNonInclusive;

                    _EditView.CursorPos = aEndPos;
                    _EditView.MoveViewToCursor();
                    _EditView.Block.EndBlock();
                }
            }
        }

        /**************************************************************/
        /*                         Code Dom
        /**************************************************************/

        private static void LoadClassInfoInList(CodeTypeDeclaration aCodeTypeDeclaration, ArrayList aArrayList) {
            if (aCodeTypeDeclaration.IsClass) {
                ClassInfo _ClassInfo = new ClassInfo();
                _ClassInfo.Tag = aCodeTypeDeclaration;
                aArrayList.Add(_ClassInfo);

                for (int i = 0; i < aCodeTypeDeclaration.Members.Count; i++) {
                    if (aCodeTypeDeclaration.Members[i] is CodeTypeDeclaration) {
                        CodeTypeDeclaration _CodeTypeDeclaration = (CodeTypeDeclaration) aCodeTypeDeclaration.Members[i];

						if (_CodeTypeDeclaration.IsClass) {
							LoadClassInfoInList(_CodeTypeDeclaration, aArrayList);
                        }
                    }
                }
            }
        }

        public static void LoadClassInfoInList(IOTAModule aModule, ArrayList aArrayList) {
            if (aModule == null) {
                return;
            }

            IOTACodeDomProvider _CodeDomProvider = (IOTACodeDomProvider) aModule.GetService(typeof(IOTACodeDomProvider));

			if (_CodeDomProvider != null) {
				IOTACodeDomFile _CodeDomFile = _CodeDomProvider.CodeDomFile;
                CodeObject _CodeObject = _CodeDomFile.GetDom();

                if (_CodeObject != null) {
                    CodeCompileUnit _CodeCompileUnit = (CodeCompileUnit) _CodeObject;

                    for (int i = 0; i < _CodeCompileUnit.Namespaces.Count; i++) {
                        CodeNamespace _CodeNamespace = _CodeCompileUnit.Namespaces[i];

                        for (int j = 0; j < _CodeNamespace.Types.Count; j++) {
                            CodeTypeDeclaration _CodeTypeDeclaration = _CodeNamespace.Types[j];
                            LoadClassInfoInList(_CodeTypeDeclaration, aArrayList);
                        }
                    }

                }
            }
        }

        private static void LoadEventInfoInList(CodeTypeDeclaration aCodeTypeDeclaration, ArrayList aArrayList) {
            if (aCodeTypeDeclaration.IsClass || aCodeTypeDeclaration.IsInterface) {
                for (int i = 0; i < aCodeTypeDeclaration.Members.Count; i++) {
                    CodeTypeMember _CodeTypeMember = aCodeTypeDeclaration.Members[i];

                    if (_CodeTypeMember is CodeMemberEvent) {
                        LoadEventInfoInList(_CodeTypeMember, aCodeTypeDeclaration.Name, aArrayList);
                    } else if (aCodeTypeDeclaration.Members[i] is CodeTypeDeclaration) {
                        CodeTypeDeclaration _CodeTypeDeclaration = (CodeTypeDeclaration) aCodeTypeDeclaration.Members[i];

						if (_CodeTypeDeclaration.IsClass ||_CodeTypeDeclaration.IsInterface) {
							LoadEventInfoInList(_CodeTypeDeclaration, aArrayList);
                        }
                    }
                }
            }
        }

        private static void LoadEventInfoInList(CodeTypeMember aCodeTypeMember, string aClassName, ArrayList aArrayList) {
            if (aCodeTypeMember is CodeMemberEvent) {
                CodeMemberEvent _CodeMemberEvent = (CodeMemberEvent) aCodeTypeMember;

                if (_CodeMemberEvent.LinePragma != null) {
                    EventInfo _EventInfo;
                    _EventInfo = new EventInfo();
					_EventInfo.Tag = _CodeMemberEvent;
					_EventInfo.ClassName = aClassName;
                    aArrayList.Add(_EventInfo);
                }
            }
        }

        public static void LoadEventInfoInList(IOTAModule aModule, ArrayList aArrayList) {
            if (aModule == null) {
                return;
            }

            IOTACodeDomProvider _CodeDomProvider = (IOTACodeDomProvider) aModule.GetService(typeof(IOTACodeDomProvider));

            if (_CodeDomProvider != null) {
                IOTACodeDomFile _CodeDomFile = _CodeDomProvider.CodeDomFile;
                CodeObject _CodeObject = _CodeDomFile.GetDom();

                if (_CodeObject != null) {
                    CodeCompileUnit _CodeCompileUnit = (CodeCompileUnit) _CodeObject;

                    for (int i = 0; i < _CodeCompileUnit.Namespaces.Count; i++) {
                        CodeNamespace _CodeNamespace = _CodeCompileUnit.Namespaces[i];

                        for (int j = 0; j < _CodeNamespace.Types.Count; j++) {
                            CodeTypeDeclaration _CodeTypeDeclaration = _CodeNamespace.Types[j];

                            if (_CodeTypeDeclaration.IsClass ||_CodeTypeDeclaration.IsInterface) {
                                string _ClassName = _CodeTypeDeclaration.Name;

								for (int k = 0; k < _CodeTypeDeclaration.Members.Count; k++) {
									CodeTypeMember _CodeTypeMember  = _CodeTypeDeclaration.Members[k];

                                    if (_CodeTypeMember is CodeMemberEvent) {
                                        LoadEventInfoInList(_CodeTypeMember, _ClassName, aArrayList);
                                    } else if (_CodeTypeMember is CodeTypeDeclaration) {
                                        LoadEventInfoInList(_CodeTypeMember as CodeTypeDeclaration, aArrayList);
                                    }
                                }
                            }
                        }
                    }

                }
            }
		}

        private static void LoadFieldInfoInList(CodeTypeDeclaration aCodeTypeDeclaration, ArrayList aArrayList) {
            if (aCodeTypeDeclaration.IsClass || aCodeTypeDeclaration.IsInterface) {
                for (int i = 0; i < aCodeTypeDeclaration.Members.Count; i++) {
                    CodeTypeMember _CodeTypeMember = aCodeTypeDeclaration.Members[i];

                    if (_CodeTypeMember is CodeMemberProperty ||
                            _CodeTypeMember is CodeMemberField) {
                        LoadFieldInfoInList(_CodeTypeMember, aCodeTypeDeclaration.Name, aArrayList);
                    } else if (aCodeTypeDeclaration.Members[i] is CodeTypeDeclaration) {
                        CodeTypeDeclaration _CodeTypeDeclaration = (CodeTypeDeclaration) aCodeTypeDeclaration.Members[i];

                        if (_CodeTypeDeclaration.IsClass ||_CodeTypeDeclaration.IsInterface) {
                            LoadFieldInfoInList(_CodeTypeDeclaration, aArrayList);
                        }
                    }
                }
            }
        }

        private static void LoadFieldInfoInList(CodeTypeMember aCodeTypeMember, string aClassName, ArrayList aArrayList) {
            if (aCodeTypeMember is CodeMemberProperty ||
                    aCodeTypeMember is CodeMemberField) {

                if (aCodeTypeMember.LinePragma != null) {
                    FieldInfo _FieldInfo = new FieldInfo();
                    _FieldInfo.Tag = aCodeTypeMember;
                    _FieldInfo.ClassName = aClassName;
                    aArrayList.Add(_FieldInfo);
				}
			}
        }

        public static void LoadFieldInfoInList(IOTAModule aModule, ArrayList aArrayList) {
            if (aModule == null) {
                return;
            }

            IOTACodeDomProvider _CodeDomProvider = (IOTACodeDomProvider) aModule.GetService(typeof(IOTACodeDomProvider));

            if (_CodeDomProvider != null) {
                IOTACodeDomFile _CodeDomFile = _CodeDomProvider.CodeDomFile;
                CodeObject _CodeObject = _CodeDomFile.GetDom();

				if (_CodeObject != null) {
					CodeCompileUnit _CodeCompileUnit = (CodeCompileUnit) _CodeObject;

                    for (int i = 0; i < _CodeCompileUnit.Namespaces.Count; i++) {
                        CodeNamespace _CodeNamespace = _CodeCompileUnit.Namespaces[i];

                        for (int j = 0; j < _CodeNamespace.Types.Count; j++) {
                            CodeTypeDeclaration _CodeTypeDeclaration = _CodeNamespace.Types[j];

                            if (_CodeTypeDeclaration.IsClass ||_CodeTypeDeclaration.IsInterface) {
                                string _ClassName = _CodeTypeDeclaration.Name;

                                for (int k = 0; k < _CodeTypeDeclaration.Members.Count; k++) {
                                    CodeTypeMember _CodeTypeMember  = _CodeTypeDeclaration.Members[k];

                                    if (_CodeTypeMember is CodeMemberProperty ||
                                            _CodeTypeMember is CodeMemberField) {
                                        LoadFieldInfoInList(_CodeTypeMember, _ClassName, aArrayList);
                                    } else if (_CodeTypeMember is CodeTypeDeclaration) {
                                        LoadFieldInfoInList(_CodeTypeMember as CodeTypeDeclaration, aArrayList);
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }

		private static void LoadMethodInfoInList(CodeTypeDeclaration aCodeTypeDeclaration, ArrayList aArrayList) {
			if (aCodeTypeDeclaration.IsClass || aCodeTypeDeclaration.IsInterface) {
                for (int i = 0; i < aCodeTypeDeclaration.Members.Count; i++) {
                    CodeTypeMember _CodeTypeMember = aCodeTypeDeclaration.Members[i];

                    if (_CodeTypeMember is CodeMemberMethod) {
                        LoadMethodInfoInList(_CodeTypeMember, aCodeTypeDeclaration.Name, aArrayList);
                    } else if (aCodeTypeDeclaration.Members[i] is CodeTypeDeclaration) {
                        CodeTypeDeclaration _CodeTypeDeclaration = (CodeTypeDeclaration) aCodeTypeDeclaration.Members[i];

                        if (_CodeTypeDeclaration.IsClass ||_CodeTypeDeclaration.IsInterface) {
                            LoadMethodInfoInList(_CodeTypeDeclaration, aArrayList);
                        }
                    }
                }
			}
		}

        private static MethodInfo LoadMethodInfoInList(CodeTypeMember aCodeTypeMember, string aClassName, ArrayList aArrayList) {
            if (aCodeTypeMember is CodeMemberMethod || aCodeTypeMember is CodeMemberProperty) {
                if (aCodeTypeMember.LinePragma != null) {
                    MethodInfo _MethodInfo;
                    _MethodInfo = new MethodInfo();
                    _MethodInfo.Tag = aCodeTypeMember;
                    _MethodInfo.ClassName = aClassName;
                    aArrayList.Add(_MethodInfo);

                    return _MethodInfo;
                }
            }

            return null;
        }

        private static void LoadMethodInfoInList(CodeTypeMember aCodeTypeMember,
                                                 CodeStatement aCodeStatement,
                                                 bool aIsGet,
                                                 string aClassName,
                                                 ArrayList aArrayList) {
            MethodInfo _MethodInfo = LoadMethodInfoInList(aCodeTypeMember,
                                                          aClassName,
                                                          aArrayList);

            if (_MethodInfo != null) {
                _MethodInfo.CodeStatement = aCodeStatement;

				if (aIsGet) {
                    _MethodInfo.IsGet = true;
                } else {
                    _MethodInfo.IsSet = true;
                }
            }
        }

        public static void LoadMethodInfoInList(IOTAModule aModule, ArrayList aArrayList) {
            if (aModule == null) {
                return;
            }

            IOTACodeDomProvider _CodeDomProvider = (IOTACodeDomProvider) aModule.GetService(typeof(IOTACodeDomProvider));

			if (_CodeDomProvider != null) {
                IOTACodeDomFile _CodeDomFile = _CodeDomProvider.CodeDomFile;
                CodeObject _CodeObject = _CodeDomFile.GetDom();

                if (_CodeObject != null) {
                    CodeCompileUnit _CodeCompileUnit = (CodeCompileUnit) _CodeObject;

                    for (int i = 0; i < _CodeCompileUnit.Namespaces.Count; i++) {
                        CodeNamespace _CodeNamespace = _CodeCompileUnit.Namespaces[i];

                        for (int j = 0; j < _CodeNamespace.Types.Count; j++) {
                            CodeTypeDeclaration _CodeTypeDeclaration = _CodeNamespace.Types[j];

                            if (_CodeTypeDeclaration.IsClass || _CodeTypeDeclaration.IsInterface) {
                                string _ClassName = _CodeTypeDeclaration.Name;

                                for (int k = 0; k < _CodeTypeDeclaration.Members.Count; k++) {
                                    CodeTypeMember _CodeTypeMember  = _CodeTypeDeclaration.Members[k];

                                    if (_CodeTypeMember is CodeMemberMethod) {
                                        LoadMethodInfoInList(_CodeTypeMember, _ClassName, aArrayList);
                                    } else if (_CodeTypeMember is CodeMemberProperty) {
                                        CodeMemberProperty _CodeMemberProperty = _CodeTypeMember as CodeMemberProperty;

                                        if (_CodeMemberProperty.HasGet) {
                                            if (_CodeMemberProperty.GetStatements.Count > 0) {
                                                LoadMethodInfoInList(_CodeMemberProperty,
                                                                     _CodeMemberProperty.GetStatements[0],
                                                                     true,
																	 _ClassName,
																	 aArrayList);
                                            } else {
                                                LoadMethodInfoInList(_CodeMemberProperty,
                                                                     null,
                                                                     true,
                                                                     _ClassName,
                                                                     aArrayList);

                                            }
                                        }

                                        if (_CodeMemberProperty.HasSet) {
                                            if (_CodeMemberProperty.SetStatements.Count > 0) {
                                                LoadMethodInfoInList(_CodeMemberProperty,
																	 _CodeMemberProperty.SetStatements[0],
																	 false,
                                                                     _ClassName,
                                                                     aArrayList);
                                            } else {
                                                LoadMethodInfoInList(_CodeMemberProperty,
                                                                     null,
                                                                     false,
                                                                     _ClassName,
                                                                     aArrayList);
                                            }

                                        }
                                    } else if (_CodeTypeMember is CodeTypeDeclaration) {
                                        LoadMethodInfoInList(_CodeTypeMember as CodeTypeDeclaration, aArrayList);
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }

        private static void LoadTypeInfoInList(CodeTypeDeclaration aCodeTypeDeclaration, ArrayList aArrayList) {
            TypeInfo _TypeInfo = new TypeInfo();
            _TypeInfo.Tag = aCodeTypeDeclaration;
            aArrayList.Add(_TypeInfo);

			if (aCodeTypeDeclaration.IsClass) {
				for (int i = 0; i < aCodeTypeDeclaration.Members.Count; i++) {
                    if (aCodeTypeDeclaration.Members[i] is CodeTypeDeclaration) {
                        CodeTypeDeclaration _CodeTypeDeclaration = (CodeTypeDeclaration) aCodeTypeDeclaration.Members[i];

                        LoadTypeInfoInList(_CodeTypeDeclaration, aArrayList);
                    }
                }
            }
        }

        public static void LoadTypeInfoInList(IOTAModule aModule, ArrayList aArrayList) {
            if (aModule == null) {
                return;
            }

			IOTACodeDomProvider _CodeDomProvider = (IOTACodeDomProvider) aModule.GetService(typeof(IOTACodeDomProvider));

            if (_CodeDomProvider != null) {
                IOTACodeDomFile _CodeDomFile = _CodeDomProvider.CodeDomFile;
                CodeObject _CodeObject = _CodeDomFile.GetDom();

                if (_CodeObject != null) {
                    CodeCompileUnit _CodeCompileUnit = (CodeCompileUnit) _CodeObject;

                    for (int i = 0; i < _CodeCompileUnit.Namespaces.Count; i++) {
                        CodeNamespace _CodeNamespace = _CodeCompileUnit.Namespaces[i];

                        for (int j = 0; j < _CodeNamespace.Types.Count; j++) {
                            CodeTypeDeclaration _CodeTypeDeclaration = _CodeNamespace.Types[j];
                            LoadTypeInfoInList(_CodeTypeDeclaration, aArrayList);
                        }
                    }

                }
            }
        }

        /**************************************************************/
        /*                       Options
        /**************************************************************/

        public static string[] GetCsExtensionsFromOptions() {
            RegistryKey _Key = Registry.CurrentUser.OpenSubKey(OTAUtils.IDERegKey + CommonConsts.REG_KEY_BORLAND_CS_SOURCE_OPTIONS);

			if ( _Key != null && Array.IndexOf(_Key.GetValueNames(), @"File Extensions") > -1) {

                return _Key.GetValue(@"File Extensions").ToString().Split(';');
            } else {
                return null;
            }
        }

        public static int GetCsIndentationFromOptions() {
            RegistryKey _Key = Registry.CurrentUser.OpenSubKey(OTAUtils.IDERegKey + CommonConsts.REG_KEY_BORLAND_CS_SOURCE_OPTIONS);

            if ( _Key != null && Array.IndexOf(_Key.GetValueNames(), @"Block Indent") > -1) {

                return (int) _Key.GetValue(@"Block Indent");
            } else {
				return CommonConsts.DEFAULT_INDENTATION;
			}
        }

        public static string[] GetDelphiExtensionsFromOptions() {
            RegistryKey _Key = Registry.CurrentUser.OpenSubKey(OTAUtils.IDERegKey + CommonConsts.REG_KEY_BORLAND_DELPHI_SOURCE_OPTIONS);

            if ( _Key != null && Array.IndexOf(_Key.GetValueNames(), @"File Extensions") > -1) {

                return _Key.GetValue(@"File Extensions").ToString().Split(';');
            } else {
                return null;
            }
        }

        public static string [] GetXmlExtensionsFromOptions() {
            RegistryKey _Key = Registry.CurrentUser.OpenSubKey(OTAUtils.IDERegKey + CommonConsts.REG_KEY_BORLAND_XML_SOURCE_OPTIONS);

            if ( _Key != null && Array.IndexOf(_Key.GetValueNames(), @"File Extensions") > -1) {

                return _Key.GetValue(@"File Extensions").ToString().Split(';');
            } else {
                return null;
            }
        }

        /**************************************************************/
        /*                         Menu
        /**************************************************************/

		public static bool ExecuteMenu(string aMenuName) {
			IOTAMainMenuService _MenuService = GetMainMenuService();

			IOTAMenuItem _MenuItem = _MenuService.GetMenuItem(aMenuName);

			if (_MenuItem != null) {
				_MenuItem.Execute();
				return true;
			} else {
				return false;
			}
		}

		/**************************************************************/
		/*                         IDE
		/**************************************************************/

		public static BDSVersion CurrentBDSVersion {
			get {
				  int i = Array.IndexOf( CommonConsts.BDS_REG_KEYS, IDERegKey );
				  return (BDSVersion) Enum.GetValues( typeof ( BDSVersion ) ).GetValue( i );

//                #if V30
//					return BDSVersion.v30;
//                #endif
//                #if V20
//                    return BDSVersion.v20;
//                #else
//                    return BDSVersion.v10;
//                #endif
			}
		}

		public static string GetDotNetFrameworkInstallationPath() {
			RegistryKey _Key = Registry.CurrentUser.OpenSubKey(OTAUtils.IDERegKey + CommonConsts.BDS_DOT_NET_FRAMEWORK_REG_KEY);

			if (_Key != null && Array.IndexOf(_Key.GetValueNames(), @"CSharpCompiler") > -1) {
				return Path.GetDirectoryName(_Key.GetValue(@"CSharpCompiler").ToString());
			}

			return "";
		}

		public static string IDERegKey {
			get {
					if( ideRegKey == "" )
					{
						ideRegKey = GetService().BaseRegistryKey;
						if( ideRegKey.Substring( ideRegKey.Length - 1, 1 ) != "\\" )
						{
							ideRegKey += "\\";
						}
					}
					return ideRegKey;
//				if (CurrentBDSVersion == BDSVersion.v30) {
//					return CommonConsts.BDS_V3_REG_KEY;
//				} else if (CurrentBDSVersion == BDSVersion.v10) {
//					return CommonConsts.BDS_V1_REG_KEY;
//				} else {
//					return CommonConsts.BDS_V2_REG_KEY;
//				}
			}
		}

		public static string GetDisabledAssembliesRegKey(BDSVersion aBDSVersion) {
			return CommonConsts.BDS_REG_KEYS[(int) aBDSVersion] + CommonConsts.BDS_DISABLED_ASSEMBLIES_REG_KEY;
//			if (aBDSVersion == BDSVersion.v30) {
//				return CommonConsts.BDS_V3_DISABLED_ASSEMBLIES_REG_KEY;
//			} else if (aBDSVersion == BDSVersion.v10) {
//				return CommonConsts.BDS_V1_DISABLED_ASSEMBLIES_REG_KEY;
//			} else {
//				return CommonConsts.BDS_V2_DISABLED_ASSEMBLIES_REG_KEY;
//			}
		}

		public static string GetEnabledAssembliesRegKey(BDSVersion aBDSVersion) {
			return CommonConsts.BDS_REG_KEYS[(int) aBDSVersion] + CommonConsts.BDS_ENABLED_ASSEMBLIES_REG_KEY;
//			if (aBDSVersion == BDSVersion.v30) {
//				return CommonConsts.BDS_V3_ENABLED_ASSEMBLIES_REG_KEY;
//			} else if (aBDSVersion == BDSVersion.v10) {
//				return CommonConsts.BDS_V1_ENABLED_ASSEMBLIES_REG_KEY;
//			} else {
//				return CommonConsts.BDS_V2_ENABLED_ASSEMBLIES_REG_KEY;
//			}
		}

		/**************************************************************/
		/*                         Misc
		/**************************************************************/

		public static void ShowDebug(string aMessage) {
			IOTAMessageService _MessageService = GetMessageService();
			_MessageService.ShowMessageView(null);
			_MessageService.AddTitleMessage("SBT DEBUG: " + aMessage);
		}

	}
}
