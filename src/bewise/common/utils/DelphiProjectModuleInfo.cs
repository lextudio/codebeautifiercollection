using System.IO;
using Borland.Studio.ToolsAPI;

namespace BeWise.Common.Utils
{
	internal class DelphiProjectModuleInfo : IOTAModuleInfo {
		
		public DelphiProjectModuleInfo(string fileName) {
			this.fileName = fileName;
		}
		
		private string fileName;
		
		string IOTAModuleInfo.Name {
			get {
				return Path.GetFileNameWithoutExtension(fileName);
			}
		}
		
		string IOTAModuleInfo.FileName {
			get {
				return fileName;
			}
		}
		
		string IOTAModuleInfo.ModuleType {
			get {
				return "Dpr files";
			}
		}
		
		string IOTAModuleInfo.ClassName {
			get {
				return null;
			}
		}
		
		string IOTAModuleInfo.DesignClassName {
			get {
				return null;
			}
		}
		
		IOTAModule IOTAModuleInfo.OpenModule()
		{
			return OtaUtils.GetModuleServices().OpenModule(fileName);
		}
		
		int IOTAModuleInfo.AdditionalFilesCount {
			get {
				return 0;
			}
		}
		
		string IOTAModuleInfo.AdditionalFiles(int index)
		{
			return null;
		}
	}
}