using System;
using System.CodeDom;
using System.Collections;
using Borland.Studio.ToolsAPI;
using BeWise.Common;

namespace BeWise.Common.Info {

    public class ClassInfo : CodeDomInfo {

        /**************************************************************/
        /*                        Protected
        /**************************************************************/

        protected override string GetCodeDomType() {
            return "Class";
        }

        protected override string GetFullName() {
            return Name;
        }

        protected override int GetImageIndex() {
            if (IsPublic) {
                return CommonConsts.PUBLIC_CLASS_IMAGEINDEX;
            } else if (IsInternal) {
                return CommonConsts.INTERNAL_CLASS_IMAGEINDEX;
            } else if (IsPrivate) {
                return CommonConsts.PRIVATE_CLASS_IMAGEINDEX;
            } else {
                return CommonConsts.DEFAULT_CLASS_IMAGEINDEX;
            }
        }

        protected override int GetLineNumber() {
            if (CodeTypeDeclaration != null) {
                return CodeTypeDeclaration.LinePragma.LineNumber;
            } else {
                return 0;
            }
        }

        protected override string GetName() {
            if (CodeTypeDeclaration != null) {
                return CodeTypeDeclaration.Name;
            } else {
                return "";
            }
        }

        /**************************************************************/
        /*                    Properties
        /**************************************************************/

        public CodeTypeDeclaration CodeTypeDeclaration {
            get {
                return (CodeTypeDeclaration) Tag;
            }
        }

        public string Parent {
            get {
                if (CodeTypeDeclaration.BaseTypes.Count > 0) {
                    return CodeTypeDeclaration.BaseTypes[0].BaseType;
                } else {
                    return "object";
                }
            }

            set {

            }
        }

    }
}
