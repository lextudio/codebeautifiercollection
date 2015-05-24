using System;
using System.CodeDom;
using System.Collections;
using Borland.Studio.ToolsAPI;
using BeWise.Common;

namespace BeWise.Common.Info {

    public class TypeInfo : CodeDomInfo {

        /**************************************************************/
        /*                        Protected
        /**************************************************************/

        protected override string GetCodeDomType() {
            if (CodeTypeDeclaration.IsClass) {
                return "Class";
            } else if (CodeTypeDeclaration.IsEnum) {
                return "Enum";
            } else if (CodeTypeDeclaration.IsInterface) {
                return "Interface";
            } else if (CodeTypeDeclaration.IsStruct) {
                return "Struc";
            } else {
                return "";
            }
        }

        protected override string GetFullName() {
            return Name;
        }

        protected override int GetImageIndex() {
            if (CodeTypeDeclaration.IsClass) {
                return CommonConsts.DEFAULT_CLASS_IMAGEINDEX;
            } else if (CodeTypeDeclaration.IsEnum) {
                return CommonConsts.DEFAULT_CLASS_IMAGEINDEX;
            } else if (CodeTypeDeclaration.IsInterface) {
                return CommonConsts.DEFAULT_INTERFACE_IMAGEINDEX;
            } else if (CodeTypeDeclaration.IsStruct) {
                return CommonConsts.DEFAULT_CLASS_IMAGEINDEX;
                ;
            } else {
                return -1;
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
    }
}
