using System;
using System.Diagnostics;
using System.Windows.Forms;
using Borland.Studio.ToolsAPI;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Gui.NUnit;
using BeWise.SharpBuilderTools.Wizard.Creator;

namespace BeWise.SharpBuilderTools.Wizard {

    public class TestFixtureFileWizard : BaseWizard, IOTAWizard, IOTARepositoryWizard, IOTAFormWizard {

        // *************************************************************************
        //                              Constructor
        // *************************************************************************
		[CLSCompliant(false)]
        public TestFixtureFileWizard(IOTAGalleryCategory aCat) : base(aCat) {
        }

        /**************************************************************/
        /*                       Public
        /**************************************************************/

        // IOTAWizard
        public string IDString {
            get {
                return "SharpBuilderTools.TestFixtureFileWizard";
            }
        }

        public void Execute() {
            FrmNUnitWizard _Frm = new FrmNUnitWizard();
            _Frm.ShowDialog();
        }

        public string Name {
            get {
                return "Test Fixture File Wizard";
            }
        }

        public string Comment {
            get {
                return "Test Fixture File Wizard";
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
