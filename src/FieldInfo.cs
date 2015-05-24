using System;
using System.CodeDom;
using System.Collections;
using Borland.Studio.ToolsAPI;

namespace Lextm.JcfExpert.Utils {

	public class FieldInfo {

		/**************************************************************/
		/*                      Public Fields
		/**************************************************************/

		public bool IsStatic;
		public bool IsProperty;
		public string ClassName;
		public CodeTypeMember CodeTypeMember;

	}
}