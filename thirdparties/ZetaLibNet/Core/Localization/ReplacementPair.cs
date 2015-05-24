namespace ZetaLib.Core.Localization
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Collections.Generic;
	using System.Text;
	using ZetaLib.Core.Collections;
	using System.Runtime.InteropServices;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// A pair that helps in replacing placeholders in resource strings.
	/// </summary>
	[ComVisible( false )]
	public class ReplacementPair :
		Pair<string, object>
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Initializes a new instance of the <see cref="ReplacementPair"/> class.
		/// </summary>
		/// <param name="a">A.</param>
		/// <param name="b">The b.</param>
		public ReplacementPair(
			string a,
			object b )
			:
			base( a, b )
		{
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}