using System;
using Borland.Studio.ToolsAPI;

namespace BeWise.SharpBuilderTools.Wizard.Creator {

    public class SqlFileCreator : BaseModuleCreator {

        /**************************************************************/
        /*                     Constructor
        /**************************************************************/

        public SqlFileCreator(string aFileName) {
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
            string _Source = "/*" + Environment.NewLine;
            _Source += "/* New SQL script */" + Environment.NewLine;
            _Source += "/*" + Environment.NewLine;

            return new CustomFile(_Source);
        }
    }
}
