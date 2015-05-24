namespace ZetaLib.Windows.Common
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Diagnostics;
	using System.Windows.Forms;
	using ZetaLib.Core.Common;
	using ZetaLib.Core.Collections;
	using System.Collections.Generic;
	using ZetaLib.Windows.Properties;
	using System.Runtime.Serialization;
	using ZetaLib.Core.Logging;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Options for sizing a list view.
	/// </summary>
	public class ListViewSizeOptions
	{
		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Get or set whether the vertical scrollbar width is
		/// subtracted from the total width.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [subtract vertical scroll bar width]; otherwise, <c>false</c>.
		/// </value>
		public bool SubtractVerticalScrollBarWidth
		{
			get
			{
				return subtractVerticalScrollBarWidth;
			}
			set
			{
				subtractVerticalScrollBarWidth = value;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		private bool subtractVerticalScrollBarWidth = true;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}