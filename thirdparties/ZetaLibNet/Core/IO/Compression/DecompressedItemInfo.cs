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
	/// Info class for decompressing into memory.
	/// </summary>
	public class DecompressedItemInfo
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="decompressedBytes">The decompressed bytes.</param>
		/// <param name="fileName">Name of the file.</param>
		public DecompressedItemInfo(
			byte[] decompressedBytes,
			string fileName )
		{
			this.decompressedBytes = decompressedBytes;
			this.fileName = fileName;
		}

		/// <summary>
		/// Saves the decompressed bytes to the given path.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		public void SaveToFile(
			string filePath )
		{
			string folderPath = Path.GetDirectoryName( filePath );

			if ( !Directory.Exists( folderPath ) )
			{
				Directory.CreateDirectory( folderPath );
			}

			if ( File.Exists( filePath ) )
			{
				File.Delete( filePath );
			}

			using ( FileStream fs = new FileStream(
				filePath,
				FileMode.CreateNew,
				FileAccess.Write ) )
			using ( BinaryWriter w = new BinaryWriter( fs ) )
			{
				w.Write( decompressedBytes );
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// The decompressed bytes.
		/// </summary>
		/// <value>The decompressed bytes.</value>
		public byte[] DecompressedBytes
		{
			get
			{
				return decompressedBytes;
			}
		}

		/// <summary>
		/// The decompressed bytes as a string.
		/// </summary>
		/// <value>The decompressed string.</value>
		public string DecompressedString
		{
			get
			{
				return Encoding.UTF8.GetString( decompressedBytes );
			}
		}

		/// <summary>
		/// The filename that was stored for the decompressed bytes.
		/// </summary>
		/// <value>The name of the file.</value>
		public string FileName
		{
			get
			{
				return fileName;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		private byte[] decompressedBytes = null;
		private string fileName = null;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}