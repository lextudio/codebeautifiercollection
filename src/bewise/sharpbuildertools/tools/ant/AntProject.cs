using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace BeWise.SharpBuilderTools.Tools.Ant {
	/// <summary>
	/// Ant project.
	/// </summary>
    class AntProject : BaseAntProject {
        // *************************************************************************
        //                           Constructor
        // *************************************************************************
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="fileName">File name</param>
        internal AntProject(string fileName) : base (fileName) {}

        // *************************************************************************
        //                           Protected
        // *************************************************************************
		/// <summary>
		/// Creates Ant property.
		/// </summary>
		/// <param name="name">Name</param>
		/// <param name="aValue">Value</param>
		/// <returns></returns>
		protected override BaseAntProperty CreateAntProperty(string name, string aValue) {
            return new AntProperty(name, aValue);
        }
		/// <summary>
		/// Creates Ant target.
		/// </summary>
		/// <param name="name">Name</param>
		/// <param name="description">Description</param>
		/// <returns></returns>
        protected override BaseAntTarget CreateAntTarget(string name, string description) {
            return new AntTarget(name, description);
        }

    }
}
