using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using BeWise.Common;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Helpers;
using NUnit.Framework;

namespace BeWise.SharpBuilderTools.Tools.NUnit {
	/// <summary>
	/// NUnit tool.
	/// </summary>
	class NUnit : BaseTool {

        internal NUnit() {}
        // NUnit
       	internal const string NUNIT_CONSOLE_EXE_NAME                              = "nunit-console.exe";
        internal const string NUNIT_GUI_EXE_NAME                                  = "nunit-gui.exe";
        /**************************************************************/
        /*                        Protected
        /**************************************************************/
		/// <summary>
		/// Gets tool path.
		/// </summary>
		/// <returns></returns>
        protected override string GetToolPath () {
			return (string)Lextm.LeXDK.PropertyRegistry.Get("NUnitPath");
        }
		/// <summary>
		/// Gets GUI command line.
		/// </summary>
		/// <returns></returns>
        protected override string GetGuiArgument () {
            return NUNIT_GUI_EXE_NAME;
        }
        /// <summary>
        /// Gets console command line.
        /// </summary>
        /// <returns></returns>
        protected override string GetConsoleArgument () {
            return NUNIT_CONSOLE_EXE_NAME;
        }
		/// <summary>
		/// Verifies that tool is installed.
		/// </summary>
		/// <returns></returns>
        protected override bool GetInstalled () {
            return (File.Exists(GetGui()) || File.Exists(GetConsole()));
        }
		/// <summary>
		/// Gets name.
		/// </summary>
		/// <returns></returns>
        protected override string GetName () {
            return "NUnit";
        }

        /**************************************************************/
        /*                        Public
        /**************************************************************/
		/// <summary>
		/// Gets test fixtures.
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <returns>new string[0] if wrong.</returns>
		internal static string[] GetTestFixtures(string fileName) {
            if (File.Exists(fileName)) {
                Assembly _Assembly = Assembly.LoadFrom(fileName);

                if (_Assembly != null) {
                    Type[] _Types = _Assembly.GetExportedTypes();

                    ArrayList _List = new ArrayList();

                    foreach (Type _Type in _Types) {
                        if (_Type.GetCustomAttributes(typeof(TestFixtureAttribute), false).Length > 0) {
                            _List.Add(_Type.Namespace + "." +_Type.Name);
                        }
                    }

                    return (string[]) _List.ToArray(typeof(string));
                }
            }

            return new string[0];
        }
		/// <summary>
		/// Is tool file.
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <returns></returns>
        public override bool IsToolFile(string fileName) {
            return (Path.GetExtension(fileName).ToUpperInvariant() == ".NUNIT".ToUpperInvariant()) || (Path.GetExtension(fileName).ToUpperInvariant() == ".EXE".ToUpperInvariant());
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

            string _Assembly = (string) parameters[0];
            string _Fixture = (string) parameters[1];

            RunProcess _RunProcess = new RunProcess();
            string[] _Params = new String[4];

            _Params[0] = Console;

            if (_Assembly != "") {
                //2.0 _Params[1] = "/assembly:" + _Assembly;
                _Params[1] = _Assembly;
            }

            if (_Fixture != "") {
                _Params[2] = "/fixture:" + _Fixture;
            }

            _Params[3] = "/xml:" + _Assembly + CommonConsts.XML_EXTENSION;

            _RunProcess.OnConsoleOutput += OnConsoleOutput;
            _RunProcess.OnRunCompleted += OnRunCompleted;
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

            string _Assembly = (string) parameters[0];
            string _Fixture = (string) parameters[1];

            RunProcess _RunProcess = new RunProcess();
            string[] _Params = new String[3];

            _Params[0] = Gui;

            if (_Assembly != "") {
                //_Params[1] = "/assembly:" + _Assembly;
                _Params[1] = _Assembly;
            }

            if (_Fixture != "") {
                _Params[2] = "/fixture:" + _Fixture;
            }

            _RunProcess.Run(_Params, "");
        }
		/// <summary>
		/// Opens project GUI.
		/// </summary>
		/// <param name="projectFileName">Project file name</param>
		/// <param name="workingDir">Working directory</param>
        internal void OpenProjectGui(string projectFileName, string workingDir) {
            if (!ValidationHelpers.ValidateToolInstalled(this)) {
                return;
            }

            // Create the process
            ProcessStartInfo _ProcessStartInfo = new ProcessStartInfo();

            _ProcessStartInfo.FileName = Gui;
            _ProcessStartInfo.Arguments = projectFileName;
            _ProcessStartInfo.RedirectStandardOutput = false;
            _ProcessStartInfo.CreateNoWindow = true;
            _ProcessStartInfo.UseShellExecute = false;

            Process _P = new Process();
            _P.EnableRaisingEvents = true;
            _P.StartInfo = _ProcessStartInfo;
            _P.Start();
            //_P.WaitForExit();
        }
    }
}
