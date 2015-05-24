using System;
using System.Diagnostics;
using System.Windows.Forms;
using Borland.Studio.ToolsAPI;
using BeWise.Common;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Gui;
using BeWise.SharpBuilderTools.Wizard.Creator;

namespace BeWise.SharpBuilderTools.Wizard {

    public class EmptyProjectWizard : BaseWizard, IOTAWizard, IOTARepositoryWizard, IOTAProjectWizard {

        // *************************************************************************
        //                              Constructor
        // *************************************************************************
        []
		public EmptyProjectWizard(IOTAGalleryCategory aCat) : base(aCat) {
        }

        /**************************************************************/
        /*                       Public
        /**************************************************************/

        public void Execute() {
            FrmNewFileName _FrmNewFileName = new FrmNewFileName();

            _FrmNewFileName.lblTitle.Text = "Empty Project Wizard";
            _FrmNewFileName.lblSubTitle.Text = "Enter a project filename";

            if (_FrmNewFileName.ShowDialog() == DialogResult.OK) {
                IOTAModuleServices _ModuleServices = OTAUtils.GetModuleServices();
                EmptyProjectCreator _ProjectCreator = new EmptyProjectCreator(Utils.AddExtension(_FrmNewFileName.FileName, CommonConsts.PROJECT_EXTENSION));
                _ModuleServices.CreateModule(_ProjectCreator);
            }
        }

        /**************************************************************/
        /*                       Properties
        /**************************************************************/
        public string Comment {
            get {
                return "Empty Project Wizard";
            }
        }

        public System.IntPtr Glyph {
            get {
                return System.IntPtr.Zero;
            }
        }

        public string IDString {
            get {
                return "SharpBuilderTools.EmptyProjectWizard";
            }
        }

        public string Name {
            get {
                return "Empty Project Wizard";
            }
        }

        public string Personality {
            get {
                return OTAIDEPersonalities.sDefaultPersonality;
            }
        }
    }
}
