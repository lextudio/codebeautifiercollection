using System;
using Borland.Studio.ToolsAPI;

namespace BeWise.SharpBuilderTools.Wizard.Creator {

    public class ExpertProjectCreator : BaseProjectCreator {

        // *************************************************************************
        //                              Constructor
        // *************************************************************************
        public ExpertProjectCreator(string aFileName) : base(aFileName) {
        }

        // *************************************************************************
        //                              Protected
        // *************************************************************************


        protected override string GetCreatorType() {
            return OTACreatorTypes.sCSLibrary;
        }
    }
}
