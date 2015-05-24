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
//		/// <summary>
//		/// Verifies whether it is a .NET file.
//		/// </summary>
//		/// <param name="fileName">File name</param>
//		/// <returns>true if it is .NET, false if else.</returns>		
//		public static bool IsDotNetAssembly3(string fileName)
//		{
//			bool result = false;
//			uint peHeader;
//			uint peHeaderSignature;
//			ushort machine;
//			ushort sections;
//			uint timestamp;
//			uint pSymbolTable;
//			uint noOfSymbol;
//			ushort optionalHeaderSize;
//			ushort characteristics;
//			ushort dataDictionaryStart;
//			uint[] dataDictionaryRVA = new uint[16] ;
//			uint[] dataDictionarySize = new uint[16];
//
//			Stream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
//			BinaryReader reader = new BinaryReader(fs);
//			
//			//PE Header starts @ 0x3C (60). Its a 4 byte header.
//			fs.Position = 0x3C;			
//			peHeader = reader.ReadUInt32();
//			
//			//Moving to PE Header start location...
//			fs.Position = peHeader;
//			peHeaderSignature = reader.ReadUInt32();
//			
//			//We can also show all these value, but we will be
//			//limiting to the CLI header test.			
//			machine = reader.ReadUInt16();
//			sections = reader.ReadUInt16();
//			timestamp = reader.ReadUInt32();
//			pSymbolTable = reader.ReadUInt32();
//			noOfSymbol = reader.ReadUInt32();
//			optionalHeaderSize = reader.ReadUInt16();
//			characteristics = reader.ReadUInt16();
//			
//			/*
//Now we are at the end of the PE Header and from here, the
//            PE Optional Headers starts...
//      To go directly to the datadictionary, we'll increase the
//      stream¡¯s current position to with 96 (0x60). 96 because,
//           28 for Standard fields
//            68 for NT-specific fields
//From here DataDictionary starts...and its of total 128 bytes. DataDictionay has 16 directories in total,
//doing simple maths 128/16 = 8.
//So each directory is of 8 bytes.
//            In this 8 bytes, 4 bytes is of RVA and 4 bytes of Size.
//           
//btw, the 15th directory consist of CLR header! if its 0, its not a CLR file :)
//			 */
//			dataDictionaryStart = Convert.ToUInt16 (Convert.ToUInt16(fs.Position) + 0x60);
//			fs.Position = dataDictionaryStart;
//			for (int i = 0; i < 15; i++)
//			{
//				dataDictionaryRVA[i] = reader.ReadUInt32();
//				dataDictionarySize[i] = reader.ReadUInt32();
//			}
//			result = dataDictionaryRVA[14] != 0;
//			fs.Close();
//			return result;
//		}
		/// <summary>
		/// Gets content of a file.
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <returns></returns>
		public static string GetContentOf(string fileName) {
			string content;
			using(System.IO.StreamReader reader = new System.IO.StreamReader(fileName)) {
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
