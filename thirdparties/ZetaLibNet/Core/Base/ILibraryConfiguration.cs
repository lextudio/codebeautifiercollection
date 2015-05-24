namespace ZetaLib.Core.Base
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Xml;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Interface for library-specific configurations.
	/// </summary>
	public interface ILibraryConfiguration
	{
		#region Interface member.
		// ------------------------------------------------------------------

		/// <summary>
		/// Loads this class form the given configuration node.
		/// </summary>
		/// <param name="node">The node.</param>
		void LoadFromXml( 
			XmlNode node );

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}