namespace ZetaLib.Windows.Common
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Information for restoring a stored form size.
	/// </summary>
	public class RestoreInformation
	{
		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// How much to zoom the form, relative to the screen size.
		/// Only does a zoom if never resized manually before.
		/// </summary>
		/// <value>The suggest zoom percent.</value>
		public int SuggestZoomPercent
		{
			get
			{
				return suggestZoomPercent;
			}
			set
			{
				suggestZoomPercent = value;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		private int suggestZoomPercent;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}