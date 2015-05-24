namespace ZetaLib.Core.Data
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Data.Odbc;
	using ZetaLib.Core.Properties;
	using System.Runtime.InteropServices;

	// ----------------------------------------------------------------------
	#endregion

	////////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Because OdbcParameterCollection cannot be instantiated,
	/// this class mimics the required operations of OdbcParameterCollection.
	/// </summary>
	[ComVisible( false )]
	public class AdoNetOdbcParamCollection :
		AdoNetBaseParamCollection<OdbcParameter>
	{
		#region Public constructors.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		public AdoNetOdbcParamCollection()
			:
			base()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parameters">The parameters.</param>
		public AdoNetOdbcParamCollection(
			params OdbcParameter[] parameters )
			:
			base( parameters )
		{
		}

		// ------------------------------------------------------------------
		#endregion
	}

	////////////////////////////////////////////////////////////////////////////
}