using System;
using System.Diagnostics;
using System.IO;

namespace BeWise.SharpBuilderTools.Tools {
	///<summary>Base tool.</summary>
	public abstract class BaseTool {

		/// <summary>
		/// Gets GUI executable.
		/// </summary>
		/// <returns>Path</returns>
		public virtual string GetGui() {
			string toolPath = GetToolPath();
			if (string.IsNullOrEmpty(toolPath))
			{
				return null;
			}
			return Path.Combine(toolPath, GetGuiArgument());
		}
		/// <summary>
		/// Gets console executable.
		/// </summary>
		/// <returns>Path</returns>
		public virtual string GetConsole() {
			string toolPath = GetToolPath();
			if (string.IsNullOrEmpty(toolPath))
			{
				return null;
			}
			return Path.Combine(toolPath, GetConsoleArgument());
		}
		/// <summary>
		/// Gets folder.
		/// </summary>
		/// <returns>Folder</returns>
		protected abstract string GetToolPath ();
		/// <summary>
		/// Gets GUI command line.
		/// </summary>
		/// <returns>Command line.</returns>
		protected virtual string GetGuiArgument () {
			return null;
		}
		/// <summary>
		/// Gets console command line.
		/// </summary>
		/// <returns>Command line</returns>
		protected virtual string GetConsoleArgument () {
			return null;
		}
		/// <summary>
		/// Verifies if tool is installed.
		/// </summary>
		/// <returns>true if tool is installed, false if not.</returns>
		protected abstract bool GetInstalled ();
		/// <summary>
		/// Gets name.
		/// </summary>
		/// <returns>Name</returns>
		public abstract string GetName ();
		/// <summary>
		/// Verifies if it is tool file.
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <returns>true if it is, false if not.</returns>
		public abstract bool IsToolFile(string fileName);
		/// <summary>
		/// Opens GUI.
		/// </summary>
		public virtual void OpenGui() {
			Process _P;

			// Create the process
			ProcessStartInfo _ProcessStartInfo = new ProcessStartInfo();

			_ProcessStartInfo.FileName = GetGui();
			_ProcessStartInfo.Arguments = String.Empty;
			_ProcessStartInfo.RedirectStandardOutput = false;
			_ProcessStartInfo.CreateNoWindow = true;
			_ProcessStartInfo.UseShellExecute = false;

			_P = new Process();
			_P.EnableRaisingEvents = true;
			_P.StartInfo = _ProcessStartInfo;
			_P.Start();
			_P.WaitForExit();
		}

		/// <summary>
		/// Runs console.
		/// </summary>
		/// <param name="aParams">Parameters</param>
		/// <param name="aWorkingDir">Working directory</param>
		public virtual void RunConsole(object[] aParams, string aWorkingDir) {
			Lextm.Diagnostics.LoggingService.Debug(GetConsole() + " - " + aParams + " at " + aWorkingDir);
		}
		/// <summary>
		/// Runs GUI.
		/// </summary>
		/// <param name="aParams">Parameters</param>
		/// <param name="aWorkingDir">Working directory</param>
		public virtual void RunGui(object[] aParams, string aWorkingDir) {
			Lextm.Diagnostics.LoggingService.Debug(GetGui() + " - " + aParams + " at " + aWorkingDir);
		}

//		/// <summary>
//		/// Console command line.
//		/// </summary>
//		protected string ConsoleArgument
//		{
//			get {
//				return GetConsoleArgument();
//			}
//		}
//		/// <summary>
//		/// GUI command line.
//		/// </summary>
//		public string GuiArgument
//		{
//			get {
//				return GetGuiArgument();
//			}
//		}
//		/// <summary>
//		/// Name.
//		/// </summary>
//		public string Name
//		{
//			get {
//				return GetName();
//			}
//		}
//		/// <summary>
//		/// Tool console executable.
//		/// </summary>
//		public string Console
//		{
//			get {
//				return GetConsole();
//			}
//		}
//		/// <summary>
//		/// Tool GUI executable.
//		/// </summary>
//		public string Gui
//		{
//			get {
//				return GetGui();
//			}
//		}
		/// <summary>
		/// Verifies if tool is installed.
		/// </summary>
		internal bool Installed {
			get {
				return GetInstalled();
			}
		}
//		/// <summary>
//		/// Tool path.
//		/// </summary>
//		public string ToolPath
//		{
//			get {
//				return GetToolPath();
//			}
//		}
		/// <summary>
		/// OnConsoleOutput delegate.
		/// </summary>
		public EventHandler OnConsoleOutput;
		/// <summary>
		/// OnRunCompleted delegate.
		/// </summary>
		public EventHandler OnRunCompleted;
	}
}
