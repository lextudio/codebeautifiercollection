using System;
using System.Diagnostics;
using System.Windows.Forms;
using Borland.Studio.ToolsAPI;
using BeWise.Common;
using BeWise.Common.Utils;

namespace BeWise.SharpBuilderTools.Wizard.Creator {

    public class XmlFileCreator : BaseModuleCreator {

        /**************************************************************/
        /*                     Constructor
        /**************************************************************/

        public XmlFileCreator(string aFileName) {
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
            string _Source = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";

            return new CustomFile(_Source);
        }
    }
}
