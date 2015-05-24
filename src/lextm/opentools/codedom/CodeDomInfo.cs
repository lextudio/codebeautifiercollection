// This is code dom info class. Ported from SBT code dom info class.
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

using BeWise.Common.Utils;
using Borland.Studio.ToolsAPI;

namespace Lextm.OpenTools.CodeDom
{
	/// <summary>
	/// CodeDomInfo.
	/// </summary>
	public abstract class CodeDomInfo
	{
		#region Consts
		// Scope Text
		private const string SCOPE_ABSTRACT_TEXT                             = "Abstract";
		private const string SCOPE_FINAL_TEXT                                = "Final";
		private const string SCOPE_OVERRIDE_TEXT                             = "Override";
		private const string SCOPE_STATIC_TEXT                               = "Static";
		private const string SCOPE_VIRTUAL_TEXT                              = "Virtual";

		// Access Text
		private const string ACCESS_PRIVATE_TEXT                             = "Private";
		private const string ACCESS_PROTECTED_TEXT                           = "Protected";
		private const string ACCESS_PROTECTED_INTERNAL_TEXT                  = "Protected Internal";
		private const string ACCESS_INTERNAL_TEXT                            = "Internal";
		private const string ACCESS_PUBLIC_TEXT                              = "Public";

		// Code Dom ImageIndex
		internal const int DEFAULT_CLASS_IMAGEINDEX                           = 0;
		internal const int DEFAULT_EVENT_IMAGEINDEX                           = 1;
		internal const int DEFAULT_FIELD_IMAGEINDEX                           = 2;
		internal const int DEFAULT_INTERFACE_IMAGEINDEX                       = 3;
		internal const int DEFAULT_METHOD_IMAGEINDEX                          = 4;
		internal const int DEFAULT_PROPERTY_IMAGEINDEX                        = 5;
		internal const int INTERNAL_METHOD_IMAGEINDEX                         = 6;
		internal const int INTERNAL_STATIC_METHOD_IMAGEINDEX                  = 7;
		internal const int INTERNAL_VIRTUAL_METHOD_IMAGEINDEX                 = 8;
		internal const int PRIVATE_METHOD_IMAGEINDEX                          = 9;
		internal const int PRIVATE_STATIC_METHOD_IMAGEINDEX                   = 10;
		internal const int PRIVATE_VIRTUAL_METHOD_IMAGEINDEX                  = 11;
		internal const int PROTECTED_METHOD_IMAGEINDEX                        = 12;
		internal const int PROTECTED_STATIC_METHOD_IMAGEINDEX                 = 13;
		internal const int PROTECTED_VIRTUAL_METHOD_IMAGEINDEX                = 14;
		internal const int PUBLIC_METHOD_IMAGEINDEX                           = 15;
		internal const int PUBLIC_STATIC_METHOD_IMAGEINDEX                    = 16;
		internal const int PUBLIC_VIRTUAL_METHOD_IMAGEINDEX                   = 17;
		internal const int INTERNAL_CLASS_IMAGEINDEX                          = 18;
		internal const int PRIVATE_CLASS_IMAGEINDEX                           = 19;
		internal const int PUBLIC_CLASS_IMAGEINDEX                            = 20;
		internal const int INTERNAL_FIELD_IMAGEINDEX                          = 21;
		internal const int INTERNAL_STATIC_FIELD_IMAGEINDEX                   = 22;
		internal const int PRIVATE_FIELD_IMAGEINDEX                           = 23;
		internal const int PRIVATE_STATIC_FIELD_IMAGEINDEX                    = 24;
		internal const int PROTECTED_FIELD_IMAGEINDEX                         = 25;
		internal const int PROTECTED_STATIC_FIELD_IMAGEINDEX                  = 26;
		internal const int PUBLIC_FIELD_IMAGEINDEX                            = 27;
		internal const int PUBLIC_STATIC_FIELD_IMAGEINDEX                     = 28;
		#endregion
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="tag">Tag</param>
		/// <param name="className">Class name</param>
		protected CodeDomInfo(CodeTypeMember tag, string className)
		{
			this.attributes = tag.Attributes;
			this.className = className;
		}
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="tag">Tag</param>
		protected CodeDomInfo(CodeTypeMember tag) : this(tag, null) {	}

