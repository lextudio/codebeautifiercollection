using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Helpers;
using BeWise.SharpBuilderTools.Tools;
using Borland.Studio.ToolsAPI;

namespace Lextm.OpenTools {
	//TODO: remove this class some day
	/// <summary>
	/// Tool base.
	/// </summary>
    public abstract class CustomFeatureTool : CustomFeature {
        /**************************************************************/
        /*                     Private
        /**************************************************************/

        private bool running;
        private string msg = String.Empty; // Used for the console

        /**************************************************************/
        /*                     Protected
        /**************************************************************/
		/// <summary>
		/// Message.
		/// </summary>
        protected string Msg {
            get {
                return msg;
            }

            set {
                msg = value;
            }
        }
		/// <summary>
		/// Refresh console handler.
		/// </summary>
		/// <param name="sender">String</param>
		/// <param name="e"></param>
        internal protected virtual void DoRefreshConsole(object sender, EventArgs e) {
            msg += sender;
            StringReader _Reader = new StringReader(msg);

            string _Line = _Reader.ReadLine();
            string _PreviousLine = String.Empty;

            while (_Line != null) {
                _PreviousLine = _Line;
                _Line = _Reader.ReadLine();

                if (_Line != null) {
                    OtaUtils.AddMessage(_PreviousLine);
                } else if (sender.ToString().EndsWith("\r", StringComparison.Ordinal) 
                           || sender.ToString().EndsWith(Environment.NewLine, StringComparison.Ordinal)) 
                {
                    OtaUtils.AddMessage(_PreviousLine);
                    _PreviousLine = String.Empty;
                }
            }

            msg = _PreviousLine;
        }
		/// <summary>
		/// Run completed handler.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        internal protected virtual void DoRunCompleted(object sender, EventArgs e) {
            // Nothing by Default
        }
		/// <summary>
		/// Gets current tool.
		/// </summary>
		/// <returns></returns>
        internal protected abstract BaseTool GetCurrentTool();

		/// <summary>
		/// Gets tool file in current project.
		/// </summary>
		/// <returns>null if failed.</returns>
        protected string GetToolFileFromCurrentProject() {
            if (!ValidationHelpers.ValidateCurrentProjectNotNull()) {
                return null;
            }

            IOTAProjectGroup _ProjectGroup = OtaUtils.GetCurrentProjectGroup();
            IOTAProject _Project = _ProjectGroup.ActiveProject;

            for (int k = 0; k < _Project.ModuleCount; k++) {
                IOTAModuleInfo _ModuleInfo = _Project.GetModuleInfo(k);

                if (!String.IsNullOrEmpty(_ModuleInfo.FileName) && GetCurrentTool().IsToolFile(_ModuleInfo.FileName) && File.Exists(_ModuleInfo.FileName)) {
                    return _ModuleInfo.FileName;
                }
            }

            ValidationHelpers.ShowWarning("No " + GetCurrentTool().GetName()  + " file is found in the active project !");
            return null;
        }
		/// <summary>
		/// Adds a message.
		/// </summary>
		/// <param name="text">Text</param>
		protected virtual void AddMessage(string text) { }

        /**************************************************************/
        /*                     Properties
        /**************************************************************/
//		/// <summary>
//		/// Current tool.
//		/// </summary>
//        protected internal BaseTool CurrentTool {
//            get {
//                return GetCurrentTool();
//            }
//        }
		/// <summary>
		/// Running.
		/// </summary>
        public bool Running {
            get {
                return running;
            }

            set {
                running = value;
            }
		}
		/// <summary>
		/// Updates UI for path checking.
        /// </summary>
		/// <param name="folder">Folder</param>
		/// <param name="tool">Tool</param>
        /// <param name="caption">Caption</param>
        public static void UpdateUIForPathChecking(string folder, string tool, Control caption)
        {
        	string[] tools = tool.Split(';');
        	bool exists = true;
        	foreach(string t in tools) {
        		string path = System.IO.Path.Combine(folder, t);
        		exists = exists && System.IO.File.Exists(path);
        	}        	
			if (exists) {
				caption.ForeColor = SystemColors.ControlText;
			} else {
				caption.ForeColor = Color.Red;
			}   
		}
    }
}
