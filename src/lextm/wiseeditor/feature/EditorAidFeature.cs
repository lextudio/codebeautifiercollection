using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Windows.Forms;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Gui;
using BeWise.SharpBuilderTools.Helpers;
using Borland.Studio.ToolsAPI;
using Lextm.OpenTools;

namespace Lextm.WiseEditor.Feature {

	public class EditorAidFeature : Lextm.OpenTools.CustomFeature {

        /**************************************************************/
        /*                        Private
        /**************************************************************/

        private static void HyperMove(bool aBack) {
            if (!ValidationHelpers.ValidateCurrentSourceEditorNotNull()) {
                return;
            }

            IOTASourceEditor _SourceEditor = OtaUtils.GetCurrentSourceEditor();

			IOTAEditView _EditView = _SourceEditor.GetEditView(0);

            string _Source = OtaUtils.GetSourceEditorText(_SourceEditor);
            int _StartPos = OtaUtils.GetSourceEditorTextPos(_SourceEditor);
            int _Pos = -1;
            bool _Found = false;

            if (aBack) {
                int i = _StartPos - 1;
                while (i > 0) {
                    if (// 'zA'
                        (Char.IsLower(_Source, i-1) && Char.IsUpper(_Source, i)) ||
                        // '.R' or '_T' or ' P'
                        (Char.IsLetterOrDigit(_Source, i-1) && !Char.IsLetterOrDigit(_Source,i)) ||
                        // '8y' or '7P'
                        (Char.IsDigit(_Source, i-1) && Char.IsLetter(_Source, i)) ||
                        // 'A3' or 'd6'
                        (Char.IsLetter(_Source, i-1) && Char.IsDigit(_Source, i)) ||
                        // 'RGx'
                        (Char.IsUpper(_Source, i) && Char.IsUpper(_Source, i-1) && i + 1 < _Source.Length && Char.IsLower(_Source, i+1))
                    ) {
                        _Pos = i;
                        _Found = true;
                        break;
                    }
                    i--;
                }
                if (!_Found) {
                    _Pos = 0;
                    _Found = true;
                }
            } else {
                int i = _StartPos;
                while (i < _Source.Length - 1) {
                    if (// 'aZ'
                        (Char.IsLower(_Source, i) && Char.IsUpper(_Source, i+1)) ||
                        // '8a' or '8A'
                        (Char.IsLetter(_Source, i) && Char.IsDigit(_Source, i+1)) ||
                        // 'a8' or 'A8'
                        (Char.IsDigit(_Source, i) && Char.IsLetter(_Source, i+1)) ||
                        // '.R' or '_T' or ' P'
                        (!Char.IsLetterOrDigit(_Source,i) && Char.IsLetterOrDigit(_Source, i+1)) ||
                        // 'RGx'
                        (Char.IsUpper(_Source, i) && Char.IsUpper(_Source, i+1) && i+2 < _Source.Length - 1 && Char.IsLower(_Source, i+2))
                    ) {
                        _Pos = i + 1;
                        _Found = true;
                        break;
                    }
                    i++;
                }
                if (!_Found) {
                    _Pos = _Source.Length;
                    _Found = true;
                }
            }

            if (_Found) {
                if ((_EditView.Block.Style == OTABlockType.btNonInclusive) ||
                        (_EditView.Block.Size > 0)) {
                    int _SelStart = OtaUtils.CharPosToBufferPos(_SourceEditor, _SourceEditor.BlockStart);
                    int _SelEnd   = OtaUtils.CharPosToBufferPos(_SourceEditor, _SourceEditor.BlockAfter);
                    _EditView.Block.Reset();
                    if (_SelStart == _StartPos) {
                        _EditView.CursorPos = OtaUtils.BufferPosToEditPos(_SourceEditor, _Pos);
                        _EditView.Block.BeginBlock();
                        _EditView.CursorPos = OtaUtils.BufferPosToEditPos(_SourceEditor, _SelEnd);
                    } else if (_SelEnd == _StartPos) {
                        _EditView.CursorPos = OtaUtils.BufferPosToEditPos(_SourceEditor, _SelStart);
                        _EditView.Block.BeginBlock();
                        _EditView.CursorPos = OtaUtils.BufferPosToEditPos(_SourceEditor, _Pos);
                    }
                    _EditView.Block.EndBlock();
                }
                _EditView.CursorPos = OtaUtils.BufferPosToEditPos(_SourceEditor, _Pos);
                _EditView.MoveViewToCursor();
                _EditView.Paint();
            }
        }

