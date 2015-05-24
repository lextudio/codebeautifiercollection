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
	/// Info class for compressing a single miscellaneous source.
	/// </summary>
	public class CompressHeterogenousInfo
	{
		#region Public enums.
		// ------------------------------------------------------------------

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

		// ------------------------------------------------------------------
		#endregion

		#region Public properties.
		// ------------------------------------------------------------------

		/// <summary>
		/// The type contained.
		/// </summary>
		/// <value>The type.</value>
		public InfoType Type
		{
			get
			{
				return type;
			}
			set
			{
				type = value;
			}
		}

		/// <summary>
		/// Depending on the InfoType.
		/// </summary>
		/// <value>The content.</value>
		public string Content
		{
			get
			{
				return content;
			}
			set
			{
				content = value;
			}
		}

		/// <summary>
		/// Depending on the InfoType.
		/// </summary>
		/// <value>The file path.</value>
		public string FilePath
		{
			get
			{
				return filePath;
			}
			set
			{
				filePath = value;
			}
		}

		/// <summary>
		/// Depending on the InfoType.
		/// </summary>
		/// <value>The bytes.</value>
		public byte[] Bytes
		{
			get
			{
				return bytes;
			}
			set
			{
				bytes = value;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private variables.
		// ------------------------------------------------------------------

		private InfoType type;
		private string content;
		private string filePath;
		private byte[] bytes;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}