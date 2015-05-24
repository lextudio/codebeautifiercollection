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
	/// Miscellaneous helper functions for dealing with files.
	/// </summary>
	public sealed class FileHelper
	{
		#region Reading in blocks.
		// ------------------------------------------------------------------

		/// <summary>
		/// Reads from a stream in small blocks
		/// to enable progress-pumping.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <param name="progress">The progress.</param>
		/// <returns></returns>
		public static byte[] BlockRead(
			FileInfo filePath,
			IProgress progress )
		{
			using ( FileStream stream = new FileStream(
				filePath.FullName,
				FileMode.Open,
				FileAccess.Read,
				FileShare.Read ) )
			{
				return BlockRead(
					stream,
					0,
					(int)stream.Length,
					progress );
			}
		}

		/// <summary>
		/// Reads from a stream in small blocks
		/// to enable progress-pumping.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="progress">The progress.</param>
		/// <returns></returns>
		public static byte[] BlockRead(
			Stream stream,
			IProgress progress )
		{
			return BlockRead(
				stream,
				(int)stream.Position,
				(int)(stream.Length - stream.Position),
				progress );
		}

		/// <summary>
		/// Reads from a stream in small blocks
		/// to enable progress-pumping.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="length">The length.</param>
		/// <param name="progress">The progress.</param>
		/// <returns></returns>
		public static byte[] BlockRead(
			Stream stream,
			int length,
			IProgress progress )
		{
			return BlockRead(
				stream,
				(int)stream.Position,
				length,
				progress );
		}

		/// <summary>
		/// Reads from a stream in small blocks
		/// to enable progress-pumping.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="length">The length.</param>
		/// <param name="progress">The progress.</param>
		/// <returns></returns>
		public static byte[] BlockRead(
			Stream stream,
			int offset,
			int length,
			IProgress progress )
		{
			if ( offset != stream.Position )
			{
				stream.Seek( offset, SeekOrigin.Begin );
			}

			byte[] buffer = new byte[length];

			int bytesToRead = length;
			int index = offset;

			while ( bytesToRead > 0 )
			{
				if ( progress != null )
				{
					progress.OnProgress(
						null,
						EventArgs.Empty );
				}

				int count = Math.Min( blockSize, bytesToRead );

				stream.Read( buffer, index - offset, count );

				index += count;
				bytesToRead -= count;
			}

			return buffer;
		}

		// ------------------------------------------------------------------
		#endregion

		#region Writing in blocks.
		// ------------------------------------------------------------------

		/// <summary>
		/// Writes a buffer to a file in small blocks
		/// to enable progress-pumping.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <param name="buffer">The buffer.</param>
		/// <param name="progress">The progress.</param>
		public static void BlockWrite(
			FileInfo filePath,
			byte[] buffer,
			IProgress progress )
		{
			BlockWrite(
				filePath.FullName,
				buffer,
				0,
				buffer.Length,
				progress );
		}

		/// <summary>
		/// Writes a buffer to a file in small blocks
		/// to enable progress-pumping.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <param name="buffer">The buffer.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="length">The length.</param>
		/// <param name="progress">The progress.</param>
		public static void BlockWrite(
			string filePath,
			byte[] buffer,
			int offset,
			int length,
			IProgress progress )
		{
			if ( File.Exists( filePath ) )
			{
				File.Delete( filePath );
			}

			using ( FileStream stream = new FileStream(
				filePath,
				FileMode.Create,
				FileAccess.Write,
				FileShare.Write ) )
			{
				BlockWrite(
					stream,
					buffer,
					offset,
					length,
					progress );
			}
		}

		/// <summary>
		/// Writes a buffer to a file in small blocks
		/// to enable progress-pumping.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="buffer">The buffer.</param>
		/// <param name="progress">The progress.</param>
		public static void BlockWrite(
			Stream stream,
			byte[] buffer,
			IProgress progress )
		{
			BlockWrite(
				stream,
				buffer,
				0,
				buffer.Length,
				progress );
		}

		/// <summary>
		/// Writes a buffer to a file in small blocks
		/// to enable progress-pumping.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="buffer">The buffer.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="length">The length.</param>
		/// <param name="progress">The progress.</param>
		public static void BlockWrite(
			Stream stream,
			byte[] buffer,
			int offset,
			int length,
			IProgress progress )
		{
			int index = offset;
			int bytesToWrite = length;

			while ( bytesToWrite > 0 )
			{
				if ( progress != null )
				{
					progress.OnProgress(
						null,
						EventArgs.Empty );
				}

				int count = Math.Min( blockSize, bytesToWrite );

				stream.Write( buffer, index, count );

				index += count;
				bytesToWrite -= count;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region File version information.
		// ------------------------------------------------------------------

		/// <summary>
		/// Get the version information of a file.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <returns>
		/// Returns the version information or an empty Version
		/// object if no version information is present.
		/// </returns>
		public static Version GetFileVersion(
			string filePath )
		{
			FileVersionInfo fvi =
				FileVersionInfo.GetVersionInfo( filePath );

			return new Version( fvi.FileVersion );
		}

		/// <summary>
		/// Gets the assembly version info of an assembly file.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <returns></returns>
		public static Version GetAssemblyVersion(
			string filePath )
		{
			Assembly a = Assembly.LoadFile( filePath );
			return a.GetName().Version;
		}

		// ------------------------------------------------------------------
		#endregion

		#region Private member.
		// ------------------------------------------------------------------

		/// <summary>
		/// The size of one block.
		/// </summary>
		private const int blockSize = 4096;

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}