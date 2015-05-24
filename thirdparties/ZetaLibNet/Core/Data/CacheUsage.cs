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
	/// Use this enum to control the cache usage. I.e. when/whether to use
	/// the cache or to not use the cache.
	/// </summary>
	public enum CacheUsage
	{
		#region Enum members.
		// ------------------------------------------------------------------

		/// <summary>
		/// Use the default behaviour. I.e. use the cache if it is activated,
		/// don't use if it is deactivated. Also don't use if the cache is
		/// activated paritally.
		/// </summary>
		Default,

		/// <summary>
		/// Don't use the cache, no matter whether it is globally activated.
		/// Also implies 'DontClear'.
		/// </summary>
		IgnoreCache,

		/// <summary>
		/// Hint for an update function that it should NOT clear the cache.
		/// </summary>
		DontClear,

		/// <summary>
		/// Do use the cache, no matter whether it is globally deactivated.
		/// Also does use if the cache is partially activated.
		/// </summary>
		UseCache

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}