// this is the shared name class.
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
using System;
using Lextm.Reflection;
using Lextm.Win32;
using System.IO;
using Lextm.Windows.Forms;

namespace Lextm.OpenTools
{
	/// <summary>
	/// Shared properties.
	/// </summary>
	public sealed class ShareUtils
	{
		private ShareUtils() {}
		/// <summary>
		/// MessageBox title.
		/// </summary>
		public const string MessageBoxTitle = "CBC";
		/// <summary>
		/// Menu root.
		/// </summary>
		public const string MenuRootDefault = "CBCLeXtudioMenu";
		/// <summary>
		/// Tab root.
		/// </summary>
		public const string TabRoot = "Features";
		/// <summary>
		/// Alt value.
		/// </summary>
        public const int Alt   = 32768;  // 1000000000000000 == 0X8000
        /// <summary>
        /// Ctrl value.
        /// </summary>
        public const int Ctrl  = 16384;  //  100000000000000 == 0X4000
        /// <summary>
        /// Shift value.
        /// </summary>
        public const int Shift = 8192;   //   10000000000000 == 0X2000
		/// <summary>
		/// OpenTools version.
		/// </summary>
        public static Version CoreVersion {
        	get {
        		return AssemblyHelper.GetVersion(System.Reflection.Assembly.GetExecutingAssembly());
        	}
		}
        /// <summary>
        /// Checks if CBC is installed for current user.
        /// </summary>
        public static bool InstalledForCurrentUser
        {
            get
            {
                bool installed = RegistryHelper.GetValueFromRegKey(@"Software\LeXtudio\CodeBeautifierCollection", "InstalledVersion", null) != null;
                if (!installed)
                {
                    return false;
                }
                Version installedVersion = new Version(RegistryHelper.GetValueFromRegKey(@"Software\LeXtudio\CodeBeautifierCollection", "InstalledVersion", null) as string);
                if (installedVersion < CoreVersion)
                {         
                	// old version is installed
                    return false;
                }
                return true;
            }
        }
        /// <summary>
        /// Install CBC for current user.
        /// </summary>
        public static void InstallForCurrentUser()
        {
            MessageService.Show("Preferences are installed for current user.");
            RegistryHelper.SetValueToRegKey(@"Software\LeXtudio\CodeBeautifierCollection", "InstalledVersion", CoreVersion.ToString());
            string destination = Lextm.OpenTools.IO.Path.PreferencesFolder;
            CopyPreferencesFolder(Path.Combine(Lextm.OpenTools.IO.Path.ProgramFilesFolder, "preferences"), destination);
            CopyPlus2Files(Lextm.OpenTools.IO.Path.ProgramFilesFolder, destination);
        }

        private static void CopyPreferencesFolder(string source, string destination)
        {
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            DirectoryInfo sourceInfo = new DirectoryInfo(source);
            foreach (FileInfo file in sourceInfo.GetFiles())
            {
            	// must override in order to make sure compatibility
                file.CopyTo(Path.Combine(destination, file.Name), true);
            }
        }

        private static void CopyPlus2Files(string source, string destination)
        {
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            DirectoryInfo sourceInfo = new DirectoryInfo(source);
            foreach (FileInfo file in sourceInfo.GetFiles(PatternPlus2, SearchOption.TopDirectoryOnly))
            {
            	// must override in order to make sure compatibility
                file.CopyTo(Path.Combine(destination, file.Name), true);
            }
        }

        private const string PatternPlus2 = "*.plus2";
    }
}
