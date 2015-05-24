using System;
using System.CodeDom;
using System.Collections;
using Borland.Studio.ToolsAPI;
using BeWise.Common;

namespace BeWise.Common.Info {

    public class FieldInfo : CodeDomInfo {
        /**************************************************************/
        /*                        Protected
        /**************************************************************/

        protected override string GetCodeDomType() {
            if (IsProperty) {
                return "Property";
            } else {
                return "Field";
            }
        }

        protected override string GetFullName() {
            return ClassName + "." + Name;
        }

        protected override int GetImageIndex() {
            if (IsProperty) {
                return CommonConsts.DEFAULT_PROPERTY_IMAGEINDEX;
            } else {
                if (IsInternal && IsStatic) {
                    return CommonConsts.INTERNAL_STATIC_FIELD_IMAGEINDEX;
                } else if (IsInternal) {
                    return CommonConsts.INTERNAL_FIELD_IMAGEINDEX;
                } else if (IsPrivate && IsStatic) {
                    return CommonConsts.PRIVATE_STATIC_FIELD_IMAGEINDEX;
                } else if (IsPrivate) {
                    return CommonConsts.PRIVATE_FIELD_IMAGEINDEX;
                } else if (IsProtected && IsStatic) {
                    return CommonConsts.PROTECTED_STATIC_FIELD_IMAGEINDEX;
                } else if (IsProtected) {
                    return CommonConsts.PROTECTED_FIELD_IMAGEINDEX;
                } else if (IsPublic && IsStatic) {
                    return CommonConsts.PUBLIC_STATIC_FIELD_IMAGEINDEX;
                } else if (IsPublic) {
                    return CommonConsts.PUBLIC_FIELD_IMAGEINDEX;
                } else {
                    return CommonConsts.DEFAULT_FIELD_IMAGEINDEX;
                }
            }
        }

        protected override int GetLineNumber() {
            if (CodeTypeMember != null) {
                return CodeTypeMember.LinePragma.LineNumber;
            } else {
                return 0;
            }
        }

        protected override string GetName() {
            if (CodeTypeMember != null) {
                return CodeTypeMember.Name;
            } else {
                return "";
            }
        }

        /**************************************************************/
        /*                      Properties
        /**************************************************************/

        public bool IsProperty {
            get {
                return (CodeTypeMember is CodeMemberProperty);
            }
        }

        public CodeTypeMember CodeTypeMember {
            get {
                return (CodeTypeMember) Tag;
            }
        }

        public string ReturnType {
            get {
                if (IsProperty) {
                    return (CodeTypeMember as CodeMemberProperty).Type.BaseType;
                } else {
                    return (CodeTypeMember as CodeMemberField).Type.BaseType;
                }
            }
        }

    }
}
