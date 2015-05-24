using System;
using Borland.Studio.ToolsAPI;

namespace BeWise.SharpBuilderTools.Wizard.Creator {

    public class CustomFile : IOTAFile {

        /**************************************************************/
        /*                     Constructor
        /**************************************************************/

        public CustomFile(string aSource) {
            fSource = aSource;
        }

        /**************************************************************/
        /*                     Private
        /**************************************************************/

        private string fSource;

        /**************************************************************/
        /*                     Public
        /**************************************************************/

        public DateTime Age {
            get {
                return new DateTime();
            }
        }

        public string Source {
            get {
                return fSource;
            }
        }
    }
}
