namespace ZetaLib.Core.Base
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Runtime.InteropServices;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Provide an environment to the classes.
	/// </summary>
	[ComVisible( false )]
	public interface IApplicationEnvironment
	{
		#region Interface member.
		// ------------------------------------------------------------------

		/// <summary>
		/// Pumps the GUI, to stay responsive.
		/// </summary>
		void Pump();

		/// <summary>
		/// Pumps the GUI, to stay responsive.
		/// Overload with optional message to pass.
		/// </summary>
		/// <param name="message">The message.</param>
		void Pump( 
			string message );

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}