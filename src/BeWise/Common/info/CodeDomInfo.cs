using System;
using System.CodeDom;
using System.Collections;
using Borland.Studio.ToolsAPI;
using BeWise.Common.Utils;
using BeWise.Common;

namespace BeWise.Common.Info {

    public abstract class CodeDomInfo {

        /**************************************************************/
        /*                        Private
        /**************************************************************/

        private string fClassName;
        private CodeTypeMember fTag;

        /**************************************************************/
        /*                        Protected
        /**************************************************************/

        protected abstract string GetCodeDomType();
        protected abstract string GetFullName();

        protected virtual int GetImageIndex() {
            return -1;
        }

        protected abstract int GetLineNumber();
        protected abstract string GetName();

        /**************************************************************/
        /*                        Public
        /**************************************************************/

        public void Goto () {
            // Show the Source Editor
            OTAUtils.GetCurrentModule().ShowFileName((OTAUtils.GetSourceEditor(OTAUtils.GetCurrentModule()) as IOTAEditor).FileName);

            int _LineNumber = GetLineNumber();

            if (_LineNumber > 0) {
                IOTASourceEditor _SourceEditor = OTAUtils.GetSourceEditor(OTAUtils.GetCurrentModule());

                IOTAEditView _EditView = _SourceEditor.GetEditView(0);

                _EditView.Buffer.EditPosition.GotoLine(_LineNumber);
                _EditView.MoveViewToCursor();
                _EditView.Paint();
            }
        }

        /**************************************************************/
        /*                         Properties
        /**************************************************************/

        public string Access {
            get {
                switch (Tag.Attributes & MemberAttributes.AccessMask) {
                case MemberAttributes.Family:
                    return CommonConsts.ACCESS_PROTECTED_TEXT;
                case MemberAttributes.Public:
                    return CommonConsts.ACCESS_PUBLIC_TEXT;
                case MemberAttributes.Assembly:
                    return CommonConsts.ACCESS_INTERNAL_TEXT;
                case MemberAttributes.FamilyOrAssembly:
                    return CommonConsts.ACCESS_PROTECTED_INTERNAL_TEXT;
                default:
                    return CommonConsts.ACCESS_PRIVATE_TEXT;
                }
            }
        }

        public string ClassName {
            get {
                return fClassName;
            }
            set {
                fClassName = value;
            }
        }

        public string CodeDomType {
            get {
                return GetCodeDomType();
            }
        }

        public string FullName {
            get {
                return GetFullName();
            }
        }

        public int ImageIndex {
            get {
                return GetImageIndex();
            }
        }

        public int LineNumber {
            get {
                return GetLineNumber();
            }
        }

        public string Name {
            get {
                return GetName();
            }
        }

        public bool IsPrivate {
            get {
                return Access == CommonConsts.ACCESS_PRIVATE_TEXT;
            }
        }

        public bool IsProtected {
            get {
                return Access == CommonConsts.ACCESS_PROTECTED_TEXT;
            }
        }

        public bool IsProtectedInternal {
            get {
                return Access == CommonConsts.ACCESS_PROTECTED_INTERNAL_TEXT;
            }
        }

        public bool IsInternal {
            get {
                return Access == CommonConsts.ACCESS_INTERNAL_TEXT;
            }
        }

        public bool IsPublic {
            get {
                return Access == CommonConsts.ACCESS_PUBLIC_TEXT;
            }
        }

        public bool IsAbstract {
            get {
                return Scope == CommonConsts.SCOPE_ABSTRACT_TEXT;
            }
        }

        public bool IsFinal {
            get {
                return Scope == CommonConsts.SCOPE_FINAL_TEXT;
            }
        }

        public bool IsOverride {
            get {
                return Scope == CommonConsts.SCOPE_OVERRIDE_TEXT;
            }
        }

        public bool IsStatic {
            get {
                return Scope == CommonConsts.SCOPE_STATIC_TEXT;
            }
        }

        public bool IsVirtual {
            get {
                return Scope == CommonConsts.SCOPE_VIRTUAL_TEXT;
            }
        }

        public string Scope {
            get {
                switch (Tag.Attributes & MemberAttributes.ScopeMask) {
                case MemberAttributes.Abstract:
                    return CommonConsts.SCOPE_ABSTRACT_TEXT;
                case MemberAttributes.Final:
                    return CommonConsts.SCOPE_FINAL_TEXT;
                case MemberAttributes.Static:
                    return CommonConsts.SCOPE_STATIC_TEXT;
                case MemberAttributes.Override:
                    return CommonConsts.SCOPE_OVERRIDE_TEXT;
                default:
                    switch (Tag.Attributes & MemberAttributes.AccessMask) {
                    case MemberAttributes.Family:
                    case MemberAttributes.Public:
                        return CommonConsts.SCOPE_VIRTUAL_TEXT;
                    default:
                        return "";
                    }
                }
            }
        }

        public CodeTypeMember Tag {
            get {
                return fTag;
            }
            set {
                fTag = value;
            }
        }

    }
}
