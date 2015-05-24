using System;

namespace BeWise.Common {

	public class CommonConsts {
		// Access Text
		public const string ACCESS_PRIVATE_TEXT                             = "Private";
		public const string ACCESS_PROTECTED_TEXT                           = "Protected";
		public const string ACCESS_PROTECTED_INTERNAL_TEXT                  = "Protected Internal";
		public const string ACCESS_INTERNAL_TEXT                            = "Internal";
		public const string ACCESS_PUBLIC_TEXT                              = "Public";

		public const string BDS_DOT_NET_FRAMEWORK_REG_KEY                   = "DotNetFramework";
//		public const string BDS_V1_REG_KEY                                  = @"Software\Borland\BDS\1.0\";
//		public const string BDS_V2_REG_KEY                                  = @"Software\Borland\BDS\2.0\";
//		public const string BDS_V3_REG_KEY                                  = @"Software\Borland\BDS\3.0\";

		public static string[] BDS_REG_KEYS = {
								  @"Software\Borland\BDS\1.0\",
								  @"Software\Borland\BDS\2.0\",
								  @"Software\Borland\BDS\3.0\"
								};

		public static string[] BDS_VERSIONS = {
								  @"BDS 1 - C# Builder 1.0",
								  @"BDS 2 - Delphi 8 for .NET",
								  @"BDS 3 - Delphi 2005"
								};

		// Borland Assembly Experts
		public const string BDS_ENABLED_ASSEMBLIES_REG_KEY              = "Known IDE Assemblies";
		public const string BDS_DISABLED_ASSEMBLIES_REG_KEY             = "Disabled IDE Assemblies";

//		public const string BDS_V1_ENABLED_ASSEMBLIES_REG_KEY              = BDS_V1_REG_KEY + "Known IDE Assemblies";
//		public const string BDS_V1_DISABLED_ASSEMBLIES_REG_KEY             = BDS_V1_REG_KEY + "Disabled IDE Assemblies";
//		public const string BDS_V2_ENABLED_ASSEMBLIES_REG_KEY              = BDS_V2_REG_KEY + "Known IDE Assemblies";
//		public const string BDS_V2_DISABLED_ASSEMBLIES_REG_KEY             = BDS_V2_REG_KEY + "Disabled IDE Assemblies";
//		public const string BDS_V3_ENABLED_ASSEMBLIES_REG_KEY              = BDS_V3_REG_KEY + "Known IDE Assemblies";
//		public const string BDS_V3_DISABLED_ASSEMBLIES_REG_KEY             = BDS_V3_REG_KEY + "Disabled IDE Assemblies";

		// Code Dom ImageIndex
		public const int DEFAULT_CLASS_IMAGEINDEX                           = 0;
		public const int DEFAULT_EVENT_IMAGEINDEX                           = 1;
		public const int DEFAULT_FIELD_IMAGEINDEX                           = 2;
		public const int DEFAULT_INTERFACE_IMAGEINDEX                       = 3;
		public const int DEFAULT_METHOD_IMAGEINDEX                          = 4;
		public const int DEFAULT_PROPERTY_IMAGEINDEX                        = 5;

		public const int INTERNAL_METHOD_IMAGEINDEX                         = 6;
		public const int INTERNAL_STATIC_METHOD_IMAGEINDEX                  = 7;
        public const int INTERNAL_VIRTUAL_METHOD_IMAGEINDEX                 = 8;

        public const int PRIVATE_METHOD_IMAGEINDEX                          = 9;
        public const int PRIVATE_STATIC_METHOD_IMAGEINDEX                   = 10;
        public const int PRIVATE_VIRTUAL_METHOD_IMAGEINDEX                  = 11;

        public const int PROTECTED_METHOD_IMAGEINDEX                        = 12;
        public const int PROTECTED_STATIC_METHOD_IMAGEINDEX                 = 13;
        public const int PROTECTED_VIRTUAL_METHOD_IMAGEINDEX                = 14;

        public const int PUBLIC_METHOD_IMAGEINDEX                           = 15;
        public const int PUBLIC_STATIC_METHOD_IMAGEINDEX                    = 16;
        public const int PUBLIC_VIRTUAL_METHOD_IMAGEINDEX                   = 17;

		public const int INTERNAL_CLASS_IMAGEINDEX                          = 18;
		public const int PRIVATE_CLASS_IMAGEINDEX                           = 19;
		public const int PUBLIC_CLASS_IMAGEINDEX                            = 20;

        public const int INTERNAL_FIELD_IMAGEINDEX                          = 21;
        public const int INTERNAL_STATIC_FIELD_IMAGEINDEX                   = 22;

		public const int PRIVATE_FIELD_IMAGEINDEX                           = 23;
		public const int PRIVATE_STATIC_FIELD_IMAGEINDEX                    = 24;

        public const int PROTECTED_FIELD_IMAGEINDEX                         = 25;
        public const int PROTECTED_STATIC_FIELD_IMAGEINDEX                  = 26;

        public const int PUBLIC_FIELD_IMAGEINDEX                            = 27;
        public const int PUBLIC_STATIC_FIELD_IMAGEINDEX                     = 28;

        // File ImageIndex
        public const int ASAX_FILE_IMAGEINDEX                               = 0;
        public const int ASPX_FILE_IMAGEINDEX                               = 1;
        public const int ASSEMBLY_FILE_IMAGEINDEX                           = 2;
        public const int CLASS_FILE_IMAGEINDEX                              = 3;
        public const int CUSTOM_FILE_IMAGEINDEX                             = 4;
        public const int FORM_FILE_IMAGEINDEX                               = 5;
        public const int HTML_FILE_IMAGEINDEX                               = 6;

        // General
        public const string CS_EXTENSION                                    = ".cs";
        public const string CR_LF                                           = "\r\n";
        public const int DEFAULT_INDENTATION                                = 4;
        public const string PROJECT_EXTENSION                               = ".bdsproj";
        public const string PROJECT_GROUP_EXTENSION                         = ".bdsgroup";
        public const string RESX_EXTENSION                                  = ".resx";
        public const string XML_EXTENSION                                  = ".xml";

        // Reg keys
        public const string REG_KEY_BORLAND_CS_SOURCE_OPTIONS               = @"Editor\Source Options\Borland.EditOptions.C#";
        public const string REG_KEY_BORLAND_DELPHI_SOURCE_OPTIONS           = @"Editor\Source Options\Borland.EditOptions.Pascal";
        public const string REG_KEY_BORLAND_XML_SOURCE_OPTIONS              = @"Editor\Source Options\Borland.EditOptions.XML";

		// Scope Text
		public const string SCOPE_ABSTRACT_TEXT                             = "Abstract";
        public const string SCOPE_FINAL_TEXT                                = "Final";
        public const string SCOPE_OVERRIDE_TEXT                             = "Override";
        public const string SCOPE_STATIC_TEXT                               = "Static";
        public const string SCOPE_VIRTUAL_TEXT                              = "Virtual";
	}
}
