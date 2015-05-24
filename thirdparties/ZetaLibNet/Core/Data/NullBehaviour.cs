namespace ZetaLib.Core.Data
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// How the CreateParameter()-functions behave when passing null-values.
	/// </summary>
	public enum NullBehaviour
	{
		#region Enum members.
		// ------------------------------------------------------------------

		/// <summary>
		/// Convert NULL values to DBNull.Value.
		/// </summary>
		ConvertNullToDBNull,

		/// <summary>
		/// Just pass on the parameters with no auto-conversion.
		/// </summary>
		NoConversion,

		/// <summary>
		/// Convert a null value to string.Empty.
		/// </summary>
		ConvertNullToEmptyString,

		/// <summary>
		/// Convert a string.Empty to DBNull.Value.
		/// </summary>
		ConvertEmptyStringToDBNull,

		/// <summary>
		/// Convert an integer of zero to DBNull.Value.
		/// </summary>
		ConvertZeroIntToDBNull

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}