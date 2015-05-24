// this is the assembly helper class.
// Copyright (C) 2005-2006  Lex Y. Li
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
using System;

namespace Lextm.Reflection {


///<summary>
///Assembly information provider.
///</summary>
/// <version>1</version>
/// <since>2006-1-17</since>
/// <author>lextm</author>
    public sealed class AssemblyHelper {

        /// <summary>
        /// Folder.
        /// </summary>
        /// <param name="assembly">Assembly</param>
        /// <remarks>null if wrong.</remarks>
        public static string GetFolder(System.Reflection.Assembly assembly) {
            if (assembly == null) {
                return null;
            } else {
                return System.IO.Path.GetDirectoryName(
                           assembly.Location);
            }
        }

        ///<summary>
        ///Version info.
        ///</summary>
        /// <param name="assembly">Assembly</param>
        /// <remarks><para>Version of this assembly.</para>
        /// <para>0.0.0.0 if wrong.</para></remarks>
        public static Version GetVersion(System.Reflection.Assembly assembly) {
            if (assembly != null) {
                Version version = assembly.GetName().Version;
                if (version != null) {
                    return version;
                }
            }
			return new Version(0, 0, 0, 0);
		}

		///<summary>
		///Name info.
		///</summary>
		/// <param name="assembly">Assembly</param>
		/// <remarks>Unknown name if wrong.</remarks>
		public static string GetName(System.Reflection.Assembly assembly) {
			if (assembly != null) {
				string name = assembly.GetName().Name;
				if (name != null) {
					return name;
				}
			}
			return "Unknown name";
		}

		///<summary>
		///Company info.
		///</summary>
		/// <param name="assembly">Assembly</param>
		/// <remarks><para>Company of this assembly.</para>
		/// <para>Unknown company if wrong.</para></remarks>
		public static string GetCompany(System.Reflection.Assembly assembly) {
			if (assembly != null) {
				System.Reflection.AssemblyCompanyAttribute attribute =
					(System.Reflection.AssemblyCompanyAttribute)System.Attribute.GetCustomAttribute(assembly, typeof(System.Reflection.AssemblyCompanyAttribute));
				if (attribute != null) {
					return attribute.Company;
				}

			}
			return "Unknown company";
		}
		
		///<summary>
		///Copyright info.
		///</summary>
		/// <param name="assembly">Assembly</param>
		/// <remarks><para>Copyright info of this assembly.</para>
		/// <para>Unknown ccopyright if wrong.</para></remarks>
		public static string GetCopyright(System.Reflection.Assembly assembly) {
			if (assembly != null) {
				System.Reflection.AssemblyCopyrightAttribute attribute =
					(System.Reflection.AssemblyCopyrightAttribute)System.Attribute.GetCustomAttribute(assembly, typeof(System.Reflection.AssemblyCopyrightAttribute));
				if (attribute != null) {
					return attribute.Copyright;
				}

			}
			return "Unknown copyright info";
		}
		
		///<summary>
		///Title info.
		///</summary>
		/// <param name="assembly">Assembly</param>
		/// <remarks><para>Returns title of this assembly.</para>
		/// <para>Unknown title if wrong.</para></remarks>
		public static string GetTitle(System.Reflection.Assembly assembly) {
			if (assembly != null) {
				System.Reflection.AssemblyTitleAttribute attribute = (System.Reflection.AssemblyTitleAttribute)Attribute.GetCustomAttribute(assembly, typeof(System.Reflection.AssemblyTitleAttribute));
				if (attribute != null) {
					return attribute.Title;
				}
			}
			return "Unknown title";
		}
		///<summary>
		///Product info.
		///</summary>
		/// <param name="assembly">Assembly</param>
		/// <remarks><para>Returns title of this assembly.</para>
		/// <para>Unknown product if wrong.</para></remarks>
		public static string GetProduct(System.Reflection.Assembly assembly) {
			if (assembly != null) {
				System.Reflection.AssemblyProductAttribute attribute = (System.Reflection.AssemblyProductAttribute)Attribute.GetCustomAttribute(assembly, typeof(System.Reflection.AssemblyProductAttribute));
				if (attribute != null) {
					return attribute.Product;
				}
			}
			return "Unknown product";
		}
		///<summary>
		///Description info.
		///</summary>
		/// <param name="assembly">Assembly</param>
		/// <remarks><para>Description of this assembly.</para>
		/// <para>Unknown description if wrong.</para></remarks>
		public static string GetDescription(System.Reflection.Assembly assembly) {
			if (assembly != null) {
				System.Reflection.AssemblyDescriptionAttribute attribute =
					(System.Reflection.AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(assembly, typeof(System.Reflection.AssemblyDescriptionAttribute));
				if (attribute != null) {
					return attribute.Description;
				}
			}
			return "Unknown description";
		}

		///<summary>
		///Configuration info.
		///</summary>
		/// <param name="assembly">Assembly</param>
		/// <remarks><para>Configuration of this assembly.</para>
		/// <para>Unknown configuration if wrong.</para></remarks>
		public static string GetConfiguration(System.Reflection.Assembly assembly) {

			if (assembly != null) {
				System.Reflection.AssemblyConfigurationAttribute attribute =
					(System.Reflection.AssemblyConfigurationAttribute)Attribute.GetCustomAttribute(assembly, typeof(System.Reflection.AssemblyConfigurationAttribute));
				if (attribute != null) {
					return attribute.Configuration;
				}
			}
			return "Unknown configuration";

		}

		private AssemblyHelper( ) {}

	}
}

