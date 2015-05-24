// This is method info class. Ported from SBT method info class.
// Copyright (C) 2007  Lex Y. Li
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
using System;
using System.CodeDom;
using System.Text;

namespace Lextm.OpenTools.CodeDom
{
	/// <summary>
	/// MethodInfo.
	/// </summary>
	public class MethodInfo : CodeDomInfo
	{
		/// <summary>
		/// Creation method.
		/// </summary>
		/// <param name="className">Class name</param>
		/// <param name="isGet">isGet flag</param>
		/// <param name="member">Member</param>
		/// <param name="statement">Statement</param>
		/// <returns></returns>
		/// <remarks>For properties.</remarks>
		public static MethodInfo CreateProperty(string className,
		                                        bool isGet,
												CodeTypeMember member,
		                                        CodeStatement statement)
		{
			return new MethodInfo(member, className, true, isGet, statement);
		}
		/// <summary>
		/// Creation method.
		/// </summary>
		/// <param name="member">Member</param>
		/// <param name="className">Class name</param>
		/// <remarks>For general methods.</remarks>		
		public static MethodInfo CreateMethod(string className, CodeTypeMember member) {
			return new MethodInfo(member, className, false, false, null);
		}
		
		private MethodInfo(CodeTypeMember tag, string className, 
		                   bool isProperty, bool isGet,
		                   CodeStatement statement) : base(tag, className)
		{
			System.Diagnostics.Trace.Assert(tag != null);
			this.isGet = isProperty && isGet;
			this.isSet = isProperty && !isGet;
			this.codeStatement = statement;
			this.isConstructor = tag is CodeConstructor || tag is CodeTypeConstructor;
			// name section
			if (this.isGet) {
				name = "get_" + tag.Name;
			} else if (this.isSet) {
				name = "set_" + tag.Name;
			} else if (this.isConstructor) {
				name = className;
			} else {
				name = tag.Name;
			}
			// line number section
			if (codeStatement != null && codeStatement.LinePragma != null) {
				lineNumber = codeStatement.LinePragma.LineNumber;
			} 
			#region comments
			/*
                ArrayList result = new ArrayList(CodeMemberMethod.UserData.Keys);

                OTAUtils.ShowDebug("------------" + Name + "--------------");
                foreach(string _Item in result) {
                    OTAUtils.ShowDebug(_Item + "[" + CodeMemberMethod.UserData[_Item].GetType().Name + "]=" + CodeMemberMethod.UserData[_Item]);

                    if (CodeMemberMethod.UserData[_Item] is ArrayList) {
                        OTAUtils.ShowDebug("--- >>");

                        foreach(string _I in (CodeMemberMethod.UserData[_Item] as ArrayList)) {
                            OTAUtils.ShowDebug(_I + "[" + _I.GetType().Name + "]");
                         }
                        OTAUtils.ShowDebug("<<");
                    }
                }
                OTAUtils.ShowDebug("------------------------------");
			 */
			#endregion

			// If the Line exists, uses it to locate the source
			// Delphi set the Line to the line number in the implementation
			else if (tag.UserData["Line"] != null) {
				//OTAUtils.ShowDebug("LineNumber: " + CodeMemberMethod.UserData["Line"]);
				lineNumber = (int) tag.UserData["Line"];
				/*
                } else if (CodeMemberMethod.LinePragma == null) {
                    if (CodeMemberMethod.UserData["TopPosition"] != null) {
                        return (int) CodeMemberMethod.UserData["TopPosition"];
                    } else {
                        return 0;
                    }
				 */
			} else {
				if (tag.LinePragma != null)
				{
					lineNumber = tag.LinePragma.LineNumber;
				} 
			}
			// extra
			if (isProperty) {
				CodeMemberProperty pro = (CodeMemberProperty)tag; 
				pars = pro.Parameters;
				returnType = pro.Type.BaseType;
			} else {
				CodeMemberMethod met = (CodeMemberMethod)tag;
				pars = met.Parameters;
				returnType = met.ReturnType.BaseType;
			}
		}
	
		private CodeStatement codeStatement;
		/// <summary>
		/// Code statement.
		/// </summary>
		public CodeStatement CodeStatement {
			get { return codeStatement; }
		}
		private bool isGet;
		/// <summary>
		/// Is getter.
		/// </summary>
		public bool IsGet {
			get { return isGet; }
		}
        private bool isSet;
        /// <summary>
        /// Is setter.
        /// </summary>
		public bool IsSet {
			get { return isSet; }
		}
        