        private static void SortSelectedText(bool aAscending, bool aCaseSensitive) {
            IOTASourceEditor _SourceEditor = OtaUtils.GetCurrentSourceEditor();
			if (_SourceEditor == null)
			{
				return;
            }
			string _Text = OtaUtils.GetSelectedText(OtaUtils.GetCurrentModule());

            if (_Text == null || _Text.Length == 0) {
                OtaUtils.SelectLine(OtaUtils.GetCurrentSourceEditor());
                _Text = OtaUtils.GetSelectedText(OtaUtils.GetCurrentModule());
            }

			List<string>  _Lines = new List<string>(Lextm.StringHelper.GetLinesFromString(_Text));

            // Do the sorting
            SortHelper _SortHelper = new SortHelper();
            _SortHelper.Ascending = aAscending;
            _SortHelper.CaseSensitive = aCaseSensitive;

            _Lines.Sort(_SortHelper);

            _Text = OtaUtils.BuildLines(_Lines, true);
            OtaUtils.ReplaceSelectedText(_SourceEditor, _Text);
            _SourceEditor.GetEditView(0).Paint();
        }

        /**************************************************************/
        /*                        Protected
        /**************************************************************/

        //private void DoBackHistory(object aSender, EventArgs e) {
        //    bool result = OTAUtils.ExecuteMenu("BackMenuItem");
        //}
/*
		private void DoComment(object aSender, EventArgs e) {
            if (!ValidationHelpers.ValidateCurrentModuleIsCSOrXmlOrDelphiFile()) {
                return;
            }

            IOTASourceEditor _SourceEditor = OTAUtils.GetCurrentSourceEditor();

            if (_SourceEditor != null) {
                string _Text = OTAUtils.GetSelectedText(OTAUtils.GetCurrentModule());
                bool _NoTextSelected = (_Text == null || _Text.Length == 0);

                if (_NoTextSelected) {
                    OTAUtils.SelectLine(OTAUtils.GetCurrentSourceEditor());
                    _Text = OTAUtils.GetSelectedText(OTAUtils.GetCurrentModule());
                }

				string[] _Lines = ShareUtils.GetLinesFromString(_Text);

                if (_Lines.Length > 0) {
                    BaseLanguageCodeHelper _LanguageCodeHelper = LanguageCodeHelperFactory.GetLanguageCodeHelper(OTAUtils.GetCurrentEditorFileName());
                    _Lines = _LanguageCodeHelper.CommentLines(_Lines);

                    _Text = OTAUtils.BuildLines(_Lines, _Text.EndsWith("\n"));
                    OTAUtils.ReplaceSelectedText(_SourceEditor, _Text);

                    _SourceEditor.GetEditView(0).Paint();
                }
            }
        }
*/
		private void DoConvertSpaceToTab(object aSender, EventArgs AEventArgs) {
            if (!ValidationHelpers.ValidateCurrentSourceEditorNotNull()) {
                return;
            }

            IOTASourceEditor _SourceEditor = OtaUtils.GetCurrentSourceEditor();
			IOTAEditView _EditView = _SourceEditor.GetEditView(0);

			int _Error;

            // Replace All
			while (_EditView.Buffer.EditPosition.Replace(new string(' ', OtaUtils.GetCSIndentationFromOptions()),
                    "\t",
                    false,
                    false,
                    true,
                    OTASearchDirection.sdForward,
                    out _Error) > 0)
                ;
            _EditView.Paint();
        }

		private void DoConvertTabToSpace(object aSender, EventArgs AEventArgs) {
            if (!ValidationHelpers.ValidateCurrentSourceEditorNotNull()) {
                return;
            }

            IOTASourceEditor _SourceEditor = OtaUtils.GetCurrentSourceEditor();
			IOTAEditView _EditView = _SourceEditor.GetEditView(0);

            int _Error;

            // Replace All
            while (_EditView.Buffer.EditPosition.Replace("\t",
                    new string(' ', OtaUtils.GetCSIndentationFromOptions()),
                    false,
                    false,
                    true,
                    OTASearchDirection.sdForward,
                    out _Error) > 0)
                ;
            _EditView.Paint();
        }

        //private void DoCreateRegion(object aSender, EventArgs e) {
        //    if (!ValidationHelpers.ValidateCurrentModuleIsSourceFile()) {
        //        return;
        //    }

        //    IOTASourceEditor _SourceEditor = OtaUtils.GetCurrentSourceEditor();

        //    if (_SourceEditor != null) {
        //        IOTAEditView _EditView = _SourceEditor.GetEditView(0);

        //        if (_EditView != null) {
        //            FrmCreateRegion _Frm = new FrmCreateRegion();

        //            if (_Frm.ShowDialog() == DialogResult.OK) {
        //                string _Text = OtaUtils.GetSelectedText(OtaUtils.GetCurrentModule());
        //                bool _NoTextSelected = (_Text == null || _Text.Length == 0);

