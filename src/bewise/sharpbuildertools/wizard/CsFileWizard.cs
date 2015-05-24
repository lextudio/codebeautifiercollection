using System;
using System.Diagnostics;
using System.Windows.Forms;
using Borland.Studio.ToolsAPI;
using BeWise.Common;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Gui;
using BeWise.SharpBuilderTools.Wizard.Creator;

namespace BeWise.SharpBuilderTools.Wizard {
	/// <summary>CSharp file wizard.</summary>
    public class CsFileWizard : BaseWizard, IOTAWizard, IOTARepositoryWizard, IOTAFormWizard {

        // *************************************************************************
        //                              Constructor
        // *************************************************************************
        /// 
		public CsFileWizard(IOTAGalleryCategory aCat) : base(aCat) {
        }

        /**************************************************************/
        /*                       Public
        /**************************************************************/

        public void Execute() {
            FrmNewClassName _FrmNewClassName = new FrmNewClassName();

            if (_FrmNewClassName.ShowDialog() == DialogResult.OK) {
                IOTAModuleServices _ModuleServices = OTAUtils.GetModuleServices();
                CsFileCreator _CsFileCreator = new CsFileCreator(Utils.AddExtension(_FrmNewClassName.FileName, CommonConsts.CS_EXTENSION), _FrmNewClassName.Namespace, _FrmNewClassName.ClassName);
                _ModuleServices.CreateModule(_CsFileCreator);

            }
        }

        /**************************************************************/
        /*                       Properties
        /**************************************************************/
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

        public string IDString {
            get {
                return "SharpBuilderTools.CSFileWizard";
            }
        }

        public string Name {
            get {
                return "Cs File Wizard";
            }
        }

        public string Personality {
            get {
                return OTAIDEPersonalities.sCSharpPersonality;
            }
        }
    }
}
