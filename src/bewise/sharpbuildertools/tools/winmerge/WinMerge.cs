using System;
using System.IO;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Helpers;

namespace BeWise.SharpBuilderTools.Tools.WinMerge {
	/// <summary>
	/// WinMerge tool.
	/// </summary>
	class WinMerge : BaseTool {

        internal WinMerge() {}
        // Win Merge
		internal const string WIN_MERGE_GUI_EXE_NAME                               = "winmerge.exe";
        private const string WIN_MERGE_NAME                                       = "WinMerge";
        /**************************************************************/
        /*                        Protected
        /**************************************************************/
		/// <summary>
		/// Gets tool path.
		/// </summary>
		/// <returns></returns>
        protected override string GetToolPath () {
			return (string)Lextm.OpenTools.PropertyRegistry.Get("WinMergePath");
        }
		/// <summary>
		/// Gets GUI command line.
		/// </summary>
		/// <returns></returns>
        protected override string GetGuiArgument () {
            return WIN_MERGE_GUI_EXE_NAME;
        }
		/// <summary>
		/// Gets console command line.
		/// </summary>
		/// <returns></returns>
        protected override string GetConsoleArgument () {
            return WIN_MERGE_GUI_EXE_NAME;
        }
		/// <summary>
		/// Gets tool is installed.
		/// </summary>
		/// <returns></returns>
        protected override bool GetInstalled () {
			return File.Exists(GetGui());
        }
		/// <summary>
		/// Gets name.
		/// </summary>
		/// <returns></returns>
        public override string GetName () {
            return WIN_MERGE_NAME;
        }

        /**************************************************************/
        /*                        Public
        /**************************************************************/
		/// <summary>
		/// Is tool file.
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <returns></returns>
        public override bool IsToolFile(string fileName) {
            return true;
        }

		/// <summary>
		/// Runs console.
		/// </summary>
		/// <param name="parameters">Parameters</param>
		/// <param name="workingDir">Working directory</param>
        public override void RunConsole(object[] parameters, string workingDir) {
			base.RunConsole(parameters, workingDir);
            if (!ValidationHelpers.ValidateToolInstalled(this)) {
                return;
            }

            RunProcess _RunProcess = new RunProcess();
            string[] _Params = new String[3];

            _Params[0] = GetConsole();
            _Params[1] = (string) parameters[0];
            _Params[2] = (string) parameters[1];

            //_RunProcess.OnConsoleOutput += OnConsoleOutput;
            //_RunProcess.OnRunCompleted += OnRunCompleted;
            _RunProcess.Run(_Params, "");
        }
		/// <summary>
		/// Runs GUI.
		/// </summary>
		/// <param name="parameters">Parameters</param>
		/// <param name="workingDir">Working directory</param>
        public override void RunGui(object[] parameters, string workingDir) {
            if (!ValidationHelpers.ValidateToolInstalled(this)) {
                return;
            }

            RunProcess _RunProcess = new RunProcess();
            string[] _Params = new String[2];

            _Params[0] = GetGui();

            _RunProcess.Run(_Params, "");
        }
    }
}
