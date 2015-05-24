using System;
using System.CodeDom;
using System.Collections;
using Borland.Studio.ToolsAPI;

namespace BeWise.Common.Info {

    public class EventInfo : CodeDomInfo {
        /**************************************************************/
        /*                        Protected
        /**************************************************************/

        protected override string GetCodeDomType() {
            return "Event";
        }

        protected override string GetFullName() {
            return ClassName + "." + Name;
        }

        protected override int GetImageIndex() {
            //return Consts.DEFAULT_EVENT_IMAGEINDEX;
            return 0;
        }

        protected override int GetLineNumber() {
            if (CodeMemberEvent != null) {
                return CodeMemberEvent.LinePragma.LineNumber;
            } else {
                return 0;
            }
        }

        protected override string GetName() {
            if (CodeMemberEvent != null) {
                return CodeMemberEvent.Name;
            } else {
                return "";
            }
        }

        /**************************************************************/
        /*                      Properties
        /**************************************************************/

        public CodeMemberEvent CodeMemberEvent {
            get {
                return (CodeMemberEvent) Tag;
            }
        }

        public string ReturnType {
            get {
                return CodeMemberEvent.Type.BaseType;
            }
        }

    }
}
