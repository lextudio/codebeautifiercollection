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

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Information of a splitted path.
	/// </summary>
	public sealed class SplittedPath
	{
		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="path">The path.</param>
		public SplittedPath(
			string path )
		{
			this.info = new FileOrDirectoryInfo( path );
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="path">The path.</param>
		public SplittedPath(
			FileInfo path )
		{
			this.info = new FileOrDirectoryInfo( path );
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="path">The path.</param>
		public SplittedPath(
			DirectoryInfo path )
		{
			this.info = new FileOrDirectoryInfo( path );
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="path">The path.</param>
		public SplittedPath(
			FileOrDirectoryInfo path )
		{
			this.info = new FileOrDirectoryInfo( path );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// The complete path.
		/// </summary>
		/// <value>The full path.</value>
		public string FullPath
		{
			get
			{
				return info.FullName;
			}
		}

		/// <summary>
		/// The complete path as a rich object.
		/// </summary>
		/// <value>The info.</value>
		public FileOrDirectoryInfo Info
		{
			get
			{
				return info;
			}
		}

		/// <summary>
		/// Only the drive.
		/// </summary>
		/// <value>The drive.</value>
		public string Drive
		{
			get
			{
				return PathHelper.GetDrive( info.FullName );
			}
		}

		/// <summary>
		/// Only the share.
		/// </summary>
		/// <value>The share.</value>
		public string Share
		{
			get
			{
				return PathHelper.GetShare( info.FullName );
			}
		}

		/// <summary>
		/// The drive or the share.
		/// </summary>
		/// <value>The drive or share.</value>
		public string DriveOrShare
		{
			get
			{
				return PathHelper.GetDriveOrShare( info.FullName );
			}
		}

		/// <summary>
		/// The directory parts.
		/// </summary>
		/// <value>The directory.</value>
		public string Directory
		{
			get
			{
				return PathHelper.GetDirectory( info.FullName );
			}
		}

		/// <summary>
		/// The filename without the extension.
		/// </summary>
		/// <value>The name without extension.</value>
		public string NameWithoutExtension
		{
			get
			{
				return PathHelper.GetNameWithoutExtension( info.FullName );
			}
		}

		/// <summary>
		/// The filename with the extension.
		/// </summary>
		/// <value>The name with extension.</value>
		public string NameWithExtension
		{
			get
			{
				return PathHelper.GetNameWithExtension( info.FullName );
			}
		}

		/// <summary>
		/// The extension only.
		/// </summary>
		/// <value>The extension.</value>
		public string Extension
		{
			get
			{
				return PathHelper.GetExtension( info.FullName );
			}
		}

		/// <summary>
		/// The drive or the share together with the directory parts.
		/// </summary>
		/// <value>The drive or share and directory.</value>
		public string DriveOrShareAndDirectory
		{
			get
			{
				return PathHelper.Combine( DriveOrShare, Directory );
			}
		}

		/// <summary>
		/// The drive or the share together with the directory parts
		/// and the name without extension.
		/// </summary>
		/// <value>The drive or share and directory and name without extension.</value>
		public string DriveOrShareAndDirectoryAndNameWithoutExtension
		{
			get
			{
				return
					PathHelper.Combine(
					PathHelper.Combine( DriveOrShare, Directory ),
					NameWithoutExtension );
			}
		}

		/// <summary>
		/// The directory parts together with the filename and the extension.
		/// </summary>
		/// <value>The directory and name with extension.</value>
		public string DirectoryAndNameWithExtension
		{
			get
			{
				return PathHelper.Combine( Directory, NameWithExtension );
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		private FileOrDirectoryInfo info;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}