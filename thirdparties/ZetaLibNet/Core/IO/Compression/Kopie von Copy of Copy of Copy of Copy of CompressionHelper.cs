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
	/// Routines for compressing and decompressing various
	/// types of data.
	/// </summary>
	/// <remarks>	
	/// Please contact the author Uwe Keim (mailto:uwe.keim@zeta-software.de)
	/// for questions regarding this class.
	/// </remarks>
	public sealed class CompressionHelper
	{
		#region Info class for compressing multiple strings.
		// ------------------------------------------------------------------

		/// <summary>
		/// Info class for compressing multiple strings.
		/// </summary>
		public class CompressStringsInfo
		{
			/// <summary>
			/// Add a string with an auto-generated filename.
			/// </summary>
			/// <param name="content"></param>
			public void AddString(
				string content )
			{
				AddString( content, null );
			}

			/// <summary>
			/// Add a string with a provided filename.
			/// </summary>
			/// <param name="content"></param>
			/// <param name="fileName"></param>
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

			private List<StringPair> stringPairs = new List<StringPair>();

			/// <summary>
			/// Used internally only.
			/// </summary>
			public List<StringPair> InternalStringPairs
			{
				get
				{
					return stringPairs;
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Info class for compressing multiple items.
		// ------------------------------------------------------------------

		/// <summary>
		/// How paths are stored when packing files to an archive.
		/// </summary>
		public enum PathSaveMode
		{
			#region Enum members.

			/// <summary>
			/// Only save the file name and extension.
			/// </summary>
			None,

			/// <summary>
			/// Save relative to the given folder.
			/// </summary>
			Relative,

			/// <summary>
			/// Save (nearly) absolute path. 
			/// "Nearly", because Winzip always displays absolute paths with
			/// the drive resp. server+share removed, so do it here the same way.
			/// Internally, the function PathHelper.SplitPath().DirectoryAndNameWithExtension
			/// is being used. So when comparing with a given path, use the same function
			/// to adjust the path to compare with.
			/// </summary>
			Absolute

			#endregion
		}

		/// <summary>
		/// How to traverse a folder when adding files.
		/// </summary>
		public enum DeepMode
		{
			#region Enum members.

			/// <summary>
			/// Visit subfolders, too.
			/// </summary>
			Deep,

			/// <summary>
			/// Only add the files in the given folder, do not
			/// visit subfolders.
			/// </summary>
			Shallow

			#endregion
		}

		/// <summary>
		/// Allows to finer control whether certain files/folders are
		/// added to the ZIP file.
		/// </summary>
		public interface IZipEventSink
		{
			/// <summary>
			/// Return FALSE to prevent adding of the file.
			/// </summary>
			/// <param name="sender"></param>
			/// <param name="filePathToAdd"></param>
			/// <returns></returns>
			bool OnBeforeAddFile(
				CompressHeterogenousInfos sender,
				FileInfo filePathToAdd );

			/// <summary>
			/// Return FALSE to prevent entering folder.
			/// </summary>
			/// <param name="sender"></param>
			/// <param name="folderPathToEnter"></param>
			/// <returns></returns>
			bool OnBeforeEnterFolder(
				CompressHeterogenousInfos sender,
				DirectoryInfo folderPathToEnter );
		}

		/// <summary>
		/// Info class for compressing a single miscellaneous source.
		/// </summary>
		public class CompressHeterogenousInfo
		{
			/// <summary>
			/// The type contained.
			/// </summary>
			public enum InfoType
			{
				#region Enum member.

				/// <summary>
				/// A string.
				/// </summary>
				String,

				/// <summary>
				/// A file.
				/// </summary>
				File,

				/// <summary>
				/// A byte-array.
				/// </summary>
				Bytes

				#endregion
			}

			/// <summary>
			/// The type contained.
			/// </summary>
			public InfoType Type;

			/// <summary>
			/// Depending on the InfoType.
			/// </summary>
			public string Content;

			/// <summary>
			/// Depending on the InfoType.
			/// </summary>
			public string FilePath;

			/// <summary>
			/// Depending on the InfoType.
			/// </summary>
			public byte[] Bytes;
		}

		/// <summary>
		/// Info class for compressing multiple miscellaneous sources.
		/// </summary>
		public class CompressHeterogenousInfos
		{
			/// <summary>
			/// Add a definition.
			/// </summary>
			/// <param name="info"></param>
			public void Add(
				CompressHeterogenousInfo info )
			{
				items.Add( info );
			}

			/// <summary>
			/// Add a file.
			/// </summary>
			/// <param name="destinationFilePath"></param>
			public void AddFile(
				string filePath )
			{
				CompressHeterogenousInfo info = new CompressHeterogenousInfo();

				info.Type = CompressHeterogenousInfo.InfoType.File;
				info.FilePath = filePath;

				items.Add( info );
			}

			/// <summary>
			/// Add multiple files.
			/// </summary>
			/// <param name="filePaths"></param>
			public void AddFiles(
				string[] filePaths )
			{
				if ( filePaths != null && filePaths.Length > 0 )
				{
					foreach ( string filePath in filePaths )
					{
						CompressHeterogenousInfo info = new CompressHeterogenousInfo();

						info.Type = CompressHeterogenousInfo.InfoType.File;
						info.FilePath = filePath;

						items.Add( info );
					}
				}
			}

			/// <summary>
			/// Add multiple files from the given folder, recursively.
			/// The filepaths are added relatively to the given folder path.
			/// </summary>
			public void AddFolder(
				DirectoryInfo folderPath )
			{
				AddFolder(
					folderPath,
					null,
					DeepMode.Deep,
					PathSaveMode.Relative );
			}

			/// <summary>
			/// Add multiple files from the given folder, recursively.
			/// The filepaths are added relatively to the given folder path.
			/// </summary>
			public void AddFolder(
				DirectoryInfo folderPath,
				IZipEventSink eventSink )
			{
				AddFolder(
					folderPath,
					eventSink,
					DeepMode.Deep,
					PathSaveMode.Relative );
			}

			/// <summary>
			/// Add multiple files from the given folder, recursively.
			/// The filepaths are added relatively to the given folder path.
			/// </summary>
			public void AddFolder(
				DirectoryInfo folderPath,
				IZipEventSink eventSink,
				DeepMode deep )
			{
				AddFolder(
					folderPath,
					eventSink,
					deep,
					PathSaveMode.Relative );
			}

			/// <summary>
			/// Add multiple files from the given folder, recursively.
			/// The filepaths are added relatively to the given folder path.
			/// </summary>
			public void AddFolder(
				DirectoryInfo folderPath,
				IZipEventSink eventSink,
				DeepMode deep,
				PathSaveMode pathSaveMode )
			{
				DoAddFolder(
					folderPath,
					folderPath,
					eventSink,
					deep,
					pathSaveMode,
					null );
			}

			/// <summary>
			/// Add multiple files from the given folder, recursively.
			/// The filepaths are added relatively to the given folder path.
			/// </summary>
			public void AddFolder(
				DirectoryInfo folderPath,
				IZipEventSink eventSink,
				DeepMode deep,
				PathSaveMode pathSaveMode,
				string relativeRootPostfix )
			{
				DoAddFolder(
					folderPath,
					folderPath,
					eventSink,
					deep,
					pathSaveMode,
					relativeRootPostfix );
			}

			/// <summary>
			/// 
			/// </summary>
			private void DoAddFolder(
				DirectoryInfo baseFolderPath,
				DirectoryInfo currentFolderPath,
				IZipEventSink eventSink,
				DeepMode deep,
				PathSaveMode pathSaveMode,
				string relativeRootPostfix )
			{
				Debug.Assert(
					string.IsNullOrEmpty( relativeRootPostfix ) ||
					pathSaveMode == PathSaveMode.Relative );

				if ( eventSink == null ||
					eventSink.OnBeforeEnterFolder( this, currentFolderPath ) )
				{
					// All files.
					foreach ( FileInfo filePath
						in currentFolderPath.GetFiles() )
					{
						if ( eventSink == null ||
							eventSink.OnBeforeAddFile( this, filePath ) )
						{
							string pathSaveValue;
							switch ( pathSaveMode )
							{
								case PathSaveMode.Absolute:
									// Winzip always displays absolute paths with
									// the drive resp. server+share removed, so pack
									// it here in this way, too.
									pathSaveValue =
										PathHelper.SplitPath(
										filePath.FullName ).DirectoryAndNameWithExtension;
									break;
								case PathSaveMode.Relative:
									pathSaveValue =
										PathHelper.GetRelativePath(
										baseFolderPath.FullName,
										filePath.FullName );

									if ( !string.IsNullOrEmpty( relativeRootPostfix ) )
									{
										pathSaveValue = PathHelper.Combine(
											relativeRootPostfix,
											pathSaveValue );
									}
									break;
								case PathSaveMode.None:
									pathSaveValue = filePath.Name;
									break;
								default:
									Debug.Assert( false );
									pathSaveValue = filePath.Name;
									break;
							}

							using ( FileStream fs = new FileStream(
								filePath.FullName,
								FileMode.Open,
								FileAccess.Read,
								FileShare.Read ) )
							using ( BinaryReader r = new BinaryReader( fs ) )
							{
								byte[] buffer = new byte[fs.Length];
								fs.Read( buffer, 0, buffer.Length );

								AddBytes( buffer, pathSaveValue );
							}
						}
					}

					// --

					if ( deep == DeepMode.Deep )
					{
						// All folders.
						foreach ( DirectoryInfo folderPath in currentFolderPath.GetDirectories() )
						{
							DoAddFolder(
								baseFolderPath,
								folderPath,
								eventSink,
								deep,
								pathSaveMode,
								relativeRootPostfix );
						}
					}
				}
			}

			/// <summary>
			/// Add a string with an auto-generated filename.
			/// </summary>
			/// <param name="content"></param>
			public void AddString(
				string content )
			{
				AddString( content, null );
			}

			/// <summary>
			/// Add a string with a provided filename.
			/// </summary>
			/// <param name="content"></param>
			/// <param name="fileName"></param>
			public void AddString(
				string content,
				string fileName )
			{
				if ( fileName == null )
				{
					fileName = string.Empty;
				}

				CompressHeterogenousInfo info = new CompressHeterogenousInfo();

				info.Type = CompressHeterogenousInfo.InfoType.String;
				info.Content = content;
				info.FilePath = fileName;

				items.Add( info );
			}

			/// <summary>
			/// Add a byte[] with an auto-generated filename.
			/// </summary>
			/// <param name="content"></param>
			public void AddBytes(
				byte[] content )
			{
				AddBytes( content, null );
			}

			/// <summary>
			/// Add a byte[] with a provided filename.
			/// </summary>
			/// <param name="content"></param>
			/// <param name="fileName"></param>
			public void AddBytes(
				byte[] content,
				string fileName )
			{
				if ( fileName == null )
				{
					fileName = string.Empty;
				}

				CompressHeterogenousInfo info = new CompressHeterogenousInfo();

				info.Type = CompressHeterogenousInfo.InfoType.Bytes;
				info.Bytes = content;
				info.FilePath = fileName;

				items.Add( info );
			}

			private List<CompressHeterogenousInfo> items =
				new List<CompressHeterogenousInfo>();

			/// <summary>
			/// Used internally only.
			/// </summary>
			internal List<CompressHeterogenousInfo> InternalItems
			{
				get
				{
					return items;
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

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
		/// <param name="destinationFilePath">The file to compress.</param>
		/// <returns>Returns the compressed file content.</returns>
		public static byte[] CompressFile(
			string filePath )
		{
			using ( FileStream fs = new FileStream(
				filePath,
				FileMode.Open,
				FileAccess.Read ) )
			using ( BinaryReader r = new BinaryReader( fs ) )
			{
				byte[] buf = new byte[fs.Length];
				r.Read( buf, 0, (int)fs.Length );

				return CompressBytes( buf, Path.GetFileName( filePath ) );
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
			if ( infos == null ||
				infos.InternalItems == null ||
				infos.InternalItems.Count <= 0 )
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
					foreach ( CompressHeterogenousInfo info in infos.InternalItems )
					{
						byte[] buffer = null;
						string fileName = null;

						switch ( info.Type )
						{
							case CompressHeterogenousInfo.InfoType.String:
								{
									fileName = info.FilePath;
									if ( fileName == null || fileName.Length <= 0 )
									{
										fileName =
											string.Format(
											@"file{0}.bin",
											index );
									}

									buffer = Encoding.UTF8.GetBytes( info.Content );
								}
								break;

							case CompressHeterogenousInfo.InfoType.File:
								{
									using ( FileStream fs = new FileStream(
										info.FilePath,
										FileMode.Open,
										FileAccess.Read,
										FileShare.Read ) )
									using ( BinaryReader r = new BinaryReader( fs ) )
									{
										buffer = new byte[fs.Length];
										fs.Read( buffer, 0, buffer.Length );

										fileName = Path.GetFileName( info.FilePath );
									}
								}
								break;

							case CompressHeterogenousInfo.InfoType.Bytes:
								{
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
								}
								break;

							default:
								Debug.Assert(
									false,
									string.Format(
									@"Unknown compression info type '{0}'.",
									info.Type ) );
								break;
						}

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
		/// Compress a DataSet with the ZIP algorithm.
		/// Use the DecompressDataSet() routine to decompress the compressed bytes.
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

		#region Info class for decompressing into memory.
		// ------------------------------------------------------------------

		/// <summary>
		/// Info about a decompressed item.
		/// </summary>
		public class DecompressedItemInfo
		{
			/// <summary>
			/// Constructor.
			/// </summary>
			/// <param name="decompressedBytes"></param>
			/// <param name="fileName"></param>
			public DecompressedItemInfo(
				byte[] decompressedBytes,
				string fileName )
			{
				this.decompressedBytes = decompressedBytes;
				this.fileName = fileName;
			}

			/// <summary>
			/// The decompressed bytes.
			/// </summary>
			public byte[] DecompressedBytes
			{
				get
				{
					return decompressedBytes;
				}
			}

			/// <summary>
			/// Saves the decompressed bytes to the given path.
			/// </summary>
			/// <param name="destinationFilePath">The full qualified path
			/// to save the bytes to.</param>
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

			/// <summary>
			/// The decompressed bytes as a string.
			/// </summary>
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
			public string FileName
			{
				get
				{
					return fileName;
				}
			}

			private byte[] decompressedBytes = null;
			private string fileName = null;
		}

		// ------------------------------------------------------------------
		#endregion

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
		/// <param name="input">The buffer that contains the compressed
		/// stream with the file.</param>
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
		/// <param name="destinationFilePath">The file path where the file 
		/// will be stored.</param>
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

		#region Helper routines for compressing/decompressing.
		// ------------------------------------------------------------------

		/// <summary>
		/// Helper routine for recursive compressing a folder.
		/// </summary>
		/// <param name="buf">The buffer to write to.</param>
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