        //                if (_NoTextSelected) {
        //                    OtaUtils.SelectLine(OtaUtils.GetCurrentSourceEditor());
        //                    _Text = OtaUtils.GetSelectedText(OtaUtils.GetCurrentModule());
        //                }

        //                IList<string> _Lines = MiscUtils.GetLinesFromString(_Text);
        //                string[] _NewLines = new string[_Lines.Count + 2];
        //                BaseLanguageCodeHelper _LanguageCodeHelper = LanguageCodeHelperFactory.GetLanguageCodeHelper(OtaUtils.GetCurrentEditorFileName());

        //                if (_EditView.CursorPos.Col > 1 && !_NoTextSelected) {
        //                    _NewLines[0] = new String(' ', _EditView.CursorPos.Col) + _LanguageCodeHelper.CreateRegionHeader(_Frm.RegionText);
        //                } else  {
        //                    _NewLines[0] = OtaUtils.GetStringBeforeFirstWord() + _LanguageCodeHelper.CreateRegionHeader(_Frm.RegionText);
        //                }

        //                for (int i = 0; i < _Lines.Count; i++) {
        //                    _NewLines[i+1] = _Lines[i];
        //                }

        //                if (_EditView.CursorPos.Col > 1 && !_NoTextSelected) {
        //                    _NewLines[_NewLines.Length -1] = new String(' ', _EditView.CursorPos.Col) + _LanguageCodeHelper.CreateRegionFooter(_Frm.RegionText);
        //                } else  {
        //                    _NewLines[_NewLines.Length -1] = OtaUtils.GetStringBeforeFirstWord() + _LanguageCodeHelper.CreateRegionFooter(_Frm.RegionText);
        //                }

        //                _Text = OtaUtils.BuildLines(_NewLines, true);
        //                OtaUtils.ReplaceSelectedText(_SourceEditor, _Text);
        //                _EditView.Paint();
        //            }
        //        }
        //    }
        //}

		private void DoDefaultSort(object aSender, EventArgs AEventArgs) {
            if (!ValidationHelpers.ValidateCurrentSourceEditorNotNull()) {
                return;
            }

            SortSelectedText(true, true);
        }

		private void DoElideToDefinitions(object sender, EventArgs e) {
			if (!ValidationHelpers.ValidateCurrentModuleNotNull()) {
				return;
            }
			OtaUtils.GetCurrentEditor().Show();
            IOTAEditView _EditView = OtaUtils.GetCurrentEditView();

			_EditView.Paint();

			int Line = _EditView.CursorPos.Line;
            short Col = _EditView.CursorPos.Col;

			int _count = 0;
			InteriorElideToDefinitions(null , null, ref _count);
			//OTAUtils.ElideNearestBlock(_EditView, false);
			_EditView.Paint();

            OtaUtils.GoToPosition(OtaUtils.GetCurrentSourceEditor(), Line, Col);
			_EditView.MoveViewToCursor();
			_EditView.Paint();
		}
		
		private void DoElideNearestBlock(object aSender, EventArgs AEventArgs) {
            IOTAEditView _EditView = OtaUtils.GetCurrentEditView();
            if (_EditView != null) {
                OtaUtils.ElideNearestBlock(_EditView, false);
                _EditView.Paint();
            }
        }

        //private void DoForwardHistory(object aSender, EventArgs e) {
        //    OTAUtils.ExecuteMenu("ForwardMenuItem");
        //}

		private void DoGetNextOccurence(object aSender, EventArgs AEventArgs) {
			DoGetNextPriorOccurence(false);
        }

