using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace BeWise.Common.Utils {
	
	/// <summary>WinAPI wrapper.</summary>
	public sealed class WinApi {

		private WinApi() { }

		/**************************************************************/
		/*                        Const
        /**************************************************************/
		
		/// <summary>
		/// WM_KEYDOWN.
		/// </summary>
		public const int WM_KEYDOWN = 0x0100;

		/**************************************************************/
		/*                        Enum
        /**************************************************************/
		
		/// <summary>
		/// SHGFI.
		/// </summary>		
		[SuppressMessage("Microsoft.Naming", "CA1712:DoNotPrefixEnumValuesWithTypeName")]
		[SuppressMessage("Microsoft.Naming", "CA1714:FlagsEnumsShouldHavePluralNames")]
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId="SHGFI")]
		[Flags]
		public enum SHGFI
		{
			/// <summary>get icon</summary>
			SHGFI_ICON =             0x000000100,
			/// <summary>
			/// get display name
			/// </summary>
			SHGFI_DISPLAYNAME =      0x000000200,     // 
			/// <summary>
			/// get type name
			/// </summary>
			SHGFI_TYPENAME =         0x000000400,     // 
			/// <summary>
			/// get attributes
			/// </summary>
			SHGFI_ATTRIBUTES =       0x000000800,     // 
			/// <summary>
			/// get icon location
			/// </summary>
			SHGFI_ICONLOCATION =     0x000001000,     // 
			/// <summary>
			/// return exe type
			/// </summary>
			SHGFI_EXETYPE =          0x000002000,     // 
			/// <summary>
			/// get system icon index
			/// </summary>
			SHGFI_SYSICONINDEX =     0x000004000,     // 
			/// <summary>
			/// put a link overlay on icon
			/// </summary>
			SHGFI_LINKOVERLAY =      0x000008000,     // 
			/// <summary>
			/// show icon in selected state
			/// </summary>
			SHGFI_SELECTED =         0x000010000,     // 
			/// <summary>
			/// get only specified attributes
			/// </summary>
			SHGFI_ATTR_SPECIFIED =   0x000020000,     // 
			/// <summary>
			/// get large icon
			/// </summary>
			SHGFI_LARGEICON =        0x000000000,     // 
			/// <summary>
			/// get small icon
			/// </summary>
			SHGFI_SMALLICON =        0x000000001,     // 
			/// <summary>
			/// get open icon
			/// </summary>
			SHGFI_OPENICON =         0x000000002,     // 
			/// <summary>
			/// get shell size icon
			/// </summary>
			SHGFI_SHELLICONSIZE =    0x000000004,     // 
			/// <summary>
			/// pszPath is a pidl
			/// </summary>
			SHGFI_PIDL =             0x000000008,     // 
			/// <summary>
			/// use passed dwFileAttribute
			/// </summary>
			SHGFI_USEFILEATTRIBUTES = 0x000000010     // 
		}

		/**************************************************************/
		/*                        Methods
        /**************************************************************/
		
		/// <summary>
		/// Changes clip board chain.
		/// </summary>
		/// <param name="hWndRemove">hWndRemove</param>
		/// <param name="hWndNewNext">hWndNewNext</param>
		/// <returns></returns>
		[DllImport("user32.dll", ExactSpelling=true)]
		public static extern int ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);
		/// <summary>
		/// Extracts IconEx.
		/// </summary>
		/// <param name="lpszFile">lpszFile</param>
		/// <param name="nIconIndex">nIconIndex</param>
		/// <param name="phiconLarge">phiconLarge</param>
		/// <param name="phiconSmall">phiconSmall</param>
		/// <param name="nIcons">nIcons</param>
		/// <returns></returns>
		[DllImport("Shell32.dll", CharSet=CharSet.Unicode)]
		internal static extern uint ExtractIconEx(string lpszFile, int nIconIndex, IntPtr[] phiconLarge, IntPtr[] phiconSmall, uint nIcons );
		/// <summary>
		/// Posts message.
		/// </summary>
		/// <param name="hWnd">hWnd</param>
		/// <param name="Msg">Msg</param>
		/// <param name="wParam">wParam</param>
		/// <param name="lParam">lParam</param>
		/// <returns></returns>
		[DllImport( "User32.dll" )]
		[CLSCompliant(false)]
		public static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
		/// <summary>
		/// Sends message.
		/// </summary>
		/// <param name="hWnd">hWnd</param>
		/// <param name="Msg">Msg</param>
		/// <param name="wParam">wParam</param>
		/// <param name="lParam">lParam</param>
		/// <returns></returns>
		[DllImport("user32.dll", EntryPoint="SendMessageW")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
		/// <summary>
		/// Sets clip board viewer.
		/// </summary>
		/// <param name="hWndNewViewer">hWndNewViewer</param>
		/// <returns></returns>
		[DllImport("user32.dll", ExactSpelling=true)]
		public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

		/// <summary>
		/// Gets file info.
		/// </summary>
		/// <param name="pszPath">Path</param>
		/// <param name="dwFileAttributes">File attributes</param>
		/// <param name="psfi">SHFileInfo</param>
		/// <param name="cbfileInfo">File info</param>
		/// <param name="uFlags">Flags</param>
		/// <returns>Pointer.</returns>
		[DllImport("Shell32.dll", CharSet=CharSet.Unicode)]
		internal static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, out SHFILEINFO psfi, uint cbfileInfo, SHGFI uFlags );
		
		/**************************************************************/
		/*                        Struct
        /**************************************************************/
		/// <summary>
		/// SHFILEINFO structure.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		internal struct SHFILEINFO {

			/// <summary>
			/// Constructor.
			/// </summary>
			public SHFILEINFO(bool b) {
				hIcon=IntPtr.Zero;
				iIcon=0;
				dwAttributes=0;
				szDisplayName=String.Empty;
				szTypeName=String.Empty;
			}
			/// <summary>
			/// hIcon.
			/// </summary>
			internal IntPtr hIcon;
			/// <summary>
			/// iIcon.
			/// </summary>
			private int iIcon;
			/// <summary>
			/// dwAttributes.
			/// </summary>
			private uint dwAttributes;
			/// <summary>
			/// szDisplayName.
			/// </summary>
			[MarshalAs(UnmanagedType.LPStr, SizeConst=260)]
			private string szDisplayName;
			/// <summary>
			/// szTypeName.
			/// </summary>
			[MarshalAs(UnmanagedType.LPStr, SizeConst=80)]
			private string szTypeName;
		};
		
		#region ArtCSB extensions
		/// <summary>
		/// Beeps.
		/// </summary>
		/// <param name="dwFreq">Frequency</param>
		/// <param name="dwDuration">Duration</param>
		/// <returns></returns>
		[DllImport("kernel32.dll")]
		[CLSCompliant(false)]
		public static extern bool Beep(uint dwFreq, uint dwDuration);
		
		[DllImport("winmm.dll", CharSet=CharSet.Unicode)]
		private static extern bool PlaySoundA(string fileName, uint hmod, uint fdwSound);
		
		/// <summary>
		/// Plays sound file.
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <returns></returns>
		public static bool PlaySound(string fileName){
			return PlaySoundA(fileName, 0, 0x00020000);
		}
		#endregion
	}
}
