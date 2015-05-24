using System.IO;
using Borland.Studio.ToolsAPI;

namespace Lextm.JcfExpert.Utils {

	public class OTAUtils {

		public static bool CurrentModuleIsCFile() {
			IOTAModule _Module = BeWise.Common.Utils.OTAUtils.GetCurrentModule();
			string _Extension = Path.GetExtension(_Module.FileName).ToUpper();

			return (_Extension == ".CS") ||
				   (_Extension == ".CPP") || (_Extension == ".C") ||
				   (_Extension == ".CXX") || (_Extension == ".CC") ||
				   (_Extension == ".HPP") || (_Extension == ".H") ||
				   (_Extension == ".HXX") || (_Extension == ".HH");
		}

	}

}