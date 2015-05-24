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
	/// Allows to finer control whether certain files/folders are
	/// added to the ZIP file.
	/// </summary>
	public interface IZipEventSink
	{
		#region Interface members.
		// ------------------------------------------------------------------

		/// <summary>
		/// Return FALSE to prevent adding of the file.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="filePathToAdd">The file path to add.</param>
		/// <returns></returns>
		bool OnBeforeAddFile(
			CompressHeterogenousInfos sender,
			FileInfo filePathToAdd );

		/// <summary>
		/// Return FALSE to prevent entering folder.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="folderPathToEnter">The folder path to enter.</param>
		/// <returns></returns>
		bool OnBeforeEnterFolder(
			CompressHeterogenousInfos sender,
			DirectoryInfo folderPathToEnter );

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}