		private static void DoGetNextPriorOccurence(bool aPrev) {
            if (!ValidationHelpers.ValidateCurrentSourceEditorNotNull()) {
                return;
            }

            IOTASourceEditor _SourceEditor = OtaUtils.GetCurrentSourceEditor();
			IOTAEditView _EditView = _SourceEditor.GetEditView(0);
            string _Word = OtaUtils.GetCurrentSelectedText();
            bool _Found = false;
            int _Pos = 0;

            // Find with the Current Word
            if (String.IsNullOrEmpty(OtaUtils.GetCurrentSelectedText())) {
                _Word = OtaUtils.GetCurrentWord(_SourceEditor);

                _Found = OtaUtils.FindTextIdent(_Word,
                                                OtaUtils.GetSourceEditorText(_SourceEditor),
                                                OtaUtils.GetSourceEditorTextPos(_SourceEditor),
                                                aPrev,
                                                out _Pos);

                if (_Found) {
                    _EditView.CursorPos = OtaUtils.BufferPosToEditPos(_SourceEditor, _Pos);
                    OtaUtils.ElideNearestBlock(_EditView, true);
                    // clear selected block
                    _EditView.Block.Reset();
                    _EditView.CursorPos = OtaUtils.BufferPosToEditPos(_SourceEditor, _Pos);
                }
            // Find with the SelectedText
            } else {
                _Found = OtaUtils.FindSelectionText(_Word,
                                                    OtaUtils.GetSourceEditorText(_SourceEditor),
                                                    OtaUtils.GetSourceEditorTextPos(_SourceEditor),
                                                    aPrev,
                                                    out _Pos);

                if (_Found) {
                    _EditView.CursorPos = OtaUtils.BufferPosToEditPos(_SourceEditor, _Pos);
                    OtaUtils.ElideNearestBlock(_EditView, true);

                    // clear selected block
                    _EditView.Block.Reset();
                    _EditView.CursorPos = OtaUtils.BufferPosToEditPos(_SourceEditor, _Pos);

                    _EditView.Block.BeginBlock();
                    _EditView.CursorPos = OtaUtils.BufferPosToEditPos(_SourceEditor, _Pos + _Word.Length);
                    _EditView.Block.EndBlock();

                }
            }

            if (_Found) {
                _EditView.MoveViewToCursor();
                _EditView.Paint();
            }
        }

		private void DoGetPriorOccurence(object aSender, EventArgs AEventArgs) {
            DoGetNextPriorOccurence(true);
        }

        //private void DoGoToNextError(object aSender, EventArgs e) {
        //    IOTAMessageService _MessageService = OTAUtils.GetMessageService();

        //    _MessageService.NextErrorMessage(true, true);
        //}

        //private void DoGoToPreviousError(object aSender, EventArgs e) {
        //    IOTAMessageService _MessageService = OTAUtils.GetMessageService();

        //    _MessageService.NextErrorMessage(false, true);
        //}

		private void DoHyperBack(object aSender, EventArgs AEventArgs) {
            HyperMove(true);
        }

		private void DoHyperForward(object aSender, EventArgs AEventArgs) {
            HyperMove(false);
        }

		private void DoIndent(object aSender, EventArgs AEventArgs) {
            if (!ValidationHelpers.ValidateCurrentSourceEditorNotNull()) {
                return;
			}

            IOTASourceEditor _SourceEditor = OtaUtils.GetCurrentSourceEditor();
			IOTAEditView _EditView = _SourceEditor.GetEditView(0);
            BaseLanguageCodeHelper _LanguageCodeHelper = LanguageCodeHelperFactory.GetLanguageCodeHelper(OtaUtils.GetCurrentEditorFileName());

            _EditView.Buffer.EditBlock.Indent(_LanguageCodeHelper.GetIndentation());
            _EditView.Paint();
        }

        //private void DoInsertCodeTemplate(object aSender, EventArgs e) {
        //    IOTAEditView _EditView = OTAUtils.GetCurrentEditView();

        //    if (_EditView != null) {
        //        IOTAEditActions _EditActions = _EditView as IOTAEditActions;

        //        if (_EditActions != null) {
        //            _EditActions.CodeTemplate();
        //        }
        //    }
        //}

		private void DoInsertFileAsText(object aSender, EventArgs AEventArgs) {
            if (!ValidationHelpers.ValidateCurrentSourceEditorNotNull()) {
                return;
            }

            IOTAEditView _EditView = OtaUtils.GetCurrentEditView();

            if (_EditView != null) {
                Vista_Api.OpenFileDialog _OpenFileDialog = new Vista_Api.OpenFileDialog();
                _OpenFileDialog.Title = "Sharp Builder Tools - Select the file to insert";
                _OpenFileDialog.Filter += "Any file (*.*)|*.*";
                _OpenFileDialog.CheckFileExists = true;

                if (_OpenFileDialog.ShowDialog() == DialogResult.OK) {
                    _EditView.Position.InsertFile(_OpenFileDialog.FileName);
                }
            }
        }

		private void DoLowerCase(object aSender, EventArgs AEventArgs) {
            if (!ValidationHelpers.ValidateCurrentSourceEditorNotNull()) {
                return;
            }

            IOTASourceEditor _SourceEditor = OtaUtils.GetCurrentSourceEditor();
			IOTAEditView _EditView =  _SourceEditor.GetEditView(0);
            
            _EditView.Buffer.EditBlock.LowerCase();
            _EditView.Paint();
        }

        //private void DoShowCodeComplete(object aSender, EventArgs e)    {
        //    IOTAEditView _EditView = OTAUtils.GetCurrentEditView();

