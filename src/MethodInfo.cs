using System;
using System.CodeDom;
using System.Collections;
using Borland.Studio.ToolsAPI;

namespace Lextm.JcfExpert.Utils {

	public class MethodInfo {

		/**************************************************************/
		/*                    Public
		/**************************************************************/

		public void Goto () {
			int _LineNumber = CodeMemberMethod.LinePragma.LineNumber;

			if (_LineNumber > 0) {
				IOTASourceEditor _SourceEditor = OTAUtils.GetSourceEditor(OTAUtils.GetCurrentModule());

				IOTAEditView _EditView = _SourceEditor.GetEditView(0);

				_EditView.Buffer.EditPosition.GotoLine(_LineNumber);
				_EditView.MoveViewToCursor();
				_EditView.Paint();
				}
		}

		/**************************************************************/
		/*                      Public Fields
		/**************************************************************/

		public string ClassName;
		public CodeMemberMethod CodeMemberMethod;
		public bool IsConstructor;
		public bool IsStatic;
	}
}