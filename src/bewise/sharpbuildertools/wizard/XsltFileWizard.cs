using System;
using System.Diagnostics;
using System.Windows.Forms;
using Borland.Studio.ToolsAPI;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Gui;
using BeWise.SharpBuilderTools.Wizard.Creator;

namespace BeWise.SharpBuilderTools.Wizard {

    public class XsltFileWizard : BaseWizard, IOTAWizard, IOTARepositoryWizard, IOTAFormWizard {

        // *************************************************************************
        //                              Constructor
        // *************************************************************************
        []
		public XsltFileWizard(IOTAGalleryCategory aCat) : base(aCat) {
        }

        /**************************************************************/
        /*                       Public
        /**************************************************************/

        // IOTAWizard
        public string IDString {
            get {
                return "SharpBuilderTools.XsltFileWizard";
            }
        }

        public void Execute() {
            FrmNewFileName _FrmNewFileName = new FrmNewFileName();

            if (_FrmNewFileName.ShowDialog() == DialogResult.OK) {
                IOTAModuleServices _ModuleServices = OTAUtils.GetModuleServices();
                XsltFileCreator _XsltFileCreator = new XsltFileCreator(Utils.AddExtension(_FrmNewFileName.FileName, Consts.XSLT_FILE_EXTENSION));
                _ModuleServices.CreateModule(_XsltFileCreator);
            }
        }

        public string Name {
            get {
                return "Xslt File Wizard";
            }
        }

        public string Comment {
            get {
                return "Cs File Wizard";
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
