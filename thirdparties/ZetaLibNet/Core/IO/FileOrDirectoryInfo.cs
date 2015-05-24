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
	/// Wraps a FileInfo and a DirectoryInfo in one class.
	/// </summary>
	public sealed class FileOrDirectoryInfo
	{
		#region Public static methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Compares the specified one.
		/// </summary>
		/// <param name="one">The one.</param>
		/// <param name="two">The two.</param>
		/// <returns></returns>
		public static int Compare(
			DirectoryInfo one,
			DirectoryInfo two )
		{
			return string.Compare(
				one.FullName.TrimEnd( '\\' ),
				two.FullName.TrimEnd( '\\' ),
				true );
		}

		/// <summary>
		/// Compares the specified one.
		/// </summary>
		/// <param name="one">The one.</param>
		/// <param name="two">The two.</param>
		/// <returns></returns>
		public static int Compare(
			FileInfo one,
			FileInfo two )
		{
			return string.Compare(
				one.FullName.TrimEnd( '\\' ),
				two.FullName.TrimEnd( '\\' ),
				true );
		}

		/// <summary>
		/// Compares the specified one.
		/// </summary>
		/// <param name="one">The one.</param>
		/// <param name="two">The two.</param>
		/// <returns></returns>
		public static int Compare(
			FileOrDirectoryInfo one,
			FileOrDirectoryInfo two )
		{
			return string.Compare(
				one.FullName.TrimEnd( '\\' ),
				two.FullName.TrimEnd( '\\' ),
				true );
		}

		/// <summary>
		/// Compares the specified one.
		/// </summary>
		/// <param name="one">The one.</param>
		/// <param name="two">The two.</param>
		/// <returns></returns>
		public static int Compare(
			FileOrDirectoryInfo one,
			FileInfo two )
		{
			return string.Compare(
				one.FullName.TrimEnd( '\\' ),
				two.FullName.TrimEnd( '\\' ),
				true );
		}

		/// <summary>
		/// Compares the specified one.
		/// </summary>
		/// <param name="one">The one.</param>
		/// <param name="two">The two.</param>
		/// <returns></returns>
		public static int Compare(
			FileOrDirectoryInfo one,
			DirectoryInfo two )
		{
			return string.Compare(
				one.FullName.TrimEnd( '\\' ),
				two.FullName.TrimEnd( '\\' ),
				true );
		}

		/// <summary>
		/// Compares the specified one.
		/// </summary>
		/// <param name="one">The one.</param>
		/// <param name="two">The two.</param>
		/// <returns></returns>
		public static int Compare(
			string one,
			DirectoryInfo two )
		{
			return new FileOrDirectoryInfo( one ).Compare( two );
		}

		/// <summary>
		/// Compares the specified one.
		/// </summary>
		/// <param name="one">The one.</param>
		/// <param name="two">The two.</param>
		/// <returns></returns>
		public static int Compare(
			string one,
			FileInfo two )
		{
			return new FileOrDirectoryInfo( one ).Compare( two );
		}

		/// <summary>
		/// Compares the specified one.
		/// </summary>
		/// <param name="one">The one.</param>
		/// <param name="two">The two.</param>
		/// <returns></returns>
		public static int Compare(
			string one,
			FileOrDirectoryInfo two )
		{
			return new FileOrDirectoryInfo( one ).Compare( two );
		}

		/// <summary>
		/// Compares the specified one.
		/// </summary>
		/// <param name="one">The one.</param>
		/// <param name="two">The two.</param>
		/// <returns></returns>
		public static int Compare(
			DirectoryInfo one,
			string two )
		{
			return new FileOrDirectoryInfo( one ).Compare( two );
		}

		/// <summary>
		/// Compares the specified one.
		/// </summary>
		/// <param name="one">The one.</param>
		/// <param name="two">The two.</param>
		/// <returns></returns>
		public static int Compare(
			FileInfo one,
			string two )
		{
			return new FileOrDirectoryInfo( one ).Compare( two );
		}

		/// <summary>
		/// Compares the specified one.
		/// </summary>
		/// <param name="one">The one.</param>
		/// <param name="two">The two.</param>
		/// <returns></returns>
		public static int Compare(
			FileOrDirectoryInfo one,
			string two )
		{
			return new FileOrDirectoryInfo( one ).Compare( two );
		}

		/// <summary>
		/// Compares the specified one.
		/// </summary>
		/// <param name="one">The one.</param>
		/// <param name="two">The two.</param>
		/// <returns></returns>
		public static int Compare(
			string one,
			string two )
		{
			return new FileOrDirectoryInfo( one ).Compare( two );
		}

		/// <summary>
		/// Combines the specified one.
		/// </summary>
		/// <param name="one">The one.</param>
		/// <param name="two">The two.</param>
		/// <returns></returns>
		public static FileOrDirectoryInfo Combine(
			DirectoryInfo one,
			DirectoryInfo two )
		{
			return new FileOrDirectoryInfo( one ).Combine( two );
		}

		/// <summary>
		/// Combines the specified one.
		/// </summary>
		/// <param name="one">The one.</param>
		/// <param name="two">The two.</param>
		/// <returns></returns>
		public static FileOrDirectoryInfo Combine(
			FileInfo one,
			FileInfo two )
		{
			return new FileOrDirectoryInfo( one ).Combine( two );
		}

		/// <summary>
		/// Combines the specified one.
		/// </summary>
		/// <param name="one">The one.</param>
		/// <param name="two">The two.</param>
		/// <returns></returns>
		public static FileOrDirectoryInfo Combine(
			FileOrDirectoryInfo one,
			FileOrDirectoryInfo two )
		{
			return new FileOrDirectoryInfo( one ).Combine( two );
		}

		/// <summary>
		/// Combines the specified one.
		/// </summary>
		/// <param name="one">The one.</param>
		/// <param name="two">The two.</param>
		/// <returns></returns>
		public static FileOrDirectoryInfo Combine(
			FileOrDirectoryInfo one,
			FileInfo two )
		{
			return new FileOrDirectoryInfo( one ).Combine( two );
		}

		/// <summary>
		/// Combines the specified one.
		/// </summary>
		/// <param name="one">The one.</param>
		/// <param name="two">The two.</param>
		/// <returns></returns>
		public static FileOrDirectoryInfo Combine(
			FileOrDirectoryInfo one,
			DirectoryInfo two )
		{
			return new FileOrDirectoryInfo( one ).Combine( two );
		}

		/// <summary>
		/// Combines the specified one.
		/// </summary>
		/// <param name="one">The one.</param>
		/// <param name="two">The two.</param>
		/// <returns></returns>
		public static FileOrDirectoryInfo Combine(
			string one,
			DirectoryInfo two )
		{
			return new FileOrDirectoryInfo( one ).Combine( two );
		}

		/// <summary>
		/// Combines the specified one.
		/// </summary>
		/// <param name="one">The one.</param>
		/// <param name="two">The two.</param>
		/// <returns></returns>
		public static FileOrDirectoryInfo Combine(
			string one,
			FileInfo two )
		{
			return new FileOrDirectoryInfo( one ).Combine( two );
		}

		/// <summary>
		/// Combines the specified one.
		/// </summary>
		/// <param name="one">The one.</param>
		/// <param name="two">The two.</param>
		/// <returns></returns>
		public static FileOrDirectoryInfo Combine(
			string one,
			FileOrDirectoryInfo two )
		{
			return new FileOrDirectoryInfo( one ).Combine( two );
		}

		/// <summary>
		/// Combines the specified one.
		/// </summary>
		/// <param name="one">The one.</param>
		/// <param name="two">The two.</param>
		/// <returns></returns>
		public static FileOrDirectoryInfo Combine(
			DirectoryInfo one,
			string two )
		{
			return new FileOrDirectoryInfo( one ).Combine( two );
		}

		/// <summary>
		/// Combines the specified one.
		/// </summary>
		/// <param name="one">The one.</param>
		/// <param name="two">The two.</param>
		/// <returns></returns>
		public static FileOrDirectoryInfo Combine(
			FileInfo one,
			string two )
		{
			return new FileOrDirectoryInfo( one ).Combine( two );
		}

		/// <summary>
		/// Combines the specified one.
		/// </summary>
		/// <param name="one">The one.</param>
		/// <param name="two">The two.</param>
		/// <returns></returns>
		public static FileOrDirectoryInfo Combine(
			FileOrDirectoryInfo one,
			string two )
		{
			return new FileOrDirectoryInfo( one ).Combine( two );
		}

		/// <summary>
		/// Combines the specified one.
		/// </summary>
		/// <param name="one">The one.</param>
		/// <param name="two">The two.</param>
		/// <returns></returns>
		public static FileOrDirectoryInfo Combine(
			string one,
			string two )
		{
			return new FileOrDirectoryInfo( one ).Combine( two );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Constructors.
		// ------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		public FileOrDirectoryInfo()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="fullPath">The full path.</param>
		public FileOrDirectoryInfo(
			string fullPath )
		{
			this.fullPath = fullPath;
			this.originalPath = fullPath;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="info">The info.</param>
		public FileOrDirectoryInfo(
			FileOrDirectoryInfo info )
		{
			this.fullPath = info.FullName;
			this.originalPath = info.originalPath;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="info">The info.</param>
		public FileOrDirectoryInfo(
			DirectoryInfo info )
		{
			this.fullPath = info.FullName;
			this.originalPath = info.ToString();
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="info">The info.</param>
		public FileOrDirectoryInfo(
			FileInfo info )
		{
			this.fullPath = info.FullName;
			this.originalPath = info.ToString();
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// Checks whether the contained file name or folder name is empty.
		/// </summary>
		/// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
		public bool IsEmpty
		{
			get
			{
				return string.IsNullOrEmpty( fullPath );
			}
		}

		/// <summary>
		/// Access as a FileInfo object.
		/// </summary>
		/// <value>The file.</value>
		public FileInfo File
		{
			get
			{
				return new FileInfo( fullPath );
			}
		}

		/// <summary>
		/// Access as a DirectoryInfo object.
		/// </summary>
		/// <value>The directory.</value>
		public DirectoryInfo Directory
		{
			get
			{
				return new DirectoryInfo( fullPath );
			}
		}

		/// <summary>
		/// Get a value indicating whether the file or the directory exists.
		/// </summary>
		/// <value><c>true</c> if exists; otherwise, <c>false</c>.</value>
		public bool Exists
		{
			get
			{
				return
					!string.IsNullOrEmpty( fullPath ) &&
					(File.Exists || Directory.Exists);
			}
		}

		/// <summary>
		/// Gets the full name.
		/// </summary>
		/// <value>The full name.</value>
		public string FullName
		{
			get
			{
				return fullPath;
			}
		}

		/// <summary>
		/// Gets the original path.
		/// </summary>
		/// <value>The original path.</value>
		public string OriginalPath
		{
			get
			{
				return originalPath;
			}
		}

		/// <summary>
		/// Splits a path into its parts.
		/// </summary>
		/// <value>The splitted path.</value>
		public SplittedPath SplittedPath
		{
			get
			{
				return new SplittedPath( this );
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is file.
		/// </summary>
		/// <value><c>true</c> if this instance is file; otherwise, <c>false</c>.</value>
		public bool IsFile
		{
			get
			{
				return File.Exists;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is directory.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is directory; otherwise, <c>false</c>.
		/// </value>
		public bool IsDirectory
		{
			get
			{
				return Directory.Exists;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Public methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Returns a <see cref="T:System.String"></see> that represents the 
		/// current <see cref="T:System.Object"></see>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"></see> that represents the current
		/// <see cref="T:System.Object"></see>.
		/// </returns>
		public override string ToString()
		{
			if ( string.IsNullOrEmpty( originalPath ) )
			{
				return fullPath;
			}
			else
			{
				return originalPath;
			}
		}

		/// <summary>
		/// Combines the specified info.
		/// </summary>
		/// <param name="info">The info.</param>
		/// <returns></returns>
		public FileOrDirectoryInfo Combine(
			FileOrDirectoryInfo info )
		{
			return
				new FileOrDirectoryInfo(
				Path.Combine(
				fullPath,
				info.FullName ) );
		}

		/// <summary>
		/// Combines the specified path.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns></returns>
		public FileOrDirectoryInfo Combine(
			string path )
		{
			return
				new FileOrDirectoryInfo(
				Path.Combine(
				fullPath,
				path ) );
		}

		/// <summary>
		/// Combines the specified info.
		/// </summary>
		/// <param name="info">The info.</param>
		/// <returns></returns>
		public FileOrDirectoryInfo Combine(
			FileInfo info )
		{
			return
				new FileOrDirectoryInfo(
				Path.Combine(
				fullPath,
				// According to Reflector, "ToString()" returns the 
				// "OriginalPath". This is what we need here.
				info.ToString() ) );
		}

		/// <summary>
		/// Combines the specified info.
		/// </summary>
		/// <param name="info">The info.</param>
		/// <returns></returns>
		public FileOrDirectoryInfo Combine(
			DirectoryInfo info )
		{
			return
				new FileOrDirectoryInfo(
				Path.Combine(
				fullPath,
				// According to Reflector, "ToString()" returns the 
				// "OriginalPath". This is what we need here.
				info.ToString() ) );
		}

		/// <summary>
		/// Compares the specified b.
		/// </summary>
		/// <param name="b">The b.</param>
		/// <returns></returns>
		public int Compare(
			string b )
		{
			return Compare( this, new FileOrDirectoryInfo( b ) );
		}

		/// <summary>
		/// Compares the specified b.
		/// </summary>
		/// <param name="b">The b.</param>
		/// <returns></returns>
		public int Compare(
			FileInfo b )
		{
			return Compare( b.FullName );
		}

		/// <summary>
		/// Compares the specified b.
		/// </summary>
		/// <param name="b">The b.</param>
		/// <returns></returns>
		public int Compare(
			DirectoryInfo b )
		{
			return Compare( b.FullName );
		}

		/// <summary>
		/// Compares the specified b.
		/// </summary>
		/// <param name="b">The b.</param>
		/// <returns></returns>
		public int Compare(
			FileOrDirectoryInfo b )
		{
			return Compare( b.FullName );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		private string fullPath;

		/// <summary>
		/// 
		/// </summary>
		private string originalPath;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}