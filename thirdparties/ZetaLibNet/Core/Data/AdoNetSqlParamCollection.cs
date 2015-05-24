namespace ZetaLib.Core.Data
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Data;
	using System.Data.SqlClient;
	using System.Text.RegularExpressions;
	using System.Collections.Generic;
	using System.Runtime.InteropServices;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Because SqlParameterCollection cannot be instantiated,
	/// this class mimics the required operations of SqlParameterCollection.
	/// </summary>
	[ComVisible( false )]
	public class AdoNetSqlParamCollection :
		AdoNetBaseParamCollection<SqlParameter>
	{
		#region Public constructors.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		public AdoNetSqlParamCollection()
			:
			base()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parameters">The parameters.</param>
		public AdoNetSqlParamCollection(
			params SqlParameter[] parameters )
			:
			base( parameters )
		{
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}