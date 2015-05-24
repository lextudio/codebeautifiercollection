using System;
using System.IO;
using BeWise.Common.Info;
using BeWise.Common.Utils;
using BeWise.Common;

namespace Lextm.WiseEditor.FileAid {
    /// <summary>
    /// Description of TypeSpec.
    /// </summary>
    class TypeSpec : Spec {
        ModuleType type;
        internal TypeSpec(ModuleType type) {
            this.type = type;
        }

        internal override bool IsSatisfiedBy(ModuleFileInfo file) {
			if (!File.Exists(file.FileName))
			{
                return false;
            }
			switch (type) {
            case ModuleType.All:
                return true;
            case ModuleType.Assemblies:
                return IsAssembly(file);
            case ModuleType.Forms:
                return IsForm(file);
            case ModuleType.Sources:
                return IsSource(file);
            default:
                return false;
            }
        }
		
        private static bool IsSource(ModuleFileInfo file) {
        	return OtaUtils.IsSourceFile(file.FileName);
        }
        
		private static bool IsAssembly(ModuleFileInfo file) {
            return Path.GetExtension(file.FileName).ToUpperInvariant() == ".DLL";
        }

        private static bool IsForm(ModuleFileInfo file) {
        	#region Approach One: fails because of an OTA bug.
			// this approach cannot show all files. Some forms and user controls give null, too.
//			IOTAModuleInfo info = (IOTAModuleInfo)file.Tag;
//			LoggingService.Info(file.FileName + " gives " +
//				info.DesignClassName);
//			if (String.IsNullOrEmpty(info.DesignClassName))
//			{
//				return false;
//			} else {
//				return true;
//			}
			#endregion
			#region Approach Two: too slow.
			/* if (IsAssembly(file))
			{
				// prevent them from opening.
				return false;
			}
			// method 1 will open the module:
			// IOTAModule module = info.OpenModule();
			// method 2 will open the file and make it invisible.
			IOTAModule module = OTAUtils.GetModuleServices().OpenModule(file.FileName);
			if (module == null)
			{
				return false;
			}
			IOTADotNetModule dotNet = module.GetService(typeof(IOTADotNetModule))
									  as IOTADotNetModule;
			if (dotNet == null) {
				return false;  // not .NET module.
			}
			if (dotNet.HasDesignableType) {
				return true;
			}
			return false;
			//*/
			#endregion
			// if .resx exists and a source file with the same name, it may be a Form or User Controls.
			// There might be few exceptions.
			string ext = Path.GetExtension(file.FileName);
			if (String.IsNullOrEmpty(ext)
                || ext.ToUpperInvariant() == ".RESX")
			{
				return false;
			}
            string resxFileName = Path.ChangeExtension(file.FileName, ".resx");
            string nfmFileName = Path.ChangeExtension(file.FileName, ".nfm");
            string dfmFileName = Path.ChangeExtension(file.FileName, ".dfm");
			if (File.Exists(resxFileName) || File.Exists(nfmFileName) || File.Exists(dfmFileName)) {
				return true;
			} else {
                return false;
            }
		}
    }

    internal enum ModuleType {
        Unknown = 0,
        All,
        Forms,
        Sources,
        Assemblies
    };
}
