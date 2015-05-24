using System;

namespace Lextm.CBC
{
	[Serializable]
	public class JcfConfiguration {

		// *************************************************************************
		//                            Public Fields
		// *************************************************************************

		public bool IndentNamespace = true;
		public JcfStyle Style;
		public string CommandLineParams = null;

	}

	[Serializable]
	public class AStyleConfiguration {

		public bool IndentNamespace = true;
		public AStyleStyle Style;
		public string CommandLineParams = null;

	}

	[Serializable]
	public class Configuration
	{
		// *************************************************************************
		//                            Public Fields
		// *************************************************************************

		//JCF
		public string JcfPath = null;
		public string JcfParams = null;
		public JcfConfiguration JcfConfiguration = new JcfConfiguration();

		//AStyle
		public string AStylePath = null;
		public string AStyleParams = null;
		public AStyleConfiguration AStyleConfiguration = new AStyleConfiguration();

		//Misc.
		public int XmlIndentation = 2;
	}
}
