using System;
using Borland.Studio.ToolsAPI;

namespace BeWise.SharpBuilderTools.Wizard.Creator {

    public class StyleSheetFileCreator : BaseModuleCreator {

        /**************************************************************/
        /*                     Constructor
        /**************************************************************/

        public StyleSheetFileCreator(string aFileName) {
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
            string _Source = "BODY {" + Environment.NewLine + Environment.NewLine;
            _Source += "}";

            return new CustomFile(_Source);
        }
    }
}
