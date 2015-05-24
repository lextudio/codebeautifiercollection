using System;
using System.Diagnostics;
using System.Windows.Forms;
using Borland.Studio.ToolsAPI;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Gui;
using BeWise.SharpBuilderTools.Wizard.Creator;

namespace BeWise.SharpBuilderTools.Wizard {

    public class StrongNameFileWizard : BaseWizard, IOTAWizard, IOTARepositoryWizard, IOTAFormWizard {

        // *************************************************************************
        //                              Constructor
        // *************************************************************************
        []
		public StrongNameFileWizard(IOTAGalleryCategory aCat) : base(aCat) {
        }

        /**************************************************************/
        /*                       Public
        /**************************************************************/

        // IOTAWizard
        public string IDString {
            get {
                return "SharpBuilderTools.StrongNameFileWizard";
            }
        }

        public void Execute() {
            FrmNewFileName _Frm = new FrmNewFileName();

            if (_Frm.ShowDialog() == DialogResult.OK) {
                RunProcess _RunProcess = new RunProcess();
                string[] _Params = new String[3];

                _Params[0] = "sn.exe";
                _Params[1] = "-k";

                string _FileName = Utils.AddExtension(_Frm.FileName, Consts.STRONG_NAME_KEY_FILE_EXTENSION);
                _Params[2] = _FileName;

                string result = _RunProcess.Run(_Params, "");

                if (Utils.FileIsValid(_FileName)) {
                    IOTAProject _Project = OTAUtils.GetCurrentProject();

                    if (_Project != null) {
                        _Project.AddFile(_FileName);
                    }
                }
            }
        }

        public string Name {
            get {
                return "Strong Name File Wizard";
            }
        }

        public string Comment {
            get {
                return "Strong Name File Wizard";
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