        //    if (_EditView != null) {
        //        IOTAEditActions _EditActions = _EditView as IOTAEditActions;

        //        if (_EditActions != null) {
        //            _EditActions.CodeCompletion(OTACodeCompleteStyle.csCodelist);
        //        }
        //    }
        //}

        //private void DoShowCodeCompleteParams(object aSender, EventArgs e) {
        //    IOTAEditView _EditView = OTAUtils.GetCurrentEditView();

        //    if (_EditView != null) {
        //        IOTAEditActions _EditActions = _EditView as IOTAEditActions;

        //        if (_EditActions != null) {
        //            _EditActions.CodeCompletion(OTACodeCompleteStyle.csParamList);
        //        }
        //    }
        //}

		private void DoSort(object aSender, EventArgs AEventArgs) {
            if (!ValidationHelpers.ValidateCurrentSourceEditorNotNull()) {
                return;
            }

            FrmSortOptions _Frm = new FrmSortOptions();

            if (_Frm.ShowDialog() == DialogResult.OK) {
                SortSelectedText(_Frm.Ascending, _Frm.CaseSensitive);
            }
        }

		private void DoToggleCase(object aSender, EventArgs AEventArgs) {
            if (!ValidationHelpers.ValidateCurrentSourceEditorNotNull()) {
                return;
            }

            IOTASourceEditor _SourceEditor = OtaUtils.GetCurrentSourceEditor();
            IOTAEditView _EditView = _SourceEditor.GetEditView(0);

			_EditView.Buffer.EditBlock.ToggleCase();
            _EditView.Paint();
        }

		private void DoToggleComment(object aSender, EventArgs AEventArgs) {
			if (!ValidationHelpers.ValidateCurrentModuleIsSourceFile()) {
				return;
            }

            IOTASourceEditor _SourceEditor = OtaUtils.GetCurrentSourceEditor();

            if (_SourceEditor != null) {
                string _Text = OtaUtils.GetSelectedText(OtaUtils.GetCurrentModule());
                bool _NoTextSelected = string.IsNullOrEmpty(_Text);

                if (_NoTextSelected) {
                    OtaUtils.SelectLine(OtaUtils.GetCurrentSourceEditor());
                    _Text = OtaUtils.GetSelectedText(OtaUtils.GetCurrentModule());
                }

				IList<string> _Lines = Lextm.StringHelper.GetLinesFromString(_Text);

                if (_Lines.Count > 0) {
                    BaseLanguageCodeHelper _LanguageCodeHelper = LanguageCodeHelperFactory.GetLanguageCodeHelper(OtaUtils.GetCurrentEditorFileName());

                    for (int i = 0; i < _Lines.Count; i++) {
                        if (_LanguageCodeHelper.IsCommented(_Lines[i])) {
                            _Lines[i] = _LanguageCodeHelper.UncommentLine(_Lines[i]);
                        } else {
                            _Lines[i] = _LanguageCodeHelper.CommentLine(_Lines[i]);
                        }
                    }

                    _Text = OtaUtils.BuildLines(_Lines, _Text.EndsWith(Environment.NewLine, StringComparison.Ordinal));
                    OtaUtils.ReplaceSelectedText(_SourceEditor, _Text);

                    _SourceEditor.GetEditView(0).Paint();
                }
            }
        }
/*
        protected void DoViewUsing(object aSender, EventArgs e) {
            IOTAModule _Module = OTAUtils.GetCurrentModule();

            IOTACodeDomProvider _CodeDomProvider = (IOTACodeDomProvider) _Module.GetService(typeof(IOTACodeDomProvider));

            if (_CodeDomProvider != null) {
                IOTACodeDomFile _CodeDomFile = _CodeDomProvider.CodeDomFile;
                CodeObject _CodeObject = _CodeDomFile.GetDom();

                if (_CodeObject != null) {
                    CodeCompileUnit _CodeCompileUnit = (CodeCompileUnit) _CodeObject;

                    ArrayList result = new ArrayList(_CodeCompileUnit.UserData.Keys);
                    for (int i = 0; i < result.Count; i++) {
                        ValidationHelpers.ShowDebug(result[i].ToString());
                        ValidationHelpers.ShowDebug(_CodeCompileUnit.UserData[result[i]].ToString());

                    }
                }
            }
         }
*/
  /*      
		private void DoUnComment(object aSender, EventArgs e) {
            if (!ValidationHelpers.ValidateCurrentModuleIsCSOrXmlOrDelphiFile()) {
                return;
            }

            IOTASourceEditor _SourceEditor = OTAUtils.GetCurrentSourceEditor();

            if (_SourceEditor != null) {
                string _Text = OTAUtils.GetSelectedText(OTAUtils.GetCurrentModule());
                bool _NoTextSelected = (_Text == null || _Text.Length == 0);

                if (_NoTextSelected) {
                    OTAUtils.SelectLine(OTAUtils.GetCurrentSourceEditor());
                    _Text = OTAUtils.GetSelectedText(OTAUtils.GetCurrentModule());
                }

                string[] _Lines = ShareUtils.GetLinesFromString(_Text);

                if (_Lines.Length > 0) {
                    BaseLanguageCodeHelper _LanguageCodeHelper = LanguageCodeHelperFactory.GetLanguageCodeHelper(OTAUtils.GetCurrentEditorFileName());

                    if (_LanguageCodeHelper.IsCommented(_Lines)) {
                        _Lines = _LanguageCodeHelper.UnCommentLines(_Lines);
                        _Text = OTAUtils.BuildLines(_Lines, _Text.EndsWith("\n"));
                        OTAUtils.ReplaceSelectedText(_SourceEditor, _Text);
                        _SourceEditor.GetEditView(0).Paint();
                    }
                }
            }
        }
*/
		private void DoUnElideNearestBlock(object aSender, EventArgs AEventArgs) {
            IOTAEditView _EditView = OtaUtils.GetCurrentEditView();
            if (_EditView != null) {
                OtaUtils.ElideNearestBlock(_EditView, true);
                _EditView.Paint();
            }
        }

