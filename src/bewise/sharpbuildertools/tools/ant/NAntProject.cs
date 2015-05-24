using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace BeWise.SharpBuilderTools.Tools.Ant {

	class NAntProject : BaseAntProject {
        // *************************************************************************
        //                           Constructor
        // *************************************************************************

        internal NAntProject(string aFileName) : base (aFileName) {}

        // *************************************************************************
        //                           Protected
        // *************************************************************************

        protected override BaseAntProperty CreateAntProperty(string aName, string aValue) {
            return new NAntProperty(aName, aValue);
        }

        protected override BaseAntTarget CreateAntTarget(string aName, string aDescription) {
            return new NAntTarget(aName, aDescription);
        }
    }
}
