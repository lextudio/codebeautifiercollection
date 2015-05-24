// this is the image loader class.
// Copyright (C) 2005-2006  Lex Y. Li
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
using System;
using System.IO;
using System.Drawing;
using Lextm.Diagnostics;
using System.Globalization;

namespace Lextm.Drawing {


	///<summary>
	///Bitmap and icon loader.
	///</summary>
	/// <version>1</version>
	/// <since>2006-1-17</since>
	/// <author>lextm</author>
	public sealed class ImageLoader {

        ///<summary>
		///Loads an image for an add-on menu
		///</summary>
		///<param name="imageName">Name (without extension)</param>
		///<returns>
		///The image data IntPtr if loaded or IntPtr.Zero if not.
		///</returns>
        public static IntPtr GetImageDataPtr(string imageName)
        {
            string ico = imageName + ".ico";
            if (File.Exists(ico))
            {
                return new Icon(ico).ToBitmap().GetHbitmap();
            }
            string bmp = imageName + ".bmp";
            if (File.Exists(bmp))
            {
                return new Bitmap(bmp).GetHbitmap();
            }
            string png = imageName + ".png";
            if (File.Exists(png))
            {
                return new Bitmap(Bitmap.FromFile(png)).GetHbitmap();
            }
            LoggingService.Warn("fail to find an image.");
            return IntPtr.Zero;
        }

		private ImageLoader( ) {
		}
	}
}
