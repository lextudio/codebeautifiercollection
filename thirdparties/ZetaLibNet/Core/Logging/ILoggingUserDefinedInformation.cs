namespace ZetaLib.Core.Logging
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Interface to pass additional information during logging to
	/// a logging routine and in turn forward this information to a
	/// user defined log handler.
	/// 
	/// I.e. you usually use this interface to "pipline" objects from a
	/// call to a LogXxx()-function in your code to a logging event handler
	/// in your code through the logging code of this library.
	/// </summary>
	public interface ILoggingUserDefinedInformation
	{
		#region Interface member.
		// ------------------------------------------------------------------
		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}