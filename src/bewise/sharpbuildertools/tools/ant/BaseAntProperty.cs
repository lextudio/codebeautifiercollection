using System;

namespace BeWise.SharpBuilderTools.Tools.Ant {
	/// <summary>
	/// Base Ant property.
	/// </summary>
    public class BaseAntProperty : BaseAntItem {
        // *************************************************************************
        //                             Private Fields
        // *************************************************************************
        private const string ANT_PROPERTY_TYPE                                   = "Property";

        string fValue;

        // *************************************************************************
        //                             Constructor
        // *************************************************************************
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">Name</param>
		/// <param name="aValue">Value</param>
        protected BaseAntProperty(string name, string aValue) : base(name) {
            fValue = aValue;
        }

        // *************************************************************************
        //                                Protected
        // *************************************************************************

        protected override string GetAntTypeName() {
            return ANT_PROPERTY_TYPE;
        }

        // *************************************************************************
        //                             Public Properties
        // *************************************************************************
		/// <summary>
		/// Value.
		/// </summary>
        public string Value {
            get {
                return fValue;
            }
        }

    }
}
