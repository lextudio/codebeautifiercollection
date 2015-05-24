using System;
using System.Diagnostics;
using System.Windows.Forms;
using Borland.Studio.ToolsAPI;
using BeWise.Common;
using BeWise.Common.Utils;

namespace BeWise.SharpBuilderTools.Wizard.Creator {

    public class XsltFileCreator : BaseModuleCreator {

        /**************************************************************/
        /*                     Constructor
        /**************************************************************/

        public XsltFileCreator(string aFileName) {
            fFileName = aFileName;
        }

        /**************************************************************/
        /*                     Private
        /**************************************************************/
        private string fFileName;

        /**************************************************************/
        /*                     Protected
        /**************************************************************/

        protected override string GetImplFileName() {
            return fFileName;
        }

        protected override string GetCreatorType() {
            return OTACreatorTypes.sText;
        }

        /**************************************************************/
        /*                     Public
        /**************************************************************/
		[]
        public override IOTAFile NewImplSource(string moduleIdent, string formIdent, string ancestorIdent) {
            string _Source = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" + Environment.NewLine;
            _Source += "<stylesheet version=\"1.0\" xmlns=\"http://www.w3.org/1999/XSL/Transform\">" + Environment.NewLine;
            _Source += "</stylesheet>";

            return new CustomFile(_Source);
        }
    }
}
