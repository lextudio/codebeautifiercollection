using System;
using System.Diagnostics;
using System.Windows.Forms;
using Borland.Studio.ToolsAPI;
using BeWise.Common;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Gui;
using BeWise.SharpBuilderTools.Wizard.Creator;

namespace BeWise.SharpBuilderTools.Wizard {

    public class ExpertProjectWizard : BaseWizard, IOTAWizard, IOTARepositoryWizard, IOTAProjectWizard {

        // *************************************************************************
        //                              Constructor
        // *************************************************************************
        []
		public ExpertProjectWizard(IOTAGalleryCategory aCat) : base(aCat) {
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
                ExpertProjectCreator _ProjectCreator = new ExpertProjectCreator(Utils.AddExtension(_FrmNewFileName.FileName, CommonConsts.PROJECT_EXTENSION));
                IOTAProject _Project = (IOTAProject) _ModuleServices.CreateModule(_ProjectCreator);

                //_ProjectCreator.NewDefaultProjectModule(_Project);

                FrmNewClassName _FrmNewClassName = new FrmNewClassName();

                if (_FrmNewClassName.ShowDialog() == DialogResult.OK) {
                    ExpertFileCreator _ExpertFileCreator = new ExpertFileCreator(Utils.AddExtension(_FrmNewClassName.FileName, CommonConsts.CS_EXTENSION), _FrmNewClassName.Namespace, _FrmNewClassName.ClassName);
                    IOTAModule _Module = _ModuleServices.CreateModule(_ExpertFileCreator);

                    _Project.AddFile(_Module.FileName);
                }

            }
        }


        /**************************************************************/
        /*                       Public
        /**************************************************************/

        public string Comment {
            get {
                return "Expert Project Wizard";
            }
        }

        public System.IntPtr Glyph {
            get {
                return System.IntPtr.Zero;
            }
        }

        public string IDString {
            get {
                return "SharpBuilderTools.ExpertProjectWizard";
            }
        }

        public string Name {
            get {
                return "Expert Project Wizard";
            }
        }

        public string Personality {
            get {
                return OTAIDEPersonalities.sDefaultPersonality;
            }
        }
    }
}