		private string name;
		private int lineNumber;
		private CodeParameterDeclarationExpressionCollection pars;
		private string returnType;
		private bool isConstructor;
		/// <summary>
		/// Generates XML comments.
		/// </summary>
		/// <returns></returns>
		public override string GenerateXmlComments()
		{
			StringBuilder result = 
				new StringBuilder("<summary>" + Environment.NewLine + "/// " + 
				                  name +
				                  "." + Environment.NewLine + "/// </summary>");
			
			foreach(CodeParameterDeclarationExpression par in pars)
			{
				result.Append(Environment.NewLine + "/// <param name=\"" + par.Name + "\"></param>");
			}
			if (ReturnType.ToUpperInvariant() != "SYSTEM.VOID") {
				result.Append(Environment.NewLine + "/// <returns></returns>");
			}
			return result.ToString();
		}		
		
		private static string FindParametersFrom(CodeParameterDeclarationExpressionCollection pars) {
			if (pars.Count == 0) {
				return String.Empty;
			}
			StringBuilder result = new StringBuilder("(");

			for(int i = 0; i < pars.Count; i++)
			{
				FormatParamCSharpInto(result, pars[i]);
				if (i == pars.Count - 1) {
					break;
				}
				result.Append(", ");
			}
			result.Append(")");
			return result.ToString();
		}
		private static void FormatParamCSharpInto(
			StringBuilder name,
			CodeParameterDeclarationExpression exp) {
			switch(exp.Direction) {
					case FieldDirection.In: break;
					case FieldDirection.Out: name.Append("out "); break;
					case FieldDirection.Ref: name.Append("ref "); break;
					default: break;
			}
			name.Append(exp.Type.BaseType);
		}
		/// <summary>
		/// Is constructor.
		/// </summary>
		public bool IsConstructor {
			get { return isConstructor; }
		}
		/// <summary>
		/// Code DOM type.
		/// </summary>
		public override string CodeDomType {
			get { return "Method"; }
		}
		/// <summary>
		/// Full name.
		/// </summary>
		public override string FullName {
			get { return ClassName + "." + Name + FindParametersFrom(pars); }
		}
		/// <summary>
		/// Image index.
		/// </summary>
		public override int ImageIndex {
			get {
				if (IsInternal && IsStatic) {
					return INTERNAL_STATIC_METHOD_IMAGEINDEX;
				} else if (IsInternal && IsVirtual) {
					return INTERNAL_VIRTUAL_METHOD_IMAGEINDEX;
				} else if (IsInternal) {
					return INTERNAL_METHOD_IMAGEINDEX;
				} else if (IsPrivate && IsStatic) {
					return PRIVATE_STATIC_METHOD_IMAGEINDEX;
				} else if (IsPrivate && IsVirtual) {
					return PRIVATE_VIRTUAL_METHOD_IMAGEINDEX;
				} else if (IsPrivate) {
					return PRIVATE_METHOD_IMAGEINDEX;
				} else if ((IsProtected || IsProtectedInternal)&& IsStatic) {
					return PROTECTED_STATIC_METHOD_IMAGEINDEX;
				} else if ((IsProtected || IsProtectedInternal) && IsVirtual) {
					return PROTECTED_VIRTUAL_METHOD_IMAGEINDEX;
				} else if (IsProtected || IsProtectedInternal) {
					return PROTECTED_METHOD_IMAGEINDEX;
				} else if (IsPublic && IsStatic) {
					return PUBLIC_STATIC_METHOD_IMAGEINDEX;
				} else if (IsPublic && IsVirtual) {
					return PUBLIC_VIRTUAL_METHOD_IMAGEINDEX;
				} else if (IsPublic) {
					return PUBLIC_METHOD_IMAGEINDEX;
				} else {
					return DEFAULT_METHOD_IMAGEINDEX;
				}
			}
		}
		/// <summary>
		/// Line number.
		/// </summary>
		public override int LineNumber {
			get { return lineNumber; }
		}
		/// <summary>
		/// Name.
		/// </summary>
		public override string Name {
			get { return name; }
		}

		/// <summary>
		/// Return type.
		/// </summary>
		public string ReturnType {
			get {
				return returnType;
			}
		}
//		/// <summary>
//		/// Abstract.
//		/// </summary>
//		public string Abstract {
//			get {
//				return IsAbstract;
//			}
//		}
	}
}
