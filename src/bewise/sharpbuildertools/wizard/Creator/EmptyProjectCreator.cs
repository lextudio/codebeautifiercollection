using System;
using Borland.Studio.ToolsAPI;

namespace BeWise.SharpBuilderTools.Wizard.Creator {

    public class EmptyProjectCreator : IOTAProjectCreator {

        // *************************************************************************
        //                              Constructor
        // *************************************************************************
        public EmptyProjectCreator(string aFileName) {
            fFileName= aFileName;
        }

        // *************************************************************************
        //                              Private
        // *************************************************************************
        private string fFileName = "";

        // *************************************************************************
        //                              Public
        // *************************************************************************
		[]
        public void NewDefaultProjectModule(IOTAProject project) {}
		[]
        public void  NewProjectResource(IOTAProject project) {
        }
		[]
        public IOTAFile NewProjectSource(string ProjectName) {
            return null;
        }

        // *************************************************************************
        //                              Properties
        // *************************************************************************
        // Project Creator
        public string FileName {
            get {
                return fFileName;
            }
        }

        public string ProjectPersonality {
            get {
                return OTAIDEPersonalities.sCSharpPersonality;
            }
        }

        public bool ShowSource {
            get {
                return false;
            }
        }

        public string CreatorType {
            get {
                return OTACreatorTypes.sCSApplication;
            }
        }

        public bool Existing {
            get {
                return false;
            }
        }

        public string FileSystem {
            get {
                return "";
            }
        }
		[]
        public IOTAModule Owner {
            get {
                return null;
            }
        }

        public bool Unnamed {
            get {
                return true;
            }
        }
    }
}
