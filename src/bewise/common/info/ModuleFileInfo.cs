using System;
using System.IO;
using Borland.Studio.ToolsAPI;
using BeWise.Common.Utils;

namespace BeWise.Common.Info {
	/// <summary>
	/// Module file info.
	/// </summary>
    public class ModuleFileInfo {


        /**************************************************************/
        /*                      Public Fields
        /**************************************************************/
		/// <summary>
		/// File name.
		/// </summary>
		/// <remarks>String.Empty if failed.</remarks>
        public string FileName {
            get {
                if (Tag is IOTAModuleInfo) {
                    return (Tag as IOTAModuleInfo).FileName;
                } else if (Tag is IOTAModule) {
                    return (Tag as IOTAModule).FileName;
                } else {
                    return String.Empty;
                }

            }
        }
		/// <summary>
		/// Is in active project.
		/// </summary>
        public bool IsPartOfActiveProject
        {
            get
            {
                IOTAProject proj = OtaUtils.GetCurrentProjectGroup().ActiveProject;
                return ProjectFileName.Contains(proj.FileName);
            }
        }

        bool isFormInfo;
        /// <summary>
        /// Is form info.
        /// </summary>
        public bool IsFormInfo
        {
            get
            {
                return isFormInfo;
            }
            set
            {
                isFormInfo = value;
            }
        }

        string projectFileName;
        /// <summary>
        /// Project file name.
        /// </summary>
        public string ProjectFileName
        {
            get
            {
                return projectFileName;
            }
            set
            {
                projectFileName = value;
            }
        }

        object tag;

        /// <summary>
        /// Tag.
        /// </summary>
        public object Tag
        {
            get
            {
                return tag;
            }
            set
            {
                tag = value;
            }
        }

        /**************************************************************/
        /*                      Properties
        /**************************************************************/
		/// <summary>
		/// Image index.
		/// </summary>
        public int ImageIndex {
            get {
                string _Ext = Path.GetExtension(FileName).ToUpperInvariant();

                if (_Ext == ".ASAX") {
                    return ASAX_FILE_IMAGEINDEX;
                } else if (_Ext == ".ASPX") {
                    return ASPX_FILE_IMAGEINDEX;
                } else if (_Ext == ".DLL") {
                    return ASSEMBLY_FILE_IMAGEINDEX;
                } else if (_Ext == ".CS") {
                    if (IsFormInfo) {
                        return FORM_FILE_IMAGEINDEX;
                    } else {
                        return CLASS_FILE_IMAGEINDEX;
                    }
                } else if (_Ext == ".HTML" || _Ext == ".ASP" || _Ext == ".HTM") {
                    return HTML_FILE_IMAGEINDEX;
				} else {
					return CUSTOM_FILE_IMAGEINDEX;
                }
            }
        }
        private const int ASAX_FILE_IMAGEINDEX                               = 0;
        private const int ASPX_FILE_IMAGEINDEX                               = 1;
        private const int ASSEMBLY_FILE_IMAGEINDEX                           = 2;
        private const int CLASS_FILE_IMAGEINDEX                              = 3;
        private const int CUSTOM_FILE_IMAGEINDEX                             = 4;
        private const int FORM_FILE_IMAGEINDEX                               = 5;
        private const int HTML_FILE_IMAGEINDEX                               = 6;

	}
}
