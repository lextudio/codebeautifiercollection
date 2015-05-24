namespace ZetaLib.Core.IO.Compression
{
	#region Using directives.
	// ------------------------------------------------------------------

	using System;
	using System.Collections;
	using System.Data;
	using System.Diagnostics;
	using System.IO;
	using System.Runtime.Serialization;
	using System.Runtime.Serialization.Formatters;
	using System.Runtime.Serialization.Formatters.Binary;
	using System.Text;
	using System.Xml;
	using ICSharpCode.SharpZipLib;
	using ICSharpCode.SharpZipLib.Checksums;
	using ICSharpCode.SharpZipLib.Zip;
	using System.Collections.Generic;
	using ZetaLib.Core.Collections;
	using ZetaLib.Core.IO;

	// ------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Info class for compressing multiple strings.
	/// </summary>
	/// <summary>
	/// Info class for compressing multiple strings.
	/// </summary>
	public sealed class CompressStringsInfo
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Add a string with an auto-generated filename.
		/// </summary>
		/// <param name="content">The content.</param>
		public void AddString(
			string content )
		{
			AddString( content, null );
		}

		/// <summary>
		/// Add a string with a provided filename.
		/// </summary>
		/// <param name="content">The content.</param>
		/// <param name="fileName">Name of the file.</param>
		public void AddString(
			string content,
			string fileName )
		{
			if ( fileName == null )
			{
				fileName = string.Empty;
			}

			stringPairs.Add( new StringPair( content, fileName ) );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		private List<StringPair> stringPairs = new List<StringPair>();

		// ------------------------------------------------------------------
		#endregion

		#region Private properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Used internally only.
		/// </summary>
		/// <value>The internal string pairs.</value>
		internal List<StringPair> InternalStringPairs
		{
			get
			{
				return stringPairs;
			}
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}