using System;
using System.Diagnostics;
using System.Windows.Forms;
using Borland.Studio.ToolsAPI;
using BeWise.Common.Utils;

namespace BeWise.SharpBuilderTools.Wizard.Creator {

    public class WindowsServiceProjectCreator : BaseProjectCreator {

        // *************************************************************************
        //                              Constructor
        // *************************************************************************
        public WindowsServiceProjectCreator(string aFileName) : base(aFileName) {
        }

        // *************************************************************************
        //                              Protected
        // *************************************************************************

        protected override string GetCreatorType() {
            return OTACreatorTypes.sCSLibrary;
        }
    }
}
