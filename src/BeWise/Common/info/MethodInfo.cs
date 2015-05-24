using System;
using System.CodeDom;
using System.Collections;
using System.Windows.Forms;
using Borland.Studio.ToolsAPI;
using BeWise.Common;
using BeWise.Common.Utils;

namespace BeWise.Common.Info {

    public class MethodInfo : CodeDomInfo {

        /**************************************************************/
        /*                    Protected
        /**************************************************************/
        private CodeStatement fCodeStatement;
        private bool fIsGet;
        private bool fIsSet;

        /**************************************************************/
        /*                    Protected
        /**************************************************************/

        protected override string GetCodeDomType() {
            return "Method";
        }

        protected override string GetFullName() {
            return ClassName + "." + Name;
        }

        protected override int GetImageIndex() {
            if (IsInternal && IsStatic) {
                return CommonConsts.INTERNAL_STATIC_METHOD_IMAGEINDEX;
            } else if (IsInternal && IsVirtual) {
                return CommonConsts.INTERNAL_VIRTUAL_METHOD_IMAGEINDEX;
            } else if (IsInternal) {
                return CommonConsts.INTERNAL_METHOD_IMAGEINDEX;
            } else if (IsPrivate && IsStatic) {
                return CommonConsts.PRIVATE_STATIC_METHOD_IMAGEINDEX;
            } else if (IsPrivate && IsVirtual) {
                return CommonConsts.PRIVATE_VIRTUAL_METHOD_IMAGEINDEX;
            } else if (IsPrivate) {
                return CommonConsts.PRIVATE_METHOD_IMAGEINDEX;
            } else if ((IsProtected || IsProtectedInternal)&& IsStatic) {
                return CommonConsts.PROTECTED_STATIC_METHOD_IMAGEINDEX;
            } else if ((IsProtected || IsProtectedInternal) && IsVirtual) {
                return CommonConsts.PROTECTED_VIRTUAL_METHOD_IMAGEINDEX;
            } else if (IsProtected || IsProtectedInternal) {
                return CommonConsts.PROTECTED_METHOD_IMAGEINDEX;
            } else if (IsPublic && IsStatic) {
                return CommonConsts.PUBLIC_STATIC_METHOD_IMAGEINDEX;
            } else if (IsPublic && IsVirtual) {
                return CommonConsts.PUBLIC_VIRTUAL_METHOD_IMAGEINDEX;
            } else if (IsPublic) {
                return CommonConsts.PUBLIC_METHOD_IMAGEINDEX;
            } else {
                return CommonConsts.DEFAULT_METHOD_IMAGEINDEX;
            }
        }

        protected override int GetLineNumber() {
            if (CodeMemberMethod != null) {
                if (CodeStatement != null) {
                    return CodeStatement.LinePragma.LineNumber;
                }

                /*
                ArrayList _List = new ArrayList(CodeMemberMethod.UserData.Keys);

                OTAUtils.ShowDebug("------------" + Name + "--------------");
                foreach(string _Item in _List) {
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

                // If the Line exists, uses it to locate the source
                // Delphi set the Line to the line number in the implementation
                if (CodeMemberMethod.UserData["Line"] != null) {
                    //OTAUtils.ShowDebug("LineNumber: " + CodeMemberMethod.UserData["Line"]);
                    return (int) CodeMemberMethod.UserData["Line"];
                /*
                } else if (CodeMemberMethod.LinePragma == null) {
                    if (CodeMemberMethod.UserData["TopPosition"] != null) {
                        return (int) CodeMemberMethod.UserData["TopPosition"];
                    } else {
                        return 0;
                    }
                */
                } else {
                    return CodeMemberMethod.LinePragma.LineNumber;
                }
            } else {
                return 0;
            }
        }

        protected override string GetName() {
            if (CodeMemberMethod != null) {
                if (IsGet) {
                    return "get_" + CodeMemberMethod.Name;
                }

                if (IsSet) {
                    return "set_" + CodeMemberMethod.Name;
                }
                
                return CodeMemberMethod.Name;
            } else {
                return "";
            }
        }

        /**************************************************************/
        /*                      Properties
        /**************************************************************/

        public bool IsConstructor {
            get {
                return CodeMemberMethod is CodeConstructor;
            }
        }

        public CodeTypeMember CodeMemberMethod {
            get {
                return (CodeTypeMember) Tag;
            }
        }

        public CodeStatement CodeStatement {
            get {
                return fCodeStatement;
            }

            set {
                fCodeStatement = value;
            }
        }

        public bool IsGet {
            get {
                return fIsGet;
            }

            set {
                fIsGet = value;
            }
        }

        public bool IsSet {
            get {
                return fIsSet;
            }

            set {
                fIsSet = value;
            }
        }

        public string ReturnType {
            get {
                if (CodeMemberMethod is CodeMemberMethod) {
                    return (CodeMemberMethod as CodeMemberMethod).ReturnType.BaseType;
                // getter / setter case
                } else {
                    return (CodeMemberMethod as CodeMemberProperty).Type.BaseType;
                }
            }
        }

        public string Abstract {
            get {
                return "";
            }
        }

    }
}
