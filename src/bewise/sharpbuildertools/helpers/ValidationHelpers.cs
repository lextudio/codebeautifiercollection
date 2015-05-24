/************************************************************/
/*	Ported from SBT										    */
/************************************************************/
using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Tools;
using Borland.Studio.ToolsAPI;
using Lextm.Diagnostics;
using Lextm.OpenTools;

namespace BeWise.SharpBuilderTools.Helpers {
	/// <summary>
	/// Validation helpers.
	/// </summary>
	public sealed class ValidationHelpers {

		private const string MessageBoxTitle                                   = "Code Beautifier Collection";//"Sharp Builder Tools";

		private ValidationHelpers( ) { }

		/**************************************************************/
		/*                     Messages
        /**************************************************************/

		/// <summary>
		/// Shows a warning.
		/// </summary>
		/// <param name="message">Message</param>
		public static void ShowWarning(string message) {
			//MessageBox.Show(message,
			//MessageBoxTitle,
			//MessageBoxButtons.OK,
			//MessageBoxIcon.Warning);
			MessageService.Show(message);
		}

		/**************************************************************/
		/*                     Validation
        /**************************************************************/
		/// <summary>
		/// Validates current module is C# file.
		/// </summary>
		/// <returns></returns>
		public static bool ValidateCurrentModuleIsCSFile() {
			if (!ValidateCurrentModuleNotNull()) {
				return false;
			}

			if (!OtaUtils.CurrentModuleIsCSFile()) {
				ShowWarning("The current file is not a C# file !");
				return false;
			}

			return true;
		}
		/// <summary>
		/// Validates current module is Delphi file.
		/// </summary>
		/// <returns></returns>
		public static bool ValidateCurrentModuleIsDelphiFile() {
			if (!ValidateCurrentModuleNotNull()) {
				return false;
			}

			if (!OtaUtils.CurrentModuleIsDelphiFile()) {
				ShowWarning("The current file is not a Delphi file !");
				return false;
			}

			return true;
		}
		/// <summary>
        /// Validates current module is source file (C/C++/C#, Delphi, VB).
		/// </summary>
		/// <returns></returns>
		public static bool ValidateCurrentModuleIsSourceFile() {
			if (!ValidateCurrentModuleNotNull()) {
				return false;
			}

			if (!OtaUtils.CurrentModuleIsSourceFile()) {
				ShowWarning("The current file is not a Source file (C/C++/C#, Delphi, VB)!");
				return false;
			}

			return true;
		}
		/// <summary>
		/// Validates current module is XML file.
		/// </summary>
		/// <returns></returns>
		public static bool ValidateCurrentModuleIsXmlFile() {
			if (!ValidateCurrentModuleNotNull()) {
				return false;
			}

			if (!OtaUtils.CurrentModuleIsXmlFile()) {
				ShowWarning("The current file is not an Xml file !");
				return false;
			}

			return true;
		}
		/// <summary>
		/// Validates current module is C# or XML file.
		/// </summary>
		/// <returns></returns>
		public static bool ValidateCurrentModuleIsCSOrXmlFile() {
			if (!ValidateCurrentModuleNotNull()) {
				return false;
			}
			
			if (!(OtaUtils.CurrentModuleIsCSFile() || OtaUtils.CurrentModuleIsXmlFile())) {
				ShowWarning("The current file is not a C# file or a Xml file!");
				return false;
			}

			return true;
		}
		/// <summary>
		/// Validates current module is C, XML, or Delphi file.
		/// </summary>
		/// <returns></returns>
		public static bool ValidateCurrentModuleIsCSOrXmlOrDelphiFile() {
			if (!ValidateCurrentModuleNotNull()) {
				return false;
			}
			
			if (!(OtaUtils.CurrentModuleIsCSFile() || OtaUtils.CurrentModuleIsXmlFile() || OtaUtils.CurrentModuleIsDelphiFile())) {
				ShowWarning("The current file is not a C# file, a Xml file or a Delphi file!");
				return false;
			}

			return true;
		}
		/// <summary>
		/// Validates current editor is not null.
		/// </summary>
		/// <returns></returns>
		public static bool ValidateCurrentEditorNotNull() {
			if (!ValidateCurrentModuleNotNull()) {
				return false;
			}

			if (OtaUtils.GetCurrentEditor(OtaUtils.GetCurrentModule()) == null) {
				ShowWarning("Current editor is null!");
				return false;
			}

			return true;
		}
		/// <summary>
		/// Validates current module is not null.
		/// </summary>
		/// <returns></returns>
		public static bool ValidateCurrentModuleNotNull() {
			if (OtaUtils.GetCurrentModule() == null) {
				ShowWarning("Current file is null!");
				return false;
			}

			return true;
		}
		/// <summary>
		/// Validates current project group is not null.
		/// </summary>
		/// <returns></returns>
		public static bool ValidateCurrentProjectGroupNotNull() {
			if (OtaUtils.GetCurrentProjectGroup() == null) {
				LoggingService.Warn("Current project group is null!");
				return false;
			}

			return true;
		}
		/// <summary>
		/// Validates current project is not null.
		/// </summary>
		/// <returns></returns>
		public static bool ValidateCurrentProjectNotNull() {
			if (OtaUtils.GetCurrentProject() == null) {
				ShowWarning("Current project is null!");
				return false;
			}

			return true;
		}
		/// <summary>
		/// Validates current project target exists.
		/// </summary>
		/// <returns></returns>
		public static bool ValidateCurrentProjectTargetExists() {
			if (!ValidateCurrentProjectNotNull()) {
				return false;
			}

			string _Target = OtaUtils.GetProjectTarget(OtaUtils.GetCurrentProject());

			if (String.IsNullOrEmpty(_Target)
			    || !File.Exists(_Target)) {
				ShowWarning("Project target is not found !");
				return false;
			}

			return true;
		}
		/// <summary>
		/// Validates current project target is not null.
		/// </summary>
		/// <returns></returns>
		public static bool ValidateCurrentProjectTargetNotNull() {
			if (!ValidateCurrentProjectNotNull()) {
				return false;
			}

			string _Target = OtaUtils.GetProjectTarget(OtaUtils.GetCurrentProject());

			if (String.IsNullOrEmpty(_Target)) {
				ShowWarning("No project target is defined !");
				return false;
			}

			return true;
		}
		/// <summary>
		/// Validates current source editor is not null.
		/// </summary>
		/// <returns>true if it is not, false if else.</returns>
		public static bool ValidateCurrentSourceEditorNotNull() {
			if (OtaUtils.GetCurrentSourceEditor() == null || OtaUtils.GetCurrentSourceEditor().EditViewCount == 0) {
				ShowWarning("Current file in the editor is null!");
				return false;
			} else {
				return true;
			}
		}
		/// <summary>
		/// Validates a custom condition.
		/// </summary>
		/// <param name="condition">Condition</param>
		/// <param name="message">Message</param>
		/// <returns></returns>
		public static bool ValidateCustom(bool condition, string message) {
			if (!condition) {
				ShowWarning(message);
				return false;
			}

			return true;
		}
		/// <summary>
		/// Validates a custom condition, with a customized dialog.
		/// </summary>
		/// <param name="condition">Condition</param>
		/// <param name="form">Custom dialog</param>
		/// <returns></returns>
		/// <remarks>The dialog should contain only an ok button.</remarks>
		public static bool ValidateCustom(bool condition, Form form) {
			if (!condition) {
				form.ShowDialog();
				return false;
			}

			return true;
		}
		/// <summary>
		/// Valiates selection exists.
		/// </summary>
		/// <returns></returns>
		public static bool ValidateSelectionExists() {
			return ValidateSelectionExists(OtaUtils.GetCurrentModule());
		}
		/// <summary>
		/// Validates selection exists.
		/// </summary>
		/// <param name="module">Module</param>
		/// <returns>true if it exists, false if not.</returns>
		private static bool ValidateSelectionExists(IOTAModule module) {
			if (module == null ||
			    String.IsNullOrEmpty(OtaUtils.GetSelectedText(module))) {
				ShowWarning("No text is selected !");
				return false;
			}

			return true;
		}
		/// <summary>
		/// Validates a tool is installed.
		/// </summary>
		/// <param name="tool">Tool</param>
		/// <returns>true if it is, false if not.</returns>
		public static bool ValidateToolInstalled(BaseTool tool) {
			if (tool == null)
			{
				return false;
			}
			if (!tool.Installed) {
				ShowWarning(String.Format(CultureInfo.InvariantCulture, "{0} is not correctly installed or not found, check your preferences !", tool.GetName()));
				return false;
			}
			return true;
		}
		/// <summary>
		/// Validates a tool is not running.
		/// </summary>
		/// <param name="tool">A OTA tool</param>
		/// <returns>true if not running, false if else.</returns>
		public static bool ValidateToolNotRunning(CustomFeatureTool tool) {
			if (tool.Running) {
				ShowWarning(String.Format(CultureInfo.InvariantCulture, "{0} is already running !", tool.GetCurrentTool().GetName()));
				return false;
			}
			return true;
		}
    }
}
