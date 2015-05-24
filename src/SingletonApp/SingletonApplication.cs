/*
 *		How to prevent multiple application instances?
 *		See cSingletonApp class
 *		
 *		cSingletonApp.PreviousInstanceDetected detects previous instance of the application and returns
 *		its MainWindowHandle (if any)
 *
 *		cSingletonApp.ShowPreviousInstance is more tricky. If the MainWindowHandle of the previous application
 *		instance is available, it activates the main window.
 *		What makes it tricky is that
 *		it works correctly even if some modal form is active or some dialog is running.
 *
 *		Copyright ?2003 by Sergey A. Kryukov
 *		http://www.SAKryukov.org
 *		 
*/
using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace SingletonApp {
	/// <summary>
	/// Singleton application class.
	/// </summary>
	public sealed class SingletonAppliation {
		
		/// <summary>
		/// Verifies if previous instance is detected.
		/// </summary>
		/// <param name="handle"></param>
		/// <returns></returns>
		public static bool PreviousInstanceDetected(out IntPtr handle) {
			handle = IntPtr.Zero;
			Process currentProcess=Process.GetCurrentProcess();
			string currentName = currentProcess.MainModule.FileName;
			string shortCurrentName = Path.GetFileNameWithoutExtension(currentName);
			Process[] processes = Process.GetProcessesByName(shortCurrentName);
			foreach (Process aproc in processes) {
				if (aproc.Id!=currentProcess.Id) {
					if (aproc.MainModule.FileName==currentName) {
						handle = aproc.MainWindowHandle;
						return true;
					} // if same full name
				} // if not the same process
			}
			return false;
		} // PreviousInstanceDetected()
		/// <summary>
		/// Shows previous instance.
		/// </summary>
		/// <param name="handle"></param>
		public static void ShowPreviousInstance(IntPtr handle) {
			if (handle==IntPtr.Zero) {return;  }
			IntPtr topWindow = GetLastActivePopup(handle);
			if (topWindow==IntPtr.Zero) {return; }
			if ( (IsWindowVisible(topWindow)) && (IsWindowEnabled(topWindow)) )
			{	SetForegroundWindow(topWindow);		}
		} // ShowPreviousInstance()
		#region private: user32.dll imports
		[DllImport("user32.dll")]
		static extern IntPtr GetLastActivePopup(IntPtr hWnd);
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool IsWindowVisible(IntPtr hWnd);		
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool IsWindowEnabled(IntPtr hWnd);		
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private	static extern void SetForegroundWindow(IntPtr handle);
		#endregion // private
    	private SingletonAppliation() {}
	} // class cSingletonApp
} // namespace SingletonAppliation
