using System;

namespace Lextm.JcfExpert
{
	[Serializable]
	public class JcfConfiguration {

		// *************************************************************************
		//                            Public Fields
		// *************************************************************************

		public bool IndentNamespace = true;
		public JcfStyle Style;
		public string CommandLineParams = "";

	}

	[Serializable]
	public class Configuration
	{
		// *************************************************************************
		//                            Public Fields
		// *************************************************************************

		public string JcfPath = "";
		public string JcfParams = "";
		public JcfConfiguration JcfConfiguration = new JcfConfiguration();

	}
}
