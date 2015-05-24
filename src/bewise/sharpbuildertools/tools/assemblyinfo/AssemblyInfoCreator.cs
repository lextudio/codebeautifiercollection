using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using BeWise.Common.Utils;
using BeWise.SharpBuilderTools.Info;
using Lextm.OpenTools;
using System.Globalization;
using System.Collections.Generic;

namespace BeWise.SharpBuilderTools.Tools.AssemblyInfo {
	/// <summary>
	/// Assembly info creator.
	/// </summary>
    class AssemblyInfoCreator: IComparer<AssemblyInfoDef> {

        internal AssemblyInfoCreator() {}	
        // *************************************************************************
        //                              Private
        // *************************************************************************
        private AssemblyInfoDef BuildMainAssemblyInfoDef(IList<AssemblyInfoDef> aAssemblyInfoDefs) {
            LogMessage(String.Format(CultureInfo.InvariantCulture, "Merging {0} AssemblyInfo(s)", aAssemblyInfoDefs.Count));
            AssemblyInfoDef _AssemblyInfoDef = new AssemblyInfoDef();

            foreach (AssemblyInfoDef _Def in aAssemblyInfoDefs) {
                _AssemblyInfoDef.Assign(_Def);
            }

            return _AssemblyInfoDef;
        }

        private bool GenerateAssemblyInfoCSharpFile(AssemblyInfoDef aAssemblyInfoDef, string aOutputFileName) {
            LogMessage(String.Format(CultureInfo.InvariantCulture, "Building AssemblyInfo file: {0} ", aOutputFileName));
            StringBuilder _Str = new StringBuilder();

            _Str.Append("using System.Reflection;\r\n");
            _Str.Append("using System.Runtime.CompilerServices;\r\n\r\n");

            _Str.Append("//\r\n");
            _Str.Append("// General Information about an assembly is controlled through the following\r\n");
            _Str.Append("// set of attributes. Change these attribute values to modify the information\r\n");
            _Str.Append("// associated with an assembly.\r\n");
            _Str.Append("//\r\n");
            _Str.AppendFormat("[assembly: AssemblyTitle(\"{0}\")]\r\n", aAssemblyInfoDef.AssemblyTitle);
            _Str.AppendFormat("[assembly: AssemblyDescription(\"{0}\")]\r\n", aAssemblyInfoDef.AssemblyDescription);
            _Str.AppendFormat("[assembly: AssemblyConfiguration(\"{0}\")]\r\n", aAssemblyInfoDef.AssemblyConfiguration);
            _Str.AppendFormat("[assembly: AssemblyCompany(\"{0}\")]\r\n", aAssemblyInfoDef.AssemblyCompany);
            _Str.AppendFormat("[assembly: AssemblyProduct(\"{0}\")]\r\n", aAssemblyInfoDef.AssemblyProduct);
            _Str.AppendFormat("[assembly: AssemblyCopyright(\"{0}\")]\r\n", aAssemblyInfoDef.AssemblyCopyright);
            _Str.AppendFormat("[assembly: AssemblyTrademark(\"{0}\")]\r\n", aAssemblyInfoDef.AssemblyTrademark);
            _Str.AppendFormat("[assembly: AssemblyCulture(\"{0}\")]\r\n\r\n", aAssemblyInfoDef.AssemblyCulture);

            _Str.Append("//\r\n");
            _Str.Append("// Version information for an assembly consists of the following four values:\r\n");
            _Str.Append("//\r\n");
            _Str.Append("//      Major Version\r\n");
            _Str.Append("//      Minor Version\r\n");
            _Str.Append("//      Build Number\r\n");
            _Str.Append("//      Revision\r\n");
            _Str.Append("//\r\n");
            _Str.Append("// You can specify all the values or you can default the Revision and Build Numbers\r\n");
            _Str.Append("// by using the '*' as shown below:\r\n");
            _Str.Append("\r\n");
            _Str.AppendFormat("[assembly: AssemblyVersion(\"{0}\")]\r\n\r\n", aAssemblyInfoDef.AssemblyVersion);

            _Str.Append("//\r\n");
            _Str.Append("// In order to sign your assembly you must specify a key to use. Refer to the\r\n");
            _Str.Append("// Microsoft .NET Framework documentation for more information on assembly signing.\r\n");
            _Str.Append("//\r\n");
            _Str.Append("// Use the attributes below to control which key is used for signing.\r\n");
            _Str.Append("//\r\n");
            _Str.Append("// Notes:\r\n");
            _Str.Append("//   (*) If no key is specified, the assembly is not signed.\r\n");
            _Str.Append("//   (*) KeyName refers to a key that has been installed in the Crypto Service\r\n");
            _Str.Append("//       Provider (CSP) on your machine. KeyFile refers to a file which contains\r\n");
            _Str.Append("//       a key.\r\n");
            _Str.Append("//   (*) If the KeyFile and the KeyName values are both specified, the\r\n");
            _Str.Append("//       following processing occurs:\r\n");
            _Str.Append("//       (1) If the KeyName can be found in the CSP, that key is used.\r\n");
            _Str.Append("//       (2) If the KeyName does not exist and the KeyFile does exist, the key\r\n");
            _Str.Append("//           in the KeyFile is installed into the CSP and used.\r\n");
            _Str.Append("//   (*) In order to create a KeyFile, you can use the sn.exe (Strong Name) utility.\r\n");
            _Str.Append( "//       When specifying the KeyFile, the location of the KeyFile should be\r\n");
            _Str.Append("//       relative to the project output directory which is\r\n");
            _Str.Append("//       %Project Directory%\\bin\\<configuration>. For example, if your KeyFile is\r\n");
            _Str.Append("//       located in the project directory, you would specify the AssemblyKeyFile\r\n");
            _Str.Append("//       attribute as [assembly: AssemblyKeyFile(\"{0}\")]\r\n");
            _Str.Append("//   (*) Delay Signing is an advanced option - see the Microsoft .NET Framework\r\n");
            _Str.Append("//       documentation for more information on this.\r\n");
            _Str.Append("//\r\n");

            if (aAssemblyInfoDef.AssemblyDelaySign) {
                _Str.Append("[assembly: AssemblyDelaySign(true)]\r\n");
            } else {
                _Str.Append("[assembly: AssemblyDelaySign(false)]\r\n");
            }
            
            _Str.AppendFormat("[assembly: AssemblyKeyFile(\"{0}\")]\r\n", aAssemblyInfoDef.AssemblyKeyFile);
            _Str.AppendFormat("[assembly: AssemblyKeyName(\"{0}\")]\r\n", aAssemblyInfoDef.AssemblyKeyName);

            FileStream _FileStream = new FileStream(aOutputFileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            try {
                System.Text.UTF8Encoding _Enc = new System.Text.UTF8Encoding();
                byte[] _Bytes = _Enc.GetBytes(_Str.ToString());

                _FileStream.Write(_Bytes, 0, _Bytes.Length);
                LogMessage(aOutputFileName + " file created sucessfully");
            } finally {
                _FileStream.Close();
            }

            return true;
        }

        private bool GenerateAssemblyInfoDelphiFile(AssemblyInfoDef aAssemblyInfoDef, string aOutputFileName) {
            LogMessage(String.Format(CultureInfo.InvariantCulture, "Building AssemblyInfo file: {0} ", aOutputFileName));
            StringBuilder _Str = new StringBuilder();

            _Str.AppendFormat("unit {0};\r\n", Path.GetFileNameWithoutExtension(aOutputFileName));
            _Str.Append("\r\n");
            _Str.Append("interface\r\n");
            _Str.Append("\r\n");
            _Str.Append("uses\r\n");
            _Str.Append("  System.Reflection, System.Runtime.CompilerServices;\r\n\r\n");

            _Str.Append("//\r\n");
            _Str.Append("// General Information about an assembly is controlled through the following\r\n");
            _Str.Append("// set of attributes. Change these attribute values to modify the information\r\n");
            _Str.Append("// associated with an assembly.\r\n");
            _Str.Append("//\r\n");
            _Str.AppendFormat("[assembly: AssemblyTitle('{0}')]\r\n", aAssemblyInfoDef.AssemblyTitle);
            _Str.AppendFormat("[assembly: AssemblyDescription('{0}')]\r\n", aAssemblyInfoDef.AssemblyDescription);
            _Str.AppendFormat("[assembly: AssemblyConfiguration('{0}')]\r\n", aAssemblyInfoDef.AssemblyConfiguration);
            _Str.AppendFormat("[assembly: AssemblyCompany('{0}')]\r\n", aAssemblyInfoDef.AssemblyCompany);
            _Str.AppendFormat("[assembly: AssemblyProduct('{0}')]\r\n", aAssemblyInfoDef.AssemblyProduct);
            _Str.AppendFormat("[assembly: AssemblyCopyright('{0}')]\r\n", aAssemblyInfoDef.AssemblyCopyright);
            _Str.AppendFormat("[assembly: AssemblyTrademark('{0}')]\r\n", aAssemblyInfoDef.AssemblyTrademark);
            _Str.AppendFormat("[assembly: AssemblyCulture('{0}')]\r\n\r\n", aAssemblyInfoDef.AssemblyCulture);

            _Str.Append("//\r\n");
            _Str.Append("// Version information for an assembly consists of the following four values:\r\n");
            _Str.Append("//\r\n");
            _Str.Append("//      Major Version\r\n");
            _Str.Append("//      Minor Version\r\n");
            _Str.Append("//      Build Number\r\n");
            _Str.Append("//      Revision\r\n");
            _Str.Append("//\r\n");
            _Str.Append("// You can specify all the values or you can default the Revision and Build Numbers\r\n");
            _Str.Append("// by using the '*' as shown below:\r\n");
            _Str.Append("\r\n");
            _Str.AppendFormat("[assembly: AssemblyVersion('{0}')]\r\n\r\n", aAssemblyInfoDef.AssemblyVersion);

            _Str.Append("//\r\n");
            _Str.Append("// In order to sign your assembly you must specify a key to use. Refer to the\r\n");
            _Str.Append("// Microsoft .NET Framework documentation for more information on assembly signing.\r\n");
            _Str.Append("//\r\n");
            _Str.Append("// Use the attributes below to control which key is used for signing.\r\n");
            _Str.Append("//\r\n");
            _Str.Append("// Notes:\r\n");
            _Str.Append("//   (*) If no key is specified, the assembly is not signed.\r\n");
            _Str.Append("//   (*) KeyName refers to a key that has been installed in the Crypto Service\r\n");
            _Str.Append("//       Provider (CSP) on your machine. KeyFile refers to a file which contains\r\n");
            _Str.Append("//       a key.\r\n");
            _Str.Append("//   (*) If the KeyFile and the KeyName values are both specified, the\r\n");
            _Str.Append("//       following processing occurs:\r\n");
            _Str.Append("//       (1) If the KeyName can be found in the CSP, that key is used.\r\n");
            _Str.Append("//       (2) If the KeyName does not exist and the KeyFile does exist, the key\r\n");
            _Str.Append("//           in the KeyFile is installed into the CSP and used.\r\n");
            _Str.Append("//   (*) In order to create a KeyFile, you can use the sn.exe (Strong Name) utility.\r\n");
            _Str.Append("//       When specifying the KeyFile, the location of the KeyFile should be\r\n");
            _Str.Append("//       relative to the project output directory which is\r\n");
            _Str.Append("//       %Project Directory%\\bin\\<configuration>. For example, if your KeyFile is\r\n");
            _Str.Append("//       located in the project directory, you would specify the AssemblyKeyFile\r\n");
            _Str.Append("//       attribute as [assembly: AssemblyKeyFile('{0}')]\r\n");
            _Str.Append("//   (*) Delay Signing is an advanced option - see the Microsoft .NET Framework\r\n");
            _Str.Append("//       documentation for more information on this.\r\n");
            _Str.Append("//\r\n");

            if (aAssemblyInfoDef.AssemblyDelaySign) {
                _Str.Append("[assembly: AssemblyDelaySign(true)]\r\n");
            } else {
                _Str.Append("[assembly: AssemblyDelaySign(false)]\r\n");
            }
            
            _Str.AppendFormat("[assembly: AssemblyKeyFile('{0}')]\r\n", aAssemblyInfoDef.AssemblyKeyFile);
            _Str.AppendFormat("[assembly: AssemblyKeyName('{0}')]\r\n\r\n", aAssemblyInfoDef.AssemblyKeyName);
            _Str.Append("implementation\r\n\r\n");
            _Str.Append("end.");

            FileStream _FileStream = new FileStream(aOutputFileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            try {
                System.Text.UTF8Encoding _Enc = new System.Text.UTF8Encoding();
                byte[] _Bytes = _Enc.GetBytes(_Str.ToString());

                _FileStream.Write(_Bytes, 0, _Bytes.Length);
                LogMessage(aOutputFileName + " file created sucessfully");
            } finally {
                _FileStream.Close();
            }

            return true;
        }

        private static AssemblyInfoDef LoadAssemblyInfoDefFromXml(string aInputFileName) {
            if (Lextm.IO.FileHelper.FileIsValid(aInputFileName))
            {
                XmlSerializer _Serializer = new XmlSerializer(typeof(AssemblyInfoDef));

                FileStream _FileStream = File.OpenRead(aInputFileName);
                XmlReader _Reader = new XmlTextReader(_FileStream);
                try {
                    try {
                        AssemblyInfoDef _AssemblyInfoDef = (AssemblyInfoDef) _Serializer.Deserialize(_Reader);
                        _AssemblyInfoDef.InputFileName = aInputFileName;

                        return _AssemblyInfoDef;
                    } catch {
                        return null;
                    }
                } finally {
                    _Reader.Close();
                }
            }

            return null;
        }

        private List<AssemblyInfoDef> LoadAllAssemblyInfoDefFromFiles(string[] aInputFileNames) {
            List<AssemblyInfoDef> _List = new List<AssemblyInfoDef>();

            foreach (string _InputFileName in aInputFileNames) {
                AssemblyInfoDef _AssemblyInfoDef = LoadAssemblyInfoDefFromXml(_InputFileName);

                if (_AssemblyInfoDef != null) {
                    _List.Add(_AssemblyInfoDef);
                    LogMessage(String.Format(CultureInfo.InvariantCulture, "{0} Loaded", _InputFileName));
                } else {
                    LogMessage(String.Format(CultureInfo.InvariantCulture, "Input file {0} not found or invalid", _InputFileName));
                }
            }

            if (_List.Count == 0) {
                LogMessage("No input filename specified or found");
            }

            return _List;
        }

        private List<AssemblyInfoDef> SortAssemblyInfoDefFromFiles(List<AssemblyInfoDef> list) {
            list.Sort(this);
            return list;
        }
		/// <summary>
		/// Logs message.
		/// </summary>
		/// <param name="message">Message</param>
        private void LogMessage(string message) {
            if (MessageEvent != null)
            {    MessageEvent(this, message);}
        }

        // *************************************************************************
        //                              Public
        // *************************************************************************
        /// <summary>
        /// Compares.
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <returns></returns>
        public int Compare(AssemblyInfoDef x, AssemblyInfoDef y) {
            return x.Index.CompareTo(y.Index);
        }
		/// <summary>
		/// Creates assembly info.
		/// </summary>
		/// <param name="language">Language</param>
		/// <param name="inputFileNames">Input file names</param>
		/// <returns></returns>
		internal bool CreateAssemblyInfo(Language language, string[] inputFileNames) {
            return CreateAssemblyInfo(language, inputFileNames, null);
		}
		/// <summary>
		/// Creates assembly info.
		/// </summary>
		/// <param name="language">Language</param>
		/// <param name="inputFileNames">Input file names</param>
		/// <param name="outputFileName">Output file name</param>
		/// <returns></returns>
		internal bool CreateAssemblyInfo(Language language, string[] inputFileNames, string outputFileName) {
            bool _Result = false;

            LogMessage("Starting AssemblyInfo Creation");

            List<AssemblyInfoDef> _AssemblyInfoDefs = LoadAllAssemblyInfoDefFromFiles(inputFileNames);
            _AssemblyInfoDefs = SortAssemblyInfoDefFromFiles(_AssemblyInfoDefs);

            if (_AssemblyInfoDefs != null && _AssemblyInfoDefs.Count != 0) {
                AssemblyInfoDef _AssemblyInfoDef = BuildMainAssemblyInfoDef(_AssemblyInfoDefs);

                string _OutputFileName;
                
                if (!String.IsNullOrEmpty(outputFileName)) {
                   _OutputFileName = outputFileName;
                } else {
                  _OutputFileName = _AssemblyInfoDef.OutputFileName;
                }

                if (!String.IsNullOrEmpty(_OutputFileName)) {
                    if (String.IsNullOrEmpty(Path.GetDirectoryName(_OutputFileName))) {
                		_OutputFileName = Path.Combine(Path.GetDirectoryName(_AssemblyInfoDefs[_AssemblyInfoDefs.Count -1].InputFileName), 
                		                               _OutputFileName);
                    }

                    if (language == Language.CSharp) {
                        _Result = GenerateAssemblyInfoCSharpFile(_AssemblyInfoDef, _OutputFileName);
                    } else {
                        _Result = GenerateAssemblyInfoDelphiFile(_AssemblyInfoDef, _OutputFileName);
                    }
                } else {
                    LogMessage("No output filename specified");
                }
            }

            LogMessage("Done !");
            return _Result;
        }

        // *************************************************************************
        //                              Delegate
        // *************************************************************************
        /// <summary>
        /// Message event definition.
        /// </summary>
        public delegate void AssemblyInfoCreatorMessageEvent(object sender, string message);

        // *************************************************************************
        //                              Events
        // *************************************************************************
        /// <summary>
        /// Message event.
        /// </summary>
        internal event AssemblyInfoCreatorMessageEvent MessageEvent;
    }
}
