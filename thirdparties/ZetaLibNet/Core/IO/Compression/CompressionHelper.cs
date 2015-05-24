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
	/// Routines for compressingvarious types of data.
	/// </summary>
	public sealed class CompressionHelper
	{
		#region Compressing.
		// ------------------------------------------------------------------

		/// <summary>
		/// Compress a folder with multiple files and subfolders with
		/// the ZIP algorithm. Use the DecompressFolder() routine to
		/// decompress the compressed bytes.
		/// </summary>
		/// <param name="folderPath">The path to the folder
		/// that will be compressed.</param>
		/// <returns>Returns the compressed folder contents.</returns>
		public static byte[] CompressFolder(
			string folderPath )
		{
			if ( folderPath == null || folderPath.Length <= 0 )
			{
				return null;
			}
			else
			{
				using ( MemoryStream buf = new MemoryStream() )
				using ( ZipOutputStream zip = new ZipOutputStream( buf ) )
				{
					Crc32 crc = new Crc32();
					zip.SetLevel( 9 );	// 0..9.

					DoCompressFolder(
						buf,
						zip,
						crc,
						folderPath,
						folderPath );

					zip.Finish();

					// --

					byte[] c = new byte[buf.Length];
					buf.Seek( 0, SeekOrigin.Begin );
					buf.Read( c, 0, c.Length );

					// --

					zip.Close();

					return c;
				}
			}
		}

		/// <summary>
		/// Compress multiple files with the ZIP algorithm.
		/// Use the DecompressFiles() routine to decompress the compressed bytes.
		/// </summary>
		/// <param name="filePaths">The list of all files to
		/// compress.</param>
		/// <returns>Returns the compressed files contents.</returns>
		public static byte[] CompressFiles(
			string[] filePaths )
		{
			if ( filePaths == null || filePaths.Length <= 0 )
			{
				return null;
			}
			else
			{
				using ( MemoryStream buf = new MemoryStream() )
				using ( ZipOutputStream zip = new ZipOutputStream( buf ) )
				{
					Crc32 crc = new Crc32();
					zip.SetLevel( 9 );	// 0..9.

					foreach ( string filePath in filePaths )
					{
						using ( FileStream fs = new FileStream(
							filePath,
							FileMode.Open,
							FileAccess.Read,
							FileShare.Read ) )
						using ( BinaryReader r = new BinaryReader( fs ) )
						{
							byte[] buffer = new byte[fs.Length];
							fs.Read( buffer, 0, buffer.Length );

							ZipEntry entry = new ZipEntry( Path.GetFileName( filePath ) );
							entry.DateTime = DateTime.Now;
							entry.Size = buffer.Length;

							crc.Reset();
							crc.Update( buffer );

							entry.Crc = crc.Value;

							zip.PutNextEntry( entry );
							zip.Write( buffer, 0, buffer.Length );
						}
					}

					zip.Finish();

					// --

					byte[] c = new byte[buf.Length];
					buf.Seek( 0, SeekOrigin.Begin );
					buf.Read( c, 0, c.Length );

					// --

					zip.Close();

					return c;
				}
			}
		}

		/// <summary>
		/// Compress a file with the ZIP algorithm.
		/// Use the DecompressFile() routine to decompress the compressed bytes.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <returns>Returns the compressed file content.</returns>
		public static byte[] CompressFile(
			string filePath )
		{
			return CompressFile( new FileInfo( filePath ) );
		}

		/// <summary>
		/// Compress a file with the ZIP algorithm.
		/// Use the DecompressFile() routine to decompress the compressed bytes.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <returns>Returns the compressed file content.</returns>
		public static byte[] CompressFile(
			FileInfo filePath )
		{
			using ( FileStream fs = new FileStream(
				filePath.FullName,
				FileMode.Open,
				FileAccess.Read ) )
			using ( BinaryReader r = new BinaryReader( fs ) )
			{
				byte[] buf = new byte[fs.Length];
				r.Read( buf, 0, (int)fs.Length );

				return CompressBytes( buf, filePath.Name );
			}
		}

		/// <summary>
		/// Compress a XML document with the ZIP algorithm.
		/// Use the DecompressXmlDocument() routine to decompress
		/// the compressed bytes.
		/// </summary>
		/// <param name="input">The XML document to compress.</param>
		/// <returns>Returns the compressed XML document.</returns>
		public static byte[] CompressXmlDocument(
			XmlDocument input )
		{
			return CompressString( input.InnerXml );
		}

		/// <summary>
		/// Compress a string with the ZIP algorithm.
		/// Use the DecompressString() routine to decompress the compressed bytes.
		/// </summary>
		/// <param name="input">The string to compress.</param>
		/// <returns>Returns the compressed string.</returns>
		public static byte[] CompressString(
			string input )
		{
			return CompressBytes(
				Encoding.UTF8.GetBytes( input ) );
		}

		/// <summary>
		/// Compress a string with the ZIP algorithm.
		/// Use the DecompressString() routine to decompress the compressed bytes.
		/// </summary>
		/// <param name="input">The string to compress.</param>
		/// <param name="fileName">The name of the file to store the string in
		/// the zip archive.</param>
		/// <returns>Returns the compressed string.</returns>
		public static byte[] CompressString(
			string input,
			string fileName )
		{
			return CompressBytes(
				Encoding.UTF8.GetBytes( input ),
				fileName );
		}

		/// <summary>
		/// Compress multiple stringPairs with the ZIP algorithm.
		/// Use the DecompressStrings() routine to decompress the compressed bytes.
		/// </summary>
		/// <param name="info">The stringPairs to compress.</param>
		/// <returns>Returns the compressed stringPairs.</returns>
		public static byte[] CompressStrings(
			CompressStringsInfo info )
		{
			if ( info == null ||
				info.InternalStringPairs == null ||
				info.InternalStringPairs.Count <= 0 )
			{
				return null;
			}
			else
			{
				using ( MemoryStream buf = new MemoryStream() )
				using ( ZipOutputStream zip = new ZipOutputStream( buf ) )
				{
					Crc32 crc = new Crc32();
					zip.SetLevel( 9 );	// 0..9.

					int index = 1;
					foreach ( StringPair p in info.InternalStringPairs )
					{
						string fileName = p.Two;
						if ( fileName == null || fileName.Length <= 0 )
						{
							fileName =
								string.Format(
								@"file{0}.bin",
								index );
						}

						byte[] buffer = Encoding.UTF8.GetBytes( p.One );

						ZipEntry entry = new ZipEntry( fileName );
						entry.DateTime = DateTime.Now;
						entry.Size = buffer.Length;

						crc.Reset();
						crc.Update( buffer );

						entry.Crc = crc.Value;

						zip.PutNextEntry( entry );
						zip.Write( buffer, 0, buffer.Length );

						index++;
					}

					zip.Finish();

					// --

					byte[] c = new byte[buf.Length];
					buf.Seek( 0, SeekOrigin.Begin );
					buf.Read( c, 0, c.Length );

					// --

					zip.Close();

					return c;
				}
			}
		}

		/// <summary>
		/// Compress multiple items with the ZIP algorithm.
		/// </summary>
		/// <param name="infos">The items to compress.</param>
		/// <returns>Returns the compressed items.</returns>
		public static byte[] CompressHeterogenous(
			CompressHeterogenousInfos infos )
		{
			return CompressHeterogenous( infos, null, null );
		}

		/// <summary>
		/// Compress multiple items with the ZIP algorithm.
		/// </summary>
		/// <param name="infos">The items to compress.</param>
		/// <param name="progress">The progress.</param>
		/// <returns>Returns the compressed items.</returns>
		public static byte[] CompressHeterogenous(
			CompressHeterogenousInfos infos,
			ICancelableProgress progress )
		{
			return CompressHeterogenous( infos, progress, null );
		}

		/// <summary>
		/// Compress multiple items with the ZIP algorithm.
		/// </summary>
		/// <param name="infos">The items to compress.</param>
		/// <param name="progress">The progress.</param>
		/// <returns>Returns the compressed items.</returns>
		public static byte[] CompressHeterogenous(
			CompressHeterogenousInfos infos,
			Cancelable progress )
		{
			return CompressHeterogenous( infos, null, progress );
		}

		/// <summary>
		/// Compress multiple items with the ZIP algorithm.
		/// </summary>
		/// <param name="infos">The items to compress.</param>
		/// <param name="progress1">The progress1.</param>
		/// <param name="progress2">The progress2.</param>
		/// <returns>Returns the compressed items.</returns>
		private static byte[] CompressHeterogenous(
			CompressHeterogenousInfos infos,
			ICancelableProgress progress1,
			Cancelable progress2 )
		{
			using ( MemoryStream destinationStream = new MemoryStream() )
			{
				Stream realStream = CompressHeterogenousToStream(
					infos,
					progress1,
					progress2,
					destinationStream );

				if ( realStream == null )
				{
					return null;
				}
				else
				{
					using ( realStream )
					{
						byte[] c = new byte[destinationStream.Length];

						destinationStream.Seek( 0, SeekOrigin.Begin );
						destinationStream.Read( c, 0, c.Length );

						return c;
					}
				}
			}
		}

		/// <summary>
		/// Compress multiple items with the ZIP algorithm.
		/// </summary>
		/// <param name="infos">The items to compress.</param>
		/// <param name="destinationFilePath">The destination file path.</param>
		public static void CompressHeterogenousToFile(
			CompressHeterogenousInfos infos,
			FileInfo destinationFilePath )
		{
			CompressHeterogenousToFile(
				infos,
				null,
				null,
				destinationFilePath );
		}

		/// <summary>
		/// Compress multiple items with the ZIP algorithm.
		/// </summary>
		/// <param name="infos">The items to compress.</param>
		/// <param name="progress">The progress.</param>
		/// <param name="destinationFilePath">The destination file path.</param>
		public static void CompressHeterogenousToFile(
			CompressHeterogenousInfos infos,
			ICancelableProgress progress,
			FileInfo destinationFilePath )
		{
			CompressHeterogenousToFile(
				infos,
				progress,
				null,
				destinationFilePath );
		}

		/// <summary>
		/// Compress multiple items with the ZIP algorithm.
		/// </summary>
		/// <param name="infos">The items to compress.</param>
		/// <param name="progress">The progress.</param>
		/// <param name="destinationFilePath">The destination file path.</param>
		public static void CompressHeterogenousToFile(
			CompressHeterogenousInfos infos,
			Cancelable progress,
			FileInfo destinationFilePath )
		{
			CompressHeterogenousToFile(
				infos,
				null,
				progress,
				destinationFilePath );
		}

		/// <summary>
		/// Compress multiple items with the ZIP algorithm.
		/// </summary>
		/// <param name="infos">The items to compress.</param>
		/// <param name="progress1">The progress1.</param>
		/// <param name="progress2">The progress2.</param>
		/// <param name="destinationFilePath">The destination file path.</param>
		private static void CompressHeterogenousToFile(
			CompressHeterogenousInfos infos,
			ICancelableProgress progress1,
			Cancelable progress2,
			FileInfo destinationFilePath )
		{
			if ( destinationFilePath.Exists )
			{
				destinationFilePath.Delete();
			}

			using ( FileStream destinationStream = new FileStream(
				destinationFilePath.FullName,
				FileMode.OpenOrCreate,
				FileAccess.ReadWrite ) )
			{
				Stream realStream =
					CompressHeterogenousToStream(
					infos,
					progress1,
					progress2,
					destinationStream );

				if ( realStream != null )
				{
					realStream.Dispose();
				}
			}
		}

		/// <summary>
		/// Compress multiple items with the ZIP algorithm.
		/// </summary>
		/// <param name="infos">The items to compress.</param>
		/// <param name="progress1">The progress1.</param>
		/// <param name="progress2">The progress2.</param>
		/// <param name="destinationStream">The destination stream.</param>
		/// <returns></returns>
		private static Stream CompressHeterogenousToStream(
			CompressHeterogenousInfos infos,
			ICancelableProgress progress1,
			Cancelable progress2,
			Stream destinationStream )
		{
			if ( infos == null ||
				infos.InternalItems == null ||
				infos.InternalItems.Count <= 0 )
			{
				return null;
			}
			else
			{
				// No "using" block.
				ZipOutputStream zip = new ZipOutputStream( destinationStream );

				Crc32 crc = new Crc32();
				zip.SetLevel( 9 );	// 0..9.

				int index = 1;
				foreach ( CompressHeterogenousInfo info
					in infos.InternalItems )
				{
					// Cancel, if requested.
					if ( progress1 != null )
					{
						if ( progress1.OnProgress( null, EventArgs.Empty ) ==
							CancelMode.Cancel )
						{
							return null;
						}
					}
					// Cancel, if requested.
					if ( progress2 != null )
					{
						if ( progress2( info ) == CancelMode.Cancel )
						{
							return null;
						}
					}

					// --

					byte[] buffer = null;
					string fileName = null;
					bool putEntry;

					switch ( info.Type )
					{
						case CompressHeterogenousInfo.InfoType.String:
							fileName = info.FilePath;
							if ( fileName == null || fileName.Length <= 0 )
							{
								fileName =
									string.Format(
									@"file{0}.bin",
									index );
							}

							buffer = Encoding.UTF8.GetBytes( info.Content );

							putEntry = true;
							break;

						case CompressHeterogenousInfo.InfoType.File:
							fileName = Path.GetFileName( info.FilePath );

							ZipEntry entry = new ZipEntry( fileName );
							zip.PutNextEntry( entry );

							using ( FileStream fs = new FileStream(
								info.FilePath,
								FileMode.Open,
								FileAccess.Read,
								FileShare.Read ) )
							using ( BinaryReader r = new BinaryReader( fs ) )
							{
								byte[] smallBuffer = new byte[16384];

								int bytesRead;
								do
								{
									bytesRead = r.Read(
										smallBuffer,
										0,
										smallBuffer.Length );
									zip.Write( smallBuffer, 0, bytesRead );

									// --

									// Cancel, if requested.
									if ( progress1 != null )
									{
										if ( progress1.OnProgress(
											null,
											EventArgs.Empty ) ==
											CancelMode.Cancel )
										{
											return null;
										}
									}
									// Cancel, if requested.
									if ( progress2 != null )
									{
										if ( progress2( info ) ==
											CancelMode.Cancel )
										{
											return null;
										}
									}
								}
								while ( bytesRead > 0 );
							}

							putEntry = false;
							break;

						case CompressHeterogenousInfo.InfoType.Bytes:
							fileName = info.FilePath;
							if ( fileName == null ||
								fileName.Length <= 0 )
							{
								fileName =
									string.Format(
									@"file{0}.bin",
									index );
							}

							buffer = info.Bytes;

							putEntry = true;
							break;

						default:
							Debug.Assert(
								false,
								string.Format(
								@"Unknown compression info type '{0}'.",
								info.Type ) );
							putEntry = false;
							break;
					}

					// --

					if ( putEntry )
					{
						ZipEntry entry = new ZipEntry( fileName );
						entry.DateTime = DateTime.Now;
						entry.Size = buffer.Length;

						crc.Reset();
						crc.Update( buffer );

						entry.Crc = crc.Value;

						zip.PutNextEntry( entry );
						zip.Write( buffer, 0, buffer.Length );
					}

					index++;
				}

				zip.Finish();
				return zip;
			}
		}

		/// <summary>
		/// Compress a DataSet with the ZIP algorithm.
		/// Use the DecompressDataSet() routine to decompress
		/// the compressed bytes.
		/// </summary>
		/// <param name="input">The DataSet to compress.</param>
		/// <returns>Returns the compressed DataSet.</returns>
		public static byte[] CompressDataSet(
			DataSet input )
		{
			BinaryFormatter bf = new BinaryFormatter();

			using ( MemoryStream ms = new MemoryStream() )
			{
				bf.Serialize( ms, input );
				return CompressBytes( ms.GetBuffer() );
			}
		}

		/// <summary>
		/// Compress a byte array with the ZIP algorithm.
		/// Use the DecompressBytes() routine to decompress the compressed bytes.
		/// </summary>
		/// <param name="input">The bytes to compress.</param>
		/// <returns>Returns the compressed bytes.</returns>
		public static byte[] CompressBytes(
			byte[] input )
		{
			return CompressBytes( input, @"file.bin" );
		}

		/// <summary>
		/// Compress a byte array with the ZIP algorithm.
		/// Use the DecompressBytes() routine to decompress the compressed bytes.
		/// </summary>
		/// <param name="input">The bytes to compress.</param>
		/// <param name="fileName">The name of the byte stream in the zip file.</param>
		/// <returns>Returns the compressed bytes.</returns>
		private static byte[] CompressBytes(
			byte[] input,
			string fileName )
		{
			using ( MemoryStream buf = new MemoryStream() )
			using ( ZipOutputStream zip = new ZipOutputStream( buf ) )
			{
				Crc32 crc = new Crc32();
				zip.SetLevel( 9 );	// 0..9.

				ZipEntry entry = new ZipEntry( fileName );
				entry.DateTime = DateTime.Now;
				entry.Size = input.Length;

				crc.Reset();
				crc.Update( input );

				entry.Crc = crc.Value;

				zip.PutNextEntry( entry );

				zip.Write( input, 0, input.Length );
				zip.Finish();

				// --

				byte[] c = new byte[buf.Length];
				buf.Seek( 0, SeekOrigin.Begin );
				buf.Read( c, 0, c.Length );

				// --

				zip.Close();

				return c;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Helper routines for compressing.
		// ------------------------------------------------------------------

		/// <summary>
		/// Helper routine for recursive compressing a folder.
		/// </summary>
		/// <param name="buf">The buf.</param>
		/// <param name="zip">The associated ZIP stream to write to.</param>
		/// <param name="crc">The associated CRC object.</param>
		/// <param name="baseFolderPath">The path of the initial folder.</param>
		/// <param name="currentFolderPath">The path of the current folder to process.</param>
		private static void DoCompressFolder(
			MemoryStream buf,
			ZipOutputStream zip,
			Crc32 crc,
			string baseFolderPath,
			string currentFolderPath )
		{
			// Add all files of the current folder.
			foreach ( string filePath in
				Directory.GetFiles( currentFolderPath ) )
			{
				// Make relative path for storing the information
				// inside the ZIP file.
				string relativeFilePath = filePath.Substring(
					baseFolderPath.Length ).Trim( '\\' );

				using ( FileStream fs = new FileStream(
					filePath,
					FileMode.Open,
					FileAccess.Read ) )
				using ( BinaryReader r = new BinaryReader( fs ) )
				{
					byte[] buffer = new byte[fs.Length];
					fs.Read( buffer, 0, buffer.Length );

					ZipEntry entry = new ZipEntry( relativeFilePath );
					entry.DateTime = DateTime.Now;
					entry.Size = buffer.Length;

					crc.Reset();
					crc.Update( buffer );

					entry.Crc = crc.Value;

					zip.PutNextEntry( entry );
					zip.Write( buffer, 0, buffer.Length );
				}
			}

			// Recurse all subfolders.
			foreach ( string folderPath in
				Directory.GetDirectories( currentFolderPath ) )
			{
				DoCompressFolder(
					buf,
					zip,
					crc,
					baseFolderPath,
					folderPath );
			}
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}