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
	/// <param name="userState">The user state.</param>
	public delegate CancelMode Cancelable(
		object userState );

	/////////////////////////////////////////////////////////////////////////
}