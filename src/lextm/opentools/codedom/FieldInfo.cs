// This is field info class. Ported from SBT field info class.
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

namespace Lextm.OpenTools.CodeDom
{
	/// <summary>
	/// FieldInfo.
	/// </summary>
	public class FieldInfo : CodeDomInfo
	{	
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="tag"></param>
		/// <param name="className"></param>
		public FieldInfo(CodeTypeMember tag, string className)
			: base(tag, className)
		{
			System.Diagnostics.Trace.Assert(tag != null);
			name = tag.Name;
			lineNumber = tag.LinePragma.LineNumber;
            CodeMemberProperty pro = tag as CodeMemberProperty;
			isProperty = pro != null;
			if (isProperty) {
				returnType = pro.Type.BaseType;
			} else {
				returnType = (tag as CodeMemberField).Type.BaseType;
			}
		}
		private string name;
		private int lineNumber;
		private bool isProperty;
		private string returnType;
		/// <summary>
		/// Generates XML comments.
		/// </summary>
		/// <returns></returns>
		public override string GenerateXmlComments()
		{
			if (IsProperty) {
				return "<summary>" + Environment.NewLine + "/// " + Name +
					" property." + Environment.NewLine + "/// </summary>" +
					Environment.NewLine + "/// <returns></returns>";
			} else {
				return "<summary>" + Environment.NewLine + "/// " + Name +
					" property." + Environment.NewLine + "/// </summary>";
			}
		}
		/// <summary>
		/// Is property.
		/// </summary>
		public bool IsProperty {
			get { return isProperty; }
		}
		/// <summary>
		/// Code DOM type.
		/// </summary>
		public override string CodeDomType {
			get {
				if (IsProperty) {
					return "Property";
				} else {
					return "Field";
				}
			}
		}
		/// <summary>
		/// Full name.
		/// </summary>
		public override string FullName {
			get { return ClassName + "." + Name;; }
		}
		/// <summary>
		/// Image index.
		/// </summary>
		public override int ImageIndex {
			get {
				if (IsProperty) {
					return DEFAULT_PROPERTY_IMAGEINDEX;
				} else {
					if (IsInternal && IsStatic) {
						return INTERNAL_STATIC_FIELD_IMAGEINDEX;
					} else if (IsInternal) {
						return INTERNAL_FIELD_IMAGEINDEX;
					} else if (IsPrivate && IsStatic) {
						return PRIVATE_STATIC_FIELD_IMAGEINDEX;
					} else if (IsPrivate) {
						return PRIVATE_FIELD_IMAGEINDEX;
					} else if (IsProtected && IsStatic) {
						return PROTECTED_STATIC_FIELD_IMAGEINDEX;
					} else if (IsProtected) {
						return PROTECTED_FIELD_IMAGEINDEX;
					} else if (IsPublic && IsStatic) {
						return PUBLIC_STATIC_FIELD_IMAGEINDEX;
					} else if (IsPublic) {
						return PUBLIC_FIELD_IMAGEINDEX;
					} else {
						return DEFAULT_FIELD_IMAGEINDEX;
					}
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
			get { return returnType; }
		}
	}
}
