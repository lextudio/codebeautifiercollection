namespace ZetaLib.Core.Data
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Data;
	using System.Data.OleDb;
	using System.IO;
	using System.Text.RegularExpressions;
	using ZetaLib.Core.Common;
	using System.Collections.Generic;
	using ZetaLib.Core.Properties;
	using System.Runtime.InteropServices;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Because OleDbParameterCollection cannot be instantiated,
	/// this class mimics the required operations of OleDbParameterCollection.
	/// </summary>
	[ComVisible( false )]
	public class AdoNetOleDBParamCollection :
		AdoNetBaseParamCollection<OleDbParameter>
	{
		#region Public constructors.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		public AdoNetOleDBParamCollection()
			:
			base()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parameters">The parameters.</param>
		public AdoNetOleDBParamCollection(
			params OleDbParameter[] parameters )
			:
			base( parameters )
		{
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}