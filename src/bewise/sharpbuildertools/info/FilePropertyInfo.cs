using System.IO;
using BeWise.Common.Utils;
using Borland.Studio.ToolsAPI;

namespace BeWise.SharpBuilderTools.Info {
	
	/// <summary>
	/// FileProperty info.
	/// </summary>
    public class FilePropertyInfo {
        /**************************************************************/
        /*                        Constructor
        /**************************************************************/
		/// <summary>
		/// Constructor.
		/// </summary>
        public FilePropertyInfo() {
            ResetFileInfo();
        }

        /**************************************************************/
        /*                        Private
        /**************************************************************/

        private string fileName;
        private FileInfo fileInfo;

        /**************************************************************/
        /*                        Protected
        /**************************************************************/
		/// <summary>
		/// Reset file info.
		/// </summary>
        private void ResetFileInfo() {
            fileName = OtaUtils.GetCurrentEditorFileName();
            fileInfo = new FileInfo(fileName);
        }
		/// <summary>
		/// File info.
		/// </summary>
        private FileInfo FileInfo {
            get {
                return fileInfo;
            }
        }

        /**************************************************************/
        /*                        Public
        /**************************************************************/
		/// <summary>
		/// ReadOnly.
		/// </summary>
        public bool ReadOnly {
            get {
                return ((FileInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly) ;
            }

            set {
                IOTAEditor _Editor = OtaUtils.GetEditorWithSourceEditor(OtaUtils.GetCurrentModule());

                if (_Editor != null && !_Editor.IsModified) {
                    if (value) {
                        File.SetAttributes(fileName, File.GetAttributes(fileName) | FileAttributes.ReadOnly);
                    } else {
                        File.SetAttributes(fileName, File.GetAttributes(fileName) ^ FileAttributes.ReadOnly);
                    }

                    IOTAActionService _ActionService = OtaUtils.GetActionService();
                    _ActionService.ReloadFile(fileName);

                    ResetFileInfo();
                } else {
                    Lextm.Windows.Forms.MessageBoxFactory.Info("Save your file before changing the attributes!");
                }
            }
        }
		/// <summary>
		/// File name.
		/// </summary>
        public string FileName {
            get {
                return System.IO.Path.GetFileName(fileName);
            }

//            set {
//            }
        }
		/// <summary>
		/// Path.
		/// </summary>
		/// <remarks>Directory.</remarks>
        public string Path {
            get {
                return System.IO.Path.GetDirectoryName(fileName);
            }

//            set {
//            }
        }
    }
}
