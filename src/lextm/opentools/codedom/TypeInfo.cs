// This is type info class. Ported from SBT type info class.
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
using System.Diagnostics;

namespace Lextm.OpenTools.CodeDom
{
	/// <summary>
	/// TypeInfo.
	/// </summary>
	public class TypeInfo : CodeDomInfo
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="tag">Tag</param>
		public TypeInfo(CodeTypeDeclaration tag)
			: base(tag)
		{
			Trace.Assert(tag != null);
			name = tag.Name;
			if (tag.LinePragma != null) {
				lineNumber = tag.LinePragma.LineNumber;
			} 
			if (tag.IsClass) {
				imageIndex = DEFAULT_CLASS_IMAGEINDEX;
			} else if (tag.IsEnum) {
				imageIndex = DEFAULT_CLASS_IMAGEINDEX;
			} else if (tag.IsInterface) {
				imageIndex = DEFAULT_INTERFACE_IMAGEINDEX;
			} else if (tag.IsStruct) {
				imageIndex = DEFAULT_CLASS_IMAGEINDEX;
			} else {
				imageIndex = -1;
			}
            if (tag.IsClass) {
                codeDomType = "Class";
            } else if (tag.IsEnum) {
                codeDomType = "Enum";
            } else if (tag.IsInterface) {
                codeDomType = "Interface";
            } else if (tag.IsStruct) {
                codeDomType = "Struc";
            } else {
                codeDomType = "Delegate";
            }			
		}
		private string name;
		private int lineNumber;
		private int imageIndex;
		private string codeDomType;
		/// <summary>
		/// Generates XML comments.
		/// </summary>
		/// <returns></returns>
		public override string GenerateXmlComments()
		{
			return "<summary>" + Environment.NewLine + "/// " + name + CodeDomType.ToLowerInvariant() +
				"." + Environment.NewLine + "/// </summary>";
		}
		/// <summary>
		/// Code DOM type.
		/// </summary>
		public override string CodeDomType {
			get { return codeDomType; }
		}
		/// <summary>
		/// Full name.
		/// </summary>
		public override string FullName {
			get { return name; }
		}
		/// <summary>
		/// Image index.
		/// </summary>
		public override int ImageIndex {
			get { return imageIndex; }
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
	}
}
