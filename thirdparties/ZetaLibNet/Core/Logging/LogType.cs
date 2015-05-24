namespace ZetaLib.Core.Logging
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// The type of the texts to log.
	/// </summary>
	public enum LogType
	{
		#region Enum member.
		// ------------------------------------------------------------------

		/// <summary>
		/// Unknown type.
		/// </summary>
		Unknown,

		/// <summary>
		/// Info.
		/// </summary>
		Info,

		/// <summary>
		/// Error.
		/// </summary>
		Error,

		/// <summary>
		/// Debug.
		/// </summary>
		Debug,

		/// <summary>
		/// Warning.
		/// </summary>
		Warn,

		/// <summary>
		/// Fatal.
		/// </summary>
		Fatal,

		/// <summary>
		/// Indicated a section that needs (re)work.
		/// </summary>
		Todo

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}