using System;
using System.Diagnostics;
using System.Windows.Forms;
using Borland.Studio.ToolsAPI;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Gui;
using BeWise.SharpBuilderTools.Wizard.Creator;

namespace BeWise.SharpBuilderTools.Wizard {

    public class StyleSheetFileWizard : BaseWizard, IOTAWizard, IOTARepositoryWizard, IOTAFormWizard {

        // *************************************************************************
        //                              Constructor
        // *************************************************************************
        []
		public StyleSheetFileWizard(IOTAGalleryCategory aCat) : base(aCat) {
        }

        /**************************************************************/
        /*                       Public
        /**************************************************************/

        // IOTAWizard
        public string IDString {
            get {
                return "SharpBuilderTools.StyleSheetFileWizard";
            }
        }

        public void Execute() {
            FrmNewFileName _FrmNewFileName = new FrmNewFileName();

            if (_FrmNewFileName.ShowDialog() == DialogResult.OK) {
                IOTAModuleServices _ModuleServices = OTAUtils.GetModuleServices();
                StyleSheetFileCreator _StyleSheetFileCreator = new StyleSheetFileCreator(Utils.AddExtension(_FrmNewFileName.FileName, Consts.STYLE_SHEET_FILE_EXTENSION));
                _ModuleServices.CreateModule(_StyleSheetFileCreator);
            }
        }

        public string Name {
            get {
                return "StyleSheet File Wizard";
            }
        }

        public string Comment {
            get {
                return "StyleSheet File Wizard";
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