		private void DoUnElideAllBlocks(object aSender, EventArgs AEventArgs) {
            IOTAEditView _EditView = OtaUtils.GetCurrentEditView();

            if (_EditView != null) {
                IOTAElideActions _ElideActions = _EditView as IOTAElideActions;

                if (_ElideActions != null) {
                    _ElideActions.UnElideAllBlocks();
                    _EditView.Paint();
                }
            }
        }

		private void DoUnindent(object aSender, EventArgs AEventArgs) {
            if (!ValidationHelpers.ValidateCurrentSourceEditorNotNull()) {
                return;
            }

            IOTASourceEditor _SourceEditor = OtaUtils.GetCurrentSourceEditor();
            IOTAEditView _EditView = _SourceEditor.GetEditView(0);
			BaseLanguageCodeHelper _LanguageCodeHelper = LanguageCodeHelperFactory.GetLanguageCodeHelper(OtaUtils.GetCurrentEditorFileName());

            _EditView.Buffer.EditBlock.Indent(0 - _LanguageCodeHelper.GetIndentation());
            _EditView.Paint();
        }

        private void DoUpperCase(object aSender, EventArgs AEventArgs) {
            if (!ValidationHelpers.ValidateCurrentSourceEditorNotNull()) {
                return;
            }

            IOTASourceEditor _SourceEditor = OtaUtils.GetCurrentSourceEditor();

            IOTAEditView _EditView = _SourceEditor.GetEditView(0);

            _EditView.Buffer.EditBlock.UpperCase();
            _EditView.Paint();
        }

        /**************************************************************/
        /*                         Public
        /**************************************************************/
		private const string MenuEdit = "CBCEditMenu";
		private const string MenuTextEdit = "Editor Aid";

		protected override void IdeRegisterMenus() {
			RegisterEditMenu();
			RegisterSearchMenu();
		}

