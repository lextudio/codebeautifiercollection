using System;
using BeWise.Common.Utils;
using Borland.Studio.ToolsAPI;

namespace BeWise.SharpBuilderTools.Wizard.Creator {

    public class ExpertFileCreator : CsFileCreator {

        /**************************************************************/
        /*                     Constructor
        /**************************************************************/

        public ExpertFileCreator(string aFileName, string aNamespace, string aClassName) : base(aFileName, aNamespace, aClassName) {
        }

        /**************************************************************/
        /*                     Public
        /**************************************************************/
		[]
        public override IOTAFile NewImplSource(string moduleIdent, string formIdent, string ancestorIdent) {
            string _Source = "";

            _Source += "using System;" + Environment.NewLine;
            _Source += "using Borland.Studio.ToolsAPI;" + Environment.NewLine + Environment.NewLine;

            if (!String.IsNullOrEmpty(Namespace)) {
                _Source += "namespace " + Namespace + " {" + Environment.NewLine;
            }

            _Source += "\tpublic class " + ClassName + " {" + Environment.NewLine + Environment.NewLine;
            _Source += "\t\t public void IDERegister() {" + Environment.NewLine;
            _Source += "\t\t" + Environment.NewLine;
            _Source += "\t\t }" + Environment.NewLine;
            _Source += "\t}" + Environment.NewLine;

            if (!String.IsNullOrEmpty(Namespace)) {
                _Source += "}";
            }

            return new CustomFile(_Source);
        }
    }
}
