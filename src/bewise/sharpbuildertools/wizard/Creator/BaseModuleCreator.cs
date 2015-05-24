using System;
using Borland.Studio.ToolsAPI;
using BeWise.Common.Utils;

namespace BeWise.SharpBuilderTools.Wizard.Creator {

    public class BaseModuleCreator : IOTAModuleCreator {

        /**************************************************************/
        /*                     Protected
        /**************************************************************/

        protected virtual string GetCreatorType() {
            return OTACreatorTypes.sClass;
        }

        protected virtual bool GetExisting() {
            return false;
        }

        protected virtual string GetFileSystem() {
            return "";
        }
		[]
        protected virtual IOTAModule GetOwner() {
            return OTAUtils.GetCurrentProject();
        }

        protected virtual bool GetUnnamed() {
            return true;
        }

        protected virtual string GetAncestorName() {
            return "";
        }

        protected virtual string GetFormName() {
            return "";
        }

        protected virtual string GetImplFileName() {
            return "";
        }

        protected virtual string GetIntfFileName() {
            return "";
        }

        protected virtual bool GetMainForm() {
            return false;
        }

        protected virtual bool GetShowForm() {
            return false;
        }

        protected virtual bool GetShowSource() {
            return true;
        }

        /**************************************************************/
        /*                     Public
        /**************************************************************/
		[]
        public virtual IOTAFile NewFormFile(string formIdent, string ancestorIdent) {
            return null;
        }
		[]
        public virtual IOTAFile NewImplSource(string moduleIdent, string formIdent, string ancestorIdent) {
            return null;
        }
		[]
        public virtual IOTAFile NewIntfSource(string moduleIdent, string formIdent, string ancestorIdent) {
            return null;
        }

        /**************************************************************/
        /*                     Properties
        /**************************************************************/

        public string CreatorType {
            get {
                return GetCreatorType();
            }
        }

        public bool Existing {
            get {
                return GetExisting();
            }
        }

        public string FileSystem {
            get {
                return GetFileSystem();
            }
        }
		[]
        public IOTAModule Owner {
            get {
                return GetOwner();
            }
        }

        public bool Unnamed {
            get {
                return GetUnnamed();
            }
        }

        public string AncestorName {
            get {
                return GetAncestorName();
            }
        }

        public string FormName {
            get {
                return GetFormName();
            }
        }

        public string ImplFileName {
            get {
                return GetImplFileName();
            }
        }
        public string IntfFileName {
            get {
                return GetIntfFileName();
            }
        }

        public bool MainForm {
            get {
                return GetMainForm();
            }
        }

        public bool ShowForm {
            get {
                return GetShowForm();
            }
        }

        public bool ShowSource {
            get {
                return GetShowSource();
            }
        }
    }
}
