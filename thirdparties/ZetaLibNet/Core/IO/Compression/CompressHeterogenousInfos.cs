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
	/// Info class for compressing multiple miscellaneous sources.
	/// </summary>
	public class CompressHeterogenousInfos
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Add a definition.
		/// </summary>
		/// <param name="info">The info.</param>
		public void Add(
			CompressHeterogenousInfo info )
		{
			items.Add( info );
		}

		/// <summary>
		/// Add a file.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		public void AddFile(
			FileInfo filePath )
		{
			AddFile( filePath.FullName );
		}

		/// <summary>
		/// Add a file.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		public void AddFile(
			string filePath )
		{
			CompressHeterogenousInfo info = 
				new CompressHeterogenousInfo();

			info.Type = CompressHeterogenousInfo.InfoType.File;
			info.FilePath = filePath;

			items.Add( info );
		}

		/// <summary>
		/// Add multiple files.
		/// </summary>
		/// <param name="filePaths">The file paths.</param>
		public void AddFiles(
			string[] filePaths )
		{
			if ( filePaths != null && filePaths.Length > 0 )
			{
				foreach ( string filePath in filePaths )
				{
					CompressHeterogenousInfo info = 
						new CompressHeterogenousInfo();

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
		/// <param name="folderPath">The folder path.</param>
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
		/// <param name="folderPath">The folder path.</param>
		/// <param name="eventSink">The event sink.</param>
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
		/// <param name="folderPath">The folder path.</param>
		/// <param name="eventSink">The event sink.</param>
		/// <param name="deep">The deep.</param>
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
		/// <param name="folderPath">The folder path.</param>
		/// <param name="eventSink">The event sink.</param>
		/// <param name="deep">The deep.</param>
		/// <param name="pathSaveMode">The path save mode.</param>
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
		/// <param name="folderPath">The folder path.</param>
		/// <param name="eventSink">The event sink.</param>
		/// <param name="deep">The deep.</param>
		/// <param name="pathSaveMode">The path save mode.</param>
		/// <param name="relativeRootPostfix">The relative root postfix.</param>
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
		/// Does the add folder.
		/// </summary>
		/// <param name="baseFolderPath">The base folder path.</param>
		/// <param name="currentFolderPath">The current folder path.</param>
		/// <param name="eventSink">The event sink.</param>
		/// <param name="deep">The deep.</param>
		/// <param name="pathSaveMode">The path save mode.</param>
		/// <param name="relativeRootPostfix">The relative root postfix.</param>
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

			CompressHeterogenousInfo info = new CompressHeterogenousInfo();

			info.Type = CompressHeterogenousInfo.InfoType.String;
			info.Content = content;
			info.FilePath = fileName;

			items.Add( info );
		}

		/// <summary>
		/// Add a byte[] with an auto-generated filename.
		/// </summary>
		/// <param name="content">The content.</param>
		public void AddBytes(
			byte[] content )
		{
			AddBytes( content, null );
		}

		/// <summary>
		/// Add a byte[] with a provided filename.
		/// </summary>
		/// <param name="content">The content.</param>
		/// <param name="fileName">Name of the file.</param>
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

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		private List<CompressHeterogenousInfo> items =
			new List<CompressHeterogenousInfo>();

		// ------------------------------------------------------------------
		#endregion

		#region Private properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Used internally only.
		/// </summary>
		/// <value>The internal items.</value>
		internal List<CompressHeterogenousInfo> InternalItems
		{
			get
			{
				return items;
			}
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}