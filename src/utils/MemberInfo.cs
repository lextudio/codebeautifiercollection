using System;
using System.CodeDom;
using System.Collections;
using Borland.Studio.ToolsAPI;

namespace Lextm.JcfExpert.Utils {

	public class MemberInfo {

		/**************************************************************/
		/*                      Public Fields
		/**************************************************************/

		public string ClassName;
		public CodeTypeMember CodeTypeMember;
		public bool IsStatic;
		public bool IsProperty;
		public bool IsField;

	}
}