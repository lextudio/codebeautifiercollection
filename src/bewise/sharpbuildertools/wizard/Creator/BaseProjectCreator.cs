using System;
using Borland.Studio.ToolsAPI;
using BeWise.Common.Utils;

namespace BeWise.SharpBuilderTools.Wizard.Creator {

    public class BaseProjectCreator : IOTAProjectCreator {

        // *************************************************************************
        //                              Constructor
        // *************************************************************************
        public BaseProjectCreator(string aFileName) {
            fFileName= aFileName;
        }

        // *************************************************************************
        //                              Private
        // *************************************************************************
        private string fFileName = "";


        // *************************************************************************
        //                              Protected
        // *************************************************************************

        protected virtual string GetCreatorType() {
            return OTACreatorTypes.sCSApplication;
        }

        // *************************************************************************
        //                              Public
        // *************************************************************************
		[]
        public virtual void NewDefaultProjectModule(IOTAProject project) {}
		[]
        public virtual void NewProjectResource(IOTAProject project) {
        }
		[]
        public virtual IOTAFile NewProjectSource(string ProjectName) {
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
                return GetCreatorType();
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
                return OTAUtils.GetCurrentProjectGroup();
            }
        }

        public bool Unnamed {
            get {
                return true;
            }
        }
    }
}
