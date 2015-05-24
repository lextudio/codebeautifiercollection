namespace ZetaLib.Core.Collections
{
	#region Using directives.
	// ------------------------------------------------------------------

	using System;
	using System.Collections;
	using System.Data;
	using System.IO;
	using System.Runtime.Serialization;
	using System.Runtime.Serialization.Formatters;
	using System.Runtime.Serialization.Formatters.Binary;
	using System.Text;
	using System.Xml;
	using ICSharpCode.SharpZipLib;
	using ICSharpCode.SharpZipLib.Checksums;
	using ICSharpCode.SharpZipLib.Zip;
	using System.Diagnostics;
	using System.Runtime.InteropServices;

	// ------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// A simple pair of strings, used for e.g. name-value pairs inside
	/// combobox values.
	/// </summary>
	[Serializable]
	[ComVisible( false )]
	public class StringPair :
		Pair<string, string>
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Initializes a new instance of the <see cref="StringPair"/> class.
		/// </summary>
		public StringPair()
			:
			base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="StringPair"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		public StringPair(
			string name )
			:
			base( name )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="StringPair"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="val">The val.</param>
		public StringPair(
			string name,
			string val )
			:
			base( name, val )
		{
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}