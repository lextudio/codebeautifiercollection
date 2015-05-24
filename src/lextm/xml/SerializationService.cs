// this is serialization service class.
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
using System.Diagnostics;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.IO;
using System.Text;
using System.Xml;
using Lextm.Diagnostics;

namespace Lextm.Xml {
    /// <summary>
    /// Service that simplifies serialization.
    /// </summary>
    /// <remarks>Two kinds of implementation are present here. You may use the standard service implemented using <see cref="System.Xml.Serialization"></see>
    /// or try the other implemented using <see cref="Yaowi.Common.Serialization"/>.</remarks>
    public sealed class SerializationService {
        private SerializationService() {}
		/// <summary>
		/// Saves.
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <param name="instance">Instance</param>
		/// <remarks>Standard deserialization method implemented using <see cref="System.Runtime.Serialization.Formatters.Soap.SoapFormatter"></see>.</remarks>
		public static void SoapSave(string fileName, object instance) {
			Trace.Assert(fileName != null);

			Stream _FileStream = null;
			string folder = Path.GetDirectoryName(fileName);
			if (!Directory.Exists(folder))
			{
				Directory.CreateDirectory(folder);
			}

			try {
				_FileStream = new FileStream(fileName, FileMode.Create);
			} catch (DirectoryNotFoundException) {
				// no such directory
				LoggingService.Error("no such directory");
			} finally {
				if (_FileStream != null) {
				    new SoapFormatter().Serialize(_FileStream, instance);
					_FileStream.Close();
					_FileStream.Dispose();
				} else {
					LoggingService.Warn("null file stream");
				}
			}
		}
		/// <summary>
		/// Loads.
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <remarks>Standard deserialization method implemented using <see cref="System.Runtime.Serialization.Formatters.Soap.SoapFormatter"></see>.</remarks>
		public static object SoapLoad(string fileName)
		{    
			Trace.Assert(fileName != null);

			object result = null;
			FileStream _FileStream = null;

			try {
				_FileStream = File.OpenRead(fileName);
			} catch (DirectoryNotFoundException) {
				// no such directory
				LoggingService.Error("no such diretory");
			} catch (FileNotFoundException) {
				// cannot find the file
				LoggingService.Error("no file");
			}

			if (_FileStream != null) {
				try {
					result = new SoapFormatter().Deserialize(_FileStream);//_Serializer.Deserialize(_Reader);
				} finally {
					_FileStream.Dispose();
				}
			} 
			return result;
		}
		/// <summary>
		/// Saves.
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <param name="instance">Instance</param>
		/// <remarks>Standard deserialization method implemented using <see cref="System.Runtime.Serialization.Formatters.Binary.BinaryFormatter"></see>.</remarks>
		public static void BinarySave(string fileName, object instance) {
			Trace.Assert(fileName != null);

			Stream _FileStream = null;
			string folder = Path.GetDirectoryName(fileName);
			if (!Directory.Exists(folder))
			{
				Directory.CreateDirectory(folder);
			}

			try {
				_FileStream = new FileStream(fileName, FileMode.Create);
			} catch (DirectoryNotFoundException) {
				// no such directory
				LoggingService.Error("no such directory");
			} finally {
				if (_FileStream != null) {
					new BinaryFormatter().Serialize (_FileStream, instance);
					_FileStream.Close();
					_FileStream.Dispose();
				} else {
					LoggingService.Warn("null file stream");
				}
			}
		}
		
		/// <summary>
		/// Loads.
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <remarks>Standard deserialization method implemented using <see cref="System.Runtime.Serialization.Formatters.Binary.BinaryFormatter"></see>.</remarks>
		public static object BinaryLoad(string fileName)
		{    
			Trace.Assert(fileName != null);

			object result = null;
			FileStream _FileStream = null;

			try {
				_FileStream = File.OpenRead(fileName);
			} catch (DirectoryNotFoundException) {
				// no such directory
				LoggingService.Error("no such diretory");
			} catch (FileNotFoundException) {
				// cannot find the file
				LoggingService.Error("no file");
			}

			if (_FileStream != null) {
				try {
					result = new BinaryFormatter().Deserialize(_FileStream);
				} finally {
					_FileStream.Dispose();
				}
			} 
			return result;
		}

