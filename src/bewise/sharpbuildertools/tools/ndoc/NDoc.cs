using System;
using System.Diagnostics;
using System.Collections.Specialized;
using System.IO;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Helpers;

namespace BeWise.SharpBuilderTools.Tools.NDoc {

	class NDoc : BaseTool {

    	internal NDoc() {}
        /**************************************************************/
        /*                        Protected
        /**************************************************************/
		/// <summary>
		/// All NDoc documentors.
		/// </summary>
		internal readonly static string[] NDocDocumentors
			= new string[] {
            	"Intellisense",
        		"JavaDoc",
				"LaTeX",
				"Linear_Html",
				"MSDN",
				"MSDN_2003",
				"VS.NET_2003",
        		"XML"};
        /// <summary>
        /// Default documentor.
        /// </summary>
		public const string DefaultDocumentor = "MSDN_2003";
        // NDoc
        private const string NDOC_FILE_EXTENSION                                 = ".ndoc";
        private const string NDOC_NAME                                           = "NDoc";
        internal const string NDOC_GUI_EXE_NAME                                   = "NDocGui.exe";
        internal const string NDOC_CONSOLE_EXE_NAME                               = "NDocConsole.exe";

        protected override string GetToolPath () {
			return (string)Lextm.LeXDK.PropertyRegistry.Get("NDocPath");
        }

        protected override string GetGuiArgument () {
            return NDOC_GUI_EXE_NAME;
        }

        protected override string GetConsoleArgument () {
            return NDOC_CONSOLE_EXE_NAME;
        }

        protected override bool GetInstalled () {
            return (File.Exists(Gui) && File.Exists(Console));
        }

        protected override string GetName () {
            return NDOC_NAME;
        }

        /**************************************************************/
        /*                        Public
        /**************************************************************/

        public override bool IsToolFile(string aFileName) {
            return (Path.GetExtension(aFileName).ToUpperInvariant() == NDOC_FILE_EXTENSION.ToUpperInvariant());
        }


        public override void RunConsole(object[] aParams, string aWorkingDir) {
        	base.RunConsole(aParams, aWorkingDir);
            if (!ValidationHelpers.ValidateToolInstalled(this)) {
                return;
            }

            RunProcess _RunProcess = new RunProcess();
            string[] _Params = new String[2];

            _Params[0] = Console;

            if (aParams.Length > 0) {
                _Params[1] = (string) aParams[0];
            }

            _RunProcess.OnConsoleOutput += OnConsoleOutput;
            _RunProcess.OnRunCompleted += OnRunCompleted;
            _RunProcess.Run(_Params, aWorkingDir);
        }

        public override void RunGui(object[] aParams, string aWorkingDir) {
            if (!ValidationHelpers.ValidateToolInstalled(this)) {
                return;
            }

            RunProcess _RunProcess = new RunProcess();
            string[] _Params = new String[1];

            _Params[0] = Gui;

            _RunProcess.Run(_Params, aWorkingDir);
        }
    }
}
