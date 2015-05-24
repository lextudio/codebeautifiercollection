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
	/// Miscellaneous helper functions for dealing with folders.
	/// </summary>
	public sealed class DirectoryHelper
	{
		#region Public miscellaneous methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Checks whether a given folder is empty.
		/// </summary>
		/// <param name="folder">The folder.</param>
		/// <returns>
		/// 	<c>true</c> if the specified folder is empty; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsEmpty(
			DirectoryInfo folder )
		{
			return IsEmpty( folder.FullName );
		}

		/// <summary>
		/// Checks whether a given folder is empty.
		/// </summary>
		/// <param name="folder">The folder.</param>
		/// <returns>
		/// 	<c>true</c> if the specified folder is empty; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsEmpty(
			FileOrDirectoryInfo folder )
		{
			return IsEmpty( folder.FullName );
		}

		/// <summary>
		/// Checks whether a given folder is empty.
		/// </summary>
		/// <param name="folderPath">The folder path.</param>
		/// <returns>
		/// 	<c>true</c> if the specified folder path is empty; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsEmpty(
			string folderPath )
		{
			return
				Directory.GetFiles( folderPath ).Length <= 0 &&
				Directory.GetDirectories( folderPath ).Length <= 0;
		}

		/// <summary>
		/// Deep-deletes the contents, but not the folder itself.
		/// </summary>
		/// <param name="folderPath">The folder path.</param>
		public static void DeleteDirectoryContents(
			string folderPath )
		{
			DirectoryInfo info = new DirectoryInfo( folderPath );

			foreach ( FileInfo filePath in info.GetFiles() )
			{
				filePath.Delete();
			}

			foreach ( DirectoryInfo directoryPath in info.GetDirectories() )
			{
				directoryPath.Delete();
			}
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}