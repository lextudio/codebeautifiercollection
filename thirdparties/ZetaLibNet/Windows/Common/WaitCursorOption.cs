namespace ZetaLib.Windows.Common
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Collections.Generic;
	using System.Text;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Options for the WaitCursor class.
	/// </summary>
	[Flags]
	public enum WaitCursorOption
	{
		#region Enum members.
		// ------------------------------------------------------------------

		/// <summary>
		/// No options.
		/// </summary>
		None = 0x00,

		/// <summary>
		/// The standard options.
		/// </summary>
		Standard = ShortSleep,

		/// <summary>
		/// Put a small sleep on the executing thread. Usually for actually
		/// showing the wait cursor at all.
		/// </summary>
		ShortSleep = 0x01,

		/// <summary>
		/// Calls Application.DoEvents().
		/// </summary>
		DoEvents = 0x02

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}