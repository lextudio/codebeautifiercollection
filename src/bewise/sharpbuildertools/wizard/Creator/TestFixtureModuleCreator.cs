using SMS.Windows.Forms;
using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Borland.Studio.ToolsAPI;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Gui.NUnit;

namespace BeWise.SharpBuilderTools.Wizard.Creator {

    public class TestFixtureModuleCreator : IOTAModuleCreator {

        /**************************************************************/
        /*                     Constructor
        /**************************************************************/
		[CLSCompliant(false)]
        public TestFixtureModuleCreator(string iCode, IOTAModule iOwner) {
            fCodeFile = new TestFixtureCodeFile(iCode);
            fOwner = iOwner;
        }

        /**************************************************************/
        /*                      Private
        /**************************************************************/

        private TestFixtureCodeFile fCodeFile;
        private IOTAModule fOwner;

        private string NewBuildFilename() {
            return Path.Combine(SecondPage.GetTestSourceFolder(),
        	                    "NewTestFilename.cs");
        }

        private string BuildFilename() {
            return Path.Combine(SecondPage.GetTestSourceFolder(),
        	                    SecondPage.GetTestClassName() + ".cs");
        }

        /**************************************************************/
        /*                     Public
        /**************************************************************/
		[CLSCompliant(false)]
        public IOTAFile NewFormFile(string formIdent, string ancestorIdent) {
            return null;
        }
		[CLSCompliant(false)]
        public IOTAFile NewImplSource(string moduleIdent, string formIdent, string ancestorIdent) {
            return fCodeFile;
        }
		[CLSCompliant(false)]
		public IOTAFile NewIntfSource(string moduleIdent, string formIdent, string ancestorIdent) {
            return null;
        }

        /**************************************************************/
        /*                     Properties
        /**************************************************************/

        public string CreatorType {
            get {
                return OTACreatorTypes.sClass;
            }
        }

        public bool Existing {
            get {
                return false;
            }
        }

        public string FileSystem {
            get {
                return "";
            }
        }
		[CLSCompliant(false)]
        public IOTAModule Owner {
            get {
                return fOwner;
            }
        }

        public bool Unnamed {
            get {
                return true;
            }
        }

        public string AncestorName {
            get {
                return "";
            }
        }

        public string FormName {
            get {
                return "";
            }
        }

        public string ImplFileName
        {
            get {
                IOTAProject lProject = fOwner as IOTAProject;

				if (lProject.FileInProject(BuildFilename()) | File.Exists(BuildFilename())) {
                    if (FourthPage.ShowFileExistsMsgBox)
					{    
                    	Lextm.Windows.Forms.MessageBoxFactory.Warn("The TestFixture \"" + BuildFilename() + "\" already exists inside the project!" + Environment.NewLine + Environment.NewLine + "Please choose a different filename for this generated TestFixture.");
                    }
					FourthPage.ShowFileExistsMsgBox = false;

                    return NewBuildFilename();
				} else {
					return BuildFilename(); }
            }
        }

		public string IntfFileName {
            get {
                return "";
            }
        }

        public bool MainForm {
            get {
                return false;
            }
        }

        public bool ShowForm {
            get {
                return false;
            }
        }

        public bool ShowSource {
            get {
                return true;
            }
        }

        /**************************************************************/
        /*                     Helper Class
        /**************************************************************/

	private sealed class TestFixtureCodeFile : IOTAFile {
            private string fCode;
            internal TestFixtureCodeFile(string iCode) {
                fCode = iCode;
            }
            public DateTime Age { get {
                                      return new DateTime();
                                  } }
            public string Source { get {
                                       return fCode;
                                   } }
        }
    }
}
