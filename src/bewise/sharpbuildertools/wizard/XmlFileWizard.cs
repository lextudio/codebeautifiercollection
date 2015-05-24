using System;
using System.Diagnostics;
using System.Windows.Forms;
using Borland.Studio.ToolsAPI;
using BeWise.Common;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Gui;
using BeWise.SharpBuilderTools.Wizard.Creator;

namespace BeWise.SharpBuilderTools.Wizard {

    public class XmlFileWizard : BaseWizard, IOTAWizard, IOTARepositoryWizard, IOTAFormWizard {

        // *************************************************************************
        //                              Constructor
        // *************************************************************************
        []
		public XmlFileWizard(IOTAGalleryCategory aCat) : base(aCat) {
        }

        /**************************************************************/
        /*                       Public
        /**************************************************************/

        // IOTAWizard
        public string IDString {
            get {
                return "SharpBuilderTools.XmlFileWizard";
            }
        }

        public void Execute() {
            FrmNewFileName _FrmNewFileName = new FrmNewFileName();

            if (_FrmNewFileName.ShowDialog() == DialogResult.OK) {
                IOTAModuleServices _ModuleServices = OTAUtils.GetModuleServices();
                XmlFileCreator _XmlFileCreator = new XmlFileCreator(Utils.AddExtension(_FrmNewFileName.FileName, CommonConsts.XML_EXTENSION));
                _ModuleServices.CreateModule(_XmlFileCreator);
            }
        }

        public string Name {
            get {
                return "Xml File Wizard";
            }
        }

        public string Comment {
            get {
                return "Xml File Wizard";
            }
        }

        public System.IntPtr Glyph {
            get {
                return System.IntPtr.Zero;
            }
        }

        public string Personality {
            get {
                return OTAIDEPersonalities.sDefaultPersonality;
            }
        }
    }
}
