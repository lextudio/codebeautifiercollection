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
	public interface IProgress
	{
		#region Interface member.
		// ------------------------------------------------------------------

		/// <summary>
		/// Called when [progress].
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The <see cref="System.EventArgs"/>
		/// instance containing the event data.</param>
		void OnProgress(
			object sender,
			EventArgs args );

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}