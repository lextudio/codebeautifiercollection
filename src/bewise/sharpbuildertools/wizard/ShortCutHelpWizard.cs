using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Borland.Studio.ToolsAPI;
using BeWise.Common.Utils;

namespace BeWise.SharpBuilderTools.Wizard {

    public class ShortCutHelpWizard : IOTAMenuWizard {

        /**************************************************************/
        /*                     Public
        /**************************************************************/

        public string IDString {
            get {
                return "SharpBuilderTools.ShortCutHelpWizard";
            }
        }

        public void Execute() {
            string _ShortCutHelpFileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\help\csharpbuilder_keys.pdf";

            Process.Start(_ShortCutHelpFileName);
        }

        public string Name {
            get {
                return "SharpBuilderTools ShorCut Help";
            }
        }

        public void Destroyed() {
            /* nothing */
        }

        public string MenuText {
            get {
                return "SharpBuilderTools shorcut Help";
            }
        }

        public bool Checked {
            get {
                return false;
            }
        }

        public bool Enabled {
            get {
                return true;
            }
        }
    }

}
