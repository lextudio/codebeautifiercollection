// this is the plus2 class.
// Copyright (C) 2006  Lex Y. Li
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

namespace Lextm.OpenTools.Plus {

    using System;
    using System.IO;
    using System.Xml;
    using System.Reflection;
    using System.Diagnostics;
    using System.Xml.Serialization;
    using System.Text;
    using System.Globalization;
    ///<summary>
    /// Plus record.
    /// </summary>
    [Serializable]
    public class Plus2 {

        /// <summary>
        /// Constructor.
        /// </summary>
        public Plus2( ) { }

        private string name;
        private Feature2[] features;
        [XmlIgnore]
        private string moduleName;
        /// <summary>Module name.</summary>
        /// <remarks>File name without extension.</remarks>
        [XmlIgnore]
        public string ModuleName {
            get {
                return moduleName;
            }
            set {
                moduleName = value;
            }
        }
        /// <summary>
        /// Name.
        /// </summary>
        public string Name {
            get {
                return name;
            }
            set {
                name = value;
            }
        }
        /// <summary>
        /// Feature list.
        /// </summary>
        public Feature2[] Features {
            get {
                return features;
            }
            set {
                features = value;
            }
        }

        private const string PatternPlus2 = "*.plus2";
        /// <summary>
        /// Gets .plus2 files.
        /// </summary>
        /// <returns></returns>
        internal static string[] GetPlus2Files() {
            return Directory.GetFiles(
        			   Lextm.OpenTools.IO.Path.PreferencesFolder,
                       //Lextm.Reflection.AssemblyHelper.GetFolder(System.Reflection.Assembly.GetExecutingAssembly()),
                       PatternPlus2);
        }

        private const string MaskDll = "{0}.dll";
        private const string MaskExe = "{0}.exe";

        /// <summary>Loads assembly for plus.</summary>
        public static Assembly LoadAssembly(string moduleName ) {
            string dllName = String.Format(CultureInfo.InvariantCulture, MaskDll, moduleName); // full path, .dll
            string exeName = String.Format(CultureInfo.InvariantCulture, MaskExe, moduleName); // full path, .exe

            if (File.Exists(dllName)) // dll name is first.
            {
                return Assembly.LoadFrom(dllName);
            } else if (File.Exists(exeName)) {
                return Assembly.LoadFrom(exeName);
            } else {
                return null;
            }
        }
        /// <summary>
        /// Is plus enabled for an IDE version.
        /// </summary>
        /// <param name="version">Version</param>
        /// <returns></returns>
        public bool IsEnabledFor(int version) {
        	foreach(Feature2 feature in features) {
        		if (feature.IsEnabledFor(version)) {
        			return true;
        		}
        	}
        	return false;
        }

    }
}
