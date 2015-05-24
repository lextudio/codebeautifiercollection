namespace ZetaLib.Core.Data
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Collections;
	using System.Data;
	using System.Data.OleDb;
	using System.Diagnostics;
	using ZetaLib.Core.Common;
	using ZetaLib.Core.Caching;
	using ZetaLib.Core.Logging;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Tells the cache what kind of operation was performed, to allow
	/// for decision about whether to clear the cache after the operation.
	/// </summary>
	public enum AdoNetCacheDBOperation
	{
		#region Enum members.
		// ------------------------------------------------------------------

		/// <summary>
		/// No explicit operation was specified, let the cache
		/// decide itself.
		/// </summary>
		Default,

		/// <summary>
		/// A SELECT, no need to update the cache.
		/// </summary>
		Select,

		/// <summary>
		/// An INSERT, UPDATE or DELETE, cache must be invalidated.
		/// </summary>
		Modifying

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}