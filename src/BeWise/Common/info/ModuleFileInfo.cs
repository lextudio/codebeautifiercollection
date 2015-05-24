using System;
using System.IO;
using System.CodeDom;
using System.Collections;
using System.Windows.Forms;
using Borland.Studio.ToolsAPI;
using BeWise.Common;

namespace BeWise.Common.Info {

    public class ModuleFileInfo {


        /**************************************************************/
        /*                      Public Fields
        /**************************************************************/

        public string FileName {
            get {
                if (Tag is IOTAModuleInfo) {
                    return (Tag as IOTAModuleInfo).FileName;
                } else if (Tag is IOTAModule) {
                    return (Tag as IOTAModule).FileName;
                } else {
                    return "";
                }

            }
        }
        public bool IsInActiveProject;
        public bool IsFormInfo;
        public string ProjectFileName;
        public object Tag;

        /**************************************************************/
        /*                      Properties
        /**************************************************************/

        public int ImageIndex {
            get {
                string _Ext = Path.GetExtension(FileName).ToUpper();

                if (_Ext == ".ASAX") {
                    return CommonConsts.ASAX_FILE_IMAGEINDEX;
                } else if (_Ext == ".ASPX") {
                    return CommonConsts.ASPX_FILE_IMAGEINDEX;
                } else if (_Ext == ".DLL") {
                    return CommonConsts.ASSEMBLY_FILE_IMAGEINDEX;
                } else if (_Ext == ".CS") {
                    if (IsFormInfo) {
                        return CommonConsts.FORM_FILE_IMAGEINDEX;
                    } else {
                        return CommonConsts.CLASS_FILE_IMAGEINDEX;
                    }
                } else if (_Ext == ".HTML" || _Ext == ".ASP" || _Ext == ".HTM") {
                    return CommonConsts.HTML_FILE_IMAGEINDEX;
                } else
                    return CommonConsts.CUSTOM_FILE_IMAGEINDEX;
            }
        }
    }
}
