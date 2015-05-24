using System;
using System.IO;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools;

namespace BeWise.SharpBuilderTools.Tools.Ant {

	class NAnt: BaseAnt {
	
		internal NAnt() {}
        // NAnt
        internal const string NANT_CONSOLE_EXE_NAME                               = "nant.exe";
        internal const string NANT_FILE_EXTENSION                                 = ".build";
        private const string NANT_NAME                                           = "NAnt";

        protected override string GetToolPath () {
        	return (string)Lextm.LeXDK.PropertyRegistry.Get("NAntPath");
        }

        protected override string GetConsoleArgument () {
            return NANT_CONSOLE_EXE_NAME;
        }

        protected override string GetName () {
            return NANT_NAME;
        }


        // *************************************************************************
        //                              Public
        // *************************************************************************

        internal override BaseAntProject CreateAntProject(string aFileName) {
            return new NAntProject(aFileName);
        }

        public override bool IsToolFile(string aFileName) {
            return Path.GetExtension(aFileName).ToUpperInvariant() == NANT_FILE_EXTENSION.ToUpperInvariant();
        }

    }
}
