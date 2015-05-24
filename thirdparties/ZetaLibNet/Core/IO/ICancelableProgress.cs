namespace ZetaLib.Core.IO
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Collections;
	using System.Globalization;
	using System.IO;
	using System.Text;
	using System.Runtime.InteropServices;
	using System.Diagnostics;
	using System.Reflection;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Generic interface that can be passed to various functions for
	/// being notified upon progress. Useful for e.g. pumping Windows 
	/// messages during lengthy tasks.
	/// </summary>
	public interface ICancelableProgress
	{
		#region Interface member.
		// ------------------------------------------------------------------

		/// <summary>
		/// Return TRUE to continue, FALSE to cancel.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The <see cref="System.EventArgs"/>
		/// instance containing the event data.</param>
		/// <returns></returns>
		CancelMode OnProgress(
			object sender,
			EventArgs args );

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}