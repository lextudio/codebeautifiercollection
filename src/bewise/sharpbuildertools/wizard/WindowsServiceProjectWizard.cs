using System;
using System.Diagnostics;
using System.Windows.Forms;
using Borland.Studio.ToolsAPI;
using BeWise.Common;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Gui;
using BeWise.SharpBuilderTools.Wizard.Creator;

namespace BeWise.SharpBuilderTools.Wizard {

    public class WindowsServiceProjectWizard : BaseWizard, IOTAWizard, IOTARepositoryWizard, IOTAProjectWizard {

        // *************************************************************************
        //                              Constructor
        // *************************************************************************
        []
		public WindowsServiceProjectWizard(IOTAGalleryCategory aCat) : base(aCat) {
        }

        /**************************************************************/
        /*                       Public
        /**************************************************************/

        // IOTAWizard
        public string IDString {
            get {
                return "SharpBuilderTools.WindowsServiceProjectWizard";
            }
        }

        public void Execute() {
            FrmNewFileName _FrmNewFileName = new FrmNewFileName();

            _FrmNewFileName.lblTitle.Text = "Empty Windows Service Wizard";
            _FrmNewFileName.lblSubTitle.Text = "Enter a project filename";

            if (_FrmNewFileName.ShowDialog() == DialogResult.OK) {
                IOTAModuleServices _ModuleServices = OTAUtils.GetModuleServices();
                WindowsServiceProjectCreator _ProjectCreator = new WindowsServiceProjectCreator(Utils.AddExtension(_FrmNewFileName.FileName, CommonConsts.PROJECT_EXTENSION));
                IOTAProject _Project = (IOTAProject) _ModuleServices.CreateModule(_ProjectCreator);

                FrmNewClassName _FrmNewClassName = new FrmNewClassName();

                if (_FrmNewClassName.ShowDialog() == DialogResult.OK) {
                    WindowsServiceFileCreator _WindowsServiceFileCreator = new WindowsServiceFileCreator(Utils.AddExtension(_FrmNewClassName.FileName, CommonConsts.CS_EXTENSION), _FrmNewClassName.Namespace, _FrmNewClassName.ClassName);
                    IOTAModule _Module = _ModuleServices.CreateModule(_WindowsServiceFileCreator);

                    _Project.AddFile(_Module.FileName);
                }

            }
        }

        public string Name {
            get {
                return "Windows Service Project Wizard";
            }
        }

        public string Comment {
            get {
                return "Windows Service Project Wizard";
            }
        }

        public System.IntPtr Glyph {
            get {
                return System.IntPtr.Zero;
            }
        }

        public string Personality {
            get {
                return OTAIDEPersonalities.sCSharpPersonality;
            }
        }
    }
}
