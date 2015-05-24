using System;
using System.Runtime.InteropServices;

namespace Lextm.JcfExpert.Utils {

	public class WinAPI {

		/**************************************************************/
		/*                        Clipboard
		/**************************************************************/

		[DllImport("user32.dll", EntryPoint="SendMessageW")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", ExactSpelling=true)]
		public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

		[DllImport("user32.dll", ExactSpelling=true)]
		public static extern int ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);
	}
}
