using System;
using System.IO;
using System.Windows.Forms;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools;

namespace BeWise.SharpBuilderTools.Tools.Ant {
	/// <summary>
	/// Ant tool.
	/// </summary>
    class Ant: BaseAnt {
        
		internal Ant() {}
		// Ant
        internal const string ANT_CONSOLE_EXE_NAME                                = "ant.bat";
        private const string ANT_FILE_NAME                                       = "build.xml";
        private const string ANT_NAME                                            = "Ant";

        // *************************************************************************
        //                              Protected
        // *************************************************************************
		/// <summary>
		/// Gets console command line.
		/// </summary>
		/// <returns></returns>
        protected override string GetConsoleArgument () {
            return ANT_CONSOLE_EXE_NAME;
        }
		/// <summary>
		/// Gets name.
		/// </summary>
		/// <returns></returns>
        protected override string GetName () {
            return ANT_NAME;
        }
		/// <summary>
		/// Gets tool path.
		/// </summary>
		/// <returns></returns>
		protected override string GetToolPath () {
			return (string)Lextm.LeXDK.PropertyRegistry.Get("AntPath");
        }

        // *************************************************************************
        //                              Public
        // *************************************************************************

		/// <summary>
		/// Creates Ant project
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <returns></returns>
        internal override BaseAntProject CreateAntProject(string fileName) {
            return new AntProject(fileName);
        }
		/// <summary>
		/// Is tool file.
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <returns></returns>
        public override bool IsToolFile(string fileName) {
            return Path.GetFileName(fileName).ToUpperInvariant() == ANT_FILE_NAME.ToUpperInvariant();
        }

    }
}