		private static void InteriorElideToDefinitions(IOTACodeDomProvider _CodeDomProvider,
				CodeObject _CodeObject, ref int _count)
		{
			_count++;
			if (_count > 100000){
				Lextm.Windows.Forms.MessageBoxFactory.Warn("Too large elide operation. Terminated.");
				return;
			}
			if (_CodeDomProvider == null) {
				_CodeDomProvider = (IOTACodeDomProvider) OtaUtils.GetCurrentModule().GetService(typeof(IOTACodeDomProvider));
			}

			if (_CodeDomProvider != null) {
				if (_CodeObject == null){
					IOTACodeDomFile _CodeDomFile = _CodeDomProvider.CodeDomFile;
					_CodeObject = _CodeDomFile.GetDom();
				}
				if (_CodeObject != null) {


					if (_CodeObject is CodeCompileUnit){
						for (int i = 0; i < ((CodeCompileUnit)_CodeObject).Namespaces.Count; i++){
							InteriorElideToDefinitions(_CodeDomProvider, ((CodeCompileUnit)_CodeObject).Namespaces[i], ref _count);
						}
						return;
					}
					//..............................
					if (_CodeObject is CodeNamespace){
						for (int i = 0; i < ((CodeNamespace)_CodeObject).Types.Count; i++){
							InteriorElideToDefinitions(_CodeDomProvider, ((CodeNamespace)_CodeObject).Types[i], ref _count);
						}
						return;
					}
					//..............................
					if (_CodeObject is CodeTypeMember){
						//if ( ((CodeTypeMember)_CodeObject).Name == "toolBar_Source_ButtonClick"){
						//	System.Windows.Forms.Messag/eBox.Show("!");
						//}

						//...................
						if (_CodeObject is CodeMemberEvent){
							return;
						}
						//...................
						if (_CodeObject is CodeMemberField){
							return;
						}
						//...................
						if (_CodeObject is CodeMemberMethod){

							int Line = -1;
							int StatementsLine = -1;

							try{
								if ( ((CodeTypeMember)_CodeObject).LinePragma != null){
									Line = ((CodeTypeMember)_CodeObject).LinePragma.LineNumber;
								}
							}catch{
								Line = -1;
							}

							try{
								if (((CodeMemberMethod)_CodeObject).Statements[0] != null){
									if (((CodeMemberMethod)_CodeObject).Statements[0].LinePragma != null){
										StatementsLine  = ((CodeMemberMethod)_CodeObject).Statements[0].LinePragma.LineNumber;
									}
								}
							}catch{
								StatementsLine = -1;
							}
							/*************************************/
							if (StatementsLine == -1){
								if (Line != -1){
									ElideAt(Line+1);
								}
							}else{
								if (Line == -1){
									ElideAt(StatementsLine);
								}else{
									if ( Line == StatementsLine ){
										ElideAt(Line);
									}else{
										ElideAt(StatementsLine);
									}
								}
							}
							/*************************************/
							return;
						}
						//...................
						if (_CodeObject is CodeMemberProperty){

							int Line;
							CodeStatementCollection csc;

							try{
								csc = ((CodeMemberProperty)_CodeObject).GetStatements;
								if (csc != null){
									if (csc[0].LinePragma != null){
										Line = csc[0].LinePragma.LineNumber;
										if (Line >0){
											/*************************************/
											ElideAt(Line);
											/*************************************/
										}
									}
								}
							}catch{}

							try{
								csc = ((CodeMemberProperty)_CodeObject).SetStatements;
								if (csc != null){
									if (csc[0].LinePragma != null){
										Line = csc[0].LinePragma.LineNumber;
										if (Line >0){
											/*************************************/
											ElideAt(Line);
											/*************************************/
										}
									}
								}
							}catch{}

							return;
						}
						//...................
						if (_CodeObject is CodeTypeDeclaration){

							if (_CodeObject is CodeTypeDelegate){
							}else{
								if ( ((CodeTypeDeclaration)_CodeObject).IsClass ){
								}
								if ( ((CodeTypeDeclaration)_CodeObject).IsEnum ){
								}
								if ( ((CodeTypeDeclaration)_CodeObject).IsInterface ){
								}
								if ( ((CodeTypeDeclaration)_CodeObject).IsStruct ){
								}
								for (int i=0; i < ((CodeTypeDeclaration)_CodeObject).Members.Count; i++){
									InteriorElideToDefinitions(_CodeDomProvider, ((CodeTypeDeclaration)_CodeObject).Members[i], ref _count);
								}
							}
							return;

						}//if (_CodeObject is CodeTypeDeclaration)
						//...................
					}//if (_CodeObject is CodeTypeMember)

				}//if (_CodeObject != null)
			}

		}
		private static void ElideAt(int Line){
			//System.Windows.Forms.Messag/eBox.Show("ElideAt");
			OtaUtils.GoToPosition(OtaUtils.GetCurrentSourceEditor(), Line, 1);

			//SelectCurrentLine();
			//Utils_OTAPI.CurrentEditView.Paint();
			//Utils_WinAPI.Beep(5000,1000);

			OtaUtils.GetCurrentElideActions().ElideNearestBlock();
			OtaUtils.GetCurrentEditView().Paint();
		}

