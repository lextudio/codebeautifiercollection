using System;
using System.Diagnostics;
using System.Collections.Specialized;
using System.IO;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Helpers;

namespace BeWise.SharpBuilderTools.Tools.Expresso {
	class Expresso : BaseTool {

        internal Expresso() {}
        /**************************************************************/
        /*                        Protected
        /**************************************************************/
        // Expresso
        private const string EXPRESSO_FILE_EXTENSION                             = ".xso";
        internal const string EXPRESSO_GUI_EXE_NAME                               = "expresso.exe";
        private const string EXPRESSO_NAME                                       = "Expresso";

        protected override string GetToolPath () {
			return (string)Lextm.OpenTools.PropertyRegistry.Get("ExpressoPath");
        }

        protected override string GetGuiArgument () {
            return EXPRESSO_GUI_EXE_NAME;
        }

        protected override bool GetInstalled () {
        	return (File.Exists(GetGui()));
        }

        public override string GetName () {
            return EXPRESSO_NAME;
        }

        /**************************************************************/
        /*                        Public
        /**************************************************************/

        public override bool IsToolFile(string aFileName) {
            return (Path.GetExtension(aFileName).ToUpperInvariant() == EXPRESSO_FILE_EXTENSION.ToUpperInvariant());
        }

        public override void RunGui(object[] aParams, string aWorkingDir) {
            if (!ValidationHelpers.ValidateToolInstalled(this)) {
                return;
            }

            RunProcess _RunProcess = new RunProcess();
            string[] _Params = new String[2];

            _Params[0] = GetGui();

            if (aParams.Length > 0) {
                _Params[1] = (string) aParams[0];
            }

            _RunProcess.Run(_Params, aWorkingDir);
        }
    }
}
