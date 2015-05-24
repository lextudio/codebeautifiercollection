using System;
using BeWise.Common.Utils;
using Borland.Studio.ToolsAPI;

namespace BeWise.SharpBuilderTools.Wizard.Creator {

    public class CsFileCreator : BaseModuleCreator {

        /**************************************************************/
        /*                     Constructor
        /**************************************************************/

        public CsFileCreator(string aFileName, string aNamespace, string aClassName) {
            fFileName = aFileName;
            fNamespace = aNamespace;
            fClassName = aClassName;
        }

        /**************************************************************/
        /*                     Private
        /**************************************************************/
        private string fClassName;
        private string fFileName;
        private string fNamespace;

        /**************************************************************/
        /*                     Protected
        /**************************************************************/

        protected override string GetImplFileName() {
            return fFileName;
        }

        /**************************************************************/
        /*                   Protected Properties
        /**************************************************************/

        protected string ClassName {
            get {
                return fClassName;
            }
        }

        protected string FileName {
            get {
                return fFileName;
            }
        }

        protected string Namespace {
            get {
                return fNamespace;
            }
        }

        /**************************************************************/
        /*                     Public
        /**************************************************************/
		[]
        public override IOTAFile NewImplSource(string moduleIdent, string formIdent, string ancestorIdent) {
            string _Source = "";

            _Source += "using System;" + Environment.NewLine + Environment.NewLine;

            if (!String.IsNullOrEmpty(fNamespace)) {
                _Source += "namespace " + fNamespace + " {" + Environment.NewLine;
            }

            _Source += "\tpublic class " + fClassName + " {" + Environment.NewLine + Environment.NewLine;
            _Source += "\t}" + Environment.NewLine;

            if (!String.IsNullOrEmpty(fNamespace)) {
                _Source += "}";
            }

            return new CustomFile(_Source);
        }
    }
}
