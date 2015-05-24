using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Helpers;
using BeWise.SharpBuilderTools.Tools;

namespace BeWise.SharpBuilderTools.Tools.Ant {

    public abstract class BaseAnt : BaseTool {
        // *************************************************************************
        //                               Protected
        // *************************************************************************

        protected override string GetConsole() {
            return GetToolPath() + GetConsoleArgument();
        }

        protected override bool GetInstalled () {
            return File.Exists(GetConsole());
        }


        // *************************************************************************
        //                                Public
        // *************************************************************************

        internal abstract BaseAntProject CreateAntProject(string aFileName);

        public override void RunConsole(object[] aParams, string aWorkingDir) {
        	base.RunConsole(aParams, aWorkingDir);
            if (!ValidationHelpers.ValidateToolInstalled(this)) {
                return;
            }

            RunProcess _RunProcess = new RunProcess();
            string[] _Params = new String[aParams.Length +1];

            _Params[0] = ConsoleArgument;

            for (int i=0; i < aParams.Length; i++)
            {    _Params[i + 1] = (string) aParams[i];}

            _RunProcess.OnConsoleOutput += OnConsoleOutput;
            _RunProcess.OnRunCompleted += OnRunCompleted;
            _RunProcess.Run(_Params, aWorkingDir);
        }

		internal static string TargetsToString(string[] aTargets) {
        	StringBuilder _Targets = new StringBuilder();

            if (aTargets != null) {
                for (int i = 0; i < aTargets.Length; i++) {
        			_Targets.Append(aTargets[i]);

                    if (i != aTargets.Length -1) {
        				_Targets.Append(", ");
                    }
                }
            }

        	return _Targets.ToString();
        }
    }
}
