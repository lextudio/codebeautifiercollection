using System;
using System.CodeDom;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using Borland.Studio.ToolsAPI;

namespace Lextm.JcfExpert.Utils {

	public class OTAUtils {

		/**************************************************************/
		/*                         Project Group
		/**************************************************************/

		public static IOTAProjectGroup GetCurrentProjectGroup() {
			IOTAModuleServices _ModuleServices = (IOTAModuleServices) BorlandIDE.GetService(typeof(IOTAModuleServices));
			IOTAProjectGroup _ProjectGroup = _ModuleServices.MainProjectGroup;

			return _ProjectGroup;
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

		/**************************************************************/
		/*                         Modules
		/**************************************************************/

		public static bool CurrentModuleIsPasFile() {
			IOTAModule _Module = GetCurrentModule();

			return (Path.GetExtension(_Module.FileName).ToUpper() == Consts.PAS_EXTENSION.ToUpper());
		}

		public static IOTAModule GetCurrentModule() {
			IOTAModuleServices _ModuleServices = (IOTAModuleServices) BorlandIDE.GetService(typeof(IOTAModuleServices));

			return _ModuleServices.CurrentModule;
		}

		/**************************************************************/
		/*                         Editor
		/**************************************************************/

		public static IOTAEditor GetEditorWithSourceEditor(IOTAModule aModule)
		{
			IOTAEditor _Editor;

			for (int i = 0; i < aModule.ModuleFileCount; i++) {
				_Editor = aModule.ModuleFileEditors(i);

				if (_Editor is IOTASourceEditor) {
					return _Editor;
				}
			}

			return null;
		}
                                 
		/**************************************************************/
		/*                         Source Editor
		/**************************************************************/

		public static IOTASourceEditor GetSourceEditor(IOTAModule aModule)
		{
			IOTAEditor _Editor;

			for (int i = 0; i < aModule.ModuleFileCount; i++) {
				_Editor = aModule.ModuleFileEditors(i);

				if (_Editor is IOTASourceEditor) {
					return (_Editor as IOTASourceEditor);
				}
			}

			return null;
		}

		public static IOTASourceEditor GetCurrentSourceEditor()
		{
			return GetSourceEditor(OTAUtils.GetCurrentModule());
		}


		/**************************************************************/
		/*                         EditView
		/**************************************************************/

		public static IOTAEditView GetCurrentEditView()
		{
			IOTASourceEditor _SourceEditor = OTAUtils.GetSourceEditor(OTAUtils.GetCurrentModule());

			return _SourceEditor.GetEditView(0);
		}

		public static int GetCurrentLineNumber()
		{
			IOTAEditView _EditView = GetCurrentEditView();

			return _EditView.Buffer.EditPosition.Row;
		}
                       
		public static string GetSelectedText(IOTAModule aModule) {
			IOTASourceEditor _SourceEditor = OTAUtils.GetSourceEditor(aModule);

			if (_SourceEditor != null) {
				IOTAEditView _EditView = _SourceEditor.GetEditView(0);

				return _EditView.Buffer.EditBlock.Text;
			}
			else {
				return null;
			}
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

		public static string BuildLines(string [] aLines, bool aAddLastNewLine) {
			string _Lines = "";

			for (int i = 0; i < aLines.Length; i++) {
				_Lines = _Lines + aLines[i];

				if (i != aLines.Length -1 || aAddLastNewLine) {
					_Lines = _Lines + "\n";
				}
			}
			
			return _Lines;
		}

		public static string[] GetLinesFromString(string aString) {
			StringReader  _Reader = new StringReader(aString);
			ArrayList _List = new ArrayList();
			 
			string _Line = _Reader.ReadLine();

			while (_Line != null) {
				_List.Add(_Line);
				_Line = _Reader.ReadLine();
			}

			return (string[]) _List.ToArray(typeof(string));
		}

		private static void ReplaceInclusiveExclusive(IOTASourceEditor aSourceEditor, IOTAEditView aEditView, bool aIsInclusive, OTACharPos _Start, OTACharPos _After, string aText) {
			bool _FirstCharInLineDeleted;

			if (aIsInclusive == false )  {
				_FirstCharInLineDeleted = (_After.CharIndex == 1);

				if (_After.CharIndex > 0) {
					_After.CharIndex -= 1;
				}
			}
			else
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
			}
			else  {
				if (_FirstCharInLineDeleted) {
					_DeleteToPos++;
				} else if (_After.CharIndex > 0) {
					_DeleteToPos++;
				}
			}
                     
			if (_DeleteToPos > _StartPos){
				_Writer.DeleteTo(_DeleteToPos);
			}

			_Writer.Insert(aText);
			_Writer.CopyTo(Int32.MaxValue);
			_Writer.Close();
			}

		public static void ReplaceSelectedText(IOTASourceEditor aSourceEditor, string aLines) {
			if (aSourceEditor != null && aSourceEditor.EditViewCount > 0) {

				IOTAEditView _EditView = aSourceEditor.GetEditView(0);

				if (_EditView != null && _EditView.Block != null) {
					string  _Selection = _EditView.Buffer.EditBlock.Text;

					OTACharPos _Start = aSourceEditor.BlockStart;
					OTACharPos _After = aSourceEditor.BlockAfter;

					if (aSourceEditor.BlockType == OTABlockType.btInclusive) {
						OTAUtils.ReplaceInclusiveExclusive(aSourceEditor, _EditView, true, _Start, _After, aLines);
					}
					else if (aSourceEditor.BlockType == OTABlockType.btNonInclusive) {
						OTAUtils.ReplaceInclusiveExclusive(aSourceEditor, _EditView, false, _Start, _After, aLines);
					}
					else if (aSourceEditor.BlockType == OTABlockType.btColumn) {
						//ReplaceColumns(TOTAEditPos(_Start), TOTAEditPos(_After), Text);
					}

					else if (aSourceEditor.BlockType == OTABlockType.btLine) {
						_Start.CharIndex = 0;
						_After.CharIndex = 1023; // Max line length
						OTAUtils.ReplaceInclusiveExclusive(aSourceEditor, _EditView, true, _Start, _After, aLines);
					}
				}
			}
		}

		public static string GetStringBeforeFirstWord() {
			string _Text = OTAUtils.GetSelectedText(GetCurrentModule());
			int _Index = 0;

			for (int i = 0; i < _Text.Length; i++) {
				if (_Text[i] != '\t' && _Text[i] != ' ') {
					_Index = i;
					break;
				}
			}

			return _Text.Substring(0, _Index);
		}

		/**************************************************************/
		/*                         Code Dom
		/**************************************************************/

		public static void LoadMethodInfoInList(ArrayList aArrayList) {
			IOTAModuleServices _ModuleServices = (IOTAModuleServices) BorlandIDE.GetService(typeof(IOTAModuleServices));
			IOTACodeDomProvider _CodeDomProvider = (IOTACodeDomProvider) _ModuleServices.CurrentModule.GetService(typeof(IOTACodeDomProvider));

			if (_CodeDomProvider != null) {
				IOTACodeDomFile _CodeDomFile = _CodeDomProvider.CodeDomFile;
				CodeObject _CodeObject = _CodeDomFile.GetDom();

				if (_CodeObject != null) {
					CodeCompileUnit _CodeCompileUnit = (CodeCompileUnit) _CodeObject;

					for (int i = 0; i < _CodeCompileUnit.Namespaces.Count; i++) {
						CodeNamespace _CodeNamespace = _CodeCompileUnit.Namespaces[i];

						for (int j = 0; j < _CodeNamespace.Types.Count; j++) {
							CodeTypeDeclaration _CodeTypeDeclaration = _CodeNamespace.Types[j];

							if (_CodeTypeDeclaration.IsClass) {
								string _ClassName = _CodeTypeDeclaration.Name;

								for (int k = 0; k < _CodeTypeDeclaration.Members.Count; k++) {
									CodeTypeMember _CodeTypeMember  = _CodeTypeDeclaration.Members[k];
									MethodInfo _MethodInfo;

									if (_CodeTypeMember is CodeConstructor) {
										CodeConstructor _CodeConstructor = (CodeConstructor) _CodeTypeMember;

										if (_CodeConstructor.LinePragma != null) {
											_MethodInfo = new MethodInfo();
											_MethodInfo.CodeMemberMethod = _CodeConstructor;
											_MethodInfo.ClassName = _ClassName;
											_MethodInfo.IsConstructor = true;
											aArrayList.Add(_MethodInfo);
										}
									}
									else if (_CodeTypeMember is CodeMemberMethod) {
										CodeMemberMethod _CodeMemberMethod = (CodeMemberMethod) _CodeTypeMember;

										if (_CodeMemberMethod.LinePragma != null) {
											_MethodInfo = new MethodInfo();
											_MethodInfo.CodeMemberMethod = _CodeMemberMethod;
											_MethodInfo.ClassName = _ClassName;
											aArrayList.Add(_MethodInfo);
										}
									}
						}
							}
						}
					}

				}
			}
      }
	               
	  public static void LoadFieldInfoInList(ArrayList aArrayList) {
			IOTAModuleServices _ModuleServices = (IOTAModuleServices) BorlandIDE.GetService(typeof(IOTAModuleServices));
			IOTACodeDomProvider _CodeDomProvider = (IOTACodeDomProvider) _ModuleServices.CurrentModule.GetService(typeof(IOTACodeDomProvider));

			if (_CodeDomProvider != null) {
				IOTACodeDomFile _CodeDomFile = _CodeDomProvider.CodeDomFile;
				CodeObject _CodeObject = _CodeDomFile.GetDom();

				if (_CodeObject != null) {
					CodeCompileUnit _CodeCompileUnit = (CodeCompileUnit) _CodeObject;

					for (int i = 0; i < _CodeCompileUnit.Namespaces.Count; i++) {
						CodeNamespace _CodeNamespace = _CodeCompileUnit.Namespaces[i];

						for (int j = 0; j < _CodeNamespace.Types.Count; j++) {
							CodeTypeDeclaration _CodeTypeDeclaration = _CodeNamespace.Types[j];

							if (_CodeTypeDeclaration.IsClass) {
								string _ClassName = _CodeTypeDeclaration.Name;

								for (int k = 0; k < _CodeTypeDeclaration.Members.Count; k++) {
									CodeTypeMember _CodeTypeMember  = _CodeTypeDeclaration.Members[k];
									FieldInfo _FieldInfo;

									if (_CodeTypeMember is CodeMemberProperty ||
										_CodeTypeMember is CodeMemberField) {

										CodeMemberProperty _CodeMemberProperty = (CodeMemberProperty) _CodeTypeMember;

										if (_CodeTypeMember.LinePragma != null ) {
											_FieldInfo = new FieldInfo();
											_FieldInfo.CodeTypeMember = _CodeTypeMember;
											_FieldInfo.ClassName = _ClassName;
											_FieldInfo.IsProperty = _CodeTypeMember is CodeMemberProperty;
											aArrayList.Add(_FieldInfo);
										}
									}
								}
							}
						}
					}

				}
			}
      	}
	}

}