using System;
using System.Diagnostics;
using System.Collections.Specialized;
using System.IO;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Helpers;
using Lextm.OpenTools;

namespace BeWise.SharpBuilderTools.Tools.FxCop {
	/// <summary>
	/// FxCop tool.
	/// </summary>
	class FxCop : BaseTool {

    	internal FxCop () {}
        /**************************************************************/
		/*                        Protected
        /**************************************************************/
        // FxCop
        internal const string FXCOP_CONSOLE_EXE_NAME                              = "fxcopcmd.exe";
        private const string FXCOP_FILE_EXTENSION                                = ".fxcop";
        internal const string FXCOP_GUI_EXE_NAME                                  = "fxcop.exe";
        private const string FXCOP_NAME                                          = "FxCop";

        protected override string GetToolPath () {
			return (string)PropertyRegistry.Get("FxCopPath");
        }

        protected override string GetGuiArgument () {
            return FXCOP_GUI_EXE_NAME;
        }

        protected override string GetConsoleArgument () {
            return FXCOP_CONSOLE_EXE_NAME;
        }

        protected override bool GetInstalled () {
        	return (File.Exists(GetGui()) && File.Exists(GetConsole()));
        }

        public override string GetName () {
            return FXCOP_NAME;
        }

        /**************************************************************/
        /*                        Public
        /**************************************************************/

        public override bool IsToolFile(string aFileName) {
            return (Path.GetExtension(aFileName).ToUpperInvariant() == FXCOP_FILE_EXTENSION.ToUpperInvariant());
        }


        public override void RunConsole(object[] aParams, string aWorkingDir) {
        	base.RunConsole(aParams, aWorkingDir);
            if (!ValidationHelpers.ValidateToolInstalled(this)) {
                return;
            }

            RunProcess _RunProcess = new RunProcess();
            string[] _Params = new String[3];

            _Params[0] = GetConsole();
            _Params[1] = "/v /c";

            if (aParams.Length > 0) {
                _Params[2] =  "/p:" + (string) aParams[0];
            }

            _RunProcess.OnConsoleOutput += OnConsoleOutput;
            _RunProcess.OnRunCompleted += OnRunCompleted;
            _RunProcess.Run(_Params, "");
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

            _RunProcess.Run(_Params, "");
        }
    }
}
