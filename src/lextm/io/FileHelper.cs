// this is File class.
// Copyright (C) 2006-2007  Lex Y. Li
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
using System.IO;
using NDepend.Helpers.FileDirectoryPath;

namespace Lextm.IO
{
	/// <summary>
	/// File class.
	/// </summary>
	public sealed class FileHelper
	{
		private FileHelper()
		{
		}
		
//		/// <summary>
//		/// Verifies whether it is a .NET file.
//		/// </summary>
//		/// <param name="fileName">File name</param>
//		/// <returns>true if it is .NET, false if else.</returns>
//		public static bool IsDotNetAssembly2(string fileName) {
//			bool result = true;
//			try {
//				System.Reflection.Assembly.LoadFrom(fileName);
//			} catch (BadImageFormatException ex) {
//				int errorCode = System.Runtime.InteropServices.Marshal.GetHRForException(ex);
//				if (errorCode == COR_E_ASSEMBLYEXPECTED)
//				{
//					result = false;
//				}
//			}
//			return result;
//		}
//		
//		private const int COR_E_ASSEMBLYEXPECTED = -2146234344;	
		
		/// <summary>
		/// Verifies whether it is a .NET file.
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <returns>true if it is .NET, false if else.</returns>
		public static bool IsDotNetAssembly(string fileName)
		{
			bool result = true;
			Mono.Cecil.AssemblyDefinition myLibrary = null;
			try 
			{
				myLibrary = Mono.Cecil.AssemblyFactory.GetAssembly (fileName);
    		} 
			catch (Mono.Cecil.Binary.ImageFormatException) 
			{
				// for win32 dll
				result = false;
			} 
			catch (ArgumentOutOfRangeException)
			{
				// for win32 exe
				result = false;
			}
			return result && myLibrary != null;
		}

		/// <summary>
		/// Gets content of a file.
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <returns></returns>
		public static string GetContentOf(string fileName) {
			string content;
			using(System.IO.StreamReader reader = new System.IO.StreamReader(fileName, System.Text.Encoding.Unicode)) {
				content = reader.ReadToEnd();
				reader.Close();
			}
			return content;
		}
		
		/// <summary>
		/// Verifies if the two absolute file paths are the same.
		/// </summary>
		/// <param name="fileName1">File path 1</param>
		/// <param name="fileName2">File path 2</param>
		/// <returns></returns>
		public static bool IsTheSameFile(string fileName1, string fileName2)
		{
			FilePathAbsolute filePathAbsolute1 = new FilePathAbsolute(fileName1);
			FilePathAbsolute filePathAbsolute2 = new FilePathAbsolute(fileName2);
			return filePathAbsolute1 == filePathAbsolute2;
		}

        /// <summary>
        /// Adds extension.
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <param name="extension">Extension</param>
        /// <returns></returns>
        public static string AddExtension(string fileName, string extension)
        {
            if (!String.IsNullOrEmpty(fileName) &&
                    !String.IsNullOrEmpty(extension))
            {

                if (!fileName.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
                {
                    return fileName + extension;
                }
            }

            return fileName;
        }
        /// <summary>
        /// Verifies a file is valid.
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns></returns>
        public static bool FileIsValid(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return false;
            }
            else
            {
                FileInfo _FileInfo = new FileInfo(fileName);

                return (_FileInfo.Length > 0);
            }
        }
        /// <summary>
        /// Verifies a file is read-only.
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns></returns>
        public static bool GetFileIsReadOnly(string fileName)
        {
            FileInfo _FileInfo = new FileInfo(fileName);
            return ((_FileInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly);
        }
        /// <summary>
        /// Sets a file to read-only.
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <param name="aValue">Read-only flag</param>
        public static void SetReadOnlyFile(string fileName, bool aValue)
        {
            bool _ReadOnly = GetFileIsReadOnly(fileName);

            if (_ReadOnly && !aValue)
            {
                File.SetAttributes(fileName, File.GetAttributes(fileName) ^ FileAttributes.ReadOnly);

            }
            else if (!_ReadOnly && aValue)
            {
                File.SetAttributes(fileName, File.GetAttributes(fileName) | FileAttributes.ReadOnly);
            }
        }
	}
}
