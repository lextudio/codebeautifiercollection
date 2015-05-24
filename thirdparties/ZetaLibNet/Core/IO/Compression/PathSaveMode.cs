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
	/// How paths are stored when packing files to an archive.
	/// </summary>
	public enum PathSaveMode
	{
		#region Enum members.
		// ------------------------------------------------------------------

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

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}