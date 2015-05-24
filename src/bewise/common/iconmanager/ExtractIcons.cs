using System;
using System.Drawing;
using BeWise.Common.Utils;
using System.Runtime.InteropServices;

namespace BeWise.Common.IconManager {
	/// <summary>
	/// Extract icons.
	/// </summary>
	public sealed class ExtractIcons {
		private ExtractIcons( ) { }
		/// <summary>
		/// Gets icon.
		/// </summary>
		/// <param name="path">Path</param>
		/// <param name="selected">Selected flag</param>
		/// <param name="small">small flag</param>
		/// <returns>Icon.</returns>
		public static Icon GetIcon(string path, bool selected, bool small) {
			WinAPI.SHFILEINFO info = new WinAPI.SHFILEINFO(true);
			int cbFileInfo = Marshal.SizeOf(info);
			WinAPI.SHGFI flags = WinAPI.SHGFI.SHGFI_ICON | WinAPI.SHGFI.SHGFI_LARGEICON;
			if (selected) {
				flags |= WinAPI.SHGFI.SHGFI_OPENICON;
			}
			if (small) {
				flags |= WinAPI.SHGFI.SHGFI_SMALLICON;
			}
			WinAPI.SHGetFileInfo(path, 0, out info, (uint)cbFileInfo, flags);
			Icon result = null;
			try {
				Icon dispose = Icon.FromHandle(info.hIcon);
                result = (Icon)dispose.Clone();
				DestroyIcon(dispose.Handle); // <== added to destroy the handle.
			} catch (ArgumentException) {
			}
			return result;
		}

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        extern static bool DestroyIcon(IntPtr handle);
		/// <summary>
		/// Gets desktop icon.
		/// </summary>
		/// <returns>Icon.</returns>
		/// <remarks>Retreive the desktop icon from Shell32.dll - it always appears at index 34 in all shell32 versions.
		/// This is probably NOT the best way to retreive this icon, but it works - if you have a better way
		/// by all means let me know..</remarks>
		public static Icon GetDesktopIcon() {
			IntPtr[] handlesIconLarge = new IntPtr[1];
			IntPtr[] handlesIconSmall = new IntPtr[1];
			WinAPI.ExtractIconEx(Environment.SystemDirectory + "\\shell32.dll", 34, handlesIconLarge, handlesIconSmall, 1);
			Icon result = null;
			try {
				Icon dispose = Icon.FromHandle(handlesIconSmall[0]);
                result = (Icon)dispose.Clone();
				DestroyIcon(dispose.Handle); // <== added to destroy the handle.
			} catch (ArgumentException) {
			}
			return result;
		}
	}
}

