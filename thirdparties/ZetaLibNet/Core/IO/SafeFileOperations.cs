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

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// The goal of this class is to provide more error-tolerant functions
	/// for basic file operations. Especially when you have a larger project
	/// and ask yourself "why is this file being deleted?" this class helps
	/// by logging each operation and doing it in a more error-tolerant way, 
	/// too. So do all file operations through this class and you get a more 
	/// determinable system, hopefully.
	/// </summary>
	/// <remarks>
	/// 2007-03-08, Uwe Keim: Initially created class.
	/// </remarks>
	public sealed class SafeFileOperations
	{
		#region Public operations.
		// ------------------------------------------------------------------

		/// <summary>
		/// Safes the delete file.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		public static void SafeDeleteFile(
			FileInfo filePath )
		{
			if ( filePath != null )
			{
				SafeDeleteFile( filePath.FullName );
			}
		}

		/// <summary>
		/// Safes the delete file.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		public static void SafeDeleteFile(
			string filePath )
		{
			LogCentral.Current[@"ZetaLib.Core"].LogInfo(
				string.Format(
				@"About to safe-delete file '{0}'.",
				filePath ) );

			if ( !string.IsNullOrEmpty( filePath ) &&
				SafeFileExists( filePath ) )
			{
				try
				{
					File.Delete( filePath );
				}
				catch ( IOException x )
				{
					string newFilePath =
						filePath + @"." + Guid.NewGuid();

					LogCentral.Current[@"ZetaLib.Core"].LogWarn(
						string.Format(
						@"Caught IOException while deleting file '{0}'. " +
						@"Renaming now to '{1}'.",
						filePath,
						newFilePath ),
						x );

					File.Move(
						filePath,
						newFilePath );
				}
			}
			else
			{
				LogCentral.Current[@"ZetaLib.Core"].LogInfo(
					string.Format(
					@"Not safe-deleting file '{0}', " +
					@"because the file does not exist.",
					filePath ) );
			}
		}

		/// <summary>
		/// Safes the file exists.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <returns></returns>
		public static bool SafeFileExists(
			FileInfo filePath )
		{
			if ( filePath == null )
			{
				return false;
			}
			else
			{
				return SafeFileExists( filePath.FullName );
			}
		}

		/// <summary>
		/// Safes the file exists.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <returns></returns>
		public static bool SafeFileExists(
			string filePath )
		{
			if ( string.IsNullOrEmpty( filePath ) )
			{
				return false;
			}
			else
			{
				return File.Exists( filePath );
			}
		}

		/// <summary>
		/// Safes the move file.
		/// </summary>
		/// <param name="sourcePath">The source path.</param>
		/// <param name="dstFilePath">The DST file path.</param>
		public static void SafeMoveFile(
			FileInfo sourcePath,
			string dstFilePath )
		{
			SafeMoveFile(
				sourcePath == null ? null : sourcePath.FullName,
				dstFilePath );
		}

		/// <summary>
		/// Safes the move file.
		/// </summary>
		/// <param name="sourcePath">The source path.</param>
		/// <param name="dstFilePath">The DST file path.</param>
		public static void SafeMoveFile(
			string sourcePath,
			FileInfo dstFilePath )
		{
			SafeMoveFile(
				sourcePath,
				dstFilePath == null ? null : dstFilePath.FullName );
		}

		/// <summary>
		/// Safes the move file.
		/// </summary>
		/// <param name="sourcePath">The source path.</param>
		/// <param name="dstFilePath">The DST file path.</param>
		public static void SafeMoveFile(
			FileInfo sourcePath,
			FileInfo dstFilePath )
		{
			SafeMoveFile(
				sourcePath == null ? null : sourcePath.FullName,
				dstFilePath == null ? null : dstFilePath.FullName );
		}

		/// <summary>
		/// Safes the move file.
		/// </summary>
		/// <param name="sourcePath">The source path.</param>
		/// <param name="dstFilePath">The DST file path.</param>
		public static void SafeMoveFile(
			string sourcePath,
			string dstFilePath )
		{
			LogCentral.Current[@"ZetaLib.Core"].LogInfo(
				string.Format(
				@"About to safe-move file from '{0}' to '{1}'.",
				sourcePath,
				dstFilePath ) );

			if ( sourcePath == null || dstFilePath == null )
			{
				LogCentral.Current[@"ZetaLib.Core"].LogInfo(
					string.Format(
					@"Source file path or destination file path does not exist. " +
					@"Not moving."
					) );
			}
			else
			{
				if ( SafeFileExists( sourcePath ) )
				{
					SafeDeleteFile( dstFilePath );

					string d = Path.GetDirectoryName( dstFilePath );

					if ( !Directory.Exists( d ) )
					{
						LogCentral.Current[@"ZetaLib.Core"].LogInfo(
							string.Format(
							@"Creating non-existing folder '{0}'.",
							d ) );
						Directory.CreateDirectory( d );
					}

					File.Move( sourcePath, dstFilePath );
				}
				else
				{
					LogCentral.Current[@"ZetaLib.Core"].LogInfo(
						string.Format(
						@"Source file path to move does not exist: '{0}'.",
						sourcePath ) );
				}
			}
		}

		/// <summary>
		/// Safes the copy file.
		/// </summary>
		/// <param name="sourcePath">The source path.</param>
		/// <param name="dstFilePath">The DST file path.</param>
		/// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
		public static void SafeCopyFile(
			string sourcePath,
			string dstFilePath,
			bool overwrite )
		{
			LogCentral.Current[@"ZetaLib.Core"].LogInfo(
				string.Format(
				@"About to safe-copy file from '{0}' to '{1}' " +
				@"with overwrite = '{2}'.",
				sourcePath,
				dstFilePath,
				overwrite ) );

			if ( sourcePath == null || dstFilePath == null )
			{
				LogCentral.Current[@"ZetaLib.Core"].LogInfo(
					string.Format(
					@"Source file path or destination file path does not exist. " +
					@"Not copying."
					) );
			}
			else
			{
				if ( string.Compare( sourcePath, dstFilePath, true ) == 0 )
				{
					LogCentral.Current[@"ZetaLib.Core"].LogInfo(
						string.Format(
						@"Source path and destination path are the same: " +
						@"'{0}' is '{1}'. Not copying.",
						sourcePath,
						dstFilePath ) );
				}
				else
				{
					if ( SafeFileExists( sourcePath ) )
					{
						if ( overwrite )
						{
							SafeDeleteFile( dstFilePath );
						}

						string d = Path.GetDirectoryName( dstFilePath );

						if ( !Directory.Exists( d ) )
						{
							LogCentral.Current[@"ZetaLib.Core"].LogInfo(
								string.Format(
								@"Creating non-existing folder '{0}'.",
								d ) );
							Directory.CreateDirectory( d );
						}

						File.Copy( sourcePath, dstFilePath, overwrite );
					}
					else
					{
						LogCentral.Current[@"ZetaLib.Core"].LogInfo(
							string.Format(
							@"Source file path to copy does not exist: '{0}'.",
							sourcePath ) );
					}
				}
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Don't instantiate.
		/// </summary>
		private SafeFileOperations()
		{
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}