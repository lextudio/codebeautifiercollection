namespace ZetaLib.Core.Common
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Configuration;
	using System.Collections;
	using System.Diagnostics;
	using System.IO;
	using System.Text;
	using System.Text.RegularExpressions;
	using ZetaLib.Core.Common;
	using System.Data;
	using ZetaLib.Core.Data;
	using System.Data.OleDb;
	using ZetaLib.Core.Properties;
	using System.Runtime.InteropServices;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Classes implementing this know how to dump themself.
	/// </summary>
	[ComVisible( false )]
	public interface IDumpable
	{
		#region Interface members.
		// ------------------------------------------------------------------

		/// <summary>
		/// Dump for a current indent level and deep.
		/// </summary>
		/// <param name="indentLevel">How many tabs to indent.</param>
		/// <param name="deep">Whether to dump child objects.</param>
		/// <returns>
		/// Returns the textual representation of the dump.
		/// </returns>
		/// <remarks>
		/// Use the DumpBuilder class like this:
		/// "DumpBuilder db = new DumpBuilder( indentLevel, deep, GetType() );".
		/// </remarks>
		string Dump(
			int indentLevel,
			bool deep );

		/// <summary>
		/// Dump for a indent level of zero and deep=true.
		/// </summary>
		/// <returns>
		/// Returns the textual representation of the dump.
		/// </returns>
		/// <remarks>
		/// Usually you just redirect this overload to "return Dump( 0, true );".
		/// </remarks>
		string Dump();

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}