        /// <summary>
        /// Saves.
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <param name="instance">Instance</param>
        /// <remarks>Standard deserialization method implemented using <see cref="System.Xml.Serialization.XmlSerializer"></see>.</remarks>
        public static void Save(string fileName, object instance) {
            Save(fileName, instance, instance.GetType());
        }
        /// <summary>
        /// Saves preferences.
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <param name="instance">Instance</param>
        /// <param name="type">Type</param>
        /// <remarks>Standard deserialization method implemented using <see cref="System.Xml.Serialization.XmlSerializer"></see>.</remarks>
        private static void Save(string fileName, object instance, Type type) {
            Trace.Assert(fileName != null);

            XmlSerializer _Serializer = new XmlSerializer(type);
            Stream _FileStream = null;
			string folder = Path.GetDirectoryName(fileName);
			if (!Directory.Exists(folder))
			{
				Directory.CreateDirectory(folder);
			}

            try {
                _FileStream = new FileStream(fileName, FileMode.Create);

            } catch (DirectoryNotFoundException) {
                // no such directory
                LoggingService.Error("no such directory");
            } finally {
                if (_FileStream != null) {
                    using (XmlTextWriter _Writer = new XmlTextWriter(_FileStream, new UTF8Encoding()))
                    {
                        _Writer.Formatting = Formatting.Indented;

                        // Serialize using the XmlTextWriter.
                        _Serializer.Serialize(_Writer, instance);
                        _Writer.Close();
                    }
                    _FileStream.Close();
                    _FileStream.Dispose();
                } else {
                    LoggingService.Warn("null file stream");
                }
            }
        }
//        /// <summary>
//        /// Saves preferences.
//        /// </summary>
//        /// <param name="featureType">Feature type</param>
//        /// <param name="type">Preferences type</param>
//        /// <remarks>Standard deserialization method implemented using <see cref="System.Xml.Serialization.XmlSerializer"></see>.</remarks>
//        public static object Load(Type featureType, Type type) {
//            string fileName = IO.Path.GetPreferencesFile(featureType.Name);
//            return Load(fileName, type);
//        }
        /// <summary>
        /// Loads.
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <param name="type">Type of object</param>
        /// <remarks>Standard deserialization method implemented using <see cref="System.Xml.Serialization.XmlSerializer"></see>.</remarks>
        public static object Load(string fileName, Type type) {
            Trace.Assert(fileName != null);

            object result = null;

            XmlSerializer _Serializer = new XmlSerializer(type);
            FileStream _FileStream = null;

            try {
                _FileStream = File.OpenRead(fileName);
            } catch (DirectoryNotFoundException) {
                // no such directory
                LoggingService.Error("no such diretory");
            } catch (FileNotFoundException) {
                // cannot find the file
                LoggingService.Error("no file");
			} 
			
            XmlReader _Reader = null;

            try {

                if (_FileStream != null) {
                    _Reader = new XmlTextReader(_FileStream);
                    result = _Serializer.Deserialize(_Reader);
                    _FileStream.Dispose();
                } else {
                    LoggingService.Warn("null file stream");
                }
            } finally {
                if (_Reader != null) {
                    _Reader.Close();
                } else {
                    LoggingService.Warn("null reader");
                }
            }
            return result;
        }
        /// <summary>
        /// Loads.
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns>Deserialized object.</returns>
        /// <remarks>Implemented using <see cref="Yaowi.Common.Serialization.XmlDeserializer"/>.</remarks>
        /// <seealso cref="Save( object, string )"/>
        public static object Load( string fileName )
        {
        	object result = null;
            if (File.Exists(fileName))
            {
				Yaowi.Common.Serialization.XmlDeserializer reader = 
					new Yaowi.Common.Serialization.XmlDeserializer();
				result = reader.Deserialize(fileName);
            }
			return result;
        }
		/// <summary>
		/// Saves.
		/// </summary>
		/// <param name="instance">Instance</param>
		/// <param name="fileName">File name</param>
		/// <remarks> Implemented using <see cref="Yaowi.Common.Serialization.XmlSerializer"/>.</remarks>
		/// <seealso cref="Load( string )"></seealso>
        public static void Save( object instance, string fileName )
        {
            string folder = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
			Yaowi.Common.Serialization.XmlSerializer writer = 
				new Yaowi.Common.Serialization.XmlSerializer();
			writer.Serialize(instance, fileName);
        }   
    }

}

