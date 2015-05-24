namespace ZetaLib.Core.Data
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Data.OracleClient;
	using System.Runtime.InteropServices;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Because OracleParameterCollection cannot be instantiated,
	/// this class mimics the required operations of OracleParameterCollection.
	/// </summary>
	[ComVisible( false )]
	public class AdoNetOracleParamCollection :
		AdoNetBaseParamCollection<OracleParameter>
	{
		#region Public constructors.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		public AdoNetOracleParamCollection()
			:
			base()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parameters">The parameters.</param>
		public AdoNetOracleParamCollection(
			params OracleParameter[] parameters )
			:
			base( parameters )
		{
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}