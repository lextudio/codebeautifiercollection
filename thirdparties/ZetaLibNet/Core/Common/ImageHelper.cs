namespace ZetaLib.Core.Common
{
	#region Using directives.
	// ----------------------------------------------------------------------

	using System;
	using System.Collections;
	using System.Diagnostics;
	using System.IO;
	using System.Drawing;
	using System.Drawing.Imaging;
	using System.Drawing.Drawing2D;
	using ZetaLib.Core.Logging;

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Miscellaneous routines for handling GDI+ images.
	/// </summary>
	public class ImageHelper
	{
		#region Miscellaneous methods.
		// ------------------------------------------------------------------

		/// <summary>
		/// Tries to detect the file extension for a given image.
		/// </summary>
		/// <param name="image">The image to detect the file extension for.</param>
		/// <returns>
		/// Returns ".png" as the default if cannot detect.
		/// </returns>
		public static string SuggestFileExtension(
			Image image )
		{
			// TODO.
			return @".png";
		}

		/// <summary>
		/// Provides a file-locking-safe alternative to Image.FromFile().
		/// See http://support.microsoft.com/kb/311754/EN-US/ for details.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <returns>Returns the loaded image.</returns>
		public static Image LoadImage(
			FileInfo filePath )
		{
			return LoadImage( filePath.FullName );
		}

		/// <summary>
		/// Provides a file-locking-safe alternative to Image.FromFile().
		/// See http://support.microsoft.com/kb/311754/EN-US/ for details.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <returns>Returns the loaded image.</returns>
		public static Image LoadImage(
			string filePath )
		{
			using ( FileStream fs = new FileStream(
				filePath,
				FileMode.Open,
				FileAccess.Read ) )
			using ( BinaryReader br = new BinaryReader(
				fs ) )
			{
				byte[] buffer = new byte[fs.Length];
				br.Read( buffer, 0, (int)fs.Length );

				// The documentation says "...You must keep the stream open 
				// for the lifetime of the Image object...".
				// Therefore read into memory ad keep the memory stream open.
				// I.e. use NO "using" directive here.
				MemoryStream ms = new MemoryStream( buffer );
				return Image.FromStream( ms );
			}
		}

		/// <summary>
		/// Generic saving function. Correctly handles file extension and
		/// the associated image format.
		/// </summary>
		/// <param name="image">The image to save.</param>
		/// <param name="filePath">The file path.</param>
		/// <returns>
		/// Since the function could change the file extension, it returns the
		/// newly written path.
		/// </returns>
		public static FileInfo SaveImage(
			Image image,
			FileInfo filePath )
		{
			return new FileInfo( SaveImage( image, filePath.FullName ) );
		}

		/// <summary>
		/// Generic saving function. Correctly handles file extension and
		/// the associated image format.
		/// </summary>
		/// <param name="image">The image to save.</param>
		/// <param name="filePath">The file path.</param>
		/// <returns>
		/// Since the function could change the file extension, it returns the
		/// newly written path.
		/// </returns>
		public static string SaveImage(
			Image image,
			string filePath )
		{
			string initialFilePath = filePath;

			LogCentral.Current.LogDebug(
				string.Format(
				@"About to save image to file path '{0}'...",
				filePath ) );

			string ext = Path.GetExtension(
				filePath ).Trim( '.' ).ToLowerInvariant();

			ImageFormat format = ImageFormat.Png;
			switch ( ext )
			{
				case @"bmp":
					format = ImageFormat.Bmp;
					break;

				case @"png":
					format = ImageFormat.Png;
					break;

				case @"gif":
					format = ImageFormat.Gif;
					break;

				case @"jpg":
				case @"jpeg":
					format = ImageFormat.Jpeg;
					break;

				default:
					LogCentral.Current.LogWarn(
						string.Format(
						@"Unknown file format extension '{0}'. Using PNG instead.",
						ext ) );
					format = ImageFormat.Png;
					break;
			}

			if ( string.Compare( filePath, initialFilePath, true ) != 0 )
			{
				LogCentral.Current.LogDebug(
					string.Format(
					@"Changed image file path path from '{0}' to '{1}'.",
					initialFilePath,
					filePath ) );
			}

			// --

			if ( File.Exists( filePath ) )
			{
				File.Delete( filePath );
			}

			// Use NO "using" directive here.
			MemoryStream ms = new MemoryStream();
			image.Save( ms, format );

			using ( FileStream fs = new FileStream(
				filePath,
				FileMode.CreateNew,
				FileAccess.ReadWrite ) )
			using ( BinaryWriter bw = new BinaryWriter(
				fs ) )
			{
				byte[] buffer = new byte[ms.Length];
				ms.Seek( 0, SeekOrigin.Begin );
				ms.Read( buffer, 0, buffer.Length );

				bw.Write( buffer );
			}

			LogCentral.Current.LogDebug(
				string.Format(
				@"Successfully saved image to file path '{0}'.",
				filePath ) );

			return filePath;
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}