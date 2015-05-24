// this is the path class.
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
using Lextm.Win32;
using System.Reflection;

namespace Lextm.OpenTools.IO
{

    ///<summary>
    ///Path related staffs.
    ///</summary>
    public sealed class Path
    {
        #region Root Folders
        const string baseName = "LeXtudio";

        //static string commonAppDataFolder;

        //static string CommonAppDataFolder
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(commonAppDataFolder))
        //        {
        //            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        //            commonAppDataFolder = System.IO.Path.Combine(appDataPath, baseName);
        //        }
        //        return commonAppDataFolder;
        //    }
        //}

        static string localAppDataFolder;

        static string LocalAppDataFolder
        {
            get
            {
                if (String.IsNullOrEmpty(localAppDataFolder))
                {
                    string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    localAppDataFolder = System.IO.Path.Combine(appDataPath, baseName);
                }
                return localAppDataFolder;
            }
        }

        static string programFilesFolder;
        /// <summary>
        /// Program Files folder containing CBC.
        /// </summary>
        public static string ProgramFilesFolder
        {
            get
            {
                if (String.IsNullOrEmpty(programFilesFolder))
                {
                    programFilesFolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                }
                return programFilesFolder;
            }
        }
        #endregion
        static string docFolder;

        static string DocFolder
        {
            get
            {
                if (String.IsNullOrEmpty(docFolder))
                {
                    docFolder = System.IO.Path.Combine(programFilesFolder, "doc");
                }
                return docFolder;
            }
        }
        /// <summary>
        /// Gets a doc file path.
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns></returns>
        public static string GetDocFile( string name )
        {
            //return System.IO.Path.Combine(thePathTree.getPath(PathType.DocFolder),
                                         // name);
            return System.IO.Path.Combine(DocFolder, name);
        }
        ///<summary>
        ///About box icon.
        ///</summary>
        public static string AboutBoxFile
        {
            get
            {
            	return System.IO.Path.Combine(ImageFolder, "LeXtudio36");
                //return thePathTree.getPath(PathType.AboutBoxFile);
            }
        }
        
        static string preferencesFolder;
        
        /// <summary>
        /// Preferences folder.
        /// </summary>
        /// <remarks>Contains .plus2 files and .ota files.</remarks>
        public static string PreferencesFolder
        {
        	get
        	{
        		if (String.IsNullOrEmpty(preferencesFolder))
        		{
                    preferencesFolder = System.IO.Path.Combine(LocalAppDataFolder, "preferences");
        		}
        		return preferencesFolder;
        	}
        }
        ///<summary>
        ///Gets preferences file.
        ///</summary>
        /// <param name="otaName">Feature name</param>
        /// <returns>Full path.</returns>
        public static string GetPreferencesFile (string otaName)
        {
            if (PreferencesFolder == null)
            {
                return null;
            }
            else
            {
                return System.IO.Path.Combine(PreferencesFolder, otaName);
            }
            //return System.IO.Path.Combine(thePathTree.getPath(PathType.PreferencesFolder),
            //                              otaName + ".ota");
        }
        /// <summary>
        /// Gets preferences file.
        /// </summary>
        /// <param name="type">Feature type</param>
        /// <returns></returns>
        public static string GetPreferencesFile (MemberInfo type)
        {
            return GetPreferencesFile(type.Name + ".ota");
        }
        ///<summary>
        ///Gets preferences folder.
        ///</summary>
        /// <returns>Full path.</returns>
//        public static string PreferencesFolder
//        {
//            get
//            {
//                return thePathTree.getPath(PathType.PreferencesFolder);
//            }
//        }

        static string dataFolder;
        /// <summary>
        /// Data folder.
        /// </summary>
        public static string DataFolder
        {
            get
            {
                if (String.IsNullOrEmpty(dataFolder))
                {
                    dataFolder = System.IO.Path.Combine(ProgramFilesFolder, "data");
                }
                return dataFolder;
            }
        }
        /// <summary>
        /// Gets data file.
        /// </summary>
        /// <param name="fileName">File name (with extension)</param>
        /// <returns>Full path.</returns>
        /// <remarks>Custom files should be put in Data folder.</remarks>
        public static string GetDataFile(string fileName)
        {
            return System.IO.Path.Combine(DataFolder, fileName);
        }
        ///<summary>
        ///JCF configuration file.
        ///</summary>
        public static string DefaultConfigFile
        {
            get
            {
                return System.IO.Path.Combine(PreferencesFolder, "DefaultSettings.cfg");
                //return thePathTree.getPath(PathType.DefaultConfigFile);
            }
        }
        /// <summary>
        /// User configuration file.
        /// </summary>
        public static string UserConfigFile
        {
        	get {
        		return System.IO.Path.Combine(PreferencesFolder, "JCFSettings.cfg");
        	}
        }
        /// <summary>
        /// JCF style configurator.
        /// </summary>
        public static string JcfStyler
        {
        	get {
        		return System.IO.Path.Combine(PreferencesFolder, "JcfStyle.exe");
        	}
        }
        ///<summary>
        ///Splash screen icon.
        ///</summary>
        public static string SplashFile
        {
            get
            {
                return System.IO.Path.Combine(ImageFolder, "LeXtudio24");
                //return thePathTree.getPath(PathType.SplashFile);
            }
        }
        static string bundledFolder;
        /// <summary>
        /// Bundled folder.
        /// </summary>
        public static string BundledFolder
        {
            get
            {
                if (String.IsNullOrEmpty(bundledFolder))
                {
                    bundledFolder = System.IO.Path.Combine(programFilesFolder, "bundled");
                }
                return bundledFolder;
                //return thePathTree.getPath(PathType.BundledFolder);
            }
        }
        /// <summary>
        /// Gets a bundled file path.
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns></returns>
        public static string GetBundledFile( string name )
        {
            return System.IO.Path.Combine(BundledFolder, name);
        }
        //TODO: remove later
        //private static PathTree thePathTree = PathTree.getInstance();

        static string imageFolder;

        static string ImageFolder
        {
            get
            {
                if (String.IsNullOrEmpty(imageFolder))
                {
                    imageFolder = System.IO.Path.Combine(DataFolder, "images");
                }
                return imageFolder;
            }
        }
        ///<summary>
        ///Gets an image file name.
        ///</summary>
        /// <param name="name">Name (without extension)</param>
        /// <returns>Path without extension.</returns>
        public static string GetImageFile( string name )
        {
            //return System.IO.Path.Combine(thePathTree.getPath(PathType.ImageFolder),
                                          //name);
            return System.IO.Path.Combine(ImageFolder, name);
        }

        private Path( ) {}                           
    }
}
