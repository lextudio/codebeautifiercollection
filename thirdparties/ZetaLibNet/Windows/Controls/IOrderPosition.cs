namespace ZetaLib.Windows.Controls
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Interface for items that have an order position.
	/// </summary>
	public interface IOrderPosition
	{
		#region Interface members.
		// ------------------------------------------------------------------

		/// <summary>
		/// Gets or sets the order position.
		/// </summary>
		/// <value>The order position.</value>
		int OrderPosition
		{
			get;
			set;
		}

		/// <summary>
		/// Persist the new order position to database.
		/// </summary>
		void StoreOrderPosition();

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}