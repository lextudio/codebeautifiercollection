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
	using ZetaLib.Core.Logging;
	using ZetaLib.Core.Common;
	using System.Configuration;
	using ZetaLib.Core.Localization;
	using ZetaLib.Core.Properties;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Helper class that simulates the deletion of a file by moving
	/// it to a "delete" folder instead of actually deleting.
	/// </summary>
	/// <remarks>
	/// Used in several projects that seem to accidentially delete a file
	/// when not requested to.
	/// </remarks>
	public sealed class FileDeleteHelper
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Helper to delete a file (by actually moving to another folder).
		/// </summary>
		/// <param name="filePath">The file path to delete.</param>
		/// <returns>
		/// Returns the new file path where the file was
		/// moved to.
		/// </returns>
		public FileInfo DeleteFile(
			FileInfo filePath )
		{
			if ( filePath.Exists )
			{
				if ( DeletedFilesFolderPath != null &&
					!string.IsNullOrEmpty( DeletedFilesFolderPath.FullName ) &&
					DeletedFilesFolderPath.Exists )
				{
					FileInfo destinationFilePath =
						new FileInfo(
						PathHelper.Combine(
						DeletedFilesFolderPath.FullName,
						PathHelper.GetNameWithExtension( filePath ) ) );

					LogCentral.Current.LogDebug(
						string.Format(
						@"[Attachment handling] About to delete file '{0}' " +
						@"by moving to deleted folder '{1}'.",
						filePath,
						destinationFilePath ) );

					File.Move(
						filePath.FullName,
						destinationFilePath.FullName );

					LogCentral.Current.LogDebug(
						string.Format(
						@"[Attachment handling] Successfully deleted file '{0}' " +
						@"by moving to deleted folder '{1}'.",
						filePath,
						destinationFilePath ) );

					return destinationFilePath;
				}
				else
				{
					LogCentral.Current.LogDebug(
						string.Format(
						@"[Attachment handling] Deleted folder path '{0}' does not exist," +
						@"Actually deleting file '{1}'.",
						DeletedFilesFolderPath,
						filePath ) );

					filePath.Delete();
					return null;
				}
			}
			else
			{
				LogCentral.Current.LogDebug(
					string.Format(
					@"[Attachment handling] Not deleting file '{0}' " +
					@"because it does not exist.",
					filePath ) );

				return filePath;
			}
		}

		/// <summary>
		/// Tries to locate the given file in the specified file path, and,
		/// if not found, in the deleted files path.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <param name="alternativePaths">The alternative paths.</param>
		/// <returns>
		/// Returns the file path or NULL if not exists.
		/// </returns>
		public FileInfo LocateFile(
			FileInfo filePath,
			params FileOrDirectoryInfo[] alternativePaths )
		{
			if ( filePath == null ||
				string.IsNullOrEmpty( filePath.FullName ) )
			{
				throw new ArgumentException(
					"Please specify a file name.",
					@"filePath" );
			}
			else
			{
				FileInfo originalFilePath = filePath;

				if ( filePath.Exists )
				{
					LogCentral.Current.LogDebug(
						string.Format(
						@"[Attachment handling] File path '{0}' exists.",
						filePath ) );

					return filePath;
				}
				else
				{
					filePath = new FileInfo(
						PathHelper.Combine(
						DeletedFilesFolderPath.FullName,
						filePath.Name ) );

					if ( filePath.Exists )
					{
						LogCentral.Current.LogDebug(
							string.Format(
							@"[Attachment handling] Deleted file path '{0}' exists.",
							filePath ) );

						return filePath;
					}
					else if ( alternativePaths != null &&
						alternativePaths.Length > 0 )
					{
						foreach ( FileOrDirectoryInfo alternativePath in
							alternativePaths )
						{
							if ( alternativePath.IsFile )
							{
								if ( alternativePath.File.Exists )
								{
									LogCentral.Current.LogDebug(
										string.Format(
										@"[Attachment handling] Alternative file path '{0}' for original file path '{1}' exists.",
										alternativePath.File.FullName,
										originalFilePath ) );

									return alternativePath.File;
								}
							}
							else
							{
								FileInfo tryFilePath = new FileInfo(
									PathHelper.Combine(
									alternativePath.Directory.FullName,
									originalFilePath.Name ) );

								if ( tryFilePath.Exists )
								{
									LogCentral.Current.LogDebug(
										string.Format(
										@"[Attachment handling] Alternative file path '{0}' with folder path '{1}' for original file path '{2}' exists.",
										tryFilePath.FullName,
										alternativePath.Directory.FullName,
										originalFilePath ) );

									return tryFilePath;
								}
							}
						}

						LogCentral.Current.LogDebug(
							string.Format(
							@"[Attachment handling] File path '{0}' does not exist, and no suitable alternative found.",
							originalFilePath ) );

						return originalFilePath;
					}
					else
					{
						LogCentral.Current.LogDebug(
							string.Format(
							@"[Attachment handling] File path '{0}' does not exist, no alternative found.",
							originalFilePath ) );

						return originalFilePath;
					}
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Singleton access.
		/// </summary>
		/// <value>The current instance.</value>
		public static FileDeleteHelper Current
		{
			get
			{
				lock ( typeLock )
				{
					if ( current == null )
					{
						current = new FileDeleteHelper();
					}

					return current;
				}
			}
		}

		/// <summary>
		/// Where to store deleted files. Default value can be configures
		/// through the application configuration value
		/// "deletedFilesFolderPath".
		/// </summary>
		/// <value>The deleted files folder path.</value>
		public DirectoryInfo DeletedFilesFolderPath
		{
			get
			{
				if ( deletedFilesFolderPath != null )
				{
					if ( !deletedFilesFolderPath.Exists )
					{
						deletedFilesFolderPath.Create();
					}
				}

				return deletedFilesFolderPath;
			}
			set
			{
				deletedFilesFolderPath = value;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		private FileDeleteHelper()
		{
			DirectoryInfo di =
				LibraryConfiguration.Current.DeletedFilesFolderPath;

			if ( di != null && !string.IsNullOrEmpty( di.FullName ) )
			{
				deletedFilesFolderPath = di;
			}
			else
			{
				// Try to locate the default deleted-files folder path.
				string s = ConvertHelper.ToString(
					ConfigurationManager.AppSettings[
					@"deletedFilesFolderPath"] );

				if ( !string.IsNullOrEmpty( s ) )
				{
					deletedFilesFolderPath = new DirectoryInfo( s );
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		private static object typeLock = new object();
		private static FileDeleteHelper current;

		/// <summary>
		/// 
		/// </summary>
		private DirectoryInfo deletedFilesFolderPath;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}