		/// <summary>
		/// To string.
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return FullName;
		}
		/// <summary>
		/// Generates XML comments.
		/// </summary>
		/// <returns></returns>
		public virtual string GenerateXmlComments() {return String.Empty;}
		private string className;
		private MemberAttributes attributes;
		/// <summary>
		/// Gets CodeDOM type.
		/// </summary>
		/// <returns></returns>
		public abstract string CodeDomType {
			get;
		}
		/// <summary>
		/// Gets full name.
		/// </summary>
		/// <returns></returns>
		public abstract string FullName {
			get;
		}
		/// <summary>
		/// Gets image index.
		/// </summary>
		/// <returns>-1.</returns>
		public virtual int ImageIndex {
			get {
				return -1;
			}
		}
		/// <summary>
		/// Gets line number.
		/// </summary>
		/// <returns></returns>
		public abstract int LineNumber {
			get;
		}
		/// <summary>
		/// Gets name.
		/// </summary>
		/// <returns></returns>
		public abstract string Name {
			get;
		}

		/**************************************************************/
		/*                        Public
        /**************************************************************/
		/// <summary>
		/// Goto.
		/// </summary>
		public void Goto () {
			// Show the Source Editor
			IOTAModule module = OtaUtils.GetCurrentModule();
			if (module == null) {
				return;
			}
			module.ShowFileName(OtaUtils.GetEditorWithSourceEditor(module).FileName);

			if (LineNumber > 0) {
				IOTASourceEditor _SourceEditor = OtaUtils.GetSourceEditor(module);

				if (_SourceEditor != null) {
					IOTAEditView _EditView = _SourceEditor.GetEditView(0);
					_EditView.Buffer.EditPosition.GotoLine(LineNumber);
					_EditView.MoveViewToCursor();
					_EditView.Paint();
				}
			}
		}

		/**************************************************************/
		/*                         Properties
        /**************************************************************/
		/// <summary>
		/// Accesses.
		/// </summary>
		public string Access {
			get {
				switch (attributes & MemberAttributes.AccessMask) {
					case MemberAttributes.Family:
						return ACCESS_PROTECTED_TEXT;
					case MemberAttributes.Public:
						return ACCESS_PUBLIC_TEXT;
					case MemberAttributes.Assembly:
						return ACCESS_INTERNAL_TEXT;
					case MemberAttributes.FamilyOrAssembly:
						return ACCESS_PROTECTED_INTERNAL_TEXT;
					default:
						return ACCESS_PRIVATE_TEXT;
				}
			}
		}
		/// <summary>
		/// Class name.
		/// </summary>
		public string ClassName {
			get {
				return className;
			}
		}
		/// <summary>
		/// Is private.
		/// </summary>
		public bool IsPrivate {
			get {
				return Access == ACCESS_PRIVATE_TEXT;
			}
		}
		/// <summary>
		/// Is protected.
		/// </summary>
		public bool IsProtected {
			get {
				return Access == ACCESS_PROTECTED_TEXT;
			}
		}
		/// <summary>
		/// Is protected internal.
		/// </summary>
		public bool IsProtectedInternal {
			get {
				return Access == ACCESS_PROTECTED_INTERNAL_TEXT;
			}
		}
		/// <summary>
		/// Is internal.
		/// </summary>
		public bool IsInternal {
			get {
				return Access == ACCESS_INTERNAL_TEXT;
			}
		}
		/// <summary>
		/// Is public.
		/// </summary>
		public bool IsPublic {
			get {
				return Access == ACCESS_PUBLIC_TEXT;
			}
		}
		/// <summary>
		/// Is abstract.
		/// </summary>
		public bool IsAbstract {
			get {
				return Scope == SCOPE_ABSTRACT_TEXT;
			}
		}
		/// <summary>
		/// Is final.
		/// </summary>
		public bool IsFinal {
			get {
				return Scope == SCOPE_FINAL_TEXT;
			}
		}
		/// <summary>
		/// Is override.
		/// </summary>
		public bool IsOverride {
			get {
				return Scope == SCOPE_OVERRIDE_TEXT;
			}
		}
		/// <summary>
		/// Is static.
		/// </summary>
		public bool IsStatic {
			get {
				return Scope == SCOPE_STATIC_TEXT;
			}
		}
		/// <summary>
		/// Is virtual.
		/// </summary>
		public bool IsVirtual {
			get {
				return Scope == SCOPE_VIRTUAL_TEXT;
			}
		}
		/// <summary>
		/// Scope.
		/// </summary>
		public string Scope {
			get {
				switch (attributes & MemberAttributes.ScopeMask) {
					case MemberAttributes.Abstract:
						return SCOPE_ABSTRACT_TEXT;
					case MemberAttributes.Final:
						return SCOPE_FINAL_TEXT;
					case MemberAttributes.Static:
						return SCOPE_STATIC_TEXT;
					case MemberAttributes.Override:
						return SCOPE_OVERRIDE_TEXT;
					default:
						switch (attributes & MemberAttributes.AccessMask) {
							case MemberAttributes.Family:
							case MemberAttributes.Public:
								return SCOPE_VIRTUAL_TEXT;
							default:
								return String.Empty;
						}
				}
			}
		}
	}
}
