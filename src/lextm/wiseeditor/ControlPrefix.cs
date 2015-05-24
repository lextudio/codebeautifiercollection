namespace Lextm.WiseEditor {

    using System;
	///<summary>
	/// Control prefix.
	/// </summary>
	[Serializable]
	public class ControlPrefix {

		// *************************************************************************
		//                            Cosntructor
		// *************************************************************************
		/// <summary>
		/// Constructor.
		/// </summary>
	    public ControlPrefix( ) {
		}

		/// <summary>
		/// Contructor.
		/// </summary>
		/// <param name="prefix">Prefix</param>
		/// <param name="controlTypeName">Control type name</param>
	    public ControlPrefix( string prefix, string controlTypeName ) {
			Prefix = prefix;
			ControlTypeName = controlTypeName;
	    }

		// *************************************************************************
		//                            Public Fields
		// *************************************************************************
		/// <summary>
		/// Prefix.
		/// </summary>
		public string Prefix = System.String.Empty;
		/// <summary>
		/// Control type name.
		/// </summary>
	    public string ControlTypeName = System.String.Empty;

	}
}
