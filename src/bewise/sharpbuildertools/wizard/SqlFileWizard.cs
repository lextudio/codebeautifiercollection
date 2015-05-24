using System;
using System.Diagnostics;
using System.Windows.Forms;
using Borland.Studio.ToolsAPI;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Gui;
using BeWise.SharpBuilderTools.Wizard.Creator;

namespace BeWise.SharpBuilderTools.Wizard {

    public class SqlFileWizard : BaseWizard, IOTAWizard, IOTARepositoryWizard, IOTAFormWizard {

        // *************************************************************************
        //                              Constructor
        // *************************************************************************
        []
		public SqlFileWizard(IOTAGalleryCategory aCat) : base(aCat) {
        }

        /**************************************************************/
        /*                       Public
        /**************************************************************/

        public void Execute() {
            FrmNewFileName _FrmNewFileName = new FrmNewFileName();

            if (_FrmNewFileName.ShowDialog() == DialogResult.OK) {
                IOTAModuleServices _ModuleServices = OTAUtils.GetModuleServices();
                SqlFileCreator _SqlFileCreator = new SqlFileCreator(Utils.AddExtension(_FrmNewFileName.FileName, Consts.SQL_FILE_EXTENSION));
                _ModuleServices.CreateModule(_SqlFileCreator);
            }
        }

        /**************************************************************/
        /*                       Properties
        /**************************************************************/

        public string IDString {
            get {
                return "SharpBuilderTools.SQLFileWizard";
            }
        }

        public string Name {
            get {
                return "SQL File Wizard";
            }
        }

        public string Comment {
            get {
                return "SQL File Wizard";
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
