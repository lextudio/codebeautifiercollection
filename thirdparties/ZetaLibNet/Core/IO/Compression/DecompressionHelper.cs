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
	/// Routines for decompressing various types of data.
	/// </summary>
	public sealed class DecompressionHelper
	{
		#region Decompressing.
		// ------------------------------------------------------------------

		/// <summary>
		/// Decompress a byte stream that was formerly compressed
		/// with the CompressFolder() routine with the ZIP algorithm
		/// and store it to a folder.
		/// </summary>
		/// <param name="input">The buffer that contains the compressed
		/// stream with the files and subfolders.</param>
		/// <param name="folderPath">The base path where the files and folders
		/// will be stored.</param>
		public static void DecompressFolder(
			byte[] input,
			string folderPath )
		{
			if ( !Directory.Exists( folderPath ) )
			{
				Directory.CreateDirectory( folderPath );
			}

			using ( MemoryStream mem = new MemoryStream( input ) )
			using ( ZipInputStream stm = new ZipInputStream( mem ) )
			{
				ZipEntry entry;
				while ( (entry = stm.GetNextEntry()) != null )
				{
					// Create this stream new for each zip entry.
					using ( MemoryStream mem2 = new MemoryStream() )
					{
						byte[] data = new byte[4096];

						while ( true )
						{
							int size = stm.Read( data, 0, data.Length );
							if ( size > 0 )
							{
								mem2.Write( data, 0, size );
							}
							else
							{
								break;
							}
						}

						// --
						// Finished reading, now write to file.

						string filePath = Path.Combine( folderPath, entry.Name );

						if ( !Directory.Exists( Path.GetDirectoryName( filePath ) ) )
						{
							Directory.CreateDirectory( Path.GetDirectoryName( filePath ) );
						}

						if ( File.Exists( filePath ) )
						{
							File.Delete( filePath );
						}

						using ( BinaryReader r = new BinaryReader( mem2 ) )
						using ( FileStream fs = new FileStream(
							filePath,
							FileMode.CreateNew,
							FileAccess.Write ) )
						using ( BinaryWriter w = new BinaryWriter( fs ) )
						{
							byte[] buf = new byte[mem2.Length];
							mem2.Seek( 0, SeekOrigin.Begin );
							r.Read( buf, 0, (int)mem2.Length );

							w.Write( buf );
						}
					}
				}
			}
		}

		/// <summary>
		/// Decompress a byte stream that was formerly compressed
		/// with the CompressFiles() routine with the ZIP algorithm
		/// and store it to a file.
		/// </summary>
		/// <param name="input">The buffer that contains the compressed
		/// stream with the files.</param>
		/// <param name="folderPath">The base path where the files will be
		/// stored.</param>
		public static void DecompressFiles(
			byte[] input,
			string folderPath )
		{
			if ( !Directory.Exists( folderPath ) )
			{
				Directory.CreateDirectory( folderPath );
			}

			using ( MemoryStream mem = new MemoryStream( input ) )
			using ( ZipInputStream stm = new ZipInputStream( mem ) )
			{
				ZipEntry entry;
				while ( (entry = stm.GetNextEntry()) != null )
				{
					// Make this stream new for each zip entry.
					using ( MemoryStream mem2 = new MemoryStream() )
					{
						byte[] data = new byte[4096];

						while ( true )
						{
							int size = stm.Read( data, 0, data.Length );
							if ( size > 0 )
							{
								mem2.Write( data, 0, size );
							}
							else
							{
								break;
							}
						}

						// --
						// Finished reading, now write to file.

						string filePath = Path.Combine( folderPath, entry.Name );

						if ( !Directory.Exists( Path.GetDirectoryName( filePath ) ) )
						{
							Directory.CreateDirectory( Path.GetDirectoryName( filePath ) );
						}

						if ( File.Exists( filePath ) )
						{
							File.Delete( filePath );
						}

						using ( BinaryReader r = new BinaryReader( mem2 ) )
						using ( FileStream fs = new FileStream(
							filePath,
							FileMode.CreateNew,
							FileAccess.Write ) )
						using ( BinaryWriter w = new BinaryWriter( fs ) )
						{
							byte[] buf = new byte[mem2.Length];
							mem2.Seek( 0, SeekOrigin.Begin );
							r.Read( buf, 0, (int)mem2.Length );

							w.Write( buf );
						}
					}
				}
			}
		}

		/// <summary>
		/// Decompress a byte stream that was formerly compressed
		/// with the ZIP algorithm.
		/// </summary>
		/// <param name="input">The buffer that contains the compressed
		/// stream with the items.</param>
		/// <returns>Returns the decompressed item(s).</returns>
		public static DecompressedItemInfo[] DecompressItems(
			byte[] input )
		{
			if ( input == null || input.Length <= 0 )
			{
				return null;
			}
			else
			{
				List<DecompressedItemInfo> result =
					new List<DecompressedItemInfo>();

				using ( MemoryStream mem = new MemoryStream( input ) )
				using ( ZipInputStream stm = new ZipInputStream( mem ) )
				{
					ZipEntry entry;
					while ( (entry = stm.GetNextEntry()) != null )
					{
						// Make this stream new for each zip entry.
						using ( MemoryStream mem2 = new MemoryStream() )
						{
							byte[] data = new byte[4096];

							while ( true )
							{
								int size = stm.Read( data, 0, data.Length );
								if ( size > 0 )
								{
									mem2.Write( data, 0, size );
								}
								else
								{
									break;
								}
							}

							// --
							// Finished reading, now write to memory.

							using ( BinaryReader r = new BinaryReader( mem2 ) )
							{
								byte[] c = new byte[mem2.Length];
								mem2.Seek( 0, SeekOrigin.Begin );
								r.Read( c, 0, (int)mem2.Length );

								DecompressedItemInfo info = new DecompressedItemInfo(
									c,
									entry.Name );

								result.Add( info );
							}
						}
					}
				}

				// --

				if ( result.Count <= 0 )
				{
					return null;
				}
				else
				{
					return result.ToArray();
				}
			}
		}

		/// <summary>
		/// Decompress a byte stream that was formerly compressed
		/// with the CompressFile() routine with the ZIP algorithm and
		/// store it to a file.
		/// </summary>
		/// <param name="sourceFilePath">The source file path.</param>
		/// <param name="destinationFilePath">The file path where the file will be
		/// stored.</param>
		public static void DecompressFile(
			FileInfo sourceFilePath,
			FileInfo destinationFilePath )
		{
			DecompressFile(
				sourceFilePath.FullName,
				destinationFilePath.FullName );
		}

		/// <summary>
		/// Decompress a byte stream that was formerly compressed
		/// with the CompressFile() routine with the ZIP algorithm and
		/// store it to a file.
		/// </summary>
		/// <param name="sourceFilePath">The source file path.</param>
		/// <param name="destinationFilePath">The file path where the file will be
		/// stored.</param>
		public static void DecompressFile(
			string sourceFilePath,
			FileInfo destinationFilePath )
		{
			DecompressFile(
				sourceFilePath,
				destinationFilePath.FullName );
		}

		/// <summary>
		/// Decompress a byte stream that was formerly compressed
		/// with the CompressFile() routine with the ZIP algorithm and
		/// store it to a file.
		/// </summary>
		/// <param name="sourceFilePath">The source file path.</param>
		/// <param name="destinationFilePath">The file path where the file will be
		/// stored.</param>
		public static void DecompressFile(
			FileInfo sourceFilePath,
			string destinationFilePath )
		{
			DecompressFile(
			   sourceFilePath.FullName,
			   destinationFilePath );
		}

		/// <summary>
		/// Decompress a byte stream that was formerly compressed
		/// with the CompressFile() routine with the ZIP algorithm and
		/// store it to a file.
		/// </summary>
		/// <param name="sourceFilePath">The source file path.</param>
		/// <param name="destinationFilePath">The file path where the file will be
		/// stored.</param>
		public static void DecompressFile(
			string sourceFilePath,
			string destinationFilePath )
		{
			if ( File.Exists( destinationFilePath ) )
			{
				File.Delete( destinationFilePath );
			}

			using ( FileStream sourceFS = new FileStream(
				sourceFilePath,
				FileMode.Open,
				FileAccess.Read ) )
			using ( BinaryReader r = new BinaryReader( sourceFS ) )
			using ( FileStream destinationFS = new FileStream(
				destinationFilePath,
				FileMode.CreateNew,
				FileAccess.Write ) )
			using ( BinaryWriter w = new BinaryWriter( destinationFS ) )
			{
				byte[] input = new byte[sourceFS.Length];
				r.Read( input, 0, (int)sourceFS.Length );

				byte[] buf = DecompressBytes( input );
				w.Write( buf );
			}
		}

		/// <summary>
		/// Decompress a byte stream that was formerly compressed
		/// with the CompressFile() routine with the ZIP algorithm and
		/// store it to a file.
		/// </summary>
		/// <param name="input">The buffer that contains the compressed
		/// stream with the file.</param>
		/// <param name="filePath">The file path.</param>
		public static void DecompressFile(
			byte[] input,
			FileInfo filePath )
		{
			DecompressFile( input, filePath.FullName );
		}

		/// <summary>
		/// Decompress a byte stream that was formerly compressed
		/// with the CompressFile() routine with the ZIP algorithm and
		/// store it to a file.
		/// </summary>
		/// <param name="input">The buffer that contains the compressed
		/// stream with the file.</param>
		/// <param name="filePath">The file path.</param>
		public static void DecompressFile(
			byte[] input,
			string filePath )
		{
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
				byte[] buf = DecompressBytes( input );
				w.Write( buf );
			}
		}

		/// <summary>
		/// Decompress a byte stream of an XML document that was formerly
		/// compressed with the CompressXmlDocument() routine with
		/// the ZIP algorithm.
		/// </summary>
		/// <param name="input">The buffer that contains the compressed
		/// stream with the XML document.</param>
		/// <returns>Returns the decompressed XML document.</returns>
		public static XmlDocument DecompressXmlDocument(
			byte[] input )
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml( DecompressString( input ) );

			return doc;
		}

		/// <summary>
		/// Decompress a byte stream of a string that was formerly
		/// compressed with the CompressString() routine with the ZIP algorithm.
		/// </summary>
		/// <param name="input">The buffer that contains the compressed
		/// stream with the string.</param>
		/// <returns>Returns the decompressed string.</returns>
		public static string DecompressString(
			byte[] input )
		{
			return Encoding.UTF8.GetString( DecompressBytes( input ) );
		}

		/// <summary>
		/// Decompress a byte stream of a DataSet that was formerly
		/// compressed with the CompressDataSet() routine with the ZIP algorithm.
		/// </summary>
		/// <param name="input">The buffer that contains the compressed
		/// stream with the DataSet.</param>
		/// <returns>Returns the decompressed DataSet.</returns>
		public static DataSet DecompressDataSet(
			byte[] input )
		{
			BinaryFormatter bf = new BinaryFormatter();

			byte[] buffer = DecompressBytes( input );
			using ( MemoryStream ms = new MemoryStream( buffer ) )
			{
				return (DataSet)bf.Deserialize( ms );
			}
		}

		/// <summary>
		/// Decompress a byte stream that was formerly compressed
		/// with the CompressBytes() routine with the ZIP algorithm.
		/// </summary>
		/// <param name="input">The buffer that contains the compressed
		/// stream with the bytes.</param>
		/// <returns>Returns the decompressed bytes.</returns>
		public static byte[] DecompressBytes(
			byte[] input )
		{
			using ( MemoryStream mem = new MemoryStream( input ) )
			using ( ZipInputStream stm = new ZipInputStream( mem ) )
			using ( MemoryStream mem2 = new MemoryStream() )
			{
				ZipEntry entry = stm.GetNextEntry();
				if ( entry != null )
				{
					byte[] data = new byte[4096];

					while ( true )
					{
						int size = stm.Read( data, 0, data.Length );
						if ( size > 0 )
						{
							mem2.Write( data, 0, size );
						}
						else
						{
							break;
						}
					}
				}

				using ( BinaryReader r = new BinaryReader( mem2 ) )
				{
					byte[] c = new byte[mem2.Length];
					mem2.Seek( 0, SeekOrigin.Begin );
					r.Read( c, 0, (int)mem2.Length );

					return c;
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}