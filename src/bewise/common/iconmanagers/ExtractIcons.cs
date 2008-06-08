using System;
using System.Drawing;
using BeWise.Common.Utils;
using System.Runtime.InteropServices;

namespace BeWise.Common.IconManagers {
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
			WinApi.SHFILEINFO info = new WinApi.SHFILEINFO(true);
			int cbFileInfo = Marshal.SizeOf(info);
			WinApi.SHGFI flags = WinApi.SHGFI.SHGFI_ICON | WinApi.SHGFI.SHGFI_LARGEICON;
			if (selected) {
				flags |= WinApi.SHGFI.SHGFI_OPENICON;
			}
			if (small) {
				flags |= WinApi.SHGFI.SHGFI_SMALLICON;
			}
			WinApi.SHGetFileInfo(path, 0, out info, (uint)cbFileInfo, flags);
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
		public static Icon DesktopIcon
		{
			get
			{
				IntPtr[] handlesIconLarge = new IntPtr[1];
				IntPtr[] handlesIconSmall = new IntPtr[1];
				uint code = WinApi.ExtractIconEx(Environment.SystemDirectory + "\\shell32.dll", 34, handlesIconLarge, handlesIconSmall, 1);
				if (code != 0) {
					return null;	
				}
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
}

