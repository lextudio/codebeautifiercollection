using System;

namespace BeWise.SharpBuilderTools.Tools.Ant {
	/// <summary>
	/// Base Ant target.
	/// </summary>
	public class BaseAntTarget : BaseAntItem {

		// *************************************************************************
        //                             Constructor
        // *************************************************************************
        private const string ANT_TARGET_TYPE                                     = "Target";
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">Name</param>
		/// <param name="description">Description</param>
        protected BaseAntTarget(string name, string description) : base(name) {
            this.description = description;
        }

        // *************************************************************************
        //                                Private
        // *************************************************************************

        private string description;

        // *************************************************************************
        //                                Protected
        // *************************************************************************

        protected override string GetAntTypeName() {
            return ANT_TARGET_TYPE;
        }

        // *************************************************************************
        //                             Properties
        // *************************************************************************
		/// <summary>
		/// Description.
		/// </summary>
        public string Description {
            get {
                return description;
            }
        }
    }
}
