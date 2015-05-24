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

	// ----------------------------------------------------------------------
	#endregion

	/////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Class for scaling an image.
	/// </summary>
	public class ImageScaler
	{
		#region Checking for reducing size.
		// ------------------------------------------------------------------

		/// <summary>
		/// Check whether need to scale.
		/// Only the X dimension is specified. Y is ignored.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="maxWidth">Width of the max.</param>
		/// <returns></returns>
		public static bool NeedReduceSizeProportionallyX(
			Image image,
			int maxWidth )
		{
			return NeedReduceSizeProportionally(
				image,
				maxWidth,
				int.MaxValue );
		}

		/// <summary>
		/// Check whether need to scale.
		/// Only the Y dimension is specified. X is ignored.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="maxHeight">Height of the max.</param>
		/// <returns></returns>
		public static bool NeedReduceSizeProportionallyY(
			Image image,
			int maxHeight )
		{
			return NeedReduceSizeProportionally(
				image,
				int.MaxValue,
				maxHeight );
		}

		/// <summary>
		/// Check whether need to scale.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="maxWidth">Width of the max.</param>
		/// <param name="maxHeight">Height of the max.</param>
		/// <returns></returns>
		public static bool NeedReduceSizeProportionally(
			Image image,
			int maxWidth,
			int maxHeight )
		{
			if ( image==null )
			{
				return false;
			}
			else
			{
				if ( image.Width<=maxWidth &&
					image.Height<=maxHeight )
				{
					// Nothing to do.
					return false;
				}
				else
				{
					return true;
				}
			}
		}

		/// <summary>
		/// Check whether need to scale.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="maxWidth">Width of the max.</param>
		/// <param name="maxHeight">Height of the max.</param>
		/// <returns></returns>
		public static bool NeedReducePixelSizeProportionally(
			Image image,
			int maxWidth,
			int maxHeight )
		{
			if ( image==null )
			{
				return false;
			}
			else
			{
				if ( image.Width<=maxWidth &&
					image.Height<=maxHeight )
				{
					// Nothing to do.
					return false;
				}
				else
				{
					return true;
				}
			}
		}

		/// <summary>
		/// Check whether need to scale.
		/// Only the X dimension is specified. Y is ignored.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="maxWidth">Width of the max.</param>
		/// <returns></returns>
		public static bool NeedReducePixelSizeProportionallyX(
			Image image,
			int maxWidth )
		{
			return NeedReducePixelSizeProportionally(
				image,
				maxWidth,
				int.MaxValue );
		}

		/// <summary>
		/// Check whether need to scale.
		/// Only the Y dimension is specified. X is ignored.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="maxHeight">Height of the max.</param>
		/// <returns></returns>
		public static bool NeedReducePixelSizeProportionallyY(
			Image image,
			int maxHeight )
		{
			return NeedReducePixelSizeProportionally(
				image,
				int.MaxValue,
				maxHeight );
		}

		// ------------------------------------------------------------------
		#endregion

		#region Reducing size.
		// ------------------------------------------------------------------

		/// <summary>
		/// If the image is larger than the specified
		/// dimensions, it is proportionally scaled.
		/// If the image is smaller, nothing happens.
		/// Only the X dimension is specified. Y is ignored.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="maxWidth">Width of the max.</param>
		/// <param name="dpi">The dpi.</param>
		/// <returns></returns>
		public static Image ReduceSizeProportionallyX(
			Image image,
			int maxWidth,
			int dpi )
		{
			return ReduceSizeProportionally(
				image,
				maxWidth,
				int.MaxValue,
				dpi );
		}

		/// <summary>
		/// If the image is larger than the specified
		/// dimensions, it is proportionally scaled.
		/// If the image is smaller, nothing happens.
		/// Only the Y dimension is specified. X is ignored.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="maxHeight">Height of the max.</param>
		/// <param name="dpi">The dpi.</param>
		/// <returns></returns>
		public static Image ReduceSizeProportionallyY(
			Image image,
			int maxHeight,
			int dpi )
		{
			return ReduceSizeProportionally(
				image,
				int.MaxValue,
				maxHeight,
				dpi );
		}

		/// <summary>
		/// If the image is larger than the specified
		/// dimensions, it is proportionally scaled.
		/// If the image is smaller, nothing happens.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="maxWidth">Width of the max.</param>
		/// <param name="maxHeight">Height of the max.</param>
		/// <param name="dpi">The dpi.</param>
		/// <returns></returns>
		public static Image ReduceSizeProportionally(
			Image image,
			int maxWidth,
			int maxHeight,
			int dpi )
		{
			if ( NeedReduceSizeProportionally( image, maxWidth, maxHeight ) )
			{
				// The factors in x and y.
				double facX = ((double)maxWidth) / ((double)image.Width);
				double facY = ((double)maxHeight) / ((double)image.Height);

				// Select factor.
				double fac;
				if ( facX<facY )
				{
					fac = facX;
				}
				else
				{
					fac = facY;
				}

				return ScaleImage( 
					image,
					(int)(image.Width*fac), 
					(int)(image.Height*fac),
					dpi );
			}
			else
			{
				return image;
			}
		}

		/// <summary>
		/// If the image is larger than the specified
		/// dimensions, it is proportionally scaled.
		/// If the image is smaller, nothing happens.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="maxWidth">Width of the max.</param>
		/// <param name="maxHeight">Height of the max.</param>
		/// <returns></returns>
		public static Image ReduceSizeProportionally(
			Image image,
			int maxWidth,
			int maxHeight )
		{
			if ( NeedReduceSizeProportionally( image, maxWidth, maxHeight ) )
			{
				// The factors in x and y.
				double facX = ((double)maxWidth) / ((double)image.Width);
				double facY = ((double)maxHeight) / ((double)image.Height);

				// Select factor.
				double fac;
				if ( facX<facY )
				{
					fac = facX;
				}
				else
				{
					fac = facY;
				}

				return PixelScaleImage( 
					image,
					(int)(image.Width*fac), 
					(int)(image.Height*fac) );
			}
			else
			{
				return image;
			}
		}

		/// <summary>
		/// If the image is larger than the specified
		/// dimensions, it is proportionally scaled.
		/// If the image is smaller, nothing happens.
		/// Only the X dimension is specified. Y is ignored.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="maxWidth">Width of the max.</param>
		/// <returns></returns>
		public static Image ReducePixelSizeProportionallyX(
			Image image,
			int maxWidth )
		{
			return ReducePixelSizeProportionally(
				image,
				maxWidth,
				int.MaxValue );
		}

		/// <summary>
		/// If the image is larger than the specified
		/// dimensions, it is proportionally scaled.
		/// If the image is smaller, nothing happens.
		/// Only the Y dimension is specified. X is ignored.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="maxHeight">Height of the max.</param>
		/// <returns></returns>
		public static Image ReducePixelSizeProportionallyY(
			Image image,
			int maxHeight )
		{
			return ReducePixelSizeProportionally(
				image,
				int.MaxValue,
				maxHeight );
		}

		/// <summary>
		/// If the image is larger than the specified
		/// dimensions, it is proportionally scaled.
		/// If the image is smaller, nothing happens.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="maxWidth">Width of the max.</param>
		/// <param name="maxHeight">Height of the max.</param>
		/// <returns></returns>
		public static Image ReducePixelSizeProportionally(
			Image image,
			int maxWidth,
			int maxHeight )
		{
			if ( NeedReducePixelSizeProportionally( image, maxWidth, maxHeight ) )
			{
				// The factors in x and y.
				double facX = ((double)maxWidth) / ((double)image.Width);
				double facY = ((double)maxHeight) / ((double)image.Height);

				// Select factor.
				double fac;
				if ( facX<facY )
				{
					fac = facX;
				}
				else
				{
					fac = facY;
				}

				return PixelScaleImage( 
					image,
					(int)(image.Width*fac), 
					(int)(image.Height*fac) );
			}
			else
			{
				return image;
			}
		}

		// ------------------------------------------------------------------
		#endregion

		#region Scaling.
		// ------------------------------------------------------------------

		/// <summary>
		/// Scale the given image to the given size.
		/// Uses a high interpolation mode quality.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="dpi">The dpi.</param>
		/// <returns></returns>
		public static Image ScaleImage( 
			Image image, 
			int width, 
			int height,
			int dpi )
		{
			return ScaleImage(
				image,
				width,
				height,
				dpi,
				InterpolationMode.HighQualityBicubic );
		}

		/// <summary>
		/// Scale the given image to the given size.
		/// Uses the specifiied modes.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="dpi">The dpi.</param>
		/// <param name="ipm">The ipm.</param>
		/// <returns></returns>
		public static Image ScaleImage( 
			Image image, 
			int width, 
			int height,
			int dpi,
			InterpolationMode ipm )
		{
			// NO "using" here, because the image is returned (and
			// would be disposed by "using")!
			Bitmap dst = new Bitmap( image, width, height );

			dst.SetResolution( (float)dpi, (float)dpi );

			using ( Graphics g = Graphics.FromImage( dst ) )
			{
				// White background.
				g.FillRectangle( Brushes.White, 0, 0, width, height );

				g.InterpolationMode = ipm;

				g.DrawImage( 
					image, 
					new Rectangle( 
					0, 
					0, 
					width, 
					height ),
					0,
					0,
					image.Width,
					image.Height,
					GraphicsUnit.Pixel );

				return dst;
			}
		}

		/// <summary>
		/// Scale the given image to the given size.
		/// Uses a high interpolation mode quality.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <returns></returns>
		public static Image PixelScaleImage( 
			Image image, 
			int width, 
			int height )
		{
			return PixelScaleImage(
				image,
				width,
				height,
				InterpolationMode.HighQualityBicubic );
		}

		/// <summary>
		/// Scale the given image to the given size.
		/// Uses the specifiied modes.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="ipm">The ipm.</param>
		/// <returns></returns>
		public static Image PixelScaleImage( 
			Image image, 
			int width, 
			int height,
			InterpolationMode ipm )
		{
			// NO "using" here, because the image is returned (and
			// would be disposed by "using")!
			Bitmap dst = new Bitmap( image, width, height );

			using ( Graphics g = Graphics.FromImage( dst ) )
			{
				// White background.
				g.FillRectangle( Brushes.White, 0, 0, width, height );

				g.InterpolationMode = ipm;

				g.DrawImage( 
					image, 
					new Rectangle( 
					0, 
					0, 
					width, 
					height ),
					0,
					0,
					image.Width,
					image.Height,
					GraphicsUnit.Pixel );

				return dst;
			}
		}

		/// <summary>
		/// Use the internal thumbnail functionality.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <returns></returns>
		public static Image ScaleImageTn( 
			Image image, 
			int width, 
			int height )
		{
			return image.GetThumbnailImage( width, height, null, IntPtr.Zero );
		}

		// ------------------------------------------------------------------
		#endregion
	}

	/////////////////////////////////////////////////////////////////////////
}