		private void RegisterEditMenu( ) {
			RegisterAction(CreateEmptyMenu(Lextm.OpenTools.MenuItemLocation.Child,
                                           ShareUtils.MenuRootDefault,
										   MenuEdit,
										   MenuTextEdit)
						  );
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
												 MenuEdit,
                                                 "ToggleCaseMenu",
                                                 0,
                                                 "Toggle Case",
                                                 new EventHandler(DoToggleCase)));
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
												 MenuEdit,
                                                 "UpCaseCaseMenu",
                                                 0,
                                                 "Up Case                                      Ctrl+K+N",
                                                 new EventHandler(DoUpperCase)));
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
                                                 MenuEdit,
                                                 "LowCaseMenu",
                                                 0,
                                                 "Low Case                                    Ctrl+K+O",
                                                 new EventHandler(DoLowerCase)));
			RegisterAction(CreateSeparatorMenu(MenuItemLocation.Child,
                                                 MenuEdit));
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
												 MenuEdit,
												 "ToggleCommentMenu",
                                                 ShareUtils.Shift + ShareUtils.Ctrl + (int)Keys.Delete,      // Ctrl /
												 "ToggleComment",
												 new EventHandler(DoToggleComment)));
			RegisterAction(CreateSeparatorMenu(MenuItemLocation.Child,
                                                 MenuEdit));
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
												 MenuEdit,
												 "SortMenu",
												 0,
												 "Sort...",
												 new EventHandler(DoSort)));
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
												 MenuEdit,
												 "DefaultSortMenu",
												 OtaUtils.Ctrl + OtaUtils.Shift + OtaUtils.Alt + (int)Keys.S,
												 "Default Sort (Ascending / Case Sensitive)",
												 new EventHandler(DoDefaultSort)));
            //RegisterAction(CreateActionMenu(MenuItemLocation.Child,
            //                                     MenuEdit,
            //                                     "CreateRegionMenu",
            //                                     49234,      // Alt Ctrl C
            //                                     "Create Region...",
            //                                     new EventHandler(DoCreateRegion)));
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
												 MenuEdit,
												 "SpaceToTabMenu",
												 0,
												 "Convert Space to Tab",
												 new EventHandler(DoConvertSpaceToTab)));
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
												 MenuEdit,
                                                 "TabToSpaceMenu",
												 0,
                                                 "Convert Tab to Space",
                                                 new EventHandler(DoConvertTabToSpace)));
			RegisterAction(CreateSeparatorMenu(MenuItemLocation.Child,
                                                 MenuEdit));
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
												 MenuEdit,
												 "UnindentMenu",
                                                 ShareUtils.Alt + (int)Keys.U,      // Alt U
												 "Unindent",
												 new EventHandler(DoUnindent)));
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
												 MenuEdit,
												 "IndentMenu",
                                                 ShareUtils.Alt + (int)Keys.I,      // Alt I
												 "Indent",
												 new EventHandler(DoIndent)));
			RegisterAction(CreateSeparatorMenu(MenuItemLocation.Child,
												 MenuEdit));
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
												 MenuEdit,
												 "InsertTextFileMenu",
												 0,
												 "Insert Text File...",
												 new EventHandler(DoInsertFileAsText)));
		}

		private void RegisterSearchMenu( ) {
			RegisterAction(CreateEmptyMenu(MenuItemLocation.Child,
                                                    ShareUtils.MenuRootDefault,
													"CBCSearchMenu",
													"Search")
								   );
            //TODO: create a outline item
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
															"CBCSearchMenu",
															"UnElideAllBlocksMenu",
															0,
															"Unfold All Blocks               Shift+Ctrl+K+A",
															new EventHandler(DoUnElideAllBlocks)));
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
															"CBCSearchMenu",
															"UnElideNearestBlockMenu",
															0,
															"Unfold Nearest Block                  Shift+Ctrl+K+U",
															new EventHandler(DoUnElideNearestBlock)));
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
															"CBCSearchMenu",
															"ElideNearestBlockMenu",
															0,
															"Fold Nearest Block                   Shift+Ctrl+K+E",
															new EventHandler(DoElideNearestBlock)));
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
															"CBCSearchMenu",
															"ElideToDefinitionMenu",
															0,
															"Fold to Definition",
															new EventHandler(DoElideToDefinitions)));
			RegisterAction(CreateSeparatorMenu(MenuItemLocation.Child,
															"CBCSearchMenu"));
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
															"CBCSearchMenu",
															"GotoNextOccurenceMenu",
															OtaUtils.Ctrl + OtaUtils.Alt + (int)Keys.Down,
															"Go to Next Occurence",
															new EventHandler(DoGetNextOccurence)));
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
															"CBCSearchMenu",
															"GotoPriorOccurenceMenu",
															OtaUtils.Ctrl + OtaUtils.Alt + (int)Keys.Up,
															"Go to Prior Occurence",
															new EventHandler(DoGetPriorOccurence)));
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
															"CBCSearchMenu",
															"HyperForwardMenu",
															OtaUtils.Ctrl + OtaUtils.Alt + (int)Keys.Right,
															"Hyper Forward",
															new EventHandler(DoHyperForward)));
            RegisterAction(CreateActionMenu(MenuItemLocation.Child,
															"CBCSearchMenu",
															"HyperBackwardMenu",
															OtaUtils.Ctrl + OtaUtils.Alt + (int)Keys.Left,
															"Hyper Backward",
															new EventHandler(DoHyperBack)));
		}
	}
}
